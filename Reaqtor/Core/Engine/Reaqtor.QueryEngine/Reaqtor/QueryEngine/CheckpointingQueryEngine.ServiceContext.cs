// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Implementation of <see cref="IReactive"/> exposed by the engine.
        /// </summary>
        private sealed class ServiceContext(CheckpointingQueryEngine.CoreReactiveEngine engine) : ReactiveServiceContext(engine.ExpressionService, new ReactiveEngine(engine))
        {
        }
    }
}
