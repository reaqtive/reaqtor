// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive.Scheduler;
using Reaqtive.Storage;

#pragma warning disable CA1051 // Do not declare visible instance fields. (We're okay with protected readonly fields here.)

namespace Reaqtive.Operators
{
    public abstract class Buffer<TSource> : SubscribableBase<IList<TSource>>
    {
        private protected readonly ISubscribable<TSource> _source;

        protected Buffer(ISubscribable<TSource> source)
        {
            _source = source;
        }

        protected abstract class Sink<TParams> : StatefulUnaryOperator<TParams, IList<TSource>>, IObserver<TSource>
            where TParams : Buffer<TSource>
        {
            private IPersistedObjectSpace _storage;

            protected Sink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected IPersistedObjectSpace Storage => _storage;

            public override void SetContext(IOperatorContext context)
            {
                Debug.Assert(context != null);

                base.SetContext(context);

                //
                // TODO: Upon integrating this work with the rest of IRP, make this accessible through a property instead.
                //

                if (!context.TryGetElement("PersistedObjectSpace", out _storage))
                {
                    _storage = new VolatilePersistedObjectSpace();
                }
            }

            protected override ISubscription OnSubscribe() => Params._source.Subscribe(this);

            public abstract void OnNext(TSource value);
#pragma warning disable CA1716 // Identifiers should not match keywords. (Inherited from IObserver<T>.OnError(Exception).)
            public abstract void OnError(Exception error);
#pragma warning restore CA1716
            public abstract void OnCompleted();
        }

        protected abstract class OneSink<TParams> : Sink<TParams>
            where TParams : Buffer<TSource>
        {
            private IPersistedList<TSource> _buffer;

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

                    //
                    // NB: We still need to toggle StateChanged here because we have to store the persisted list's identifier. Note we likely
                    //     won't be able to derive the identifier from the parent artifact identifier because we'd also need to a way to reliably
                    //     identify the operator instance, of which there can be many per subscription (and the count is dynamic as well).
                    //

                    StateChanged = true;
                }
            }

            protected virtual IList<TSource> NextBuffer()
            {
                //
                // NB: We prefer to create an in-memory copy so we can call Clear on the persisted list, which can be reused for the next buffer.
                //     This strategy can reduce the I/O incurred by a checkpoint; a Delete for the persisted list would have O(n1) delete calls,
                //     and a subsequent re-create and filling of the new buffer will add O(n2) write calls. By reusing the persisted list, we can
                //     get O(n2) edits and O(n1-n2) deletes or O(n2-n1) adds (depending which "generation" of the list is larger).
                //

                var res = new List<TSource>(_buffer);

                _buffer.Clear();

                //
                // NB: We don't need to call StateChanged here anymore; the edits to the list are tracked by the persisted state manager.
                //

                return res;
            }

            protected virtual IPersistedList<TSource> CreateBuffer() => Storage.CreateList<TSource>(Guid.NewGuid().ToString()); // REVIEW: Derive the identifier from the parent?

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
                //
                // NB: We don't call Clear anymore; OnDispose deletes the buffer and gets rid of the reference to it.
                //

