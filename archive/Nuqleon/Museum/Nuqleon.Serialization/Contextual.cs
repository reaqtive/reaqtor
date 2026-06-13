// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Associates a payload object with a context object.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object.</typeparam>
    /// <typeparam name="TPayload">Type of the payload object.</typeparam>
    public sealed class Contextual<TContext, TPayload>
    {
        /// <summary>
        /// Gets or sets the context object.
        /// </summary>
        public TContext Context { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public TPayload Payload { get; set; }
    }
}
