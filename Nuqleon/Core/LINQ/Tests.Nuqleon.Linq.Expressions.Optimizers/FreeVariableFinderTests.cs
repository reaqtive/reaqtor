// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public partial class FreeVariableFinderTests
    {
        [TestMethod]
        public void FreeVariableFinder_Basics()
        {
            var finder = new FreeVariableFinder();

            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    new[] { x },
                    Expression.Lambda(
                        Expression.TryCatch(
                            Expression.Add(x, y),
                            Expression.Catch(
                                ex,
                                z
                            )
                        ),
                        y
                    )
                );

            finder.Visit(e);

            CollectionAssert.AreEquivalent(new[] { z }, finder.FreeVariables.ToArray());
        }
    }
}
