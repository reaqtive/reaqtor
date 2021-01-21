// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Interface for policies related to evaluation and storage of expressions.
    /// </summary>
    /// <remarks>
    /// This interface is intended to be used as infrastructure for a query engine
    /// implementation, there are no guarantees about version compatibility.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExpressionPolicy : IExpressionEvaluationPolicy, IExpressionSerializationPolicy
    {
    }
}
