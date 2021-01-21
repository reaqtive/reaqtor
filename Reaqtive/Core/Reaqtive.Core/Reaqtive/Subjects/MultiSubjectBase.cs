// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Reaqtive
{
    /// <summary>
    /// Base implementation of the non-generic multi-subject.
    /// </summary>
    public abstract class MultiSubjectBase : IMultiSubject, IOperator, ISubscription
    {
        private long _disposed = 0;

        /// <summary>
        /// Gets an observer to push values into the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>An observer.</returns>
        public IObserver<T> GetObserver<T>()
        {
            return GetObserverCore<T>();
        }

        /// <summary>
        /// Gets an observer to push values into the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>An observer.</returns>
        protected abstract IObserver<T> GetObserverCore<T>();

        /// <summary>
        /// Gets a subscribable to subscribe to the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>A subscribable.</returns>
        public ISubscribable<T> GetObservable<T>()
        {
            return GetObservableCore<T>();
        }

        /// <summary>
        /// Gets a subscribable to subscribe to the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>A subscribable.</returns>
        protected abstract ISubscribable<T> GetObservableCore<T>();

        /// <summary>
        /// A set of subscriptions to visit upon visiting the current instance.
        /// </summary>
        public virtual IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

        /// <summary>
        /// Reports whether subject is disposed.
        /// </summary>
        protected bool IsDisposed => Interlocked.Read(ref _disposed) != 0 || IsDisposedCore;

        /// <summary>
        /// Used internally to modify the IsDisposed flag, in case it should return
        /// true before a call to the Dispose method is made.
        /// </summary>
        internal virtual bool IsDisposedCore => false;

        /// <summary>
        /// Initializes <see cref="Inputs"/>.
        /// </summary>
        public virtual void Subscribe()
        {
        }

        /// <summary>
        /// Sets the operator's current context. Contains things like the reactive service,
        /// scheduler, and so on.
        /// </summary>
        /// <param name="context">Context data being passed in.</param>
        public virtual void SetContext(IOperatorContext context)
        {
        }

        /// <summary>
        /// Accepts a visitor. Useful, e.g., when we want to perform an action on all
        /// the subscriptions in the graph (e.g., call `OnSubscribe`). The visitor
        /// will traverse the graph and perform actions on subscriptions. `Accept`
        /// will accept such actions to be performed.
        /// </summary>
        /// <param name="visitor">Visitor performing the action.</param>
        public void Accept(ISubscriptionVisitor visitor)
        {
            Debug.Assert(visitor != null);

            visitor.Visit(this);
        }

        /// <summary>
        /// Called after the state and/or context are completely initialized; generally
        /// subscribes operator to sources, etc.
        /// </summary>
        public void Start()
        {
            if (IsDisposed)
            {
                Dispose();
                return;
            }

            OnStart();
        }

        /// <summary>
        /// Defines how subject is disposed of.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
                {
                    return;
                }

                OnDispose();
            }
        }

        /// <summary>
        /// Defines behavior immediately after subject is `Start`ed. This method is
        /// usually called at the very end of the `Start` method.
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Defines behavior immediately after we `Dispose` of the subject. This method is
        /// usually called at the very end of the `Dispose` method.
        /// </summary>
        protected virtual void OnDispose()
        {
        }
    }
}
