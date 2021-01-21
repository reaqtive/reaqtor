// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive.Scheduler;
using Reaqtive.Testing;
using Reaqtive.TestingFramework.Mocks;

namespace Reaqtive.TestingFramework
{
    /// <summary>
    /// Virtual time test scheduler.
    /// </summary>
    public class TestScheduler : VirtualTimeLogicalScheduler<long, long>, IClockable<long>, ITestScheduler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestScheduler"/> class.
        /// </summary>
        public TestScheduler()
            : this(new PhysicalTestScheduler())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestScheduler"/> class
        /// with the given physical scheduler.
        /// </summary>
        /// <param name="physicalScheduler">The physical scheduler.</param>
        internal TestScheduler(PhysicalTestScheduler physicalScheduler)
            : base(physicalScheduler, parent: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestScheduler"/> class
        /// with the given parent virtual scheduler.
        /// </summary>
        /// <param name="parent">The parent virtual scheduler.</param>
        protected TestScheduler(TestScheduler parent)
            : base((parent ?? throw new ArgumentNullException(nameof(parent))).Physical, parent)
        {
        }

        /// <summary>
        /// Sets the time increment for when events scheduled to
        /// execute immediately will occur.
        /// </summary>
        public virtual long Increment => 1L;

        public override IScheduler CreateChildScheduler()
        {
            return CreateChildTestScheduler();
        }

        public TestScheduler CreateChildTestScheduler()
        {
            return new TestScheduler(this);
        }

        /// <summary>
        /// Schedules a task to be executed at dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state.</param>
        /// <param name="dueTime">Absolute time at which to execute the task.</param>
        /// <param name="action">The action.</param>
        public void ScheduleAbsolute<TState>(TState state, long dueTime, Action<IScheduler, TState> action)
        {
            ScheduleAbsolute(dueTime, new ActionTask<TState>(action, state));
        }

        /// <summary>
        /// Schedules a task to be executed at dueTime.
        /// </summary>
        /// <param name="dueTime">The absolute time at which to execute the task.</param>
        /// <param name="task">The task.</param>
        public override void ScheduleAbsolute(long dueTime, ISchedulerTask task)
        {
            if (dueTime <= Clock)
            {
                dueTime = Clock + Increment;
            }

            base.ScheduleAbsolute(dueTime, task);
        }

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        public void ScheduleRelative<TState>(TState state, long dueTime, Action<IScheduler, TState> action)
        {
            ScheduleRelative(dueTime, new ActionTask<TState>(action, state));
        }

        /// <summary>
        /// Creates an observer that records received notification messages and timestamps those.
        /// </summary>
        /// <typeparam name="T">The element type of the observer being created.</typeparam>
        /// <returns>Observer that can be used to assert the timing of received notifications.</returns>
        public ITestableObserver<T> CreateObserver<T>()
        {
            return new MockObserver<T>(this);
        }

        /// <summary>
        /// Creates a hot observable using the specified timestamped notification messages.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being created.</typeparam>
        /// <param name="messages">Notifications to surface through the created sequence at their specified absolute virtual times.</param>
        /// <returns>Hot observable sequence that can be used to assert the timing of subscriptions and notifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="messages"/> is null.</exception>
        public ITestableSubscribable<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            return new HotSubscribable<T>(this, messages);
        }

        /// <summary>
        /// Creates a cold observable using the specified timestamped notification messages.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being created.</typeparam>
        /// <param name="messages">Notifications to surface through the created sequence at their specified virtual time offsets from the sequence subscription time.</param>
        /// <returns>Cold observable sequence that can be used to assert the timing of subscriptions and notifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="messages"/> is null.</exception>
        public ITestableSubscribable<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            return new ColdSubscribable<T>(this, messages);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable CA1822 // Mark members as static (Future extensibility)
        /// <summary>
        /// Creates a state container.
        /// </summary>
        /// <returns>A new instance of state container.</returns>
        public IOperatorStateContainer CreateStateContainer()
        {
            // CONSIDER: We could pass the scheduler down, in order to keep track when OnSave/OnLoad happened (and allow asserting on timing of these).
            return new MockOperatorStateContainer();
        }
#pragma warning restore CA1822
#pragma warning restore IDE0079

        /// <summary>
        /// Creates an operator context with current scheduler.
        /// </summary>
        /// <param name="environment">Environment operators run in.</param>
        /// <param name="trace">Trace source to log to.</param>
        /// <param name="settings">Settings to expose on the context.</param>
        /// <returns>A new instance of operator context.</returns>
        public IOperatorContext CreateContext(IExecutionEnvironment environment = null, TraceSource trace = null, IDictionary<string, object> settings = null)
        {
            return new MockOperatorContext { Scheduler = this, ExecutionEnvironment = environment, TraceSource = trace, Settings = settings };
        }

        private sealed class ActionTask<TState> : ISchedulerTask
        {
            private readonly Action<IScheduler, TState> _action;
            private readonly TState _state;

            public ActionTask(Action<IScheduler, TState> action, TState state)
            {
                _action = action;
                _state = state;
                IsRunnable = true;
            }

            public long Priority => 0;

            public bool IsRunnable { get; set; }

            public bool Execute(IScheduler scheduler)
            {
                _action(scheduler, _state);
                return true;
            }

            public void RecalculatePriority()
            {
            }
        }
    }
}
