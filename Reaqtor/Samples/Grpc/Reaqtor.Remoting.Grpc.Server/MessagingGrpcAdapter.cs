// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using global::Grpc.Core;

using ProtoBuf.Grpc;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Server;

//
// Server side of the broker (plan §4.3): a thin gRPC facade over a MessagingConnection (the same
// ConcurrentDictionary<topic, Action<INotification<byte[]>>> fan-out the engine uses, §3.6). Publish injects;
// Subscribe bridges the broker's synchronous receive-callback to a server-streaming IAsyncEnumerable via a
// per-stream BOUNDED Channel with the §4.3 default "error-the-stream" overflow policy: a subscriber that cannot
// keep up has its stream completed with ResourceExhausted (the subscriber observes OnError and may resubscribe),
// rather than the broker buffering without bound (a memory-exhaustion risk).
//
// The adapter depends on IReactiveMessagingConnection, not on the EngineHost, so the SAME adapter serves both
// topologies (Milestone 5): the in-QE-host broker (EngineHost.MessagingConnection) and the standalone Messaging
// broker host (a dedicated MessagingConnection with no engine).
//
/// <summary>Code-first implementation of <see cref="IMessagingService"/> over a broker connection.</summary>
public sealed class MessagingGrpcAdapter : IMessagingService
{
    // Per-subscriber buffer bound. Small by default (§4.3); a slow consumer is failed fast rather than buffered.
    private const int SubscriberBufferCapacity = 1024;

    private readonly IReactiveMessagingConnection _messaging;

    public MessagingGrpcAdapter(IReactiveMessagingConnection messaging)
    {
        _messaging = messaging ?? throw new ArgumentNullException(nameof(messaging));
    }

    public Task<Empty> PublishAsync(PublishRequest request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var notification = NotificationConverter.FromWire(request.Notification);
        _messaging.Publish(request.Topic, notification);
        return Task.FromResult(Empty.Instance);
    }

    public async IAsyncEnumerable<Contracts.Notification> SubscribeAsync(SubscribeRequest request, CallContext context = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var token = context.ServerCallContext?.CancellationToken ?? CancellationToken.None;
        var channel = Channel.CreateBounded<Contracts.Notification>(new BoundedChannelOptions(SubscriberBufferCapacity)
        {
            SingleReader = true,
            FullMode = BoundedChannelFullMode.Wait, // TryWrite returns false when full instead of blocking the publisher
        });

        using var subscription = _messaging.Subscribe(request.Topic, notification =>
        {
            // The broker callback is synchronous and must never block the publisher, so use the non-blocking
            // TryWrite. A false result means the buffer is full (subscriber too slow) → §4.3 error-the-stream:
            // complete the stream with ResourceExhausted so the subscriber observes OnError and may resubscribe.
            if (!channel.Writer.TryWrite(NotificationConverter.ToWire(notification)))
            {
                channel.Writer.TryComplete(new RpcException(new Status(
                    StatusCode.ResourceExhausted, "Subscriber could not keep up with the topic stream.")));
                return;
            }

            // OnError/OnCompleted terminate the topic stream (plan §5.3).
            if (notification.Kind != NotificationKind.OnNext)
            {
                channel.Writer.TryComplete();
            }
        });

        await foreach (var item in channel.Reader.ReadAllAsync(token).ConfigureAwait(false))
        {
            yield return item;
        }
    }
}
