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
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Expressions.Core
{
    [TestClass]
    public class AsyncReactiveQbserverTests
    {
        [TestMethod]
        public void AsyncReactiveQbserver_ElementType()
        {
            var q = new MyAsyncReactiveQbserver(null);
            Assert.AreEqual(typeof(int), q.ElementType);
        }

        private sealed class MyAsyncReactiveQbserver : AsyncReactiveQbserverBase<int>
        {
            public MyAsyncReactiveQbserver(IAsyncReactiveQueryProvider provider)
                : base(provider)
            {
            }

            public override Expression Expression => throw new NotImplementedException();

            protected override Task OnNextAsyncCore(int value, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task OnErrorAsyncCore(Exception error, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task OnCompletedAsyncCore(CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
