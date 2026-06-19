// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;
using Reaqtor.TestingFramework;

namespace Tests.Reaqtor.Service
{
    public class ReactiveServiceContextTestBase
    {
        internal static void Apply(Action<ReactiveServiceContext> action, params ServiceOperation[] expectedOperations)
        {
            Apply<ReactiveServiceContext>(provider => new TestServiceContext(provider), action, expectedOperations);
        }

        internal static void Apply<T>(Func<IReactiveEngineProvider, T> create, Action<T> action, params ServiceOperation[] expectedOperations)
            where T : IReactiveClient
        {
            using var assert = new SequentialAssertionTestEngineProvider(new StructuralServiceOperationEqualityComparer(), expectedOperations);

            var ctx = create(assert);
            action(ctx);
        }
    }

    internal class MyContext : TestServiceContext
    {
        public MyContext(IReactiveEngineProvider provider)
            : base(provider)
        {
        }

        [KnownResource(Constants.Observable.XS)]
        public IReactiveQbservable<int> Xs => GetObservable<int>(new Uri(Constants.Observable.XS));
    }

    internal static class Operators
    {
        [KnownResource(Constants.Observable.Where)]
        public static IReactiveQbservable<T> Where<T>(this IReactiveQbservable<T> source, Expression<Func<T, bool>> predicate)
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
        public static IReactiveQbservable<R> Select<T, R>(this IReactiveQbservable<T> source, Expression<Func<T, R>> selector)
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
        public static IReactiveQbservable<R> SelectMany<T, C, R>(this IReactiveQbservable<T> source, Expression<Func<T, IReactiveQbservable<C>>> collectionSelector, Expression<Func<T, C, R>> resultSelector)
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
        public static IReactiveQbservable<R> SelectMany<T, R>(this IReactiveQbservable<T> source, Expression<Func<T, IReactiveQbservable<R>>> resultSelector)
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
        public static IReactiveQbservable<long> Timer(this IReactive context, TimeSpan dueTime)
        {
            return context.Provider.CreateQbservable<long>(
                Expression.Call(
                    (MethodInfo)MethodInfo.GetCurrentMethod(),
                    Expression.Constant(context, typeof(IReactive)),
                    Expression.Constant(dueTime, typeof(TimeSpan))
                )
            );
        }
    }
}
