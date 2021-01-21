// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TreeVisitorTests
    {
        [TestMethod]
        public void TreeVisitor_ArgumentChecking()
        {
            new MyVisitor().TestArgs();
            new MyVisitor<int>().TestArgs();
        }

        [TestMethod]
        public void TreeVisitor_Null()
        {
            Assert.IsNull(new TreeVisitor().Visit(node: null));
            Assert.IsNull(new TreeVisitor<int>().Visit(node: null));
        }

        [TestMethod]
        public void TreeVisitor_NoChange()
        {
            var tree = new Tree<int>(42, new Tree<int>(43), new Tree<int>(44, new Tree<int>(45)));

            var res1 = new TreeVisitor().Visit(tree);
            var res2 = new TreeVisitor<int>().Visit(tree);

            Assert.AreSame(tree, res1);
            Assert.AreSame(tree, res2);
        }

        [TestMethod]
        public void TreeVisitor_Trivial()
        {
            var tree = new Tree<int>(42, new Tree<int>(43), new Tree<int>(44, new Tree<int>(45), new Tree<int>(46)));

            var res1 = new MyVisitor().Visit(tree);
            var res2 = new MyVisitor<int>().Visit(tree);

            var expected = new Tree<int>(42, new Tree<int>(44, new Tree<int>(46), new Tree<int>(45)), new Tree<int>(43));

            var eq1 = new TreeEqualityComparer();
            var eq2 = new TreeEqualityComparer<int>();
            Assert.IsTrue(eq1.Equals(expected, res1));
            Assert.IsTrue(eq2.Equals(expected, res2));
        }

        private sealed class MyVisitor : TreeVisitor
        {
            public void TestArgs()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Visit(default(IReadOnlyList<ITree>)), ex => Assert.AreEqual("nodes", ex.ParamName));
            }

            public override ITree Visit(ITree node)
            {
                return node.Update(Visit(node.Children).Reverse());
            }
        }

        private sealed class MyVisitor<T> : TreeVisitor<T>
        {
            public void TestArgs()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Visit(default(IReadOnlyList<ITree<T>>)), ex => Assert.AreEqual("nodes", ex.ParamName));
            }

            public override ITree<T> Visit(ITree<T> node)
            {
                return node.Update(Visit(node.Children).Reverse());
            }
        }
    }
}
