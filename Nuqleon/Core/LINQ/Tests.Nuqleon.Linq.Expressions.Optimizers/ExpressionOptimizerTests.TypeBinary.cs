// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void TypeBinary_TypeIs_Throw()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.TypeIs(
                    Expression.Throw(ex, typeof(int)),
                    typeof(int)
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_NotPure()
        {
            var e =
                Expression.TypeIs(
                    F,
                    typeof(int)
                );

            var r =
                Expression.TypeIs(
                    F,
                    typeof(int)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure1()
        {
            var e =
                Expression.TypeIs(
                    Expression.Constant(1),
                    typeof(int)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure2()
        {
            var e =
                Expression.TypeIs(
                    Expression.Parameter(typeof(int)),
                    typeof(int)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure3()
        {
            var e =
                Expression.TypeIs(
                    Expression.Constant("bar"),
                    typeof(int)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure4()
        {
            var e =
                Expression.TypeIs(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(string)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure5()
        {
            var e =
                Expression.TypeIs(
                    Expression.Constant(value: null, typeof(int?)),
                    typeof(int?)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure6()
        {
            var e =
                Expression.TypeIs(
                    Expression.Constant("bar", typeof(string)),
                    typeof(string)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure7()
        {
            var s = Expression.Parameter(typeof(string));

            var e =
                Expression.TypeIs(
                    s,
                    typeof(string)
                );

            var r =
                Expression.TypeIs(
                    s,
                    typeof(string)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure8()
        {
            var e =
                Expression.TypeIs(
                    Expression.Constant("bar", typeof(string)),
                    typeof(void)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure9()
        {
            var e =
                Expression.TypeIs(
                    Expression.Empty(),
                    typeof(string)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeIs_Pure10()
        {
            var xs = Expression.Parameter(typeof(List<string>));

            var e =
                Expression.TypeIs(
                    xs,
                    typeof(IEnumerable<int>)
                );

            // NB: This tests some conservative behavior that may be revised.

            var r =
                Expression.TypeIs(
                    xs,
                    typeof(IEnumerable<int>)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Throw()
        {
            var ex = Expression.Parameter(typeof(Exception));

            var e =
                Expression.TypeEqual(
                    Expression.Throw(ex, typeof(int)),
                    typeof(int)
                );

            var r =
                Expression.Throw(ex, typeof(bool));

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_NotPure()
        {
            var e =
                Expression.TypeEqual(
                    F,
                    typeof(int)
                );

            var r =
                Expression.TypeEqual(
                    F,
                    typeof(int)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure1()
        {
            var e =
                Expression.TypeEqual(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(string)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure2()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.TypeEqual(
                    x,
                    typeof(int)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure3()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.TypeEqual(
                    x,
                    typeof(int?)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure4()
        {
            var x = Expression.Parameter(typeof(int));

            var e =
                Expression.TypeEqual(
                    x,
                    typeof(long)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure5()
        {
            var x = Expression.Parameter(typeof(int?));

            var e =
                Expression.TypeEqual(
                    x,
                    typeof(int?)
                );

            // NB: Operand could be null.

            var r =
                Expression.TypeEqual(
                    x,
                    typeof(int?)
                );

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure6()
        {
            var e =
                Expression.TypeEqual(
                    Expression.Constant("bar"),
                    typeof(string)
                );

            var r =
                Expression.Constant(true);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure7()
        {
            var e =
                Expression.TypeEqual(
                    Expression.Constant("bar"),
                    typeof(object)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }

        [TestMethod]
        public void TypeBinary_TypeEqual_Pure8()
        {
            var e =
                Expression.TypeEqual(
                    Expression.Constant("bar"),
                    typeof(int)
                );

            var r =
                Expression.Constant(false);

            AssertOptimized(e, r);
        }
    }
}
