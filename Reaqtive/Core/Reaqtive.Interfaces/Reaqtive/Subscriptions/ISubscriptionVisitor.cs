// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive
{
    /// <summary>
    /// Represents a subscription visitor that visits operator nodes.
    /// </summary>
    public interface ISubscriptionVisitor
    {
        /// <summary>
        /// Visits the specified operator node.
        /// </summary>
        /// <param name="node">Node to apply the visitor to.</param>
        void Visit(IOperator node);
    }
}
