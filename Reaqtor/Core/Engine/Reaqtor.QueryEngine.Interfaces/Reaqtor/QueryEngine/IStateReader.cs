// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Provides capabilities to read an engine's state from a store.
    /// </summary>
    public interface IStateReader : IDisposable
    {
        /// <summary>
        /// Gets the list of categories which are part of the state of the engine that was saved in the store.
        /// </summary>
        /// <returns>The list of categories.</returns>
        IEnumerable<string> GetCategories();

        /// <summary>
        /// Gets the list of item keys belonging to the provided category. 
        /// </summary>
        /// <param name="category">Category to retrieve keys for.</param>
        /// <param name="keys">Retrieved keys for the specified category.</param>
        /// <returns><b>true</b> if the category exists; otherwise, <b>false</b>.</returns>
        bool TryGetItemKeys(string category, out IEnumerable<string> keys);

        /// <summary>
        /// Gets a stream to read the state of the item specified by the provided category and key.
        /// </summary>
        /// <param name="category">Category of the item to read state for.</param>
        /// <param name="key">Key of the item to read state for.</param>
        /// <param name="stream">Stream to read that state of the item from.</param>
        /// <returns><b>true</b> if the category and item key exists; otherwise, <b>false</b>.</returns>
        bool TryGetItemReader(string category, string key, out Stream stream);
    }
}
