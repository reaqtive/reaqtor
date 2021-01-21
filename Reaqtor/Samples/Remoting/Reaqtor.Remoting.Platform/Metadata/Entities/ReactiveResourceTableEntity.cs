// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table entities representing known resources that are represented by an expression tree.
    /// </summary>
    public abstract class ReactiveResourceTableEntity : KnownTableEntity, IReactiveResource
    {
        /// <summary>
        /// Serialized form of the expression representing the resource.
        /// </summary>
        private volatile string _bonsai;

        /// <summary>
        /// Expression representation of the resource.
        /// </summary>
        private volatile Expression _expression;

        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        protected ReactiveResourceTableEntity()
        {
        }

        /// <summary>
        /// Gets the expression representation of the resource.
        /// </summary>
        public Expression Expression
        {
            get
            {
                if (_expression == null)
                {
                    var rw = new AsyncToSyncRewriter(new Dictionary<Type, Type>
                    {
                        { typeof(IReactiveClientProxy), typeof(IReactiveClient) },
                        { typeof(IReactiveDefinitionProxy), typeof(IReactiveDefinition) },
                        { typeof(IReactiveMetadataProxy), typeof(IReactiveMetadata) },
                        { typeof(IReactiveProxy), typeof(IReactive) },
                    });

                    var asyncExpression = new SerializationHelpers().Deserialize<Expression>(_bonsai);

                    _expression = rw.Rewrite(asyncExpression);
                }

                return _expression;
            }
        }

        /// <summary>
        /// (Infrastructure) Gets or sets the serialized form of the expression.
        /// This property is used by the framework infrastructure and is not intended to be used directly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Bonsai
        {
            get => _bonsai ??= new SerializationHelpers().Serialize(_expression);

            set
            {
                if (_bonsai != null && _bonsai != value)
                {
                    throw new InvalidOperationException("Bonsai property already assigned.");
                }

                _bonsai = value;
            }
        }

        /// <summary>
        /// Disposes the resource.
        /// </summary>
        public void Dispose()
        {
            // CONSIDER: Revisit this limitaton; we don't have access to access the parent context here.
            throw new NotSupportedException("The Azure metadata provider doesn't support operations on metadata entities.");
        }
    }
}
