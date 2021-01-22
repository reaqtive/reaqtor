// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace Reaqtor.Metadata
{
    /// <summary>
    /// Interface representing a stream in a reactive processing service.
    /// </summary>
    public interface IAsyncReactiveStreamProcess : IAsyncReactiveProcessResource
    {
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
        IAsyncReactiveQubject<TInput, TOutput> ToStream<TInput, TOutput>();
    }
}
