// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.TestingFramework;
using System;
using System.Linq.Expressions;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Interface for binding reified operation to a given environment.
    /// </summary>
    /// <typeparam name="TEnvironment">The environment type.</typeparam>
    public interface IReificationBinder<TEnvironment>
    {
        /// <summary>
        /// Binds a service operation to the environment.
        /// </summary>
        /// <param name="operation">The operation to bind.</param>
        /// <returns>
        /// A lambda expression that can be evaluated with an environment
        /// instance returned from the `CreateEnvironment` method.
        /// </returns>
        Expression<Action<TEnvironment>> Bind(ServiceOperation operation);

        /// <summary>
        /// Binds a query engine operation to the environment.
        /// </summary>
        /// <param name="operation">The operation to bind.</param>
        /// <returns>
        /// A lambda expression that can be evaluated with an environment
        /// instance returned from the `CreateEnvironment` method.
        /// </returns>
        Expression<Action<TEnvironment>> Bind(QueryEngineOperation operation);

        /// <summary>
        /// Optimizes an expression a bound reified operation.
        /// </summary>
        /// <param name="expression">The expression to optimize.</param>
        /// <returns>The optimized expression.</returns>
        /// <remarks>
        /// E.g., an optimization might share resources over successive calls in a loop.
        /// </remarks>
        Expression<Action<TEnvironment>> Optimize(Expression<Action<TEnvironment>> expression);

        /// <summary>
        /// Creates a fresh instance of the environment.
        /// </summary>
        /// <returns>A fresh instance of the environment.</returns>
        TEnvironment CreateEnvironment();
    }
}
