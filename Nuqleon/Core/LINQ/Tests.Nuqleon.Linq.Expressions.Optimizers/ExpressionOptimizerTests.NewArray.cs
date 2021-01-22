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
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void NewArrayBounds_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e =
                Expression.NewArrayBounds(typeof(int), t);

            var r =
                Expression.Throw(ex, typeof(int[]));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void NewArrayBounds_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));
            var x = Expression.Parameter(typeof(int));
            var c = Expression.Constant(1);

            var e =
                Expression.NewArrayBounds(typeof(int), x, c, t);

            var r =
                Expression.Throw(ex, typeof(int[,,]));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void NewArrayInit_Throw1()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));

            var e =
                Expression.NewArrayInit(typeof(int), t);

            var r =
                Expression.Throw(ex, typeof(int[]));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void NewArrayInit_Throw2()
        {
            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(int));
            var x = Expression.Parameter(typeof(int));
            var c = Expression.Constant(1);

            var e =
                Expression.NewArrayInit(typeof(int), x, c, t);

            var r =
                Expression.Throw(ex, typeof(int[]));

            AssertOptimized(e, r);
        }
    }
}
