// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table entities representing process resources that have a creation time and were created with a state.
    /// </summary>
    public abstract class AsyncReactiveProcessResourceTableEntity : AsyncReactiveResourceTableEntity, IAsyncReactiveProcessResource
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public AsyncReactiveProcessResourceTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing a known expressible resource with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the resource represented by the table entity.</param>
        /// <param name="expression">Expression representation of the resource.</param>
        /// <param name="state">The state.</param>
        public AsyncReactiveProcessResourceTableEntity(Uri uri, Expression expression, object state)
            : base(uri, expression)
        {
            CreationTime = DateTime.Now;
            State = state;
        }

        // TODO: Support serialization of state using the same tricks and using the DataSerializer.

        /// <summary>
        /// Gets the state that was passed during creation of the resource.
        /// </summary>
        /// <remarks>Implementers can provide statically typed accessors in derived types.</remarks>
        public object State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date and time when the process was created.
        /// </summary>
        public DateTimeOffset CreationTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Disposes the resource asynchronously.
        /// </summary>
        /// <param name="token">Token to observe for cancellation of the disposal request.</param>
        /// <returns>Task representing the eventual completion of the disposal request.</returns>
        public Task DisposeAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on metadata entities.");
        }
    }
}
