// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Engine;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Reliable.Service
{
    public class ReliableReactiveClient : ReliableReactiveClientBase
    {
        public ReliableReactiveClient(IReliableReactiveClientEngineProvider provider, IReactiveExpressionServices expressionServices)
            : base(expressionServices)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            Provider = new ReliableQueryProvider(provider, expressionServices);

            // TODO: Re-enable this.
            //var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            //expressionServices.RegisterObject(this, thisParameter);
            //expressionServices.RegisterObject(_provider, Expression.Property(thisParameter, (PropertyInfo)ReflectionHelpers.InfoOf((IReactiveClient c) => c.Provider)));
        }

        public override IReliableQueryProvider Provider { get; }
    }
}
