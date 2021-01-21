// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Represents a source of yield tokens.
    /// </summary>
    public interface IYieldTokenSource
    {
        /// <summary>
        /// Gets a yield token to observe yield requests.
        /// </summary>
        YieldToken Token { get; }

        /// <summary>
        /// Gets a value indicating whether a yield has been requested.
        /// </summary>
        bool IsYieldRequested { get; }
    }
}
