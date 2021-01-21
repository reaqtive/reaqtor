// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using System;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices.Bonsai;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices
{
    [TestClass]
    public class SlimReflectionHelpersTests : TestBase
    {
        [TestMethod]
        public void SlimInfoOf_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimReflectionHelpers.InfoOf(default(Expression)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimReflectionHelpers.InfoOf(default(Expression<Action>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimReflectionHelpers.InfoOf(default(Expression<Action<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimReflectionHelpers.InfoOf(default(Expression<Func<int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => SlimReflectionHelpers.InfoOf(default(Expression<Func<int, int>>)), ex => Assert.AreEqual("expression", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void SlimInfoOf_NoReflectionInfo()
        {
            Assert.ThrowsException<NotSupportedException>(() => SlimReflectionHelpers.InfoOf(Expression.Constant(42)));
            Assert.ThrowsException<NotSupportedException>(() => SlimReflectionHelpers.InfoOf(Expression.Negate(Expression.Constant(42))));
            Assert.ThrowsException<NotSupportedException>(() => SlimReflectionHelpers.InfoOf(Expression.Add(Expression.Constant(1), Expression.Constant(2))));
        }

        [TestMethod]
        public void SlimInfoOf_MethodCall_MethodInfo1()
        {
            AssertAreSame(typeof(Console).GetMethod("WriteLine", Type.EmptyTypes), SlimReflectionHelpers.InfoOf(() => Console.WriteLine()));
        }

        [TestMethod]
        public void SlimInfoOf_MethodCall_MethodInfo2()
        {
            AssertAreSame(typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }), SlimReflectionHelpers.InfoOf((string s) => Console.WriteLine(s)));
        }

        [TestMethod]
        public void SlimInfoOf_Unary_Convert_MethodInfo()
        {
            var convert = typeof(DateTimeOffset).GetMethods().Single(m => m.Name == "op_Implicit" && m.GetParameters()[0].ParameterType == typeof(DateTime) && m.ReturnType == typeof(DateTimeOffset));
            AssertAreSame(convert, SlimReflectionHelpers.InfoOf<DateTime, DateTimeOffset>(dt => dt));
        }

        [TestMethod]
        public void SlimInfoOf_Unary_Negate_MethodInfo()
        {
            var negate = typeof(TimeSpan).GetMethods().Single(m => m.Name == "op_UnaryNegation" && m.GetParameters()[0].ParameterType == typeof(TimeSpan) && m.ReturnType == typeof(TimeSpan));
            AssertAreSame(negate, SlimReflectionHelpers.InfoOf<TimeSpan, TimeSpan>(t => -t));
        }

        [TestMethod]
        public void SlimInfoOf_Binary_Add_MethodInfo()
        {
            var add = typeof(DateTime).GetMethods().Single(m => m.Name == "op_Addition" && m.GetParameters()[0].ParameterType == typeof(DateTime) && m.GetParameters()[1].ParameterType == typeof(TimeSpan) && m.ReturnType == typeof(DateTime));
            AssertAreSame(add, SlimReflectionHelpers.InfoOf<DateTime, DateTime>(d => d + default(TimeSpan)));
        }

        [TestMethod]
        public void SlimInfoOf_Member_PropertyInfo()
        {
            AssertAreSame(typeof(Console).GetProperty("ForegroundColor"), SlimReflectionHelpers.InfoOf(() => Console.ForegroundColor));
        }

        [TestMethod]
        public void SlimInfoOf_New_ConstructorInfo()
        {
            AssertAreSame(typeof(Exception).GetConstructor(new[] { typeof(string) }), SlimReflectionHelpers.InfoOf((string s) => new Exception(s)));
        }

        private static void AssertAreSame(MemberInfo member, MemberInfoSlim slimMember)
        {
            switch (slimMember.MemberType)
            {
                case MemberTypes.Constructor:
                    AssertAreSameConstructor((ConstructorInfo)member, (ConstructorInfoSlim)slimMember);
                    return;
                case MemberTypes.Field:
                    AssertAreSameField((FieldInfo)member, (FieldInfoSlim)slimMember);
                    return;
                case MemberTypes.Method:
                    AssertAreSameMethod((MethodInfo)member, (MethodInfoSlim)slimMember);
                    return;
                case MemberTypes.Property:
                    AssertAreSameProperty((PropertyInfo)member, (PropertyInfoSlim)slimMember);
                    return;
            }

            Fail(member, slimMember);
        }

        private static void AssertAreSameConstructor(ConstructorInfo constructorInfo, ConstructorInfoSlim constructorInfoSlim)
        {
            Assert.IsTrue(TypeSlimExtensions.Equals(constructorInfo.DeclaringType.ToTypeSlim(), constructorInfoSlim.DeclaringType));
            Assert.IsTrue(constructorInfoSlim.ParameterTypes.SequenceEqual(constructorInfo.GetParameters().Select(p => p.ParameterType.ToTypeSlim())));
        }

        private static void AssertAreSameField(FieldInfo fieldInfo, FieldInfoSlim fieldInfoSlim)
        {
            Assert.AreEqual(fieldInfo.Name, fieldInfoSlim.Name);
            Assert.IsTrue(TypeSlimExtensions.Equals(fieldInfo.DeclaringType.ToTypeSlim(), fieldInfoSlim.DeclaringType));
            Assert.IsTrue(TypeSlimExtensions.Equals(fieldInfo.FieldType.ToTypeSlim(), fieldInfoSlim.FieldType));
        }

        private static void AssertAreSameMethod(MethodInfo methodInfo, MethodInfoSlim methodInfoSlim)
        {
            Assert.IsTrue(TypeSlimExtensions.Equals(methodInfo.ReturnType.ToTypeSlim(), methodInfoSlim.ReturnType));
            Assert.IsTrue(methodInfoSlim.ParameterTypes.SequenceEqual(methodInfo.GetParameters().Select(p => p.ParameterType.ToTypeSlim())));

            switch (methodInfoSlim.Kind)
            {
                case MethodInfoSlimKind.Simple:
                    AssertAreSameSimpleMethod(methodInfo, (SimpleMethodInfoSlim)methodInfoSlim);
                    return;
                case MethodInfoSlimKind.GenericDefinition:
                    Assert.IsTrue(methodInfo.IsGenericMethodDefinition);
                    AssertAreSameGenericDefinitionMethod(methodInfo, (GenericDefinitionMethodInfoSlim)methodInfoSlim);
                    return;
                case MethodInfoSlimKind.Generic:
                    AssertAreSameGenericMethod(methodInfo, (GenericMethodInfoSlim)methodInfoSlim);
                    return;
            }

            Fail(methodInfo, methodInfoSlim);
        }

        private static void AssertAreSameSimpleMethod(MethodInfo methodInfo, SimpleMethodInfoSlim simpleMethodInfoSlim)
        {
            Assert.AreEqual(methodInfo.Name, simpleMethodInfoSlim.Name);
        }

        private static void AssertAreSameGenericDefinitionMethod(MethodInfo methodInfo, GenericDefinitionMethodInfoSlim genericDefinitionMethodInfoSlim)
        {
            Assert.AreEqual(methodInfo.Name, genericDefinitionMethodInfoSlim.Name);
            Assert.AreEqual(methodInfo.GetGenericArguments().Length, genericDefinitionMethodInfoSlim.GenericParameterTypes.Count);
        }

        private static void AssertAreSameGenericMethod(MethodInfo methodInfo, GenericMethodInfoSlim genericMethodInfoSlim)
        {
            AssertAreSameMethod(methodInfo.GetGenericMethodDefinition(), genericMethodInfoSlim.GenericMethodDefinition);
            Assert.IsTrue(genericMethodInfoSlim.GenericArguments.SequenceEqual(methodInfo.GetGenericArguments().Select(t => t.ToTypeSlim())));
        }

        private static void AssertAreSameProperty(PropertyInfo propertyInfo, PropertyInfoSlim propertyInfoSlim)
        {
            Assert.AreEqual(propertyInfo.Name, propertyInfoSlim.Name);
            Assert.IsTrue(TypeSlimExtensions.Equals(propertyInfo.DeclaringType.ToTypeSlim(), propertyInfoSlim.DeclaringType));
            Assert.IsTrue(TypeSlimExtensions.Equals(propertyInfo.PropertyType.ToTypeSlim(), propertyInfoSlim.PropertyType));
        }

        private static void Fail(MemberInfo member, MemberInfoSlim slimMember)
        {
            Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Expected '{0}', Actual: '{1}'", member, slimMember));
        }
    }
}
