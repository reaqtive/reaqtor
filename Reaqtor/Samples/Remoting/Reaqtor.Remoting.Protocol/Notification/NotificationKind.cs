// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Indicates the type of a notification.
    /// </summary>
    public enum NotificationKind
    {
        /// <summary>
        /// Represents an OnNext notification.
        /// </summary>
        OnNext = 0,

        /// <summary>
        /// Represents an OnError notification.
        /// </summary>
        OnError = 1,

        /// <summary>
        /// Represents an OnCompleted notification.
        /// </summary>
        OnCompleted = 2,
    }
}
