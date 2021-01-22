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
    internal sealed class Label<TValue>
    {
        public Label(ITree<TValue> tree, IList<Match> labels)
        {
            Tree = tree;
            Labels = labels;
        }

        public ITree<TValue> Tree { get; }

        public TValue Value => Tree.Value;

        public IList<Match> Labels { get; }

        public override string ToString() => ToString(withValue: true);

        public string ToString(bool withValue) => "[" + string.Join(", ", Labels) + "]" + (withValue ? " " + Value.ToString() : "");
    }
}
