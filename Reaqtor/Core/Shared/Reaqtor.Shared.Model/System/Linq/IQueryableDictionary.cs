// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// Interface for queryable dictionaries, allowing execution of queries against dictionary collections.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">Type of the values in the dictionary.</typeparam>
    public interface IQueryableDictionary<TKey, TValue> : IQueryable<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets the keys in the dictionary.
        /// </summary>
        new IQueryable<TKey> Keys { get; }

        /// <summary>
        /// Gets the values in the dictionary.
        /// </summary>
        new IQueryable<TValue> Values { get; }
    }
}
