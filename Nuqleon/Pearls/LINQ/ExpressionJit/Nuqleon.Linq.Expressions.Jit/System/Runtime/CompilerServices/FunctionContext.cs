// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Represents the context in which a function executes by providing access to the closure object.
    /// </summary>
    /// <typeparam name="TClosure">The type of the closure object.</typeparam>
    public class FunctionContext<TClosure>
    {
#pragma warning disable CA1051 // Do not declare visible instance field. (Usage of field is by design.)

        /// <summary>
        /// The closure passed to the function as the first parameter.
        /// </summary>
        public TClosure Closure;

#pragma warning restore CA1051
    }
}
