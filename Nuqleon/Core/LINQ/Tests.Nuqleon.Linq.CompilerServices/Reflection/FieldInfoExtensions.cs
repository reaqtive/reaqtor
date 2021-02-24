// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.CompilerServices;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class FieldInfoExtensions
    {
        [TestMethod]
        public void FieldInfoExtensions_Static()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var fld = ReflectionHelpers.InfoOf(() => Foo.Qux);
            var res = fld.ToCSharpString();
            Assert.AreEqual("string Foo::Qux", res);
        }

        [TestMethod]
        public void FieldInfoExtensions_Instance()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var fld = ReflectionHelpers.InfoOf((Foo f) => f.Bar);
            var res = fld.ToCSharpString();
            Assert.AreEqual("int Foo.Bar", res);
        }

        private sealed class Foo
        {
#pragma warning disable 0649 // Used through reflection
            public static string Qux;
            public int Bar;
#pragma warning restore
        }
    }
}
