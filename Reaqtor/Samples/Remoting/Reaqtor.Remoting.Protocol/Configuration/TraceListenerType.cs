﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// An enumeration for scheduler types.
    /// </summary>
    [Serializable]
    [Flags]
    public enum TraceListenerType
    {
        /// <summary>
        /// Denotes the default trace listener.
        /// </summary>
#pragma warning disable CA1008 // Enums should have zero value. (Suggested name 'None' isn't great.)
        Default = 0,
#pragma warning restore CA1008

        /// <summary>
        /// Denotes the console trace listener.
        /// </summary>
        Console = 1,

        /// <summary>
        /// Denotes a trace listener that logs to a file.
        /// </summary>
        File = 2,
    }
}
