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
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions
{
    [TestClass]
    public class ExpressionSlimTests : TestBase
    {
        private static readonly DefaultExpressionSlim s_void = (DefaultExpressionSlim)Expression.Empty().ToExpressionSlim();

        [TestMethod]
        public void ExpressionSlim_BinaryFactoryTests()
        {
            var left = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var right = ExpressionSlim.Constant(ObjectSlim.Create(1, SlimType, typeof(int)), SlimType);
            var array = Expression.Constant(new[] { 1, 2, 3 }).ToExpressionSlim();
            var method = ((MethodCallExpressionSlim)((Expression<Func<string>>)(() => "".ToUpper())).Body.ToExpressionSlim()).Method;
            var conv = ExpressionSlim.Lambda(ExpressionSlim.Empty());

            var binary = ExpressionSlim.Add(left, right);
            AssertBinary(binary, left, right, ExpressionType.Add);

            binary = ExpressionSlim.Add(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Add, isLiftedToNull: true, method);

            binary = ExpressionSlim.AddChecked(left, right);
            AssertBinary(binary, left, right, ExpressionType.AddChecked);

            binary = ExpressionSlim.AddChecked(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.AddChecked, isLiftedToNull: true, method);

            binary = ExpressionSlim.And(left, right);
            AssertBinary(binary, left, right, ExpressionType.And);

            binary = ExpressionSlim.And(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.And, isLiftedToNull: true, method);

            binary = ExpressionSlim.AndAlso(left, right);
            AssertBinary(binary, left, right, ExpressionType.AndAlso);

            binary = ExpressionSlim.AndAlso(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.AndAlso, isLiftedToNull: true, method);

            binary = ExpressionSlim.Coalesce(left, right);
            AssertBinary(binary, left, right, ExpressionType.Coalesce, isLiftedToNull: false);

            binary = ExpressionSlim.Coalesce(left, right, conv);
            AssertBinary(binary, left, right, ExpressionType.Coalesce, isLiftedToNull: false, method: null, conv);

            binary = ExpressionSlim.Divide(left, right);
            AssertBinary(binary, left, right, ExpressionType.Divide);

            binary = ExpressionSlim.Divide(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Divide, isLiftedToNull: true, method);

            binary = ExpressionSlim.Equal(left, right);
            AssertBinary(binary, left, right, ExpressionType.Equal, isLiftedToNull: false);

            binary = ExpressionSlim.Equal(left, right, liftToNull: true, method);
            AssertBinary(binary, left, right, ExpressionType.Equal, isLiftedToNull: true, method);

            binary = ExpressionSlim.ExclusiveOr(left, right);
            AssertBinary(binary, left, right, ExpressionType.ExclusiveOr);

            binary = ExpressionSlim.ExclusiveOr(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.ExclusiveOr, isLiftedToNull: true, method);

            binary = ExpressionSlim.GreaterThan(left, right);
            AssertBinary(binary, left, right, ExpressionType.GreaterThan, isLiftedToNull: false);

            binary = ExpressionSlim.GreaterThan(left, right, liftToNull: true, method);
            AssertBinary(binary, left, right, ExpressionType.GreaterThan, isLiftedToNull: true, method);

            binary = ExpressionSlim.GreaterThanOrEqual(left, right);
            AssertBinary(binary, left, right, ExpressionType.GreaterThanOrEqual, isLiftedToNull: false);

            binary = ExpressionSlim.GreaterThanOrEqual(left, right, liftToNull: true, method);
            AssertBinary(binary, left, right, ExpressionType.GreaterThanOrEqual, isLiftedToNull: true, method);

            binary = ExpressionSlim.LeftShift(left, right);
            AssertBinary(binary, left, right, ExpressionType.LeftShift);

            binary = ExpressionSlim.LeftShift(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.LeftShift, isLiftedToNull: true, method);

            binary = ExpressionSlim.LessThanOrEqual(left, right);
            AssertBinary(binary, left, right, ExpressionType.LessThanOrEqual, isLiftedToNull: false);

            binary = ExpressionSlim.LessThanOrEqual(left, right, liftToNull: true, method);
            AssertBinary(binary, left, right, ExpressionType.LessThanOrEqual, isLiftedToNull: true, method);

            binary = ExpressionSlim.Modulo(left, right);
            AssertBinary(binary, left, right, ExpressionType.Modulo);

            binary = ExpressionSlim.Modulo(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Modulo, isLiftedToNull: true, method);

            binary = ExpressionSlim.Multiply(left, right);
            AssertBinary(binary, left, right, ExpressionType.Multiply);

            binary = ExpressionSlim.Multiply(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Multiply, isLiftedToNull: true, method);

            binary = ExpressionSlim.MultiplyChecked(left, right);
            AssertBinary(binary, left, right, ExpressionType.MultiplyChecked);

            binary = ExpressionSlim.MultiplyChecked(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.MultiplyChecked, isLiftedToNull: true, method);

            binary = ExpressionSlim.NotEqual(left, right);
            AssertBinary(binary, left, right, ExpressionType.NotEqual, isLiftedToNull: false);

            binary = ExpressionSlim.NotEqual(left, right, liftToNull: true, method);
            AssertBinary(binary, left, right, ExpressionType.NotEqual, isLiftedToNull: true, method);

            binary = ExpressionSlim.Or(left, right);
            AssertBinary(binary, left, right, ExpressionType.Or);

            binary = ExpressionSlim.Or(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Or, isLiftedToNull: true, method);

            binary = ExpressionSlim.OrElse(left, right);
            AssertBinary(binary, left, right, ExpressionType.OrElse);

            binary = ExpressionSlim.OrElse(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.OrElse, isLiftedToNull: true, method);

            binary = ExpressionSlim.Power(left, right);
            AssertBinary(binary, left, right, ExpressionType.Power);

            binary = ExpressionSlim.Power(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Power, isLiftedToNull: true, method);

            binary = ExpressionSlim.RightShift(left, right);
            AssertBinary(binary, left, right, ExpressionType.RightShift);

            binary = ExpressionSlim.RightShift(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.RightShift, isLiftedToNull: true, method);

            binary = ExpressionSlim.Subtract(left, right);
            AssertBinary(binary, left, right, ExpressionType.Subtract);

            binary = ExpressionSlim.Subtract(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.Subtract, isLiftedToNull: true, method);

            binary = ExpressionSlim.SubtractChecked(left, right);
            AssertBinary(binary, left, right, ExpressionType.SubtractChecked);

            binary = ExpressionSlim.SubtractChecked(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.SubtractChecked, isLiftedToNull: true, method);


            binary = ExpressionSlim.AddAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.AddAssign);

            binary = ExpressionSlim.AddAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.AddAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.AddAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.AddAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.AddAssignChecked(left, right);
            AssertBinary(binary, left, right, ExpressionType.AddAssignChecked);

            binary = ExpressionSlim.AddAssignChecked(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.AddAssignChecked, isLiftedToNull: true, method);

            binary = ExpressionSlim.AddAssignChecked(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.AddAssignChecked, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.AndAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.AndAssign);

            binary = ExpressionSlim.AndAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.AndAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.AndAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.AndAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.Assign(left, right);
            AssertBinary(binary, left, right, ExpressionType.Assign);

            binary = ExpressionSlim.DivideAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.DivideAssign);

            binary = ExpressionSlim.DivideAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.DivideAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.DivideAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.DivideAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.ExclusiveOrAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.ExclusiveOrAssign);

            binary = ExpressionSlim.ExclusiveOrAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.ExclusiveOrAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.ExclusiveOrAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.ExclusiveOrAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.LeftShiftAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.LeftShiftAssign);

            binary = ExpressionSlim.LeftShiftAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.LeftShiftAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.LeftShiftAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.LeftShiftAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.ModuloAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.ModuloAssign);

            binary = ExpressionSlim.ModuloAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.ModuloAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.ModuloAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.ModuloAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.MultiplyAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.MultiplyAssign);

            binary = ExpressionSlim.MultiplyAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.MultiplyAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.MultiplyAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.MultiplyAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.MultiplyAssignChecked(left, right);
            AssertBinary(binary, left, right, ExpressionType.MultiplyAssignChecked);

            binary = ExpressionSlim.MultiplyAssignChecked(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.MultiplyAssignChecked, isLiftedToNull: true, method);

            binary = ExpressionSlim.MultiplyAssignChecked(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.MultiplyAssignChecked, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.OrAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.OrAssign);

            binary = ExpressionSlim.OrAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.OrAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.OrAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.OrAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.PowerAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.PowerAssign);

            binary = ExpressionSlim.PowerAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.PowerAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.PowerAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.PowerAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.RightShiftAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.RightShiftAssign);

            binary = ExpressionSlim.RightShiftAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.RightShiftAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.RightShiftAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.RightShiftAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.SubtractAssign(left, right);
            AssertBinary(binary, left, right, ExpressionType.SubtractAssign);

            binary = ExpressionSlim.SubtractAssign(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.SubtractAssign, isLiftedToNull: true, method);

            binary = ExpressionSlim.SubtractAssign(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.SubtractAssign, isLiftedToNull: true, method, conv);

            binary = ExpressionSlim.SubtractAssignChecked(left, right);
            AssertBinary(binary, left, right, ExpressionType.SubtractAssignChecked);

            binary = ExpressionSlim.SubtractAssignChecked(left, right, method);
            AssertBinary(binary, left, right, ExpressionType.SubtractAssignChecked, isLiftedToNull: true, method);

            binary = ExpressionSlim.SubtractAssignChecked(left, right, method, conv);
            AssertBinary(binary, left, right, ExpressionType.SubtractAssignChecked, isLiftedToNull: true, method, conv);


            binary = ExpressionSlim.ArrayIndex(array, right);
            AssertBinary(binary, array, right, ExpressionType.ArrayIndex, isLiftedToNull: false);


            binary = ExpressionSlim.ReferenceEqual(left, right);
            AssertBinary(binary, left, right, ExpressionType.Equal, isLiftedToNull: false);

            binary = ExpressionSlim.ReferenceNotEqual(left, right);
            AssertBinary(binary, left, right, ExpressionType.NotEqual, isLiftedToNull: false);

            /*
            binary = ExpressionSlim.MakeBinary(ExpressionType.Add, left, right, isLiftedToNull: true, method: null, conversion: null);
            AssertBinary(binary, left, right, ExpressionType.Add);

            binary = ExpressionSlim.MakeBinary(ExpressionType.Add, left, right, isLiftedToNull: false, method: null, conversion: null);
            AssertBinary(binary, left, right, ExpressionType.Add, isLiftedToNull: false);

            binary = ExpressionSlim.MakeBinary(ExpressionType.Add, left, right, isLiftedToNull: false, method, conversion: null);
            AssertBinary(binary, left, right, ExpressionType.Add, isLiftedToNull: false, method);

            binary = ExpressionSlim.MakeBinary(ExpressionType.Add, left, right, isLiftedToNull: false, method, conv);
            AssertBinary(binary, left, right, ExpressionType.Add, isLiftedToNull: false, method, conv);
             */
        }

        [TestMethod]
        public void ExpressionSlim_MakeBinaryFactoryTests_ArgumentChecking()
        {
            var nt = ExpressionType.Call;
            var left = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var right = ExpressionSlim.Constant(ObjectSlim.Create(1, SlimType, typeof(int)), SlimType);
            var method = ((MethodCallExpressionSlim)((Expression<Func<string>>)(() => "".ToUpper())).Body.ToExpressionSlim()).Method;
            var conv = ExpressionSlim.Lambda(ExpressionSlim.Empty());

            AssertEx.ThrowsException<ArgumentException>(() => ExpressionSlim.MakeBinary(nt, left, right), ex => Assert.IsTrue(ex.Message.Contains("not a valid binary expression type")));
            AssertEx.ThrowsException<ArgumentException>(() => ExpressionSlim.MakeBinary(nt, left, right, liftToNull: false, method), ex => Assert.IsTrue(ex.Message.Contains("not a valid binary expression type")));
            AssertEx.ThrowsException<ArgumentException>(() => ExpressionSlim.MakeBinary(nt, left, right, liftToNull: false, method, conv), ex => Assert.IsTrue(ex.Message.Contains("not a valid binary expression type")));
        }

        [TestMethod]
        public void ExpressionSlim_MakeBinaryFactoryTests()
        {
            var left = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var right = ExpressionSlim.Constant(ObjectSlim.Create(1, SlimType, typeof(int)), SlimType);
            var method = ((MethodCallExpressionSlim)((Expression<Func<string>>)(() => "".ToUpper())).Body.ToExpressionSlim()).Method;
            var conv = ExpressionSlim.Lambda(ExpressionSlim.Empty());

            // left, right, method
            foreach (var type in new[]
            {
                ExpressionType.Add,
                ExpressionType.AddChecked,
                ExpressionType.Subtract,
                ExpressionType.SubtractChecked,
                ExpressionType.Multiply,
                ExpressionType.MultiplyChecked,
                ExpressionType.Divide,
                ExpressionType.Modulo,
                ExpressionType.Power,
                ExpressionType.And,
                ExpressionType.AndAlso,
                ExpressionType.Or,
                ExpressionType.OrElse,
                ExpressionType.ExclusiveOr,
                ExpressionType.RightShift,
                ExpressionType.LeftShift,
            })
            {
                {
                    var res = ExpressionSlim.MakeBinary(type, left, right);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Conversion);
                }

                {
                    var res = ExpressionSlim.MakeBinary(type, left, right, liftToNull: false, method);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.AreSame(res.Method, method);
                    Assert.IsNull(res.Conversion);
                }

                {
                    var res = ExpressionSlim.MakeBinary(type, left, right, liftToNull: false, method, conv);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.AreSame(res.Method, method);
                    Assert.IsNull(res.Conversion);
                }
            }

            // left, right, method, lift
            foreach (var type in new[]
            {
                ExpressionType.LessThan,
                ExpressionType.LessThanOrEqual,
                ExpressionType.GreaterThan,
                ExpressionType.GreaterThanOrEqual,
                ExpressionType.Equal,
                ExpressionType.NotEqual,
            })
            {
                {
                    var res = ExpressionSlim.MakeBinary(type, left, right);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Conversion);
                }

                foreach (var b in new[] { true, false })
                {
                    {
                        var res = ExpressionSlim.MakeBinary(type, left, right, b, method);
                        Assert.AreSame(res.Left, left);
                        Assert.AreSame(res.Right, right);
                        Assert.AreSame(res.Method, method);
                        Assert.IsNull(res.Conversion);
                        Assert.AreEqual(res.IsLiftedToNull, b);
                    }

                    {
                        var res = ExpressionSlim.MakeBinary(type, left, right, b, method, conv);
                        Assert.AreSame(res.Left, left);
                        Assert.AreSame(res.Right, right);
                        Assert.AreSame(res.Method, method);
                        Assert.IsNull(res.Conversion);
                        Assert.AreEqual(res.IsLiftedToNull, b);
                    }
                }
            }

            // left, right, conversion
            foreach (var type in new[]
            {
                ExpressionType.Coalesce,
            })
            {
                {
                    var res = ExpressionSlim.MakeBinary(type, left, right);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Conversion);
                }

                {
                    var res = ExpressionSlim.MakeBinary(type, left, right, liftToNull: false, method: null, conv);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.IsNull(res.Method);
                    Assert.AreSame(res.Conversion, conv);
                }
            }

            // left, right
            foreach (var type in new[]
            {
                ExpressionType.Assign,
                ExpressionType.ArrayIndex,
            })
            {
                var res = ExpressionSlim.MakeBinary(type, left, right);
                Assert.AreSame(res.Left, left);
                Assert.AreSame(res.Right, right);
                Assert.IsNull(res.Method);
                Assert.IsNull(res.Conversion);
            }

            // left, right, method, conversion
            foreach (var type in new[]
            {
                ExpressionType.AddAssign,
                ExpressionType.AddAssignChecked,
                ExpressionType.SubtractAssign,
                ExpressionType.SubtractAssignChecked,
                ExpressionType.MultiplyAssign,
                ExpressionType.MultiplyAssignChecked,
                ExpressionType.DivideAssign,
                ExpressionType.ModuloAssign,
                ExpressionType.PowerAssign,
                ExpressionType.AndAssign,
                ExpressionType.OrAssign,
                ExpressionType.ExclusiveOrAssign,
                ExpressionType.LeftShiftAssign,
                ExpressionType.RightShiftAssign,
            })
            {
                {
                    var res = ExpressionSlim.MakeBinary(type, left, right);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Conversion);
                }

                {
                    var res = ExpressionSlim.MakeBinary(type, left, right, liftToNull: false, method);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.AreSame(res.Method, method);
                    Assert.IsNull(res.Conversion);
                }

                {
                    var res = ExpressionSlim.MakeBinary(type, left, right, liftToNull: false, method, conv);
                    Assert.AreSame(res.Left, left);
                    Assert.AreSame(res.Right, right);
                    Assert.AreSame(res.Method, method);
                    Assert.AreSame(res.Conversion, conv);
                }
            }
        }

        private static void AssertBinary(BinaryExpressionSlim expr, ExpressionSlim left, ExpressionSlim right, ExpressionType type, bool isLiftedToNull = true, MethodInfoSlim method = null, LambdaExpressionSlim conversion = null)
        {
            Assert.AreSame(left, expr.Left);
            Assert.AreSame(right, expr.Right);
            Assert.AreEqual(type, expr.NodeType);
            Assert.AreEqual(method, expr.Method);
            Assert.AreEqual(isLiftedToNull, expr.IsLiftedToNull);
            Assert.AreEqual(conversion, expr.Conversion);
        }

        [TestMethod]
        public void ExpressionSlim_ConstantFactoryTests()
        {
            var v1 = ((ConstantExpressionSlim)Expression.Constant(42).ToExpressionSlim()).Value;

            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Constant(value: null));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Constant(value: null, typeof(string).ToTypeSlim()));

            var c2 = ExpressionSlim.Constant(v1);
            var c3 = ExpressionSlim.Constant(v1, v1.TypeSlim);

            var e2 = (ConstantExpression)c2.ToExpression();
            var e3 = (ConstantExpression)c3.ToExpression();

            Assert.AreEqual(typeof(int), e2.Type);
            Assert.AreEqual(42, e2.Value);

            Assert.AreEqual(typeof(int), e3.Type);
            Assert.AreEqual(42, e3.Value);
        }

        [TestMethod]
        public void ExpressionSlim_DefaultFactoryTests()
        {
            var d1 = ExpressionSlim.Empty();
            Assert.AreEqual(typeof(void).ToTypeSlim(), d1.Type);

            var t = typeof(int).ToTypeSlim();

            var d2 = ExpressionSlim.Default(t);
            Assert.AreEqual(t, d2.Type);

            var d3 = ExpressionSlim.Default(typeof(void).ToTypeSlim());
            Assert.AreEqual(typeof(void).ToTypeSlim(), d3.Type);
        }

        [TestMethod]
        public void ExpressionSlim_CallFactoryTests()
        {
            var emptyTypeList = EmptyReadOnlyCollection<TypeSlim>.Instance;
            var methodSlim = SlimType.GetSimpleMethod("Foo", emptyTypeList, SlimType);
            var inst = ExpressionSlim.Default(SlimType);
            var arg0 = ExpressionSlim.Default(SlimType);
            var arg1 = ExpressionSlim.Default(SlimType);

            var call = ExpressionSlim.Call(methodSlim);
            Assert.AreSame(methodSlim, call.Method);
            Assert.AreEqual(ExpressionType.Call, call.NodeType);
            Assert.IsNull(call.Object);
            Assert.IsTrue(call.Arguments.Count == 0);

            call = ExpressionSlim.Call(methodSlim, new[] { arg0, arg1 }.ToList());
            Assert.AreSame(methodSlim, call.Method);
            Assert.AreEqual(ExpressionType.Call, call.NodeType);
            Assert.IsNull(call.Object);
            Assert.IsTrue(call.Arguments.SequenceEqual(new[] { arg0, arg1 }));

            call = ExpressionSlim.Call(methodSlim, arg0, arg1);
            Assert.AreSame(methodSlim, call.Method);
            Assert.AreEqual(ExpressionType.Call, call.NodeType);
            Assert.IsNull(call.Object);
            Assert.IsTrue(call.Arguments.SequenceEqual(new[] { arg0, arg1 }));

            call = ExpressionSlim.Call(inst, methodSlim, new[] { arg0, arg1 }.ToList());
            Assert.AreSame(methodSlim, call.Method);
            Assert.AreEqual(ExpressionType.Call, call.NodeType);
            Assert.AreSame(inst, call.Object);
            Assert.IsTrue(call.Arguments.SequenceEqual(new[] { arg0, arg1 }));

            call = ExpressionSlim.Call(inst, methodSlim, new[] { arg0, arg1 });
            Assert.AreSame(methodSlim, call.Method);
            Assert.AreEqual(ExpressionType.Call, call.NodeType);
            Assert.AreSame(inst, call.Object);
            Assert.IsTrue(call.Arguments.SequenceEqual(new[] { arg0, arg1 }));
        }

        [TestMethod]
        public void ExpressionSlim_ConditionFactoryTests()
        {
            var test = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var ifTrue = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var ifFalse = ExpressionSlim.Constant(ObjectSlim.Create(1, SlimType, typeof(int)), SlimType);
            var condition = ExpressionSlim.Condition(test, ifTrue, ifFalse, SlimType);
            Assert.AreSame(test, condition.Test);
            Assert.AreSame(ifTrue, condition.IfTrue);
            Assert.AreSame(ifFalse, condition.IfFalse);
            Assert.AreSame(SlimType, condition.Type);
            Assert.AreEqual(ExpressionType.Conditional, condition.NodeType);
        }

        [TestMethod]
        public void ExpressionSlim_IfThenElseFactoryTests()
        {
            var test = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var ifTrue = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var ifFalse = ExpressionSlim.Constant(ObjectSlim.Create(1, SlimType, typeof(int)), SlimType);

            var ifThen = ExpressionSlim.IfThen(test, ifTrue);
            Assert.AreSame(test, ifThen.Test);
            Assert.AreSame(ifTrue, ifThen.IfTrue);
            Assert.IsTrue(ifThen.IfFalse.NodeType == ExpressionType.Default);
            Assert.AreEqual(typeof(void).ToTypeSlim(), ifThen.Type);
            Assert.AreEqual(ExpressionType.Conditional, ifThen.NodeType);

            var ifThenElse = ExpressionSlim.IfThenElse(test, ifTrue, ifFalse);
            Assert.AreSame(test, ifThenElse.Test);
            Assert.AreSame(ifTrue, ifThenElse.IfTrue);
            Assert.AreSame(ifFalse, ifThenElse.IfFalse);
            Assert.AreEqual(typeof(void).ToTypeSlim(), ifThenElse.Type);
            Assert.AreEqual(ExpressionType.Conditional, ifThenElse.NodeType);
        }

        [TestMethod]
        public void ExpressionSlim_MemberAccessFactoryTests()
        {
            var field = SlimType.GetField("Foo", fieldType: null);
            var prop = SlimType.GetProperty("Foo", propertyType: null, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            var instance = ExpressionSlim.Parameter(SlimType);

            var access = ExpressionSlim.MakeMemberAccess(instance: null, field);
            AssertMemberAccess(access, field);

            access = ExpressionSlim.Field(instance: null, field);
            AssertMemberAccess(access, field);

            access = ExpressionSlim.Field(instance, field);
            AssertMemberAccess(access, field, instance);

            access = ExpressionSlim.Property(instance: null, prop);
            AssertMemberAccess(access, prop);

            access = ExpressionSlim.Property(instance, prop);
            AssertMemberAccess(access, prop, instance);
        }

        private static void AssertMemberAccess(MemberExpressionSlim expr, MemberInfoSlim member, ExpressionSlim instance = null)
        {
            Assert.AreSame(member, expr.Member);
            Assert.AreEqual(ExpressionType.MemberAccess, expr.NodeType);
            Assert.AreSame(instance, expr.Expression);
        }

        [TestMethod]
        public void ExpressionSlim_MemberInitFactoryTests()
        {
            Expression<Func<MyObj>> e = () => new MyObj { X = 42 };
            var mie = (MemberInitExpressionSlim)e.Body.ToExpressionSlim();

            var m1 = ExpressionSlim.MemberInit(mie.NewExpression, mie.Bindings[0]);
            var m2 = ExpressionSlim.MemberInit(mie.NewExpression, new[] { mie.Bindings[0] }.AsEnumerable());

            var e1 = m1.ToExpression();
            var e2 = m2.ToExpression();

            var v1 = e1.Evaluate<MyObj>();
            var v2 = e2.Evaluate<MyObj>();

            Assert.AreEqual(42, v1.X);
            Assert.AreEqual(42, v2.X);
        }

        private sealed class MyObj
        {
#pragma warning disable 0649
            public int X;
#pragma warning restore
        }

        [TestMethod]
        public void ExpressionSlim_LambdaFactoryTests()
        {
            var body = ExpressionSlim.Parameter(SlimType);
            var lambda = ExpressionSlim.Lambda(delegateType: null, body, new[] { body });
            Assert.AreSame(body, lambda.Body);
            Assert.AreEqual(1, lambda.Parameters.Count);
            Assert.AreSame(body, lambda.Parameters[0]);
            Assert.AreEqual(ExpressionType.Lambda, lambda.NodeType);
        }

        [TestMethod]
        public void ExpressionSlim_ElementInitFactoryTests()
        {
            Expression<Func<List<int>>> e = () => new List<int> { 42 };
            var lie = (ListInitExpressionSlim)e.Body.ToExpressionSlim();

            var add = lie.Initializers[0].AddMethod;
            var x = lie.Initializers[0].Arguments[0];

            var i1 = ExpressionSlim.ElementInit(add, new[] { x });
            Assert.AreEqual(1, i1.ArgumentCount);
            Assert.AreSame(x, i1.GetArgument(0));

            var i2 = ExpressionSlim.ElementInit(add, new List<ExpressionSlim> { x });
            Assert.AreEqual(1, i2.ArgumentCount);
            Assert.AreSame(x, i2.GetArgument(0));
        }

        [TestMethod]
        public void ExpressionSlim_ListInitFactoryTests()
        {
            Expression<Func<List<int>>> e = () => new List<int> { 42 };
            var lie = (ListInitExpressionSlim)e.Body.ToExpressionSlim();

            var l1 = ExpressionSlim.ListInit(lie.NewExpression, lie.Initializers[0]);
            var l2 = ExpressionSlim.ListInit(lie.NewExpression, new[] { lie.Initializers[0] }.AsEnumerable());
            var l3 = ExpressionSlim.ListInit(lie.NewExpression, ExpressionSlim.ElementInit(lie.Initializers[0].AddMethod, lie.Initializers[0].Arguments.AsEnumerable()));
            var l4 = ExpressionSlim.ListInit(lie.NewExpression, ExpressionSlim.ElementInit(lie.Initializers[0].AddMethod, lie.Initializers[0].Arguments[0]));
            var l5 = ExpressionSlim.ListInit(lie.NewExpression, lie.Initializers[0].AddMethod, lie.Initializers[0].Arguments.Single());
            var l6 = ExpressionSlim.ListInit(lie.NewExpression, lie.Initializers[0].AddMethod, (IEnumerable<ExpressionSlim>)new[] { lie.Initializers[0].Arguments.Single() });

            var e1 = l1.ToExpression();
            var e2 = l2.ToExpression();
            var e3 = l3.ToExpression();
            var e4 = l4.ToExpression();
            var e5 = l5.ToExpression();
            var e6 = l6.ToExpression();

            var v1 = e1.Evaluate<List<int>>();
            var v2 = e2.Evaluate<List<int>>();
            var v3 = e3.Evaluate<List<int>>();
            var v4 = e4.Evaluate<List<int>>();
            var v5 = e5.Evaluate<List<int>>();
            var v6 = e6.Evaluate<List<int>>();

            Assert.AreEqual(1, v1.Count);
            Assert.AreEqual(1, v2.Count);
            Assert.AreEqual(1, v3.Count);
            Assert.AreEqual(1, v4.Count);
            Assert.AreEqual(1, v5.Count);
            Assert.AreEqual(1, v6.Count);

            Assert.AreEqual(42, v1[0]);
            Assert.AreEqual(42, v2[0]);
            Assert.AreEqual(42, v3[0]);
            Assert.AreEqual(42, v4[0]);
            Assert.AreEqual(42, v5[0]);
            Assert.AreEqual(42, v6[0]);
        }

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

        [TestMethod]
        public void ExpressionSlim_NewFactoryTests()
        {
            Expression<Func<List<int>>> e = () => new List<int>();
            var ne = (NewExpressionSlim)e.Body.ToExpressionSlim();

            var n1 = ExpressionSlim.New(ne.Constructor);
            var n2 = ExpressionSlim.New(ne.Constructor, Array.Empty<ExpressionSlim>());
            var n3 = ExpressionSlim.New(ne.Constructor, EmptyReadOnlyCollection<ExpressionSlim>.Instance);
            var n4 = ExpressionSlim.New(ne.Constructor, EmptyReadOnlyCollection<ExpressionSlim>.Instance, members: null);

            var e1 = n1.ToExpression();
            var e2 = n2.ToExpression();
            var e3 = n3.ToExpression();
            var e4 = n4.ToExpression();

            var v1 = e1.Evaluate<List<int>>();
            var v2 = e2.Evaluate<List<int>>();
            var v3 = e3.Evaluate<List<int>>();
            var v4 = e4.Evaluate<List<int>>();

            Assert.AreEqual(0, v1.Count);
            Assert.AreEqual(0, v2.Count);
            Assert.AreEqual(0, v3.Count);
            Assert.AreEqual(0, v4.Count);

            Expression<Func<int>> a = () => new { X = 43, Y = "foo" }.X;
            var na = (NewExpressionSlim)((MemberExpression)a.Body).Expression.ToExpressionSlim();

            var n5 = ExpressionSlim.New(na.Constructor, na.Arguments, na.Members);

            var e5 = n5.ToExpression();

            dynamic v5 = e5.Evaluate();
            Assert.AreEqual(43, v5.X);
            Assert.AreEqual("foo", v5.Y);
        }

#pragma warning restore IDE0050

        [TestMethod]
        public void ExpressionSlim_NewArrayFactoryTests()
        {
            var arg0 = ExpressionSlim.Default(SlimType);
            var arg1 = ExpressionSlim.Default(SlimType);

            var newArrExpr = ExpressionSlim.NewArrayBounds(SlimType, new[] { arg0, arg1 });
            Assert.AreEqual(SlimType, newArrExpr.ElementType);
            Assert.IsTrue(newArrExpr.Expressions.SequenceEqual(new[] { arg0, arg1 }));

            newArrExpr = ExpressionSlim.NewArrayBounds(SlimType, new[] { arg0, arg1 }.ToList());
            Assert.AreEqual(SlimType, newArrExpr.ElementType);
            Assert.IsTrue(newArrExpr.Expressions.SequenceEqual(new[] { arg0, arg1 }));

            newArrExpr = ExpressionSlim.NewArrayInit(SlimType, new[] { arg0, arg1 });
            Assert.AreEqual(SlimType, newArrExpr.ElementType);
            Assert.IsTrue(newArrExpr.Expressions.SequenceEqual(new[] { arg0, arg1 }));

            newArrExpr = ExpressionSlim.NewArrayInit(SlimType, new[] { arg0, arg1 }.ToList());
            Assert.AreEqual(SlimType, newArrExpr.ElementType);
            Assert.IsTrue(newArrExpr.Expressions.SequenceEqual(new[] { arg0, arg1 }));
        }

        [TestMethod]
        public void ExpressionSlim_UnaryFactoryTests()
        {
            var operand = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var array = Expression.Constant(new[] { 1, 2, 3 }).ToExpressionSlim();
            var err = Expression.Constant(new Exception()).ToExpressionSlim();
            var ex = typeof(Exception).ToTypeSlim();

            var unary = ExpressionSlim.Convert(operand, SlimType);
            AssertUnary(unary, operand, SlimType, ExpressionType.Convert);

            unary = ExpressionSlim.ConvertChecked(operand, SlimType);
            AssertUnary(unary, operand, SlimType, ExpressionType.ConvertChecked);

            unary = ExpressionSlim.TypeAs(operand, SlimType);
            AssertUnary(unary, operand, SlimType, ExpressionType.TypeAs);


            unary = ExpressionSlim.ArrayLength(array);
            AssertUnary(unary, array, type: null, ExpressionType.ArrayLength);


            unary = ExpressionSlim.Negate(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.Negate);

            unary = ExpressionSlim.Negate(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.Negate);

            unary = ExpressionSlim.NegateChecked(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.NegateChecked);

            unary = ExpressionSlim.NegateChecked(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.NegateChecked);

            unary = ExpressionSlim.Not(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.Not);

            unary = ExpressionSlim.Not(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.Not);

            unary = ExpressionSlim.OnesComplement(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.OnesComplement);

            unary = ExpressionSlim.OnesComplement(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.OnesComplement);

            unary = ExpressionSlim.UnaryPlus(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.UnaryPlus);

            unary = ExpressionSlim.UnaryPlus(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.UnaryPlus);


            unary = ExpressionSlim.Increment(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.Increment);

            unary = ExpressionSlim.Increment(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.Increment);

            unary = ExpressionSlim.Decrement(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.Decrement);

            unary = ExpressionSlim.Decrement(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.Decrement);


            unary = ExpressionSlim.PreDecrementAssign(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.PreDecrementAssign);

            unary = ExpressionSlim.PreDecrementAssign(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.PreDecrementAssign);

            unary = ExpressionSlim.PreIncrementAssign(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.PreIncrementAssign);

            unary = ExpressionSlim.PreIncrementAssign(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.PreIncrementAssign);

            unary = ExpressionSlim.PostDecrementAssign(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.PostDecrementAssign);

            unary = ExpressionSlim.PostDecrementAssign(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.PostDecrementAssign);

            unary = ExpressionSlim.PostIncrementAssign(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.PostIncrementAssign);

            unary = ExpressionSlim.PostIncrementAssign(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.PostIncrementAssign);


            unary = ExpressionSlim.Rethrow();
            AssertUnary(unary, operand: null, TypeSlimExtensions.VoidType, ExpressionType.Throw);

            unary = ExpressionSlim.Rethrow(type: null);
            AssertUnary(unary, operand: null, TypeSlimExtensions.VoidType, ExpressionType.Throw);

            unary = ExpressionSlim.Rethrow(ex);
            AssertUnary(unary, operand: null, ex, ExpressionType.Throw);

            unary = ExpressionSlim.Throw(err);
            AssertUnary(unary, err, TypeSlimExtensions.VoidType, ExpressionType.Throw);

            unary = ExpressionSlim.Throw(err, type: null);
            AssertUnary(unary, err, TypeSlimExtensions.VoidType, ExpressionType.Throw);

            unary = ExpressionSlim.Throw(err, ex);
            AssertUnary(unary, err, ex, ExpressionType.Throw);


            unary = ExpressionSlim.Unbox(operand, SlimType);
            AssertUnary(unary, operand, SlimType, ExpressionType.Unbox);


            unary = ExpressionSlim.IsFalse(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.IsFalse);

            unary = ExpressionSlim.IsFalse(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.IsFalse);

            unary = ExpressionSlim.IsTrue(operand);
            AssertUnary(unary, operand, type: null, ExpressionType.IsTrue);

            unary = ExpressionSlim.IsTrue(operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.IsTrue);

            /*
            unary = ExpressionSlim.MakeUnary(ExpressionType.UnaryPlus, operand, method: null);
            AssertUnary(unary, operand, type: null, ExpressionType.UnaryPlus);

            unary = ExpressionSlim.MakeUnary(ExpressionType.UnaryPlus, operand, method: null, type: null);
            AssertUnary(unary, operand, type: null, ExpressionType.UnaryPlus);
             */
        }

        [TestMethod]
        public void ExpressionSlim_MakeUnaryFactoryTests_ArgumentChecking()
        {
            var nt = ExpressionType.Call;
            var operand = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var method = ((MethodCallExpressionSlim)((Expression<Func<string>>)(() => "".ToUpper())).Body.ToExpressionSlim()).Method;
            var typ = TypeSlimExtensions.IntegerType;

            AssertEx.ThrowsException<ArgumentException>(() => ExpressionSlim.MakeUnary(nt, operand, typ), ex => Assert.IsTrue(ex.Message.Contains("not a valid unary expression type")));
            AssertEx.ThrowsException<ArgumentException>(() => ExpressionSlim.MakeUnary(nt, operand, typ, method), ex => Assert.IsTrue(ex.Message.Contains("not a valid unary expression type")));
        }

        [TestMethod]
        public void ExpressionSlim_MakeUnaryFactoryTests()
        {
            var operand = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var method = ((MethodCallExpressionSlim)((Expression<Func<string>>)(() => "".ToUpper())).Body.ToExpressionSlim()).Method;
            var typ = TypeSlimExtensions.IntegerType;

            // operand, method
            foreach (var type in new[]
            {
                ExpressionType.Negate,
                ExpressionType.NegateChecked,
                ExpressionType.Not,
                ExpressionType.IsFalse,
                ExpressionType.IsTrue,
                ExpressionType.OnesComplement,
                ExpressionType.UnaryPlus,
                ExpressionType.Increment,
                ExpressionType.Decrement,
                ExpressionType.PreIncrementAssign,
                ExpressionType.PreDecrementAssign,
                ExpressionType.PostIncrementAssign,
                ExpressionType.PostDecrementAssign,
            })
            {
                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ);
                    Assert.AreSame(res.Operand, operand);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Type);
                }

                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ, method);
                    Assert.AreSame(res.Operand, operand);
                    Assert.AreSame(res.Method, method);
                    Assert.IsNull(res.Type);
                }
            }

            // operand, type
            foreach (var type in new[]
            {
                ExpressionType.Throw,
                ExpressionType.TypeAs,
                ExpressionType.Unbox,
            })
            {
                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ);
                    Assert.AreSame(res.Operand, operand);
                    Assert.AreSame(res.Type, typ);
                    Assert.IsNull(res.Method);
                }

                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ, method);
                    Assert.AreSame(res.Operand, operand);
                    Assert.AreSame(res.Type, typ);
                    Assert.IsNull(res.Method);
                }
            }

            // operand, type, method
            foreach (var type in new[]
            {
                ExpressionType.Convert,
                ExpressionType.ConvertChecked,
            })
            {
                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ);
                    Assert.AreSame(res.Operand, operand);
                    Assert.AreSame(res.Type, typ);
                    Assert.IsNull(res.Method);
                }

                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ, method);
                    Assert.AreSame(res.Operand, operand);
                    Assert.AreSame(res.Type, typ);
                    Assert.AreSame(res.Method, method);
                }
            }

            // operand
            foreach (var type in new[]
            {
                ExpressionType.ArrayLength,
                ExpressionType.Quote,
            })
            {
                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ);
                    Assert.AreSame(res.Operand, operand);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Type);
                }

                {
                    var res = ExpressionSlim.MakeUnary(type, operand, typ, method);
                    Assert.AreSame(res.Operand, operand);
                    Assert.IsNull(res.Method);
                    Assert.IsNull(res.Type);
                }
            }
        }

        private static void AssertUnary(UnaryExpressionSlim expr, ExpressionSlim operand, TypeSlim type, ExpressionType nodeType)
        {
            Assert.AreSame(operand, expr.Operand);
            Assert.AreSame(type, expr.Type);
            Assert.AreEqual(nodeType, expr.NodeType);
            Assert.IsNull(expr.Method);
        }

        [TestMethod]
        public void ExpressionSlim_LabelTargetFactoryTests()
        {
            const string name = "foo";
            var type = SlimType;

            var l1 = ExpressionSlim.Label();
            Assert.AreEqual(s_void.Type, l1.Type);
            Assert.IsNull(l1.Name);

            var l2 = ExpressionSlim.Label(name);
            Assert.AreEqual(s_void.Type, l2.Type);
            Assert.AreEqual(name, l2.Name);

            var l3 = ExpressionSlim.Label(type);
            Assert.IsNull(l3.Name);
            Assert.AreEqual(type, l3.Type);

            var l4 = ExpressionSlim.Label(type, name);
            Assert.AreEqual(name, l4.Name);
            Assert.AreEqual(type, l4.Type);
        }

        [TestMethod]
        public void ExpressionSlim_LabelFactoryTests()
        {
            var t = ExpressionSlim.Label();

            var l1 = ExpressionSlim.Label(t);
            Assert.AreEqual(t, l1.Target);
            Assert.IsNull(l1.DefaultValue);

            var c = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var l2 = ExpressionSlim.Label(t, c);
            Assert.AreEqual(t, l2.Target);
            Assert.AreEqual(c, l2.DefaultValue);
        }

        [TestMethod]
        public void ExpressionSlim_BreakFactoryTests()
        {
            var t = ExpressionSlim.Label();
            var c = ExpressionSlim.Default(SlimType);

            var b1 = ExpressionSlim.Break(t);
            Assert.AreEqual(GotoExpressionKind.Break, b1.Kind);
            Assert.AreSame(t, b1.Target);
            Assert.IsNull(b1.Value);
            Assert.AreEqual(s_void.Type, b1.Type);

            var b2 = ExpressionSlim.Break(t, c);
            Assert.AreEqual(GotoExpressionKind.Break, b2.Kind);
            Assert.AreSame(t, b2.Target);
            Assert.AreEqual(c, b2.Value);
            Assert.AreEqual(s_void.Type, b2.Type);

            var b3 = ExpressionSlim.Break(t, SlimType);
            Assert.AreEqual(GotoExpressionKind.Break, b3.Kind);
            Assert.AreSame(t, b3.Target);
            Assert.IsNull(b3.Value);
            Assert.AreEqual(SlimType, b3.Type);

            var b4 = ExpressionSlim.Break(t, c, SlimType);
            Assert.AreEqual(GotoExpressionKind.Break, b4.Kind);
            Assert.AreSame(t, b4.Target);
            Assert.AreSame(c, b4.Value);
            Assert.AreEqual(SlimType, b4.Type);
        }

        [TestMethod]
        public void ExpressionSlim_ContinueFactoryTests()
        {
            var t = ExpressionSlim.Label();

            var c1 = ExpressionSlim.Continue(t);
            Assert.AreEqual(GotoExpressionKind.Continue, c1.Kind);
            Assert.AreSame(t, c1.Target);
            Assert.IsNull(c1.Value);
            Assert.AreEqual(s_void.Type, c1.Type); // TODO should the type be null instead?

            var c2 = ExpressionSlim.Continue(t, SlimType);
            Assert.AreEqual(GotoExpressionKind.Continue, c2.Kind);
            Assert.AreSame(t, c2.Target);
            Assert.IsNull(c2.Value);
            Assert.AreEqual(SlimType, c2.Type);
        }

        [TestMethod]
        public void ExpressionSlim_ReturnFactoryTests()
        {
            var t = ExpressionSlim.Label();
            var c = ExpressionSlim.Default(SlimType);

            var r1 = ExpressionSlim.Return(t);
            Assert.AreEqual(GotoExpressionKind.Return, r1.Kind);
            Assert.AreSame(t, r1.Target);
            Assert.IsNull(r1.Value);
            Assert.AreEqual(s_void.Type, r1.Type);

            var r2 = ExpressionSlim.Return(t, c);
            Assert.AreEqual(GotoExpressionKind.Return, r2.Kind);
            Assert.AreSame(t, r2.Target);
            Assert.AreEqual(c, r2.Value);
            Assert.AreEqual(s_void.Type, r2.Type);

            var r3 = ExpressionSlim.Return(t, SlimType);
            Assert.AreEqual(GotoExpressionKind.Return, r3.Kind);
            Assert.AreSame(t, r3.Target);
            Assert.IsNull(r3.Value);
            Assert.AreEqual(SlimType, r3.Type);

            var r4 = ExpressionSlim.Return(t, c, SlimType);
            Assert.AreEqual(GotoExpressionKind.Return, r4.Kind);
            Assert.AreSame(t, r4.Target);
            Assert.AreSame(c, r4.Value);
            Assert.AreEqual(SlimType, r4.Type);
        }

        [TestMethod]
        public void ExpressionSlim_GotoFactoryTests()
        {
            var t = ExpressionSlim.Label();
            var c = ExpressionSlim.Default(SlimType);

            var g1 = ExpressionSlim.Goto(t);
            Assert.AreEqual(GotoExpressionKind.Goto, g1.Kind);
            Assert.AreSame(t, g1.Target);
            Assert.IsNull(g1.Value);
            Assert.AreEqual(s_void.Type, g1.Type);

            var g2 = ExpressionSlim.Goto(t, c);
            Assert.AreEqual(GotoExpressionKind.Goto, g2.Kind);
            Assert.AreSame(t, g2.Target);
            Assert.AreEqual(c, g2.Value);
            Assert.AreEqual(s_void.Type, g2.Type);

            var g3 = ExpressionSlim.Goto(t, SlimType);
            Assert.AreEqual(GotoExpressionKind.Goto, g3.Kind);
            Assert.AreSame(t, g3.Target);
            Assert.IsNull(g3.Value);
            Assert.AreEqual(SlimType, g3.Type);

            var g4 = ExpressionSlim.Goto(t, c, SlimType);
            Assert.AreEqual(GotoExpressionKind.Goto, g4.Kind);
            Assert.AreSame(t, g4.Target);
            Assert.AreSame(c, g4.Value);
            Assert.AreEqual(SlimType, g4.Type);
        }

        [TestMethod]
        public void ExpressionSlim_BlockFactoryTests()
        {
            var a = ExpressionSlim.Constant(ObjectSlim.Create(0, SlimType, typeof(int)), SlimType);
            var b = ExpressionSlim.Constant(ObjectSlim.Create(1, SlimType, typeof(int)), SlimType);
            var c = ExpressionSlim.Constant(ObjectSlim.Create(2, SlimType, typeof(int)), SlimType);
            var cs = new List<ExpressionSlim> { a, b, c };

            var x = ExpressionSlim.Parameter(SlimType);
            var y = ExpressionSlim.Parameter(SlimType);
            var z = ExpressionSlim.Parameter(SlimType);
            var ps = new List<ParameterExpressionSlim> { x, y, z };

            {
                var blk = ExpressionSlim.Block(cs);
                AssertElementsAreSame(cs, blk.Expressions);
                Assert.AreEqual(0, blk.Variables.Count);
                Assert.IsNull(blk.Type);
                Assert.AreEqual(c, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(a, b, c);
                AssertElementsAreSame(new[] { a, b, c }, blk.Expressions);
                Assert.AreEqual(0, blk.Variables.Count);
                Assert.IsNull(blk.Type);
                Assert.AreEqual(c, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(ps, cs);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(cs, blk.Expressions);
                Assert.IsNull(blk.Type);
                Assert.AreEqual(c, blk.Result);

                blk = ExpressionSlim.Block(ps, ps);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(ps.Cast<ExpressionSlim>().ToArray(), blk.Expressions);
                Assert.IsNull(blk.Type);
                Assert.AreEqual(z, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(ps, a, b, c);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(cs, blk.Expressions);
                Assert.IsNull(blk.Type);
                Assert.AreEqual(c, blk.Result);

                blk = ExpressionSlim.Block(ps, x, y, z);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(ps.Cast<ExpressionSlim>().ToArray(), blk.Expressions);
                Assert.IsNull(blk.Type);
                Assert.AreEqual(z, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(SlimType, cs);
                AssertElementsAreSame(cs, blk.Expressions);
                Assert.AreEqual(0, blk.Variables.Count);
                Assert.AreSame(SlimType, blk.Type);
                Assert.AreEqual(c, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(SlimType, a, b, c);
                AssertElementsAreSame(new[] { a, b, c }, blk.Expressions);
                Assert.AreEqual(0, blk.Variables.Count);
                Assert.AreSame(SlimType, blk.Type);
                Assert.AreEqual(c, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(SlimType, ps, cs);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(cs, blk.Expressions);
                Assert.AreSame(SlimType, blk.Type);
                Assert.AreEqual(c, blk.Result);

                blk = ExpressionSlim.Block(SlimType, ps, ps);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(ps.Cast<ExpressionSlim>().ToArray(), blk.Expressions);
                Assert.AreSame(SlimType, blk.Type);
                Assert.AreEqual(z, blk.Result);
            }

            {
                var blk = ExpressionSlim.Block(SlimType, ps, a, b, c);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(cs, blk.Expressions);
                Assert.AreSame(SlimType, blk.Type);
                Assert.AreEqual(c, blk.Result);

                blk = ExpressionSlim.Block(SlimType, ps, x, y, z);
                AssertElementsAreSame(ps, blk.Variables);
                AssertElementsAreSame(ps.Cast<ExpressionSlim>().ToArray(), blk.Expressions);
                Assert.AreSame(SlimType, blk.Type);
                Assert.AreEqual(z, blk.Result);
            }

            {
                AssertEx.ThrowsException<ArgumentException>(
                    () => ExpressionSlim.Block(Array.Empty<ExpressionSlim>()),
                    ex => Assert.AreEqual("expressions", ex.ParamName)
                );
            }
        }

        [TestMethod]
        public void ExpressionSlim_LoopFactoryTests()
        {
            LoopExpressionSlim l;
            var b = ExpressionSlim.Parameter(SlimType);
            var @break = ExpressionSlim.Label();
            var @continue = ExpressionSlim.Label();

            l = ExpressionSlim.Loop(b);
            Assert.AreSame(b, l.Body);
            Assert.IsNull(l.BreakLabel);
            Assert.IsNull(l.ContinueLabel);

            l = ExpressionSlim.Loop(b, @break);
            Assert.AreSame(b, l.Body);
            Assert.AreSame(@break, l.BreakLabel);
            Assert.IsNull(l.ContinueLabel);

            l = ExpressionSlim.Loop(b, @break, @continue);
            Assert.AreSame(b, l.Body);
            Assert.AreSame(@break, l.BreakLabel);
            Assert.AreSame(@continue, l.ContinueLabel);
        }

        private static void AssertElementsAreSame<T>(ICollection<T> expected, ICollection<T> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var pair in expected.Zip(actual, (Expected, Actual) => (Expected, Actual)))
                Assert.AreSame(pair.Expected, pair.Actual);
        }

        [TestMethod]
        public void ExpressionSlim_SwitchFactoryTests()
        {
            // SwitchCase
            var b = ExpressionSlim.Parameter(SlimType);
            var x = ExpressionSlim.Parameter(SlimType);
            var y = ExpressionSlim.Parameter(SlimType);

            {
                var sc = ExpressionSlim.SwitchCase(b, x, y);
                Assert.AreSame(b, sc.Body);
                AssertElementsAreSame(new[] { x, y }, sc.TestValues);
            }

            {
                var ts = new List<ExpressionSlim> { x, y };
                var sc = ExpressionSlim.SwitchCase(b, ts);
                Assert.AreSame(b, sc.Body);
                AssertElementsAreSame(ts, sc.TestValues);
            }

            // Switch
            var v = ExpressionSlim.Parameter(SlimType);
            var d = ExpressionSlim.Parameter(SlimType);
            var m = ((MethodCallExpressionSlim)((Expression<Func<string>>)(() => "".ToUpper())).Body.ToExpressionSlim()).Method;
            var c = ExpressionSlim.SwitchCase(b, x, y);
            var t = SlimType;

            {
                var s = ExpressionSlim.Switch(v, c);
                AssertElementsAreSame(new[] { c }, s.Cases);
                Assert.IsNull(s.Comparison);
                Assert.IsNull(s.DefaultBody);
                Assert.AreSame(v, s.SwitchValue);
                Assert.IsNull(s.Type);
            }

            {
                var s = ExpressionSlim.Switch(v, d, c);
                AssertElementsAreSame(new[] { c }, s.Cases);
                Assert.IsNull(s.Comparison);
                Assert.AreSame(d, s.DefaultBody);
                Assert.AreSame(v, s.SwitchValue);
                Assert.IsNull(s.Type);
            }

            {
                var s = ExpressionSlim.Switch(v, d, m, c);
                AssertElementsAreSame(new[] { c }, s.Cases);
                Assert.AreSame(m, s.Comparison);
                Assert.AreSame(d, s.DefaultBody);
                Assert.AreSame(v, s.SwitchValue);
                Assert.IsNull(s.Type);
            }

            {
                var s = ExpressionSlim.Switch(t, v, d, m, c);
                AssertElementsAreSame(new[] { c }, s.Cases);
                Assert.AreSame(m, s.Comparison);
                Assert.AreSame(d, s.DefaultBody);
                Assert.AreSame(v, s.SwitchValue);
                Assert.AreSame(t, s.Type);
            }

            {
                var s = ExpressionSlim.Switch(v, d, m, new List<SwitchCaseSlim> { c });
                AssertElementsAreSame(new[] { c }, s.Cases);
                Assert.AreSame(m, s.Comparison);
                Assert.AreSame(d, s.DefaultBody);
                Assert.AreSame(v, s.SwitchValue);
                Assert.IsNull(s.Type);
            }

            {
                var s = ExpressionSlim.Switch(t, v, d, m, new List<SwitchCaseSlim> { c });
                AssertElementsAreSame(new[] { c }, s.Cases);
                Assert.AreSame(m, s.Comparison);
                Assert.AreSame(d, s.DefaultBody);
                Assert.AreSame(v, s.SwitchValue);
                Assert.AreSame(t, s.Type);
            }

            {
                AssertEx.ThrowsException<ArgumentNullException>(
                    () => ExpressionSlim.Switch(t, v, d, m, new List<SwitchCaseSlim> { null }),
                    ex => Assert.AreEqual("cases[0]", ex.ParamName)
                );
            }
        }

        [TestMethod]
        public void ExpressionSlim_TryCatchFinallyFaultFactoryTests()
        {
            // Catch
            {
                var e = ExpressionSlim.Parameter(SlimType);
                var c = ExpressionSlim.Catch(SlimType, e);
                Assert.AreSame(SlimType, c.Test);
                Assert.IsNull(c.Variable);
                Assert.AreSame(e, c.Body);
                Assert.IsNull(c.Filter);
            }

            {
                var e = ExpressionSlim.Parameter(SlimType);
                var f = ExpressionSlim.Empty();
                var c = ExpressionSlim.Catch(SlimType, e, f);
                Assert.AreSame(SlimType, c.Test);
                Assert.IsNull(c.Variable);
                Assert.AreSame(e, c.Body);
                Assert.AreSame(f, c.Filter);
            }

            {
                var p = ExpressionSlim.Parameter(SlimType);
                var e = ExpressionSlim.Parameter(SlimType);
                var c = ExpressionSlim.Catch(p, e);
                Assert.AreSame(SlimType, c.Test);
                Assert.AreEqual(p, c.Variable);
                Assert.AreSame(e, c.Body);
                Assert.IsNull(c.Filter);
            }

            {
                var p = ExpressionSlim.Parameter(SlimType);
                var e = ExpressionSlim.Parameter(SlimType);
                var f = ExpressionSlim.Empty();
                var c = ExpressionSlim.Catch(p, e, f);
                Assert.AreSame(SlimType, c.Test);
                Assert.AreEqual(p, c.Variable);
                Assert.AreSame(e, c.Body);
                Assert.AreSame(f, c.Filter);
            }

            // Try/Catch/Finally/Fault
            {
                var cs = new[] { ExpressionSlim.Catch(SlimType, ExpressionSlim.Parameter(SlimType)) };
                var b = ExpressionSlim.Parameter(SlimType);
                var @finally = ExpressionSlim.Parameter(SlimType);
                var @fault = ExpressionSlim.Parameter(SlimType);

                var trycatch = ExpressionSlim.TryCatch(b, cs);
                Assert.AreSame(b, trycatch.Body);
                AssertElementsAreSame(cs, trycatch.Handlers);
                Assert.IsNull(trycatch.Fault);
                Assert.IsNull(trycatch.Finally);
                Assert.IsNull(trycatch.Type);

                var trycatchfinally = ExpressionSlim.TryCatchFinally(b, @finally, cs);
                Assert.AreSame(b, trycatchfinally.Body);
                AssertElementsAreSame(cs, trycatchfinally.Handlers);
                Assert.IsNull(trycatchfinally.Fault);
                Assert.AreSame(@finally, trycatchfinally.Finally);
                Assert.IsNull(trycatchfinally.Type);

                var tryfinally = ExpressionSlim.TryFinally(b, @finally);
                Assert.AreSame(b, tryfinally.Body);
                Assert.AreEqual(0, tryfinally.Handlers.Count);
                Assert.IsNull(tryfinally.Fault);
                Assert.AreSame(@finally, tryfinally.Finally);
                Assert.IsNull(tryfinally.Type);

                var tryfault = ExpressionSlim.TryFault(b, @fault);
                Assert.AreSame(b, tryfault.Body);
                Assert.AreEqual(0, tryfault.Handlers.Count);
                Assert.AreSame(@fault, tryfault.Fault);
                Assert.IsNull(tryfault.Finally);
                Assert.IsNull(tryfault.Type);
            }

            try
            {
                ExpressionSlim.MakeTry(type: null, ExpressionSlim.Parameter(SlimType), ExpressionSlim.Parameter(SlimType), ExpressionSlim.Parameter(SlimType), Array.Empty<CatchBlockSlim>());
                Assert.Fail("ExpressionSlim.MakeTry did not throw when both finally and fault handlers expressions were specified.");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                ExpressionSlim.MakeTry(type: null, ExpressionSlim.Parameter(SlimType), @finally: null, ExpressionSlim.Parameter(SlimType), new[] { ExpressionSlim.Catch(SlimType, ExpressionSlim.Parameter(SlimType)) });
                Assert.Fail("ExpressionSlim.MakeTry did not throw when both catch blocks and fault handlers were specified.");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                ExpressionSlim.MakeTry(type: null, ExpressionSlim.Parameter(SlimType), @finally: null, fault: null, Array.Empty<CatchBlockSlim>());
                Assert.Fail("ExpressionSlim.MakeTry did not have fault, finally or catch.");
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void ExpressionSlim_IndexExpressionFactoryTests()
        {
            var e = Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<string, int>)), typeof(Dictionary<string, int>).GetProperty("Item"), new Expression[] { Expression.Constant("foo") });
            var ie = (IndexExpressionSlim)e.ToExpressionSlim();

            var i1 = ExpressionSlim.MakeIndex(ie.Object, ie.Indexer, ie.Arguments);
            var i2 = ExpressionSlim.Property(ie.Object, ie.Indexer, ie.Arguments.ToArray());
            var i3 = ExpressionSlim.Property(ie.Object, ie.Indexer, ie.Arguments);

            var e1 = (IndexExpression)i1.ToExpression();
            var e2 = (IndexExpression)i2.ToExpression();
            var e3 = (IndexExpression)i3.ToExpression();

            var l1 = Expression.Lambda<Func<Dictionary<string, int>, int>>(e1, (ParameterExpression)e1.Object);
            var l2 = Expression.Lambda<Func<Dictionary<string, int>, int>>(e2, (ParameterExpression)e2.Object);
            var l3 = Expression.Lambda<Func<Dictionary<string, int>, int>>(e3, (ParameterExpression)e3.Object);

            var d1 = l1.Compile();
            var d2 = l2.Compile();
            var d3 = l3.Compile();

            var dic = new Dictionary<string, int> { { "foo", 42 } };
            Assert.AreEqual(42, d1(dic));
            Assert.AreEqual(42, d2(dic));
            Assert.AreEqual(42, d3(dic));

            var a = ie.Arguments[0];

            Assert.AreEqual(1, i1.ArgumentCount);
            Assert.AreEqual(1, i2.ArgumentCount);
            Assert.AreEqual(1, i3.ArgumentCount);

            Assert.AreSame(a, i1.GetArgument(0));
            Assert.AreSame(a, i2.GetArgument(0));
            Assert.AreSame(a, i3.GetArgument(0));
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest0()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(default(MethodInfoSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var i = ExpressionSlim.Call(m);

            Assert.AreEqual(0, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(0));

            Assert.AreEqual(0, i.Arguments.Count);

            var n2 = (MethodCallExpressionSlim)new ArgVisitor(0).Visit(i);

            Assert.IsNull(n2.Object);
            Assert.AreSame(m, n2.Method);

            Assert.AreSame(i, n2);
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest1()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(default(MethodInfoSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");

            var i = ExpressionSlim.Call(m, a0);

            Assert.AreEqual(1, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(1));

            Assert.AreEqual(1, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);

            for (var j = 0; j < 1; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.IsNull(n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 1; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest2()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(default(MethodInfoSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");

            var i = ExpressionSlim.Call(m, a0, a1);

            Assert.AreEqual(2, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(2));

            Assert.AreEqual(2, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);

            for (var j = 0; j < 2; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.IsNull(n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 2; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest3()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(default(MethodInfoSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");

            var i = ExpressionSlim.Call(m, a0, a1, a2);

            Assert.AreEqual(3, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(3));

            Assert.AreEqual(3, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);

            for (var j = 0; j < 3; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.IsNull(n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 3; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest4()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(default(MethodInfoSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");

            var i = ExpressionSlim.Call(m, a0, a1, a2, a3);

            Assert.AreEqual(4, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(4));

            Assert.AreEqual(4, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);

            for (var j = 0; j < 4; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.IsNull(n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 4; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest5()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(default(MethodInfoSlim), e, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, default(ExpressionSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(m, e, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");
            var a4 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a4");

            var i = ExpressionSlim.Call(m, a0, a1, a2, a3, a4);

            Assert.AreEqual(5, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.AreSame(a4, i.GetArgument(4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(5));

            Assert.AreEqual(5, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);
            Assert.AreSame(a4, i.Arguments[4]);

            for (var j = 0; j < 5; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.IsNull(n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 5; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_StaticCallFactoryTest6()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");
            var a4 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a4");
            var a5 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a5");

            var i = ExpressionSlim.Call(m, a0, a1, a2, a3, a4, a5);

            Assert.AreEqual(6, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.AreSame(a4, i.GetArgument(4));
            Assert.AreSame(a5, i.GetArgument(5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(6));

            Assert.AreEqual(6, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);
            Assert.AreSame(a4, i.Arguments[4]);
            Assert.AreSame(a5, i.Arguments[5]);

            for (var j = 0; j < 6; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.IsNull(n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 6; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest0()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, default(MethodInfoSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var i = ExpressionSlim.Call(e, m);

            Assert.AreEqual(0, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(0));

            Assert.AreEqual(0, i.Arguments.Count);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            var n2 = (MethodCallExpressionSlim)new ArgVisitor(0).Visit(i);

            Assert.AreSame(e, n2.Object);
            Assert.AreSame(m, n2.Method);

            Assert.AreSame(i, n2);
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest1()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, default(MethodInfoSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");

            var i = ExpressionSlim.Call(e, m, a0);

            Assert.AreEqual(1, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(1));

            Assert.AreEqual(1, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 1; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(e, n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 1; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest2()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, default(MethodInfoSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");

            var i = ExpressionSlim.Call(e, m, a0, a1);

            Assert.AreEqual(2, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(2));

            Assert.AreEqual(2, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 2; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(e, n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 2; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest3()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, default(MethodInfoSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");

            var i = ExpressionSlim.Call(e, m, a0, a1, a2);

            Assert.AreEqual(3, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(3));

            Assert.AreEqual(3, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 3; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(e, n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 3; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest4()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, default(MethodInfoSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");

            var i = ExpressionSlim.Call(e, m, a0, a1, a2, a3);

            Assert.AreEqual(4, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(4));

            Assert.AreEqual(4, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 4; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(e, n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 4; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest5()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, default(MethodInfoSlim), e, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, default(ExpressionSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Call(e, m, e, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");
            var a4 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a4");

            var i = ExpressionSlim.Call(e, m, a0, a1, a2, a3, a4);

            Assert.AreEqual(5, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.AreSame(a4, i.GetArgument(4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(5));

            Assert.AreEqual(5, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);
            Assert.AreSame(a4, i.Arguments[4]);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 5; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(e, n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 5; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InstanceCallFactoryTest6()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "o");
            var m = typeof(int).ToTypeSlim().GetSimpleMethod("Bar", EmptyReadOnlyCollection<TypeSlim>.Instance, typeof(void).ToTypeSlim());

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");
            var a4 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a4");
            var a5 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a5");

            var i = ExpressionSlim.Call(e, m, a0, a1, a2, a3, a4, a5);

            Assert.AreEqual(6, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.AreSame(a4, i.GetArgument(4));
            Assert.AreSame(a5, i.GetArgument(5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(6));

            Assert.AreEqual(6, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);
            Assert.AreSame(a4, i.Arguments[4]);
            Assert.AreSame(a5, i.Arguments[5]);

            var n1 = (MethodCallExpressionSlim)new ObjectVisitor().Visit(i);

            Assert.AreNotSame(i.Object, n1.Object);
            Assert.AreSame(m, n1.Method);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 6; j++)
            {
                var n2 = (MethodCallExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(e, n2.Object);
                Assert.AreSame(m, n2.Method);

                for (var k = 0; k < 6; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InvokeFactoryTest0()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var f = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "f");

            var i = ExpressionSlim.Invoke(f);

            Assert.AreSame(f, i.Expression);

            Assert.AreEqual(0, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(0));

            Assert.AreEqual(0, i.Arguments.Count);

            var n1 = (InvocationExpressionSlim)new FuncVisitor().Visit(i);

            Assert.AreNotSame(i.Expression, n1.Expression);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            var n2 = (InvocationExpressionSlim)new ArgVisitor(0).Visit(i);
            Assert.AreSame(i, n2);
        }

        [TestMethod]
        public void ExpressionSlim_InvokeFactoryTest1()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var f = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "f");
            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");

            var i = ExpressionSlim.Invoke(f, a0);

            Assert.AreSame(f, i.Expression);

            Assert.AreEqual(1, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(1));

            Assert.AreEqual(1, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);

            var n1 = (InvocationExpressionSlim)new FuncVisitor().Visit(i);

            Assert.AreNotSame(i.Expression, n1.Expression);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 1; j++)
            {
                var n2 = (InvocationExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(i.Expression, n2.Expression);

                for (var k = 0; k < 1; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InvokeFactoryTest2()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var f = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "f");
            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");

            var i = ExpressionSlim.Invoke(f, a0, a1);

            Assert.AreSame(f, i.Expression);

            Assert.AreEqual(2, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(2));

            Assert.AreEqual(2, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);

            var n1 = (InvocationExpressionSlim)new FuncVisitor().Visit(i);

            Assert.AreNotSame(i.Expression, n1.Expression);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 2; j++)
            {
                var n2 = (InvocationExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(i.Expression, n2.Expression);

                for (var k = 0; k < 2; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InvokeFactoryTest3()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var f = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "f");
            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");

            var i = ExpressionSlim.Invoke(f, a0, a1, a2);

            Assert.AreSame(f, i.Expression);

            Assert.AreEqual(3, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(3));

            Assert.AreEqual(3, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);

            var n1 = (InvocationExpressionSlim)new FuncVisitor().Visit(i);

            Assert.AreNotSame(i.Expression, n1.Expression);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 3; j++)
            {
                var n2 = (InvocationExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(i.Expression, n2.Expression);

                for (var k = 0; k < 3; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InvokeFactoryTest4()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(default(ExpressionSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var f = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "f");
            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");

            var i = ExpressionSlim.Invoke(f, a0, a1, a2, a3);

            Assert.AreSame(f, i.Expression);

            Assert.AreEqual(4, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(4));

            Assert.AreEqual(4, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);

            var n1 = (InvocationExpressionSlim)new FuncVisitor().Visit(i);

            Assert.AreNotSame(i.Expression, n1.Expression);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 4; j++)
            {
                var n2 = (InvocationExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(i.Expression, n2.Expression);

                for (var k = 0; k < 4; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_InvokeFactoryTest5()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(default(ExpressionSlim), e, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, default(ExpressionSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.Invoke(e, e, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var f = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "f");
            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");
            var a4 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a4");

            var i = ExpressionSlim.Invoke(f, a0, a1, a2, a3, a4);

            Assert.AreSame(f, i.Expression);

            Assert.AreEqual(5, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.AreSame(a4, i.GetArgument(4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(5));

            Assert.AreEqual(5, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);
            Assert.AreSame(a4, i.Arguments[4]);

            var n1 = (InvocationExpressionSlim)new FuncVisitor().Visit(i);

            Assert.AreNotSame(i.Expression, n1.Expression);
            Assert.IsTrue(i.Arguments.SequenceEqual(n1.Arguments));

            for (var j = 0; j < 5; j++)
            {
                var n2 = (InvocationExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(i.Expression, n2.Expression);

                for (var k = 0; k < 5; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTestValueType()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(TypeSlim)));

            var t = typeof(int).ToTypeSlim();
            var i = ExpressionSlim.New(t);

            Assert.IsNull(i.Constructor);
            Assert.AreEqual(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(0, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(0));

            Assert.AreEqual(0, i.Arguments.Count);

            var n2 = (NewExpressionSlim)new ArgVisitor(0).Visit(i);

            Assert.IsNull(n2.Constructor);
            Assert.AreEqual(t, n2.Type);

            Assert.AreSame(i, n2);
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTest0()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var t = typeof(int).ToTypeSlim();
            var c = t.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(ConstructorInfoSlim)));

            var i = ExpressionSlim.New(c);

            Assert.AreSame(c, i.Constructor);
            Assert.AreSame(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(0, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(0));

            Assert.AreEqual(0, i.Arguments.Count);

            var n2 = (NewExpressionSlim)new ArgVisitor(0).Visit(i);

            Assert.AreSame(c, n2.Constructor);

            Assert.AreSame(i, n2);
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTest1()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var t = typeof(int).ToTypeSlim();
            var c = t.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(ConstructorInfoSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");

            var i = ExpressionSlim.New(c, a0);

            Assert.AreSame(c, i.Constructor);
            Assert.AreSame(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(1, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(1));

            Assert.AreEqual(1, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);

            for (var j = 0; j < 1; j++)
            {
                var n2 = (NewExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(c, n2.Constructor);

                for (var k = 0; k < 1; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTest2()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var t = typeof(int).ToTypeSlim();
            var c = t.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(ConstructorInfoSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");

            var i = ExpressionSlim.New(c, a0, a1);

            Assert.AreSame(c, i.Constructor);
            Assert.AreSame(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(2, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(2));

            Assert.AreEqual(2, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);

            for (var j = 0; j < 2; j++)
            {
                var n2 = (NewExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(c, n2.Constructor);

                for (var k = 0; k < 2; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTest3()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var t = typeof(int).ToTypeSlim();
            var c = t.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(ConstructorInfoSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");

            var i = ExpressionSlim.New(c, a0, a1, a2);

            Assert.AreSame(c, i.Constructor);
            Assert.AreSame(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(3, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(3));

            Assert.AreEqual(3, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);

            for (var j = 0; j < 3; j++)
            {
                var n2 = (NewExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(c, n2.Constructor);

                for (var k = 0; k < 3; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTest4()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var t = typeof(int).ToTypeSlim();
            var c = t.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(ConstructorInfoSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");

            var i = ExpressionSlim.New(c, a0, a1, a2, a3);

            Assert.AreSame(c, i.Constructor);
            Assert.AreSame(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(4, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(4));

            Assert.AreEqual(4, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);

            for (var j = 0; j < 4; j++)
            {
                var n2 = (NewExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(c, n2.Constructor);

                for (var k = 0; k < 4; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        [TestMethod]
        public void ExpressionSlim_NewFactoryTest5()
        {
            var e = ExpressionSlim.Parameter(typeof(int).ToTypeSlim());
            var t = typeof(int).ToTypeSlim();
            var c = t.GetConstructor(EmptyReadOnlyCollection<TypeSlim>.Instance);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(default(ConstructorInfoSlim), e, e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, default(ExpressionSlim), e, e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, default(ExpressionSlim), e, e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, e, default(ExpressionSlim), e, e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, e, e, default(ExpressionSlim), e));
            Assert.ThrowsException<ArgumentNullException>(() => ExpressionSlim.New(c, e, e, e, e, default(ExpressionSlim)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var a0 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a0");
            var a1 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a1");
            var a2 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a2");
            var a3 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a3");
            var a4 = ExpressionSlim.Parameter(typeof(int).ToTypeSlim(), "a4");

            var i = ExpressionSlim.New(c, a0, a1, a2, a3, a4);

            Assert.AreSame(c, i.Constructor);
            Assert.AreSame(t, i.Type);
            Assert.IsNull(i.Members);

            Assert.AreEqual(5, i.ArgumentCount);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(-1));
            Assert.AreSame(a0, i.GetArgument(0));
            Assert.AreSame(a1, i.GetArgument(1));
            Assert.AreSame(a2, i.GetArgument(2));
            Assert.AreSame(a3, i.GetArgument(3));
            Assert.AreSame(a4, i.GetArgument(4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => i.GetArgument(5));

            Assert.AreEqual(5, i.Arguments.Count);

            Assert.AreSame(a0, i.Arguments[0]);
            Assert.AreSame(a1, i.Arguments[1]);
            Assert.AreSame(a2, i.Arguments[2]);
            Assert.AreSame(a3, i.Arguments[3]);
            Assert.AreSame(a4, i.Arguments[4]);

            for (var j = 0; j < 5; j++)
            {
                var n2 = (NewExpressionSlim)new ArgVisitor(j).Visit(i);

                Assert.AreSame(c, n2.Constructor);

                for (var k = 0; k < 5; k++)
                {
                    if (k == j)
                    {
                        Assert.AreNotSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                    else
                    {
                        Assert.AreSame(i.GetArgument(k), n2.GetArgument(k));
                    }
                }
            }
        }

        private sealed class ObjectVisitor : ExpressionSlimVisitor
        {
            protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                if (node.Name == "o")
                    return ExpressionSlim.Parameter(node.Type, node.Name);

                return node;
            }
        }

        private sealed class FuncVisitor : ExpressionSlimVisitor
        {
            protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                if (node.Name == "f")
                    return ExpressionSlim.Parameter(node.Type, node.Name);

                return node;
            }
        }

        private sealed class ArgVisitor : ExpressionSlimVisitor
        {
            private readonly int _index;

            public ArgVisitor(int index)
            {
                _index = index;
            }

            protected internal override ExpressionSlim VisitParameter(ParameterExpressionSlim node)
            {
                if (node.Name == "a" + _index)
                    return ExpressionSlim.Parameter(node.Type, node.Name);

                return node;
            }
        }
    }
}
