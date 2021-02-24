// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor;
using Reaqtor.QueryEngine;

namespace Tests.Reaqtor.QueryEngine
{
    public static class EngineTestExtensions
    {
        public static ReactiveServiceContext GetReactiveService(this ICheckpointingQueryEngine qe)
        {
            return new TupletizingContext(qe.ReactiveService);
        }
    }
}
