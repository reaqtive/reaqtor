// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Shebang.Linq
{
    //
    // Some extra utilities that aren't public (yet).
    //
    // NB: These are handy but confusing, which is why they're not exposed. A function without parameters is used to capture
    //     an expression tree, but only to get the body expression. So we're not representing a function but a value. We want
    //     to reserve these signatures for "deferred creation of T" operators, i.e. true `Func<T>` use cases.
    //

    internal static class Utils
    {
        public static Task DefineObservableAsync<TResult>(this ReactiveClientContextBase ctx, Uri uri, Expression<Func<IAsyncReactiveQbservable<TResult>>> observable, object state = null, CancellationToken token = default)
            => ctx.DefineObservableAsync(uri, ctx.Provider.CreateQbservable<TResult>(observable.Body), state, token);

        public static Task DefineObserverAsync<TResult>(this ReactiveClientContextBase ctx, Uri uri, Expression<Func<IAsyncReactiveQbserver<TResult>>> observer, object state = null, CancellationToken token = default)
            => ctx.DefineObserverAsync(uri, ctx.Provider.CreateQbserver<TResult>(observer.Body), state, token);
    }
}
