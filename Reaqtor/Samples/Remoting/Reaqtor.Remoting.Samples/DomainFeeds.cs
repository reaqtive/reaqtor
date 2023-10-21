// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

using Nuqleon.DataModel;

using Reaqtor.Remoting.Samples.Models;
using Reaqtor.Remoting.Samples.Models.V2;

namespace Reaqtor.Remoting.Samples
{
    public static class DomainFeeds
    {
        #region Scenarios

        public static IAsyncReactiveQbservable<TrafficInfo> CreateTrafficQbservable(ReactiveClientContext clientContext)
        {
            var tarnEndpoint = "bing://platform.bing.com/tarn/";
            var startMonitoring = DateTimeOffset.Now.AddMinutes(TrafficSubscriptionStartListeningAfterInMinutes);
            var stopMonitoring = DateTimeOffset.Now.AddMinutes(TrafficSubscriptionStopMonitoringAfterInMinutes);
            var noTrafficNotifyTime = stopMonitoring.AddMinutes(-15);

            int notificationThresholdInSecs = 300;
            int renotificationThresholdInSecs = 300;
            TimeSpan extraBuffer = TimeSpan.FromMinutes(2);

            TimeSpan travelTimeWithoutTraffic = TimeSpan.FromSeconds(700);
            TimeSpan initialTravelTimeWithTraffic = TimeSpan.FromSeconds(800);

            return clientContext.GetObservable<TrafficParameters, TrafficConfiguration, TrafficInfo>(
                new Uri("bing://platform.bing.com/observable/real/trafficinfo/generic"))(
                    new TrafficParameters { RouteId = "bla" }, new TrafficConfiguration { ServiceEndpoint = tarnEndpoint })
                    .DelaySubscription(startMonitoring)
                    .TakeUntil(stopMonitoring)
                    .Where(traffic => traffic.NotificationType == NotificationTypeEnum.Flow)
                    .Select(traffic =>
                        clientContext.Timer(
                            // the right time to fire is the different between UTC now and the right
                            // time to leave for the event. The right time to leave is the UTC event
                            // start time minus the travel duration without traffic, minus the traffic
                            // minus a configurable extra buffer
                            startMonitoring
                            - TimeSpan.FromSeconds(traffic.FlowInfo.FreeFlowTravelDurationInSeconds)
                            - TimeSpan.FromSeconds(traffic.FlowInfo.DelayInSeconds)
                            - TimeSpan.FromSeconds(notificationThresholdInSecs)
                            - extraBuffer)
                            .Select(_ => traffic))
                    .StartWith(clientContext.Timer(noTrafficNotifyTime).Select(_ => new TrafficInfo()
                    {
                        FlowInfo = new TrafficFlowInfo()
                        {
                            DelayInSeconds = 0,
                            FreeFlowTravelDurationInSeconds = (int)travelTimeWithoutTraffic.TotalSeconds,
                            HovDelayInSeconds = 0,
                        },
                        NotificationType = NotificationTypeEnum.Flow,
                        Subscription = new TrafficParameters()
                        {
                            StartAddress = "startBla",
                            EndAddress = "endBla",
                            StartTime = startMonitoring,
                            EndTime = stopMonitoring,
                            FlowParameters = new TrafficFlowParameters()
                            {
                                NotificationThresholdInSeconds = notificationThresholdInSecs,
                                RenotificationThresholdInSeconds = renotificationThresholdInSecs
                            }
                        },
                        SubscriptionId = "NoTrafficEventsSubscriptionId",
                        TimestampUTC = DateTime.UtcNow
                    }))
                    .Switch()
                    .Take(TrafficSubscriptionTakeCount);
        }

        public static IAsyncReactiveQbservable<TrafficNotification> CreateTrafficV2Qbservable(ReactiveClientContext clientContext)
        {
            var arrivalTime = DateTimeOffset.Now.AddHours(3);
            int notificationThresholdInSecs = 300;

            return clientContext.GetObservable<TrafficInput, TrafficNotification>(
                new Uri("bing://platform.bing.com/observable/real/trafficnotification"))(
                    new TrafficInput
                    {
                        NotificationThresholdInSeconds = notificationThresholdInSecs,
                        ArrivalTime = arrivalTime,
                        RouteWayPoints = new WayPoint[]
                        {
                            new() { Address = "Bellevue, WA" },
                            new() { Address = "Redmond, WA" }
                        }
                    });
        }

