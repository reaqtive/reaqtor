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
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class EventInfoExtensions
    {
        [TestMethod]
        public void EventInfoExtensions_Static()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var evt = (MemberInfo)typeof(Foo).GetEvent("Bar");
            var res = evt.ToCSharpString();
            Assert.AreEqual("event Action<int> Foo::Bar", res);
        }

        [TestMethod]
        public void EventInfoExtensions_Instance()
        {
            // NOTE: This internal method produces pseudo-C#. If ever exposed publicly, this would need to align with the declaration syntax.

            var evt = (MemberInfo)typeof(AppDomain).GetEvent("TypeResolve");
            var res = evt.ToCSharpString();
            Assert.AreEqual("event ResolveEventHandler AppDomain.TypeResolve", res);
        }

        private sealed class Foo
        {
#pragma warning disable 0067 // Used through reflection
            public static event Action<int> Bar;
#pragma warning restore
        }
    }
}
