// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices.TypeSystem;
using System.Threading;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor;
using Reaqtor.QueryEngine.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    public abstract class PhysicalTimeEngineTest : QueryEngineTest
    {
        protected static readonly Uri MockObservableUri = new("io:/MockObservable");
        protected static readonly Uri MockObserverUri = new("iv:/MockObserver");
        protected static readonly Uri SynchronizeSubscribeUri = new("test://await/subscribe");
        protected static readonly Uri BlockSubscribeUri = new("test://block/subscribe");
        protected static readonly Uri AwaitDoUri = new("test://await/do");
        protected static readonly Uri BlockDoUri = new("test://block/do");
        protected static readonly Uri AwaitDisposeUri = new("test://await/dispose");

        private static PhysicalScheduler _physicalScheduler;
        private static LogicalScheduler _scheduler;

        public static void ClassSetup()
        {
            _physicalScheduler = PhysicalScheduler.Create();
            _scheduler = new LogicalScheduler(_physicalScheduler);
        }

        public static void ClassCleanup()
        {
            _scheduler.Dispose();
            _physicalScheduler.Dispose();
        }

        protected void Setup()
        {
            MockObservable.Clear();
            MockObserver.Clear();
            base.TestInitialize();
        }

        protected void Cleanup()
        {
            MockObservable.Clear();
            MockObserver.Clear();
            base.TestCleanup();
        }

        protected override void AddCommonDefinitions(IReactive ctx)
        {
            base.AddCommonDefinitions(ctx);

            ctx.DefineObservable<Tuple<string>, T>(MockObservableUri, t => MockObservable.CreateObservable<T>(t.Item1).AsQbservable(), null);
            ctx.DefineObserver<Tuple<string>, T>(MockObserverUri, t => MockObserver.CreateObserver<T>(t.Item1).AsQbserver(), null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, string>, T>(SynchronizeSubscribeUri, t => t.Item1.AsSubscribable().AwaitSubscribe(t.Item2).AsQbservable(), null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, string>, T>(BlockSubscribeUri, t => t.Item1.AsSubscribable().BlockSubscribe(t.Item2).AsQbservable(), null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, string, string, string>, T>(AwaitDoUri, t => t.Item1.AsSubscribable().AwaitDo(t.Item2, t.Item3, t.Item4).AsQbservable(), null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, string, string, string>, T>(BlockDoUri, t => t.Item1.AsSubscribable().BlockDo(t.Item2, t.Item3, t.Item4).AsQbservable(), null);
            ctx.DefineObservable<Tuple<IReactiveQbservable<T>, string>, T>(AwaitDisposeUri, t => t.Item1.AsSubscribable().AwaitDispose(t.Item2).AsQbservable(), null);
        }

        protected static void AssertObservableNotCreated<T>(string id)
        {
            var o = MockObservable.Get<T>(id);
            Assert.IsNull(o);
        }

        protected static void AssertObserverNotCreated<T>(string id)
        {
            var v = MockObserver.Get<T>(id);
            Assert.IsNull(v);
        }

        internal static MockObserver<T> GetObserver<T>(string id)
        {
            var v = MockObserver.Get<T>(id);
            Assert.IsNotNull(v);
            return v;
        }

        internal static MockObservable<T> GetObservable<T>(string id)
        {
            var v = MockObservable.Get<T>(id);
            Assert.IsNotNull(v);
            return v;
        }

        internal static MockObserver<T> GetMockObserver<T>(string id, bool validate = true)
        {
            var v = MockObserver.Get<T>(id);
            Assert.IsNotNull(v);

            if (validate)
            {
                Assert.IsFalse(v.Completed);
                Assert.IsFalse(v.Error);
                Assert.AreEqual(0, v.Values.Count);
            }

            return v;
        }

        internal static void AssertResult<T>(MockObserver<T> observer, int expectedCount, Action<int, T> validator)
        {
            Assert.AreEqual(expectedCount, observer.Values.Count);
            for (int i = 0; i < expectedCount; i++)
            {
                validator(i, observer.Values[i]);
            }
        }

        internal static void AssertResultSequence<T>(MockObserver<T> observer, IEnumerable<T> expected)
        {
            Assert.AreEqual(expected.Count(), observer.Values.Count);
            foreach (var (Expected, Actual) in expected.Zip(observer.Values, (e, a) => (Expected: e, Actual: a)))
            {
                Assert.AreEqual(Expected, Actual);
            }
        }

        internal static void AssertResultSequenceSet<T>(MockObserver<T> observer, IEnumerable<T> expected)
        {
            Assert.AreEqual(expected.Count(), observer.Values.Count);

            var obs = new HashSet<T>(observer.Values);

            Assert.IsTrue(obs.SetEquals(expected));
        }

        protected override IScheduler GetScheduler()
        {
            return _scheduler;
        }
    }

    public static class LockManager
    {
        private static readonly ConcurrentDictionary<string, CountdownEvent> _countdowns = new();
        private static readonly ConcurrentDictionary<string, EventWaitHandle> _locks = new();

        public static string NewAutoReset()
        {
            return NewLock(() => new AutoResetEvent(false));
        }

        public static string NewManualReset()
        {
            return NewLock(() => new ManualResetEvent(false));
        }

        public static string NewCountdownEvent(int initialCount)
        {
            var l = Guid.NewGuid().ToString();
            var c = new CountdownEvent(initialCount);

            if (!_countdowns.TryAdd(l, c))
            {
                throw new InvalidOperationException("Countdown already exists.");
            }

            return l;
        }

        public static string NewLock(Func<EventWaitHandle> getLock)
        {
            var l = Guid.NewGuid().ToString();

            if (!_locks.TryAdd(l, getLock()))
            {
                throw new InvalidOperationException("Lock already exists.");
            }

            return l;
        }

        public static void Signal(string lockName)
        {
            if (_locks.TryGetValue(lockName, out var e))
            {
                e.Set();
            }
            else if (_countdowns.TryGetValue(lockName, out var c))
            {
                c.Signal();
            }
            else
            {
                throw new InvalidOperationException("Lock does not exist.");
            }
        }

        public static bool Wait(string lockName, int timeout = -1)
        {
            if (_locks.TryGetValue(lockName, out var e))
            {
                return e.WaitOne(timeout);
            }
            else if (_countdowns.TryGetValue(lockName, out var c))
            {
                return c.Wait(timeout);
            }
            else
            {
                throw new InvalidOperationException("Lock does not exist.");
            }
        }
    }

    public static class TestExtensions
    {
        public static ISubscribable<T> AwaitSubscribe<T>(this ISubscribable<T> source, string lockName)
        {
            return new AwaitSubscribeSubscribable<T>(source, lockName);
        }

        private sealed class AwaitSubscribeSubscribable<T> : SubscribableBase<T>
        {
            private readonly ISubscribable<T> _source;
            private readonly string _lockName;

            public AwaitSubscribeSubscribable(ISubscribable<T> source, string lockName)
            {
                _source = source;
                _lockName = lockName;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<AwaitSubscribeSubscribable<T>, T>, IObserver<T>
            {
                public _(AwaitSubscribeSubscribable<T> o, IObserver<T> v)
                    : base(o, v)
                {
                }

                protected override void OnStart()
                {
                    try
                    {
                        base.OnStart();
                    }
                    finally
                    {
                        LockManager.Signal(Params._lockName);
                    }
                }

                protected override IEnumerable<ISubscription> OnSubscribe()
                {
                    return new ISubscription[] { Params._source.Subscribe(this) };
                }

                public void OnCompleted()
                {
                    Output.OnCompleted();
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    Output.OnError(error);
                    Dispose();
                }

                public void OnNext(T value)
                {
                    Output.OnNext(value);
                }
            }
        }

        public static ISubscribable<T> BlockSubscribe<T>(this ISubscribable<T> source, string lockName)
        {
            return new BlockSubscribeSubscribable<T>(source, lockName);
        }

        private sealed class BlockSubscribeSubscribable<T> : SubscribableBase<T>
        {
            private readonly ISubscribable<T> _source;
            private readonly string _lockName;

            public BlockSubscribeSubscribable(ISubscribable<T> source, string lockName)
            {
                _source = source;
                _lockName = lockName;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<BlockSubscribeSubscribable<T>, T>, IObserver<T>
            {
                public _(BlockSubscribeSubscribable<T> o, IObserver<T> v)
                    : base(o, v)
                {
                }

                protected override void OnStart()
                {
                    try
                    {
                        base.OnStart();
                    }
                    finally
                    {
                        LockManager.Wait(Params._lockName);
                    }
                }

                protected override IEnumerable<ISubscription> OnSubscribe()
                {
                    return new ISubscription[] { Params._source.Subscribe(this) };
                }

                public void OnCompleted()
                {
                    Output.OnCompleted();
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    Output.OnError(error);
                    Dispose();
                }

                public void OnNext(T value)
                {
                    Output.OnNext(value);
                }
            }
        }

        public static ISubscribable<T> AwaitDo<T>(this ISubscribable<T> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
        {
            return new AwaitDoSubscribable<T>(source, onNextLockName, onErrorLockName, onCompletedLockName);
        }

        private sealed class AwaitDoSubscribable<T> : SubscribableBase<T>
        {
            private readonly ISubscribable<T> _source;
            private readonly string _onNextLockName;
            private readonly string _onErrorLockName;
            private readonly string _onCompletedLockName;

            public AwaitDoSubscribable(ISubscribable<T> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
            {
                _source = source;
                _onNextLockName = onNextLockName;
                _onErrorLockName = onErrorLockName;
                _onCompletedLockName = onCompletedLockName;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<AwaitDoSubscribable<T>, T>, IObserver<T>
            {
                public _(AwaitDoSubscribable<T> parent, IObserver<T> observer)
                    : base(parent, observer)
                {
                }

                protected override IEnumerable<ISubscription> OnSubscribe()
                {
                    return new ISubscription[] { Params._source.Subscribe(this) };
                }

                public void OnCompleted()
                {
                    if (Params._onCompletedLockName != null)
                    {
                        LockManager.Signal(Params._onCompletedLockName);
                    }

                    Output.OnCompleted();
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    if (Params._onErrorLockName != null)
                    {
                        LockManager.Signal(Params._onErrorLockName);
                    }

                    Output.OnError(error);
                    Dispose();
                }

                public void OnNext(T value)
                {
                    if (Params._onNextLockName != null)
                    {
                        LockManager.Signal(Params._onNextLockName);
                    }

                    Output.OnNext(value);
                }
            }
        }

        public static ISubscribable<T> BlockDo<T>(this ISubscribable<T> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
        {
            return new BlockDoSubscribable<T>(source, onNextLockName, onErrorLockName, onCompletedLockName);
        }

        private sealed class BlockDoSubscribable<T> : SubscribableBase<T>
        {
            private readonly ISubscribable<T> _source;
            private readonly string _onNextLockName;
            private readonly string _onErrorLockName;
            private readonly string _onCompletedLockName;

            public BlockDoSubscribable(ISubscribable<T> source, string onNextLockName, string onErrorLockName, string onCompletedLockName)
            {
                _source = source;
                _onNextLockName = onNextLockName;
                _onErrorLockName = onErrorLockName;
                _onCompletedLockName = onCompletedLockName;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<BlockDoSubscribable<T>, T>, IObserver<T>
            {
                public _(BlockDoSubscribable<T> parent, IObserver<T> observer)
                    : base(parent, observer)
                {
                }

                protected override IEnumerable<ISubscription> OnSubscribe()
                {
                    return new ISubscription[] { Params._source.Subscribe(this) };
                }

                public void OnCompleted()
                {
                    if (Params._onCompletedLockName != null)
                    {
                        LockManager.Wait(Params._onCompletedLockName);
                    }

                    Output.OnCompleted();
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    if (Params._onErrorLockName != null)
                    {
                        LockManager.Wait(Params._onErrorLockName);
                    }

                    Output.OnError(error);
                    Dispose();
                }

                public void OnNext(T value)
                {
                    if (Params._onNextLockName != null)
                    {
                        LockManager.Wait(Params._onNextLockName);
                    }

                    Output.OnNext(value);
                }
            }
        }

        public static ISubscribable<T> AwaitDispose<T>(this ISubscribable<T> source, string lockName)
        {
            return new AwaitDisposeSubscribable<T>(source, lockName);
        }

        private sealed class AwaitDisposeSubscribable<T> : SubscribableBase<T>
        {
            private readonly ISubscribable<T> _source;
            private readonly string _lockName;

            public AwaitDisposeSubscribable(ISubscribable<T> source, string lockName)
            {
                _source = source;
                _lockName = lockName;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<AwaitDisposeSubscribable<T>, T>
            {
                public _(AwaitDisposeSubscribable<T> o, IObserver<T> v)
                    : base(o, v)
                {
                }

                protected override IEnumerable<ISubscription> OnSubscribe()
                {
                    return new[] { Params._source.Subscribe(Output) };
                }

                protected override void OnDispose()
                {
                    LockManager.Signal(Params._lockName);
                }
            }
        }
    }
}
