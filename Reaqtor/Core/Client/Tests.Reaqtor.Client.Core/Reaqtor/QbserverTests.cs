// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor
{
    [TestClass]
    public class QbserverTests
    {
        [TestMethod]
        public void Qbserver_ReentrancyDuringInitialization_OnNext()
        {
            Qbserver_ReentrancyDuringInitialization_Impl(
                observer => observer.OnNextAsync(42, CancellationToken.None),
                observer => observer.OnNextTask,
                observer => Assert.IsTrue(observer.OnNextLog.SequenceEqual(new[] { 42 }))
            );
        }

        [TestMethod]
        public void Qbserver_ReentrancyDuringInitialization_OnError()
        {
            var ex = new Exception();

            Qbserver_ReentrancyDuringInitialization_Impl(
                observer => observer.OnErrorAsync(ex, CancellationToken.None),
                observer => observer.OnErrorTask,
                observer => Assert.AreSame(observer.OnErrorLog, ex)
            );
        }

        [TestMethod]
        public void Qbserver_ReentrancyDuringInitialization_OnCompleted()
        {
            Qbserver_ReentrancyDuringInitialization_Impl(
                observer => observer.OnCompletedAsync(CancellationToken.None),
                observer => observer.OnCompletedTask,
                observer => Assert.IsTrue(observer.OnCompletedLog)
            );
        }

        private static void Qbserver_ReentrancyDuringInitialization_Impl(Func<IAsyncReactiveQbserver<int>, Task> action, Func<MyObserver<int>, TaskCompletionSource<object>> complete, Action<MyObserver<int>> assert)
        {
            var provider = new MyProvider();
            var observer = provider.CreateQbserver<int>(Expression.Default(typeof(IAsyncReactiveQbserver<int>)));

            var opAsync = action(observer);

            provider.Acquire.WaitOne();

            AssertEx.ThrowsException<AggregateException>(
                () =>
                {
                    observer.OnNextAsync(43, CancellationToken.None).Wait();
                },
                ex =>
                {
                    var ioe = ex.InnerException as InvalidOperationException;
                    Assert.IsNotNull(ioe);
                    Assert.IsTrue(ioe.Message.Contains("Concurrent calls"));
                });

            AssertEx.ThrowsException<AggregateException>(
                () =>
                {
                    observer.OnErrorAsync(new Exception(), CancellationToken.None).Wait();
                },
                ex =>
                {
                    var ioe = ex.InnerException as InvalidOperationException;
                    Assert.IsNotNull(ioe);
                    Assert.IsTrue(ioe.Message.Contains("Concurrent calls"));
                });

            AssertEx.ThrowsException<AggregateException>(
                () =>
                {
                    observer.OnCompletedAsync(CancellationToken.None).Wait();
                },
                ex =>
                {
                    var ioe = ex.InnerException as InvalidOperationException;
                    Assert.IsNotNull(ioe);
                    Assert.IsTrue(ioe.Message.Contains("Concurrent calls"));
                });

            var iv = new MyObserver<int>();

            var tcs = provider.GetObserverAsyncCoreTask;
            tcs.SetResult(iv);

            complete(iv).SetResult(null);
            opAsync.Wait();

            assert(iv);
        }

        [TestMethod]
        public void Qbserver_RegularFlow()
        {
            var provider = new MyProvider();
            var observer = provider.CreateQbserver<int>(Expression.Default(typeof(IAsyncReactiveQbserver<int>)));

            var iv = new MyObserver<int>();

            provider.GetObserverAsyncCoreTask.SetResult(iv);

            iv.OnNextTask.SetResult(null);
            observer.OnNextAsync(42, CancellationToken.None).Wait();
            observer.OnNextAsync(43, CancellationToken.None).Wait();
            observer.OnNextAsync(44, CancellationToken.None).Wait();

            iv.OnCompletedTask.SetResult(null);
            observer.OnCompletedAsync(CancellationToken.None).Wait();

            Assert.IsTrue(iv.OnNextLog.SequenceEqual(new[] { 42, 43, 44 }));
            Assert.IsTrue(iv.OnCompletedLog);
        }

        private sealed class MyObserver<T> : IAsyncReactiveObserver<T>
        {
            public List<T> OnNextLog = new();
            public Exception OnErrorLog;
            public bool OnCompletedLog;

            public TaskCompletionSource<object> OnNextTask = new();
            public TaskCompletionSource<object> OnErrorTask = new();
            public TaskCompletionSource<object> OnCompletedTask = new();

            public Task OnNextAsync(T value, CancellationToken token)
            {
                OnNextLog.Add(value);
                return OnNextTask.Task;
            }

            public Task OnErrorAsync(Exception error, CancellationToken token)
            {
                OnErrorLog = error;
                return OnErrorTask.Task;
            }

            public Task OnCompletedAsync(CancellationToken token)
            {
                OnCompletedLog = true;
                return OnCompletedTask.Task;
            }
        }

        private sealed class MyProvider : AsyncReactiveQueryProviderBase
        {
            public MyProvider()
                : base(new SimpleExpressionServices())
            {
            }

            private sealed class SimpleExpressionServices : IReactiveExpressionServices
            {
                public void RegisterObject(object value, Expression expression)
                {
                }

                public bool TryGetObject(object value, out Expression expression)
                {
                    expression = null;
                    return false;
                }

                public Expression Normalize(Expression expression)
                {
                    return expression;
                }

                public Expression GetNamedExpression(Type type, Uri uri)
                {
                    return Expression.Parameter(type, uri.OriginalString);
                }

                public bool TryGetName(Expression expression, out Uri uri)
                {
                    uri = null;
                    return false;
                }
            }

            public ManualResetEvent Acquire = new(false);
            public TaskCompletionSource<object> GetObserverAsyncCoreTask = new();

            protected override async Task<IAsyncReactiveObserver<T>> GetObserverAsyncCore<T>(IAsyncReactiveQbserver<T> observer, System.Threading.CancellationToken token)
            {
                Acquire.Set();
                var res = await GetObserverAsyncCoreTask.Task;
                return (IAsyncReactiveObserver<T>)res;
            }

            protected override Task CreateSubscriptionAsyncCore(IAsyncReactiveQubscription subscription, object state, System.Threading.CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task DeleteSubscriptionAsyncCore(IAsyncReactiveQubscription subscription, System.Threading.CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task CreateStreamAsyncCore<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, object state, System.Threading.CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task DeleteStreamAsyncCore<TInput, TOutput>(IAsyncReactiveQubject<TInput, TOutput> stream, System.Threading.CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
