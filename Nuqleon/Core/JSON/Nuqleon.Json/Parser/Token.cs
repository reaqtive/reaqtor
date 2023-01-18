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
    /// JSON token.
    /// </summary>
    internal readonly struct Token
    {
        #region Constructors

        /// <summary>
        /// Creates a new token of the given type, with the given data.
        /// </summary>
        /// <param name="type">Type of the token.</param>
        /// <param name="position">Position of the first character of the token in the input stream.</param>
        /// <param name="data">Data for the token.</param>
        private Token(TokenType type, int position, string data)
        {
            Type = type;
            Position = position;
            Data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// Gets the position of the first character of the token in the input stream.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Gets the data for the token.
        /// </summary>
        /// <remarks>Will be null for anything other than a Number or String token type.</remarks>
        public string Data { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Provides a string representation of the token, based on the token type and its data (if any).
        /// </summary>
        /// <returns>String representation of the token.</returns>
        public override string ToString()
        {
            return Type switch
            {
                TokenType.String => "STRING(" + Data + ")",
                TokenType.Number => "NUM(" + Data + ")",
                _ => Type.ToString().ToUpperInvariant(),
            };
        }

        #endregion

        #region Factory methods

        /// <summary>
        /// Creates an Eof token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>Eof token.</returns>
        public static Token Eof(int pos) => new(TokenType.Eof, pos, data: null);

        /// <summary>
        /// Creates a White token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>White token.</returns>
        public static Token White(int pos) => new(TokenType.White, pos, data: null);

        /// <summary>
        /// Creates a LeftCurly token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>LeftCurly token.</returns>
        public static Token LeftCurly(int pos) => new(TokenType.LeftCurly, pos, data: null);

        /// <summary>
        /// Creates a RightCurly token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>RightCurly token.</returns>
        public static Token RightCurly(int pos) => new(TokenType.RightCurly, pos, data: null);

        /// <summary>
        /// Creates a LeftBracket token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>LeftBracket token.</returns>
        public static Token LeftBracket(int pos) => new(TokenType.LeftBracket, pos, data: null);

        /// <summary>
        /// Creates a RightBracket token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>RightBracket token.</returns>
        public static Token RightBracket(int pos) => new(TokenType.RightBracket, pos, data: null);

        /// <summary>
        /// Creates a True token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>True token.</returns>
        public static Token True(int pos) => new(TokenType.True, pos, data: null);

        /// <summary>
        /// Creates a False token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>False token.</returns>
        public static Token False(int pos) => new(TokenType.False, pos, data: null);

        /// <summary>
        /// Creates a Null token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>Null token.</returns>
        public static Token Null(int pos) => new(TokenType.Null, pos, data: null);

        /// <summary>
        /// Creates a Comma token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>Comma token.</returns>
        public static Token Comma(int pos) => new(TokenType.Comma, pos, data: null);

        /// <summary>
        /// Creates a Colon token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <returns>Colon token.</returns>
        public static Token Colon(int pos) => new(TokenType.Colon, pos, data: null);

        /// <summary>
        /// Creates a Number token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <param name="num">Text representing the number.</param>
        /// <returns>Number token for the given number text.</returns>
        public static Token Number(int pos, string num) => new(TokenType.Number, pos, num);

        /// <summary>
        /// Creates a String token.
        /// </summary>
        /// <param name="pos">Position of the first character of the token in the input stream.</param>
        /// <param name="str">Text representing the string (including surrounding quotes).</param>
        /// <returns>String token for the given string text.</returns>
        public static Token String(int pos, string str) => new(TokenType.String, pos, str);

        #endregion
    }
}
