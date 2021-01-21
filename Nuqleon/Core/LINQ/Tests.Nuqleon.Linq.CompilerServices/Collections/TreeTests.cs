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
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void Tree_ArgumentChecking()
        {
            var tree = new Tree<int>(42);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => new Tree<int>(42, default(IEnumerable<ITree<int>>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new Tree<int>(42, default(ITree<int>[])), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => tree.ToString(-1), ex => Assert.AreEqual("indent", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void Tree_Leaf()
        {
            var tree = new Tree<int>(42);

            Assert.AreEqual(42, tree.Value);
            Assert.AreEqual(42, ((ITree)tree).Value);
            Assert.AreEqual(0, tree.Children.Count);
            Assert.AreEqual(0, ((ITree)tree).Children.Count);

            Assert.AreEqual("42()", tree.ToString());
            Assert.AreEqual("42()", tree.ToString(0));
        }

        [TestMethod]
        public void Tree_Leaf_Null()
        {
            var tree = new Tree<string>(value: null);

            Assert.IsNull(tree.Value);
            Assert.AreEqual(0, tree.Children.Count);

            Assert.AreEqual("<null>()", tree.ToString());
            Assert.AreEqual("<null>()", tree.ToString(0));
        }

        [TestMethod]
        public void Tree_Children()
        {
            var leaf1 = new Tree<int>(42);
            var leaf2 = new Tree<int>(43);
            var leaf3 = new Tree<int>(44);
            var node1 = new Tree<int>(91, leaf1, leaf2);
            var node2 = new Tree<int>(92, new[] { node1, leaf3 });
            var node3 = new Tree<int>(93, (IEnumerable<ITree<int>>)new[] { node2 });

            Assert.AreEqual(93, node3.Value);
            Assert.AreEqual(92, node2.Value);
            Assert.AreEqual(91, node1.Value);
            Assert.AreEqual(44, leaf3.Value);
            Assert.AreEqual(43, leaf2.Value);
            Assert.AreEqual(42, leaf1.Value);

            Assert.AreEqual(1, node3.Children.Count);
            Assert.AreEqual(2, node2.Children.Count);
            Assert.AreEqual(2, node1.Children.Count);
            Assert.AreEqual(0, leaf3.Children.Count);
            Assert.AreEqual(0, leaf2.Children.Count);
            Assert.AreEqual(0, leaf1.Children.Count);

            Assert.IsTrue(new[] { node2 }.SequenceEqual(node3.Children));
            Assert.IsTrue(new[] { node1, leaf3 }.SequenceEqual(node2.Children));
            Assert.IsTrue(new[] { leaf1, leaf2 }.SequenceEqual(node1.Children));

            Assert.AreEqual("93(92(91(42(), 43()), 44()))", node3.ToString());
            var expected = @"93(
  92(
    91(
      42(), 
      43()
    ), 
    44()
  )
)";
            Assert.AreEqual(expected.Replace("\r", string.Empty), node3.ToString(0).Replace("\r", string.Empty));
        }

        [TestMethod]
        public void Tree_FormatStrings()
        {
            var leaf1 = new Tree<string>("{0} {{1}}");
            var leaf2 = new Tree<string>("{{{2}}}");
            var node1 = new Tree<string>("{1}{2}{0}", leaf1, leaf2);

            var res = node1.ToString(0);
            var expected =
@"{1}{2}{0}(
  {0} {{1}}(), 
  {{{2}}}()
)";
            Assert.AreEqual(expected.Replace("\r", string.Empty), res.Replace("\r", string.Empty));
        }

        [TestMethod]
        public void Tree_Update_ArgumentChecking()
        {
            var tree = new Tree<int>(42);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => tree.Update(default(IEnumerable<ITree<int>>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ((ITree)tree).Update(default(IEnumerable<ITree>)), ex => Assert.AreEqual("children", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => tree.Update(default(ITree<int>[])), ex => Assert.AreEqual("children", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression

            new MyTree().TestUpdateArgs();
        }

        [TestMethod]
        public void Tree_Update1()
        {
            var leaf1 = new Tree<int>(42);
            var leaf2 = new Tree<int>(43);
            var leaf3 = new Tree<int>(44);

            var tree1 = new Tree<int>(1, leaf1, leaf2);
            var tree2 = tree1.Update(leaf2, leaf3);

            Assert.AreEqual(tree1.Value, tree2.Value);
            Assert.IsTrue(new[] { leaf1, leaf2 }.SequenceEqual(tree1.Children));
            Assert.IsTrue(new[] { leaf2, leaf3 }.SequenceEqual(tree2.Children));
        }

        [TestMethod]
        public void Tree_Update2()
        {
            var leaf1 = new Tree<int>(42);
            var leaf2 = new Tree<int>(43);
            var leaf3 = new Tree<int>(44);

            var tree1 = new Tree<int>(1, leaf1, leaf2);
            var tree2 = tree1.Update((IEnumerable<ITree<int>>)new[] { leaf2, leaf3 });

            Assert.AreEqual(tree1.Value, tree2.Value);
            Assert.IsTrue(new[] { leaf1, leaf2 }.SequenceEqual(tree1.Children));
            Assert.IsTrue(new[] { leaf2, leaf3 }.SequenceEqual(tree2.Children));
        }

        [TestMethod]
        public void Tree_Update3()
        {
            var leaf1 = new Tree<int>(42);
            var leaf2 = new Tree<int>(43);
            var leaf3 = new Tree<int>(44);

            var tree1 = new Tree<int>(1, leaf1, leaf2);
            var tree2 = ((ITree)tree1).Update(new[] { leaf2, leaf3 });

            Assert.AreEqual(tree1.Value, tree2.Value);
            Assert.IsTrue(new[] { leaf1, leaf2 }.SequenceEqual(tree1.Children));
            Assert.IsTrue(new[] { leaf2, leaf3 }.SequenceEqual(tree2.Children));
        }

        [TestMethod]
        public void Tree_Update4()
        {
            var leaf1 = new Tree<int>(42);
            var leaf2 = new Tree<int>(43);

            var tree1 = new Tree<int>(1, leaf1, leaf2);
            var tree2 = tree1.Update(leaf1, leaf2);

            Assert.AreSame(tree1, tree2);
        }

        [TestMethod]
        public void Tree_Accept_ArgumentChecking()
        {
            var tree = new Tree<int>(42);
            AssertEx.ThrowsException<ArgumentNullException>(() => tree.Accept(visitor: null), ex => Assert.AreEqual("visitor", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ((ITree)tree).Accept(visitor: null), ex => Assert.AreEqual("visitor", ex.ParamName));
        }

        [TestMethod]
        public void Tree_Accept_Simple()
        {
            var tree = new Tree<int>(1, new Tree<int>(3), new Tree<int>(4), new Tree<int>(5));

            var res1 = tree.Accept(new MyVisitor1());
            var res2 = ((ITree)tree).Accept(new MyVisitor2());

            var expected = new Tree<int>(1, new Tree<int>(3), new Tree<int>(2), new Tree<int>(5));

            var eq1 = new TreeEqualityComparer<int>();
            var eq2 = new TreeEqualityComparer();
            Assert.IsTrue(eq1.Equals(expected, res1));
            Assert.IsTrue(eq2.Equals(expected, res2));
        }

        [TestMethod]
        public void Tree_NullNode()
        {
            var tree = new Tree<int>(1, new Tree<int>(2), null, new Tree<int>(3));

            Assert.AreEqual("1(2(), <null>, 3())", tree.ToString());
            Assert.AreEqual(
@"1(
  2(), 
  <null>, 
  3()
)".Replace("\r", string.Empty), tree.ToString(0).Replace("\r", string.Empty));

            Assert.IsTrue(new TreeEqualityComparer().Equals(tree, tree));
            Assert.IsTrue(new TreeEqualityComparer<int>().Equals(tree, tree));

            Assert.AreSame(tree, tree.Accept(new TreeVisitor<int>()));
            Assert.AreSame(tree, ((ITree)tree).Accept(new TreeVisitor()));
        }

        private sealed class MyTree : Tree<int>
        {
            public MyTree()
                : base(42)
            {
            }

            public void TestUpdateArgs()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.UpdateCore(children: null), ex => Assert.AreEqual("children", ex.ParamName));
            }
        }

        private sealed class MyVisitor1 : TreeVisitor<int>
        {
            public override ITree<int> Visit(ITree<int> node)
            {
                if (node.Value % 2 == 0)
                    return new Tree<int>(node.Value / 2, Visit(node.Children));

                return base.Visit(node);
            }
        }

        private sealed class MyVisitor2 : TreeVisitor
        {
            public override ITree Visit(ITree node)
            {
                if ((int)node.Value % 2 == 0)
                    return new Tree<int>((int)node.Value / 2, Visit(node.Children).Cast<ITree<int>>());

                return base.Visit(node);
            }
        }
    }
}
