// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Threading;

using Reaqtive;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private class NotSoSimpleUntypedSubject<T> : MultiSubjectBase, IReactiveQubject<T>, ISealable, IDisposable
        {
#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood OnDispose
            private readonly SimpleSubject<T> _subject = new();
#pragma warning restore CA2213

            private readonly TestExecutionEnvironment _parent;
            private readonly Uri _streamUri;
            private int _count;

            public NotSoSimpleUntypedSubject(TestExecutionEnvironment parent, Uri streamUri)
            {
                _parent = parent;
                _streamUri = streamUri;
            }

            public int RefCount => _count;

            protected virtual void AddRef()
            {
                Interlocked.Increment(ref _count);
            }

            protected virtual void Release()
            {
                if (Interlocked.Decrement(ref _count) == 0)
                {
                    _parent.RemoveArtifact(_streamUri);
                }
            }

            #region MultiSubjectBase

            protected override IObserver<TInput> GetObserverCore<TInput>() => (IObserver<TInput>)_subject.CreateObserver();

            protected override ISubscribable<TOutput> GetObservableCore<TOutput>() => new Impl<TOutput>((NotSoSimpleUntypedSubject<TOutput>)(object)this);

            protected override void OnDispose()
            {
                base.Dispose();
                _parent.RemoveArtifact(_streamUri);
                _subject.Dispose();
            }

            private sealed class Impl<TOutput> : SubscribableBase<TOutput>
            {
                private readonly NotSoSimpleUntypedSubject<TOutput> _parent;

                public Impl(NotSoSimpleUntypedSubject<TOutput> parent)
                {
                    _parent = parent;
                }

                protected override ISubscription SubscribeCore(IObserver<TOutput> observer)
                {
                    var res = new Subscription<TOutput>(_parent);

                    _parent.AddRef();

                    res.Inner = _parent._subject.Subscribe(observer);

                    return res;
                }
            }

            private sealed class Subscription<TOutput> : ISubscription
            {
                private readonly NotSoSimpleUntypedSubject<TOutput> _parent;
                private int _disposed;

                public Subscription(NotSoSimpleUntypedSubject<TOutput> parent)
                {
                    _parent = parent;
                }

                public ISubscription Inner;

                public void Accept(ISubscriptionVisitor visitor)
                {
                    Inner.Accept(visitor);
                }

                public void Dispose()
                {
                    if (Interlocked.Exchange(ref _disposed, 1) == 0)
                    {
                        _parent.Release();
                    }

                    Inner.Dispose();
                }
            }

            #endregion

            #region IReactiveQubject<T>

            public void OnNext(T value) => _subject.OnNext(value);

            public void OnError(Exception error) => _subject.OnError(error);

            public void OnCompleted() => _subject.OnCompleted();

            #region Not Implemented

            public IReactiveSubscription Subscribe(IReactiveObserver<T> observer, Uri subscriptionUri, object state)
            {
                throw new NotImplementedException();
            }

            public Type ElementType => throw new NotImplementedException();

            public IReactiveQueryProvider Provider => throw new NotImplementedException();

            public Expression Expression => throw new NotImplementedException();

            public IReactiveQubscription Subscribe(IReactiveQbserver<T> observer, Uri subscriptionUri, object state)
            {
                throw new NotImplementedException();
            }

            #endregion

            #endregion

            #region ISealable

            public void Seal()
            {
                if (RefCount == 0)
                {
                    // Not disposing; Reaqtor.Subject<T> will throw when attempting to publish into disposed subscription.
                    // Good enough for testing.
                    _parent.RemoveArtifact(_streamUri);
                }
            }

            #endregion
        }
    }
}
