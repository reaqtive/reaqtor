// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Provides access to a set of native methods.
    /// </summary>
    internal static class NativeMethods
    {
        private static bool s_failedToLoadKernel32 = false;

        /// <summary>
        /// Tries to get the cycle time for the current thread.
        /// </summary>
        /// <param name="cycleTime">The number of CPU clock cycles used by the thread. This value includes cycles spent in both user mode and kernel mode.</param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>.</returns>
        public static bool TryGetThreadCycleTime(out ulong cycleTime)
        {
            if (!s_failedToLoadKernel32)
            {
                try
                {
                    return QueryThreadCycleTime(CurrentThreadHandle, out cycleTime);
                }
                catch (DllNotFoundException)
                {
                    s_failedToLoadKernel32 = true;
                }
                catch (EntryPointNotFoundException)
                {
                    s_failedToLoadKernel32 = true;
                }
            }

            cycleTime = 0UL;
            return false;
        }

        /// <summary>
        /// Retrieves the cycle time for the specified thread.
        /// </summary>
        /// <param name="threadHandle">A handle to the thread.</param>
        /// <param name="cycleTime">The number of CPU clock cycles used by the thread. This value includes cycles spent in both user mode and kernel mode.</param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryThreadCycleTime(IntPtr threadHandle, out ulong cycleTime);

        /// <summary>
        /// A pseudo-handle representing the current thread.
        /// </summary>
        private static readonly IntPtr CurrentThreadHandle = new(-2);
    }
}
