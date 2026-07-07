// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions;

[TestClass]
public class ExpressionSlimExtensionsTests : TestBase
{
    [TestMethod]
    public void ExpressionSlimExtensions_ToExpressionSlim_Null()
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ((Expression)null).ToExpressionSlim());
        Assert.AreEqual("expression", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => ((Expression)null).ToExpressionSlim(ExpressionSlimFactory.Instance));
        Assert.AreEqual("expression", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => Expression.Constant(1).ToExpressionSlim(default(IExpressionSlimFactory)));
        Assert.AreEqual("factory", ex3.ParamName);

        var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => ((ExpressionSlim)null).ToExpression());
        Assert.AreEqual("expression", ex4.ParamName);
        var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => ((ExpressionSlim)null).ToExpression(ExpressionFactory.Instance));
        Assert.AreEqual("expression", ex5.ParamName);
        var ex6 = Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionSlim.Default(typeof(int).ToTypeSlim()).ToExpression(default(IExpressionFactory)));
        Assert.AreEqual("factory", ex6.ParamName);
#pragma warning restore IDE0034 // Simplify 'default' expression
    }

    [TestMethod]
    public void ExpressionSlimExtensions_ToExpressionSlim_Roundtrip()
    {
        var expected = Expression.Constant(0, typeof(int));
        var actual = Roundtrip(expected) as ConstantExpression;

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected.Value, actual.Value);
        Assert.AreEqual(expected.Type, actual.Type);
    }

    [TestMethod]
    public void ExpressionSlimExtensions_ToExpressionSlim_RoundtripMore()
    {
        var exprs = new Expression[]
        {
            Expression.Constant(value: null, typeof(object)),
            Expression.Constant(1),
            Expression.Constant("foo"),

            Expression.Add(Expression.Constant(1), Expression.Constant(2)),
            Expression.Add(Expression.Constant(DateTime.Now), Expression.Constant(TimeSpan.Zero)),

            (Expression<Func<int, int>>)(x => x),

#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
            (Expression<Func<DateTime>>)(() => DateTime.Now),
            (Expression<Func<int>>)(() => "Bart".Length),

            //(Expression<Func<TimeSpan>>)(() => new TimeSpan()), // REVIEW
            (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3)),
#pragma warning restore IDE0004 // Remove Unnecessary Cast
        };

        var eq = new ExpressionEqualityComparer();

        foreach (var ex in exprs)
        {
            var r1 = Roundtrip(ex);
            Assert.IsTrue(eq.Equals(ex, r1), ex.ToString());

            var r2 = RoundtripFactories(ex);
            Assert.IsTrue(eq.Equals(ex, r2), ex.ToString());
        }
    }

    private static Expression Roundtrip(Expression e)
    {
        var slim = e.ToExpressionSlim();
        return slim.ToExpression();
    }

    private static Expression RoundtripFactories(Expression e)
    {
        var slim = e.ToExpressionSlim(ExpressionSlimFactory.Instance);
        return slim.ToExpression(ExpressionFactory.Instance);
    }
}
