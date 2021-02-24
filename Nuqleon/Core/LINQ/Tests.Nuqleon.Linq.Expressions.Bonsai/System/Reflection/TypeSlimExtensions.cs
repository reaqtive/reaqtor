// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class TypeSlimExtensionsTest : TestBase
    {
        [TestMethod]
        public void TypeSlimExtensions_ToTypeSlim_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.ToTypeSlim(type: null));
        }

        [TestMethod]
        public void TypeSlimExtensions_ToType_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.ToType(type: null));
        }

        [TestMethod]
        public void TypeSlimExtensions_GetConstructor_ArgumentChecking()
        {
            var ts = typeof(int).ToTypeSlim();

            Assert.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetConstructor(type: null, Array.Empty<TypeSlim>().ToReadOnly()));
            Assert.ThrowsException<ArgumentNullException>(() => TypeSlimExtensions.GetConstructor(ts, parameterTypes: null));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsVoidType()
        {
            Assert.IsTrue(TypeSlimExtensions.IsVoidType(TypeSlimExtensions.VoidType));
            Assert.IsTrue(TypeSlimExtensions.IsVoidType(typeof(void).ToTypeSlim()));

            Assert.IsFalse(TypeSlimExtensions.IsVoidType(typeof(int).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsBooleanType()
        {
            Assert.IsTrue(TypeSlimExtensions.IsBooleanType(TypeSlimExtensions.BooleanType));
            Assert.IsTrue(TypeSlimExtensions.IsBooleanType(typeof(bool).ToTypeSlim()));

            Assert.IsFalse(TypeSlimExtensions.IsBooleanType(typeof(int).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsValueType()
        {
            Assert.IsTrue(TypeSlimExtensions.IsValueType(typeof(int).ToTypeSlim()));
            Assert.IsTrue(TypeSlimExtensions.IsValueType(typeof(TimeSpan).ToTypeSlim()));

            Assert.IsFalse(TypeSlimExtensions.IsValueType(typeof(string).ToTypeSlim()));
            Assert.IsFalse(TypeSlimExtensions.IsValueType(typeof(AppDomain).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsNumeric()
        {
            var types = new[]
            {
                typeof(char),
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(float),
                typeof(double),
            };

            foreach (var t in types)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsTrue(TypeSlimExtensions.IsNumeric(t.ToTypeSlim()));
                Assert.IsTrue(TypeSlimExtensions.IsNumeric(n.ToTypeSlim()));
            }

            Assert.IsFalse(TypeSlimExtensions.IsNumeric(typeof(ConsoleColor).ToTypeSlim()));
            Assert.IsFalse(TypeSlimExtensions.IsNumeric(typeof(string).ToTypeSlim()));

            Assert.IsTrue(TypeSlimExtensions.IsNumeric(typeof(decimal).ToTypeSlim(), includeDecimal: true));
            Assert.IsTrue(TypeSlimExtensions.IsNumeric(typeof(decimal?).ToTypeSlim(), includeDecimal: true));

            Assert.IsFalse(TypeSlimExtensions.IsNumeric(typeof(decimal).ToTypeSlim(), includeDecimal: false));
            Assert.IsFalse(TypeSlimExtensions.IsNumeric(typeof(decimal?).ToTypeSlim(), includeDecimal: false));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsUnsignedInt()
        {
            var types = new[]
            {
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
            };

            var ntypes = new[]
            {
                typeof(char),
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(float),
                typeof(double),
            };

            foreach (var t in types)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsTrue(TypeSlimExtensions.IsUnsignedInt(t.ToTypeSlim()));
                Assert.IsTrue(TypeSlimExtensions.IsUnsignedInt(n.ToTypeSlim()));
            }

            foreach (var t in ntypes)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsFalse(TypeSlimExtensions.IsUnsignedInt(t.ToTypeSlim()));
                Assert.IsFalse(TypeSlimExtensions.IsUnsignedInt(n.ToTypeSlim()));
            }

            Assert.IsFalse(TypeSlimExtensions.IsUnsignedInt(typeof(ConsoleColor).ToTypeSlim()));
            Assert.IsFalse(TypeSlimExtensions.IsUnsignedInt(typeof(string).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsArithmetic()
        {
            var types = new[]
            {
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(float),
                typeof(double),
            };

            var ntypes = new[]
            {
                typeof(byte),
                typeof(sbyte),
                typeof(char),
                typeof(bool),
            };

            foreach (var t in types)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsTrue(TypeSlimExtensions.IsArithmetic(t.ToTypeSlim()));
                Assert.IsTrue(TypeSlimExtensions.IsArithmetic(n.ToTypeSlim()));
            }

            foreach (var t in ntypes)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsFalse(TypeSlimExtensions.IsArithmetic(t.ToTypeSlim()));
                Assert.IsFalse(TypeSlimExtensions.IsArithmetic(n.ToTypeSlim()));
            }

            Assert.IsFalse(TypeSlimExtensions.IsArithmetic(typeof(ConsoleColor).ToTypeSlim()));
            Assert.IsFalse(TypeSlimExtensions.IsArithmetic(typeof(string).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsInteger()
        {
            var types = new[]
            {
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
            };

            var ntypes = new[]
            {
                typeof(char),
                typeof(float),
                typeof(double),
                typeof(bool),
            };

            foreach (var t in types)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsTrue(TypeSlimExtensions.IsInteger(t.ToTypeSlim()));
                Assert.IsTrue(TypeSlimExtensions.IsInteger(n.ToTypeSlim()));
            }

            foreach (var t in ntypes)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsFalse(TypeSlimExtensions.IsInteger(t.ToTypeSlim()));
                Assert.IsFalse(TypeSlimExtensions.IsInteger(n.ToTypeSlim()));
            }

            Assert.IsFalse(TypeSlimExtensions.IsInteger(typeof(ConsoleColor).ToTypeSlim()));
            Assert.IsFalse(TypeSlimExtensions.IsInteger(typeof(string).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_IsIntegerOrBool()
        {
            var types = new[]
            {
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(bool),
            };

            var ntypes = new[]
            {
                typeof(char),
                typeof(float),
                typeof(double),
            };

            foreach (var t in types)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsTrue(TypeSlimExtensions.IsIntegerOrBool(t.ToTypeSlim()));
                Assert.IsTrue(TypeSlimExtensions.IsIntegerOrBool(n.ToTypeSlim()));
            }

            foreach (var t in ntypes)
            {
                var n = typeof(Nullable<>).MakeGenericType(t);

                Assert.IsFalse(TypeSlimExtensions.IsIntegerOrBool(t.ToTypeSlim()));
                Assert.IsFalse(TypeSlimExtensions.IsIntegerOrBool(n.ToTypeSlim()));
            }

            Assert.IsFalse(TypeSlimExtensions.IsIntegerOrBool(typeof(ConsoleColor).ToTypeSlim()));
            Assert.IsFalse(TypeSlimExtensions.IsIntegerOrBool(typeof(string).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_GetFuncType()
        {
            var eq = new TypeSlimEqualityComparer();

            var func0_0 = TypeSlimExtensions.GetFuncType(0);
            var func0_1 = TypeSlimExtensions.GetFuncType(0);
            var func0_2 = typeof(Func<>).ToTypeSlim();

            Assert.IsTrue(eq.Equals(func0_0, func0_1));
            Assert.IsTrue(eq.Equals(func0_0, func0_2));

            var func1_0 = TypeSlimExtensions.GetFuncType(1);
            var func1_1 = TypeSlimExtensions.GetFuncType(1);
            var func1_2 = typeof(Func<,>).ToTypeSlim();

            Assert.IsTrue(eq.Equals(func1_0, func1_1));
            Assert.IsTrue(eq.Equals(func1_0, func1_2));

            var func7_0 = TypeSlimExtensions.GetFuncType(7);
            var func7_1 = TypeSlimExtensions.GetFuncType(7);
            var func7_2 = typeof(Func<,,,,,,,>).ToTypeSlim();

            Assert.IsTrue(eq.Equals(func7_0, func7_1));
            Assert.IsTrue(eq.Equals(func7_0, func7_2));
        }

        [TestMethod]
        public void TypeSlimExtensions_GetActionType()
        {
            var eq = new TypeSlimEqualityComparer();

            var action0_0 = TypeSlimExtensions.GetActionType(1);
            var action0_1 = TypeSlimExtensions.GetActionType(1);
            var action0_2 = typeof(Action<>).ToTypeSlim();

            Assert.IsTrue(eq.Equals(action0_0, action0_1));
            Assert.IsTrue(eq.Equals(action0_0, action0_2));

            var action1_0 = TypeSlimExtensions.GetActionType(2);
            var action1_1 = TypeSlimExtensions.GetActionType(2);
            var action1_2 = typeof(Action<,>).ToTypeSlim();

            Assert.IsTrue(eq.Equals(action1_0, action1_1));
            Assert.IsTrue(eq.Equals(action1_0, action1_2));

            var action7_0 = TypeSlimExtensions.GetActionType(7);
            var action7_1 = TypeSlimExtensions.GetActionType(7);
            var action7_2 = typeof(Action<,,,,,,>).ToTypeSlim();

            Assert.IsTrue(eq.Equals(action7_0, action7_1));
            Assert.IsTrue(eq.Equals(action7_0, action7_2));
        }

        [TestMethod]
        public void TypeSlimExtensions_GetGenericTypeDefinition()
        {
            var genDef = typeof(List<>).ToTypeSlim();
            var res1 = genDef.GetGenericTypeDefinition();
            Assert.AreSame(res1, genDef);

            var genType = (GenericTypeSlim)typeof(List<int>).ToTypeSlim();
            var res2 = genType.GetGenericTypeDefinition();
            Assert.AreSame(res2, genType.GenericTypeDefinition);

            Assert.ThrowsException<InvalidOperationException>(() => typeof(int).ToTypeSlim().GetGenericTypeDefinition());
        }

        [TestMethod]
        public void TypeSlimExtensions_GetGenericArguments()
        {
            /* TODO - fill in when open generic types are supported
            var genDef = typeof(List<>).ToTypeSlim();
            var res1 = genDef.GetGenericArguments();
            */

            var genType = (GenericTypeSlim)typeof(List<int>).ToTypeSlim();
            var res2 = genType.GetGenericArguments();
            Assert.IsTrue(res2.Length == 1);
            Assert.AreSame(res2[0], genType.GenericArguments[0]);

            var res3 = typeof(int).ToTypeSlim().GetGenericArguments();
            Assert.IsTrue(res3.Length == 0);
        }

        [TestMethod]
        public void TypeSlimExtensions_MakeGenericType()
        {
            var genDef = typeof(List<>).ToTypeSlim();
            var genArg = typeof(int).ToTypeSlim();
            var genArgs = new[] { genArg }.ToReadOnly();

            var res = (GenericTypeSlim)genDef.MakeGenericType(genArgs);

            Assert.AreSame(genDef, res.GenericTypeDefinition);
            Assert.IsTrue(res.GenericArguments.Count == 1);
            Assert.AreSame(genArg, res.GenericArguments[0]);

            Assert.ThrowsException<InvalidOperationException>(() => typeof(int).ToTypeSlim().MakeGenericType(genArgs));
            Assert.ThrowsException<InvalidOperationException>(() => typeof(List<int>).ToTypeSlim().MakeGenericType(genArgs));
        }

        [TestMethod]
        public void TypeSlimExtensions_KnownTypes()
        {
            var d = new Dictionary<Type, TypeSlim>
            {
                { typeof(Action), TypeSlimExtensions.ActionType },
                { typeof(bool), TypeSlimExtensions.BooleanType },
                { typeof(Expression<>), TypeSlimExtensions.GenericExpressionType },
                { typeof(int), TypeSlimExtensions.IntegerType },
                { typeof(void), TypeSlimExtensions.VoidType },
            };

            var eqt = new TypeEqualityComparer();
            var eqs = new TypeSlimEqualityComparer();

            foreach (var kv in d)
            {
                Assert.IsTrue(eqt.Equals(kv.Key, kv.Value.ToType()));
                Assert.IsTrue(eqs.Equals(kv.Key.ToTypeSlim(), kv.Value));
            }
        }

        [TestMethod]
        public void TypeSlimExtensions_GetNonNullableType()
        {
            var n = typeof(Nullable<int>).ToTypeSlim();
            var x = typeof(List<int>).ToTypeSlim();
            var y = typeof(int).ToTypeSlim();

            var nn = n.GetNonNullableType();
            var xn = x.GetNonNullableType();
            var yn = y.GetNonNullableType();

            Assert.AreSame(x, xn);
            Assert.AreSame(y, yn);

            var eqs = new TypeSlimEqualityComparer();

            Assert.IsTrue(eqs.Equals(nn, typeof(int).ToTypeSlim()));
        }

        [TestMethod]
        public void TypeSlimExtensions_GetNullableType()
        {
            var ts = new[] { typeof(int), typeof(long), typeof(TimeSpan) };

            foreach (var t in ts)
            {
                var s = t.ToTypeSlim();
                var n = s.GetNullableType();

                Assert.IsTrue(n.IsNullableType());
                Assert.AreSame(s, n.GetNonNullableType());
            }

            var ns = new[] { typeof(Nullable<int>), typeof(string), typeof(List<int>) };

            foreach (var t in ns)
            {
                var s = t.ToTypeSlim();
                var n = s.GetNullableType();

                Assert.AreSame(s, n);
            }
        }

        [TestMethod]
        public void TypeSlimExtensions_IsValueType_More()
        {
            var y = new[] { typeof(int), typeof(long?), typeof(TimeSpan), typeof(Foo<>), typeof(Foo<string>) };
            var n = new[] { typeof(string), typeof(List<int>), typeof(int[]) };

            foreach (var t in y)
            {
                Assert.IsTrue(t.ToTypeSlim().IsValueType());
            }

            foreach (var t in n)
            {
                Assert.IsFalse(t.ToTypeSlim().IsValueType());
            }
        }

        private struct Foo<T>
        {
        }
    }
}
