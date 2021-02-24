// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//

using System;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Serialization;

namespace Tests
{
    [TestClass]
    public class DefaultNameResolverTests
    {
        [TestMethod]
        public void DefaultNameResolver_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => DefaultNameResolver.Instance.GetNames(default(FieldInfo)));
            Assert.ThrowsException<ArgumentNullException>(() => DefaultNameResolver.Instance.GetNames(default(PropertyInfo)));
        }

        [TestMethod]
        public void DefaultNameResolver_Simple()
        {
            Assert.IsTrue(new[] { "Foo" }.SequenceEqual(DefaultNameResolver.Instance.GetNames(typeof(Bar).GetProperty("Foo"))));
            Assert.IsTrue(new[] { "Qux" }.SequenceEqual(DefaultNameResolver.Instance.GetNames(typeof(Bar).GetField("Qux"))));
        }

        private sealed class Bar
        {
            public int Foo { get; set; }
            public int Qux = 42;
        }
    }
}
