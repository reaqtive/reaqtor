// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor
{
    using Metadata;

    /// <summary>
    /// Base class for reactive processing metadata discovery operations.
    /// </summary>
    public abstract class ReactiveMetadataProxyBase : IReactiveMetadataProxy
    {
        #region Constructor & fields

        private readonly QueryProvider _provider;

        /// <summary>
        /// Creates a new instance of a reactive processing metadata discovery object.
        /// </summary>
        protected ReactiveMetadataProxyBase()
        {
            _provider = new QueryProvider(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a queryable dictionary of stream factory definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveStreamFactoryDefinition> StreamFactories => _provider.CreateSource<IAsyncReactiveStreamFactoryDefinition>(Constants.MetadataStreamFactoriesUri);

        /// <summary>
        /// Gets a queryable dictionary of stream objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveStreamProcess> Streams => _provider.CreateSource<IAsyncReactiveStreamProcess>(Constants.MetadataStreamsUri);

        /// <summary>
        /// Gets a queryable dictionary of observable definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveObservableDefinition> Observables => _provider.CreateSource<IAsyncReactiveObservableDefinition>(Constants.MetadataObservablesUri);

        /// <summary>
        /// Gets a queryable dictionary of observer definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveObserverDefinition> Observers => _provider.CreateSource<IAsyncReactiveObserverDefinition>(Constants.MetadataObserversUri);

        /// <summary>
        /// Gets a queryable dictionary of subscription factory definition objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveSubscriptionFactoryDefinition> SubscriptionFactories => _provider.CreateSource<IAsyncReactiveSubscriptionFactoryDefinition>(Constants.MetadataSubscriptionFactoriesUri);

        /// <summary>
        /// Gets a queryable dictionary of subscription objects.
        /// </summary>
        public IQueryableDictionary<Uri, IAsyncReactiveSubscriptionProcess> Subscriptions => _provider.CreateSource<IAsyncReactiveSubscriptionProcess>(Constants.MetadataSubscriptionsUri);

        #endregion

        #region Methods

        /// <summary>
        /// Executes the specified expression.
        /// </summary>
        /// <typeparam name="TResult">Result type of the expression.</typeparam>
        /// <param name="expression">Expression to execute.</param>
        /// <returns>Result of executing the expression.</returns>
        protected abstract TResult Execute<TResult>(Expression expression);

        #endregion

        #region Private implementation

        private sealed class QueryableDictionary<T> : QueryableDictionaryBase<Uri, T>
        {
            public QueryableDictionary(IQueryProvider provider, Expression expression)
            {
                Provider = provider;
                Expression = expression;
            }

            public override IEnumerator<KeyValuePair<Uri, T>> GetEnumerator() => Provider.Execute<IEnumerable<KeyValuePair<Uri, T>>>(Expression).GetEnumerator();

            public override Expression Expression { get; }

            public override IQueryProvider Provider { get; }
        }

        private sealed class Queryable<T> : IQueryable<T>
        {
            public Queryable(IQueryProvider provider, Expression expression)
            {
                Provider = provider;
                Expression = expression;
            }

            public IEnumerator<T> GetEnumerator() => Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public Type ElementType => typeof(T);

            public Expression Expression { get; }

            public IQueryProvider Provider { get; }
        }

        private sealed class QueryProvider : IQueryProvider
        {
            private readonly ReactiveMetadataProxyBase _parent;

            public QueryProvider(ReactiveMetadataProxyBase parent) => _parent = parent;

            public IQueryableDictionary<Uri, T> CreateSource<T>(string name)
            {
                var expression = Expression.Parameter(typeof(IQueryableDictionary<Uri, T>), name);
                return new QueryableDictionary<T>(this, expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                if (expression == null)
                    throw new ArgumentNullException(nameof(expression));

                return new Queryable<TElement>(this, expression);
            }

            public IQueryable CreateQuery(Expression expression)
            {
                if (expression == null)
                    throw new ArgumentNullException(nameof(expression));

                var type = expression.Type.FindGenericType(typeof(IQueryable<>));
                if (type == null)
                    throw new InvalidOperationException("Specified expression is not compatible with IQueryable<T>.");

                var elementTypeArgs = type.GetGenericArguments();

                var queryableType = typeof(Queryable<>).MakeGenericType(elementTypeArgs);

                return (IQueryable)Activator.CreateInstance(queryableType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, binder: null, new object[] { this, expression }, culture: null);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                if (expression == null)
                    throw new ArgumentNullException(nameof(expression));

                if (!typeof(TResult).IsAssignableFrom(expression.Type))
                    throw new InvalidOperationException("Specified expression is not assignable to " + typeof(TResult) + ".");

                var rewritten = CollectionInliner.Instance.Visit(expression);
                return _parent.Execute<TResult>(rewritten);
            }

            public object Execute(Expression expression)
            {
                if (expression == null)
                    throw new ArgumentNullException(nameof(expression));

                var genericExecuteMethod = ((MethodInfo)ReflectionHelpers.InfoOf((ReactiveMetadataProxyBase rmpb) => rmpb.Execute<object>(null))).GetGenericMethodDefinition();
                var executeMethod = genericExecuteMethod.MakeGenericMethod(expression.Type);
                return executeMethod.Invoke(_parent, new object[] { expression });
            }

            private sealed class CollectionInliner : ExpressionVisitor
            {
                public static readonly CollectionInliner Instance = new();

                protected override Expression VisitMember(MemberExpression node)
                {
                    var result = base.VisitMember(node);

                    if (result is MemberExpression asMember)
                    {
                        // Assuming only metadata collection properties exist on the `IReactiveMetadataProxy` interface.
                        if (asMember.Member is PropertyInfo property && IsInterfaceProperty(typeof(IReactiveMetadataProxy), property))
                        {
                            var fvs = FreeVariableScanner.HasFreeVariables(asMember);
                            if (!fvs)
                            {
                                // For now, all properties on `IReactiveMetadataProxy` are `IQueryable`.
                                // If this is no longer the case, this call should be revisited.
                                return asMember.Evaluate<IQueryable>().Expression;
                            }
                        }
                    }

                    return result;
                }

                private static bool IsInterfaceProperty(Type interfaceType, PropertyInfo property)
                {
                    var declaringType = property.DeclaringType;
                    if (declaringType == interfaceType)
                    {
                        return true;
                    }
                    else if (interfaceType.IsAssignableFrom(declaringType))
                    {
                        var accessors = property.GetAccessors();
                        var interfaceMap = declaringType.GetInterfaceMap(interfaceType);
                        var targetMethods = interfaceMap.TargetMethods;
                        return accessors.Intersect(targetMethods).Count() == accessors.Length;
                    }

                    return false;
                }
            }
        }

        #endregion
    }
}
