// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;
using Reaqtor.Remoting.Client;

namespace Tests.Reaqtor.Remoting.Client
{
    [TestClass]
    public class DetupletizingExpressionServicesTests
    {
        [TestMethod]
        public void DetupletizingExpressionServices_OneWay_NotDetupletized()
        {
            var expr = Expression.Invoke(Expression.Parameter(typeof(Func<List<int>, int>)), Expression.New(typeof(List<int>)));
            var detupletizer = new DetupletizingExpressionServices(typeof(IReactiveClientProxy));
            var actual = detupletizer.Normalize(expr);
            AssertEqual(expr, actual);
        }

        [TestMethod]
        public void DetupletizingExpressionServices_OneWay_Lambda()
        {
            Expression<Func<int, int>> expected = x => x;
            Expression<Func<Tuple<int>, int>> expr = t => t.Item1;
            var detupletizer = new DetupletizingExpressionServices(typeof(IReactiveClientProxy));
            var actual = detupletizer.Normalize(expr);
            AssertEqual(expected, actual);
        }

        [TestMethod]
        public void DetupletizingExpressionServices_OneWay_Invocation()
        {
            var expected = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(42));
            var ctor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Tuple<int>(default));
            var expr = Expression.Invoke(Expression.Parameter(typeof(Func<Tuple<int>, int>)), Expression.New(ctor, Expression.Constant(42)));
            var detupletizer = new DetupletizingExpressionServices(typeof(IReactiveClientProxy));
            var actual = detupletizer.Normalize(expr);
            AssertEqual(expected, actual);
        }

        [TestMethod]
        public void DetupletizingExpressionServices_Roundtrip_Lambda()
        {
            Expression<Func<int, int>> expr = x => x;
            var tupletizer = new TupletizingExpressionServices(typeof(IReactiveClientProxy));
            var normalized = tupletizer.Normalize(expr);
            AssertNotEqual(expr, normalized);
            var detupletizer = new DetupletizingExpressionServices(typeof(IReactiveClientProxy));
            var rt = detupletizer.Normalize(normalized);
            AssertEqual(expr, rt);
        }

        [TestMethod]
        public void DetupletizingExpressionServices_Roundtrip_Invocation()
        {
            var expr = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>)), Expression.Constant(42));
            var tupletizer = new TupletizingExpressionServices(typeof(IReactiveClientProxy));
            var normalized = tupletizer.Normalize(expr);
            AssertNotEqual(expr, normalized);
            var detupletizer = new DetupletizingExpressionServices(typeof(IReactiveClientProxy));
            var rt = detupletizer.Normalize(normalized);
            AssertEqual(expr, rt);
        }

        private static void AssertEqual(Expression x, Expression y)
        {
            Assert.IsTrue(new ExpressionEqualityComparer(() => new Comparator()).Equals(x, y));
        }

        private static void AssertNotEqual(Expression x, Expression y)
        {
            Assert.IsFalse(new ExpressionEqualityComparer(() => new Comparator()).Equals(x, y));
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
    }
}
