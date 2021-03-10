// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Shebang.Linq
{
    public static partial class QueryOperators
    {
        [KnownResource("rx://observable/cast")]
        public static IAsyncReactiveQbservable<TResult> Cast<TResult>(this IAsyncReactiveQbservable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), source.Expression));
        }

        [KnownResource("rx://observable/oftype")]
        public static IAsyncReactiveQbservable<TResult> OfType<TResult>(this IAsyncReactiveQbservable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQbservable<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), source.Expression));
        }
    }
}
