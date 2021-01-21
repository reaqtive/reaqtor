// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;

namespace Tests.System.Collections.Specialized
{
    [TestClass]
    public partial class EnumDictionaryFactoryTests
    {
        [TestMethod]
        public void EnumDictionaryFactory_Parameter_Validation()
        {
            try
            {
                EnumDictionary.Create<int, bool>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                EnumDictionary.Create<Foo, bool>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                EnumDictionary.Create<Bar, bool>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                EnumDictionary.Create<Baz, bool>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }
        }

        private enum Foo : long
        {
        }

        private enum Bar : int
        {
            Qux = -1
        }

        [Flags]
        private enum Baz
        {
        }
    }
}
