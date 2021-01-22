// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions
{
    [TestClass]
    public class ExpressionSlimToExpressionConverterTests : TestBase
    {
        [TestMethod]
        public void ExpressionSlimToExpressionConverter_NullArgument()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionSlimToExpressionConverter(typeSpace: null), ex => Assert.AreEqual("typeSpace", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new ExpressionSlimToExpressionConverter(new InvertedTypeSpace(), factory: null), ex => Assert.AreEqual("factory", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionSlimToExpressionConverter_BaseClass()
        {
            new MyConverter().TestBase();
        }

        [TestMethod]
        public void ExpressionSlimToExpressionConverter_Constant_Null()
        {
            var objNull = ObjectSlim.Create(value: null, typeof(object).ToTypeSlim(), typeof(object));
            var strNull = ObjectSlim.Create(value: null, typeof(string).ToTypeSlim(), typeof(string));

            var n1 = ExpressionSlim.Constant(objNull);
            var n2 = ExpressionSlim.Constant(strNull, typeof(string).ToTypeSlim());

            var c1 = (ConstantExpression)n1.ToExpression();
            var c2 = (ConstantExpression)n2.ToExpression();

            Assert.IsNull(c1.Value);
            Assert.AreEqual(typeof(object), c1.Type);

            Assert.IsNull(c2.Value);
            Assert.AreEqual(typeof(string), c2.Type);
        }

        [TestMethod]
        public void ExpressionSlimToExpressionConverter_Throw()
        {
            var ex = new Exception();

            var conv = new ExpressionSlimToExpressionConverter();

            {
                var t = ExpressionSlim.Throw(Expression.Constant(ex).ToExpressionSlim());
                var e = conv.Visit(t);

                Assert.AreEqual(ExpressionType.Throw, e.NodeType);

                var u = (UnaryExpression)e;
                Assert.AreEqual(typeof(void), u.Type);

                Assert.AreEqual(ExpressionType.Constant, u.Operand.NodeType);
                var c = (ConstantExpression)u.Operand;
                Assert.AreSame(ex, c.Value);
            }

            {
                var t = ExpressionSlim.Throw(Expression.Constant(ex).ToExpressionSlim(), typeof(int).ToTypeSlim());
                var e = conv.Visit(t);

                Assert.AreEqual(ExpressionType.Throw, e.NodeType);

                var u = (UnaryExpression)e;
                Assert.AreEqual(typeof(int), u.Type);

                Assert.AreEqual(ExpressionType.Constant, u.Operand.NodeType);
                var c = (ConstantExpression)u.Operand;
                Assert.AreSame(ex, c.Value);
            }

            {
                var t = ExpressionSlim.MakeUnary(ExpressionType.Throw, Expression.Constant(ex).ToExpressionSlim(), type: null);
                var e = conv.Visit(t);

                Assert.AreEqual(ExpressionType.Throw, e.NodeType);

                var u = (UnaryExpression)e;
                Assert.AreEqual(typeof(void), u.Type);

                Assert.AreEqual(ExpressionType.Constant, u.Operand.NodeType);
                var c = (ConstantExpression)u.Operand;
                Assert.AreSame(ex, c.Value);
            }
        }

        private sealed class MyConverter : ExpressionSlimToExpressionConverter
        {
            public MyConverter()
                : base()
            {
            }

            public void TestBase()
            {
                Assert.IsNotNull(base.TypeSpace);
            }
        }
    }
}
