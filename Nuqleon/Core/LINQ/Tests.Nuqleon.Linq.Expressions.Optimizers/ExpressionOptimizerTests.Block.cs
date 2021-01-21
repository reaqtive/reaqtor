// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Threading;
using System;
using System.Linq;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Block_UnusedVariables1()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x },
                    V,
                    Expression.Constant(1)
                );

            var r =
                Expression.Block(
                    V,
                    Expression.Constant(1)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_UnusedVariables2()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    typeof(void),
                    new[] { x },
                    V,
                    Expression.Constant(1)
                );

            var r =
                Expression.Block(
                    typeof(void),
                    V,
                    Expression.Constant(1)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_UnusedVariables3()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x, y, z },
                    V,
                    Expression.Assign(x, F),
                    Expression.Assign(z, G),
                    Expression.Add(x, z)
                );

            var r =
                Expression.Block(
                    new[] { x, z },
                    V,
                    Expression.Assign(x, F),
                    Expression.Assign(z, G),
                    Expression.Add(x, z)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Unassigned1()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x },
                    V,
                    x
                );

            var r =
                Expression.Block(
                    V,
                    Expression.Default(typeof(int))
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Unassigned2()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x },
                    V,
                    WriteLine(x)
                );

            var r =
                Expression.Block(
                    V,
                    WriteLine(Expression.Default(typeof(int)))
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Unassigned3()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x, y, z },
                    Expression.Assign(x, Expression.Constant(1)),
                    WriteLine(y),
                    Expression.Assign(z, Expression.Constant(3)),
                    WriteLine(x),
                    WriteLine(z)
                );

            var r =
                Expression.Block(
                    new[] { x, z },
                    Expression.Assign(x, Expression.Constant(1)),
                    WriteLine(Expression.Default(typeof(int))),
                    Expression.Assign(z, Expression.Constant(3)),
                    WriteLine(x),
                    WriteLine(z)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Flatten1()
        {
            var e =
                Expression.Block(
                    V
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Flatten2()
        {
            var e =
                Expression.Block(
                    Expression.Block(
                        V
                    )
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Flatten3()
        {
            var e =
                Expression.Block(
                    Expression.Block(
                        Expression.Block(
                            V
                        )
                    )
                );

            var r =
                V;

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Flatten4()
        {
            var e =
                Expression.Block(
                    Expression.Block(
                        Expression.Block(
                            V,
                            V
                        )
                    )
                );

            var r =
                Expression.Block(
                    V,
                    V
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Flatten5()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x },
                    Expression.Block(
                        Expression.Block(
                            new[] { y },
                            Expression.Block(
                                Expression.Assign(x, F),
                                Expression.Assign(y, G),
                                Expression.Add(x, y)
                            )
                        )
                    )
                );

            var r =
                Expression.Block(
                    new[] { x, y },
                    Expression.Assign(x, F),
                    Expression.Assign(y, G),
                    Expression.Add(x, y)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    Expression.Throw(ex, typeof(int))
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    Expression.Constant(1),
                    Expression.Throw(ex, typeof(int))
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Throw3()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    Expression.Constant(1),
                    Expression.Throw(ex, typeof(int)),
                    Expression.Constant(2)
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Throw4()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    Expression.Constant(1),
                    Expression.Throw(ex, typeof(void)),
                    Expression.Constant(2)
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_Throw5()
        {
            var x = Expression.Parameter(typeof(int));
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.Block(
                    new[] { x },
                    Expression.Constant(1),
                    Expression.Throw(ex, typeof(void)),
                    Expression.Assign(x, Expression.Constant(2))
                );

            var r =
                Expression.Throw(ex, typeof(int));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_AllPure1()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    new[] { x },
                    Expression.Constant(1),
                    x,
                    Expression.Constant(3)
                );

            var r =
                Expression.Constant(3);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_AllPure2()
        {
            var e =
                Expression.Block(
                    typeof(void),
                    Expression.Constant(1),
                    Expression.Constant(2),
                    Expression.Constant(3)
                );

            var r =
                Expression.Empty();

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void Block_RemovePure()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.Block(
                    V1,
                    Expression.Empty(),
                    V2,
                    Expression.Constant(1),
                    Expression.Empty(),
                    x,
                    V3,
                    Expression.Constant(3)
                );

            var r =
                Expression.Block(
                    V1,
                    V2,
                    V3,
                    Expression.Constant(3)
                );

            AssertOptimized(e, r);
        }
    }
}
