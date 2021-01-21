// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - October 2013 - Created this file.
//

using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of extension methods for expressions.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Returns a friendly string representation of the expression for use in tracing.
        /// </summary>
        /// <param name="expression">Expression to provide a string representation for.</param>
        /// <returns>Friendly string representation of the expression for use in tracing.</returns>
        public static string ToTraceString(this Expression expression)
        {
            if (expression == null)
            {
                return "(null)";
            }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (See remarks.)

            try
            {
                return expression.ToCSharpString(allowCompilerGeneratedNames: true); // TODO: try helper
            }
            catch
            {
                // NB: Deals with some unsupported nodes to avoid taking down the host. To be addressed in C#.
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079

            return expression.ToString();
        }
    }
}
