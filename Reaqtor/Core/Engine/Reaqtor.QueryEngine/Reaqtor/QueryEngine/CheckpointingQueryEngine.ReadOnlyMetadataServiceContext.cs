// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        /// <summary>
        /// Implementation of metadata querying support for use within operators through <see cref="IOperatorContext"/>.
        /// </summary>
        private sealed class ReadOnlyMetadataServiceContext : ReactiveServiceContext
        {
            public ReadOnlyMetadataServiceContext(CoreReactiveEngine engine)
                : this(engine, new ReactiveEngine(engine))
            {
            }

            private ReadOnlyMetadataServiceContext(CoreReactiveEngine engine, ReactiveEngine e)
                : base(engine.ExpressionService, e, e, new ReadOnlyMetadataProvider(engine))
            {
            }

            private sealed class ReadOnlyMetadataProvider(CheckpointingQueryEngine.CoreReactiveEngine engine) : IReactiveMetadataEngineProvider
            {
                public IQueryProvider Provider { get; } = new OperatorContextRegistryQueryProvider(engine.Registry);
            }
        }
    }
}
