// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/20/2014 - Added tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Documentation;
using System.Linq;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void XmlDocs_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => XmlDocumentation.GetXmlDoc(default(MemberInfo)));
            Assert.ThrowsException<ArgumentNullException>(() => XmlDocumentation.GetXmlDoc(default(ParameterInfo)));
        }

        [TestMethod]
        public void XmlDocs_EnumerateTypes()
        {
            foreach (var t in GetTypes())
            {
                var d = XmlDocumentation.GetXmlDoc(t);
                Assert.IsNotNull(d);
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateMethods()
        {
            var forbiddenPrefixes = new[] { "get_", "set_", "add_", "remove_" };

            var res = from t in GetTypes()
                      from m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                      where !forbiddenPrefixes.Any(p => m.Name.StartsWith(p))
                      where !(typeof(Delegate).IsAssignableFrom(m.DeclaringType) && m.Name.Contains("Invoke"))
                      select m;

            var exclude = new Dictionary<Type, List<string>>
            {
                { typeof(DateTimeOffset), new List<string> { "FromUnixTimeMilliseconds", "FromUnixTimeSeconds", "ToUnixTimeMilliseconds", "ToUnixTimeSeconds" } },
                { typeof(Buffer), new List<string> { "MemoryCopy" } },
                { typeof(Array), new List<string> { "Empty" } },
                { typeof(string), new List<string> { "Format" } },
                { typeof(GC), new List<string> { "Collect", "EndNoGCRegion", "TryStartNoGCRegion" } },
            };

            foreach (var m in res)
            {
                if (exclude.TryGetValue(m.DeclaringType, out var ex) && ex.Contains(m.Name))
                {
                    continue;
                }

                var d = XmlDocumentation.GetXmlDoc(m);

                Assert.IsNotNull(d, m.ToString() + " on " + m.DeclaringType.ToString());

                foreach (var p in m.GetParameters())
                {
                    var e = XmlDocumentation.GetXmlDoc(p);
                    Assert.IsNotNull(e);
                }
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateProperties()
        {
            var res = from t in GetTypes()
                      from p in t.GetProperties()
                      where !(t == typeof(string) && p.Name == "Chars")
                      select p;

            foreach (var m in res)
            {
                var d = XmlDocumentation.GetXmlDoc(m);
                Assert.IsNotNull(d, m.ToString() + " on " + m.DeclaringType.ToString());


                foreach (var p in m.GetIndexParameters())
                {
                    var e = XmlDocumentation.GetXmlDoc(p);
                    Assert.IsNotNull(e);
                }
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateFields()
        {
            var res = from t in GetTypes()
                      from f in t.GetFields()
                      where f.Name != "value__" // enums
                      select f;

            foreach (var m in res)
            {
                var d = XmlDocumentation.GetXmlDoc(m);
                Assert.IsNotNull(d, m.ToString() + " on " + m.DeclaringType.ToString());
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateEvents()
        {
            var res = from t in GetTypes()
                      from e in t.GetEvents()
                      select e;

            foreach (var m in res)
            {
                var d = XmlDocumentation.GetXmlDoc(m);
                Assert.IsNotNull(d, m.ToString() + " on " + m.DeclaringType.ToString());
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateConstructors()
        {
            var res = from t in GetTypes()
                      where !typeof(Delegate).IsAssignableFrom(t)
                      from c in t.GetConstructors()
                      select c;

            foreach (var m in res)
            {
                var d = XmlDocumentation.GetXmlDoc(m);
                Assert.IsNotNull(d, m.ToString() + " on " + m.DeclaringType.ToString());

                foreach (var p in m.GetParameters())
                {
                    var e = XmlDocumentation.GetXmlDoc(p);
                    Assert.IsNotNull(e);
                }
            }
        }

        private static IEnumerable<Type> GetTypes()
        {
            var blackList = new[] { "System.__ComObject", "System.AppContext", "System.FormattableString" };

            var res =
                from t in typeof(int).Assembly.GetTypes()
                where t.Namespace is "System" or "System.Collections" or "System.Collections.Generic"
                where t.IsPublic
                where !blackList.Contains(t.FullName)
                select t;

            res = res.Concat(
                from t in typeof(Enumerable).Assembly.GetTypes()
                where t.Namespace == "System.Linq"
                where t.IsPublic
                where !blackList.Contains(t.FullName)
                select t);

            return res;
        }

        [TestMethod]
        public void XmlDocs_GetXmlDocName_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => XmlDocumentation.GetXmlDocName(null));
        }

        [TestMethod]
        public void XmlDocs_GetXmlDocName_Types()
        {
            var ts = new Dictionary<Type, string>
            {
                { typeof(int), "T:System.Int32" },
                { typeof(Func<>), "T:System.Func`1" },
                { typeof(int[]), "T:System.Int32[]" },
                { typeof(int?), "T:System.Nullable{System.Int32}" },
                { typeof(int[,]), "T:System.Int32[0:,0:]" },
            };

            foreach (var t in ts)
            {
                Assert.AreEqual(t.Value, t.Key.GetXmlDocName(), t.Key.FullName);
            }
        }
    }
}
