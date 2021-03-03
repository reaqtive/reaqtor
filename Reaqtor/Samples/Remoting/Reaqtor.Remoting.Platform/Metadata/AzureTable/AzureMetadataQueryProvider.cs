// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

using Reaqtor.Metadata;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Metadata query provider translating IReactiveMetadata-based queries onto Azure table storage queries for remote execution.
    /// </summary>
    public class AzureMetadataQueryProvider : IQueryProvider
    {
        /// <summary>
        /// The cloud table client to use.
        /// </summary>
        private readonly ITableClient _tableClient;

        /// <summary>
        /// The table address and partition key resolver.
        /// </summary>
        private readonly IStorageResolver _storageResolver;

        /// <summary>
        /// The request options to use for table operations.
        /// </summary>
        private readonly TableRequestOptions _requestOptions;

        /// <summary>
        /// Instantiates the query provider with an empty connection string, which signals to use the developer emulator.
        /// </summary>
        /// <param name="tableClient">The cloud table client.</param>
        /// <param name="storageResolver">The table address and partition key resolver.</param>
        /// <param name="requestOptions">Table request options, including retry policy.</param>
        public AzureMetadataQueryProvider(ITableClient tableClient, IStorageResolver storageResolver, TableRequestOptions requestOptions)
        {
            _tableClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
            _storageResolver = storageResolver ?? throw new ArgumentNullException(nameof(storageResolver));
            _requestOptions = requestOptions;
        }

        /// <summary>
        /// Creates a new metadata query representing by the specified expression.
        /// </summary>
        /// <param name="expression">Expression representing the metadata query.</param>
        /// <returns>Query object representing the specified metadata query.</returns>
        public IQueryable CreateQuery(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var queryableType = expression.Type.FindGenericType(typeof(IQueryable<>));

            Contract.RequiresNotNull(queryableType);

            var elementType = queryableType.GetGenericArguments()[0];

            var createQueryLambda = (Expression<Func<IQueryProvider, IQueryable<T>>>)(qp => qp.CreateQuery<T>(default));
            var createQueryDef = ((MethodInfo)ReflectionHelpers.InfoOf(createQueryLambda)).GetGenericMethodDefinition();
            var createQuery = createQueryDef.MakeGenericMethod(elementType);

            var result = (IQueryable)createQuery.Invoke(this, new[] { expression });

            return result;
        }

        /// <summary>
        /// Creates a new metadata query representing by the specified expression.
        /// </summary>
        /// <typeparam name="TElement">Type of the elements in the result of the metadata query.</typeparam>
        /// <param name="expression">Expression representing the metadata query.</param>
        /// <returns>Query object representing the specified metadata query.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return new MetadataQuery<TElement>(this, expression);
        }

        /// <summary>
        /// Executes the query represented by the specified expression.
        /// </summary>
        /// <param name="expression">Expression representing the metadata query.</param>
        /// <returns>Result of executing the query.</returns>
        public object Execute(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return Execute<object>(expression);
        }

        /// <summary>
        /// Executes the query represented by the specified expression.
        /// </summary>
        /// <typeparam name="TResult">Type of the query result.</typeparam>
        /// <param name="expression">Expression representing the metadata query.</param>
        /// <returns>Result of executing the query.</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            //Tracer.TraceSource.TraceInformation("EXECUTE {0}", expression);

            var azureQueryFriendly = new AzureTableQueryRewriter().Visit(expression);

            //Tracer.TraceSource.TraceInformation("USING {0}", azureQueryFriendly);

            var split = AzureTableQuerySplitter.Split(azureQueryFriendly);

            var result = ExecuteRemoteQuery(split.Remote);

            if (split.Local != null)
            {
                result = split.Local.Compile().DynamicInvoke(result);
            }

            return (TResult)result;
        }

