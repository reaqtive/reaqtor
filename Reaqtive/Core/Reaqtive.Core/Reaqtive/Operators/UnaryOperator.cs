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
    /// Base class for query operator implementations.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameters passed to the observer.</typeparam>
    /// <typeparam name="TResult">Element type of the result sequence produced by the operator.</typeparam>
    public abstract class UnaryOperator<TParam, TResult> : IUnaryOperator, ISubscription
    {
        private IObserver<TResult> _observer;
        private int _started;

        /// <summary>
        /// Creates a new operator instance using the given parameters and the
        /// observer to report downstream notifications to.
        /// </summary>
        /// <param name="parent">Parameters used by the operator.</param>
        /// <param name="observer">Observer receiving the operator's output.</param>
        protected UnaryOperator(TParam parent, IObserver<TResult> observer)
        {
            Params = parent;
            _observer = observer;
        }

        /// <summary>
        /// Gets a list of upstream subscriptions established by the operator.
        /// </summary>
        public IEnumerable<ISubscription> Inputs
        {
            get
            {
                var inputs = Input as IEnumerable<ISubscription>;
                return inputs ?? AsEnumerable(Input);
            }
        }

        /// <summary>
        /// Gets the single input to the operator.
        /// </summary>
        public ISubscription Input { get; private set; } = NopSubscription.NopChild;

        /// <summary>
        /// Gets the parameters used by the operator. These parameters may include
        /// subscribable objects representing input sequences for the operator.
        /// </summary>
        protected TParam Params { get; private set; }

        /// <summary>
        /// The observer that is subscribed to the operator. Is set when the object
        /// is instantiated.
        /// </summary>
        protected IObserver<TResult> Output
        {
            get
            {
                var output = Volatile.Read(ref _observer);
                if (output == Sentinels<TResult>.Disposed)
                {
                    // Don't leak the sentinel!
                    output = NopObserver<TResult>.Instance;
                }

                return output;
            }
        }

        /// <summary>
        /// Returns whether the operator has been disposed.
        /// </summary>
        protected bool IsDisposed => Volatile.Read(ref _observer) == Sentinels<TResult>.Disposed || IsDisposedCore;

        /// <summary>
        /// Used internally to modify the IsDisposed flag, in case it should return
        /// true before a call to the Dispose method is made.
        /// </summary>
        internal virtual bool IsDisposedCore => false;

        /// <summary>
        /// Initializes <see cref="Input"/> by calling <see cref="OnSubscribe"/>.
        /// </summary>
        public virtual void Subscribe()
        {
            Input = OnSubscribe();
        }

        /// <summary>
        /// Sets the operator's context in which it operates. The context contains things
        /// like the reactive service, scheduler, and so on.
        /// </summary>
        /// <param name="context">Context in which the operator operates.</param>
        public virtual void SetContext(IOperatorContext context)
        {
        }

        /// <summary>
        /// Called after the state and/or context are completely initialized; generally
        /// subscribes operator to sources, etc.
        /// </summary>
        public void Start()
        {
            if (Interlocked.Exchange(ref _started, 1) != 0)
            {
                return;
            }

            if (IsDisposed)
            {
                Dispose();
                return;
            }

            OnStart();
        }

        /// <summary>
        /// Defines how the operator is disposed of.
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
                var old = Interlocked.Exchange(ref _observer, Sentinels<TResult>.Disposed);
                if (old != Sentinels<TResult>.Disposed)
                {
                    Input.Dispose();
                    OnDispose();
                }
            }
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
        /// Defines behavior immediately after operator is subscribed to. This
        /// method is generally responsible for subscribe'ing the operator to the
        /// `Params`, e.g., `Input = OnSubscribe();`. It is usually the case
        /// that this method is called as the last thing the constructor does.
        /// </summary>
        /// <returns>A child subscription.</returns>
        protected virtual ISubscription OnSubscribe()
        {
            return NopSubscription.NopChild;
        }

        /// <summary>
        /// Defines behavior immediately after operator is `Start`ed. This method is
        /// usually called at the very end of the `Start` method.
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Defines behavior immediately after we `Dispose` of the operator. This method is
        /// usually called at the very end of the `Dispose` method.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        private static IEnumerable<ISubscription> AsEnumerable(ISubscription value)
        {
            if (value != NopSubscription.NopChild)
            {
                yield return value;
            }
        }
    }
}
