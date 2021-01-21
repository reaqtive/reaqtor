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
using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Expressions;
using Reaqtor.Reliable.Service;
using Reaqtor.TestingFramework;

namespace Tests.Reaqtor.Reliable
{
    public class ReliableReactiveServiceContextTestBase
    {
        internal static void Apply(Action<ReliableReactiveServiceContext> action, params ServiceOperation[] expectedOperations)
        {
            Apply<ReliableReactiveServiceContext>(provider => new TestReliableServiceContext(provider), action, expectedOperations);
        }

        internal static void Apply<T>(Func<IReliableReactiveEngineProvider, T> create, Action<T> action, params ServiceOperation[] expectedOperations)
            where T : IReliableReactiveClient
        {
            using var assert = new SequentialAssertionTestReliableEngineProvider(new StructuralServiceOperationEqualityComparer(), expectedOperations);

            var ctx = create(assert);

            action(ctx);
        }
    }

    internal sealed class MyContext : TestReliableServiceContext
    {
        public MyContext(IReliableReactiveEngineProvider provider)
            : base(provider)
        {
        }

        [KnownResource(Constants.Observable.XS)]
        public IReliableQbservable<int> Xs => GetObservable<int>(new Uri(Constants.Observable.XS));
    }

    internal static class Operators
    {
        [KnownResource(Constants.Observable.Where)]
        public static IReliableQbservable<T> Where<T>(this IReliableQbservable<T> source, Expression<Func<T, bool>> predicate)
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
        public static IReliableQbservable<R> Select<T, R>(this IReliableQbservable<T> source, Expression<Func<T, R>> selector)
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
        public static IReliableQbservable<R> SelectMany<T, C, R>(this IReliableQbservable<T> source, Expression<Func<T, IReliableQbservable<C>>> collectionSelector, Expression<Func<T, C, R>> resultSelector)
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
        public static IReliableQbservable<R> SelectMany<T, R>(this IReliableQbservable<T> source, Expression<Func<T, IReliableQbservable<R>>> resultSelector)
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
        public static IReliableQbservable<long> Timer(this IReliableReactive context, TimeSpan dueTime)
        {
            return context.Provider.CreateQbservable<long>(
                Expression.Call(
                    (MethodInfo)MethodInfo.GetCurrentMethod(),
                    Expression.Constant(context, typeof(IReliableReactive)),
                    Expression.Constant(dueTime, typeof(TimeSpan))
                )
            );
        }
    }
}
