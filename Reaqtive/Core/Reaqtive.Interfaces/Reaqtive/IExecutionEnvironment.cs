// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents the execution environment in which operators execute.
    /// </summary>
    public interface IExecutionEnvironment
    {
        /// <summary>
        /// Gets the subject with the specified identifier from the execution environment.
        /// </summary>
        /// <typeparam name="TInput">Type of the elements received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the elements produced by the subject.</typeparam>
        /// <param name="uri">Identifier of the subject.</param>
        /// <returns>Subject with the specified identifier, obtained from the execution environment.</returns>
        IMultiSubject<TInput, TOutput> GetSubject<TInput, TOutput>(Uri uri);

        /// <summary>
        /// Gets the subscription with the specified identifier from the execution environment.
        /// </summary>
        /// <param name="uri">Identifier of the subscription.</param>
        /// <returns>Subscription with the specified identifier, obtained from the execution environment.</returns>
        ISubscription GetSubscription(Uri uri);
    }
}
