// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeEqualityComparerTests
    {
        [TestMethod]
        public void TypeEquality_ProtectedNulls()
        {
            var eq = new TypeEqualityComparer();

            foreach (var equals in eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name.StartsWith("Equals")))
            {
                var t = default(object);

                var ps = equals.GetParameters();
                if (ps.Length == 2)
                {
                    if (ps[0].ParameterType == typeof(Type) && ps[1].ParameterType == typeof(Type))
                    {
                        t = typeof(int);
                    }
                    else if (ps[0].ParameterType == typeof(IEnumerable<Type>) && ps[1].ParameterType == typeof(IEnumerable<Type>))
                    {
                        t = new[] { typeof(int) };
                    }
                    else
                    {
                        continue;
                    }

                    Assert.IsTrue((bool)equals.Invoke(eq, new object[] { null, null }));
                    Assert.IsFalse((bool)equals.Invoke(eq, new object[] { t, null }));
                    Assert.IsFalse((bool)equals.Invoke(eq, new object[] { null, t }));
                }
            }

            foreach (var getHashCode in eq.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name.StartsWith("GetHashCode")))
            {
                var ps = getHashCode.GetParameters();
                if (ps.Length == 1)
                {
                    if (ps[0].ParameterType == typeof(Type))
                    {
                        Assert.AreNotEqual(0, (int)getHashCode.Invoke(eq, new object[] { null }));
                    }
                }
            }
        }

        [TestMethod]
        public void TypeEquality_Simple()
        {
            AssertEqual(typeof(int), typeof(int));
            AssertNotEqual(typeof(int), typeof(long));
        }

        [TestMethod]
        public void TypeEquality_Array()
        {
            AssertEqual(typeof(int[]), typeof(int[]));
            AssertNotEqual(typeof(int[]), typeof(long[]));

            AssertEqual(typeof(int[,]), typeof(int[,]));
            AssertNotEqual(typeof(int[,]), typeof(long[,]));

            AssertEqual(typeof(int).MakeArrayType(1), typeof(int).MakeArrayType(1));
            AssertNotEqual(typeof(int).MakeArrayType(1), typeof(long).MakeArrayType(1));
            AssertNotEqual(typeof(int).MakeArrayType(1), typeof(int[]));

            AssertNotEqual(typeof(int), typeof(int[]));
            AssertNotEqual(typeof(int), typeof(int[,]));
            AssertNotEqual(typeof(int[]), typeof(int[,]));
        }

        [TestMethod]
        public void TypeEquality_Pointer()
        {
            AssertEqual(typeof(int*), typeof(int*));
            AssertNotEqual(typeof(int*), typeof(long*));

            AssertNotEqual(typeof(int), typeof(int*));
            AssertNotEqual(typeof(int[]), typeof(int*));
        }

        [TestMethod]
        public void TypeEquality_ByRef()
        {
            AssertEqual(typeof(int).MakeByRefType(), typeof(int).MakeByRefType());
            AssertNotEqual(typeof(int).MakeByRefType(), typeof(long).MakeByRefType());

            AssertNotEqual(typeof(int), typeof(int).MakeByRefType());
            AssertNotEqual(typeof(int[]), typeof(int).MakeByRefType());
        }

        [TestMethod]
        public void TypeEquality_GenericDefinitions()
        {
            AssertEqual(typeof(Func<>), typeof(Func<>));
            AssertEqual(typeof(Func<,>), typeof(Func<,>));
            AssertNotEqual(typeof(Func<>), typeof(Action));
            AssertNotEqual(typeof(Func<>), typeof(Func<int>));
            AssertNotEqual(typeof(Func<>), typeof(Func<,>));
            AssertNotEqual(typeof(Func<>), typeof(Action<>));
        }

        [TestMethod]
        public void TypeEquality_GenericParameters()
        {
            AssertEqual(typeof(Func<>).GetGenericArguments()[0], typeof(Func<>).GetGenericArguments()[0]);
            AssertNotEqual(typeof(Func<>).GetGenericArguments()[0], typeof(Action<>).GetGenericArguments()[0]);
        }

        [TestMethod]
        public void TypeEquality_Generics()
        {
            AssertEqual(typeof(IEnumerable<int>), typeof(IEnumerable<int>));
            AssertNotEqual(typeof(IEnumerable<int>), typeof(IEnumerable<long>));

            AssertEqual(typeof(Func<int>), typeof(Func<int>));
            AssertEqual(typeof(Func<int, Func<int>>), typeof(Func<int, Func<int>>));
            AssertNotEqual(typeof(Func<int, int>), typeof(Func<int, int, int>));
            AssertNotEqual(typeof(Func<int, int>), typeof(Func<int, long>));
            AssertNotEqual(typeof(Func<int, int>), typeof(Func<long, int>));
        }

        private static void AssertEqual(Type x, Type y)
        {
            var eq = new TypeEqualityComparer();
            Assert.IsTrue(eq.Equals(x, y));
            Assert.IsTrue(eq.Equals(y, x));
            Assert.AreEqual(eq.GetHashCode(x), eq.GetHashCode(y));
        }

        private static void AssertNotEqual(Type x, Type y)
        {
            var eq = new TypeEqualityComparer();
            Assert.IsFalse(eq.Equals(x, y));
            Assert.IsFalse(eq.Equals(y, x));
            Assert.AreNotEqual(eq.GetHashCode(x), eq.GetHashCode(y));
        }
    }
}
