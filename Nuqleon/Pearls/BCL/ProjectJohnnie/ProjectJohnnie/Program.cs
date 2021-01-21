// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Memory.Diagnostics;

namespace ProjectJohnnie
{
    class Program
    {
        static void Main()
        {
            DemoStats();
            DemoOptimizer();
            DemoAnalyzer();
        }

        static void DemoStats()
        {
            Console.WriteLine(GetStats(new KeyValuePair<string, int>[2, 2] { { new KeyValuePair<string, int>("bar", 1), new KeyValuePair<string, int>("foo", 2) }, { new KeyValuePair<string, int>("baz", 3), new KeyValuePair<string, int>("qux", 4) } }));
        }

        static void DemoAnalyzer()
        {
            Expression<Func<string, int>> f = s => (s + "foo").Length * 2 - 1;
            Expression<Func<string, int>> g = s => (s + "foo").Length * 2 - 1;
            Expression<Func<string, int>> h = s => s.ToUpper().Length;

            var a = new HeapUsageAnalyzer();

            a.AddPartition("QE1", f);
            a.AddPartition("QE2", g);
            a.AddPartition("QE3", h);

            var r = a.Analyze(new HeapAnalysisOptions { ComputeSharedHeap = true });

            foreach (var kv in r.Reports)
            {
                Console.WriteLine(kv.Key.Name);
                Console.WriteLine(new string('*', kv.Key.Name.Length));
                Console.WriteLine();

                Console.WriteLine(kv.Value.GetStats().ToString());

                Console.WriteLine();
            }

            Console.WriteLine("Shared");
            Console.WriteLine("******");
            Console.WriteLine();

            Console.WriteLine(r.Shared.GetStats().ToString());
        }

#if FALSE
        static HashSet<object> AnalyzeFast(object obj)
        {
            var a = new FastHeapReferenceWalker();

            var set = new HashSet<object>(ReferenceEqualityComparer<object>.Instance);

            a.Walk(obj, set.Add);

            return set;
        }
#endif

        static void DemoOptimizer()
        {
            var obj1 = new Quxie
            {
                bar = "qux",
                foo = "QUX".ToLower(),
                baz = new[]
                {
                    new KeyValuePair<string, int>("qux ".Trim(), 1),
                    new KeyValuePair<string, int>(" qux".Trim(), 2),
                },
                fred = new KeyValuePair<string, int>[2, 2]
                {
                    {
                        new KeyValuePair<string, int>("Qux".ToLower(), 1),
                        new KeyValuePair<string, int>("qUx".ToLower(), 2),
                    },
                    {
                        new KeyValuePair<string, int>("quX".ToLower(), 3),
                        new KeyValuePair<string, int>("QuX".ToLower(), 4),
                    },
                },
                quxes = new[]
                {
                    "QUx".ToLower(),
                    "qUX".ToLower(),
                },
                dave = new string[1, 1]
                {
                    {
                        " qux ".Trim()
                    }
                }
            };

            DemoOptimizer(obj1);

            var obj2 = Expression.Add(Expression.Constant(1), Expression.Constant(1));

            DemoOptimizer(obj2);

            var obj3 = new { bar = "qux", foo = "QUX".ToLower() };

            DemoOptimizer(obj3);

            var obj4 = new[]
            {
                new KeyValuePair<string, int>("qux", 1),
                new KeyValuePair<string, int>("QUX".ToLower(), 2),
            };

            DemoOptimizer(obj4);

            var obj5 = new[]
            {
                new Bar("qux"),
                new Bar("QUX".ToLower()),
            };

            DemoOptimizer(obj5);

            var obj6 = new object[]
            {
                new Tuple<string, int>("qux", 1),
                new Tuple<int, string, int>(2, "QUX".ToLower(), 3),
            };

            DemoOptimizer(obj6);
        }

        static void DemoOptimizer(object obj)
        {
            var opt = new MyHeapOptimizer();

            Console.WriteLine(GetStats(obj));

            opt.Walk(obj, _ => true);

            Console.WriteLine(GetStats(obj));
        }

        static HeapStats GetStats(object obj)
        {
            var a = new HeapUsageAnalyzer();

            var p = a.AddPartition("it", obj);

            var r = a.Analyze(new HeapAnalysisOptions { ComputeSharedHeap = true });

            return r.Reports[p].GetStats();
        }
    }

    class Quxie
    {
        public string bar;
        public string foo;
        public KeyValuePair<string, int>[] baz;
        public KeyValuePair<string, int>[,] fred;
        public string[] quxes;
        public string[,] dave;
    }

    struct Bar
    {
        public Bar(string foo)
        {
            Foo = foo;
        }

        public readonly string Foo;
    }
}
