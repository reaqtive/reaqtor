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
    // REVIEW: Consider using an explicit catalog of identifier-to-type mappings (in lieu of an enum) to support both `T Create<T>(string) where T : IPersisted` activator-style instantiation and state persistence.

    /// <summary>
    /// Enumeration with the kinds of persistable objects.
    /// </summary>
    /// <remarks>
    /// This enum is used to write type tags for the top-level registry of the <see cref="PersistedObjectSpace"/>.
    /// </remarks>
    internal enum PersistableKind
    {
        // NB: Don't change the names of any of these entries; these are used in the persisted state.

        /// <summary>
        /// Explicit zero value. Not used.
        /// </summary>
        None,

        /// <summary>
        /// The persistable object represents a value.
        /// </summary>
        Value,

        /// <summary>
        /// The persistable object represents a fixed-size array.
        /// </summary>
        Array,

        /// <summary>
        /// The persistable object represents a dynamically sized list.
        /// </summary>
        List,

        /// <summary>
        /// The persistable object represents a linked list.
        /// </summary>
        LinkedList,

        /// <summary>
        /// The persistable object represents a queue.
        /// </summary>
        Queue,

        /// <summary>
        /// The persistable object represents a stack.
        /// </summary>
        Stack,

        /// <summary>
        /// The persistable object represents a set.
        /// </summary>
        Set,

        /// <summary>
        /// The persistable object represents a sorted set.
        /// </summary>
        SortedSet,

        /// <summary>
        /// The persistable object represents a dictionary.
        /// </summary>
        Dictionary,

        /// <summary>
        /// The persistable object represents a sorted dictionary.
        /// </summary>
        SortedDictionary,
    }
}
