// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

using System;
using System.Collections.Generic;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Result-producing visitor for JSON expression trees.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    public class ExpressionVisitor<TResult>
    {
        /// <summary>
        /// Visits a JSON expression tree.
        /// </summary>
        /// <param name="node">JSON expression tree to visit.</param>
        /// <returns>Result of visit.</returns>
        public virtual TResult Visit(Expression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.NodeType switch
            {
                ExpressionType.Object => VisitObject((ObjectExpression)node),

                ExpressionType.Array => VisitArray((ArrayExpression)node),

                ExpressionType.Boolean or
                ExpressionType.Number or
                ExpressionType.String or
                ExpressionType.Null => VisitConstant((ConstantExpression)node),

                _ => throw new InvalidOperationException("Unknown JSON node type encountered."),
            };
        }

        /// <summary>
        /// Visits a JSON expression tree object node.
        /// </summary>
        /// <param name="node">JSON expression tree object node to visit.</param>
        /// <returns>Result of visit.</returns>
        public virtual TResult VisitObject(ObjectExpression node) => throw new NotImplementedException();

        /// <summary>
        /// Visits a JSON expression tree array node.
        /// </summary>
        /// <param name="node">JSON expression tree array node to visit.</param>
        /// <returns>Result of visit.</returns>
        public virtual TResult VisitArray(ArrayExpression node) => throw new NotImplementedException();

        /// <summary>
        /// Visits a JSON expression tree constant node.
        /// </summary>
        /// <param name="node">JSON expression tree constant node to visit.</param>
        /// <returns>Result of visit.</returns>
        public virtual TResult VisitConstant(ConstantExpression node) => throw new NotImplementedException();
    }

    /// <summary>
    /// Visitor for JSON expression trees.
    /// </summary>
    public class ExpressionVisitor : ExpressionVisitor<Expression>
    {
        /// <summary>
        /// Visits a JSON expression tree object node.
        /// </summary>
        /// <param name="node">JSON expression tree object node to visit.</param>
        /// <returns>Copy of the expression tree node.</returns>
        public override Expression VisitObject(ObjectExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var res = default(IDictionary<string, Expression>);

            var members = node.Members;
            var n = members.Count;
            var i = 0;

            foreach (var kv in members)
            {
                var oldValue = kv.Value;
                var newValue = Visit(oldValue);

                if (res != null)
                {
                    res[kv.Key] = newValue;
                }
                else
                {
                    if (oldValue != newValue)
                    {
                        res = new Dictionary<string, Expression>(n);

                        using (var entries = members.GetEnumerator())
                        {
                            for (var j = 0; j < i; j++)
                            {
#if DEBUG
                                var moved = entries.MoveNext();
                                System.Diagnostics.Debug.Assert(moved);
#else
                                entries.MoveNext();
#endif
                                res[entries.Current.Key] = entries.Current.Value;
                            }
                        }

                        res[kv.Key] = newValue;
                    }
                }

                i++;
            }

            if (res != null)
            {
                return Expression.Object(res);
            }

            return node;
        }

        /// <summary>
        /// Visits a JSON expression tree array node.
        /// </summary>
        /// <param name="node">JSON expression tree array node to visit.</param>
        /// <returns>Copy of the expression tree node.</returns>
        public override Expression VisitArray(ArrayExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var elements = default(Expression[]);

            var n = node.ElementCount;
            for (var i = 0; i < n; i++)
            {
                var oldElement = node.GetElement(i);
                var newElement = Visit(oldElement);

                if (elements != null)
                {
                    elements[i] = newElement;
                }
                else
                {
                    if (oldElement != newElement)
                    {
                        elements = new Expression[n];
                        for (var j = 0; j < i; j++)
                        {
                            elements[j] = node.GetElement(j);
                        }

                        elements[i] = newElement;
                    }
                }
            }

            if (elements != null)
            {
                return Expression.Array(elements);
            }

            return node;
        }

        /// <summary>
        /// Visits a JSON expression tree constant node.
        /// </summary>
        /// <param name="node">JSON expression tree constant node to visit.</param>
        /// <returns>Copy of the expression tree node.</returns>
        public override Expression VisitConstant(ConstantExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node;
        }
    }
}
