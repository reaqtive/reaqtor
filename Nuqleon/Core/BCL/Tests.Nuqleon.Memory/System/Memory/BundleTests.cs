// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 10/27/2014 - Created bundle functionality.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Memory;

namespace Tests
{
    [TestClass]
    public class BundleTests
    {
        [TestMethod]
        public void Bundles_ManOrBoy()
        {
            for (var i = 1; i < 100; i++)
            {
                var vals = Enumerable.Range(0, i);

                var res = Bundle.Create(vals.Select(v => (object)v).ToArray());

                for (var j = 0; j < i; j++)
                {
                    if ((int)res[j] != j)
                    {
                        Assert.Fail();
                        break;
                    }
                }
            }
        }

        [TestMethod]
        public void Bundles_Enumerable()
        {
            var vals = Enumerable.Range(0, 10);
            var res = Bundle.Create(vals.Select(v => (object)v));

            for (var i = 0; i < 10; i++)
            {
                if ((int)res[i] != i)
                {
                    Assert.Fail();
                    break;
                }
            }
        }

        [TestMethod]
        public void Bundles_List()
        {
            var vals = Enumerable.Range(0, 10);
            var res = Bundle.Create((IEnumerable<object>)vals.Select(v => (object)v).ToList());

            for (var i = 0; i < 10; i++)
            {
                if ((int)res[i] != i)
                {
                    Assert.Fail();
                    break;
                }
            }
        }

        [TestMethod]
        public void Bundles_ArgumentOutOfRangeExceptions()
        {
            for (var i = 1; i < 100; i++)
            {
                var vals = Enumerable.Range(0, i);

                var res = Bundle.Create(vals.Select(v => (object)v).ToArray());

                Assert.ThrowsException<ArgumentOutOfRangeException>(() => res[-1]);
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => res[i]);
            }
        }

        [TestMethod]
        public void Bundles_Create_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => Bundle.Create(default(object[])));
            Assert.ThrowsException<ArgumentNullException>(() => Bundle.Create(default(IEnumerable<object>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Bundles_Assembly()
        {
            Type type = typeof(Bundle);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Memory", assembly);
        }

        [TestMethod]
        public void Bundles_Create_Empty()
        {
            var empty = Bundle.Create(Array.Empty<object>());

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => empty[-1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => empty[0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => empty[1]);
        }
    }
}
