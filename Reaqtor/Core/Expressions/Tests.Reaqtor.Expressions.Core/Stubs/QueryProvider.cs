// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

using System;
using System.Linq.Expressions;

using Reaqtor;

namespace Tests.Reaqtor.Expressions.Core
{
    public class AsyncQueryProvider : IAsyncReactiveQueryProvider
    {
        public IAsyncReactiveQbservable<T> CreateQbservable<T>(Expression expression) => new MyAsyncReactiveQbservable<T>(this, expression);
        public Func<TArgs, IAsyncReactiveQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> expression) => throw new NotImplementedException();
        public IAsyncReactiveQbserver<T> CreateQbserver<T>(Expression expression) => new MyAsyncReactiveQbserver<T>(this, expression);
        public Func<TArgs, IAsyncReactiveQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> expression) => throw new NotImplementedException();
        public IAsyncReactiveQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression) => new MyAsyncReactiveQubject<TInput, TOutput>(this, expression);
        public IAsyncReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression) => throw new NotImplementedException();
        public IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression) => throw new NotImplementedException();
        public IAsyncReactiveQubscription CreateQubscription(Expression expression) => new MyAsyncReactiveQubscription(this, expression);
        public IAsyncReactiveQubscriptionFactory CreateQubscriptionFactory(Expression expression) => throw new NotImplementedException();
        public IAsyncReactiveQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression) => throw new NotImplementedException();
    }
}
