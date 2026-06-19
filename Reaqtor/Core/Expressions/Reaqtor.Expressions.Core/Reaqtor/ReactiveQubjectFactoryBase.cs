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
    /// Base class for the implementation of subject factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the factory.</typeparam>
    public abstract class ReactiveQubjectFactoryBase<TInput, TOutput> : IReactiveQubjectFactory<TInput, TOutput>
    {
        /// <summary>
        /// Creates a new subject factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subject factory.</param>
        protected ReactiveQubjectFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubject<TInput, TOutput> Create(Uri streamUri, object state = null)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, state);
        }

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubject<TInput, TOutput> IReactiveSubjectFactory<TInput, TOutput>.Create(Uri streamUri, object state)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, state);
        }

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, object state);

        /// <summary>
        /// Gets the query provider that is associated with the subject factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

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
    public abstract class ReactiveQubjectFactoryBase<TInput, TOutput, TArgs> : IReactiveQubjectFactory<TInput, TOutput, TArgs>
    {
        /// <summary>
        /// Creates a new subject factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subject factory.</param>
        protected ReactiveQubjectFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubject<TInput, TOutput> Create(Uri streamUri, TArgs argument, object state = null)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, argument, state);
        }

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubject<TInput, TOutput> IReactiveSubjectFactory<TInput, TOutput, TArgs>.Create(Uri streamUri, TArgs argument, object state)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, argument, state);
        }

        /// <summary>
        /// Creates a new stream with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArgs argument, object state);

        /// <summary>
        /// Gets the query provider that is associated with the subject factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subject factory.
        /// </summary>
        public abstract Expression Expression { get; }
    }
}
