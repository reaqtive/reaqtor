// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System
{

    namespace Diagnostics
    {
        /// <summary>
        /// Interface representing a stopwatch.
        /// </summary>
        public interface IStopwatch
        {
            /// <summary>
            /// Starts the stopwatch.
            /// </summary>
            void Start();

            /// <summary>
            /// Restarts the stopwatch.
            /// </summary>
            void Restart();

            /// <summary>
            /// Gets the time elapsed since the stopwatch was started.
            /// </summary>
            TimeSpan Elapsed { get; }
        }
    }
}
