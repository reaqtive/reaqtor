// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Memory.Diagnostics;
using System.Reflection;

namespace Perf.System.Memory.Diagnostics
{
    public class Program
    {
        public static void Main()
        {
            WalkerPerf_FineGrained();

            while (true)
            {
                WalkerPerf_Large();
            }
        }

        private static void WalkerPerf_FineGrained()
        {
            const int N = 100_000;

            var es = new Expression[]
            {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                (Expression<Func<int>>)(() => 42),
#pragma warning restore IDE0004
                (Expression<Func<int, int>>)(x => x),
                (Expression<Func<int, int>>)(x => x + 1),
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<string, int>>)(s => s.Length + 1),
                (Expression<Func<string, string>>)(s => s.ToUpper()),
                (Expression<Func<string, string>>)(s => s.Substring(1)),
                (Expression<Func<string, string>>)(s => s.Substring(1, 2)),
            };

            foreach (var e in es)
            {
                Walk(e, N);
            }
        }

        private static void WalkerPerf_Large()
        {
            const int N = 1_000;

            var es = new List<Expression>();

            for (var i = 0; i < N; i++)
            {
                es.Add((Expression<Func<int, int>>)(x => x + 42));
                es.Add((Expression<Func<string, int>>)(s => s.Length));
                es.Add((Expression<Func<string, string>>)(s => s.Substring(1, 2).ToUpper()));
                es.Add((Expression<Func<DateTime, int>>)(dt => dt.Year));
                es.Add((Expression<Func<Assembly[], int>>)(a => a[0].GetName().GetPublicKeyToken()[0]));
            }

            Walk(es, N);
        }

        private static void Walk(object o, int count)
        {
            Console.Write(o.ToString().PadRight(70, ' ') + count + "\t");

            var w = new FastHeapReferenceWalker();

            var set =
                //new ObjectSet<object>();
                new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            var sw = new Stopwatch();

            for (var i = 0; i < count; i++)
            {
                sw.Start();
                {
                    w.Walk(o, set.Add);
                }
                sw.Stop();

                set.Clear();
            }

            Console.WriteLine(sw.Elapsed);
        }
    }
}
