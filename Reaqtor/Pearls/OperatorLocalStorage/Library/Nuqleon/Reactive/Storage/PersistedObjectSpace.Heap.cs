// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;
using System.IO;

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    public sealed partial class PersistedObjectSpace
    {
        /// <summary>
        /// Storage entity representing a heap.
        /// </summary>
        /// <remarks>
        /// Persistence of a heap looks as follows:
        /// <c>
        /// items/0 = 42
        /// items/1 = 43
        /// items/2 = 46
        /// items/4 = 44
        /// items/6 = 45
        /// </c>
        /// The keys of the items are insignificant due to the unordered nature of the heap. Holes can exist in the key range (e.g. 3 in the same above).
        /// <list type="bullet">
        ///   <item><c>heap.Add(v)</c> results in <c> Add($"items/{getUnusedKey()}", v)</c></item>
        ///   <item><c>heap.Remove(v)</c> results in <c>Delete($"items/{getKeyOf(v)}")</c></item>
        /// </list>
        /// </remarks>
        private abstract partial class Heap : HeapBase
        {
            /// <summary>
            /// The category to store the data (i.e. the heap elements) in.
            /// </summary>
            private const string ItemsCategory = "items";

            /// <summary>
            /// The list of pairs that associate storage keys to the eventual objects containing the heap elements, set by <see cref="LoadCore(IStateReader)"/> and deserialized by a <c>Restore</c> method in a derived type.
            /// </summary>
            protected List<(long key, EventualObject data)> _data;

            /// <summary>
            /// Creates a new entity representing a heap.
            /// </summary>
            /// <param name="parent">The parent object space, used to access serialization facilities.</param>
            public Heap(PersistedObjectSpace parent) : base(parent)
            {
            }

            /// <summary>
            /// Deletes the heap item with the specified <paramref name="key"/> from storage.
            /// </summary>
            /// <param name="writer">The writer to apply the deletion operation to.</param>
            /// <param name="key">The key of the item to remove.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected override void DeleteItemCore(IStateWriter writer, string key, long keyValue) => writer.DeleteItem(ItemsCategory, key);

            /// <summary>
            /// Gets the list of keys for the items on the heap.
            /// </summary>
            /// <param name="reader">The reader to obtain the list of keys from.</param>
            /// <param name="keys">The keys of the items on the heap.</param>
            /// <returns><c>true</c> if items were found and returned in <paramref name="keys"/>; otherwise, <c>false</c>.</returns>
            protected override bool TryGetItemKeysCore(IStateReader reader, out IEnumerable<string> keys) => reader.TryGetItemKeys(ItemsCategory, out keys);

            /// <summary>
            /// Loads the heap from storage.
            /// </summary>
            /// <param name="reader">The reader to load the heap from.</param>
            protected override void LoadCore(IStateReader reader)
            {
                //
                // Prepare the storage for eventual objects.
                //

                _data = new List<(long key, EventualObject data)>();

                //
                // Perform the core load operations, which will result in calls to TryGetItemKeysCore and LoadItemCore.
                //

                base.LoadCore(reader);

                //
                // We may never be asked to deserialize the heap elements, so let's try to be space efficient.
                //
                // NB: The design assumption right now is that the call to Load is our only chance to read from the key/value store, so we have to keep eventual objects around.
                //     However, the caller of Load could have more awareness of whether the entity will (eventually) be used, thus allowing it to prevent unnecessary loads.
                //

                _data.TrimExcess();
            }

            /// <summary>
            /// Loads the heap item with the specified <paramref name="key"/> from storage.
            /// </summary>
            /// <param name="reader">The reader to load the item from.</param>
            /// <param name="key">The key of the item to load.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected override void LoadItemCore(IStateReader reader, string key, long keyValue)
            {
                //
                // Create an entry in _data containing the key and the eventual object for deferred deserialization in Restore<T>.
                //

                using var itemStream = reader.GetItemReader(ItemsCategory, key);

                var obj = EventualObject.FromState(itemStream);

                _data.Add((keyValue, obj));
            }

            /// <summary>
            /// Saves the heap item with the specified <paramref name="key"/> to storage.
            /// </summary>
            /// <param name="writer">The writer to apply the save operation to.</param>
            /// <param name="key">The key of the item to save.</param>
            /// <param name="keyValue">The numeric representation of <paramref name="key"/>.</param>
            protected override void SaveItemCore(IStateWriter writer, string key, long keyValue)
            {
                //
                // Dispatch through IHeapPersistence.Save to get access to the static type of the heap element to obtain a serializer.
                //
                // REVIEW: Is the performance impact of repeatedly casting the wrapper tolerable?
                //

                var target = (IHeapPersistence)_wrapper;

                using var stream = writer.GetItemWriter(ItemsCategory, key);

                target.Save(SerializationFactory, stream, key);
            }

            /// <summary>
            /// Interface to support virtual dispatch of persistence operations from the weakly typed <see cref="Set"/> storage entity to the statically typed wrapper instance declared by a type derived from <see cref="Heap"/>.
            /// </summary>
            protected interface IHeapPersistence
            {
                /// <summary>
                /// Saves the heap element stored at the specified <paramref name="key"/> to the specified <paramref name="stream"/>.
                /// </summary>
                /// <param name="serializerFactory">The factory to use to obtain a statically typed serializer.</param>
                /// <param name="stream">The stream to save the heap element to.</param>
                /// <param name="key">The key of the heap element to save.</param>
                void Save(ISerializerFactory serializerFactory, Stream stream, string key);
            }
        }
    }
}
