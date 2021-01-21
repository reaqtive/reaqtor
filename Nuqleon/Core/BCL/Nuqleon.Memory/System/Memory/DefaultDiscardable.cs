// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 10/27/2014 - Created this type.
//

namespace System.Memory
{
    internal sealed class DefaultDiscardable<T> : IDiscardable<T>
    {
        public static readonly DefaultDiscardable<T> Instance = new();

        private DefaultDiscardable() { }

        public T Value => default;

        public void Dispose()
        {
            // No-op, this will be called many times.
        }
    }
}
