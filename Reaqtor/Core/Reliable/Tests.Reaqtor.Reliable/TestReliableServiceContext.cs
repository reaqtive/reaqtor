// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using System.Linq.Expressions;
using Reaqtor;
using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Expressions;
using Reaqtor.Reliable.Service;

namespace Tests.Reaqtor.Reliable
{
    internal class TestReliableServiceContext : ReliableReactiveServiceContext
    {
        public TestReliableServiceContext(IReliableReactiveEngineProvider serviceProvider)
            : base(new TestExpressionServices(), serviceProvider)
        {
        }

        private sealed class TestExpressionServices : ReactiveExpressionServices
        {
            public TestExpressionServices()
                : base(typeof(IReliableReactiveClient))
            {
            }

            public override Expression Normalize(Expression expression) => base.Normalize(expression);
        }
    }
}
