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
    /// Interface for resources that have a known identity.
    /// </summary>
    public interface IKnownResource
    {
        /// <summary>
        /// Gets the URI that identifies the resource.
        /// </summary>
        Uri Uri { get; }
    }
}
