// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Memory;

namespace Tests
{
    [TestClass]
    public partial class FunctionMemoizationExtensionsTests
    {
        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoArgs_ArgumentChecking()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize(default(Func<int>)));

            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize(default(IMemoizer), () => 42));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize(m, default(Func<int>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoArgs_Simple()
        {
            foreach (var options in new[] { MemoizationOptions.None, MemoizationOptions.CacheException })
            {
                var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);

                var n = 0;
                var res = m.Memoize(() => { n++; return 42; }, options);

                Assert.AreEqual(0, n);
                Assert.AreEqual(0, res.Cache.Count);

                Assert.AreEqual(42, res.Delegate());
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, res.Cache.Count);

                Assert.AreEqual(42, res.Delegate());
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, res.Cache.Count);

                res.Cache.Clear();

                Assert.AreEqual(1, n);
                Assert.AreEqual(0, res.Cache.Count);

                Assert.AreEqual(42, res.Delegate());
                Assert.AreEqual(2, n);
                Assert.AreEqual(1, res.Cache.Count);
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoArgs_Error()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var n = 0;
            var res = m.Memoize<int>(() => { n++; throw new InvalidOperationException(); });

            Assert.AreEqual(0, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(1, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(2, n);
            Assert.AreEqual(0, res.Cache.Count);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoArgs_CacheError()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var n = 0;
            var res = m.Memoize<int>(() => { n++; throw new InvalidOperationException(); }, MemoizationOptions.CacheException);

            Assert.AreEqual(0, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, res.Cache.Count);

            res.Cache.Clear();

            Assert.AreEqual(1, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, res.Cache.Count);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoMemoizer_NoArgs_Simple()
        {
            foreach (var options in new[] { MemoizationOptions.None, MemoizationOptions.CacheException })
            {
                var n = 0;
                var res = FunctionMemoizationExtensions.Memoize(() => { n++; return 42; }, options);

                Assert.AreEqual(0, n);
                Assert.AreEqual(0, res.Cache.Count);

                Assert.AreEqual(42, res.Delegate());
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, res.Cache.Count);

                Assert.AreEqual(42, res.Delegate());
                Assert.AreEqual(1, n);
                Assert.AreEqual(1, res.Cache.Count);

                res.Cache.Clear();

                Assert.AreEqual(1, n);
                Assert.AreEqual(0, res.Cache.Count);

                Assert.AreEqual(42, res.Delegate());
                Assert.AreEqual(2, n);
                Assert.AreEqual(1, res.Cache.Count);

                Assert.IsTrue(!string.IsNullOrEmpty(res.Cache.DebugView));
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoMemoizer_NoArgs_Error()
        {
            var n = 0;
            var res = FunctionMemoizationExtensions.Memoize<int>(() => { n++; throw new InvalidOperationException(); });

            Assert.AreEqual(0, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(1, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(2, n);
            Assert.AreEqual(0, res.Cache.Count);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_NoMemoizer_NoArgs_CacheError()
        {
            var n = 0;
            var res = FunctionMemoizationExtensions.Memoize<int>(() => { n++; throw new InvalidOperationException(); }, MemoizationOptions.CacheException);

            Assert.AreEqual(0, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, res.Cache.Count);

            res.Cache.Clear();

            Assert.AreEqual(1, n);
            Assert.AreEqual(0, res.Cache.Count);

            Assert.ThrowsException<InvalidOperationException>(() => res.Delegate());
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, res.Cache.Count);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_ArgumentChecking()
        {
            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var a = new A(() => { });

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<A>(default(IMemoizer), a, MemoizationOptions.None));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<A>(mem, default(A), MemoizationOptions.None));

            Assert.ThrowsException<ArgumentException>(() => FunctionMemoizationExtensions.Memoize<int>(mem, 42, MemoizationOptions.None));
            Assert.ThrowsException<NotSupportedException>(() => FunctionMemoizationExtensions.Memoize<R>(mem, (ref int x) => { }, MemoizationOptions.None));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_VoidToVoid()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var a = new A(() => { n++; });

            var res = mem.Memoize(a, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            res.Delegate();
            Assert.AreEqual(1, n);

            res.Delegate();
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_VoidToBool()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var b = new B(() => { n++; return true; });

            var res = mem.Memoize(b, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_IntToVoid()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = new C(_ => { n++; });

            var res = mem.Memoize(c, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            res.Delegate(42);
            Assert.AreEqual(1, n);

            res.Delegate(42);
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_IntToBool()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var d = new D(x => { n++; return x > 0; });

            var res = mem.Memoize(d, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate(42));
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate(42));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_ArgumentChecking()
        {
            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var a = new A(() => { });

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<A>(default(IWeakMemoizer), a, MemoizationOptions.None));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<A>(mem, default(A), MemoizationOptions.None));

            Assert.ThrowsException<ArgumentException>(() => FunctionMemoizationExtensions.MemoizeWeak<int>(mem, 42, MemoizationOptions.None));
            Assert.ThrowsException<NotSupportedException>(() => FunctionMemoizationExtensions.MemoizeWeak<Q>(mem, (ref string x) => { }, MemoizationOptions.None));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_VoidToVoid()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var a = new A(() => { n++; });

            var res = mem.MemoizeWeak(a, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            res.Delegate();
            Assert.AreEqual(1, n);

            res.Delegate();
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_VoidToBool()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var b = new B(() => { n++; return true; });

            var res = mem.MemoizeWeak(b, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_StringToInt()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var e = new E(s => { n++; return s.Length; });

            var res = mem.MemoizeWeak(e, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(3, res.Delegate("foo"));
            Assert.AreEqual(1, n);

            Assert.AreEqual(3, res.Delegate("foo"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_StringStringToInt()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new F((s, t) => { n++; return (s + t).Length; });

            var res = mem.MemoizeWeak(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(6, res.Delegate("foo", "bar"));
            Assert.AreEqual(1, n);

            Assert.AreEqual(6, res.Delegate("foo", "bar"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_StringStringToVoid()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var g = new G((s, t) => { n++; });

            var res = mem.MemoizeWeak(g, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            res.Delegate("foo", "bar");
            Assert.AreEqual(1, n);

            res.Delegate("foo", "bar");
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_IntToInt()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int>(x => { n++; return x * 2; });

            var mes = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var res = mem.MemoizeWeak(f, MemoizationOptions.None, mes);

            Assert.AreEqual(0, n);

            Assert.AreEqual(2, res.Delegate(1));
            Assert.AreEqual(1, n);

            Assert.AreEqual(2, res.Delegate(1));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_StringIntToBool()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, bool>((s, x) => { n++; return s.Length == x; });

            var mes = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var res = mem.MemoizeWeak(f, MemoizationOptions.None, mes);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate("foo", 3));
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate("foo", 3));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_IntStringToBool()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, bool>((x, s) => { n++; return s.Length == x; });

            var mes = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var res = mem.MemoizeWeak(f, MemoizationOptions.None, mes);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate(3, "foo"));
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate(3, "foo"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_HasValueTypeButNoMemoizer()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, bool>((x, s) => { n++; return s.Length == x; });

            Assert.ThrowsException<ArgumentNullException>(() => mem.MemoizeWeak(f, MemoizationOptions.None, memoizer: null));
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Potpourri1()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, bool, long, string, bool>((p0, p1, p2, p3, p4) => { n++; return true; });

            var mes = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var res = mem.MemoizeWeak(f, MemoizationOptions.None, mes);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate(3, "foo", false, 42L, "qux"));
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate(3, "foo", false, 42L, "qux"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Potpourri2()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, bool, long, string, bool>((p0, p1, p2, p3, p4) => { n++; return true; });

            var mes = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var res = mem.MemoizeWeak(f, MemoizationOptions.None, mes);

            Assert.AreEqual(0, n);

            Assert.AreEqual(true, res.Delegate(3, null, false, 42L, "qux"));
            Assert.AreEqual(1, n);

            Assert.AreEqual(true, res.Delegate(3, null, false, 42L, "qux"));
            Assert.AreEqual(1, n);
        }
    }

    internal delegate void A();

    internal delegate bool B();

    internal delegate void C(int x);

    internal delegate bool D(int x);

    internal delegate int E(string x);

    internal delegate int F(string x, string y);

    internal delegate void G(string x, string y);

    internal delegate void R(ref int x);

    internal delegate void Q(ref string x);
}
