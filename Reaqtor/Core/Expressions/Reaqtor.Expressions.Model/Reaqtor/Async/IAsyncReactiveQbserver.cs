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
    /// Interface for an observer represented by an expression tree.
    /// </summary>
    public interface IAsyncReactiveQbserver : IAsyncReactiveExpressible
    {
        /// <summary>
        /// Gets the type of the data received by the observer.
        /// </summary>
        Type ElementType { get; }
    }
}
