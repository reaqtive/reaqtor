// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Reaqtive.Scheduler;

namespace Reaqtor.Remoting.Protocol
{
    // NB (plan §3.4): the archived RemoteSchedulerTask was [Serializable] so it could be marshaled across the
    //     .NET Remoting boundary; the scheduler now runs fully in-process (no marshaling), so the attribute (and
    //     its now-unused System using) is removed. The Priority member and the comment above it are kept
    //     byte-for-byte: the glitching tests' execution order depends on it.
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
