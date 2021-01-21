// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.QueryCoordinator;

namespace Reaqtor.Remoting.ReificationFramework
{
    public sealed class ReactiveServiceConnectionEnvironment : IDisposable
    {
        private readonly InMemoryReactivePlatform<QueryCoordinatorServiceConnection, ConfiguredQueryEvaluatorServiceConnection> _platform;

        public ReactiveServiceConnectionEnvironment()
        {
            _platform = new InMemoryReactivePlatform<QueryCoordinatorServiceConnection, ConfiguredQueryEvaluatorServiceConnection>();
            _platform.StartAsync(CancellationToken.None).Wait();
            new ReactivePlatformDeployer(_platform, new Deployable.Deployable()).Deploy();
        }

        public IReactiveServiceConnection QueryCoordinator => _platform.QueryCoordinator.GetInstance<IReactiveServiceConnection>();

        public IReactiveServiceConnection QueryEvaluator => _platform.QueryEvaluators.First().GetInstance<IReactiveServiceConnection>();

        public void DifferentialCheckpoint(Uri uri)
        {
            _ = uri; // TODO: could use the URI to determine which QE to checkpoint

            _platform.QueryEvaluators.First().Checkpoint();
        }

        public void FullCheckpoint(Uri uri)
        {
            _ = this; // NB: Suppress CA1822
            _ = uri; // TODO: could use the URI to determine which QE to checkpoint

            throw new NotImplementedException();
        }

        public void Recover(Uri uri)
        {
            _ = uri; // TODO: could use the URI to determine which QE to checkpoint

            _platform.QueryEvaluators.First().Unload();
            _platform.QueryEvaluators.First().Recover();
        }

        public void Dispose()
        {
            _platform.Dispose();
        }
    }
}
