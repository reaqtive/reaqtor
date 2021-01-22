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

namespace Reaqtor.Remoting.Platform
{
    public sealed class TcpReactivePlatform : ReactivePlatformBase
    {
        private readonly TcpQueryCoordinator _queryCoordinator;
        private readonly TcpQueryEvaluator _queryEvaluator;
        private readonly bool _selfContainedEnvironment;

        public TcpReactivePlatform()
            : this(new TcpReactivePlatformSettings())
        {
        }

        public TcpReactivePlatform(ITcpReactivePlatformSettings settings)
            : this(settings, new TcpReactiveEnvironment())
        {
            _selfContainedEnvironment = true;
            Environment.StartAsync(CancellationToken.None).Wait();
        }

        public TcpReactivePlatform(IReactiveEnvironment environment)
            : this(new TcpReactivePlatformSettings(), environment)
        {
        }

        public TcpReactivePlatform(ITcpReactivePlatformSettings settings, IReactiveEnvironment environment)
            : base(environment)
        {
            _queryEvaluator = new TcpQueryEvaluator(this, settings.GetExecutablePath("QueryEvaluatorHost"), settings.QueryEvaluatorPort, settings.QueryEvaluatorUri);
            _queryCoordinator = new TcpQueryCoordinator(this, settings.GetExecutablePath("QueryCoordinatorHost"), settings.QueryCoordinatorPort, settings.QueryCoordinatorUri);
        }

        public override IReactiveQueryCoordinator QueryCoordinator => _queryCoordinator;

        public override IEnumerable<IReactiveQueryEvaluator> QueryEvaluators
        {
            get
            {
                yield return _queryEvaluator;
            }
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
}
