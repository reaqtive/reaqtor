// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Scheduler
{
    //
    // NB: This type is very similar to KernelAccountant, but uses an interface to be able to report
    //     measurements to an accountable object. For measurements of scheduler infrastructure
    //     activity, we prefer to do away with an interface virtual call indirection, so the kernel
    //     accountant is a specialized implementation.
    //

    /// <summary>
    /// Accountant for execution of user code.
    /// </summary>
    internal readonly struct Accountant : IDisposable
    {
        /// <summary>
        /// The accountable object to report the measurement to.
        /// </summary>
        private readonly IAccountable _accountable;

        /// <summary>
        /// The total number of thread cycles measured.
        /// </summary>
        private readonly ulong _cycles;

        /// <summary>
        /// The total number of <see cref="Stopwatch"/> ticks measured.
        /// </summary>
        private readonly long _start;

        /// <summary>
        /// Creates a new accountant reporting its measurement to the specified <paramref name="accountable"/> object.
        /// </summary>
        /// <param name="accountable">The accountable object to report the measurement to.</param>
        public Accountant(IAccountable accountable)
        {
            _accountable = accountable;

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
            if (_accountable != null)
            {
                var stop = Stopwatch.GetTimestamp();

                if (!NativeMethods.TryGetThreadCycleTime(out var cycles))
                {
                    cycles = 0;
                }

                _accountable.ChargeTaskExecution(cycles - _cycles, stop - _start);
            }
        }
    }
}
