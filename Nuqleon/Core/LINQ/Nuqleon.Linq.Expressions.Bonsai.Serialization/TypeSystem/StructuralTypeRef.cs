// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2014 - Created this file.
//

using System.Diagnostics;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal sealed class StructuralTypeRef : TypeRef
    {
        private int _index;

        public StructuralTypeRef()
        {
            _index = int.MinValue;
        }

        public override int Index
        {
            get
            {
                Debug.Assert(_index != int.MinValue);
                return _index;
            }
        }

        public void SetIndex(int index)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(_index == int.MinValue);
            _index = index;
        }
    }
}
