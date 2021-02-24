// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;
using Reaqtor.Hosting.Shared.Serialization;

namespace Reaqtor.Remoting.Client
{
    public class RemotingServiceProvider : ReactiveServiceProvider
    {
        private readonly Func<Type, Uri, object> _observerFactory;

        public RemotingServiceProvider(IReactiveServiceConnection connection, Func<Type, Uri, object> observerFactory)
            : this(connection)
        {
            _observerFactory = observerFactory;
        }

        public RemotingServiceProvider(IReactiveServiceConnection connection)
            : this(connection, new CommandTextFactory<Expression>(new ClientSerializationHelpers()))
        {
        }

        private RemotingServiceProvider(IReactiveServiceConnection connection, CommandTextFactory<Expression> commandTextFactory)
            : base(connection, commandTextFactory, commandTextFactory)
        {
        }

        public override Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
        {
            if (_observerFactory != null)
            {
                var remoteObserverClient = (IObserver<T>)_observerFactory(typeof(T), observerUri);
                var client = new RemoteObserverClient<T>(remoteObserverClient) as IAsyncReactiveObserver<T>;
                return Task.FromResult(client);
            }

            throw new NotSupportedException("The get observer method is not supported on this service provider.");
        }

        private sealed class RemoteObserverClient<T> : IAsyncReactiveObserver<T>
        {
            private readonly IObserver<T> _remoteObserver;

            public RemoteObserverClient(IObserver<T> remoteObserver)
            {
                _remoteObserver = remoteObserver ?? throw new ArgumentNullException(nameof(remoteObserver));
            }

            public Task OnNextAsync(T value, CancellationToken token)
            {
                _remoteObserver.OnNext(value);
                return Task.FromResult(true);
            }

            public Task OnErrorAsync(Exception error, CancellationToken token)
            {
                _remoteObserver.OnError(error);
                return Task.FromResult(true);
            }

            public Task OnCompletedAsync(CancellationToken token)
            {
                _remoteObserver.OnCompleted();
                return Task.FromResult(false);
            }
        }
    }
}
