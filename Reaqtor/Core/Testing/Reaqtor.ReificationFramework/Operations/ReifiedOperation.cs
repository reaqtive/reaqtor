// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.TestingFramework;
using System;
using System.Runtime.CompilerServices;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// A reified representation of service operations, query engine
    /// operations, and higher order operations thereof.
    /// </summary>
    public class ReifiedOperation
    {
        private static readonly ConditionalWeakTable<object, ReifiedOperation> s_upcasts = new();

        private readonly QueryEngineOperation _qeOp;
        private readonly ServiceOperation _svcOp;

        internal ReifiedOperation(ReifiedOperationKind kind)
        {
            Kind = kind;
        }

        private ReifiedOperation(QueryEngineOperation op)
        {
            _qeOp = op;
            Kind = ReifiedOperationKind.QueryEngineOperation;
        }

        private ReifiedOperation(ServiceOperation op)
        {
            _svcOp = op;
            Kind = ReifiedOperationKind.ServiceOperation;
        }

        /// <summary>
        /// The kind of reified operation.
        /// </summary>
        public ReifiedOperationKind Kind { get; }

        /// <summary>
        /// Converts a query engine operation into a reified operation instance.
        /// </summary>
        /// <param name="op">The query engine operation.</param>
        /// <returns>The reified operation.</returns>
        public static implicit operator ReifiedOperation(QueryEngineOperation op)
        {
            if (op == null)
            {
                return null;
            }

            if (!s_upcasts.TryGetValue(op, out var result))
            {
                lock (op)
                {
                    if (!s_upcasts.TryGetValue(op, out result))
                    {
                        result = new ReifiedOperation(op);
                        s_upcasts.Add(op, result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a query engine operation into a reified operation instance.
        /// </summary>
        /// <returns>The reified operation.</returns>
        public ReifiedOperation ToReifiedOperation() => this;

        /// <summary>
        /// Converts a service operation into a reified operation instance.
        /// </summary>
        /// <param name="op">The service operation.</param>
        /// <returns>The reified operation.</returns>
        public static implicit operator ReifiedOperation(ServiceOperation op)
        {
            if (op == null)
            {
                return null;
            }

            if (!s_upcasts.TryGetValue(op, out var result))
            {
                lock (op)
                {
                    if (!s_upcasts.TryGetValue(op, out result))
                    {
                        result = new ReifiedOperation(op);
                        s_upcasts.Add(op, result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a reified operation to a service operation instance.
        /// </summary>
        /// <param name="op">The reified operation.</param>
        /// <returns>The service operation.</returns>
        /// <exception cref="System.InvalidCastException">
        /// Thrown if the reified operation is not a service operation.
        /// </exception>
        public static explicit operator ServiceOperation(ReifiedOperation op)
        {
            if (op == null)
            {
                return null;
            }

            if (op.Kind != ReifiedOperationKind.ServiceOperation)
            {
                throw new InvalidCastException("Instance is not a ServiceOperation.");
            }

            return op._svcOp;
        }

        /// <summary>
        /// Converts a reified operation to a service operation instance.
        /// </summary>
        /// <returns>The service operation.</returns>
        /// <exception cref="System.InvalidCastException">
        /// Thrown if the reified operation is not a service operation.
        /// </exception>
        public ServiceOperation ToServiceOperation() => (ServiceOperation)this;

        /// <summary>
        /// Converts a reified operation to a query engine operation instance.
        /// </summary>
        /// <param name="op">The reified operation.</param>
        /// <returns>The query engine operation.</returns>
        /// <exception cref="System.InvalidCastException">
        /// Thrown if the reified operation is not a query engine operation.
        /// </exception>
        public static explicit operator QueryEngineOperation(ReifiedOperation op)
        {
            if (op == null)
            {
                return null;
            }

            if (op.Kind != ReifiedOperationKind.QueryEngineOperation)
            {
                throw new InvalidCastException("Instance is not a ServiceOperation.");
            }

            return op._qeOp;
        }

        /// <summary>
        /// Converts a reified operation to a query engine operation instance.
        /// </summary>
        /// <returns>The query engine operation.</returns>
        /// <exception cref="System.InvalidCastException">
        /// Thrown if the reified operation is not a query engine operation.
        /// </exception>
        public QueryEngineOperation ToQueryEngineOperation() => (QueryEngineOperation)this;
    }
}
