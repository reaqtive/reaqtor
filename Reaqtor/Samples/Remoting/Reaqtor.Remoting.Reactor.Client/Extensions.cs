// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Remoting
{
    // TODO: move these to the library

    internal static class Extensions
    {
        [KnownResource(Constants.SubscribeUri)]
        public static IAsyncReactiveQubscription Subscribe<T>(this IAsyncReactiveQbservable<T> observable, IAsyncReactiveQbserver<T> observer)
        {
            var method = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T));
            return observer.Provider.CreateQubscription(Expression.Call(method, observable.Expression, observer.Expression));
        }
    }
}
