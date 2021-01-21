// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

using System;
using System.Collections.Generic;

namespace Nuqleon.Json.Parser
{
    using Expressions;
    using Internal;

    /// <summary>
    /// Parser for JSON data.
    /// </summary>
    public static class JsonParser
    {
        private static readonly Expression[] s_empty = Array.Empty<Expression>();

        #region Public methods

        /// <summary>
        /// Parses the given JSON text and returns an expression tree representing the JSON expression.
        /// </summary>
        /// <param name="input">JSON text to be parsed.</param>
        /// <returns>Expression tree representing the JSON expression.</returns>
        /// <remarks>See RFC 4627 for more information. This parses the production specified in section 2: <code>JSON-text = object / array</code>.</remarks>
        public static Expression Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return Parse(input, ensureTopLevelObjectOrArray: true);
        }

        /// <summary>
        /// Parses the given JSON text and returns an expression tree representing the JSON expression.
        /// </summary>
        /// <param name="input">JSON text to be parsed.</param>
        /// <param name="ensureTopLevelObjectOrArray">Checks whether the top-level JSON text is an object or an array, confirm RFC 4627 for JSON text (true by default).</param>
        /// <returns>Expression tree representing the JSON expression.</returns>
        /// <remarks>See RFC 4627 for more information. This parses the production specified in section 2: <code>JSON-text = object / array</code>.</remarks>
        public static Expression Parse(string input, bool ensureTopLevelObjectOrArray)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            //
            // Tokenize the input, ignoring whitespace between tokens.
            //
            var tokenizer = new Tokenizer(input);
            var tokenStream = tokenizer.Tokenize(includeWhite: false);

            //
            // Initialize token enumeration, ensuring we're not faced with empty input.
            //
            if (!tokenStream.TryMoveNext(out Token firstToken) || firstToken.Type == TokenType.Eof)
                throw new ParseException("Empty input.", 0, ParseError.EmptyInput);

            //
            // Recursive parsing of the token stream, making sure the returned value is a
            // top-level array or object expression.
            //
            var res = Parse(tokenStream);
            if (ensureTopLevelObjectOrArray && res.NodeType != ExpressionType.Array && res.NodeType != ExpressionType.Object)
                throw new ParseException("Unexpected start token.", 0, ParseError.NoArrayOrObjectTopLevelExpression);

            //
            // Ensure proper termination, either by seeing an EOF (ignoring further input
            // that may follow it) or by not seeing any further input.
            //
            if (tokenStream.TryMoveNext(out Token lastToken) && lastToken.Type != TokenType.Eof)
                throw new ParseException("Not properly terminated.", lastToken.Position, ParseError.ImproperTermination);

            return res;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Parses the given JSON token stream and returns an expression tree representing the JSON expression.
        /// </summary>
        /// <param name="tokens">JSON token stream to be parsed.</param>
        /// <returns>Expression tree representing the JSON expression.</returns>
        private static Expression Parse(IEnumerator<Token> tokens)
        {
            //
            // JSON is LL(1); we can simply look-ahead one symbol to decide how to proceed
            // with parsing, i.e. { indicates an object, [ indicates an array, etc.
            //
            var token = tokens.Current;
            return token.Type switch
            {
                TokenType.LeftCurly => ParseObject(tokens),
                TokenType.LeftBracket => ParseArray(tokens),
                TokenType.False => Expression.Boolean(false),
                TokenType.True => Expression.Boolean(true),
                TokenType.Null => Expression.Null(),
                TokenType.String => Expression.String(token.Data),
                TokenType.Number => Expression.Number(token.Data),
                _ => throw new ParseException("Unexpected token: " + token.Type, token.Position, ParseError.UnexpectedToken),
            };
        }

