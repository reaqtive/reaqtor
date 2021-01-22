// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using Nuqleon.Json.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class DefaultNameProviderTests
    {
        [TestMethod]
        public void DefaultNameProvider_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => DefaultNameProvider.Instance.GetName(default(FieldInfo)));
            Assert.ThrowsException<ArgumentNullException>(() => DefaultNameProvider.Instance.GetName(default(PropertyInfo)));
        }

        [TestMethod]
        public void DefaultNameProvider_Simple()
        {
            Assert.AreEqual("Foo", DefaultNameProvider.Instance.GetName(typeof(Bar).GetProperty("Foo")));
            Assert.AreEqual("Qux", DefaultNameProvider.Instance.GetName(typeof(Bar).GetField("Qux")));
        }

        private sealed class Bar
        {
            public int Foo { get; set; }
            public int Qux = 42;
        }
    }
}
