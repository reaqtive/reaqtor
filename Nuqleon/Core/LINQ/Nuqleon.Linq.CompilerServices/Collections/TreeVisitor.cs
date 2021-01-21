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
    /// Base class for visitors over non-generic trees.
    /// </summary>
    public class TreeVisitor : ITreeVisitor
    {
        /// <summary>
        /// Visits the specified tree.
        /// </summary>
        /// <param name="node">Tree to visit.</param>
        /// <returns>Result of the visit.</returns>
        public virtual ITree Visit(ITree node) => node?.Update(Visit(node.Children));

        /// <summary>
        /// Visits the specified tree nodes.
        /// </summary>
        /// <param name="nodes">Tree nodes to visit.</param>
        /// <returns>Result of visiting the tree nodes. This collection will be equal to the original collection if none of the tree nodes changed.</returns>
        protected IReadOnlyList<ITree> Visit(IReadOnlyList<ITree> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var res = default(List<ITree>);

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
                        res = new List<ITree>(nodes.Count);
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
