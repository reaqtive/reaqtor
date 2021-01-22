// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Runtime.CompilerServices;
using System.Text;

namespace System.Memory
{
    internal static class EntryMetricsExtensions
    {
        private static readonly ConditionalWeakTable<object, EntityMetrics> s_metrics = new();

        public static IWritableMemoizationCacheEntryMetrics GetMetrics(this object entry) => s_metrics.GetOrCreateValue(entry);

        public static void ToDebugView(this IMemoizationCacheEntryMetrics metrics, StringBuilder sb, string indent)
        {
            sb
                .AppendLine(indent + "Invoke duration     = " + metrics.InvokeDuration)
                .AppendLine(indent + "Hit count           = " + metrics.HitCount)
                .AppendLine(indent + "Average access time = " + metrics.AverageAccessTime)
                .AppendLine(indent + "Speed up factor     = " + metrics.SpeedupFactor + (metrics.SpeedupFactor < 1 ? " [DEGRADATION]" : ""));
        }
    }

    internal sealed class EntityMetrics : IWritableMemoizationCacheEntryMetrics
    {
        public TimeSpan CreationTime
        {
            get;
            set;
        }

        public TimeSpan InvokeDuration
        {
            get;
            set;
        }

        public int HitCount
        {
            get;
            set;
        }

        public TimeSpan LastAccessTime
        {
            get;
            set;
        }

        public TimeSpan TotalDuration
        {
            get;
            set;
        }

        public TimeSpan AverageAccessTime => new(TotalDuration.Ticks / HitCount);

        public double SpeedupFactor => (double)InvokeDuration.Ticks / AverageAccessTime.Ticks;
    }
}
