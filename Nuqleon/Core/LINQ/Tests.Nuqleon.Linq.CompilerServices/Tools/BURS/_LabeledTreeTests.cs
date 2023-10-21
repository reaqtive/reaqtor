// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class _LabeledTreeTests
    {
        [TestMethod]
        public void LabeledTree_ToString()
        {
            Tree<int> tree = new Tree<int>(42,
                new Tree<int>(19),
                new Tree<int>(23,
                    new Tree<int>(7)
                )
            );

            LabeledTree<int> ltre = new LabeledTree<int>(new Label<int>(tree, new List<Match> { new(1) }), new LabeledTree<int>[] {
                new(new Label<int>(tree.Children[0], new List<Match> { new(2) }), Array.Empty<LabeledTree<int>>()),
                new(new Label<int>(tree.Children[1], new List<Match> { new(3) }), new LabeledTree<int>[] {
                    new(new Label<int>(tree.Children[1].Children[0], new List<Match> { new(4) }), Array.Empty<LabeledTree<int>>())
                }),
            });

            Assert.AreEqual("[1 (0$)] 42([2 (0$)] 19(), [3 (0$)] 23([4 (0$)] 7()))", ltre.ToString());
            Assert.AreEqual(@"[1 (0$)] 42(
  [2 (0$)] 19(), 
  [3 (0$)] 23(
    [4 (0$)] 7()
  )
)".Replace("\r", string.Empty), ltre.ToString(indent: 0).Replace("\r", string.Empty));
        }
    }
}
