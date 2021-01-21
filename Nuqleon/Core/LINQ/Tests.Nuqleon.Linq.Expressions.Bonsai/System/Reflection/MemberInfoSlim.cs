// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class MemberInfoSlimTests : TestBase
    {
        [TestMethod]
        public void MemberInfoSlim_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ((TypeSlim)null).GetField(name: null, fieldType: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void MemberInfoSlim_ToString_Constructor()
        {
            Expression<Func<DateTime>> f = () => new DateTime(0L);

            var e = (NewExpressionSlim)f.Body.ToExpressionSlim();
            var m = e.Constructor;

            var s = m.ToString();

            Assert.AreEqual("System.DateTime..ctor(System.Int64)", s);
        }

        [TestMethod]
        public void MemberInfoSlim_ToString_Field()
        {
            Expression<Func<DateTime>> f = () => DateTime.MaxValue;

            var e = (MemberExpressionSlim)f.Body.ToExpressionSlim();
            var m = (FieldInfoSlim)e.Member;

            var s = m.ToString();

            Assert.AreEqual("System.DateTime.MaxValue", s);
        }

        [TestMethod]
        public void MemberInfoSlim_ToString_Property()
        {
            Expression<Func<DateTime, int>> f = dt => dt.Day;

            var e = (MemberExpressionSlim)f.Body.ToExpressionSlim();
            var m = (PropertyInfoSlim)e.Member;

            var s = m.ToString();

            Assert.AreEqual("System.DateTime.Day", s);
        }

        [TestMethod]
        public void MemberInfoSlim_ToString_SimpleMethod()
        {
            Expression<Func<string, string>> f = x => x.Substring(1, 2);

            var e = (MethodCallExpressionSlim)f.Body.ToExpressionSlim();
            var m = e.Method;

            var s = m.ToString();

            Assert.AreEqual("System.String System.String.Substring(System.Int32, System.Int32)", s);
        }

        [TestMethod]
        public void MemberInfoSlim_ToString_GenericMethodDefinition()
        {
            Expression<Func<DateTime>> f = () => Activator.CreateInstance<DateTime>();

            var e = (MethodCallExpressionSlim)f.Body.ToExpressionSlim();
            var m = ((GenericMethodInfoSlim)e.Method).GenericMethodDefinition;

            var s = m.ToString();

            Assert.AreEqual("T System.Activator.CreateInstance<T>()", s);
        }

        [TestMethod]
        public void MemberInfoSlim_ToString_GenericMethod()
        {
            Expression<Func<IEnumerable<int>, IEnumerable<int>>> f = xs => xs.DefaultIfEmpty(42);

            var e = (MethodCallExpressionSlim)f.Body.ToExpressionSlim();
            var m = e.Method;

            var s = m.ToString();

            Assert.AreEqual("System.Collections.Generic.IEnumerable`1<System.Int32> System.Linq.Enumerable.DefaultIfEmpty<System.Int32>(System.Collections.Generic.IEnumerable`1<System.Int32>, System.Int32)", s);
        }
    }
}
