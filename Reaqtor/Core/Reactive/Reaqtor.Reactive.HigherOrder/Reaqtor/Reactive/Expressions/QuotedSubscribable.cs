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
    /// Quoted representation of a subscribable sequence.
    /// </summary>
    /// <typeparam name="T">Element type of the sequence.</typeparam>
    public class QuotedSubscribable<T> : Quoted<ISubscribable<T>>, IQubscribable<T>
    {
        /// <summary>
        /// Creates a new quoted representation of a subscribable sequence.
        /// </summary>
        /// <param name="expression">Expression representing the subscribable sequence.</param>
        public QuotedSubscribable(Expression expression)
            : base(expression)
        {
            //
            // WARNING: This constructor gets called during deserialization of persisted subscriptions.
            //
        }

        /// <summary>
        /// Creates a new quoted representation of a subscribable sequence.
        /// </summary>
        /// <param name="value">Subscribable sequence to create a quote for.</param>
        /// <param name="expression">Expression representing the subscribable sequence.</param>
        public QuotedSubscribable(ISubscribable<T> value, Expression expression)
            : base(value, expression)
        {
        }

        /// <summary>
        /// Creates a new quoted representation of a subscribable sequence.
        /// </summary>
        /// <param name="expression">Expression representing the subscribable sequence.</param>
        /// <param name="policy">Policy used to evaluate the expression.</param>
        public QuotedSubscribable(Expression expression, IExpressionEvaluationPolicy policy)
            : base(expression, policy)
        {
            //
            // WARNING: This constructor gets called during deserialization of persisted subscriptions.
            //
        }

        /// <summary>
        /// Creates a new quoted representation of a subscribable sequence.
        /// </summary>
        /// <param name="value">Subscribable sequence to create a quote for.</param>
        /// <param name="expression">Expression representing the subscribable sequence.</param>
        /// <param name="policy">Policy used to evaluate the expression.</param>
        public QuotedSubscribable(ISubscribable<T> value, Expression expression, IExpressionEvaluationPolicy policy)
            : base(value, expression, policy)
        {
            //
            // WARNING: This constructor gets called during quote instantiation.
            //
        }

        /// <summary>
        /// Subscribes the given observer to the subscribable sequence.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the sequence.</param>
        /// <returns>Object used to visit the newly created subscription.</returns>
        public ISubscription Subscribe(IObserver<T> observer) => Value.Subscribe(observer);

        /// <summary>
        /// Subscribes the given observer to the observable sequence.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the sequence.</param>
        /// <returns>Disposable used to cancel the subscription.</returns>
        IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => ((IObservable<T>)Value).Subscribe(observer);
    }
}
