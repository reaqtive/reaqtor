// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

namespace System.Collections.Specialized
{
    /// <summary>
    /// An interface for a minimal array of bits.
    /// </summary>
    public interface IBitArray
    {
        /// <summary>
        /// Gets the number of bits in the bit array.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets or sets the bit at position index.
        /// </summary>
        /// <param name="index">The index of the bit.</param>
        /// <returns>The bit at position index.</returns>
        bool this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Sets all the bits to value.
        /// </summary>
        /// <param name="value">The value to set all the bits to.</param>
        void SetAll(bool value);
    }
}
