// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class InMemoryQueryEvaluator<TQueryEvaluator> : ReactiveQueryEvaluatorBase, IReactiveQueryEvaluator
        where TQueryEvaluator : IReactiveQueryEvaluatorConnection, new()
    {
        public InMemoryQueryEvaluator(IReactivePlatform platform)
            : base(platform, new InMemoryRunnable(CreateQueryEvaluator), ReactiveServiceType.QueryEvaluator)
        {
        }

        private static Task<object> CreateQueryEvaluator(CancellationToken token)
        {
            return Task.FromResult<object>(new TQueryEvaluator());
        }
    }
}
