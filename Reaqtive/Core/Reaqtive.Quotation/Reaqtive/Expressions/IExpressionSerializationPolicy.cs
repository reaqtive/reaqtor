// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Interface for policies related to storage of expressions.
    /// </summary>
    /// <remarks>
    /// This interface is intended to be used as infrastructure for a query engine
    /// implementation, there are no guarantees about version compatibility.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExpressionSerializationPolicy
    {
        /// <summary>
        /// Gets the reflection provider to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        IReflectionProvider ReflectionProvider { get; }

        /// <summary>
        /// Gets the expression factory to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        IExpressionFactory ExpressionFactory { get; }

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed lift functions used
        /// by the expression serializer.
        /// </summary>
        IMemoizer LiftMemoizer { get; }

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed reduce functions used
        /// by the expression deserializer.
        /// </summary>
        IMemoizer ReduceMemoizer { get; }
    }
}
