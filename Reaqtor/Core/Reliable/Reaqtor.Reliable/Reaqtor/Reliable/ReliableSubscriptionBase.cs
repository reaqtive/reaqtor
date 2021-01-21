// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks; will become non-nullable references going forward.

using System;
using System.Collections.Generic;

using Reaqtive;

namespace Reaqtor.Reliable
{
    /// <summary>
    /// Base implementation of a reliable subscription.
    /// </summary>
    /// <remarks>
    /// This base implementation adds an additional state transition to the lifecycle
    /// of the subscription. In addition to OnSubscribe, Start(long),
    /// AcknowledgeRange(long), and Dispose(), we have added a Stop() transition,
    /// which has the role of allowing certain lifecycle state transitions to be
    /// muted if they are not relevant to the current state of the subscription. For
    /// example, we can mute AcknowledgeRange(long) events for subscriptions that
    /// have been disposed. The following state transitions are now possible:
    ///
    /// 1) .ctor -> Dispose()
    /// 2) .ctor -> Start(long) -> AcknowledgeRange(long)* -> Stop() ->
    ///        AcknowledgeRange(long) -> Dispose()
    ///
    /// The '*' after AcknowledgeRange(long) means that any number of these events can
    /// occur between Start(long) and Stop(). As you can see, this base implementation
    /// is no longer sending acknowledgements after the subscription is disposed.
    /// </remarks>
    public abstract class ReliableSubscriptionBase : IReliableSubscription, IOperator
    {
        private IOperatorContext _context;
        private bool _started;
        private bool _disposed;
        private bool _disposeAcknowledged;

        #region IReliableSubscription

        /// <summary>
        /// The URI used to resubscribe.
        /// </summary>
        public abstract Uri ResubscribeUri
        {
            get;
        }

        //
        // TODO: Clean up the explicit interface implementations to support a *Core pattern
        //       using protected members that can be overridden by derived classes.
        //

        /// <summary>
        /// Called to trigger the start of the subscription at a given sequence id.
        /// </summary>
        /// <param name="sequenceId">The sequence id to start at.</param>
        void IReliableSubscription.Start(long sequenceId)
        {
            _started = true;

            Start(sequenceId);
        }

        /// <summary>
        /// Called to trigger the start of the subscription at a given sequence id.
        /// </summary>
        /// <param name="sequenceId">The sequence id to start at.</param>
        public abstract void Start(long sequenceId);

        /// <summary>
        /// Called to acknowledge that all messages up to a given sequence id have
        /// been received.
        /// </summary>
        /// <param name="sequenceId">The acknowledged sequence id.</param>
        void IReliableSubscription.AcknowledgeRange(long sequenceId)
        {
            // If the subscription has not yet been "stopped" or "disposed",
            // send the acknowledge as usual.
            if (!_disposed)
            {
                AcknowledgeRange(sequenceId);

                _context?.TraceSource.ReliableSubscriptionBase_OnStateSaved(_context.InstanceId, sequenceId);
            }
            // Otherwise, assuming the subscription is in the "stopped" state
            //
            else if (_started && !_disposeAcknowledged)
            {
                AcknowledgeRange(sequenceId);
                _disposeAcknowledged = true;
                DisposeCore();

                _context?.TraceSource.ReliableSubscriptionBase_OnStateSavedDispose(_context.InstanceId, sequenceId);
            }
            else
            {
                _context?.TraceSource.ReliableSubscriptionBase_OnStateSavedMuted(_context.InstanceId, sequenceId);
            }
        }

        /// <summary>
        /// Called to acknowledge that all messages up to a given sequence id have
        /// been received.
        /// </summary>
        /// <param name="sequenceId">The acknowledged sequence id.</param>
        public abstract void AcknowledgeRange(long sequenceId);

        /// <summary>
        /// Visits the subscription for actions such as
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(ISubscriptionVisitor visitor) => visitor.Visit(this);

        #endregion

        #region IOperator

        /// <summary>
        /// The subscription inputs to the reliable subscription.
        /// </summary>
        public virtual IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

        public virtual void Subscribe() { }

        /// <summary>
        /// This method is called before the subscription is started.
        /// </summary>
        /// <param name="context">The operator context.</param>
        public virtual void SetContext(IOperatorContext context) => _context = context;

        /// <summary>
        /// This start method should not be used. Instead use the start
        /// method taking a sequence id as a parameter.
        /// </summary>
        public void Start()
        {
        }

#pragma warning disable CA1716 // Identifiers should not match keywords. (Start and Stop seems reasonable.)

        /// <summary>
        /// A lifecycle hook that will be called when a reliable subscription is
        /// disposed, but is still awaiting a final acknowledgement event before the
        /// call to dispose is made. This is the entry point to the "stopped" state.
        /// </summary>
        protected virtual void Stop()
        {
            _context?.TraceSource?.ReliableSubscriptionBase_Stop(_context.InstanceId);
        }

#pragma warning restore CA1716

        /// <summary>
        /// Called to dispose the reliable subscription. This implementation
        /// will delay the disposal of the subscription until an acknowledgement has
        /// been received.
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
                _disposed = true;

                // If the subscription has been started, do not dispose. Instead, transition
                // to "stopped" state which should no longer propogate any observer events.
                // Once a final acknowledgement is received by the subscription, the
                // subscription may enter the disposed state.
                if (_started && !_disposeAcknowledged)
                {
                    Stop();

                    _context?.TraceSource?.TraceInformation("Reliable subscription input for '{0}' deferring Dispose() until AcknowledgeRange(long) received.", _context.InstanceId);
                }
                // Note, if the subscription was not started, as would be the case if we
                // recover a disposed subscription, we will immediately enter the disposed
                // state.
                else
                {
                    DisposeCore();

                    _context?.TraceSource?.TraceInformation("Reliable subscription input for '{0}' sent Dispose().", _context.InstanceId);
                }
            }
        }

        /// <summary>
        /// Called to dispose the reliable subscription.
        /// </summary>
        public abstract void DisposeCore();

        #endregion
    }
}
