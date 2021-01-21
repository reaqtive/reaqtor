// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 05/09/2017 - Added caching of reflection.
//

using System.Reflection;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Provides access to some cached reflection members.
    /// </summary>
    internal static class ReflectionCache
    {
        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the <see cref="Parser.StartsWith(string, int, int, ref int, string)"/> method.
        /// </summary>
        public static readonly MethodInfo StartsWithString = typeof(Parser).GetMethod(nameof(Parser.StartsWith), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the <see cref="Parser.StartsWithFast(string, ref int, string)"/> method.
        /// </summary>
        public static readonly MethodInfo StartsWithStringFast = typeof(Parser).GetMethod(nameof(Parser.StartsWithFast), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the <see cref="Parser.TryGetNextChar(string, int, ref int, out char)"/> method.
        /// </summary>
        public static readonly MethodInfo TryGetNextCharString = typeof(Parser).GetMethod(nameof(Parser.TryGetNextChar), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> object representing the character indexer on <see cref="string"/>.
        /// </summary>
        public static readonly PropertyInfo Chars = typeof(string).GetProperty("Chars"); // NB: This indexed property is not visible to C#.

#if !NO_IO
        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the <see cref="Parser.StartsWithReader(System.IO.TextReader, string)"/> method.
        /// </summary>
        public static readonly MethodInfo StartsWithReader = typeof(Parser).GetMethod(nameof(Parser.StartsWithReader), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the <see cref="Parser.TryGetNextCharReader(System.IO.TextReader, out int)"/> method.
        /// </summary>
        public static readonly MethodInfo TryGetNextCharReader = typeof(Parser).GetMethod(nameof(Parser.TryGetNextCharReader), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> object representing the <see cref="System.IO.TextReader.Peek()"/> method.
        /// </summary>
        public static readonly MethodInfo Peek = typeof(System.IO.TextReader).GetMethod(nameof(System.IO.TextReader.Peek), BindingFlags.Public | BindingFlags.Instance);
#endif
    }
}
