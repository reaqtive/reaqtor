// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using global::Grpc.Net.Client;

using ProtoBuf.Grpc.Client;

using Reaqtor.Remoting.Grpc.Contracts;

namespace Reaqtor.Remoting.Grpc.Client
{
    /// <summary>
    /// Creates gRPC channels and typed code-first clients for the spike.
    /// </summary>
    /// <remarks>
    /// Cleartext HTTP/2 (h2c) lets the loopback hosts run without a dev TLS certificate (plan §7): <c>Grpc.Net.Client</c>
    /// over insecure HTTP/2 needs the <c>SocketsHttpHandler.Http2UnencryptedSupport</c> switch. Review #8: rather than a
    /// surprising import-time side-effect in a type initializer, the switch is now enabled only when an <c>http://</c>
    /// (insecure) channel is actually created, and never for <c>https://</c>. It remains process-global (an `AppContext`
    /// switch is), so PRODUCTION should use TLS (<c>https://</c>) and not depend on cleartext h2c at all.
    /// </remarks>
    public static class GrpcConnectionFactory
    {
        /// <summary>Creates a channel to a gRPC host address (e.g. <c>http://localhost:8081</c>).</summary>
        public static GrpcChannel CreateChannel(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            // Enable cleartext h2c only for explicitly-insecure http:// loopback addresses, at the point a channel is
            // created (not at type-load). https:// channels never relax the transport. Setting the switch is idempotent.
            if (address.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            }

            return GrpcChannel.ForAddress(address);
        }

        /// <summary>Creates a typed control client over the given channel.</summary>
        public static IReactiveServiceControl CreateControlClient(GrpcChannel channel)
        {
            ArgumentNullException.ThrowIfNull(channel);

            return channel.CreateGrpcService<IReactiveServiceControl>();
        }
    }
}
