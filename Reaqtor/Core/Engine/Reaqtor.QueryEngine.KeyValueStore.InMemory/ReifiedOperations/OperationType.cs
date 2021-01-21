// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Enumeration of types for reified key/value store operations.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// The operation is undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The operation is an addition of a key/value pair.
        /// </summary>
        Add = 1,

        /// <summary>
        /// The operation is a removal of a key/value pair.
        /// </summary>
        Remove = 2,

        /// <summary>
        /// The operation is a retrieval of a key/value pair.
        /// </summary>
        Get = 3,

        /// <summary>
        /// The operation is an update to an existing key/value pair.
        /// </summary>
        Update = 4,

        /// <summary>
        /// The operation checks whether a key/value pair with the given key exists.
        /// </summary>
        Contains = 5,

        /// <summary>
        /// The operation is an enumeration over the key/value store.
        /// </summary>
        Enumerate = 6,
    }
}
