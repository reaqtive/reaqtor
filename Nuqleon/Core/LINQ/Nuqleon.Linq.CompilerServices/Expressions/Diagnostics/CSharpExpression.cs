// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Information about the representation of an expression tree in C#.
    /// </summary>
    public class CSharpExpression
    {
        internal CSharpExpression(string code, IDictionary<string, ConstantExpression> constants, IList<ParameterExpression> globalVariables)
        {
            Code = code;
            Constants = constants;
            GlobalVariables = globalVariables;
        }

        /// <summary>
        /// Gets the expression tree string representation using C# syntax.
        /// This string is not guaranteed to be semantically equivalent and should be used for diagnostic purposes only.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets a dictionary mapping auto-generated variables names onto constants that occur in the expression tree but cannot be represented using C# literals.
        /// </summary>
        public IDictionary<string, ConstantExpression> Constants { get; }

        /// <summary>
        /// Gets a list of unbound global variables that occur in the expression tree.
        /// </summary>
        public IList<ParameterExpression> GlobalVariables { get; }
    }
}
