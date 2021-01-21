// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// <WARNING>
//   This file was auto-generated on 01/13/2021 10:18:09.
//   To make changes, edit the .tt file.
// </WARNING>

namespace Reaqtive
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Tracing
    {
        public static void DelaySubscription_Subscribing(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceDelaySubscription_Subscribing;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 1, "DelaySubscription operator for subscription '{0}' subscribing to upstream source.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.DelaySubscription_Subscribing(arg0);
                }
            }
        }

        public static void Empty_Scheduling(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceEmpty_Scheduling;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 2, "Empty operator for subscription '{0}' scheduled terminal completed message.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Empty_Scheduling(arg0);
                }
            }
        }

        public static void Empty_Processing(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceEmpty_Processing;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 3, "Empty operator for subscription '{0}' processing terminal completed message.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Empty_Processing(arg0);
                }
            }
        }

        public static void Return_Scheduling(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReturn_Scheduling;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 4, "Return operator for subscription '{0}' scheduled message.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Return_Scheduling(arg0);
                }
            }
        }

        public static void Return_Processing(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReturn_Processing;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 5, "Return operator for subscription '{0}' processing message.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Return_Processing(arg0);
                }
            }
        }

        public static void Throw_Scheduling(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceThrow_Scheduling;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 6, "Throw operator for subscription '{0}' scheduled terminal error message.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Throw_Scheduling(arg0);
                }
            }
        }

        public static void Throw_Processing(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceThrow_Processing;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 7, "Throw operator for subscription '{0}' processing terminal error message.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Throw_Processing(arg0);
                }
            }
        }

        public static void StartWith_Scheduling(this TraceSource source, System.Uri subscriptionId, Int32 valuesIndex, Int32 length)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceStartWith_Scheduling;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();
                var arg1 = valuesIndex;
                var arg2 = length;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 8, "StartWith operator for subscription '{0}' scheduled message for item at index '{1}' (count = '{2}').", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.StartWith_Scheduling(arg0, arg1, arg2);
                }
            }
        }

        public static void StartWith_Processing(this TraceSource source, System.Uri subscriptionId, Int32 valuesIndex, Int32 length)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceStartWith_Processing;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();
                var arg1 = valuesIndex;
                var arg2 = length;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 9, "StartWith operator for subscription '{0}' processing message for item at index '{1}' (count = '{2}').", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.StartWith_Processing(arg0, arg1, arg2);
                }
            }
        }

        public static void StartWith_Subscribing(this TraceSource source, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceStartWith_Subscribing;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 10, "StartWith operator for subscription '{0}' subscribing to upstream source.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.StartWith_Subscribing(arg0);
                }
            }
        }

        public static void Retry_Retrying(this TraceSource source, System.Uri subscriptionId, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRetry_Retrying;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();
                var arg1 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 11, "Retry operator for subscription '{0}' is retrying. Error = {1}", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Retry_Retrying(arg0, arg1);
                }
            }
        }

        public static void Retry_Retrying_Count(this TraceSource source, System.Uri subscriptionId, Int32 count, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceRetry_Retrying_Count;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = subscriptionId.ToTraceString();
                var arg1 = count;
                var arg2 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 12, "Retry operator for subscription '{0}' is retrying. Remaining retry count = {1}, Error = {2}", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Retry_Retrying_Count(arg0, arg1, arg2);
                }
            }
        }

        public static void Timer_ScheduledToFire(this TraceSource source, Int32 timerId, System.Uri subscriptionId, Int64 tick, System.DateTimeOffset due)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_ScheduledToFire;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = tick;
                var arg3 = due.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 13, "Timer {0} for subscription '{1}' scheduled to fire '{2}' at {3}.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_ScheduledToFire(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Timer_Disposed(this TraceSource source, Int32 timerId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Disposed;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 14, "Timer {0} for subscription '{1}' disposed.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Disposed(arg0, arg1);
                }
            }
        }

        public static void Timer_Muted(this TraceSource source, Int32 timerId, System.Uri subscriptionId, Int64 tick, System.DateTimeOffset now, System.DateTimeOffset due, System.TimeSpan delta)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Muted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = tick;
                var arg3 = now.ToTraceString();
                var arg4 = due.ToTraceString();
                var arg5 = delta.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 15, "Timer {0} for subscription '{1}' muted '{2}' at {3}, was due at {4} (delta = {5}).", arg0, arg1, arg2, arg3, arg4, arg5);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Muted(arg0, arg1, arg2, arg3, arg4, arg5);
                }
            }
        }

        public static void Timer_Fired(this TraceSource source, Int32 timerId, System.Uri subscriptionId, Int64 tick, System.DateTimeOffset now, System.DateTimeOffset due, System.TimeSpan delta)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Fired;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = tick;
                var arg3 = now.ToTraceString();
                var arg4 = due.ToTraceString();
                var arg5 = delta.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 16, "Timer {0} for subscription '{1}' fired '{2}' at {3}, was due at {4} (delta = {5}).", arg0, arg1, arg2, arg3, arg4, arg5);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Fired(arg0, arg1, arg2, arg3, arg4, arg5);
                }
            }
        }

        public static void Timer_CatchUp(this TraceSource source, Int32 timerId, System.Uri subscriptionId, System.DateTimeOffset due)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_CatchUp;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = due.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 17, "Timer {0} for subscription '{1}' running with catch-up tick skip behavior; first due time shifted to {2}.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_CatchUp(arg0, arg1, arg2);
                }
            }
        }

        public static void Invalid_Setting(this TraceSource source, String settingId, String settingValue, System.Uri instanceId, String defaultSettingValue, String message)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceInvalid_Setting;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = settingId;
                var arg1 = settingValue;
                var arg2 = instanceId.ToTraceString();
                var arg3 = defaultSettingValue;
                var arg4 = message;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 18, "Setting '{0}' has an invalid value of '{1}' for artifact '{2}'. Using default value '{3}' instead. {4}", arg0, arg1, arg2, arg3, arg4);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Invalid_Setting(arg0, arg1, arg2, arg3, arg4);
                }
            }
        }

        public static void Timer_Single_Absolute_Created(this TraceSource source, Int32 timerId, System.Uri subscriptionId, System.DateTimeOffset due)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Single_Absolute_Created;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = due.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 19, "Timer {0} for subscription '{1}' created with initial absolute due time {2}.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Single_Absolute_Created(arg0, arg1, arg2);
                }
            }
        }

        public static void Timer_Single_Relative_Created(this TraceSource source, Int32 timerId, System.Uri subscriptionId, System.TimeSpan due)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Single_Relative_Created;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = due.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 20, "Timer {0} for subscription '{1}' created with initial relative due time {2}.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Single_Relative_Created(arg0, arg1, arg2);
                }
            }
        }

        public static void Timer_Periodic_Absolute_Created(this TraceSource source, Int32 timerId, System.Uri subscriptionId, System.DateTimeOffset due, System.TimeSpan period)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Periodic_Absolute_Created;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = due.ToTraceString();
                var arg3 = period.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 21, "Timer {0} for subscription '{1}' created with initial absolute due time {2} and period {3}.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Periodic_Absolute_Created(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Timer_Periodic_Relative_Created(this TraceSource source, Int32 timerId, System.Uri subscriptionId, System.TimeSpan due, System.TimeSpan period)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceTimer_Periodic_Relative_Created;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = timerId;
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = due.ToTraceString();
                var arg3 = period.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 22, "Timer {0} for subscription '{1}' created with initial relative due time {2} and period {3}.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Timer_Periodic_Relative_Created(arg0, arg1, arg2, arg3);
                }
            }
        }


        [EventSource(Name = "Reaqtive-Linq")]
        private class Impl : EventSource
        {
            internal static Impl Instance = new();

            private Impl()
                : base()
            {
            }

            /// <summary>
            /// Class providing keywords to annotate events with in order to allow filtering.
            /// </summary>
            public static class Keywords
            {
                /// <summary>
                /// Underlying value for keyword DelaySubscription
                /// </summary>
                public const EventKeywords DelaySubscription = (EventKeywords)4UL;

                /// <summary>
                /// Underlying value for keyword Empty
                /// </summary>
                public const EventKeywords Empty = (EventKeywords)8UL;

                /// <summary>
                /// Underlying value for keyword Return
                /// </summary>
                public const EventKeywords Return = (EventKeywords)16UL;

                /// <summary>
                /// Underlying value for keyword Throw
                /// </summary>
                public const EventKeywords Throw = (EventKeywords)32UL;

                /// <summary>
                /// Underlying value for keyword StartWith
                /// </summary>
                public const EventKeywords StartWith = (EventKeywords)64UL;

                /// <summary>
                /// Underlying value for keyword Retry
                /// </summary>
                public const EventKeywords Retry = (EventKeywords)128UL;

                /// <summary>
                /// Underlying value for keyword Timer
                /// </summary>
                public const EventKeywords Timer = (EventKeywords)256UL;

                /// <summary>
                /// Underlying value for keyword Setting
                /// </summary>
                public const EventKeywords Setting = (EventKeywords)512UL;

            }

            public static class Tasks
            {
                public const EventTask None = (EventTask)1;
            }

            public static class Opcodes
            {
            }

            [Event(1,
                Level = EventLevel.Verbose,
                Keywords = Keywords.DelaySubscription,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "DelaySubscription operator for subscription '{0}' subscribing to upstream source.",
                Version = 1)]
            internal unsafe void DelaySubscription_Subscribing(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(1, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceDelaySubscription_Subscribing => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.DelaySubscription);

            [Event(2,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Empty,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Empty operator for subscription '{0}' scheduled terminal completed message.",
                Version = 1)]
            internal unsafe void Empty_Scheduling(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(2, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceEmpty_Scheduling => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Empty);

            [Event(3,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Empty,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Empty operator for subscription '{0}' processing terminal completed message.",
                Version = 1)]
            internal unsafe void Empty_Processing(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(3, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceEmpty_Processing => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Empty);

            [Event(4,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Return,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Return operator for subscription '{0}' scheduled message.",
                Version = 1)]
            internal unsafe void Return_Scheduling(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(4, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReturn_Scheduling => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Return);

            [Event(5,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Return,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Return operator for subscription '{0}' processing message.",
                Version = 1)]
            internal unsafe void Return_Processing(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(5, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReturn_Processing => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Return);

            [Event(6,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Throw,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Throw operator for subscription '{0}' scheduled terminal error message.",
                Version = 1)]
            internal unsafe void Throw_Scheduling(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(6, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceThrow_Scheduling => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Throw);

            [Event(7,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Throw,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Throw operator for subscription '{0}' processing terminal error message.",
                Version = 1)]
            internal unsafe void Throw_Processing(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(7, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceThrow_Processing => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Throw);

            [Event(8,
                Level = EventLevel.Verbose,
                Keywords = Keywords.StartWith,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "StartWith operator for subscription '{0}' scheduled message for item at index '{1}' (count = '{2}').",
                Version = 1)]
            internal unsafe void StartWith_Scheduling(String subscriptionId, Int32 valuesIndex, Int32 length)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&valuesIndex);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&length);
                    descrs[2].Size = sizeof(Int32);

                    WriteEventCore(8, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceStartWith_Scheduling => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.StartWith);

            [Event(9,
                Level = EventLevel.Verbose,
                Keywords = Keywords.StartWith,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "StartWith operator for subscription '{0}' processing message for item at index '{1}' (count = '{2}').",
                Version = 1)]
            internal unsafe void StartWith_Processing(String subscriptionId, Int32 valuesIndex, Int32 length)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&valuesIndex);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&length);
                    descrs[2].Size = sizeof(Int32);

                    WriteEventCore(9, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceStartWith_Processing => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.StartWith);

            [Event(10,
                Level = EventLevel.Verbose,
                Keywords = Keywords.StartWith,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "StartWith operator for subscription '{0}' subscribing to upstream source.",
                Version = 1)]
            internal unsafe void StartWith_Subscribing(String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(10, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceStartWith_Subscribing => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.StartWith);

            [Event(11,
                Level = EventLevel.Informational,
                Keywords = Keywords.Retry,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Retry operator for subscription '{0}' is retrying. Error = {1}",
                Version = 1)]
            internal unsafe void Retry_Retrying(String subscriptionId, String error)
            {
                subscriptionId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)errorBytes;
                    descrs[1].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(11, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRetry_Retrying => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Retry);

            [Event(12,
                Level = EventLevel.Informational,
                Keywords = Keywords.Retry,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Retry operator for subscription '{0}' is retrying. Remaining retry count = {1}, Error = {2}",
                Version = 1)]
            internal unsafe void Retry_Retrying_Count(String subscriptionId, Int32 count, String error)
            {
                subscriptionId ??= string.Empty;
                error ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[0].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&count);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)errorBytes;
                    descrs[2].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(12, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceRetry_Retrying_Count => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Retry);

            [Event(13,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' scheduled to fire '{2}' at {3}.",
                Version = 2)]
            internal unsafe void Timer_ScheduledToFire(Int32 timerId, String subscriptionId, Int64 tick, String due)
            {
                subscriptionId ??= string.Empty;
                due ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* dueBytes = due)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&tick);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)dueBytes;
                    descrs[3].Size = checked((due.Length + 1) * 2);

                    WriteEventCore(13, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_ScheduledToFire => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Timer);

            [Event(14,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' disposed.",
                Version = 2)]
            internal unsafe void Timer_Disposed(Int32 timerId, String subscriptionId)
            {
                subscriptionId ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(14, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Disposed => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Timer);

            [Event(15,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' muted '{2}' at {3}, was due at {4} (delta = {5}).",
                Version = 2)]
            internal unsafe void Timer_Muted(Int32 timerId, String subscriptionId, Int64 tick, String now, String due, String delta)
            {
                subscriptionId ??= string.Empty;
                now ??= string.Empty;
                due ??= string.Empty;
                delta ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* nowBytes = now)
                fixed (char* dueBytes = due)
                fixed (char* deltaBytes = delta)
                {
                    var dataCount = 6;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&tick);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)nowBytes;
                    descrs[3].Size = checked((now.Length + 1) * 2);

                    descrs[4].DataPointer = (IntPtr)dueBytes;
                    descrs[4].Size = checked((due.Length + 1) * 2);

                    descrs[5].DataPointer = (IntPtr)deltaBytes;
                    descrs[5].Size = checked((delta.Length + 1) * 2);

                    WriteEventCore(15, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Muted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);

            [Event(16,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' fired '{2}' at {3}, was due at {4} (delta = {5}).",
                Version = 2)]
            internal unsafe void Timer_Fired(Int32 timerId, String subscriptionId, Int64 tick, String now, String due, String delta)
            {
                subscriptionId ??= string.Empty;
                now ??= string.Empty;
                due ??= string.Empty;
                delta ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* nowBytes = now)
                fixed (char* dueBytes = due)
                fixed (char* deltaBytes = delta)
                {
                    var dataCount = 6;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)(&tick);
                    descrs[2].Size = sizeof(Int64);

                    descrs[3].DataPointer = (IntPtr)nowBytes;
                    descrs[3].Size = checked((now.Length + 1) * 2);

                    descrs[4].DataPointer = (IntPtr)dueBytes;
                    descrs[4].Size = checked((due.Length + 1) * 2);

                    descrs[5].DataPointer = (IntPtr)deltaBytes;
                    descrs[5].Size = checked((delta.Length + 1) * 2);

                    WriteEventCore(16, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Fired => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);

            [Event(17,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' running with catch-up tick skip behavior; first due time shifted to {2}.",
                Version = 2)]
            internal unsafe void Timer_CatchUp(Int32 timerId, String subscriptionId, String due)
            {
                subscriptionId ??= string.Empty;
                due ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* dueBytes = due)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)dueBytes;
                    descrs[2].Size = checked((due.Length + 1) * 2);

                    WriteEventCore(17, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_CatchUp => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);

            [Event(18,
                Level = EventLevel.Error,
                Keywords = Keywords.Setting,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Setting '{0}' has an invalid value of '{1}' for artifact '{2}'. Using default value '{3}' instead. {4}",
                Version = 1)]
            internal unsafe void Invalid_Setting(String settingId, String settingValue, String instanceId, String defaultSettingValue, String message)
            {
                settingId ??= string.Empty;
                settingValue ??= string.Empty;
                instanceId ??= string.Empty;
                defaultSettingValue ??= string.Empty;
                message ??= string.Empty;
                fixed (char* settingIdBytes = settingId)
                fixed (char* settingValueBytes = settingValue)
                fixed (char* instanceIdBytes = instanceId)
                fixed (char* defaultSettingValueBytes = defaultSettingValue)
                fixed (char* messageBytes = message)
                {
                    var dataCount = 5;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)settingIdBytes;
                    descrs[0].Size = checked((settingId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)settingValueBytes;
                    descrs[1].Size = checked((settingValue.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[2].Size = checked((instanceId.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)defaultSettingValueBytes;
                    descrs[3].Size = checked((defaultSettingValue.Length + 1) * 2);

                    descrs[4].DataPointer = (IntPtr)messageBytes;
                    descrs[4].Size = checked((message.Length + 1) * 2);

                    WriteEventCore(18, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceInvalid_Setting => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.Setting);

            [Event(19,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' created with initial absolute due time {2}.",
                Version = 1)]
            internal unsafe void Timer_Single_Absolute_Created(Int32 timerId, String subscriptionId, String due)
            {
                subscriptionId ??= string.Empty;
                due ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* dueBytes = due)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)dueBytes;
                    descrs[2].Size = checked((due.Length + 1) * 2);

                    WriteEventCore(19, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Single_Absolute_Created => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);

            [Event(20,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' created with initial relative due time {2}.",
                Version = 1)]
            internal unsafe void Timer_Single_Relative_Created(Int32 timerId, String subscriptionId, String due)
            {
                subscriptionId ??= string.Empty;
                due ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* dueBytes = due)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)dueBytes;
                    descrs[2].Size = checked((due.Length + 1) * 2);

                    WriteEventCore(20, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Single_Relative_Created => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);

            [Event(21,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' created with initial absolute due time {2} and period {3}.",
                Version = 1)]
            internal unsafe void Timer_Periodic_Absolute_Created(Int32 timerId, String subscriptionId, String due, String period)
            {
                subscriptionId ??= string.Empty;
                due ??= string.Empty;
                period ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* dueBytes = due)
                fixed (char* periodBytes = period)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)dueBytes;
                    descrs[2].Size = checked((due.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)periodBytes;
                    descrs[3].Size = checked((period.Length + 1) * 2);

                    WriteEventCore(21, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Periodic_Absolute_Created => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);

            [Event(22,
                Level = EventLevel.Informational,
                Keywords = Keywords.Timer,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Timer {0} for subscription '{1}' created with initial relative due time {2} and period {3}.",
                Version = 1)]
            internal unsafe void Timer_Periodic_Relative_Created(Int32 timerId, String subscriptionId, String due, String period)
            {
                subscriptionId ??= string.Empty;
                due ??= string.Empty;
                period ??= string.Empty;
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* dueBytes = due)
                fixed (char* periodBytes = period)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)(&timerId);
                    descrs[0].Size = sizeof(Int32);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)dueBytes;
                    descrs[2].Size = checked((due.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)periodBytes;
                    descrs[3].Size = checked((period.Length + 1) * 2);

                    WriteEventCore(22, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceTimer_Periodic_Relative_Created => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Timer);
        }

        private static string ToTraceString<T>(this T obj) => obj?.ToString();
    }
}



