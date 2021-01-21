// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0049 // Name can be simplified.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#if !USE_SLIM
using System.Runtime.CompilerServices;

using CS = Microsoft.CSharp.RuntimeBinder;
#endif

namespace Tests.System.Linq.CompilerServices
{
#if USE_SLIM
    #region Aliases

    using ConstructorInfo = global::System.Reflection.ConstructorInfoSlim;
    using ElementInit = global::System.Linq.Expressions.ElementInitSlim;
    using Expression = global::System.Linq.Expressions.ExpressionSlim;
    using ExpressionFactory = global::System.Linq.Expressions.ExpressionSlimFactory;
    using ExpressionUnsafeFactory = global::System.Linq.Expressions.ExpressionSlimUnsafeFactory;
    using FieldInfo = global::System.Reflection.FieldInfoSlim;
    using IExpressionFactory = global::System.Linq.Expressions.IExpressionSlimFactory;
    using MemberBinding = global::System.Linq.Expressions.MemberBindingSlim;
    using MemberInfo = global::System.Reflection.MemberInfoSlim;
    using MethodInfo = global::System.Reflection.MethodInfoSlim;
    using Object = global::System.ObjectSlim;
    using ParameterExpression = global::System.Linq.Expressions.ParameterExpressionSlim;
    using PropertyInfo = global::System.Reflection.PropertyInfoSlim;
    using SwitchCase = global::System.Linq.Expressions.SwitchCaseSlim;
    using Type = global::System.Reflection.TypeSlim;

    #endregion
#endif

