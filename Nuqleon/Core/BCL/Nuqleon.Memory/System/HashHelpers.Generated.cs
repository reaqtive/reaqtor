// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System
{
    // REVIEW: Consider using System.HashCode when we move to .NET Standard 2.1.

    partial class HashHelpers
    {
        public static int Combine(int h1, int h2, int h3) => Combine(Combine(h1, h2), h3);
        public static int Combine(int h1, int h2, int h3, int h4) => Combine(Combine(Combine(h1, h2), h3), h4);
        public static int Combine(int h1, int h2, int h3, int h4, int h5) => Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6) => Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7) => Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11), h12);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11), h12), h13);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13, int h14) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11), h12), h13), h14);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13, int h14, int h15) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11), h12), h13), h14), h15);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13, int h14, int h15, int h16) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11), h12), h13), h14), h15), h16);
        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9, int h10, int h11, int h12, int h13, int h14, int h15, int h16, int h17) => Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(h1, h2), h3), h4), h5), h6), h7), h8), h9), h10), h11), h12), h13), h14), h15), h16), h17);
    }
}
