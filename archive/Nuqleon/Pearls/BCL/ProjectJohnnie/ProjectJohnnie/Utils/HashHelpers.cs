// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ProjectJohnnie
{
    internal static class HashHelpers
    {
        public static int Combine(int h1) => h1;

        public static int Combine(int h1, int h2)
        {
            uint num = (uint)(h1 << 5 | (int)((uint)h1 >> 27));

            return (int)(num + (uint)h1 ^ (uint)h2);
        }

        public static int Combine(int h1, int h2, int h3) => Combine(Combine(h1, h2), h3);
        public static int Combine(int h1, int h2, int h3, int h4) => Combine(Combine(h1, h2), Combine(h3, h4));
        public static int Combine(int h1, int h2, int h3, int h4, int h5) => Combine(Combine(Combine(h1, h2), Combine(h3, h4)), h5);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6) => Combine(Combine(Combine(h1, h2), Combine(h3, h4)), Combine(h5, h6));
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7) => Combine(Combine(Combine(h1, h2), Combine(h3, h4)), Combine(Combine(h5, h6), h7));
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8) => Combine(Combine(Combine(h1, h2), Combine(h3, h4)), Combine(Combine(h5, h6), Combine(h7, h8)));
    }
}
