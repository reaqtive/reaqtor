// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq.Expressions;
using System.Memory;
using System.Runtime.CompilerServices;
using System.Threading;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Base class for entities in QueryEngineRegistry.
    /// </summary>
    internal abstract class ReactiveEntity : IReactiveResource
    {
        private volatile object _expression;
        private volatile bool _isPersisted;

        private int _transactionLogState;

        protected ReactiveEntity(Uri uri, Expression expression, object state)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            State = state;
            _expression = expression;
        }

        /// <summary>
        /// Gets the entity kind.
        /// </summary>
        public abstract ReactiveEntityKind Kind { get; }

        /// <summary>
        /// The URI of the entity.
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// The expression representation of the entity.
        /// </summary>
        public Expression Expression => _expression is IDiscardable<Expression> cached ? cached.Value : (Expression)_expression;

        /// <summary>
        /// Additional state associated with the entity. Must be an object
        /// conforming to the data model (JSON serializable).
        /// </summary>
        public object State { get; }

        /// <summary>
        /// Gets a value indicating whether the entity has been initialized already.
        /// </summary>
        public virtual bool IsInitialized => true;

        /// <summary>
        /// Gets a value indicating whether the state has changed and the object should be persisted.
        /// </summary>
        public bool IsPersisted => _isPersisted;

        /// <summary>
        /// Called when the object has been saved.
        /// </summary>
        public void OnPersisted() => _isPersisted = true;

        /// <summary>
        /// The state diagram for the transaction log state is:
        /// None --> Active &lt;-> Deleting --> Deleted
        /// </summary>
        internal TransactionState TransactionLogState => (TransactionState)Volatile.Read(ref _transactionLogState);

        internal void AdvanceState(TransactionState nextState)
        {
            switch (nextState)
            {
                case TransactionState.None:
                    throw CreateTransitionException((TransactionState)_transactionLogState, TransactionState.None);
                case TransactionState.Active:
                    {
                        var orig = (TransactionState)Interlocked.CompareExchange(ref _transactionLogState, (int)TransactionState.Active, (int)TransactionState.None);
                        if (orig != TransactionState.None)
                            throw CreateTransitionException(orig, TransactionState.Active);
                        return;
                    }
                case TransactionState.Deleting:
                    {
                        var orig = (TransactionState)Interlocked.CompareExchange(ref _transactionLogState, (int)TransactionState.Deleting, (int)TransactionState.Active);
                        if (orig != TransactionState.Active)
                            throw CreateTransitionException(orig, TransactionState.Deleting);
                        return;
                    }
                case TransactionState.Deleted:
                    {
                        var orig = (TransactionState)Interlocked.CompareExchange(ref _transactionLogState, (int)TransactionState.Deleted, (int)TransactionState.Deleting);
                        if (orig != TransactionState.Deleting)
                            throw CreateTransitionException(orig, TransactionState.Deleted);
                        return;
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        internal void RollbackState(TransactionState nextState)
        {
            switch (nextState)
            {
                case TransactionState.Active:
                    var orig = (TransactionState)Interlocked.CompareExchange(ref _transactionLogState, (int)TransactionState.Active, (int)TransactionState.Deleting);
                    if (orig != TransactionState.Deleting)
                        throw CreateTransitionException(orig, TransactionState.Active);
                    return;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Exception CreateTransitionException(TransactionState current, TransactionState requested, [CallerMemberName] string method = "")
        {
            return new InvalidOperationException("Cannot " + method + " from " + current.ToString() + " to " + requested.ToString() + ".");
        }

        /// <summary>
        /// Updates the expression of the entity.
        /// </summary>
        /// <param name="expression">
        /// The expression to update the entity with.
        /// </param>
        public void Update(Expression expression)
        {
            _expression = expression;
            _isPersisted = false;
        }

        /// <summary>
        /// Caches the entity expression.
        /// </summary>
        /// <param name="cache">The expression cache.</param>
        public void Cache(ICache<Expression> cache)
        {
            var expression = (Expression)_expression;
            _expression = cache.Create(expression);
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="stream">The stream.</param>
        public virtual void Serialize(ISerializer serializer, Stream stream)
        {
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="stream">The stream.</param>
        public virtual void Deserialize(ISerializer serializer, Stream stream)
        {
        }

        /// <summary>
        /// Creates an invalid instance of the given reactive entity kind with the given URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="kind">The reactive entity kind.</param>
        /// <returns>
        /// The invalid instance, or null if an unexpected kind is provided.
        /// </returns>
        public static ReactiveEntity CreateInvalidInstance(Uri uri, ReactiveEntityKind kind)
        {
            return kind switch
            {
                ReactiveEntityKind.Observable => ObservableDefinitionEntity.CreateInvalidInstance(uri),
                ReactiveEntityKind.Observer => ObserverDefinitionEntity.CreateInvalidInstance(uri),
                ReactiveEntityKind.Stream => SubjectEntity.CreateInvalidInstance(uri),
                ReactiveEntityKind.StreamFactory => StreamFactoryDefinitionEntity.CreateInvalidInstance(uri),
                ReactiveEntityKind.Subscription => SubscriptionEntity.CreateInvalidInstance(uri),
                ReactiveEntityKind.ReliableSubscription => ReliableSubscriptionEntity.CreateInvalidInstance(uri),
                ReactiveEntityKind.Template or ReactiveEntityKind.Other => OtherDefinitionEntity.CreateInvalidInstance(uri),
                _ => null,
            };
        }
    }
}
