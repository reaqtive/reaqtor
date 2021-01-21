// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// A small CSE implementation using classic Rx
//
// BD - September 2014
//

//
// Design notes:
//
//   This project illustrates the core concepts of common subexpression elimination (CSE)
//   in the context of reactive event processing. The goal of applying CSE is to reduce
//   redundant computation by sharing intermediate results using a subject. Consider the
//   following two queries:
//
//     var d1 = xs.Where(x => x > 0).Select(x => x * x).Subscribe(o1);
//     var d2 = xs.Where(x => x > 0).Select(x => x + 1).Subscribe(o2);
//              ^^^^^^^^^^^^^^^^^^^^
//
//   In here, the underlined part is the same. Upon receiving the first query, various
//   rules can decide on the sharing likelihood of the underlined portion (and for that
//   matter bigger chunks of the query). We leave this decision process open-ended in this
//   example, but it could be fed by metrics (beyond classic cardinality estimation given
//   this is a streaming system, so dV/dt is what matters), domain-specific knowledge,
//   various algebraic rewrite rules, etc.
//
//   Upon determining subexpressions whose results can be shared, a check is made to see
//   whether a subject exposing that portion's evaluation results already exists. If it
//   does - say it's called cse1 - the expressions would get rewritten to:
//
//     var d1 = cse1.Select(x => x * x).Subscribe(o1);
//     var d2 = cse1.Select(x => x + 1).Subscribe(o2);
//
//   If a node matching the common subexpression doesn't exist yet, the query is split
//   into a set of operations. Consider the first query:
//
//     var d1 = xs.Where(x => x > 0).Select(x => x * x).Subscribe(o1);
//              ^^^^^^^^^^^^^^^^^^^^
//
//   First, the CSE candidate is backed by a new subject:
//
//     var cse1 = new Subject<T>();
//
//   Second, the new subject is added to a dictionary so it can be looked up for reuse:
//
//     registry[E(xs.Where(x => x > 0))] = cse1; // where E(...) quotes the expression
//
//   Third, the downstream portion of the query is established:
//
//     var i1 = cse1.Select(x => x * x).Subscribe(o1);
//
//   Fourth, the upstream portion of the query is established:
//
//     var i2 = xs.Where(x => x > 0).Subscribe(cse1);
//
//   Finally, i2 is decorated with a ref count mechanism (detailed omitted here) and bundled
//   up with i1 to comprise the user's subscription handle. When the ref count for the
//   subject drops to zero, it can be reclaimed from the dictionary. It goes without saying
//   that this mechanism needs to be concurrency safe.
//

