// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Scheduler;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Reaqtive.Operators
{
    internal abstract class Buffer<TSource> : SubscribableBase<IList<TSource>>
    {
        protected readonly ISubscribable<TSource> _source;

        public Buffer(ISubscribable<TSource> source)
        {
            _source = source;
        }

        protected abstract class Sink<TParams> : StatefulUnaryOperator<TParams, IList<TSource>>, IObserver<TSource>
            where TParams : Buffer<TSource>
        {
            private const string MAXBUFFERSIZESETTING = "rx://operators/buffer/settings/maxBufferSize";
            private int _maxBufferSize;

            protected Sink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXBUFFERSIZESETTING, out _maxBufferSize);
            }

            protected bool CheckBufferSize(List<TSource> buffer)
            {
                if (buffer.Count >= _maxBufferSize)
                {
                    Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of elements in a buffer produced by the Buffer operator exceeded {0} items. Please adjust the Buffer operator parameters to avoid exceeding this limit.", _maxBufferSize)));
                    Dispose();

                    return false;
                }

                return true;
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            public abstract void OnNext(TSource value);
            public abstract void OnError(Exception error);
            public abstract void OnCompleted();
        }

        protected abstract class OneSink<TParams> : Sink<TParams>
            where TParams : Buffer<TSource>
        {
            private List<TSource> _buffer;

            protected OneSink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected bool HasRecovered { get; private set; }

            protected override void OnStart()
            {
                if (!HasRecovered)
                {
                    _buffer = CreateBuffer();
                    StateChanged = true;
                }
            }

            protected virtual List<TSource> NextBuffer()
            {
                var res = _buffer;

                _buffer = CreateBuffer();
                StateChanged = true;

                return res;
            }

            protected virtual List<TSource> CreateBuffer()
            {
                return new List<TSource>();
            }

            public override void OnCompleted()
            {
                if (_buffer.Count > 0)
                {
                    Output.OnNext(_buffer);
                }

                Output.OnCompleted();
                Dispose();
            }

            public override void OnError(Exception error)
            {
                _buffer.Clear();

                Output.OnError(error);
                Dispose();
            }

            public override void OnNext(TSource value)
            {
                if (!CheckBufferSize(_buffer))
                {
                    return;
                }

                _buffer.Add(value);
                StateChanged = true;

                OnNextCore();
            }

            protected virtual void OnNextCore()
            {
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _buffer = reader.Read<List<TSource>>();

                HasRecovered = true;
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<List<TSource>>(_buffer);
            }
        }

        protected abstract class SyncOneSink<TParams> : OneSink<TParams>
            where TParams : Buffer<TSource>
        {
            protected readonly object _gate = new();

            protected SyncOneSink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected override List<TSource> NextBuffer()
            {
                lock (_gate)
                {
                    return base.NextBuffer();
                }
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    base.OnNext(value);
                }
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    base.OnError(error);
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    base.OnCompleted();
                }
            }
        }

        protected abstract class ManySink<TParams> : Sink<TParams>
            where TParams : Buffer<TSource>
        {
            private const string MAXBUFFERCOUNTSETTING = "rx://operators/buffer/settings/maxBufferCount";
            private int _maxBufferCount;

            private Queue<List<TSource>> _buffers;

            protected ManySink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXBUFFERCOUNTSETTING, out _maxBufferCount);
            }

            private bool CheckBufferCount()
            {
                if (_buffers.Count >= _maxBufferCount)
                {
                    Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of buffers produced by the Buffer operator exceeded {0} items. Please adjust the Buffer operator parameters to avoid exceeding this limit.", _maxBufferCount)));
                    Dispose();

                    return false;
                }

                return true;
            }

            protected override void OnStart()
            {
                if (_buffers == null)
                {
                    _buffers = CreateQueue();
                    OpenBuffer(); // should never fail with OnError because _maxBufferCount > 0
                }
            }

            protected abstract Queue<List<TSource>> CreateQueue();

            protected virtual bool OpenBuffer()
            {
                var buffer = CreateBuffer();

                if (!CheckBufferCount())
                {
                    return false;
                }

                _buffers.Enqueue(buffer);

                StateChanged = true;

                return true;
            }

            protected virtual List<TSource> CreateBuffer()
            {
                return new List<TSource>();
            }

            protected virtual List<TSource> CloseBuffer()
            {
                var buffer = _buffers.Dequeue();

                StateChanged = true;

                return buffer;
            }

            public override void OnCompleted()
            {
                while (_buffers.Count > 0)
                {
                    var buffer = _buffers.Dequeue();
                    if (buffer.Count > 0)
                    {
                        Output.OnNext(buffer);
                    }
                }

                Output.OnCompleted();
                Dispose();
            }

            public override void OnError(Exception error)
            {
                while (_buffers.Count > 0)
                {
                    _buffers.Dequeue().Clear();
                }

                Output.OnError(error);
                Dispose();
            }

            public override void OnNext(TSource value)
            {
                foreach (var buffer in _buffers)
                {
                    if (!CheckBufferSize(buffer))
                    {
                        return;
                    }

                    buffer.Add(value);
                }

                if (_buffers.Count > 0)
                {
                    StateChanged = true;
                }

                OnNextCore();
            }

            protected virtual void OnNextCore()
            {
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _buffers = CreateQueue();

                var count = reader.Read<int>();

                for (var i = 0; i < count; i++)
                {
                    var buffer = reader.Read<List<TSource>>();
                    _buffers.Enqueue(buffer);
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<int>(_buffers.Count);

                foreach (var buffer in _buffers)
                {
                    writer.Write<List<TSource>>(buffer);
                }
            }
        }

        protected abstract class SyncManySink<TParams> : ManySink<TParams>
            where TParams : Buffer<TSource>
        {
            protected readonly object _gate = new();

            protected SyncManySink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected override bool OpenBuffer()
            {
                lock (_gate)
                {
                    return base.OpenBuffer();
                }
            }

            protected override List<TSource> CloseBuffer()
            {
                lock (_gate)
                {
                    return base.CloseBuffer();
                }
            }

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    base.OnNext(value);
                }
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    base.OnError(error);
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    base.OnCompleted();
                }
            }
        }
    }

    internal sealed class BufferDuration<TSource> : Buffer<TSource>
    {
        private readonly TimeSpan _duration;

        public BufferDuration(ISubscribable<TSource> source, TimeSpan duration)
            : base(source)
        {
            _duration = duration;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : SyncOneSink<BufferDuration<TSource>>, ISchedulerTask
        {
            private DateTimeOffset? _nextTick;
            private IOperatorContext _context;

            public _(BufferDuration<TSource> parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Buffer/Time/D";

            public override Version Version => Versioning.v1;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void OnStart()
            {
                base.OnStart();

                if (_nextTick == null)
                {
                    _nextTick = _context.Scheduler.Now + Params._duration;
                    StateChanged = true;
                }

                _context.Scheduler.Schedule(_nextTick.Value, this);
            }

            private void Tick()
            {
                if (IsDisposed)
                {
                    return;
                }

                lock (_gate)
                {
                    var buffer = NextBuffer();
                    Output.OnNext(buffer);

                    _nextTick += Params._duration;
                    StateChanged = true;

                    _context.Scheduler.Schedule(_nextTick.Value, this);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _nextTick = reader.Read<DateTimeOffset>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<DateTimeOffset>(_nextTick.Value);
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

    internal sealed class BufferDurationShift<TSource> : Buffer<TSource>
    {
        private readonly TimeSpan _duration;
        private readonly TimeSpan _shift;

        public BufferDurationShift(ISubscribable<TSource> source, TimeSpan duration, TimeSpan shift)
            : base(source)
        {
            _duration = duration;
            _shift = shift;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : SyncManySink<BufferDurationShift<TSource>>, ISchedulerTask
        {
            private DateTimeOffset? _nextTick;
            private DateTimeOffset _nextOpen;
            private DateTimeOffset _nextClose;
            private IOperatorContext _context;

            public _(BufferDurationShift<TSource> parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Buffer/Time/D+S";

            public override Version Version => Versioning.v1;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override Queue<List<TSource>> CreateQueue()
            {
                //
                // Similar calculation as for the count-based operator. Notice that we
                // don't need any adjustment for timer oddities given we use a single
                // scheduled task to run to the next tick kind, which is based on an
                // initial time value obtained from the scheduler and just increments
                // from there. Also notice that concurrent open and close events will
                // first close the current buffer and then open a new buffer, so the
                // queue size will remain constant.
                //

                var duration = Params._duration.Ticks;
                var shift = Params._shift.Ticks;

                var max = duration / shift;

                var rem = duration % shift;
                if (rem != 0)
                {
                    max++;
                }

                return new Queue<List<TSource>>(checked((int)max));
            }

            protected override void OnStart()
            {
                base.OnStart();

                if (_nextTick == null)
                {
                    var now = _context.Scheduler.Now;
                    _nextOpen = now + Params._shift;
                    _nextClose = now + Params._duration;
                }

                var nextDue = _nextOpen <= _nextClose ? _nextOpen : _nextClose;

                if (_nextTick == null)
                {
                    _nextTick = nextDue;

                    StateChanged = true;
                }

                _context.Scheduler.Schedule(nextDue, this);
            }

            private void Tick()
            {
                if (IsDisposed)
                {
                    return;
                }

                bool shouldClose, shouldOpen;

                //
                // If open and close coincide, we'll atomically close the current buffer
                // and open the next buffer. This avoids dropping events or getting them
                // to occur in multiple buffers.
                //
                if (_nextOpen <= _nextClose)
                {
                    shouldClose = (_nextOpen == _nextClose);
                    shouldOpen = true;
                }
                else
                {
                    shouldClose = true;
                    shouldOpen = false;
                }

                lock (_gate)
                {
                    if (shouldClose)
                    {
                        _nextClose += Params._shift;

                        var buffer = CloseBuffer();
                        Output.OnNext(buffer);
                    }

                    if (shouldOpen)
                    {
                        _nextOpen += Params._shift;

                        if (!OpenBuffer())
                        {
                            return;
                        }
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
                }

                _context.Scheduler.Schedule(_nextTick.Value, this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _nextTick = reader.Read<DateTimeOffset>();
                _nextOpen = reader.Read<DateTimeOffset>();
                _nextClose = reader.Read<DateTimeOffset>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<DateTimeOffset>(_nextTick.Value);
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

    internal sealed class BufferCount<TSource> : Buffer<TSource>
    {
        private readonly int _count;

        public BufferCount(ISubscribable<TSource> source, int count)
            : base(source)
        {
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : OneSink<BufferCount<TSource>>
        {
            private int _remaining;

            public _(BufferCount<TSource> parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Buffer/Count/N";

            public override Version Version => Versioning.v1;

            protected override void OnStart()
            {
                base.OnStart();

                if (!HasRecovered)
                {
                    _remaining = Params._count;
                    StateChanged = true;
                }
            }

            protected override List<TSource> CreateBuffer()
            {
                return new List<TSource>(Params._count);
            }

            protected override void OnNextCore()
            {
                if (--_remaining == 0)
                {
                    var buffer = NextBuffer();
                    if (buffer.Count > 0)
                    {
                        Output.OnNext(buffer);
                    }

                    _remaining = Params._count;
                }

                StateChanged = true;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _remaining = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<int>(_remaining);
            }
        }
    }

    internal sealed class BufferCountSkip<TSource> : Buffer<TSource>
    {
        private readonly int _count;
        private readonly int _skip;

        public BufferCountSkip(ISubscribable<TSource> source, int count, int skip)
            : base(source)
        {
            _count = count;
            _skip = skip;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : ManySink<BufferCountSkip<TSource>>
        {
            private int _n;

            public _(BufferCountSkip<TSource> parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Buffer/Count/N+S";

            public override Version Version => Versioning.v1;

            protected override Queue<List<TSource>> CreateQueue()
            {
                //
                // Examples of calculation below (count, skip):
                //
                //           x   x   x   x   x   x   x   x   x   x
                //
                // (3,1)    +---------+
                //              +---------+
                //                  +---------+                     3 buffers = Ceiling(3 / 1)
                //                      +---------+
                //                          +---------+
                //                              +---------+
                //                                  +---------+
                //                                      +---------
                // (3,2)    +---------+
                //                  +---------+                     2 buffers = Ceiling(3 / 2)
                //                          +---------+
                //                                  +---------+
                //                                          +-----
                // (3,3)    +---------+                             1 buffer  = Ceiling(3 / 3)
                //                      +---------+
                //                                  +---------+
                //                                              +-
                // (2,3)    +-----+
                //                      +-----+                     1 buffer  = Ceiling(2 / 3)
                //                                  +-----+
                //                                              +-
                //

                var max = Params._count / Params._skip;

                var rem = Params._count % Params._skip;
                if (rem != 0)
                {
                    max++;
                }

                return new Queue<List<TSource>>(max);
            }

            protected override List<TSource> CreateBuffer()
            {
                return new List<TSource>(Params._count);
            }

            protected override void OnNextCore()
            {
                var count = _n - Params._count + 1;

                if (count >= 0 && count % Params._skip == 0)
                {
                    var buffer = CloseBuffer();
                    if (buffer.Count > 0)
                    {
                        Output.OnNext(buffer);
                    }
                }

                _n++;
                StateChanged = true;

                if (_n % Params._skip == 0)
                {
                    OpenBuffer(); // can fail, but will produce OnError
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _n = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<int>(_n);
            }
        }
    }

    internal sealed class BufferDurationCount<TSource> : Buffer<TSource>
    {
        private readonly TimeSpan _duration;
        private readonly int _count;

        public BufferDurationCount(ISubscribable<TSource> source, TimeSpan duration, int count)
            : base(source)
        {
            _duration = duration;
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : SyncOneSink<BufferDurationCount<TSource>>, ISchedulerTask
        {
            private DateTimeOffset _nextTick;
            private int _remaining;
            private volatile int _id;
            private volatile int _nextId;

            private IOperatorContext _context;

            public _(BufferDurationCount<TSource> parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void OnStart()
            {
                base.OnStart();

                if (!HasRecovered)
                {
                    _nextTick = _context.Scheduler.Now + Params._duration;
                    _remaining = Params._count;
                    StateChanged = true;
                }

                ScheduleNextTick(_id);
            }

            protected override void OnNextCore()
            {
                lock (_gate)
                {
                    if (--_remaining == 0)
                    {
                        Advance();
                    }
                }

                StateChanged = true;
            }

            private void Advance()
            {
                var buffer = NextBuffer();
                Output.OnNext(buffer);

                _nextTick = _context.Scheduler.Now + Params._duration;
                _remaining = Params._count;
                _id++;
                StateChanged = true;
            }

            private void ScheduleNextTick(int id)
            {
                _nextId = id;
                _context.Scheduler.Schedule(_nextTick, this);
            }

            private void Tick()
            {
                if (IsDisposed)
                {
                    return;
                }

                lock (_gate)
                {
                    if (_id == _nextId)
                    {
                        Advance();
                    }

                    ScheduleNextTick(_id);
                }
            }

            public override string Name => "rc:Buffer/Ferry";

            public override Version Version => Versioning.v1;

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _nextTick = reader.Read<DateTimeOffset>();
                _remaining = reader.Read<int>();
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
                Tick();
                return true;
            }

            public void RecalculatePriority()
            {
            }

            #endregion
        }
    }
}