        public static IAsyncReactiveQbservable<NewsInfo> CreateNewsQbservable(ReactiveClientContext clientContext)
        {
            return
                clientContext.GetObservable<NewsParameters, NewsInfo>(
                    new Uri("bing://platform.bing.com/observable/real/newsinfo"))(
                        new NewsParameters { SearchTerm = "bla", NotificationThresholdInSeconds = 10, TriggerImmediately = true });
        }

        public static IAsyncReactiveQbservable<Models.V2.WeatherAlert> CreateWeatherQbservable(ReactiveClientContext clientContext, long id, string firehoseUri)
        {
            string hoistedId = id.ToString();

            return
                clientContext.GetObservable<Models.V2.WeatherAlert>(new Uri(firehoseUri))
                        .Where(weatherAlert => string.Equals(weatherAlert.Id, hoistedId))
                        .DistinctUntilChanged(t => string.Format(CultureInfo.InvariantCulture, "{0}:{1}", t.Title, t.StartTime));
        }

        public static IAsyncReactiveQbservable<Models.V2.WeatherAlert> CreateRawWeatherQbservable(ReactiveClientContext clientContext, string firehoseUri)
        {
            return clientContext.GetObservable<Models.V2.WeatherAlert>(new Uri(firehoseUri));
        }

        public static IAsyncReactiveQbservable<Models.V2.FlightInfo> CreateFlightsQbservable(ReactiveClientContext clientContext, long id, string firehoseUri)
        {
            // skip until and take until to keep the query running for a long time
            var skipUntil = DateTimeOffset.Now.AddMinutes(1);
            var takeUntil = DateTimeOffset.Now.AddDays(7);

            string hoistedId = id.ToString();

            return
                clientContext.GetObservable
                    <Models.V2.FlightInfo>(
                        new Uri(firehoseUri))
                        .Where(flightInfo => string.Equals(flightInfo.FlightNumber, hoistedId))
                        .Select(t => t)
                        .DistinctUntilChanged(t => t.DateTimeRecorded)
                        .SkipUntil(skipUntil)
                        .TakeUntil(takeUntil)
                        .Take(FlightsSubscriptionTakeCount);
        }

        public static IAsyncReactiveQbservable<Models.V2.FlightInfo> CreateRawFlightsQbservable(ReactiveClientContext clientContext, string firehoseUri)
        {
            return clientContext.GetObservable<Models.V2.FlightInfo>(new Uri(firehoseUri));
        }

        public static IAsyncReactiveQbservable<CasiEvent> CreateCasiQbservable(ReactiveClientContext clientContext, long id, string firehoseUri)
        {
            string hoistedId = id.ToString();

            return
                clientContext.GetObservable<CasiEvent>(new Uri(firehoseUri))
                    .Where(casiEvent => string.Equals(casiEvent.ItemId, hoistedId))
                    .DistinctUntilChanged(t => string.Format(CultureInfo.InvariantCulture, "{0}:{1}", t.Action, t.Environment));
        }

        public static IAsyncReactiveQbservable<CasiEvent> CreateRawCasiQbservable(ReactiveClientContext clientContext, string firehoseUri)
        {
            return clientContext.GetObservable<CasiEvent>(new Uri(firehoseUri));
        }

        #endregion

        #region Constants

        #region Static Properties

        // NB: The used to come from config and are hence defined as properties.

        private static int TrafficSubscriptionStartListeningAfterInMinutes => 5;

        private static int TrafficSubscriptionStopMonitoringAfterInMinutes => 15;

        private static int TrafficSubscriptionTakeCount => 1;

        private static int FlightsSubscriptionTakeCount => 1;

        #endregion

        #endregion

        private class TrafficConfiguration
        {
            /// <summary>
            /// Gets or sets the TaRN service endpoint.
            /// </summary>
            [Mapping("bing://reactiveprocessingentity/traffic/configuration/serviceendpoint")]
            public string ServiceEndpoint { get; set; }
        }
    }
}
