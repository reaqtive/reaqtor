// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class EvaluationTests
    {
        [TestMethod]
        public void CachedLambdaCompiler_ArgumentChecking()
        {
            var f = (Expression<Action>)(() => Console.WriteLine());
            var l = (LambdaExpression)f;
            var c = VoidCompiledDelegateCache.Instance;
            var h = ConstantHoister.Create(useDefaultForNull: false);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(default(LambdaExpression), c));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(l, cache: null));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(default(Expression<Action>), c));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(f, cache: null));

            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(default(LambdaExpression), c, outliningEnabled: false));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(l, cache: null, outliningEnabled: false));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(default(Expression<Action>), c, outliningEnabled: false));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(f, cache: null, outliningEnabled: false));

            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(default(LambdaExpression), c, outliningEnabled: false, h));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(l, cache: null, outliningEnabled: false, h));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(default(Expression<Action>), c, outliningEnabled: false, h));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(f, cache: null, outliningEnabled: false, h));

            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(l, c, outliningEnabled: false, hoister: null));
            Assert.ThrowsException<ArgumentNullException>(() => CachedLambdaCompiler.Compile(f, c, outliningEnabled: false, hoister: null));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple1()
        {
            var added = 0;

            var c = new MyCache();
            c.Added += () => added++;

            Expression<Func<int>> f = () => Environment.ProcessorCount;

            var d1 = f.Compile(c);
            Assert.AreEqual(1, added);

            var d2 = f.Compile(c);
            Assert.AreEqual(1, added);

            Assert.AreEqual(Environment.ProcessorCount, d1());
            Assert.AreEqual(Environment.ProcessorCount, d2());
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple1b()
        {
            var added = 0;

            var c = new MyCache();
            c.Added += () => added++;

            Expression<Func<int>> f = () => Environment.ProcessorCount;

            var d1 = f.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            var d2 = f.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            Assert.AreEqual(Environment.ProcessorCount, d1());
            Assert.AreEqual(Environment.ProcessorCount, d2());
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple2()
        {
            var added = 0;

            var c = new MyCache();
            c.Added += () => added++;

            Expression<Func<int>> f1 = () => 42;
            Expression<Func<int>> f2 = () => 43;
            Expression<Func<int>> f3 = () => 44;

            var d1 = f1.Compile(c);
            Assert.AreEqual(1, added);

            var d2 = f1.Compile(c);
            Assert.AreEqual(1, added);

            var d3 = f2.Compile(c);
            Assert.AreEqual(1, added);

            var d4 = f3.Compile(c);
            Assert.AreEqual(1, added);

            Assert.AreEqual(42, d1());
            Assert.AreEqual(42, d2());
            Assert.AreEqual(43, d3());
            Assert.AreEqual(44, d4());
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple2b()
        {
            var added = 0;

            var c = new MyCache();
            c.Added += () => added++;

            Expression<Func<int>> f1 = () => 42;
            Expression<Func<int>> f2 = () => 43;
            Expression<Func<int>> f3 = () => 44;

            var d1 = f1.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            var d2 = f1.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            var d3 = f2.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            var d4 = f3.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            Assert.AreEqual(42, d1());
            Assert.AreEqual(42, d2());
            Assert.AreEqual(43, d3());
            Assert.AreEqual(44, d4());
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple3()
        {
            var added = 0;

            var c = new MyCache();
            c.Added += () => added++;

            Expression<Func<int, int>> f1 = x => x + 1;
            Expression<Func<int, int>> f2 = x => x + 2;
            Expression<Func<int, int>> f3 = y => y + 3;

            var d1 = f1.Compile(c);
            Assert.AreEqual(1, added);

            var d2 = f2.Compile(c);
            Assert.AreEqual(1, added);

            var d3 = f3.Compile(c);
            Assert.AreEqual(1, added);

            Assert.AreEqual(43, d1(42));
            Assert.AreEqual(46, d2(44));
            Assert.AreEqual(49, d3(46));
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple3b()
        {
            var added = 0;

            var c = new MyCache();
            c.Added += () => added++;

            Expression<Func<int, int>> f1 = x => x + 1;
            Expression<Func<int, int>> f2 = x => x + 2;
            Expression<Func<int, int>> f3 = y => y + 3;

            var d1 = f1.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            var d2 = f2.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            var d3 = f3.Compile(c, outliningEnabled: false);
            Assert.AreEqual(1, added);

            Assert.AreEqual(43, d1(42));
            Assert.AreEqual(46, d2(44));
            Assert.AreEqual(49, d3(46));
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple4()
        {
            var added1 = 0;
            var added2 = 0;

            var c1 = new MyCache();
            c1.Added += () => added1++;

            var c2 = new MyCache();
            c2.Added += () => added2++;

            Expression<Func<int, int>> f1 = x => x + 1;
            Expression<Func<int, int>> f2 = x => x + 2;
            Expression<Func<int, int>> f3 = y => y + 3;

            var d1 = f1.Compile(c1);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(0, added2);

            var d2 = f2.Compile(c2);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(1, added2);

            var d3 = ((LambdaExpression)f3).Compile(c1);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(1, added2);

            var d4 = ((LambdaExpression)f3).Compile(c2);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(1, added2);

            Assert.AreEqual(43, d1(42));
            Assert.AreEqual(46, d2(44));
            Assert.AreEqual(49, ((Func<int, int>)d3)(46));
            Assert.AreEqual(51, ((Func<int, int>)d4)(48));
        }

        [TestMethod]
        public void CachedLambdaCompiler_Simple4b()
        {
            var added1 = 0;
            var added2 = 0;

            var c1 = new MyCache();
            c1.Added += () => added1++;

            var c2 = new MyCache();
            c2.Added += () => added2++;

            Expression<Func<int, int>> f1 = x => x + 1;
            Expression<Func<int, int>> f2 = x => x + 2;
            Expression<Func<int, int>> f3 = y => y + 3;

            var d1 = f1.Compile(c1, outliningEnabled: false);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(0, added2);

            var d2 = f2.Compile(c2, outliningEnabled: false);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(1, added2);

            var d3 = ((LambdaExpression)f3).Compile(c1, outliningEnabled: false);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(1, added2);

            var d4 = ((LambdaExpression)f3).Compile(c2, outliningEnabled: false);
            Assert.AreEqual(1, added1);
            Assert.AreEqual(1, added2);

            Assert.AreEqual(43, d1(42));
            Assert.AreEqual(46, d2(44));
            Assert.AreEqual(49, ((Func<int, int>)d3)(46));
            Assert.AreEqual(51, ((Func<int, int>)d4)(48));
        }

        [TestMethod]
        public void CachedLambdaCompiler_BigLambda()
        {
            var added = 0;

            var cache = new MyCache();
            cache.Added += () => added++;

            var rnd = new Random(1983);

            const int N = 24;
            const int M = 4;

            for (var n = 0; n < N; n++)
            {
                var ps = Enumerable.Range(0, n).Select(i => Expression.Parameter(typeof(int))).ToArray();

                for (var j = 0; j < M; j++)
                {
                    var cs = Enumerable.Range(0, n).Select(i => Expression.Constant(rnd.Next(0, 100))).ToArray();
                    var ex = ps.Zip(cs, (p, c) => (p, c)).Aggregate((Expression)Expression.Constant(42), (a, pc) => Expression.Add(a, Expression.Multiply(pc.p, pc.c)));

                    var f = Expression.Lambda(ex, ps);

                    var args = Enumerable.Range(0, n).Select(i => (object)rnd.Next(0, 100)).ToArray();

                    var d1 = f.Compile();
                    var d2 = f.Compile(cache);

                    var res1 = d1.DynamicInvoke(args);
                    var res2 = d2.DynamicInvoke(args);

                    Assert.AreEqual(res1, res2);
                }
            }

            if (Type.GetType("Mono.Runtime") == null)
            {
                Assert.AreEqual(24, added);
            }
            else
            {
                //
                // NB: Delegate caching for arities > 16 does not work on Mono. We get fresh runtime-generated delegate
                //     types every time we construct a new LambdaExpression of high arity.
                //
                //     We'll just check on upper and lower bounds for caching. In practice we've seen values of 45, but
                //     it's flaky to rely on that exact number. (45 = 17 + 4 * 7 where the latter product illustrates
                //     the lack of caching for higher arity delegates.)
                //

                Assert.IsTrue(added is >= N and < N * M);
            }
        }

        [TestMethod]
        public void CachedLambdaCompiler_TooManyConstantsForLCGTypes()
        {
#if NETCOREAPP2_1 || NETCOREAPP3_1 || NET5_0
            var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("EvalTests_foo"), AssemblyBuilderAccess.RunAndCollect);
#else
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("EvalTests_foo"), AssemblyBuilderAccess.RunAndCollect);
#endif
            var mod = asm.DefineDynamicModule("mod");
            var typ = mod.DefineType("qux", TypeAttributes.Class);
            var qux = typ.CreateType();

            var qxs = Enumerable.Range(0, 17).Select(_ => Activator.CreateInstance(qux)).Select(q => Expression.Constant(q, qux)).ToArray();
            var exp = Expression.NewArrayInit(qux, qxs);

            var del = Expression.Lambda(exp).Compile(new SimpleCompiledDelegateCache());

            Assert.AreEqual(17, ((Array)del.DynamicInvoke()).Length);
        }

        [TestMethod]
        public void CachedLambdaCompiler_AllSortsOfCaches()
        {
            var cs = new ICompiledDelegateCache[]
            {
                VoidCompiledDelegateCache.Instance,
                new SimpleCompiledDelegateCache(),
                new LeastRecentlyUsedCompiledDelegateCache(4)
            };

            var es = new LambdaExpression[]
            {
                (Expression<Func<int>>)(() => 42),
                (Expression<Func<int, int>>)(x => x),
                (Expression<Func<int, int>>)(x => x * 2),
                (Expression<Func<int, int>>)(y => y),
                (Expression<Func<int, int, int>>)((a, b) => a * 2 + b * 3),
                (Expression<Func<int>>)(() => 43),
                (Expression<Func<int, int>>)(x => x * 17),
                (Expression<Func<int, int, int>>)((a, b) => a * 7 + b * 4),
                (Expression<Func<int, int, int>>)((a, b) => a * 8 + b * 5),
                (Expression<Func<int, int>>)(x => 3 * x),
                (Expression<Func<int>>)(() => 44),
                (Expression<Func<int, int>>)(x => 1 + x),
                (Expression<Func<int, int>>)(x => 2 + x),
            };

            var nums = new[] { 7, 12, 64, 49, 18 };

            foreach (var e in es)
            {
                var d0 = e.Compile();

                var args = nums.Take(e.Parameters.Count).Cast<object>().ToArray();

                var r = d0.DynamicInvoke(args);

                foreach (var c in cs)
                {
                    var d = e.Compile(c);

                    var s = d.DynamicInvoke(args);

                    Assert.AreEqual(s, r);
                }
            }
        }

        [TestMethod]
        public void CachedLambdaCompiler_LRU()
        {
            var lru = new LeastRecentlyUsedCompiledDelegateCache(4);

            int hit = 0, add = 0, evict = 0;
            var evicted = new List<CacheEventArgs>();

            var assert = AssertHelper(() => (hit, add, evict));

            lru.Hit += (o, e) => { hit++; };
            lru.Added += (o, e) => { add++; };
            lru.Evicted += (o, e) => { evict++; evicted.Add(e); };


            var l1 = (Expression<Func<int>>)(() => 42);

            var d1 = lru.GetOrAdd(l1);
            assert((hit: 0, add: 1, evict: 0));

            var d2 = lru.GetOrAdd(l1);
            assert((hit: 1, add: 1, evict: 0));
            Assert.AreSame(d1, d2);

            var d3 = lru.GetOrAdd(l1);
            assert((hit: 2, add: 1, evict: 0));
            Assert.AreSame(d1, d3);


            var l2 = (Expression<Func<int>>)(() => 43);

            var d4 = lru.GetOrAdd(l2);
            assert((hit: 2, add: 2, evict: 0));

            var d5 = lru.GetOrAdd(l2);
            assert((hit: 3, add: 2, evict: 0));
            Assert.AreSame(d4, d5);

            var d6 = lru.GetOrAdd(l2);
            assert((hit: 4, add: 2, evict: 0));
            Assert.AreSame(d4, d6);


            var l3 = (Expression<Func<int>>)(() => 44);

            var d7 = lru.GetOrAdd(l3);
            assert((hit: 4, add: 3, evict: 0));


            var l4 = (Expression<Func<int>>)(() => 45);

            var d8 = lru.GetOrAdd(l4);
            assert((hit: 4, add: 4, evict: 0));


            var d11 = lru.GetOrAdd(l2);
            assert((hit: 5, add: 4, evict: 0));
            Assert.AreSame(d4, d11);


            var l5 = (Expression<Func<int>>)(() => 46);

            var d9 = lru.GetOrAdd(l5);
            assert((hit: 5, add: 5, evict: 1));

            Assert.AreSame(d1, evicted[0].Delegate);
            Assert.AreSame(l1, evicted[0].Lambda);

            var d10 = lru.GetOrAdd(l1);
            assert((hit: 5, add: 6, evict: 2));

            Assert.AreSame(d7, evicted[1].Delegate);
            Assert.AreSame(l3, evicted[1].Lambda);

            Assert.AreNotSame(d1, d10);
        }

        [TestMethod]
        public void CachedLambdaCompiler_OutliningEnabled()
        {
            var added = 0;

            var cache = new MyCache();
            cache.Added += () => added++;

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f1 = xs => xs.Where(x => x > 1).Select(x => x * 2);
            var d1 = f1.Compile(cache, outliningEnabled: true);
            var r1 = d1(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { 4, 6, 8 }.SequenceEqual(r1));
            Assert.AreEqual(3, added);

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f2 = xs => xs.Where(x => x > 0).Select(x => x * 3);
            var d2 = f2.Compile(cache, outliningEnabled: true);
            var r2 = d2(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { 3, 6, 9, 12 }.SequenceEqual(r2));
            Assert.AreEqual(3, added);

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f3 = xs => xs.OrderBy(x => x * -1).Where(x => x > 2).Select(x => x * -2);
            var d3 = f3.Compile(cache, outliningEnabled: true);
            var r3 = d3(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { -8, -6 }.SequenceEqual(r3));
            Assert.AreEqual(4, added);
        }

        [TestMethod]
        public void CachedLambdaCompiler_OutliningEnabled_Lambda()
        {
            var added = 0;

            var cache = new MyCache();
            cache.Added += () => added++;

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f1 = xs => xs.Where(x => x > 1).Select(x => x * 2).ToArray();
            var d1 = ((LambdaExpression)f1).Compile(cache, outliningEnabled: true);
            var r1 = (int[])d1.DynamicInvoke(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { 4, 6, 8 }.SequenceEqual(r1));
            Assert.AreEqual(3, added);

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f2 = xs => xs.Where(x => x > 0).Select(x => x * 3).ToArray();
            var d2 = ((LambdaExpression)f2).Compile(cache, outliningEnabled: true);
            var r2 = (int[])d2.DynamicInvoke(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { 3, 6, 9, 12 }.SequenceEqual(r2));
            Assert.AreEqual(3, added);

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f3 = xs => xs.OrderBy(x => x * -1).Where(x => x > 2).Select(x => x * -2).ToArray();
            var d3 = ((LambdaExpression)f3).Compile(cache, outliningEnabled: true);
            var r3 = (int[])d3.DynamicInvoke(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { -8, -6 }.SequenceEqual(r3));
            Assert.AreEqual(4, added);
        }

        [TestMethod]
        public void CachedLambdaCompiler_OutliningDisabled()
        {
            var added = 0;

            var cache = new MyCache();
            cache.Added += () => added++;

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f1 = xs => xs.Where(x => x > 1).Select(x => x * 2);
            var d1 = f1.Compile(cache, outliningEnabled: false);
            var r1 = d1(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { 4, 6, 8 }.SequenceEqual(r1));
            Assert.AreEqual(1, added);

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f2 = xs => xs.Where(x => x > 0).Select(x => x * 3);
            var d2 = f2.Compile(cache, outliningEnabled: false);
            var r2 = d2(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { 3, 6, 9, 12 }.SequenceEqual(r2));
            Assert.AreEqual(1, added);

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f3 = xs => xs.OrderBy(x => x * -1).Where(x => x > 2).Select(x => x * -2);
            var d3 = f3.Compile(cache, outliningEnabled: false);
            var r3 = d3(new[] { 1, 2, 3, 4 });

            Assert.IsTrue(new[] { -8, -6 }.SequenceEqual(r3));
            Assert.AreEqual(2, added);
        }

        [TestMethod]
        public void CachedLambdaCompiler_OutliningDontDoIt_Quote()
        {
            var added = 0;

            var cache = new MyCache();
            cache.Added += () => added++;

            Expression<Func<IQueryable<int>, IQueryable<int>>> f1 = xs => xs.Where(x => x > 1).Select(x => x * 2);
            var d1 = f1.Compile(cache, outliningEnabled: true);
            var r1 = d1(new[] { 1, 2, 3, 4 }.AsQueryable());

            Assert.IsTrue(new[] { 4, 6, 8 }.SequenceEqual(r1));
            Assert.AreEqual(1, added);
        }

        [TestMethod]
        public void CachedLambdaCompiler_OutliningDontDoIt_FreeVariables()
        {
            var added = 0;

            var cache = new MyCache();
            cache.Added += () => added++;

            Expression<Func<int, Func<int>>> f1 = x => () => x;
            var d1 = f1.Compile(cache, outliningEnabled: true);
            var r1 = d1(42)();

            Assert.AreEqual(42, r1);
            Assert.AreEqual(1, added);
        }

        [TestMethod]
        public void CachedLambdaCompiler_CustomHoister1()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var h = ConstantHoister.Create(false,
                (Expression<Func<string, string>>)(s => string.Format(s, default(object[])))
            );
#pragma warning restore IDE0034 // Simplify 'default' expression

            foreach (var outline in new[] { false, true })
            {
                var added = 0;

                var cache = new MyCache();
                cache.Added += () => added++;

                Expression<Func<int, string>> f1 = x => string.Format("{0}:{1}", new object[] { x, 43 });
                Expression<Func<int, string>> f2 = x => string.Format("{0}:{1}", new object[] { x, 44 });
                Expression<Func<int, string>> f3 = x => string.Format("{1}:{0}", new object[] { x, 45 });

                for (var i = 0; i < 2; i++)
                {
                    var d1 = f1.Compile(cache, outline, h);
                    var r1 = d1(42);

                    Assert.AreEqual("42:43", r1);
                    Assert.AreEqual(1, added);
                }

                for (var i = 0; i < 2; i++)
                {
                    var d2 = f2.Compile(cache, outline, h);
                    var r2 = d2(42);

                    Assert.AreEqual("42:44", r2);
                    Assert.AreEqual(1, added);
                }

                for (var i = 0; i < 2; i++)
                {
                    var d3 = f3.Compile(cache, outline, h);
                    var r3 = d3(42);

                    Assert.AreEqual("45:42", r3);
                    Assert.AreEqual(2, added);
                }
            }
        }

        [TestMethod]
        public void CachedLambdaCompiler_CustomHoister2()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var h = ConstantHoister.Create(false,
                (Expression<Func<string, string>>)(s => string.Format(s, default(object[])))
            );
#pragma warning restore IDE0034 // Simplify 'default' expression

            foreach (var outline in new[] { false, true })
            {
                var added = 0;

                var cache = new MyCache();
                cache.Added += () => added++;

                Expression<Func<int, string>> f1 = x => string.Format("{0}:{1}", new object[] { x, 43 });
                Expression<Func<int, string>> f2 = x => string.Format("{0}:{1}", new object[] { x, 44 });
                Expression<Func<int, string>> f3 = x => string.Format("{1}:{0}", new object[] { x, 45 });

                for (var i = 0; i < 2; i++)
                {
                    var d1 = ((LambdaExpression)f1).Compile(cache, outline, h);
                    var r1 = d1.DynamicInvoke(42);

                    Assert.AreEqual("42:43", r1);
                    Assert.AreEqual(1, added);
                }

                for (var i = 0; i < 2; i++)
                {
                    var d2 = ((LambdaExpression)f2).Compile(cache, outline, h);
                    var r2 = d2.DynamicInvoke(42);

                    Assert.AreEqual("42:44", r2);
                    Assert.AreEqual(1, added);
                }

                for (var i = 0; i < 2; i++)
                {
                    var d3 = ((LambdaExpression)f3).Compile(cache, outline, h);
                    var r3 = d3.DynamicInvoke(42);

                    Assert.AreEqual("45:42", r3);
                    Assert.AreEqual(2, added);
                }
            }
        }

        [TestMethod]
        public void LeastRecentlyUsedCompiledDelegateCache_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new LeastRecentlyUsedCompiledDelegateCache(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new LeastRecentlyUsedCompiledDelegateCache(-1));

            var lru = new LeastRecentlyUsedCompiledDelegateCache(4);
            Assert.ThrowsException<ArgumentNullException>(() => lru.GetOrAdd(expression: null));
        }

        [TestMethod]
        public void LeastRecentlyUsedCompiledDelegateCache_Basics()
        {
            var lru = new LeastRecentlyUsedCompiledDelegateCache(4);

            Assert.AreEqual(0, lru.Count);

            var f1 = (Func<int>)lru.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f1());

            Assert.AreEqual(1, lru.Count);

            var f2 = (Func<int>)lru.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f2());
            Assert.AreSame(f1, f2);

            Assert.AreEqual(1, lru.Count);

            var f3 = (Func<int>)lru.GetOrAdd(Expression.Lambda(Expression.Constant(43)));
            Assert.AreEqual(43, f3());

            Assert.AreEqual(2, lru.Count);

            lru.Clear();

            var f4 = (Func<int>)lru.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f4());
            Assert.AreNotSame(f1, f4);

            Assert.AreEqual(1, lru.Count);
        }

        [TestMethod]
        public void SimpleCompiledDelegateCache_ArgumentChecking()
        {
            var cache = new SimpleCompiledDelegateCache();
            Assert.ThrowsException<ArgumentNullException>(() => cache.GetOrAdd(expression: null));
        }

        [TestMethod]
        public void SimpleCompiledDelegateCache_Basics()
        {
            var cache = new SimpleCompiledDelegateCache();

            Assert.AreEqual(0, cache.Count);

            var f1 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f1());

            Assert.AreEqual(1, cache.Count);

            var f2 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f2());
            Assert.AreSame(f1, f2);

            Assert.AreEqual(1, cache.Count);

            var f3 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(43)));
            Assert.AreEqual(43, f3());

            Assert.AreEqual(2, cache.Count);

            cache.Clear();

            var f4 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f4());
            Assert.AreNotSame(f1, f4);

            Assert.AreEqual(1, cache.Count);
        }

        [TestMethod]
        public void VoidCompiledDelegateCache_ArgumentChecking()
        {
            var cache = VoidCompiledDelegateCache.Instance;
            Assert.ThrowsException<ArgumentNullException>(() => cache.GetOrAdd(expression: null));
        }

        [TestMethod]
        public void VoidCompiledDelegateCache_Basics()
        {
            var cache = VoidCompiledDelegateCache.Instance;

            Assert.AreEqual(0, cache.Count);

            var f1 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f1());

            Assert.AreEqual(0, cache.Count);

            var f2 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f2());

            Assert.AreEqual(0, cache.Count);

            var f3 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(43)));
            Assert.AreEqual(43, f3());

            Assert.AreEqual(0, cache.Count);

            cache.Clear();

            var f4 = (Func<int>)cache.GetOrAdd(Expression.Lambda(Expression.Constant(42)));
            Assert.AreEqual(42, f4());

            Assert.AreEqual(0, cache.Count);
        }

        private static Action<T> AssertHelper<T>(Func<T> get)
        {
            return x => Assert.AreEqual(x, get());
        }

        private sealed class MyCache : ICompiledDelegateCache
        {
            private readonly Dictionary<Expression, Delegate> _cache;

            public MyCache()
            {
                _cache = new Dictionary<Expression, Delegate>(new ExpressionEqualityComparer());
            }

            public int Count => _cache.Count;

            public void Clear()
            {
                _cache.Clear();
            }

            public event Action Added;

            public Delegate GetOrAdd(LambdaExpression expression)
            {
                if (!_cache.TryGetValue(expression, out var res))
                {
                    res = expression.Compile();

                    _cache.Add(expression, res);

                    Added();
                }

                return res;
            }
        }
    }
}
