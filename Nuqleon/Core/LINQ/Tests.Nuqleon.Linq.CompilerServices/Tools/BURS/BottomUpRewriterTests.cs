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
using System.Collections;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using Tests.System.Linq.CompilerServices.Tools.BURS;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public partial class BottomUpRewriterTests
    {
        [TestMethod]
        public void BottomUpRewriter_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => new BottomUpRewriter<ITree<Bar>, Bar, ITree<Foo>, BarWildcards>(sourceNodeComparer: null), ex => Assert.AreEqual("sourceNodeComparer", ex.ParamName));

            var burs = new BottomUpRewriter<ITree<Bar>, Bar, ITree<Foo>, BarWildcards>();

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Leaves.Add(default(Expression<Func<ITree<Bar>, ITree<Foo>>>), 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Leaves.Add((ITree<Bar> b) => null, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Leaves.Add(default(Expression<Func<ITree<Bar>, ITree<Foo>>>), (ITree<Bar> b) => true, 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Leaves.Add((ITree<Bar> b) => null, default(Expression<Func<ITree<Bar>, bool>>), 1), ex => Assert.AreEqual("predicate", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Leaves.Add((ITree<Bar> b) => null, (ITree<Bar> b) => true, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(pattern: null, () => null, 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(() => null, goal: null, 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add(() => null, () => null, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(pattern: null, (f1) => f1, 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((b1) => b1, goal: null, 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((b1) => b1, (f1) => f1, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(pattern: null, (f1, f2) => f1, 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((b1, b2) => b1, goal: null, 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((b1, b2) => b1, (f1, f2) => f1, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add(pattern: null, (f1, f2, f3) => f1, 1), ex => Assert.AreEqual("pattern", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Rules.Add((b1, b2, b3) => b1, goal: null, 1), ex => Assert.AreEqual("goal", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Rules.Add((b1, b2, b3) => b1, (f1, f2, f3) => f1, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Fallbacks.Add(default(Expression<Func<ITree<Bar>, ITree<Foo>>>), 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Fallbacks.Add((ITree<Bar> b) => null, -1), ex => Assert.AreEqual("cost", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Fallbacks.Add(default(Expression<Func<ITree<Bar>, ITree<Foo>>>), (ITree<Bar> b) => true, 1), ex => Assert.AreEqual("convert", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Fallbacks.Add((ITree<Bar> b) => null, default(Expression<Func<ITree<Bar>, bool>>), 1), ex => Assert.AreEqual("predicate", ex.ParamName));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => burs.Fallbacks.Add((ITree<Bar> b) => null, (ITree<Bar> b) => true, -1), ex => Assert.AreEqual("cost", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var mb = new MyBurs();
            mb.Fallbacks.Add(b => null, 1);
            mb.Leaves.Add<ITree<Bar>>(b => null, 1);
            mb.Rules.Add(b => null, f => null, 1);
            Assert.AreEqual(3, mb.Count);
        }

        private sealed class MyBurs : BottomUpRewriter<ITree<Bar>, Bar, ITree<Foo>, BarWildcards>
        {
            public int Count;

            protected override void OnFallbackAdded(Fallback<ITree<Bar>, ITree<Foo>> fallback)
            {
                Count++;
                AssertEx.ThrowsException<ArgumentNullException>(() => base.OnFallbackAdded(fallback: null), ex => Assert.AreEqual("fallback", ex.ParamName));
            }

            protected override void OnLeafAdded(Leaf<ITree<Bar>, ITree<Foo>> leaf)
            {
                Count++;
                AssertEx.ThrowsException<ArgumentNullException>(() => base.OnLeafAdded(leaf: null), ex => Assert.AreEqual("leaf", ex.ParamName));
            }

            protected override void OnRuleAdded(Rule<ITree<Bar>, ITree<Foo>> rule)
            {
                Count++;
                AssertEx.ThrowsException<ArgumentNullException>(() => base.OnRuleAdded(rule: null), ex => Assert.AreEqual("rule", ex.ParamName));
            }
        }

        [TestMethod]
        public void BottomUpRewriter_RuleTableSetup()
        {
            var burs = GetNumOptimizerWithFallback();

            Assert.AreEqual(2, burs.Leaves.Count);
            Assert.AreEqual(5, burs.Rules.Count);
            Assert.AreEqual(1, burs.Fallbacks.Count);

            {
                var le = burs.Leaves.GetEnumerator();
                Assert.IsNotNull(le);
                Assert.IsTrue(le.MoveNext() && le.MoveNext() && !le.MoveNext());

                var re = burs.Rules.GetEnumerator();
                Assert.IsNotNull(re);
                Assert.IsTrue(re.MoveNext() && re.MoveNext() && re.MoveNext() && re.MoveNext() && re.MoveNext() && !re.MoveNext());

                var fe = burs.Fallbacks.GetEnumerator();
                Assert.IsNotNull(fe);
                Assert.IsTrue(fe.MoveNext() && !fe.MoveNext());
            }

            {
                var le = ((IEnumerable)burs.Leaves).GetEnumerator();
                Assert.IsNotNull(le);
                Assert.IsTrue(le.MoveNext() && le.MoveNext() && !le.MoveNext());

                var re = ((IEnumerable)burs.Rules).GetEnumerator();
                Assert.IsNotNull(re);
                Assert.IsTrue(re.MoveNext() && re.MoveNext() && re.MoveNext() && re.MoveNext() && re.MoveNext() && !re.MoveNext());

                var fe = ((IEnumerable)burs.Fallbacks).GetEnumerator();
                Assert.IsNotNull(fe);
                Assert.IsTrue(fe.MoveNext() && !fe.MoveNext());
            }
        }

        [TestMethod]
        public void BottomUpRewriter_Simple1()
        {
            var burs = GetNumOptimizer();

            var res = burs.Rewrite(new ConstNumTree(42));

            var cst = res as ConstNumTree;
            Assert.IsNotNull(cst);

            Assert.AreEqual(42, cst.Value);
        }

        [TestMethod]
        public void BottomUpRewriter_Simple2()
        {
            var burs = GetNumOptimizer();

            var res = burs.Rewrite(new BinaryNumTree(NumKind.Add, new ConstNumTree(42), ConstNumTree.Zero));

            var cst = res as ConstNumTree;
            Assert.IsNotNull(cst);

            Assert.AreEqual(42, cst.Value);
        }

        [TestMethod]
        public void BottomUpRewriter_Simple3()
        {
            var burs = GetNumOptimizer();

            var res = burs.Rewrite(new BinaryNumTree(NumKind.Add, ConstNumTree.Zero, new ConstNumTree(42)));

            var cst = res as ConstNumTree;
            Assert.IsNotNull(cst);

            Assert.AreEqual(42, cst.Value);
        }

        [TestMethod]
        public void BottomUpRewriter_Simple4()
        {
            var burs = GetNumOptimizerWithFallback();

            var res = burs.Rewrite(new BinaryNumTree(NumKind.Add, new ConstNumTree(19), new ConstNumTree(23)));

            var cst = res as ConstNumTree;
            Assert.IsNotNull(cst);

            Assert.AreEqual(42, cst.Value);
        }

        [TestMethod]
        public void BottomUpRewriter_Simple5()
        {
            var burs = GetNumOptimizer();

            var num = new BinaryNumTree(NumKind.Add, new ConstNumTree(19), new ConstNumTree(23));
            var res = burs.Rewrite(num);

            Assert.AreEqual(num.Eval(), res.Eval());
        }

        [TestMethod]
        public void BottomUpRewriter_Simple6()
        {
            var burs = GetNumOptimizer();

            var num = new BinaryNumTree(NumKind.Add, new BinaryNumTree(NumKind.Add, new UnaryNumTree(NumKind.Negate, ConstNumTree.Zero), new ConstNumTree(19)), new BinaryNumTree(NumKind.Multiply, new ConstNumTree(23), ConstNumTree.One));
            var res = burs.Rewrite(num);

            Assert.AreEqual(42, res.Eval());
        }

        [TestMethod]
        public void BottomUpRewriter_UnusedWildcard()
        {
            var burs = new BottomUpRewriter<NumTree, Num, NumTree, NumWildcards>();

            Assert.ThrowsException<InvalidOperationException>(() => burs.Rules.Add((a, b, c) => new BinaryNumTree(NumKind.Add, a, b), (a, b, c) => a, 1));
        }

        private static BottomUpRewriter<NumTree, Num, NumTree, NumWildcards> GetNumOptimizer()
        {
            var burs = new BottomUpRewriter<NumTree, Num, NumTree, NumWildcards>();

            burs.Leaves.Add<ConstNumTree>(c => c, 1);
            burs.Leaves.Add<ConstNumTree>(c => c, c => c.Value > 0, 1);

            burs.Rules.Add(() => new UnaryNumTree(NumKind.Negate, ConstNumTree.Zero), () => ConstNumTree.Zero, 1);

            burs.Rules.Add(n => new BinaryNumTree(NumKind.Add, n, ConstNumTree.Zero), n => n, 1);
            burs.Rules.Add(n => new BinaryNumTree(NumKind.Add, ConstNumTree.Zero, n), n => n, 1);

            burs.Rules.Add(n => new BinaryNumTree(NumKind.Multiply, n, ConstNumTree.One), n => n, 1);
            burs.Rules.Add(n => new BinaryNumTree(NumKind.Multiply, ConstNumTree.One, n), n => n, 1);

            return burs;
        }

        private static BottomUpRewriter<NumTree, Num, NumTree, NumWildcards> GetNumOptimizerWithFallback()
        {
            var burs = GetNumOptimizer();

            burs.Fallbacks.Add(n => new ConstNumTree(n.Eval()), 1);

            return burs;
        }
    }
}
