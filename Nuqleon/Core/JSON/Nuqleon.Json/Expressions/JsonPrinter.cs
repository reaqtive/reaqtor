// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009, June 2013, June 2014 - Created this file.
//

using System.Text;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Helper class to print JSON strings.
    /// </summary>
    internal static class JsonPrinter
    {
        /// <summary>
        /// Gets the JSON string representation (using escape sequences where needed) of the specified string.
        /// </summary>
        /// <param name="str">The string to format as JSON.</param>
        /// <returns>The JSON string representation (using escape sequences where needed) of the specified string.</returns>
        internal static string ToJsonString(this string str)
        {
            var sb = new StringBuilder(str.Length + 2);

            sb.AppendJsonString(str);

            return sb.ToString();
        }

        /// <summary>
        /// Appends the JSON string representation (using escape sequences where needed) of the specified string to the specified builder.
        /// </summary>
        /// <param name="builder">The builder to append to.</param>
        /// <param name="str">The string to format as JSON.</param>
        public static void AppendJsonString(this StringBuilder builder, string str)
        {
            builder.EnsureCapacity(builder.Length + str.Length + 2);

            builder.Append('\"');

            var n = str.Length;
            for (int i = 0; i < n; i++)
            {
                var c = str[i];
                switch (c)
                {
                    case '\\':
                        builder.Append(@"\\");
                        break;
                    case '\"':
                        builder.Append(@"\""");
                        break;
                    case '\b':
                        builder.Append(@"\b");
                        break;
                    case '\f':
                        builder.Append(@"\f");
                        break;
                    case '\t':
                        builder.Append(@"\t");
                        break;
                    case '\r':
                        builder.Append(@"\r");
                        break;
                    case '\n':
                        builder.Append(@"\n");
                        break;
                    default:
                        builder.Append(c);
                        break;
                }
            }

            builder.Append('\"');
        }
    }
}
