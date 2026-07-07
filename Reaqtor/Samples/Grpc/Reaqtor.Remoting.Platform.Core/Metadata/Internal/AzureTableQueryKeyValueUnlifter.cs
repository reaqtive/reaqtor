// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Provides expression tree rewrites to unlift an expression over a queryable dictionary with KeyValuePair&lt;Uri, T&gt; entries to the underlying entity model, by projecting Key and Value navigations onto the underlying object model.
    /// </summary>
    internal static class AzureTableQueryKeyValueUnlifter
    {
        /// <summary>
        /// Unlifts the specified expression by rewriting KeyValuePair&lt;Uri, T&gt; occurrences by uses of T, where T corresponds to the specified entity type.
        /// </summary>
        /// <param name="expression">Expression to unlift.</param>
        /// <param name="entityType">Entity type to search for and to perform the unlifting for.</param>
        /// <returns>Expression with occurrences of KeyValuePair&lt;Uri, T&gt; replaced for uses of T.</returns>
        /// <example>
        /// <code>(IQueryable&lt;KeyValuePair&lt;Uri, Foo&gt;&gt; xs) => xs.Where(x => x.Key == bar &amp;&amp; x.Value.Qux == baz)</code>
        /// becomes
        /// <code>(IQueryable&lt;Foo&gt; xs) => xs.Where(x => x.Uri == bar &amp;&amp; x.Qux == baz)</code>
        /// </example>
        public static Expression Unlift(Expression expression, Type entityType)
        {
            var result = new Subst(entityType).Visit(expression);
            return result;
        }

        /// <summary>
        /// Type substitutor to perform the unlifting operation.
        /// </summary>
        private sealed class Subst : TypeSubstitutionExpressionVisitor
        {
            /// <summary>
            /// KeyValuePair&lt;Uri, T&gt; type, where T corresponds to the entity type.
            /// </summary>
            private readonly Type _entryType;

            /// <summary>
            /// Instantiates the unlifting type substitutor.
            /// </summary>
            /// <param name="entityType">The entity type to search for and unlift.</param>
            public Subst(Type entityType)
                : this(entityType, typeof(KeyValuePair<,>).MakeGenericType(typeof(Uri), entityType))
            {
            }

            /// <summary>
            /// Instantiates the unlifting type substitutor.
            /// </summary>
            /// <param name="entityType">The entity type to search for and unlift.</param>
            /// <param name="entryType">The type the entity type is lifted to.</param>
            private Subst(Type entityType, Type entryType)
                : base(new Dictionary<Type, Type>
                {
                    { entryType, entityType },
                    { typeof(IQueryableDictionary<,>).MakeGenericType(typeof(Uri), entityType), typeof(IQueryable<>).MakeGenericType(entityType) },
                })
            {
                _entryType = entryType;
            }

            /// <summary>
            /// Visits a member expression.
            /// </summary>
            /// <param name="node">The member expression to visit.</param>
            /// <returns>The result of the visit.</returns>
            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member == _entryType.GetProperty("Value"))
                {
                    // Skip the .Value portion in the query expression. E.g. item.Value.Expression == bar --> item.Expression == bar
                    var lhs = Visit(node.Expression);
                    return lhs;
                }

                return base.VisitMember(node);
            }

            /// <summary>
            /// Called when a property from the original type is not available on the replacement type.
            /// </summary>
            /// <param name="originalProperty">The property visited on the original type.</param>
            /// <param name="declaringType">The type to find the replacement property on.</param>
            /// <param name="propertyType">The type of the property.</param>
            /// <param name="indexerParameters">The index parameters of the property.</param>
            /// <returns>A resolved type member or failure.</returns>
            protected override MemberInfo FailResolveProperty(PropertyInfo originalProperty, Type declaringType, Type propertyType, Type[] indexerParameters)
            {
                // The .Key property corresponds to the .Uri property on the entity type. E.g. item.Key == foo --> item.Uri == foo
                if (originalProperty == _entryType.GetProperty("Key"))
                {
                    var uri = declaringType.GetProperty("Uri");

                    if (uri == null)
                    {
                        foreach (var type in declaringType.GetInterfaces())
                        {
                            uri = type.GetProperty("Uri");

                            if (uri != null)
                            {
                                break;
                            }
                        }
                    }

                    Debug.Assert(uri != null);

                    return uri;
                }

                return base.FailResolveProperty(originalProperty, declaringType, propertyType, indexerParameters);
            }
        }
    }
}
