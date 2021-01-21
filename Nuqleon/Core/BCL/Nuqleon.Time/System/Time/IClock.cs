// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

namespace System.Time
{
    /// <summary>
    /// Interface for clocks.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current time in ticks.
        /// </summary>
        long Now { get; }
    }
}
