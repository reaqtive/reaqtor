// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Linq.Expressions;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Client;

/// <summary>
/// Client context for initializing remoting service providers.
/// </summary>
/// <remarks>
/// <para>
/// In the archived .NET Remoting client library, this context obtained its
/// <see cref="IReactiveServiceProvider"/> by registering a <c>TcpChannel</c> (or by accepting an
/// <c>IRemotingReactiveServiceConnection</c>) and then <c>new</c>-ing a <c>LocalReactiveServiceConnection</c>
/// wrapping a <c>ReactiveServiceCommandProxy</c> (a <c>RemoteProxyBase</c>). Those types belong to the
/// deleted .NET Remoting transport layer and are not ported.
/// </para>
/// <para>
/// Instead, the transport-specific connection is <b>injected via the constructor</b>: callers supply either a
/// fully-built <see cref="IReactiveServiceProvider"/> or an <see cref="IReactiveServiceConnection"/>. The
/// concrete connection (an in-process connection for the InMemory oracle, or a gRPC-backed connection for the
/// cross-process transport) is provided by the hosting platform, not constructed here.
/// </para>
/// </remarks>
public class RemotingClientContext : ReactiveClientContext
{
    /// <summary>
    /// Initializes the remoting client context using a given service provider.
    /// </summary>
    /// <param name="expressionServices">The expression services for normalization of expressions.</param>
    /// <param name="serviceProvider">The service provider underlying the client context.</param>
    public RemotingClientContext(IReactiveExpressionServices expressionServices, IReactiveServiceProvider serviceProvider)
        : base(expressionServices, serviceProvider)
    {
    }

    /// <summary>
    /// Initializes the remoting client context over a service connection injected by the caller.
    /// </summary>
    /// <param name="connection">
    /// The reactive service connection. This is supplied by the hosting platform (an in-process connection for
    /// the InMemory oracle, or a gRPC-backed connection for the cross-process transport); it is not constructed
    /// here. The client serializes a full <see cref="Expression"/> via <see cref="ClientSerializationHelpers"/>
    /// to Bonsai/DataModel JSON; only that JSON string crosses the connection.
    /// </param>
    public RemotingClientContext(IReactiveServiceConnection connection)
        : base(new TupletizingExpressionServices(typeof(IReactiveClientProxy)), GetServiceProvider(connection))
    {
    }

    private static IReactiveServiceProvider GetServiceProvider(IReactiveServiceConnection connection)
    {
        var commandTextFactory = new CommandTextFactory<Expression>(new ClientSerializationHelpers());
        return new ReactiveServiceProvider(connection, commandTextFactory, commandTextFactory);
    }
}
