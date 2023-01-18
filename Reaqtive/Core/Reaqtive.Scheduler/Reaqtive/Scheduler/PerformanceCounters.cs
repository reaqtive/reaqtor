// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents the performance counters associated with a scheduler object.
    /// </summary>
    internal sealed class PerformanceCounters : IDisposable
    {
        /// <summary>
        /// The <see cref="Stopwatch"/> used to accumulate pause times.
        /// </summary>
        private readonly Stopwatch _paused = new();

        /// <summary>
        /// If positive, the time in <see cref="Stopwatch"/> ticks when the scheduler was started;
        /// if negative, the negative representation of the total uptime of the scheduler.
        /// </summary>
        /// <remarks>
        /// This field gets overwritten by the <see cref="Dispose"/> method when the counters are
        /// disposed, using the negative representation of the total uptime of the scheduler. This
        /// prevents retrieval of the counters after disposing a scheduler from exposing a run-away
        /// uptime value.
        /// </remarks>
        private long _startTime = Stopwatch.GetTimestamp();

        /// <summary>
        /// The <see cref="SchedulerPerformanceCounters"/> value containing the most recent
        /// measurements of various counters. A copy of this value is returned and augmented
        /// in the <see cref="GetCounters"/> method.
        /// </summary>
        private SchedulerPerformanceCounters _counters = new();

        /// <summary>
        /// Gets the current value of the performance counters.
        /// </summary>
        /// <param name="isTopLevel">
        /// Indicates whether the counters are being obtained for a root scheduler in a scheduler
        /// tree. If set to <c>true</c>, the returned counters will include information about
        /// uptime and paused time; if set to <c>false</c>, these values will be reported as zero.
        /// By using this parameter, a caller can ensure that the sum of counter values does is
        /// not subject to double-counting because the lifetime and lifecycle management of a
        /// parent scheduler affects all of its children.
        /// </param>
        /// <returns></returns>
        public SchedulerPerformanceCounters GetCounters(bool isTopLevel)
        {
            var res = _counters;

            if (isTopLevel)
            {
                res._uptime = _startTime < 0 ? -_startTime : (Stopwatch.GetTimestamp() - _startTime);
                res._pausedTime = _paused.ElapsedTicks;
            }

            return res;
        }

        /// <summary>
        /// Charges the execution of a task.
        /// </summary>
        /// <param name="cycles">The number of thread cycles to charge.</param>
        /// <param name="ticks">The number of <see cref="Stopwatch"/> ticks to charge.</param>
        public void ChargeTaskExecution(ulong cycles, long ticks)
        {
            Interlocked.Increment(ref _counters._taskExecutionCount);

            ChargeUser(cycles, ticks);
        }

        /// <summary>
        /// Charges a single timer tick upon a timer task being transferred to the ready queue.
        /// </summary>
        public void ChargeTimerTick() => Interlocked.Increment(ref _counters._timerTickCount);

        /// <summary>
        /// Marks the begin of scheduler being in the paused state. This method is used to
        /// calculate the total time the scheduler is paused.
        /// </summary>
        public void MarkPauseBegin() => _paused.Start();

        /// <summary>
        /// Marks the end of a scheduler being in the paused state, thus resuming execution.
        /// This method is used to calculate the total time the scheduler is paused.
        /// </summary>
        public void MarkContinueEnd() => _paused.Stop();

        /// <summary>
        /// Creates an accountant to measure time spent performing scheduler infrastructure work.
        /// </summary>
        /// <returns>
        /// An accountant object which implements <see cref="IDisposable"/>. Upon calling this
        /// method, the accounting operation starts. In order to stop accounting and apply the
        /// measurement as a charge to the target object, call <see cref="IDisposable.Dispose"/>.
        /// </returns>
        public KernelAccountant AccountKernel() => new(this);

        /// <summary>
        /// Stops the collection of counters, causing values to be frozen.
        /// </summary>
        public void Dispose()
        {
            if (_startTime >= 0)
            {
                _startTime = -(Stopwatch.GetTimestamp() - _startTime);
            }
        }

        /// <summary>
        /// Charges the specified cycle count and tick count to user time.
        /// </summary>
        /// <param name="cycles">The number of thread cycles to charge.</param>
        /// <param name="ticks">The number of <see cref="Stopwatch"/> ticks to charge.</param>
        private void ChargeUser(ulong cycles, long ticks) => ChargeDuration(ref _counters._userTime, cycles, ticks);

        /// <summary>
        /// Charges the specified cycle count and tick count to kernel time.
        /// </summary>
        /// <param name="cycles">The number of thread cycles to charge.</param>
        /// <param name="ticks">The number of <see cref="Stopwatch"/> ticks to charge.</param>
        private void ChargeKernel(ulong cycles, long ticks) => ChargeDuration(ref _counters._kernelTime, cycles, ticks);

        /// <summary>
        /// Charges the specified cycle count and tick count to the specified duration.
        /// </summary>
        /// <param name="duration">The duration to charge to.</param>
        /// <param name="cycles">The number of thread cycles to charge.</param>
        /// <param name="ticks">The number of <see cref="Stopwatch"/> ticks to charge.</param>
        private static void ChargeDuration(ref Duration duration, ulong cycles, long ticks)
        {
            Interlocked.Add(ref duration._cycleTime, (long)cycles);
            Interlocked.Add(ref duration._tickCount, ticks);
        }

        //
        // NB: This type is very similar to Accountant, which uses an interface to be able to report
        //     measurements to an accountable object. For measurements of scheduler infrastructure
        //     activity, we prefer to do away with an interface virtual call indirection, so the kernel
        //     accountant is a specialized implementation.
        //

        /// <summary>
        /// Accountant for scheduler infrastructure activity.
        /// </summary>
        public readonly struct KernelAccountant : IDisposable
        {
            /// <summary>
            /// The parent performance counters object to report the measurement to.
            /// </summary>
            private readonly PerformanceCounters _parent;

            /// <summary>
            /// The total number of thread cycles measured.
            /// </summary>
            private readonly ulong _cycles;

            /// <summary>
            /// The total number of <see cref="Stopwatch"/> ticks measured.
            /// </summary>
            private readonly long _start;

            /// <summary>
            /// Creates a new kernel accountant reporting its measurement to the specified <paramref name="parent"/>.
            /// </summary>
            /// <param name="parent">The parent performance counters object to report the measurement to.</param>
            public KernelAccountant(PerformanceCounters parent)
            {
                _parent = parent;

                if (!NativeMethods.TryGetThreadCycleTime(out _cycles))
                {
                    _cycles = 0UL;
                }

                _start = Stopwatch.GetTimestamp();
            }

            /// <summary>
            /// Stops the accounting operation and reports the measurement.
            /// </summary>
            public void Dispose()
            {
                var stop = Stopwatch.GetTimestamp();

                if (!NativeMethods.TryGetThreadCycleTime(out var cycles))
                {
                    cycles = 0;
                }

                _parent.ChargeKernel(cycles - _cycles, stop - _start);
            }
        }
    }
}
