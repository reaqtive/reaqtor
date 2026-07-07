// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Grpc.Core;
using global::Grpc.Net.Client;

using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Grpc.Client;

//
// Client-side synchronous adapter (plan §4.4): implements the engine-facing IReactiveMessagingConnection over
// the async gRPC Messaging service. Publish is a unary RPC (sync-over-async, §5.2); Subscribe opens a
// server-streaming RPC on a background pump that invokes the synchronous receive callback per notification, and
// the returned IDisposable cancels the stream. This is the §3.7 egress used by the client and the §3.6 messaging
// adapter injected into the engine's operator context.
//
/// <summary>A gRPC-backed <see cref="IReactiveMessagingConnection"/> over the broker's Messaging service.</summary>
public sealed class GrpcMessagingConnection : ReactiveConnectionBase, IReactiveMessagingConnection, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly bool _ownsChannel;
    private readonly IMessagingService _service;

    /// <summary>Connects to a broker address, owning the underlying channel.</summary>
    public GrpcMessagingConnection(string address)
        : this(GrpcConnectionFactory.CreateChannel(address), ownsChannel: true)
    {
    }

    /// <summary>Connects over an existing channel.</summary>
    public GrpcMessagingConnection(GrpcChannel channel, bool ownsChannel = false)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _ownsChannel = ownsChannel;
        _service = channel.CreateGrpcService<IMessagingService>();
    }

    public void Publish(string topic, INotification<byte[]> data)
    {
        var request = new PublishRequest { Topic = topic, Notification = NotificationConverter.ToWire(data) };

        // Sync-over-async bridge (§5.2): the engine-facing contract is synchronous.
        _service.PublishAsync(request).GetAwaiter().GetResult();
    }

    public IDisposable Subscribe(string topic, Action<INotification<byte[]>> receive)
    {
        ArgumentNullException.ThrowIfNull(receive);

        var cts = new CancellationTokenSource();

        var pump = Task.Run(async () =>
        {
            var context = new CallContext(new CallOptions(cancellationToken: cts.Token));
            try
            {
                await foreach (var wire in _service.SubscribeAsync(new SubscribeRequest { Topic = topic }, context)
                    .WithCancellation(cts.Token).ConfigureAwait(false))
                {
                    receive(NotificationConverter.FromWire(wire));
                }
            }
            catch (OperationCanceledException)
            {
                // Expected on Dispose — the subscription was torn down.
            }
#pragma warning disable CA1031 // The stream fault is surfaced to the subscriber as OnError; it must not crash the pump.
            catch (Exception ex) when (!cts.IsCancellationRequested)
            {
                // The stream died with a real error (transport drop, server fault, deserialization). Surface it to
                // the subscriber as OnError (reconstructing the original CLR type for an RpcException) instead of
                // silently dropping it (review #9). Guard the callback's own OnError so it can't crash the pump.
                var error = ex is RpcException rpc ? GrpcFault.ToException(rpc) : ex;
                try
                {
                    receive(ObserverNotification.CreateOnError<byte[]>(error));
                }
                catch (Exception)
                {
                }
            }
#pragma warning restore CA1031
        }, cts.Token);

        return new SubscriptionHandle(cts, pump);
    }

    public void Dispose()
    {
        if (_ownsChannel)
        {
            _channel.Dispose();
        }
    }

    private sealed class SubscriptionHandle : IDisposable
    {
        private CancellationTokenSource _cts;
        private readonly Task _pump;

        public SubscriptionHandle(CancellationTokenSource cts, Task pump)
        {
            _cts = cts;
            _pump = pump;
        }

        public void Dispose()
        {
            var cts = Interlocked.Exchange(ref _cts, null);
            if (cts == null)
            {
                return;
            }

            cts.Cancel();
            try
            {
                _pump.Wait(TimeSpan.FromSeconds(5));
            }
#pragma warning disable CA1031 // Best-effort drain of the background pump on teardown.
            catch (Exception)
            {
            }
#pragma warning restore CA1031
            finally
            {
                cts.Dispose();
            }
        }
    }
}
