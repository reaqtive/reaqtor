// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using Reaqtive;
using Reaqtive.Subjects;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine.Mocks
{
    public class SubjectManager
    {
        private readonly ConcurrentDictionary<Uri, object> _subjects = new();

        public IMultiSubject<T> Create<T>() => new Typed<T>(Add, Remove);

        public IMultiSubject CreateUntyped() => new Untyped(Add, Remove);

        public IReliableMultiSubject<T> CreateReliable<T>() => new Reliable<T>(Add, Remove);

        public IObserver<T> GetObserver<T>(Uri id)
        {
            var result = Get(id);
            if (result is Untyped untyped)
            {
                return untyped.GetObserver<T>();
            }
            else
            {
                return ((Typed<T>)result).CreateObserver();
            }
        }

        public IObserver<T> GetExternalObserver<T>(Uri id)
        {
            return ((Untyped)Get(id)).GetExternalObserver<T>();
        }

        public IReliableObserver<T> GetReliableObserver<T>(Uri id) => ((Reliable<T>)Get(id)).CreateObserver();

        public void AwaitSubscribe(Uri id)
        {
            var d = (Operator)Get(id);
            d.OnSubscribe.WaitOne();
        }

        public void Clear()
        {
            var values = _subjects.Values;

            foreach (var value in values)
            {
                (value as IDisposable)?.Dispose();
            }
        }

        private void Add(Uri id, object obj)
        {
            if (!_subjects.TryAdd(id, obj))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Subject with URI '{0}' already exists.", id));
            }
        }

        private void Remove(Uri id)
        {
            if (!_subjects.TryRemove(id, out _))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Could not find subject with URI '{0}'.", id));
            }
        }

        private object Get(Uri id)
        {
            if (!_subjects.TryGetValue(id, out var obj))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Could not find subject with URI '{0}'.", id));
            }

            return obj;
        }

        private abstract class Operator : IOperator
        {
            private readonly Action<Uri> _onDispose;
            private readonly Action<Uri, object> _onStart;

            public Operator(Action<Uri, object> onStart, Action<Uri> onDispose)
            {
                _onStart = onStart;
                _onDispose = onDispose;
            }

            public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

            protected IOperatorContext Context { get; private set; }

            public void Subscribe() { }

            public void SetContext(IOperatorContext context) => Context = context;

            public void Start() => _onStart(Context.InstanceId, this);

            public void Dispose()
            {
                if (Context != null)
                {
                    _onDispose(Context.InstanceId);
                }

                DisposeCore();
            }

            protected virtual void DisposeCore() { }

            public abstract EventWaitHandle OnSubscribe
            {
                get;
            }
        }

        private sealed class Reliable<T> : Operator, IReliableMultiSubject<T>
        {
            private readonly Subject<Tuple<T, long>> _subject = new();
            private readonly AutoResetEvent _onSubscribe = new(false);

            public Reliable(Action<Uri, object> onCreate, Action<Uri> onDispose)
                : base(onCreate, onDispose)
            {
            }

            public override EventWaitHandle OnSubscribe => _onSubscribe;

            public IReliableObserver<T> CreateObserver() => new Observer(this);

            public IReliableSubscription Subscribe(IReliableObserver<T> observer) => new Sub(this, observer);

            protected override void DisposeCore()
            {
                _onSubscribe?.Dispose();
                _subject?.Dispose();
            }

            private sealed class Observer : IReliableObserver<T>
            {
                private readonly Reliable<T> _parent;

                public Observer(Reliable<T> parent) => _parent = parent;

                public Uri ResubscribeUri => throw new NotImplementedException();

                public void OnStarted() { }

                public void OnNext(T item, long sequenceId) => _parent._subject.OnNext(Tuple.Create(item, sequenceId));

                public void OnError(Exception error) => _parent._subject.OnError(error);

                public void OnCompleted() => _parent._subject.OnCompleted();
            }

            private sealed class Sub : ReliableSubscriptionBase
            {
                private readonly Reliable<T> _parent;
                private readonly IReliableObserver<T> _observer;

#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood DisposeCore
                private IDisposable _disposable;
#pragma warning restore CA2213

                public Sub(Reliable<T> parent, IReliableObserver<T> observer)
                {
                    _parent = parent;
                    _observer = observer;
                }

                public override Uri ResubscribeUri => throw new NotImplementedException();

                public override void Start(long sequenceId)
                {
                    _disposable = _parent._subject.Subscribe(
                        t => _observer.OnNext(t.Item1, t.Item2),
                        e => _observer.OnError(e),
                        () => _observer.OnCompleted());

                    _parent._onSubscribe.Set();
                }

                public override void AcknowledgeRange(long sequenceId)
                {
                }

                public override void DisposeCore()
                {
                    _disposable?.Dispose();
                }
            }
        }

        private sealed class Typed<T> : Operator, IMultiSubject<T>
        {
            private readonly Subject<T> _subject = new();
            private readonly AutoResetEvent _onSubscribe = new(false);

            public Typed(Action<Uri, object> onCreate, Action<Uri> onDispose)
                : base(onCreate, onDispose)
            {
            }

            public override EventWaitHandle OnSubscribe => _onSubscribe;

            public IObserver<T> CreateObserver() => _subject;

            public ISubscription Subscribe(IObserver<T> observer) => new Sub(this, observer);

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => Subscribe(observer);

            private sealed class Sub : Operator<Typed<T>, T>
            {
#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood OnDispose
                private IDisposable _disposable;
#pragma warning restore CA2213

                public Sub(Typed<T> parent, IObserver<T> observer)
                    : base(parent, observer)
                {
                }

                protected override void OnStart()
                {
                    _disposable = Params._subject.Subscribe(
                        x => Output.OnNext(x),
                        e => Output.OnError(e),
                        () => Output.OnCompleted());

                    Params._onSubscribe.Set();
                }

                protected override void OnDispose()
                {
                    _disposable?.Dispose();
                }
            }
        }

        private sealed class Untyped : Operator, IMultiSubject
        {
            private readonly AutoResetEvent _onSubscribe = new(false);

            private readonly object _subjectGate = new();
            private object _subject;
            private object _contextSwitchedSubject;
            private ISubscription _contextSwitchedSubscription;

            public Untyped(Action<Uri, object> onCreate, Action<Uri> onDispose)
                : base(onCreate, onDispose)
            {
            }

            public override EventWaitHandle OnSubscribe => _onSubscribe;

            public IObserver<T> GetObserver<T>() => Get<T>(false);

            public IObserver<T> GetExternalObserver<T>() => Get<T>(true);

            public ISubscribable<T> GetObservable<T>() => new IO<T>(this);

            protected override void DisposeCore()
            {
                _onSubscribe?.Dispose();
                (_subject as IDisposable)?.Dispose();
            }

            private Subject<T> Get<T>(bool contextSwitched)
            {
                if (_subject == null)
                {
                    lock (_subjectGate)
                    {
                        _subject ??= new Subject<T>();
                    }
                }

                var result = (Subject<T>)_subject;
                if (!contextSwitched)
                {
                    return result;
                }
                else
                {
                    return GetContextSwitched(result);
                }
            }

            private Subject<T> GetContextSwitched<T>(Subject<T> subject)
            {
                if (_contextSwitchedSubject == null)
                {
                    lock (_subjectGate)
                    {
                        if (_contextSwitchedSubject == null)
                        {
                            var inner = new Subject<T>();
                            _contextSwitchedSubject = inner;
                            _contextSwitchedSubscription = inner.ToSubscribable().Subscribe(subject);
                            new SubscriptionInitializeVisitor(_contextSwitchedSubscription).Initialize(Context);
                        }
                    }
                }

                return (Subject<T>)_contextSwitchedSubject;
            }

            private sealed class IO<T> : SubscribableBase<T>
            {
                private readonly Untyped _parent;

                public IO(Untyped parent) => _parent = parent;

                protected override ISubscription SubscribeCore(IObserver<T> observer) => new _(this, observer);

                private sealed class _ : Operator<IO<T>, T>
                {
#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood OnDispose
                    private IDisposable _disposable;
#pragma warning restore CA2213

                    public _(IO<T> parent, IObserver<T> observer)
                        : base(parent, observer)
                    {
                    }

                    protected override void OnStart()
                    {
                        var output = Output;
                        _disposable = Params._parent.Get<T>(false).Subscribe(
                            x => output.OnNext(x),
                            e => output.OnError(e),
                            () => output.OnCompleted());

                        Params._parent._onSubscribe.Set();
                    }

                    protected override void OnDispose()
                    {
                        _disposable?.Dispose();
                    }
                }
            }
        }
    }
}
