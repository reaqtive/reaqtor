// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Testing;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TestFirehoseObservable<TSource> : SubscribableBase<TSource>
    {
        private readonly Uri _topic;

        public TestFirehoseObservable(Uri topic)
        {
            _topic = topic;
        }

        private IList<Subscription> Subscriptions
        {
            get
            {
                var subscriptions = (IList<Subscription>)AppDomain.CurrentDomain.GetData(_topic.ToCanonicalString());
                if (subscriptions == null)
                {
                    subscriptions = new List<Subscription>();
                    AppDomain.CurrentDomain.SetData(_topic.ToCanonicalString(), subscriptions);
                }
                return subscriptions;
            }
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : ContextSwitchOperator<TestFirehoseObservable<TSource>, TSource>, IObserver<TSource>
        {
            private int _subscriptionIndex;
            private TopicObserver _topicObserver;
            private IScheduler _scheduler;

            public _(TestFirehoseObservable<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rct:FirehoseObservable";

            public override Version Version => Versioning.v1;

            public override void SetContext(IOperatorContext context)
            {
                if (!context.TryGetElement<MessageRouter>(MessageRouter.ContextHandle, out var messageRouter))
                {
                    throw new InvalidOperationException("Could not retrieve the message router from the operator context.");
                }
                _topicObserver = new TopicObserver(Params._topic, messageRouter, this);
                _scheduler = context.Scheduler;
                base.SetContext(context);
            }

            protected override void OnStart()
            {
                base.OnStart();
                _topicObserver.Start();
                Params.Subscriptions.Add(new Subscription(_scheduler.Now.Ticks));
                _subscriptionIndex = Params.Subscriptions.Count - 1;
            }

            protected override void OnDispose()
            {
                base.OnDispose();
                _topicObserver.Stop();
                Params.Subscriptions[_subscriptionIndex] = new Subscription(Params.Subscriptions[_subscriptionIndex].Subscribe, _scheduler.Now.Ticks);
            }
        }

        private sealed class TopicObserver : IObserver<byte[]>
        {
            private readonly Uri _topic;
            private readonly MessageRouter _messageRouter;
            private readonly IObserver<TSource> _observer;
            private readonly SerializationHelpers _serializer;
            private IDisposable _stop;

            public TopicObserver(Uri topic, MessageRouter messageRouter, IObserver<TSource> observer)
            {
                _topic = topic;
                _messageRouter = messageRouter;
                _observer = observer;
                _serializer = new SerializationHelpers();
            }

            public void Start()
            {
                _stop = _messageRouter.Subscribe(_topic, this);
            }

            public void Stop()
            {
                _stop.Dispose();
            }

            public void OnNext(byte[] value)
            {
                _observer.OnNext(_serializer.Deserialize<TSource>(value));
            }

            public void OnCompleted()
            {
                _observer.OnCompleted();
            }

            public void OnError(Exception exception)
            {
                _observer.OnError(exception);
            }
        }
    }
}
