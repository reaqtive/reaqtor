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
    /// <remarks>
    /// Creates a new mapping attribute to annotate a target with a URI identifying it.
    /// </remarks>
    /// <param name="uri">URI to identify the target the attribute is applied to.</param>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MappingAttribute(string uri) : Attribute
    {

        /// <summary>
        /// Gets the URI identifying the target.
        /// </summary>
        public string Uri { get; } = uri;
    }
}
