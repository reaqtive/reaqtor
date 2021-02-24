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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of static methods to work with query providers.
    /// </summary>
    public static class ReactiveQueryProviderExtensions
    {
        #region CreateQbservable

        /// <summary>
        /// Creates an observable from the specified observable's <see cref="IReactiveQbservable{T}.Subscribe"/> implementation method.
        /// </summary>
        /// <typeparam name="T">Type of the data produced by the observable.</typeparam>
        /// <param name="provider">Provider to use for creation of the observable.</param>
        /// <param name="subscribe">The observable's <see cref="IReactiveQbservable{T}.Subscribe"/> method implementation.</param>
        /// <returns>Newly created observable. To define the observable on a service, use <c>IReactiveDefinition.DefineObservable</c>.</returns>
        public static IReactiveQbservable<T> CreateQbservable<T>(this IReactiveQueryProvider provider, Expression<Func<IReactiveQbserver<T>, Uri, object, CancellationToken, Task<IReactiveQubscription>>> subscribe)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (subscribe == null)
                throw new ArgumentNullException(nameof(subscribe));

            return provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    Expression.Constant(provider, typeof(IReactiveQueryProvider)),
                    subscribe
                )
            );
        }

        #endregion

        #region CreateQubjectFactory

        /// <summary>
        /// Creates a subject factory from the specified factory's <see cref="IReactiveQubjectFactory{TInput, TOutput}.Create"/> implementation method.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="provider">Provider to use for creation of the factory.</param>
        /// <param name="create">The factory's <see cref="IReactiveQubjectFactory{TInput, TOutput}.Create"/> method implementation.</param>
        /// <returns>Newly created stream factory. To define the factory on a service, use <c>IReactiveDefinition.DefineStreamFactory</c>.</returns>
        public static IReactiveQubjectFactory<TInput, TOutput> CreateQubjectFactory<TInput, TOutput>(this IReactiveQueryProvider provider, Expression<Func<Uri, object, CancellationToken, Task<IReactiveQubject<TInput, TOutput>>>> create)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (create == null)
                throw new ArgumentNullException(nameof(create));

            return provider.CreateQubjectFactory<TInput, TOutput>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TInput), typeof(TOutput)),
                    Expression.Constant(provider, typeof(IReactiveQueryProvider)),
                    create
                )
            );
        }

        /// <summary>
        /// Creates a parameterized subject factory from the specified factory's <see cref="IReactiveQubjectFactory{TInput, TOutput, TArgs}.Create"/> implementation method.
        /// </summary>
        /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="provider">Provider to use for creation of the factory.</param>
        /// <param name="create">The factory's <see cref="IReactiveQubjectFactory{TInput, TOutput, TArgs}.Create"/> method implementation.</param>
        /// <returns>Newly created parameterized stream factory. To define the factory on a service, use <c>IReactiveDefinition.DefineStreamFactory</c>.</returns>
        public static IReactiveQubjectFactory<TInput, TOutput, TArgs> CreateQubjectFactory<TArgs, TInput, TOutput>(this IReactiveQueryProvider provider, Expression<Func<Uri, TArgs, object, CancellationToken, Task<IReactiveQubject<TInput, TOutput>>>> create)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (create == null)
                throw new ArgumentNullException(nameof(create));

            return provider.CreateQubjectFactory<TArgs, TInput, TOutput>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TArgs), typeof(TInput), typeof(TOutput)),
                    Expression.Constant(provider, typeof(IReactiveQueryProvider)),
                    create
                )
            );
        }

        #endregion
    }
}
