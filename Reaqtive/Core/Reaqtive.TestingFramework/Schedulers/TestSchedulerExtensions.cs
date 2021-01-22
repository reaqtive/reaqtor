// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive.Scheduler;
using Reaqtive.Testing;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Extensions for the test scheduler.
    /// </summary>
    public static class TestSchedulerExtensions
    {
        /// <summary>
        /// Starts the test scheduler and uses the specified virtual time to create, subscribe and dispose the subscription to the sequence
        /// obtained through the factory function. Scheduler is passed to the subscription using the provided operator context.
        /// Default virtual times are used for <see cref="ReactiveTest.Created">factory invocation</see> and <see cref="ReactiveTest.Subscribed">sequence subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="context">The context.</param>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <param name="created">The created time.</param>
        /// <param name="subscribed">The subscribed time.</param>
        /// <param name="disposed">Virtual time at which to dispose the subscription.</param>
        /// <param name="recovery">Optional state container to reload state from at subscription time.</param>
        /// <returns>
        /// Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.
        /// </returns>
        public static ITestableObserver<T> Start<T>(
            this TestScheduler scheduler,
            IOperatorContext context,
            Func<ISubscribable<T>> create,
            long created,
            long subscribed,
            long disposed,
            IOperatorStateContainer recovery = null)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (create == null)
            {
                throw new ArgumentNullException(nameof(create));
            }

            if (created > subscribed)
            {
                throw new ArgumentOutOfRangeException(nameof(created));
            }

            if (subscribed > disposed)
            {
                throw new ArgumentOutOfRangeException(nameof(subscribed));
            }

            var source = default(ISubscribable<T>);
            var subscription = default(ISubscription);
            var observer = scheduler.CreateObserver<T>();

            scheduler.ScheduleAbsolute(default(object), created, (s, state) => source = create());
            scheduler.ScheduleAbsolute(
                default(object),
                subscribed,
                (s, state) =>
                {
                    subscription = source.Subscribe(observer);

                    var reader = default(IOperatorStateReaderFactory);

                    if (recovery != null)
                    {
                        reader = recovery.CreateReader();
                    }

                    new SubscriptionInitializeVisitor(subscription).Initialize(context, reader);
                });
            scheduler.ScheduleAbsolute(default(object), disposed, (s, state) => subscription.Dispose());
            scheduler.Start();
            return observer;
        }

        /// <summary>
        /// Starts the test scheduler and uses the specified virtual time to create, subscribe and dispose the subscription to the sequence
        /// obtained through the factory function. Scheduler is passed to the subscription using the provided operator context.
        /// Default virtual times are used for <see cref="ReactiveTest.Created">factory invocation</see> and <see cref="ReactiveTest.Subscribed">sequence subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <param name="created">The created time.</param>
        /// <param name="subscribed">The subscribed time.</param>
        /// <param name="disposed">Virtual time at which to dispose the subscription.</param>
        /// <param name="recovery">Optional state container to reload state from at subscription time.</param>
        /// <returns>
        /// Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.
        /// </returns>
        public static ITestableObserver<T> Start<T>(
            this TestScheduler scheduler,
            Func<ISubscribable<T>> create,
            long created,
            long subscribed,
            long disposed,
            IOperatorStateContainer recovery = null)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return scheduler.Start(scheduler.CreateContext(), create, created, subscribed, disposed, recovery);
        }

        /// <summary>
        /// Starts the test scheduler and creates the subscription to the sequence obtained through the factory function.
        /// Scheduler is passed to the subscription using the provided operator context.
        /// Default virtual times are used for <see cref="ReactiveTest.Created">factory invocation</see> and <see cref="ReactiveTest.Subscribed">sequence subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <returns>
        /// Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.
        /// </returns>
        public static ITestableObserver<T> Start<T>(this TestScheduler scheduler, Func<ISubscribable<T>> create)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return scheduler.Start(scheduler.CreateContext(), create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);
        }

        /// <summary>
        /// Starts the test scheduler and uses the specified virtual time to dispose the subscription to the sequence obtained through
        /// the factory function. Scheduler is passed to the subscription using the provided operator context.
        /// Default virtual times are used for <see cref="ReactiveTest.Created">factory invocation</see> and <see cref="ReactiveTest.Subscribed">sequence subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="scheduler">The scheduler.</param>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <param name="disposed">Virtual time at which to dispose the subscription.</param>
        /// <returns>
        /// Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.
        /// </returns>
        public static ITestableObserver<T> Start<T>(this TestScheduler scheduler, Func<ISubscribable<T>> create, long disposed)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return scheduler.Start(scheduler.CreateContext(), create, ReactiveTest.Created, ReactiveTest.Subscribed, disposed);
        }

        /// <summary>
        /// Schedules an action to be executed at relative dueTime.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Relative time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        public static void ScheduleRelative(this TestScheduler scheduler, long dueTime, Action action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            scheduler.ScheduleRelative(action, dueTime, Invoke);
        }

        /// <summary>
        /// Schedules an action to be executed at absolute dueTime.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        public static void ScheduleAbsolute(this TestScheduler scheduler, long dueTime, Action action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            scheduler.ScheduleAbsolute(action, dueTime, Invoke);
        }

        private static void Invoke(IScheduler scheduler, Action action)
        {
            Debug.Assert(scheduler != null, "Scheduler should not be null.");
            Debug.Assert(action != null, "Action should not be null.");

            action();
        }
    }
}
