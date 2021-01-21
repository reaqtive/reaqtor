// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Expression tree node representing a JSON constant value.
    /// </summary>
    public abstract class ConstantExpression : Expression
    {
        /// <summary>
        /// Creates a new constant expression tree node.
        /// </summary>
        internal ConstantExpression()
        {
        }

        /// <summary>
        /// Gets the value of constant represented by this expression tree node.
        /// </summary>
        public abstract object Value { get; }
    }
}
