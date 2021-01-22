// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Tests
{
    internal class Data
    {
        public static readonly Type[] Types;
        public static readonly Dictionary<Type, object[]> Values;

        static Data()
        {
            Types = new[]
            {
                typeof(bool),      //  0
                typeof(byte),      //  1
                typeof(sbyte),     //  2
                typeof(short),     //  3
                typeof(ushort),    //  4
                typeof(int),       //  5
                typeof(uint),      //  6
                typeof(long),      //  7
                typeof(ulong),     //  8
                typeof(float),     //  9
                typeof(double),    // 10
                typeof(decimal),   // 11
                typeof(char),      // 12
                typeof(string),    // 13
                typeof(DateTime),  // 14
                typeof(TimeSpan),  // 15
                typeof(Guid),      // 16
                typeof(Version),   // 17
            };

            var dt1 = new DateTime(1983, 2, 11);
            var dt2 = new DateTime(2016, 10, 7);

            var ts1 = new TimeSpan(3, 14, 15);
            var ts2 = new TimeSpan(6, 28, 30);

            var guid1 = new Guid("{8CE64185-3362-4431-85B0-7E6E534278C4}");
            var guid2 = new Guid("{EF087F04-025F-4C59-8322-816D1D2C6F53}");

            var v1 = new Version(1, 0, 0, 0);
            var v2 = new Version(2, 0, 0, 0);

            Values = new Dictionary<Type, object[]>
            {
                { typeof(bool)    ,        true ,      false  }, //  0
                { typeof(byte)    ,    (byte)42 ,    (byte)1  }, //  1
                { typeof(sbyte)   ,   (sbyte)42 ,   (sbyte)1  }, //  2
                { typeof(short)   ,   (short)42 ,   (short)1  }, //  3
                { typeof(ushort)  ,  (ushort)42 ,  (ushort)1  }, //  4
                { typeof(int)     ,     (int)42L,     (int)1L }, //  5
                { typeof(uint)    ,    (uint)42 ,    (uint)1  }, //  6
                { typeof(long)    ,    (long)42 ,    (long)1  }, //  7
                { typeof(ulong)   ,   (ulong)42 ,   (ulong)1  }, //  8
                { typeof(float)   ,   (float)16 ,   (float)1  }, //  9
                { typeof(double)  ,  (double)16 ,  (double)1  }, // 10
                { typeof(decimal) , (decimal)42 , (decimal)1  }, // 11
                { typeof(char)    ,    (char)42 ,    (char)1  }, // 12
                { typeof(string)  ,         "42",      "null" }, // 13
                { typeof(DateTime),         dt1 ,        dt2  }, // 14
                { typeof(TimeSpan),         ts1 ,        ts2  }, // 15
                { typeof(Guid)    ,       guid1 ,      guid2  }, // 16
                { typeof(Version) ,          v1 ,         v2  }, // 17
            };
        }
    }

    internal static class MultiDictionaryExtensions
    {
        public static void Add<K, V>(this Dictionary<K, V[]> d, K key, params V[] values)
        {
            d.Add(key, values);
        }
    }
}
