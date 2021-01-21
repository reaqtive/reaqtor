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
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Checks if the specified <paramref name="expression"/> does not have any side-effects for evaluation.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> is considered pure; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Any node for which this method returns <c>true</c> is not allowed to throw a synchronous exception.
        /// </remarks>
        private bool IsPure(Expression expression) => SemanticProvider.IsPure(expression);

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
        private bool IsPure(MemberInfo member) => SemanticProvider.IsPure(member);

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
        private bool HasConstantValue(Expression expression) => SemanticProvider.HasConstantValue(expression);

        /// <summary>
        /// Gets the constant value represented by the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression to get a constant value for.</param>
        /// <returns>The constant value represented by the specified <paramref name="expression"/>.</returns>
        private object GetConstantValue(Expression expression) => SemanticProvider.GetConstantValue(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a null reference.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a null reference; otherwise, <c>false</c>.</returns>
        private bool IsAlwaysNull(Expression expression) => SemanticProvider.IsAlwaysNull(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> is guaranteed to evaluate to a non-null value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression cannot evaluate to null; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Note that this method can return <c>true</c> even if the specified <paramref name="expression"/> may
        /// throw an exception.
        /// </remarks>
        private bool IsNeverNull(Expression expression) => SemanticProvider.IsNeverNull(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents an unconditional throw
        /// of an exception with no other side-effects.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents an unconditional throw; otherwise, <c>false</c>.</returns>
        private bool AlwaysThrows(Expression expression) => SemanticProvider.AlwaysThrows(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents an identity function.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents an identity function; otherwise, <c>false</c>.</returns>
        private bool IsIdentityFunction(Expression expression) => SemanticProvider.IsIdentityFunction(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> can never throw.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> can never throw an exception; otherwise, <c>false</c>.</returns>
        private bool NeverThrows(Expression expression) => SemanticProvider.NeverThrows(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a <c>false</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a <c>false</c> value; otherwise, <c>false</c>.</returns>
        private bool IsFalse(Expression expression) => SemanticProvider.IsFalse(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a <c>true</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a <c>true</c> value; otherwise, <c>false</c>.</returns>
        private bool IsTrue(Expression expression) => SemanticProvider.IsTrue(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a zero (0) value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a zero (0) value; otherwise, <c>false</c>.</returns>
        private bool IsZero(Expression expression) => SemanticProvider.IsZero(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a one (1) value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a one (1) value; otherwise, <c>false</c>.</returns>
        private bool IsOne(Expression expression) => SemanticProvider.IsOne(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a value where all bits are zero (0).
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a value where all bits are zero (0); otherwise, <c>false</c>.</returns>
        private bool AllBitsZero(Expression expression) => SemanticProvider.AllBitsZero(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a value where all bits are one (1).
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a value where all bits are one (1); otherwise, <c>false</c>.</returns>
        private bool AllBitsOne(Expression expression) => SemanticProvider.AllBitsOne(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a minimum value, such as <see cref="int.MinValue"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a minimum value; otherwise, <c>false</c>.</returns>
        private bool IsMinValue(Expression expression) => SemanticProvider.IsMinValue(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a maximum value, such as <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a maximum value; otherwise, <c>false</c>.</returns>
        private bool IsMaxValue(Expression expression) => SemanticProvider.IsMaxValue(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> is pure and can never return a <c>null</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> is pure and can never return a <c>null</c> value; otherwise, <c>false</c>.</returns>
        private bool IsPureNeverNull(Expression expression) => IsPure(expression) && IsNeverNull(expression);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> is pure and can never return a <c>null</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <param name="liftedToNull">Specifies whether the behavior of the parent node is to lift the result to <c>null</c>.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> is pure and can never return a <c>null</c> value; otherwise, <c>false</c>.</returns>
        private bool IsPureNeverNull(Expression expression, bool liftedToNull) => IsPure(expression) && (!liftedToNull || IsNeverNull(expression));

        /// <summary>
        /// Checks if the specified <paramref name="parameter"/> uses the argument in a <c>const</c> (<c>readonly</c>)
        /// fashion, i.e. it doesn't cause any mutation to the argument passed to it.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns><c>true</c> if the specified parameter has <c>const</c> behavior; otherwise, <c>false</c>.</returns>
        private bool IsConst(ParameterInfo parameter) => SemanticProvider.IsConst(parameter);

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> can be constant folded.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression can be constant folded; otherwise, <c>false</c>.</returns>
        private bool CanConstantFold(Expression expression) => IsImmutable(expression) || _isConst;

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> evaluates to an immutable value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression has an immutable value; otherwise, <c>false</c>.</returns>
        private bool IsImmutable(Expression expression) => IsAlwaysNull(expression) || IsImmutable(expression.Type);

        /// <summary>
        /// Checks if instances of the specified <paramref name="type"/> are immutable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type is immutable; otherwise, <c>false</c>.</returns>
        private bool IsImmutable(Type type) => SemanticProvider.IsImmutable(type);
    }
}
