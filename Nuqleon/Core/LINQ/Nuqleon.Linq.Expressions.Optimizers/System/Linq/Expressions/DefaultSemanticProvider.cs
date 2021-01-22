// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Default implementation of a provider of semantic information about expressions and reflection objects.
    /// </summary>
    public class DefaultSemanticProvider : ISemanticProvider
    {
        /// <summary>
        /// A set of objects that represent integers with a zero (0) value.
        /// </summary>
        private static readonly HashSet<object> s_zeros = new(new object[] { (byte)0, (sbyte)0, (short)0, (ushort)0, 0, (uint)0, (long)0, (ulong)0 });

        /// <summary>
        /// A set of objects that represent integers with a one (1) value.
        /// </summary>
        private static readonly HashSet<object> s_ones = new(new object[] { (byte)1, (sbyte)1, (short)1, (ushort)1, 1, (uint)1, (long)1, (ulong)1 });

        /// <summary>
        /// A set of objects that represent integers with a value whose bits are all zero (0).
        /// </summary>
        private static readonly HashSet<object> s_allBitsZeros = new(new object[] { false, (byte)0, (sbyte)0, (short)0, (ushort)0, 0, (uint)0, (long)0, (ulong)0 });

        /// <summary>
        /// A set of objects that represent integers with a value whose bits are all one (1).
        /// </summary>
        private static readonly HashSet<object> s_allBitsOnes = new(new object[] { true, (byte)0xFF, unchecked((sbyte)0xFF), unchecked((short)0XFFFF), unchecked((ushort)0xFFFF), unchecked((int)0xFFFFFFFF), 0xFFFFFFFF, unchecked((long)0xFFFFFFFFFFFFFFFF), 0xFFFFFFFFFFFFFFFF });

        /// <summary>
        /// A set of objects that represent integers with a minimum values.
        /// </summary>
        private static readonly HashSet<object> s_minValues = new(new object[] { byte.MinValue, sbyte.MinValue, short.MinValue, ushort.MinValue, int.MinValue, uint.MinValue, long.MinValue, ulong.MinValue });

        /// <summary>
        /// A set of objects that represent integers with a maximum values.
        /// </summary>
        private static readonly HashSet<object> s_maxValues = new(new object[] { byte.MaxValue, sbyte.MaxValue, short.MaxValue, ushort.MaxValue, int.MaxValue, uint.MaxValue, long.MaxValue, ulong.MaxValue });

        /// <summary>
        /// Checks if the specified <paramref name="member"/> can never throw.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><c>true</c> if the specified member can never throw an exception; otherwise, <c>false</c>.</returns>
        public virtual bool NeverThrows(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return member.MemberType == MemberTypes.Field;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents an unconditional throw
        /// of an exception with no other side-effects.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents an unconditional throw; otherwise, <c>false</c>.</returns>
        public virtual bool AlwaysThrows(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            switch (expression.NodeType)
            {
                case ExpressionType.Throw:
                    //
                    // NB: We don't deal with rethrow yet in order to avoid trickiness with regards
                    //     to movement of expressions which are context-sensitive. For example, if
                    //     we were to move a rethrow out of its CatchBlock, the resulting tree becomes
                    //     invalid.
                    //
                    var u = (UnaryExpression)expression;
                    return u.Operand != null;
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> can never throw.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> can never throw an exception; otherwise, <c>false</c>.</returns>
        public virtual bool NeverThrows(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            // Nodes that are guaranteed to never throw. Note that some
            // rely on runtime infrastructure which we assume to be free
            // of exceptions.

            // NB: Quote is omitted; it can reduce extension nodes
            //     which involves running user code.

            return expression.NodeType
                is ExpressionType.Constant
                or ExpressionType.Default
                or ExpressionType.Lambda
                or ExpressionType.Parameter
                or ExpressionType.RuntimeVariables;
        }

        /// <summary>
        /// Gets the constant value represented by the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression to get a constant value for.</param>
        /// <returns>The constant value represented by the specified <paramref name="expression"/>.</returns>
        public virtual object GetConstantValue(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return expression.NodeType switch
            {
                ExpressionType.Constant => ((ConstantExpression)expression).Value,
                ExpressionType.Default => GetDefaultValue(expression.Type),
                // REVIEW: See HasConstantValue; when is this safe to do?
                //ExpressionType.New => GetDefaultValue(expression.Type),
                _ => throw new InvalidOperationException($"The expression '{expression}' does not represent a constant value."),
            };
        }

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
        public virtual bool HasConstantValue(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            //
            // REVIEW: when is this safe to do?
            //
            //  case ExpressionType.New:
            //      return ((NewExpression)expression).Constructor == null;
            //

            return expression.NodeType
                is ExpressionType.Constant
                or ExpressionType.Default;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a null reference.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a null reference; otherwise, <c>false</c>.</returns>
        public virtual bool IsAlwaysNull(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return HasConstantValue(expression) && GetConstantValue(expression) == null;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> is guaranteed to evaluate to a non-null value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression cannot evaluate to null; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Note that this method can return <c>true</c> even if the specified <paramref name="expression"/> may
        /// throw an exception.
        /// </remarks>
        public virtual bool IsNeverNull(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            // Nodes whose outcome is guaranteed to be non-null.

            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return ((ConstantExpression)expression).Value != null;

                case ExpressionType.New:
                    var @new = (NewExpression)expression;
                    return !@new.Type.IsNullableType() || @new.Constructor != null /* new Nullable<T>(T value) */;

                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                case ExpressionType.MemberInit:
                case ExpressionType.ListInit:
                case ExpressionType.Lambda:
                case ExpressionType.Quote:
                case ExpressionType.RuntimeVariables:
                    return true;
            }

            if (expression.Type.IsValueType && !expression.Type.IsNullableType())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> does not have any side-effects for evaluation.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="expression"/> is considered pure; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Any node for which this method returns <c>true</c> is not allowed to throw a synchronous exception.
        /// </remarks>
        public virtual bool IsPure(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            // NB: Omitting Quote because it can result in the evaluation of extension node
            //     reduction at runtime.

            return expression.NodeType
                is ExpressionType.Constant
                or ExpressionType.Default
                or ExpressionType.Lambda
                or ExpressionType.Parameter;
        }

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
        public virtual bool IsPure(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return false;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents an identity function.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents an identity function; otherwise, <c>false</c>.</returns>
        public virtual bool IsIdentityFunction(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression is LambdaExpression lambda)
            {
                if (lambda.Parameters.Count == 1)
                {
                    var invoke = lambda.Type.GetMethod("Invoke");

                    if (invoke.ReturnType == invoke.GetParameters()[0].ParameterType)
                    {
                        var parameter = lambda.Parameters[0];

                        return lambda.Body == parameter;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a <c>false</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a <c>false</c> value; otherwise, <c>false</c>.</returns>
        public virtual bool IsFalse(Expression expression) => HasConstantValue(expression) && false.Equals(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a <c>true</c> value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a <c>true</c> value; otherwise, <c>false</c>.</returns>
        public virtual bool IsTrue(Expression expression) => HasConstantValue(expression) && true.Equals(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a zero (0) value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a zero (0) value; otherwise, <c>false</c>.</returns>
        public virtual bool IsZero(Expression expression) => HasConstantValue(expression) && s_zeros.Contains(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a one (1) value.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a one (1) value; otherwise, <c>false</c>.</returns>
        public virtual bool IsOne(Expression expression) => HasConstantValue(expression) && s_ones.Contains(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a value where all bits are zero (0).
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a value where all bits are zero (0); otherwise, <c>false</c>.</returns>
        public virtual bool AllBitsZero(Expression expression) => HasConstantValue(expression) && s_allBitsZeros.Contains(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a value where all bits are one (1).
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a value where all bits are one (1); otherwise, <c>false</c>.</returns>
        public virtual bool AllBitsOne(Expression expression) => HasConstantValue(expression) && s_allBitsOnes.Contains(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a minimum value, such as <see cref="int.MinValue"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a minimum value; otherwise, <c>false</c>.</returns>
        public virtual bool IsMinValue(Expression expression) => HasConstantValue(expression) && s_minValues.Contains(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="expression"/> represents a maximum value, such as <see cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the specified expression represents a maximum value; otherwise, <c>false</c>.</returns>
        public virtual bool IsMaxValue(Expression expression) => HasConstantValue(expression) && s_maxValues.Contains(GetConstantValue(expression));

        /// <summary>
        /// Checks if the specified <paramref name="parameter"/> uses the argument in a <c>const</c> (<c>readonly</c>)
        /// fashion, i.e. it doesn't cause any mutation to the argument passed to it.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns><c>true</c> if the specified parameter has <c>const</c> behavior; otherwise, <c>false</c>.</returns>
        public virtual bool IsConst(ParameterInfo parameter) => false;

        /// <summary>
        /// Checks if instances of the specified <paramref name="type"/> are immutable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type is immutable; otherwise, <c>false</c>.</returns>
        public virtual bool IsImmutable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type == typeof(void))
            {
                return true;
            }

            if (type.IsNullableType())
            {
                return true;
            }

            return Type.GetTypeCode(type.GetNonNullableType())
                is TypeCode.Boolean
                or TypeCode.Char
                or TypeCode.SByte
                or TypeCode.Byte
                or TypeCode.Int16
                or TypeCode.UInt16
                or TypeCode.Int32
                or TypeCode.UInt32
                or TypeCode.Int64
                or TypeCode.UInt64
                or TypeCode.Single
                or TypeCode.Double
                or TypeCode.Decimal
                or TypeCode.DateTime
                or TypeCode.String;
        }

        /// <summary>
        /// Gets the default value of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the default value for.</param>
        /// <returns>The default value of the specified <paramref name="type"/>.</returns>
        private static object GetDefaultValue(Type type)
        {
            if (!type.IsValueType || type.IsNullableType())
            {
                return null;
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}
