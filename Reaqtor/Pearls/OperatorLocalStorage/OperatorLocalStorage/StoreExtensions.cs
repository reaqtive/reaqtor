// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.IO;
using System.Linq;
using Utilities;

using Reaqtor.QueryEngine;

namespace Playground
{
    /// <summary>
    /// Provides a set of extension methods to work with in-memory key/value stores, state readers, and state writers.
    /// </summary>
    internal static class StoreExtensions
    {
        public static IStateReader WithLogging(this IStateReader reader) => new LoggingStateReader(reader, Console.Out);

        public static IStateWriter WithLogging(this IStateWriter writer) => new LoggingStateWriter(writer, Console.Out);

        public static void Print(this Store store)
        {
            Console.WriteLine("--------------------------------STORE--------------------------------");
            Console.WriteLine();

            foreach (var table in store.Data)
            {
                Console.WriteLine(table.Key);

                foreach (var entry in table.Value.OrderBy(entry => entry.Key))
                {
                    Console.WriteLine("  " + entry.Key + " = " + new StreamReader(new MemoryStream(entry.Value)).ReadToEnd());
                }

                Console.WriteLine();
            }

            Console.WriteLine("--------------------------------STORE--------------------------------");
        }
    }
}
