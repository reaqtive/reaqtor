// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/20/2014 - Added tests.
//   IG - 2025/12    - Updated after changes made in migrating from .NET FX to .NET.
//

using System;
using System.Collections.Generic;
using System.Documentation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public sealed class Tests : IDisposable
    {
        private readonly MetadataLoadContext mlc;
        private Type metadataDelegateType;

        // NB: This assembly is only supported on .NET (and not .NET FX) because of how it locates reference assemblies.

        public Tests()
        {
            // Only types visible in reference assemblies have XML docs, so this test should only be asking
            // about those. That means GetType can't just use typeof(int).Assembly, because that returns
            // the runtime assembly, which turns out to define various public types that are really
            // internal (because they're not in the reference assemblies), and thus don't have XML docs.
            var referenceAssembliesFolder = Nuqleon.Documentation.Tests.RuntimeInfo.ReferenceAssembliesFolder;
            var referenceAssemblies = Directory.Exists(referenceAssembliesFolder)
                ? Directory.GetFiles(referenceAssembliesFolder, "*.dll", SearchOption.AllDirectories)
                : Array.Empty<string>();
            var resolver = new PathAssemblyResolver(referenceAssemblies);
            mlc = new MetadataLoadContext(resolver);
        }

        public void Dispose()
        {
            mlc.Dispose();
        }

        private static bool RunningOnMono => Type.GetType("Mono.Runtime") != null;

        [TestMethod]
        public void XmlDocs_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => XmlDocumentation.GetXmlDoc(default(MemberInfo)));
            Assert.ThrowsException<ArgumentNullException>(() => XmlDocumentation.GetXmlDoc(default(ParameterInfo)));
        }

        [TestMethod]
        public void XmlDocs_EnumerateTypes()
        {
            if (RunningOnMono)
            {
                return;
            }

            foreach (var t in GetTypes())
            {
                var d = XmlDocumentation.GetXmlDoc(t);
                Assert.IsNotNull(d);
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateMethods()
        {
            if (RunningOnMono)
            {
                return;
            }

            var forbiddenPrefixes = new[] { "get_", "set_", "add_", "remove_" };

            var res = from t in GetTypes()
                      from m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                      where !forbiddenPrefixes.Any(p => m.Name.StartsWith(p))
                      where !(metadataDelegateType.IsAssignableFrom(m.DeclaringType) && m.Name.Contains("Invoke"))
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

                bool isExtensionMethod = m.GetCustomAttributesData().Any(ad =>
                    ad.AttributeType.FullName == "System.Runtime.CompilerServices.ExtensionAttribute");
                var parametersWithDocumentation = isExtensionMethod
                    ? m.GetParameters().Skip(1)
                    : m.GetParameters();
                foreach (var p in parametersWithDocumentation)
                {
                    var e = XmlDocumentation.GetXmlDoc(p);
                    Assert.IsNotNull(e);
                }
            }
        }

        [TestMethod]
        public void XmlDocs_EnumerateProperties()
        {
            if (RunningOnMono)
            {
                return;
            }

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
            if (RunningOnMono)
            {
                return;
            }

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
            if (RunningOnMono)
            {
                return;
            }

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
            if (RunningOnMono)
            {
                return;
            }

            var res = from t in GetTypes()
                      where !metadataDelegateType.IsAssignableFrom(t)
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

        private IEnumerable<Type> GetTypes()
        {
            var blackList = new[]
            {
                "System.__ComObject", "System.AppContext", "System.FormattableString",

                // Until either the TextTransformCore tool supports .NET 10, or we're able to move
                // off T4, we need to skip types that are only present in .NET 10, because the
                // Nuqleon.Documentation assembly needs to be built against a version of .NET that
                // T4 can use. That means it is unable to find XML docs for newer types.
                // (To date, none of the templates have ever generated anything that wasn't present
                // in .NET FX, so this shouldn't be a problem in practice. It just means this test
                // needs to be careful about what it asks for.)
                "System.EventHandler`2", "System.Collections.Generic.IAlternateEqualityComparer`2"
            };

            var systemRuntime = mlc.LoadFromAssemblyName("System.Runtime");
            var systemLinq = mlc.LoadFromAssemblyName("System.Linq");
            metadataDelegateType = systemRuntime.GetType("System.Delegate");

            var res =
                from t in systemRuntime.GetTypes()
                where t.Namespace is "System" or "System.Collections" or "System.Collections.Generic"
                where t.IsPublic
                where !blackList.Contains(t.FullName)
                select t;

            res = res.Concat(
                from t in systemLinq.GetTypes()
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
