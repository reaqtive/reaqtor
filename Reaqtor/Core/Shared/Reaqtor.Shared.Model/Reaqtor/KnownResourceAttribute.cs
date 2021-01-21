// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Attribute to denote known resources, which can be represented using a URI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class KnownResourceAttribute : Attribute
    {
        /// <summary>
        /// Creates a new attribute with the specified URI identifying the resource the attribute is applied to.
        /// </summary>
        /// <param name="uri">URI that identifies the resource.</param>
        public KnownResourceAttribute(string uri) => Uri = uri;

        /// <summary>
        /// Gets the URI that identifies the resource.
        /// </summary>
        public string Uri { get; }
    }
}
