// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Linq.CompilerServices.TypeSystem;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TestDeployable : Deployable.CoreDeployable
    {
        protected override Task UndefineOperators(ReactiveClientContext context)
        {
            return Task.FromResult(true);
        }

        protected override Task UndefineObservables(ReactiveClientContext context)
        {
            return Task.FromResult(true);
        }

        protected override async Task DefineObservables(ReactiveClientContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            await base.DefineObservables(context).ConfigureAwait(false);

            await context.UndefineObservableAsync(Platform.Constants.Identifiers.Observable.FireHose.Uri, CancellationToken.None).ConfigureAwait(false);
            await context.DefineObservableAsync<Uri, T>(
                Platform.Constants.Identifiers.Observable.FireHose.Uri,
                topic => new TestFirehoseObservable<T>(topic).AsQbservable(),
                null,
                CancellationToken.None).ConfigureAwait(false);

            await context.DefineObservableAsync<Uri, T>(
                Constants.Test.HotTimelineObservable.Uri,
                uri => new TimelineObservable<T>(uri, false).AsQbservable(),
                null,
                CancellationToken.None).ConfigureAwait(false);

            await context.DefineObservableAsync<Uri, T>(
                Constants.Test.ColdTimelineObservable.Uri,
                uri => new TimelineObservable<T>(uri, true).AsQbservable(),
                null,
                CancellationToken.None).ConfigureAwait(false);
        }

        protected override Task UndefineObservers(ReactiveClientContext context)
        {
            return Task.FromResult(true);
        }

        protected override async Task DefineObservers(ReactiveClientContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            await base.DefineObservers(context).ConfigureAwait(false);

            await context.DefineObserverAsync<Uri, T>(
                Constants.Test.TestObserver.Uri,
                name => new TestObserver<T>(name).AsQbserver(),
                null,
                CancellationToken.None).ConfigureAwait(false);
        }
    }
}
