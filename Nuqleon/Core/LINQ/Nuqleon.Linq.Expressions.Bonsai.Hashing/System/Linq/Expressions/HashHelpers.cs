// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Provides a set of helpers to create stable hash codes.
    /// </summary>
    internal static class HashHelpers
    {
        /// <summary>
        /// Combines two hash values.
        /// </summary>
        /// <param name="a">The first hash value to combine.</param>
        /// <param name="b">The second hash value to combine.</param>
        /// <returns>A combination of the two hash codes.</returns>
        public static int Combine(int a, int b)
        {
            // NB: RyuJIT optimizes this to use the ROL instruction.
            //     This code is borrowed from the corefx implementation, see https://github.com/dotnet/corefx/blob/src/Common/src/System/Numerics/Hashing/HashHelpers.cs

            uint rol5 = ((uint)a << 5) | ((uint)a >> 27);
            return ((int)rol5 + a) ^ b;
        }

        /// <summary>
        /// Combines three hash values.
        /// </summary>
        /// <param name="a">The first hash value to combine.</param>
        /// <param name="b">The second hash value to combine.</param>
        /// <param name="c">The third hash value to combine.</param>
        /// <returns>A combination of the three hash codes.</returns>
        public static int Combine(int a, int b, int c) => Combine(Combine(a, b), c);

        /// <summary>
        /// Combines four hash values.
        /// </summary>
        /// <param name="a">The first hash value to combine.</param>
        /// <param name="b">The second hash value to combine.</param>
        /// <param name="c">The third hash value to combine.</param>
        /// <param name="d">The fourth hash value to combine.</param>
        /// <returns>A combination of the four hash codes.</returns>
        public static int Combine(int a, int b, int c, int d) => Combine(Combine(a, b, c), d);

        /// <summary>
        /// Combines five hash values.
        /// </summary>
        /// <param name="a">The first hash value to combine.</param>
        /// <param name="b">The second hash value to combine.</param>
        /// <param name="c">The third hash value to combine.</param>
        /// <param name="d">The fourth hash value to combine.</param>
        /// <param name="e">The fifth hash value to combine.</param>
        /// <returns>A combination of the five hash codes.</returns>
        public static int Combine(int a, int b, int c, int d, int e) => Combine(Combine(a, b, c, d), e);

        /// <summary>
        /// Combines six hash values.
        /// </summary>
        /// <param name="a">The first hash value to combine.</param>
        /// <param name="b">The second hash value to combine.</param>
        /// <param name="c">The third hash value to combine.</param>
        /// <param name="d">The fourth hash value to combine.</param>
        /// <param name="e">The fifth hash value to combine.</param>
        /// <param name="f">The sixth hash value to combine.</param>
        /// <returns>A combination of the five hash codes.</returns>
        public static int Combine(int a, int b, int c, int d, int e, int f) => Combine(Combine(a, b, c, d, e), f);

        /// <summary>
        /// Computes the Marvin32 hash code for the specified string.
        /// </summary>
        /// <param name="str">The string to compute the Marvin32 hash code for.</param>
        /// <param name="seed">Seed value to compute the hash. This can be used to randomize the hash code.</param>
        /// <returns>The Marvin32 hash code of the specified string.</returns>
        public static int GetMarvin32Hash(this string str, ulong seed)
        {
            // NB: This code is adapted from the CoreCLR hash code in https://github.com/dotnet/coreclr/blob/master/src/vm/marvin32.cpp
            //     and is patented by Microsoft (US 20130262421 A1).

            if (str == null)
            {
                return 0;
            }

            var state = new Marvin32State
            {
                Lo = (uint)(seed),
                Hi = (uint)(seed >> 32)
            };

            var len = str.Length;

            for (var i = 0; len >= 2; i += 2, len -= 2)
            {
                Marvin32Mix(ref state, TwoInt16ToInt32LittleEndian(str, i));
            }

            uint final = 0x80;

            if (len != 0)
            {
                Debug.Assert(len == 1);

#if NET5_0 || NETSTANDARD2_1
                var c = (uint)str[^1];
#else
                var c = (uint)str[str.Length - 1];
#endif

                var b1 = c & 0x00FF;
                var b2 = c >> 8;

                final = (final << 8) | b1;
                final = (final << 8) | b2;
            }

            Marvin32Mix(ref state, final);
            Marvin32Mix(ref state, 0);

            return unchecked((int)(state.Lo ^ state.Hi));
        }

        private static void Marvin32Mix(ref Marvin32State state, uint value)
        {
            state.Lo += value;
            state.Hi ^= state.Lo;

            state.Lo = RotateLeft(state.Lo, 20) + state.Hi;
            state.Hi = RotateLeft(state.Hi, 09) ^ state.Lo;

            state.Lo = RotateLeft(state.Lo, 27) + state.Hi;
            state.Hi = RotateLeft(state.Hi, 19);
        }

        private static uint RotateLeft(uint x, int k) => (x << k) | (x >> (32 - k));

        private static uint TwoInt16ToInt32LittleEndian(string str, int i)
        {
            var c1 = (uint)str[i];
            var b1 = c1 & 0x00FF;
            var b2 = c1 & 0xFF00;

            var c2 = (uint)str[i + 1];
            var b3 = c2 & 0x00FF;
            var b4 = c2 & 0xFF00;

            return (b1 | b2) | (b3 | b4) << 16;
        }

        private struct Marvin32State
        {
            public uint Hi;
            public uint Lo;
        }
    }
}
