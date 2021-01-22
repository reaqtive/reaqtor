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

using Reaqtive.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtive.Storage
{
    /// <summary>
    /// Represents a container for persisted objects.
    /// </summary>
    public sealed partial class PersistedObjectSpace : IPersistedObjectSpace
    {
        /// <summary>
        /// The category used in the persisted key/value store to keep the index of the persisted entities in the object space.
        /// </summary>
        /// <remarks>
        /// Each item in this category represents a persisted entity. The key represents the identifier, the value contains the serialized <see cref="Descriptor"/>.
        /// </remarks>
        private const string OperatorStateIndexCategory = "state/index";

        /// <summary>
        /// The category used in the persisted key/value store to keep the state for persisted entities. The persisted entity's key is appended to this prefix to obtain the category used to store all of the state of the entity.
        /// </summary>
        /// <example>
        /// A persisted entity with identifier <c>foo</c> will contain the following state:
        /// <code>
        /// state/index/foo -> descriptor
        /// state/item/foo/* -> state
        /// </code>
        /// </example>
        private const string OperatorStateItemCategoryPrefix = "state/item/";

        /// <summary>
        /// The serialization factory to use to serialize and deserialize state.
        /// </summary>
        private readonly ISerializationFactory _serializationFactory;

        /// <summary>
        /// The registry holding the persisted entities in this persisted object space.
        /// </summary>
        private readonly Registry _items;

        /// <summary>
        /// Creates a new persisted object space using the specified serialization factory used to serialize and deserialize state.
        /// </summary>
        /// <param name="serializationFactory">The serialization factory to use to serialize and deserialize state.</param>
        public PersistedObjectSpace(ISerializationFactory serializationFactory)
        {
            _serializationFactory = serializationFactory ?? throw new ArgumentNullException(nameof(serializationFactory));
            _items = new Registry(this);
        }

        /// <summary>
        /// Deletes the persisted object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        public void Delete(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (!_items.Remove(id))
                throw new KeyNotFoundException();
        }

        /// <summary>
        /// Loads the persisted object space from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader to load the persisted object space from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is <c>null.</c></exception>
        public void Load(IStateReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _items.Load(reader);
        }

        /// <summary>
        /// Saves the persisted object space to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to save the persisted object space to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <c>null.</c></exception>
        public void Save(IStateWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _items.Save(writer);
        }

        /// <summary>
        /// Called when saving the persisted object space (see <see cref="Save"/>) succeeded.
        /// </summary>
        public void OnSaved()
        {
            _items.OnSaved();
        }
    }
}
