// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Tasks;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// A sample observable sending an event every 2 seconds.
    /// </summary>
    public sealed class PersonObservable : ISubscribable<Person>
    {
        /// <summary>
        /// The parameters passed to the observable. For example the
        /// start, end and the thresholds for the traffic observable.
        /// </summary>
        private readonly PersonObservableParameters _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonObservable"/> class.
        /// </summary>
        public PersonObservable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonObservable"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public PersonObservable(PersonObservableParameters parameters)
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>the IDisposable.</returns>
        IDisposable IObservable<Person>.Subscribe(IObserver<Person> observer) => Subscribe(observer);

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>the ISubscription.</returns>
        public ISubscription Subscribe(IObserver<Person> observer) => new _(this, observer);

        /// <summary>
        /// An implementation of the context switch operator for subscriptions to
        /// the person observable, which seamlessly handles scheduling observer
        /// operations on worker threads on the query engine.
        /// </summary>
        private sealed class _ : ContextSwitchOperator<PersonObservable, Person>
        {
            /// <summary>
            /// The scheduler.
            /// </summary>
            private IScheduler _scheduler;

            /// <summary>
            /// Initializes the subscription object.
            /// </summary>
            /// <param name="parent">The parent subscribable.</param>
            /// <param name="observer">The observer to subscribe to.</param>
            public _(PersonObservable parent, IObserver<Person> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rct:Person";

            public override Version Version => new(1, 0, 0, 0);

            /// <summary>
            /// Sets the context for the subscription, including the scheduler.
            /// </summary>
            /// <param name="context">The operator context.</param>
            public override void SetContext(IOperatorContext context)
            {
                _scheduler = context.Scheduler;
                base.SetContext(context);
            }

            /// <summary>
            /// Called after the subscription has been set up.
            /// </summary>
            protected override void OnStart()
            {
                _scheduler.Schedule(new ActionTask(DoAndReschedule));
                base.OnStart();
            }

            /// <summary>
            /// Action to send a new event and schedule the next event.
            /// </summary>
            private void DoAndReschedule()
            {
                if (!IsDisposed)
                {
                    var person = new Person()
                    {
                        FirstName = Params._parameters.FirstName,
                        LastName = Params._parameters.LastName,
                        Age = Params._parameters.Age,
                        Occupation = Params._parameters.Occupation,
                        Location = "Home"
                    };

                    Trace.WriteLine(
                        string.Format(
                            "Publishing event: {0} {1}, {2}",
                            person.FirstName,
                            person.LastName,
                            person.Location));

                    OnNext(person);

                    _scheduler.Schedule(TimeSpan.FromTicks(2000), new ActionTask(DoAndReschedule));
                }
            }
        }
    }
}
