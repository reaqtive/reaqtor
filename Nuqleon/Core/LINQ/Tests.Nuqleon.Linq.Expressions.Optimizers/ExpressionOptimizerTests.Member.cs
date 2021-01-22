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
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    public partial class ExpressionOptimizerTests
    {
        [TestMethod]
        public void Member_Static_Eval()
        {
            var f = typeof(PureMembers).GetField(nameof(PureMembers.StaticField));

            var e = Expression.Field(expression: null, f);

            var r = Expression.Constant(42, typeof(int));

            AssertOptimized(
                GetPureMemberOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Member_Static_NoEval()
        {
            var f = typeof(PureMembers).GetField(nameof(PureMembers.StaticFieldNotPure));

            var e = Expression.Field(expression: null, f);

            var r = e;

            AssertOptimized(
                GetPureMemberOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Member_Instance_Throw()
        {
            var f = typeof(PureMembers).GetField(nameof(PureMembers.InstanceField));

            var ex = Expression.Parameter(typeof(Exception));
            var t = Expression.Throw(ex, typeof(PureMembers));

            var e = Expression.Field(t, f);

            var r = Expression.Throw(ex, typeof(int));

            AssertOptimized(
                GetPureMemberOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Member_Instance_Eval()
        {
            var f = typeof(PureMembers).GetField(nameof(PureMembers.InstanceField));

            var o = Expression.Constant(new PureMembers(42));
            var e = Expression.Field(o, f);

            var r = Expression.Constant(42, typeof(int));

            AssertOptimized(
                GetPureMemberOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Member_Instance_NoEval1()
        {
            var f = typeof(PureMembers).GetField(nameof(PureMembers.InstanceFieldNotPure));

            var o = Expression.Constant(new PureMembers(42));
            var e = Expression.Field(o, f);

            var r = e;

            AssertOptimized(
                GetPureMemberOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Member_Instance_NoEval2()
        {
            var f = typeof(PureMembers).GetField(nameof(PureMembers.InstanceField));

            var o = Expression.Parameter(typeof(PureMembers));
            var e = Expression.Field(o, f);

            var r = e;

            AssertOptimized(
                GetPureMemberOptimizer(),
                e,
                r
            );
        }

        [TestMethod]
        public void Member_Instance_Null()
        {
            var e =
                Expression.Property(
                    Expression.Constant(value: null, typeof(string)),
                    typeof(string).GetProperty(nameof(string.Length))
                );

            AssertThrows(e, typeof(NullReferenceException));
        }

        private static ExpressionOptimizer GetPureMemberOptimizer() => new(new PureMemberSemanticProvider(), GetEvaluatorFactory());

        private sealed class PureMemberSemanticProvider : DefaultSemanticProvider
        {
            public override bool IsPure(MemberInfo member)
            {
                return member == typeof(PureMembers).GetField(nameof(PureMembers.StaticField))
                    || member == typeof(PureMembers).GetField(nameof(PureMembers.InstanceField));
            }
        }

        private sealed class PureMembers
        {
            public PureMembers(int value)
            {
                InstanceField = value;
                InstanceFieldNotPure = value;
            }

            public static int StaticField = 42;
            public static int StaticFieldNotPure = -1;

            public int InstanceField;
            public int InstanceFieldNotPure;
        }
    }
}
