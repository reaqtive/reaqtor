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
    public class ConstructorInfoExtensions
    {
        [TestMethod]
        public void ConstructorInfoExtensions_Simple()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var ctor = ReflectionHelpers.InfoOf(() => new TimeSpan(5, 6, 7));
            var res = ctor.ToCSharpString();
            Assert.AreEqual("new TimeSpan(int hours, int minutes, int seconds)", res);
        }

        [TestMethod]
        public void ReflectionHelpers_Assembly()
        {
            Type type = typeof(ReflectionHelpers);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Linq.CompilerServices", assembly);
        }
    }
}
