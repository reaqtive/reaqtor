// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reflection;

using Reaqtive;

using Reaqtor.QueryEngine;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public abstract class PartitionedMultiSubject : StatefulMultiSubjectBase, IDynamicMultiSubject, IDelegationTarget, IPartitionedMultiSubject, IExpressible, IOperator
    {
        private readonly RefCountingDictionary<Type, Tuple<object, IDisposable>> _map = new();

        protected sealed override IObserver<T> GetObserverCore<T>() => GetObserverCoreCore<T>();

        protected abstract IObserver<T> GetObserverCoreCore<T>();

        protected abstract ISubscribable<T> GetObservableCoreCore<T>();

        protected sealed override ISubscribable<T> GetObservableCore<T>() => new ObservableImpl<T>(this);

        private sealed class ObservableImpl<T> : PartitionableSubscribableBase<T>
        {
            private readonly PartitionedMultiSubject _parent;

            public ObservableImpl(PartitionedMultiSubject parent)
                : this(parent, null)
            {
            }

            private ObservableImpl(PartitionedMultiSubject parent, IList<TypeErasedKeyBinding<T>> bindings)
                : base(bindings)
            {
                _parent = parent;
            }

            protected override IPartitionableSubscribable<T> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<T>> bindings) => new ObservableImpl<T>(_parent, bindings);

            protected override ISubscription SubscribeCore(IObserver<T> observer) => _parent.Subscribe<T>(observer, _bindings);
        }

        private ISubscription Subscribe<T>(IObserver<T> observer, IReadOnlyList<TypeErasedKeyBinding<T>> bindings)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            var sad = new SingleAssignmentDisposable();

            var subj = _map.AddRef(typeof(T), t => new Tuple<object, IDisposable>(new PartitionedMultiSubject<T>(), sad));

            var pms = (PartitionedMultiSubject<T>)subj.Item1;

            if (sad == subj.Item2)
            {
                var dis = GetObservableCoreCore<T>().Subscribe(pms.CreateObserver());
                new SubscriptionInitializeVisitor(dis).Initialize(_context);
                sad.Disposable = dis;
            }

            return new SubscriptionImpl<T>(this, pms.Subscribe(observer, bindings));
