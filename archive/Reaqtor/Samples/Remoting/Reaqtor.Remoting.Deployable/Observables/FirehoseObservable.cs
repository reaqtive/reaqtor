// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Reactive.Disposables;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.Deployable
{
    /// <summary>
    /// The observable receiving events in a fire hose fashion
    /// over a messaging service.
    /// </summary>
    /// <typeparam name="T">the generic type parameter</typeparam>
    public class FirehoseObservable<T> : IObservable<T>
    {
        /// <summary>
        /// The messaging topic.
        /// </summary>
        private readonly Uri _topic;

        /// <summary>
        /// The only observer of this observable
        /// </summary>
        private IObserver<T> _theObserver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirehoseObservable{T}"/> class.
        /// </summary>
        /// <param name="messageTopic">The message topic.</param>
        public FirehoseObservable(Uri messageTopic)
        {
            _topic = messageTopic ?? throw new ArgumentNullException(nameof(messageTopic));
        }

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>An IDisposable.</returns>
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_theObserver != null)
            {
                throw new InvalidOperationException(
                    "There should be only one observer attached to the Firehose Observable");
            }

            var messageTransportWaitCallBackId = Guid.Empty;
            try
            {
                _theObserver = observer;
                var topicObserver = new TopicObserver(_topic, observer);
                topicObserver.Start();

                return
                    Disposable.Create(
                        () => topicObserver.Stop());
            }
            catch (Exception ex)
            {
                Tracer.TraceSource.TraceEvent(
                    TraceEventType.Error,
                    0,
                    "Fire hose observable - topic:{0}, waitCallbackId:{1}, unable to subscribe to fire hose observable - {2}",
                    _topic,
                    messageTransportWaitCallBackId,
                    ex);

                throw;
            }
        }

        private sealed class TopicObserver : IObserver<byte[]>
        {
            private readonly Uri _topic;
            private readonly IObserver<T> _observer;
            private readonly SerializationHelpers _serializer;
            private IDisposable _stop;

            public TopicObserver(Uri topic, IObserver<T> observer)
            {
                _topic = topic;
                _observer = observer;
                _serializer = new SerializationHelpers();
            }

            public void Start() => _stop = MessageRouter.Instance.Subscribe(_topic, this);

            public void Stop() => _stop.Dispose();

            public void OnNext(byte[] value) => _observer.OnNext(_serializer.Deserialize<T>(value));

            public void OnCompleted() => _observer.OnCompleted();

            public void OnError(Exception exception) => _observer.OnError(exception);
        }
    }
}