    [TestClass]
#if USE_SLIM
    public class UnsafeExpressionSlimTests
#else
    public class UnsafeExpressionTests
#endif
    {
#if !USE_SLIM
        [TestMethod]
        public void ExpressionFactory_ExpressionUnsafe_Static()
        {
            var ctor = typeof(ExpressionUnsafe).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single();
            var eu = ctor.Invoke(Array.Empty<object>());
            Assert.IsNotNull(eu);
        }
#endif

        [TestMethod]
        public void ExpressionFactory_Arrays()
        {
            var arr1 = Expression.Constant(ObjectOf(new int[1]));
            var arr2 = Expression.Constant(ObjectOf(new int[1, 1]));
            var arr3 = Expression.Constant(ObjectOf(new int[1, 1, 1]));

            var i = Expression.Constant(ObjectOf(0));
            var j = Expression.Constant(ObjectOf(1));
            var k = Expression.Constant(ObjectOf(2));

            AssertExpr(f => f.ArrayAccess(arr1, i));
            AssertExpr(f => f.ArrayAccess(arr1, new Expression[] { i }));
            AssertExpr(f => f.ArrayAccess(arr1, new List<Expression> { i }));

            AssertExpr(f => f.ArrayAccess(arr2, i, j));
            AssertExpr(f => f.ArrayAccess(arr2, new Expression[] { i, j }));
            AssertExpr(f => f.ArrayAccess(arr2, new List<Expression> { i, j }));

            AssertExpr(f => f.ArrayAccess(arr3, i, j, k));
            AssertExpr(f => f.ArrayAccess(arr3, new Expression[] { i, j, k }));
            AssertExpr(f => f.ArrayAccess(arr3, new List<Expression> { i, j, k }));

            AssertExpr(f => f.ArrayIndex(arr1, i));

#if !USE_SLIM
            AssertExpr(f => f.ArrayIndex(arr1, new Expression[] { i }));
            AssertExpr(f => f.ArrayIndex(arr1, new List<Expression> { i }));

            AssertExpr(f => f.ArrayIndex(arr2, i, j));
            AssertExpr(f => f.ArrayIndex(arr2, new Expression[] { i, j }));
            AssertExpr(f => f.ArrayIndex(arr2, new List<Expression> { i, j }));

            AssertExpr(f => f.ArrayIndex(arr3, i, j, k));
            AssertExpr(f => f.ArrayIndex(arr3, new Expression[] { i, j, k }));
            AssertExpr(f => f.ArrayIndex(arr3, new List<Expression> { i, j, k }));
#endif

            AssertExpr(f => f.ArrayLength(arr1));

            AssertExpr(f => f.NewArrayBounds(TypeOf(typeof(int)), new Expression[] { i }));
            AssertExpr(f => f.NewArrayBounds(TypeOf(typeof(int)), new List<Expression> { i }.Select(x => x)));

            AssertExpr(f => f.NewArrayBounds(TypeOf(typeof(int)), new Expression[] { i, j }));
            AssertExpr(f => f.NewArrayBounds(TypeOf(typeof(int)), new List<Expression> { i, j }.Select(x => x)));

            AssertExpr(f => f.NewArrayInit(TypeOf(typeof(int)), new Expression[] { i }));
            AssertExpr(f => f.NewArrayInit(TypeOf(typeof(int)), new List<Expression> { i }.Select(x => x)));

            AssertExpr(f => f.NewArrayInit(TypeOf(typeof(int)), new Expression[] { i, j }));
            AssertExpr(f => f.NewArrayInit(TypeOf(typeof(int)), new List<Expression> { i, j }.Select(x => x)));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Arithmetic()
        {
            var x1 = Expression.Constant(ObjectOf(1));
            var x2 = Expression.Constant(ObjectOf(2));

            var d1 = Expression.Constant(ObjectOf(1.0));
            var d2 = Expression.Constant(ObjectOf(2.0));

            var dt = Expression.Constant(ObjectOf(DateTime.Now));
            var ts = Expression.Constant(ObjectOf(TimeSpan.Zero));

            var b1 = Expression.Constant(ObjectOf(new BigInteger(1)));
            var b2 = Expression.Constant(ObjectOf(new BigInteger(2)));

            var p1 = Expression.Parameter(TypeOf(typeof(int)));
            var p2 = Expression.Parameter(TypeOf(typeof(DateTime)));
            var p3 = Expression.Parameter(TypeOf(typeof(BigInteger)));
            var p4 = Expression.Parameter(TypeOf(typeof(double)));

            var q = Expression.Parameter(TypeOf(typeof(BigInteger)));
            var i = Expression.Lambda(q, q);

            var r = Expression.Parameter(TypeOf(typeof(double)));
            var j = Expression.Lambda(r, r);

            var bAdd = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Addition", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bSub = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Subtraction", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bMul = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Multiply", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bDiv = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Division", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bMod = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Modulus", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bLsh = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_LeftShift", new[] { typeof(BigInteger), typeof(int) }));
            var bRsh = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_RightShift", new[] { typeof(BigInteger), typeof(int) }));
            var pow = (MethodInfo)GetMember(typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) }));

            Assert.IsNotNull(bAdd);
            Assert.IsNotNull(bSub);
            Assert.IsNotNull(bMul);
            Assert.IsNotNull(bDiv);
            Assert.IsNotNull(bMod);
            Assert.IsNotNull(pow);

            AssertExpr(f => f.Add(x1, x2));
            AssertExpr(f => f.Add(x1, x2, method: null));
            AssertExpr(f => f.Add(dt, ts));
            AssertExpr(f => f.Add(b1, b2, bAdd));

            AssertExpr(f => f.AddAssign(p1, x2));
            AssertExpr(f => f.AddAssign(p1, x2, method: null));
            AssertExpr(f => f.AddAssign(p2, ts));
            AssertExpr(f => f.AddAssign(p3, b2, bAdd));
            AssertExpr(f => f.AddAssign(p3, b2, bAdd, i));

            AssertExpr(f => f.AddChecked(x1, x2));
            AssertExpr(f => f.AddChecked(x1, x2, method: null));
            AssertExpr(f => f.AddChecked(dt, ts));
            AssertExpr(f => f.AddChecked(b1, b2, bAdd));

            AssertExpr(f => f.AddAssignChecked(p1, x2));
            AssertExpr(f => f.AddAssignChecked(p1, x2, method: null));
            AssertExpr(f => f.AddAssignChecked(p2, ts));
            AssertExpr(f => f.AddAssignChecked(p3, b2, bAdd));
            AssertExpr(f => f.AddAssignChecked(p3, b2, bAdd, i));

            AssertExpr(f => f.Subtract(x1, x2));
            AssertExpr(f => f.Subtract(x1, x2, method: null));
            AssertExpr(f => f.Subtract(dt, ts));
            AssertExpr(f => f.Subtract(b1, b2, bSub));

            AssertExpr(f => f.SubtractAssign(p1, x2));
            AssertExpr(f => f.SubtractAssign(p1, x2, method: null));
            AssertExpr(f => f.SubtractAssign(p2, ts));
            AssertExpr(f => f.SubtractAssign(p3, b2, bSub));
            AssertExpr(f => f.SubtractAssign(p3, b2, bSub, i));

            AssertExpr(f => f.SubtractChecked(x1, x2));
            AssertExpr(f => f.SubtractChecked(x1, x2, method: null));
            AssertExpr(f => f.SubtractChecked(dt, ts));
            AssertExpr(f => f.SubtractChecked(b1, b2, bSub));

            AssertExpr(f => f.SubtractAssignChecked(p1, x2));
            AssertExpr(f => f.SubtractAssignChecked(p1, x2, method: null));
            AssertExpr(f => f.SubtractAssignChecked(p2, ts));
            AssertExpr(f => f.SubtractAssignChecked(p3, b2, bSub));
            AssertExpr(f => f.SubtractAssignChecked(p3, b2, bSub, i));

            AssertExpr(f => f.Multiply(x1, x2));
            AssertExpr(f => f.Multiply(x1, x2, method: null));
            AssertExpr(f => f.Multiply(b1, b2, bMul));

            AssertExpr(f => f.MultiplyAssign(p1, x2));
            AssertExpr(f => f.MultiplyAssign(p1, x2, method: null));
            AssertExpr(f => f.MultiplyAssign(p3, b2, bMul));
            AssertExpr(f => f.MultiplyAssign(p3, b2, bMul, i));

            AssertExpr(f => f.MultiplyChecked(x1, x2));
            AssertExpr(f => f.MultiplyChecked(x1, x2, method: null));
            AssertExpr(f => f.MultiplyChecked(b1, b2, bMul));

            AssertExpr(f => f.MultiplyAssignChecked(p1, x2));
            AssertExpr(f => f.MultiplyAssignChecked(p1, x2, method: null));
            AssertExpr(f => f.MultiplyAssignChecked(p3, b2, bMul));
            AssertExpr(f => f.MultiplyAssignChecked(p3, b2, bMul, i));

            AssertExpr(f => f.Divide(x1, x2));
            AssertExpr(f => f.Divide(x1, x2, method: null));
            AssertExpr(f => f.Divide(b1, b2, bDiv));

            AssertExpr(f => f.DivideAssign(p1, x2));
            AssertExpr(f => f.DivideAssign(p1, x2, method: null));
            AssertExpr(f => f.DivideAssign(p3, b2, bDiv));
            AssertExpr(f => f.DivideAssign(p3, b2, bDiv, i));

            AssertExpr(f => f.Modulo(x1, x2));
            AssertExpr(f => f.Modulo(x1, x2, method: null));
            AssertExpr(f => f.Modulo(b1, b2, bMod));

            AssertExpr(f => f.ModuloAssign(p1, x2));
            AssertExpr(f => f.ModuloAssign(p1, x2, method: null));
            AssertExpr(f => f.ModuloAssign(p3, b2, bMod));
            AssertExpr(f => f.ModuloAssign(p3, b2, bMod, i));

            AssertExpr(f => f.LeftShift(x1, x2));
            AssertExpr(f => f.LeftShift(x1, x2, method: null));
            AssertExpr(f => f.LeftShift(b1, x2, bLsh));

            AssertExpr(f => f.LeftShiftAssign(p1, x2));
            AssertExpr(f => f.LeftShiftAssign(p1, x2, method: null));
            AssertExpr(f => f.LeftShiftAssign(p3, x2, bLsh));
            AssertExpr(f => f.LeftShiftAssign(p3, x2, bLsh, i));

            AssertExpr(f => f.RightShift(x1, x2));
            AssertExpr(f => f.RightShift(x1, x2, method: null));
            AssertExpr(f => f.RightShift(b1, x2, bRsh));

            AssertExpr(f => f.RightShiftAssign(p1, x2));
            AssertExpr(f => f.RightShiftAssign(p1, x2, method: null));
            AssertExpr(f => f.RightShiftAssign(p3, x2, bRsh));
            AssertExpr(f => f.RightShiftAssign(p3, x2, bRsh, i));

            AssertExpr(f => f.Power(d1, d2));
            AssertExpr(f => f.Power(d1, d2, method: null));
            AssertExpr(f => f.Power(d1, d2, pow));

            AssertExpr(f => f.PowerAssign(p4, d2));
            AssertExpr(f => f.PowerAssign(p4, d2, method: null));
            AssertExpr(f => f.PowerAssign(p4, d2, pow));
            AssertExpr(f => f.PowerAssign(p4, d2, pow, j));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Bitwise()
        {
            var x1 = Expression.Constant(ObjectOf(1));
            var x2 = Expression.Constant(ObjectOf(2));

            var b1 = Expression.Constant(ObjectOf(new BigInteger(1)));
            var b2 = Expression.Constant(ObjectOf(new BigInteger(2)));

            var p1 = Expression.Parameter(TypeOf(typeof(int)));
            var p2 = Expression.Parameter(TypeOf(typeof(BigInteger)));

            var q = Expression.Parameter(TypeOf(typeof(BigInteger)));
            var i = Expression.Lambda(q, q);

            var bAnd = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_BitwiseAnd", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bOr = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_BitwiseOr", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bXor = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_ExclusiveOr", new[] { typeof(BigInteger), typeof(BigInteger) }));

            Assert.IsNotNull(bAnd);
            Assert.IsNotNull(bOr);
            Assert.IsNotNull(bXor);

            AssertExpr(f => f.And(x1, x2));
            AssertExpr(f => f.And(x1, x2, method: null));
            AssertExpr(f => f.And(b1, b2, bAnd));

            AssertExpr(f => f.AndAssign(p1, x2));
            AssertExpr(f => f.AndAssign(p1, x2, method: null));
            AssertExpr(f => f.AndAssign(p2, b2, bAnd));
            AssertExpr(f => f.AndAssign(p2, b2, bAnd, i));

            AssertExpr(f => f.Or(x1, x2));
            AssertExpr(f => f.Or(x1, x2, method: null));
            AssertExpr(f => f.Or(b1, b2, bOr));

            AssertExpr(f => f.OrAssign(p1, x2));
            AssertExpr(f => f.OrAssign(p1, x2, method: null));
            AssertExpr(f => f.OrAssign(p2, b2, bOr));
            AssertExpr(f => f.OrAssign(p2, b2, bOr, i));

            AssertExpr(f => f.ExclusiveOr(x1, x2));
            AssertExpr(f => f.ExclusiveOr(x1, x2, method: null));
            AssertExpr(f => f.ExclusiveOr(b1, b2, bXor));

            AssertExpr(f => f.ExclusiveOrAssign(p1, x2));
            AssertExpr(f => f.ExclusiveOrAssign(p1, x2, method: null));
            AssertExpr(f => f.ExclusiveOrAssign(p2, b2, bXor));
            AssertExpr(f => f.ExclusiveOrAssign(p2, b2, bXor, i));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Logic()
        {
            var b1 = Expression.Constant(ObjectOf(true));
            var b2 = Expression.Constant(ObjectOf(false));

            var b = new MyBool();

            var c1 = Expression.Constant(ObjectOf(b));
            var c2 = Expression.Constant(ObjectOf(b));

            var bAnd = (MethodInfo)GetMember(typeof(MyBool).GetMethod("op_BitwiseAnd", new[] { typeof(MyBool), typeof(MyBool) }));
            var bOr = (MethodInfo)GetMember(typeof(MyBool).GetMethod("op_BitwiseOr", new[] { typeof(MyBool), typeof(MyBool) }));

            Assert.IsNotNull(bAnd);
            Assert.IsNotNull(bOr);

            AssertExpr(f => f.AndAlso(b1, b2));
            AssertExpr(f => f.AndAlso(b1, b2, method: null));
            AssertExpr(f => f.AndAlso(c1, c2));
            AssertExpr(f => f.AndAlso(c1, c2, bAnd));

            AssertExpr(f => f.OrElse(b1, b2));
            AssertExpr(f => f.OrElse(b1, b2, method: null));
            AssertExpr(f => f.OrElse(c1, c2));
            AssertExpr(f => f.OrElse(c1, c2, bOr));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Make_Basics()
        {
            var x = Expression.Constant(ObjectOf(1));
            var y = Expression.Constant(ObjectOf(2));
            var a = Expression.Parameter(TypeOf(typeof(int)));

            var b = Expression.Constant(ObjectOf(new BigInteger(42)));
            var c = Expression.Constant(ObjectOf(new BigInteger(43)));
            var d = Expression.Parameter(TypeOf(typeof(BigInteger)));

            var add = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Addition", new[] { typeof(BigInteger), typeof(BigInteger) }));

            var p = Expression.Parameter(TypeOf(typeof(BigInteger)));
            var i = Expression.Lambda(p, p);

            AssertExpr(f => f.MakeBinary(ExpressionType.Add, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Add, x, y, liftToNull: false, method: null));
            AssertExpr(f => f.MakeBinary(ExpressionType.Add, x, y, liftToNull: false, method: null, conversion: null));

            AssertExpr(f => f.MakeBinary(ExpressionType.Add, x, y, liftToNull: true, method: null));
            AssertExpr(f => f.MakeBinary(ExpressionType.Add, x, y, liftToNull: true, method: null, conversion: null));

            AssertExpr(f => f.MakeBinary(ExpressionType.Add, b, c));
            AssertExpr(f => f.MakeBinary(ExpressionType.Add, b, c, liftToNull: false, method: null));
            AssertExpr(f => f.MakeBinary(ExpressionType.Add, b, c, liftToNull: false, method: null, conversion: null));

            AssertExpr(f => f.MakeBinary(ExpressionType.Add, b, c, liftToNull: false, add));
            AssertExpr(f => f.MakeBinary(ExpressionType.Add, b, c, liftToNull: false, add, conversion: null));

            AssertExpr(f => f.MakeBinary(ExpressionType.Add, b, c, liftToNull: false, add, i));

            AssertExpr(f => f.MakeBinary(ExpressionType.AddAssign, a, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.AddAssign, d, c));
            AssertExpr(f => f.MakeBinary(ExpressionType.AddAssign, d, c, liftToNull: false, add));
            AssertExpr(f => f.MakeBinary(ExpressionType.AddAssign, d, c, liftToNull: false, add, i));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Make_Switch()
        {
            var x = Expression.Constant(ObjectOf(1));
            var y = Expression.Constant(ObjectOf(2));
            var p = Expression.Parameter(TypeOf(typeof(int)));
            var xs = Expression.Constant(ObjectOf(new[] { 1, 2, 3 }));
            var s = Expression.Constant(ObjectOf("bar"));
            var t = Expression.Constant(ObjectOf("foo"));
            var b = Expression.Constant(ObjectOf(true));
            var c = Expression.Constant(ObjectOf(false));
            var d = Expression.Constant(ObjectOf(1.0));
            var e = Expression.Constant(ObjectOf(2.0));
            var q = Expression.Parameter(TypeOf(typeof(double)));

            AssertExpr(f => f.MakeBinary(ExpressionType.Add, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.AddChecked, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Subtract, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.SubtractChecked, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Multiply, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.MultiplyChecked, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Divide, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Modulo, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Power, d, e));

            AssertExpr(f => f.MakeBinary(ExpressionType.And, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.Or, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.ExclusiveOr, x, y));

            AssertExpr(f => f.MakeBinary(ExpressionType.Equal, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.NotEqual, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.LessThan, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.LessThanOrEqual, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.GreaterThan, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.GreaterThanOrEqual, x, y));

            AssertExpr(f => f.MakeBinary(ExpressionType.LeftShift, x, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.RightShift, x, y));

            AssertExpr(f => f.MakeBinary(ExpressionType.Assign, p, y));

            AssertExpr(f => f.MakeBinary(ExpressionType.AddAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.AddAssignChecked, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.SubtractAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.SubtractAssignChecked, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.MultiplyAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.MultiplyAssignChecked, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.DivideAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.ModuloAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.PowerAssign, q, e));

            AssertExpr(f => f.MakeBinary(ExpressionType.AndAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.OrAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.ExclusiveOrAssign, p, y));

            AssertExpr(f => f.MakeBinary(ExpressionType.LeftShiftAssign, p, y));
            AssertExpr(f => f.MakeBinary(ExpressionType.RightShiftAssign, p, y));

            AssertExpr(f => f.MakeBinary(ExpressionType.ArrayIndex, xs, x));

            AssertExpr(f => f.MakeBinary(ExpressionType.Coalesce, s, t));

            AssertExpr(f => f.MakeBinary(ExpressionType.AndAlso, b, c));
            AssertExpr(f => f.MakeBinary(ExpressionType.OrElse, b, c));

            Assert.ThrowsException<ArgumentException>(() => ExpressionFactory.Instance.MakeBinary(ExpressionType.Lambda, x, y));
            Assert.ThrowsException<ArgumentException>(() => ExpressionUnsafeFactory.Instance.MakeBinary(ExpressionType.Lambda, x, y));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Misc()
        {
            var p = Expression.Parameter(TypeOf(typeof(int)));
            var v = Expression.Constant(ObjectOf(42));

            var s1 = Expression.Constant(ObjectOf(null, typeof(string)), TypeOf(typeof(string)));
            var s2 = Expression.Constant(ObjectOf("bar"));
            var s = Expression.Parameter(TypeOf(typeof(string)));
            var i = Expression.Lambda(s, s);

            AssertExpr(f => f.Assign(p, v));

            AssertExpr(f => f.Coalesce(s1, s2));
            AssertExpr(f => f.Coalesce(s1, s2, i));

            AssertExpr(f => f.ReferenceEqual(s1, s2));
            AssertExpr(f => f.ReferenceNotEqual(s1, s2));
        }

        [TestMethod]
        public void ExpressionFactory_Binary_Relational()
        {
            var x1 = Expression.Constant(ObjectOf(1));
            var x2 = Expression.Constant(ObjectOf(2));

            var b1 = Expression.Constant(ObjectOf(new BigInteger(1)));
            var b2 = Expression.Constant(ObjectOf(new BigInteger(2)));

            var bEq = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Equality", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bNe = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Inequality", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bGt = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_GreaterThan", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bGe = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_GreaterThanOrEqual", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bLt = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_LessThan", new[] { typeof(BigInteger), typeof(BigInteger) }));
            var bLe = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_LessThanOrEqual", new[] { typeof(BigInteger), typeof(BigInteger) }));

            Assert.IsNotNull(bEq);
            Assert.IsNotNull(bNe);
            Assert.IsNotNull(bGt);
            Assert.IsNotNull(bGe);
            Assert.IsNotNull(bLt);
            Assert.IsNotNull(bLe);

            AssertExpr(f => f.Equal(x1, x2));
            AssertExpr(f => f.Equal(x1, x2, liftToNull: false, method: null));
            AssertExpr(f => f.Equal(b1, b2, liftToNull: false, bEq));

            AssertExpr(f => f.NotEqual(x1, x2));
            AssertExpr(f => f.NotEqual(x1, x2, liftToNull: false, method: null));
            AssertExpr(f => f.NotEqual(b1, b2, liftToNull: false, bNe));

            AssertExpr(f => f.LessThan(x1, x2));
            AssertExpr(f => f.LessThan(x1, x2, liftToNull: false, method: null));
            AssertExpr(f => f.LessThan(b1, b2, liftToNull: false, bLt));

            AssertExpr(f => f.LessThanOrEqual(x1, x2));
            AssertExpr(f => f.LessThanOrEqual(x1, x2, liftToNull: false, method: null));
            AssertExpr(f => f.LessThanOrEqual(b1, b2, liftToNull: false, bLe));

            AssertExpr(f => f.GreaterThan(x1, x2));
            AssertExpr(f => f.GreaterThan(x1, x2, liftToNull: false, method: null));
            AssertExpr(f => f.GreaterThan(b1, b2, liftToNull: false, bGt));

            AssertExpr(f => f.GreaterThanOrEqual(x1, x2));
            AssertExpr(f => f.GreaterThanOrEqual(x1, x2, liftToNull: false, method: null));
            AssertExpr(f => f.GreaterThanOrEqual(b1, b2, liftToNull: false, bGe));
        }

        [TestMethod]
        public void ExpressionFactory_Block()
        {
            var stmt1 = Expression.Constant(ObjectOf(1));
            var stmt2 = Expression.Constant(ObjectOf("bar"));
            var stmt3 = Expression.Constant(ObjectOf(false));
            var stmt4 = Expression.Constant(ObjectOf(3.14));
            var stmt5 = Expression.Constant(ObjectOf(42L));
            var stmt6 = Expression.Empty();

            var p1 = Expression.Parameter(TypeOf(typeof(char)));

            AssertExpr(f => f.Block(stmt1));
            AssertExpr(f => f.Block(stmt1, stmt2));
            AssertExpr(f => f.Block(stmt1, stmt2, stmt3));
            AssertExpr(f => f.Block(stmt1, stmt2, stmt3, stmt4));
            AssertExpr(f => f.Block(stmt1, stmt2, stmt3, stmt4, stmt5));
            AssertExpr(f => f.Block(stmt1, stmt2, stmt3, stmt4, stmt5, stmt6));

            AssertExpr(f => f.Block(new Expression[] { stmt1 }));
            AssertExpr(f => f.Block(new Expression[] { stmt1, stmt2 }));
            AssertExpr(f => f.Block(new Expression[] { stmt1, stmt2, stmt3 }));
            AssertExpr(f => f.Block(new Expression[] { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(new Expression[] { stmt1, stmt2, stmt3, stmt4, stmt5 }));
            AssertExpr(f => f.Block(new Expression[] { stmt1, stmt2, stmt3, stmt4, stmt5, stmt6 }));

            AssertExpr(f => f.Block(new List<Expression> { stmt1 }));
            AssertExpr(f => f.Block(new List<Expression> { stmt1, stmt2 }));
            AssertExpr(f => f.Block(new List<Expression> { stmt1, stmt2, stmt3 }));
            AssertExpr(f => f.Block(new List<Expression> { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(new List<Expression> { stmt1, stmt2, stmt3, stmt4, stmt5 }));
            AssertExpr(f => f.Block(new List<Expression> { stmt1, stmt2, stmt3, stmt4, stmt5, stmt6 }));

            AssertExpr(f => f.Block(TypeOf(typeof(double)), new Expression[] { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(TypeOf(typeof(double)), new List<Expression> { stmt1, stmt2, stmt3, stmt4 }));

            AssertExpr(f => f.Block(TypeOf(typeof(double)), new ParameterExpression[] { p1 }, new List<Expression> { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(TypeOf(typeof(double)), new List<ParameterExpression> { p1 }, new List<Expression> { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(TypeOf(typeof(double)), new ParameterExpression[] { p1 }, new Expression[] { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(TypeOf(typeof(double)), new List<ParameterExpression> { p1 }, new Expression[] { stmt1, stmt2, stmt3, stmt4 }));

            AssertExpr(f => f.Block(new ParameterExpression[] { p1 }, new List<Expression> { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(new List<ParameterExpression> { p1 }, new List<Expression> { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(new ParameterExpression[] { p1 }, new Expression[] { stmt1, stmt2, stmt3, stmt4 }));
            AssertExpr(f => f.Block(new List<ParameterExpression> { p1 }, new Expression[] { stmt1, stmt2, stmt3, stmt4 }));
        }

        [TestMethod]
        public void ExpressionFactory_Break()
        {
            var l1 = Expression.Label();
            var l2 = Expression.Label(TypeOf(typeof(int)), "foo");

            var c = Expression.Constant(ObjectOf(42));

            AssertExpr(f => f.Break(l1));
            AssertExpr(f => f.Break(l1, TypeOf(typeof(int))));

            AssertExpr(f => f.Break(l2, c));
            AssertExpr(f => f.Break(l2, c, TypeOf(typeof(int))));
        }

        [TestMethod]
        public void ExpressionFactory_Call()
        {
            var baz = Expression.Constant(ObjectOf(new MyBaz()));

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var bazFoo0 = InfoOf((MyBaz b) => b.Foo());
            var bazFoo1 = InfoOf((MyBaz b) => b.Foo(default(int)));
            var bazFoo2 = InfoOf((MyBaz b) => b.Foo(default(int), default(bool)));
            var bazFoo3 = InfoOf((MyBaz b) => b.Foo(default(int), default(bool), default(long)));
            var bazFoo4 = InfoOf((MyBaz b) => b.Foo(default(int), default(bool), default(long), default(byte)));
            var bazFoo5 = InfoOf((MyBaz b) => b.Foo(default(int), default(bool), default(long), default(byte), default(string)));

            var bazQux0 = InfoOf((MyBaz b) => b.Qux());
            var bazQux1 = InfoOf((MyBaz b) => b.Qux(default(int)));
            var bazQux2 = InfoOf((MyBaz b) => b.Qux(default(int), default(bool)));
            var bazQux3 = InfoOf((MyBaz b) => b.Qux(default(int), default(bool), default(long)));
            var bazQux4 = InfoOf((MyBaz b) => b.Qux(default(int), default(bool), default(long), default(byte)));
            var bazQux5 = InfoOf((MyBaz b) => b.Qux(default(int), default(bool), default(long), default(byte), default(string)));

            var barFoo0 = InfoOf(() => MyBar.Foo());
            var barFoo1 = InfoOf(() => MyBar.Foo(default(int)));
            var barFoo2 = InfoOf(() => MyBar.Foo(default(int), default(bool)));
            var barFoo3 = InfoOf(() => MyBar.Foo(default(int), default(bool), default(long)));
            var barFoo4 = InfoOf(() => MyBar.Foo(default(int), default(bool), default(long), default(byte)));
            var barFoo5 = InfoOf(() => MyBar.Foo(default(int), default(bool), default(long), default(byte), default(string)));

            var barQux0 = InfoOf(() => MyBar.Qux());
            var barQux1 = InfoOf(() => MyBar.Qux(default(int)));
            var barQux2 = InfoOf(() => MyBar.Qux(default(int), default(bool)));
            var barQux3 = InfoOf(() => MyBar.Qux(default(int), default(bool), default(long)));
            var barQux4 = InfoOf(() => MyBar.Qux(default(int), default(bool), default(long), default(byte)));
            var barQux5 = InfoOf(() => MyBar.Qux(default(int), default(bool), default(long), default(byte), default(string)));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var ci = Expression.Constant(ObjectOf(1));
            var cb = Expression.Constant(ObjectOf(false));
            var cl = Expression.Constant(ObjectOf(1L));
            var cd = Expression.Constant(ObjectOf((byte)1));
            var cs = Expression.Constant(ObjectOf("baz"));

            AssertExpr(f => f.Call(baz, bazFoo0));
            AssertExpr(f => f.Call(baz, bazFoo1, ci));
            AssertExpr(f => f.Call(baz, bazFoo2, ci, cb));
            AssertExpr(f => f.Call(baz, bazFoo3, ci, cb, cl));
            AssertExpr(f => f.Call(baz, bazFoo4, ci, cb, cl, cd));
            AssertExpr(f => f.Call(baz, bazFoo5, ci, cb, cl, cd, cs));

            AssertExpr(f => f.Call(baz, bazQux0));
            AssertExpr(f => f.Call(baz, bazQux1, ci));
            AssertExpr(f => f.Call(baz, bazQux2, ci, cb));
            AssertExpr(f => f.Call(baz, bazQux3, ci, cb, cl));
            AssertExpr(f => f.Call(baz, bazQux4, ci, cb, cl, cd));
            AssertExpr(f => f.Call(baz, bazQux5, ci, cb, cl, cd, cs));

            AssertExpr(f => f.Call(baz, bazFoo0, Array.Empty<Expression>()));
            AssertExpr(f => f.Call(baz, bazFoo1, new Expression[] { ci }));
            AssertExpr(f => f.Call(baz, bazFoo2, new Expression[] { ci, cb }));
            AssertExpr(f => f.Call(baz, bazFoo3, new Expression[] { ci, cb, cl }));
            AssertExpr(f => f.Call(baz, bazFoo4, new Expression[] { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(baz, bazFoo5, new Expression[] { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(baz, bazQux0, Array.Empty<Expression>()));
            AssertExpr(f => f.Call(baz, bazQux1, new Expression[] { ci }));
            AssertExpr(f => f.Call(baz, bazQux2, new Expression[] { ci, cb }));
            AssertExpr(f => f.Call(baz, bazQux3, new Expression[] { ci, cb, cl }));
            AssertExpr(f => f.Call(baz, bazQux4, new Expression[] { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(baz, bazQux5, new Expression[] { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(baz, bazFoo0, new List<Expression> { }));
            AssertExpr(f => f.Call(baz, bazFoo1, new List<Expression> { ci }));
            AssertExpr(f => f.Call(baz, bazFoo2, new List<Expression> { ci, cb }));
            AssertExpr(f => f.Call(baz, bazFoo3, new List<Expression> { ci, cb, cl }));
            AssertExpr(f => f.Call(baz, bazFoo4, new List<Expression> { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(baz, bazFoo5, new List<Expression> { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(baz, bazQux0, new List<Expression> { }));
            AssertExpr(f => f.Call(baz, bazQux1, new List<Expression> { ci }));
            AssertExpr(f => f.Call(baz, bazQux2, new List<Expression> { ci, cb }));
            AssertExpr(f => f.Call(baz, bazQux3, new List<Expression> { ci, cb, cl }));
            AssertExpr(f => f.Call(baz, bazQux4, new List<Expression> { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(baz, bazQux5, new List<Expression> { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(instance: null, barFoo0));
            AssertExpr(f => f.Call(instance: null, barFoo1, ci));
            AssertExpr(f => f.Call(instance: null, barFoo2, ci, cb));
            AssertExpr(f => f.Call(instance: null, barFoo3, ci, cb, cl));
            AssertExpr(f => f.Call(instance: null, barFoo4, ci, cb, cl, cd));
            AssertExpr(f => f.Call(instance: null, barFoo5, ci, cb, cl, cd, cs));

            AssertExpr(f => f.Call(instance: null, barQux0));
            AssertExpr(f => f.Call(instance: null, barQux1, ci));
            AssertExpr(f => f.Call(instance: null, barQux2, ci, cb));
            AssertExpr(f => f.Call(instance: null, barQux3, ci, cb, cl));
            AssertExpr(f => f.Call(instance: null, barQux4, ci, cb, cl, cd));
            AssertExpr(f => f.Call(instance: null, barQux5, ci, cb, cl, cd, cs));

            AssertExpr(f => f.Call(instance: null, barFoo0, Array.Empty<Expression>()));
            AssertExpr(f => f.Call(instance: null, barFoo1, new Expression[] { ci }));
            AssertExpr(f => f.Call(instance: null, barFoo2, new Expression[] { ci, cb }));
            AssertExpr(f => f.Call(instance: null, barFoo3, new Expression[] { ci, cb, cl }));
            AssertExpr(f => f.Call(instance: null, barFoo4, new Expression[] { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(instance: null, barFoo5, new Expression[] { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(instance: null, barQux0, Array.Empty<Expression>()));
            AssertExpr(f => f.Call(instance: null, barQux1, new Expression[] { ci }));
            AssertExpr(f => f.Call(instance: null, barQux2, new Expression[] { ci, cb }));
            AssertExpr(f => f.Call(instance: null, barQux3, new Expression[] { ci, cb, cl }));
            AssertExpr(f => f.Call(instance: null, barQux4, new Expression[] { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(instance: null, barQux5, new Expression[] { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(instance: null, barFoo0, new List<Expression> { }));
            AssertExpr(f => f.Call(instance: null, barFoo1, new List<Expression> { ci }));
            AssertExpr(f => f.Call(instance: null, barFoo2, new List<Expression> { ci, cb }));
            AssertExpr(f => f.Call(instance: null, barFoo3, new List<Expression> { ci, cb, cl }));
            AssertExpr(f => f.Call(instance: null, barFoo4, new List<Expression> { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(instance: null, barFoo5, new List<Expression> { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(instance: null, barQux0, new List<Expression> { }));
            AssertExpr(f => f.Call(instance: null, barQux1, new List<Expression> { ci }));
            AssertExpr(f => f.Call(instance: null, barQux2, new List<Expression> { ci, cb }));
            AssertExpr(f => f.Call(instance: null, barQux3, new List<Expression> { ci, cb, cl }));
            AssertExpr(f => f.Call(instance: null, barQux4, new List<Expression> { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(instance: null, barQux5, new List<Expression> { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(barFoo0));
            AssertExpr(f => f.Call(barFoo1, ci));
            AssertExpr(f => f.Call(barFoo2, ci, cb));
            AssertExpr(f => f.Call(barFoo3, ci, cb, cl));
            AssertExpr(f => f.Call(barFoo4, ci, cb, cl, cd));
            AssertExpr(f => f.Call(barFoo5, ci, cb, cl, cd, cs));

            AssertExpr(f => f.Call(barQux0));
            AssertExpr(f => f.Call(barQux1, ci));
            AssertExpr(f => f.Call(barQux2, ci, cb));
            AssertExpr(f => f.Call(barQux3, ci, cb, cl));
            AssertExpr(f => f.Call(barQux4, ci, cb, cl, cd));
            AssertExpr(f => f.Call(barQux5, ci, cb, cl, cd, cs));

            AssertExpr(f => f.Call(barFoo0, Array.Empty<Expression>()));
            AssertExpr(f => f.Call(barFoo1, new Expression[] { ci }));
            AssertExpr(f => f.Call(barFoo2, new Expression[] { ci, cb }));
            AssertExpr(f => f.Call(barFoo3, new Expression[] { ci, cb, cl }));
            AssertExpr(f => f.Call(barFoo4, new Expression[] { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(barFoo5, new Expression[] { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(barQux0, Array.Empty<Expression>()));
            AssertExpr(f => f.Call(barQux1, new Expression[] { ci }));
            AssertExpr(f => f.Call(barQux2, new Expression[] { ci, cb }));
            AssertExpr(f => f.Call(barQux3, new Expression[] { ci, cb, cl }));
            AssertExpr(f => f.Call(barQux4, new Expression[] { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(barQux5, new Expression[] { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(barFoo0, new List<Expression> { }));
            AssertExpr(f => f.Call(barFoo1, new List<Expression> { ci }));
            AssertExpr(f => f.Call(barFoo2, new List<Expression> { ci, cb }));
            AssertExpr(f => f.Call(barFoo3, new List<Expression> { ci, cb, cl }));
            AssertExpr(f => f.Call(barFoo4, new List<Expression> { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(barFoo5, new List<Expression> { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(barFoo0, new List<Expression> { }.Select(x => x)));
            AssertExpr(f => f.Call(barFoo1, new List<Expression> { ci }.Select(x => x)));
            AssertExpr(f => f.Call(barFoo2, new List<Expression> { ci, cb }.Select(x => x)));
            AssertExpr(f => f.Call(barFoo3, new List<Expression> { ci, cb, cl }.Select(x => x)));
            AssertExpr(f => f.Call(barFoo4, new List<Expression> { ci, cb, cl, cd }.Select(x => x)));
            AssertExpr(f => f.Call(barFoo5, new List<Expression> { ci, cb, cl, cd, cs }.Select(x => x)));

            AssertExpr(f => f.Call(barQux0, new List<Expression> { }));
            AssertExpr(f => f.Call(barQux1, new List<Expression> { ci }));
            AssertExpr(f => f.Call(barQux2, new List<Expression> { ci, cb }));
            AssertExpr(f => f.Call(barQux3, new List<Expression> { ci, cb, cl }));
            AssertExpr(f => f.Call(barQux4, new List<Expression> { ci, cb, cl, cd }));
            AssertExpr(f => f.Call(barQux5, new List<Expression> { ci, cb, cl, cd, cs }));

            AssertExpr(f => f.Call(barQux0, new List<Expression> { }.Select(x => x)));
            AssertExpr(f => f.Call(barQux1, new List<Expression> { ci }.Select(x => x)));
            AssertExpr(f => f.Call(barQux2, new List<Expression> { ci, cb }.Select(x => x)));
            AssertExpr(f => f.Call(barQux3, new List<Expression> { ci, cb, cl }.Select(x => x)));
            AssertExpr(f => f.Call(barQux4, new List<Expression> { ci, cb, cl, cd }.Select(x => x)));
            AssertExpr(f => f.Call(barQux5, new List<Expression> { ci, cb, cl, cd, cs }.Select(x => x)));

#if !USE_SLIM
            AssertExpr(f => f.Call(TypeOf(typeof(int)), "Parse", Type.EmptyTypes, Expression.Constant(ObjectOf("foo"))));
            AssertExpr(f => f.Call(Expression.Constant(ObjectOf("foo")), "Substring", Type.EmptyTypes, Expression.Constant(ObjectOf(1))));
#endif
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionFactory_ClearDebugInfo()
        {
            var s = Expression.SymbolDocument("foo.cs");

            AssertExpr(f => f.ClearDebugInfo(s));
        }
#endif

        [TestMethod]
        public void ExpressionFactory_Conditional()
        {
            var bv = Expression.Constant(ObjectOf(false));
            var tv = Expression.Constant(ObjectOf(1));
            var fv = Expression.Constant(ObjectOf(2));

            AssertExpr(f => f.Condition(bv, tv, fv));
            AssertExpr(f => f.Condition(bv, tv, fv, TypeOf(typeof(int))));

            AssertExpr(f => f.IfThen(bv, tv));
            AssertExpr(f => f.IfThenElse(bv, tv, fv));
        }

        [TestMethod]
        public void ExpressionFactory_Constant()
        {
            AssertExpr(f => f.Constant(ObjectOf(42)));
            AssertExpr(f => f.Constant(ObjectOf(42), TypeOf(typeof(int))));

            AssertExpr(f => f.Constant(ObjectOf(@object: null)));
            AssertExpr(f => f.Constant(ObjectOf(@object: null, typeof(int?)), TypeOf(typeof(int?))));
            AssertExpr(f => f.Constant(ObjectOf(@object: null, typeof(string)), TypeOf(typeof(string))));

            AssertExpr(f => f.Constant(ObjectOf("bar")));
            AssertExpr(f => f.Constant(ObjectOf("bar"), TypeOf(typeof(string))));
        }

        [TestMethod]
        public void ExpressionFactory_Continue()
        {
            var l1 = Expression.Label();
            var l2 = Expression.Label(TypeOf(typeof(int)), "foo");

            var c = Expression.Constant(ObjectOf(42));

            AssertExpr(f => f.Continue(l1));
            AssertExpr(f => f.Continue(l1, TypeOf(typeof(int))));
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionFactory_DebugInfo()
        {
            var s = Expression.SymbolDocument("foo.cs");
            var g = Guid.NewGuid();
            var h = Guid.NewGuid();
            var d = Guid.NewGuid();

            AssertExpr(f => f.DebugInfo(s, 1, 2, 3, 4));
            AssertExpr(f => f.DebugInfo(f.SymbolDocument("foo.cs"), 1, 2, 3, 4));
            AssertExpr(f => f.DebugInfo(f.SymbolDocument("foo.cs", g), 1, 2, 3, 4));
            AssertExpr(f => f.DebugInfo(f.SymbolDocument("foo.cs", g, h), 1, 2, 3, 4));
            AssertExpr(f => f.DebugInfo(f.SymbolDocument("foo.cs", g, h, d), 1, 2, 3, 4));
        }
#endif

        [TestMethod]
        public void ExpressionFactory_Default()
        {
            AssertExpr(f => f.Default(TypeOf(typeof(int))));
            AssertExpr(f => f.Default(TypeOf(typeof(int?))));
            AssertExpr(f => f.Default(TypeOf(typeof(string))));
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionFactory_Dynamic()
        {
            var conv = CS.Binder.Convert(CS.CSharpBinderFlags.ConvertExplicit, TypeOf(typeof(int)), TypeOf(typeof(UnsafeExpressionTests)));
            var add = CS.Binder.BinaryOperation(CS.CSharpBinderFlags.None, ExpressionType.Add, TypeOf(typeof(UnsafeExpressionTests)), new[] { CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null), CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null) });
            var mtd3 = CS.Binder.InvokeMember(CS.CSharpBinderFlags.None, "foo", Type.EmptyTypes, TypeOf(typeof(UnsafeExpressionTests)), new[] { CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null), CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null), CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null) });
            var mtd4 = CS.Binder.InvokeMember(CS.CSharpBinderFlags.None, "foo", Type.EmptyTypes, TypeOf(typeof(UnsafeExpressionTests)), new[] { CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null), CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null), CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null), CS.CSharpArgumentInfo.Create(CS.CSharpArgumentInfoFlags.None, name: null) });

            var c = Expression.Constant(ObjectOf(42L));
            var x = Expression.Constant(ObjectOf(1));
            var y = Expression.Constant(ObjectOf(2));
            var z = Expression.Constant(ObjectOf(3));
            var u = Expression.Constant(ObjectOf(4));

            AssertExpr(f => f.Dynamic(conv, TypeOf(typeof(int)), c));
            AssertExpr(f => f.Dynamic(conv, TypeOf(typeof(int)), new Expression[] { c }));
            AssertExpr(f => f.Dynamic(conv, TypeOf(typeof(int)), new List<Expression> { c }));

            AssertExpr(f => f.Dynamic(add, TypeOf(typeof(int)), x, y));
            AssertExpr(f => f.Dynamic(add, TypeOf(typeof(int)), new Expression[] { x, y }));
            AssertExpr(f => f.Dynamic(add, TypeOf(typeof(int)), new List<Expression> { x, y }));

            AssertExpr(f => f.Dynamic(mtd3, TypeOf(typeof(int)), x, y, z));
            AssertExpr(f => f.Dynamic(mtd3, TypeOf(typeof(int)), new Expression[] { x, y, z }));
            AssertExpr(f => f.Dynamic(mtd3, TypeOf(typeof(int)), new List<Expression> { x, y, z }));

            AssertExpr(f => f.Dynamic(mtd4, TypeOf(typeof(int)), x, y, z, u));
            AssertExpr(f => f.Dynamic(mtd4, TypeOf(typeof(int)), new Expression[] { x, y, z, u }));
            AssertExpr(f => f.Dynamic(mtd4, TypeOf(typeof(int)), new List<Expression> { x, y, z, u }));

            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, long, int>)), conv, c));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, long, int>)), conv, new Expression[] { c }));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, long, int>)), conv, new List<Expression> { c }));

            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int>)), add, x, y));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int>)), add, new Expression[] { x, y }));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int>)), add, new List<Expression> { x, y }));

            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int, object>)), mtd3, x, y, z));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int, object>)), mtd3, new Expression[] { x, y, z }));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int, object>)), mtd3, new List<Expression> { x, y, z }));

            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int, int, object>)), mtd4, x, y, z, u));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int, int, object>)), mtd4, new Expression[] { x, y, z, u }));
            AssertExpr(f => f.MakeDynamic(TypeOf(typeof(Func<CallSite, int, int, int, int, object>)), mtd4, new List<Expression> { x, y, z, u }));
        }
#endif

        [TestMethod]
        public void ExpressionFactory_Empty()
        {
            AssertExpr(f => f.Empty());
        }

        [TestMethod]
        public void ExpressionFactory_Goto()
        {
            var l1 = Expression.Label();

            AssertExpr(f => f.Goto(l1));
            AssertExpr(f => f.Goto(l1, Expression.Empty()));
            AssertExpr(f => f.Goto(l1, TypeOf(typeof(void))));
            AssertExpr(f => f.Goto(l1, Expression.Empty(), TypeOf(typeof(void))));

            AssertExpr(f => f.MakeGoto(GotoExpressionKind.Break, l1, Expression.Empty(), TypeOf(typeof(void))));
        }

        [TestMethod]
        public void ExpressionFactory_Invoke()
        {
            var f0 = Expression.Constant(ObjectOf(new Action(() => { })));
            var f1 = Expression.Constant(ObjectOf(new Action<int>(_ => { })));
            var f2 = Expression.Constant(ObjectOf(new Action<int, int>((_1, _2) => { })));
            var f3 = Expression.Constant(ObjectOf(new Action<int, int, int>((_1, _2, _3) => { })));
            var f4 = Expression.Constant(ObjectOf(new Action<int, int, int, int>((_1, _2, _3, _4) => { })));
            var f5 = Expression.Constant(ObjectOf(new Action<int, int, int, int, int>((_1, _2, _3, _4, _5) => { })));
            var f6 = Expression.Constant(ObjectOf(new Action<int, int, int, int, int, int>((_1, _2, _3, _4, _5, _6) => { })));

            var a1 = Expression.Constant(ObjectOf(1));
            var a2 = Expression.Constant(ObjectOf(2));
            var a3 = Expression.Constant(ObjectOf(3));
            var a4 = Expression.Constant(ObjectOf(4));
            var a5 = Expression.Constant(ObjectOf(5));
            var a6 = Expression.Constant(ObjectOf(6));

            AssertExpr(f => f.Invoke(f0));
            AssertExpr(f => f.Invoke(f0, Array.Empty<Expression>()));
            AssertExpr(f => f.Invoke(f0, new List<Expression> { }));

            AssertExpr(f => f.Invoke(f1, a1));
            AssertExpr(f => f.Invoke(f1, new Expression[] { a1 }));
            AssertExpr(f => f.Invoke(f1, new List<Expression> { a1 }));

            AssertExpr(f => f.Invoke(f2, a1, a2));
            AssertExpr(f => f.Invoke(f2, new Expression[] { a1, a2 }));
            AssertExpr(f => f.Invoke(f2, new List<Expression> { a1, a2 }));

            AssertExpr(f => f.Invoke(f3, a1, a2, a3));
            AssertExpr(f => f.Invoke(f3, new Expression[] { a1, a2, a3 }));
            AssertExpr(f => f.Invoke(f3, new List<Expression> { a1, a2, a3 }));

            AssertExpr(f => f.Invoke(f4, a1, a2, a3, a4));
            AssertExpr(f => f.Invoke(f4, new Expression[] { a1, a2, a3, a4 }));
            AssertExpr(f => f.Invoke(f4, new List<Expression> { a1, a2, a3, a4 }));

            AssertExpr(f => f.Invoke(f5, a1, a2, a3, a4, a5));
            AssertExpr(f => f.Invoke(f5, new Expression[] { a1, a2, a3, a4, a5 }));
            AssertExpr(f => f.Invoke(f5, new List<Expression> { a1, a2, a3, a4, a5 }));

            AssertExpr(f => f.Invoke(f6, a1, a2, a3, a4, a5, a6));
            AssertExpr(f => f.Invoke(f6, new Expression[] { a1, a2, a3, a4, a5, a6 }));
            AssertExpr(f => f.Invoke(f6, new List<Expression> { a1, a2, a3, a4, a5, a6 }));
        }

        [TestMethod]
        public void ExpressionFactory_Label()
        {
            var l1 = Expression.Label();

            AssertExpr(f => f.Label(l1));
            AssertExpr(f => f.Label(l1, Expression.Empty()));

            AssertExpr(f => f.Label(f.Label()));
            AssertExpr(f => f.Label(f.Label("foo")));
            AssertExpr(f => f.Label(f.Label(TypeOf(typeof(void)))));
            AssertExpr(f => f.Label(f.Label(TypeOf(typeof(void)), "foo")));
        }

        [TestMethod]
        public void ExpressionFactory_Lambda()
        {
            var p1 = Expression.Parameter(TypeOf(typeof(int)));
            var p2 = Expression.Parameter(TypeOf(typeof(string)));

            var b1 = Expression.Empty();
            var b2 = Expression.Constant(ObjectOf(1));

            AssertExpr(f => f.Lambda(b1));
            AssertExpr(f => f.Lambda(b2));
            AssertExpr(f => f.Lambda(b1, p1));
            AssertExpr(f => f.Lambda(b2, p1));
            AssertExpr(f => f.Lambda(b1, p1, p2));
            AssertExpr(f => f.Lambda(b2, p1, p2));

            AssertExpr(f => f.Lambda(b1, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(b2, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(b1, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(b2, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(b1, new ParameterExpression[] { p1, p2 }));
            AssertExpr(f => f.Lambda(b2, new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.Lambda(b1, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b2, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b1, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b2, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b1, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(b2, new List<ParameterExpression> { p1, p2 }));

#if !USE_SLIM
            AssertExpr(f => f.Lambda(b1, tailCall: false));
            AssertExpr(f => f.Lambda(b2, tailCall: false));
            AssertExpr(f => f.Lambda(b1, tailCall: false, p1));
            AssertExpr(f => f.Lambda(b2, tailCall: false, p1));
            AssertExpr(f => f.Lambda(b1, tailCall: false, p1, p2));
            AssertExpr(f => f.Lambda(b2, tailCall: false, p1, p2));

            AssertExpr(f => f.Lambda(b1, tailCall: false, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(b2, tailCall: false, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(b1, tailCall: false, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(b2, tailCall: false, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(b1, tailCall: false, new ParameterExpression[] { p1, p2 }));
            AssertExpr(f => f.Lambda(b2, tailCall: false, new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.Lambda(b1, tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b2, tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b1, tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b2, tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b1, tailCall: false, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(b2, tailCall: false, new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda(b1, "f", new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b2, "f", new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b1, "f", new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b2, "f", new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b1, "f", new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(b2, "f", new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda(b1, "f", tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b2, "f", tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(b1, "f", tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b2, "f", tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(b1, "f", tailCall: false, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(b2, "f", tailCall: false, new List<ParameterExpression> { p1, p2 }));
#endif

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, p1));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, p1));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, p1, p2));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, p1, p2));

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, new ParameterExpression[] { p1, p2 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, new List<ParameterExpression> { p1, p2 }));

#if !USE_SLIM
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, tailCall: false));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, tailCall: false));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, tailCall: false, p1));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, tailCall: false, p1));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, tailCall: false, p1, p2));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, tailCall: false, p1, p2));

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, tailCall: false, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, tailCall: false, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, tailCall: false, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, tailCall: false, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, tailCall: false, new ParameterExpression[] { p1, p2 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, tailCall: false, new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, tailCall: false, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, tailCall: false, new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, "f", new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, "f", new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, "f", new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, "f", new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, "f", new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, "f", new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda(TypeOf(typeof(Action)), b1, "f", tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int>)), b2, "f", tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int>)), b1, "f", tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, int>)), b2, "f", tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Action<int, string>)), b1, "f", tailCall: false, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda(TypeOf(typeof(Func<int, string, int>)), b2, "f", tailCall: false, new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda<Action>(b1));
            AssertExpr(f => f.Lambda<Func<int>>(b2));
            AssertExpr(f => f.Lambda<Action<int>>(b1, p1));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, p1));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, p1, p2));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, p1, p2));

            AssertExpr(f => f.Lambda<Action>(b1, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda<Func<int>>(b2, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda<Action<int>>(b1, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, new ParameterExpression[] { p1, p2 }));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.Lambda<Action>(b1, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Func<int>>(b2, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Action<int>>(b1, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda<Action>(b1, tailCall: false));
            AssertExpr(f => f.Lambda<Func<int>>(b2, tailCall: false));
            AssertExpr(f => f.Lambda<Action<int>>(b1, tailCall: false, p1));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, tailCall: false, p1));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, tailCall: false, p1, p2));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, tailCall: false, p1, p2));

            AssertExpr(f => f.Lambda<Action>(b1, tailCall: false, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda<Func<int>>(b2, tailCall: false, Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.Lambda<Action<int>>(b1, tailCall: false, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, tailCall: false, new ParameterExpression[] { p1 }));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, tailCall: false, new ParameterExpression[] { p1, p2 }));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, tailCall: false, new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.Lambda<Action>(b1, tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Func<int>>(b2, tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Action<int>>(b1, tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, tailCall: false, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, tailCall: false, new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda<Action>(b1, "f", new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Func<int>>(b2, "f", new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Action<int>>(b1, "f", new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, "f", new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, "f", new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, "f", new List<ParameterExpression> { p1, p2 }));

            AssertExpr(f => f.Lambda<Action>(b1, "f", tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Func<int>>(b2, "f", tailCall: false, new List<ParameterExpression> { }));
            AssertExpr(f => f.Lambda<Action<int>>(b1, "f", tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Func<int, int>>(b2, "f", tailCall: false, new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.Lambda<Action<int, string>>(b1, "f", tailCall: false, new List<ParameterExpression> { p1, p2 }));
            AssertExpr(f => f.Lambda<Func<int, string, int>>(b2, "f", tailCall: false, new List<ParameterExpression> { p1, p2 }));
#endif
        }

        [TestMethod]
        public void ExpressionFactory_ListInit()
        {
            var n = Expression.New(TypeOf(typeof(List<int>)));
            var d = Expression.New(TypeOf(typeof(Dictionary<string, int>)));

            var add = (MethodInfo)GetMember(typeof(List<int>).GetMethod("Add", new[] { typeof(int) }));
            var addDict = (MethodInfo)GetMember(typeof(Dictionary<string, int>).GetMethod("Add", new[] { typeof(string), typeof(int) }));

            var c1 = Expression.Constant(ObjectOf(1));
            var c2 = Expression.Constant(ObjectOf(2));
            var c3 = Expression.Constant(ObjectOf(3));
            var c4 = Expression.Constant(ObjectOf("qux"));

            var e1 = Expression.ElementInit(add, c1);
            var e2 = Expression.ElementInit(add, c2);
            var e3 = Expression.ElementInit(add, c3);

            AssertExpr(f => f.ListInit(n, e1));
            AssertExpr(f => f.ListInit(n, e1, e2));
            AssertExpr(f => f.ListInit(n, e1, e2, e3));

            AssertExpr(f => f.ListInit(n, new ElementInit[] { e1 }));
            AssertExpr(f => f.ListInit(n, new ElementInit[] { e1, e2 }));
            AssertExpr(f => f.ListInit(n, new ElementInit[] { e1, e2, e3 }));

            AssertExpr(f => f.ListInit(n, new List<ElementInit> { e1 }));
            AssertExpr(f => f.ListInit(n, new List<ElementInit> { e1, e2 }));
            AssertExpr(f => f.ListInit(n, new List<ElementInit> { e1, e2, e3 }));

#if !USE_SLIM
            AssertExpr(f => f.ListInit(n, c1));
            AssertExpr(f => f.ListInit(n, c1, c2));
            AssertExpr(f => f.ListInit(n, c1, c2, c3));

            AssertExpr(f => f.ListInit(n, new Expression[] { c1 }));
            AssertExpr(f => f.ListInit(n, new Expression[] { c1, c2 }));
            AssertExpr(f => f.ListInit(n, new Expression[] { c1, c2, c3 }));

            AssertExpr(f => f.ListInit(n, new List<Expression> { c1 }));
            AssertExpr(f => f.ListInit(n, new List<Expression> { c1, c2 }));
            AssertExpr(f => f.ListInit(n, new List<Expression> { c1, c2, c3 }));
#endif

            AssertExpr(f => f.ListInit(n, add, c1));
            AssertExpr(f => f.ListInit(n, add, c1, c2));
            AssertExpr(f => f.ListInit(n, add, c1, c2, c3));

            AssertExpr(f => f.ListInit(n, add, new Expression[] { c1 }));
            AssertExpr(f => f.ListInit(n, add, new Expression[] { c1, c2 }));
            AssertExpr(f => f.ListInit(n, add, new Expression[] { c1, c2, c3 }));

            AssertExpr(f => f.ListInit(n, add, new List<Expression> { c1 }));
            AssertExpr(f => f.ListInit(n, add, new List<Expression> { c1, c2 }));
            AssertExpr(f => f.ListInit(n, add, new List<Expression> { c1, c2, c3 }));

            AssertExpr(f => f.ListInit(n, f.ElementInit(add, c1)));
            AssertExpr(f => f.ListInit(n, f.ElementInit(add, new Expression[] { c1 })));
            AssertExpr(f => f.ListInit(n, f.ElementInit(add, new List<Expression> { c1 })));

            AssertExpr(f => f.ListInit(d, f.ElementInit(addDict, c4, c1)));
            AssertExpr(f => f.ListInit(d, f.ElementInit(addDict, new Expression[] { c4, c1 })));
            AssertExpr(f => f.ListInit(d, f.ElementInit(addDict, new List<Expression> { c4, c1 })));
        }

        [TestMethod]
        public void ExpressionFactory_Loop()
        {
            var b = Expression.Label();
            var c = Expression.Label();
            var e = Expression.Empty();

            AssertExpr(f => f.Loop(e));
            AssertExpr(f => f.Loop(e, b));
            AssertExpr(f => f.Loop(e, b, c));
        }

        [TestMethod]
        public void ExpressionFactory_Member()
        {
            var max = (FieldInfo)GetMember(typeof(DateTime).GetField("MaxValue"));
            var now = (PropertyInfo)GetMember(typeof(DateTime).GetProperty("Now"));
            var hour = (PropertyInfo)GetMember(typeof(DateTime).GetProperty("Hour"));
            var bazX = (FieldInfo)GetMember(typeof(MyBaz).GetField("X"));

#if !USE_SLIM
            var nowM = now.GetGetMethod();
            var hourM = hour.GetGetMethod();
#endif

            var dt = Expression.Constant(ObjectOf(DateTime.Now));
            var baz = Expression.Constant(ObjectOf(new MyBaz()));

            AssertExpr(f => f.Field(expression: null, max));
            AssertExpr(f => f.Property(expression: null, now));
            AssertExpr(f => f.Property(dt, hour));

#if !USE_SLIM
            AssertExpr(f => f.Field(expression: null, TypeOf(typeof(DateTime)), "MaxValue"));
            AssertExpr(f => f.Property(expression: null, nowM));

            AssertExpr(f => f.Field(baz, TypeOf(typeof(MyBaz)), "X"));
            AssertExpr(f => f.Field(baz, "X"));

            AssertExpr(f => f.Property(dt, hourM));
            AssertExpr(f => f.Property(dt, TypeOf(typeof(DateTime)), "Hour"));
            AssertExpr(f => f.Property(dt, "Hour"));

            AssertExpr(f => f.PropertyOrField(dt, "Hour"));
            AssertExpr(f => f.PropertyOrField(baz, "X"));
#endif

            AssertExpr(f => f.MakeMemberAccess(expression: null, max));
            AssertExpr(f => f.MakeMemberAccess(expression: null, now));

            AssertExpr(f => f.MakeMemberAccess(dt, hour));
            AssertExpr(f => f.MakeMemberAccess(baz, bazX));
        }

        [TestMethod]
        public void ExpressionFactory_Member_Index()
        {
            var c = Expression.Constant(ObjectOf(new List<int>()));
            var xs = Expression.Constant(ObjectOf(new[] { 1, 2 }));
            var i = Expression.Constant(ObjectOf(1));

            var idx = (PropertyInfo)GetMember(typeof(List<int>).GetProperty("Item"));

#if !USE_SLIM
            AssertExpr(f => f.Property(c, "Item", i));
#endif
            AssertExpr(f => f.Property(c, idx, i));
            AssertExpr(f => f.Property(c, idx, new Expression[] { i }));
            AssertExpr(f => f.Property(c, idx, new List<Expression> { i }));

            AssertExpr(f => f.MakeIndex(c, idx, new List<Expression> { i }));

            AssertExpr(f => f.MakeIndex(xs, indexer: null, new List<Expression> { i }));
        }

        [TestMethod]
        public void ExpressionFactory_MemberInit()
        {
            var pt = TypeOf(typeof(Person));
            var pn = (PropertyInfo)GetMember(typeof(Person).GetProperty("Name"));
            var pa = (PropertyInfo)GetMember(typeof(Person).GetProperty("Age"));
            var pc = (PropertyInfo)GetMember(typeof(Person).GetProperty("City"));
            var px = (PropertyInfo)GetMember(typeof(Person).GetProperty("Numbers"));

            var ct = TypeOf(typeof(City));
            var cz = (PropertyInfo)GetMember(typeof(City).GetProperty("Zip"));

            var add = (MethodInfo)GetMember(typeof(List<int>).GetMethod("Add", new[] { typeof(int) }));

            var n = Expression.New((ConstructorInfo)GetMember(typeof(Person).GetConstructor(global::System.Type.EmptyTypes)));

            var b1 = Expression.Bind(pn, Expression.Constant(ObjectOf("Bart")));
            var b2 = Expression.Bind(pa, Expression.Constant(ObjectOf(21)));
            var b3 = Expression.MemberBind(pc, Expression.Bind(cz, Expression.Constant(ObjectOf("98004"))));
            var b4 = Expression.ListBind(px, Expression.ElementInit(add, Expression.Constant(ObjectOf(42))));

            AssertExpr(f => f.MemberInit(n, new MemberBinding[] { b1, b2, b3, b4 }));
            AssertExpr(f => f.MemberInit(n, new List<MemberBinding> { b1, b2, b3, b4 }));

            AssertExpr(f => f.MemberInit(n,
                f.Bind(pn, Expression.Constant(ObjectOf("Bart"))),
                f.Bind(pa, Expression.Constant(ObjectOf(21))),
                f.MemberBind(pc, new MemberBinding[] { f.Bind(cz, Expression.Constant(ObjectOf("98004"))) }),
                f.ListBind(px, new ElementInit[] { f.ElementInit(add, Expression.Constant(ObjectOf(42))) })
            ));

            AssertExpr(f => f.MemberInit(n,
                f.Bind(pn, Expression.Constant(ObjectOf("Bart"))),
                f.Bind(pa, Expression.Constant(ObjectOf(21))),
                f.MemberBind(pc, new List<MemberBinding> { f.Bind(cz, Expression.Constant(ObjectOf("98004"))) }),
                f.ListBind(px, new List<ElementInit> { f.ElementInit(add, Expression.Constant(ObjectOf(42))) })
            ));

#if !USE_SLIM
            AssertExpr(f => f.MemberInit(n,
                f.Bind(pn.GetSetMethod(), Expression.Constant(ObjectOf("Bart"))),
                f.Bind(pa.GetSetMethod(), Expression.Constant(ObjectOf(21))),
                f.MemberBind(pc.GetSetMethod(), new MemberBinding[] { f.Bind(cz.GetSetMethod(), Expression.Constant(ObjectOf("98004"))) }),
                f.ListBind(px.GetSetMethod(), new ElementInit[] { f.ElementInit(add, Expression.Constant(ObjectOf(42))) })
            ));

            AssertExpr(f => f.MemberInit(n,
                f.Bind(pn.GetSetMethod(), Expression.Constant(ObjectOf("Bart"))),
                f.Bind(pa.GetSetMethod(), Expression.Constant(ObjectOf(21))),
                f.MemberBind(pc.GetSetMethod(), new List<MemberBinding> { f.Bind(cz.GetSetMethod(), Expression.Constant(ObjectOf("98004"))) }),
                f.ListBind(px.GetSetMethod(), new List<ElementInit> { f.ElementInit(add, Expression.Constant(ObjectOf(42))) })
            ));
#endif
        }

        [TestMethod]
        public void ExpressionFactory_New()
        {
            var t = TypeOf(typeof(Tuple<string, int>));

            var c = (ConstructorInfo)GetMember(typeof(Tuple<string, int>).GetConstructor(new[] { typeof(string), typeof(int) }));
            var i = (PropertyInfo)GetMember(typeof(Tuple<string, int>).GetProperty("Item1"));
            var j = (PropertyInfo)GetMember(typeof(Tuple<string, int>).GetProperty("Item2"));

            var newObj = (ConstructorInfo)GetMember(typeof(object).GetConstructor(global::System.Type.EmptyTypes));

            var s = Expression.Constant(ObjectOf("foo"));
            var x = Expression.Constant(ObjectOf(42));

            AssertExpr(f => f.New(TypeOf(typeof(object))));
            AssertExpr(f => f.New(TypeOf(typeof(TimeSpan))));

            AssertExpr(f => f.New(newObj));

            AssertExpr(f => f.New(c, s, x));
            AssertExpr(f => f.New(c, new Expression[] { s, x }));
            AssertExpr(f => f.New(c, new List<Expression> { s, x }));

            AssertExpr(f => f.New(c, new Expression[] { s, x }, new MemberInfo[] { i, j }));
            AssertExpr(f => f.New(c, new List<Expression> { s, x }, new MemberInfo[] { i, j }));

            AssertExpr(f => f.New(c, new Expression[] { s, x }, new List<MemberInfo> { i, j }));
            AssertExpr(f => f.New(c, new List<Expression> { s, x }, new List<MemberInfo> { i, j }));
        }

        [TestMethod]
        public void ExpressionFactory_New_Large()
        {
            var a1 = Expression.Constant(ObjectOf(1));
            var a2 = Expression.Constant(ObjectOf(2));
            var a3 = Expression.Constant(ObjectOf(3));
            var a4 = Expression.Constant(ObjectOf(4));
            var a5 = Expression.Constant(ObjectOf(5));
            var a6 = Expression.Constant(ObjectOf(6));

            var t0 = TypeOf(typeof(A0));
            var c0 = (ConstructorInfo)GetMember(typeof(A0).GetConstructors().Single());
            AssertExpr(f => f.New(c0));
            AssertExpr(f => f.New(c0, Array.Empty<Expression>()));
            AssertExpr(f => f.New(c0, new List<Expression> { }));

            var t1 = TypeOf(typeof(A1));
            var c1 = (ConstructorInfo)GetMember(typeof(A1).GetConstructors().Single());
            AssertExpr(f => f.New(c1, a1));
            AssertExpr(f => f.New(c1, new Expression[] { a1 }));
            AssertExpr(f => f.New(c1, new List<Expression> { a1 }));

            var t2 = TypeOf(typeof(A2));
            var c2 = (ConstructorInfo)GetMember(typeof(A2).GetConstructors().Single());
            AssertExpr(f => f.New(c2, a1, a2));
            AssertExpr(f => f.New(c2, new Expression[] { a1, a2 }));
            AssertExpr(f => f.New(c2, new List<Expression> { a1, a2 }));

            var t3 = TypeOf(typeof(A3));
            var c3 = (ConstructorInfo)GetMember(typeof(A3).GetConstructors().Single());
            AssertExpr(f => f.New(c3, a1, a2, a3));
            AssertExpr(f => f.New(c3, new Expression[] { a1, a2, a3 }));
            AssertExpr(f => f.New(c3, new List<Expression> { a1, a2, a3 }));

            var t4 = TypeOf(typeof(A4));
            var c4 = (ConstructorInfo)GetMember(typeof(A4).GetConstructors().Single());
            AssertExpr(f => f.New(c4, a1, a2, a3, a4));
            AssertExpr(f => f.New(c4, new Expression[] { a1, a2, a3, a4 }));
            AssertExpr(f => f.New(c4, new List<Expression> { a1, a2, a3, a4 }));

            var t5 = TypeOf(typeof(A5));
            var c5 = (ConstructorInfo)GetMember(typeof(A5).GetConstructors().Single());
            AssertExpr(f => f.New(c5, a1, a2, a3, a4, a5));
            AssertExpr(f => f.New(c5, new Expression[] { a1, a2, a3, a4, a5 }));
            AssertExpr(f => f.New(c5, new List<Expression> { a1, a2, a3, a4, a5 }));

            var t6 = TypeOf(typeof(A6));
            var c6 = (ConstructorInfo)GetMember(typeof(A6).GetConstructors().Single());
            AssertExpr(f => f.New(c6, a1, a2, a3, a4, a5, a6));
            AssertExpr(f => f.New(c6, new Expression[] { a1, a2, a3, a4, a5, a6 }));
            AssertExpr(f => f.New(c6, new List<Expression> { a1, a2, a3, a4, a5, a6 }));
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private sealed class A0 { }
        private sealed class A1 { public A1(int a) { } }
        private sealed class A2 { public A2(int a, int b) { } }
        private sealed class A3 { public A3(int a, int b, int c) { } }
        private sealed class A4 { public A4(int a, int b, int c, int d) { } }
        private sealed class A5 { public A5(int a, int b, int c, int d, int e) { } }
        private sealed class A6 { public A6(int a, int b, int c, int d, int e, int f) { } }
#pragma warning restore IDE0060 // Remove unused parameter

        [TestMethod]
        public void ExpressionFactory_Parameter()
        {
            var p1 = ExpressionFactory.Instance.Parameter(TypeOf(typeof(int)));
            var p2 = ExpressionUnsafeFactory.Instance.Parameter(TypeOf(typeof(int)));

            Assert.AreEqual(p1.Type, p2.Type);
            Assert.AreEqual(p1.Name, p2.Name);
#if !USE_SLIM
            Assert.AreEqual(p1.IsByRef, p2.IsByRef);
#endif

            var p3 = ExpressionFactory.Instance.Parameter(TypeOf(typeof(int)), "x");
            var p4 = ExpressionUnsafeFactory.Instance.Parameter(TypeOf(typeof(int)), "x");

            Assert.AreEqual(p3.Type, p4.Type);
            Assert.AreEqual(p3.Name, p4.Name);
#if !USE_SLIM
            Assert.AreEqual(p3.IsByRef, p4.IsByRef);

            var v1 = ExpressionFactory.Instance.Variable(TypeOf(typeof(int)));
            var v2 = ExpressionUnsafeFactory.Instance.Variable(TypeOf(typeof(int)));

            Assert.AreEqual(v1.Type, v2.Type);
            Assert.AreEqual(v1.Name, v2.Name);
            Assert.AreEqual(v1.IsByRef, v2.IsByRef);

            var v3 = ExpressionFactory.Instance.Variable(TypeOf(typeof(int)), "x");
            var v4 = ExpressionUnsafeFactory.Instance.Variable(TypeOf(typeof(int)), "x");

            Assert.AreEqual(v3.Type, v4.Type);
            Assert.AreEqual(v3.Name, v4.Name);
            Assert.AreEqual(v3.IsByRef, v4.IsByRef);
#endif
        }

        [TestMethod]
        public void ExpressionFactory_Return()
        {
            var l1 = Expression.Label();

            AssertExpr(f => f.Return(l1));
            AssertExpr(f => f.Return(l1, Expression.Empty()));
            AssertExpr(f => f.Return(l1, TypeOf(typeof(void))));
            AssertExpr(f => f.Return(l1, Expression.Empty(), TypeOf(typeof(void))));
        }

#if !USE_SLIM
        [TestMethod]
        public void ExpressionFactory_RuntimeVariables()
        {
            var p1 = Expression.Parameter(TypeOf(typeof(int)));
            var p2 = Expression.Parameter(TypeOf(typeof(int)));

            AssertExpr(f => f.RuntimeVariables());
            AssertExpr(f => f.RuntimeVariables(p1));
            AssertExpr(f => f.RuntimeVariables(p1, p2));

            AssertExpr(f => f.RuntimeVariables(Array.Empty<ParameterExpression>()));
            AssertExpr(f => f.RuntimeVariables(new ParameterExpression[] { p1 }));
            AssertExpr(f => f.RuntimeVariables(new ParameterExpression[] { p1, p2 }));

            AssertExpr(f => f.RuntimeVariables(new List<ParameterExpression> { }));
            AssertExpr(f => f.RuntimeVariables(new List<ParameterExpression> { p1 }));
            AssertExpr(f => f.RuntimeVariables(new List<ParameterExpression> { p1, p2 }));
        }
#endif

        [TestMethod]
        public void ExpressionFactory_Switch()
        {
            var c = Expression.Constant(ObjectOf(1));
            var d = Expression.Constant(ObjectOf("bar"));
            var e = Expression.Constant(ObjectOf("qux"));
            var v = Expression.Empty();

            var c1 = Expression.Constant(ObjectOf(1));
            var c2 = Expression.Constant(ObjectOf(2));

            var c3 = Expression.Constant(ObjectOf("a"));
            var c4 = Expression.Constant(ObjectOf("b"));

            var s = Expression.Constant(ObjectOf("bar"));
            var mtd = (MethodInfo)GetMember(typeof(EqualityComparer<string>).GetMethod("Equals", new[] { typeof(string), typeof(string) }));

            AssertExpr(f => f.Switch(c, f.SwitchCase(v, c1)));
            AssertExpr(f => f.Switch(c, f.SwitchCase(v, c1, c2)));
            AssertExpr(f => f.Switch(c, f.SwitchCase(v, new Expression[] { c1 })));
            AssertExpr(f => f.Switch(c, f.SwitchCase(v, new Expression[] { c1, c2 })));
            AssertExpr(f => f.Switch(c, f.SwitchCase(v, new List<Expression> { c1 })));
            AssertExpr(f => f.Switch(c, f.SwitchCase(v, new List<Expression> { c1, c2 })));

            AssertExpr(f => f.Switch(c, new SwitchCase[] { f.SwitchCase(v, c1), f.SwitchCase(v, c2) }));

            AssertExpr(f => f.Switch(c, e, f.SwitchCase(d, c1)));
            AssertExpr(f => f.Switch(c, e, f.SwitchCase(d, c1, c2)));
            AssertExpr(f => f.Switch(c, e, f.SwitchCase(d, new Expression[] { c1 })));
            AssertExpr(f => f.Switch(c, e, f.SwitchCase(d, new Expression[] { c1, c2 })));
            AssertExpr(f => f.Switch(c, e, f.SwitchCase(d, new List<Expression> { c1 })));
            AssertExpr(f => f.Switch(c, e, f.SwitchCase(d, new List<Expression> { c1, c2 })));

            AssertExpr(f => f.Switch(c, e, new SwitchCase[] { f.SwitchCase(d, c1), f.SwitchCase(e, c2) }));

            AssertExpr(f => f.Switch(s, f.SwitchCase(v, c3)));
            AssertExpr(f => f.Switch(s, f.SwitchCase(v, c3, c4)));
            AssertExpr(f => f.Switch(s, f.SwitchCase(v, new Expression[] { c3 })));
            AssertExpr(f => f.Switch(s, f.SwitchCase(v, new Expression[] { c3, c4 })));
            AssertExpr(f => f.Switch(s, f.SwitchCase(v, new List<Expression> { c3 })));
            AssertExpr(f => f.Switch(s, f.SwitchCase(v, new List<Expression> { c3, c4 })));

            AssertExpr(f => f.Switch(s, new SwitchCase[] { f.SwitchCase(v, c3), f.SwitchCase(v, c4) }));

            AssertExpr(f => f.Switch(s, e, f.SwitchCase(d, c3)));
            AssertExpr(f => f.Switch(s, e, f.SwitchCase(d, c3, c4)));
            AssertExpr(f => f.Switch(s, e, f.SwitchCase(d, new Expression[] { c3 })));
            AssertExpr(f => f.Switch(s, e, f.SwitchCase(d, new Expression[] { c3, c4 })));
            AssertExpr(f => f.Switch(s, e, f.SwitchCase(d, new List<Expression> { c3 })));
            AssertExpr(f => f.Switch(s, e, f.SwitchCase(d, new List<Expression> { c3, c4 })));

            AssertExpr(f => f.Switch(s, e, new SwitchCase[] { f.SwitchCase(d, c3), f.SwitchCase(e, c4) }));

            AssertExpr(f => f.Switch(s, e, mtd, f.SwitchCase(d, c3)));
            AssertExpr(f => f.Switch(s, e, mtd, f.SwitchCase(d, c3, c4)));
            AssertExpr(f => f.Switch(s, e, mtd, f.SwitchCase(d, new Expression[] { c3 })));
            AssertExpr(f => f.Switch(s, e, mtd, f.SwitchCase(d, new Expression[] { c3, c4 })));
            AssertExpr(f => f.Switch(s, e, mtd, f.SwitchCase(d, new List<Expression> { c3 })));
            AssertExpr(f => f.Switch(s, e, mtd, f.SwitchCase(d, new List<Expression> { c3, c4 })));

            AssertExpr(f => f.Switch(s, e, mtd, new SwitchCase[] { f.SwitchCase(d, c3), f.SwitchCase(e, c4) }));
            AssertExpr(f => f.Switch(s, e, mtd, new List<SwitchCase> { f.SwitchCase(d, c3), f.SwitchCase(e, c4) }));

            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, f.SwitchCase(d, c3)));
            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, f.SwitchCase(d, c3, c4)));
            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, f.SwitchCase(d, new Expression[] { c3 })));
            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, f.SwitchCase(d, new Expression[] { c3, c4 })));
            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, f.SwitchCase(d, new List<Expression> { c3 })));
            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, f.SwitchCase(d, new List<Expression> { c3, c4 })));

            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, new SwitchCase[] { f.SwitchCase(d, c3), f.SwitchCase(e, c4) }));
            AssertExpr(f => f.Switch(TypeOf(typeof(string)), s, e, mtd, new List<SwitchCase> { f.SwitchCase(d, c3), f.SwitchCase(e, c4) }));
        }

        [TestMethod]
        public void ExpressionFactory_TypeBinary()
        {
            var c = Expression.Constant(ObjectOf(1));
            var d = Expression.Constant(ObjectOf("bar"));
            var t = Expression.Constant(ObjectOf(TypeOf(typeof(int))));

            AssertExpr(f => f.TypeIs(c, TypeOf(typeof(int))));
            AssertExpr(f => f.TypeIs(c, TypeOf(typeof(object))));
            AssertExpr(f => f.TypeIs(c, TypeOf(typeof(string))));

            AssertExpr(f => f.TypeIs(d, TypeOf(typeof(int))));
            AssertExpr(f => f.TypeIs(d, TypeOf(typeof(object))));
            AssertExpr(f => f.TypeIs(d, TypeOf(typeof(string))));

            AssertExpr(f => f.TypeEqual(t, TypeOf(typeof(int))));
        }

        [TestMethod]
        public void ExpressionFactory_Unary_Arithmetic()
        {
            var x = Expression.Constant(ObjectOf(1));
            var b = Expression.Constant(ObjectOf(true));
            var i = Expression.Constant(ObjectOf(new BigInteger(42)));
            var p = Expression.Parameter(TypeOf(typeof(int)));
            var q = Expression.Parameter(TypeOf(typeof(BigInteger)));

            var bOc = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_OnesComplement"));
            var bNe = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_UnaryNegation"));
            var bPl = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_UnaryPlus"));
            var bDe = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Decrement"));
            var bIn = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_Increment"));

            Assert.IsNotNull(bOc);
            Assert.IsNotNull(bNe);
            Assert.IsNotNull(bPl);
            Assert.IsNotNull(bDe);
            Assert.IsNotNull(bIn);

            AssertExpr(f => f.Decrement(x));
            AssertExpr(f => f.Decrement(i));
            AssertExpr(f => f.Decrement(i, bDe));

            AssertExpr(f => f.Increment(x));
            AssertExpr(f => f.Increment(i));
            AssertExpr(f => f.Increment(i, bIn));

            AssertExpr(f => f.OnesComplement(x));
            AssertExpr(f => f.OnesComplement(i));
            AssertExpr(f => f.OnesComplement(i, bOc));

            AssertExpr(f => f.Negate(x));
            AssertExpr(f => f.Negate(i));
            AssertExpr(f => f.Negate(i, bNe));

            AssertExpr(f => f.NegateChecked(x));
            AssertExpr(f => f.NegateChecked(i));
            AssertExpr(f => f.NegateChecked(i, bNe));

            AssertExpr(f => f.UnaryPlus(x));
            AssertExpr(f => f.UnaryPlus(i));
            AssertExpr(f => f.UnaryPlus(i, bPl));

            AssertExpr(f => f.PostIncrementAssign(p));
            AssertExpr(f => f.PostIncrementAssign(q));
            AssertExpr(f => f.PostIncrementAssign(q, bIn));

            AssertExpr(f => f.PostDecrementAssign(p));
            AssertExpr(f => f.PostDecrementAssign(q));
            AssertExpr(f => f.PostDecrementAssign(q, bDe));

            AssertExpr(f => f.PreIncrementAssign(p));
            AssertExpr(f => f.PreIncrementAssign(q));
            AssertExpr(f => f.PreIncrementAssign(q, bIn));

            AssertExpr(f => f.PreDecrementAssign(p));
            AssertExpr(f => f.PreDecrementAssign(q));
            AssertExpr(f => f.PreDecrementAssign(q, bDe));
        }

        [TestMethod]
        public void ExpressionFactory_Unary_Convert()
        {
            var i = Expression.Constant(ObjectOf(1));
            var j = Expression.Constant(ObjectOf(1, typeof(object)), TypeOf(typeof(object)));
            var b = Expression.Constant(ObjectOf(new BigInteger(42)));

            var toInt32 = (MethodInfo)GetMember(typeof(BigInteger).GetMethods().Single(m => m.Name == "op_Explicit" && m.ReturnType == typeof(int)));

            Assert.IsNotNull(toInt32);

            AssertExpr(f => f.Convert(i, TypeOf(typeof(int))));
            AssertExpr(f => f.Convert(i, TypeOf(typeof(long))));
            AssertExpr(f => f.Convert(i, TypeOf(typeof(object))));

            AssertExpr(f => f.Convert(j, TypeOf(typeof(int))));
            AssertExpr(f => f.Convert(j, TypeOf(typeof(long))));
            AssertExpr(f => f.Convert(j, TypeOf(typeof(object))));

            AssertExpr(f => f.ConvertChecked(i, TypeOf(typeof(int))));
            AssertExpr(f => f.ConvertChecked(i, TypeOf(typeof(long))));
            AssertExpr(f => f.ConvertChecked(i, TypeOf(typeof(object))));

            AssertExpr(f => f.ConvertChecked(j, TypeOf(typeof(int))));
            AssertExpr(f => f.ConvertChecked(j, TypeOf(typeof(long))));
            AssertExpr(f => f.ConvertChecked(j, TypeOf(typeof(object))));

            AssertExpr(f => f.Convert(b, TypeOf(typeof(int))));
            AssertExpr(f => f.Convert(b, TypeOf(typeof(int)), toInt32));

            AssertExpr(f => f.ConvertChecked(b, TypeOf(typeof(int))));
            AssertExpr(f => f.ConvertChecked(b, TypeOf(typeof(int)), toInt32));
        }

        [TestMethod]
        public void ExpressionFactory_Unary_Logic()
        {
            var b = Expression.Constant(ObjectOf(true));
            var m = Expression.Constant(ObjectOf(new MyBool()));

            var not = (MethodInfo)GetMember(typeof(MyBool).GetMethod("op_LogicalNot"));
            var tru = (MethodInfo)GetMember(typeof(MyBool).GetMethod("op_True"));
            var fls = (MethodInfo)GetMember(typeof(MyBool).GetMethod("op_False"));

            Assert.IsNotNull(not);
            Assert.IsNotNull(tru);
            Assert.IsNotNull(fls);

            AssertExpr(f => f.Not(b));
            AssertExpr(f => f.Not(m));
            AssertExpr(f => f.Not(m, not));

            AssertExpr(f => f.IsFalse(b));
            AssertExpr(f => f.IsFalse(m));
            AssertExpr(f => f.IsFalse(m, fls));

            AssertExpr(f => f.IsTrue(b));
            AssertExpr(f => f.IsTrue(m));
            AssertExpr(f => f.IsTrue(m, tru));
        }

        [TestMethod]
        public void ExpressionFactory_Unary_Make_Basics()
        {
            var a = Expression.Constant(ObjectOf(1));
            var b = Expression.Constant(ObjectOf(new BigInteger(42)));

            var neg = (MethodInfo)GetMember(typeof(BigInteger).GetMethod("op_UnaryNegation", new[] { typeof(BigInteger) }));

            Assert.IsNotNull(neg);

            AssertExpr(f => f.MakeUnary(ExpressionType.Convert, a, TypeOf(typeof(long))));
            AssertExpr(f => f.MakeUnary(ExpressionType.Negate, b, TypeOf(typeof(BigInteger))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Convert, a, TypeOf(typeof(long)), method: null));
            AssertExpr(f => f.MakeUnary(ExpressionType.Negate, b, TypeOf(typeof(BigInteger)), method: null));

            AssertExpr(f => f.MakeUnary(ExpressionType.Negate, b, TypeOf(typeof(BigInteger)), neg));
        }

        [TestMethod]
        public void ExpressionFactory_Unary_Make_Switch()
        {
            var a = Expression.Constant(ObjectOf(1));
            var b = Expression.Constant(ObjectOf(false));
            var s = Expression.Constant(ObjectOf("bar"));
            var x = Expression.Parameter(TypeOf(typeof(int)));
            var o = Expression.Constant(ObjectOf(42, typeof(object)), TypeOf(typeof(object)));
            var xs = Expression.Constant(ObjectOf(new[] { 1, 2 }));
            var l = Expression.Lambda(Expression.Empty());
            var m = Expression.Constant(ObjectOf(new MyBool()));
            var ex = Expression.Constant(ObjectOf(new Exception()));

            AssertExpr(f => f.MakeUnary(ExpressionType.Convert, a, TypeOf(typeof(long))));
            AssertExpr(f => f.MakeUnary(ExpressionType.ConvertChecked, a, TypeOf(typeof(long))));
            AssertExpr(f => f.MakeUnary(ExpressionType.TypeAs, s, TypeOf(typeof(string))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Negate, a, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.NegateChecked, a, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.OnesComplement, a, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.UnaryPlus, a, TypeOf(typeof(int))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Not, b, TypeOf(typeof(bool))));

            AssertExpr(f => f.MakeUnary(ExpressionType.PreDecrementAssign, x, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.PostDecrementAssign, x, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.PreIncrementAssign, x, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.PostIncrementAssign, x, TypeOf(typeof(int))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Increment, a, TypeOf(typeof(int))));
            AssertExpr(f => f.MakeUnary(ExpressionType.Decrement, a, TypeOf(typeof(int))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Unbox, o, TypeOf(typeof(int))));

            AssertExpr(f => f.MakeUnary(ExpressionType.ArrayLength, xs, TypeOf(typeof(int))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Quote, l, TypeOf(typeof(Expression))));

            AssertExpr(f => f.MakeUnary(ExpressionType.IsTrue, m, TypeOf(typeof(bool))));
            AssertExpr(f => f.MakeUnary(ExpressionType.IsFalse, m, TypeOf(typeof(bool))));

            AssertExpr(f => f.MakeUnary(ExpressionType.Throw, ex, TypeOf(typeof(Exception))));

            Assert.ThrowsException<ArgumentException>(() => ExpressionFactory.Instance.MakeUnary(ExpressionType.Lambda, x, TypeOf(typeof(int))));
            Assert.ThrowsException<ArgumentException>(() => ExpressionUnsafeFactory.Instance.MakeUnary(ExpressionType.Lambda, x, TypeOf(typeof(int))));
        }

        [TestMethod]
        public void ExpressionFactory_Unary_Misc()
        {
            var c = Expression.Constant(ObjectOf(42, typeof(object)), TypeOf(typeof(object)));
            var d = Expression.Constant(ObjectOf("bar", typeof(object)), TypeOf(typeof(object)));
            var e = Expression.Constant(ObjectOf(new InvalidOperationException()));
            var l = Expression.Lambda(Expression.Parameter(TypeOf(typeof(int))));

            AssertExpr(f => f.Unbox(c, TypeOf(typeof(int))));

            AssertExpr(f => f.TypeAs(d, TypeOf(typeof(string))));

            AssertExpr(f => f.Throw(e));
            AssertExpr(f => f.Throw(e, TypeOf(typeof(Exception))));

            AssertExpr(f => f.Rethrow());
            AssertExpr(f => f.Rethrow(TypeOf(typeof(Exception))));

            AssertExpr(f => f.Quote(l));
        }

        [TestMethod]
        public void ExpressionFactory_TryCatch()
        {
            var e = Expression.Empty();

            AssertExpr(f => f.TryCatch(e, f.Catch(f.Parameter(TypeOf(typeof(Expression))), e)));
            AssertExpr(f => f.TryCatch(e, f.Catch(TypeOf(typeof(Expression)), e)));
            AssertExpr(f => f.TryCatch(e, f.Catch(f.Parameter(TypeOf(typeof(Expression))), e, f.Constant(ObjectOf(true)))));
            AssertExpr(f => f.TryCatch(e, f.Catch(TypeOf(typeof(Expression)), e, f.Constant(ObjectOf(true)))));
        }

        [TestMethod]
        public void ExpressionFactory_TryCatchFinally()
        {
            var x = Expression.Constant(ObjectOf(42));
            var y = Expression.Constant(ObjectOf(43));
            var e = Expression.Empty();

            AssertExpr(f => f.TryCatchFinally(x, e, f.Catch(f.Parameter(TypeOf(typeof(Expression))), y)));
            AssertExpr(f => f.TryCatchFinally(x, e, f.Catch(TypeOf(typeof(Expression)), y)));
            AssertExpr(f => f.TryCatchFinally(x, e, f.Catch(f.Parameter(TypeOf(typeof(Expression))), y, f.Constant(ObjectOf(true)))));
            AssertExpr(f => f.TryCatchFinally(x, e, f.Catch(TypeOf(typeof(Expression)), y, f.Constant(ObjectOf(true)))));
        }

        [TestMethod]
        public void ExpressionFactory_TryFinally()
        {
            var e = Expression.Constant(ObjectOf(42));
            var x = Expression.Empty();

            AssertExpr(f => f.TryFinally(e, x));
        }

        [TestMethod]
        public void ExpressionFactory_TryFault()
        {
            var e = Expression.Constant(ObjectOf(42));
            var x = Expression.Empty();

            AssertExpr(f => f.TryFault(e, x));
        }

        [TestMethod]
        public void ExpressionFactory_Try()
        {
            var e = Expression.Empty();
            var x = Expression.Constant(ObjectOf(42));
            var y = Expression.Constant(ObjectOf(43));
            var b = Expression.Constant(ObjectOf(true));

            AssertExpr(f => f.MakeTry(TypeOf(typeof(int)), x, e, fault: null, new[] { f.Catch(TypeOf(typeof(Exception)), y) }));
            AssertExpr(f => f.MakeTry(TypeOf(typeof(int)), x, e, fault: null, new[] { f.MakeCatchBlock(TypeOf(typeof(Exception)), Expression.Parameter(TypeOf(typeof(Exception))), y, b) }));
            AssertExpr(f => f.MakeTry(TypeOf(typeof(int)), x, e, fault: null, new[] { f.MakeCatchBlock(TypeOf(typeof(Exception)), Expression.Parameter(TypeOf(typeof(Exception))), y, filter: null) }));
            AssertExpr(f => f.MakeTry(TypeOf(typeof(int)), x, e, fault: null, new[] { f.MakeCatchBlock(TypeOf(typeof(Exception)), variable: null, y, b) }));
            AssertExpr(f => f.MakeTry(TypeOf(typeof(int)), x, @finally: null, e, handlers: null));
        }

        private static void AssertExpr(Func<IExpressionFactory, Expression> f)
        {
            var regular = ToExpression(f(ExpressionFactory.Instance));
            var @unsafe = ToExpression(f(ExpressionUnsafeFactory.Instance));

            var b = new ExpressionEqualityComparer(() => new MyExpressionEqualityComparator()).Equals(regular, @unsafe);
            Assert.IsTrue(b);
        }

        private static Type TypeOf(global::System.Type type)
        {
#if USE_SLIM
            return type.ToTypeSlim();
#else
            return type;
#endif
        }

        private static Object ObjectOf(global::System.Object @object)
        {
#if USE_SLIM
            var type = @object != null ? @object.GetType() : typeof(object);
            return ObjectOf(@object, type);
#else
            return @object;
#endif
        }

        private static Object ObjectOf(global::System.Object @object, global::System.Type type)
        {
#if USE_SLIM
            return Object.Create(@object, type.ToTypeSlim(), type);
#else
            return @object;
#endif
        }

        private static global::System.Linq.Expressions.Expression ToExpression(Expression expression)
        {
#if USE_SLIM
            // For now, we'll lambda lift globals because they will not be reference equal.
            var reduced = expression.ToExpression();
            return global::System.Linq.Expressions.Expression.Lambda(reduced, FreeVariableScanner.Scan(reduced));
#else
            return expression;
#endif
        }

        private static MemberInfo GetMember(global::System.Reflection.MemberInfo member)
        {
#if USE_SLIM
            return new TypeSpace().GetMember(member);
#else
            return member;
#endif
        }

        private static MethodInfo InfoOf(Expression<Action> f)
        {
            return (MethodInfo)GetMember(((MethodCallExpression)f.Body).Method);
        }

        private static MethodInfo InfoOf<T>(Expression<Func<T>> f)
        {
            return (MethodInfo)GetMember(((MethodCallExpression)f.Body).Method);
        }

        private static MethodInfo InfoOf<T>(Expression<Action<T>> f)
        {
            return (MethodInfo)GetMember(((MethodCallExpression)f.Body).Method);
        }

        private static MethodInfo InfoOf<T, R>(Expression<Func<T, R>> f)
        {
            return (MethodInfo)GetMember(((MethodCallExpression)f.Body).Method);
        }
    }

