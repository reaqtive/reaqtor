// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Provides a conversion of a query plan with a local and a remote portion, indicated using the QueryableExtensions.ToLocal marker, into a distributed query representation.
    /// </summary>
    internal static class AzureTableQuerySplitter
    {
        /// <summary>
        /// Splits the specified query expression into a distributed query by searching for the QueryableExtensions.ToLocal marker.
        /// </summary>
        /// <param name="expression">Query expression to split.</param>
        /// <returns>Representation of the distributed query.</returns>
        public static DistributedQuery Split(Expression expression)
        {
            var impl = new Impl();

            var local = impl.Visit(expression);
            var remote = impl.RemoteExpression;

            if (remote == null)
            {
                // No ToLocal marker was found; everything runs remotely.
                remote = local;

                // And the local portion is set to null.
                local = null;
            }
            else
            {
                local = Expression.Lambda(local, impl.RemoteResultParameter);
            }

            return new DistributedQuery(remote, (LambdaExpression)local);
        }

        /// <summary>
        /// Expression visitor to search for the ToLocal marker. In case multiple ToLocal markers occur in the expression, behavior is undefined.
        /// </summary>
        private sealed class Impl : ExpressionVisitor
        {
            /// <summary>
            /// Generic method representation of the QueryableExtensions.ToLocal&lt;T&gt; method.
            /// </summary>
            private static readonly Lazy<MethodInfo> s_toLocal = new(() => ((MethodInfo)ReflectionHelpers.InfoOf((IQueryable<T> q) => q.ToLocal())).GetGenericMethodDefinition());

            /// <summary>
            /// Gets the remote expression, i.e. the part before the ToLocal marker, if any.
            /// </summary>
            public Expression RemoteExpression
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the parameter that was left in place of the ToLocal call site, if found. This parameter can be used for lambda lifting of the remainder portion into a local query.
            /// </summary>
            public ParameterExpression RemoteResultParameter
            {
                get;
                private set;
            }

            /// <summary>
            /// Visits method call nodes search for the ToLocal marker. If a marker is found, the RemoteExpression property is set and the unbound parameter in RemoteResultParameter is returned.
            /// </summary>
            /// <param name="node">Method call node to analyze for a ToLocal marker.</param>
            /// <returns>The RemoteResultParameter object in case the method represents a ToLocal marker; otherwise, the original expression.</returns>
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsGenericMethod)
                {
                    var genDef = node.Method.GetGenericMethodDefinition();

                    if (genDef == s_toLocal.Value)
                    {
                        RemoteExpression = node.Arguments[0];
                        RemoteResultParameter = Expression.Parameter(node.Type);
                        return RemoteResultParameter;
                    }
                }

                return base.VisitMethodCall(node);
            }
        }
    }
}
