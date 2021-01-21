// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.Reliable.Expressions
{
    public interface IReliableQueryProvider
    {
        IReliableQbservable<T> CreateQbservable<T>(Expression expression);

        Func<TArgs, IReliableQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IReliableQbservable<TResult>>> expression);

        IReliableQbserver<T> CreateQbserver<T>(Expression expression);

        Func<TArgs, IReliableQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IReliableQbserver<TResult>>> expression);

        IReliableQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression);

        IReliableQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression);

        IReliableMultiQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression);

        IReliableQubscriptionFactory CreateQubscriptionFactory(Expression expression);

        IReliableQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression);

        IReliableQubscription CreateQubscription(Expression expression);
    }
}