#pragma warning disable CA1054 // URI-like parameters should not be strings. (Legacy approach.)

        /// <summary>
        /// Inserts the specified table entity using the specified URI as the key.
        /// </summary>
        /// <param name="uri">URI identifying the entity to insert.</param>
        /// <param name="entity">Entity to insert.</param>
        /// <param name="metadataCollectionUri">The metadata collection to add the entity to.</param>
        /// <returns>A task to await the insert operation.</returns>
        public async Task InsertAsync(Uri uri, TableEntity entity, string metadataCollectionUri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Contract.RequiresNotNullOrEmpty(metadataCollectionUri);

            var tableAddress = _storageResolver.ResolveTable(metadataCollectionUri);

            entity.PartitionKey = _storageResolver.ResolvePartition(uri.ToCanonicalString());

            //Tracer.TraceSource.TraceInformation("INSERT {0} INTO {1}", uri, tableAddress);

            var table = _tableClient.GetTableReference(tableAddress);
            await table.CreateIfNotExistsAsync(_requestOptions, null).ConfigureAwait(false);
            var insert = new InsertTableOperation(entity);
            await table.ExecuteAsync(insert, _requestOptions, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the specified table entity.
        /// </summary>
        /// <param name="uri">The URI of the table entity to remove.</param>
        /// <param name="metadataCollectionUri">The metadata collection to add the entity to.</param>
        /// <returns>A task to await the delete operation.</returns>
        public async Task DeleteAsync(Uri uri, string metadataCollectionUri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            Contract.RequiresNotNullOrEmpty(metadataCollectionUri);

            var tableAddress = _storageResolver.ResolveTable(metadataCollectionUri);

            //Tracer.TraceSource.TraceInformation("DELETE {0} FROM {1}", uri, tableAddress);

            var entity = new DeletionKnownTableEntity(uri)
            {
                PartitionKey = _storageResolver.ResolvePartition(uri.ToCanonicalString())
            };

            var table = _tableClient.GetTableReference(tableAddress);
            var delete = new DeleteTableOperation(entity);
            await table.ExecuteAsync(delete, _requestOptions, null).ConfigureAwait(false);
        }

#pragma warning restore CA1054

        /// <summary>
        /// Materializes the result set by lifting entities into KeyValuePair&lt;Uri, T&gt; representations.
        /// </summary>
        /// <typeparam name="T">Type of the entities in the result set.</typeparam>
        /// <param name="source">Result set to materialize elements for.</param>
        /// <returns>Materialized result set available for local querying.</returns>
        private static IQueryable<KeyValuePair<Uri, T>> Materialize<T>(IEnumerable<T> source)
            where T : IKnownResource
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Select(t => new KeyValuePair<Uri, T>(t.Uri, t)).AsQueryable();
        }

        /// <summary>
        /// Finds the metadata source parameter.
        /// </summary>
        /// <param name="expression">Expression to search the metadata source parameter in.</param>
        /// <returns>Metadata source parameter.</returns>
        private static ParameterExpression FindSourceParameter(Expression expression)
        {
            var freeVariables = FreeVariableScanner.Scan(expression);
            Contract.RequiresNotNull(freeVariables);

            var sources = freeVariables.ToArray();

            if (sources.Length == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No queryable source found in metadata query expression '{0}'.", expression));
            }

            if (sources.Length > 1)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unbound parameters detected in metadata query expression '{0}'. One or more of the following parameters could not be bound: {1}.", expression, string.Join(", ", sources.Select(p => p.ToString()))));
            }

            var source = sources[0];

            return source;
        }

        /// <summary>
        /// Executes the query expression against the Azure query provider for remote execution.
        /// </summary>
        /// <param name="queryExpression">Query expression to run against the Azure query provider.</param>
        /// <returns>Result set returned from executing the remote query.</returns>
        private object ExecuteRemoteQuery(Expression queryExpression)
        {
            // Phase 1: Convert the entity representation from the IRP metadata interface to the concrete class implementing the table entity.
            //          This is needed to rewrite URI-based lookup operations into PartitionKey/RowKey based lookups. Later, this can be extended
            //          to support queries that involve other property operations, such as comparing expression trees, by means of rewrites.
            var desiredResultType = queryExpression.Type.FindGenericType(typeof(IQueryable<>));
            Debug.Assert(desiredResultType != null);

            var desiredElementResultType = desiredResultType.GetGenericArguments()[0];
            if (!desiredElementResultType.IsGenericType || desiredElementResultType.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Query expression '{0}' has an unexpected result type. Expected no service-side projection.", queryExpression.ToCSharp()));
            }

            var desiredElementEntityResultType = desiredElementResultType.GetGenericArguments()[1];

            // TODO: Replace by a call to AzureQueryableDictionary<TMetadataInterface, TMetadataEntity>.CastExpression after wiring everything through.
            var substEntities = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(IAsyncReactiveSubscriptionProcess),           typeof(AsyncReactiveSubscriptionTableEntity)        },
                { typeof(IAsyncReactiveObservableDefinition),          typeof(AsyncReactiveObservableTableEntity)          },
                { typeof(IAsyncReactiveObserverDefinition),            typeof(AsyncReactiveObserverTableEntity)            },
                { typeof(IAsyncReactiveStreamFactoryDefinition),       typeof(AsyncReactiveStreamFactoryTableEntity)       },
                { typeof(IAsyncReactiveSubscriptionFactoryDefinition), typeof(AsyncReactiveSubscriptionFactoryTableEntity) },
                { typeof(IAsyncReactiveStreamProcess),                 typeof(AsyncReactiveStreamTableEntity)              },
                { typeof(IReactiveSubscriptionProcess),                typeof(ReactiveSubscriptionTableEntity)             },
                { typeof(IReactiveObservableDefinition),               typeof(ReactiveObservableTableEntity)               },
                { typeof(IReactiveObserverDefinition),                 typeof(ReactiveObserverTableEntity)                 },
                { typeof(IReactiveStreamFactoryDefinition),            typeof(ReactiveStreamFactoryTableEntity)            },
                { typeof(IReactiveSubscriptionFactoryDefinition),      typeof(ReactiveSubscriptionFactoryTableEntity)      },
                { typeof(IReactiveStreamProcess),                      typeof(ReactiveStreamTableEntity)                   },
            });
            var expression = substEntities.Apply(queryExpression);

            // Phase 2: Massage the query expression to unlift from KeyValuePair<Uri, T> to T, and to rewrite URI-based key lookups.
            var source = FindSourceParameter(expression);
            var tableAddress = _storageResolver.ResolveTable(source.Name);

            var queryableDictionaryType = source.Type.FindGenericType(typeof(IQueryableDictionary<,>));
            if (queryableDictionaryType == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Source parameter '{0}' in metadata query expression does not represent a queryable metadata source.", source));
            }

            var entityType = queryableDictionaryType.GetGenericArguments()[1];

            var unlifted = AzureTableQueryKeyValueUnlifter.Unlift(expression, entityType);
            var withKeyChecks = new AzureTableQueryKeyMatchRewriter(_storageResolver.ResolvePartition).Visit(unlifted);
            var query = withKeyChecks;

            //Tracer.TraceSource.TraceInformation("AZURE QUERY {0}", query);

            // Phase 3: Fetch query results.
            var queryableResultType = query.Type.FindGenericType(typeof(IQueryable<>));
            if (queryableResultType == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Metadata query expression '{0}' does not represent an enumerable result set.", expression));
            }

            var resultType = queryableResultType.GetGenericArguments()[0];

            var fetchQueryResultsDef = ((MethodInfo)ReflectionHelpers.InfoOf((AzureMetadataQueryProvider q) => q.FetchQueryResults<object, object>(default, default))).GetGenericMethodDefinition();
            var fetchQueryResults = fetchQueryResultsDef.MakeGenericMethod(entityType, resultType);

            var resultSet = fetchQueryResults.Invoke(this, new object[] { tableAddress, query });

            // Phase 4: Assert results a materialize the result set.
            var actualResultType = resultSet.GetType().FindGenericType(typeof(IQueryable<>));
            Debug.Assert(actualResultType != null);

            if (actualResultType != queryableResultType)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected result type '{0}' returned from Azure query. Expected type '{1}'.", actualResultType, resultType));
            }

            var actualElementResultType = actualResultType.GetGenericArguments()[0];
            if (!desiredElementEntityResultType.IsAssignableFrom(actualElementResultType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected element type '{0}' returned from Azure query. Expected type assignable to '{1}'.", actualElementResultType, desiredElementEntityResultType));
            }

            var materializeDef = ((MethodInfo)ReflectionHelpers.InfoOf(() => Materialize<IKnownResource>(default))).GetGenericMethodDefinition();
            var materialize = materializeDef.MakeGenericMethod(desiredElementEntityResultType);

            var result = materialize.Invoke(null, new object[] { resultSet });

            return result;
        }

