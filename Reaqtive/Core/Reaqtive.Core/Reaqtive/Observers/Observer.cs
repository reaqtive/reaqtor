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
    /// Provides a set of methods to create observers.
    /// </summary>
    public static class Observer
    {
        /// <summary>
        /// Creates an observer using the specified delegates to implement its operations.
        /// </summary>
        /// <typeparam name="T">Type of the elements received by the observer.</typeparam>
        /// <param name="onNext">Delegate implementing the observer's OnNext method.</param>
        /// <param name="onError">Delegate implementing the observer's OnError method.</param>
        /// <param name="onCompleted">Delegate implementing the observer's OnCompleted method.</param>
        /// <returns>An observer whose implementation invokes the specified delegates.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new SimpleObserver<T>(onNext, onError, onCompleted);
        }

        /// <summary>
        /// Creates an observer using the specified delegates to implement its operations.
        /// </summary>
        /// <typeparam name="T">Type of the elements received by the observer.</typeparam>
        /// <param name="onNext">Delegate implementing the observer's OnNext method.</param>
        /// <returns>An observer whose implementation invokes the specified delegates.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext)
        {
            return new SimpleObserver<T>(onNext, ex => throw ex, () => { });
        }

        /// <summary>
        /// Returns an instance of an observer that ignores all of the notifications it receives.
        /// </summary>
        /// <typeparam name="T">Type of the elements received by the observer.</typeparam>
        /// <returns>An observer that ignores all of the notifications it receives.</returns>
        public static IObserver<T> Nop<T>()
        {
            return NopObserver<T>.Instance;
        }
    }

    /// <summary>
    /// Base class for observers that can be visited as part of a subscription tree.
    /// </summary>
    /// <typeparam name="T">Type of the elements received by the observer.</typeparam>
    public abstract class Observer<T> : ObserverBase<T>, IOperator, ISubscription
    {
        private long _disposed = 0;

        /// <summary>
        /// Creates a new observer instance.
        /// </summary>
        protected Observer()
        {
        }

        /// <summary>
        /// Gets the inputs of the observer.
        /// </summary>
        public IEnumerable<ISubscription> Inputs
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a flag indicating whether observer is disposed.
        /// </summary>
        protected bool IsDisposed => Interlocked.Read(ref _disposed) != 0 || IsDisposedCore;

        /// <summary>
        /// Gets a flag indicating whether observer is disposed in accordance with the behavior of a derived class.
        /// </summary>
        internal virtual bool IsDisposedCore => false;

        /// <summary>
        /// Initializes <see cref="Inputs"/> by calling <see cref="OnSubscribe"/>.
        /// </summary>
        public virtual void Subscribe()
        {
            Inputs = OnSubscribe();
        }

        /// <summary>
        /// Sets the operator context on the observer.
        /// </summary>
        /// <param name="context">Operator context to set on the observer.</param>
        public virtual void SetContext(IOperatorContext context)
        {
        }

        /// <summary>
        /// Accepts a visitor that will be dispatched through the subscription.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(ISubscriptionVisitor visitor)
        {
            Debug.Assert(visitor != null);

            visitor.Visit(this);
        }

        /// <summary>
        /// Defines behavior immediately after the observer is used in a subscription.
        /// </summary>
        /// <returns>Enumerable containing the subscriptions.</returns>
        protected virtual IEnumerable<ISubscription> OnSubscribe()
        {
            return Array.Empty<ISubscription>();
        }

        /// <summary>
        /// Starts the observer.
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
        /// Disposes the current observer instance.
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
        /// Performs custom operations upon starting the observer.
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Performs custom operations upon disposing the observer.
        /// </summary>
        protected virtual void OnDispose()
        {
        }
    }
}
