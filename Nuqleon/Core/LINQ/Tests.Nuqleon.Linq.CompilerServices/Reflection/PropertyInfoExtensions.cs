// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class PropertyInfoExtensions
    {
        [TestMethod]
        public void PropertyInfoExtensions_Static()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var prp = ReflectionHelpers.InfoOf(() => DateTime.Now);
            var res = prp.ToCSharpString();
            Assert.AreEqual("DateTime DateTime::Now { get; }", res);
        }

        [TestMethod]
        public void PropertyInfoExtensions_Static_GetOnly()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var prp = ReflectionHelpers.InfoOf(() => Foo.Baz);
            var res = prp.ToCSharpString();
            Assert.AreEqual("int Foo::Baz { get; }", res);
        }

        [TestMethod]
        public void PropertyInfoExtensions_Static_SetOnly()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var prp = typeof(Foo).GetProperty("Quz");
            var res = prp.ToCSharpString();
            Assert.AreEqual("int Foo::Quz { set; }", res);
        }

        [TestMethod]
        public void PropertyInfoExtensions_Instance()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var prp = ReflectionHelpers.InfoOf((ADS s) => s.ApplicationBase);
            var res = prp.ToCSharpString();
            Assert.AreEqual("string ADS.ApplicationBase { get; set; }", res);
        }

        private sealed class ADS
        {
            public string ApplicationBase { get; set; }
        }

        [TestMethod]
        public void PropertyInfoExtensions_Instance_GetOnly()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var prp = ReflectionHelpers.InfoOf((Foo f) => f.Bar);
            var res = prp.ToCSharpString();
            Assert.AreEqual("int Foo.Bar { get; }", res);
        }

        [TestMethod]
        public void PropertyInfoExtensions_Instance_SetOnly()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var prp = typeof(Foo).GetProperty("Qux");
            var res = prp.ToCSharpString();
            Assert.AreEqual("int Foo.Qux { set; }", res);
        }

#pragma warning disable CA1822 // Mark static
        private sealed class Foo
        {
            public int Bar => 1;
            public int Qux { set { } }

            public static int Baz => 1;
            public static int Quz { set { } }
        }
#pragma warning restore CA1822
    }
}
