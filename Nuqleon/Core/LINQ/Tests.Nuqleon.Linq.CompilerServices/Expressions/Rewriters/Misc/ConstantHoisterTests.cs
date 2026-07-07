// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tests.System.Linq.CompilerServices;

[TestClass]
public class ConstantHoisterTests
{
    [TestMethod]
    public void ConstantHoister_ArgumentChecking()
    {
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ConstantHoister.Hoist(expression: null, useDefaultForNull: false));
        Assert.AreEqual("expression", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => ConstantHoister.Create(useDefaultForNull: false, exclusions: null));
        Assert.AreEqual("exclusions", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => ConstantHoister.Create(useDefaultForNull: false).Hoist(expression: null));
        Assert.AreEqual("expression", ex3.ParamName);
    }

    [TestMethod]
    public void ConstantHoister_Constant1()
    {
        var c = Expression.Constant(42);
        HoistAndEval(c);
    }

    [TestMethod]
    public void ConstantHoister_Constant2()
    {
        var c = Expression.Constant(new Bar());
        HoistAndEval(c);
    }

    [TestMethod]
    public void ConstantHoister_Simple1()
    {
        var b = Expression.Constant(new Bar());
        var c = Expression.Equal(b, b);
        var res = HoistAndEval(c);
        Assert.HasCount(1, res.Environment);
    }

    [TestMethod]
    public void ConstantHoister_Simple2()
    {
        var b = Expression.Constant(new Bar());
        var c = Expression.NotEqual(b, b);
        var res = HoistAndEval(c);
        Assert.HasCount(1, res.Environment);
    }

    [TestMethod]
    public void ConstantHoister_Simple3()
    {
        var b = Expression.Constant(new Bar());
        var c = Expression.Call(b, (MethodInfo)ReflectionHelpers.InfoOf((Bar _) => _.Equals(_)), b);
        var res = HoistAndEval(c);
        Assert.HasCount(1, res.Environment);
    }

    [TestMethod]
    public void ConstantHoister_TypeSeparation()
    {
        var e = Expression.Add(Expression.Constant(new Bar()), Expression.Constant(new Foo()));
        var res = ConstantHoister.Hoist(e, useDefaultForNull: false);
        Assert.HasCount(2, res.Environment);
    }

    [TestMethod]
    public void ConstantHoister_Null1()
    {
        var c = Expression.Constant(value: null, typeof(string));
        var res = ConstantHoister.Hoist(c, useDefaultForNull: false);
        Assert.HasCount(1, res.Environment);
        var e = res.Environment.Single();
        Assert.AreEqual(c.Type, e.Key.Type);
        Assert.AreEqual(default(string), e.Value);
    }

    [TestMethod]
    public void ConstantHoister_Null2()
    {
        var c = Expression.Constant(value: null, typeof(int?));
        var res = ConstantHoister.Hoist(c, useDefaultForNull: false);
        Assert.HasCount(1, res.Environment);
        var e = res.Environment.Single();
        Assert.AreEqual(c.Type, e.Key.Type);
        Assert.AreEqual(default(int?), e.Value);
    }

    [TestMethod]
    public void ConstantHoister_Null3()
    {
        var c = Expression.Constant(value: null, typeof(string));
        var res = ConstantHoister.Hoist(c, useDefaultForNull: true);
        Assert.IsEmpty(res.Environment);
        var e = res.Expression as DefaultExpression;
        Assert.IsNotNull(e);
        Assert.AreEqual(c.Type, e.Type);
    }

    [TestMethod]
    public void ConstantHoister_ManOrBoy()
    {
#pragma warning disable IDE0079 // The following supression is flagged as unnecessary on .NET Framework (but is required for other targets)
#pragma warning disable CA1845  // Use span-based 'string.Concat' and 'AsSpan' instead of 'Substring'
        var e = (Expression<Func<IEnumerable<int>, IEnumerable<string>>>)(xs => from x in xs let y = x + 1 where y > 0 let s = x.ToString() where !s.EndsWith("Foo") select s.Substring(0, 1) + "Foo");
#pragma warning restore CA1847
#pragma warning restore IDE0079
        var c = ConstantHoister.Hoist(e, useDefaultForNull: false);

        Assert.HasCount(3, c.Environment);
        Assert.Contains(0, c.Environment.Values);
        Assert.Contains(1, c.Environment.Values);
        Assert.Contains("Foo", c.Environment.Values);

        var i = (Expression)c.ToInvocation();

        Assert.AreEqual("((Func<int, int, string, Func<IEnumerable<int>, IEnumerable<string>>>)((int @p0, int @p1, string @p2) => (IEnumerable<int> xs) => xs.Select((int x) => new { x, y = x + @p0 }).Where(t => t.y > @p1).Select(t => new { t, s = t.x.ToString() }).Where(t => !t.s.EndsWith(@p2)).Select(t => t.s.Substring(@p1, @p0) + @p2)))(1, 0, \"Foo\")", i.ToCSharpString());
        Assert.AreEqual("int @p0 = 1;\r\nint @p1 = 0;\r\nstring @p2 = \"Foo\";\r\nreturn (IEnumerable<int> xs) => xs.Select((int x) => new { x, y = x + @p0 }).Where(t => t.y > @p1).Select(t => new { t, s = t.x.ToString() }).Where(t => !t.s.EndsWith(@p2)).Select(t => t.s.Substring(@p1, @p0) + @p2);\r\n", c.ToCSharpString());

        var t = c.ToString();
        var u = Regex.Replace(t, @"\<\>f__AnonymousType[\da-fA-F]*", "anon");
        var v = Regex.Replace(u, @"\<\>h__TransparentIdentifier[\da-fA-F]*", "tran");

        Assert.AreEqual("(xs => xs.Select(x => new anon`2(x = x, y = (x + @p0))).Where(tran => (tran.y > @p1)).Select(tran => new anon`2(tran = tran, s = tran.x.ToString())).Where(tran => Not(tran.s.EndsWith(@p2))).Select(tran => (tran.s.Substring(@p1, @p0) + @p2)))[|@p0 : System.Int32 = 1, @p1 : System.Int32 = 0, @p2 : System.String = \"Foo\"|]", v);

        i = AnonymousTypeTupletizer.Tupletize(i, Expression.Constant(value: null));
        i = CompilerGeneratedNameEliminator.Prettify(i);

        var f = i.Evaluate<Func<IEnumerable<int>, IEnumerable<string>>>();
        var g = f([123, 234, 987, 876]);
        Assert.IsTrue(new[] { "1Foo", "2Foo", "9Foo", "8Foo" }.SequenceEqual(g));

        Assert.AreEqual(@"Invoke((@p0, @p1, @p2) => xs => xs.Select(x => new Tuple`2(x, (x + @p0))).Where(t => (t.Item2 > @p1)).Select(t => new Tuple`2(t, t.Item1.ToString())).Where(t => Not(t.Item2.EndsWith(@p2))).Select(t => (t.Item2.Substring(@p1, @p0) + @p2)), 1, 0, ""Foo"")", i.ToString());
    }

    [TestMethod]
    public void ConstantHoister_Exclusions_Checks()
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
#pragma warning disable IDE0004 // Cast is redundant.
        var ex = Assert.ThrowsExactly<ArgumentException>(() =>
        {
            ConstantHoister.Create(false,
                (Expression<Func<int, int>>)(h => h + 1)
            );
        });
        Assert.AreEqual(typeof(ArgumentException), ex.GetType());

        Assert.Contains("not a method call, new, or member access", ex.Message);

        var ex2 = Assert.ThrowsExactly<ArgumentException>(() =>
        {
            ConstantHoister.Create(false,
                (Expression<Func<string, string>>)(h => string.Format(h, /* convert */ 42))
            );
        });
        Assert.AreEqual(typeof(ArgumentException), ex2.GetType());

        Assert.Contains("not a constant or a default expression", ex2.Message);

        var ex3 = Assert.ThrowsExactly<ArgumentException>(() =>
        {
            ConstantHoister.Create(false,
                (Expression<Func<string>>)(() => string.Format("bar", default(object[])))
            );
        });
        Assert.AreEqual(typeof(ArgumentException), ex3.GetType());

        Assert.Contains("no holes for constants", ex3.Message);

        var ex4 = Assert.ThrowsExactly<ArgumentException>(() =>
        {
            ConstantHoister.Create(false,
                (Expression<Func<string, string>>)(h => string.Format("bar", default(object[])))
            );
        });
        Assert.AreEqual(typeof(ArgumentException), ex4.GetType());

        Assert.Contains("not used", ex4.Message);

        var ex5 = Assert.ThrowsExactly<ArgumentException>(() =>
        {
            ConstantHoister.Create(false,
                (Expression<Func<string, string>>)(h => string.Concat(h, h))
            );
        });
        Assert.AreEqual(typeof(ArgumentException), ex5.GetType());

        Assert.Contains("used multiple times", ex5.Message);
