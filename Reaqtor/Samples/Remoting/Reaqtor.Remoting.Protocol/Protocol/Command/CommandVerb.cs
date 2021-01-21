// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Verbs for commands sent to a reactive service.
    /// </summary>
    public enum CommandVerb
    {
        /// <summary>
        /// A resource is defined or created.
        /// </summary>
        New,

        /// <summary>
        /// A resource is undefined or deleted.
        /// </summary>
        Remove,

        /// <summary>
        /// A resource is retrieved.
        /// </summary>
        Get,
    }
}
