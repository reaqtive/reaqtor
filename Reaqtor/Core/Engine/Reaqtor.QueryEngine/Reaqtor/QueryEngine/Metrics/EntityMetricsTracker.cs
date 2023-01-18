// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Metrics
{
    /// <summary>
    /// A utility to track metrics for Reactive entities.
    /// </summary>
    public static class EntityMetricsTracker
    {
        /// <summary>
        /// Sets whether or not metrics should be tracked for entities.
        /// </summary>
        public static bool ShouldTrack { get; set; }

        /// <summary>
        /// Start a timer for a given entity and metric.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="metric">The metric.</param>
        internal static void BeginMetric(this IReactiveResource entity, EntityMetric metric)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (ShouldTrack)
            {
                Impl.Instance.BeginMetric(entity, metric);
            }
        }

        /// <summary>
        /// Stops a timer for a given entity and metric, and store the elapsed time.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="metric">The metric.</param>
        internal static void EndMetric(this IReactiveResource entity, EntityMetric metric)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (ShouldTrack)
            {
                Impl.Instance.EndMetric(entity, metric);
            }
        }

        /// <summary>
        /// Creates and starts a new measurement.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="metric">The metric.</param>
        /// <returns>A measurement object that should be disposed to end the measurement.</returns>
        internal static MetricMeasurement Measure(this IReactiveResource entity, EntityMetric metric)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new MetricMeasurement(entity, metric);
        }

        /// <summary>
        /// Set a time span for a given entity and metric.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="metric">The metric.</param>
        /// <param name="elapsed">The time span.</param>
        internal static void SetMetric(this IReactiveResource entity, EntityMetric metric, TimeSpan elapsed)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (elapsed < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(elapsed));
            }

            if (ShouldTrack)
            {
                Impl.Instance.SetMetric(entity, metric, elapsed);
            }
        }

        /// <summary>
        /// Get all the metrics for a given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The set of metrics that have been collected for that entity.</returns>
        public static IReadOnlyDictionary<EntityMetric, TimeSpan> GetMetrics(this IReactiveResource entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return Impl.Instance.GetMetrics(entity);
        }

        internal sealed class Impl
        {
            private const long EmptyMetric = long.MinValue;

            private static readonly Stopwatch s_timer = Stopwatch.StartNew();
            private static readonly int s_metricCount = Enum.GetValues(typeof(EntityMetric)).Cast<int>().Max() + 1;
            private static readonly long[] s_emptyMetrics = Enumerable.Repeat(EmptyMetric, s_metricCount).ToArray();

            private readonly ConditionalWeakTable<IReactiveResource, long[]> _metrics = new();

            private Impl() { }

            public static Impl Instance { get; } = new Impl();

            public void BeginMetric(IReactiveResource entity, EntityMetric metric)
            {
                var entityMetrics = GetEntityMetrics(entity);
                Interlocked.Exchange(ref entityMetrics[(int)metric], -1 * s_timer.Elapsed.Ticks);
            }

            public void EndMetric(IReactiveResource entity, EntityMetric metric)
            {
                var entityMetrics = GetEntityMetrics(entity);
                var idx = (int)metric;

                var startingTicks = entityMetrics[idx];
                if (startingTicks == EmptyMetric)
                {
                    // End before start
                    return;
                }

                if (startingTicks >= 0)
                {
                    // The metric was already set. We can't do anything else.
                    return;
                }

                entityMetrics[idx] = startingTicks + s_timer.Elapsed.Ticks;
            }

            public void SetMetric(IReactiveResource entity, EntityMetric metric, TimeSpan elapsed)
            {
                GetEntityMetrics(entity)[(int)metric] = elapsed.Ticks;
            }

            // PERF: Expose the metrics as a struct that contains a reference to the long[] and provides
            //       dictionary-like APIs to reduce allocations.

            public IReadOnlyDictionary<EntityMetric, TimeSpan> GetMetrics(IReactiveResource entity)
            {
                var entityMetrics = GetEntityMetrics(entity);
                var results = EnumDictionary.Create<EntityMetric, TimeSpan>();
                for (var i = 0; i < s_metricCount; ++i)
                {
                    var metric = entityMetrics[i];
                    if (metric >= 0)
                    {
                        results[(EntityMetric)i] = new TimeSpan(metric);
                    }
                }

                // CONSIDER: Consider whether we need to create a wrapper.

                return new ReadOnlyDictionary<EntityMetric, TimeSpan>(results);
            }

            private long[] GetEntityMetrics(IReactiveResource entity)
            {
                return _metrics.GetValue(entity, _ =>
                {
                    var res = new long[s_metricCount];
                    Array.Copy(s_emptyMetrics, 0, res, 0, s_metricCount);
                    return res;
                });
            }
        }

        /// <summary>
        /// Representation of a running measurement.
        /// </summary>
        internal readonly struct MetricMeasurement : IDisposable, IEquatable<MetricMeasurement>
        {
            private readonly IReactiveResource _entity;
            private readonly EntityMetric _metric;

            /// <summary>
            /// Creates and begins a new measurement.
            /// </summary>
            /// <param name="entity">The entity.</param>
            /// <param name="metric">The metric.</param>
            public MetricMeasurement(IReactiveResource entity, EntityMetric metric)
            {
                _entity = entity;
                _metric = metric;

                entity.BeginMetric(metric);
            }

            /// <summary>
            /// Disposes the measurement, causing it to end.
            /// </summary>
            public void Dispose()
            {
                _entity.EndMetric(_metric);
            }

            public override bool Equals(object obj) => obj is MetricMeasurement m && Equals(m);

            public bool Equals(MetricMeasurement other) => _entity == other._entity && _metric == other._metric;

            public override int GetHashCode() => ((_entity?.GetHashCode() ?? 0) << 16) | (int)_metric;

            public static bool operator ==(MetricMeasurement left, MetricMeasurement right) => left.Equals(right);

            public static bool operator !=(MetricMeasurement left, MetricMeasurement right) => !(left == right);
        }
    }
}
