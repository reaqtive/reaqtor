// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal static class CustomMemoryStreamPool
    {
        /// <remarks>
        /// Be sure to set the `maxCapacity` to some value that makes sense for the domain.
        /// </remarks>
        private static readonly MemoryStreamPool s_pool = MemoryStreamPool.Create(1024, 1024, 1024 * 10);

        public static PooledMemoryStream Allocate() => s_pool.Allocate();
    }
}
