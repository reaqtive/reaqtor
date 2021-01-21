// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Interface for a subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    public interface IAsyncReactiveQubjectFactory<TInput, TOutput> : IAsyncReactiveSubjectFactory<TInput, TOutput>, IAsyncReactiveExpressible
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        new Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsync(Uri streamUri, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs> : IAsyncReactiveSubjectFactory<TInput, TOutput, TArgs>, IAsyncReactiveExpressible
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        new Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArgs argument, object state = null, CancellationToken token = default);
    }
}
