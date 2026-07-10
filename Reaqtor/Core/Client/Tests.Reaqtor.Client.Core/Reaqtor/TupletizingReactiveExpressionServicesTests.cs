// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// HvR - July 2026 - Created this file as part of factoring expression services out of the archived Remoting sample (see #158).
//

using System.Linq.Expressions;

using Reaqtor;

namespace Tests.Reaqtor;

[TestClass]
public class TupletizingReactiveExpressionServicesTests
{
    [TestMethod]
    public void TupletizingReactiveExpressionServices_Lambda_Packed()
    {
        Expression<Func<int, int, int>> expr = (x, y) => x + y;

        var tupletizer = new TupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var normalized = tupletizer.Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        Assert.HasCount(1, lambda.Parameters);
        Assert.AreEqual(typeof(Tuple<int, int>), lambda.Parameters[0].Type);
    }

    [TestMethod]
    public void TupletizingReactiveExpressionServices_UnboundInvocation_Tupletized()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<int, int, int>), "f"), Expression.Constant(1), Expression.Constant(2));

        var tupletizer = new TupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var normalized = tupletizer.Normalize(expr);

        var invoke = (InvocationExpression)normalized;
        Assert.HasCount(1, invoke.Arguments);
        Assert.AreEqual(typeof(Tuple<int, int>), invoke.Arguments[0].Type);
        Assert.AreEqual(typeof(Func<Tuple<int, int>, int>), invoke.Expression.Type);
    }

    [TestMethod]
    public void TupletizingReactiveExpressionServices_AnonymousType_Tupletized()
    {
        Expression<Func<int, object>> expr = x => new { a = x, b = x + 1 };

        var tupletizer = new TupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var normalized = tupletizer.Normalize(expr);

        // The anonymous type is replaced by a tuple, and the root lambda is packed.
        var lambda = (LambdaExpression)normalized;
        Assert.HasCount(1, lambda.Parameters);
        Assert.AreEqual(typeof(Tuple<int>), lambda.Parameters[0].Type);

        var finder = new AnonymousTypeFinder();
        finder.Visit(normalized);
        Assert.IsFalse(finder.Found);
    }

    private sealed class AnonymousTypeFinder : ExpressionVisitor
    {
        public bool Found { get; private set; }

        public override Expression Visit(Expression node)
        {
            if (node != null && node.Type.Name.Contains("AnonymousType", StringComparison.Ordinal))
            {
                Found = true;
            }

            return base.Visit(node);
        }
    }
}
