// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Jit
{
    /// <summary>
    /// Struct representing the result of preparing a lambda expression for
    /// JIT compilation support using <see cref="JitCompiler.Prepare"/>.
    /// </summary>
    internal struct JitCompilationInfo
    {
        /// <summary>
        /// The parameter expression representing the method table parameter
        /// passed as the first argument to the top-level lambda. References
        /// to this parameter will occur in <see cref="Expression"/>. The
        /// caller should construct a lambda expression using this parameter
        /// in order to supply the <see cref="MethodTable"/> instance to the
        /// compiled top-level lambda expression.
        /// </summary>
        public ParameterExpression MethodTableParameter;

        /// <summary>
        /// The rewritten expression representing the top-level lambda with
        /// JIT compilation support for all inner lambdas. This expression
        /// will contain references to <see cref="MethodTableParameter"/> in
        /// order to supply the runtime <see cref="MethodTable"/> instance
        /// to the compiled expression to retrieve the inner lambda thunks.
        /// </summary>
        public Expression Expression;

        /// <summary>
        /// The method table instance containing the thunks for the inner
        /// lambdas to provide JIT compilation support. This instance should
        /// be passed to the compiled delegate obtained from building a lambda
        /// expression the parameterizes the <see cref="Expression"/> on the
        /// <see cref="MethodTableParameter"/>.
        /// </summary>
        public MethodTable MethodTable;
    }
}
