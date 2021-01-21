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
    public class QueryProvider : IReactiveQueryProvider
    {
        public IReactiveQbservable<T> CreateQbservable<T>(Expression expression) => new MyReactiveQbservable<T>(this, expression);
        public Func<TArgs, IReactiveQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IReactiveQbservable<TResult>>> expression) => throw new NotImplementedException();
        public IReactiveQbserver<T> CreateQbserver<T>(Expression expression) => new MyReactiveQbserver<T>(this, expression);
        public Func<TArgs, IReactiveQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IReactiveQbserver<TResult>>> expression) => throw new NotImplementedException();
        public IReactiveQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression) => new MyReactiveQubject<TInput, TOutput>(this, expression);
        public IReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression) => throw new NotImplementedException();
        public IReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression) => throw new NotImplementedException();
        public IReactiveQubscription CreateQubscription(Expression expression) => new MyReactiveQubscription(this, expression);
        public IReactiveQubscriptionFactory CreateQubscriptionFactory(Expression expression) => throw new NotImplementedException();
        public IReactiveQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression) => throw new NotImplementedException();
    }
}
