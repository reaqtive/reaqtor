// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Exposes reactive processing metadata discovery operations using a provider to perform service-side operations.
    /// </summary>
    public class ReactiveMetadata : ReactiveMetadataBase
    {
        #region Constructor & fields

        private readonly IReactiveMetadataEngineProvider _provider;
        private readonly IReactiveExpressionServices _expressionServices;

        /// <summary>
        /// Creates a new reactive processing metadata object using the specified metadata operations provider.
        /// </summary>
        /// <param name="provider">Metadata operations provider.</param>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        public ReactiveMetadata(IReactiveMetadataEngineProvider provider, IReactiveExpressionServices expressionServices)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _expressionServices = expressionServices ?? throw new ArgumentNullException(nameof(expressionServices));

            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified expression.
        /// </summary>
        /// <typeparam name="TResult">Result type of the expression.</typeparam>
        /// <param name="expression">Expression to execute.</param>
        /// <returns>Result of executing the expression.</returns>
        protected override TResult Execute<TResult>(Expression expression)
        {
            var expr = _expressionServices.Normalize(expression);
            return _provider.Provider.Execute<TResult>(expr);
        }

        #endregion
    }
}
