// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Wrote these tests.
//   BD - 07/29/2015 - Added tests for pooling of objects without IFreeable.
//

using System.Memory;

namespace Tests
{
    internal class MyClearable : IClearable
    {
        public MyClearable()
        {
        }

        public bool used;

        public void Use()
        {
            used = true;
        }

        public void Clear()
        {
            used = false;
        }
    }
}
