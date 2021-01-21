// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Reaqtive;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.Deployable
{
    /// <summary>
    /// Manages all the FireHose subscriptions at the AppDomain level. The subscriptions are being mapped per topic and then per QE instance Id.
    /// </summary>
    internal class FirehoseSubscriptionManager
    {
        /// <summary>
        /// maps an IObservable{T} which will contain the deserialized message stream for each topic
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> _deserializedMessageMulticasterMap = new();

        /// <summary>
        /// Mapping between topic to a map of QE Ids to type of emission to FireHose notifications.
        /// </summary>
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Tuple<string, Type>, object>> _topicMapper =
            new();

        /// <summary>
        /// Prevents an instance of this class to be created
        /// </summary>
        private FirehoseSubscriptionManager()
        {
        }

        /// <summary>
        /// Returns the only instance of this class
        /// </summary>
        public static FirehoseSubscriptionManager Instance { get; } = new FirehoseSubscriptionManager();

        /// <summary>
        /// Subscribes to the underlying stream of deserialized FireHose messages
        /// </summary>
        /// <typeparam name="T">the expected type for the notifications</typeparam>
        /// <param name="topic">the topic of the FireHose messages</param>
        /// <param name="context">the operator context</param>
        /// <param name="observer">the observer to subscribe with</param>
        /// <returns>an <see cref="IDisposable"/> implementation</returns>
        public IDisposable Subscribe<T>(string topic, IOperatorContext context, IObserver<T> observer)
        {
            var qeNameFound = context.TryGetElement(Platform.Constants.ContextKey.Name, out Uri qeName);
            if (!qeNameFound)
            {
                throw new InvalidOperationException("QE Name not found in the context elements");
            }

            // try to see if we already have a multi-caster for this current topic, if we don't, then create one
            // NOTE that the value type in the dictionary is Lazy<> because ConcurrentDictionary doesn't guarantee 
            // the one-time call to the delegate factory (in concurrent access) to create the value in case it doesn't already exist in the dictionary
            // Lazy<> is going to prevent us from having multiple subscriptions to the TopicRouter in case multi-threaded access here
            var typeToFirehoseMap = _deserializedMessageMulticasterMap.GetOrAdd(
                topic,
                messageTopic => new ConcurrentDictionary<Type, object>());

            var firehose = typeToFirehoseMap.GetOrAdd(
                typeof(T),
                new Lazy<IObservable<T>>(() => CreateDeserializedMessagesStream<T>(topic), isThreadSafe: true));

            Debug.Assert(
                firehose is Lazy<IObservable<T>>,
                "FirehoseSubscriptionManager received a Subscribe for a different type than was initially registered");

            var deserializedMessageMulticaster = (Lazy<IObservable<T>>)firehose;
            var qeMapper = _topicMapper.GetOrAdd(topic, _ => new ConcurrentDictionary<Tuple<string, Type>, object>());

            var key = Tuple.Create<string, Type>(qeName.ToCanonicalString(), typeof(T));
            var source = qeMapper.GetOrAdd(
                key,
                tpl => new Lazy<ISubject<T>>(
                           () =>
                           {
                               var qId = tpl.Item1;
                               var subject = new Subject<T>();
                               var contextSwitchingObserver = new WrappedContextSwitchingOperator<T>(qId, subject, topic);

                               SubscriptionInitializeVisitor.Initialize(contextSwitchingObserver, context);

                               var subscription =
                                   deserializedMessageMulticaster.Value.Subscribe(contextSwitchingObserver);

                               // register the QE Unload handler to clean up our shared dictionary
                               var remover = Disposable.Create(
                                   () =>
                                   {
                                       qeMapper.TryRemove(tpl, out _);

                                       Tracer.TraceSource.TraceEvent(TraceEventType.Information, 0, "Removed the FireHose Fan-Out subject for Topic {0}, QE Id {1}", topic, qId);
                                   });

                               Tracer.TraceSource.TraceEvent(TraceEventType.Information, 0, "Created the FireHose Fan-Out subject for Topic {0}, QE Id {1}", topic, qId);

                               return subject;
                           },
                           true));

            Debug.Assert(
                source is Lazy<ISubject<T>>,
                "FirehoseSubscriptionManager received a Subscribe for a different type than was initially registered");

            var sub = new SingleAssignmentDisposable
            {
                // Subscribe on a Subject<T> can cause a synchronous call to OnError or OnCompleted,
                // which has to run on the scheduler thread not to violate check-pointing threading
                // requirements (cf. context switching).
                // Short of building our own subject that is tied to the context switching behavior,
                // this is the quickest fix, akin to Rx's SubscribeOn operator behavior which would
                // be used there to address similar threading constraints.
                Disposable = ((Lazy<ISubject<T>>)source).Value.Subscribe(observer)
            };

            Tracer.TraceSource.TraceEvent(TraceEventType.Information, 0, "Finished setting up FireHose subscription");

            return sub;
        }

        /// <summary>
        /// Returns an observable that's publishing <typeparamref name="T" /> messages that are being deserialized from the FireHose <see cref="Message" /> objects
        /// </summary>
        /// <typeparam name="T">the type of the messages to be deserialized into</typeparam>
        /// <param name="messageTopic">the topic to be used to subscribe to TopicRouter</param>
        /// <returns>
        /// An <see cref="IObservable{T}" /> which will multi-cast its notifications to multiple observers (through the Publish().RefCount() operators)
        /// </returns>
        /// <remarks>
        /// When all the observers have unsubscribed from the returned observable, we will also unsubscribe from the TopicRouter
        /// </remarks>
        private IObservable<T> CreateDeserializedMessagesStream<T>(string messageTopic)
        {
            var observable = Observable.Create<byte[]>(obs => MessageRouter.Instance.Subscribe(new Uri(messageTopic), obs));

            // now attach the deserializer to convert all the Message objects to objects of type T
            var feed = new MessageStreamDeserializer<T>(observable);
            return feed.Finally(
                () =>
                {
                    _deserializedMessageMulticasterMap.TryRemove(messageTopic, out _);

                    Tracer.TraceSource.TraceInformation("Removed the FireHose Deserializer for topic :{0}", messageTopic);
                }).Publish().RefCount();
        }

        /// <summary>
        /// An <see cref="IObservable{TResult}"/> implementation where all the wrapped messages are being deserialized into <typeparamref name="TResult"/> objects
        /// </summary>
        /// <typeparam name="TResult">the type used to deserialize the messages</typeparam>
        /// <remarks>There is a 1:1 mapping between the observable that's pushing the <see cref="Message"/> instances and this one which is going 
        /// to convert those messages into objects of type {T} and also extract the OnError and OnCompleted messages</remarks>
        private class MessageStreamDeserializer<TResult> : IObservable<TResult>
        {
            /// <summary>
            /// the source feed
            /// </summary>
            private readonly IObservable<byte[]> _sourceFeed;

            /// <summary>
            /// Creates an instance of the <see cref="MessageStreamDeserializer{TResult}" /> class
            /// </summary>
            /// <param name="topic">the topic the firehose uses to publish messages</param>
            /// <param name="sourceFeed">the source feed of messages that need to be deserialized</param>
            public MessageStreamDeserializer(IObservable<byte[]> sourceFeed)
            {
                _sourceFeed = sourceFeed ?? throw new ArgumentNullException(nameof(sourceFeed));
            }

            /// <summary>
            /// Subscribes to the messages published by the underlying source feed
            /// </summary>
            /// <param name="observer">the observer to subscribe with</param>
            /// <returns>a subscription object that can later be disposed</returns>
            public IDisposable Subscribe(IObserver<TResult> observer)
            {
                var serializer = new SerializationHelpers();

                return _sourceFeed.Subscribe(
                    s => observer.OnNext(serializer.Deserialize<TResult>(s)),
                    ex => observer.OnError(ex),
                    () => observer.OnCompleted());
            }
        }

        /// <summary>
        /// An implementation of the ContextSwitchOperator
        /// </summary>
        /// <typeparam name="T">type of messages flowing</typeparam>
        private class WrappedContextSwitchingOperator<T> : ContextSwitchOperator<string, T>
        {
            #region private members

            /// <summary>
            /// The message topic for the firehose
            /// </summary>
            private readonly string _topic;

            /// <summary>
            /// The operator context we initialize this CSO with.
            /// </summary>
            private IOperatorContext _context;

            #endregion

            /// <summary>
            /// Instantiates an instance of the <see cref="WrappedContextSwitchingOperator{T}"/> class
            /// </summary>
            /// <param name="queryEvalId">the id of the QueryEvaluator where the query/operator executes</param>
            /// <param name="observer">the wrapped observer</param>
            /// <param name="topic">the expected messages topic</param>
            public WrappedContextSwitchingOperator(string queryEvalId, IObserver<T> observer, string topic)
                : base(queryEvalId, observer)
            {
                _topic = topic;
            }

            /// <summary>
            /// Gets the name of the operator to use in the operator state header.
            /// </summary>
            /// <remarks>
            /// This name should remain stable across versions of the operator.
            /// </remarks>
            public override string Name => "reactor:FirehoseObservable";

            /// <summary>
            /// Gets the version of the operator to use in the operator state header.
            /// </summary>
            /// <remarks>
            /// The version number may change, but it is up to the operator implementation
            /// to provide backward compatibility support.
            /// </remarks>
            public override Version Version => new(1, 0, 0, 0);

            /// <summary>
            /// Handles the observer OnNext call
            /// </summary>
            /// <param name="item">the notification that's being sent</param>
            public override void OnNext(T item)
            {
                base.OnNext(item);
            }

            #region IOperator members

            /// <summary>
            /// Sets the context of this CSO.
            /// </summary>
            /// <param name="context">IOperatorContext to initialize with.</param>
            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            #endregion

            #region Operator members

            /// <summary>
            /// Dispose of this operator.
            /// </summary>
            protected override void OnDispose()
            {
                base.OnDispose();

                if (_context != null && _context.TraceSource != null)
                {
                    // NOTE: implemented to help diagnose regressions of nasty
                    // bugs like 148734. In particular, if UnloadAsync is called
                    // and then fails, it is possible to have disposed of the
                    // scheduler but not disposed of the CSO. In short, this
                    // can cause really weird bugs, and so this will let us know
                    // that the CSO was properly disposed, thereby making it easy
                    // to diagnose this issue.
                    _context.TraceSource.TraceInformation(
                        "WrappedContextSwitchingOperator successfully disposed for QE '{0}' on topic '{1}'.",
                        Params,
                        _topic);
                }
            }

            #endregion
        }
    }
}
