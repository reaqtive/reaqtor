// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//#define STACKTRACE // Enable this symbol for debugging of invalid concurrent accesses

using System;

#if DEBUG
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

using Reaqtive.Scheduler;
#endif

#if STACKTRACE
using System.Diagnostics;
#endif

namespace Reaqtive
{
    /// <summary>
    /// Base class for subscribable sources.
    /// </summary>
    /// <typeparam name="TResult">Type of the elements produced by the subscribable source.</typeparam>
    public abstract class SubscribableBase<TResult> : ISubscribable<TResult>
    {
        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        public ISubscription Subscribe(IObserver<TResult> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

#if DEBUG
            var assert = new AssertObserver(observer);
            assert.Subscription = SubscribeCore(assert);
            return assert;
#else
            return SubscribeCore(observer);
#endif
        }

        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        protected abstract ISubscription SubscribeCore(IObserver<TResult> observer);

        IDisposable IObservable<TResult>.Subscribe(IObserver<TResult> observer)
        {
            // TODO: Revisit this. Without a call to Start, this has little use. We can't really perform
            //       the Start on the user's behalf because we also need to do SetContext. Maybe we can
            //       do away with the ISubscribable<T> : IObservable<T> definition?
            return Subscribe(observer);
        }

#if DEBUG
        private sealed class AssertObserver : IObserver<TResult>, ISubscription, IOperator
        {
            private readonly IObserver<TResult> _observer;
            private int _stopped = 0;
            private int _busy = 0;
            private IScheduler _scheduler;

            public AssertObserver(IObserver<TResult> observer)
            {
                _observer = observer;
            }

            public ISubscription Subscription
            {
                get;
                set;
            }

            public void OnCompleted()
            {
                Enter();

                try
                {
                    if (Interlocked.CompareExchange(ref _stopped, 1, 0) == 1)
                    {
                        throw AlreadyStopped();
                    }

                    _observer.OnCompleted();
                }
                finally
                {
                    Leave();
                }
            }

            public void OnError(Exception error)
            {
                Enter();

                try
                {
                    if (Interlocked.CompareExchange(ref _stopped, 1, 0) == 1)
                    {
                        throw AlreadyStopped();
                    }

                    _observer.OnError(error);
                }
                finally
                {
                    Leave();
                }
            }

            public void OnNext(TResult value)
            {
                Enter();

                try
                {
                    if (Volatile.Read(ref _stopped) == 1)
                    {
                        throw AlreadyStopped();
                    }

                    _observer.OnNext(value);
                }
                finally
                {
                    Leave();
                }
            }

#if STACKTRACE
            private object _gate = new object();
            private StackTrace _trace;
#endif

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Enter([CallerMemberName] string method = null)
            {
#if STACKTRACE
                lock (_gate)
                {
                    if (Interlocked.CompareExchange(ref _busy, 1, 0) == 1)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected call to {0}. The observer is still processing a notification. Other thread's call stack: {1}", method, _trace));
                    }

                    _trace = new StackTrace(false);
                }
#else
                if (Interlocked.CompareExchange(ref _busy, 1, 0) == 1)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected call to {0}. The observer is still processing a notification.", method));
                }
#endif

                _scheduler?.VerifyAccess();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Leave()
            {
#if STACKTRACE
                lock (_gate)
                {
                    Interlocked.Exchange(ref _busy, 0);

                    _trace = null;
                }
#else
                Interlocked.Exchange(ref _busy, 0);
#endif
            }

            private static Exception AlreadyStopped([CallerMemberName] string method = null)
            {
                return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected call to {0}. The observer has already terminated.", method));
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
                visitor.Visit(this);
            }

            public void Dispose()
            {
                Subscription.Dispose();
            }

            public IEnumerable<ISubscription> Inputs => new[] { Subscription };

            public void Subscribe()
            {
            }

            public void SetContext(IOperatorContext context)
            {
                _scheduler = context.Scheduler;
            }

            public void Start()
            {
            }
        }
#endif
    }
}
