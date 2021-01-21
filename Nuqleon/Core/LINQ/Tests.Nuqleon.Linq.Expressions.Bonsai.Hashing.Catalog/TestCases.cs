// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Tests.System.Linq.Expressions.Bonsai.Hashing
{
    public class TestCases
    {
        public static IEnumerable<int> GetHashes() => GetExpressions().Select(GetHashCode);

        public static int GetHashCode(Expression expr)
        {
            var slim = expr?.ToExpressionSlim();

            return slim.GetStableHashCode(StableExpressionSlimHashingOptions.All);
        }

        public static IEnumerable<Expression> GetExpressions() => GetExpressionsUnique();

        public static IEnumerable<Expression> GetExpressionsUnique()
        {
            var px = Expression.Parameter(typeof(int), "x");
            var py = Expression.Parameter(typeof(int), "y");
            var pz = Expression.Parameter(typeof(int), "z");
            var b1 = Expression.Parameter(typeof(bool), "b1");
            var b2 = Expression.Parameter(typeof(bool), "b2");
            var e1 = Expression.Parameter(typeof(Exception), "e1");
            var e2 = Expression.Parameter(typeof(Exception), "e2");

            yield return null;

            // Constant
            yield return Expression.Constant(1);
            yield return Expression.Constant(1L);

            // Default
            yield return Expression.Default(typeof(int));
            yield return Expression.Default(typeof(string));

            // Parameter
            yield return Expression.Parameter(typeof(string), "x");
            yield return Expression.Parameter(typeof(int), "x");
            yield return Expression.Parameter(typeof(int), "y");
            yield return Expression.Parameter(typeof(int[]), "xs");
            yield return Expression.Parameter(typeof(int[,]), "xs");
            yield return Expression.Parameter(typeof(int[,,]), "xs");
            yield return Expression.Parameter(typeof(List<int>), "xs");
            yield return Expression.Parameter(typeof(Func<string, int>), "xs");
            yield return Expression.Parameter(typeof(Func<int, string>), "xs");

            // Unary
            yield return Expression.Negate(Expression.Constant(1));
            yield return Expression.Negate(Expression.Constant(49.95m));
            yield return Expression.Rethrow();

            // Binary
            yield return Expression.Add(Expression.Constant(1), Expression.Constant(2));
            yield return Expression.Add(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?)));
            yield return Expression.Add(Expression.Constant(49.95m), Expression.Constant(19.95m));
            yield return Expression.Equal(Expression.Constant(1), Expression.Constant(2));
            yield return Expression.Equal(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?)));
            yield return Expression.GreaterThan(Expression.Constant(1), Expression.Constant(2));
            yield return Expression.GreaterThan(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?)));

            // TypeBinary
            yield return Expression.TypeEqual(Expression.Constant(1), typeof(int));
            yield return Expression.TypeEqual(Expression.Constant(1), typeof(long));
            yield return Expression.TypeIs(Expression.Constant(1), typeof(int));
            yield return Expression.TypeIs(Expression.Constant(1), typeof(long));

            // Conditional
            yield return Expression.Condition(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2));
            yield return Expression.IfThenElse(Expression.Constant(true), Expression.Constant(1), Expression.Constant(2));
            yield return Expression.IfThen(Expression.Constant(true), Expression.Constant(1));

            // Call
            yield return Expression.Call(typeof(Math).GetMethod(nameof(Math.Abs), new[] { typeof(int) }), Expression.Constant(1));
            yield return Expression.Call(Expression.Constant("bar"), typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) }), Expression.Constant(1), Expression.Constant(2));
            yield return ((Expression<Func<Bar>>)(() => Activator.CreateInstance<Bar>()));
            yield return ((Expression<Func<IEnumerable<int>, Func<int, bool>, IEnumerable<int>>>)((xs, f) => xs.Where(f)));
            yield return ((Expression<Func<IEnumerable<string>, Func<string, int>, IEnumerable<int>>>)((xs, f) => xs.Select(f)));

            // New
            yield return Expression.New(typeof(TimeSpan));
            yield return Expression.New(typeof(TimeSpan).GetConstructor(new[] { typeof(long) }), Expression.Constant(42L));
            yield return ((Expression<Func<object>>)(() => new { a = 1 })).Body;
            yield return ((Expression<Func<object>>)(() => new { a = 1L })).Body;
            yield return ((Expression<Func<object>>)(() => new { b = 1 })).Body;

            // NewArray
            yield return ((Expression<Func<int[]>>)(() => new int[1])).Body;
            yield return ((Expression<Func<int[]>>)(() => new int[] { 1 })).Body;
            yield return ((Expression<Func<int[]>>)(() => new int[] { 1, 2 })).Body;

            // MemberInit
            yield return ((Expression<Func<Bar>>)(() => new Bar { Foo = 1 })).Body;
            yield return ((Expression<Func<Bar>>)(() => new Bar { Foo = 1, Qux = 1 })).Body;
            yield return ((Expression<Func<Bar>>)(() => new Bar { Qux = 1 })).Body;
            yield return ((Expression<Func<Bar>>)(() => new Bar(1) { Foo = 1 })).Body;
            yield return ((Expression<Func<Bar>>)(() => new Bar(1) { Qux = 1 })).Body;
            yield return ((Expression<Func<Bar>>)(() => new Bar(1) { Qux = 1, Baz = { Foo = 1 } })).Body;
            yield return ((Expression<Func<Bar>>)(() => new Bar(1) { Qux = 1, Xs = { 1 } })).Body;

            // ListInit
            yield return ((Expression<Func<List<int>>>)(() => new List<int> { 1 })).Body;
            yield return ((Expression<Func<List<int>>>)(() => new List<int> { 1, 2 })).Body;
            yield return ((Expression<Func<List<int>>>)(() => new List<int>(1) { 1 })).Body;
            yield return ((Expression<Func<List<int>>>)(() => new List<int>(1) { 1, 2 })).Body;

            // Member
            yield return Expression.MakeMemberAccess(expression: null, typeof(DateTime).GetProperty(nameof(DateTime.Now)));
            yield return Expression.MakeMemberAccess(Expression.Constant(new Bar()), typeof(Bar).GetField(nameof(Bar.Foo)));
            yield return Expression.MakeMemberAccess(Expression.Constant(new Bar()), typeof(Bar).GetProperty(nameof(Bar.Qux)));

            // Index
            yield return Expression.MakeIndex(Expression.Default(typeof(List<int>)), typeof(List<int>).GetProperty("Item"), new[] { Expression.Constant(1) });

            // Invocation
            yield return Expression.Invoke(Expression.Parameter(typeof(Func<int>)));
            yield return Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(1));

            // Lambda
            yield return (Expression<Func<int>>)(() => 1);
            yield return (Expression<Func<int, int>>)(x => x);
            yield return (Expression<Func<long, long>>)(x => x);
            yield return (Expression<Func<int, int, int>>)((x, y) => x + y);
            yield return Expression.Lambda(Expression.Lambda(px, px), px);
            yield return Expression.Lambda(Expression.Lambda(px, py), px);

            // Block
            yield return Expression.Block(Expression.Empty());
            yield return Expression.Block(Expression.Empty(), Expression.Empty());
            yield return Expression.Block(px);
            yield return Expression.Block(px, py);
            yield return Expression.Block(new[] { px }, Expression.Empty());
            yield return Expression.Block(new[] { px }, px);
            yield return Expression.Block(new[] { px }, py);
            yield return Expression.Block(new[] { px, py }, px, py);
            yield return Expression.Block(new[] { px, py }, py, px);
            yield return Expression.Block(typeof(void), new[] { px }, px);

            // Try
            yield return Expression.TryCatch(px, Expression.Catch(typeof(Exception), py));
            yield return Expression.TryCatch(px, Expression.Catch(typeof(DivideByZeroException), py));
            yield return Expression.TryCatch(px, Expression.Catch(typeof(DivideByZeroException), py, b1));
            yield return Expression.TryCatch(px, Expression.Catch(typeof(DivideByZeroException), py, b2));
            yield return Expression.TryCatch(px, Expression.Catch(e1, py));
            yield return Expression.TryCatch(px, Expression.Catch(e1, pz));
            yield return Expression.TryCatch(px, Expression.Catch(e2, py, b1));
            yield return Expression.TryCatch(px, Expression.Catch(e2, py, b2));
            yield return Expression.TryCatch(px, Expression.Catch(e1, py), Expression.Catch(e2, pz));
            yield return Expression.TryCatch(px, Expression.Catch(e1, pz), Expression.Catch(e2, py));
            yield return Expression.TryFault(Expression.Empty(), Expression.Empty());
            yield return Expression.TryFault(px, Expression.Empty());
            yield return Expression.TryFault(px, py);
            yield return Expression.TryFinally(Expression.Empty(), Expression.Empty());
            yield return Expression.TryFinally(px, Expression.Empty());
            yield return Expression.TryFinally(px, py);

            // Switch
            yield return Expression.Switch(px, Expression.SwitchCase(Expression.Empty(), Expression.Constant(1)));
            yield return Expression.Switch(px, Expression.SwitchCase(Expression.Empty(), Expression.Constant(1), Expression.Constant(2)));
            yield return Expression.Switch(px, Expression.SwitchCase(Expression.Empty(), Expression.Constant(1)), Expression.SwitchCase(Expression.Empty(), Expression.Constant(2)));
            yield return Expression.Switch(px, pz, Expression.SwitchCase(py, Expression.Constant(1)));
            yield return Expression.Switch(px, py, Expression.SwitchCase(pz, Expression.Constant(1)));
            yield return Expression.Switch(typeof(void), px, py, comparison: null, Expression.SwitchCase(pz, Expression.Constant(1)));
            yield return Expression.Switch(typeof(void), px, py, typeof(Bar).GetMethod("EqualsInt32"), Expression.SwitchCase(pz, Expression.Constant(1)));

            // Label
            yield return Expression.Label(Expression.Label());
            yield return Expression.Label(Expression.Label("bar"));
            yield return Expression.Label(Expression.Label("foo"));
            yield return Expression.Label(Expression.Label(typeof(byte)), Expression.Constant((byte)42));
            yield return Expression.Label(Expression.Label(typeof(int)), px);
            yield return Expression.Label(Expression.Label(typeof(int), "bar"), px);
            yield return Expression.Label(Expression.Label(typeof(int), "foo"), px);

            // Goto
            yield return Expression.Goto(Expression.Label());
            yield return Expression.Goto(Expression.Label(typeof(int)), px);
            yield return Expression.Goto(Expression.Label(typeof(int)), px, typeof(long));
            yield return Expression.Return(Expression.Label());
            yield return Expression.Return(Expression.Label(typeof(int)), px);
            yield return Expression.Return(Expression.Label(typeof(int)), px, typeof(long));
            yield return Expression.Break(Expression.Label());
            yield return Expression.Break(Expression.Label(typeof(int)), px);
            yield return Expression.Break(Expression.Label(typeof(int)), px, typeof(long));
            yield return Expression.Continue(Expression.Label());
            yield return Expression.Continue(Expression.Label(), typeof(long));

            // Loop
            yield return Expression.Loop(px);
            yield return Expression.Loop(Expression.Empty());
            yield return Expression.Loop(Expression.Empty(), Expression.Label(typeof(void), "l1"));
            yield return Expression.Loop(Expression.Empty(), Expression.Label(typeof(void), "l1"), Expression.Label(typeof(void), "l2"));
        }

        public static IEnumerable<IEnumerable<Expression>> GetExpressionsEquivalent()
        {
            var px = Expression.Parameter(typeof(int), "x");
            var py = Expression.Parameter(typeof(int), "y");
            var pg = Expression.Parameter(typeof(int), "g");
            var e1 = Expression.Parameter(typeof(Exception), "e1");
            var e2 = Expression.Parameter(typeof(Exception), "e2");

            yield return new Expression[]
            {
                Expression.Constant(1),
                Expression.Constant(2),
            };

            yield return new Expression[]
            {
                Expression.Default(typeof(int)),
                Expression.Default(typeof(int)),
            };

            yield return new Expression[]
            {
                pg,
                pg,
            };

            yield return new Expression[]
            {
                Expression.Lambda(Expression.Lambda(px, px), px),
                Expression.Lambda(Expression.Lambda(px, px), py),
                Expression.Lambda(Expression.Lambda(py, py), px),
            };

            yield return new Expression[]
            {
                Expression.Lambda(Expression.Lambda(px, py), px),
                Expression.Lambda(Expression.Lambda(py, px), py),
            };

            yield return new Expression[]
            {
                Expression.Lambda(pg, px),
                Expression.Lambda(pg, py),
            };

            yield return new Expression[]
            {
                Expression.Block(new[] { px }, px),
                Expression.Block(new[] { py }, py),
            };

            yield return new Expression[]
            {
                Expression.Block(new[] { px }, px),
                Expression.Block(typeof(int), new[] { px }, px),
            };

            yield return new Expression[]
            {
                Expression.TryCatch(Expression.Constant(0), Expression.Catch(typeof(Exception), Expression.Constant(-1))),
                Expression.TryCatch(Expression.Constant(1), Expression.Catch(typeof(Exception), Expression.Constant(-1))),
                Expression.TryCatch(Expression.Constant(1), Expression.Catch(typeof(Exception), Expression.Constant(-2))),
            };

            yield return new Expression[]
            {
                Expression.TryCatch(px, Expression.Catch(e1, py)),
                Expression.TryCatch(px, Expression.Catch(e2, py)),
            };
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private sealed class Bar
        {
            public Bar()
            {
            }

            public Bar(int foo)
            {
            }

            public int Foo;
            public int Qux { get; set; }

            public Baz Baz { get; } = new Baz();
            public List<int> Xs { get; } = new List<int>();

            public static bool EqualsInt32(int x, int y) => false;
        }
#pragma warning restore IDE0060 // Remove unused parameter

        private sealed class Baz
        {
            public int Foo;
        }
    }
}
