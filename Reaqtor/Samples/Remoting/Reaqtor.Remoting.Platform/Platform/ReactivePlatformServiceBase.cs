// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ReactivePlatformServiceBase : ReactiveServiceBase, IReactivePlatformService
    {
        protected ReactivePlatformServiceBase(IReactivePlatform platform, IRunnable runnable, ReactiveServiceType serviceType)
            : base(runnable, serviceType)
        {
            Platform = platform;
        }

        public IReactivePlatform Platform { get; }

        public override async Task StartAsync(CancellationToken token)
        {
            await base.StartAsync(token);
            var instance = GetInstance<IRemotingReactiveServiceConnection>();
            instance.Configure(Platform.Configuration);
            instance.Start();
        }

        public override Task StopAsync(CancellationToken token)
        {
            using (GetInstance<IRemotingReactiveServiceConnection>()) { }
            return base.StopAsync(token);
        }
    }
}
