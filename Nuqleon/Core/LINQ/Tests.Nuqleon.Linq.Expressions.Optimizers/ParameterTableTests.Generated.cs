// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    partial class ParameterTableTests
    {
        [TestMethod]
        public void ParameterTable_Add_Action1()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1) => Methods.Action1(arg1));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action1));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action2()
        {
            var pt = new ParameterTable();

            pt.Add((int arg2, int arg1) => Methods.Action2(arg1, arg2));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action2));

            Assert.IsTrue(pt.Contains(m.GetParameters()[1]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action3()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2) => Methods.Action3(arg1, arg2, arg3));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action3));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action4()
        {
            var pt = new ParameterTable();

            pt.Add((int arg4, int arg1, int arg2, int arg3) => Methods.Action4(arg1, arg2, arg3, arg4));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action4));

            Assert.IsTrue(pt.Contains(m.GetParameters()[3]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action5()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5) => Methods.Action5(arg1, arg2, arg3, arg4, arg5));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action5));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action6()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5, int arg6) => Methods.Action6(arg1, arg2, arg3, arg4, arg5, arg6));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action6));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action7()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5, int arg6, int arg7) => Methods.Action7(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action7));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action8()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5, int arg6, int arg7, int arg8) => Methods.Action8(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action8));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action9()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9) => Methods.Action9(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action9));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action10()
        {
            var pt = new ParameterTable();

            pt.Add((int arg6, int arg1, int arg2, int arg3, int arg4, int arg5, int arg7, int arg8, int arg9, int arg10) => Methods.Action10(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action10));

            Assert.IsTrue(pt.Contains(m.GetParameters()[5]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action11()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11) => Methods.Action11(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action11));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action12()
        {
            var pt = new ParameterTable();

            pt.Add((int arg8, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg9, int arg10, int arg11, int arg12) => Methods.Action12(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action12));

            Assert.IsTrue(pt.Contains(m.GetParameters()[7]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action13()
        {
            var pt = new ParameterTable();

            pt.Add((int arg12, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg13) => Methods.Action13(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action13));

            Assert.IsTrue(pt.Contains(m.GetParameters()[11]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action14()
        {
            var pt = new ParameterTable();

            pt.Add((int arg2, int arg1, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14) => Methods.Action14(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action14));

            Assert.IsTrue(pt.Contains(m.GetParameters()[1]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action15()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15) => Methods.Action15(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action15));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Action16()
        {
            var pt = new ParameterTable();

            pt.Add((int arg8, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15, int arg16) => Methods.Action16(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));

            var m = typeof(Methods).GetMethod(nameof(Methods.Action16));

            Assert.IsTrue(pt.Contains(m.GetParameters()[7]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func1()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1) => Methods.Func1(arg1));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func1));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func2()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1, int arg2) => Methods.Func2(arg1, arg2));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func2));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func3()
        {
            var pt = new ParameterTable();

            pt.Add((int arg2, int arg1, int arg3) => Methods.Func3(arg1, arg2, arg3));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func3));

            Assert.IsTrue(pt.Contains(m.GetParameters()[1]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func4()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1, int arg2, int arg3, int arg4) => Methods.Func4(arg1, arg2, arg3, arg4));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func4));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func5()
        {
            var pt = new ParameterTable();

            pt.Add((int arg2, int arg1, int arg3, int arg4, int arg5) => Methods.Func5(arg1, arg2, arg3, arg4, arg5));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func5));

            Assert.IsTrue(pt.Contains(m.GetParameters()[1]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func6()
        {
            var pt = new ParameterTable();

            pt.Add((int arg3, int arg1, int arg2, int arg4, int arg5, int arg6) => Methods.Func6(arg1, arg2, arg3, arg4, arg5, arg6));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func6));

            Assert.IsTrue(pt.Contains(m.GetParameters()[2]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func7()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7) => Methods.Func7(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func7));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func8()
        {
            var pt = new ParameterTable();

            pt.Add((int arg7, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg8) => Methods.Func8(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func8));

            Assert.IsTrue(pt.Contains(m.GetParameters()[6]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func9()
        {
            var pt = new ParameterTable();

            pt.Add((int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9) => Methods.Func9(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func9));

            Assert.IsTrue(pt.Contains(m.GetParameters()[0]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func10()
        {
            var pt = new ParameterTable();

            pt.Add((int arg10, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9) => Methods.Func10(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func10));

            Assert.IsTrue(pt.Contains(m.GetParameters()[9]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func11()
        {
            var pt = new ParameterTable();

            pt.Add((int arg10, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg11) => Methods.Func11(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func11));

            Assert.IsTrue(pt.Contains(m.GetParameters()[9]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func12()
        {
            var pt = new ParameterTable();

            pt.Add((int arg5, int arg1, int arg2, int arg3, int arg4, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12) => Methods.Func12(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func12));

            Assert.IsTrue(pt.Contains(m.GetParameters()[4]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func13()
        {
            var pt = new ParameterTable();

            pt.Add((int arg4, int arg1, int arg2, int arg3, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13) => Methods.Func13(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func13));

            Assert.IsTrue(pt.Contains(m.GetParameters()[3]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func14()
        {
            var pt = new ParameterTable();

            pt.Add((int arg2, int arg1, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14) => Methods.Func14(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func14));

            Assert.IsTrue(pt.Contains(m.GetParameters()[1]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func15()
        {
            var pt = new ParameterTable();

            pt.Add((int arg4, int arg1, int arg2, int arg3, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15) => Methods.Func15(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func15));

            Assert.IsTrue(pt.Contains(m.GetParameters()[3]));
        }

        [TestMethod]
        public void ParameterTable_Add_Func16()
        {
            var pt = new ParameterTable();

            pt.Add((int arg13, int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg14, int arg15, int arg16) => Methods.Func16(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));

            var m = typeof(Methods).GetMethod(nameof(Methods.Func16));

            Assert.IsTrue(pt.Contains(m.GetParameters()[12]));
        }


        private sealed class Methods
        {
            public static void Action1(int arg1) { }
            public static void Action2(int arg1, int arg2) { }
            public static void Action3(int arg1, int arg2, int arg3) { }
            public static void Action4(int arg1, int arg2, int arg3, int arg4) { }
            public static void Action5(int arg1, int arg2, int arg3, int arg4, int arg5) { }
            public static void Action6(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6) { }
            public static void Action7(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7) { }
            public static void Action8(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8) { }
            public static void Action9(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9) { }
            public static void Action10(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10) { }
            public static void Action11(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11) { }
            public static void Action12(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12) { }
            public static void Action13(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13) { }
            public static void Action14(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14) { }
            public static void Action15(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15) { }
            public static void Action16(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15, int arg16) { }
            public static int Func1(int arg1) => 42;
            public static int Func2(int arg1, int arg2) => 42;
            public static int Func3(int arg1, int arg2, int arg3) => 42;
            public static int Func4(int arg1, int arg2, int arg3, int arg4) => 42;
            public static int Func5(int arg1, int arg2, int arg3, int arg4, int arg5) => 42;
            public static int Func6(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6) => 42;
            public static int Func7(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7) => 42;
            public static int Func8(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8) => 42;
            public static int Func9(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9) => 42;
            public static int Func10(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10) => 42;
            public static int Func11(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11) => 42;
            public static int Func12(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12) => 42;
            public static int Func13(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13) => 42;
            public static int Func14(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14) => 42;
            public static int Func15(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15) => 42;
            public static int Func16(int arg1, int arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, int arg11, int arg12, int arg13, int arg14, int arg15, int arg16) => 42;
        }
    }
}
