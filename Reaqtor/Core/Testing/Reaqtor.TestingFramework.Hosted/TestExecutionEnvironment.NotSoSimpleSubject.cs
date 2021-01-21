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
        private class NotSoSimpleSubject<T> : SimpleSubject<T>, IReactiveQubject<T>, ISubscribable<T>, ISealable, IDisposable
        {
            private readonly TestExecutionEnvironment _parent;
            private readonly Uri _streamUri;
            private int _count;

            public NotSoSimpleSubject(TestExecutionEnvironment parent, Uri streamUri)
            {
                _parent = parent;
                _streamUri = streamUri;
            }

            public int RefCount => _count;

            ISubscription ISubscribable<T>.Subscribe(IObserver<T> observer)
            {
                var res = new Subscription(this);

                AddRef();

                res.Inner = base.Subscribe(observer);

                return res;
            }

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

            private sealed class Subscription : ISubscription
            {
                private readonly NotSoSimpleSubject<T> _parent;
                private int _disposed;

                public Subscription(NotSoSimpleSubject<T> parent)
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

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
            {
                return ((ISubscribable<T>)this).Subscribe(observer);
            }

            #region Not implemented

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

            public void Seal()
            {
                if (RefCount == 0)
                {
                    // Not disposing; Reaqtor.Subject<T> will throw when attempting to publish into disposed subscription.
                    // Good enough for testing.
                    _parent.RemoveArtifact(_streamUri);
                }
            }

            void IDisposable.Dispose()
            {
                base.Dispose();
                _parent.RemoveArtifact(_streamUri);
            }
        }
    }
}
