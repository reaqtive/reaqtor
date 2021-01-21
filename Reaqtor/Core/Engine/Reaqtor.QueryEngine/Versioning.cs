// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor
{
    /// <summary>
    /// Exposed version numbers of operator state.
    /// </summary>
    internal static class Versioning
    {
        /// <summary>
        /// Version 1.0 of operator state.
        /// </summary>
        public static readonly Version v1 = new(1, 0, 0, 0);

        /// <summary>
        /// Version 2.0 of operator state.
        /// </summary>
        public static readonly Version v2 = new(2, 0, 0, 0);

        /// <summary>
        /// Version 3.0 of operator state.
        /// </summary>
        public static readonly Version v3 = new(3, 0, 0, 0);
    }
}
