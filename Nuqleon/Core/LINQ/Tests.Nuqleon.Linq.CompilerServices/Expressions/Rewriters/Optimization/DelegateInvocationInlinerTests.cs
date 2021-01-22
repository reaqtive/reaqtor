// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class DelegateInvocationInlinerTests
    {
        [TestMethod]
        public void DelegateInvocationInliner_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => DelegateInvocationInliner.Apply(expression: null, inlineNonPublicMethods: true), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void DelegateInvocationInliner_NoDelegate()
        {
            var f = (Expression<Func<int, int>>)(x => x * 2);
            var i = Expression.Invoke(f, Expression.Constant(1));

            var a = DelegateInvocationInliner.Apply(i, inlineNonPublicMethods: true);

            Assert.AreSame(i, a);
        }

        [TestMethod]
        public void DelegateInvocationInliner_Static()
        {
            var f = new Func<int, int>(x => x * 2);
            var d = Expression.Constant(f);
            var i = Expression.Invoke(d, Expression.Constant(1));

            var a = DelegateInvocationInliner.Apply(i, inlineNonPublicMethods: true);
            var b = DelegateInvocationInliner.Apply(i, inlineNonPublicMethods: false);

            Assert.AreNotSame(i, a);
            Assert.AreSame(i, b);

            var m = a as MethodCallExpression;
            Assert.IsNotNull(m);
            Assert.AreEqual(f.Method, m.Method);

            Assert.AreEqual(f(1), a.Evaluate<int>());
        }

        [TestMethod]
        public void DelegateInvocationInliner_Instance()
        {
            var c = new Bar();
            var f = new Func<int, int>(c.Twice);
            var d = Expression.Constant(f);
            var i = Expression.Invoke(d, Expression.Constant(1));

            var a = DelegateInvocationInliner.Apply(i, inlineNonPublicMethods: true);

            Assert.AreNotSame(i, a);

            var m = a as MethodCallExpression;
            Assert.IsNotNull(m);
            Assert.AreEqual(f.Method, m.Method);

            Assert.AreEqual(f(1), a.Evaluate<int>());
        }

        [TestMethod]
        public void DelegateInvocationInliner_Multicast()
        {
            var f = new Func<int, int>(x => x * x);
            var g = new Func<int, int>(x => x + x);
            var d = Expression.Constant(f + g);
            var i = Expression.Invoke(d, Expression.Constant(1));

            var a = DelegateInvocationInliner.Apply(i, inlineNonPublicMethods: true);

            Assert.AreSame(i, a);
        }

#pragma warning disable CA1822 // Mark static
        private sealed class Bar
        {
            public int Twice(int x) => x * 2;
        }
#pragma warning restore CA1822
    }
}
