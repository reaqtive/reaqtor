// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Interface for reactive processing query providers, used to build observables, observers, subjects, and subscriptions represented by expression trees.
    /// </summary>
    public interface IAsyncReactiveQueryProvider
    {
        /// <summary>
        /// Creates an observable based on the given expression.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Observable represented by the given expression.</returns>
        IAsyncReactiveQbservable<T> CreateQbservable<T>(Expression expression);

        /// <summary>
        /// Creates a parameterized observable based on the given expression.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        Func<TArgs, IAsyncReactiveQbservable<TResult>> CreateQbservable<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbservable<TResult>>> expression);

        /// <summary>
        /// Creates an observer based on the given expression.
        /// </summary>
        /// <typeparam name="T">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Observer represented by the given expression.</returns>
        IAsyncReactiveQbserver<T> CreateQbserver<T>(Expression expression);

        /// <summary>
        /// Creates a parameterized observer based on the given expression.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        Func<TArgs, IAsyncReactiveQbserver<TResult>> CreateQbserver<TArgs, TResult>(Expression<Func<TArgs, IAsyncReactiveQbserver<TResult>>> expression);

        /// <summary>
        /// Creates a subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Subject factory represented by the given expression.</returns>
        IAsyncReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(Expression expression);

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(Expression expression);

        /// <summary>
        /// Creates a subject based on the given expression.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <param name="expression">Expression representing the subject.</param>
        /// <returns>Subject represented by the given expression.</returns>
        IAsyncReactiveQubject<TInput, TOutput> CreateQubject<TInput, TOutput>(Expression expression);

        /// <summary>
        /// Creates a subscription factory based on the given expression.
        /// </summary>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Subscription factory represented by the given expression.</returns>
        IAsyncReactiveQubscriptionFactory CreateQubscriptionFactory(Expression expression);

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        IAsyncReactiveQubscriptionFactory<TArgs> CreateQubscriptionFactory<TArgs>(Expression expression);

        /// <summary>
        /// Creates a subscription based on the given expression.
        /// </summary>
        /// <param name="expression">Expression representing the subscription.</param>
        /// <returns>Subscription represented by the given expression.</returns>
        IAsyncReactiveQubscription CreateQubscription(Expression expression);
    }
}
