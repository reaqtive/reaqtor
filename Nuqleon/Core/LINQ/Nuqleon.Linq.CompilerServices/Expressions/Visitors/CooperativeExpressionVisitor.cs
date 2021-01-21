// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Cooperative expression visitor that dispatches expression tree node visits to other visitors based on metadata attributes
    /// applied to reflection objects.
    /// </summary>
    /// <example>
    /// Consider the following method definition:
    /// <code>
    /// [Visitor(typeof(MyVisitor))]
    /// static int Foo(int x)
    /// {
    ///     throw new NotImplementedException();
    /// }
    /// </code>
    /// In here, the Foo method is annotated with the VisitorAttribute, specifying the type of the visitor that will process nodes
    /// using this method, e.g. in a MethodCallExpression. This type implements IExpressionVisitor:
    /// <code>
    /// class MyVisitor : IRecursiveExpressionVisitor
    /// {
    ///     public bool TryVisit(Expression expression, Func&lt;Expression, Expression&gt; visit, out Expression result)
    ///     {
    ///         // No-op rewrite for illustration purposes
    ///         var call = (MethodCallExpression)expression;
    ///         var arg = visit(call.Arguments[0]);
    ///         result = call.Update(null, new[] { arg });
    ///         return true;
    ///     }
    /// }
    /// </code>
    /// Using the CooperativeExpressionVisitor, we can visit a tree that calls Foo:
    /// <code>
    /// var f = (Expression&lt;Func&lt;int, int&gt;&gt;)(x => Foo(x + 1));
    /// var g = new CooperativeExpressionVisitor().Visit(f);
    /// </code>
    /// </example>
    public class CooperativeExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Visits a BinaryExpression. If the binary expression is using operator overloading, the MethodInfo of the operator
        /// implementation is considered for cooperative rewriting.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Method != null)
            {
                if (TryDispatchToRewriter(node, node.Method, out Expression result))
                    return result;
            }

            return base.VisitBinary(node);
        }

        /// <summary>
        /// Visits an IndexExpression. The PropertyInfo of the indexing operation is considered for cooperative rewriting.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitIndex(IndexExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (TryDispatchToRewriter(node, node.Indexer, out Expression result))
                return result;

            return base.VisitIndex(node);
        }

        /// <summary>
        /// Visits a MemberExpression. The MemberInfo of the member lookup is considered for cooperative rewriting.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (TryDispatchToRewriter(node, node.Member, out Expression result))
                return result;

            return base.VisitMember(node);
        }

        /// <summary>
        /// Visits a MethodCallExpression. The MethodInfo of the method call is considered for cooperative rewriting.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (TryDispatchToRewriter(node, node.Method, out Expression result))
                return result;

            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// Visits a NewExpression. The ConstructorInfo of the constructor call is considered for cooperative rewriting.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitNew(NewExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Constructor != null)
            {
                if (TryDispatchToRewriter(node, node.Constructor, out var result))
                    return result;
            }

            return base.VisitNew(node);
        }

        /// <summary>
        /// Visits a UnaryExpression. If the unary expression is using operator overloading, the MethodInfo of the operator
        /// implementation is considered for cooperative rewriting.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Method != null)
            {
                if (TryDispatchToRewriter(node, node.Method, out Expression result))
                    return result;
            }

            return base.VisitUnary(node);
        }

        private bool TryDispatchToRewriter(Expression expression, MemberInfo method, out Expression result)
        {
            var attr = method.GetCustomAttribute<VisitorAttribute>();
            if (attr != null)
            {
                var type = attr.VisitorType;

                var rewriter = CreateRewriterInstance(type);
                return rewriter.TryVisit(expression, base.Visit, out result);
            }

            result = null;
            return false;
        }

        private static IRecursiveExpressionVisitor CreateRewriterInstance(Type type) => (IRecursiveExpressionVisitor)Activator.CreateInstance(type);
    }
}
