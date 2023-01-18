// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Added this type to make dictionaries with null entries easier to build.
//

namespace System
{
    internal readonly struct Maybe<T>
    {
        public Maybe(T value) => Value = value;

        public T Value { get; }
    }
}
