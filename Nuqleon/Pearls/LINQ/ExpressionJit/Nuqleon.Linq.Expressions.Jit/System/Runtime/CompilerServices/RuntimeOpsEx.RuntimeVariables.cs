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

namespace System.Runtime.CompilerServices
{
    public partial class RuntimeOpsEx
    {
        /// <summary>
        /// Implementation of <see cref="IRuntimeVariables"/> providing access to variables through
        /// strong boxes.
        /// </summary>
        private sealed class RuntimeVariables : IRuntimeVariables
        {
            /// <summary>
            /// The strong boxes used as variable storage locations.
            /// </summary>
            private readonly IStrongBox[] _boxes;

            /// <summary>
            /// Creates an <see cref="IRuntimeVariables"/> object providing access to the specified variables.
            /// </summary>
            /// <param name="boxes">The strong boxes used as variable storage locations.</param>
            public RuntimeVariables(IStrongBox[] boxes)
            {
                _boxes = boxes;
            }

            /// <summary>
            /// Gets or sets the variable at the specified index.
            /// </summary>
            /// <param name="index">The index of the variable to get or set.</param>
            /// <returns>The value of the variable at the specified index.</returns>
            public object this[int index]
            {
                get => _boxes[index].Value;
                set => _boxes[index].Value = value;
            }

            /// <summary>
            /// Gets the number of variables.
            /// </summary>
            public int Count => _boxes.Length;
        }
    }
}