        /// <summary>
        /// Parses a JSON object consuming tokens from the given token stream.
        /// </summary>
        /// <param name="tokens">JSON token stream to consume tokens from.</param>
        /// <returns>Object expression for the JSON object expression.</returns>
        private static ObjectExpression ParseObject(IEnumerator<Token> tokens)
        {
            var members = new Dictionary<string, Expression>();

            bool? expectNext = null;

            Token token;
            while (tokens.TryMoveNext(out token) && token.Type != TokenType.RightCurly && token.Type != TokenType.Eof)
            {
                if (token.Type != TokenType.String)
                    throw new ParseException("Invalid member declaration on object. Expected string for member name.", token.Position, ParseError.ObjectNoStringMemberName);

                string name = token.Data;

                if (!tokens.TryMoveNext(out token))
                    throw new ParseException("Premature end of input during object expression parsing. Expected colon separator between member name and value.", -1 /* end */, ParseError.PrematureEndOfInput);

                if (token.Type != TokenType.Colon)
                    throw new ParseException("Invalid member declaration on object. Expected colon separator between member name and value.", token.Position, ParseError.ObjectNoColonMemberNameValueSeparator);

                if (!tokens.MoveNext())
                    throw new ParseException("Premature end of input during object expression parsing. Expected member value.", -1 /* end */, ParseError.PrematureEndOfInput);

                members[name] = Parse(tokens);

                if (!tokens.TryMoveNext(out token))
                    throw new ParseException("Premature end of input during object expression parsing. Expected either comma or closing curly brace.", -1 /* end */, ParseError.PrematureEndOfInput);

                if (token.Type is not TokenType.Comma and not TokenType.RightCurly)
                    throw new ParseException("Invalid member declaration on object. Expected proper separator between members.", token.Position, ParseError.ObjectInvalidMemberSeparator);

                expectNext = token.Type == TokenType.Comma;

                if (token.Type == TokenType.RightCurly)
                    break;
            }

            if (token.Type is TokenType.None or not TokenType.RightCurly)
                throw new ParseException("Premature end of input during object expression parsing. Expected to reach a closing curly brace.", -1 /* end */, ParseError.PrematureEndOfInput);

            if (expectNext.HasValue && expectNext.Value)
                throw new ParseException("Empty member declaration on object.", token.Position, ParseError.ObjectEmptyMember);

            return Expression.Object(members);
        }

        /// <summary>
        /// Parses a JSON array consuming tokens from the given token stream.
        /// </summary>
        /// <param name="tokens">JSON token stream to consume tokens from.</param>
        /// <returns>Array expression for the JSON array expression.</returns>
        private static ArrayExpression ParseArray(IEnumerator<Token> tokens)
        {
            var values = default(List<Expression>);

            bool? expectNext = null;

            Expression element1 = null;
            Expression element2 = null;
            Expression element3 = null;
            Expression element4 = null;
            Expression element5 = null;
            Expression element6 = null;

            int i = 0;

            Token token;
            while (tokens.TryMoveNext(out token) && token.Type != TokenType.RightBracket && token.Type != TokenType.Eof)
            {
                var element = Parse(tokens);

                switch (i)
                {
                    case 0:
                        element1 = element;
                        break;
                    case 1:
                        element2 = element;
                        break;
                    case 2:
                        element3 = element;
                        break;
                    case 3:
                        element4 = element;
                        break;
                    case 4:
                        element5 = element;
                        break;
                    case 5:
                        element6 = element;
                        break;
                    case 6:
                        values = new List<Expression>(8) { element1, element2, element3, element4, element5, element6, element };
                        break;
                    default:
                        values.Add(element);
                        break;
                }

                ++i;

                if (!tokens.TryMoveNext(out token))
                    throw new ParseException("Premature end of input during array expression parsing. Expected colon separator between elements or closing square bracket.", -1 /* end */, ParseError.PrematureEndOfInput);

                if (token.Type is not TokenType.Comma and not TokenType.RightBracket)
                    throw new ParseException("Invalid element declaration in array. Expected proper separator between elements.", token.Position, ParseError.ArrayInvalidElementSeparator);

                expectNext = token.Type == TokenType.Comma;

                if (token.Type == TokenType.RightBracket)
                    break;
            }

            if (token.Type is TokenType.None or not TokenType.RightBracket)
                throw new ParseException("Premature end of input during array expression parsing. Expected to reach a closing square bracket.", -1 /* end */, ParseError.PrematureEndOfInput);

            if (expectNext.HasValue && expectNext.Value)
                throw new ParseException("Empty element declaration on array.", token.Position, ParseError.ArrayEmptyElement);

            if (values == null)
            {
                switch (i)
                {
                    case 0:
                        return Expression.Array(s_empty);
                    case 1:
                        return Expression.Array(element1);
                    case 2:
                        return Expression.Array(element1, element2);
                    case 3:
                        return Expression.Array(element1, element2, element3);
                    case 4:
                        return Expression.Array(element1, element2, element3, element4);
                    case 5:
                        return Expression.Array(element1, element2, element3, element4, element5);
                    case 6:
                        return Expression.Array(element1, element2, element3, element4, element5, element6);
                }
            }

            return Expression.Array(values);
        }

        #endregion
    }
}
