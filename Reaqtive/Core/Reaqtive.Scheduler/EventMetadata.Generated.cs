// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// <WARNING>
//   This file was auto-generated on 01/13/2021 10:18:15.
//   To make changes, edit the .tt file.
// </WARNING>

namespace Reaqtive.Scheduler
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Tracing
    {
        public static void Worker_Heartbeat(this TraceSource source, System.String workerName, System.Int32 readyTasks, System.Int32 notReadyTasks, System.Int32 timeScheduledItems)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceWorker_Heartbeat;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = workerName;
                var arg1 = readyTasks;
                var arg2 = notReadyTasks;
                var arg3 = timeScheduledItems;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 1, "Worker '{0}' received heartbeat. Ready queue size = '{1}'. Not ready queue size = '{2}'. Time scheduled queue size = '{3}'.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Worker_Heartbeat(arg0, arg1, arg2, arg3);
                }
            }
        }

        public static void Worker_NextTimeScheduledItem(this TraceSource source, System.String workerName, System.DateTimeOffset dueTime)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceWorker_NextTimeScheduledItem;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = workerName;
                var arg1 = dueTime.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 2, "Worker '{0}' next time scheduled item due at '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.Worker_NextTimeScheduledItem(arg0, arg1);
                }
            }
        }

        public static void WorkItemBase_ExecutionException(this TraceSource source, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceWorkItemBase_ExecutionException;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 3, "Work item disposing because of exception {0}.", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.WorkItemBase_ExecutionException(arg0);
                }
            }
        }


        [EventSource(Name = "Reaqtive-Scheduler")]
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
                /// Underlying value for keyword Worker
                /// </summary>
                public const EventKeywords Worker = (EventKeywords)1UL;

                /// <summary>
                /// Underlying value for keyword WorkItemBase
                /// </summary>
                public const EventKeywords WorkItemBase = (EventKeywords)2UL;

            }

            public static class Tasks
            {
                public const EventTask None = (EventTask)1;
            }

            public static class Opcodes
            {
            }

            [Event(1,
                Level = EventLevel.Informational,
                Keywords = Keywords.Worker,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Worker '{0}' received heartbeat. Ready queue size = '{1}'. Not ready queue size = '{2}'. Time scheduled queue size = '{3}'.",
                Version = 1)]
            internal unsafe void Worker_Heartbeat(String workerName, Int32 readyTasks, Int32 notReadyTasks, Int32 timeScheduledItems)
            {
                workerName ??= string.Empty;
                fixed (char* workerNameBytes = workerName)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)workerNameBytes;
                    descrs[0].Size = checked((workerName.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&readyTasks);
                    descrs[1].Size = sizeof(Int32);

                    descrs[2].DataPointer = (IntPtr)(&notReadyTasks);
                    descrs[2].Size = sizeof(Int32);

                    descrs[3].DataPointer = (IntPtr)(&timeScheduledItems);
                    descrs[3].Size = sizeof(Int32);

                    WriteEventCore(1, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceWorker_Heartbeat => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.Worker);

            [Event(2,
                Level = EventLevel.Verbose,
                Keywords = Keywords.Worker,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Worker '{0}' next time scheduled item due at '{1}'.",
                Version = 1)]
            internal unsafe void Worker_NextTimeScheduledItem(String workerName, String dueTime)
            {
                workerName ??= string.Empty;
                dueTime ??= string.Empty;
                fixed (char* workerNameBytes = workerName)
                fixed (char* dueTimeBytes = dueTime)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)workerNameBytes;
                    descrs[0].Size = checked((workerName.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)dueTimeBytes;
                    descrs[1].Size = checked((dueTime.Length + 1) * 2);

                    WriteEventCore(2, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceWorker_NextTimeScheduledItem => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.Worker);

            [Event(3,
                Level = EventLevel.Error,
                Keywords = Keywords.WorkItemBase,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Work item disposing because of exception {0}.",
                Version = 1)]
            internal unsafe void WorkItemBase_ExecutionException(String error)
            {
                error ??= string.Empty;
                fixed (char* errorBytes = error)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)errorBytes;
                    descrs[0].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(3, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceWorkItemBase_ExecutionException => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.WorkItemBase);
        }

        private static string ToTraceString<T>(this T obj) => obj?.ToString();
    }
}



