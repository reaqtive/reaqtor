// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Remove Ex suffix. (Infrastructure only.)

using System.ComponentModel;
using System.Linq.Expressions;
using System.Linq.Expressions.Jit;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Contains helper methods called from dynamically generated methods.
    /// </summary>
    public static partial class RuntimeOpsEx
    {
        /// <summary>
        /// Creates an interface that can be used to modify closed over variables at runtime.
        /// </summary>
        /// <param name="closure">The closure containing the variables.</param>
        /// <param name="indexes">An array of indices into the closure where variables are found.</param>
        /// <returns>An interface to access variables.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IRuntimeVariables CreateRuntimeVariables(IRuntimeVariables closure, long[] indexes)
        {
            return new RuntimeVariableList(closure, indexes);
        }

        /// <summary>
        /// Quotes the specified expression at runtime.
        /// </summary>
        /// <param name="expression">The expression to quote.</param>
        /// <param name="hoistedLocals">The hoisted local state provided by the compiler.</param>
        /// <param name="closure">The closure used to access hoisted local variables.</param>
        /// <returns>The quoted expression with hoisted locals bound to their storage locations.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Expression Quote(Expression expression, object hoistedLocals, IRuntimeVariables closure)
        {
            return new ExpressionQuoter((HoistedLocals)hoistedLocals, closure).Visit(expression);
        }
    }
}
