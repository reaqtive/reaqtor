// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Custmm implementation of ExpressionEqualityComparer from Nuqleon.Linq.CompilerServices. Differs in use of quick hashing.
//
// BD - September 2014
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Equality comparer for expressions.
    /// </summary>
    internal class ExpressionEqualityComparer : IEqualityComparer<Expression>
    {
        /// <summary>
        /// Checks whether the specified expressions are equal.
        /// </summary>
        /// <param name="x">First expression to compare.</param>
        /// <param name="y">Second expression to compare.</param>
        /// <returns><c>true</c> if both expressions are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(Expression x, Expression y)
        {
            return new Impl().Equals(x, y);
        }

        /// <summary>
        /// Gets a hash code for the specified expression.
        /// </summary>
        /// <param name="obj">Expression to compute a hash code for.</param>
        /// <returns>Hash code of the specified expression.</returns>
        public int GetHashCode(Expression obj)
        {
            return new Impl().GetHashCode(obj);
        }

        /// <summary>
        /// Stateful private implementation of the equality comparer.
        /// </summary>
        private class Impl : IEqualityComparer<Expression>
        {
            /// <summary>
            /// Checks whether the specified expressions are equal.
            /// </summary>
            /// <param name="x">First expression to compare.</param>
            /// <param name="y">Second expression to compare.</param>
            /// <returns><c>true</c> if both expressions are equal; otherwise, <c>false</c>.</returns>
            public bool Equals(Expression x, Expression y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.NodeType != y.NodeType)
                {
                    return false;
                }

                if (x.Type != y.Type)
                {
                    return false;
                }

                if (x is BinaryExpression b)
                {
                    return Equals(b, (BinaryExpression)y);
                }
                else if (x is UnaryExpression u)
                {
                    return Equals(u, (UnaryExpression)y);
                }
                else if (x is MemberExpression m)
                {
                    return Equals(m, (MemberExpression)y);
                }
                else if (x is MethodCallExpression c)
                {
                    return Equals(c, (MethodCallExpression)y);
                }
                else if (x is InvocationExpression i)
                {
                    return Equals(i, (InvocationExpression)y);
                }
                else if (x is NewExpression n)
                {
                    return Equals(n, (NewExpression)y);
                }
                else if (x is LambdaExpression l)
                {
                    return Equals(l, (LambdaExpression)y);
                }
                else if (x is ParameterExpression p)
                {
                    return Equals(p, (ParameterExpression)y);
                }
                else if (x is ConstantExpression v)
                {
                    return Equals(v, (ConstantExpression)y);
                }

                throw new NotImplementedException(); // omitted many cases
            }

            private bool Equals(BinaryExpression x, BinaryExpression y)
            {
                return Equals(x.Left, y.Left) && Equals(x.Right, y.Right) && Equals(x.Method, y.Method) && Equals(x.Conversion, y.Conversion);
            }

            private bool Equals(UnaryExpression x, UnaryExpression y)
            {
                return Equals(x.Operand, y.Operand) && Equals(x.Method, y.Method);
            }

            private bool Equals(MemberExpression x, MemberExpression y)
            {
                return Equals(x.Expression, y.Expression) && Equals(x.Member, y.Member);
            }

            private bool Equals(MethodCallExpression x, MethodCallExpression y)
            {
                return Equals(x.Object, y.Object) && Equals(x.Method, y.Method) && Equals(x.Arguments, y.Arguments);
            }

            private bool Equals(InvocationExpression x, InvocationExpression y)
            {
                return Equals(x.Expression, y.Expression) && Equals(x.Arguments, y.Arguments);
            }

            private bool Equals(NewExpression x, NewExpression y)
            {
                return Equals(x.Constructor, y.Constructor) && Equals(x.Arguments, y.Arguments);
            }

            /// <summary>
            /// Environment to map variable declarations of corresponding nodes that introduce a scope, used for semantic equivalence checking.
            /// </summary>
            private readonly Stack<Dictionary<ParameterExpression, ParameterExpression>> _env = new();

            private bool Equals(LambdaExpression x, LambdaExpression y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                var frame = new Dictionary<ParameterExpression, ParameterExpression>();

                for (var i = 0; i < x.Parameters.Count; i++)
                {
                    frame[x.Parameters[i]] = y.Parameters[i];
                }

                _env.Push(frame);

                var res = Equals(x.Body, y.Body);

                _env.Pop();

                return res;
            }

            private bool Equals(ParameterExpression x, ParameterExpression y)
            {
                foreach (var frame in _env)
                {
                    if (frame.TryGetValue(x, out var z))
                    {
                        return y == z;
                    }
                }

                return x.Name == y.Name; // specialized for unbound parameter convention
            }

            private static bool Equals(ConstantExpression x, ConstantExpression y)
            {
                return object.Equals(x.Value, y.Value);
            }

            private bool Equals(IEnumerable<Expression> x, IEnumerable<Expression> y)
            {
                return x.SequenceEqual(y, this);
            }

            /// <summary>
            /// Gets a hash code for the specified expression.
            /// </summary>
            /// <param name="obj">Expression to compute a hash code for.</param>
            /// <returns>Hash code of the specified expression.</returns>

            public int GetHashCode(Expression obj)
            {
                var h = new QuickHasher();
                h.Visit(obj);
                return h._result;
            }

            // NOTE: the below is a quick-n-dirty implementation but does reasonably well for our purposes

            /// <summary>
            /// Quick hasher that only incorporates node types for an expression tree hash code.
            /// </summary>
            private class QuickHasher : ExpressionVisitor
            {
                /// <summary>
                /// Result of the hashing operation.
                /// </summary>
                public int _result;

                /// <summary>
                /// Vists the nodes of an expression tree to compute a hash code.
                /// </summary>
                /// <param name="node">Node to visit.</param>
                /// <returns>Original node.</returns>
                public override Expression Visit(Expression node)
                {
                    _result = _result * 17 + (int)node.NodeType;

                    return base.Visit(node);
                }
            }
        }
    }
}
