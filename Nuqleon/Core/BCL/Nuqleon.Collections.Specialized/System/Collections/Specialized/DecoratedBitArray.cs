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
    internal readonly struct DecoratedBitArray : IBitArray
    {
        private readonly BitArray _bitArray;

        public DecoratedBitArray(int size) => _bitArray = new BitArray(size);

        public int Count => _bitArray.Length;

        public bool this[int index]
        {
            get => _bitArray[index];
            set => _bitArray[index] = value;
        }

        public void SetAll(bool value) => _bitArray.SetAll(value);
    }
}
