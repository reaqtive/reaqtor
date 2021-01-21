// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table entities representing defined resources.
    /// </summary>
    public abstract class AsyncReactiveDefinedResourceTableEntity : AsyncReactiveResourceTableEntity, IAsyncReactiveDefinedResource
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        public AsyncReactiveDefinedResourceTableEntity()
        {
        }

        /// <summary>
        /// Creates a new table entity representing a defined resource with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the defined resource represented by the table entity.</param>
        /// <param name="expression">Expression representation of the defined resource.</param>
        /// <param name="state">The state.</param>
        public AsyncReactiveDefinedResourceTableEntity(Uri uri, Expression expression, object state)
            : base(uri, expression)
        {
            DefinitionTime = DateTime.Now;
            State = state;
        }

        // TODO: support serialization of state using the same tricks and using the DataSerializer.

        /// <summary>
        /// Gets the state that was passed during definition of the resource.
        /// </summary>
        /// <remarks>Implementers can provide statically typed accessors in derived types.</remarks>
        public object State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the date and time when the resource was defined.
        /// </summary>
        public DateTimeOffset DefinitionTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a flag indicating whether the definition is parameterized.
        /// </summary>
        /// <remarks>Type information of the parameter can be inferred through analysis of the expression tree.</remarks>
        public bool IsParameterized => Expression.NodeType == ExpressionType.Lambda;
    }
}
