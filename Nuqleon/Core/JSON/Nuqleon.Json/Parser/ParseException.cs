// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1032 // Implement standard exception constructors. (Only meant to be instantiated by this library.)

using System;

namespace Nuqleon.Json.Parser
{
    /// <summary>
    /// Parser exception signaling a parsing error in the input stream.
    /// </summary>
    /// <remarks>
    /// Creates a new parser exception signaling an error in the input stream at the given position.
    /// </remarks>
    /// <param name="message">Message describing the parsing error.</param>
    /// <param name="position">Position in the input stream where the parsing error occurred.</param>
    /// <param name="error">Parser error.</param>
    [Serializable]
    public class ParseException(string message, int position, ParseError error) : Exception(message)
    {

        /// <summary>
        /// Gets the position in the input stream where the parsing error occurred.
        /// </summary>
        public int Position { get; } = position;

        /// <summary>
        /// Gets the parser error.
        /// </summary>
        public ParseError Error { get; } = error;
    }
}
