// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine.Metrics
{
    /// <summary>
    /// The set of entity metrics.
    /// </summary>
    public enum EntityMetric : int
    {
        /// <summary>
        /// Unknown metric.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Metric for reading entity from state reader.
        /// </summary>
        ReadEntity = 1,

        /// <summary>
        /// Metric for loading entity, including deserializing expression.
        /// </summary>
        LoadEntity = 2,

        /// <summary>
        /// Metric for saving entity, including serializing expression.
        /// </summary>
        SaveEntity = 3,

        /// <summary>
        /// Metric for reading runtime entity state from state reader.
        /// </summary>
        ReadState = 4,

        /// <summary>
        /// Metric for loading the runtime state of an entity.
        /// </summary>
        LoadState = 5,

        /// <summary>
        /// Metric for saving the runtime state of an entity.
        /// </summary>
        SaveState = 6,

        /// <summary>
        /// Metric for inlining the definitions of operators in an entity.
        /// </summary>
        Inline = 7,

        /// <summary>
        /// Metric for templatizing an expression for an entity.
        /// </summary>
        Templatize = 8,

        /// <summary>
        /// Metric for evaluating the expression of an entity.
        /// </summary>
        Evaluate = 9,

        /// <summary>
        /// Metric for wiring up the operator tree for an entity.
        /// </summary>
        Subscribe = 10,

        /// <summary>
        /// Metric for setting the operator context for an entity.
        /// </summary>
        SetContext = 11,

        /// <summary>
        /// Metric for starting a runtime entity.
        /// </summary>
        Start = 12,

        /// <summary>
        /// Metric for disposing a runtime entity.
        /// </summary>
        Dispose = 13,

        /// <summary>
        /// Metric for unloading an entity.
        /// </summary>
        Unload = 14,

        /// <summary>
        /// Metric for delegation in an expression for an entity.
        /// </summary>
        Delegate = 15,
    }
}
