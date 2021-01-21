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
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of subject factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the factory.</typeparam>
    public abstract class AsyncReactiveQubjectFactoryBase<TInput, TOutput> : IAsyncReactiveQubjectFactory<TInput, TOutput>
    {
        /// <summary>
        /// Creates a new subject factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subject factory.</param>
        protected AsyncReactiveQubjectFactoryBase(IAsyncReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        public Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsync(Uri streamUri, object state = null, CancellationToken token = default)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateAsyncCore(streamUri, state, token);
        }

        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> IAsyncReactiveSubjectFactory<TInput, TOutput>.CreateAsync(Uri streamUri, object state, CancellationToken token)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateAsyncCore(streamUri, state, token).ContinueWith(t => (IAsyncReactiveSubject<TInput, TOutput>)t.Result, token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        protected abstract Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, object state, CancellationToken token);

        /// <summary>
        /// Gets the query provider that is associated with the subject factory.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subject factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }

    /// <summary>
    /// Base class for the implementation of parameterized subject factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the factory.</typeparam>
    /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>
    public abstract class AsyncReactiveQubjectFactoryBase<TInput, TOutput, TArgs> : IAsyncReactiveQubjectFactory<TInput, TOutput, TArgs>
    {
        /// <summary>
        /// Creates a new subject factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subject factory.</param>
        protected AsyncReactiveQubjectFactoryBase(IAsyncReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        public Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArgs argument, object state = null, CancellationToken token = default)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateAsyncCore(streamUri, argument, state, token);
        }

        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> IAsyncReactiveSubjectFactory<TInput, TOutput, TArgs>.CreateAsync(Uri streamUri, TArgs argument, object state, CancellationToken token)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateAsyncCore(streamUri, argument, state, token).ContinueWith(t => (IAsyncReactiveSubject<TInput, TOutput>)t.Result, token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
        }

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        protected abstract Task<IAsyncReactiveQubject<TInput, TOutput>> CreateAsyncCore(Uri streamUri, TArgs argument, object state, CancellationToken token);

        /// <summary>
        /// Gets the query provider that is associated with the subject factory.
        /// </summary>
        public IAsyncReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subject factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
