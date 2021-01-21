// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reactive;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private sealed class ExecutionEnvironment : HigherOrderExecutionEnvironment, IReliableExecutionEnvironment
        {
            private readonly QueryEngineRegistry _registry;

            public ExecutionEnvironment(QueryEngineRegistry registry, ReadOnlyMetadataServiceContext context)
                : base(registry, context)
            {
                _registry = registry;
            }

            public IReliableMultiSubject<TInput, TOutput> GetReliableSubject<TInput, TOutput>(Uri uri) => _registry.GetReliableSubject<TInput, TOutput>(uri);
        }
    }
}
