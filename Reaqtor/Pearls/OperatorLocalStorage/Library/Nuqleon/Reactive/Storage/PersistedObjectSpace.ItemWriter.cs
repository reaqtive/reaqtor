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
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Implementation of <see cref="IStateWriter"/> to support writing a persisted entity's state to its own state partition under the <see cref="OperatorStateItemCategoryPrefix"/> category.
        /// </summary>
        private sealed class ItemWriter : IStateWriter
        {
            /// <summary>
            /// The state writer obtained from the entity's persisted object space. State for the persisted entity will be written in a partition under the <see cref="OperatorStateItemCategoryPrefix"/>.
            /// </summary>
            private readonly IStateWriter _writer;

            /// <summary>
            /// The category prefix used to save state for the entity (see the <c>key</c> parameter in the constructor) to the underlying <see cref="_writer"/>.
            /// </summary>
            /// <example>
            /// The state for a persisted entity with identifier <c>foo</c> will be saved under category <c>state/item/foo</c>.
            /// </example>
            private readonly string _prefix;

            /// <summary>
            /// Creates a new item writer to save the persisted entity with the specified <paramref name="key"/> identifier.
            /// </summary>
            /// <param name="writer">The state writer obtained from the entity's persisted object space. State for the persisted entity will be written in a partition under the <see cref="OperatorStateItemCategoryPrefix"/>.</param>
            /// <param name="key">The identifier of the persisted entity to save.</param>
            public ItemWriter(IStateWriter writer, string key)
            {
                //
                // REVIEW: We don't impose any limits on key (length, characters, etc.). Can we assume the IStateWriter has to deal with this, or should we have our own mapping somewhere (hashing, use of a dictionary, etc.)?
                //

                _writer = writer;
                _prefix = OperatorStateItemCategoryPrefix + key + "/";
            }

            /// <summary>
            /// Gets the checkpoint kind from the underlying writer.
            /// </summary>
            public CheckpointKind CheckpointKind => _writer.CheckpointKind;

            /// <summary>
            /// Commits the checkpoint. Not supported.
            /// </summary>
            /// <param name="token">Token to observe to cancel the commit operation.</param>
            /// <param name="progress">Progress object used to report progress of the commit operation.</param>
            /// <returns>Task to indicate the completion of the commit operation.</returns>
            /// <exception cref="NotSupportedException">This method is not supported on <see cref="ItemWriter"/>. The parent state writer (see <see cref="_writer"/>) is used to commit all the state.</exception>
            public Task CommitAsync(CancellationToken token, IProgress<int> progress) => throw new NotSupportedException();

            /// <summary>
            /// Deletes the persisted entity state in the specified <paramref name="category"/> and with the specified <paramref name="key"/>.
            /// </summary>
            /// <param name="category">The category of the persisted entity state to remove.</param>
            /// <param name="key">The key of the persisted entity state to remove.</param>
            public void DeleteItem(string category, string key) => _writer.DeleteItem(_prefix + category, key);

            /// <summary>
            /// Disposes the item writer.
            /// </summary>
            public void Dispose() { }

            /// <summary>
            /// Gets an item writer for the persisted entity state in the specified <paramref name="category"/> and with the specified <paramref name="key"/>.
            /// </summary>
            /// <param name="category">The category of the persisted entity state to write to.</param>
            /// <param name="key">The key of the persisted entity state to write to.</param>
            /// <returns>A <see cref="Stream"/> used to write to the item.</returns>
            public Stream GetItemWriter(string category, string key) => _writer.GetItemWriter(_prefix + category, key);

            /// <summary>
            /// Rolls back the checkpoint. Not supported.
            /// </summary>
            /// <exception cref="NotSupportedException">This method is not supported on <see cref="ItemWriter"/>. The parent state writer (see <see cref="_writer"/>) is used to commit all the state.</exception>
            public void Rollback() => throw new NotSupportedException();
        }
    }
}
