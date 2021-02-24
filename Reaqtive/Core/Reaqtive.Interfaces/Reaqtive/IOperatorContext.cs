// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Scheduler;
using System;
using System.Diagnostics;

namespace Reaqtive
{
    /// <summary>
    /// This is the context provided to each reactive operator after it is instantiated.
    /// Operators must interact with the environment only through this context to
    /// ensure that they can be properly hosted inside QueryEngines.
    /// </summary>
    public interface IOperatorContext
    {
        /// <summary>
        /// The instance owning the operators. This is the URI of the subscription or the stream.
        /// </summary>
        Uri InstanceId { get; }

        /// <summary>
        /// The scheduler that must be used for any asynchronous processing by the operator.
        /// </summary>
        IScheduler Scheduler { get; }

        /// <summary>
        /// A trace source that can be used by the operator.
        /// </summary>
        TraceSource TraceSource { get; }

        /// <summary>
        /// The execution environment in which the operator is being executed. The operator can
        /// access other runtime artifacts in the execution environment through this interface.
        /// E.g. late binding to subjects by id.
        /// </summary>
        IExecutionEnvironment ExecutionEnvironment { get; }

        /// <summary>
        /// Tries to get an element from the context with the given identifier.
        /// </summary>
        /// <typeparam name="T">The type of the element stored in the context.</typeparam>
        /// <param name="id">The identifier handle for the context element.</param>
        /// <param name="value">Result of obtaining the context element with the specified identifier. If not element is found, this value is left empty.</param>
        /// <returns><c>true</c> if element with given handle exists; otherwise, <c>false</c>.</returns>
        bool TryGetElement<T>(string id, out T value);
    }
}
