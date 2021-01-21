// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Entity recovery failure mitigation options.
    /// </summary>
    public enum ReactiveEntityRecoveryFailureMitigation
    {
        /// <summary>
        /// Ignores the failure. A future recovery attempt will retry the load.
        /// This option is the default and recommended to investigate issues.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// Permanently removes the entity.
        /// </summary>
        Remove = 1,

        /// <summary>
        /// Attempts to regenerate the entity.
        /// </summary>
        Regenerate = 2,
    }
}