#pragma warning restore CA2000
        }

        private void Unsubscribe<T>()
        {
            if (_map.Release(typeof(T), out Tuple<object, IDisposable> underlyingSubscription))
            {
                underlyingSubscription.Item2.Dispose();
            }
        }

        private sealed class SubscriptionImpl<T> : ISubscription
        {
            private readonly PartitionedMultiSubject _parent;
            private readonly ISubscription _inner;

            public SubscriptionImpl(PartitionedMultiSubject parent, ISubscription inner)
            {
                _parent = parent;
                _inner = inner;
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
            }

            public void Dispose()
            {
                _inner.Dispose();
                _parent.Unsubscribe<T>();
            }
        }

        private IDelegationTarget _delegator;

        public bool CanDelegate(ParameterExpression node, Expression expression) => _delegator.CanDelegate(node, expression);

        public Expression Delegate(ParameterExpression node, Expression expression) => _delegator.Delegate(node, expression);

        private sealed class PartitionedMultiSubjectProxy : IPartitionedMultiSubject
        {
            private readonly Uri _subjectUri;

            public PartitionedMultiSubjectProxy(Uri subjectUri) => _subjectUri = subjectUri;

            private ISubscription Subscribe<T>(IObserver<T> observer, IReadOnlyList<TypeErasedKeyBinding<T>> bindings)
            {
                return new PartitionedMultiSubjectSubscriptionProxy<T>(Tuple.Create(_subjectUri, bindings), observer);
            }

            internal class PartitionedMultiSubjectSubscriptionProxy<T> : UnaryOperator<Tuple<Uri, IReadOnlyList<TypeErasedKeyBinding<T>>>, T>
            {
                private readonly SingleAssignmentSubscription _subscription = new();

                public PartitionedMultiSubjectSubscriptionProxy(Tuple<Uri, IReadOnlyList<TypeErasedKeyBinding<T>>> args, IObserver<T> observer)
                    : base(args, observer)
                {
                }

                protected override ISubscription OnSubscribe() => _subscription;

                public override void SetContext(IOperatorContext context)
                {
                    base.SetContext(context);

                    if (context.ExecutionEnvironment.GetSubject<T, T>(Params.Item1) is not ToTypedMultiSubject<T, T> obs)
                        throw new InvalidOperationException();

                    var sub = obs._innerSubject.Subscribe(Output, Params.Item2);
                    new SubscriptionInitializeVisitor(sub).Initialize(context);
                    _subscription.Subscription = sub;
                }
            }

            public IPartitionableSubscribable<T> CreatePartitionableSubscribable<T>() => new PartitionableSubscribableImpl<T>(this);

            private sealed class PartitionableSubscribableImpl<T> : PartitionableSubscribableBase<T>
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

                protected override IPartitionableSubscribable<T> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<T>> bindings) => new PartitionableSubscribableImpl<T>(_parent, bindings);


                protected override ISubscription SubscribeCore(IObserver<T> observer) => _parent.Subscribe(observer, _bindings);
            }

            public IObserver<T> GetObserver<T>() => throw new NotSupportedException("Use in observer positions not currently supported.");

            public ISubscribable<T> GetObservable<T>() => CreatePartitionableSubscribable<T>();

            public void Dispose()
            {
            }
        }

        private sealed class MyDelegator : PartitionDelegator
        {
            private static readonly MethodInfo s_createPartitionedSubscribable = ((MethodInfo)ReflectionHelpers.InfoOf((IPartitionedMultiSubject s) => s.CreatePartitionableSubscribable<T>())).GetGenericMethodDefinition();

            private readonly Expression _subjectProxy;

            public MyDelegator(Expression subjectProxy)
            {
                _subjectProxy = subjectProxy;
            }

            protected override Expression CreateSubscribableProxy(Type elementType)
            {
                return Expression.Call(_subjectProxy, s_createPartitionedSubscribable.MakeGenericMethod(elementType));
            }
        }

        public IPartitionableSubscribable<T> CreatePartitionableSubscribable<T>() => new ObservableImpl<T>(this);

        private Expression _expression;
        private Uri _uri;
        private IOperatorContext _context;
        private static readonly ConstructorInfo s_proxyConstructor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new PartitionedMultiSubjectProxy(default));

        public Expression Expression
        {
            get
            {
                if (_expression == null)
                {
                    if (_uri == null)
                        throw new InvalidOperationException("Subject has not yet been initialized.");

                    _expression = Expression.New(s_proxyConstructor, Expression.Constant(_uri));
                }

                return _expression;
            }
        }

        public override void SetContext(IOperatorContext context)
        {
            Debug.Assert(context != null);

            base.SetContext(context);

            _uri = context.InstanceId;
            // _uri must be set before initializing _delegator because Expression depends on _uri
            _delegator = new MyDelegator(Expression);
            _context = context;
        }

        public IMultiSubject<TInput, TOutput> ToTyped<TInput, TOutput>() => new ToTypedMultiSubject<TInput, TOutput>(this);

        private sealed class ToTypedMultiSubject<TInput, TOutput> : IMultiSubject<TInput, TOutput>
        {
            internal readonly PartitionedMultiSubject _innerSubject;

            public ToTypedMultiSubject(PartitionedMultiSubject innerSubject) => _innerSubject = innerSubject;

            public IObserver<TInput> CreateObserver() => _innerSubject.GetObserver<TInput>();

            public ISubscription Subscribe(IObserver<TOutput> observer) => _innerSubject.GetObservable<TOutput>().Subscribe(observer);

            IDisposable IObservable<TOutput>.Subscribe(IObserver<TOutput> observer) => Subscribe(observer);

            public void Dispose() => _innerSubject.Dispose();
        }
    }
}
