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
    internal interface IByteArray
    {
        int Length { get; }

        byte this[int index]
        {
            get;
            set;
        }
    }
}
