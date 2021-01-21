// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Expressions;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Reactive.Expressions;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TimelineObservable<T> : SubscribableBase<T>
    {
        private readonly Uri _uri;
        private readonly bool _useRelativeScheduling;
        private int _eventIndex;

        public TimelineObservable(Uri uri, bool useRelativeScheduling)
        {
            _uri = uri;
            _useRelativeScheduling = useRelativeScheduling;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            return _useRelativeScheduling
                ? new ColdTimelineSubscription(this, observer)
                : (ISubscription)new HotTimelineSubscription(this, observer);
        }

        private sealed class ColdTimelineSubscription : BaseTimelineSubscription
        {
            private long _subscribed;

            public ColdTimelineSubscription(TimelineObservable<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rct:TimelineObservable+Cold";

            public override Version Version => Versioning.v1;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _subscribed = _scheduler.Clock;
            }

            /// <summary>
            /// Loads the state of the cold timeline observable.
            /// </summary>
            /// <param name="writer">The state reader.</param>
            /// <remarks>
            /// For cold-scheduled timelines, recovery should not have the effect
            /// of triggering a re-scheduling from the start of the event list.
            /// Thus, we must save the time at which the timeline observable was
            /// subscribed to in order to filter out cold-subscribed events that
            /// would have occurred prior to the time of recovery.
            /// </remarks>
            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _subscribed = reader.Read<long>();
            }

            /// <summary>
            /// Saves the state of the cold timeline observable.
            /// </summary>
            /// <param name="writer">The state writer.</param>
            /// <remarks>
            /// For cold-scheduled timelines, recovery should not have the effect
            /// of triggering a re-scheduling from the start of the event list.
            /// Thus, we must save the time at which the timeline observable was
            /// subscribed to in order to filter out cold-subscribed events that
            /// would have occurred prior to the time of recovery.
            /// </remarks>
            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_subscribed);
            }

            #region Scheduling Helpers

            protected override long DueTime(long eventTime)
            {
                return _subscribed + eventTime;
            }

            protected override int SchedulingStartIndex => 0;

            #endregion
        }

        private sealed class HotTimelineSubscription : BaseTimelineSubscription
        {
            public HotTimelineSubscription(TimelineObservable<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rct:TimelineObservable+Hot";

            public override Version Version => Versioning.v1;

            #region Scheduling Helpers

            protected override long DueTime(long eventTime)
            {
                return eventTime;
            }

            protected override int SchedulingStartIndex => Params._eventIndex;

            #endregion
        }

        private abstract class BaseTimelineSubscription : StatefulOperator<TimelineObservable<T>, T>
        {
            private int _subscriptionIndex = -1;
            private IList<Recorded<INotification<string>>> _events;
            private TestSubscriptionStoreConnection _subscriptionStore;

            protected ITestScheduler _scheduler;

            public BaseTimelineSubscription(TimelineObservable<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            private IList<Subscription> Subscriptions
            {
                get
                {
                    if (!_subscriptionStore.TryGetValue(Params._uri.ToCanonicalString(), out var subscriptions))
                    {
                        subscriptions = new List<Subscription>();
                        _subscriptionStore.TryAdd(Params._uri.ToCanonicalString(), subscriptions);
                    }
                    return subscriptions;
                }
            }

            #region IOperator

            /// <summary>
            /// Sets the operator context for the timeline observable.
            /// </summary>
            /// <param name="context">The operator context.</param>
            /// <remarks>
            /// This operator is only expected to work with the remoting
            /// framework, specifically, a test query evaluator implementation
            /// designed to enable this operator.
            /// </remarks>
            public override void SetContext(IOperatorContext context)
            {
                _scheduler = (ITestScheduler)context.Scheduler;
                context.TryGetElement<TimelineStoreConnection>(TimelineStoreConnection.ContextHandle, out var eventTimelines);
                Debug.Assert(eventTimelines != null);
                eventTimelines.TryGetValue(Params._uri.ToCanonicalString(), out _events);
                context.TryGetElement<TestSubscriptionStoreConnection>(TestSubscriptionStoreConnection.ContextHandle, out _subscriptionStore);
                Debug.Assert(_subscriptionStore != null);
                base.SetContext(context);
            }

            protected override void OnStart()
            {
                if (!IsDisposed)
                {
                    Subscriptions.Add(new Subscription(_scheduler.Now.Ticks));
                    _subscriptionIndex = Subscriptions.Count - 1;
                    DoScheduling();
                }
            }

            protected override void OnDispose()
            {
                base.OnDispose();
                if (_subscriptionIndex >= 0)
                {
                    Subscriptions[_subscriptionIndex] = new Subscription(Subscriptions[_subscriptionIndex].Subscribe, _scheduler.Now.Ticks);
                }
            }

            #endregion

            #region Scheduling Helpers

            /// <summary>
            /// Schedule the events to process.
            /// </summary>
            /// <remarks>
            /// A major assumption used by this implementation is that, when used in
            /// conjunction with engine failovers, if a failover occurs at the same
            /// scheduler tick as one of the events in the timeline, the failover must
            /// always be scheduled to occur before the timeline event is scheduled
            /// (note the `dueTime >= _scheduler.Clock` check below). If the assumption
            /// does not hold true, then the timeline event will occur twice.
            /// </remarks>
            private void DoScheduling()
            {
                // For cold-scheduled events, we should always start from the beginning;
                // for hot-scheduled events, we should start from the last event processed.
                var startIndex = Params._useRelativeScheduling ? 0 : Params._eventIndex;

                for (var i = SchedulingStartIndex; i < _events.Count; ++i)
                {
                    // For cold-scheduled events, we should set the due time based on the subscribe time;
                    // For hot-scheduled events, we should set the due time to the absolute time given.
                    var dueTime = DueTime(_events[i].Time);

                    if (dueTime >= _scheduler.Clock)
                    {
                        var @event = _events[i].Value;
                        _scheduler.ScheduleAbsolute(dueTime, () =>
                        {
                            if (!IsDisposed)
                            {
                                ProcessEvent(@event);
                            }
                        });
                    }
                    else
                    {
                        // Keep the event index up to date in case resubscribes occur
                        Params._eventIndex++;
                    }
                }
            }

            private void ProcessEvent(INotification<string> @event)
            {
                // Keep the event index up to date in case resubscribes occur
                Params._eventIndex++;

                switch (@event.Kind)
                {
                    case Protocol.NotificationKind.OnCompleted:
                        Output.OnCompleted();
                        break;
                    case Protocol.NotificationKind.OnError:
                        Output.OnError(@event.Exception);
                        break;
                    case Protocol.NotificationKind.OnNext:
                        T nextValue;
                        if (typeof(T).FindGenericType(typeof(ISubscribable<>)) != null)
                        {
                            nextValue = SetupInnerSubscribable(@event);
                        }
                        else
                        {
                            nextValue = Deserialize<T>(@event.Value);
                        }
                        Output.OnNext(nextValue);
                        break;
                }
            }

            private static T SetupInnerSubscribable(INotification<string> @event)
            {
                var elementType = typeof(T).FindGenericType(typeof(ISubscribable<>)).GenericTypeArguments[0];
                var timelineType = typeof(TimelineObservable<>).MakeGenericType(elementType);
                var timelineCtor = timelineType.GetConstructor(new[] { typeof(Uri), typeof(bool) });
                var expression = (InvocationExpression)BetaReducer.Reduce(Deserialize<Expression>(@event.Value));
                var parameter = (ParameterExpression)expression.Expression;
                var timelineUri = (Uri)expression.Arguments[0].Evaluate();

                Expression subscribableExpression = parameter.Name switch
                {
                    Constants.Test.ColdTimelineObservable.String => Expression.New(timelineCtor, Expression.Constant(timelineUri), Expression.Constant(true)),
                    Constants.Test.HotTimelineObservable.String => Expression.New(timelineCtor, Expression.Constant(timelineUri), Expression.Constant(true)),
                    _ => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unexpected subscribable type '{0}'.", parameter.Name)),
                };

                var quotedSubscribableType = typeof(QuotedSubscribable<>).MakeGenericType(elementType);
                var quotedSubscribableCtor = quotedSubscribableType.GetConstructor(new[] { typeof(Expression) });
                return (T)quotedSubscribableCtor.Invoke(new object[] { subscribableExpression });
            }

            private static TValue Deserialize<TValue>(string value)
            {
                return new SerializationHelpers().Deserialize<TValue>(value);
            }

            protected abstract long DueTime(long eventTime);

            protected abstract int SchedulingStartIndex
            {
                get;
            }

            #endregion
        }
    }
}
