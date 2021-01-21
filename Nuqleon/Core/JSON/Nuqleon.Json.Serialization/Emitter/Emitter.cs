// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using System.Text;

namespace Nuqleon.Json.Serialization
{
    internal partial class Emitter
    {
        /// <summary>
        /// Returns the JSON string literal representation for the specified <paramref name="value"/> using RFC 4627 escape rules.
        /// </summary>
        /// <param name="value">The string whose JSON string literal representation to obtain.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is well-suited for usage at compile time for emitters. For runtime behavior, use EmitString instead.
        /// </remarks>
        internal static string ToJsonStringLiteral(string value)
        {
            using var psb = PooledStringBuilder.New();

            var sb = psb.StringBuilder;

            EmitString(sb, value);

            return sb.ToString();
        }
    }
}
