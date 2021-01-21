// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

using Reaqtive.Scheduler;

namespace Reaqtor.Remoting.Protocol
{
    [Serializable]
    public class RemoteSchedulerTask : ISchedulerTask
    {
        private readonly IRemoteAction _action;

        public RemoteSchedulerTask(IRemoteAction action)
        {
            _action = action;
        }

        // Don't change this for now please. If you do, it changes
        // the order of execution for the timeline observables 
        // used to implement the glitching tests. Namely that it
        // would allow timers that are scheduled to fire at the
        // same time as "hot" or "cold" observable events to fire
        // before the "hot" or "cold" event. (Desired behavior is 
        // for the "hot" or "cold" subscribables to be the first
        // to fire at a given scheduler increment.)

        public long Priority => 0; // 1;  // See remark above.

        public bool IsRunnable => true;

        public bool Execute(IScheduler scheduler)
        {
            _action.Invoke();
            return true;
        }

        public void RecalculatePriority()
        {
        }
    }
}
