// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Shared.Core
{
    [TestClass]
    public class ExpressionHeapTests
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
        [TestMethod]
        public void ExpressionHeap_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionHeap(default(IConstantHoister)), ex => Assert.AreEqual("hoister", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionHeap_Null()
        {
            Assert.IsNull(new ExpressionHeap().Create(default(Expression)).Value);
        }
#pragma warning restore IDE0034 // Simplify 'default' expression

        [TestMethod]
        public void ExpressionHeap_Roundtrip()
        {
            var exprs = new Expression[]
            {
                Expression.Parameter(typeof(int)),
                Expression.Parameter(typeof(string), Guid.NewGuid().ToString()),
                Expression.Parameter(typeof(long), "foo"),
                Expression.Parameter(typeof(double), "foo"),
                Expression.Default(typeof(int)),
                (Expression<Func<int>>)(() => 42),
                (Expression<Func<int, int>>)(x => x + 2),
                (Expression<Func<double, int>>)(x => (int)Math.Round(x) + 2),
                (Expression<Func<int[]>>)(() => new[] { 1, 2, 3 }),
                (Expression<Func<string>>)(() => Tuple.Create(1, "foo").Item2),
            };

            var heap = new ExpressionHeap();
            foreach (var expr in exprs)
            {
                AssertEqual(expr, heap.Create(expr).Value);
            }
        }

        [TestMethod]
        public void ExpressionHeap_Concurrency()
        {
            var exprs = new Expression[]
            {
                Expression.Parameter(typeof(int)),
                Expression.Parameter(typeof(string), Guid.NewGuid().ToString()),
                Expression.Parameter(typeof(long), "foo"),
                Expression.Parameter(typeof(double), "foo"),
                Expression.Default(typeof(int)),
                (Expression<Func<int>>)(() => 42),
                (Expression<Func<int, int>>)(x => x + 2),
                (Expression<Func<double, int>>)(x => (int)Math.Round(x) + 2),
                (Expression<Func<int[]>>)(() => new[] { 1, 2, 3 }),
                (Expression<Func<string>>)(() => Tuple.Create(1, "foo").Item2),
            };

            var rand = new Random(17);
            var tests = Enumerable.Repeat(exprs, 1000).SelectMany(x => x).OrderBy(_ => rand.Next());

            var heap = new ExpressionHeap();
            Parallel.ForEach(tests, e => AssertEqual(e, heap.Create(e).Value));
        }

        [TestMethod]
        public void ExpressionHeap_AssertSame()
        {
            var e1 = Expression.Lambda(Expression.Default(typeof(string)));
            var e2 = Expression.Lambda(Expression.Default(typeof(string)));
            Assert.AreNotSame(e1, e2);

            var heap = new ExpressionHeap();
            var r1 = heap.Create(e1);
            var r2 = heap.Create(e2);
            Assert.AreSame(r1.Value, r2.Value);
        }

        [TestMethod]
        public void ExpressionHeap_ConstantExpression_ExpressionValue()
        {
            var innerExpr = Expression.Lambda(Expression.Default(typeof(string)));
            var outerExpr = Expression.Constant(innerExpr);
            var innerCopy = Expression.Lambda(Expression.Default(typeof(string)));
            var heap = new ExpressionHeap();
            var outerRef = heap.Create(outerExpr);
            var copyRef = heap.Create(innerCopy);
            Assert.AreSame(((ConstantExpression)outerRef.Value).Value, copyRef.Value);
        }

        [TestMethod]
        public void ExpressionHeap_DummyCacheStorage()
        {
            var e1 = Expression.Lambda(Expression.Default(typeof(string)));
            var e2 = Expression.Lambda(Expression.Default(typeof(string)));
            Assert.AreNotSame(e1, e2);

            var heap = new ExpressionHeap(ConstantHoister.Create(false), _ => new Cache<Expression>(new DummyStorage()));
            var r1 = heap.Create(e1);
            var r2 = heap.Create(e2);
            Assert.AreNotSame(r1.Value, r2.Value);
        }

        [TestMethod]
        public void ExpressionHeap_ShareGlobals()
        {
            var p0 = Expression.Parameter(typeof(int));
            var p1 = Expression.Parameter(typeof(int));
            var ps = new Expression[]
            {
                /* 0 */ p0,
                /* 1 */ p1,
                /* 2 */ Expression.Parameter(typeof(int), "test:foo"),
                /* 3 */ Expression.Parameter(typeof(int), "test:foo"),
                /* 4 */ Expression.Parameter(typeof(int), "test:bar"),
                /* 5 */ Expression.Parameter(typeof(int), "not:foo"),
                /* 6 */ Expression.Parameter(typeof(int), "not:foo"),
                /* 7 */ Expression.Parameter(typeof(long), "test:foo"),
                /* 8 */ Expression.Invoke(Expression.Lambda(p0, p0), p0),
                /* 9 */ Expression.Invoke(Expression.Lambda(p0, p0), p1),
            };

            Assert.AreNotSame(ps[0], ps[1]);
            Assert.AreNotSame(ps[2], ps[3]);

            var heap = new TestHeap();
            var rs = ps.Select(p => heap.Create(p)).ToArray();

            Assert.AreNotSame(rs[0].Value, rs[1].Value);
            Assert.AreNotSame(rs[2].Value, rs[4].Value);
            Assert.AreNotSame(rs[5].Value, rs[6].Value);
            Assert.AreNotSame(rs[2].Value, rs[7].Value);
            Assert.AreSame(rs[2].Value, rs[3].Value);

            // Rebuild must be scoped
            Assert.AreSame(((InvocationExpression)ps[8]).Expression, ((InvocationExpression)rs[9].Value).Expression);
        }

        [TestMethod]
        public void ExpressionHeap_HoistedConstants_Shared()
        {
            var heap = new ExpressionHeap(ConstantHoister.Create(
                false,
                (Expression<Func<string, string>>)(c => string.Format(c, default(object)))));

            Expression<Func<object, string>> f1 = x => string.Format("{0}", x);
            Expression<Func<object, string>> f2 = x => string.Format("{0}", x);
            Assert.AreNotSame(f1, f2);

            var r1 = heap.Create(f1);
            var r2 = heap.Create(f2);
            var v1 = r1.Value;
            var v2 = r2.Value;
            Assert.AreSame(v1, v2);
        }

        [TestMethod]
        public void ExpressionHeap_CacheEqualityRegressionTest()
        {
            var cache = new ExpressionHeap();

            Expression<Func<bool>> f1 = () => object.Equals(42, 7.0);
            Expression<Func<bool>> f2 = () => object.Equals(7.0, 42);
            Expression<Func<bool>> f3 = () => object.Equals(42, 7.0);

            var r1 = cache.Create(f1);
            var r2 = cache.Create(f2);
            var r3 = cache.Create(f3);

            var eqc = new ExpressionEqualityComparer();
            Assert.IsTrue(eqc.Equals(f1, r1.Value));
            Assert.IsTrue(eqc.Equals(f2, r2.Value));
            Assert.IsTrue(eqc.Equals(f3, r3.Value));
        }

        [TestMethod]
        public void ExpressionHeap_ComparatorTests()
        {
            var comparer = GetExpressionHeapComparer();

            // Equals global parameters
            comparer.Equals(Expression.Parameter(typeof(int)), Expression.Parameter(typeof(long)));
            comparer.Equals(Expression.Parameter(typeof(int), "foo"), Expression.Parameter(typeof(long), "foo"));
            comparer.Equals(Expression.Parameter(typeof(int), "foo"), Expression.Parameter(typeof(long), "bar"));
            comparer.Equals(Expression.Parameter(typeof(int), "foo"), Expression.Parameter(typeof(int), "bar"));

            // Equals extension
            Assert.ThrowsException<NotImplementedException>(() => comparer.Equals(new ExtExpression(), new ExtExpression()));
            Assert.ThrowsException<NotImplementedException>(() => comparer.GetHashCode(new ExtExpression()));
        }

        private static IEqualityComparer<Expression> GetExpressionHeapComparer()
        {
            var comparer = default(IEqualityComparer<Expression>);
            var cache = new ExpressionHeap(ConstantHoister.Create(false), cmp => { comparer = cmp; return new Cache<Expression>(new CacheStorage<Expression>(cmp)); });
            return comparer;
        }

        private static void AssertEqual(Expression x, Expression y)
        {
            Assert.IsTrue(Comparer.Default.Equals(x, y), "Expected: {0}{2}Actual: {1}", x, y, Environment.NewLine);
        }

        private sealed class Comparer : ExpressionEqualityComparer
        {
            private Comparer() : base(() => new Comparator()) { }

            public static Comparer Default { get; } = new Comparer();

            private sealed class Comparator : ExpressionEqualityComparator
            {
                private const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

                protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
                {
                    return x.Name == y.Name && Equals(x.Type, y.Type);
                }

                protected override int GetHashCodeGlobalParameter(ParameterExpression obj)
                {
                    unchecked
                    {
                        var hash = obj.Name != null ? obj.Name.GetHashCode() : (int)Prime;
                        return (int)(hash * Prime) + GetHashCode(obj.Type);
                    }
                }
            }
        }

        private sealed class DummyStorage : ICacheStorage<Expression>
        {
            public int Count => 0;

            public IReference<Expression> GetEntry(Expression value)
            {
                return new Entry { Value = value };
            }

            public void ReleaseEntry(IReference<Expression> entry)
            {
            }

            private sealed class Entry : IReference<Expression>
            {
                public Expression Value
                {
                    get;
                    set;
                }
            }
        }

        private sealed class TestHeap : ExpressionHeap
        {
            protected override bool ShouldShareGlobal(ParameterExpression parameter) => parameter.Name != null && parameter.Name.StartsWith("test:");
        }

        private sealed class ExtExpression : Expression
        {
            public override ExpressionType NodeType => ExpressionType.Extension;

            public override Type Type => typeof(int);
        }

        private sealed class RefEqualType : IEquatable<RefEqualType>
        {
            public bool Equals(RefEqualType other) => object.ReferenceEquals(this, other);

            public override bool Equals(object obj) => obj is RefEqualType other && Equals(other);

            public override int GetHashCode() => 0;
        }
    }
}
