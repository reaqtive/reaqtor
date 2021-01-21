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
    /// Parse errors.
    /// </summary>
    public enum ParseError
    {
        /// <summary>
        /// Unknown token encountered.
        /// </summary>
        /// <example>new</example>
        InvalidToken,

        /// <summary>
        /// No significant input found.
        /// </summary>
        /// <example>   </example>
        EmptyInput,

        /// <summary>
        /// Input doesn't consist of a top-level array or object expression.
        /// </summary>
        /// <example>42</example>
        NoArrayOrObjectTopLevelExpression,

        /// <summary>
        /// Input not terminated with EOF token, or input found beyond the end of the top-level array or object expression.
        /// </summary>
        /// <example>[42] "Wrong"</example>
        ImproperTermination,

        /// <summary>
        /// Token not expected at current position in input.
        /// </summary>
        /// <example>}</example>
        UnexpectedToken,

        /// <summary>
        /// Object expression has improper member name token.
        /// </summary>
        /// <example>{Test:42}</example>
        ObjectNoStringMemberName,

        /// <summary>
        /// Object expression has no colon separator between member name and value.
        /// </summary>
        /// <example>{"Test" 42}</example>
        ObjectNoColonMemberNameValueSeparator,

        /// <summary>
        /// Object expression has no value for a given member.
        /// </summary>
        /// <example>{"Test":}</example>
        ObjectNoMemberValue,

        /// <summary>
        /// Object expression has no comma separator between members.
        /// </summary>
        /// <example>{"Test":42 "Bar":24}</example>
        ObjectInvalidMemberSeparator,

        /// <summary>
        /// Object expression has member separator without a member following it.
        /// </summary>
        /// <example>{"Test":42,}</example>
        ObjectEmptyMember,

        /// <summary>
        /// Array expression has no comma separator between elements.
        /// </summary>
        /// <example>[42 24]</example>
        ArrayInvalidElementSeparator,

        /// <summary>
        /// Array expression has element separator without an element following it.
        /// </summary>
        /// <example>[42,]</example>
        ArrayEmptyElement,

        /// <summary>
        /// End of input was reached prematurely.
        /// </summary>
        /// <example>{"Test"</example>
        PrematureEndOfInput,
    }
}
