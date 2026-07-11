// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this code in the Remoting sample's test library.
// HvR - July 2026 - Factored out of the archived Remoting sample (see #158).
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;

namespace Tests.Reaqtor;

[TestClass]
public class DetupletizingReactiveExpressionServicesTests
{
    [TestMethod]
    public void DetupletizingReactiveExpressionServices_OneWay_NotDetupletized()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<List<int>, int>)), Expression.New(typeof(List<int>)));
        var detupletizer = new DetupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var actual = detupletizer.Normalize(expr);
        AssertEqual(expr, actual);
    }

    [TestMethod]
    public void DetupletizingReactiveExpressionServices_OneWay_Lambda()
    {
        Expression<Func<int, int>> expected = x => x;
        Expression<Func<Tuple<int>, int>> expr = t => t.Item1;
        var detupletizer = new DetupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var actual = detupletizer.Normalize(expr);
        AssertEqual(expected, actual);
    }

    [TestMethod]
    public void DetupletizingReactiveExpressionServices_OneWay_Invocation()
    {
        var expected = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(42));
        var ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Tuple<int>(default));
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<Tuple<int>, int>)), Expression.New(ctor, Expression.Constant(42)));
        var detupletizer = new DetupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var actual = detupletizer.Normalize(expr);
        AssertEqual(expected, actual);
    }

    [TestMethod]
    public void DetupletizingReactiveExpressionServices_Roundtrip_Lambda()
    {
        Expression<Func<int, int>> expr = x => x;
        var tupletizer = new TupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var normalized = tupletizer.Normalize(expr);
        AssertNotEqual(expr, normalized);
        var detupletizer = new DetupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var rt = detupletizer.Normalize(normalized);
        AssertEqual(expr, rt);
    }

    [TestMethod]
    public void DetupletizingReactiveExpressionServices_Roundtrip_Invocation()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(42));
        var tupletizer = new TupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var normalized = tupletizer.Normalize(expr);
        AssertNotEqual(expr, normalized);
        var detupletizer = new DetupletizingReactiveExpressionServices(typeof(IReactiveClientProxy));
        var rt = detupletizer.Normalize(normalized);
        AssertEqual(expr, rt);
    }

    private static void AssertEqual(Expression x, Expression y) => ExpressionAssert.AreEqual(x, y);

    private static void AssertNotEqual(Expression x, Expression y) => ExpressionAssert.AreNotEqual(x, y);
}
