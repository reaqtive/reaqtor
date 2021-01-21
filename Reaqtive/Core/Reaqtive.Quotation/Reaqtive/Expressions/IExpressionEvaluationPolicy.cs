// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Interface for policies related to evaluation of expressions.
    /// </summary>
    /// <remarks>
    /// This interface is intended to be used as infrastructure for a query engine
    /// implementation, there are no guarantees about version compatibility.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IExpressionEvaluationPolicy
    {
        /// <summary>
        /// Compiled delegate cache for reuse across expressions.
        /// </summary>
        ICompiledDelegateCache DelegateCache { get; }

        /// <summary>
        /// Cache for in-memory storage of expressions.
        /// </summary>
        ICache<Expression> InMemoryCache { get; }

        /// <summary>
        /// Constant hoister for optimized sharing in expressions and evaluation.
        /// </summary>
        IConstantHoister ConstantHoister { get; }

        /// <summary>
        /// Specifies whether nested lambda expressions should be outlined into
        /// delegate constants by recursive compilation using the cache.
        /// </summary>
        bool OutlineCompilation { get; }
    }
}
