// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform.Firehose
{
    public class FirehoseObserver<TSource> : IObserver<TSource>
    {
        private readonly Uri _topic;
        private readonly SerializationHelpers _serializer;
        private readonly MessageRouter _messageRouter;

        public FirehoseObserver(Uri topic)
        {
            _topic = topic ?? throw new ArgumentNullException(nameof(topic));
            _serializer = new SerializationHelpers();
            _messageRouter = MessageRouter.Instance;
        }

        public FirehoseObserver(Uri topic, MessageRouter messageRouter)
        {
            _topic = topic ?? throw new ArgumentNullException(nameof(topic));
            _serializer = new SerializationHelpers();
            _messageRouter = messageRouter ?? throw new ArgumentNullException(nameof(messageRouter));
        }

        public void OnNext(TSource value)
        {
            var serialized = _serializer.ToBytes(value);
            _messageRouter.Publish(_topic, ObserverNotification.CreateOnNext<byte[]>(serialized));
        }

        public void OnError(Exception error)
        {
            _messageRouter.Publish(_topic, ObserverNotification.CreateOnError<byte[]>(error));
        }

        public void OnCompleted()
        {
            _messageRouter.Publish(_topic, ObserverNotification.CreateOnCompleted<byte[]>());
        }
    }
}
