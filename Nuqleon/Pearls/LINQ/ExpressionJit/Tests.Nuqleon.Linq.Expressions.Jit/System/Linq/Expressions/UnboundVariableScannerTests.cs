// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Tests
{
    [TestClass]
    public class UnboundVariableScannerTests
    {
        [TestMethod]
        public void Bound_Lambda()
        {
            var uvs = new UnboundVariableScanner();

            var e = (Expression<Func<int, int>>)(x => x);

            uvs.Visit(e);

            Assert.AreEqual(0, uvs.UnboundVariables.Count);
        }

        [TestMethod]
        public void Bound_Block()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(int));
            var e = Expression.Block(new[] { p }, p);

            uvs.Visit(e);

            Assert.AreEqual(0, uvs.UnboundVariables.Count);
        }

        [TestMethod]
        public void Bound_Catch()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(Exception));
            var e = Expression.TryCatch(Expression.Empty(), Expression.Catch(p, Expression.Empty()));

            uvs.Visit(e);

            Assert.AreEqual(0, uvs.UnboundVariables.Count);
        }

        [TestMethod]
        public void Unbound_Parameter()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(int));

            uvs.Visit(p);

            Assert.IsTrue(uvs.UnboundVariables.SetEquals(new[] { p }));
        }

        [TestMethod]
        public void Unbound_Lambda()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(int));
            var e = Expression.Lambda(p);

            uvs.Visit(e);

            Assert.IsTrue(uvs.UnboundVariables.SetEquals(new[] { p }));
        }

        [TestMethod]
        public void Unbound_Block()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(int));
            var e = Expression.Block(p);

            uvs.Visit(e);

            Assert.IsTrue(uvs.UnboundVariables.SetEquals(new[] { p }));
        }

        [TestMethod]
        public void Unbound_Catch_Body()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(int));
            var e = Expression.TryCatch(Expression.Default(typeof(int)), Expression.Catch(typeof(Exception), p));

            uvs.Visit(e);

            Assert.IsTrue(uvs.UnboundVariables.SetEquals(new[] { p }));
        }

        [TestMethod]
        public void Unbound_Catch_Filter()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(bool));
            var e = Expression.TryCatch(Expression.Empty(), Expression.Catch(typeof(Exception), Expression.Empty(), p));

            uvs.Visit(e);

            Assert.IsTrue(uvs.UnboundVariables.SetEquals(new[] { p }));
        }

        [TestMethod]
        public void Unbound_Nested()
        {
            var uvs = new UnboundVariableScanner();

            var p = Expression.Parameter(typeof(int));
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(Exception));
            var e = Expression.Lambda(Expression.Block(new[] { x }, Expression.TryCatch(Expression.Default(typeof(int)), Expression.Catch(z, Expression.Add(x, y))), p), y);

            uvs.Visit(e);

            Assert.IsTrue(uvs.UnboundVariables.SetEquals(new[] { p }));
        }
    }
}
