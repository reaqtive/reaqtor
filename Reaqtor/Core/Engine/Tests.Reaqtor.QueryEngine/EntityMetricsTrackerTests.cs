// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Metrics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class EntityMetricsTrackerTests
    {
        private static readonly EntityMetric Metric = EntityMetric.Dispose;
        private static readonly EntityMetric Metric2 = EntityMetric.Evaluate;

        [TestMethod]
        public void EntityMetricsTracker_Test()
        {
            var wasTracking = EntityMetricsTracker.ShouldTrack;
            EntityMetricsTracker.ShouldTrack = true;
            Assert.IsTrue(EntityMetricsTracker.ShouldTrack);
            EntityMetricsTracker.ShouldTrack = wasTracking;
        }

        [TestMethod]
        public void EntityMetricsTracker_ArgumentChecks()
        {
            var resource = default(IReactiveResource);

            Assert.ThrowsException<ArgumentNullException>(() => resource.BeginMetric(Metric));
            Assert.ThrowsException<ArgumentNullException>(() => resource.EndMetric(Metric));
            Assert.ThrowsException<ArgumentNullException>(() => resource.SetMetric(Metric, TimeSpan.Zero));
            Assert.ThrowsException<ArgumentNullException>(() => resource.GetMetrics());
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DummyResource().SetMetric(Metric, TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void EntityMetricsTracker_BeginAndEnd()
        {
            var entity = new DummyResource();
            var stopwatch = Stopwatch.StartNew();
            Begin(entity, Metric);
            End(entity, Metric);
            var elapsed = stopwatch.Elapsed;
            var metrics = Get(entity);
            var metric = metrics[Metric];
            Assert.IsTrue(elapsed > metric);
            Set(entity, Metric, elapsed);
            metrics = Get(entity);
            metric = metrics[Metric];
            Assert.AreEqual(elapsed, metric);
        }

        [TestMethod]
        public void EntityMetricsTracker_Empty()
        {
            var entity = new DummyResource();
            var metrics = Get(entity);
            Assert.AreEqual(0, metrics.Count);
        }

        [TestMethod]
        public void EntityMetricsTracker_NotTracking_Empty()
        {
            var wasTracking = EntityMetricsTracker.ShouldTrack;
            EntityMetricsTracker.ShouldTrack = false;

            var entity = new DummyResource();
            entity.BeginMetric(Metric);
            entity.EndMetric(Metric);
            entity.SetMetric(Metric2, TimeSpan.Zero);
            var metrics = entity.GetMetrics();
            Assert.AreEqual(0, metrics.Count);

            EntityMetricsTracker.ShouldTrack = wasTracking;
        }

        [TestMethod]
        public void EntityMetricsTracker_EndBeforeBegin_Empty()
        {
            var entity = new DummyResource();
            Set(entity, Metric, TimeSpan.Zero);
            End(entity, Metric);
            var metrics = Get(entity);
            Assert.AreEqual(1, metrics.Count);
            Assert.AreEqual(TimeSpan.Zero, metrics[Metric]);
        }

        [TestMethod]
        public void EntityMetricsTracker_BeginWithoutEnd_Empty()
        {
            var entity = new DummyResource();
            Begin(entity, Metric);
            var metrics = Get(entity);
            Assert.AreEqual(0, metrics.Count);
        }

        private static void Begin(IReactiveResource entity, EntityMetric metric)
        {
            EntityMetricsTracker.Impl.Instance.BeginMetric(entity, metric);
        }

        private static void End(IReactiveResource entity, EntityMetric metric)
        {
            EntityMetricsTracker.Impl.Instance.EndMetric(entity, metric);
        }

        private static void Set(IReactiveResource entity, EntityMetric metric, TimeSpan elapsed)
        {
            EntityMetricsTracker.Impl.Instance.SetMetric(entity, metric, elapsed);
        }

        private static IReadOnlyDictionary<EntityMetric, TimeSpan> Get(IReactiveResource entity)
        {
            return EntityMetricsTracker.Impl.Instance.GetMetrics(entity);
        }

        private sealed class DummyResource : IReactiveResource
        {
            public Uri Uri => throw new NotImplementedException();

            public Expression Expression => throw new NotImplementedException();
        }
    }
}
