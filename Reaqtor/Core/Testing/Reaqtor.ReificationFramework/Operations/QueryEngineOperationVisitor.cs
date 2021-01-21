// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Visitor for reified query engine operations.
    /// </summary>
    public class QueryEngineOperationVisitor : QueryEngineOperationVisitor<QueryEngineOperation>
    {
        /// <summary>
        /// Visits a reified query engine operation.
        /// </summary>
        /// <param name="operation">The query engine operation.</param>
        /// <returns>The result of visiting the query engine operation.</returns>
        public override QueryEngineOperation Visit(QueryEngineOperation operation)
        {
            if (operation == null)
            {
                return null;
            }

            return base.Visit(operation);
        }

        /// <summary>
        /// Visits a differential checkpoint operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected override QueryEngineOperation VisitDifferentialCheckpoint(DifferentialCheckpoint operation)
        {
            return operation;
        }

        /// <summary>
        /// Visits a full checkpoint operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected override QueryEngineOperation VisitFullCheckpoint(FullCheckpoint operation)
        {
            return operation;
        }

        /// <summary>
        /// Visits a recovery operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected override QueryEngineOperation VisitRecovery(Recovery operation)
        {
            return operation;
        }

        /// <summary>
        /// Visits an operation of an extension type.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The result of visiting the operation.</returns>
        protected override QueryEngineOperation VisitExtensions(QueryEngineOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}
