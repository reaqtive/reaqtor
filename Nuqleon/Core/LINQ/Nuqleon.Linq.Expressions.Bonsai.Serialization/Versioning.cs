// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Linq.Expressions.Bonsai.Serialization
{
    /// <summary>
    /// Version numbers for Bonsai expressions.
    /// </summary>
    public static class BonsaiVersion
    {
        /// <summary>
        /// Version 0.8
        /// </summary>
        public static readonly Version V08 = new(0, 8, 0, 0);

        /// <summary>
        /// Version 0.9
        /// </summary>
        public static readonly Version V09 = new(0, 9, 0, 0);

        /// <summary>
        /// Default version
        /// </summary>
        public static readonly Version Default = V09;
    }
}
