// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.QueryCoordinator;
using Reaqtor.Remoting.QueryEvaluator;

namespace Reaqtor.Remoting.Platform
{
    public class AppDomainReactivePlatform<TQueryCoordinator, TQueryEvaluator> : ReactivePlatformBase
        where TQueryCoordinator : IRemotingReactiveServiceConnection, new()
        where TQueryEvaluator : IReactiveQueryEvaluatorConnection, new()
    {
        private const string QueryEvaluatorName = "reactor://qe1";

        private readonly AppDomainQueryCoordinator<TQueryCoordinator> _queryCoordinator;
        private readonly AppDomainQueryEvaluator<TQueryEvaluator> _queryEvaluator;
        private readonly bool _selfContainedEnvironment;

        public AppDomainReactivePlatform()
            : this(new AppDomainReactiveEnvironment())
        {
            _selfContainedEnvironment = true;
        }

        public AppDomainReactivePlatform(IReactiveEnvironment environment)
            : base(environment)
        {
            _queryCoordinator = new AppDomainQueryCoordinator<TQueryCoordinator>(this);
            _queryEvaluator = new AppDomainQueryEvaluator<TQueryEvaluator>(this, QueryEvaluatorName);
        }

        public override IReactiveQueryCoordinator QueryCoordinator => _queryCoordinator;

        public override IEnumerable<IReactiveQueryEvaluator> QueryEvaluators
        {
            get
            {
                yield return _queryEvaluator;
            }
        }

        public override async Task StartAsync(CancellationToken token)
        {
            if (_selfContainedEnvironment)
            {
                await Environment.StartAsync(CancellationToken.None);
            }

            await base.StartAsync(token);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && _selfContainedEnvironment)
            {
                Environment.Dispose();
            }
        }
    }

    public class AppDomainReactivePlatform : AppDomainReactivePlatform<QueryCoordinatorServiceConnection, QueryEvaluatorServiceConnection>
    {
        public AppDomainReactivePlatform()
        {
        }

        public AppDomainReactivePlatform(IReactiveEnvironment environment)
            : base(environment)
        {
        }
    }
}
