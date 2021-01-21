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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    partial class MemberTableTests
    {
        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod0));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid0));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod0));

            mt.Add(() => Stuff.StaticMethod0());

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid0));

            mt.Add(() => Stuff.StaticMethodVoid0());

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod0));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var args = new object[] {  };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid0));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var args = new object[] {  };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod0));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid0));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod0));

            mt.Add((Stuff s) => s.InstanceMethod0());

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid0));

            mt.Add((Stuff s) => s.InstanceMethodVoid0());

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod0));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var args = new object[] {  };
            var instanceAndArgs = new object[] { stuff,  };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid0()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid0));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var args = new object[] {  };
            var instanceAndArgs = new object[] { stuff,  };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod1));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid1));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod1));

            mt.Add((A0 a0) => Stuff.StaticMethod1(a0));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid1));

            mt.Add((A0 a0) => Stuff.StaticMethodVoid1(a0));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod1));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 72 };
            var args = new object[] { a0 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid1));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 69 };
            var args = new object[] { a0 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod1));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid1));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod1));

            mt.Add((Stuff s, A0 a0) => s.InstanceMethod1(a0));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid1));

            mt.Add((Stuff s, A0 a0) => s.InstanceMethodVoid1(a0));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod1));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 87 };
            var args = new object[] { a0 };
            var instanceAndArgs = new object[] { stuff, a0 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid1()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid1));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 83 };
            var args = new object[] { a0 };
            var instanceAndArgs = new object[] { stuff, a0 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod2));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid2));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod2));

            mt.Add((A0 a0, A1 a1) => Stuff.StaticMethod2(a0, a1));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid2));

            mt.Add((A0 a0, A1 a1) => Stuff.StaticMethodVoid2(a0, a1));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod2));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 55 };
            var a1 = new A1 { Value = 54 };
            var args = new object[] { a0, a1 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid2));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 46 };
            var a1 = new A1 { Value = 35 };
            var args = new object[] { a0, a1 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod2));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid2));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod2));

            mt.Add((Stuff s, A0 a0, A1 a1) => s.InstanceMethod2(a0, a1));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid2));

            mt.Add((Stuff s, A0 a0, A1 a1) => s.InstanceMethodVoid2(a0, a1));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod2));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 37 };
            var a1 = new A1 { Value = 55 };
            var args = new object[] { a0, a1 };
            var instanceAndArgs = new object[] { stuff, a0, a1 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid2()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid2));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 30 };
            var a1 = new A1 { Value = 66 };
            var args = new object[] { a0, a1 };
            var instanceAndArgs = new object[] { stuff, a0, a1 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod3));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid3));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod3));

            mt.Add((A0 a0, A1 a1, A2 a2) => Stuff.StaticMethod3(a0, a1, a2));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid3));

            mt.Add((A0 a0, A1 a1, A2 a2) => Stuff.StaticMethodVoid3(a0, a1, a2));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod3));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 86 };
            var a1 = new A1 { Value = 22 };
            var a2 = new A2 { Value = 12 };
            var args = new object[] { a0, a1, a2 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid3));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 50 };
            var a1 = new A1 { Value = 49 };
            var a2 = new A2 { Value = 50 };
            var args = new object[] { a0, a1, a2 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod3));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid3));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod3));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2) => s.InstanceMethod3(a0, a1, a2));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid3));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2) => s.InstanceMethodVoid3(a0, a1, a2));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod3));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 42 };
            var a1 = new A1 { Value = 22 };
            var a2 = new A2 { Value = 36 };
            var args = new object[] { a0, a1, a2 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid3()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid3));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 41 };
            var a1 = new A1 { Value = 21 };
            var a2 = new A2 { Value = 80 };
            var args = new object[] { a0, a1, a2 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod4));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid4));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod4));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3) => Stuff.StaticMethod4(a0, a1, a2, a3));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid4));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3) => Stuff.StaticMethodVoid4(a0, a1, a2, a3));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod4));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 14 };
            var a1 = new A1 { Value = 96 };
            var a2 = new A2 { Value = 86 };
            var a3 = new A3 { Value = 43 };
            var args = new object[] { a0, a1, a2, a3 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid4));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 34 };
            var a1 = new A1 { Value = 19 };
            var a2 = new A2 { Value = 33 };
            var a3 = new A3 { Value = 81 };
            var args = new object[] { a0, a1, a2, a3 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod4));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid4));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod4));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3) => s.InstanceMethod4(a0, a1, a2, a3));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid4));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3) => s.InstanceMethodVoid4(a0, a1, a2, a3));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod4));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 73 };
            var a1 = new A1 { Value = 73 };
            var a2 = new A2 { Value = 15 };
            var a3 = new A3 { Value = 73 };
            var args = new object[] { a0, a1, a2, a3 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid4()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid4));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 68 };
            var a1 = new A1 { Value = 81 };
            var a2 = new A2 { Value = 94 };
            var a3 = new A3 { Value = 91 };
            var args = new object[] { a0, a1, a2, a3 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod5));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid5));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod5));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => Stuff.StaticMethod5(a0, a1, a2, a3, a4));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid5));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => Stuff.StaticMethodVoid5(a0, a1, a2, a3, a4));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod5));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 86 };
            var a1 = new A1 { Value = 67 };
            var a2 = new A2 { Value = 71 };
            var a3 = new A3 { Value = 70 };
            var a4 = new A4 { Value = 47 };
            var args = new object[] { a0, a1, a2, a3, a4 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid5));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 19 };
            var a1 = new A1 { Value = 99 };
            var a2 = new A2 { Value = 57 };
            var a3 = new A3 { Value = 88 };
            var a4 = new A4 { Value = 38 };
            var args = new object[] { a0, a1, a2, a3, a4 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod5));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid5));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod5));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => s.InstanceMethod5(a0, a1, a2, a3, a4));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid5));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => s.InstanceMethodVoid5(a0, a1, a2, a3, a4));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod5));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 81 };
            var a1 = new A1 { Value = 51 };
            var a2 = new A2 { Value = 80 };
            var a3 = new A3 { Value = 53 };
            var a4 = new A4 { Value = 32 };
            var args = new object[] { a0, a1, a2, a3, a4 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid5()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid5));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 41 };
            var a1 = new A1 { Value = 58 };
            var a2 = new A2 { Value = 16 };
            var a3 = new A3 { Value = 79 };
            var a4 = new A4 { Value = 58 };
            var args = new object[] { a0, a1, a2, a3, a4 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod6));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid6));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod6));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => Stuff.StaticMethod6(a0, a1, a2, a3, a4, a5));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid6));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => Stuff.StaticMethodVoid6(a0, a1, a2, a3, a4, a5));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod6));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 68 };
            var a1 = new A1 { Value = 12 };
            var a2 = new A2 { Value = 10 };
            var a3 = new A3 { Value = 27 };
            var a4 = new A4 { Value = 31 };
            var a5 = new A5 { Value = 49 };
            var args = new object[] { a0, a1, a2, a3, a4, a5 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid6));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 92 };
            var a1 = new A1 { Value = 23 };
            var a2 = new A2 { Value = 16 };
            var a3 = new A3 { Value = 39 };
            var a4 = new A4 { Value = 82 };
            var a5 = new A5 { Value = 67 };
            var args = new object[] { a0, a1, a2, a3, a4, a5 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod6));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid6));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod6));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => s.InstanceMethod6(a0, a1, a2, a3, a4, a5));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid6));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => s.InstanceMethodVoid6(a0, a1, a2, a3, a4, a5));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod6));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 56 };
            var a1 = new A1 { Value = 50 };
            var a2 = new A2 { Value = 36 };
            var a3 = new A3 { Value = 69 };
            var a4 = new A4 { Value = 69 };
            var a5 = new A5 { Value = 50 };
            var args = new object[] { a0, a1, a2, a3, a4, a5 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid6()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid6));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 43 };
            var a1 = new A1 { Value = 94 };
            var a2 = new A2 { Value = 96 };
            var a3 = new A3 { Value = 39 };
            var a4 = new A4 { Value = 54 };
            var a5 = new A5 { Value = 96 };
            var args = new object[] { a0, a1, a2, a3, a4, a5 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod7));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid7));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod7));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => Stuff.StaticMethod7(a0, a1, a2, a3, a4, a5, a6));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid7));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => Stuff.StaticMethodVoid7(a0, a1, a2, a3, a4, a5, a6));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod7));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 38 };
            var a1 = new A1 { Value = 82 };
            var a2 = new A2 { Value = 10 };
            var a3 = new A3 { Value = 30 };
            var a4 = new A4 { Value = 50 };
            var a5 = new A5 { Value = 74 };
            var a6 = new A6 { Value = 24 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid7));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 61 };
            var a1 = new A1 { Value = 12 };
            var a2 = new A2 { Value = 45 };
            var a3 = new A3 { Value = 33 };
            var a4 = new A4 { Value = 83 };
            var a5 = new A5 { Value = 67 };
            var a6 = new A6 { Value = 53 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod7));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid7));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod7));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => s.InstanceMethod7(a0, a1, a2, a3, a4, a5, a6));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid7));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => s.InstanceMethodVoid7(a0, a1, a2, a3, a4, a5, a6));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod7));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 49 };
            var a1 = new A1 { Value = 97 };
            var a2 = new A2 { Value = 26 };
            var a3 = new A3 { Value = 86 };
            var a4 = new A4 { Value = 50 };
            var a5 = new A5 { Value = 59 };
            var a6 = new A6 { Value = 55 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid7()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid7));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 23 };
            var a1 = new A1 { Value = 94 };
            var a2 = new A2 { Value = 39 };
            var a3 = new A3 { Value = 26 };
            var a4 = new A4 { Value = 63 };
            var a5 = new A5 { Value = 71 };
            var a6 = new A6 { Value = 17 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod8));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid8));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod8));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => Stuff.StaticMethod8(a0, a1, a2, a3, a4, a5, a6, a7));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid8));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => Stuff.StaticMethodVoid8(a0, a1, a2, a3, a4, a5, a6, a7));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod8));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 73 };
            var a1 = new A1 { Value = 84 };
            var a2 = new A2 { Value = 62 };
            var a3 = new A3 { Value = 39 };
            var a4 = new A4 { Value = 58 };
            var a5 = new A5 { Value = 13 };
            var a6 = new A6 { Value = 89 };
            var a7 = new A7 { Value = 48 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid8));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 48 };
            var a1 = new A1 { Value = 71 };
            var a2 = new A2 { Value = 72 };
            var a3 = new A3 { Value = 41 };
            var a4 = new A4 { Value = 24 };
            var a5 = new A5 { Value = 31 };
            var a6 = new A6 { Value = 65 };
            var a7 = new A7 { Value = 21 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod8));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid8));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod8));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => s.InstanceMethod8(a0, a1, a2, a3, a4, a5, a6, a7));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid8));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => s.InstanceMethodVoid8(a0, a1, a2, a3, a4, a5, a6, a7));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod8));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 26 };
            var a1 = new A1 { Value = 53 };
            var a2 = new A2 { Value = 12 };
            var a3 = new A3 { Value = 25 };
            var a4 = new A4 { Value = 11 };
            var a5 = new A5 { Value = 45 };
            var a6 = new A6 { Value = 77 };
            var a7 = new A7 { Value = 19 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid8()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid8));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 88 };
            var a1 = new A1 { Value = 95 };
            var a2 = new A2 { Value = 51 };
            var a3 = new A3 { Value = 24 };
            var a4 = new A4 { Value = 88 };
            var a5 = new A5 { Value = 70 };
            var a6 = new A6 { Value = 14 };
            var a7 = new A7 { Value = 87 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod9));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid9));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod9));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => Stuff.StaticMethod9(a0, a1, a2, a3, a4, a5, a6, a7, a8));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid9));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => Stuff.StaticMethodVoid9(a0, a1, a2, a3, a4, a5, a6, a7, a8));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod9));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 13 };
            var a1 = new A1 { Value = 17 };
            var a2 = new A2 { Value = 87 };
            var a3 = new A3 { Value = 27 };
            var a4 = new A4 { Value = 82 };
            var a5 = new A5 { Value = 94 };
            var a6 = new A6 { Value = 35 };
            var a7 = new A7 { Value = 63 };
            var a8 = new A8 { Value = 64 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid9));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 10 };
            var a1 = new A1 { Value = 59 };
            var a2 = new A2 { Value = 55 };
            var a3 = new A3 { Value = 24 };
            var a4 = new A4 { Value = 18 };
            var a5 = new A5 { Value = 45 };
            var a6 = new A6 { Value = 34 };
            var a7 = new A7 { Value = 58 };
            var a8 = new A8 { Value = 83 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod9));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid9));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod9));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => s.InstanceMethod9(a0, a1, a2, a3, a4, a5, a6, a7, a8));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid9));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => s.InstanceMethodVoid9(a0, a1, a2, a3, a4, a5, a6, a7, a8));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod9));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 23 };
            var a1 = new A1 { Value = 73 };
            var a2 = new A2 { Value = 61 };
            var a3 = new A3 { Value = 55 };
            var a4 = new A4 { Value = 16 };
            var a5 = new A5 { Value = 38 };
            var a6 = new A6 { Value = 16 };
            var a7 = new A7 { Value = 52 };
            var a8 = new A8 { Value = 50 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid9()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid9));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 62 };
            var a1 = new A1 { Value = 62 };
            var a2 = new A2 { Value = 74 };
            var a3 = new A3 { Value = 60 };
            var a4 = new A4 { Value = 77 };
            var a5 = new A5 { Value = 67 };
            var a6 = new A6 { Value = 84 };
            var a7 = new A7 { Value = 38 };
            var a8 = new A8 { Value = 17 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod10));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid10));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod10));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => Stuff.StaticMethod10(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid10));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => Stuff.StaticMethodVoid10(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod10));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 43 };
            var a1 = new A1 { Value = 48 };
            var a2 = new A2 { Value = 38 };
            var a3 = new A3 { Value = 32 };
            var a4 = new A4 { Value = 28 };
            var a5 = new A5 { Value = 48 };
            var a6 = new A6 { Value = 61 };
            var a7 = new A7 { Value = 10 };
            var a8 = new A8 { Value = 86 };
            var a9 = new A9 { Value = 32 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid10));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 95 };
            var a1 = new A1 { Value = 79 };
            var a2 = new A2 { Value = 60 };
            var a3 = new A3 { Value = 27 };
            var a4 = new A4 { Value = 66 };
            var a5 = new A5 { Value = 15 };
            var a6 = new A6 { Value = 57 };
            var a7 = new A7 { Value = 41 };
            var a8 = new A8 { Value = 36 };
            var a9 = new A9 { Value = 58 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod10));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid10));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod10));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => s.InstanceMethod10(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid10));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => s.InstanceMethodVoid10(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod10));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 10 };
            var a1 = new A1 { Value = 59 };
            var a2 = new A2 { Value = 20 };
            var a3 = new A3 { Value = 40 };
            var a4 = new A4 { Value = 53 };
            var a5 = new A5 { Value = 72 };
            var a6 = new A6 { Value = 11 };
            var a7 = new A7 { Value = 89 };
            var a8 = new A8 { Value = 49 };
            var a9 = new A9 { Value = 81 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid10()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid10));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 88 };
            var a1 = new A1 { Value = 39 };
            var a2 = new A2 { Value = 80 };
            var a3 = new A3 { Value = 37 };
            var a4 = new A4 { Value = 90 };
            var a5 = new A5 { Value = 19 };
            var a6 = new A6 { Value = 54 };
            var a7 = new A7 { Value = 90 };
            var a8 = new A8 { Value = 54 };
            var a9 = new A9 { Value = 22 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod11));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid11));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod11));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => Stuff.StaticMethod11(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid11));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => Stuff.StaticMethodVoid11(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod11));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 93 };
            var a1 = new A1 { Value = 15 };
            var a2 = new A2 { Value = 51 };
            var a3 = new A3 { Value = 84 };
            var a4 = new A4 { Value = 56 };
            var a5 = new A5 { Value = 70 };
            var a6 = new A6 { Value = 12 };
            var a7 = new A7 { Value = 45 };
            var a8 = new A8 { Value = 18 };
            var a9 = new A9 { Value = 55 };
            var a10 = new A10 { Value = 30 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid11));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 36 };
            var a1 = new A1 { Value = 58 };
            var a2 = new A2 { Value = 79 };
            var a3 = new A3 { Value = 16 };
            var a4 = new A4 { Value = 83 };
            var a5 = new A5 { Value = 37 };
            var a6 = new A6 { Value = 98 };
            var a7 = new A7 { Value = 78 };
            var a8 = new A8 { Value = 56 };
            var a9 = new A9 { Value = 47 };
            var a10 = new A10 { Value = 72 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod11));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid11));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod11));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => s.InstanceMethod11(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid11));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => s.InstanceMethodVoid11(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod11));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 60 };
            var a1 = new A1 { Value = 14 };
            var a2 = new A2 { Value = 44 };
            var a3 = new A3 { Value = 65 };
            var a4 = new A4 { Value = 99 };
            var a5 = new A5 { Value = 32 };
            var a6 = new A6 { Value = 36 };
            var a7 = new A7 { Value = 56 };
            var a8 = new A8 { Value = 60 };
            var a9 = new A9 { Value = 66 };
            var a10 = new A10 { Value = 86 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid11()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid11));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 23 };
            var a1 = new A1 { Value = 64 };
            var a2 = new A2 { Value = 94 };
            var a3 = new A3 { Value = 17 };
            var a4 = new A4 { Value = 36 };
            var a5 = new A5 { Value = 83 };
            var a6 = new A6 { Value = 82 };
            var a7 = new A7 { Value = 69 };
            var a8 = new A8 { Value = 65 };
            var a9 = new A9 { Value = 81 };
            var a10 = new A10 { Value = 94 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod12));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid12));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod12));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => Stuff.StaticMethod12(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid12));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => Stuff.StaticMethodVoid12(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod12));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 60 };
            var a1 = new A1 { Value = 61 };
            var a2 = new A2 { Value = 81 };
            var a3 = new A3 { Value = 10 };
            var a4 = new A4 { Value = 30 };
            var a5 = new A5 { Value = 16 };
            var a6 = new A6 { Value = 82 };
            var a7 = new A7 { Value = 56 };
            var a8 = new A8 { Value = 21 };
            var a9 = new A9 { Value = 98 };
            var a10 = new A10 { Value = 75 };
            var a11 = new A11 { Value = 31 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid12));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 54 };
            var a1 = new A1 { Value = 47 };
            var a2 = new A2 { Value = 50 };
            var a3 = new A3 { Value = 91 };
            var a4 = new A4 { Value = 71 };
            var a5 = new A5 { Value = 80 };
            var a6 = new A6 { Value = 18 };
            var a7 = new A7 { Value = 61 };
            var a8 = new A8 { Value = 94 };
            var a9 = new A9 { Value = 63 };
            var a10 = new A10 { Value = 49 };
            var a11 = new A11 { Value = 44 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod12));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid12));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod12));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => s.InstanceMethod12(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid12));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => s.InstanceMethodVoid12(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod12));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 25 };
            var a1 = new A1 { Value = 22 };
            var a2 = new A2 { Value = 76 };
            var a3 = new A3 { Value = 11 };
            var a4 = new A4 { Value = 24 };
            var a5 = new A5 { Value = 95 };
            var a6 = new A6 { Value = 86 };
            var a7 = new A7 { Value = 81 };
            var a8 = new A8 { Value = 90 };
            var a9 = new A9 { Value = 66 };
            var a10 = new A10 { Value = 54 };
            var a11 = new A11 { Value = 82 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid12()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid12));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 84 };
            var a1 = new A1 { Value = 98 };
            var a2 = new A2 { Value = 11 };
            var a3 = new A3 { Value = 29 };
            var a4 = new A4 { Value = 74 };
            var a5 = new A5 { Value = 13 };
            var a6 = new A6 { Value = 55 };
            var a7 = new A7 { Value = 87 };
            var a8 = new A8 { Value = 48 };
            var a9 = new A9 { Value = 43 };
            var a10 = new A10 { Value = 49 };
            var a11 = new A11 { Value = 70 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod13));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid13));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod13));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => Stuff.StaticMethod13(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid13));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => Stuff.StaticMethodVoid13(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod13));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 86 };
            var a1 = new A1 { Value = 92 };
            var a2 = new A2 { Value = 21 };
            var a3 = new A3 { Value = 89 };
            var a4 = new A4 { Value = 57 };
            var a5 = new A5 { Value = 29 };
            var a6 = new A6 { Value = 99 };
            var a7 = new A7 { Value = 97 };
            var a8 = new A8 { Value = 22 };
            var a9 = new A9 { Value = 46 };
            var a10 = new A10 { Value = 85 };
            var a11 = new A11 { Value = 18 };
            var a12 = new A12 { Value = 40 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid13));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 81 };
            var a1 = new A1 { Value = 41 };
            var a2 = new A2 { Value = 25 };
            var a3 = new A3 { Value = 21 };
            var a4 = new A4 { Value = 93 };
            var a5 = new A5 { Value = 40 };
            var a6 = new A6 { Value = 88 };
            var a7 = new A7 { Value = 92 };
            var a8 = new A8 { Value = 67 };
            var a9 = new A9 { Value = 16 };
            var a10 = new A10 { Value = 72 };
            var a11 = new A11 { Value = 78 };
            var a12 = new A12 { Value = 89 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod13));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid13));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod13));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => s.InstanceMethod13(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid13));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => s.InstanceMethodVoid13(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod13));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 87 };
            var a1 = new A1 { Value = 91 };
            var a2 = new A2 { Value = 18 };
            var a3 = new A3 { Value = 61 };
            var a4 = new A4 { Value = 95 };
            var a5 = new A5 { Value = 82 };
            var a6 = new A6 { Value = 72 };
            var a7 = new A7 { Value = 15 };
            var a8 = new A8 { Value = 25 };
            var a9 = new A9 { Value = 32 };
            var a10 = new A10 { Value = 84 };
            var a11 = new A11 { Value = 97 };
            var a12 = new A12 { Value = 34 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid13()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid13));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 71 };
            var a1 = new A1 { Value = 67 };
            var a2 = new A2 { Value = 56 };
            var a3 = new A3 { Value = 69 };
            var a4 = new A4 { Value = 47 };
            var a5 = new A5 { Value = 23 };
            var a6 = new A6 { Value = 92 };
            var a7 = new A7 { Value = 88 };
            var a8 = new A8 { Value = 93 };
            var a9 = new A9 { Value = 71 };
            var a10 = new A10 { Value = 39 };
            var a11 = new A11 { Value = 75 };
            var a12 = new A12 { Value = 55 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod14));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid14));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod14));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => Stuff.StaticMethod14(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid14));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => Stuff.StaticMethodVoid14(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod14));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 12 };
            var a1 = new A1 { Value = 61 };
            var a2 = new A2 { Value = 77 };
            var a3 = new A3 { Value = 28 };
            var a4 = new A4 { Value = 85 };
            var a5 = new A5 { Value = 48 };
            var a6 = new A6 { Value = 20 };
            var a7 = new A7 { Value = 68 };
            var a8 = new A8 { Value = 42 };
            var a9 = new A9 { Value = 18 };
            var a10 = new A10 { Value = 89 };
            var a11 = new A11 { Value = 60 };
            var a12 = new A12 { Value = 50 };
            var a13 = new A13 { Value = 13 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid14));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 46 };
            var a1 = new A1 { Value = 35 };
            var a2 = new A2 { Value = 65 };
            var a3 = new A3 { Value = 19 };
            var a4 = new A4 { Value = 41 };
            var a5 = new A5 { Value = 24 };
            var a6 = new A6 { Value = 69 };
            var a7 = new A7 { Value = 69 };
            var a8 = new A8 { Value = 31 };
            var a9 = new A9 { Value = 46 };
            var a10 = new A10 { Value = 97 };
            var a11 = new A11 { Value = 69 };
            var a12 = new A12 { Value = 59 };
            var a13 = new A13 { Value = 86 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod14));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid14));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod14));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => s.InstanceMethod14(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid14));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => s.InstanceMethodVoid14(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod14));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 10 };
            var a1 = new A1 { Value = 94 };
            var a2 = new A2 { Value = 29 };
            var a3 = new A3 { Value = 79 };
            var a4 = new A4 { Value = 85 };
            var a5 = new A5 { Value = 50 };
            var a6 = new A6 { Value = 79 };
            var a7 = new A7 { Value = 20 };
            var a8 = new A8 { Value = 37 };
            var a9 = new A9 { Value = 97 };
            var a10 = new A10 { Value = 46 };
            var a11 = new A11 { Value = 45 };
            var a12 = new A12 { Value = 87 };
            var a13 = new A13 { Value = 66 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid14()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid14));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 38 };
            var a1 = new A1 { Value = 58 };
            var a2 = new A2 { Value = 67 };
            var a3 = new A3 { Value = 19 };
            var a4 = new A4 { Value = 96 };
            var a5 = new A5 { Value = 20 };
            var a6 = new A6 { Value = 55 };
            var a7 = new A7 { Value = 63 };
            var a8 = new A8 { Value = 37 };
            var a9 = new A9 { Value = 62 };
            var a10 = new A10 { Value = 97 };
            var a11 = new A11 { Value = 61 };
            var a12 = new A12 { Value = 85 };
            var a13 = new A13 { Value = 42 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod15));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid15));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod15));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => Stuff.StaticMethod15(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid15));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => Stuff.StaticMethodVoid15(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod15));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 40 };
            var a1 = new A1 { Value = 41 };
            var a2 = new A2 { Value = 30 };
            var a3 = new A3 { Value = 26 };
            var a4 = new A4 { Value = 89 };
            var a5 = new A5 { Value = 33 };
            var a6 = new A6 { Value = 68 };
            var a7 = new A7 { Value = 48 };
            var a8 = new A8 { Value = 89 };
            var a9 = new A9 { Value = 20 };
            var a10 = new A10 { Value = 74 };
            var a11 = new A11 { Value = 99 };
            var a12 = new A12 { Value = 33 };
            var a13 = new A13 { Value = 36 };
            var a14 = new A14 { Value = 97 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid15));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 68 };
            var a1 = new A1 { Value = 73 };
            var a2 = new A2 { Value = 95 };
            var a3 = new A3 { Value = 36 };
            var a4 = new A4 { Value = 13 };
            var a5 = new A5 { Value = 41 };
            var a6 = new A6 { Value = 72 };
            var a7 = new A7 { Value = 78 };
            var a8 = new A8 { Value = 88 };
            var a9 = new A9 { Value = 72 };
            var a10 = new A10 { Value = 49 };
            var a11 = new A11 { Value = 40 };
            var a12 = new A12 { Value = 46 };
            var a13 = new A13 { Value = 66 };
            var a14 = new A14 { Value = 66 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod15));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid15));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Instance15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod15));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => s.InstanceMethod15(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_InstanceVoid15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid15));

            mt.Add((Stuff s, A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => s.InstanceMethodVoid15(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod15));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 81 };
            var a1 = new A1 { Value = 34 };
            var a2 = new A2 { Value = 64 };
            var a3 = new A3 { Value = 47 };
            var a4 = new A4 { Value = 80 };
            var a5 = new A5 { Value = 96 };
            var a6 = new A6 { Value = 76 };
            var a7 = new A7 { Value = 29 };
            var a8 = new A8 { Value = 56 };
            var a9 = new A9 { Value = 64 };
            var a10 = new A10 { Value = 98 };
            var a11 = new A11 { Value = 90 };
            var a12 = new A12 { Value = 69 };
            var a13 = new A13 { Value = 57 };
            var a14 = new A14 { Value = 45 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid15()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid15));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 96 };
            var a1 = new A1 { Value = 86 };
            var a2 = new A2 { Value = 29 };
            var a3 = new A3 { Value = 66 };
            var a4 = new A4 { Value = 68 };
            var a5 = new A5 { Value = 88 };
            var a6 = new A6 { Value = 11 };
            var a7 = new A7 { Value = 35 };
            var a8 = new A8 { Value = 82 };
            var a9 = new A9 { Value = 11 };
            var a10 = new A10 { Value = 67 };
            var a11 = new A11 { Value = 62 };
            var a12 = new A12 { Value = 42 };
            var a13 = new A13 { Value = 54 };
            var a14 = new A14 { Value = 50 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Static16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod16));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_StaticVoid16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid16));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_Static16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod16));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => Stuff.StaticMethod16(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_MethodInfo_StaticVoid16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid16));

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => Stuff.StaticMethodVoid16(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15));

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethod16));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 92 };
            var a1 = new A1 { Value = 31 };
            var a2 = new A2 { Value = 81 };
            var a3 = new A3 { Value = 32 };
            var a4 = new A4 { Value = 38 };
            var a5 = new A5 { Value = 50 };
            var a6 = new A6 { Value = 45 };
            var a7 = new A7 { Value = 86 };
            var a8 = new A8 { Value = 56 };
            var a9 = new A9 { Value = 11 };
            var a10 = new A10 { Value = 91 };
            var a11 = new A11 { Value = 53 };
            var a12 = new A12 { Value = 49 };
            var a13 = new A13 { Value = 72 };
            var a14 = new A14 { Value = 14 };
            var a15 = new A15 { Value = 50 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };

            var expected = method.Invoke(obj: null, args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_StaticVoid16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.StaticMethodVoid16));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var a0 = new A0 { Value = 13 };
            var a1 = new A1 { Value = 31 };
            var a2 = new A2 { Value = 53 };
            var a3 = new A3 { Value = 75 };
            var a4 = new A4 { Value = 62 };
            var a5 = new A5 { Value = 21 };
            var a6 = new A6 { Value = 80 };
            var a7 = new A7 { Value = 97 };
            var a8 = new A8 { Value = 78 };
            var a9 = new A9 { Value = 79 };
            var a10 = new A10 { Value = 98 };
            var a11 = new A11 { Value = 82 };
            var a12 = new A12 { Value = 45 };
            var a13 = new A13 { Value = 22 };
            var a14 = new A14 { Value = 43 };
            var a15 = new A15 { Value = 44 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };

            lock (Stuff.StaticWriteLock)
            {
                Stuff.StaticCallResult = null;

                var expected = method.Invoke(obj: null, args);
                var expectedValue = Stuff.StaticCallResult.Value;

                Stuff.StaticCallResult = null;

                var actual = eval.DynamicInvoke(args);
                var actualValue = Stuff.StaticCallResult.Value;

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_Instance16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod16));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_InstanceVoid16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid16));

            mt.Add(method);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethod16));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 75 };
            var a1 = new A1 { Value = 15 };
            var a2 = new A2 { Value = 71 };
            var a3 = new A3 { Value = 76 };
            var a4 = new A4 { Value = 18 };
            var a5 = new A5 { Value = 46 };
            var a6 = new A6 { Value = 28 };
            var a7 = new A7 { Value = 94 };
            var a8 = new A8 { Value = 61 };
            var a9 = new A9 { Value = 10 };
            var a10 = new A10 { Value = 72 };
            var a11 = new A11 { Value = 64 };
            var a12 = new A12 { Value = 76 };
            var a13 = new A13 { Value = 44 };
            var a14 = new A14 { Value = 61 };
            var a15 = new A15 { Value = 63 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };

            var expected = method.Invoke(stuff, args);
            var actual = eval.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_InstanceVoid16()
        {
            var mt = new MemberTable();

            var method = typeof(Stuff).GetMethod(nameof(Stuff.InstanceMethodVoid16));

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var stuff = new Stuff { InstanceField = 42 };
            var a0 = new A0 { Value = 77 };
            var a1 = new A1 { Value = 60 };
            var a2 = new A2 { Value = 64 };
            var a3 = new A3 { Value = 40 };
            var a4 = new A4 { Value = 88 };
            var a5 = new A5 { Value = 78 };
            var a6 = new A6 { Value = 88 };
            var a7 = new A7 { Value = 80 };
            var a8 = new A8 { Value = 51 };
            var a9 = new A9 { Value = 83 };
            var a10 = new A10 { Value = 54 };
            var a11 = new A11 { Value = 59 };
            var a12 = new A12 { Value = 51 };
            var a13 = new A13 { Value = 63 };
            var a14 = new A14 { Value = 50 };
            var a15 = new A15 { Value = 43 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };
            var instanceAndArgs = new object[] { stuff, a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };

            stuff.InstanceCallResult = null;

            var expected = method.Invoke(stuff, args);
            var expectedValue = stuff.InstanceCallResult.Value;

            stuff.InstanceCallResult = null;

            var actual = eval.DynamicInvoke(instanceAndArgs);
            var actualValue = stuff.InstanceCallResult.Value;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance1()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance1()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1) });

            mt.Add((Stuff s, A1 a1) => s[a1]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance1()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1 }), stuff, p1);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance1()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 67 };
            var args = new object[] { a1 };
            var instanceAndArgs = new object[] { stuff, a1 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance2()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance2()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2) });

            mt.Add((Stuff s, A1 a1, A2 a2) => s[a1, a2]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance2()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2 }), stuff, p1, p2);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance2()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 57 };
            var a2 = new A2 { Value = 77 };
            var args = new object[] { a1, a2 };
            var instanceAndArgs = new object[] { stuff, a1, a2 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance3()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance3()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3) => s[a1, a2, a3]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance3()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3 }), stuff, p1, p2, p3);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance3()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 44 };
            var a2 = new A2 { Value = 10 };
            var a3 = new A3 { Value = 38 };
            var args = new object[] { a1, a2, a3 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance4()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance4()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4) => s[a1, a2, a3, a4]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance4()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4 }), stuff, p1, p2, p3, p4);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance4()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 42 };
            var a2 = new A2 { Value = 66 };
            var a3 = new A3 { Value = 12 };
            var a4 = new A4 { Value = 58 };
            var args = new object[] { a1, a2, a3, a4 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance5()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance5()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => s[a1, a2, a3, a4, a5]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance5()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5 }), stuff, p1, p2, p3, p4, p5);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance5()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 24 };
            var a2 = new A2 { Value = 61 };
            var a3 = new A3 { Value = 48 };
            var a4 = new A4 { Value = 25 };
            var a5 = new A5 { Value = 31 };
            var args = new object[] { a1, a2, a3, a4, a5 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance6()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance6()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => s[a1, a2, a3, a4, a5, a6]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance6()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6 }), stuff, p1, p2, p3, p4, p5, p6);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance6()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 43 };
            var a2 = new A2 { Value = 27 };
            var a3 = new A3 { Value = 45 };
            var a4 = new A4 { Value = 14 };
            var a5 = new A5 { Value = 85 };
            var a6 = new A6 { Value = 58 };
            var args = new object[] { a1, a2, a3, a4, a5, a6 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance7()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance7()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => s[a1, a2, a3, a4, a5, a6, a7]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance7()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7 }), stuff, p1, p2, p3, p4, p5, p6, p7);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance7()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 12 };
            var a2 = new A2 { Value = 55 };
            var a3 = new A3 { Value = 97 };
            var a4 = new A4 { Value = 27 };
            var a5 = new A5 { Value = 90 };
            var a6 = new A6 { Value = 35 };
            var a7 = new A7 { Value = 34 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance8()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance8()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => s[a1, a2, a3, a4, a5, a6, a7, a8]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance8()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance8()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 92 };
            var a2 = new A2 { Value = 69 };
            var a3 = new A3 { Value = 52 };
            var a4 = new A4 { Value = 98 };
            var a5 = new A5 { Value = 60 };
            var a6 = new A6 { Value = 39 };
            var a7 = new A7 { Value = 97 };
            var a8 = new A8 { Value = 29 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance9()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance9()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance9()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance9()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 67 };
            var a2 = new A2 { Value = 27 };
            var a3 = new A3 { Value = 62 };
            var a4 = new A4 { Value = 49 };
            var a5 = new A5 { Value = 28 };
            var a6 = new A6 { Value = 97 };
            var a7 = new A7 { Value = 38 };
            var a8 = new A8 { Value = 39 };
            var a9 = new A9 { Value = 64 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance10()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance10()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9, a10]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance10()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance10()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 36 };
            var a2 = new A2 { Value = 41 };
            var a3 = new A3 { Value = 35 };
            var a4 = new A4 { Value = 62 };
            var a5 = new A5 { Value = 20 };
            var a6 = new A6 { Value = 41 };
            var a7 = new A7 { Value = 16 };
            var a8 = new A8 { Value = 58 };
            var a9 = new A9 { Value = 64 };
            var a10 = new A10 { Value = 85 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance11()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance11()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance11()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var p11 = Expression.Parameter(typeof(A11));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance11()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 64 };
            var a2 = new A2 { Value = 11 };
            var a3 = new A3 { Value = 80 };
            var a4 = new A4 { Value = 26 };
            var a5 = new A5 { Value = 20 };
            var a6 = new A6 { Value = 12 };
            var a7 = new A7 { Value = 17 };
            var a8 = new A8 { Value = 73 };
            var a9 = new A9 { Value = 43 };
            var a10 = new A10 { Value = 16 };
            var a11 = new A11 { Value = 25 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance12()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance12()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance12()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var p11 = Expression.Parameter(typeof(A11));
            var p12 = Expression.Parameter(typeof(A12));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance12()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 11 };
            var a2 = new A2 { Value = 19 };
            var a3 = new A3 { Value = 27 };
            var a4 = new A4 { Value = 11 };
            var a5 = new A5 { Value = 76 };
            var a6 = new A6 { Value = 10 };
            var a7 = new A7 { Value = 83 };
            var a8 = new A8 { Value = 64 };
            var a9 = new A9 { Value = 67 };
            var a10 = new A10 { Value = 60 };
            var a11 = new A11 { Value = 74 };
            var a12 = new A12 { Value = 25 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance13()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance13()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance13()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var p11 = Expression.Parameter(typeof(A11));
            var p12 = Expression.Parameter(typeof(A12));
            var p13 = Expression.Parameter(typeof(A13));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance13()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 43 };
            var a2 = new A2 { Value = 90 };
            var a3 = new A3 { Value = 58 };
            var a4 = new A4 { Value = 99 };
            var a5 = new A5 { Value = 72 };
            var a6 = new A6 { Value = 82 };
            var a7 = new A7 { Value = 38 };
            var a8 = new A8 { Value = 46 };
            var a9 = new A9 { Value = 49 };
            var a10 = new A10 { Value = 95 };
            var a11 = new A11 { Value = 54 };
            var a12 = new A12 { Value = 43 };
            var a13 = new A13 { Value = 28 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance14()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance14()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance14()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var p11 = Expression.Parameter(typeof(A11));
            var p12 = Expression.Parameter(typeof(A12));
            var p13 = Expression.Parameter(typeof(A13));
            var p14 = Expression.Parameter(typeof(A14));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance14()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 86 };
            var a2 = new A2 { Value = 10 };
            var a3 = new A3 { Value = 51 };
            var a4 = new A4 { Value = 46 };
            var a5 = new A5 { Value = 20 };
            var a6 = new A6 { Value = 33 };
            var a7 = new A7 { Value = 95 };
            var a8 = new A8 { Value = 33 };
            var a9 = new A9 { Value = 48 };
            var a10 = new A10 { Value = 35 };
            var a11 = new A11 { Value = 32 };
            var a12 = new A12 { Value = 17 };
            var a13 = new A13 { Value = 60 };
            var a14 = new A14 { Value = 44 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance15()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Indexer_Instance15()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            mt.Add((Stuff s, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => s[a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15]);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance15()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var p11 = Expression.Parameter(typeof(A11));
            var p12 = Expression.Parameter(typeof(A12));
            var p13 = Expression.Parameter(typeof(A13));
            var p14 = Expression.Parameter(typeof(A14));
            var p15 = Expression.Parameter(typeof(A15));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance15()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 40 };
            var a2 = new A2 { Value = 32 };
            var a3 = new A3 { Value = 94 };
            var a4 = new A4 { Value = 97 };
            var a5 = new A5 { Value = 34 };
            var a6 = new A6 { Value = 89 };
            var a7 = new A7 { Value = 85 };
            var a8 = new A8 { Value = 46 };
            var a9 = new A9 { Value = 35 };
            var a10 = new A10 { Value = 61 };
            var a11 = new A11 { Value = 13 };
            var a12 = new A12 { Value = 45 };
            var a13 = new A13 { Value = 91 };
            var a14 = new A14 { Value = 14 };
            var a15 = new A15 { Value = 69 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Indexer_Instance16()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15), typeof(A16) });

            mt.Add(property);

            AssertHasMember(mt, property);
            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Add_IndexExpression_PropertyInfo_Indexer_Instance16()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15), typeof(A16) });

            var stuff = Expression.Parameter(typeof(Stuff));
            var p1 = Expression.Parameter(typeof(A1));
            var p2 = Expression.Parameter(typeof(A2));
            var p3 = Expression.Parameter(typeof(A3));
            var p4 = Expression.Parameter(typeof(A4));
            var p5 = Expression.Parameter(typeof(A5));
            var p6 = Expression.Parameter(typeof(A6));
            var p7 = Expression.Parameter(typeof(A7));
            var p8 = Expression.Parameter(typeof(A8));
            var p9 = Expression.Parameter(typeof(A9));
            var p10 = Expression.Parameter(typeof(A10));
            var p11 = Expression.Parameter(typeof(A11));
            var p12 = Expression.Parameter(typeof(A12));
            var p13 = Expression.Parameter(typeof(A13));
            var p14 = Expression.Parameter(typeof(A14));
            var p15 = Expression.Parameter(typeof(A15));
            var p16 = Expression.Parameter(typeof(A16));
            var e = Expression.Lambda(Expression.MakeIndex(stuff, property, new Expression[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16 }), stuff, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
            mt.Add(e);

            AssertHasMember(mt, property.GetGetMethod());
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Indexer_Instance16()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty("Item", new[] { typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15), typeof(A16) });

            mt.Add(property);

            var eval1 = GetEvaluator(mt, property);
            var eval2 = GetEvaluator(mt, property.GetGetMethod());

            var stuff = new Stuff();
            var a1 = new A1 { Value = 75 };
            var a2 = new A2 { Value = 15 };
            var a3 = new A3 { Value = 65 };
            var a4 = new A4 { Value = 84 };
            var a5 = new A5 { Value = 83 };
            var a6 = new A6 { Value = 89 };
            var a7 = new A7 { Value = 99 };
            var a8 = new A8 { Value = 41 };
            var a9 = new A9 { Value = 27 };
            var a10 = new A10 { Value = 56 };
            var a11 = new A11 { Value = 36 };
            var a12 = new A12 { Value = 79 };
            var a13 = new A13 { Value = 92 };
            var a14 = new A14 { Value = 94 };
            var a15 = new A15 { Value = 65 };
            var a16 = new A16 { Value = 36 };
            var args = new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 };
            var instanceAndArgs = new object[] { stuff, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 };

            var expected = property.GetValue(stuff, args);
            var actual1 = eval1.DynamicInvoke(instanceAndArgs);
            var actual2 = eval2.DynamicInvoke(instanceAndArgs);

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance0()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff0).GetConstructor(new Type[] {  });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance0()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff0).GetConstructor(new Type[] {  });

            mt.Add(() => new Stuff0());

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance0()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff0).GetConstructor(new Type[] {  });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var args = new object[] {  };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance1()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff1).GetConstructor(new Type[] { typeof(A0) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance1()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff1).GetConstructor(new Type[] { typeof(A0) });

            mt.Add((A0 a0) => new Stuff1(a0));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance1()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff1).GetConstructor(new Type[] { typeof(A0) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 92 };
            var args = new object[] { a0 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance2()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff2).GetConstructor(new Type[] { typeof(A0), typeof(A1) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance2()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff2).GetConstructor(new Type[] { typeof(A0), typeof(A1) });

            mt.Add((A0 a0, A1 a1) => new Stuff2(a0, a1));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance2()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff2).GetConstructor(new Type[] { typeof(A0), typeof(A1) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 21 };
            var a1 = new A1 { Value = 48 };
            var args = new object[] { a0, a1 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance3()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff3).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance3()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff3).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2) });

            mt.Add((A0 a0, A1 a1, A2 a2) => new Stuff3(a0, a1, a2));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance3()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff3).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 97 };
            var a1 = new A1 { Value = 23 };
            var a2 = new A2 { Value = 55 };
            var args = new object[] { a0, a1, a2 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance4()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff4).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance4()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff4).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3) => new Stuff4(a0, a1, a2, a3));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance4()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff4).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 97 };
            var a1 = new A1 { Value = 29 };
            var a2 = new A2 { Value = 53 };
            var a3 = new A3 { Value = 42 };
            var args = new object[] { a0, a1, a2, a3 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance5()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff5).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance5()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff5).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => new Stuff5(a0, a1, a2, a3, a4));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance5()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff5).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 49 };
            var a1 = new A1 { Value = 74 };
            var a2 = new A2 { Value = 90 };
            var a3 = new A3 { Value = 43 };
            var a4 = new A4 { Value = 75 };
            var args = new object[] { a0, a1, a2, a3, a4 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance6()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff6).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance6()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff6).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => new Stuff6(a0, a1, a2, a3, a4, a5));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance6()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff6).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 42 };
            var a1 = new A1 { Value = 90 };
            var a2 = new A2 { Value = 64 };
            var a3 = new A3 { Value = 72 };
            var a4 = new A4 { Value = 29 };
            var a5 = new A5 { Value = 66 };
            var args = new object[] { a0, a1, a2, a3, a4, a5 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance7()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff7).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance7()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff7).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => new Stuff7(a0, a1, a2, a3, a4, a5, a6));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance7()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff7).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 33 };
            var a1 = new A1 { Value = 76 };
            var a2 = new A2 { Value = 54 };
            var a3 = new A3 { Value = 41 };
            var a4 = new A4 { Value = 90 };
            var a5 = new A5 { Value = 76 };
            var a6 = new A6 { Value = 50 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance8()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff8).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance8()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff8).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => new Stuff8(a0, a1, a2, a3, a4, a5, a6, a7));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance8()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff8).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 97 };
            var a1 = new A1 { Value = 19 };
            var a2 = new A2 { Value = 93 };
            var a3 = new A3 { Value = 52 };
            var a4 = new A4 { Value = 70 };
            var a5 = new A5 { Value = 34 };
            var a6 = new A6 { Value = 20 };
            var a7 = new A7 { Value = 33 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance9()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff9).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance9()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff9).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => new Stuff9(a0, a1, a2, a3, a4, a5, a6, a7, a8));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance9()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff9).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 53 };
            var a1 = new A1 { Value = 17 };
            var a2 = new A2 { Value = 55 };
            var a3 = new A3 { Value = 30 };
            var a4 = new A4 { Value = 17 };
            var a5 = new A5 { Value = 45 };
            var a6 = new A6 { Value = 40 };
            var a7 = new A7 { Value = 51 };
            var a8 = new A8 { Value = 50 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance10()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff10).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance10()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff10).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => new Stuff10(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance10()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff10).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 35 };
            var a1 = new A1 { Value = 51 };
            var a2 = new A2 { Value = 84 };
            var a3 = new A3 { Value = 81 };
            var a4 = new A4 { Value = 94 };
            var a5 = new A5 { Value = 89 };
            var a6 = new A6 { Value = 37 };
            var a7 = new A7 { Value = 31 };
            var a8 = new A8 { Value = 45 };
            var a9 = new A9 { Value = 70 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance11()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff11).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance11()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff11).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => new Stuff11(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance11()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff11).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 69 };
            var a1 = new A1 { Value = 45 };
            var a2 = new A2 { Value = 93 };
            var a3 = new A3 { Value = 66 };
            var a4 = new A4 { Value = 33 };
            var a5 = new A5 { Value = 78 };
            var a6 = new A6 { Value = 56 };
            var a7 = new A7 { Value = 32 };
            var a8 = new A8 { Value = 43 };
            var a9 = new A9 { Value = 49 };
            var a10 = new A10 { Value = 97 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance12()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff12).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance12()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff12).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => new Stuff12(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance12()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff12).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 14 };
            var a1 = new A1 { Value = 65 };
            var a2 = new A2 { Value = 32 };
            var a3 = new A3 { Value = 52 };
            var a4 = new A4 { Value = 88 };
            var a5 = new A5 { Value = 83 };
            var a6 = new A6 { Value = 18 };
            var a7 = new A7 { Value = 52 };
            var a8 = new A8 { Value = 21 };
            var a9 = new A9 { Value = 30 };
            var a10 = new A10 { Value = 92 };
            var a11 = new A11 { Value = 35 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance13()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff13).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance13()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff13).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => new Stuff13(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance13()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff13).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 14 };
            var a1 = new A1 { Value = 15 };
            var a2 = new A2 { Value = 49 };
            var a3 = new A3 { Value = 92 };
            var a4 = new A4 { Value = 69 };
            var a5 = new A5 { Value = 12 };
            var a6 = new A6 { Value = 30 };
            var a7 = new A7 { Value = 65 };
            var a8 = new A8 { Value = 30 };
            var a9 = new A9 { Value = 34 };
            var a10 = new A10 { Value = 64 };
            var a11 = new A11 { Value = 51 };
            var a12 = new A12 { Value = 87 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance14()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff14).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance14()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff14).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => new Stuff14(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance14()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff14).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 60 };
            var a1 = new A1 { Value = 50 };
            var a2 = new A2 { Value = 32 };
            var a3 = new A3 { Value = 51 };
            var a4 = new A4 { Value = 61 };
            var a5 = new A5 { Value = 23 };
            var a6 = new A6 { Value = 96 };
            var a7 = new A7 { Value = 11 };
            var a8 = new A8 { Value = 52 };
            var a9 = new A9 { Value = 31 };
            var a10 = new A10 { Value = 85 };
            var a11 = new A11 { Value = 61 };
            var a12 = new A12 { Value = 38 };
            var a13 = new A13 { Value = 16 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance15()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff15).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance15()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff15).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => new Stuff15(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance15()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff15).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 15 };
            var a1 = new A1 { Value = 29 };
            var a2 = new A2 { Value = 79 };
            var a3 = new A3 { Value = 33 };
            var a4 = new A4 { Value = 49 };
            var a5 = new A5 { Value = 76 };
            var a6 = new A6 { Value = 19 };
            var a7 = new A7 { Value = 89 };
            var a8 = new A8 { Value = 60 };
            var a9 = new A9 { Value = 84 };
            var a10 = new A10 { Value = 86 };
            var a11 = new A11 { Value = 87 };
            var a12 = new A12 { Value = 29 };
            var a13 = new A13 { Value = 22 };
            var a14 = new A14 { Value = 83 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_Instance16()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff16).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            mt.Add(constructor);

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_ConstructorInfo_Instance16()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff16).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            mt.Add((A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => new Stuff16(a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15));

            AssertHasMember(mt, constructor);
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance16()
        {
            var mt = new MemberTable();

            var constructor = typeof(Stuff16).GetConstructor(new Type[] { typeof(A0), typeof(A1), typeof(A2), typeof(A3), typeof(A4), typeof(A5), typeof(A6), typeof(A7), typeof(A8), typeof(A9), typeof(A10), typeof(A11), typeof(A12), typeof(A13), typeof(A14), typeof(A15) });

            mt.Add(constructor);

            var eval = GetEvaluator(mt, constructor);

            var stuff = new Stuff();
            var a0 = new A0 { Value = 77 };
            var a1 = new A1 { Value = 79 };
            var a2 = new A2 { Value = 11 };
            var a3 = new A3 { Value = 80 };
            var a4 = new A4 { Value = 64 };
            var a5 = new A5 { Value = 38 };
            var a6 = new A6 { Value = 42 };
            var a7 = new A7 { Value = 86 };
            var a8 = new A8 { Value = 10 };
            var a9 = new A9 { Value = 60 };
            var a10 = new A10 { Value = 17 };
            var a11 = new A11 { Value = 95 };
            var a12 = new A12 { Value = 33 };
            var a13 = new A13 { Value = 61 };
            var a14 = new A14 { Value = 84 };
            var a15 = new A15 { Value = 63 };
            var args = new object[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 };

            var expected = constructor.Invoke(args);
            var actual = eval.DynamicInvoke(args);

            Assert.AreEqual(expected, actual);
        }

        partial class Stuff
        {
            public static int? StaticCallResult;
            public int? InstanceCallResult;

            public static int StaticMethod0() => StaticField;
            public static void StaticMethodVoid0() { StaticCallResult = StaticField; }
            public static int StaticMethod1(A0 a0) => StaticField + a0.Value;
            public static void StaticMethodVoid1(A0 a0) { StaticCallResult = StaticField + a0.Value; }
            public static int StaticMethod2(A0 a0, A1 a1) => StaticField + a0.Value + a1.Value;
            public static void StaticMethodVoid2(A0 a0, A1 a1) { StaticCallResult = StaticField + a0.Value + a1.Value; }
            public static int StaticMethod3(A0 a0, A1 a1, A2 a2) => StaticField + a0.Value + a1.Value + a2.Value;
            public static void StaticMethodVoid3(A0 a0, A1 a1, A2 a2) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value; }
            public static int StaticMethod4(A0 a0, A1 a1, A2 a2, A3 a3) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value;
            public static void StaticMethodVoid4(A0 a0, A1 a1, A2 a2, A3 a3) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value; }
            public static int StaticMethod5(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value;
            public static void StaticMethodVoid5(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value; }
            public static int StaticMethod6(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value;
            public static void StaticMethodVoid6(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value; }
            public static int StaticMethod7(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value;
            public static void StaticMethodVoid7(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value; }
            public static int StaticMethod8(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value;
            public static void StaticMethodVoid8(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value; }
            public static int StaticMethod9(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value;
            public static void StaticMethodVoid9(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value; }
            public static int StaticMethod10(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value;
            public static void StaticMethodVoid10(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value; }
            public static int StaticMethod11(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value;
            public static void StaticMethodVoid11(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value; }
            public static int StaticMethod12(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value;
            public static void StaticMethodVoid12(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value; }
            public static int StaticMethod13(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value;
            public static void StaticMethodVoid13(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value; }
            public static int StaticMethod14(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value;
            public static void StaticMethodVoid14(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value; }
            public static int StaticMethod15(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value;
            public static void StaticMethodVoid15(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value; }
            public static int StaticMethod16(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value;
            public static void StaticMethodVoid16(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) { StaticCallResult = StaticField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value; }
            public int InstanceMethod0() => InstanceField;
            public void InstanceMethodVoid0() { InstanceCallResult = InstanceField; }
            public int InstanceMethod1(A0 a0) => InstanceField + a0.Value;
            public void InstanceMethodVoid1(A0 a0) { InstanceCallResult = InstanceField + a0.Value; }
            public int InstanceMethod2(A0 a0, A1 a1) => InstanceField + a0.Value + a1.Value;
            public void InstanceMethodVoid2(A0 a0, A1 a1) { InstanceCallResult = InstanceField + a0.Value + a1.Value; }
            public int InstanceMethod3(A0 a0, A1 a1, A2 a2) => InstanceField + a0.Value + a1.Value + a2.Value;
            public void InstanceMethodVoid3(A0 a0, A1 a1, A2 a2) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value; }
            public int InstanceMethod4(A0 a0, A1 a1, A2 a2, A3 a3) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value;
            public void InstanceMethodVoid4(A0 a0, A1 a1, A2 a2, A3 a3) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value; }
            public int InstanceMethod5(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value;
            public void InstanceMethodVoid5(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value; }
            public int InstanceMethod6(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value;
            public void InstanceMethodVoid6(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value; }
            public int InstanceMethod7(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value;
            public void InstanceMethodVoid7(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value; }
            public int InstanceMethod8(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value;
            public void InstanceMethodVoid8(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value; }
            public int InstanceMethod9(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value;
            public void InstanceMethodVoid9(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value; }
            public int InstanceMethod10(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value;
            public void InstanceMethodVoid10(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value; }
            public int InstanceMethod11(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value;
            public void InstanceMethodVoid11(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value; }
            public int InstanceMethod12(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value;
            public void InstanceMethodVoid12(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value; }
            public int InstanceMethod13(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value;
            public void InstanceMethodVoid13(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value; }
            public int InstanceMethod14(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value;
            public void InstanceMethodVoid14(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value; }
            public int InstanceMethod15(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value;
            public void InstanceMethodVoid15(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value; }
            public int InstanceMethod16(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value;
            public void InstanceMethodVoid16(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) { InstanceCallResult = InstanceField + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value; }
            public int this[A1 a1] => InstanceField + a1.Value;
            public int this[A1 a1, A2 a2] => InstanceField + a1.Value + a2.Value;
            public int this[A1 a1, A2 a2, A3 a3] => InstanceField + a1.Value + a2.Value + a3.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value;
            public int this[A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15, A16 a16] => InstanceField + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value + a16.Value;
        }

        private class Stuff0 : IEquatable<Stuff0>
        {
            public int Value;

            public Stuff0() => Value = 42;

            public bool Equals(Stuff0 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff0);
            public override int GetHashCode() => Value;
        }

        private class Stuff1 : IEquatable<Stuff1>
        {
            public int Value;

            public Stuff1(A0 a0) => Value = 42 + a0.Value;

            public bool Equals(Stuff1 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff1);
            public override int GetHashCode() => Value;
        }

        private class Stuff2 : IEquatable<Stuff2>
        {
            public int Value;

            public Stuff2(A0 a0, A1 a1) => Value = 42 + a0.Value + a1.Value;

            public bool Equals(Stuff2 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff2);
            public override int GetHashCode() => Value;
        }

        private class Stuff3 : IEquatable<Stuff3>
        {
            public int Value;

            public Stuff3(A0 a0, A1 a1, A2 a2) => Value = 42 + a0.Value + a1.Value + a2.Value;

            public bool Equals(Stuff3 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff3);
            public override int GetHashCode() => Value;
        }

        private class Stuff4 : IEquatable<Stuff4>
        {
            public int Value;

            public Stuff4(A0 a0, A1 a1, A2 a2, A3 a3) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value;

            public bool Equals(Stuff4 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff4);
            public override int GetHashCode() => Value;
        }

        private class Stuff5 : IEquatable<Stuff5>
        {
            public int Value;

            public Stuff5(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value;

            public bool Equals(Stuff5 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff5);
            public override int GetHashCode() => Value;
        }

        private class Stuff6 : IEquatable<Stuff6>
        {
            public int Value;

            public Stuff6(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value;

            public bool Equals(Stuff6 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff6);
            public override int GetHashCode() => Value;
        }

        private class Stuff7 : IEquatable<Stuff7>
        {
            public int Value;

            public Stuff7(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value;

            public bool Equals(Stuff7 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff7);
            public override int GetHashCode() => Value;
        }

        private class Stuff8 : IEquatable<Stuff8>
        {
            public int Value;

            public Stuff8(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value;

            public bool Equals(Stuff8 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff8);
            public override int GetHashCode() => Value;
        }

        private class Stuff9 : IEquatable<Stuff9>
        {
            public int Value;

            public Stuff9(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value;

            public bool Equals(Stuff9 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff9);
            public override int GetHashCode() => Value;
        }

        private class Stuff10 : IEquatable<Stuff10>
        {
            public int Value;

            public Stuff10(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value;

            public bool Equals(Stuff10 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff10);
            public override int GetHashCode() => Value;
        }

        private class Stuff11 : IEquatable<Stuff11>
        {
            public int Value;

            public Stuff11(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value;

            public bool Equals(Stuff11 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff11);
            public override int GetHashCode() => Value;
        }

        private class Stuff12 : IEquatable<Stuff12>
        {
            public int Value;

            public Stuff12(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value;

            public bool Equals(Stuff12 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff12);
            public override int GetHashCode() => Value;
        }

        private class Stuff13 : IEquatable<Stuff13>
        {
            public int Value;

            public Stuff13(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value;

            public bool Equals(Stuff13 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff13);
            public override int GetHashCode() => Value;
        }

        private class Stuff14 : IEquatable<Stuff14>
        {
            public int Value;

            public Stuff14(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value;

            public bool Equals(Stuff14 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff14);
            public override int GetHashCode() => Value;
        }

        private class Stuff15 : IEquatable<Stuff15>
        {
            public int Value;

            public Stuff15(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value;

            public bool Equals(Stuff15 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff15);
            public override int GetHashCode() => Value;
        }

        private class Stuff16 : IEquatable<Stuff16>
        {
            public int Value;

            public Stuff16(A0 a0, A1 a1, A2 a2, A3 a3, A4 a4, A5 a5, A6 a6, A7 a7, A8 a8, A9 a9, A10 a10, A11 a11, A12 a12, A13 a13, A14 a14, A15 a15) => Value = 42 + a0.Value + a1.Value + a2.Value + a3.Value + a4.Value + a5.Value + a6.Value + a7.Value + a8.Value + a9.Value + a10.Value + a11.Value + a12.Value + a13.Value + a14.Value + a15.Value;

            public bool Equals(Stuff16 obj) => obj != null && obj.Value == Value;
            public override bool Equals(object obj) => Equals(obj as Stuff16);
            public override int GetHashCode() => Value;
        }


        private class A0
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 1;
            }
        }

        private class A1
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 2;
            }
        }

        private class A2
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 4;
            }
        }

        private class A3
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 8;
            }
        }

        private class A4
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 16;
            }
        }

        private class A5
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 32;
            }
        }

        private class A6
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 64;
            }
        }

        private class A7
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 128;
            }
        }

        private class A8
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 256;
            }
        }

        private class A9
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 512;
            }
        }

        private class A10
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 1024;
            }
        }

        private class A11
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 2048;
            }
        }

        private class A12
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 4096;
            }
        }

        private class A13
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 8192;
            }
        }

        private class A14
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 16384;
            }
        }

        private class A15
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 32768;
            }
        }

        private class A16
        {
            private int _value;

            public int Value
            {
                get => _value;
                set => _value = value * 65536;
            }
        }
    }
}
