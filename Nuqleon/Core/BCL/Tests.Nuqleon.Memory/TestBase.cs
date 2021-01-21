// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created test base class.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Tests
{
    public class TestBase
    {
        private readonly ThreadLocal<Random> _random;

        public TestBase()
        {
            var rnd = new Random();

            _random = new ThreadLocal<Random>(() =>
            {
                lock (rnd)
                {
                    return new Random(rnd.Next() * 17);
                }
            });
        }

        protected static TimeSpan Run<T, R>(Func<T> alloc, Func<T, R> extract, Action<T> release, Action<R> action, int threadCount, int iterationCount, bool noisy = false)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var gen0 = GC.CollectionCount(0);
            var gen1 = GC.CollectionCount(1);
            var gen2 = GC.CollectionCount(2);

            var stopped = false;

            var tr = new Thread(() =>
            {
                if (noisy)
                {
                    var mem = new List<byte[]>();
                    while (!Volatile.Read(ref stopped))
                    {
                        mem.Add(new byte[1024]);
                    }
                }
            });

            tr.Start();

            var n = 0;
            var r = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref n) * 17));

            var N = threadCount;
            var M = iterationCount;

            var cd = new CountdownEvent(N);

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < N; i++)
            {
                new Thread(() =>
                {
                    for (var j = 0; j < M; j++)
                    {
                        var t = alloc();
                        action(extract(t));
                        release(t);
                    }

                    cd.Signal();
                }).Start();
            }

            cd.Wait();

            sw.Stop();

            Volatile.Write(ref stopped, true);
            tr.Join();

            gen0 = GC.CollectionCount(0) - gen0;
            gen1 = GC.CollectionCount(1) - gen1;
            gen2 = GC.CollectionCount(2) - gen2;

            return sw.Elapsed;
        }

        protected Random GetRandom()
        {
            return _random.Value;
        }

        protected static void TooBig<T, R>(Func<T> alloc, Func<T, R> getInstance, Action<R, int> bloat, int threshold)
            where T : IDisposable
            where R : class
        {
            var res1 = alloc();
            var it1 = getInstance(res1);
            bloat(it1, threshold - 1);
            res1.Dispose();

            var res2 = alloc();
            var it2 = getInstance(res2);
            res2.Dispose();

            Assert.AreSame(it1, it2);

            var res3 = alloc();
            var it3 = getInstance(res3);
            bloat(it3, threshold + 1);
            res3.Dispose();

            var res4 = alloc();
            var it4 = getInstance(res4);
            res4.Dispose();

            Assert.AreNotSame(it3, it4);
            Assert.AreNotSame(it1, it4);
        }
    }
}
