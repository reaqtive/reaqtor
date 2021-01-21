// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table entities representing process resources that have a creation time and were created with a state.
    /// </summary>
    public abstract class ReactiveProcessResourceTableEntity : ReactiveResourceTableEntity, IReactiveProcessResource
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public ReactiveProcessResourceTableEntity()
        {
        }

        // TODO: support serialization of state using the same tricks and using the DataSerializer.

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
    }
}