using System;
using System.Diagnostics;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    public class Program
    {
        public static void Main()
        {
            Demo1();
            Demo2();
            Perf();
        }

        /// <summary>
        /// Quick perf test.
        /// </summary>
        private static void Perf()
        {
            var sub = new Subject<int>("xs");

            var cout = Observer.Create<int>(
                x => { },
                ex => { },
                () => { }
            );

            var r = new Registry
            {
                { "my://xs", new Entry { Expression = Expression.Lambda<Func<IObservable<int>>>(Expression.Constant(sub, typeof(IObservable<int>))), CanShare = true, IsSubject = true } },
                { "my://o", new Entry { Expression = Expression.Lambda<Func<IObserver<int>>>(Expression.Constant(cout, typeof(IObserver<int>))), CanShare = false } },
                { "rx://operators/filter", new Entry { Expression = (Expression<Func<IObservable<T>, Func<T, bool>, IObservable<T>>>)((xs, f) => xs.Where(f)), CanShare = true } },
                { "rx://operators/map", new Entry { Expression = (Expression<Func<IObservable<T>, Func<T, R>, IObservable<R>>>)((xs, f) => xs.Select(f)), CanShare = true } },
                { "rx://operators/take", new Entry { Expression = (Expression<Func<IObservable<T>, int, IObservable<T>>>)((xs, n) => xs.Take(n)), CanShare = false } },
            };

            var svc = new Service(r)
            {
                //Logger = new ConsoleLogger()
            };

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var N = 10000;
            var opt = false;

            Console.WriteLine("Optimized = " + opt);
            Console.WriteLine("N = " + N);

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < N; i++)
            {
                var d = CreateSubscription(
                    svc,
                    () => Xs().Where(x => x > 0).Select(x => x * x).Take(int.MaxValue).Subscribe(O()),
                    opt
                );
            }

            Console.WriteLine("Time = " + sw.Elapsed);
            Console.WriteLine("Memory = " + GC.GetTotalMemory(forceFullCollection: true));

            sw.Restart();

            Console.WriteLine("M = " + N);

            var M = 10000;

            for (var i = 1; i <= M; i++)
            {
                sub.OnNext(i);
            }

            Console.WriteLine("Time = " + sw.Elapsed);
            Console.WriteLine("Memory = " + GC.GetTotalMemory(forceFullCollection: true));
        }

        /// <summary>
        /// Illustrates normalization of expressions to increase sharing likelihood.
        /// </summary>
        private static void Demo1()
        {
            var sub = new Subject<int>("xs");

            var cout = Observer.Create<int>(
                x => Console.WriteLine(x),
                ex => { },
                () => { }
            );

            var r = new Registry
            {
                { "my://xs", new Entry { Expression = Expression.Lambda<Func<IObservable<int>>>(Expression.Constant(sub, typeof(IObservable<int>))), CanShare = true, IsSubject = true } },
                { "my://o", new Entry { Expression = Expression.Lambda<Func<IObserver<int>>>(Expression.Constant(cout, typeof(IObserver<int>))), CanShare = false } },
                { "rx://operators/filter", new Entry { Expression = (Expression<Func<IObservable<T>, Func<T, bool>, IObservable<T>>>)((xs, f) => xs.Where(f)), CanShare = true } },
                { "rx://operators/map", new Entry { Expression = (Expression<Func<IObservable<T>, Func<T, R>, IObservable<R>>>)((xs, f) => xs.Select(f)), CanShare = true } },
                { "rx://operators/take", new Entry { Expression = (Expression<Func<IObservable<T>, int, IObservable<T>>>)((xs, n) => xs.Take(n)), CanShare = false } },
            };

            var svc = new Service(r)
            {
                Logger = new ConsoleLogger()
            };

            //
            // All of these get normalized to the same expression.
            //

            Console.WriteLine("1>");
            var d1 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Where(x => x % 2 == 0).Select(x => x * x).Take(10).Take(7).Subscribe(O()));
            Console.WriteLine("2>");
            var d2 = CreateSubscription(svc, () => Xs().Where(x => x % 2 == 0).Where(x => x > 0).Select(x => x * x).Take(10).Take(7).Subscribe(O()));
            //Console.WriteLine("3>");
            //var d3 = CreateSubscription(svc, () => Xs().Where(x => x % 2 == 0).Where(x => x < 0).Select(x => x * x).Take(10).Take(7).Subscribe(O()));
            Console.WriteLine("4>");
            var d4 = CreateSubscription(svc, () => Xs().Where(x => !(x <= 0)).Where(x => !(x % 2 != 0)).Select(x => x * x).Take(10).Take(7).Subscribe(O()));
            Console.WriteLine("5>");
            var d5 = CreateSubscription(svc, () => Xs().Where(x => x > 0 && x % 2 == 0).Select(x => x * x).Take(10).Take(7).Subscribe(O()));
            Console.WriteLine("6>");
            var d6 = CreateSubscription(svc, () => Xs().Where(x => !(!(x > 0) || !(x % 2 == 0))).Select(x => x * x).Take(10).Take(7).Subscribe(O()));
            Console.WriteLine("7>");
            var d7 = CreateSubscription(svc, () => Xs().Where(x => 0 < x && 0 == x % 2).Select(x => x * x).Take(10).Take(7).Subscribe(O()));

            sub.OnNext(1);
            sub.OnNext(2);
            sub.OnNext(3);
            sub.OnCompleted();
        }

        /// <summary>
        /// Illustrates reuse of common sub-expressions and refcounting of intermediaries.
        /// </summary>
        private static void Demo2()
        {
            var sub = new Subject<int>("xs");

            var cout = Observer.Create<int>(
                x => Console.WriteLine(x),
                ex => { },
                () => { }
            );

            var r = new Registry
            {
                { "my://xs", new Entry { Expression = Expression.Lambda<Func<IObservable<int>>>(Expression.Constant(sub, typeof(IObservable<int>))), CanShare = true, IsSubject = true } },
                { "my://o", new Entry { Expression = Expression.Lambda<Func<IObserver<int>>>(Expression.Constant(cout, typeof(IObserver<int>))), CanShare = false } },
                { "rx://operators/filter", new Entry { Expression = (Expression<Func<IObservable<T>, Func<T, bool>, IObservable<T>>>)((xs, f) => xs.Where(f)), CanShare = true } },
                { "rx://operators/map", new Entry { Expression = (Expression<Func<IObservable<T>, Func<T, R>, IObservable<R>>>)((xs, f) => xs.Select(f)), CanShare = true } },
                { "rx://operators/take", new Entry { Expression = (Expression<Func<IObservable<T>, int, IObservable<T>>>)((xs, n) => xs.Take(n)), CanShare = false } },
            };

            var svc = new Service(r)
            {
                Logger = new ConsoleLogger()
            };

            //
            // All of these share stuff.
            //

            Console.WriteLine("1>");
            var d1 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Select(x => x * x).Subscribe(O()));
            Console.WriteLine("2>");
            var d2 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Select(x => x + 1).Subscribe(O()));
            Console.WriteLine("3>");
            var d3 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Select(x => x * x).Subscribe(O()));
            Console.WriteLine("4>");
            var d4 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Select(x => x + 1).Select(x => x - 1).Subscribe(O()));
            Console.WriteLine("5>");
            var d5 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Subscribe(O()));
            Console.WriteLine("6>");
            var d6 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Take(10).Subscribe(O())); // NOTE: for n == 3, this will crash right now on collection mutation in Subject<T>
            Console.WriteLine("7>");
            var d7 = CreateSubscription(svc, () => Xs().Where(x => x > 0).Take(10).Subscribe(O()));

            sub.OnNext(1);
            sub.OnNext(2);
            sub.OnNext(3);
            sub.OnCompleted();

            //
            // Effects of refcounting.
            //

            d5.Dispose();
            d3.Dispose();
            d1.Dispose();
            d6.Dispose();
            d4.Dispose();
            d7.Dispose();
            d2.Dispose();
        }

        /// <summary>
        /// Creates a subscription in the specified service using the given expression.
        /// </summary>
        /// <param name="svc">Service to create the subscription in.</param>
        /// <param name="subscribe">Expression representing a subscription.</param>
        /// <param name="optimize">Enables or disables CSE optimizations.</param>
        /// <returns>Disposable resource used to cancel the subscription.</returns>
        private static IDisposable CreateSubscription(Service svc, Expression<Func<IDisposable>> subscribe, bool optimize = true)
        {
            var n = new Normalizer().Visit(subscribe);

            if (optimize)
            {
                var o = new QueryOperatorOptimizer().Optimize(n); // TODO: relocate in service front-end?
                return svc.CreateSubscription(o);
            }
            else
            {
                return svc.CreateSubscription(n, share: false);
            }
        }

        // NOTE: the below is a gross simplification of the well-understood mechanisms to compose queries using proxies.

        /// <summary>
        /// Accessor for the observable side of the subject identified as "my://xs".
        /// </summary>
        /// <returns>Always throws an exception; meant to used within an expression.</returns>
        [KnownResource("my://xs")]
        private static IObservable<int> Xs()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Accessor for the observer identified as "my://o".
        /// </summary>
        /// <returns>Always throws an exception; meant to used within an expression.</returns>
        [KnownResource("my://o")]
        private static IObserver<int> O()
        {
            throw new NotImplementedException();
        }
    }
}
