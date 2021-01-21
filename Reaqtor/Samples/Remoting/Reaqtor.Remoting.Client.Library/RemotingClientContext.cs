// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Client context for to initialize remoting service providers.
    /// </summary>
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
        /// Initializes the remoting client context using a well-known service provider identified by serviceProviderUri.
        /// </summary>
        /// <param name="serviceProviderUri">URI of a well-known service provider.</param>
        public RemotingClientContext(Uri serviceProviderUri)
            : this(serviceProviderUri?.ToString())
        {
        }

        /// <summary>
        /// Initializes the remoting client context using a well-known service provider identified by serviceProviderUri.
        /// </summary>
        /// <param name="serviceProviderUri">URI of a well-known service provider.</param>
        public RemotingClientContext(string serviceProviderUri)
            : base(new TupletizingExpressionServices(typeof(IReactiveClientProxy)), GetServiceProvider(serviceProviderUri))
        {
        }

        /// <summary>
        /// Initializes the remoting client context using a service connection interface.
        /// </summary>
        /// <param name="connection">The service connection.</param>
        public RemotingClientContext(IRemotingReactiveServiceConnection connection)
            : base(new TupletizingExpressionServices(typeof(IReactiveClientProxy)), GetServiceProvider(connection))
        {
        }

        private static IReactiveServiceProvider GetServiceProvider(string serviceProviderUri)
        {
            if (serviceProviderUri == null)
                throw new ArgumentNullException(nameof(serviceProviderUri));

            var clientProvider = new BinaryClientFormatterSinkProvider();
            var serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
            var props = new Hashtable
                    {
                        { "port", 0 },
                        { "name", Guid.NewGuid().ToString() },
                        { "typeFilterLevel", TypeFilterLevel.Full }
                    };
            ChannelServices.RegisterChannel(new TcpChannel(props, clientProvider, serverProvider), ensureSecurity: false);

            var connection = (IRemotingReactiveServiceConnection)Activator.GetObject(typeof(IRemotingReactiveServiceConnection), serviceProviderUri);
            return GetServiceProvider(connection);
        }

        private static IReactiveServiceProvider GetServiceProvider(IRemotingReactiveServiceConnection connection)
        {
            var commandTextFactory = new CommandTextFactory<Expression>(new ClientSerializationHelpers());
            var localConnection = new LocalReactiveServiceConnection(connection);
            return new ReactiveServiceProvider(localConnection, commandTextFactory, commandTextFactory);
        }
    }
}
