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
    /// Expression tree node representing a JSON constant value of type Null.
    /// </summary>
    internal sealed class NullConstantExpression : ConstantExpression
    {
        /// <summary>
        /// Singleton expression node object for a null value.
        /// </summary>
        internal static readonly ConstantExpression Instance = new NullConstantExpression();

        /// <summary>
        /// Creates a new constant expression tree node with a null value.
        /// </summary>
        private NullConstantExpression()
        {
        }

        /// <summary>
        /// Gets the type of the JSON expression tree node.
        /// </summary>
        /// <remarks>
        /// Always returns <see cref="ExpressionType.Null"/>.
        /// </remarks>
        public override ExpressionType NodeType => ExpressionType.Null;

        /// <summary>
        /// Gets the value of constant represented by this expression tree node.
        /// </summary>
        public override object Value => null;

        /// <summary>
        /// Returns the JSON fragment corresponding to the expression tree node.
        /// </summary>
        /// <returns>JSON fragment corresponding to the expression tree node.</returns>
        public override string ToString() => "null";

        /// <summary>
        /// Appends the JSON fragment corresponding to the expression tree node to the specified string builder.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        internal override void ToStringCore(StringBuilder builder) => builder.Append("null");
    }
}
