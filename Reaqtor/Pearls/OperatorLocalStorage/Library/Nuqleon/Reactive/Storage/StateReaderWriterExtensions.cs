// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.IO;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Provides a set of extesion methods for <see cref="IStateReader"/> and <see cref="IStateWriter"/>.
    /// </summary>
    internal static class StateReaderWriterExtensions
    {
        /// <summary>
        /// Gets an item reader for the specified <paramref name="category"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="reader">The reader to obtain the item reader from.</param>
        /// <param name="category">The category of the item to get a reader for.</param>
        /// <param name="key">The key of the item to get a reader for.</param>
        /// <returns>A <see cref="Stream"/> that can be used to read the item.</returns>
        /// <exception cref="InvalidOperationException">An item with the specified <paramref name="category"/> and <paramref name="key"/> does not exist.</exception>
        public static Stream GetItemReader(this IStateReader reader, string category, string key)
        {
            if (!reader.TryGetItemReader(category, key, out var stream))
            {
                throw new InvalidOperationException(FormattableString.Invariant($"Failed to load '{category}/{key}'."));
            }

            return stream;
        }
    }
}
