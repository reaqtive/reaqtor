// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests.Reaqtor.Shared.Core.Reaqtor
{
    [TestClass]
    public class ReactiveExpressionServicesTests
    {
        [TestMethod]
        public void ReactiveExpressionServices_ArgumentChecking()
        {
            var expression = default(Expression);
            var uri = default(Uri);
            var expressionServices = new ReactiveExpressionServices(typeof(ITestClient));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.RegisterObject(null, Expression.Default(typeof(int))));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.RegisterObject("foo", null));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.TryGetObject(null, out expression));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.Normalize(null));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.GetNamedExpression(null, new Uri("eg:/foo")));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.GetNamedExpression(typeof(int), null));
            Assert.ThrowsExactly<ArgumentNullException>(() => expressionServices.TryGetName(null, out uri));
        }

        private interface ITestClient
        {
        }
    }
}
