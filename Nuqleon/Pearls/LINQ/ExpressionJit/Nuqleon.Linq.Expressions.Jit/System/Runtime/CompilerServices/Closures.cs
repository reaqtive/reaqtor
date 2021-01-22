// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.ComponentModel;
using System.Diagnostics;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.
    /// Represents the runtime state of a dynamically generated method.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerStepThrough]
    public sealed class Empty : IRuntimeVariables
    {
        /// <summary>
        /// Count of the variables.
        /// </summary>
        int IRuntimeVariables.Count => 0;

        /// <summary>
        /// An indexer to get/set the values of the runtime variables.
        /// </summary>
        /// <param name="index">An index of the runtime variable.</param>
        /// <returns>The value of the runtime variable.</returns>
        object IRuntimeVariables.this[int index]
        {
            get => throw new ArgumentOutOfRangeException(nameof(index));
            set => throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}
