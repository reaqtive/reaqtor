// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Base class for higher order query operator implementations. Operators are higher order when they
    /// receive inner subscribable sequences.
    /// </summary>
    /// <typeparam name="TParam">Type of the parameters passed to the observer.</typeparam>
    /// <typeparam name="TResult">Element type of the result sequence produced by the operator.</typeparam>
    public abstract class HigherOrderInputStatefulOperator<TParam, TResult> : StatefulOperator<TParam, TResult>
    {
        private IOperatorContext _context;

        /// <summary>
        /// Creates a new higher order operator instance using the given parameters and the
        /// observer to report downstream notifications to.
        /// </summary>
        /// <param name="parent">Parameters used by the operator.</param>
        /// <param name="observer">Observer receiving the operator's output.</param>
        protected HigherOrderInputStatefulOperator(TParam parent, IObserver<TResult> observer)
            : base(parent, observer)
        {
        }

        /// <summary>
        /// Sets the operator's context in which it operates.
        /// </summary>
        /// <param name="context">Context in which the operator operates.</param>
        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            _context = context;
        }

        /// <summary>
        /// Tries to get the underlying higher-order environment, if supported by the hosting infrastructure.
        /// </summary>
        /// <param name="env">The higher-order environment.</param>
        /// <returns>true if supported; otherwise, false.</returns>
        private bool TryGetHigherOrderExecutionEnvironment(out IHigherOrderExecutionEnvironment env)
        {
            env = _context?.ExecutionEnvironment as IHigherOrderExecutionEnvironment;
            return env != null;
        }

        /// <summary>
        /// Gets the underlying higher-order environment, if supported by the hosting infrastructure.
        /// </summary>
        private IHigherOrderExecutionEnvironment HigherOrderExecutionEnvironment
        {
            get
            {
                if (!TryGetHigherOrderExecutionEnvironment(out var env))
                {
                    throw new NotSupportedException("The operation is not supported without the execution environment providing support for higher-order operators.");
                }

                return env;
            }
        }

        /// <summary>
        /// Subscribes to an inner sequence using the given observer. The inner sequence should have
        /// an expression representation in order for the subscription to be checkpointable. If the
        /// observer has state that needs to be checkpointed, it should implement load and save
        /// functionality using the IStatefulOperator interface. The returned subscription object is
        /// cold and needs to be activated by the caller.
        /// </summary>
        /// <typeparam name="TInner">Element type of the inner sequence.</typeparam>
        /// <param name="inner">Inner sequence to subscribe to.</param>
        /// <param name="observer">Observer to subscribe to the inner sequence.</param>
        /// <returns>
        /// Subscription to the inner sequence. The caller is responsible to activate the subscription.
        /// In order to save and load the subscription for use in checkpointing, the LoadInner and
        /// SaveInner methods should be used.
        /// </returns>
        protected ISubscription SubscribeInner<TInner>(ISubscribable<TInner> inner, IObserver<TInner> observer)
        {
            if (TryGetHigherOrderExecutionEnvironment(out var env))
            {
                return env.CreateBridge(inner, observer, _context);
            }
            else
            {
                //
                // NB: This has been put in to allow the library to be used in a way similar to classic Rx,
                //     outside a hosting environment.
                //

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Derived classes should not pass null.)

                return inner.Subscribe(observer);

#pragma warning restore CA1062
#pragma warning restore IDE0079
            }
        }

        /// <summary>
        /// Loads a subscription to an inner sequence during recovery. The caller is responsible
        /// to create a new observer to subscribe to the inner sequence used in the recovered
        /// subscription. If the observer has state, it should implement load and save functionality
        /// using the IStatefulOperator interface.
        /// </summary>
        /// <typeparam name="TInner">Element type of the inner sequence.</typeparam>
        /// <param name="reader">Reader to read checkpoint state from.</param>
        /// <param name="observer">Observer to subscribe to the inner sequence.</param>
        /// <returns>Reconstructed subscription to the inner sequence.</returns>
        protected ISubscription LoadInner<TInner>(IOperatorStateReader reader, IObserver<TInner> observer)
        {
            return HigherOrderExecutionEnvironment.LoadBridge(reader, observer, _context);
        }

        /// <summary>
        /// Saves a subscription to an inner sequence during checkpointing. If the observer that was used
        /// to create the subscription has state, it should implement load and save functionality using
        /// the IStatefulOperator interface.
        /// </summary>
        /// <param name="innerSubscription">Inner subscription, retrieved from SubscribeInner, to persist.</param>
        /// <param name="writer">Writer to write checkpoint state to.</param>
        protected void SaveInner(ISubscription innerSubscription, IOperatorStateWriter writer)
        {
            HigherOrderExecutionEnvironment.SaveBridge(innerSubscription, writer, _context);
        }
    }
}
