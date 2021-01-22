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

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class _WildcardTraversalMapTests
    {
        [TestMethod]
        public void WildcardTraversal_Simple()
        {
            var traversal_1_0 = new WildcardTraversal<string>("qux");
            traversal_1_0.Push(0);
            traversal_1_0.Push(1);

            Assert.AreEqual("1 -> 0", traversal_1_0.ToString());

            var tree_1_0 = new Tree<string>("qux");
            var tree_0 = new Tree<string>("foo");
            var tree_1 = new Tree<string>("bar", tree_1_0);
            var tree = new Tree<string>("baz", tree_0, tree_1);

            Assert.AreEqual("qux", traversal_1_0.Get(tree).Value);
        }

        [TestMethod]
        public void WildcardTraversalMap_Simple()
        {
            var traversal_1_0 = new WildcardTraversal<string>("qux");
            var map1_0 = new WildcardTraversalMap<string>();
            map1_0["qux"] = traversal_1_0;
            map1_0.PushPathSegment(0);
            map1_0.PushPathSegment(1);

            var traversal_1_1 = new WildcardTraversal<string>("xuq");
            var map1_1 = new WildcardTraversalMap<string>();
            map1_1["xuq"] = traversal_1_1;
            map1_1.PushPathSegment(1);
            map1_1.PushPathSegment(1);

            var map = new WildcardTraversalMap<string>();
            map.Merge(map1_0);
            map.Merge(map1_1);

            Assert.ThrowsException<InvalidOperationException>(() => map.Merge(map1_0));

            Assert.AreEqual("{ qux: 1 -> 0, xuq: 1 -> 1 }", map.ToString());

            var tree_1_0 = new Tree<string>("qux");
            var tree_1_1 = new Tree<string>("xuq");
            var tree_0 = new Tree<string>("foo");
            var tree_1 = new Tree<string>("bar", tree_1_0, tree_1_1);
            var tree = new Tree<string>("baz", tree_0, tree_1);

            Assert.AreEqual("qux", map["qux"].Get(tree).Value);
            Assert.AreEqual("xuq", map["xuq"].Get(tree).Value);
        }
    }
}
