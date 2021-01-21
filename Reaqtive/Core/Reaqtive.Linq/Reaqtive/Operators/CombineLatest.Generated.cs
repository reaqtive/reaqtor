// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    #region CombineLatest2

    internal sealed class CombineLatest<T1, T2, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly Func<T1, T2, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, Func<T1, T2, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;

            public _(CombineLatest<T1, T2, TResult> parent, IObserver<TResult> observer)
                : base(2, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 2;

            public override string Name => "rc:CombineLatest+2";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 1)
                        {
                            _index++;
                            return true;
                        }

                        _index = 2;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest3

    internal sealed class CombineLatest<T1, T2, T3, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly Func<T1, T2, T3, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, Func<T1, T2, T3, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;

            public _(CombineLatest<T1, T2, T3, TResult> parent, IObserver<TResult> observer)
                : base(3, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 3;

            public override string Name => "rc:CombineLatest+3";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 2)
                        {
                            _index++;
                            return true;
                        }

                        _index = 3;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest4

    internal sealed class CombineLatest<T1, T2, T3, T4, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly Func<T1, T2, T3, T4, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, Func<T1, T2, T3, T4, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;

            public _(CombineLatest<T1, T2, T3, T4, TResult> parent, IObserver<TResult> observer)
                : base(4, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 4;

            public override string Name => "rc:CombineLatest+4";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 3)
                        {
                            _index++;
                            return true;
                        }

                        _index = 4;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest5

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly Func<T1, T2, T3, T4, T5, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;

            public _(CombineLatest<T1, T2, T3, T4, T5, TResult> parent, IObserver<TResult> observer)
                : base(5, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 5;

            public override string Name => "rc:CombineLatest+5";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 4)
                        {
                            _index++;
                            return true;
                        }

                        _index = 5;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest6

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly Func<T1, T2, T3, T4, T5, T6, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, TResult> parent, IObserver<TResult> observer)
                : base(6, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 6;

            public override string Name => "rc:CombineLatest+6";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 5)
                        {
                            _index++;
                            return true;
                        }

                        _index = 6;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest7

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> parent, IObserver<TResult> observer)
                : base(7, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 7;

            public override string Name => "rc:CombineLatest+7";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 6)
                        {
                            _index++;
                            return true;
                        }

                        _index = 7;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest8

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> parent, IObserver<TResult> observer)
                : base(8, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 8;

            public override string Name => "rc:CombineLatest+8";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 7)
                        {
                            _index++;
                            return true;
                        }

                        _index = 8;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest9

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> parent, IObserver<TResult> observer)
                : base(9, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 9;

            public override string Name => "rc:CombineLatest+9";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 8)
                        {
                            _index++;
                            return true;
                        }

                        _index = 9;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest10

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> parent, IObserver<TResult> observer)
                : base(10, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 10;

            public override string Name => "rc:CombineLatest+10";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 9)
                        {
                            _index++;
                            return true;
                        }

                        _index = 10;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest11

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly ISubscribable<T11> _source11;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, ISubscribable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(source11 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> parent, IObserver<TResult> observer)
                : base(11, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 11;

            public override string Name => "rc:CombineLatest+11";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                _observer11 = new CombineLatestObserver<T11>(this, 10);
                _observer11.Subscription = Params._source11.Subscribe(_observer11);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue, _observer11.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
                _observer11.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
                _observer11.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 10)
                        {
                            _index++;
                            return true;
                        }

                        _index = 11;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        10 => _parent._observer11.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest12

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly ISubscribable<T11> _source11;
        private readonly ISubscribable<T12> _source12;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, ISubscribable<T11> source11, ISubscribable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(source11 != null);
            Debug.Assert(source12 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> parent, IObserver<TResult> observer)
                : base(12, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 12;

            public override string Name => "rc:CombineLatest+12";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                _observer11 = new CombineLatestObserver<T11>(this, 10);
                _observer11.Subscription = Params._source11.Subscribe(_observer11);

                _observer12 = new CombineLatestObserver<T12>(this, 11);
                _observer12.Subscription = Params._source12.Subscribe(_observer12);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue, _observer11.LastValue, _observer12.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
                _observer11.SaveState(writer);
                _observer12.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
                _observer11.LoadState(reader);
                _observer12.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 11)
                        {
                            _index++;
                            return true;
                        }

                        _index = 12;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        10 => _parent._observer11.Subscription,
                        11 => _parent._observer12.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest13

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly ISubscribable<T11> _source11;
        private readonly ISubscribable<T12> _source12;
        private readonly ISubscribable<T13> _source13;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, ISubscribable<T11> source11, ISubscribable<T12> source12, ISubscribable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(source11 != null);
            Debug.Assert(source12 != null);
            Debug.Assert(source13 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> parent, IObserver<TResult> observer)
                : base(13, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 13;

            public override string Name => "rc:CombineLatest+13";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                _observer11 = new CombineLatestObserver<T11>(this, 10);
                _observer11.Subscription = Params._source11.Subscribe(_observer11);

                _observer12 = new CombineLatestObserver<T12>(this, 11);
                _observer12.Subscription = Params._source12.Subscribe(_observer12);

                _observer13 = new CombineLatestObserver<T13>(this, 12);
                _observer13.Subscription = Params._source13.Subscribe(_observer13);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue, _observer11.LastValue, _observer12.LastValue, _observer13.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
                _observer11.SaveState(writer);
                _observer12.SaveState(writer);
                _observer13.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
                _observer11.LoadState(reader);
                _observer12.LoadState(reader);
                _observer13.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 12)
                        {
                            _index++;
                            return true;
                        }

                        _index = 13;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        10 => _parent._observer11.Subscription,
                        11 => _parent._observer12.Subscription,
                        12 => _parent._observer13.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest14

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly ISubscribable<T11> _source11;
        private readonly ISubscribable<T12> _source12;
        private readonly ISubscribable<T13> _source13;
        private readonly ISubscribable<T14> _source14;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, ISubscribable<T11> source11, ISubscribable<T12> source12, ISubscribable<T13> source13, ISubscribable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(source11 != null);
            Debug.Assert(source12 != null);
            Debug.Assert(source13 != null);
            Debug.Assert(source14 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _source14 = source14;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;
            private CombineLatestObserver<T14> _observer14;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> parent, IObserver<TResult> observer)
                : base(14, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 14;

            public override string Name => "rc:CombineLatest+14";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                _observer11 = new CombineLatestObserver<T11>(this, 10);
                _observer11.Subscription = Params._source11.Subscribe(_observer11);

                _observer12 = new CombineLatestObserver<T12>(this, 11);
                _observer12.Subscription = Params._source12.Subscribe(_observer12);

                _observer13 = new CombineLatestObserver<T13>(this, 12);
                _observer13.Subscription = Params._source13.Subscribe(_observer13);

                _observer14 = new CombineLatestObserver<T14>(this, 13);
                _observer14.Subscription = Params._source14.Subscribe(_observer14);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue, _observer11.LastValue, _observer12.LastValue, _observer13.LastValue, _observer14.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
                _observer11.SaveState(writer);
                _observer12.SaveState(writer);
                _observer13.SaveState(writer);
                _observer14.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
                _observer11.LoadState(reader);
                _observer12.LoadState(reader);
                _observer13.LoadState(reader);
                _observer14.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 13)
                        {
                            _index++;
                            return true;
                        }

                        _index = 14;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        10 => _parent._observer11.Subscription,
                        11 => _parent._observer12.Subscription,
                        12 => _parent._observer13.Subscription,
                        13 => _parent._observer14.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest15

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly ISubscribable<T11> _source11;
        private readonly ISubscribable<T12> _source12;
        private readonly ISubscribable<T13> _source13;
        private readonly ISubscribable<T14> _source14;
        private readonly ISubscribable<T15> _source15;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, ISubscribable<T11> source11, ISubscribable<T12> source12, ISubscribable<T13> source13, ISubscribable<T14> source14, ISubscribable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(source11 != null);
            Debug.Assert(source12 != null);
            Debug.Assert(source13 != null);
            Debug.Assert(source14 != null);
            Debug.Assert(source15 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _source14 = source14;
            _source15 = source15;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;
            private CombineLatestObserver<T14> _observer14;
            private CombineLatestObserver<T15> _observer15;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> parent, IObserver<TResult> observer)
                : base(15, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 15;

            public override string Name => "rc:CombineLatest+15";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                _observer11 = new CombineLatestObserver<T11>(this, 10);
                _observer11.Subscription = Params._source11.Subscribe(_observer11);

                _observer12 = new CombineLatestObserver<T12>(this, 11);
                _observer12.Subscription = Params._source12.Subscribe(_observer12);

                _observer13 = new CombineLatestObserver<T13>(this, 12);
                _observer13.Subscription = Params._source13.Subscribe(_observer13);

                _observer14 = new CombineLatestObserver<T14>(this, 13);
                _observer14.Subscription = Params._source14.Subscribe(_observer14);

                _observer15 = new CombineLatestObserver<T15>(this, 14);
                _observer15.Subscription = Params._source15.Subscribe(_observer15);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue, _observer11.LastValue, _observer12.LastValue, _observer13.LastValue, _observer14.LastValue, _observer15.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
                _observer11.SaveState(writer);
                _observer12.SaveState(writer);
                _observer13.SaveState(writer);
                _observer14.SaveState(writer);
                _observer15.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
                _observer11.LoadState(reader);
                _observer12.LoadState(reader);
                _observer13.LoadState(reader);
                _observer14.LoadState(reader);
                _observer15.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 14)
                        {
                            _index++;
                            return true;
                        }

                        _index = 15;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        10 => _parent._observer11.Subscription,
                        11 => _parent._observer12.Subscription,
                        12 => _parent._observer13.Subscription,
                        13 => _parent._observer14.Subscription,
                        14 => _parent._observer15.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

    #region CombineLatest16

    internal sealed class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<T1> _source1;
        private readonly ISubscribable<T2> _source2;
        private readonly ISubscribable<T3> _source3;
        private readonly ISubscribable<T4> _source4;
        private readonly ISubscribable<T5> _source5;
        private readonly ISubscribable<T6> _source6;
        private readonly ISubscribable<T7> _source7;
        private readonly ISubscribable<T8> _source8;
        private readonly ISubscribable<T9> _source9;
        private readonly ISubscribable<T10> _source10;
        private readonly ISubscribable<T11> _source11;
        private readonly ISubscribable<T12> _source12;
        private readonly ISubscribable<T13> _source13;
        private readonly ISubscribable<T14> _source14;
        private readonly ISubscribable<T15> _source15;
        private readonly ISubscribable<T16> _source16;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _selector;

        public CombineLatest(ISubscribable<T1> source1, ISubscribable<T2> source2, ISubscribable<T3> source3, ISubscribable<T4> source4, ISubscribable<T5> source5, ISubscribable<T6> source6, ISubscribable<T7> source7, ISubscribable<T8> source8, ISubscribable<T9> source9, ISubscribable<T10> source10, ISubscribable<T11> source11, ISubscribable<T12> source12, ISubscribable<T13> source13, ISubscribable<T14> source14, ISubscribable<T15> source15, ISubscribable<T16> source16, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> selector)
        {
            Debug.Assert(source1 != null);
            Debug.Assert(source2 != null);
            Debug.Assert(source3 != null);
            Debug.Assert(source4 != null);
            Debug.Assert(source5 != null);
            Debug.Assert(source6 != null);
            Debug.Assert(source7 != null);
            Debug.Assert(source8 != null);
            Debug.Assert(source9 != null);
            Debug.Assert(source10 != null);
            Debug.Assert(source11 != null);
            Debug.Assert(source12 != null);
            Debug.Assert(source13 != null);
            Debug.Assert(source14 != null);
            Debug.Assert(source15 != null);
            Debug.Assert(source16 != null);
            Debug.Assert(selector != null);

            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _source14 = source14;
            _source15 = source15;
            _source16 = source16;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : CombineLatestCommon<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>, TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _parent;
            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;
            private CombineLatestObserver<T14> _observer14;
            private CombineLatestObserver<T15> _observer15;
            private CombineLatestObserver<T16> _observer16;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> parent, IObserver<TResult> observer)
                : base(16, parent, observer)
            {
                _parent = parent;
            }

            protected override int Arity => 16;

            public override string Name => "rc:CombineLatest+16";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _observer1 = new CombineLatestObserver<T1>(this, 0);
                _observer1.Subscription = Params._source1.Subscribe(_observer1);

                _observer2 = new CombineLatestObserver<T2>(this, 1);
                _observer2.Subscription = Params._source2.Subscribe(_observer2);

                _observer3 = new CombineLatestObserver<T3>(this, 2);
                _observer3.Subscription = Params._source3.Subscribe(_observer3);

                _observer4 = new CombineLatestObserver<T4>(this, 3);
                _observer4.Subscription = Params._source4.Subscribe(_observer4);

                _observer5 = new CombineLatestObserver<T5>(this, 4);
                _observer5.Subscription = Params._source5.Subscribe(_observer5);

                _observer6 = new CombineLatestObserver<T6>(this, 5);
                _observer6.Subscription = Params._source6.Subscribe(_observer6);

                _observer7 = new CombineLatestObserver<T7>(this, 6);
                _observer7.Subscription = Params._source7.Subscribe(_observer7);

                _observer8 = new CombineLatestObserver<T8>(this, 7);
                _observer8.Subscription = Params._source8.Subscribe(_observer8);

                _observer9 = new CombineLatestObserver<T9>(this, 8);
                _observer9.Subscription = Params._source9.Subscribe(_observer9);

                _observer10 = new CombineLatestObserver<T10>(this, 9);
                _observer10.Subscription = Params._source10.Subscribe(_observer10);

                _observer11 = new CombineLatestObserver<T11>(this, 10);
                _observer11.Subscription = Params._source11.Subscribe(_observer11);

                _observer12 = new CombineLatestObserver<T12>(this, 11);
                _observer12.Subscription = Params._source12.Subscribe(_observer12);

                _observer13 = new CombineLatestObserver<T13>(this, 12);
                _observer13.Subscription = Params._source13.Subscribe(_observer13);

                _observer14 = new CombineLatestObserver<T14>(this, 13);
                _observer14.Subscription = Params._source14.Subscribe(_observer14);

                _observer15 = new CombineLatestObserver<T15>(this, 14);
                _observer15.Subscription = Params._source15.Subscribe(_observer15);

                _observer16 = new CombineLatestObserver<T16>(this, 15);
                _observer16.Subscription = Params._source16.Subscribe(_observer16);

                return new InputSubscriptions(this);
            }

            protected override TResult GetResult() => _parent._selector(_observer1.LastValue, _observer2.LastValue, _observer3.LastValue, _observer4.LastValue, _observer5.LastValue, _observer6.LastValue, _observer7.LastValue, _observer8.LastValue, _observer9.LastValue, _observer10.LastValue, _observer11.LastValue, _observer12.LastValue, _observer13.LastValue, _observer14.LastValue, _observer15.LastValue, _observer16.LastValue);

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                _observer1.SaveState(writer);
                _observer2.SaveState(writer);
                _observer3.SaveState(writer);
                _observer4.SaveState(writer);
                _observer5.SaveState(writer);
                _observer6.SaveState(writer);
                _observer7.SaveState(writer);
                _observer8.SaveState(writer);
                _observer9.SaveState(writer);
                _observer10.SaveState(writer);
                _observer11.SaveState(writer);
                _observer12.SaveState(writer);
                _observer13.SaveState(writer);
                _observer14.SaveState(writer);
                _observer15.SaveState(writer);
                _observer16.SaveState(writer);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _observer1.LoadState(reader);
                _observer2.LoadState(reader);
                _observer3.LoadState(reader);
                _observer4.LoadState(reader);
                _observer5.LoadState(reader);
                _observer6.LoadState(reader);
                _observer7.LoadState(reader);
                _observer8.LoadState(reader);
                _observer9.LoadState(reader);
                _observer10.LoadState(reader);
                _observer11.LoadState(reader);
                _observer12.LoadState(reader);
                _observer13.LoadState(reader);
                _observer14.LoadState(reader);
                _observer15.LoadState(reader);
                _observer16.LoadState(reader);
            }

            private sealed class InputSubscriptions : IEnumerable<ISubscription>
            {
                private readonly _ _parent;

                public InputSubscriptions(_ parent) => _parent = parent;

                public IEnumerator<ISubscription> GetEnumerator() => new Enumerator(_parent);

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                private sealed class Enumerator : IEnumerator<ISubscription>
                {
                    private readonly _ _parent;
                    private int _index = -1;

                    public Enumerator(_ parent) => _parent = parent;

                    public bool MoveNext()
                    {
                        if (_index < 15)
                        {
                            _index++;
                            return true;
                        }

                        _index = 16;
                        return false;
                    }

                    public ISubscription Current => _index switch
                    {
                        0 => _parent._observer1.Subscription,
                        1 => _parent._observer2.Subscription,
                        2 => _parent._observer3.Subscription,
                        3 => _parent._observer4.Subscription,
                        4 => _parent._observer5.Subscription,
                        5 => _parent._observer6.Subscription,
                        6 => _parent._observer7.Subscription,
                        7 => _parent._observer8.Subscription,
                        8 => _parent._observer9.Subscription,
                        9 => _parent._observer10.Subscription,
                        10 => _parent._observer11.Subscription,
                        11 => _parent._observer12.Subscription,
                        12 => _parent._observer13.Subscription,
                        13 => _parent._observer14.Subscription,
                        14 => _parent._observer15.Subscription,
                        15 => _parent._observer16.Subscription,
                        _ => null
                    };

                    object IEnumerator.Current => Current;

                    public void Reset() => _index = -1;

                    public void Dispose() { }
                }
            }
        }
    }

    #endregion

}
