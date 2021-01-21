// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Wrote these tests.
//

using System;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class WeakReferenceExtensionsTests
    {
        [TestMethod]
        public void WeakReferenceExtensions_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => WeakReferenceExtensions.GetTarget(default(WeakReference<string>)));
            Assert.ThrowsException<ArgumentNullException>(() => WeakReferenceExtensions.GetOrSetTarget(default(WeakReference<string>), () => ""));
            Assert.ThrowsException<ArgumentNullException>(() => WeakReferenceExtensions.GetOrSetTarget(new WeakReference<string>(""), default(Func<string>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void WeakReferenceExtensions_Create_Null()
        {
            var w = WeakReferenceExtensions.Create<string>(target: null);

            Assert.IsFalse(w.TryGetTarget(out _));

            Assert.IsNull(w.GetTarget());

            Assert.IsNull(w.GetOrSetTarget(() => { Assert.Fail(); return ""; }));
        }

        [TestMethod]
        public void WeakReferenceExtensions_Create_Alive()
        {
            var s = "bar".ToUpper();

            var w = WeakReferenceExtensions.Create<string>(s);

            Assert.IsTrue(w.TryGetTarget(out string target));
            Assert.AreSame(s, target);

            Assert.AreSame(s, w.GetTarget());

            Assert.AreSame(s, w.GetOrSetTarget(() => { Assert.Fail(); return ""; }));

            GC.KeepAlive(s);
        }

        [TestMethod]
        public void WeakReferenceExtensions_Create_Dead()
        {
            // NB: This has shown to be flaky on Mono.
            if (Type.GetType("Mono.Runtime") == null)
            {
                var w = CreateWeakString();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Assert.IsFalse(w.TryGetTarget(out string ignored));

                Assert.ThrowsException<InvalidOperationException>(() => w.GetTarget());

                var s = w.GetOrSetTarget(() => { return "qux".ToUpper(); });
                Assert.AreEqual("QUX", s);

                Assert.IsTrue(w.TryGetTarget(out string target));
                Assert.AreSame(s, target);

                Assert.AreSame(s, w.GetTarget());

                Assert.AreSame(s, w.GetOrSetTarget(() => { Assert.Fail(); return ""; }));

                GC.KeepAlive(s);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static WeakReference<string> CreateWeakString()
        {
            var s = GetString();

            var w = WeakReferenceExtensions.Create<string>(s);

            Assert.AreSame(s, w.GetTarget());

            GC.KeepAlive(s);

            return w;
        }

        private static string GetString()
        {
            return "bar".ToUpper();
        }
    }
}
