// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Base class for visitors over generic trees.
    /// </summary>
    /// <typeparam name="T">Type of the data contained in the tree nodes.</typeparam>
    public class TreeVisitor<T> : ITreeVisitor<T>
    {
        /// <summary>
        /// Visits the specified tree.
        /// </summary>
        /// <param name="node">Tree to visit.</param>
        /// <returns>Result of the visit.</returns>
        public virtual ITree<T> Visit(ITree<T> node) => node?.Update(Visit(node.Children));

        /// <summary>
        /// Visits the specified tree nodes.
        /// </summary>
        /// <param name="nodes">Tree nodes to visit.</param>
        /// <returns>Result of visiting the tree nodes. This collection will be equal to the original collection if none of the tree nodes changed.</returns>
        protected IReadOnlyList<ITree<T>> Visit(IReadOnlyList<ITree<T>> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var res = default(List<ITree<T>>);

            for (int i = 0, n = nodes.Count; i < n; i++)
            {
                var node = nodes[i];
                var result = Visit(node);

                if (res != null)
                {
                    res.Add(result);
                }
                else
                {
                    if (!Equals(node, result))
                    {
                        res = new List<ITree<T>>(nodes.Count);
                        for (int j = 0; j < i; j++)
                        {
                            res.Add(nodes[j]);
                        }

                        res.Add(result);
                    }
                }
            }

            if (res != null)
            {
                return res.AsReadOnly();
            }

            return nodes;
        }
    }
}
