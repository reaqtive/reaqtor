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
using Tests.System.Linq.CompilerServices.Tools.BURS;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public partial class BottomUpOptimizerTests
    {
        [TestMethod]
        public void BottomUpOptimizer_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new BottomUpOptimizer<ITree<Bar>, Bar, BarWildcards>(sourceTreeComparer: null, EqualityComparer<Bar>.Default), ex => Assert.AreEqual("sourceTreeComparer", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new BottomUpOptimizer<ITree<Bar>, Bar, BarWildcards>(EqualityComparer<ITree<Bar>>.Default, sourceNodeComparer: null), ex => Assert.AreEqual("sourceNodeComparer", ex.ParamName));

            var burs = new BottomUpOptimizer<ITree<Bar>, Bar, BarWildcards>();

            AssertEx.ThrowsException<ArgumentNullException>(() => burs.Optimize(tree: null), ex => Assert.AreEqual("tree", ex.ParamName));
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple1()
        {
            var burs = GetNumOptimizer();

            var t = new BinaryNumTree(NumKind.Add, new ConstNumTree(42), ConstNumTree.Zero);
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const(42)", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple2()
        {
            var burs = GetNumOptimizer();

            var t = new BinaryNumTree(NumKind.Add, ConstNumTree.Zero, new ConstNumTree(42));
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const(42)", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple3()
        {
            var burs = GetNumOptimizer();

            var t = new BinaryNumTree(NumKind.Multiply, ConstNumTree.One, new ConstNumTree(42));
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const(42)", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple4()
        {
            var burs = GetNumOptimizer();

            var t = new BinaryNumTree(NumKind.Multiply, new ConstNumTree(42), ConstNumTree.One);
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const(42)", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple5()
        {
            var burs = GetNumOptimizer();

            var t = new BinaryNumTree(NumKind.Multiply, new BinaryNumTree(NumKind.Add, new ConstNumTree(42), ConstNumTree.Zero), ConstNumTree.One);
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const(42)", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple6()
        {
            var burs = GetNumOptimizer();

            var t = new BinaryNumTree(NumKind.Add, new UnaryNumTree(NumKind.Negate, ConstNumTree.Zero), new ConstNumTree(42));
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const(42)", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple7()
        {
            var burs = GetStrOptimizer();

            var t = new ConcatStrTree();
            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Const()", r.ToString());
        }

        [TestMethod]
        public void BottomUpOptimizer_Simple8()
        {
            var burs = GetStrOptimizer();

            var t = new ConcatStrTree(
                new UnaryStrTree(StrKind.ToUpper,
                    new UnaryStrTree(StrKind.ToLower,
                        new ConstStrTree("FoO")
                    )
                ),
                new UnaryStrTree(StrKind.ToLower,
                    new ConcatStrTree(
                        ConstStrTree.Empty,
                        ConstStrTree.Empty
                    )
                ),
                new UnaryStrTree(StrKind.ToLower,
                    new UnaryStrTree(StrKind.ToUpper,
                        new ConstStrTree("bAr")
                    )
                )
            );

            var r = burs.Optimize(t);
            Assert.AreEqual(t.Eval(), r.Eval());
            Assert.AreEqual("Concat(ToUpper(Const(FoO)), ToLower(Const(bAr)))", r.ToString());
        }

        private static BottomUpOptimizer<NumTree, Num, NumWildcards> GetNumOptimizer()
        {
            var burs = new BottomUpOptimizer<NumTree, Num, NumWildcards>
            {
                Rules =
                {
                    { n => new BinaryNumTree(NumKind.Add, n, ConstNumTree.Zero), n => n, 1 },
                    { n => new BinaryNumTree(NumKind.Add, ConstNumTree.Zero, n), n => n, 1 },
                    { n => new BinaryNumTree(NumKind.Multiply, n, ConstNumTree.One), n => n, 1 },
                    { n => new BinaryNumTree(NumKind.Multiply, ConstNumTree.One, n), n => n, 1 },
                    { n => new BinaryNumTree(NumKind.Multiply, n, ConstNumTree.Zero), n => ConstNumTree.Zero, 1 },
                    { n => new BinaryNumTree(NumKind.Multiply, ConstNumTree.Zero, n), n => ConstNumTree.Zero, 1 },
                    { n => new UnaryNumTree(NumKind.Negate, n), n => ConstNumTree.Zero, 1 },
                },
            };

            return burs;
        }

        private static BottomUpOptimizer<StrTree, StrKind, StrWildcards> GetStrOptimizer()
        {
            var burs = new BottomUpOptimizer<StrTree, StrKind, StrWildcards>(new TreeEqualityComparer<StrKind>(), EqualityComparer<StrKind>.Default)
            {
                Leaves =
                {
                    { (ConcatStrTree c) => ConstStrTree.Empty, c => c.Children.Count == 0, 1 },
                },

                Rules =
                {
                    { () => new UnaryStrTree(StrKind.ToLower, ConstStrTree.Empty), () => ConstStrTree.Empty, 1 },
                    { () => new UnaryStrTree(StrKind.ToUpper, ConstStrTree.Empty), () => ConstStrTree.Empty, 1 },
                    { s => new UnaryStrTree(StrKind.ToUpper, new UnaryStrTree(StrKind.ToUpper, s)), s => new UnaryStrTree(StrKind.ToUpper, s), 1 },
                    { s => new UnaryStrTree(StrKind.ToUpper, new UnaryStrTree(StrKind.ToLower, s)), s => new UnaryStrTree(StrKind.ToUpper, s), 1 },
                    { s => new UnaryStrTree(StrKind.ToLower, new UnaryStrTree(StrKind.ToUpper, s)), s => new UnaryStrTree(StrKind.ToLower, s), 1 },
                    { s => new UnaryStrTree(StrKind.ToLower, new UnaryStrTree(StrKind.ToLower, s)), s => new UnaryStrTree(StrKind.ToLower, s), 1 },
                },

                Fallbacks =
                {
                    { s => new ConcatStrTree(s.Children.Cast<StrTree>().Where(c => c != ConstStrTree.Empty).ToArray()), s => s is ConcatStrTree && s.Children.Contains(ConstStrTree.Empty), 1 },
                },
            };

            return burs;
        }
    }
}
