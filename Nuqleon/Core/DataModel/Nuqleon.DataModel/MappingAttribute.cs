// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Nuqleon.DataModel
{
    /// <summary>
    /// Attribute to associate a URI-based identity with a target it is applied to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MappingAttribute : Attribute
    {
        /// <summary>
        /// Creates a new mapping attribute to annotate a target with a URI identifying it.
        /// </summary>
        /// <param name="uri">URI to identify the target the attribute is applied to.</param>
        public MappingAttribute(string uri) => Uri = uri;

        /// <summary>
        /// Gets the URI identifying the target.
        /// </summary>
        public string Uri { get; }
    }
}
