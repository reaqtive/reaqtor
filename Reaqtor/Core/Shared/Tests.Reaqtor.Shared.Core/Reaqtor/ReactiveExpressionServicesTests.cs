// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.RegisterObject(null, Expression.Default(typeof(int))));
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.RegisterObject("foo", null));
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.TryGetObject(null, out expression));
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.Normalize(null));
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.GetNamedExpression(null, new Uri("eg:/foo")));
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.GetNamedExpression(typeof(int), null));
            Assert.ThrowsException<ArgumentNullException>(() => expressionServices.TryGetName(null, out uri));
        }

        private interface ITestClient
        {
        }
    }
}
