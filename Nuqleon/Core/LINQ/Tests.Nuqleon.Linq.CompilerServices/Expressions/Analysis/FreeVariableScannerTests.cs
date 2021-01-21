// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class FreeVariableScannerTests
    {
        [TestMethod]
        public void FreeVariableScanner_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => FreeVariableScanner.Scan(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => FreeVariableScanner.HasFreeVariables(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void FreeVariableScanner_None()
        {
            var e = Expression.Constant(42);

            var res = FreeVariableScanner.Scan(e);
            Assert.AreEqual(0, res.Count());

            Assert.IsFalse(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_One()
        {
            var e = Expression.Parameter(typeof(int));

            var res = FreeVariableScanner.Scan(e);
            Assert.IsTrue(res.SequenceEqual(new[] { e }));

            Assert.IsTrue(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_Lambda()
        {
            var e = (Expression<Func<int, int>>)(x => x);

            var res = FreeVariableScanner.Scan(e);
            Assert.AreEqual(0, res.Count());

            Assert.IsFalse(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_Lambda_Nested()
        {
            var e = (Expression<Func<int, Func<int, int>>>)(x => y => x + y);

            var res = FreeVariableScanner.Scan(e);
            Assert.AreEqual(0, res.Count());

            Assert.IsFalse(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_Lambda_Unbound()
        {
            var l = (Expression<Func<int, Func<int, int>>>)(x => y => x + y);
            var e = l.Body;

            var res = FreeVariableScanner.Scan(e);
            Assert.IsTrue(new[] { l.Parameters[0] }.SequenceEqual(res));

            Assert.IsTrue(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_Block()
        {
            var p = Expression.Parameter(typeof(int));
            var e = Expression.Block(new[] { p }, p);

            var res = FreeVariableScanner.Scan(e);
            Assert.AreEqual(0, res.Count());

            Assert.IsFalse(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_Block_Unbound()
        {
            var p = Expression.Parameter(typeof(int));
            var q = Expression.Parameter(typeof(int));
            var b = Expression.Block(new[] { p }, Expression.Block(new[] { q }, Expression.Add(p, q)));
            var e = b.Expressions[0];

            var res = FreeVariableScanner.Scan(e);
            Assert.IsTrue(new[] { p }.SequenceEqual(res));

            Assert.IsTrue(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_NestedScopes()
        {
            var p = Expression.Parameter(typeof(int));
            var e = Expression.Lambda(Expression.Lambda(p, p), p);

            var res = FreeVariableScanner.Scan(e);
            Assert.AreEqual(0, res.Count());

            Assert.IsFalse(FreeVariableScanner.HasFreeVariables(e));
        }

        [TestMethod]
        public void FreeVariableScanner_CatchBlock()
        {
            var exMessage = (PropertyInfo)ReflectionHelpers.InfoOf((Exception ex) => ex.Message);
            var writeLine = (MethodInfo)ReflectionHelpers.InfoOf((string s) => Console.WriteLine(s));

            var p = Expression.Parameter(typeof(Exception));
            var e = Expression.TryCatch(Expression.Call(writeLine, Expression.Constant("Hello")), Expression.Catch(p, Expression.Call(writeLine, Expression.MakeMemberAccess(p, exMessage))));

            var res = FreeVariableScanner.Scan(e);
            Assert.AreEqual(0, res.Count());

            Assert.IsFalse(FreeVariableScanner.HasFreeVariables(e));
        }
    }
}
