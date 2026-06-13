// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Representation of a metadata query.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the result of the query.</typeparam>
    internal class MetadataQuery<T> : IQueryable<T>
    {
        /// <summary>
        /// Metadata query provider used to compose queries and to execute the query against Azure table storage.
        /// </summary>
        private readonly AzureMetadataQueryProvider _provider;

        /// <summary>
        /// Creates a new metadata query using the specified provider and expression tree representation.
        /// </summary>
        /// <param name="provider">Metadata query provider used to compose queries and to execute the query against Azure table storage.</param>
        /// <param name="expression">Expression representing the metadata query.</param>
        public MetadataQuery(AzureMetadataQueryProvider provider, Expression expression)
        {
            _provider = provider;
            Expression = expression;
        }

        /// <summary>
        /// Gets the element type of the results of the query.
        /// </summary>
        public Type ElementType => typeof(T);

        /// <summary>
        /// Gets the expression representing the metadata query.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Gets the metadata query provider used to compose queries and to execute the query against Azure table storage.
        /// </summary>
        public IQueryProvider Provider => _provider;

        /// <summary>
        /// Gets an enumerator to enumerate over the results of the query.
        /// </summary>
        /// <returns>Enumerator to enumerate over the results of the query.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var result = _provider.Execute<IEnumerable<T>>(Expression);

            foreach (var element in result)
            {
                yield return element;
            }
        }

        /// <summary>
        /// Gets an enumerator to enumerate over the results of the query.
        /// </summary>
        /// <returns>Enumerator to enumerate over the results of the query.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
