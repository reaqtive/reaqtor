// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtive;

using Reaqtor.QueryEngine;
using Reaqtor.Reliable;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public sealed class PartitionedMultiSubject<T> : IMultiSubject<T>, IDelegationTarget, IPartitionedMultiSubject<T>, IExpressible, IOperator
    {
        private readonly IObserver<T> _observer;
        private readonly PartitionedMulticaster _multicaster;

        private Uri _uri;

        public PartitionedMultiSubject()
        {
            _observer = new ObserverImpl(this);
            _multicaster = new PartitionedMulticaster();
        }

        public IPartitionableSubscribable<T> CreatePartitionableSubscribable()
        {
            return new PartitionableSubscribableImpl(this);
        }

        public ISubscription Subscribe(IObserver<T> observer)
        {
            return _multicaster.Subscribe(observer);
        }

        internal ISubscription Subscribe(IObserver<T> observer, IReadOnlyList<TypeErasedKeyBinding<T>> bindings)
        {
            return _multicaster.Subscribe(observer, bindings);
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return Subscribe(observer);
        }

        public IObserver<T> CreateObserver()
        {
            return _observer;
        }

        public void Dispose()
        {
        }

        private class ObserverImpl : IObserver<T>
        {
            private readonly PartitionedMultiSubject<T> _parent;

            public ObserverImpl(PartitionedMultiSubject<T> parent)
            {
                _parent = parent;
            }

            public void OnCompleted()
            {
                _parent._multicaster.OnCompleted();
            }

            public void OnError(Exception error)
            {
                _parent._multicaster.OnError(error);
            }

            public void OnNext(T value)
            {
                _parent._multicaster.OnNext(value);
            }
        }

        private class PartitionableSubscribableImpl : PartitionableSubscribableBase<T>
        {
            private readonly PartitionedMultiSubject<T> _parent;

            public PartitionableSubscribableImpl(PartitionedMultiSubject<T> parent)
                : this(parent, null)
            {
            }

            private PartitionableSubscribableImpl(PartitionedMultiSubject<T> parent, IList<TypeErasedKeyBinding<T>> bindings)
                : base(bindings)
            {
                _parent = parent;
            }

            protected override IPartitionableSubscribable<T> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<T>> bindings)
            {
                return new PartitionableSubscribableImpl(_parent, bindings);
            }


            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return _parent.Subscribe(observer, _bindings);
            }
        }

        private abstract class Partition : IObserver<T>
        {
            public abstract void OnCompleted();
            public abstract void OnError(Exception error);
            public abstract void OnNext(T value);
        }

        private sealed class PartitionedMulticaster : IObserver<T>
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
            private static readonly MethodInfo s_subscribeTyped = ((MethodInfo)ReflectionHelpers.InfoOf((PartitionedMulticaster p) => p.SubscribeTyped<int>(default(IObserver<T>), default(KeyBinding<T, int>), default(IReadOnlyList<TypeErasedKeyBinding<T>>)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

            private readonly IRefCountingDictionary<object, Partition> _partitions;
            private readonly IMultiSubject<T> _default;

            public PartitionedMulticaster()
            {
                _partitions = new RefCountingDictionary<object, Partition>();
                _default = new SimpleSubject<T>();
            }

            public ISubscription Subscribe(IObserver<T> observer)
            {
                return _default.Subscribe(observer);
            }

            public ISubscription Subscribe(IObserver<T> observer, IReadOnlyList<TypeErasedKeyBinding<T>> bindings)
            {
                if (bindings == null || bindings.Count == 0)
                    return _default.Subscribe(observer);

                var fst = bindings[0];

                var lst = bindings.Sublist(1);

                return (ISubscription)s_subscribeTyped.MakeGenericMethod(fst.KeyType).Invoke(this, new object[] { observer, fst, lst });
            }

            private ISubscription SubscribeTyped<TKey>(IObserver<T> observer, KeyBinding<T, TKey> keyBinding, IReadOnlyList<TypeErasedKeyBinding<T>> rest)
            {
                var partition = _partitions.AddRef(keyBinding.KeySelector, key => new Partition<TKey>((IKeySelector<T, TKey>)key));
                return
                    WrappedSubscription.Create(
                        ((Partition<TKey>)partition).Subscribe(observer, keyBinding.KeyComparer, keyBinding.Key, rest),
                        () => _partitions.Release(keyBinding.KeySelector)
                    );
            }

            public void OnCompleted()
            {
                _default.CreateObserver().OnCompleted();
                foreach (var partition in _partitions)
                {
                    partition.Value.OnCompleted();
                }
            }

            public void OnError(Exception error)
            {
                _default.CreateObserver().OnError(error);
                foreach (var partition in _partitions)
                {
                    partition.Value.OnError(error);
                }
            }

            public void OnNext(T value)
            {
                _default.CreateObserver().OnNext(value);
                foreach (var partition in _partitions)
                {
                    partition.Value.OnNext(value);
                }
            }
        }

        private class Partition<TKey> : Partition, IObserver<T>
        {
            private readonly Func<T, TKey> _partitionKeySelector;
            private readonly IRefCountingDictionary<IEqualityComparer<TKey>, IRefCountingDictionary<TKey, PartitionedMulticaster>> _observers;

            public Partition(IKeySelector<T, TKey> partitionKeySelector)
            {
                _partitionKeySelector = partitionKeySelector.Invoke;
                _observers = new RefCountingDictionary<IEqualityComparer<TKey>, IRefCountingDictionary<TKey, PartitionedMulticaster>>();
            }

            public override void OnCompleted()
            {
                foreach (var observer in _observers)
                {
                    foreach (var o in observer.Value)
                    {
                        o.Value.OnCompleted();
                    }
                }
            }

            public override void OnError(Exception error)
            {
                foreach (var observer in _observers)
                {
                    foreach (var o in observer.Value)
                    {
                        o.Value.OnError(error);
                    }
                }
            }

            public override void OnNext(T value)
            {
                TKey key;
                try
                {
                    key = _partitionKeySelector(value);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                    return;
                }

                foreach (var observer in _observers)
                {
                    if (observer.Value.TryGetValue(key, out var o))
                    {
                        o.OnNext(value);
                    }
                }
            }

            public ISubscription Subscribe(IObserver<T> observer, IEqualityComparer<TKey> comparer, TKey key, IReadOnlyList<TypeErasedKeyBinding<T>> rest)
            {
                var dict = _observers.AddRef(comparer, c => new RefCountingDictionary<TKey, PartitionedMulticaster>(c));
                var subject = dict.AddRef(key, _ => new PartitionedMulticaster());

                return WrappedSubscription.Create(
                    rest.Count == 0 ? subject.Subscribe(observer) : subject.Subscribe(observer, rest),
                    () =>
                    {
                        if (_observers.TryGetValue(comparer, out var d))
                        {
                            d.Release(key);
                            _observers.Release(comparer);
                        }

                        // throw here?
                    }
                );
            }

            public ISubscription Subscribe(IObserver<T> observer, TKey key, IReadOnlyList<TypeErasedKeyBinding<T>> rest)
            {
                return Subscribe(observer, EqualityComparer<TKey>.Default, key, rest);
            }
        }

        private class WrappedObserver : IReliableObserver<T>
        {
            private readonly IObserver<T> _observer;

            public WrappedObserver(IObserver<T> observer)
            {
                _observer = observer;
            }

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId)
            {
                _observer.OnNext(item);
            }

            public void OnStarted()
            {
            }

            public void OnError(Exception error)
            {
                _observer.OnError(error);
            }

            public void OnCompleted()
            {
                _observer.OnCompleted();
            }
        }

        public IReliableSubscription Subscribe(IReliableObserver<T> observer)
        {
            // Needed to pass binder scrutiny; to be revisited later.
            throw new NotImplementedException();
        }

        private class PartitionedMultiSubjectProxy : IPartitionedMultiSubject<T>
        {
            private static readonly IReadOnlyList<TypeErasedKeyBinding<T>> EmptyList = Array.Empty<TypeErasedKeyBinding<T>>().ToReadOnly();

            private readonly Uri _subjectUri;

            public PartitionedMultiSubjectProxy(Uri subjectUri)
            {
                _subjectUri = subjectUri;
            }

            private ISubscription Subscribe(IObserver<T> observer, IReadOnlyList<TypeErasedKeyBinding<T>> bindings)
            {
                return new PartitionedMultiSubjectSubscriptionProxy(Tuple.Create(_subjectUri, bindings), observer);
            }

            internal class PartitionedMultiSubjectSubscriptionProxy : UnaryOperator<Tuple<Uri, IReadOnlyList<TypeErasedKeyBinding<T>>>, T>
            {
                private readonly SingleAssignmentSubscription _subscription = new();

                public PartitionedMultiSubjectSubscriptionProxy(Tuple<Uri, IReadOnlyList<TypeErasedKeyBinding<T>>> args, IObserver<T> observer)
                    : base(args, observer)
                {
                }

                protected override ISubscription OnSubscribe()
                {
                    return _subscription;
                }

                public override void SetContext(IOperatorContext context)
                {
                    base.SetContext(context);

                    if (context.ExecutionEnvironment.GetSubject<T, T>(Params.Item1) is not PartitionedMultiSubject<T> obs)
                        throw new InvalidOperationException();

                    var sub = obs.Subscribe(Output, Params.Item2);
                    new SubscriptionInitializeVisitor(sub).Initialize(context);
                    _subscription.Subscription = sub;
                }
            }

            public IPartitionableSubscribable<T> CreatePartitionableSubscribable()
            {
                return new PartitionableSubscribableImpl(this);
            }

            private class PartitionableSubscribableImpl : PartitionableSubscribableBase<T>
            {
                private readonly PartitionedMultiSubjectProxy _parent;

                public PartitionableSubscribableImpl(PartitionedMultiSubjectProxy parent)
                    : this(parent, bindings: null)
                {
                }

                private PartitionableSubscribableImpl(PartitionedMultiSubjectProxy parent, IList<TypeErasedKeyBinding<T>> bindings)
                    : base(bindings)
                {
                    _parent = parent;
                }

                protected override IPartitionableSubscribable<T> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<T>> bindings)
                {
                    return new PartitionableSubscribableImpl(_parent, bindings);
                }


                protected override ISubscription SubscribeCore(IObserver<T> observer)
                {
                    return _parent.Subscribe(observer, _bindings);
                }
            }

            public ISubscription Subscribe(IObserver<T> observer)
            {
                return Subscribe(observer, EmptyList);
            }

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
            {
                return Subscribe(observer);
            }

            public IObserver<T> CreateObserver()
            {
                throw new NotSupportedException("Use in observer positions not currently supported.");
            }

            public void Dispose()
            {
            }
        }

        private class CustomPartitionDelegator : PartitionDelegator
        {
            private static readonly MethodInfo s_createPartitionableSubscribable = (MethodInfo)ReflectionHelpers.InfoOf((IPartitionedMultiSubject<T> s) => s.CreatePartitionableSubscribable());

            private readonly Expression _subjectProxy;

            public CustomPartitionDelegator(Expression subjectProxy)
            {
                _subjectProxy = subjectProxy;
            }

            protected override Expression CreateSubscribableProxy(Type elementType)
            {
                var subscribableProxy = Expression.Call(_subjectProxy, s_createPartitionableSubscribable);
                return subscribableProxy;
            }
        }

        private PartitionDelegator _delegator;

        private PartitionDelegator Delegator => _delegator ??= new CustomPartitionDelegator(Expression);

        public bool CanDelegate(ParameterExpression node, Expression expression)
        {
            return Delegator.CanDelegate(node, expression);
        }

        public Expression Delegate(ParameterExpression node, Expression expression)
        {
            return Delegator.Delegate(node, expression);
        }

        public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

        public void SetContext(IOperatorContext context)
        {
            _uri = context.InstanceId;
        }

        public void Subscribe() { }

        public void Start() { }

        private static readonly ConstructorInfo s_newProxy = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new PartitionedMultiSubjectProxy(default));

        private Expression _expression;

        public Expression Expression
        {
            get
            {
                if (_expression == null)
                {
                    if (_uri == null)
                        throw new InvalidOperationException("Subject has not yet been initialized.");

                    _expression = Expression.New(s_newProxy, Expression.Constant(_uri));
                }

                return _expression;
            }
        }
    }
}
