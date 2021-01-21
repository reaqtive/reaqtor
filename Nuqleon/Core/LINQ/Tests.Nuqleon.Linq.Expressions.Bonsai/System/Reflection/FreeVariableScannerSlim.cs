// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class FreeVariableScannerSlimTests : TestBase
    {
        [TestMethod]
        public void FreeVariableScannerSlim_Find_Lambda1()
        {
            var p1 = ExpressionSlim.Parameter(SlimType, "p1");

            var slimExpr = ExpressionSlim.Lambda(p1);

            var free = FreeVariableScannerSlim.Find(slimExpr);
            Assert.AreEqual(1, free.Count);
            Assert.IsTrue(free.Contains(p1));
        }

        [TestMethod]
        public void FreeVariableScannerSlim_Find_Lambda2()
        {
            var p1 = ExpressionSlim.Parameter(SlimType, "p1");
            var p2 = ExpressionSlim.Parameter(SlimType, "p2");

            var slimExpr = ExpressionSlim.Lambda(ExpressionSlim.Add(p1, p2), p1);

            var free = FreeVariableScannerSlim.Find(slimExpr);
            Assert.AreEqual(1, free.Count);
            Assert.IsTrue(free.Contains(p2));
        }

        [TestMethod]
        public void FreeVariableScannerSlim_Find_Block1()
        {
            var p1 = ExpressionSlim.Parameter(SlimType, "p1");

            var slimExpr = ExpressionSlim.Block(p1);

            var free = FreeVariableScannerSlim.Find(slimExpr);
            Assert.AreEqual(1, free.Count);
            Assert.IsTrue(free.Contains(p1));
        }

        [TestMethod]
        public void FreeVariableScannerSlim_Find_Block2()
        {
            var p1 = ExpressionSlim.Parameter(SlimType, "p1");
            var p2 = ExpressionSlim.Parameter(SlimType, "p2");

            var slimExpr = ExpressionSlim.Block(new[] { p1 }, ExpressionSlim.Add(p1, p2));

            var free = FreeVariableScannerSlim.Find(slimExpr);
            Assert.AreEqual(1, free.Count);
            Assert.IsTrue(free.Contains(p2));
        }

        [TestMethod]
        public void FreeVariableScannerSlim_Find_CatchBlock1()
        {
            var p1 = ExpressionSlim.Parameter(SlimType, "p1");

            var slimExpr = ExpressionSlim.TryCatch(ExpressionSlim.Empty(), ExpressionSlim.Catch(SlimType, p1));

            var free = FreeVariableScannerSlim.Find(slimExpr);
            Assert.AreEqual(1, free.Count);
            Assert.IsTrue(free.Contains(p1));
        }

        [TestMethod]
        public void FreeVariableScannerSlim_Find_CatchBlock2()
        {
            var p1 = ExpressionSlim.Parameter(SlimType, "p1");
            var p2 = ExpressionSlim.Parameter(SlimType, "p2");

            var slimExpr = ExpressionSlim.TryCatch(ExpressionSlim.Empty(), ExpressionSlim.Catch(p1, ExpressionSlim.Add(p1, p2)));

            var free = FreeVariableScannerSlim.Find(slimExpr);
            Assert.AreEqual(1, free.Count);
            Assert.IsTrue(free.Contains(p2));
        }
    }
}
