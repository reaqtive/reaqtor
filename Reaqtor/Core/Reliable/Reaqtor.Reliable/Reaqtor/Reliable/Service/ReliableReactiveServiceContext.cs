// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public abstract class ReliableReactiveServiceContext : ReliableReactiveServiceContextBase
    {
        protected ReliableReactiveServiceContext(IReactiveExpressionServices expressionServices, IReliableReactiveEngineProvider engineProvider)
            : this(expressionServices, engineProvider, null)
        {
        }

        // TODO: Add the definition and metadata and remove the dummy object param.
        protected ReliableReactiveServiceContext(IReactiveExpressionServices expressionServices, IReliableReactiveClientEngineProvider clientEngineProvider, object removeme)
            : this(expressionServices, new ReliableReactiveClient(clientEngineProvider, expressionServices))
        {
            _ = removeme; // See remark above.
        }

        protected ReliableReactiveServiceContext(IReactiveExpressionServices expressionServices, ReliableReactiveClient client)
        {
            if (expressionServices == null)
                throw new ArgumentNullException(nameof(expressionServices));

            Client = client ?? throw new ArgumentNullException(nameof(client));

            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
        }

        protected override IReliableReactiveClient Client { get; }
    }
}
