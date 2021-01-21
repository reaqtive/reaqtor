// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to perform a recovery.
    /// </summary>
    [Serializable]
    public class Recovery : QueryEngineOperation
    {
        /// <summary>
        /// Creates a recovery operation.
        /// </summary>
        public Recovery(/* TODO: query engine URI? */)
            : base(QueryEngineOperationKind.Recovery, targetObjectUri: null, state: null)
        {
        }
    }
}
