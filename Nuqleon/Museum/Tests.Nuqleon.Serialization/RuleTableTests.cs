// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014
//

using Nuqleon.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Linq.Expressions;

namespace Tests.Microsoft.Serialization
{
    [TestClass]
    public class RuleTableTests
    {
        [TestMethod]
        public void RuleTable_ArgumentChecking()
        {
            var tbl = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);

#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add(default(Rule<int, string, object>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), x => x));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(42, default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), 42, x => x));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(Func<int, bool>), x => x));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(x => true, default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(42, default(Func<int, bool>), x => x));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(42, x => true, default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), 42, x => true, x => x));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, default(Func<int, bool>), x => x));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, x => true, default(Expression<Func<int, int>>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), (x, rec, o) => "", (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", default(Func<int, Func<int, object, string>, object, string>), (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", (x, rec, o) => "", default(Func<string, Func<string, object, int>, object, int>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), 42, (x, rec, o) => "", (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, default(Func<int, Func<int, object, string>, object, string>), (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, (x, rec, o) => "", default(Func<string, Func<string, object, int>, object, int>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), x => true, (x, rec, o) => "", (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", default(Func<int, bool>), (x, rec, o) => "", (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", x => true, default(Func<int, Func<int, object, string>, object, string>), (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", x => true, (x, rec, o) => "", default(Func<string, Func<string, object, int>, object, int>)));

            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>(default(string), 42, x => true, (x, rec, o) => "", (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, default(Func<int, bool>), (x, rec, o) => "", (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, x => true, default(Func<int, Func<int, object, string>, object, string>), (s, rec, o) => 42));
            Assert.ThrowsException<ArgumentNullException>(() => tbl.Add<int>("foo", 42, x => true, (x, rec, o) => "", default(Func<string, Func<string, object, int>, object, int>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void RuleTable_Assembly()
        {
            Type type = typeof(RuleTable<int, string, object>);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Serialization", assembly);
        }

        [TestMethod]
        public void RuleTable_DuplicateEntry()
        {
            var tbl = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);
            tbl.Add<int>("foo", x => x);

            Assert.ThrowsException<InvalidOperationException>(() => tbl.Add<int>("foo", x => x));
        }

        [TestMethod]
        public void RuleTable_Enumeration()
        {
            var tbl = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);
            tbl.Add<int>("foo", x => x);

            var e1 = tbl.GetEnumerator();
            var e2 = ((IEnumerable)tbl).GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.IsTrue(e2.MoveNext());

            var c1 = e1.Current;
            var c2 = (Rule<int, string, object>)e2.Current;

            Assert.AreEqual("foo", c1.Name);
            Assert.AreEqual("foo", c2.Name);

            Assert.IsFalse(e1.MoveNext());
            Assert.IsFalse(e2.MoveNext());
        }

        [TestMethod]
        public void RuleTable_Extend()
        {
            var tbl = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);
            tbl.Add<int>("foo", x => x);

            var ext = tbl.Extend();
            ext.Add<int>("bar", x => x);

            var e1 = ext.GetEnumerator();
            var e2 = ((IEnumerable)ext).GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.IsTrue(e2.MoveNext());

            var c1 = e1.Current;
            var c2 = (Rule<int, string, object>)e2.Current;

            Assert.AreEqual("foo", c1.Name);
            Assert.AreEqual("foo", c2.Name);

            Assert.IsTrue(e1.MoveNext());
            Assert.IsTrue(e2.MoveNext());

            var c3 = e1.Current;
            var c4 = (Rule<int, string, object>)e2.Current;

            Assert.AreEqual("bar", c3.Name);
            Assert.AreEqual("bar", c4.Name);

            Assert.IsFalse(e1.MoveNext());
            Assert.IsFalse(e2.MoveNext());
        }

        [TestMethod]
        public void RuleTable_Concat()
        {
            var tbl1 = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);
            tbl1.Add<int>("foo", x => x);

            var tbl2 = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);
            tbl2.Add<int>("bar", x => x);

            var res = tbl1.Concat(tbl2);

            var e1 = res.GetEnumerator();
            var e2 = ((IEnumerable)res).GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.IsTrue(e2.MoveNext());

            var c1 = e1.Current;
            var c2 = (Rule<int, string, object>)e2.Current;

            Assert.AreEqual("foo", c1.Name);
            Assert.AreEqual("foo", c2.Name);

            Assert.IsTrue(e1.MoveNext());
            Assert.IsTrue(e2.MoveNext());

            var c3 = e1.Current;
            var c4 = (Rule<int, string, object>)e2.Current;

            Assert.AreEqual("bar", c3.Name);
            Assert.AreEqual("bar", c4.Name);

            Assert.IsFalse(e1.MoveNext());
            Assert.IsFalse(e2.MoveNext());

            Assert.ThrowsException<NotSupportedException>(() => res.Add<int>(x => x));
        }

        [TestMethod]
        public void RuleTable_AsReadOnly()
        {
            var tbl = new RuleTable<int, string, object>(d => Expression.Constant(1), (d, s) => d);
            tbl.Add<int>("foo", x => x);

            var rdo = tbl.AsReadOnly();

            Assert.ThrowsException<NotSupportedException>(() => rdo.Add<int>("qux", x => x));

            var e1 = rdo.GetEnumerator();
            var e2 = ((IEnumerable)rdo).GetEnumerator();

            Assert.IsTrue(e1.MoveNext());
            Assert.IsTrue(e2.MoveNext());

            var c1 = e1.Current;
            var c2 = (Rule<int, string, object>)e2.Current;

            Assert.AreEqual("foo", c1.Name);
            Assert.AreEqual("foo", c2.Name);

            Assert.IsFalse(e1.MoveNext());
            Assert.IsFalse(e2.MoveNext());
        }
    }
}
