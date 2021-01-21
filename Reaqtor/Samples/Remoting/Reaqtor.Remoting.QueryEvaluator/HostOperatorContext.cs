// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Reactive;

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Host-aware operator context.
    /// </summary>
    internal class HostOperatorContext : IHostedOperatorContext
    {
        /// <summary>
        /// Wrapped underlying operator context.
        /// </summary>
        private readonly IHostedOperatorContext _context;

        /// <summary>
        /// Context elements that can be looked up by name.
        /// </summary>
        private readonly IReadOnlyDictionary<string, object> _contextElements;

        /// <summary>
        /// Creates a new host operator context.
        /// </summary>
        /// <param name="context">Underlying operator context to wrap.</param>
        /// <param name="contextElements">Context elements that can be looked up by name.</param>
        /// <param name="traceSource">The trace source.</param>
        public HostOperatorContext(IHostedOperatorContext context, IReadOnlyDictionary<string, object> contextElements, TraceSource traceSource)
        {
            _context = context;
            TraceSource = traceSource;
            _contextElements = contextElements;
        }

        /// <summary>
        /// Gets the subscription URI the context applies to.
        /// </summary>
        public Uri InstanceId => _context.InstanceId;

        /// <summary>
        /// Reactive service exposed by the query engine.
        /// </summary>
        public IReactive ReactiveService => _context.ReactiveService;

        /// <summary>
        /// Scheduler used by the query engine. Subscribables can use this scheduler to schedule work.
        /// </summary>
        public IScheduler Scheduler => _context.Scheduler;

        /// <summary>
        /// TraceSource to log to.
        /// </summary>
        public TraceSource TraceSource { get; }

        /// <summary>
        /// Execution environment for recovery of higher-order operators. 
        /// </summary>
        public IExecutionEnvironment ExecutionEnvironment => _context.ExecutionEnvironment;

        /// <summary>
        /// Try to get an element from the context with the given identifier.
        /// </summary>
        /// <typeparam name="T">The type of the context element.</typeparam>
        /// <param name="id">The identifier handle for the context element.</param>
        /// <returns>The context element with the given identifier handle.</returns>
        /// <returns>True, if element with given handle exists, false otherwise.</returns>
        public bool TryGetElement<T>(string id, out T element)
        {
            if (_contextElements.TryGetValue(id, out var result) && result is T value)
            {
                element = value;
                return true;
            }

            element = default;
            return false;
        }
    }
}
