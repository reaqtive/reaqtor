// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

namespace Tests.Nuqleon.Json.Interop.Newtonsoft
{
    internal sealed class Big1
    {
        public bool[] Item1 { get; set; }

        public static readonly Big1[] Values = new[]
        {
            new Big1 { Item1 = new bool[] { false, false } },
            new Big1 { Item1 = new bool[] { true, true } },
            new Big1 { Item1 = new bool[] { true } },
            new Big1 { Item1 = new bool[] { true, false } },
        };
    }

    internal sealed class Big2
    {
        public double[] Item1 { get; set; }
        public bool Item2 { get; set; }

        public static readonly Big2[] Values = new[]
        {
            new Big2 { Item1 = new double[] { 1.0 }, Item2 = true },
            new Big2 { Item1 = new double[] { 1.0, 0.0 }, Item2 = true },
            new Big2 { Item1 = new double[] { 0.0, 2.25 }, Item2 = true },
            new Big2 { Item1 = new double[] { 2.25, 1.0, 1.0 }, Item2 = true },
        };
    }

    internal sealed class Big3
    {
        public double Item1 { get; set; }
        public double[] Item2 { get; set; }
        public string[] Item3 { get; set; }

        public static readonly Big3[] Values = new[]
        {
            new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } },
            new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } },
            new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } },
            new Big3 { Item1 = -1.5, Item2 = new double[] { 2.25 }, Item3 = new string[] { null, "" } },
        };
    }

    internal sealed class Big4
    {
        public int Item1 { get; set; }
        public string Item2 { get; set; }
        public double Item3 { get; set; }
        public Big3 Item4 { get; set; }

        public static readonly Big4[] Values = new[]
        {
            new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } },
            new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } },
            new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } },
            new Big4 { Item1 = -1, Item2 = "foobar", Item3 = 2.25, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } },
        };
    }

    internal sealed class Big5
    {
        public Big4 Item1 { get; set; }
        public Big4[] Item2 { get; set; }
        public Big2 Item3 { get; set; }
        public Big1[] Item4 { get; set; }
        public int Item5 { get; set; }

        public static readonly Big5[] Values = new[]
        {
            new Big5 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new Big4[] { new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item3 = new Big2 { Item1 = new double[] { 0.0, 2.25 }, Item2 = true }, Item4 = new Big1[] { new Big1 { Item1 = new bool[] { true, true } } }, Item5 = 0 },
            new Big5 { Item1 = new Big4 { Item1 = -1, Item2 = "foobar", Item3 = 2.25, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new Big4[] { new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item3 = new Big2 { Item1 = new double[] { 2.25, 1.0, 1.0 }, Item2 = true }, Item4 = new Big1[] { new Big1 { Item1 = new bool[] { true, false } } }, Item5 = -1 },
            new Big5 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new Big4[] { new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = -1, Item2 = "foobar", Item3 = 2.25, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item3 = new Big2 { Item1 = new double[] { 1.0, 0.0 }, Item2 = true }, Item4 = new Big1[] { new Big1 { Item1 = new bool[] { true } } }, Item5 = 42 },
            new Big5 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new Big4[] { new Big4 { Item1 = -1, Item2 = "foobar", Item3 = 2.25, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item3 = new Big2 { Item1 = new double[] { 0.0, 2.25 }, Item2 = true }, Item4 = new Big1[] { new Big1 { Item1 = new bool[] { true, false } } }, Item5 = -1 },
        };
    }

    internal sealed class Big6
    {
        public Big4 Item1 { get; set; }
        public double[] Item2 { get; set; }
        public double[] Item3 { get; set; }
        public Big3 Item4 { get; set; }
        public string Item5 { get; set; }
        public Big1 Item6 { get; set; }

        public static readonly Big6[] Values = new[]
        {
            new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } },
            new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } },
            new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, -1.5 }, Item3 = new double[] { 0.0, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { false, false } } },
            new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { -1.5 }, Item3 = new double[] { 0.0, -1.5, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, true } } },
        };
    }

    internal sealed class Big7
    {
        public bool Item1 { get; set; }
        public Big6 Item2 { get; set; }
        public Big6 Item3 { get; set; }
        public bool[] Item4 { get; set; }
        public bool[] Item5 { get; set; }
        public Big1[] Item6 { get; set; }
        public bool[] Item7 { get; set; }

        public static readonly Big7[] Values = new[]
        {
            new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, -1.5 }, Item3 = new double[] { 0.0, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { false, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { -1.5 }, Item3 = new double[] { 0.0, -1.5, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, true } } }, Item4 = new bool[] { true }, Item5 = new bool[] { true, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } } }, Item7 = new bool[] { false, true } },
            new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { -1.5 }, Item3 = new double[] { 0.0, -1.5, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, true } } }, Item4 = new bool[] { false, true, false }, Item5 = new bool[] { false }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { true, true } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { false } },
            new Big7 { Item1 = true, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, false, true }, Item5 = new bool[] { false, true, false }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { true, false } } }, Item7 = new bool[] { true } },
            new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, true, false }, Item5 = new bool[] { false, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { true } },
        };
    }

    internal sealed class Big8
    {
        public int[] Item1 { get; set; }
        public Big3[] Item2 { get; set; }
        public bool Item3 { get; set; }
        public Big4[] Item4 { get; set; }
        public Big3 Item5 { get; set; }
        public Big7[] Item6 { get; set; }
        public Big2[] Item7 { get; set; }
        public int[] Item8 { get; set; }

        public static readonly Big8[] Values = new[]
        {
            new Big8 { Item1 = new int[] { 42, -1 }, Item2 = new Big3[] { new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item3 = false, Item4 = new Big4[] { new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item5 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item6 = new Big7[] { new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, true, false }, Item5 = new bool[] { false, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { true } }, new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, -1.5 }, Item3 = new double[] { 0.0, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { false, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { -1.5 }, Item3 = new double[] { 0.0, -1.5, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, true } } }, Item4 = new bool[] { true }, Item5 = new bool[] { true, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } } }, Item7 = new bool[] { false, true } } }, Item7 = new Big2[] { new Big2 { Item1 = new double[] { 2.25, 1.0, 1.0 }, Item2 = true }, new Big2 { Item1 = new double[] { 1.0 }, Item2 = true } }, Item8 = new int[] { 42 } },
            new Big8 { Item1 = new int[] { -1, -1, 42 }, Item2 = new Big3[] { new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, new Big3 { Item1 = -1.5, Item2 = new double[] { 2.25 }, Item3 = new string[] { null, "" } } }, Item3 = true, Item4 = new Big4[] { new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item5 = new Big3 { Item1 = -1.5, Item2 = new double[] { 2.25 }, Item3 = new string[] { null, "" } }, Item6 = new Big7[] { new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, -1.5 }, Item3 = new double[] { 0.0, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { false, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { -1.5 }, Item3 = new double[] { 0.0, -1.5, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, true } } }, Item4 = new bool[] { true }, Item5 = new bool[] { true, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } } }, Item7 = new bool[] { false, true } }, new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, true, false }, Item5 = new bool[] { false, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { true } } }, Item7 = new Big2[] { new Big2 { Item1 = new double[] { 1.0 }, Item2 = true }, new Big2 { Item1 = new double[] { 1.0, 0.0 }, Item2 = true }, new Big2 { Item1 = new double[] { 1.0, 0.0 }, Item2 = true } }, Item8 = new int[] { 42, 42 } },
            new Big8 { Item1 = new int[] { 0, -1, 42 }, Item2 = new Big3[] { new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } }, new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item3 = true, Item4 = new Big4[] { new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item5 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item6 = new Big7[] { new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, true, false }, Item5 = new bool[] { false, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { true } } }, Item7 = new Big2[] { new Big2 { Item1 = new double[] { 2.25, 1.0, 1.0 }, Item2 = true }, new Big2 { Item1 = new double[] { 2.25, 1.0, 1.0 }, Item2 = true }, new Big2 { Item1 = new double[] { 0.0, 2.25 }, Item2 = true } }, Item8 = new int[] { 42, 42 } },
            new Big8 { Item1 = new int[] { 0, 0, 42 }, Item2 = new Big3[] { new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } } }, Item3 = false, Item4 = new Big4[] { new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } } }, Item5 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item6 = new Big7[] { new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, true, false }, Item5 = new bool[] { false, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { true } }, new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, -1.5 }, Item3 = new double[] { 0.0, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { false, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { -1.5 }, Item3 = new double[] { 0.0, -1.5, -1.5 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { 1.0, 2.25, 1.0 }, Item3 = new string[] { "bar", "bar", "bar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, true } } }, Item4 = new bool[] { true }, Item5 = new bool[] { true, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } } }, Item7 = new bool[] { false, true } }, new Big7 { Item1 = false, Item2 = new Big6 { Item1 = new Big4 { Item1 = 42, Item2 = "", Item3 = 0.0, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 2.25, 1.0, 0.0 }, Item3 = new double[] { -1.5, 0.0, 2.25 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item3 = new Big6 { Item1 = new Big4 { Item1 = 0, Item2 = null, Item3 = -1.5, Item4 = new Big3 { Item1 = 2.25, Item2 = new double[] { -1.5, -1.5, -1.5 }, Item3 = new string[] { "", "foobar" } } }, Item2 = new double[] { 0.0 }, Item3 = new double[] { 0.0, 0.0 }, Item4 = new Big3 { Item1 = -1.5, Item2 = new double[] { -1.5 }, Item3 = new string[] { "foobar", "foobar" } }, Item5 = "bar", Item6 = new Big1 { Item1 = new bool[] { true, false } } }, Item4 = new bool[] { true, true, false }, Item5 = new bool[] { false, true }, Item6 = new Big1[] { new Big1 { Item1 = new bool[] { false, false } }, new Big1 { Item1 = new bool[] { true } } }, Item7 = new bool[] { true } } }, Item7 = new Big2[] { new Big2 { Item1 = new double[] { 1.0 }, Item2 = true }, new Big2 { Item1 = new double[] { 2.25, 1.0, 1.0 }, Item2 = true } }, Item8 = new int[] { 0, -1, 42 } },
        };
    }

}
