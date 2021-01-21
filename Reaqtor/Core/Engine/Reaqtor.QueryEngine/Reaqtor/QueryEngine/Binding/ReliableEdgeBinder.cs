// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.Reliable;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Binder supporting reliable observers used for cross-engine edges.
    /// </summary>
    internal sealed class ReliableEdgeBinder : FullBinder
    {
        private static readonly MethodInfo _getReliableObserverMethod = ((MethodInfo)ReflectionHelpers.InfoOf((Uri uri) => GetReliableObserver<object>(null, uri))).GetGenericMethodDefinition();
        private readonly IReactiveServiceResolver _resolver;

        public ReliableEdgeBinder(IQueryEngineRegistry registry, IReactiveServiceResolver resolver)
            : base(registry)
        {
            Debug.Assert(resolver != null);
            _resolver = resolver;
        }

        protected override Expression LookupReliableObserver(string id, Type elementType)
        {
            Expression result = base.LookupReliableObserver(id, elementType);

            if (result == null)
            {
                var uri = new Uri(id);
                if (_resolver.TryResolveReliable(uri, out var service))
                {
                    var method = _getReliableObserverMethod.MakeGenericMethod(elementType);
                    var observer = method.Invoke(null, new object[] { service, uri });

                    result = Expression.Constant(observer, typeof(IReliableObserver<>).MakeGenericType(elementType));
                }
            }

            return result;
        }

        private static IReliableObserver<T> GetReliableObserver<T>(IReliableReactive c, Uri uri) => c.GetObserver<T>(uri);
    }
}
