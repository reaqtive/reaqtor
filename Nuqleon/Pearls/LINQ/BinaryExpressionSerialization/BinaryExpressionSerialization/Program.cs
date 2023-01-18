// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Linq.Expressions.Bonsai.Serialization.Binary;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using BenchmarkDotNet.Running;

#if !USE_SLIM
using System.Threading;
#endif

#if USE_SLIM
using ExpressionFactory = System.Linq.Expressions.ExpressionSlimFactory;
#endif

namespace BinaryExpressionSerialization
{
    public class Program
    {
        public static void Main()
        {
            Size();
            Perf();
        }

        private static void Size()
        {
            var serj = new CustomBonsaiSerializer();
            var serb = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

            var es = new Expression[]
            {
                Expression.Default(typeof(int)),
                Expression.Empty(),

                Expression.Parameter(typeof(int)),
                Expression.Parameter(typeof(int), "a"),
                Expression.Parameter(typeof(int), "ab"),
                Expression.Parameter(typeof(int), "abc"),

                ((Expression<Func<DateTime>>)(() => DateTime.Now)).Body,
                ((Expression<Func<string, int>>)(s => s.Length)).Body,

                ((Expression<Func<TimeSpan>>)(() => new TimeSpan())).Body,
                ((Expression<Func<long, DateTime>>)(t => new DateTime(t))).Body,
                ((Expression<Func<List<int>>>)(() => new List<int>())).Body,

                ((Expression<Func<int, int[]>>)(x => new int[x])).Body,
                ((Expression<Func<int, int, int[,]>>)((x, y) => new int[x, y])).Body,
#pragma warning disable IDE0079 // Next supression flagged as redundant on .NET SDK 6
#pragma warning disable CA1825 // Avoid zero-length array allocations (use in expression tree)
                ((Expression<Func<int[]>>)(() => new int[] {})).Body,
#pragma warning restore CA1825 // Avoid zero-length array allocations
#pragma warning restore IDE0079
                ((Expression<Func<int, int[]>>)(x => new int[] { x })).Body,
                ((Expression<Func<int, int, int[]>>)((x, y) => new int[] { x, y })).Body,

                ((Expression<Func<string>>)(() => Console.ReadLine())).Body,
                ((Expression<Func<string, string>>)(s => s.ToUpper())).Body,
#pragma warning disable IDE0057 // Use range operator (https://github.com/dotnet/roslyn/issues/49347)
                ((Expression<Func<string, int, string>>)((s, i) => s.Substring(i))).Body,
#pragma warning restore IDE0057 // Use range operator
                ((Expression<Func<string, int, int, string>>)((s, i, j) => s.Substring(i, j))).Body,

                ((Expression<Func<int, int>>)(x => -x)).Body,
                ((Expression<Func<int, int>>)(x => ~x)).Body,
                ((Expression<Func<bool, bool>>)(x => !x)).Body,
                ((Expression<Func<TimeSpan, TimeSpan>>)(t => -t)).Body,

                ((Expression<Func<int, int, int>>)((x, y) => x + y)).Body,
                ((Expression<Func<bool, bool, bool>>)((x, y) => x && y)).Body,
                ((Expression<Func<string, string, string>>)((x, y) => x ?? y)).Body,

                ((Expression<Func<bool, string, string, string>>)((b, x, y) => b ? x : y)).Body,

                ((Expression<Func<int, List<int>>>)(x => new List<int> { x })).Body,
                ((Expression<Func<int, int, List<int>>>)((x, y) => new List<int> { x, y })).Body,

                //(Expression<Func<AppDomainSetup>>)(() => new AppDomainSetup { }),
                //(Expression<Func<string, AppDomainSetup>>)(s => new AppDomainSetup { ApplicationName = s }),

                (Expression<Func<int, int, int, int, Bar>>)((x, y, z, a) => new Bar { X = x, Y = { A = a }, Zs = { y, z } }),

                (Expression<Func<Func<int, int>, int, int>>)((f, x) => f(x)),

                (Expression<Func<int, int>>)(x => x),

                ((Expression<Func<int, int, IEnumerable<int>>>)((x, y) => Enumerable.Range(x, y))).Body,
                ((Expression<Func<int, int, IEnumerable<int>>>)((x, y) => Enumerable.Range(x, y).Where(i => true))).Body,
                ((Expression<Func<int, int, IEnumerable<int>>>)((x, y) => Enumerable.Range(x, y).Where(i => true).Select(i => i))).Body,

                ((Expression<Func<int, int, IQueryable<int>>>)((x, y) => Enumerable.Range(x, y).AsQueryable())).Body,
                ((Expression<Func<int, int, IQueryable<int>>>)((x, y) => Enumerable.Range(x, y).AsQueryable().Where(i => true))).Body,
                ((Expression<Func<int, int, IQueryable<int>>>)((x, y) => Enumerable.Range(x, y).AsQueryable().Where(i => true).Select(i => i))).Body,
            };

            foreach (var e in es)
            {
                var slim = e.ToExpressionSlim();

                var bj = Encoding.UTF8.GetBytes(serj.Serialize(slim));

#if USE_SLIM
                var bb = serb.Serialize(slim);
                serb.Deserialize(bb);
#else
                var bb = serb.Serialize(e);
#endif

                Console.WriteLine(bj.Length + "\t" + bb.Length + "\t" + (double)bb.Length / bj.Length + "\t" + e.ToString());
            }
        }

