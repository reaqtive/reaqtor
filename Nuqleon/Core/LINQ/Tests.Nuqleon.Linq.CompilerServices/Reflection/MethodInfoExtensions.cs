// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Linq.CompilerServices;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class MethodInfoExtensions
    {
        [TestMethod]
        public void MethodInfoExtensions_Static()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var mtd = ReflectionHelpers.InfoOf(() => Console.WriteLine("Hello"));
            var res = mtd.ToCSharpString();
            Assert.AreEqual("void Console::WriteLine(string value)", res);
        }

        [TestMethod]
        public void MethodInfoExtensions_Instance()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var mtd = ReflectionHelpers.InfoOf((string s) => s.Substring(0, 1));
            var res = mtd.ToCSharpString();
            Assert.AreEqual("string string.Substring(int startIndex, int length)", res);
        }

        [TestMethod]
        public void MethodInfoExtensions_Generic()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var mtd = ReflectionHelpers.InfoOf(() => Activator.CreateInstance<int>());
            var res = mtd.ToCSharpString();
            Assert.AreEqual("int Activator::CreateInstance<int>()", res);
        }
    }
}
