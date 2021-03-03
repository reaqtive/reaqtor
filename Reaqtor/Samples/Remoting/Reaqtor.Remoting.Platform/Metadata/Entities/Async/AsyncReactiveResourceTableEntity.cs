// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq.Expressions;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Base class for table entities representing known resources that are represented by an expression tree.
    /// </summary>
    public abstract class AsyncReactiveResourceTableEntity : KnownTableEntity, IAsyncReactiveResource
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
        /// Creates a new table entity representing a known expressible resource with the specified URI and expression representation.
        /// </summary>
        /// <param name="uri">URI identifying the resource represented by the table entity.</param>
        /// <param name="expression">Expression representation of the resource.</param>
        protected AsyncReactiveResourceTableEntity(Uri uri, Expression expression)
            : base(uri)
        {
            _expression = expression;
        }

        /// <summary>
        /// Default constructor, required by the Azure Table query provider.
        /// </summary>
        protected AsyncReactiveResourceTableEntity()
        {
        }

        /// <summary>
        /// Gets the expression representation of the resource.
        /// </summary>
        public Expression Expression => _expression ??= new SerializationHelpers().Deserialize<Expression>(_bonsai);

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
    }
}
