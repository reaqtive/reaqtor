// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET5_0

//
// NB: These are stubs on .NET 5.0 because these APIs are no longer public. We could reimplement RuntimeVariables support
//     entirely if we wish to do so.
//

using System.ComponentModel;
using System.Diagnostics;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Contains helper methods called from dynamically generated methods.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public static partial class RuntimeOps
    {
        /// <summary>
        /// Creates an interface that can be used to modify closed over variables at runtime.
        /// </summary>
        /// <param name="data">The closure array.</param>
        /// <param name="indexes">An array of indexes into the closure array where variables are found.</param>
        /// <returns>An interface to access variables.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static IRuntimeVariables CreateRuntimeVariables(object[] data, long[] indexes)
        {
            throw new NotSupportedException("Currently not supported on .NET 5.0.");
        }

        /// <summary>
        /// Creates an interface that can be used to modify closed over variables at runtime.
        /// </summary>
        /// <returns>An interface to access variables.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static IRuntimeVariables CreateRuntimeVariables()
        {
            throw new NotSupportedException("Currently not supported on .NET 5.0.");
        }

        /// <summary>
        /// Combines two runtime variable lists and returns a new list.
        /// </summary>
        /// <param name="first">The first list.</param>
        /// <param name="second">The second list.</param>
        /// <param name="indexes">The index array indicating which list to get variables from.</param>
        /// <returns>The merged runtime variables.</returns>
        [Obsolete("do not use this method", true), EditorBrowsable(EditorBrowsableState.Never)]
        public static IRuntimeVariables MergeRuntimeVariables(IRuntimeVariables first, IRuntimeVariables second, int[] indexes)
        {
            throw new NotSupportedException("Currently not supported on .NET 5.0.");
        }
    }
}

#endif
