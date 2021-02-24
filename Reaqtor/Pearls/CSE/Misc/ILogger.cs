// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Interface for loggers.
//
// BD - September 2014
//

using System;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Interface for loggers.
    /// </summary>
    internal interface ILogger
    {
        /// <summary>
        /// Writes the specified message to the logger using the specified color (if specified).
        /// </summary>
        /// <param name="message">Message to write to the logger.</param>
        /// <param name="color">Color to render the message in. If omitted, the color is undefined.</param>
        void WriteLine(string message, ConsoleColor? color = null);
    }
}
