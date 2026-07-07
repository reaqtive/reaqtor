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
    public abstract class ReactiveDefinedResourceTableEntity : ReactiveResourceTableEntity, IReactiveDefinedResource
    {
        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        protected ReactiveDefinedResourceTableEntity()
        {
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

        /// <summary>
        /// Undefines the resource.
        /// </summary>
        public void Undefine()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on metadata entities.");
        }
    }
}
