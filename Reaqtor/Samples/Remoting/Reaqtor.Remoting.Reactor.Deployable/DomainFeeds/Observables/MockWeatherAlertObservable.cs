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
    using DataModels.Weather;

    /// <summary>
    /// An Observable for retrieving weather alert data
    /// </summary>
    [KnownType]
    public class MockWeatherAlertObservable : SubscribableBase<WeatherAlert>
    {
#pragma warning disable IDE0052 // Remove unread private members (placeholder)
        private readonly WeatherAlertParameters _weatherAlertParameters;
#pragma warning restore IDE0052 // Remove unread private members

        /// <summary>
        /// Initializes a new instance of the <see cref="MockWeatherAlertObservable" /> class.
        /// </summary>
        /// <param name="weatherAlertParameters">The parameters to this WeatherAlertObservable.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if weatherAlertParameters are invalid</exception>
        public MockWeatherAlertObservable(WeatherAlertParameters weatherAlertParameters)
        {
            _weatherAlertParameters = weatherAlertParameters;
        }

        /// <summary>
        /// Creates a subscription to this Observable
        /// </summary>
        /// <param name="observer">The Observer to receive subscription events</param>
        /// <returns>The subscription</returns>
        protected override ISubscription SubscribeCore(IObserver<WeatherAlert> observer) => new MockWeatherAlertSubscription(this, observer);

        /// <summary>
        /// A subscription to MockWeatherAlertObservable
        /// </summary>
        private sealed class MockWeatherAlertSubscription : Operator<MockWeatherAlertObservable, WeatherAlert>
        {
            /// <summary>
            /// The CancellationTokenSource to notify when disposing this subscription
            /// </summary>
            private readonly CancellationTokenSource _cancellationTokenSource = new();

            /// <summary>
            /// The IOperatorContext being passed to this Subscription via IRP (through the Operator interface)
            /// </summary>
            private volatile IOperatorContext _operatorContext;

            /// <summary>
            /// Initializes a new instance of the <see cref="MockWeatherAlertSubscription"/> class.
            /// </summary>
            /// <param name="mockWeatherAlertObservable">The MockWeatherAlertObservable used to create this subscription</param>
            /// <param name="observer">The observer to notify when something happens with this subscription</param>
            public MockWeatherAlertSubscription(MockWeatherAlertObservable mockWeatherAlertObservable, IObserver<WeatherAlert> observer)
                : base(mockWeatherAlertObservable, observer)
            {
            }

            /// <summary>
            /// Sets the operator's context as passed through all Operators of the current subscription in IRP.
            /// </summary>
            /// <param name="context">The context being passed.</param>
            public override void SetContext(IOperatorContext context)
            {
                Debug.Assert(context != null, "MockWeatherAlertSubscription expecting a non-null operator context.");

                context.TraceSource.TraceInformation("MockWeatherAlertSubscription IOperatorContext received.");
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
                            "MockWeatherAlertSubscription operatorContext not set. Expected SetContext() invoked before OnStart().");

                    throw exception;
                }

                _operatorContext.TraceSource.TraceInformation("MockWeatherAlertSubscription starting");

                var cancellationToken = _cancellationTokenSource.Token;

                var myTask = new Task(
                    () =>
                    {
                        while (true)
                        {
                            _operatorContext.TraceSource.TraceInformation("MockWeatherAlertSubscription notifying with an alert.");
                            Output.OnNext(new WeatherAlert { Title = "Flash Flood Watch", CreateTime = new DateTime(2013, 7, 10, 5, 0, 0), ExpirationTime = new DateTime(2013, 7, 11, 4, 0, 0), EventId = "blah" });
                            _operatorContext.TraceSource.TraceInformation("MockWeatherAlertSubscription notifying waiting 5 seconds.");
                            Thread.Sleep(5000);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                _operatorContext.TraceSource.TraceInformation("MockWeatherAlertSubscription was cancelled");
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
                            "MockWeatherAlertSubscription operatorContext not set. Expected SetContext() invoked before OnDispose().");

                    throw exception;
                }

                _operatorContext.TraceSource.TraceInformation("MockWeatherAlertSubscription disposing");

                _cancellationTokenSource.Cancel();
            }
        }
    }
}
