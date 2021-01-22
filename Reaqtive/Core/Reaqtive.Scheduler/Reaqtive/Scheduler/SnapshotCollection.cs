// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Snapshot collection provides you a way to have its snapshot in an efficient thread safe manner.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class SnapshotCollection<TCollection, TElement>
        where TCollection : ICollection<TElement>, new()
    {
        /// <summary>
        /// Two collections used for snapshots.
        /// </summary>
        private readonly TCollection[] _collections;

        /// <summary>
        /// The active collection index.
        /// </summary>
        private int _active;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotCollection{TCollection, TElement}"/> class.
        /// </summary>
        public SnapshotCollection()
        {
            _collections = new[] { new TCollection(), new TCollection() };
            _active = 0;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>True, if the added element was the first one. False otherwise.</returns>
        public bool Add(TElement item)
        {
            bool isFirst;

            lock (_collections)
            {
                isFirst = _collections[_active].Count == 0;
                _collections[_active].Add(item);
            }

            return isFirst;
        }

        /// <summary>
        /// Takes the snapshot of the current state of the collection and clears all elements.
        /// </summary>
        /// <returns>The current snapshot.</returns>
        public TCollection Snapshot()
        {
            TCollection result;

            lock (_collections)
            {
                result = _collections[_active];
                _active = 1 - _active;
                _collections[_active].Clear();
            }

            return result;
        }
    }
}
