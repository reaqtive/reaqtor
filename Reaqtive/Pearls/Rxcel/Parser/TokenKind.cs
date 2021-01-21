// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Reactive Excel implementation using classic Rx to demonstrate the concepts around
// building reactive computational graphs.
//

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

namespace Rxcel
{
    internal enum TokenKind
    {
        Identifier,
        Number,

        Plus,
        Dash,
        Star,
        Slash,
        Percent,

        OpenParen,
        CloseParen,

        Comma,
        Colon,
    }
}
