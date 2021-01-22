// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a query engine exposing a reliable reactive service, used for cross-engine communication.
    /// </summary>
    public interface IReliableQueryEngine : IQueryEngine
    {
        /// <summary>
        /// Gets the reliable reactive service exposed by the engine.
        /// </summary>
        IReliableReactive ReliableReactiveService { get; }
    }
}