#if UNUSED
        /// <summary>
        /// Resolves the Azure table name for the specified metadata collection.
        /// </summary>
        /// <param name="metadataCollectionUri">URI representing the metadata collection being queried.</param>
        /// <returns>Azure table name holding the metadata collection.</returns>
        private string Resolve(string metadataCollectionUri)
        {
            var tableAddress = _storageResolver.ResolveTable(metadataCollectionUri);

            return tableAddress;
        }
#endif

        /// <summary>
        /// Fetches the results of executing the specified query expression against the Azure query provider.
        /// This method employs an eager evaluation strategy.
        /// </summary>
        /// <typeparam name="TEntityType">Type of the entities in the entity set being queried.</typeparam>
        /// <typeparam name="TResultType">Type of the results returned from the query.</typeparam>
        /// <param name="entitySetName">Entity set being queried.</param>
        /// <param name="query">Expression representing the query over the entity set.</param>
        /// <returns>Result set containing the results of the query, supporting further local querying over the results.</returns>
        private IQueryable<TResultType> FetchQueryResults<TEntityType, TResultType>(string entitySetName, Expression query)
            where TEntityType : new()
        {
            var ctx = _tableClient.GetTableServiceContext();

            var entitySet = ctx.CreateQuery<TEntityType>(entitySetName);

            var source = FreeVariableScanner.Scan(query).Single();

            var boundQuery = new Inliner(source, entitySet.Expression).Visit(query);

            var result = entitySet.Provider.CreateQuery<TResultType>(boundQuery).ToList().AsQueryable();

            return result;
        }
    }
}
