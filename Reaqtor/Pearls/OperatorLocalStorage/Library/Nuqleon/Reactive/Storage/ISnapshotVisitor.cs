// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Reaqtive.Storage
{
    /// <summary>
    /// Interface to implement a snapshot visitor passed to the <see cref="ISnapshot{TKey, TValue}.Accept(ISnapshotVisitor{TKey, TValue})"/> method
    /// in order to apply the edits made to the in-memory representation of the key/value store to the underlying persisted key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the key/value store.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the key/value store.</typeparam>
    internal interface ISnapshotVisitor<in TKey, in TValue>
    {
        /// <summary>
        /// Adds or updates the key/value store entry with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the key/value store entry to add or edit.</param>
        /// <param name="value">The value of the key/value store entry to add or edit.</param>
        void AddOrUpdate(TKey key, TValue value);

        /// <summary>
        /// Deletes the key/value store entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the key/value store entry to delete.</param>
        void Delete(TKey key);
    }
}
