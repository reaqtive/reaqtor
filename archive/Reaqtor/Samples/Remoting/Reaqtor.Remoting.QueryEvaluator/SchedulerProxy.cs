// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Reaqtive.Scheduler;
using Reaqtive.TestingFramework;

namespace Reaqtor.Remoting.QueryEvaluator
{
    public sealed class SchedulerProxy : MarshalByRefObject, ITestScheduler, ILoggingScheduler<long>, ISchedulerExceptionHandler
    {
        private readonly IScheduler _logicalScheduler;
        private readonly IDisposable _terminator;

        public SchedulerProxy(IScheduler logicalScheduler, IDisposable terminator)
            : this(logicalScheduler)
        {
            _terminator = terminator;
        }

        public SchedulerProxy(IScheduler logicalScheduler)
        {
            _logicalScheduler = logicalScheduler;
        }

        #region ITestScheduler

        public void ScheduleAbsolute(long dueTime, ISchedulerTask task)
        {
            ((TestScheduler)_logicalScheduler).ScheduleAbsolute(dueTime, task);
        }

        public void Start()
        {
            ((TestScheduler)_logicalScheduler).Start();
        }

        public long Clock => ((TestScheduler)_logicalScheduler).Clock;

        #endregion

        #region ILoggingScheduler<long>

        public IEnumerable<long> ScheduledTimes => ((ILoggingScheduler<long>)_logicalScheduler).ScheduledTimes;

        #endregion

        public DateTimeOffset Now => _logicalScheduler.Now;

        public IScheduler CreateChildScheduler()
        {
            return new SchedulerProxy(_logicalScheduler.CreateChildScheduler(), terminator: null);
        }

        public void Schedule(ISchedulerTask task)
        {
            _logicalScheduler.Schedule(task);
        }

        public void Schedule(TimeSpan dueTime, ISchedulerTask task)
        {
            _logicalScheduler.Schedule(dueTime, task);
        }

        public void Schedule(DateTimeOffset dueTime, ISchedulerTask task)
        {
            _logicalScheduler.Schedule(dueTime, task);
        }

        public Task PauseAsync()
        {
            return _logicalScheduler.PauseAsync();
        }

        public void Continue()
        {
            _logicalScheduler.Continue();
        }

        public void RecalculatePriority()
        {
            _logicalScheduler.RecalculatePriority();
        }

        public void Dispose()
        {
            if (_terminator != null)
            {
                _logicalScheduler.PauseAsync().Wait();
            }

            _logicalScheduler.Dispose();

            _terminator?.Dispose();
        }

        public bool CheckAccess()
        {
            return _logicalScheduler.CheckAccess();
        }

        public void VerifyAccess()
        {
            _logicalScheduler.VerifyAccess();
        }

        public event EventHandler<SchedulerUnhandledExceptionEventArgs> UnhandledException;

        bool ISchedulerExceptionHandler.TryCatch(Exception error, IWorkItem task)
        {
            var handler = UnhandledException;
            if (handler != null)
            {
                var e = new SchedulerUnhandledExceptionEventArgs(task.Scheduler, error);
                handler(this, e);
                return e.Handled;
            }

            return false;
        }
    }
}