#pragma warning disable IDE0060 // Remove unused parameter
    internal static class MyBar
    {
        public static void Foo() { }
        public static void Foo(int a) { }
        public static void Foo(int a, bool b) { }
        public static void Foo(int a, bool b, long c) { }
        public static void Foo(int a, bool b, long c, byte d) { }
        public static void Foo(int a, bool b, long c, byte d, string e) { }

        public static char Qux() { throw new NotImplementedException(); }
        public static char Qux(int a) { throw new NotImplementedException(); }
        public static char Qux(int a, bool b) { throw new NotImplementedException(); }
        public static char Qux(int a, bool b, long c) { throw new NotImplementedException(); }
        public static char Qux(int a, bool b, long c, byte d) { throw new NotImplementedException(); }
        public static char Qux(int a, bool b, long c, byte d, string e) { throw new NotImplementedException(); }
    }

#pragma warning disable CA1822 // Mark static
    internal sealed class MyBaz
    {
#pragma warning disable 0649
        public int X;
#pragma warning restore 0649

        public void Foo() { }
        public void Foo(int a) { }
        public void Foo(int a, bool b) { }
        public void Foo(int a, bool b, long c) { }
        public void Foo(int a, bool b, long c, byte d) { }
        public void Foo(int a, bool b, long c, byte d, string e) { }

        public char Qux() { throw new NotImplementedException(); }
        public char Qux(int a) { throw new NotImplementedException(); }
        public char Qux(int a, bool b) { throw new NotImplementedException(); }
        public char Qux(int a, bool b, long c) { throw new NotImplementedException(); }
        public char Qux(int a, bool b, long c, byte d) { throw new NotImplementedException(); }
        public char Qux(int a, bool b, long c, byte d, string e) { throw new NotImplementedException(); }
    }
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1822

