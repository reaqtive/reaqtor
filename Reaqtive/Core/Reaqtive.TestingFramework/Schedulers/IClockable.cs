// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Interface for objects that have a clock.
    /// </summary>
    /// <typeparam name="TAbsolute">Type of the absolute time values returned by the clock.</typeparam>
    public interface IClockable<out TAbsolute>
    {
        /// <summary>
        /// Gets the clock value.
        /// </summary>
        TAbsolute Clock { get; }
    }
}
