// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a defninition (cold) entity in a QueryEngineRegistry.
    /// The definition entities are stream factories, observables and observers.
    /// </summary>
    internal abstract class DefinitionEntity : ReactiveEntity, IReactiveDefinedResource
    {
        protected DefinitionEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        public bool IsParameterized => throw new NotImplementedException();

        public DateTimeOffset DefinitionTime => throw new NotImplementedException();
    }
}
