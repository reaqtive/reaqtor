// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Threading;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Messaging
{
    public class MessagingConnection : ReactiveConnectionBase, IReactiveMessagingConnection
    {
        private readonly ConcurrentDictionary<string, Action<INotification<byte[]>>> _topics;

        public MessagingConnection()
        {
            _topics = new ConcurrentDictionary<string, Action<INotification<byte[]>>>();
        }

        public void Publish(string topic, INotification<byte[]> data)
        {
            if (_topics.TryGetValue(topic, out var receivers) && receivers != null)
            {
                receivers(data);
            }
        }

        public IDisposable Subscribe(string topic, Action<INotification<byte[]>> receive)
        {
            _topics.AddOrUpdate(topic, receive, (s, rcv) => rcv + receive);

            return new Unsubscribe(() =>
            {
                var old = default(Action<INotification<byte[]>>);
                while (_topics.TryGetValue(topic, out old) && !_topics.TryUpdate(topic, old - receive, old))
                    ;
            });
        }

        private sealed class Unsubscribe : MarshalByRefObject, IDisposable
        {
            private Action _action;

            public Unsubscribe(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref _action, new Action(() => { }))();
            }

            public override object InitializeLifetimeService() => null;
        }
    }
}
