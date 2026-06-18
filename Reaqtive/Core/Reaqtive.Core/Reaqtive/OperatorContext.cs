// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive.Scheduler;

namespace Reaqtive
{
    /// <summary>
    /// Operator context.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OperatorContext"/> class.
    /// </remarks>
    /// <param name="instanceId">The id (URI) of the instance owning the operator.</param>
    /// <param name="scheduler">The scheduler to perform asynchronous activities on.</param>
    /// <param name="traceSource">Trace source to log operator diagnostic information to.</param>
    /// <param name="executionEnvironment">The execution environment where the operator is executing in.</param>
    public class OperatorContext(Uri instanceId, IScheduler scheduler, TraceSource traceSource = null, IExecutionEnvironment executionEnvironment = null) : IOperatorContext
    {

        /// <summary>
        /// Gets the id (URI) of the instance owning the operator.
        /// </summary>
        public Uri InstanceId { get; } = instanceId ?? throw new ArgumentNullException(nameof(instanceId));

        /// <summary>
        /// Gets the scheduler to perform asynchronous activities on.
        /// </summary>
        public IScheduler Scheduler { get; } = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

        /// <summary>
        /// Gets the trace source to log operator diagnostic information to.
        /// </summary>
        public TraceSource TraceSource { get; } = traceSource;

        /// <summary>
        /// Gets the execution environment where the operator is executing in.
        /// </summary>
        public IExecutionEnvironment ExecutionEnvironment { get; } = executionEnvironment;

        /// <summary>
        /// Tries to get an element from the context with the given identifier.
        /// </summary>
        /// <typeparam name="T">The type of the element stored in the context.</typeparam>
        /// <param name="id">The identifier handle for the context element.</param>
        /// <param name="value">Result of obtaining the context element with the specified identifier. If not element is found, this value is left empty.</param>
        /// <returns><c>true</c> if element with given handle exists; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetElement<T>(string id, out T value)
        {
            value = default;
            return false;
        }
    }
}
