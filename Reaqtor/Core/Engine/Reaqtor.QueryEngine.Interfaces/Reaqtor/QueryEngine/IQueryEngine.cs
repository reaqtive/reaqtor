// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a query engine.
    /// </summary>
    public interface IQueryEngine
    {
        /// <summary>
        /// Gets the URI identifying the engine.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Gets the reactive service exposed by the engine.
        /// </summary>
        IReactive ReactiveService { get; }
    }
}