#pragma warning restore IDE0034 // Simplify 'default' expression
#pragma warning restore IDE0004 // Cast is redundant.
    }

    [TestMethod]
    public void ConstantHoister_Exclusions()
    {
        var hp = Expression.Parameter(typeof(string));
        var concat = (MethodInfo)ReflectionHelpers.InfoOf(() => string.Concat("a", "b"));

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
        var hoister = ConstantHoister.Create(false,
            (Expression<Func<string, string>>)(h => string.Format(h, default(object[]))),
            (Expression<Func<string, Regex>>)(h => new Regex(h)),
            (Expression<Func<string, int>>)(h => h.Length),
            (Expression<Func<string, string>>)(h => h.ToUpper()),
            Expression.Lambda<Func<string, string>>(Expression.Call(concat, hp, Expression.Default(typeof(string))), hp),
            Expression.Lambda<Func<string, string>>(Expression.Call(concat, Expression.Default(typeof(string)), hp), hp)
        );
#pragma warning restore IDE0034

        var e1 = (Expression<Func<string>>)(() => string.Format("{0}:{1}", new object[] { 123, "foo" }));
        var e2 = (Expression<Func<bool>>)(() => new Regex("abc").IsMatch("bar"));
        var e3 = (Expression<Func<int>>)(() => "qux".Length);
        var e4 = (Expression<Func<string>>)(() => "baz".ToUpper());
        var e5 = (Expression<Func<string>>)(() => string.Concat("bar", "foo"));

        var r1 = HoistAndEval(hoister, e1.Body);
        var r2 = HoistAndEval(hoister, e2.Body);
        var r3 = HoistAndEval(hoister, e3.Body);
        var r4 = HoistAndEval(hoister, e4.Body);
        var r5 = HoistAndEval(hoister, e5.Body);

        Assert.HasCount(2, r1.Environment);
        Assert.Contains(v => v == 123, r1.Environment.Values.OfType<int>());
        Assert.Contains(v => v == "foo", r1.Environment.Values.OfType<string>());

        Assert.HasCount(1, r2.Environment);
        Assert.Contains(v => v == "bar", r2.Environment.Values.OfType<string>());

        Assert.IsEmpty(r3.Environment);

        Assert.IsEmpty(r4.Environment);

        Assert.IsEmpty(r5.Environment);
    }

    [TestMethod]
    public void ConstantHoister_Custom_InterningBehavior()
    {
        var hoister = new CustomHoister();

        var add1 = Expression.Add(Expression.Constant(1), Expression.Constant(2));

        var ol1 = add1.Left;
        var or1 = add1.Right;

        var hi1 = hoister.Hoist(add1).ToInvocation();

        var hl1 = (ConstantExpression)hi1.Arguments[0];
        var hr1 = (ConstantExpression)hi1.Arguments[1];

        Assert.AreSame(ol1, hl1);
        Assert.AreSame(or1, hr1);

        var add2 = Expression.Add(Expression.Constant(1), Expression.Constant(2));

        var ol2 = add2.Left;
        var or2 = add2.Right;

        Assert.AreNotSame(ol1, ol2);
        Assert.AreNotSame(or1, or2);

        var hi2 = hoister.Hoist(add2).ToInvocation();

        var hl2 = (ConstantExpression)hi2.Arguments[0];
        var hr2 = (ConstantExpression)hi2.Arguments[1];

        Assert.AreSame(ol1, hl2);
        Assert.AreSame(or1, hr2);
    }

    [TestMethod]
    public void ConstantHoister_Custom_Bad()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => _ = new CustomBadHoister1().Hoist(Expression.Constant(1)));
        Assert.ThrowsExactly<InvalidOperationException>(() => _ = new CustomBadHoister2().Hoist(Expression.Constant(1)));
    }

    private static ExpressionWithEnvironment HoistAndEval(Expression e)
    {
        var c1 = ConstantHoister.Hoist(e, useDefaultForNull: false);
        var c2 = ConstantHoister.Hoist(e, useDefaultForNull: true);

        var i1 = c1.ToInvocation().Evaluate();
        var i2 = c2.ToInvocation().Evaluate();

        var e1 = e.Evaluate();

        Assert.AreEqual(e1, i1);
        Assert.AreEqual(e1, i2);

        return c1;
    }

    private static ExpressionWithEnvironment HoistAndEval(ConstantHoister h, Expression e)
    {
        var c = h.Hoist(e);

        var i = c.ToInvocation().Evaluate();

        var r = e.Evaluate();

        Assert.AreEqual(r, i);

        return c;
    }

    private sealed class Bar
    {
        public override bool Equals(object obj) => obj is Bar or Foo;

        public override int GetHashCode() => typeof(Foo).GetHashCode() * 17; // defeating the GetHashCode in TypeAwareComparer to create collisions

#pragma warning disable IDE0060 // Remove unused parameter (https://github.com/dotnet/roslyn/issues/32852)
        public static int operator +(Bar b, Foo f) => 0;
#pragma warning restore IDE0060 // Remove unused parameter

        public static bool operator ==(Bar b1, Bar b2) => ReferenceEquals(b1, b2);

        public static bool operator !=(Bar b1, Bar b2) => !ReferenceEquals(b1, b2);
    }

    private sealed class Foo
    {
        public override bool Equals(object obj) => obj is Bar or Foo;

        public override int GetHashCode() => typeof(Bar).GetHashCode() * 17; // defeating the GetHashCode in TypeAwareComparer to create collisions
    }

    private class CustomHoister : ConstantHoister
    {
        private readonly Dictionary<KeyValuePair<Type, object>, ConstantExpression> _cache = [];

        protected override Expression Hoist(ConstantExpression expression)
        {
            if (!_cache.TryGetValue(new KeyValuePair<Type, object>(expression.Type, expression.Value), out var res))
            {
                res = expression;
                _cache.Add(new KeyValuePair<Type, object>(expression.Type, expression.Value), expression);
            }

            return res;
        }
    }

    private sealed class CustomBadHoister1 : ConstantHoister
    {
        protected override Expression Hoist(ConstantExpression expression) => null;
    }

    private sealed class CustomBadHoister2 : ConstantHoister
    {
        protected override Expression Hoist(ConstantExpression expression) => Expression.Default(typeof(DateTime));
    }
}
