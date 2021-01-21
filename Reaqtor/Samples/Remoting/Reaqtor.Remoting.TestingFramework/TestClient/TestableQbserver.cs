// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive.Testing;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TestableQbserver<T> : ITestObserver<T>, IAsyncReactiveQbserver<T>
    {
        #region Constructors & Fields

        private readonly Uri _observerId;
        private readonly IAsyncReactiveQbserver<T> _inner;
        private readonly IReactivePlatform _platform;

        public TestableQbserver(Uri observerId, IAsyncReactiveQbserver<T> inner, IReactivePlatform platform)
        {
            _observerId = observerId;
            _inner = inner;
            _platform = platform;
        }

        #endregion

        #region ITestObserver<T>

        public IList<Recorded<INotification<T>>> Messages
        {
            get
            {
                var testQE = _platform.QueryEvaluators.First().GetInstance<TestQueryEvaluatorServiceConnection>();
                testQE.TestObservers.TryGetValue(_observerId.ToCanonicalString(), out var testObserver);
                return Helpers.DeserializeObserverMessages<T>(testObserver.Messages).ToList();
            }
        }

        #endregion

        #region IObserver<T>

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAsyncReactiveQbserver<T>

        public Task OnNextAsync(T value, CancellationToken token)
        {
            return _inner.OnNextAsync(value, token);
        }

        public Task OnErrorAsync(Exception error, CancellationToken token)
        {
            return _inner.OnErrorAsync(error, token);
        }

        public Task OnCompletedAsync(CancellationToken token)
        {
            return _inner.OnCompletedAsync(token);
        }

        public Type ElementType => _inner.ElementType;

        public IAsyncReactiveQueryProvider Provider => _inner.Provider;

        public Expression Expression => _inner.Expression;

        #endregion
    }
}
