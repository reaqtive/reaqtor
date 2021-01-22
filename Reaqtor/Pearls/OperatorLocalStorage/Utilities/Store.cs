// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    /// Key/value store implementation using in-memory dictionaries.
    /// </summary>
    public sealed class Store
    {
        /// <summary>
        /// Gets the dictionary underneath the key/value store implementation. The keys represent categories, the values represent the key/value stores for each category.
        /// </summary>
        public readonly Dictionary<string, Dictionary<string, byte[]>> Data = new();
    }
}
