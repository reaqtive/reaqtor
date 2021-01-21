// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Collections.Generic;

namespace System.Linq.CompilerServices
{
    internal sealed class LabeledTree<TSourceNodeType> : Tree<Label<TSourceNodeType>>
    {
        public LabeledTree(Label<TSourceNodeType> nodeType, IEnumerable<LabeledTree<TSourceNodeType>> children)
            : base(nodeType, children)
        {
        }

        public override string ToString() => Children.Count == 0 ? Value.ToString(withValue: false) + " " + Value.Tree.ToString() : base.ToString();

        public override string ToString(int indent)
        {
            if (Children.Count == 0)
            {
                var ind = new string(' ', indent * 2);
                return ind + ToString();
            }
            else
            {
                return base.ToString(indent);
            }
        }
    }
}
