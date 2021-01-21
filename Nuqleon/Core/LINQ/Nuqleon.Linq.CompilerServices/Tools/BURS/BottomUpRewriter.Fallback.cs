// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a fallback rule.
    /// </summary>
    /// <typeparam name="TSource">Type of the source tree nodes.</typeparam>
    /// <typeparam name="TTarget">Type of the target tree nodes.</typeparam>
    public sealed class Fallback<TSource, TTarget> : RuleBase<TSource, TTarget>
    {
        internal Fallback(LambdaExpression convert, Func<TSource, TTarget> invokeConvert, LambdaExpression predicate, Func<TSource, bool> invokePredicate, int cost)
            : base(cost)
        {
            Convert = convert;
            InvokeConvert = invokeConvert;
            Predicate = predicate;
            InvokePredicate = invokePredicate;
        }

        /// <summary>
        /// Gets the lambda expression representing the conversion from the remainder source subtree to the target tree node.
        /// </summary>
        public LambdaExpression Convert { get; }

        /// <summary>
        /// Gets the lambda expression representing the predicate to apply to evaluate applicability of the rule to a remainder source subtree.
        /// </summary>
        public LambdaExpression Predicate { get; }

        internal Func<TSource, TTarget> InvokeConvert { get; }

        internal Func<TSource, bool> InvokePredicate { get; }

        /// <summary>
        /// Gets a diagnostic string representation of the fallback rule.
        /// </summary>
        /// <returns>String representation of the fallback rule, used for diagnostic purposes.</returns>
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0}  IF  {1}  ({2}$)", Convert, Predicate, Cost);
    }
}
