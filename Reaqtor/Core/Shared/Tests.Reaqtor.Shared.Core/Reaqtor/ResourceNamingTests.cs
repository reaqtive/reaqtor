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
    public class ResourceNamingTests
    {
        [TestMethod]
        public void ResourceNaming_ArgumentChecks()
        {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Makes the intent clear.)
            Assert.ThrowsException<ArgumentNullException>(() => ResourceNaming.GetThisReferenceExpression((object)null));
            Assert.ThrowsException<ArgumentNullException>(() => ResourceNaming.GetThisReferenceExpression((Type)null));
            Assert.ThrowsException<ArgumentNullException>(() => ResourceNaming.IsThisReferenceExpression(typeof(int), null));
#pragma warning restore IDE0004
        }

        [TestMethod]
        public void ResourceNaming_Simple()
        {
            var x = new object();
            var p = ResourceNaming.GetThisReferenceExpression(x) as ParameterExpression;
            Assert.IsNotNull(p);
            Assert.AreEqual(typeof(object), p.Type);
            Assert.AreEqual(Constants.CurrentInstanceUri, p.Name);

            var p2 = ResourceNaming.GetThisReferenceExpression(typeof(string)) as ParameterExpression;
            Assert.AreEqual(typeof(string), p2.Type);
            Assert.AreEqual(Constants.CurrentInstanceUri, p.Name);

            var p3 = Expression.Parameter(typeof(object), Constants.IdentityFunctionUri);
            Assert.IsFalse(ResourceNaming.IsThisReferenceExpression(typeof(object), p3));
            Assert.IsFalse(ResourceNaming.IsThisReferenceExpression(typeof(string), p3));

            var p4 = Expression.Parameter(typeof(string), Constants.CurrentInstanceUri);
            Assert.IsFalse(ResourceNaming.IsThisReferenceExpression(typeof(object), p4));
            Assert.IsTrue(ResourceNaming.IsThisReferenceExpression(typeof(string), p4));
        }
    }
}
