// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Threading;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Messaging;

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
            // Invoke each subscriber independently: a plain multicast invoke (`receivers(data)`) stops at the
            // first subscriber that throws, starving every later subscriber of the notification AND faulting the
            // publisher (the engine's FirehoseObserver). Isolating each delegate keeps fan-out reliable.
            foreach (var receiver in receivers.GetInvocationList())
            {
                try
                {
                    ((Action<INotification<byte[]>>)receiver)(data);
                }
#pragma warning disable CA1031 // Best-effort fan-out: a faulting subscriber must not break delivery to others or the publisher.
                catch (Exception)
                {
                }
#pragma warning restore CA1031
            }
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

    //
    // ADAPTATION (plan §3.6, adaptation #7): the archived Unsubscribe derived from MarshalByRefObject (and overrode
    // InitializeLifetimeService to opt out of lease-based lifetime) so the unsubscribe handle could cross the .NET
    // Remoting boundary. There is no System.Runtime.Remoting / MarshalByRefObject on net10.0, so we drop the MBR base
    // (keeping IDisposable) and remove the lifetime-service override. The ConcurrentDictionary topic -> subscriber
    // fan-out kernel above is left intact (it is reused as the broker kernel server-side, plan §4.3); the outer
    // MessagingConnection already extends the ported, non-MBR ReactiveConnectionBase.
    //
    private sealed class Unsubscribe : IDisposable
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
    }
}
