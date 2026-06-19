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
    /// Representation of an edit to a key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the key/value store.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the key/value store.</typeparam>
    internal abstract class Edit<TKey, TValue>
    {
        /// <summary>
        /// Dispatches to the <paramref name="visitor"/> method corresponding to the edit type.
        /// </summary>
        /// <param name="visitor">The visitor to dispatch to.</param>
        public abstract void Accept(ISnapshotVisitor<TKey, TValue> visitor);
    }

    /// <summary>
    /// Representation of an addition or edit operation to an entry in a key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the key/value store.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the key/value store.</typeparam>
    internal sealed class AddOrUpdateEdit<TKey, TValue> : Edit<TKey, TValue>
    {
        private readonly TKey _key;
        private readonly TValue _value;

        /// <summary>
        /// Creates a new instance of <see cref="DeleteEdit{TKey, TValue}"/> representing the addition or deletion of a key/value store entry with the specified <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of the key/value store entry to add or edit.</param>
        /// <param name="value">The value of the key/value store entry to add or edit.</param>
        public AddOrUpdateEdit(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// Dispatches to the <see cref="ISnapshotVisitor{TKey, TValue}.AddOrUpdate(TKey, TValue)"/> method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor to dispatch to.</param>
        public override void Accept(ISnapshotVisitor<TKey, TValue> visitor) => visitor.AddOrUpdate(_key, _value);
    }

    /// <summary>
    /// Representation of a deletion operation of an entry in a key/value store.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys stored in the key/value store.</typeparam>
    /// <typeparam name="TValue">The type of the values stored in the key/value store.</typeparam>
    internal sealed class DeleteEdit<TKey, TValue> : Edit<TKey, TValue>
    {
        private readonly TKey _key;

        /// <summary>
        /// Creates a new instance of <see cref="DeleteEdit{TKey, TValue}"/> representing the deletion of a key/value store entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the key/value store entry to delete.</param>
        public DeleteEdit(TKey key)
        {
            _key = key;
        }

        /// <summary>
        /// Dispatches to the <see cref="ISnapshotVisitor{TKey, TValue}.Delete(TKey)"/> method on the <paramref name="visitor"/>.
        /// </summary>
        /// <param name="visitor">The visitor to dispatch to.</param>
        public override void Accept(ISnapshotVisitor<TKey, TValue> visitor) => visitor.Delete(_key);
    }
}
