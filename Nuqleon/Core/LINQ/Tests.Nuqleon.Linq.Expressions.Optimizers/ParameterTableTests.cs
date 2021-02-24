// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public partial class ParameterTableTests
    {
        [TestMethod]
        public void ParameterTable_Add_ArgumentChecking()
        {
            var pt = new ParameterTable();

            Assert.ThrowsException<ArgumentNullException>(() => pt.Add(default(LambdaExpression)));
            Assert.ThrowsException<ArgumentNullException>(() => pt.Add(default(ParameterInfo)));
            Assert.ThrowsException<ArgumentNullException>(() => pt.Add(default(ParameterTable)));
        }

        [TestMethod]
        public void ParameterTable_Contains_ArgumentChecking()
        {
            var pt = new ParameterTable();

            Assert.ThrowsException<ArgumentNullException>(() => pt.Contains(default));
        }

        [TestMethod]
        public void ParameterTable_Add_Contains()
        {
            var pt = new ParameterTable();

            var m = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) });

            pt.Add(m.GetParameters()[0]);

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
            Assert.IsFalse(pt.Contains(m.GetParameters()[1]));
        }

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_Call()
        {
            var pt = new ParameterTable();

            pt.Add<int, string>(x => "".Substring(x, 0));

            var m = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) });

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_New()
        {
            var pt = new ParameterTable();

            pt.Add<int, string>(x => new string('*', x));

            var c = typeof(string).GetConstructor(new[] { typeof(char), typeof(int) });

            Assert.IsTrue(pt.Contains(c.GetParameters()[1]));
        }

        // REVIEW: For indexers, it may be useful to add ParameterInfo objects for both the indexer and the accessor(s).

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_Index1()
        {
            var pt = new ParameterTable();

            pt.Add<int, char>(x => ""[x]);

            var p = typeof(string).GetProperties().Single(p => p.GetIndexParameters().Length == 1);

            // NB: This reflects a quirk in C# bindings for expression tree APIs which predate the addition of IndexExpression.
            Assert.IsTrue(pt.Contains(p.GetGetMethod().GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_Index2()
        {
            if (Type.GetType("Mono.Runtime") != null)
            {
                // NB: Quirk on Mono with indexer parameters.
                return;
            }

            var pt = new ParameterTable();

            var c = Expression.Parameter(typeof(int));
            var p = typeof(string).GetProperties().Single(p => p.GetIndexParameters().Length == 1);

            pt.Add(Expression.Lambda<Func<int, char>>(Expression.MakeIndex(Expression.Constant(""), p, new[] { c }), c));

            Assert.IsTrue(pt.Contains(p.GetIndexParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_Invalid_UseMoreThanOnce()
        {
            var pt = new ParameterTable();

            Assert.ThrowsException<InvalidOperationException>(() => pt.Add<int, string>(x => "".Substring(x, x)));
        }

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_Invalid_NotUsed()
        {
            var pt = new ParameterTable();

            Assert.ThrowsException<ArgumentException>(() => pt.Add<int, string>(x => "".Substring(0, 1)));
        }

        [TestMethod]
        public void ParameterTable_Add_LambdaExpression_Invalid_NoParameters()
        {
            var pt = new ParameterTable();

            Assert.ThrowsException<ArgumentException>(() => pt.Add(Expression.Lambda<Action>(Expression.Empty())));
        }

        [TestMethod]
        public void ParameterTable_ReadOnly()
        {
            var pt = new ParameterTable();
            var rpt = pt.ToReadOnly();

            var m = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) });

            Assert.ThrowsException<InvalidOperationException>(() => rpt.Add(m.GetParameters()[0]));
        }
    }
}