        private static void Perf(bool useLegacy = false)
        {
            if (useLegacy)
            {
                while (true)
                {
                    Binary();
                    Json();
                }
            }
            else
            {
                _ = BenchmarkRunner.Run<Serialize>();
                _ = BenchmarkRunner.Run<Deserialize>();
            }
        }

        private static void Json()
        {
            GC.Collect();

            Console.WriteLine("JSON");

            var ser = new CustomBonsaiSerializer();

            var N = 10000;
            var M = 10000;

            var e = (Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where(x => x > 0).Select(x => x * x));

            Console.WriteLine("  Serialize");

            var sw = Stopwatch.StartNew();
            var gc = GarbageCollectionWatch.StartNew();

            var res = default(string);

            for (var i = 0; i < N; i++)
            {
                res = ser.Serialize(e.ToExpressionSlim());
            }

            Console.WriteLine("    Elapsed (ms): " + sw.Elapsed.TotalMilliseconds / M);
            Console.WriteLine("    " + gc.Elapsed);
            Console.WriteLine("    Length (bytes): " + res.Length * 2);

            Console.WriteLine("  Deserialize");

            sw.Restart();
            gc.Restart();

            for (var i = 0; i < M; i++)
            {
                e = (Expression<Func<IEnumerable<int>>>)ser.Deserialize(res).ToExpression();
            }

            Console.WriteLine("    Elapsed (ms): " + sw.Elapsed.TotalMilliseconds / M);
            Console.WriteLine("    " + gc.Elapsed);

            Console.WriteLine();
        }

