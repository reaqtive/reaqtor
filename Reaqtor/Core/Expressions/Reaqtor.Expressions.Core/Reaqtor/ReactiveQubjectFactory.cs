// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Reaqtor.Expressions.Core
{
    /// <summary>
    /// Signatures of the available subject factories to be used in global definitions (DefineStreamFactory).
    /// This class does not contain implementations. Expressions using the subject factories have to be 
    /// rebound to a perticular implementation (Rx or Subscribable) before they can be executed.
    /// </summary>
    public static class ReactiveQubjectFactory
    {
        /// <summary>
        /// Gets a subject factory.
        /// </summary>
        /// <typeparam name="T">Type of the elements processed by the subjects returned by the factory.</typeparam>
        /// <returns>Instance of a subject factory.</returns>
        [ExcludeFromCodeCoverage]
        public static IReactiveQubjectFactory<T, T> GetSubjectFactory<T>()
        {
            throw new NotImplementedException();
        }

        #region Helper Methods

        /// <summary>
        /// Makes a subject factory with the specified expression representation.
        /// </summary>
        /// <typeparam name="TInput">Type of the elements received by the subjects returned by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the elements produced by the subjects returned by the factory.</typeparam>
        /// <param name="provider">Query provider to use for creation of the subject factory expression representation.</param>
        /// <param name="expression">Expression representation of the subject factory.</param>
        /// <returns>Instance of a subject factory with the specified expression tree as its representation.</returns>
        public static IReactiveQubjectFactory<TInput, TOutput> MakeQubjectFactory<TInput, TOutput>(this IReactiveQueryProvider provider, Expression<Func<IReactiveQubjectFactory<TInput, TOutput>>> expression)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return provider.CreateQubjectFactory<TInput, TOutput>(expression.Body);
        }

        /// <summary>
        /// Makes a parameterized subject factory with the specified expression representation.
        /// </summary>
        /// <typeparam name="TInput">Type of the elements received by the subjects returned by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the elements produced by the subjects returned by the factory.</typeparam>
        /// <typeparam name="TArg">Type of the argument passed to the stream factory.</typeparam>
        /// <param name="provider">Query provider to use for creation of the subject factory expression representation.</param>
        /// <param name="expression">Expression representation of the subject factory.</param>
        /// <returns>Instance of a parameterized subject factory with the specified expression tree as its representation.</returns>
        public static IReactiveQubjectFactory<TInput, TOutput, TArg> MakeQubjectFactory<TArg, TInput, TOutput>(this IReactiveQueryProvider provider, Expression<Func<IReactiveQubjectFactory<TInput, TOutput, TArg>>> expression)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return provider.CreateQubjectFactory<TArg, TInput, TOutput>(expression.Body);
        }

        #endregion
    }
}
