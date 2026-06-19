// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, RB - July 2013
//

using System;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;
using Reaqtor.TestingFramework;

namespace Tests.Reaqtor.Client
{
    public class ReactiveClientContextTestBase
    {
        internal static void Apply(Action<ReactiveClientContext> action, params ServiceOperation[] expectedOperations)
        {
            Apply<ReactiveClientContext>(provider => new TestClientContext(provider), action, expectedOperations);
        }

        internal static void Apply<T>(Func<IReactiveServiceProvider, T> create, Action<T> action, params ServiceOperation[] expectedOperations)
            where T : IReactiveClientProxy
        {
            using var assert = new SequentialAssertionTestServiceProvider(new StructuralServiceOperationEqualityComparer(), expectedOperations);

            var ctx = create(assert);
            action(ctx);
        }
    }

    internal class MyContext : TestClientContext
    {
        public MyContext(IReactiveServiceProvider provider)
            : base(provider)
        {
        }

        [KnownResource(Constants.Observable.XS)]
        public IAsyncReactiveQbservable<int> Xs => GetObservable<int>(new Uri(Constants.Observable.XS));
    }

    internal static class Operators
    {
        [KnownResource(Constants.Observable.Where)]
        public static IAsyncReactiveQbservable<T> Where<T>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [KnownResource(Constants.Observable.Select)]
        public static IAsyncReactiveQbservable<R> Select<T, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, R>> selector)
        {
            return source.Provider.CreateQbservable<R>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                    source.Expression,
                    selector
                )
            );
        }

        [KnownResource(Constants.Observable.SelectMany)]
        public static IAsyncReactiveQbservable<R> SelectMany<T, C, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, IAsyncReactiveQbservable<C>>> collectionSelector, Expression<Func<T, C, R>> resultSelector)
        {
            return source.Provider.CreateQbservable<R>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(C), typeof(R)),
                    source.Expression,
                    collectionSelector,
                    resultSelector
                )
            );
        }

        [KnownResource(Constants.Observable.Bind)]
        public static IAsyncReactiveQbservable<R> SelectMany<T, R>(this IAsyncReactiveQbservable<T> source, Expression<Func<T, IAsyncReactiveQbservable<R>>> resultSelector)
        {
            return source.Provider.CreateQbservable<R>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                    source.Expression,
                    resultSelector
                )
            );
        }

        [KnownResource(Constants.Observable.Timer)]
        public static IAsyncReactiveQbservable<long> Timer(this IReactiveProxy context, TimeSpan dueTime)
        {
            return context.Provider.CreateQbservable<long>(
                Expression.Call(
                    (MethodInfo)MethodInfo.GetCurrentMethod(),
                    Expression.Constant(context, typeof(IReactiveProxy)),
                    Expression.Constant(dueTime, typeof(TimeSpan))
                )
            );
        }

        [KnownResource(Constants.Observable.Empty)]
        public static IAsyncReactiveQbservable<T> Empty<T>(this IReactiveProxy context)
        {
            return context.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    Expression.Constant(context, typeof(IReactiveProxy))
                )
            );
        }

        [KnownResource(Constants.Observable.Do)]
        public static IAsyncReactiveQbservable<T> Do<T>(this IAsyncReactiveQbservable<T> source, IAsyncReactiveQbserver<T> observer)
        {
            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    observer.Expression
                )
            );
        }
    }
}
