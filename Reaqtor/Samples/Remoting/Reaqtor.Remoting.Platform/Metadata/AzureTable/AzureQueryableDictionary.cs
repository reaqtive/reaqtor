// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Mutable queryable dictionary for reactive processing entity storage, backed by Azure table storage.
    /// </summary>
    /// <typeparam name="TMetadataInterface">Type of the metadata interface for the entities contained in the dictionary.</typeparam>
    /// <typeparam name="TMetadataEntity">Type of the strongly typed metadata entities contained in the dictionary.</typeparam>
    public class AzureQueryableDictionary<TMetadataInterface, TMetadataEntity> : QueryableDictionaryBase<Uri, TMetadataEntity>
        where TMetadataEntity : TableEntity, TMetadataInterface
        where TMetadataInterface : IKnownResource
    {
        /// <summary>
        /// The query provider to the Azure table storage.
        /// </summary>
        private readonly AzureMetadataQueryProvider _queryProvider;

        /// <summary>
        /// Instantiates the queryable dictionary for reactive processing entity storage, backed by Azure table storage.
        /// </summary>
        /// <param name="queryProvider">The query provider to the Auzre table storage.</param>
        /// <param name="underlyingDictionary">The underlying queryable dictionary for the metadata entities.</param>
        public AzureQueryableDictionary(AzureMetadataQueryProvider queryProvider, IQueryableDictionary<Uri, TMetadataInterface> underlyingDictionary)
        {
            Contract.RequiresNotNull(queryProvider);
            Contract.RequiresNotNull(underlyingDictionary);

            _queryProvider = queryProvider;
            Expression = CastExpression(underlyingDictionary.Expression);
        }

        /// <summary>
        /// Gets the expression representing the dictionary as a metadata source.
        /// </summary>
        public override Expression Expression { get; }

        /// <summary>
        /// Gets the query provider used to build queries that use the current dictionary as a metadata source.
        /// </summary>
        public override IQueryProvider Provider => _queryProvider;

        /// <summary>
        /// Adds a metadata entity to the Azure table.
        /// </summary>
        /// <param name="entity">The metadata entity to add to the Azure table.</param>
        /// <returns>A task to await the add operation.</returns>
        public Task AddAsync(TMetadataEntity entity)
        {
            Contract.RequiresNotNull(entity);

            return AddAsync(entity.Uri, entity);
        }

        /// <summary>
        /// Adds a metadata entity to the Azure table.
        /// </summary>
        /// <param name="uri">The URI of the metadata entity.</param>
        /// <param name="entity">The metadata entity to add to the Azure table.</param>
        /// <returns>A task to await the add operation.</returns>
        public Task AddAsync(Uri uri, TMetadataEntity entity)
        {
            Contract.RequiresNotNull(uri);
            Contract.RequiresNotNull(entity);

            var parameterExpression = Expression as ParameterExpression;
            var metadataCollectionUri = parameterExpression.Name;
            return _queryProvider.InsertAsync(uri, entity, metadataCollectionUri);
        }

        /// <summary>
        /// Removes the metadata entity with given URI from the Azure table.
        /// </summary>
        /// <param name="uri">The URI of the metadata entity to remove.</param>
        /// <returns>A task to await the return operation.</returns>
        public Task RemoveAsync(Uri uri)
        {
            Contract.RequiresNotNull(uri);

            var parameterExpression = Expression as ParameterExpression;
            var metadataCollectionUri = parameterExpression.Name;
            return _queryProvider.DeleteAsync(uri, metadataCollectionUri);
        }

        /// <summary>
        /// Gets an enumerator of all the metadata entities of the given type.
        /// </summary>
        /// <returns>The enumerator of all metadata entities of the given type.</returns>
        public override IEnumerator<KeyValuePair<Uri, TMetadataEntity>> GetEnumerator()
        {
            // TODO: Enable fetch-all.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Casts expressions containing reactive interface types to metadata entity types.
        /// </summary>
        /// <param name="expression">The expression to replace types in.</param>
        /// <returns>The expression with interface types replaced by metadata entity types.</returns>
        private static Expression CastExpression(Expression expression)
        {
            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(TMetadataInterface), typeof(TMetadataEntity) },
            });

            var result = subst.Apply(expression);

            return result;
        }
    }
}
