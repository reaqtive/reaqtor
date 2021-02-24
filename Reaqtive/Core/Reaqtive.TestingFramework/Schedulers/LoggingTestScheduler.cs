// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework
{
    public class LoggingTestScheduler : TestScheduler, ILoggingScheduler<long>
    {
        private readonly LoggingPhysicalTestScheduler _physicalScheduler;

        public LoggingTestScheduler()
            : this(new LoggingPhysicalTestScheduler())
        {
        }

        private LoggingTestScheduler(LoggingPhysicalTestScheduler physicalScheduler)
            : base(physicalScheduler)
        {
            _physicalScheduler = physicalScheduler;
        }

        private LoggingTestScheduler(LoggingTestScheduler parent)
            : base(parent)
        {
        }

        public override long Increment => 0L;

        public override IScheduler CreateChildScheduler()
        {
            return new LoggingTestScheduler(this);
        }

        public IEnumerable<long> ScheduledTimes => _physicalScheduler.ScheduledTimes;
    }
}
