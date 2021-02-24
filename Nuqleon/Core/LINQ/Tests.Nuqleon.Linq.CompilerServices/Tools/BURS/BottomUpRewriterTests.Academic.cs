// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.IO;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleTrees.Arithmetic;
using SampleTrees.Numerical;

namespace Tests.System.Linq.CompilerServices
{
    public partial class BottomUpRewriterTests
    {
        [TestMethod]
        public void BottomUpRewriter_ArithToNum()
        {
            var logger = new StringWriter();

            var burw = new BottomUpRewriter<ArithExpr, ArithNodeType, NumExpr, ArithWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (Const c) => new Val(c.Value), 1 },
                },

                // Tree patterns
                Rules =
                {
                    { (l, r) => new Add(l, r), (l, r) => new Plus(l, r), 2 },
                    { (l, r) => new Mul(l, r), (l, r) => new Times(l, r), 3 },
                    { (a, b, c) => new Add(new Mul(a, b), c), (a, b, c) => new TimesPlus(a, b, c), 4 },
                    { x => new Add(x, new Const(1)), x => new Inc(x), 1 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = new Add(
                new Mul(
                    new Add(
                        new Mul(
                            new Const(2),
                            new Const(3)
                        ),
                        new Const(1)
                    ),
                    new Mul(
                        new Const(4),
                        new Const(5)
                    )
                ),
                new Const(6)
            );

            var res = burw.Rewrite(e);

            Assert.AreEqual("TimesPlus(Inc(Times(2, 3)), Times(4, 5), 6)", res.ToString());
            Assert.AreEqual(e.Eval(), res.Eval());
        }

        [TestMethod]
        public void BottomUpRewriter_NumToArith()
        {
            var logger = new StringWriter();

            var burw = new BottomUpRewriter<NumExpr, NumNodeType, ArithExpr, NumWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (Val c) => new Const(c.Value), 1 },
                },

                // Tree patterns
                Rules =
                {
                    { (l, r) => new Plus(l, r), (l, r) => new Add(l, r), 2 },
                    { (l, r) => new Times(l, r), (l, r) => new Mul(l, r), 2 },
                    { (a, b, c) => new TimesPlus(a, b, c), (a, b, c) => new Add(new Mul(a, b), c), 2 },
                    { x => new Inc(x), x => new Add(x, new Const(1)), 2 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = new TimesPlus(
                new Inc(
                    new Times(
                        new Val(2),
                        new Val(3)
                    )
                ),
                new Times(
                    new Val(4),
                    new Val(5)
                ),
                new Val(6)
            );

            var res = burw.Rewrite(e);

            Assert.AreEqual("Add(Mul(Add(Mul(Const(2), Const(3)), Const(1)), Mul(Const(4), Const(5))), Const(6))", res.ToString());
            Assert.AreEqual(e.Eval(), res.Eval());
        }

        [TestMethod]
        public void BottomUpRewriter_NumToArith_Partial_Irreducible()
        {
            var logger = new StringWriter();

            var burw = new BottomUpRewriter<NumExpr, NumNodeType, ArithExpr, NumWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (Val c) => new Const(c.Value), 1 },
                },

                // Tree patterns
                Rules =
                {
                    { (l, r) => new Plus(l, r), (l, r) => new Add(l, r), 2 },
                    { (a, b, c) => new TimesPlus(a, b, c), (a, b, c) => new Add(new Mul(a, b), c), 2 },
                    { x => new Inc(x), x => new Add(x, new Const(1)), 2 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = new TimesPlus(
                new Inc(
                    new Times(
                        new Val(2),
                        new Val(3)
                    )
                ),
                new Times(
                    new Val(4),
                    new Val(5)
                ),
                new Val(6)
            );

            Assert.ThrowsException<InvalidOperationException>(() => burw.Rewrite(e));
        }

        [TestMethod]
        public void BottomUpRewriter_NumToArith_Partial_FallbackUnconditional()
        {
            var logger = new StringWriter();

            var burw = new BottomUpRewriter<NumExpr, NumNodeType, ArithExpr, NumWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (Val c) => new Const(c.Value), 1 },
                },

                // Tree patterns
                Rules =
                {
                    { (l, r) => new Plus(l, r), (l, r) => new Add(l, r), 2 },
                    { (a, b, c) => new TimesPlus(a, b, c), (a, b, c) => new Add(new Mul(a, b), c), 2 },
                    { x => new Inc(x), x => new Add(x, new Const(1)), 2 },
                },

                // Fallback rules
                Fallbacks =
                {
                    { n => new Lazy(() => n.Eval()), 1 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = new TimesPlus(
                new Inc(
                    new Times(
                        new Val(2),
                        new Val(3)
                    )
                ),
                new Times(
                    new Val(4),
                    new Val(5)
                ),
                new Val(6)
            );

            var res = burw.Rewrite(e);

            Assert.IsTrue(res.ToString().Contains("Lazy"));
            Assert.AreEqual(e.Eval(), res.Eval());
        }

        [TestMethod]
        public void BottomUpRewriter_NumToArith_Partial_FallbackWithPredicate()
        {
            var logger = new StringWriter();

            var burw = new BottomUpRewriter<NumExpr, NumNodeType, ArithExpr, NumWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (Val c) => new Const(c.Value), 1 },
                },

                // Tree patterns
                Rules =
                {
                    { (l, r) => new Plus(l, r), (l, r) => new Add(l, r), 2 },
                    { (a, b, c) => new TimesPlus(a, b, c), (a, b, c) => new Add(new Mul(a, b), c), 2 },
                    { x => new Inc(x), x => new Add(x, new Const(1)), 2 },
                },

                // Fallback rules
                Fallbacks =
                {
                    { n => new Const(((Times)n).Let(t => t.Left.Eval() * t.Right.Eval())), n => n.Value == NumNodeType.Times, 1 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = new TimesPlus(
                new Inc(
                    new Times(
                        new Val(2),
                        new Val(3)
                    )
                ),
                new Times(
                    new Val(4),
                    new Val(5)
                ),
                new Val(6)
            );

            var res = burw.Rewrite(e);

            Assert.AreEqual("Add(Mul(Add(Const(6), Const(1)), Const(20)), Const(6))", res.ToString());
            Assert.AreEqual(e.Eval(), res.Eval());
        }
    }
}
