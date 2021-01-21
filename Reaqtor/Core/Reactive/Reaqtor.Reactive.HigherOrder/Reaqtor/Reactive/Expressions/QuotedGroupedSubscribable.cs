// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Expressions;

namespace Reaqtor.Reactive.Expressions
{
    /// <summary>
    /// Quoted representation of a grouped subscribable sequence.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TSource">Type of the elements produced by the subscribable source.</typeparam>
    public class QuotedGroupedSubscribable<TKey, TSource> : Quoted<IGroupedSubscribable<TKey, TSource>>, IGroupedQubscribable<TKey, TSource>
    {
        /// <summary>
        /// Creates a new quoted representation of a grouped subscribable sequence.
        /// </summary>
        /// <param name="expression">Expression representing the grouped subscribable sequence.</param>
        public QuotedGroupedSubscribable(Expression expression)
            : base(expression)
        {
            //
            // WARNING: This constructor gets called during deserialization of persisted subscriptions.
            //
        }

        /// <summary>
        /// Creates a new quoted representation of a grouped subscribable sequence.
        /// </summary>
        /// <param name="value">Subscribable sequence to create a quote for.</param>
        /// <param name="expression">Expression representing the grouped subscribable sequence.</param>
        public QuotedGroupedSubscribable(IGroupedSubscribable<TKey, TSource> value, Expression expression)
            : base(value, expression)
        {
        }

        /// <summary>
        /// Creates a new quoted representation of a grouped subscribable sequence.
        /// </summary>
        /// <param name="expression">Expression representing the grouped subscribable sequence.</param>
        /// <param name="policy">Policy used to evaluate the expression.</param>
        public QuotedGroupedSubscribable(Expression expression, IExpressionEvaluationPolicy policy)
            : base(expression, policy)
        {
            //
            // WARNING: This constructor gets called during deserialization of persisted subscriptions.
            //
        }

        /// <summary>
        /// Creates a new quoted representation of a grouped subscribable sequence.
        /// </summary>
        /// <param name="value">Grouped subscribable sequence to create a quote for.</param>
        /// <param name="expression">Expression representing the subscribable sequence.</param>
        /// <param name="policy">Policy used to evaluate the expression.</param>
        public QuotedGroupedSubscribable(IGroupedSubscribable<TKey, TSource> value, Expression expression, IExpressionEvaluationPolicy policy)
            : base(value, expression, policy)
        {
            //
            // WARNING: This constructor gets called during quote instantiation.
            //
        }

        /// <summary>
        /// Gets the grouping key.
        /// </summary>
        public TKey Key => Value.Key;

        /// <summary>
        /// Subscribes the given observer to the grouped subscribable sequence.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the sequence.</param>
        /// <returns>Object used to visit the newly created subscription.</returns>
        public ISubscription Subscribe(IObserver<TSource> observer) => Value.Subscribe(observer);

        /// <summary>
        /// Subscribes the given observer to the grouped observable sequence.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the sequence.</param>
        /// <returns>Disposable used to cancel the subscription.</returns>
        IDisposable IObservable<TSource>.Subscribe(IObserver<TSource> observer) => ((IObservable<TSource>)Value).Subscribe(observer);
    }
}
