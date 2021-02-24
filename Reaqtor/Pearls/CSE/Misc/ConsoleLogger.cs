// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Trivial console logger.
//
// BD - September 2014
//

using System;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Logger using Console.Out.
    /// </summary>
    internal class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Writes the specified message to the logger using the specified color (if specified).
        /// </summary>
        /// <param name="message">Message to write to the logger.</param>
        /// <param name="color">Color to render the message in. If omitted, the currently configured console color is used.</param>
        public void WriteLine(string message, ConsoleColor? color = null)
        {
            if (color != null)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.WriteLine(message);

            if (color != null)
            {
                Console.ResetColor();
            }
        }
    }
}
