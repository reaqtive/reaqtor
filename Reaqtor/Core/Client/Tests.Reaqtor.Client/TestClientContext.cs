﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Linq.Expressions;

using Reaqtor;

namespace Tests.Reaqtor.Client
{
    internal class TestClientContext : ReactiveClientContext
    {
        public TestClientContext(IReactiveServiceProvider serviceProvider)
            : base(new TestExpressionServices(), serviceProvider)
        {
        }

        private sealed class TestExpressionServices : ReactiveExpressionServices
        {
            public TestExpressionServices()
                : base(typeof(IReactiveClientProxy))
            {
            }

            public override Expression Normalize(Expression expression) => base.Normalize(expression);
        }
    }
}