                Output.OnError(error);
                Dispose();
            }

            public override void OnNext(TSource value)
            {
                _buffer.Add(value);

                //
                // NB: We don't need to call StateChanged here anymore; the edits to the list are tracked by the persisted state manager.
                //

                OnNextCore();
            }

            protected virtual void OnNextCore()
            {
            }

            protected override void OnDispose()
            {
                base.OnDispose();

                var buffer = _buffer;

                if (buffer != null)
                {
                    Storage.Delete(buffer.Id);
                    _buffer = null;
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                Debug.Assert(reader != null);

                base.LoadStateCore(reader);

                var bufferId = reader.Read<string>();
                _buffer = Storage.GetList<TSource>(bufferId);

                HasRecovered = true;
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                Debug.Assert(writer != null);

                base.SaveStateCore(writer);

                writer.Write(_buffer.Id);
            }
        }

        protected abstract class SyncOneSink<TParams> : OneSink<TParams>
            where TParams : Buffer<TSource>
        {
            private protected readonly object _gate = new();

            protected SyncOneSink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected override IList<TSource> NextBuffer()
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
            private IPersistedQueue<string> _persistedBuffers;
            private Queue<IPersistedList<TSource>> _buffers;

            protected ManySink(TParams parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            protected override void OnStart()
            {
                if (_buffers == null)
                {
                    (_persistedBuffers, _buffers) = CreateQueue();
                    OpenBuffer();
                }
            }

            protected virtual (IPersistedQueue<string> persistedBuffers, Queue<IPersistedList<TSource>> buffers) CreateQueue()
            {
                return (Storage.CreateQueue<string>(Guid.NewGuid().ToString()), new Queue<IPersistedList<TSource>>()); // REVIEW: Derive the identifier from the parent?
            }

            protected virtual bool OpenBuffer()
            {
                var buffer = CreateBuffer();

                _persistedBuffers.Enqueue(buffer.Id);
                _buffers.Enqueue(buffer);

                //
                // NB: We don't need to call StateChanged here anymore; the edits to the queue are tracked by the persisted state manager.
                //

                return true;
            }

            protected virtual IPersistedList<TSource> CreateBuffer() => Storage.CreateList<TSource>(Guid.NewGuid().ToString()); // REVIEW: Derive the identifier from the parent?

            protected virtual IList<TSource> CloseBuffer()
            {
                var bufferId = _persistedBuffers.Dequeue();
                var buffer = _buffers.Dequeue();

                //
                // NB: We don't need to call StateChanged here anymore; the edits to the queue are tracked by the persisted state manager.
                //

                Storage.Delete(bufferId);

                //
                // REVIEW: Will we ensure it's always safe to return a persisted object after it's been deleted from storage? I.e. do we ensure to keep the in-memory state untouched? Add tests for this.
                //

                return buffer;
            }

            public override void OnCompleted()
            {
                while (_buffers.Count > 0)
                {
                    //
                    // NB: CloseBuffer causes deallocation of the buffer in storage.
                    //

                    var buffer = CloseBuffer();

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
                //
                // NB: Deletion and deallocation of buffers is taken care of unconditionally in OnDispose.
                //

                Output.OnError(error);
                Dispose();
            }

            public override void OnNext(TSource value)
            {
                foreach (var buffer in _buffers)
                {
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

            protected override void OnDispose()
            {
                base.OnDispose();

                var buffers = _buffers;

                if (buffers != null)
                {
                    while (buffers.Count > 0)
                    {
                        _ = CloseBuffer();

                        //
                        // NB: There's no need to call Clear; we're getting rid of the references anyway.
                        //
                    }

                    _buffers = null;
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                Debug.Assert(reader != null);

                base.LoadStateCore(reader);

                var buffersId = reader.Read<string>();
                _persistedBuffers = Storage.GetQueue<string>(buffersId);

                _buffers = new Queue<IPersistedList<TSource>>();

                foreach (var bufferId in _persistedBuffers)
                {
                    _buffers.Enqueue(Storage.GetList<TSource>(bufferId));
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                Debug.Assert(writer != null);

                base.SaveStateCore(writer);

                writer.Write(_persistedBuffers.Id);
            }
        }

        protected abstract class SyncManySink<TParams> : ManySink<TParams>
            where TParams : Buffer<TSource>
        {
            private protected readonly object _gate = new();

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

            protected override IList<TSource> CloseBuffer()
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

    public sealed class BufferDuration<TSource> : Buffer<TSource>
    {
        private readonly TimeSpan _duration;

        public BufferDuration(ISubscribable<TSource> source, TimeSpan duration)
            : base(source)
        {
            _duration = duration;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer) => new _(this, observer);

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
                if (base.IsDisposed)
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

    public sealed class BufferDurationShift<TSource> : Buffer<TSource>
    {
        private readonly TimeSpan _duration;
        private readonly TimeSpan _shift;

        public BufferDurationShift(ISubscribable<TSource> source, TimeSpan duration, TimeSpan shift)
            : base(source)
        {
            _duration = duration;
            _shift = shift;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer) => new _(this, observer);

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

            //
            // NB: We don't override CreateQueue anymore, which was used here to compute the initial queue's capacity.
            //     Dynamic growth of queues will work fine, at the expense of eliminating this minor performance improvement.
            //

            protected override void OnStart()
            {
                base.OnStart();

                DateTimeOffset nextDue;

                if (_nextTick == null)
                {
                    var now = _context.Scheduler.Now;
                    _nextOpen = now + Params._shift;
                    _nextClose = now + Params._duration;
                }

                if (_nextOpen <= _nextClose)
                {
                    nextDue = _nextOpen;
                }
                else
                {
                    nextDue = _nextClose;
                }

                if (_nextTick == null)
                {
                    _nextTick = nextDue;

                    StateChanged = true;
                }

                _context.Scheduler.Schedule(nextDue, this);
            }

            private void Tick()
            {
                if (base.IsDisposed)
                {
                    return;
                }

                bool shouldClose;
                var shouldOpen = false;

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

    public sealed class BufferCount<TSource> : Buffer<TSource>
    {
        private readonly int _count;

        public BufferCount(ISubscribable<TSource> source, int count)
            : base(source)
        {
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer) => new _(this, observer);

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

            //
            // NB: We don't override CreateBuffer anymore, which was used to pass in an initial capacity. Dynamic (persisted) list growth is just fine.
            //

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

    public sealed class BufferCountSkip<TSource> : Buffer<TSource>
    {
        private readonly int _count;
        private readonly int _skip;

        public BufferCountSkip(ISubscribable<TSource> source, int count, int skip)
            : base(source)
        {
            _count = count;
            _skip = skip;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer) => new _(this, observer);

        private sealed class _ : ManySink<BufferCountSkip<TSource>>
        {
            private int _n;

            public _(BufferCountSkip<TSource> parent, IObserver<IList<TSource>> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Buffer/Count/N+S";

            public override Version Version => Versioning.v1;

            //
            // NB: We don't override CreateQueue anymore, which was used here to compute the initial queue's capacity.
            //     Dynamic growth of queues will work fine, at the expense of eliminating this minor performance improvement.
            //

            //
            // NB: We don't override CreateBuffer anymore, which was used to pass in an initial capacity. Dynamic (persisted) list growth is just fine.
            //

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

    public sealed class BufferDurationCount<TSource> : Buffer<TSource>
    {
        private readonly TimeSpan _duration;
        private readonly int _count;

        public BufferDurationCount(ISubscribable<TSource> source, TimeSpan duration, int count)
            : base(source)
        {
            _duration = duration;
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer) => new _(this, observer);

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
                if (base.IsDisposed)
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
