// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/07/2017 - Created this type.
//

using System.Collections.Generic;

namespace System.Memory.Diagnostics
{
    /// <summary>
    /// Representation of a partition of an object heap.
    /// </summary>
    /// <remarks>
    /// A heap partition is defined as the transitive closure of objects reachable from a specified set of
    /// roots, in a manner similar to the GC algorithm to find live objects. This search may be subject to
    /// predicates (also referred to as fences), thus reducing the size fo the transitive closure. For more
    /// information, see <see cref="Fence"/> and <see cref="Scan"/>.
    /// </remarks>
    public class HeapPartition
    {
        /// <summary>
        /// Creates a new heap partition.
        /// </summary>
        /// <param name="name">The name of the heap partition.</param>
        /// <param name="roots">The roots defining the heap partition.</param>
        public HeapPartition(string name, object[] roots)
            : this(name, roots, fence: null)
        {
        }

        /// <summary>
        /// Creates a new heap partition.
        /// </summary>
        /// <param name="name">The name of the heap partition.</param>
        /// <param name="roots">The roots defining the heap partition.</param>
        /// <param name="fence">The fence predicate used to determine the boundaries of the partition.</param>
        public HeapPartition(string name, object[] roots, Func<object, bool> fence)
        {
            if (roots == null)
                throw new ArgumentNullException(nameof(roots));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Roots = new ObjectSet(roots);
            Fence = fence;
        }

        /// <summary>
        /// Gets the name of the heap partition.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the roots defining the heap partition.
        /// </summary>
        public ISet<object> Roots { get; }

        /// <summary>
        /// Gets the fence predicate which is used to determine the boundaries of the partition.
        /// For every object encountered during heap traversal in <see cref="Scan"/>, this predicate
        /// is evaluated. If the predicate returns <c>true</c>, the traversal is continued.
        /// </summary>
        public Func<object, bool> Fence { get; }

        /// <summary>
        /// Scans the heap partition to find all reachable objects starting from <see cref="Roots"/>,
        /// using the specified <paramref name="fence"/> predicate to optionally prune the traversal.
        /// </summary>
        /// <param name="fence">
        /// Fence predicate representing a condition evaluated for every object encountered during the heap
        /// walk in order to determine whether further traversal is needed. When the predicate is applied to
        /// an object and returns <c>true</c>, the traversal continues. This predicate is applied in addition
        /// to the predicate supplied in <see cref="Fence"/>, if it's set.
        /// </param>
        /// <returns>A set containing all the objects reachable from the <see cref="Roots"/>.</returns>
        public ISet<object> Scan(Func<object, bool> fence)
        {
            var allReachableObjects = new ObjectSet();

            //
            // Calculate the fence predicate to use. Rather than "appending" delegates one-by-one using
            // a && conjunction, we write out all cases explicitly. This avoids additional delegate
            // invocation overhead. Note we always have a non-trivial fence using the reachable objects
            // set addition check used to determine cycles.
            //

            var finalFence = default(Func<object, bool>);

            if (Fence == null && fence == null)
            {
                finalFence = o => allReachableObjects.Add(o);
            }
            else if (Fence == null || fence == null)
            {
                var nonNullFence = Fence ?? fence;
                finalFence = o => nonNullFence(o) && allReachableObjects.Add(o);
            }
            else
            {
                finalFence = o => Fence(o) && fence(o) && allReachableObjects.Add(o);
            }

            //
            // Perform the walk starting from each root.
            //

            var walker = new FastHeapReferenceWalker();

            foreach (var root in Roots)
            {
                walker.Walk(root, finalFence);
            }

            return allReachableObjects;
        }

        /// <summary>
        /// Gets a friendly string representation of the heap partition instance.
        /// </summary>
        /// <returns>A friendly string representation of the heap partition instance.</returns>
        public override string ToString() => Name;
    }
}
