// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Nuqleon.DataModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public class KnownTypeBaseTests
    {
        [TestMethod]
        public void KT_AreEqual()
        {
            var k1 = new KTExample
            {
                MappedString = "cow",
                MappedNested = new KTNested { MappedInt = 3, MappedRecursive = new KTNested { MappedInt = 99 }, Foos = { new Foo { Val = 42 } } }
            };
            var k2 = new KTExample
            {
                MappedString = "cow",
                MappedNested = new KTNested { MappedInt = 3, MappedRecursive = new KTNested { MappedInt = 99 }, Foos = { new Foo { Val = 42 } } }
            };

            Assert.IsTrue(k1.Equals(k2));
            Assert.IsTrue(EqualityComparer<KTExample>.Default.Equals(k1, k2));
            Assert.IsTrue(k1.Equals((object)k2));
            Assert.IsTrue(k2.Equals(k1));
            Assert.IsTrue(k2.Equals((object)k1));
            Assert.IsTrue(object.Equals(k1, k2));
            Assert.AreEqual(k1, k2);
        }

        [TestMethod]
        public void KT_AreNotEqual()
        {
            var k1 = new KTExample
            {
                MappedString = "cow",
                MappedNested = new KTNested { MappedInt = 3 }
            };
            var k2 = new KTExample
            {
                MappedString = "cow",
                MappedNested = new KTNested { MappedInt = 4 }
            };

            Assert.IsFalse(k1.Equals(k2));
            Assert.IsFalse(k1.Equals(null));
            Assert.IsFalse(object.Equals(k1, null));
        }

        [TestMethod]
        public void KT_NestedAreNotEqual()
        {
            var k1 = new KTExample
            {
                MappedNested = new KTNested { MappedRecursive = new KTNested { MappedInt = 1 } }
            };
            var k2 = new KTExample
            {
                MappedNested = new KTNested { MappedRecursive = new KTNested { MappedInt = 5 } }
            };

            Assert.IsFalse(k1.Equals(k2));
        }
    }

    internal class KTExample : KnownTypeBase<KTExample>
    {
        public KTExample()
        {
            Foos = new List<Foo>();
        }

        [Mapping("mappedString")]
        public string MappedString { get; set; }

        [Mapping("mappedNested")]
        public KTNested MappedNested { get; set; }

        [Mapping("foos")]
        public List<Foo> Foos { get; set; }
    }

    internal class KTNested : KnownTypeBase<KTNested>
    {
        public KTNested()
        {
            Foos = new List<Foo>();
        }

        [Mapping("mappedInt")]
        public int MappedInt { get; set; }

        [Mapping("mappedRecursive")]
        public KTNested MappedRecursive { get; set; }

        [Mapping("foos")]
        public List<Foo> Foos { get; set; }

        public string NonmappedString { get; set; }
    }

    internal class Foo
    {
        [Mapping("fooval")]
        public int Val { get; set; }
    }
}
