// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Expression visitor to replace URI-based key lookups into RowKey and PartitionKey based lookups on Azure table storage.
    /// </summary>
    internal class AzureTableQueryKeyMatchRewriter : ExpressionVisitor
    {
        /// <summary>
        /// Function to resolve partitions from a metadata entity URI.
        /// </summary>
        private readonly Func<string, string> _partitionResolver;

        /// <summary>
        /// Creates a new expression visitor to replace URI-based key lookups, using the specified partition key for checks against the entity's PartitionKey. The URI used in a lookup will be hashed for comparison against the entity's RowKey.
        /// </summary>
        /// <param name="partitionResolver">Function to resolve partitions from a metadata entity URI.</param>
        public AzureTableQueryKeyMatchRewriter(Func<string, string> partitionResolver)
        {
            Contract.RequiresNotNull(partitionResolver);

            _partitionResolver = partitionResolver;
        }

        /// <summary>
        /// Visits binary nodes in the expression tree, looking for equality checks on an entity's URI-based key representation.
        /// </summary>
        /// <param name="node">Node to check for a URI-based key lookup.</param>
        /// <returns>Rewritten expression if a URI-aased key lookup was found; otherwise, the original expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Type == typeof(bool) && node.NodeType == ExpressionType.Equal)
            {
                if (node.Left.Type == typeof(Uri) && node.Right.Type == typeof(Uri))
                {
                    if (TryGetUriLookup(node.Left, node.Right, out var entity, out var value) || TryGetUriLookup(node.Right, node.Left, out entity, out value))
                    {
                        if (value != null)
                        {
                            var uriHash = HashingHelper.ComputeHash(value.ToCanonicalString());
                            var partitionKeyValue = _partitionResolver(value.ToCanonicalString());

                            var partitionKey = (PropertyInfo)ReflectionHelpers.InfoOf((TableEntity e) => e.PartitionKey);
                            var rowKey = (PropertyInfo)ReflectionHelpers.InfoOf((TableEntity e) => e.RowKey);

                            var partitionKeyTest = Expression.Equal(Expression.Property(entity, partitionKey), Expression.Constant(partitionKeyValue, typeof(string)));
                            var rowKeyTest = Expression.Equal(Expression.Property(entity, rowKey), Expression.Constant(uriHash, typeof(string)));

                            var test = Expression.AndAlso(partitionKeyTest, rowKeyTest);

                            return test;
                        }
                    }
                }
            }

            return base.VisitBinary(node);
        }

        /// <summary>
        /// Tries to detect a URI property lookup. Notice this method doesn't perform a commutative check and is assumed to be called twice in case commutativity of the lookup operation is desired.
        /// </summary>
        /// <param name="first">Expression to check for member access to an entity's URI property.</param>
        /// <param name="second">Expression to check for a URI representation.</param>
        /// <param name="entity">If a URI property lookup is found, this parameter will hold the expression representing the entity on which the lookup took place.</param>
        /// <param name="value">If a URI property lookup is found, this parameter will hold the value of the URI that was used in the comparison.</param>
        /// <returns>true if the specified expressions represent a URI property lookup; otherwise, false.</returns>
        private static bool TryGetUriLookup(Expression first, Expression second, out Expression entity, out Uri value)
        {
            if (first.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = (MemberExpression)first;

                if (memberExpr.Expression.NodeType == ExpressionType.Parameter)
                {
                    var member = memberExpr.Member;

                    if (typeof(IKnownResource).IsAssignableFrom(member.DeclaringType) && member.Name == "Uri")
                    {
                        if (!FreeVariableScanner.Scan(second).Any())
                        {
                            entity = memberExpr.Expression;
                            value = second.Evaluate<Uri>();
                            return true;
                        }
                    }
                }
            }

            entity = null;
            value = null;
            return false;
        }
    }
}
