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
    public partial class RuntimeOpsEx
    {
        /// <summary>
        /// Implementation of <see cref="IRuntimeVariables"/> providing access to closed over variables
        /// using indexes that encode parent traversal and slot indexing. 
        /// </summary>
        private sealed class RuntimeVariableList : IRuntimeVariables
        {
            /// <summary>
            /// The closure containing the variables.
            /// </summary>
            private readonly IRuntimeVariables _closure;

            /// <summary>
            /// The indices into the closure where variables are found.
            /// </summary>
            private readonly long[] _indexes;

            /// <summary>
            /// Creates an <see cref="IRuntimeVariables"/> object providing access to closed over variables.
            /// </summary>
            /// <param name="closure">The closure containing the variables.</param>
            /// <param name="indexes">The indices into the closure where variables are found.</param>
            public RuntimeVariableList(IRuntimeVariables closure, long[] indexes)
            {
                _closure = closure;
                _indexes = indexes;
            }

            /// <summary>
            /// Gets or sets the variable at the specified index.
            /// </summary>
            /// <param name="index">The index of the variable to get or set.</param>
            /// <returns>The value of the variable at the specified index.</returns>
            public object this[int index]
            {
                get
                {
                    Resolve(index, out var closure, out var slot);

                    return closure[slot];
                }

                set
                {
                    Resolve(index, out var closure, out var slot);

                    closure[slot] = value;
                }
            }

            /// <summary>
            /// Gets the number of variables.
            /// </summary>
            public int Count => _indexes.Length;

            /// <summary>
            /// Resolves the closure and slot containing the variable given the specified index.
            /// </summary>
            /// <param name="index">The index of the variable in the RuntimeVariables expression.</param>
            /// <param name="closure">The closure containing the storage location of the variable.</param>
            /// <param name="slot">The index of the storage location of the variable in the returned closure.</param>
            private void Resolve(int index, out IRuntimeVariables closure, out int slot)
            {
                //
                // The slot in the indexes table encodes the parent traversal in the upper
                // four bytes, and the index into the closure in the lower four bytes.
                //
                // NB: An IndexOutOfRangeException is thrown if the index does not fall in
                //     the range of variables. This is consistent with LINQ ET behavior.
                //
                var longSlot = _indexes[index];

                var parentCount = (int)(longSlot >> 32);
                slot = (int)longSlot;

                //
                // Traverse the parent chain.
                //
                closure = _closure;

                for (int i = parentCount; i > 0; i--)
                {
                    //
                    // The parent closure is stored in the first slot by convention.
                    //
                    closure = (IRuntimeVariables)closure[0];
                }
            }
        }
    }
}
