// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleTrees.Logic;
using System.IO;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    public partial class BottomUpOptimizerTests
    {
        [TestMethod]
        public void BottomUpOptimizer_Logic1()
        {
            var logger = new StringWriter();

            var burw = new BottomUpOptimizer<LogicExpr, LogicNodeType, LogicWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (BoolConst b) => b, 1 },
                },

                Rules =
                {
                    // Tree patterns
                    { () => !BoolConst.True, () => BoolConst.False, 1 },
                    { () => !BoolConst.False, () => BoolConst.True, 1 },
                    { p => !!p, p => p, 2 },

                    { p => p & BoolConst.True, p => p, 2 },
                    { p => p & BoolConst.False, p => BoolConst.False, 2 },
                    { p => BoolConst.True & p, p => p, 2 },
                    { p => BoolConst.False & p, p => BoolConst.False, 2 },

                    /*
                     * With those rules commented out, we got a partial tree rewrite using Update
                     * method calls on the tree nodes.
                     *
                    { p => p | BoolConst.True, p => BoolConst.True, 2 },
                    { p => p | BoolConst.False, p => p, 2 },
                    { p => BoolConst.True | p, p => BoolConst.True, 2 },
                    { p => BoolConst.False | p, p => p, 2 },
                     *
                     */

                    { (p, q) => !(!p & !q), (p, q) => p | q, 1 },
                    { (p, q) => !(!p | !q), (p, q) => p & q, 1 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = !(!BoolConst.True & !BoolConst.False);

            var res = burw.Optimize(e);

            Assert.AreEqual("Or(True, False)", res.ToString());
            Assert.AreEqual(e.Eval(), res.Eval());
        }

        [TestMethod]
        public void BottomUpOptimizer_Logic2()
        {
            var logger = new StringWriter();

            var burw = new BottomUpOptimizer<LogicExpr, LogicNodeType, LogicWildcardFactory>
            {
                // Leaf nodes
                Leaves =
                {
                    { (BoolConst b) => b, 1 },
                },

                Rules =
                {
                    // Tree patterns
                    { () => !BoolConst.True, () => BoolConst.False, 1 },
                    { () => !BoolConst.False, () => BoolConst.True, 1 },
                    { p => !!p, p => p, 2 },

                    { p => p & BoolConst.True, p => p, 2 },
                    { p => p & BoolConst.False, p => BoolConst.False, 2 },
                    { p => BoolConst.True & p, p => p, 2 },
                    { p => BoolConst.False & p, p => BoolConst.False, 2 },

                    { p => p | BoolConst.True, p => BoolConst.True, 2 },
                    { p => p | BoolConst.False, p => p, 2 },
                    { p => BoolConst.True | p, p => BoolConst.True, 2 },
                    { p => BoolConst.False | p, p => p, 2 },

                    { (p, q) => !(!p & !q), (p, q) => p | q, 1 },
                    { (p, q) => !(!p | !q), (p, q) => p & q, 1 },
                },

                Log = logger
            };

            // Internal tables
            var debugView = burw.DebugView;
            Assert.IsTrue(!string.IsNullOrEmpty(debugView));

            var e = !(!BoolConst.True & !BoolConst.False);

            var res = burw.Optimize(e);

            Assert.AreEqual("True", res.ToString());
            Assert.AreEqual(e.Eval(), res.Eval());
        }
    }
}