        private static void Binary()
        {
            GC.Collect();

            Console.WriteLine("Binary");

            var ser = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

            var N = 10000;
            var M = 10000;

            var e = (Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where(x => x > 0).Select(x => x * x));

            Console.WriteLine("  Serialize");

            var sw = Stopwatch.StartNew();
            var gc = GarbageCollectionWatch.StartNew();

            var res = default(byte[]);

            for (var i = 0; i < N; i++)
            {
#if USE_SLIM
                res = ser.Serialize(e.ToExpressionSlim());
#else
                res = ser.Serialize(e);
#endif
            }

            Console.WriteLine("    Elapsed (ms): " + sw.Elapsed.TotalMilliseconds / M);
            Console.WriteLine("    " + gc.Elapsed);
            Console.WriteLine("    Length (bytes): " + res.Length);

            Console.WriteLine("  Deserialize");

            sw.Restart();
            gc.Restart();

            for (var i = 0; i < M; i++)
            {
#if USE_SLIM
                e = (Expression<Func<IEnumerable<int>>>)ser.Deserialize(res).ToExpression();
#else
                e = (Expression<Func<IEnumerable<int>>>)ser.Deserialize(res);
#endif
            }

            Console.WriteLine("    Elapsed (ms): " + sw.Elapsed.TotalMilliseconds / M);
            Console.WriteLine("    " + gc.Elapsed);

            Console.WriteLine();
        }

#if !USE_SLIM
        private static void Old()
        {
            var x = Expression.Parameter(typeof(int));
            var i = typeof(Interlocked).GetMethods().Single(m => m.Name == "Exchange" && m.GetParameters().Last().ParameterType == typeof(int));

            Expression.Block(new[] { x }, Expression.Call(instance: null, i, x, Expression.Constant(1)));

            var ser = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

            var inp = Expression.Constant(42);
            var arr = ser.Serialize(inp);
            var otp = ser.Deserialize(arr);

            var equ = new ExpressionEqualityComparer();

            Console.WriteLine(equ.Equals(inp, otp));
        }
#endif
    }

    internal class Bar
    {
        public int X;
        public Qux Y { get; set; }
        public List<int> Zs { get; set; }
    }

    internal class Qux
    {
        public int A { get; set; }
    }

    internal class GarbageCollectionWatch
    {
        private int _gen0, _gen1, _gen2;

        public static GarbageCollectionWatch StartNew()
        {
            var res = new GarbageCollectionWatch();
            res.Start();
            return res;
        }

        public void Start()
        {
            _gen0 = GC.CollectionCount(0);
            _gen1 = GC.CollectionCount(1);
            _gen2 = GC.CollectionCount(2);
        }

        public void Restart()
        {
            Start();
        }

        public GarbageCollectionStats Elapsed => new(GC.CollectionCount(0) - _gen0, GC.CollectionCount(1) - _gen1, GC.CollectionCount(2) - _gen2);
    }

    internal readonly struct GarbageCollectionStats
    {
        public GarbageCollectionStats(int gen0, int gen1, int gen2)
        {
            Gen0 = gen0;
            Gen1 = gen1;
            Gen2 = gen2;
        }

        public int Gen0 { get; }
        public int Gen1 { get; }
        public int Gen2 { get; }

        public override string ToString() => $"Gen0: {Gen0}  Gen1: {Gen1}  Gen2: {Gen2}";
    }

    internal class CustomBonsaiSerializer : BonsaiExpressionSerializer
    {
        private static readonly BinaryFormatter s_formatter = new();

        protected override Func<Nuqleon.Json.Expressions.Expression, object> GetConstantDeserializer(Type type)
        {
            return e => Deserialize(type, e);
        }

        protected override Func<object, Nuqleon.Json.Expressions.Expression> GetConstantSerializer(Type type)
        {
            return o => Serialize(type, o);
        }

        private static Nuqleon.Json.Expressions.Expression Serialize(Type t, object o)
        {
            _ = t;
            var ms = new MemoryStream();
#pragma warning disable SYSLIB0011 // Type or member is obsolete - this pearl is a historical illustration, so its use of BinaryFormatter is the point
            s_formatter.Serialize(ms, o);
#pragma warning restore SYSLIB0011
            return Nuqleon.Json.Expressions.Expression.String(Convert.ToBase64String(ms.ToArray()));
        }

        private static object Deserialize(Type t, Nuqleon.Json.Expressions.Expression j)
        {
            _ = t;
            var bs = Convert.FromBase64String((string)((Nuqleon.Json.Expressions.ConstantExpression)j).Value);
            var ms = new MemoryStream(bs);
#pragma warning disable SYSLIB0011 // Type or member is obsolete - this pearl is a historical illustration, so its use of BinaryFormatter is the point
            return s_formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
        }
    }
}
