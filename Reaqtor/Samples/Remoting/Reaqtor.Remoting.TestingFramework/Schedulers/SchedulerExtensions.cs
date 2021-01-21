// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Threading.Tasks;

using Reaqtive.Scheduler;
using Reaqtive.TestingFramework;

using Reaqtor.Remoting.Client;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public static class SchedulerExtensions
    {
        public static void ScheduleAbsolute(this ITestScheduler scheduler, long dueTime, Action action)
        {
            scheduler.ScheduleAbsolute(dueTime, new RemoteSchedulerTask(new ClientAction(action)));
        }

        public static void ScheduleAbsolute(this ITestScheduler scheduler, long dueTime, Func<Task> asyncAction)
        {
            scheduler.ScheduleAbsolute(dueTime, new RemoteSchedulerTask(new AsyncClientAction(asyncAction)));
        }

        public static void Schedule(this IScheduler scheduler, Action action)
        {
            scheduler.Schedule(new RemoteSchedulerTask(new ClientAction(action)));
        }

        public static void Schedule(this IScheduler scheduler, Func<Task> asyncAction)
        {
            scheduler.Schedule(new RemoteSchedulerTask(new AsyncClientAction(asyncAction)));
        }

        public static void Schedule(this IScheduler scheduler, TimeSpan dueTime, Action action)
        {
            scheduler.Schedule(dueTime, new RemoteSchedulerTask(new ClientAction(action)));
        }

        public static void Schedule(this IScheduler scheduler, TimeSpan dueTime, Func<Task> asyncAction)
        {
            scheduler.Schedule(dueTime, new RemoteSchedulerTask(new AsyncClientAction(asyncAction)));
        }

        public static void Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
        {
            scheduler.Schedule(dueTime, new RemoteSchedulerTask(new ClientAction(action)));
        }

        public static void Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Func<Task> asyncAction)
        {
            scheduler.Schedule(dueTime, new RemoteSchedulerTask(new AsyncClientAction(asyncAction)));
        }
    }
}
