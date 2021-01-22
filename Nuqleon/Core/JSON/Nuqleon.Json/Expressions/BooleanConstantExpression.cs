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
    /// Expression tree node representing a JSON constant value of type Boolean.
    /// </summary>
    internal sealed class BooleanConstantExpression : ConstantExpression
    {
        /// <summary>
        /// Singleton expression node object for a true value.
        /// </summary>
        internal static readonly ConstantExpression True = new BooleanConstantExpression(true);

        /// <summary>
        /// Singleton expression node object for a false value.
        /// </summary>
        internal static readonly ConstantExpression False = new BooleanConstantExpression(false);

        /// <summary>
        /// Singleton instance of a boxed Boolean false value.
        /// </summary>
        private static readonly object s_false = false;

        /// <summary>
        /// Singleton instance of a boxed Boolean true value.
        /// </summary>
        private static readonly object s_true = true;

        /// <summary>
        /// The value of the Boolean.
        /// </summary>
        private readonly bool _value;

        /// <summary>
        /// Creates a new constant expression tree node with the given Boolean value.
        /// </summary>
        /// <param name="value">Value of the constant.</param>
        internal BooleanConstantExpression(bool value) => _value = value;

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        /// <remarks>
        /// Always returns <see cref="ExpressionType.Boolean"/>.
        /// </remarks>
        public override ExpressionType NodeType => ExpressionType.Boolean;

        /// <summary>
        /// Gets the value of constant represented by this expression tree node.
        /// </summary>
        public override object Value => _value ? s_true : s_false;

        /// <summary>
        /// Returns the JSON fragment corresponding to the expression tree node.
        /// </summary>
        /// <returns>JSON fragment corresponding to the expression tree node.</returns>
        public override string ToString() => _value ? "true" : "false";

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder) => builder.Append(_value ? "true" : "false");
    }
}
