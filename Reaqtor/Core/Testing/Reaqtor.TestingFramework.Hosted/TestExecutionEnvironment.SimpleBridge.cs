// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.Reactive.Expressions;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private class SimpleBridge<T> : SimpleSubject<T>, IReactiveQubject<T>, ISubscribable<T>, IDisposable
        {
            private readonly TestExecutionEnvironment _parent;
            private readonly Uri _streamUri;
            private readonly Expression _source;

            public SimpleBridge(TestExecutionEnvironment parent, Uri streamUri, Expression source)
            {
                _parent = parent;
                _streamUri = streamUri;
                _source = source;
            }

            ISubscription ISubscribable<T>.Subscribe(IObserver<T> observer)
            {
                if (_parent.BridgeSubscriptionError != null)
                {
                    throw _parent.BridgeSubscriptionError;
                }

                return new QuotedSubscribable<T>(_source).Subscribe(observer);
            }

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
            {
                return ((ISubscribable<T>)this).Subscribe(observer);
            }

            #region Not implemented

            public IReactiveQubscription Subscribe(IReactiveQbserver<T> observer, Uri subscriptionUri, object state)
            {
                throw new NotImplementedException();
            }

            public IReactiveSubscription Subscribe(IReactiveObserver<T> observer, Uri subscriptionUri, object state)
            {
                throw new NotImplementedException();
            }

            public Type ElementType => typeof(T);

            public IReactiveQueryProvider Provider => throw new NotImplementedException();

            public Expression Expression => throw new NotImplementedException();

            #endregion

            void IDisposable.Dispose()
            {
                base.Dispose();
                _parent.RemoveArtifact(_streamUri);
            }
        }
    }
}
