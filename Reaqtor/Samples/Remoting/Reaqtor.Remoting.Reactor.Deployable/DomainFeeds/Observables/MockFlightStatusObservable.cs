// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Nuqleon.DataModel;

using Reaqtive;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.Observables
{
    using DataModels.FlightStatus;

    /// <summary>
    /// An Observable for retrieving mock flight status data
    /// </summary>
    [KnownType]
    public class MockFlightStatusObservable : SubscribableBase<FlightStatus>
    {
        /// <summary>
        /// The string to use when issuing the request for flight status to BDI
        /// </summary>
        private readonly string _flightStatusQueryString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockFlightStatusObservable" /> class.
        /// </summary>
        /// <param name="flightStatusParameters">The parameters to this FlightStatusObservable. Preference is given to the FlightName, followed by AirlineCode+FlightNumber,
        /// then AirlineName+FlightNumber.</param>
        /// <exception cref="System.ArgumentException">Thrown if flightStatusParameters doesn't contain enough information to determine a flight.</exception>
        public MockFlightStatusObservable(FlightStatusParameters flightStatusParameters)
        {
            if (flightStatusParameters == null)
            {
                throw new ArgumentNullException(nameof(flightStatusParameters));
            }

            if (!string.IsNullOrWhiteSpace(flightStatusParameters.FlightName))
            {
                _flightStatusQueryString = flightStatusParameters.FlightName;
            }
            else if (!string.IsNullOrWhiteSpace(flightStatusParameters.FlightNumber))
            {
                if (!string.IsNullOrWhiteSpace(flightStatusParameters.AirlineCode))
                {
                    _flightStatusQueryString = string.Format("{0}{1}", flightStatusParameters.AirlineCode, flightStatusParameters.FlightNumber);
                }
                else if (!string.IsNullOrWhiteSpace(flightStatusParameters.AirlineName))
                {
                    _flightStatusQueryString = string.Format("{0}{1}", flightStatusParameters.AirlineName, flightStatusParameters.FlightNumber);
                }
            }

            if (string.IsNullOrEmpty(_flightStatusQueryString))
            {
                throw new ArgumentException(string.Format("Not enough information in flightStatusParameters to specify a flight: {0}", flightStatusParameters));
            }
        }

        /// <summary>
        /// Creates a subscription to this Observable
        /// </summary>
        /// <param name="observer">The Observer to receive subscription events</param>
        /// <returns>The subscription</returns>
        protected override ISubscription SubscribeCore(IObserver<FlightStatus> observer) => new MockFlightStatusSubscription(this, observer);

        /// <summary>
        /// A subscription to MockFlightStatusObservable.
        /// </summary>
        private sealed class MockFlightStatusSubscription : Operator<MockFlightStatusObservable, FlightStatus>
        {
            /// <summary>
            /// The CancellationTokenSource to notify when disposing this subscription
            /// </summary>
            private readonly CancellationTokenSource _cancellationTokenSource = new();

            /// <summary>
            /// The MockFlightStatusObservable that was used to generated this MockFlightStatusSubscription
            /// </summary>
            private readonly MockFlightStatusObservable _mockFlightStatusObservable;

            /// <summary>
            /// The IOperatorContext being passed to this Subscription via IRP (through the Operator interface)
            /// </summary>
            private volatile IOperatorContext _operatorContext;

            /// <summary>
            /// Initializes a new instance of the <see cref="MockFlightStatusSubscription"/> class.
            /// </summary>
            /// <param name="mockFlightStatusObservable">The MockFlightStatusObservable used to create this subscription</param>
            /// <param name="observer">The observer to notify when something happens with this subscription</param>
            public MockFlightStatusSubscription(MockFlightStatusObservable mockFlightStatusObservable, IObserver<FlightStatus> observer) :
                base(mockFlightStatusObservable, observer)
            {
                _mockFlightStatusObservable = mockFlightStatusObservable;
            }

            /// <summary>
            /// Sets the operator's context as passed through all Operators of the current subscription in IRP.
            /// </summary>
            /// <param name="context">The context being passed.</param>
            public override void SetContext(IOperatorContext context)
            {
                Debug.Assert(context != null, "MockFlightStatusSubscription expecting a non-null operator context.");

                context.TraceSource.TraceInformation("MockFlightStatusSubscription IOperatorContext received.");
                _operatorContext = context;
            }

            /// <summary>
            /// Called when the subscription is supposed to start.
            /// </summary>
            protected override void OnStart()
            {
                if (_operatorContext == null)
                {
                    var exception =
                        new NullReferenceException(
                            "MockFlightStatusSubscription operatorContext not set. Expected SetContext() invoked before OnStart().");

                    throw exception;
                }

                _operatorContext.TraceSource.TraceInformation("MockFlightStatusSubscription starting");

                var cancellationToken = _cancellationTokenSource.Token;

                var myTask = new Task(
                    () =>
                    {
                        while (true)
                        {
                            _operatorContext.TraceSource.TraceInformation("MockFlightStatusSubscription notifying of event");

                            Output.OnNext(new FlightStatus()
                            {
                                AirlineCode = "AA",
                                AirlineName = "American",
                                FlightNumber = "1628",
                                FlightName = _mockFlightStatusObservable._flightStatusQueryString,
                                Status = "Landed",
                                StatusCode = "76",
                                OnTime = "delayed",
                                ScheduledDepartureDateTime = new DateTime(2013, 06, 27, 5, 0, 0),
                                UpdatedDepartureDateTime = new DateTime(2013, 06, 27, 5, 30, 0),
                                ScheduledArrivalDateTime = new DateTime(2013, 06, 27, 10, 0, 0),
                                UpdatedArrivalDateTime = new DateTime(2013, 06, 27, 10, 30, 0),
                                OriginAirport = new FlightStatusAirport
                                {
                                    Code = "SEA",
                                    Name = "Seattle",
                                    BingMapVenueId = string.Empty
                                },
                                DestinationAirport = new FlightStatusAirport
                                {
                                    Code = "ORD",
                                    Name = "Chicago",
                                    BingMapVenueId = string.Empty
                                },
                                DepartureGate = "7",
                                ArrivalGate = "K18",
                                ArrivalTerminal = "3",
                                DataFreshness = "2",
                            });

                            _operatorContext.TraceSource.TraceInformation("MockFlightStatusSubscription sleeping 5s");

                            Thread.Sleep(5000);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                _operatorContext.TraceSource.TraceInformation("MockFlightStatusSubscription was cancelled");
                                break;
                            }
                        }
                    },
                    cancellationToken,
                    TaskCreationOptions.LongRunning);

                myTask.Start();
            }

            /// <summary>
            /// Called when the subscription is to be disposed
            /// </summary>
            protected override void OnDispose()
            {
                if (_operatorContext == null)
                {
                    var exception =
                        new NullReferenceException(
                            "MockFlightStatusSubscription operatorContext not set. Expected SetContext() invoked before OnDispose().");

                    throw exception;
                }

                _operatorContext.TraceSource.TraceInformation("MockFlightStatusSubscription disposing");

                _cancellationTokenSource.Cancel();
            }
        }
    }
}
