// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Interface representing a provider of semantic information about expressions and reflection objects.
    /// </summary>
    public interface ISemanticProvider
    {
        /// <summary>
        /// Gets the constant value represented by the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression to get a constant value for.</param>
        /// <returns>The constant value represented by the specified <paramref name="expression"/>.</returns>
        object GetConstantValue(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a constant value.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> represents a constant value; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// If this method returns <c>true</c> for a given expression, a subsequent call to
        /// <see cref="GetConstantValue"/> should be able to obtain the constant value that's
        /// represented by the expression.
        /// </remarks>
        bool HasConstantValue(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="member"/> is pure and doesn't have any side-effects for
        /// evaluation.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="member"/> is considered pure; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Pure members can be evaluated at compile time if all operands passed to them are constant values
        /// that can be obtained at runtime. The member is allowed to throw an exception (provided this behavior
        /// is deterministic), causing the expression optimizer to either suppress optimizing the node or to
        /// replace the subexpression with a <see cref="ExpressionType.Throw"/> node.
        ///
        /// Note that members should not be reported as pure if their result is a mutable object. All evaluations
        /// of the optimized tree containing such a member would share the resulting object.
        /// </remarks>
        bool IsPure(MemberInfo member);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> does not have any side-effects for evaluation.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> is considered pure; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Any node for which this method returns <c>true</c> is not allowed to throw a synchronous exception.
        /// </remarks>
        bool IsPure(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents an unconditional throw
        /// of an exception with no other side-effects.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents an unconditional throw; otherwise, <c>false</c>.</returns>
        bool AlwaysThrows(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> can never throw.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> can never throw an exception; otherwise, <c>false</c>.</returns>
        bool NeverThrows(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="member"/> can never throw.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><c>true</c> if the specified member can never throw an exception; otherwise, <c>false</c>.</returns>
        bool NeverThrows(MemberInfo member);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a null reference.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a null reference; otherwise, <c>false</c>.</returns>
        bool IsAlwaysNull(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> is guaranteed to evaluate to a non-null value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression cannot evaluate to null; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Note that this method can return <c>true</c> even if the specified <paramref name="expression"/> may
        /// throw an exception.
        /// </remarks>
        bool IsNeverNull(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents an identity function.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents an identity function; otherwise, <c>false</c>.</returns>
        bool IsIdentityFunction(Expression expression);

        // REVIEW: Should we make some of these extension methods (non-virtual!) that simply rely on HasConstantValue
        //         to perform the necessary checks?

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a <c>false</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a <c>false</c> value; otherwise, <c>false</c>.</returns>
        bool IsFalse(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a <c>true</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a <c>true</c> value; otherwise, <c>false</c>.</returns>
        bool IsTrue(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a zero (0) value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a zero (0) value; otherwise, <c>false</c>.</returns>
        bool IsZero(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a one (1) value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a one (1) value; otherwise, <c>false</c>.</returns>
        bool IsOne(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a value where all bits are zero (0).
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a value where all bits are zero (0); otherwise, <c>false</c>.</returns>
        bool AllBitsZero(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a value where all bits are one (1).
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a value where all bits are one (1); otherwise, <c>false</c>.</returns>
        bool AllBitsOne(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a minimum value, such as <see cref="int.MinValue"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a minimum value; otherwise, <c>false</c>.</returns>
        bool IsMinValue(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a maximum value, such as <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a maximum value; otherwise, <c>false</c>.</returns>
        bool IsMaxValue(Expression expression);

        /// <summary>
        /// Checks if the specified <paramref name="parameter"/> uses the argument in a <c>const</c> (<c>readonly</c>)
        /// fashion, i.e. it doesn't cause any mutation to the argument passed to it.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns><c>true</c> if the specified parameter has <c>const</c> behavior; otherwise, <c>false</c>.</returns>
        bool IsConst(ParameterInfo parameter);

        /// <summary>
        /// Checks if instances of the specified <paramref name="type"/> are immutable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type is immutable; otherwise, <c>false</c>.</returns>
        bool IsImmutable(Type type);
    }
}
