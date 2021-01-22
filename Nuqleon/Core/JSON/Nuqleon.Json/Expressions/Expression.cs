// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
// BD - June 2014 - Perf optimizations.
//

#pragma warning disable CA1720 // Identifier 'X' contains type name (for Object and String). By design; models JSON.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuqleon.Json.Expressions
{
    using Parser;

    /// <summary>
    /// Base type for JSON expression tree objects.
    /// </summary>
    public abstract partial class Expression
    {
        private static readonly ArrayExpression s_emptyArray = new SimpleArrayExpression(System.Array.Empty<Expression>());

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        public abstract ExpressionType NodeType { get; }

        /// <summary>
        /// Internal constructor to prevent derived types outside this assembly.
        /// </summary>
        internal Expression()
        {
        }

        /// <summary>
        /// Returns the JSON fragment corresponding to the expression tree node.
        /// </summary>
        /// <returns>JSON fragment corresponding to the expression tree node.</returns>
        public override string ToString()
        {
            // PERF: Consider using pooled string builders here.

            var sb = new StringBuilder();

            ToStringCore(sb);

            return sb.ToString();
        }

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        public void ToString(StringBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            ToStringCore(builder);
        }

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal abstract void ToStringCore(StringBuilder builder);

        /// <summary>
        /// Parses the given JSON text and returns an expression tree representing the JSON expression.
        /// </summary>
        /// <param name="input">JSON text to be parsed.</param>
        /// <returns>Expression tree representing the JSON expression.</returns>
        /// <remarks>See RFC 4627 for more information. This parses the production specified in section 2: <code>JSON-text = object / array</code>.</remarks>
        public static Expression Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return JsonParser.Parse(input, ensureTopLevelObjectOrArray: true);
        }

        /// <summary>
        /// Parses the given JSON text and returns an expression tree representing the JSON expression.
        /// </summary>
        /// <param name="input">JSON text to be parsed.</param>
        /// <param name="ensureTopLevelObjectOrArray">Checks whether the top-level JSON text is an object or an array, confirm RFC 4627 for JSON text (true by default).</param>
        /// <returns>Expression tree representing the JSON expression.</returns>
        /// <remarks>See RFC 4627 for more information. This parses the production specified in section 2: <code>JSON-text = object / array</code>.</remarks>
        public static Expression Parse(string input, bool ensureTopLevelObjectOrArray)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return JsonParser.Parse(input, ensureTopLevelObjectOrArray);
        }

        /// <summary>
        /// Creates an expression tree node representing an object.
        /// </summary>
        /// <param name="members">Members of the object, represented as key-value pairs.</param>
        /// <returns>Object expression tree node for the given key-value member pairs.</returns>
        /// <remarks>The order of the members is not defined.</remarks>
        public static ObjectExpression Object(IDictionary<string, Expression> members)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            // PERF: Consider an optimization for the case of an empty dictionary.

            return new ObjectExpression(members);
        }

        /// <summary>
        /// Creates an expression tree node representing an array.
        /// </summary>
        /// <param name="values">Expressions for the array elements.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(IEnumerable<Expression> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return MakeArrayCore(values.ToArray());
        }

        /// <summary>
        /// Creates an expression tree node representing an array.
        /// </summary>
        /// <param name="values">Expressions for the array elements.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(IList<Expression> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return MakeArrayCore(values);
        }

        /// <summary>
        /// Creates an expression tree node representing an array.
        /// </summary>
        /// <param name="values">Expressions for the array elements.</param>
        /// <returns>Array expression tree node for the given elements.</returns>
        public static ArrayExpression Array(params Expression[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return MakeArrayCore(values);
        }

        private static ArrayExpression MakeArrayCore(IList<Expression> values)
        {
            return values.Count switch
            {
                0 => s_emptyArray,
                1 => new ArrayExpression1(values[0]),
                2 => new ArrayExpression2(values[0], values[1]),
                3 => new ArrayExpression3(values[0], values[1], values[2]),
                4 => new ArrayExpression4(values[0], values[1], values[2], values[3]),
                5 => new ArrayExpression5(values[0], values[1], values[2], values[3], values[4]),
                6 => new ArrayExpression6(values[0], values[1], values[2], values[3], values[4], values[5]),
                _ => new SimpleArrayExpression(values),
            };
        }

        private static ArrayExpression MakeArrayCore(Expression[] values)
        {
            // PERF: This method specializes on T[] rather than IList<T> which has some overhead when accessing
            //       an array under the hood.

            return values.Length switch
            {
                0 => s_emptyArray,
                1 => new ArrayExpression1(values[0]),
                2 => new ArrayExpression2(values[0], values[1]),
                3 => new ArrayExpression3(values[0], values[1], values[2]),
                4 => new ArrayExpression4(values[0], values[1], values[2], values[3]),
                5 => new ArrayExpression5(values[0], values[1], values[2], values[3], values[4]),
                6 => new ArrayExpression6(values[0], values[1], values[2], values[3], values[4], values[5]),
                _ => new SimpleArrayExpression(values),
            };
        }

        /// <summary>
        /// Creates an expression tree node representing a constant Boolean value.
        /// </summary>
        /// <param name="value">Boolean value.</param>
        /// <returns>Constant expression tree node for the given Boolean value.</returns>
        public static ConstantExpression Boolean(bool value) => value ? BooleanConstantExpression.True : BooleanConstantExpression.False;

        /// <summary>
        /// Creates an expression tree node representing a constant numeric value.
        /// </summary>
        /// <param name="value">Numeric value textual representation.</param>
        /// <returns>Constant expression tree node for the given number.</returns>
        /// <remarks>JSON numbers don't have a precision limit; we use a string to cover all accepted numeric tokens.</remarks>
        public static ConstantExpression Number(string value)
        {
            if (value == null)
            {
                return NullConstantExpression.Instance;
            }

            if (value.Length == 1)
            {
                var i = value[0] - '0';
                if (i is >= 0 and <= 9)
                {
                    return NumberConstantExpression.Nums[i];
                }
            }

            return new NumberConstantExpression(value);
        }

        /// <summary>
        /// Creates an expression tree node representing a constant string value.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <returns>Constant expression tree node for the given string.</returns>
        public static ConstantExpression String(string value)
        {
            if (value == null)
            {
                return NullConstantExpression.Instance;
            }

            if (value.Length == 0)
            {
                return StringConstantExpression.Empty;
            }

            return new StringConstantExpression(value);
        }

        /// <summary>
        /// Creates an expression tree node representing the null value.
        /// </summary>
        /// <returns>Null value expression tree node.</returns>
        public static ConstantExpression Null() => NullConstantExpression.Instance;
    }
}
