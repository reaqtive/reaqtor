// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Reaqtive.Scheduler;
using Reaqtive.Tasks;

namespace Reaqtive.Operators
{
    internal abstract class Window<TSource> : SubscribableBase<ISubscribable<TSource>>
    {
        private readonly ISubscribable<TSource> _source;

        public Window(ISubscribable<TSource> source)
        {
            _source = source;
        }

        protected abstract class Sink<TParam> : HigherOrderOutputStatefulOperator<TParam, TSource>
            where TParam : Window<TSource>
        {
            private RefCountSubscription _subscription;

            public Sink(TParam parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected override string InnerStreamPrefix => "rx://tunnel/window/";

            protected override ISubscription OnSubscribe()
            {
                _subscription = new RefCountSubscription(this);
                return _subscription;
            }

            protected void DoSubscribeTask(bool recovery = false)
            {
                EnsureResourceManagement(recovery);

                if (!recovery)
                {
                    // Needs to happen before subscribing to the source, so an inner sequence has been emitted
                    // for the observed notifications to flow into.
                    Tick();
                }

                var subscription = Params._source.Subscribe(this);

                if (recovery)
                {
                    SubscriptionInitializeVisitor.Subscribe(subscription);
                    SubscriptionInitializeVisitor.SetContext(subscription, Context);
                }
                else
                {
                    SubscriptionInitializeVisitor.Initialize(subscription, Context);
                }

                _subscription.Subscription = subscription;
            }

            protected abstract void Tick();

            protected bool ShouldStart()
            {
                return !_subscription.IsDisposed;
            }

            protected override void ReleaseCore()
            {
                _subscription.Dispose();
            }
        }

        protected abstract class OneSink<TParam> : Sink<TParam>
            where TParam : Window<TSource>
        {
            private readonly object _gate = new();
            private IObserver<TSource> _currentWindow;
            private Uri _currentWindowUri;

            public OneSink(TParam parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected override IEnumerable<Uri> DependenciesCore
            {
                get
                {
                    yield return _currentWindowUri;
                }
            }

            protected override void Collect(Uri windowUri)
            {
                lock (_gate)
                {
                    if (_currentWindowUri == windowUri)
                    {
                        _currentWindowUri = null;
                        StateChanged = true;
                    }
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    var currentWindow = _currentWindow;
                    currentWindow?.OnCompleted();

                    // Concurrency with OnNextWindow
                    Output.OnCompleted();
                }

                Dispose();
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    var currentWindow = _currentWindow;
                    currentWindow?.OnError(error);

                    // Concurrency with OnNextWindow
                    Output.OnError(error);
                }

                Dispose();
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    var currentWindow = _currentWindow;
                    currentWindow?.OnNext(value);

                    OnNextCore();
                }
            }

            protected virtual void OnNextCore()
            {
            }

            protected override void Tick()
            {
                Advance(true);
            }

            protected void Advance(bool signalTickCompletion)
            {
                if (IsDisposed)
                {
                    EnsureCurrentWindowClosed();
                    return;
                }

                if (OutputSubscriptionDisposed)
                {
                    return;
                }

                var window = CreateInner(out var windowUri);

                lock (_gate)
                {
                    EnsureCurrentWindowClosed();

                    _currentWindow = window.CreateObserver();

                    _currentWindowUri = windowUri;
                    TickCore();
                    StateChanged = true;

                    OnNextInner(window, windowUri);

                    if (signalTickCompletion)
                    {
                        TickCompleted();
                    }
                }
            }

            private void EnsureCurrentWindowClosed()
            {
                lock (_gate)
                {
                    var currentWindow = _currentWindow;
                    currentWindow?.OnCompleted();

                    _currentWindow = null;
                }
            }

            protected virtual void TickCore()
            {
            }

            protected virtual void TickCompleted()
            {
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                LoadWindow(reader);
            }

            private void LoadWindow(IOperatorStateReader reader)
            {
                _currentWindowUri = reader.Read<Uri>();

                if (TryGetInner(_currentWindowUri, out var window))
                {
                    _currentWindow = window.CreateObserver();
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                SaveWindow(writer);
            }

            private void SaveWindow(IOperatorStateWriter writer)
            {
                writer.Write<Uri>(_currentWindowUri);
            }
        }

        protected abstract class ManySink<TParam> : Sink<TParam>
            where TParam : Window<TSource>
        {
            private const string MAXWINDOWCOUNTSETTING = "rx://operators/window/settings/maxWindowCount";
            private int _maxWindowCount;

            private readonly object _gate = new();
            private readonly Queue<Entry> _currentWindows = new();

            public ManySink(TParam parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXWINDOWCOUNTSETTING, out _maxWindowCount);
            }

            protected override IEnumerable<Uri> DependenciesCore
            {
                get
                {
                    foreach (var w in _currentWindows)
                    {
                        yield return w.Uri;
                    }
                }
            }

            protected override void Collect(Uri windowUri)
            {
                lock (_gate)
                {
                    foreach (var window in _currentWindows)
                    {
                        if (window.Uri == windowUri)
                        {
                            window.Uri = null;
                            StateChanged = true;

                            break;
                        }
                    }
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    foreach (var window in _currentWindows)
                    {
                        var observer = window.Observer;
                        observer?.OnCompleted();
                    }

                    // Concurrency with OnNextWindow
                    Output.OnCompleted();
                }

                Dispose();
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    foreach (var window in _currentWindows)
                    {
                        var observer = window.Observer;
                        observer?.OnError(error);
                    }

                    // Concurrency with OnNextWindow
                    Output.OnError(error);
                }

                Dispose();
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    foreach (var window in _currentWindows)
                    {
                        var observer = window.Observer;
                        observer?.OnNext(value);
                    }

                    OnNextCore();
                }
            }

