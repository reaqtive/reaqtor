// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtive.Expressions;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ExpressionPolicyTests
    {
        [TestMethod]
        public void ExpressionPolicy_PropertyChecks()
        {
            var policy = new ExpressionPolicy();

            Assert.AreSame(DefaultExpressionPolicy.Instance.DelegateCache, policy.DelegateCache);
            Assert.AreSame(DefaultExpressionPolicy.Instance.InMemoryCache, policy.InMemoryCache);
            Assert.AreSame(DefaultExpressionPolicy.Instance.ConstantHoister, policy.ConstantHoister);

            var delegateCache = new SimpleCompiledDelegateCache();
            var expressionCache = new ExpressionHeap();
            var constantHoister = ConstantHoister.Create(false);

            policy.DelegateCache = delegateCache;
            policy.InMemoryCache = expressionCache;
            policy.ConstantHoister = constantHoister;

            Assert.AreSame(delegateCache, policy.DelegateCache);
            Assert.AreSame(expressionCache, policy.InMemoryCache);
            Assert.AreSame(constantHoister, policy.ConstantHoister);

            policy.DelegateCache = null;
            policy.InMemoryCache = null;
            policy.ConstantHoister = null;

            Assert.AreSame(DefaultExpressionPolicy.Instance.DelegateCache, policy.DelegateCache);
            Assert.AreSame(DefaultExpressionPolicy.Instance.InMemoryCache, policy.InMemoryCache);
            Assert.AreSame(DefaultExpressionPolicy.Instance.ConstantHoister, policy.ConstantHoister);
        }
    }
}
