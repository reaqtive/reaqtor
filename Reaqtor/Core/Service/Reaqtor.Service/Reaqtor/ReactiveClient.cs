// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor
{
    /// <summary>
    /// Exposes reactive processing client-side operations using a provider to perform service-side operations.
    /// </summary>
    public class ReactiveClient : ReactiveClientBase
    {
        #region Constructor & fields

        private readonly ReactiveQueryProvider _provider;

        /// <summary>
        /// Creates a new reactive processing client using the specified client operations provider.
        /// </summary>
        /// <param name="provider">Client operations provider.</param>
        /// <param name="expressionServices">Expression services object, used to perform expression tree manipulations.</param>
        public ReactiveClient(IReactiveClientEngineProvider provider, IReactiveExpressionServices expressionServices)
            : base(new ReactiveQueryProvider(provider, expressionServices))
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (expressionServices == null)
                throw new ArgumentNullException(nameof(expressionServices));

            _provider = new ReactiveQueryProvider(provider, expressionServices);

            var thisParameter = ResourceNaming.GetThisReferenceExpression(this);
            expressionServices.RegisterObject(this, thisParameter);
            expressionServices.RegisterObject(_provider, Expression.Property(thisParameter, (PropertyInfo)ReflectionHelpers.InfoOf((IReactiveClient c) => c.Provider)));
        }

        #endregion
    }
}
