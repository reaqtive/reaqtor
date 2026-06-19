// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Represents a value with an associated sequence number.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public class Sequenced<T>
    {
        /// <summary>
        /// Creates a new sequenced object, using a unique sequence number obtained from a global counter.
        /// </summary>
        /// <param name="obj">The underlying object.</param>
        public Sequenced(T obj)
            : this(obj, Sequenced.Next)
        {
        }

        /// <summary>
        /// Creates a new sequenced object, using the specified sequence number.
        /// </summary>
        /// <param name="obj">The underlying object.</param>
        /// <param name="sequenceId">The sequence id to use.</param>
        public Sequenced(T obj, long sequenceId)
        {
            Object = obj;
            SequenceId = sequenceId;
        }

#pragma warning disable CA1720 // Identifier 'Object' contains type name.

        /// <summary>
        /// Gets the object.
        /// </summary>
        public T Object { get; }

#pragma warning restore CA1720

        /// <summary>
        /// Gets the sequence number.
        /// </summary>
        public long SequenceId { get; }
    }
}
