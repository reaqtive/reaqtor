// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Representation of a creation or deletion operation for an artifact, used in transaction logs.
    /// </summary>
    internal sealed class ArtifactOperation
    {
        private static readonly ArtifactOperation _deleteOperation = new(ArtifactOperationKind.Delete, expression: null, state: null);

        private ArtifactOperation(ArtifactOperationKind operation, Expression expression, object state)
        {
            OperationKind = operation;
            Expression = expression;
            State = state;
        }

        /// <summary>
        /// Gets the expression representing the artifact in case of a creation operation; otherwise, null.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Gets the additional state that's passed to a creation operation; otherwise, null.
        /// </summary>
        public object State { get; }

        /// <summary>
        /// Gets the type of the operation.
        /// </summary>
        public ArtifactOperationKind OperationKind { get; }

        /// <summary>
        /// Creates an operation of type <see cref="ArtifactOperationKind.Create"/>.
        /// </summary>
        /// <param name="expression">The expression used for the creation operation.</param>
        /// <param name="state">The state passed to the creation operation.</param>
        /// <returns>The operation representing the operation.</returns>
        public static ArtifactOperation Create(Expression expression, object state) => new(ArtifactOperationKind.Create, expression, state);

        /// <summary>
        /// Creates an operation of type <see cref="ArtifactOperationKind.Delete"/>.
        /// </summary>
        /// <returns>The operation representing the operation.</returns>
        public static ArtifactOperation Delete() => _deleteOperation;

        /// <summary>
        /// Creates an operation of type <see cref="ArtifactOperationKind.DeleteCreate"/>.
        /// </summary>
        /// <param name="expression">The expression used for the creation operation.</param>
        /// <param name="state">The state passed to the creation operation.</param>
        /// <returns>The operation representing the operation.</returns>
        public static ArtifactOperation DeleteCreate(Expression expression, object state) => new(ArtifactOperationKind.DeleteCreate, expression, state);
    }
}
