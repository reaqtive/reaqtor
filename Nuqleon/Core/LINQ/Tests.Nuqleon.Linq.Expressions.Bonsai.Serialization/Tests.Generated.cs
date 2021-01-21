// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Tests
{
    partial class Tests
    {
        [TestMethod]
        public void Bonsai_New0()
        {
            var e = (Expression<Func<NewTest0>>)(() => new NewTest0());

            var i = Roundtrip(e) as Expression<Func<NewTest0>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest0(), i.Compile()());

            var i08 = Roundtrip(e, V08) as Expression<Func<NewTest0>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest0(), i08.Compile()());
        }

        [TestMethod]
        public void Bonsai_New1()
        {
            var e = (Expression<Func<int, NewTest1>>)((arg0) => new NewTest1(arg0));

            var i = Roundtrip(e) as Expression<Func<int, NewTest1>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest1(1), i.Compile()(1));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, NewTest1>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest1(1), i08.Compile()(1));
        }

        [TestMethod]
        public void Bonsai_New2()
        {
            var e = (Expression<Func<int, int, NewTest2>>)((arg0, arg1) => new NewTest2(arg0, arg1));

            var i = Roundtrip(e) as Expression<Func<int, int, NewTest2>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest2(1, 2), i.Compile()(1, 2));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, NewTest2>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest2(1, 2), i08.Compile()(1, 2));
        }

        [TestMethod]
        public void Bonsai_New3()
        {
            var e = (Expression<Func<int, int, int, NewTest3>>)((arg0, arg1, arg2) => new NewTest3(arg0, arg1, arg2));

            var i = Roundtrip(e) as Expression<Func<int, int, int, NewTest3>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest3(1, 2, 3), i.Compile()(1, 2, 3));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, NewTest3>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest3(1, 2, 3), i08.Compile()(1, 2, 3));
        }

        [TestMethod]
        public void Bonsai_New4()
        {
            var e = (Expression<Func<int, int, int, int, NewTest4>>)((arg0, arg1, arg2, arg3) => new NewTest4(arg0, arg1, arg2, arg3));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, NewTest4>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest4(1, 2, 3, 4), i.Compile()(1, 2, 3, 4));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, NewTest4>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest4(1, 2, 3, 4), i08.Compile()(1, 2, 3, 4));
        }

        [TestMethod]
        public void Bonsai_New5()
        {
            var e = (Expression<Func<int, int, int, int, int, NewTest5>>)((arg0, arg1, arg2, arg3, arg4) => new NewTest5(arg0, arg1, arg2, arg3, arg4));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, NewTest5>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest5(1, 2, 3, 4, 5), i.Compile()(1, 2, 3, 4, 5));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, NewTest5>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest5(1, 2, 3, 4, 5), i08.Compile()(1, 2, 3, 4, 5));
        }

        [TestMethod]
        public void Bonsai_New6()
        {
            var e = (Expression<Func<int, int, int, int, int, int, NewTest6>>)((arg0, arg1, arg2, arg3, arg4, arg5) => new NewTest6(arg0, arg1, arg2, arg3, arg4, arg5));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, NewTest6>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest6(1, 2, 3, 4, 5, 6), i.Compile()(1, 2, 3, 4, 5, 6));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, NewTest6>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest6(1, 2, 3, 4, 5, 6), i08.Compile()(1, 2, 3, 4, 5, 6));
        }

        [TestMethod]
        public void Bonsai_New7()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int, NewTest7>>)((arg0, arg1, arg2, arg3, arg4, arg5, arg6) => new NewTest7(arg0, arg1, arg2, arg3, arg4, arg5, arg6));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int, NewTest7>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest7(1, 2, 3, 4, 5, 6, 7), i.Compile()(1, 2, 3, 4, 5, 6, 7));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int, NewTest7>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest7(1, 2, 3, 4, 5, 6, 7), i08.Compile()(1, 2, 3, 4, 5, 6, 7));
        }

        [TestMethod]
        public void Bonsai_New8()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int, int, NewTest8>>)((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new NewTest8(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int, int, NewTest8>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest8(1, 2, 3, 4, 5, 6, 7, 8), i.Compile()(1, 2, 3, 4, 5, 6, 7, 8));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int, int, NewTest8>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest8(1, 2, 3, 4, 5, 6, 7, 8), i08.Compile()(1, 2, 3, 4, 5, 6, 7, 8));
        }

        [TestMethod]
        public void Bonsai_New9()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int, int, int, NewTest9>>)((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new NewTest9(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int, int, int, NewTest9>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is NewExpression);

            Assert.AreEqual(new NewTest9(1, 2, 3, 4, 5, 6, 7, 8, 9), i.Compile()(1, 2, 3, 4, 5, 6, 7, 8, 9));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int, int, int, NewTest9>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is NewExpression);

            Assert.AreEqual(new NewTest9(1, 2, 3, 4, 5, 6, 7, 8, 9), i08.Compile()(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [TestMethod]
        public void Bonsai_Invocation0()
        {
            var e = (Expression<Func<Func<int>, int>>)((f) => f());

            var i = Roundtrip(e) as Expression<Func<Func<int>, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42, i.Compile()(() => 42));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int>, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42, i08.Compile()(() => 42));
        }

        [TestMethod]
        public void Bonsai_Invocation1()
        {
            var e = (Expression<Func<Func<int, int>, int, int>>)((f, arg0) => f(arg0));

            var i = Roundtrip(e) as Expression<Func<Func<int, int>, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1, i.Compile()((arg0) => 42 + arg0, 1));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int>, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1, i08.Compile()((arg0) => 42 + arg0, 1));
        }

        [TestMethod]
        public void Bonsai_Invocation2()
        {
            var e = (Expression<Func<Func<int, int, int>, int, int, int>>)((f, arg0, arg1) => f(arg0, arg1));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int>, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2, i.Compile()((arg0, arg1) => 42 + arg0 + arg1, 1, 2));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int>, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2, i08.Compile()((arg0, arg1) => 42 + arg0 + arg1, 1, 2));
        }

        [TestMethod]
        public void Bonsai_Invocation3()
        {
            var e = (Expression<Func<Func<int, int, int, int>, int, int, int, int>>)((f, arg0, arg1, arg2) => f(arg0, arg1, arg2));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int>, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3, i.Compile()((arg0, arg1, arg2) => 42 + arg0 + arg1 + arg2, 1, 2, 3));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int>, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3, i08.Compile()((arg0, arg1, arg2) => 42 + arg0 + arg1 + arg2, 1, 2, 3));
        }

        [TestMethod]
        public void Bonsai_Invocation4()
        {
            var e = (Expression<Func<Func<int, int, int, int, int>, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3) => f(arg0, arg1, arg2, arg3));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int>, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4, i.Compile()((arg0, arg1, arg2, arg3) => 42 + arg0 + arg1 + arg2 + arg3, 1, 2, 3, 4));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int>, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4, i08.Compile()((arg0, arg1, arg2, arg3) => 42 + arg0 + arg1 + arg2 + arg3, 1, 2, 3, 4));
        }

        [TestMethod]
        public void Bonsai_Invocation5()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int>, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4) => f(arg0, arg1, arg2, arg3, arg4));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int>, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5, i.Compile()((arg0, arg1, arg2, arg3, arg4) => 42 + arg0 + arg1 + arg2 + arg3 + arg4, 1, 2, 3, 4, 5));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int>, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5, i08.Compile()((arg0, arg1, arg2, arg3, arg4) => 42 + arg0 + arg1 + arg2 + arg3 + arg4, 1, 2, 3, 4, 5));
        }

        [TestMethod]
        public void Bonsai_Invocation6()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int>, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5) => f(arg0, arg1, arg2, arg3, arg4, arg5));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int>, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5, 1, 2, 3, 4, 5, 6));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int>, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5, 1, 2, 3, 4, 5, 6));
        }

        [TestMethod]
        public void Bonsai_Invocation7()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6, 1, 2, 3, 4, 5, 6, 7));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6, 1, 2, 3, 4, 5, 6, 7));
        }

        [TestMethod]
        public void Bonsai_Invocation8()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7, 1, 2, 3, 4, 5, 6, 7, 8));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7, 1, 2, 3, 4, 5, 6, 7, 8));
        }

        [TestMethod]
        public void Bonsai_Invocation9()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8, 1, 2, 3, 4, 5, 6, 7, 8, 9));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8, 1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [TestMethod]
        public void Bonsai_Invocation10()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        }

        [TestMethod]
        public void Bonsai_Invocation11()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
        }

        [TestMethod]
        public void Bonsai_Invocation12()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
        }

        [TestMethod]
        public void Bonsai_Invocation13()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11 + arg12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11 + arg12, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
        }

        [TestMethod]
        public void Bonsai_Invocation14()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11 + arg12 + arg13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11 + arg12 + arg13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
        }

        [TestMethod]
        public void Bonsai_Invocation15()
        {
            var e = (Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>)((f, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => f(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

            var i = Roundtrip(e) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15, i.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11 + arg12 + arg13 + arg14, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

            var i08 = Roundtrip(e, V08) as Expression<Func<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is InvocationExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15, i08.Compile()((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10 + arg11 + arg12 + arg13 + arg14, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
        }

        [TestMethod]
        public void Bonsai_StaticCall0()
        {
            var e = (Expression<Func<int>>)(() => CallTests.S0());

            var i = Roundtrip(e) as Expression<Func<int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42, i.Compile()());

            var i08 = Roundtrip(e, V08) as Expression<Func<int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42, i08.Compile()());
        }

        [TestMethod]
        public void Bonsai_StaticCall1()
        {
            var e = (Expression<Func<int, int>>)((arg0) => CallTests.S1(arg0));

            var i = Roundtrip(e) as Expression<Func<int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1, i.Compile()(1));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1, i08.Compile()(1));
        }

        [TestMethod]
        public void Bonsai_StaticCall2()
        {
            var e = (Expression<Func<int, int, int>>)((arg0, arg1) => CallTests.S2(arg0, arg1));

            var i = Roundtrip(e) as Expression<Func<int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2, i.Compile()(1, 2));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2, i08.Compile()(1, 2));
        }

        [TestMethod]
        public void Bonsai_StaticCall3()
        {
            var e = (Expression<Func<int, int, int, int>>)((arg0, arg1, arg2) => CallTests.S3(arg0, arg1, arg2));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3, i.Compile()(1, 2, 3));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3, i08.Compile()(1, 2, 3));
        }

        [TestMethod]
        public void Bonsai_StaticCall4()
        {
            var e = (Expression<Func<int, int, int, int, int>>)((arg0, arg1, arg2, arg3) => CallTests.S4(arg0, arg1, arg2, arg3));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4, i.Compile()(1, 2, 3, 4));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4, i08.Compile()(1, 2, 3, 4));
        }

        [TestMethod]
        public void Bonsai_StaticCall5()
        {
            var e = (Expression<Func<int, int, int, int, int, int>>)((arg0, arg1, arg2, arg3, arg4) => CallTests.S5(arg0, arg1, arg2, arg3, arg4));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5, i.Compile()(1, 2, 3, 4, 5));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5, i08.Compile()(1, 2, 3, 4, 5));
        }

        [TestMethod]
        public void Bonsai_StaticCall6()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int>>)((arg0, arg1, arg2, arg3, arg4, arg5) => CallTests.S6(arg0, arg1, arg2, arg3, arg4, arg5));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6, i.Compile()(1, 2, 3, 4, 5, 6));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6, i08.Compile()(1, 2, 3, 4, 5, 6));
        }

        [TestMethod]
        public void Bonsai_StaticCall7()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int, int>>)((arg0, arg1, arg2, arg3, arg4, arg5, arg6) => CallTests.S7(arg0, arg1, arg2, arg3, arg4, arg5, arg6));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7, i.Compile()(1, 2, 3, 4, 5, 6, 7));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7, i08.Compile()(1, 2, 3, 4, 5, 6, 7));
        }

        [TestMethod]
        public void Bonsai_StaticCall8()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int, int, int>>)((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => CallTests.S8(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8, i.Compile()(1, 2, 3, 4, 5, 6, 7, 8));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8, i08.Compile()(1, 2, 3, 4, 5, 6, 7, 8));
        }

        [TestMethod]
        public void Bonsai_StaticCall9()
        {
            var e = (Expression<Func<int, int, int, int, int, int, int, int, int, int>>)((arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => CallTests.S9(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

            var i = Roundtrip(e) as Expression<Func<int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9, i.Compile()(1, 2, 3, 4, 5, 6, 7, 8, 9));

            var i08 = Roundtrip(e, V08) as Expression<Func<int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9, i08.Compile()(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [TestMethod]
        public void Bonsai_InstanceCall0()
        {
            var e = (Expression<Func<CallTests, int>>)((c) => c.I0());

            var i = Roundtrip(e) as Expression<Func<CallTests, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42, i.Compile()(new CallTests(42)));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42, i08.Compile()(new CallTests(42)));
        }

        [TestMethod]
        public void Bonsai_InstanceCall1()
        {
            var e = (Expression<Func<CallTests, int, int>>)((c, arg0) => c.I1(arg0));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1, i.Compile()(new CallTests(42), 1));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1, i08.Compile()(new CallTests(42), 1));
        }

        [TestMethod]
        public void Bonsai_InstanceCall2()
        {
            var e = (Expression<Func<CallTests, int, int, int>>)((c, arg0, arg1) => c.I2(arg0, arg1));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2, i.Compile()(new CallTests(42), 1, 2));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2, i08.Compile()(new CallTests(42), 1, 2));
        }

        [TestMethod]
        public void Bonsai_InstanceCall3()
        {
            var e = (Expression<Func<CallTests, int, int, int, int>>)((c, arg0, arg1, arg2) => c.I3(arg0, arg1, arg2));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3, i.Compile()(new CallTests(42), 1, 2, 3));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3, i08.Compile()(new CallTests(42), 1, 2, 3));
        }

        [TestMethod]
        public void Bonsai_InstanceCall4()
        {
            var e = (Expression<Func<CallTests, int, int, int, int, int>>)((c, arg0, arg1, arg2, arg3) => c.I4(arg0, arg1, arg2, arg3));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4, i.Compile()(new CallTests(42), 1, 2, 3, 4));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4, i08.Compile()(new CallTests(42), 1, 2, 3, 4));
        }

        [TestMethod]
        public void Bonsai_InstanceCall5()
        {
            var e = (Expression<Func<CallTests, int, int, int, int, int, int>>)((c, arg0, arg1, arg2, arg3, arg4) => c.I5(arg0, arg1, arg2, arg3, arg4));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5, i.Compile()(new CallTests(42), 1, 2, 3, 4, 5));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5, i08.Compile()(new CallTests(42), 1, 2, 3, 4, 5));
        }

        [TestMethod]
        public void Bonsai_InstanceCall6()
        {
            var e = (Expression<Func<CallTests, int, int, int, int, int, int, int>>)((c, arg0, arg1, arg2, arg3, arg4, arg5) => c.I6(arg0, arg1, arg2, arg3, arg4, arg5));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6, i.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6, i08.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6));
        }

        [TestMethod]
        public void Bonsai_InstanceCall7()
        {
            var e = (Expression<Func<CallTests, int, int, int, int, int, int, int, int>>)((c, arg0, arg1, arg2, arg3, arg4, arg5, arg6) => c.I7(arg0, arg1, arg2, arg3, arg4, arg5, arg6));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7, i.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6, 7));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7, i08.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6, 7));
        }

        [TestMethod]
        public void Bonsai_InstanceCall8()
        {
            var e = (Expression<Func<CallTests, int, int, int, int, int, int, int, int, int>>)((c, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => c.I8(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8, i.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6, 7, 8));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8, i08.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6, 7, 8));
        }

        [TestMethod]
        public void Bonsai_InstanceCall9()
        {
            var e = (Expression<Func<CallTests, int, int, int, int, int, int, int, int, int, int>>)((c, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => c.I9(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

            var i = Roundtrip(e) as Expression<Func<CallTests, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i);
            Assert.IsTrue(i.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9, i.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6, 7, 8, 9));

            var i08 = Roundtrip(e, V08) as Expression<Func<CallTests, int, int, int, int, int, int, int, int, int, int>>;
            Assert.IsNotNull(i08);
            Assert.IsTrue(i08.Body is MethodCallExpression);

            Assert.AreEqual(42 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9, i08.Compile()(new CallTests(42), 1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

    }

    public class CallTests
    {
        private readonly int _seed;

        public CallTests(int seed)
        {
            _seed = seed;
        }

        public static int S0() => 42;
        public static int S1(int arg0) => 42 + arg0;
        public static int S2(int arg0, int arg1) => 42 + arg0 + arg1;
        public static int S3(int arg0, int arg1, int arg2) => 42 + arg0 + arg1 + arg2;
        public static int S4(int arg0, int arg1, int arg2, int arg3) => 42 + arg0 + arg1 + arg2 + arg3;
        public static int S5(int arg0, int arg1, int arg2, int arg3, int arg4) => 42 + arg0 + arg1 + arg2 + arg3 + arg4;
        public static int S6(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5;
        public static int S7(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6;
        public static int S8(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7;
        public static int S9(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8) => 42 + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8;

        public int I0() => _seed;
        public int I1(int arg0) => _seed + arg0;
        public int I2(int arg0, int arg1) => _seed + arg0 + arg1;
        public int I3(int arg0, int arg1, int arg2) => _seed + arg0 + arg1 + arg2;
        public int I4(int arg0, int arg1, int arg2, int arg3) => _seed + arg0 + arg1 + arg2 + arg3;
        public int I5(int arg0, int arg1, int arg2, int arg3, int arg4) => _seed + arg0 + arg1 + arg2 + arg3 + arg4;
        public int I6(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5) => _seed + arg0 + arg1 + arg2 + arg3 + arg4 + arg5;
        public int I7(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6) => _seed + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6;
        public int I8(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7) => _seed + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7;
        public int I9(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8) => _seed + arg0 + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8;
    }

    public class NewTest0 : IEquatable<NewTest0>
    {

        public NewTest0()
        {
        }

        public bool Equals(NewTest0 other)
        {
            return other != null
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest0);
        public override int GetHashCode() => 0;
    }

    public class NewTest1 : IEquatable<NewTest1>
    {
        private readonly int _arg0;

        public NewTest1(int arg0)
        {
            _arg0 = arg0;
        }

        public bool Equals(NewTest1 other)
        {
            return other != null
                && _arg0 == other._arg0
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest1);
        public override int GetHashCode() => 0;
    }

    public class NewTest2 : IEquatable<NewTest2>
    {
        private readonly int _arg0;
        private readonly int _arg1;

        public NewTest2(int arg0, int arg1)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }

        public bool Equals(NewTest2 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest2);
        public override int GetHashCode() => 0;
    }

    public class NewTest3 : IEquatable<NewTest3>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;

        public NewTest3(int arg0, int arg1, int arg2)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public bool Equals(NewTest3 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest3);
        public override int GetHashCode() => 0;
    }

    public class NewTest4 : IEquatable<NewTest4>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;
        private readonly int _arg3;

        public NewTest4(int arg0, int arg1, int arg2, int arg3)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }

        public bool Equals(NewTest4 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                && _arg3 == other._arg3
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest4);
        public override int GetHashCode() => 0;
    }

    public class NewTest5 : IEquatable<NewTest5>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;
        private readonly int _arg3;
        private readonly int _arg4;

        public NewTest5(int arg0, int arg1, int arg2, int arg3, int arg4)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }

        public bool Equals(NewTest5 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                && _arg3 == other._arg3
                && _arg4 == other._arg4
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest5);
        public override int GetHashCode() => 0;
    }

    public class NewTest6 : IEquatable<NewTest6>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;
        private readonly int _arg3;
        private readonly int _arg4;
        private readonly int _arg5;

        public NewTest6(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
            _arg5 = arg5;
        }

        public bool Equals(NewTest6 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                && _arg3 == other._arg3
                && _arg4 == other._arg4
                && _arg5 == other._arg5
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest6);
        public override int GetHashCode() => 0;
    }

    public class NewTest7 : IEquatable<NewTest7>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;
        private readonly int _arg3;
        private readonly int _arg4;
        private readonly int _arg5;
        private readonly int _arg6;

        public NewTest7(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
            _arg5 = arg5;
            _arg6 = arg6;
        }

        public bool Equals(NewTest7 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                && _arg3 == other._arg3
                && _arg4 == other._arg4
                && _arg5 == other._arg5
                && _arg6 == other._arg6
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest7);
        public override int GetHashCode() => 0;
    }

    public class NewTest8 : IEquatable<NewTest8>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;
        private readonly int _arg3;
        private readonly int _arg4;
        private readonly int _arg5;
        private readonly int _arg6;
        private readonly int _arg7;

        public NewTest8(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
            _arg5 = arg5;
            _arg6 = arg6;
            _arg7 = arg7;
        }

        public bool Equals(NewTest8 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                && _arg3 == other._arg3
                && _arg4 == other._arg4
                && _arg5 == other._arg5
                && _arg6 == other._arg6
                && _arg7 == other._arg7
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest8);
        public override int GetHashCode() => 0;
    }

    public class NewTest9 : IEquatable<NewTest9>
    {
        private readonly int _arg0;
        private readonly int _arg1;
        private readonly int _arg2;
        private readonly int _arg3;
        private readonly int _arg4;
        private readonly int _arg5;
        private readonly int _arg6;
        private readonly int _arg7;
        private readonly int _arg8;

        public NewTest9(int arg0, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
            _arg5 = arg5;
            _arg6 = arg6;
            _arg7 = arg7;
            _arg8 = arg8;
        }

        public bool Equals(NewTest9 other)
        {
            return other != null
                && _arg0 == other._arg0
                && _arg1 == other._arg1
                && _arg2 == other._arg2
                && _arg3 == other._arg3
                && _arg4 == other._arg4
                && _arg5 == other._arg5
                && _arg6 == other._arg6
                && _arg7 == other._arg7
                && _arg8 == other._arg8
                ;
        }

        public override bool Equals(object other) => Equals(other as NewTest9);
        public override int GetHashCode() => 0;
    }

}
