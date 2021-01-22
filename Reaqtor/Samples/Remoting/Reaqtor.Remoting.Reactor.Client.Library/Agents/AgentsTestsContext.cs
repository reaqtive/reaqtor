// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Reactor.Client
{
    public class AgentsTestContext : SubscriptionContext
    {
        private readonly string HttpTestEventHandlerEndpoint = "http://localhost:20287?id=" + Guid.NewGuid();

        public AgentsTestContext(IReactiveExpressionServices expressionServices, IReactiveServiceProvider serviceProvider)
            : base(expressionServices, serviceProvider)
        {
        }

        public Task<IAsyncReactiveQubscription> FlightBackwardsCompatibleSubscription
        {
            get
            {
                Uri onFlightStatusUpdate = new Uri("reactor://platform.bing.com/agents/FlightStatus/Update");
                Uri onComplete = new Uri("reactor://platform.bing.com/agents/FlightStatus/Completed");
                Uri onError = new Uri("reactor://platform.bing.com/agents/FlightStatus/Error");
                var headerCollection = new[] { new Tuple<string, string>("X-FD-ImpressionGUID", "requestId") };

                // TakeUntil value is too long in future but this is a fallback value, ideally subscription will get unsubscribed as soon as agent gets "Landed" event back
                DateTime takeUntil = DateTime.UtcNow.AddSeconds(30);
                DateTime skipUntil = DateTime.UtcNow.AddSeconds(5);
                string airlineCode = "AirlineCode";
                string flightNumber = "FlightNumber";
                DateTime flightDepartureDate = DateTime.UtcNow.Date;
                string originAirportCode = "OriginAirportCode";
                string destinationAirportCode = "DestinationAirportCode";
                int notificationThresholdInMinutes = 5;

                JObject info = new JObject(
                    new JProperty("AirlineCode", airlineCode),
                    new JProperty("FlightNumber", flightNumber),
                    new JProperty("FlightDepartureDate", flightDepartureDate),
                    new JProperty("OriginAirportCode", originAirportCode),
                    new JProperty("DestinationAirportCode", destinationAirportCode),
                    new JProperty("NotificationThresholdInMinutes", notificationThresholdInMinutes),
                    new JProperty("TakeUntil", takeUntil),
                    new JProperty("SkipUntil", skipUntil),
                    new JProperty("Track", new JArray("track")));

                List<string> statusCodes = new List<string>() { "A", "S" };

                var flights = GetFlightStatusObservable(new FlightStatusParameters { FlightName = airlineCode + flightNumber, FlightNumber = flightNumber, AirlineCode = airlineCode });

                return (from flight in flights
                        where flight.ScheduledDepartureDateTime.ToUniversalTime().Date == flightDepartureDate
                              && flight.OriginAirport.Code == originAirportCode
                              && flight.DestinationAirport.Code == destinationAirportCode
                              && statusCodes.Contains(flight.StatusCode)
                              && (flight.UpdatedDepartureDateTime.Subtract(flight.ScheduledDepartureDateTime).TotalMinutes > notificationThresholdInMinutes
                              || flight.UpdatedArrivalDateTime.Subtract(flight.ScheduledArrivalDateTime).TotalMinutes > notificationThresholdInMinutes
                              || statusCodes.Contains(flight.StatusCode))
                        select new FlightsBvtProjection()
                        {
                            AirlineCode = flight.AirlineCode,
                            FlightNumber = flight.FlightNumber,
                            ScheduledDepartureDateTime = flight.ScheduledDepartureDateTime,
                            ScheduledArrivalDateTime = flight.ScheduledArrivalDateTime,
                            OriginAirportCode = flight.OriginAirport.Code,
                            DestinationAirportCode = flight.DestinationAirport.Code,
                            StatusCode = flight.StatusCode,
                            DepartureDelayInMinutes = flight.UpdatedDepartureDateTime.Subtract(flight.ScheduledDepartureDateTime).TotalMinutes,
                            ArrivalDelayInMinutes = flight.UpdatedArrivalDateTime.Subtract(flight.ScheduledArrivalDateTime).TotalMinutes
                        })
                    .DistinctUntilChanged(t => t)
                    .SkipUntil(skipUntil)
                    .TakeUntil(takeUntil)
                    .DoHttpPost(
                        flight => onFlightStatusUpdate,
                        flight => string.Format(
                            "{{ AirlineCode: '{0}', FlightNumber: '{1}', ScheduledDepartureDateTime: '{2}', ScheduledArrivalDateTime: '{3}', OriginAirportCode: '{4}', DestinationAirportCode:'{5}', StatusCode: '{6}', DepartureDelayInMinutes: '{7}', ArrivalDelayInMinutes: '{8}'}}",
                            flight.AirlineCode,
                            flight.FlightNumber,
                            flight.ScheduledDepartureDateTime,
                            flight.ScheduledArrivalDateTime,
                            flight.OriginAirportCode,
                            flight.DestinationAirportCode,
                            flight.StatusCode,
                            flight.DepartureDelayInMinutes,
                            flight.ArrivalDelayInMinutes),
                        headers => headerCollection)
                    .SubscribeAsync(GetHttpPostObserver<FlightsBvtProjection>(onComplete, onError), new Uri("reactor://test/agents/subscription"), null);
            }
        }

        public Task<IAsyncReactiveQubscription> TrafficIncidentsBackwardsCompatibleSubscription
        {
            get
            {
                var uri = new Uri("reactor://test/agents/subscription");
                string baseUri = "http://127.0.0.1/v1/TestCallback/ripeCallback?fdtrace=1&agentId=Traffic_2.0.0.0.0&agentInstanceId=" + Guid.NewGuid().ToString("d");
                string requestId = Guid.NewGuid().ToString("d");
                Uri onEvent = new Uri(baseUri + "&action=Event");
                Uri onComplete = new Uri(baseUri + "&action=Complete");
                Uri onError = new Uri(baseUri + "&action=Error");

                var headers = new[] { new Tuple<string, string>("X-FD-ImpressionGUID", requestId) };

                var context = this; // NB: For testing purposes only; normally has a client context.

                var useMocks = true;
                string startAddress = "1020 Enterprise Way, Sunnyvale, CA";
                string endAddress = "Dublin, CA";
                DateTimeOffset startMonitoring = DateTimeOffset.UtcNow;
                DateTimeOffset stopMonitoring = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(7);

                Func<TrafficParameters, IAsyncReactiveQbservable<TrafficInfo>> incidents = context.GetRealTrafficInfoObservable;
                if (useMocks)
                {
                    incidents = context.GetTrafficInfoObservable;
                }

                return incidents(
                    new TrafficParameters()
                    {
                        StartAddress = startAddress,
                        EndAddress = endAddress,
                        StartTime = startMonitoring,
                        EndTime = stopMonitoring
                    })
                   .Where(travel =>
                       travel.NotificationType == NotificationTypeEnum.Incident
                           && travel.IncidentInfo.Severity >= IncidentSeverityEnum.Moderate
                           && travel.IncidentInfo.Status == IncidentStatusEnum.NewIncident)
                   .DelaySubscription(startMonitoring)
                   .TakeUntil(stopMonitoring)
                   .DoHttpPost(
                       _ => onEvent,
                       travel => JObject.FromObject(travel).ToString(),
                       _ => headers)
                   .SubscribeAsync(context.GetHttpPostObserver<TrafficInfo>(onComplete, onError, headers), uri, null);
            }
        }

        public Task<IAsyncReactiveQubscription> TrafficTimeToLeaveValidationNoFlowEvents
        {
            get
            {
                string requestId = Guid.NewGuid().ToString("d");

                Uri onEvent = new Uri(HttpTestEventHandlerEndpoint + "&action=Event");
                Uri onComplete = new Uri(HttpTestEventHandlerEndpoint + "&action=Complete");
                Uri onError = new Uri(HttpTestEventHandlerEndpoint + "&action=Error");
                var headers = new[] { Tuple.Create("X-FD-ImpressionGUID", requestId) };

                var context = this;

                string noTrafficSubscriptionId = "NoTrafficNotify";

                int notificationThresholdInSecs = 900;
                int renotificationThresholdInSecs = notificationThresholdInSecs;
                TimeSpan extraBuffer = TimeSpan.FromMinutes(2);

                TimeSpan travelTimeWithoutTraffic = TimeSpan.FromSeconds(1800);

                TimeSpan overhead = extraBuffer;
                DateTimeOffset eventStartTime = DateTimeOffset.UtcNow + travelTimeWithoutTraffic + overhead +
                                                TimeSpan.FromMinutes(1);
                DateTimeOffset noTrafficNotifyTime = eventStartTime - travelTimeWithoutTraffic - overhead;
                DateTimeOffset startMonitoring = DateTimeOffset.UtcNow;
                DateTimeOffset stopMonitoring = noTrafficNotifyTime + TimeSpan.FromMinutes(1);

                string startAddress = "Start";
                string endAddress = "End";

                // ================================================================
                // RIPE SUBSCRIPTION starts here
                // ================================================================
                Func<TrafficParameters, IAsyncReactiveQbservable<TrafficInfo>> flowObservable =
                    context.GetTrafficInfoObservable;

                return
                    flowObservable(new TrafficParameters()
                    {
                        StartAddress = startAddress,
                        EndAddress = endAddress,
                        StartTime = startMonitoring,
                        EndTime = stopMonitoring,
                        FlowParameters = new TrafficFlowParameters()
                        {
                            NotificationThresholdInSeconds = notificationThresholdInSecs,
                            RenotificationThresholdInSeconds = renotificationThresholdInSecs
                        }
                    })
                    .DelaySubscription(startMonitoring)
                    .TakeUntil(stopMonitoring)
                    .Select(traffic =>
                        context.Timer(
                            // the right time to fire is the different between UTC now and the right
                            // time to leave for the event. The right time to leave is the UTC event
                            // start time minus the travel duration without traffic, minus the traffic
                            // delay minus the minimum threshold to trigger a traffic notification
                            // minus a configurable extra buffer
                            eventStartTime
                            - TimeSpan.FromSeconds(traffic.FlowInfo.FreeFlowTravelDurationInSeconds)
                            - TimeSpan.FromSeconds(traffic.FlowInfo.DelayInSeconds)
                            - TimeSpan.FromSeconds(notificationThresholdInSecs)
                            - extraBuffer)
                            .Select(_ => traffic))
                    .StartWith(context.Timer(noTrafficNotifyTime).Select(_ => new TrafficInfo()
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
                            StartAddress = startAddress,
                            EndAddress = endAddress,
                            StartTime = startMonitoring,
                            EndTime = stopMonitoring,
                            FlowParameters = new TrafficFlowParameters()
                            {
                                NotificationThresholdInSeconds = notificationThresholdInSecs,
                                RenotificationThresholdInSeconds = renotificationThresholdInSecs
                            }
                        },
                        SubscriptionId = noTrafficSubscriptionId,
                        TimestampUTC = DateTime.UtcNow
                    }))
                    .Switch()
                    .Take(1)
                    .DoHttp(
                        _ => WebRequestMethods.Http.Post,
                        _ => onEvent,
                        traffic => JObject.FromObject(traffic).ToString(),
                        _ => headers,
                        _ => RetryData.NoRetry(),
                        _ => new TimeSpan(0, 1, 0))
                    .SubscribeAsync(
                        context.GetHttpFinalObserver<TrafficInfo>(WebRequestMethods.Http.Post,
                            onComplete,
                            onError,
                            headers,
                            RetryData.NoRetry(),
                            new TimeSpan(0, 1, 0)),
                        new Uri("reactor://test/agents/subscription"),
                        null,
                        CancellationToken.None);
            }
        }

