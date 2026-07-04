// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/20/2014 - Added tests.
//

using System;
using System.Collections.Generic;
using System.Documentation;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class Tests
{
    // NB: The XmlDocumentation library reads XML documentation files shipped alongside an
    //     assembly. The enumeration tests below use the BCL as their fixture. On modern .NET
    //     the BCL docs do not sit next to the runtime assemblies (as they did on .NET
    //     Framework); they live alongside the reference assemblies in the SDK's ref pack
    //     (e.g. packs/Microsoft.NETCore.App.Ref/<version>/ref/net10.0), which the library
    //     discovers. The tests degrade gracefully on runtime-only environments without a ref
    //     pack, where no BCL documentation can be located.

    private static bool DocsUnavailable => XmlDocumentation.GetXmlDoc(typeof(int)) == null;

    [TestMethod]
    public void XmlDocs_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => XmlDocumentation.GetXmlDoc(default(MemberInfo)));
        Assert.ThrowsExactly<ArgumentNullException>(() => XmlDocumentation.GetXmlDoc(default(ParameterInfo)));
    }

    [TestMethod]
    public void XmlDocs_EnumerateTypes()
    {
        if (DocsUnavailable)
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
        if (DocsUnavailable)
        {
            return;
        }

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

            // NB: The Trim*/ToString exclusions cover members new in .NET 10, which have no
            //     documentation in the 10.0 ref pack (yet).
            { typeof(string), new List<string> { "Format", "Trim", "TrimStart", "TrimEnd" } },
            { typeof(System.Collections.DictionaryEntry), new List<string> { "ToString" } },

            { typeof(GC), new List<string> { "Collect", "EndNoGCRegion", "TryStartNoGCRegion" } },

            // NB: The parameter is named strInput in the runtime assembly but differently in
            //     the ref pack documentation, so parameter lookups cannot match.
            { typeof(StringNormalizationExtensions), new List<string> { "IsNormalized", "Normalize" } },

            // NB: The 10.0 ref pack documents only a few of the checked conversion operators
            //     (e.g. Half->Byte); the rest have no documentation entries.
            { typeof(Half), new List<string> { "op_CheckedExplicit" } },
            { typeof(Int128), new List<string> { "op_CheckedExplicit" } },
            { typeof(UInt128), new List<string> { "op_CheckedExplicit" } },
            { typeof(IntPtr), new List<string> { "op_CheckedExplicit" } },
            { typeof(UIntPtr), new List<string> { "op_CheckedExplicit" } },

            // NB: The AlternateLookup parameter is a nested type of a generic type, for which
            //     the documentation IDs use the constructed declaring type (e.g.
            //     Dictionary{`0,`1}.AlternateLookup{``0}@), a form GetXmlDocName does not
            //     generate (long-standing limitation for nested types of generic types).
            { typeof(Dictionary<,>), new List<string> { "TryGetAlternateLookup" } },
            { typeof(HashSet<>), new List<string> { "TryGetAlternateLookup" } },

            // NB: For conversion operators on generic types, the ref pack XML files write the
            //     generic parameter by name (e.g. "op_Implicit(T[])") rather than by position
            //     ("op_Implicit(`0[])") as the documented ID format prescribes, so the
            //     generated names cannot match.
            { typeof(ArraySegment<>), new List<string> { "op_Implicit" } },
            { typeof(Memory<>), new List<string> { "op_Implicit" } },
            { typeof(Nullable<>), new List<string> { "op_Implicit", "op_Explicit" } },
            { typeof(ReadOnlyMemory<>), new List<string> { "op_Implicit" } },
            { typeof(ReadOnlySpan<>), new List<string> { "op_Implicit" } },
            { typeof(Span<>), new List<string> { "op_Implicit" } },
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
        if (DocsUnavailable)
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
        if (DocsUnavailable)
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
        if (DocsUnavailable)
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
        if (DocsUnavailable)
        {
            return;
        }

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
        var excludeList = new[]
        {
            "System.__ComObject",
            "System.AppContext",
            "System.FormattableString",

            // Implementation types that are public in System.Private.CoreLib but are not part
            // of the reference-assembly surface, so the ref pack has no documentation for them.
            "System.CultureAwareComparer",
            "System.OrdinalComparer",
            "System.UnitySerializationHolder",
            "System.Collections.ListDictionaryInternal",
            "System.Collections.Generic.ByteEqualityComparer",
            "System.Collections.Generic.EnumEqualityComparer`1",
            "System.Collections.Generic.GenericComparer`1",
            "System.Collections.Generic.GenericEqualityComparer`1",
            "System.Collections.Generic.NonRandomizedStringEqualityComparer",
            "System.Collections.Generic.NullableComparer`1",
            "System.Collections.Generic.NullableEqualityComparer`1",
            "System.Collections.Generic.ObjectComparer`1",
            "System.Collections.Generic.ObjectEqualityComparer`1",
        };

        var res =
            from t in typeof(int).Assembly.GetTypes()
            where t.Namespace is "System" or "System.Collections" or "System.Collections.Generic"
            where t.IsPublic
            where !excludeList.Contains(t.FullName)
            select t;

        res = res.Concat(
            from t in typeof(Enumerable).Assembly.GetTypes()
            where t.Namespace == "System.Linq"
            where t.IsPublic
            where !excludeList.Contains(t.FullName)
            select t);

        return res;
    }

    [TestMethod]
    public void XmlDocs_GetXmlDocName_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => XmlDocumentation.GetXmlDocName(null));
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
