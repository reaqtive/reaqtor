// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;

using Reaqtor.Remoting.Metadata;
using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.Reactor
{
    public class Deployable : IDeployable
    {
        public void Execute(ReactiveClientContext context)
        {
            RedefineObservables(context).Wait();
            RedefineObservers(context).Wait();
        }

        private async Task RedefineObservables(ReactiveClientContext context)
        {
            await UndefineObservables(context);
            await DefineObservables(context);
        }

        protected virtual async Task UndefineObservables(ReactiveClientContext context)
        {
            await TryUndefine(() => context.UndefineObservableAsync(Constants.Identifiers.Observable.Person.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObservableAsync(Constants.Identifiers.Observable.Range.Uri, CancellationToken.None));
        }

        protected virtual async Task DefineObservables(ReactiveClientContext context)
        {
            await context.DefineObservableAsync<PersonObservableParameters, Person>(Constants.Identifiers.Observable.Person.Uri, parameters => new PersonObservable(parameters).AsQbservable(), null, CancellationToken.None);
            await context.DefineObservableAsync<int, int>(Constants.Identifiers.Observable.Range.Uri, count => new RangeObservable(count).AsQbservable(), null, CancellationToken.None);
        }

        protected virtual async Task RedefineObservers(ReactiveClientContext context)
        {
            await UndefineObservers(context);
            await DefineObservers(context);
        }

        protected virtual async Task UndefineObservers(ReactiveClientContext context)
        {
            await TryUndefine(() => context.UndefineObserverAsync(Reactor.Constants.Identifiers.Observer.Http.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Reactor.Constants.Identifiers.Observer.HttpPost.Action.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Reactor.Constants.Identifiers.Observer.Http.Action.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(Reactor.Constants.Identifiers.Observer.Http.Final.Uri, CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(new Uri(Reactor.Constants.Identifiers.Observer.HttpPost.Final.WithHeaders), CancellationToken.None));
            await TryUndefine(() => context.UndefineObserverAsync(new Uri(Reactor.Constants.Identifiers.Observer.HttpPost.Final.NoHeaders), CancellationToken.None));
        }

        protected virtual async Task DefineObservers(ReactiveClientContext context)
        {
            var nopObserverFactory = (Expression<Func<IAsyncReactiveQbserver<T>>>)(() => NopObserver<T>.Instance.AsQbserver());
            var nopObserver = context.Provider.CreateQbserver<T>(nopObserverFactory.Body);

            await context.DefineObserverAsync<HttpVerb, Uri, Tuple<string, string>[], SerializationProtocol, RetryData, T>(
                Reactor.Constants.Identifiers.Observer.Http.Uri,
                (method, endpoint, headers, protocol, retryData) =>
                    new HttpObserver<T>(method, endpoint, headers, protocol, retryData).AsQbserver(),
                null,
                CancellationToken.None);
            await context.DefineObserverAsync<HttpData>(
                Reactor.Constants.Identifiers.Observer.Http.Action.Uri,
                context.GetObserver<string, HttpData>(Platform.Constants.Identifiers.Observer.ConsoleObserverParam.Uri)("DoHttp"),
                null,
                CancellationToken.None);
            await context.DefineObserverAsync<string, Uri, Uri, Tuple<string, string>[], RetryData, TimeSpan, T>(
                Reactor.Constants.Identifiers.Observer.Http.Final.Uri,
                (method, onCompleted, onError, headers, retryData, timeout) => nopObserver,
                null,
                CancellationToken.None);
            await context.DefineObserverAsync<HttpPost>(
                Reactor.Constants.Identifiers.Observer.HttpPost.Action.Uri,
                context.GetObserver<string, HttpPost>(Platform.Constants.Identifiers.Observer.ConsoleObserverParam.Uri)("DoHttpPost"),
                null,
                CancellationToken.None);
            await context.DefineObserverAsync<Uri, Uri, Tuple<string, string>[], T>(
                new Uri(Reactor.Constants.Identifiers.Observer.HttpPost.Final.WithHeaders),
                (onError, onCompleted, headers) => nopObserver,
                null,
                CancellationToken.None);
            await context.DefineObserverAsync<Uri, Uri, T>(
                new Uri(Reactor.Constants.Identifiers.Observer.HttpPost.Final.NoHeaders),
                (onError, onCompleted) => nopObserver,
                null,
                CancellationToken.None);
        }

        protected static async Task TryUndefine(Func<Task> undefine)
        {
            try
            {
                await undefine();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is not ReactiveProcessingStorageException ex || ex.ErrorCode != ReactiveProcessingStorageException.ErrorCodes.EntityNotFound)
                {
                    throw;
                }
            }
        }
    }
}
