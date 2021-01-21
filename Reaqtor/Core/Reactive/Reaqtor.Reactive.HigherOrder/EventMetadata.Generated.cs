// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// <WARNING>
//   This file was auto-generated on 01/13/2021 14:59:44.
//   To make changes, edit the .tt file.
// </WARNING>

namespace Reaqtor
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Tracing
    {
        public static void HigherOrderOperator_CreatingBridge(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId, System.Linq.Expressions.Expression definition)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_CreatingBridge;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();
                var arg2 = definition.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 1, "Creating bridge '{0}' for subscription '{1}'. Definition = '{2}'.", arg0, arg1, arg2);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_CreatingBridge(arg0, arg1, arg2);
                }
            }
        }

        public static void HigherOrderOperator_StartingInnerSubscription(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_StartingInnerSubscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 2, "Starting inner subscription to bridge '{0}' for higher order subscription '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_StartingInnerSubscription(arg0, arg1);
                }
            }
        }

        public static void HigherOrderOperator_DisposingInnerSubscription(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_DisposingInnerSubscription;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 3, "Disposing inner subscription to bridge '{0}' for higher order subscription '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_DisposingInnerSubscription(arg0, arg1);
                }
            }
        }

        public static void HigherOrderOperator_DeletingBridge(this TraceSource source, System.Uri bridgeId, System.Uri subscriptionId)
        {
            var shouldUseTraceSource = source != null;
            var shouldUseEventSource = Impl.ShouldTraceHigherOrderOperator_DeletingBridge;

            if (shouldUseTraceSource || shouldUseEventSource)
            {
                var arg0 = bridgeId.ToTraceString();
                var arg1 = subscriptionId.ToTraceString();

                if (shouldUseTraceSource)
                {
                    source.TraceEvent(TraceEventType.Information, 4, "Removing stream for bridge '{0}' for higher order subscription '{1}'.", arg0, arg1);
                }

                if (shouldUseEventSource)
                {
                    Impl.Instance.HigherOrderOperator_DeletingBridge(arg0, arg1);
                }
            }
        }


        [EventSource(Name = "Nuqleon-Reactive-HigherOrder")]
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
                /// Underlying value for keyword HigherOrderOperator
                /// </summary>
                public const EventKeywords HigherOrderOperator = (EventKeywords)2UL;

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
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Creating bridge '{0}' for subscription '{1}'. Definition = '{2}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_CreatingBridge(String bridgeId, String subscriptionId, String definition)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                definition ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                fixed (char* definitionBytes = definition)
                {
                    var dataCount = 3;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    descrs[2].DataPointer = (IntPtr)definitionBytes;
                    descrs[2].Size = checked((definition.Length + 1) * 2);

                    WriteEventCore(1, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_CreatingBridge => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(2,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Starting inner subscription to bridge '{0}' for higher order subscription '{1}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_StartingInnerSubscription(String bridgeId, String subscriptionId)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(2, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_StartingInnerSubscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(3,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Disposing inner subscription to bridge '{0}' for higher order subscription '{1}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_DisposingInnerSubscription(String bridgeId, String subscriptionId)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(3, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_DisposingInnerSubscription => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);

            [Event(4,
                Level = EventLevel.Informational,
                Keywords = Keywords.HigherOrderOperator,
                Opcode = EventOpcode.Info,
                Task = Tasks.None,
                Message = "Removing stream for bridge '{0}' for higher order subscription '{1}'.",
                Version = 1)]
            internal unsafe void HigherOrderOperator_DeletingBridge(String bridgeId, String subscriptionId)
            {
                bridgeId ??= string.Empty;
                subscriptionId ??= string.Empty;
                fixed (char* bridgeIdBytes = bridgeId)
                fixed (char* subscriptionIdBytes = subscriptionId)
                {
                    var dataCount = 2;
                    var descrs = stackalloc EventSource.EventData[dataCount];

                    descrs[0].DataPointer = (IntPtr)bridgeIdBytes;
                    descrs[0].Size = checked((bridgeId.Length + 1) * 2);

                    descrs[1].DataPointer = (IntPtr)subscriptionIdBytes;
                    descrs[1].Size = checked((subscriptionId.Length + 1) * 2);

                    WriteEventCore(4, dataCount, descrs);
                }
            }

            internal static bool ShouldTraceHigherOrderOperator_DeletingBridge => Impl.Instance.IsEnabled(EventLevel.Informational, Keywords.HigherOrderOperator);
        }

        private static string ToTraceString<T>(this T obj) => obj?.ToString();
    }
}



