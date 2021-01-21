// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Provides capabilities to write an engine's state to a store.
    /// </summary>
    public interface IStateWriter : IDisposable
    {
        /// <summary>
        /// The checkpoint kind.
        /// - For a full checkpoint, all items will be stored.
        /// - For a differential update, only items that have been modified since the last checkpoint will be stored.
        /// </summary>
        CheckpointKind CheckpointKind { get; }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">(Optional) Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the commit operation.</returns>
        /// <remarks>Prior to calling this method, all item writer streams should be disposed.</remarks>
        Task CommitAsync(CancellationToken token, IProgress<int> progress);

        /// <summary>
        /// Discards all changes in the transaction.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Gets a writable stream to store the state for the item with the specified <paramref name="category"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="category">The category of the item to obtain a stream for.</param>
        /// <param name="key">The key of the item to obtain a stream for.</param>
        /// <returns>Stream used to store the state for the specified item.</returns>
        /// <remarks>
        /// Notes for implementers:
        /// - The returned stream should not contain existing item content. Callers should expect a fresh stream to write to regardless of whether the requested item exists.
        /// - Writes to the returned stream should not be committed until a <see cref="CommitAsync"/> operation has been requested.
        /// </remarks>
        Stream GetItemWriter(string category, string key);

        /// <summary>
        /// Delete the item with the specified <paramref name="category"/> and <paramref name="key"/> from the store.
        /// </summary>
        /// <param name="category">The category of the item to delete.</param>
        /// <param name="key">The key of the item to delete.</param>
        /// <remarks>
        /// Notes for implementers:
        /// - If the specified item does not exist, no exception should be thrown. All error reporting should be deferred until the call to <see cref="CommitAsync"/>.
        /// - The deletion of the item should not be committed until a <see cref="CommitAsync"/> operation has been requested.
        /// </remarks>
        void DeleteItem(string category, string key);
    }

    /// <summary>
    /// Extension methods for the IStateWriter interface.
    /// </summary>
    public static class StateWriterExtensions
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="writer">State writer to commit the transaction on.</param>
        /// <returns>Task to observe the eventual completion of the commit operation.</returns>
        /// <remarks>Prior to calling this method, all item writer streams should be disposed.</remarks>
        public static Task CommitAsync(this IStateWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            return writer.CommitAsync(CancellationToken.None, progress: null);
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="writer">State writer to commit the transaction on.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <returns>Task to observe the eventual completion of the commit operation.</returns>
        /// <remarks>Prior to calling this method, all item writer streams should be disposed.</remarks>
        public static Task CommitAsync(this IStateWriter writer, CancellationToken token)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            return writer.CommitAsync(token, progress: null);
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="writer">State writer to commit the transaction on.</param>
        /// <param name="progress">(Optional) Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the commit operation.</returns>
        /// <remarks>Prior to calling this method, all item writer streams should be disposed.</remarks>
        public static Task CommitAsync(this IStateWriter writer, IProgress<int> progress)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            return writer.CommitAsync(CancellationToken.None, progress);
        }
    }
}
