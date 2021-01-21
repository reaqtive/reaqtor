// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System;
using System.Diagnostics;
using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        /// <summary>
        /// Emits a System.Char as a JSON String to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append the Char to.</param>
        /// <param name="value">The Char value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitChar(StringBuilder builder, char value, EmitterContext _ = null)
        {
            builder.Append('\"');

            EmitCharCore(builder, value);

            builder.Append('\"');
        }

#if !NO_IO
        /// <summary>
        /// Emits a System.Char as a JSON String to the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to append the Char to.</param>
        /// <param name="value">The Char value to emit.</param>
        /// <param name="_">The emitter context.</param>
        internal static void EmitChar(System.IO.TextWriter writer, char value, EmitterContext _ = null)
        {
            writer.Write('\"');

            EmitCharCore(writer, value);

            writer.Write('\"');
        }
#endif

        /// <summary>
        /// Emits the JSON representation of a character to the specified string <paramref name="builder"/> using RFC 4627 escape rules.
        /// </summary>
        /// <param name="builder">The builder to append the JSON representation of the character to.</param>
        /// <param name="c">The character whose JSON representation to append to the builder.</param>
        internal static void EmitCharCore(StringBuilder builder, char c)
        {
            //
            // NB: We don't bother escaping anything but what's required per the RFC specification. This reduces
            //     the size of the serialized payload.
            //
            // RFC 4627: All Unicode characters may be placed within the quotation marks except for the characters
            //           that must be escaped: quotation mark, reverse solidus, and the control characters (U+0000
            //           through U+001F).
            //
            // NB: For control characters that have a shorthand representation such as \r, we use this instead of
            //     the \u encoding in order to save four characters in the output.
            //

            switch (c)
            {
                case '\"':
                    builder.Append(@"\""");
                    break;
                case '\\':
                    builder.Append(@"\\");
                    break;

                case '\b':
                    builder.Append(@"\b");
                    break;
                case '\f':
                    builder.Append(@"\f");
                    break;
                case '\n':
                    builder.Append(@"\n");
                    break;
                case '\r':
                    builder.Append(@"\r");
                    break;
                case '\t':
                    builder.Append(@"\t");
                    break;

                default:
                    if (char.IsControl(c))
                    {
                        builder.Append(@"\u");
                        EmitCharHexPadFour(builder, c);
                    }
                    else
                    {
                        builder.Append(c);
                    }
                    break;
            }
        }

#if !NO_IO
        /// <summary>
        /// Emits the JSON representation of a character to the specified text <paramref name="writer"/> using RFC 4627 escape rules.
        /// </summary>
        /// <param name="writer">The text writer to append the JSON representation of the character to.</param>
        /// <param name="c">The character whose JSON representation to append to the text writer.</param>
        internal static void EmitCharCore(System.IO.TextWriter writer, char c)
        {
            //
            // NB: We don't bother escaping anything but what's required per the RFC specification. This reduces
            //     the size of the serialized payload.
            //
            // RFC 4627: All Unicode characters may be placed within the quotation marks except for the characters
            //           that must be escaped: quotation mark, reverse solidus, and the control characters (U+0000
            //           through U+001F).
            //
            // NB: For control characters that have a shorthand representation such as \r, we use this instead of
            //     the \u encoding in order to save four characters in the output.
            //

            switch (c)
            {
                case '\"':
                    writer.Write(@"\""");
                    break;
                case '\\':
                    writer.Write(@"\\");
                    break;

                case '\b':
                    writer.Write(@"\b");
                    break;
                case '\f':
                    writer.Write(@"\f");
                    break;
                case '\n':
                    writer.Write(@"\n");
                    break;
                case '\r':
                    writer.Write(@"\r");
                    break;
                case '\t':
                    writer.Write(@"\t");
                    break;

                default:
                    if (char.IsControl(c))
                    {
                        writer.Write(@"\u");
                        EmitCharHexPadFour(writer, c);
                    }
                    else
                    {
                        writer.Write(c);
                    }
                    break;
            }
        }
#endif

        /// <summary>
        /// Emits the four digit hexadecimal representation of the specified character to the specified string <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The string builder to append the four digit hexadecimal representation of the character to.</param>
        /// <param name="c">The character whose hexadecimal representation to append to the builder.</param>
        /// <remarks>
        /// If the hexadecimal representation of the character is shorter than four hexadecimal digits, leading '0' characters will be added to pad to a total length of four.
        /// </remarks>
        internal static void EmitCharHexPadFour(StringBuilder builder, char c)
        {
            var value = (int)c;

            int div = Math.DivRem(value, 16 * 16 * 16, out value);
            AppendHex(builder, div);

            div = Math.DivRem(value, 16 * 16, out value);
            AppendHex(builder, div);

            div = Math.DivRem(value, 16, out value);
            AppendHex(builder, div);
            AppendHex(builder, value);
        }

#if !NO_IO
        /// <summary>
        /// Emits the four digit hexadecimal representation of the specified character to the specified text <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The text writer to append the four digit hexadecimal representation of the character to.</param>
        /// <param name="c">The character whose hexadecimal representation to append to the text writer.</param>
        /// <remarks>
        /// If the hexadecimal representation of the character is shorter than four hexadecimal digits, leading '0' characters will be added to pad to a total length of four.
        /// </remarks>
        internal static void EmitCharHexPadFour(System.IO.TextWriter writer, char c)
        {
            var value = (int)c;

            int div = Math.DivRem(value, 16 * 16 * 16, out value);
            AppendHex(writer, div);

            div = Math.DivRem(value, 16 * 16, out value);
            AppendHex(writer, div);

            div = Math.DivRem(value, 16, out value);
            AppendHex(writer, div);
            AppendHex(writer, value);
        }
#endif

        /// <summary>
        /// Appends the hexadecimal digit representing the specified integer <paramref name="value"/> to the specified string <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The string builder to append the hexadecimal digit to.</param>
        /// <param name="value">The value whose hexadecimal digit representation to append to the builder. This value should be between 0 (inclusive) and 16 (exclusive).</param>
        private static void AppendHex(StringBuilder builder, int value)
        {
            Debug.Assert(value is >= 0 and < 16);

            if (value < 10)
            {
                builder.Append((char)('0' + value));
            }
            else
            {
                value -= 10;
                builder.Append((char)('A' + value));
            }
        }

#if !NO_IO
        /// <summary>
        /// Appends the hexadecimal digit representing the specified integer <paramref name="value"/> to the specified text <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The text writer to append the hexadecimal digit to.</param>
        /// <param name="value">The value whose hexadecimal digit representation to append to the text writer. This value should be between 0 (inclusive) and 16 (exclusive).</param>
        private static void AppendHex(System.IO.TextWriter writer, int value)
        {
            Debug.Assert(value is >= 0 and < 16);

            if (value < 10)
            {
                writer.Write((char)('0' + value));
            }
            else
            {
                value -= 10;
                writer.Write((char)('A' + value));
            }
        }
#endif
    }
}
