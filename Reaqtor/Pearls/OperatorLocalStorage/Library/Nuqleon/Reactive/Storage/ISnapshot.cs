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
    /// Representation of a snapshot of the edits made to an in-memory representation of a key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the key/value store.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the key/value store.</typeparam>
    /// <remarks>
    /// Snapshots contain a copy of all edit operations performed on the in-memory representation of a key/value store. Upon creating
    /// a snapshot by calling the <see cref="ITransactionalDictionary{TKey, TValue}.CreateSnapshot(bool)"/> method, a new page for
    /// edits is allocated, and the existing pages containing edits are made read-only and get captured by the snapshot. In order to
    /// persist the snapshot to the key/value store, one uses the <see cref="Accept(ISnapshotVisitor{TKey, TValue})"/> method using a
    /// visitor implementation that performs the edits to the store. Upon a successful commit of these edits, the snapshot can be
    /// merged to the in-memory representation of the key/value store, thus reclaiming the edit pages captured by the snapshot. To
    /// mark a snapshot as committed, a call to <see cref="OnCommitted"/> should be made. In case a commit fails, it can either be
    /// retried until success or the snapshot can be discarded, which causes all edits in the snapshot to be part of the subsequent
    /// snapshot. Note that discarding a snapshot introduces some overhead to the in-memory representation of the key/value store
    /// because all read and write operations have to traverse more edit pages.
    /// 
    /// Note that concurrent snapshots are not supported, i.e. all snapshots should be taken and applied sequentially. Attempting to
    /// commit snapshots out of order by calling <see cref="OnCommitted"/> results in undefined behavior. The correct sequence for
    /// snapshot operations is to create a snapshot and apply it to the key/value store before attempting to create the next snapshot.
    /// If a failure results when attempting to apply the snapshot, the snapshot should be discarded. Stated otherwise, once a new
    /// snapshot is created, no further operations on prior snapshots should be performed.
    /// </remarks>
    internal interface ISnapshot<out TKey, out TValue>
    {
        /// <summary>
        /// Dispatches all the edits in the snapshot to the appropriate <paramref name="visitor"/> methods.
        /// </summary>
        /// <param name="visitor">The visitor to dispatch to.</param>
        /// <remarks>
        /// There will be at most one edit per key/value store entry sharing the same key. The order of the edits in a snapshot is
        /// undefined and does not reflect the order in which edits took place. Snapshots reconcile edits to the same entries by
        /// only retaining the last edit for each key.
        /// </remarks>
        void Accept(ISnapshotVisitor<TKey, TValue> visitor);

        /// <summary>
        /// Called after a successful commit to merge the snapshot state to the in-memory representation of the key/value store.
        /// </summary>
        void OnCommitted();

        // REVIEW: Should we add a Rollback method to reconcile edit pages in the transactional dictionary?
    }
}
