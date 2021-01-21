// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// <WARNING>
//   This file was auto-generated on 01/13/2021 20:18:04.
//   To make changes, edit the .tt file.
// </WARNING>

namespace Reaqtor.Reliable
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Tracing
    {
        public static void ReliableSubscriptionBase_OnStateSaved(this TraceSource source, System.Uri instanceId, System.Int64 sequenceId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReliableSubscriptionBase_OnStateSaved;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();
                var arg1 = sequenceId;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Verbose, 1, "Reliable subscription input for '{0}' sent AcknowledgeRange({1}).", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ReliableSubscriptionBase_OnStateSaved(arg0, arg1);
                }
            }
        }

        public static void ReliableSubscriptionBase_OnStateSavedDispose(this TraceSource source, System.Uri instanceId, System.Int64 sequenceId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReliableSubscriptionBase_OnStateSavedDispose;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();
                var arg1 = sequenceId;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 2, "Reliable subscription input for '{0}' sent AcknowledgeRange({1}) then Dispose().", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ReliableSubscriptionBase_OnStateSavedDispose(arg0, arg1);
                }
            }
        }

        public static void ReliableSubscriptionBase_OnStateSavedMuted(this TraceSource source, System.Uri instanceId, System.Int64 sequenceId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReliableSubscriptionBase_OnStateSavedMuted;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();
                var arg1 = sequenceId;

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 3, "Reliable subscription input for '{0}' muting AcknowledgeRange({1}) due to disposed state.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ReliableSubscriptionBase_OnStateSavedMuted(arg0, arg1);
                }
            }
        }

        public static void ReliableSubscriptionBase_Stop(this TraceSource source, System.Uri instanceId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceReliableSubscriptionBase_Stop;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = instanceId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 4, "Reliable subscription input for '{0}' sent Stop().", arg0);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.ReliableSubscriptionBase_Stop(arg0);
                }
            }
        }


        [EventSource(Name = "Nuqleon-Reactive-Reliable")]
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
                /// Underlying value for keyword ReliableSubscriptionBase
                /// </summary>
                public const EventKeywords ReliableSubscriptionBase = (EventKeywords)1UL;

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
                Keywords = Keywords.ReliableSubscriptionBase,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reliable subscription input for '{0}' sent AcknowledgeRange({1}).",
                Version = 1)]
            internal unsafe void ReliableSubscriptionBase_OnStateSaved(String instanceId, Int64 sequenceId)
            {
                instanceId ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&sequenceId);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(1, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReliableSubscriptionBase_OnStateSaved => Impl.Instance.IsEnabled(EventLevel.Verbose, Keywords.ReliableSubscriptionBase);

            [Event(2,
                Level = EventLevel.Informational,
                Keywords = Keywords.ReliableSubscriptionBase,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reliable subscription input for '{0}' sent AcknowledgeRange({1}) then Dispose().",
                Version = 1)]
            internal unsafe void ReliableSubscriptionBase_OnStateSavedDispose(String instanceId, Int64 sequenceId)
            {
                instanceId ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&sequenceId);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(2, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReliableSubscriptionBase_OnStateSavedDispose => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.ReliableSubscriptionBase);

            [Event(3,
                Level = EventLevel.Informational,
                Keywords = Keywords.ReliableSubscriptionBase,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reliable subscription input for '{0}' muting AcknowledgeRange({1}) due to disposed state.",
                Version = 1)]
            internal unsafe void ReliableSubscriptionBase_OnStateSavedMuted(String instanceId, Int64 sequenceId)
            {
                instanceId ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)(&sequenceId);
                    descrs[1].Size = sizeof(Int64);

                    WriteEventCore(3, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReliableSubscriptionBase_OnStateSavedMuted => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.ReliableSubscriptionBase);

            [Event(4,
                Level = EventLevel.Informational,
                Keywords = Keywords.ReliableSubscriptionBase,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Reliable subscription input for '{0}' sent Stop().",
                Version = 1)]
            internal unsafe void ReliableSubscriptionBase_Stop(String instanceId)
            {
                instanceId ??= string.Empty;
                fixed (char* instanceIdBytes = instanceId)
                {
                    var dataCount = 1;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)instanceIdBytes;
                    descrs[0].Size = checked((instanceId.Length + 1) * 2);

                    WriteEventCore(4, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceReliableSubscriptionBase_Stop => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.ReliableSubscriptionBase);
        }

        private static string ToTraceString<T>(this T obj) => obj?.ToString();
    }
}



