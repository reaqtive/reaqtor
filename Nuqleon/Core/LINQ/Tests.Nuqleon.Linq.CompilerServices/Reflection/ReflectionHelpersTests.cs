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
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ReflectionHelpersTests
    {
        [TestMethod]
        public void InfoOf_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => ReflectionHelpers.InfoOf(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ReflectionHelpers.InfoOf(default(Expression<Action>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ReflectionHelpers.InfoOf(default(Expression<Action<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ReflectionHelpers.InfoOf(default(Expression<Func<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ReflectionHelpers.InfoOf(default(Expression<Func<int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void InfoOf_NoReflectionInfo()
        {
            Assert.ThrowsException<NotSupportedException>(() => ReflectionHelpers.InfoOf(Expression.Constant(42)));
            Assert.ThrowsException<NotSupportedException>(() => ReflectionHelpers.InfoOf(Expression.Negate(Expression.Constant(42))));
            Assert.ThrowsException<NotSupportedException>(() => ReflectionHelpers.InfoOf(Expression.Add(Expression.Constant(1), Expression.Constant(2))));
        }

        [TestMethod]
        public void InfoOf_MethodCall_MethodInfo1()
        {
            Assert.AreSame(typeof(Console).GetMethod("WriteLine", Type.EmptyTypes), ReflectionHelpers.InfoOf(() => Console.WriteLine()));
        }

        [TestMethod]
        public void InfoOf_MethodCall_MethodInfo2()
        {
            Assert.AreSame(typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }), ReflectionHelpers.InfoOf((string s) => Console.WriteLine(s)));
        }

        [TestMethod]
        public void InfoOf_Unary_Convert_MethodInfo()
        {
            var convert = typeof(DateTimeOffset).GetMethods().Single(m => m.Name == "op_Implicit" && m.GetParameters()[0].ParameterType == typeof(DateTime) && m.ReturnType == typeof(DateTimeOffset));
            Assert.AreSame(convert, ReflectionHelpers.InfoOf<DateTime, DateTimeOffset>(dt => dt));
        }

        [TestMethod]
        public void InfoOf_Unary_Negate_MethodInfo()
        {
            var negate = typeof(TimeSpan).GetMethods().Single(m => m.Name == "op_UnaryNegation" && m.GetParameters()[0].ParameterType == typeof(TimeSpan) && m.ReturnType == typeof(TimeSpan));
            Assert.AreSame(negate, ReflectionHelpers.InfoOf<TimeSpan, TimeSpan>(t => -t));
        }

        [TestMethod]
        public void InfoOf_Binary_Add_MethodInfo()
        {
            var add = typeof(DateTime).GetMethods().Single(m => m.Name == "op_Addition" && m.GetParameters()[0].ParameterType == typeof(DateTime) && m.GetParameters()[1].ParameterType == typeof(TimeSpan) && m.ReturnType == typeof(DateTime));
            Assert.AreSame(add, ReflectionHelpers.InfoOf<DateTime, DateTime>(d => d + default(TimeSpan)));
        }

        [TestMethod]
        public void InfoOf_Member_PropertyInfo()
        {
            Assert.AreSame(typeof(Console).GetProperty("ForegroundColor"), ReflectionHelpers.InfoOf(() => Console.ForegroundColor));
        }

        [TestMethod]
        public void InfoOf_New_ConstructorInfo()
        {
            Assert.AreSame(typeof(Exception).GetConstructor(new[] { typeof(string) }), ReflectionHelpers.InfoOf((string s) => new Exception(s)));
        }
    }
}
