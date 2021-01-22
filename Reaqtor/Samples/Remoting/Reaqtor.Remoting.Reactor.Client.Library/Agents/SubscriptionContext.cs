// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

using Reaqtor.Remoting.Client;

namespace Reaqtor.Remoting.Reactor.Client
{
    public class SubscriptionContext : RemotingClientContext
    {
        public SubscriptionContext(IReactiveExpressionServices expressionServices, IReactiveServiceProvider serviceProvider)
            : base(expressionServices, serviceProvider)
        {
        }

        /// <summary>
        /// Gets the observer that expects http post notification.
        /// </summary>
        /// <typeparam name="T">The type of the entities being subscribed to.
        /// </typeparam>
        /// <param name="onCompleted">
        /// The uri to be posted to on subscription completion.</param>
        /// <param name="onError">
        /// The uri to be posted to when there is an error during subscription
        /// processing.</param>
        /// <returns>
        /// The observer that expects http post notification.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// onCompleted
        /// or
        /// onError
        /// </exception>
        [KnownResource(Constants.Observer.Final.HttpPost.NoHeaders)]
        public IAsyncReactiveQbserver<T> GetHttpPostObserver<T>(
            Uri onCompleted,
            Uri onError)
        {
            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            var observer = GetObserver<Uri, Uri, T>(new Uri(Constants.Observer.Final.HttpPost.NoHeaders));
            return observer(onCompleted, onError);
        }

        /// <summary>
        /// Gets the final observer that performs http post notification.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the entities being observed in the subscription.
        /// </typeparam>
        /// <param name="onCompleted">
        /// The uri to be posted to on subscription completion.</param>
        /// <param name="onError">
        /// The uri to be posted to when there is an error during subscription
        /// processing.</param>
        /// <param name="headers">
        /// A collection of key/value pairs to passed on the headers of the 
        /// request.
        /// </param>
        /// <returns>
        /// The observer that expects http post notification.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">onCompleted
        /// or
        /// onError
        /// or
        /// headers</exception>
        [KnownResource(Constants.Observer.Final.HttpPost.WithHeaders)]
        public IAsyncReactiveQbserver<T> GetHttpPostObserver<T>(
            Uri onCompleted,
            Uri onError,
            Tuple<string, string>[] headers)
        {
            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            var observer = GetObserver<Uri, Uri, Tuple<string, string>[], T>(new Uri(Constants.Observer.Final.HttpPost.WithHeaders));
            return observer(onCompleted, onError, headers);
        }

        /// <summary>
        /// Gets the final observer that performs an http notification.
        /// </summary>
        /// <typeparam name="T">The type of the entity being observed</typeparam>
        /// <param name="method">The HTTP method.</param>
        /// <param name="onCompleted">
        /// The uri to be posted to on subscription completion.</param>
        /// <param name="onError">
        /// The uri to be posted to when there is an error during subscription
        /// processing.</param>
        /// <param name="headers">
        /// A collection of key/value pairs to passed as the headers of the request.
        /// </param>
        /// <param name="retryData">The retry data.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// The observer that expects http notification.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>method</c> or <c>onCompleted</c> or <c>onError</c> is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <c>method</c> is not a recognized Http method</exception>
        [KnownResource(Constants.Observer.Final.Http.String)]
        public IAsyncReactiveQbserver<T> GetHttpFinalObserver<T>(
            string method,
            Uri onCompleted,
            Uri onError,
            Tuple<string, string>[] headers,
            RetryData retryData,
            TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (retryData == null)
            {
                throw new ArgumentNullException(nameof(retryData));
            }

            if (timeout < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout), timeout, "timeout less than zero");
            }

