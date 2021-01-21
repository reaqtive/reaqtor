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
using System.Linq;
using System.Linq.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests
{
    [TestClass]
    public class TypeSlimTests : TestBase
    {
        [TestMethod]
        public void TypeSlim_RecursiveRecordType_ToString()
        {
            var rtc = new RuntimeCompiler();
            var builder = rtc.GetNewRecordTypeBuilder();

            rtc.DefineRecordType(
                builder,
                new Dictionary<string, Type>
                {
                    { "self", builder }
                }.AsEnumerable(),
                true
            );

            var type = builder.CreateType();

            var slimifier = new TypeToTypeSlimConverter();
            var slim = slimifier.Visit(type);
            Assert.AreEqual("{self : {1}} as {1}", slim.ToString());
        }

        [TestMethod]
        public void TypeSlim_EqualsOperator()
        {
            var s1 = typeof(IEnumerable<int>).ToTypeSlim();
            var s2 = typeof(IEnumerable<int>).ToTypeSlim();
            var o = "foo";

            Assert.AreNotSame(s1, s2);
            Assert.AreEqual(s1, s2);
            Assert.IsTrue(s1 == s2);
            Assert.IsFalse(s1 != s2);

            Assert.IsFalse(s1.Equals(o));
            Assert.AreNotEqual(s1, o);
            Assert.AreNotEqual(o, s2);

            Assert.IsFalse(s1 == null);
            Assert.IsFalse(null == s1);
            Assert.IsTrue(default(TypeSlim) == null);
        }

        [TestMethod]
        public void TypeSlim_NotEqualsOperator()
        {
            var s1 = typeof(IEnumerable<int>).ToTypeSlim();
            var s2 = typeof(IEnumerable<long>).ToTypeSlim();

            Assert.AreNotSame(s1, s2);
            Assert.AreNotEqual(s1, s2);
            Assert.IsTrue(s1 != s2);
            Assert.IsFalse(s1 == s2);
        }

        [TestMethod]
        public void TypeSlim_ToCSharpString()
        {
            var types = new Dictionary<Type, string>
            {
                { typeof(int), "int" },
                { typeof(int[]), "int[]" },
                { typeof(int[][]), "int[][]" },
                { typeof(int[,]), "int[,]" },
                { typeof(int?), "int?" },
                { typeof(List<int>), "System.Collections.Generic.List<int>" },
                { typeof(List<int?[]>[]), "System.Collections.Generic.List<int?[]>[]" },
                { typeof(List<>), "System.Collections.Generic.List<>" },
                { typeof(Dictionary<,>), "System.Collections.Generic.Dictionary<,>" },
            };

            foreach (var kv in types)
            {
                Assert.AreEqual(kv.Value, kv.Key.ToTypeSlim().ToCSharpString(), kv.Value);
            }
        }

        [TestMethod]
        public void TypeSlim_RecursiveRecordType_ToCSharpString()
        {
            var rtc = new RuntimeCompiler();
            var builder = rtc.GetNewRecordTypeBuilder();

            rtc.DefineRecordType(
                builder,
                new Dictionary<string, Type>
                {
                    { "self", builder }
                }.AsEnumerable(),
                true
            );

            var type = builder.CreateType();

            var slimifier = new TypeToTypeSlimConverter();
            var slim = slimifier.Visit(type);
            Assert.AreEqual("class T1 { T1 self; }", slim.ToCSharpString());
        }

        [TestMethod]
        public void TypeSlim_AnonymousType_ToCSharpString()
        {
            var type = new { a = 42 }.GetType();

            var slimifier = new TypeToTypeSlimConverter();
            var slim = slimifier.Visit(type);
            Assert.AreEqual("{ int a; }", slim.ToCSharpString());
        }
    }
}
