// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to perform a full checkpoint.
    /// </summary>
    [Serializable]
    public class FullCheckpoint : QueryEngineOperation
    {
        /// <summary>
        /// Creates a new full checkpoint operation.
        /// </summary>
        public FullCheckpoint(/* TODO: query engine URI? */)
            : base(QueryEngineOperationKind.FullCheckpoint, targetObjectUri: null, state: null)
        {
        }
    }
}