            var observer = GetObserver<string, Uri, Uri, Tuple<string, string>[], RetryData, TimeSpan, T>(Constants.Observer.Final.Http.Uri);
            return observer(method, onCompleted, onError, headers, retryData, timeout);
        }

        /// <summary>Gets the geo coordinate signals.</summary>
        [KnownResource(Constants.Stream.Real.GeoCoordinateSignal.String)]
        public IAsyncReactiveQbservable<GeoCoordinateSignal> GeoCoordinateSignals => GetObservable<GeoCoordinateSignal>(Constants.Stream.Real.GeoCoordinateSignal.Uri);

        /// <summary>
        /// Gets the observable for flight status events
        /// </summary>
        /// <param name="parameters">The parameters to flight status</param>
        /// <returns>The observable</returns>
        [KnownResource(Constants.Observable.Real.FlightStatus.String)]
        public IAsyncReactiveQbservable<FlightStatus> GetFlightStatusObservable(FlightStatusParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            bool hasValidFlightName = !string.IsNullOrWhiteSpace(parameters.FlightName);
            bool hasValidFlightNumberWithAirline = !string.IsNullOrWhiteSpace(parameters.FlightNumber) &&
                                                   (!string.IsNullOrWhiteSpace(parameters.AirlineName) ||
                                                    !string.IsNullOrWhiteSpace(parameters.AirlineCode));
            if (!hasValidFlightName && !hasValidFlightNumberWithAirline)
            {
                throw new ArgumentException(string.Format(
                    "Invalid FlightStatusParameters: missing flight name {0}, or flight number {1} with airline name {2} or airline code {3}",
                    parameters.FlightName,
                    parameters.FlightNumber,
                    parameters.AirlineName,
                    parameters.AirlineCode));
            }

            return GetObservable<FlightStatusParameters, FlightStatus>(Constants.Observable.Real.FlightStatus.Uri)(parameters);
        }

        /// <summary>
        /// Gets the observable for synthetic traffic info events
        /// </summary>
        /// <param name="parameters">The parameters to traffic info</param>
        /// <returns>The observable</returns>
        [KnownResource(Constants.Observable.Synthetic.TrafficInfo.String)]
        public IAsyncReactiveQbservable<TrafficInfo> GetTrafficInfoObservable(TrafficParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            bool hasValidRoute = (!string.IsNullOrEmpty(parameters.StartAddress) && !string.IsNullOrEmpty(parameters.EndAddress))
                              || (!string.IsNullOrEmpty(parameters.RouteId));
            if (!hasValidRoute)
            {
                throw new ArgumentException(string.Format(
                    "Invalid TrafficParameters: Either [start address:{0} and end address{1}] OR [routeid:{2}] must be provided",
                    parameters.StartAddress,
                    parameters.EndAddress,
                    parameters.RouteId));
            }

            // TODO: Make a note for a design review - this logic won't execute 
            //       if the GetTrafficInfoObservable is used in an inner position, 
            //       e.g. inside a SelectMany.
            var tuple = new Tuple<string, string, string, int, int>(
                parameters.StartAddress,
                parameters.EndAddress,
                parameters.RouteId,
                parameters.FlowParameters == null ? int.MaxValue : parameters.FlowParameters.NotificationThresholdInSeconds,
                parameters.FlowParameters == null ? int.MaxValue : parameters.FlowParameters.RenotificationThresholdInSeconds);

            return GetObservable<TrafficParameters, TrafficInfo>(Constants.Observable.Synthetic.TrafficInfo.Uri)(parameters);
        }

        /// <summary>
        /// Gets the observable for real traffic info events
        /// </summary>
        /// <param name="parameters">The parameters to traffic info</param>
        /// <returns>The observable</returns>
        [KnownResource(Constants.Observable.Real.TrafficInfo.String)]
        public IAsyncReactiveQbservable<TrafficInfo> GetRealTrafficInfoObservable(TrafficParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            bool hasValidRoute = (!string.IsNullOrEmpty(parameters.StartAddress) && !string.IsNullOrEmpty(parameters.EndAddress))
                              || (!string.IsNullOrEmpty(parameters.RouteId));
            if (!hasValidRoute)
            {
                throw new ArgumentException(string.Format(
                    "Invalid TrafficParameters: Either [start address:{0} and end address{1}] OR [routeid:{2}] must be provided",
                    parameters.StartAddress,
                    parameters.EndAddress,
                    parameters.RouteId));
            }

            var tuple = new Tuple<string, string, string, int, int>(
                parameters.StartAddress,
                parameters.EndAddress,
                parameters.RouteId,
                parameters.FlowParameters == null ? int.MaxValue : parameters.FlowParameters.NotificationThresholdInSeconds,
                parameters.FlowParameters == null ? int.MaxValue : parameters.FlowParameters.RenotificationThresholdInSeconds);

            return GetObservable<TrafficParameters, TrafficInfo>(Constants.Observable.Real.TrafficInfo.Uri)(parameters);
        }
    }
}
