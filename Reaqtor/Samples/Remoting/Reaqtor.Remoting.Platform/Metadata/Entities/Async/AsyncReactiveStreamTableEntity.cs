// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Table entity representing a stream.
    /// </summary>
    public class AsyncReactiveStreamTableEntity : AsyncReactiveProcessResourceTableEntity, IAsyncReactiveStreamProcess
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public AsyncReactiveStreamTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing a stream with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the stream represented by the table entity.</param>
        /// <param name="expression">Expression representation of the stream.</param>
        /// <param name="state">The state.</param>
        public AsyncReactiveStreamTableEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        /// <summary>
        /// Gets a subject that can be used to interact with the active stream.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
        /// <returns>Subject object to interact with the stream.</returns>
        /// <remarks>
        /// Implementation of this method requires access to a client library that can communicate with
        /// the reactive processing service (beyond the metadata layer). An implementation is allowed
        /// to discover a communication mechanism on the fly rather than relying on any particular client
        /// library. This allows dynamic discovery of streams and their use, e.g. in federation and
        /// delegation scenarios.
        /// </remarks>
        public IAsyncReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on table entities.");
        }
    }
}
