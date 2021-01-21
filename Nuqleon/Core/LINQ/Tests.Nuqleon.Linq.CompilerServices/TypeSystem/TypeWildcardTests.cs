// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq;
using System.Linq.CompilerServices.TypeSystem;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeWildcardTests
    {
        [TestMethod]
        public void Wildcards_NoConstraints()
        {
            var ts = new[] { typeof(T), typeof(R), typeof(T1), typeof(T2), typeof(T3) };

            foreach (var t in ts)
            {
                Assert.IsNull(t.GetConstructor(Type.EmptyTypes));

                var ctor = t.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
                Assert.IsNotNull(ctor);

                var obj = ctor.Invoke(new object[] { false });
                Assert.IsNotNull(obj);
            }
        }
    }
}
