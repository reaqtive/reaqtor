// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Tries to change the static type of the specified <paramref name="expression"/> to the
        /// specified <paramref name="type"/>.
        /// </summary>
        /// <param name="expression">The expression whose static type to change.</param>
        /// <param name="type">The type to change the static type of the expression to.</param>
        /// <param name="result">The resulting expression with the specified static type.</param>
        /// <returns><c>true</c> if a change of type was possible; otherwise, <c>false</c>.</returns>
        protected virtual bool TryChangeType(Expression expression, Type type, out Expression result)
        {
            if (expression.Type == type)
            {
                result = expression;
                return true;
            }

            switch (expression.NodeType)
            {
                case ExpressionType.Block:
                    var block = (BlockExpression)expression;
                    result = Expression.Block(type, block.Variables, block.Expressions);
                    return true;
                case ExpressionType.Conditional:
                    var conditional = (ConditionalExpression)expression;
                    result = Expression.Condition(conditional.Test, conditional.IfTrue, conditional.IfFalse, type);
                    return true;
                case ExpressionType.Constant:
                    if (type == typeof(void))
                    {
                        result = Expression.Empty();
                        return true;
                    }
                    else
                    {
                        var constant = (ConstantExpression)expression;
                        result = Expression.Constant(constant.Value, type);
                        return true;
                    }
                case ExpressionType.Default:
                    result = Expression.Default(type);
                    return true;
                case ExpressionType.Goto:
                    var @goto = (GotoExpression)expression;
                    result = Expression.MakeGoto(@goto.Kind, @goto.Target, @goto.Value, type);
                    return true;
                case ExpressionType.Switch:
                    var @switch = (SwitchExpression)expression;
                    result = Expression.Switch(type, @switch.SwitchValue, @switch.DefaultBody, @switch.Comparison, @switch.Cases);
                    return true;
                case ExpressionType.Throw:
                    var @throw = (UnaryExpression)expression;
                    result = Expression.Throw(@throw.Operand, type);
                    return true;
                case ExpressionType.Try:
                    var @try = (TryExpression)expression;
                    result = Expression.MakeTry(type, @try.Body, @try.Finally, @try.Fault, @try.Handlers);
                    return true;
            }

            if (type == typeof(void))
            {
                // TODO: Check whether this can result in invalid trees, e.g. jump into a block situations?

                result = Expression.Block(typeof(void), expression);
                return true;
            }

            result = Expression.Convert(expression, type);
            return true;
        }

        /// <summary>
        /// Changes the static type of the specified <paramref name="expression"/> to the
        /// specified <paramref name="type"/>.
        /// </summary>
        /// <param name="expression">The expression whose static type to change.</param>
        /// <param name="type">The type to change the static type of the expression to.</param>
        /// <returns>The resulting expression with the specified static type.</returns>
        protected Expression ChangeType(Expression expression, Type type)
        {
            if (!TryChangeType(expression, type, out var res))
            {
                throw new InvalidOperationException($"Can't change the type of expression '{expression}' to type '{type}'.");
            }

            return res;
        }

        /// <summary>
        /// Gets the runtime type of the result of evaluating the specified <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The expression whose runtime type to get.</param>
        /// <returns>The runtime type of the expression, if statically known; otherwise, <c>null</c>.</returns>
        /// <remarks>
        /// This method checks if the specified <paramref name="expression"/> does not throw in order to
        /// determine the runtime type. If an exception can occur during evaluation of the expression,
        /// a <c>null</c> reference is returned.
        /// </remarks>
        protected Type GetRuntimeType(Expression expression)
        {
            if (NeverThrows(expression))
            {
                if (HasConstantValue(expression))
                {
                    var value = GetConstantValue(expression);

                    if (value != null)
                    {
                        return value.GetType();
                    }
                }

                //
                // NB: For these, we know the exact type of the instance being
                //     created (because of `newobj`).
                //

                switch (expression.NodeType)
                {
                    case ExpressionType.New:
                    case ExpressionType.MemberInit:
                    case ExpressionType.ListInit:
                        return expression.Type;
                }

                // NB: Check if we can infer the runtime type from the static
                //     shape of the node. We also need to make sure the operand
                //     can't be null.

                if (HasExactType(expression) && IsNeverNull(expression))
                {
                    return expression.Type;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the specified <paramref name="expression"/>'s static type matches the runtime type when
        /// evaluating the expression.
        /// </summary>
        /// <param name="expression">The expression to check.</param>
        /// <returns><c>true</c> if the static type and the runtime type match; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method can return <c>true</c> even if the specified <paramref name="expression"/> may throw.
        /// </remarks>
        protected virtual bool HasExactType(Expression expression)
        {
            // Nodes where the static type of the node matches the runtime
            // type of the node's evaluation result.

            switch (expression.NodeType)
            {
                // TODO: Consider adding more.

                case ExpressionType.Constant:
                    var constant = (ConstantExpression)expression;
                    return constant.Value?.GetType() == constant.Type;

                case ExpressionType.Default:
                    var @default = (DefaultExpression)expression;
                    return @default.Type.IsValueType && !@default.Type.IsNullableType();

                // `newarr`
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:

                // `newobj`
                case ExpressionType.New:
                case ExpressionType.MemberInit:
                case ExpressionType.ListInit:

                // always returns bool
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:

                // factory calls .GetType() on expression operand
                // REVIEW: changes in .NET Core (returning first available public type)
                case ExpressionType.Quote:
                    return true;
            }

            if (expression.Type.IsValueType && !expression.Type.IsNullableType())
            {
                return true;
            }

            if (expression.Type.IsSealed)
            {
                return true;
            }

            return false;
        }
    }
}