#pragma warning disable IDE0060 // Remove unused parameter (https://github.com/dotnet/roslyn/issues/32852)
    internal sealed class MyBool
    {
        public static MyBool operator !(MyBool b) => throw new NotImplementedException();

        public static MyBool operator &(MyBool b1, MyBool b2) => throw new NotImplementedException();

        public static MyBool operator |(MyBool b1, MyBool b2) => throw new NotImplementedException();

        public static bool operator true(MyBool b) => throw new NotImplementedException();

        public static bool operator false(MyBool b) => throw new NotImplementedException();
    }
#pragma warning restore IDE0060 // Remove unused parameter

    internal sealed class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public City City { get; set; }
        public List<int> Numbers { get; set; }
    }

    internal sealed class City
    {
        public string Zip { get; set; }
    }

    internal sealed class MyExpressionEqualityComparator : ExpressionEqualityComparator
    {
        protected override bool EqualsDebugInfo(DebugInfoExpression x, DebugInfoExpression y)
        {
            return x.Document.FileName == y.Document.FileName && x.StartLine == y.StartLine && x.EndLine == y.EndLine && x.StartColumn == y.StartColumn && x.EndColumn == y.EndColumn;
        }

        protected override int GetHashCodeDebugInfo(DebugInfoExpression obj) => 0;
    }
}
