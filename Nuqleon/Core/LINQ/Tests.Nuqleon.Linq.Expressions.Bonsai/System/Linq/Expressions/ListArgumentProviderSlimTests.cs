// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions
{
    [TestClass]
    public class ListArgumentProviderSlimTests
    {
        [TestMethod]
        public void ListArgumentProvider_All()
        {
            var s = typeof(string).ToTypeSlim();
            var i = typeof(int).ToTypeSlim();
            var m = s.GetSimpleMethod("Substring", new ReadOnlyCollection<TypeSlim>(new[] { i, i }), s);

            var a0 = ExpressionSlim.Default(i);
            var a1 = ExpressionSlim.Default(i);
            var d = ExpressionSlim.Default(i);

            var c = ExpressionSlim.Call(m, a0, a1);

            var argProvider = new ListArgumentProviderSlim(c, a0);

            Assert.ThrowsException<NotSupportedException>(() => argProvider[0] = a0);
            Assert.ThrowsException<NotSupportedException>(() => argProvider.Add(a0));
            Assert.ThrowsException<NotSupportedException>(() => argProvider.Clear());
            Assert.ThrowsException<NotSupportedException>(() => argProvider.Insert(0, a0));
            Assert.ThrowsException<NotSupportedException>(() => argProvider.Remove(a0));
            Assert.ThrowsException<NotSupportedException>(() => argProvider.RemoveAt(0));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => argProvider[-1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => argProvider[2]);
            Assert.AreSame(a0, argProvider[0]);
            Assert.AreSame(a1, argProvider[1]);

            Assert.AreEqual(2, argProvider.Count);

            Assert.IsTrue(argProvider.IsReadOnly);

            Assert.IsTrue(argProvider.Contains(a0));
            Assert.IsTrue(argProvider.Contains(a1));
            Assert.IsFalse(argProvider.Contains(d));

            Assert.AreEqual(0, argProvider.IndexOf(a0));
            Assert.AreEqual(1, argProvider.IndexOf(a1));
            Assert.IsTrue(argProvider.IndexOf(d) < 0);

            var arr = new ExpressionSlim[5];
            argProvider.CopyTo(arr, 2);
            Assert.IsNull(arr[0]);
            Assert.IsNull(arr[1]);
            Assert.AreSame(a0, arr[2]);
            Assert.AreSame(a1, arr[3]);
            Assert.IsNull(arr[4]);

            var e1 = argProvider.GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreSame(a0, e1.Current);
            Assert.IsTrue(e1.MoveNext());
            Assert.AreSame(a1, e1.Current);
            Assert.IsFalse(e1.MoveNext());

            var e2 = ((IEnumerable)argProvider).GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreSame(a0, e2.Current);
            Assert.IsTrue(e2.MoveNext());
            Assert.AreSame(a1, e2.Current);
            Assert.IsFalse(e2.MoveNext());
        }
    }
}
