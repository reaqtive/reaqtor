// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Text;

namespace System
{
    internal static class FormattingHelpers
    {
        public static string EscapeFormatString(this string s)
        {
            var sb = default(StringBuilder);

            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];

                if (c is '{' or '}')
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(s.Length + 2 /* likely another escape to follow */);
                        sb.Append(s, 0, i);
                    }

                    sb.Append(c);
                }

                sb?.Append(c);
            }

            return sb?.ToString() ?? s;
        }
    }
}
