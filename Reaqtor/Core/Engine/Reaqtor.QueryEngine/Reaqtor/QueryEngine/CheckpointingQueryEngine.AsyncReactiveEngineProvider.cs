// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Supports direct creation/deletion of reactive artifacts in the engine, which will get persisted during checkpointing.
        /// All operations carried out via this interface are subject to write-ahead logging in the transaction log as well.
        /// </summary>
        /// <remarks>
        /// This provides a compliant <see cref="IReactiveServiceProvider"/> implementation that gets exposed from a query engine
        /// and is backed by the core engine's <see cref="CoreReactiveEngine.CreateArtifactAsync{T}"/> (for create and define
        /// operations) and <see cref="CoreReactiveEngine.DeleteArtifactAsync{T}"/> (for delete and undefine operations) methods.
        /// </remarks>
        private sealed class AsyncReactiveEngineProvider : IReactiveServiceProvider
        {
            private readonly CoreReactiveEngine _engine;

            public AsyncReactiveEngineProvider(CoreReactiveEngine engine)
            {
                _engine = engine;
                Provider = new AsyncQueryProvider(engine.Registry);
            }

            public Task CreateSubscriptionAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
            {
                var expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(subscription);
                return _engine.CreateSubscriptionAsync(subscriptionUri, expr, state, token);
            }

            public Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
            {
                return _engine.DeleteSubscriptionAsync(subscriptionUri, token);
            }

            public Task CreateStreamAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
            {
                var expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(stream);
                return _engine.CreateStreamAsync(streamUri, expr, state, token);
            }

            public Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
            {
                return _engine.DeleteStreamAsync(streamUri, token);
            }

            public Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public Task DefineObservableAsync(Uri observableUri, Expression observable, object state, CancellationToken token)
            {
                var expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(observable);
                return _engine.DefineObservableAsync(observableUri, expr, state, token);
            }

            public Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
            {
                return _engine.UndefineObservableAsync(observableUri, token);
            }

            public Task DefineObserverAsync(Uri observerUri, Expression observer, object state, CancellationToken token)
            {
                var expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(observer);
                return _engine.DefineObserverAsync(observerUri, expr, state, token);
            }

            public Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
            {
                return _engine.UndefineObserverAsync(observerUri, token);
            }

            public Task DefineStreamFactoryAsync(Uri streamFactoryUri, Expression streamFactory, object state, CancellationToken token)
            {
                var expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(streamFactory);
                return _engine.DefineSubjectFactoryAsync(streamFactoryUri, expr, state, token);
            }

            public Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
            {
                return _engine.UndefineSubjectFactoryAsync(streamFactoryUri, token);
            }

            public Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state, CancellationToken token)
            {
                var expr = _engine.Parent.RewriteQuotedReactiveToSubscribable(subscriptionFactory);
                return _engine.DefineSubscriptionFactoryAsync(subscriptionFactoryUri, expr, state, token);
            }

            public Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
            {
                return _engine.UndefineSubscriptionFactoryAsync(subscriptionFactoryUri, token);
            }

            public IQueryProvider Provider { get; }
        }
    }
}
