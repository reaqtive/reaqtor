// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// (Infrastructure) Represents a wildcard for expression tree matching.
    /// </summary>
    public class ExpressionTreeWildcard : ExpressionTree<ParameterExpression>
    {
        internal ExpressionTreeWildcard(ParameterExpression expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Gets a friendly string representation of the wildcard.
        /// </summary>
        /// <returns>Friendly string representation of the wildcard.</returns>
        public override string ToString() => "(" + Expression.Type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false) + ")*";
    }
}
