// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Visitor for reified query engine operations.
    /// </summary>
    /// <typeparam name="TResult">The visitor result type.</typeparam>
    public abstract class QueryEngineOperationVisitor<TResult>
    {
        /// <summary>
        /// Visits a reified query engine operation.
        /// </summary>
        /// <param name="operation">The query engine operation.</param>
        /// <returns>The result of visiting the query engine operation.</returns>
        public virtual TResult Visit(QueryEngineOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return operation.Kind switch
            {
                QueryEngineOperationKind.DifferentialCheckpoint => VisitDifferentialCheckpoint((DifferentialCheckpoint)operation),
                QueryEngineOperationKind.FullCheckpoint => VisitFullCheckpoint((FullCheckpoint)operation),
                QueryEngineOperationKind.Recovery => VisitRecovery((Recovery)operation),
                _ => VisitExtensions(operation),
            };
        }

        /// <summary>
        /// Visits a differential checkpoint operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitDifferentialCheckpoint(DifferentialCheckpoint operation);

        /// <summary>
        /// Visits a full checkpoint operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitFullCheckpoint(FullCheckpoint operation);

        /// <summary>
        /// Visits a recovery operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitRecovery(Recovery operation);

        /// <summary>
        /// Visits an operation of an extension type.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected abstract TResult VisitExtensions(QueryEngineOperation operation);
    }
}
