// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class PartialExpressionVisitorTests
    {
        [TestMethod]
        public void PartialExpressionVisitor_Nodes()
        {
            var v = new PartialExpressionVisitor<int>();

            var es = new Expression[]
            {
                Expression.Add(Expression.Constant(1), Expression.Constant(2)),
                Expression.Block(Expression.Empty()),
                Expression.Condition(Expression.Constant(true), Expression.Empty(), Expression.Empty()),
                Expression.Constant(1),
                Expression.DebugInfo(Expression.SymbolDocument("foo.cs"), 1, 2, 3, 4),
                Expression.ClearDebugInfo(Expression.SymbolDocument("foo.cs")),
                Expression.Default(typeof(int)),
                // Dynamic
                new MyNode(),
                Expression.Goto(Expression.Label()),
                Expression.MakeIndex(Expression.Constant(new List<int>()), typeof(List<int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                Expression.Invoke(Expression.Constant(new Action(() => {}))),
                Expression.Label(Expression.Label()),
                Expression.Lambda(Expression.Empty()),
                Expression.ListInit(Expression.New(typeof(List<int>)), Expression.Constant(1)),
                Expression.Loop(Expression.Empty()),
                Expression.Property(expression: null, typeof(DateTime).GetProperty("Now")),
                Expression.MemberInit(Expression.New(typeof(Bar)), Expression.Bind(typeof(Bar).GetField("Foo"), Expression.Constant(1))),
                Expression.Call(typeof(Console).GetMethod("WriteLine", Type.EmptyTypes)),
                Expression.New(typeof(object).GetConstructor(Type.EmptyTypes)),
                Expression.NewArrayBounds(typeof(int), Expression.Constant(1)),
                Expression.Parameter(typeof(int)),
                Expression.RuntimeVariables(Expression.Parameter(typeof(int))),
                Expression.Switch(Expression.Constant(1), Expression.SwitchCase(Expression.Empty(), Expression.Constant(2))),
                Expression.TryFinally(Expression.Empty(), Expression.Empty()),
                Expression.TypeIs(Expression.Constant(1), typeof(int)),
                Expression.Negate(Expression.Constant(1))
            };

            foreach (var e in es)
            {
                Assert.ThrowsException<NotSupportedException>(() => v.Visit(e));
            }
        }

        private sealed class Bar
        {
#pragma warning disable 0649
            public int Foo;
#pragma warning restore
        }

        private sealed class MyNode : Expression
        {
            public override Type Type => typeof(int);

            public override ExpressionType NodeType => ExpressionType.Extension;
        }
    }
}
