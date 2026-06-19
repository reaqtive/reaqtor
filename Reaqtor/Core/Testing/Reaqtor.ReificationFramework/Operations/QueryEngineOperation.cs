// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation applied to a query engine.
    /// </summary>
    [Serializable]
    public class QueryEngineOperation
    {
        /// <summary>
        /// Creates a query engine operation.
        /// </summary>
        /// <param name="kind">The kind of reified operation.</param>
        /// <param name="targetObjectUri">The target query engine instance.</param>
        /// <param name="state">The state passed to the operation.</param>
        public QueryEngineOperation(QueryEngineOperationKind kind, Uri targetObjectUri, object state)
        {
            Kind = kind;
            TargetObjectUri = targetObjectUri;
            State = state;
        }

        /// <summary>
        /// The kind of reified operation.
        /// </summary>
        public QueryEngineOperationKind Kind { get; }

        /// <summary>
        /// The target query engine instance.
        /// </summary>
        public Uri TargetObjectUri { get; }

        /// <summary>
        /// The state passed to the operation.
        /// </summary>
        public object State { get; }

        /// <summary>
        /// Gets a string representation of the operation.
        /// </summary>
        /// <returns>A string representation of the operation.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}({1}, {2})", Kind, TargetObjectUri, State);
        }
    }
}
