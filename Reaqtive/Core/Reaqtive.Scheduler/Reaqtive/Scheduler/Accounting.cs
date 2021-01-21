// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Provides a set of extension methods that enable accounting for schedulers and tasks.
    /// </summary>
    internal static class Accounting
    {
        /// <summary>
        /// Starts an accounting operation for the specified object. If the specified object
        /// implements <see cref="IAccountable"/>, the cost of performing the measured operation
        /// will be charged to the object. Otherwise, the measurement is a no-op.
        /// </summary>
        /// <typeparam name="T">The type of the object to (try to) account.</typeparam>
        /// <param name="obj">The object to apply accounting to.</param>
        /// <returns>
        /// An accountant object which implements <see cref="IDisposable"/>. Upon calling this
        /// method, the accounting operation starts. In order to stop accounting and apply the
        /// measurement as a charge to the target object, call <see cref="Accountant.Dispose"/>.
        /// </returns>
        public static Accountant Account<T>(this T obj)
        {
            if (obj is IAccountable accountable)
            {
                return new Accountant(accountable);
            }

            return new Accountant();
        }

        /// <summary>
        /// Charges a timer tick upon expiration of a timer.
        /// </summary>
        /// <param name="scheduler">The scheduler to charge the timer tick to.</param>
        public static void ChargeTimerTick(this IScheduler scheduler)
        {
            if (scheduler is IAccountable accountable)
            {
                accountable.ChargeTimerTick();
            }
        }
    }
}
