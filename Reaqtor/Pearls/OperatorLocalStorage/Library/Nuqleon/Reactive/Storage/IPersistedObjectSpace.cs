// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System; // NB: Used for XML doc comments.
using System.Collections.Generic; // NB: Used for XML doc comments.

namespace Reaqtive.Storage
{
    // REVIEW: Do we want GetOrCreate methods?
    // REVIEW: Should we have a more primitive `T Create<T>(string) where T : IPersisted` interface? Deeper down it'd do a bunch of Activator.CreateInstance type magic though.

    /// <summary>
    /// Interface representing a persisted object space.
    /// </summary>
    public interface IPersistedObjectSpace
    {
        /// <summary>
        /// Creates a persisted array.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the array.</typeparam>
        /// <param name="id">The identifier to use for the array.</param>
        /// <param name="length">The length of the array.</param>
        /// <returns>A new persisted array instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than zero.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedArray<T> CreateArray<T>(string id, int length);

        /// <summary>
        /// Gets a persisted array with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the array.</typeparam>
        /// <param name="id">The identifier of the array to retrieve.</param>
        /// <returns>An existing persisted array instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted array type.</exception>
        IPersistedArray<T> GetArray<T>(string id);

        /// <summary>
        /// Creates a persisted dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
        /// <param name="id">The identifier to use for the dictionary.</param>
        /// <returns>A new persisted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(string id);

        /// <summary>
        /// Gets a persisted dictionary with the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the dictionary.</typeparam>
        /// <param name="id">The identifier of the dictionary to retrieve.</param>
        /// <returns>An existing persisted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted dictionary type.</exception>
        IPersistedDictionary<TKey, TValue> GetDictionary<TKey, TValue>(string id);

        /// <summary>
        /// Creates a persisted linked list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier to use for the linked list.</param>
        /// <returns>A new persisted linked list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedLinkedList<T> CreateLinkedList<T>(string id);

        /// <summary>
        /// Gets a persisted linked list with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the linked list.</typeparam>
        /// <param name="id">The identifier of the linked list to retrieve.</param>
        /// <returns>An existing persisted linked list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted linked list type.</exception>
        IPersistedLinkedList<T> GetLinkedList<T>(string id);

        /// <summary>
        /// Creates a persisted list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="id">The identifier to use for the list.</param>
        /// <returns>A new persisted list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedList<T> CreateList<T>(string id);

        /// <summary>
        /// Gets a persisted list with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="id">The identifier of the list to retrieve.</param>
        /// <returns>An existing persisted list instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted list type.</exception>
        IPersistedList<T> GetList<T>(string id);

        /// <summary>
        /// Creates a persisted queue.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
        /// <param name="id">The identifier to use for the queue.</param>
        /// <returns>A new persisted queue instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedQueue<T> CreateQueue<T>(string id);

        /// <summary>
        /// Gets a persisted queue with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the queue.</typeparam>
        /// <param name="id">The identifier of the queue to retrieve.</param>
        /// <returns>An existing persisted queue instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted queue type.</exception>
        IPersistedQueue<T> GetQueue<T>(string id);

        /// <summary>
        /// Creates a persisted set.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the set.</typeparam>
        /// <param name="id">The identifier to use for the set.</param>
        /// <returns>A new persisted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedSet<T> CreateSet<T>(string id);

        /// <summary>
        /// Gets a persisted set with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the set.</typeparam>
        /// <param name="id">The identifier of the set to retrieve.</param>
        /// <returns>An existing persisted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted set type.</exception>
        IPersistedSet<T> GetSet<T>(string id);

        /// <summary>
        /// Creates a persisted sorted dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier to use for the dictionary.</param>
        /// <returns>A new persisted sorted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedSortedDictionary<TKey, TValue> CreateSortedDictionary<TKey, TValue>(string id);

        /// <summary>
        /// Gets a persisted sorted dictionary with the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of keys stored in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values stored in the sorted dictionary.</typeparam>
        /// <param name="id">The identifier of the sorted dictionary to retrieve.</param>
        /// <returns>An existing persisted sorted dictionary instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted sorted dictionary type.</exception>
        IPersistedSortedDictionary<TKey, TValue> GetSortedDictionary<TKey, TValue>(string id);

        /// <summary>
        /// Creates a persisted sorted set.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier to use for the sorted set.</param>
        /// <returns>A new persisted sorted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedSortedSet<T> CreateSortedSet<T>(string id);

        /// <summary>
        /// Gets a persisted sorted set with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the sorted set.</typeparam>
        /// <param name="id">The identifier of the sorted set to retrieve.</param>
        /// <returns>An existing persisted sorted set instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted sorted set type.</exception>
        IPersistedSortedSet<T> GetSortedSet<T>(string id);

        /// <summary>
        /// Creates a persisted stack.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
        /// <param name="id">The identifier to use for the stack.</param>
        /// <returns>A new persisted stack instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedStack<T> CreateStack<T>(string id);

        /// <summary>
        /// Gets a persisted stack with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the stack.</typeparam>
        /// <param name="id">The identifier of the stack to retrieve.</param>
        /// <returns>An existing persisted stack instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted stack type.</exception>
        IPersistedStack<T> GetStack<T>(string id);

        /// <summary>
        /// Creates a persisted value.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the value.</typeparam>
        /// <param name="id">The identifier to use for the value.</param>
        /// <returns>A new persisted value instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">A persisted object with identifier <paramref name="id"/> already exists.</exception>
        IPersistedValue<T> CreateValue<T>(string id);

        /// <summary>
        /// Gets a persisted value with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the value.</typeparam>
        /// <param name="id">The identifier of the value to retrieve.</param>
        /// <returns>An existing persisted value instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        /// <exception cref="InvalidCastException">A persisted object with identifier <paramref name="id"/> was found but is incompatible with the requested persisted value type.</exception>
        IPersistedValue<T> GetValue<T>(string id);

        /// <summary>
        /// Deletes the persisted object with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the object to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">A persisted object with identifier <paramref name="id"/> could not be found.</exception>
        void Delete(string id);
    }
}
