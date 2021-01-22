// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// An enumeration for scheduler types.
    /// </summary>
    [Serializable]
    public enum SchedulerType
    {
        /// <summary>
        /// Denotes the default scheduler type.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Denotes the test scheduler type.
        /// </summary>
        Test = 1,

        /// <summary>
        /// Denotes the logging scheduler type.
        /// </summary>
        Logging = 2,

        /// <summary>
        /// Denotes the shared static scheduler.
        /// </summary>
        Static = 3,
    }
}
