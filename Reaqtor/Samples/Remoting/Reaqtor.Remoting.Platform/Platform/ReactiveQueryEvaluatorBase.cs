// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using Reaqtive.Scheduler;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ReactiveQueryEvaluatorBase : ReactivePlatformServiceBase, IReactiveQueryEvaluator
    {
        protected ReactiveQueryEvaluatorBase(IReactivePlatform platform, IRunnable runnable, ReactiveServiceType serviceType)
            : base(platform, runnable, serviceType)
        {
        }

        public IScheduler Scheduler => GetInstance<IReactiveQueryEvaluatorConnection>().Scheduler;

        public void Checkpoint()
        {
            GetInstance<IReactiveQueryEvaluatorConnection>().Checkpoint();
        }

        public void Unload()
        {
            GetInstance<IReactiveQueryEvaluatorConnection>().Unload();
        }

        public void Recover()
        {
            GetInstance<IReactiveQueryEvaluatorConnection>().Recover();
        }
    }
}
