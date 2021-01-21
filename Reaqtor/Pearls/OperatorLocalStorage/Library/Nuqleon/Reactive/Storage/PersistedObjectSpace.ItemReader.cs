// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.IO;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Implementation of <see cref="IStateReader"/> to support reading a persisted entity's state from its own state partition under the <see cref="OperatorStateItemCategoryPrefix"/> category.
        /// </summary>
        private sealed class ItemReader : IStateReader
        {
            /// <summary>
            /// The state reader obtained from the entity's persisted object space. State for the persisted entity will be read from a partition under the <see cref="OperatorStateItemCategoryPrefix"/>.
            /// </summary>
            private readonly IStateReader _reader;

            /// <summary>
            /// The category prefix used to read state for the entity (see the <c>key</c> parameter in the constructor) from the underlying <see cref="_reader"/>.
            /// </summary>
            /// <example>
            /// The state for a persisted entity with identifier <c>foo</c> will be read from category <c>state/item/foo</c>.
            /// </example>
            private readonly string _prefix;

            /// <summary>
            /// Creates a new item reader to load the persisted entity with the specified <paramref name="key"/> identifier.
            /// </summary>
            /// <param name="reader">The state reader obtained from the entity's persisted object space. State for the persisted entity will be read from a partition under the <see cref="OperatorStateItemCategoryPrefix"/>.</param>
            /// <param name="key">The identifier of the persisted entity to read.</param>
            public ItemReader(IStateReader reader, string key)
            {
                //
                // REVIEW: We don't impose any limits on key (length, characters, etc.). Can we assume the IStateWriter has to deal with this, or should we have our own mapping somewhere (hashing, use of a dictionary, etc.)?
                //

                _reader = reader;
                _prefix = OperatorStateItemCategoryPrefix + key + "/";
            }

            /// <summary>
            /// Disposes the item reader.
            /// </summary>
            public void Dispose() { }

            /// <summary>
            /// Gets a list of categories. Not supported.
            /// </summary>
            /// <returns>A list of categories.</returns>
            /// <exception cref="NotSupportedException">This method is not supported for item readers.</exception>
            public IEnumerable<string> GetCategories() => throw new NotSupportedException();

            /// <summary>
            /// Gets a list of keys for the specified <paramref name="category"/>.
            /// </summary>
            /// <param name="category">The category within the persisted entity's state partition to get the keys for.</param>
            /// <param name="keys">The keys in the specified <paramref name="category"/>.</param>
            /// <returns><c>true</c> if the specified <paramref name="category"/> was found; otherwise, <c>false</c>.</returns>
            public bool TryGetItemKeys(string category, out IEnumerable<string> keys) => _reader.TryGetItemKeys(_prefix + category, out keys);

            /// <summary>
            /// Gets an item reader for the persisted entity state in the specified <paramref name="category"/> and with the specified <paramref name="key"/>.
            /// </summary>
            /// <param name="category">The category of the persisted entity state to read from.</param>
            /// <param name="key">The key of the persisted entity state to read from.</param>
            /// <param name="stream">The <see cref="Stream"/> used to read the item.</param>
            /// <returns><c>true</c> if an item with th specified <paramref name="category"/> and <paramref name="key"/> was found; otherwise, <c>false</c>.</returns>
            public bool TryGetItemReader(string category, string key, out Stream stream) => _reader.TryGetItemReader(_prefix + category, key, out stream);
        }
    }
}
