// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Linq.Expressions;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class ReactiveQbserverTests
    {
        [TestMethod]
        public void ReactiveQbserver_ElementType()
        {
            var q = new MyReactiveQbserver(null);
            Assert.AreEqual(typeof(int), q.ElementType);
        }

        private sealed class MyReactiveQbserver : ReactiveQbserverBase<int>
        {
            public MyReactiveQbserver(IReactiveQueryProvider provider)
                : base(provider)
            {
            }

            public override Expression Expression => throw new NotImplementedException();

            protected override void OnNextCore(int value)
            {
                throw new NotImplementedException();
            }

            protected override void OnErrorCore(Exception error)
            {
                throw new NotImplementedException();
            }

            protected override void OnCompletedCore()
            {
                throw new NotImplementedException();
            }
        }
    }
}
