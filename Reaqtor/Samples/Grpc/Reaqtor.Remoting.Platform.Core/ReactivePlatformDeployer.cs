// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Platform
{
    public class ReactivePlatformDeployer
    {
        private readonly IReactivePlatform _platform;
        private readonly IDeployable[] _deployables;

        public ReactivePlatformDeployer(IReactivePlatform platform, params IDeployable[] deployables)
        {
            _platform = platform;
            _deployables = deployables;
        }

        public void Deploy()
        {
            foreach (var deployable in _deployables)
            {
                deployable.Execute(_platform.CreateClient().Context);
            }
        }
    }
}
