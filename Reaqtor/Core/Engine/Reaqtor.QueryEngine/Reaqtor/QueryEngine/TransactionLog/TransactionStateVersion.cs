// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Versions used for persistence of transaction log records.
    /// </summary>
    internal static class TransactionStateVersion
    {
        /// <summary>
        /// Version 1.0.0.0 (current).
        /// </summary>
        public static readonly Version v1 = new(1, 0, 0, 0);
    }
}
