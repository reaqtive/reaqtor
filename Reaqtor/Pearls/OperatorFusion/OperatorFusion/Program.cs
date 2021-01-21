// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Demonstration of operator fusion.
//
// BD - October 2014
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace OperatorFusion
{
    class Program
    {
        static void Main()
        {
            New();

            Old();
            Tests();
        }

        static void New()
        {
            var res = QueryLanguage
                .Source<int>("xs")
                .Where(x => x > 0)
                .Where((x, i) => x != 0 && i < int.MaxValue)
                .DistinctUntilChanged()
                .Select(x => x * 2)
                .Select((x, i) => x * 3 + i)
                .Take(1000)
                .Sum()
                .FirstOrDefault()
                .Sink("o");

            var op = res.Compile<int>();

            var xs = new Subject<int>();

            xs.OnNext(1);
            xs.OnNext(2);

            var d = op(xs).Subscribe(x => Console.WriteLine(x));

            xs.OnNext(3);
            xs.OnNext(4);
            xs.OnNext(5);
            xs.OnNext(6);
            xs.OnCompleted();

            d.Dispose();
        }

        static void Tests()
        {
            Where();
            WhereIndexed();

            Select();
            SelectIndexed();

            Take();

            DistinctUntilChanged();

            Count();
            LongCount();

            SumInt32();

            First();
            First_Empty();

            Last();
            Last_Empty();
        }

        static void Where()
        {
            var wf = new WhereFactory
            {
                Predicate = (Expression<Func<int, bool>>)(x => x > 0)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                wf, // .Where(x => x > 0)
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(-1);
            iv.OnNext(0);
            iv.OnNext(-2);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(-3);
            iv.OnNext(4);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 1, 2, 4 }));
            Assert(err == null);
            Assert(fin);
        }

        static void WhereIndexed()
        {
            var wf = new WhereIndexedFactory
            {
                Predicate = (Expression<Func<int, int, bool>>)((x, i) => i % 2 == 0)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                wf, // .Where((x, i) => i % 2 == 0)
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(-1);
            iv.OnNext(0);
            iv.OnNext(-2);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(-3);
            iv.OnNext(4);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { -1, -2, 2, 4 }));
            Assert(err == null);
            Assert(fin);
        }

        static void Select()
        {
            var sf = new SelectFactory
            {
                Selector = (Expression<Func<int, int>>)(x => x * x)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                sf, // .Select(x => x * x)
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 0, 1, 4, 9 }));
            Assert(err == null);
            Assert(fin);
        }

        static void SelectIndexed()
        {
            var sf = new SelectIndexedFactory
            {
                Selector = (Expression<Func<int, int, int>>)((x, i) => x * x + i)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                sf, // .Select((x, i) => x * x + i)
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnNext(4);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 1 + 0, 4 + 1, 9 + 2, 16 + 3 }));
            Assert(err == null);
            Assert(fin);
        }

        static void Take()
        {
            var tf = new TakeFactory
            {
                Count = 3,
                OutputType = typeof(int)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                tf, // .Take(3)
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnNext(4);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 0, 1, 2 }));
            Assert(err == null);
            Assert(fin);
        }

        static void DistinctUntilChanged()
        {
            var df = new DistinctUntilChangedFactory
            {
                OutputType = typeof(int)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                df, // .DistinctUntilChanged()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(1);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(2);
            iv.OnNext(1);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 0, 1, 2, 1, 3 }));
            Assert(err == null);
            Assert(fin);
        }

        static void Count()
        {
            var cf = new CountFactory();

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                cf, // .Count()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 4 }));
            Assert(err == null);
            Assert(fin);
        }

        static void LongCount()
        {
            var cf = new LongCountFactory();

            var ff = new SinkFactory
            {
                OutputType = typeof(long)
            };

            var chain = new IFusionOperator[]
            {
                cf, // .LongCount()
                ff
            };

            var ivt = Compile(typeof(long), chain);

            var res = new List<long>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<long>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<long>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 4L }));
            Assert(err == null);
            Assert(fin);
        }

        static void SumInt32()
        {
            var sf = new SumFactory(typeof(int));

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                sf, // .Sum()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 6 }));
            Assert(err == null);
            Assert(fin);
        }

        static void First()
        {
            var af = new FirstFactory
            {
                OutputType = typeof(int)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                af, // .First()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 0 }));
            Assert(err == null);
            Assert(fin);
        }

        static void First_Empty()
        {
            var af = new FirstFactory
            {
                OutputType = typeof(int)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                af, // .First()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnCompleted();

            Assert(res.Count == 0);
            Assert(err is InvalidOperationException);
            Assert(!fin);
        }

        static void Last()
        {
            var lf = new LastFactory
            {
                OutputType = typeof(int)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                lf, // .Last()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnNext(0);
            iv.OnNext(1);
            iv.OnNext(2);
            iv.OnNext(3);
            iv.OnCompleted();

            Assert(res.SequenceEqual(new[] { 3 }));
            Assert(err == null);
            Assert(fin);
        }

        static void Last_Empty()
        {
            var lf = new LastFactory
            {
                OutputType = typeof(int)
            };

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                lf, // .Last()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            var res = new List<int>();
            var err = default(Exception);
            var fin = false;

            var otp = Observer.Create<int>(res.Add, ex => err = ex, () => fin = true);
            var dsp = System.Reactive.Disposables.Disposable.Empty;

            var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { otp, dsp });

            iv.OnCompleted();

            Assert(res.Count == 0);
            Assert(err is InvalidOperationException);
            Assert(!fin);
        }

        static void Assert(bool b, string s = "")
        {
            if (!b)
                throw new InvalidOperationException(s);
        }

        static void Old()
        {
            // TODO: implement a few operators in terms of others and see fusion emit the same code
            //       e.g. xs.Last() == xs.Aggreggate((a, x) => x)

            //
            // Aggregate
            // - Count/LongCount
            // Average/Sum/Min/Max
            // Any/All/IsEmpty
            // - First
            // - Last
            // Single
            // Contains
            // ElementAt
            // - IgnoreElements
            // Scan
            // Sample
            // - Take
            // Skip
            // TakeWhile/SkipWhile
            // Finally
            //
            // CombineLatest
            // SequenceEqual
            // TakeUntil
            //
            // SkipUntil
            // StartWith
            //
            // Merge
            // SelectMany
            // Switch
            //

            var wf1 = new WhereFactory
            {
                Predicate = (Expression<Func<int, bool>>)(x => x > 0)
            };

            var wf2 = new WhereIndexedFactory
            {
                Predicate = (Expression<Func<int, int, bool>>)((x, i) => x != 0 && i < int.MaxValue)
            };

            var df = new DistinctUntilChangedFactory
            {
                OutputType = typeof(int)
            };

            var sf1 = new SelectFactory
            {
                Selector = (Expression<Func<int, int>>)(x => x * 2)
            };

            var sf2 = new SelectIndexedFactory
            {
                Selector = (Expression<Func<int, int, int>>)((x, i) => x * 3 + i)
            };

            var tf1 = new TakeFactory
            {
                OutputType = typeof(int),
                Count = 1000
            };

            var tf2 = new TakeFactory
            {
                OutputType = typeof(int),
                Count = 2000
            };

            var af = new SumFactory(typeof(int));

            var ff = new SinkFactory
            {
                OutputType = typeof(int)
            };

            var chain = new IFusionOperator[]
            {
                wf1, // .Where(x => x > 0)
                wf2, // .Where((x, i) => x != 0 && i < int.MaxValue)
                df,  // .DistinctUntilChanged()
                sf1, // .Select(x => x * 2)
                sf2, // .Select((x, i) => x * 3 + i)
                tf1, // .Take(1000)
                tf2, // .Take(2000)
                af,  // .Sum()
                ff
            };

            var ivt = Compile(typeof(int), chain);

            Test(ivt);
        }

        static void Test(Type ivt)
        {
            var time1 = default(TimeSpan);
            var size1 = default(long);
            var time2 = default(TimeSpan);
            var size2 = default(long);

            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var stuff = new HashSet<object>();

                var sw = Stopwatch.StartNew();

                for (var n = 0; n < 20000; n++)
                {
                    var cout = new ConsoleObserver<int>(true);
                    var disp = new Disposable(true);

                    var iv = (IObserver<int>)Activator.CreateInstance(ivt, new object[] { cout, disp });

                    for (var i = -5000; i <= 5000; i++)
                    {
                        iv.OnNext(i);
                    }

                    iv.OnCompleted();

                    stuff.Add(iv);
                }

                time1 = sw.Elapsed;
                size1 = GC.GetTotalMemory(forceFullCollection: true);

                Console.WriteLine(time1 + " " + size1);

                GC.KeepAlive(stuff);
            }

            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var stuff = new HashSet<object>();

                var sw = Stopwatch.StartNew();

                for (var n = 0; n < 20000; n++)
                {
                    var cout = new ConsoleObserver<int>(true);
                    var disp = new Disposable(true);

                    var iv = new Subject<int>();
                    var d = iv
                        .Where(x => x > 0)
                        .Where((x, i) => x != 0 && i < int.MaxValue)
                        .DistinctUntilChanged()
                        .Select(x => x * 2)
                        .Select((x, i) => x * 3 + i)
                        .Take(1000)
                        .Take(2000)
                        .Sum()
                        .Subscribe(cout);

                    for (var i = -5000; i <= 5000; i++)
                    {
                        iv.OnNext(i);
                    }

                    iv.OnCompleted();

                    stuff.Add(d);
                }

                time2 = sw.Elapsed;
                size2 = GC.GetTotalMemory(forceFullCollection: true);

                Console.WriteLine(time2 + " " + size2);

                GC.KeepAlive(stuff);
            }

            Console.WriteLine("Speed factor: " + (time1.TotalMilliseconds / time2.TotalMilliseconds));
            Console.WriteLine("Space factor: " + ((double)size1 / size2));
        }

        private static Type Compile(Type type, IFusionOperator[] chain)
        {
            return Compiler.Compile(type, chain);
        }
    }
}
