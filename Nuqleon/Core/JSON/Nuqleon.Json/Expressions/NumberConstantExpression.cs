// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

using System.Text;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Expression tree node representing a JSON constant value of type Number.
    /// </summary>
    internal sealed class NumberConstantExpression : ConstantExpression
    {
        /// <summary>
        /// Singleton expression node objects for commonly used integer numeric values.
        /// </summary>
        internal static readonly ConstantExpression[] Nums = new[]
        {
            new NumberConstantExpression("0"),
            new NumberConstantExpression("1"),
            new NumberConstantExpression("2"),
            new NumberConstantExpression("3"),
            new NumberConstantExpression("4"),
            new NumberConstantExpression("5"),
            new NumberConstantExpression("6"),
            new NumberConstantExpression("7"),
            new NumberConstantExpression("8"),
            new NumberConstantExpression("9"),
        };

        /// <summary>
        /// The value of the Number.
        /// </summary>
        private readonly string _value;

        /// <summary>
        /// Creates a new constant expression tree node with the numeric value.
        /// </summary>
        /// <param name="value">Value of the constant.</param>
        internal NumberConstantExpression(string value) => _value = value;

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        /// <remarks>
        /// Always returns <see cref="ExpressionType.Number"/>.
        /// </remarks>
        public override ExpressionType NodeType => ExpressionType.Number;

        /// <summary>
        /// Gets the value of constant represented by this expression tree node.
        /// </summary>
        public override object Value => _value;

        /// <summary>
        /// Returns the JSON fragment corresponding to the expression tree node.
        /// </summary>
        /// <returns>JSON fragment corresponding to the expression tree node.</returns>
        public override string ToString() => _value;

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder) => builder.Append(_value);
    }
}
