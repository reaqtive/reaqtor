// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;

namespace Tests.Reaqtor.Shared.Core.Reaqtor;

[TestClass]
public class ExpressionTupletizationTests
{
    [TestMethod]
    public void ExpressionTupletization_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionTupletization.Tupletize(null));
        Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionTupletization.Detupletize(null));
    }

    [TestMethod]
    public void ExpressionTupletization_Tupletize_UnboundInvocation()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<int, int, int>), "f"), Expression.Constant(1), Expression.Constant(2));

        var ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Tuple<int, int>(default, default));
        var expected = Expression.Invoke(
            Expression.Parameter(typeof(Func<Tuple<int, int>, int>), "f"),
            Expression.New(ctor, [Expression.Constant(1), Expression.Constant(2)], typeof(Tuple<int, int>).GetProperty(nameof(Tuple<,>.Item1)), typeof(Tuple<int, int>).GetProperty(nameof(Tuple<,>.Item2))));

        var actual = ExpressionTupletization.Tupletize(expr);

        AssertEqual(expected, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Tupletize_ZeroArgumentInvocation_Unchanged()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<int>), "f"));

        var actual = ExpressionTupletization.Tupletize(expr);

        AssertEqual(expr, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Tupletize_BoundInvocation_NotTupletized()
    {
        Expression<Func<Func<int, int, int>, int>> expr = f => f(1, 2);

        var actual = ExpressionTupletization.Tupletize(expr);

        // The root lambda gets packed, but the invocation of the bound parameter f stays n-ary.
        var lambda = (LambdaExpression)actual;
        Assert.AreEqual(typeof(Tuple<Func<int, int, int>>), lambda.Parameters[0].Type);

        var invoke = (InvocationExpression)lambda.Body;
        Assert.HasCount(2, invoke.Arguments);
        Assert.AreEqual(typeof(Func<int, int, int>), invoke.Expression.Type);
    }

    [TestMethod]
    public void ExpressionTupletization_Tupletize_RootLambda_Packed()
    {
        Expression<Func<int, int, int>> expr = (x, y) => x + y;

        var actual = ExpressionTupletization.Tupletize(expr);

        var lambda = (LambdaExpression)actual;
        Assert.HasCount(1, lambda.Parameters);
        Assert.AreEqual(typeof(Tuple<int, int>), lambda.Parameters[0].Type);
    }

    [TestMethod]
    public void ExpressionTupletization_Detupletize_UnboundInvocation()
    {
        var expected = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>), "f"), Expression.Constant(42));

        var ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Tuple<int>(default));
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<Tuple<int>, int>), "f"), Expression.New(ctor, Expression.Constant(42)));

        var actual = ExpressionTupletization.Detupletize(expr);

        AssertEqual(expected, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Detupletize_NonTupleArgument_Unchanged()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<List<int>, int>), "f"), Expression.New(typeof(List<int>)));

        var actual = ExpressionTupletization.Detupletize(expr);

        AssertEqual(expr, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Detupletize_RootLambda_TupleParameter_Unpacked()
    {
        Expression<Func<int, int>> expected = x => x;
        Expression<Func<Tuple<int>, int>> expr = t => t.Item1;

        var actual = ExpressionTupletization.Detupletize(expr);

        AssertEqual(expected, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Detupletize_RootLambda_NonTupleParameter_Unchanged()
    {
        Expression<Func<int, int>> expr = x => x;

        var actual = ExpressionTupletization.Detupletize(expr);

        AssertEqual(expr, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Detupletize_RootLambda_NAry_Unchanged()
    {
        Expression<Func<int, int, int>> expr = (x, y) => x + y;

        var actual = ExpressionTupletization.Detupletize(expr);

        AssertEqual(expr, actual);
    }

    [TestMethod]
    public void ExpressionTupletization_Tupletize_RootLambda_TupleParameter_Throws()
    {
        Expression<Func<Tuple<int, int>, int, int>> binary = (t, y) => t.Item1 * 100 + y;
        Assert.ThrowsExactly<NotSupportedException>(() => ExpressionTupletization.Tupletize(binary));

        Expression<Func<Tuple<int, int>, int>> unary = t => t.Item1 + t.Item2;
        Assert.ThrowsExactly<NotSupportedException>(() => ExpressionTupletization.Tupletize(unary));
    }

    [TestMethod]
    public void ExpressionTupletization_Roundtrip_Lambda()
    {
        Expression<Func<int, int>> expr = x => x;

        var tupletized = ExpressionTupletization.Tupletize(expr);
        AssertNotEqual(expr, tupletized);

        var rt = ExpressionTupletization.Detupletize(tupletized);
        AssertEqual(expr, rt);
    }

    [TestMethod]
    public void ExpressionTupletization_Roundtrip_Invocation()
    {
        var expr = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>), "f"), Expression.Constant(42));

        var tupletized = ExpressionTupletization.Tupletize(expr);
        AssertNotEqual(expr, tupletized);

        var rt = ExpressionTupletization.Detupletize(tupletized);
        AssertEqual(expr, rt);
    }

    private static void AssertEqual(Expression x, Expression y)
    {
        Assert.IsTrue(new ExpressionEqualityComparer(() => new Comparator()).Equals(x, y));
    }

    private static void AssertNotEqual(Expression x, Expression y)
    {
        Assert.IsFalse(new ExpressionEqualityComparer(() => new Comparator()).Equals(x, y));
    }

    private sealed class Comparator : ExpressionEqualityComparator
    {
        protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
        {
            return x.Name == y.Name && Equals(x.Type, y.Type);
        }
    }
}
