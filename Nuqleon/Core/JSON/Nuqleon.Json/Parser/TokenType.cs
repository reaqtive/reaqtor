// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

namespace Nuqleon.Json.Parser
{
    /// <summary>
    /// JSON token types.
    /// </summary>
    internal enum TokenType : byte
    {
        /// <summary>
        /// No token, used to represent the absence of a token (i.e. C# "null").
        /// </summary>
        None = 0,

        /// <summary>
        /// End of file.
        /// </summary>
        Eof,

        /// <summary>
        /// Whitespace.
        /// </summary>
        White,

        /// <summary>
        /// Left curly brace.
        /// </summary>
        LeftCurly,

        /// <summary>
        /// Right curly brace.
        /// </summary>
        RightCurly,

        /// <summary>
        /// Left square bracket.
        /// </summary>
        LeftBracket,

        /// <summary>
        /// Right square bracket.
        /// </summary>
        RightBracket,

        /// <summary>
        /// Comma.
        /// </summary>
        Comma,

        /// <summary>
        /// Colon.
        /// </summary>
        Colon,

        /// <summary>
        /// "false" literal.
        /// </summary>
        False,

        /// <summary>
        /// "true" literal.
        /// </summary>
        True,

        /// <summary>
        /// "null" literal.
        /// </summary>
        Null,

        /// <summary>
        /// String literal.
        /// </summary>
        String,

        /// <summary>
        /// Number literal.
        /// </summary>
        Number,
    }
}