#pragma warning disable CA1822 // Mark members as static (compat)
        public IAsyncReactiveQbservable<FlightsBvtProjection> GetFlightBackwardsCompatibleQuery(IAsyncReactiveQbservable<FlightStatus> source, long now)
        {
            DateTimeOffset takeUntil = new DateTimeOffset(now, TimeSpan.Zero).AddSeconds(30);
            DateTimeOffset skipUntil = new DateTimeOffset(now, TimeSpan.Zero).AddSeconds(5);
            DateTimeOffset flightDepartureDate = DateTime.UtcNow.Date;
            string originAirportCode = "OriginAirportCode";
            string destinationAirportCode = "DestinationAirportCode";
            int notificationThresholdInMinutes = 5;

            List<string> statusCodes = new List<string>() { "A", "S" };
            return (from flight in source
                    where flight.ScheduledDepartureDateTime.ToUniversalTime().Date == flightDepartureDate
                          && flight.OriginAirport.Code == originAirportCode
                          && flight.DestinationAirport.Code == destinationAirportCode
                          && statusCodes.Contains(flight.StatusCode)
                          && (flight.UpdatedDepartureDateTime.Subtract(flight.ScheduledDepartureDateTime).TotalMinutes > notificationThresholdInMinutes
                          || flight.UpdatedArrivalDateTime.Subtract(flight.ScheduledArrivalDateTime).TotalMinutes > notificationThresholdInMinutes
                          || statusCodes.Contains(flight.StatusCode))
                    select new FlightsBvtProjection()
                    {
                        AirlineCode = flight.AirlineCode,
                        FlightNumber = flight.FlightNumber,
                        ScheduledDepartureDateTime = flight.ScheduledDepartureDateTime,
                        ScheduledArrivalDateTime = flight.ScheduledArrivalDateTime,
                        OriginAirportCode = flight.OriginAirport.Code,
                        DestinationAirportCode = flight.DestinationAirport.Code,
                        StatusCode = flight.StatusCode,
                        DepartureDelayInMinutes = flight.UpdatedDepartureDateTime.Subtract(flight.ScheduledDepartureDateTime).TotalMinutes,
                        ArrivalDelayInMinutes = flight.UpdatedArrivalDateTime.Subtract(flight.ScheduledArrivalDateTime).TotalMinutes
                    })
                .DistinctUntilChanged(t => t)
                .SkipUntil(skipUntil)
                .TakeUntil(takeUntil);
        }

        public IAsyncReactiveQbservable<TrafficInfo> GetTrafficIncidentsBackwardsCompatibleQuery(IAsyncReactiveQbservable<TrafficInfo> source, long now)
        {
            DateTimeOffset startMonitoring = new DateTimeOffset(now, TimeSpan.Zero);
            DateTimeOffset stopMonitoring = new DateTimeOffset(now, TimeSpan.FromMinutes(7));

            return source
               .Where(travel =>
                   travel.NotificationType == NotificationTypeEnum.Incident
                       && travel.IncidentInfo.Severity >= IncidentSeverityEnum.Moderate
                       && travel.IncidentInfo.Status == IncidentStatusEnum.NewIncident)
               .DelaySubscription(startMonitoring)
               .TakeUntil(stopMonitoring);
        }

        public IAsyncReactiveQbservable<TrafficInfo> GetTrafficTimeToLeaveValidationNoFlowEventsQuery(IAsyncReactiveQbservable<TrafficInfo> source, long now)
        {
            string noTrafficSubscriptionId = "NoTrafficNotify";

            int notificationThresholdInSecs = 900;
            int renotificationThresholdInSecs = notificationThresholdInSecs;
            TimeSpan extraBuffer = TimeSpan.FromMinutes(2);

            TimeSpan travelTimeWithoutTraffic = TimeSpan.FromSeconds(1800);

            TimeSpan overhead = extraBuffer;
            DateTimeOffset eventStartTime = new DateTimeOffset(now, TimeSpan.Zero) + travelTimeWithoutTraffic + overhead +
                                            TimeSpan.FromMinutes(1);
            DateTimeOffset noTrafficNotifyTime = eventStartTime - travelTimeWithoutTraffic - overhead;
            DateTimeOffset startMonitoring = new DateTimeOffset(now, TimeSpan.Zero);
            DateTimeOffset stopMonitoring = noTrafficNotifyTime + TimeSpan.FromMinutes(1);

            string startAddress = "Start";
            string endAddress = "End";

            return
                source
                .DelaySubscription(startMonitoring)
                .TakeUntil(stopMonitoring)
                .Select(traffic =>
                    this.Timer(
                        // the right time to fire is the different between UTC now and the right
                        // time to leave for the event. The right time to leave is the UTC event
                        // start time minus the travel duration without traffic, minus the traffic
                        // delay minus the minimum threshold to trigger a traffic notification
                        // minus a configurable extra buffer
                        eventStartTime
                        - TimeSpan.FromSeconds(traffic.FlowInfo.FreeFlowTravelDurationInSeconds)
                        - TimeSpan.FromSeconds(traffic.FlowInfo.DelayInSeconds)
                        - TimeSpan.FromSeconds(notificationThresholdInSecs)
                        - extraBuffer)
                        .Select(_ => traffic))
                .StartWith(this.Timer(noTrafficNotifyTime).Select(_ => new TrafficInfo()
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
                        StartAddress = startAddress,
                        EndAddress = endAddress,
                        StartTime = startMonitoring,
                        EndTime = stopMonitoring,
                        FlowParameters = new TrafficFlowParameters()
                        {
                            NotificationThresholdInSeconds = notificationThresholdInSecs,
                            RenotificationThresholdInSeconds = renotificationThresholdInSecs
                        }
                    },
                    SubscriptionId = noTrafficSubscriptionId,
                    TimestampUTC = DateTime.UtcNow
                }))
                .Switch()
                .Take(1);
        }
#pragma warning restore CA1822 // Mark members as static
    }
}
