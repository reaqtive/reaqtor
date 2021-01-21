// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.ExceptionServices;
using System.Threading;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Tasks;

using Reaqtor.Reliable;

namespace Tests.Reaqtor.QueryEngine
{
    internal static class TestObserverExtensions
    {
        public static IObserver<T> Synchronize<T>(this IObserver<T> observer, IScheduler scheduler)
        {
            return new SynchronizedObserver<T>(observer, scheduler);
        }

        public static IReliableObserver<T> Synchronize<T>(this IReliableObserver<T> observer, IScheduler scheduler)
        {
            return new SynchronizedReliableObserver<T>(observer, scheduler);
        }

        private sealed class SynchronizedObserver<T> : ObserverBase<T>
        {
            private readonly IObserver<T> _observer;
            private readonly IScheduler _scheduler;

            public SynchronizedObserver(IObserver<T> observer, IScheduler scheduler)
            {
                _observer = observer;
                _scheduler = scheduler;
            }

            protected override void OnCompletedCore()
            {
                Run(() => _observer.OnCompleted());
            }

            protected override void OnErrorCore(Exception error)
            {
                Run(() => _observer.OnError(error));
            }

            protected override void OnNextCore(T value)
            {
                Run(() => _observer.OnNext(value));
            }

            private void Run(Action a)
            {
                var e = new ManualResetEventSlim(false);
                var err = default(Exception);

                _scheduler.Schedule(new ActionTask(() =>
                {
                    try
                    {
                        a();
                    }
                    catch (Exception ex)
                    {
                        err = ex;
                    }
                    finally
                    {
                        e.Set();
                    }
                }));

                e.Wait();

                if (err != null)
                {
                    ExceptionDispatchInfo.Capture(err).Throw();
                }
            }
        }

        private sealed class SynchronizedReliableObserver<T> : IReliableObserver<T>
        {
            private readonly IReliableObserver<T> _observer;
            private readonly IScheduler _scheduler;

            public SynchronizedReliableObserver(IReliableObserver<T> observer, IScheduler scheduler)
            {
                _observer = observer;
                _scheduler = scheduler;
            }

            public void OnNext(T item, long sequenceId)
            {
                Run(() => _observer.OnNext(item, sequenceId));
            }

            public void OnError(Exception error)
            {
                Run(() => _observer.OnError(error));
            }

            public void OnCompleted()
            {
                Run(() => _observer.OnCompleted());
            }

            private void Run(Action a)
            {
                var e = new ManualResetEventSlim(false);
                var err = default(Exception);

                _scheduler.Schedule(new ActionTask(() =>
                {
                    try
                    {
                        a();
                    }
                    catch (Exception ex)
                    {
                        err = ex;
                    }
                    finally
                    {
                        e.Set();
                    }
                }));

                e.Wait();

                if (err != null)
                {
                    ExceptionDispatchInfo.Capture(err).Throw();
                }
            }

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnStarted()
            {
            }
        }
    }
}
