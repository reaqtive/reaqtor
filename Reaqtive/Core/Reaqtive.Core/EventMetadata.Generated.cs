// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// <WARNING>
//   This file was auto-generated on 01/13/2021 10:17:59.
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
        public static void ContextSwitch_OnEnqueueing_Error(this TraceSource source, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceContextSwitch_OnEnqueueing_Error;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 1, "ContextSwitchOperator Enqueueing event failure. Exception: {0}", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ContextSwitch_OnEnqueueing_Error(arg0);
                }
            }
        }

        public static void ContextSwitch_Dequeued_Error(this TraceSource source, System.Exception exception)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceContextSwitch_Dequeued_Error;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = exception.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 2, "ContextSwitchOperator Dequeued event failure. Exception: {0}", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ContextSwitch_Dequeued_Error(arg0);
                }
            }
        }

        public static void ContextSwitch_Observer_Error(this TraceSource source, System.Uri instanceId, String call, System.Object arg, System.Exception error)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceContextSwitch_Observer_Error;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();
                var arg1 = call;
                var arg2 = arg.ToTraceString();
                var arg3 = error.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Error, 3, "ContextSwitchOperator for instance '{0}' failed to call {1}({2}) on the observer due to exception '{3}'. The operator is terminating.", arg0, arg1, arg2, arg3);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ContextSwitch_Observer_Error(arg0, arg1, arg2, arg3);
                }
            }
        }


        [EventSource(Name = "Reaqtive-Core")]
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
                /// Underlying value for keyword ContextSwitch
                /// </summary>
                public const EventKeywords ContextSwitch = (EventKeywords)1UL;

            }

            public static class Tasks
            {
                public const EventTask None = (EventTask)1;
            }

            public static class Opcodes
            {
            }

            [Event(1,
                Level = EventLevel.Error,
                Keywords = Keywords.ContextSwitch,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "ContextSwitchOperator Enqueueing event failure. Exception: {0}",
                Version = 1)]
            internal unsafe void ContextSwitch_OnEnqueueing_Error(String exception)
            {
                exception ??= string.Empty;
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)exceptionBytes;
                    descrs[0].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(1, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceContextSwitch_OnEnqueueing_Error => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.ContextSwitch);

            [Event(2,
                Level = EventLevel.Error,
                Keywords = Keywords.ContextSwitch,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "ContextSwitchOperator Dequeued event failure. Exception: {0}",
                Version = 1)]
            internal unsafe void ContextSwitch_Dequeued_Error(String exception)
            {
                exception ??= string.Empty;
                fixed (char* exceptionBytes = exception)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)exceptionBytes;
                    descrs[0].Size = checked((exception.Length + 1) * 2);

                    WriteEventCore(2, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceContextSwitch_Dequeued_Error => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.ContextSwitch);

            [Event(3,
                Level = EventLevel.Error,
                Keywords = Keywords.ContextSwitch,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "ContextSwitchOperator for instance '{0}' failed to call {1}({2}) on the observer due to exception '{3}'. The operator is terminating.",
                Version = 1)]
            internal unsafe void ContextSwitch_Observer_Error(String instanceId, String call, String arg, String error)
            {
                instanceId ??= string.Empty;
                call ??= string.Empty;
                arg ??= string.Empty;
                error ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                fixed (char* callBytes = call)
                fixed (char* argBytes = arg)
                fixed (char* errorBytes = error)
                {
                    var dataCount = 4;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)callBytes;
                    descrs[1].Size = checked((call.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)argBytes;
                    descrs[2].Size = checked((arg.Length + 1) * 2);

                    descrs[3].DataPointer = (IntPtr)errorBytes;
                    descrs[3].Size = checked((error.Length + 1) * 2);

                    WriteEventCore(3, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceContextSwitch_Observer_Error => Impl.Instance.IsEnabled(EventLevel.Error, Keywords.ContextSwitch);
        }

        private static string ToTraceString<T>(this T obj) => obj?.ToString();
    }
}