            protected virtual void OnNextCore()
            {
            }

            protected void NextWindow()
            {
                lock (_gate)
                {
                    if (OutputSubscriptionDisposed)
                    {
                        return;
                    }

                    if (_currentWindows.Count >= _maxWindowCount)
                    {
                        Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of windows produced by the Window operator exceeded {0} items. Please adjust the Window operator parameters to avoid exceeding this limit.", _maxWindowCount)));
                        Dispose();

                        return;
                    }
                }

                var window = CreateInner(out var windowUri);

                var entry = new Entry
                {
                    Uri = windowUri,
                    Observer = window.CreateObserver(),
                };

                lock (_gate)
                {
                    _currentWindows.Enqueue(entry);
                    NextWindowCore();
                    StateChanged = true;

                    OnNextInner(window, windowUri);
                }
            }

            protected virtual void NextWindowCore()
            {
            }

            protected void CloseWindow()
            {
                lock (_gate)
                {
                    var window = _currentWindows.Dequeue();
                    CloseWindowCore();
                    StateChanged = true;

                    var observer = window.Observer;
                    observer?.OnCompleted();
                }
            }

            protected virtual void CloseWindowCore()
            {
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                LoadWindows(reader);
            }

            private void LoadWindows(IOperatorStateReader reader)
            {
                var n = reader.Read<int>();

                for (var i = 0; i < n; i++)
                {
                    var windowUri = reader.Read<Uri>();

                    var windowObserver = default(IObserver<TSource>);

                    if (TryGetInner(windowUri, out var window))
                    {
                        windowObserver = window.CreateObserver();
                    }

                    var entry = new Entry
                    {
                        Uri = windowUri,
                        Observer = windowObserver,
                    };

                    _currentWindows.Enqueue(entry);
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                SaveWindows(writer);
            }

            private void SaveWindows(IOperatorStateWriter writer)
            {
                writer.Write<int>(_currentWindows.Count);

                foreach (var window in _currentWindows)
                {
                    writer.Write<Uri>(window.Uri);
                }
            }
        }
    }

    internal sealed class WindowDuration<TSource> : Window<TSource>
    {
        private readonly TimeSpan _duration;

        public WindowDuration(ISubscribable<TSource> source, TimeSpan duration)
            : base(source)
        {
            _duration = duration;
        }

        protected override ISubscription SubscribeCore(IObserver<ISubscribable<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : OneSink<WindowDuration<TSource>>, ISchedulerTask
        {
            private DateTimeOffset _nextTick;
            private bool _recovered;

            public _(WindowDuration<TSource> parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Window/Time/D";

            public override Version Version => Versioning.v1;

            protected override void OnStart()
            {
                base.OnStart();

                if (_recovered)
                {
                    if (ShouldStart())
                    {
                        // TODO: catch-up?
                        Context.Scheduler.Schedule(_nextTick, this);
                    }
                }
                else
                {
                    _nextTick = Context.Scheduler.Now;
                    StateChanged = true;

                    Context.Scheduler.Schedule(new ActionTask(() => DoSubscribeTask()));
                }
            }

            protected override void TickCore()
            {
                _nextTick += Params._duration;
            }

            protected override void TickCompleted()
            {
                Context.Scheduler.Schedule(_nextTick, this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _nextTick = reader.Read<DateTimeOffset>();

                _recovered = true;
                DoSubscribeTask(recovery: true);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<DateTimeOffset>(_nextTick);
            }

            #region Inlining scheduled task support to save ActionTask allocations

            public long Priority => 1L;

            public bool IsRunnable => true;

            public bool Execute(IScheduler scheduler)
            {
                Tick();
                return true;
            }

            public void RecalculatePriority()
            {
            }

            #endregion
        }
    }

    internal sealed class WindowDurationShift<TSource> : Window<TSource>
    {
        private readonly TimeSpan _duration;
        private readonly TimeSpan _shift;

        public WindowDurationShift(ISubscribable<TSource> source, TimeSpan duration, TimeSpan shift)
            : base(source)
        {
            _duration = duration;
            _shift = shift;
        }

        protected override ISubscription SubscribeCore(IObserver<ISubscribable<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : ManySink<WindowDurationShift<TSource>>, ISchedulerTask
        {
            private DateTimeOffset _nextTick;
            private DateTimeOffset _nextOpen;
            private DateTimeOffset _nextClose;
            private bool _recovered;

            public _(WindowDurationShift<TSource> parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Window/Time/D+S";

            public override Version Version => Versioning.v1;

            protected override void OnStart()
            {
                base.OnStart();

                if (_recovered)
                {
                    if (ShouldStart())
                    {
                        var nextDue = default(DateTimeOffset);

                        if (_nextOpen <= _nextClose)
                        {
                            nextDue = _nextOpen;
                        }
                        else
                        {
                            nextDue = _nextClose;
                        }

                        // TODO: catch-up?
                        Context.Scheduler.Schedule(nextDue, this);
                    }
                }
                else
                {
                    _nextOpen = Context.Scheduler.Now;
                    _nextClose = _nextOpen + Params._duration;
                    _nextTick = _nextOpen;
                    StateChanged = true;

                    Context.Scheduler.Schedule(new ActionTask(() => DoSubscribeTask()));
                }
            }

            protected override void Tick()
            {
                if (IsDisposed)
                {
                    return;
                }

                bool shouldOpen, shouldClose;

                //
                // If open and close coincide, we'll atomically close the current window
                // and open the next window. This avoids dropping events or getting them
                // to occur in multiple windows.
                //
                if (_nextOpen <= _nextClose)
                {
                    shouldClose = _nextOpen == _nextClose;
                    shouldOpen = true;
                }
                else
                {
                    shouldClose = true;
                    shouldOpen = false;
                }

                //
                // Only one timer can be active at any time, so we don't need synchronization
                // for the writes to _nextTick, which only happens here and during initialization.
                //

                if (shouldOpen)
                {
                    NextWindow();
                }

                if (shouldClose)
                {
                    CloseWindow();
                }

                if (_nextOpen <= _nextClose)
                {
                    _nextTick = _nextOpen;
                }
                else
                {
                    _nextTick = _nextClose;
                }

                StateChanged = true;

                Context.Scheduler.Schedule(_nextTick, this);
            }

            protected override void NextWindowCore()
            {
                _nextOpen += Params._shift;
            }

            protected override void CloseWindowCore()
            {
                _nextClose += Params._shift;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _nextTick = reader.Read<DateTimeOffset>();
                _nextOpen = reader.Read<DateTimeOffset>();
                _nextClose = reader.Read<DateTimeOffset>();

                _recovered = true;
                DoSubscribeTask(recovery: true);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<DateTimeOffset>(_nextTick);
                writer.Write<DateTimeOffset>(_nextOpen);
                writer.Write<DateTimeOffset>(_nextClose);
            }

            #region Inlining scheduled task support to save ActionTask allocations

            public long Priority => 1L;

            public bool IsRunnable => true;

            public bool Execute(IScheduler scheduler)
            {
                Tick();
                return true;
            }

            public void RecalculatePriority()
            {
            }

            #endregion
        }
    }

    internal sealed class WindowCount<TSource> : Window<TSource>
    {
        private readonly int _count;

        public WindowCount(ISubscribable<TSource> source, int count)
            : base(source)
        {
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<ISubscribable<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : OneSink<WindowCount<TSource>>
        {
            private int _remaining;
            private bool _recovered;

            public _(WindowCount<TSource> parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Window/Count/N";

            public override Version Version => Versioning.v1;

            protected override void OnStart()
            {
                base.OnStart();

                if (!_recovered)
                {
                    _remaining = Params._count;
                    StateChanged = true;

                    Context.Scheduler.Schedule(new ActionTask(() => DoSubscribeTask()));
                }
            }

            protected override void OnNextCore()
            {
                if (--_remaining == 0)
                {
                    Tick();

                    _remaining = Params._count;
                }

                StateChanged = true;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _remaining = reader.Read<int>();

                _recovered = true;
                DoSubscribeTask(recovery: true);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<int>(_remaining);
            }
        }
    }

    internal sealed class WindowCountSkip<TSource> : Window<TSource>
    {
        private readonly int _count;
        private readonly int _skip;

        public WindowCountSkip(ISubscribable<TSource> source, int count, int skip)
            : base(source)
        {
            _count = count;
            _skip = skip;
        }

        protected override ISubscription SubscribeCore(IObserver<ISubscribable<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : ManySink<WindowCountSkip<TSource>>
        {
            private long _count;
            private bool _recovered;

            public _(WindowCountSkip<TSource> parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Window/Count/N+S";

            public override Version Version => Versioning.v1;

            protected override void OnStart()
            {
                base.OnStart();

                if (!_recovered)
                {
                    Context.Scheduler.Schedule(new ActionTask(() => DoSubscribeTask()));
                }
            }

            protected override void Tick()
            {
                NextWindow();
            }

            protected override void OnNextCore()
            {
                // Example:
                //
                //  count = 5
                //  skip = 3
                //
                //  #       0  1  2  3  4  5  6  7  8  9
                //  ------------------------------------
                //  _count  0  1  2  3  4  5  6  7  8  9
                //  n      -4 -3 -2 -1  0  1  2  3  4  5
                //  Close               Y]       Y]
                //  _count  1  2  3  4  5  6  7  8  9 10
                //  Tick          Y [      Y [      Y [
                //  ====================================
                //         [0  1  2  3  4]
                //                  [3  4  5  6  7]
                //                           [6  7  8  9

                var n = _count - Params._count + 1;
                if (n >= 0 && n % Params._skip == 0)
                {
                    CloseWindow();
                }

                _count++;
                StateChanged = true;

                if (_count % Params._skip == 0)
                {
                    Tick();
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _count = reader.Read<long>();

                _recovered = true;
                DoSubscribeTask(recovery: true);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<long>(_count);
            }
        }
    }

    internal sealed class WindowDurationCount<TSource> : Window<TSource>
    {
        private readonly TimeSpan _duration;
        private readonly int _count;

        public WindowDurationCount(ISubscribable<TSource> source, TimeSpan duration, int count)
            : base(source)
        {
            _duration = duration;
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<ISubscribable<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : OneSink<WindowDurationCount<TSource>>, ISchedulerTask
        {
            private DateTimeOffset _nextTick;
            private int _remaining;
            private volatile int _id;
            private volatile int _nextId;
            private bool _recovered;

            public _(WindowDurationCount<TSource> parent, IObserver<ISubscribable<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Window/Ferry";

            public override Version Version => Versioning.v1;

            protected override void OnStart()
            {
                base.OnStart();

                if (_recovered)
                {
                    if (ShouldStart())
                    {
                        // TODO: catch-up?
                        ScheduleNextTick(_id);
                    }
                }
                else
                {
                    _nextTick = Context.Scheduler.Now;
                    _remaining = Params._count;
                    StateChanged = true;

                    Context.Scheduler.Schedule(new ActionTask(() => DoSubscribeTask()));
                }
            }

            protected override void OnNextCore()
            {
                if (--_remaining == 0)
                {
                    Advance(signalTickCompletion: false);
                }

                StateChanged = true;
            }

            protected override void TickCore()
            {
                _nextTick = Context.Scheduler.Now + Params._duration;
                _remaining = Params._count;
                _id++;
            }

            protected override void TickCompleted()
            {
                ScheduleNextTick(_id);
            }

            private void ScheduleNextTick(int id)
            {
                _nextId = id;
                Context.Scheduler.Schedule(_nextTick, this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _nextTick = reader.Read<DateTimeOffset>();
                _remaining = reader.Read<int>();

                _recovered = true;
                DoSubscribeTask(recovery: true);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<DateTimeOffset>(_nextTick);
                writer.Write<int>(_remaining);
            }

            #region Inlining scheduled task support to save ActionTask allocations

            public long Priority => 1L;

            public bool IsRunnable => true;

            public bool Execute(IScheduler scheduler)
            {
                if (_id == _nextId)
                {
                    Advance(signalTickCompletion: true);
                }
                else
                {
                    ScheduleNextTick(_id);
                }

                return true;
            }

            public void RecalculatePriority()
            {
            }

            #endregion
        }
    }
}
