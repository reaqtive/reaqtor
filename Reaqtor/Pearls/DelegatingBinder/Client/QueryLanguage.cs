// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DelegatingBinder
{
    internal static class QueryLanguage
    {
        [Resource("where")]
        public static IQbservable<T> Where<T>(this IQbservable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.Provider.CreateObservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression,
                    predicate
                )
            );
        }

        [Resource("select")]
        public static IQbservable<R> Select<T, R>(this IQbservable<T> source, Expression<Func<T, R>> selector)
        {
            return source.Provider.CreateObservable<R>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T), typeof(R)),
                    source.Expression,
                    selector
                )
            );
        }
    }
}
