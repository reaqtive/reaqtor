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
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public partial class MemberTableTests
    {
        [TestMethod]
        public void MemberTable_Add_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(LambdaExpression)));
            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(MemberTable)));
            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(MemberInfo)));
        }

        [TestMethod]
        public void MemberTable_Add_Expression_Unary()
        {
            var mt = new MemberTable();

            var method = typeof(decimal).GetMethod("op_UnaryNegation", new[] { typeof(decimal) });

            mt.Add((decimal a) => -a);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_Binary()
        {
            var mt = new MemberTable();

            var method = typeof(decimal).GetMethod("op_Addition", new[] { typeof(decimal), typeof(decimal) });

            mt.Add((decimal a, decimal b) => a + b);

            AssertHasMember(mt, method);
        }

        [TestMethod]
        public void MemberTable_Add_FieldInfo_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(FieldInfo)));
        }

        [TestMethod]
        public void MemberTable_Add_FieldInfo_Static()
        {
            var mt = new MemberTable();

            var field = typeof(Stuff).GetField(nameof(Stuff.StaticField));

            mt.Add((MemberInfo)field);

            AssertHasMember(mt, field);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_FieldInfo_Static()
        {
            var mt = new MemberTable();

            var field = typeof(Stuff).GetField(nameof(Stuff.StaticField));

            mt.Add(() => Stuff.StaticField);

            AssertHasMember(mt, field);
        }

        [TestMethod]
        public void MemberTable_Evaluator_FieldInfo_Static()
        {
            var mt = new MemberTable();

            var field = typeof(Stuff).GetField(nameof(Stuff.StaticField));

            mt.Add(field);

            var eval = GetEvaluator(mt, field);

            var expected = field.GetValue(obj: null);
            var actual = eval.DynamicInvoke();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_FieldInfo_Instance()
        {
            var mt = new MemberTable();

            var field = typeof(Stuff).GetField(nameof(Stuff.InstanceField));

            mt.Add(field);

            AssertHasMember(mt, field);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_FieldInfo_Instance()
        {
            var mt = new MemberTable();

            var field = typeof(Stuff).GetField(nameof(Stuff.InstanceField));

            mt.Add((Stuff s) => s.InstanceField);

            AssertHasMember(mt, field);
        }

        [TestMethod]
        public void MemberTable_Evaluator_FieldInfo_Instance()
        {
            var mt = new MemberTable();

            var field = typeof(Stuff).GetField(nameof(Stuff.InstanceField));

            mt.Add(field);

            var eval = GetEvaluator(mt, field);

            var stuff = new Stuff { InstanceField = 42 };

            var expected = field.GetValue(stuff);
            var actual = eval.DynamicInvoke(stuff);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(PropertyInfo)));

            var property = typeof(Stuff).GetProperty(nameof(Stuff.InstancePropertySetOnly));

            Assert.ThrowsException<ArgumentException>(() => mt.Add(property));
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Static()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty(nameof(Stuff.StaticProperty));

            mt.Add((MemberInfo)property);

            AssertHasMember(mt, property);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Static()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty(nameof(Stuff.StaticProperty));

            mt.Add(() => Stuff.StaticProperty);

            AssertHasMember(mt, property);
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Static()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty(nameof(Stuff.StaticProperty));

            mt.Add(property);

            var eval = GetEvaluator(mt, property);

            var expected = property.GetValue(obj: null);
            var actual = eval.DynamicInvoke();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_PropertyInfo_Instance()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty(nameof(Stuff.InstanceProperty));

            mt.Add(property);

            AssertHasMember(mt, property);
        }

        [TestMethod]
        public void MemberTable_Add_Expression_PropertyInfo_Instance()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty(nameof(Stuff.InstanceProperty));

            mt.Add((Stuff s) => s.InstanceProperty);

            AssertHasMember(mt, property);
        }

        [TestMethod]
        public void MemberTable_Evaluator_PropertyInfo_Instance()
        {
            var mt = new MemberTable();

            var property = typeof(Stuff).GetProperty(nameof(Stuff.InstanceProperty));

            mt.Add(property);

            var eval = GetEvaluator(mt, property);

            var stuff = new Stuff { InstanceProperty = 42 };

            var expected = property.GetValue(stuff);
            var actual = eval.DynamicInvoke(stuff);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_MethodInfo_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(MethodInfo)));
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Instance()
        {
            var mt = new MemberTable();

            var method = typeof(string).GetMethod(nameof(string.ToUpper), Type.EmptyTypes);

            mt.Add((MemberInfo)method);

            var eval = GetEvaluator(mt, method);

            var expected = "BAR";
            var actual = eval.DynamicInvoke("bar");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Evaluator_MethodInfo_Static()
        {
            var mt = new MemberTable();

            var method = typeof(int).GetMethod(nameof(int.Parse), new[] { typeof(string) });

            mt.Add(method);

            var eval = GetEvaluator(mt, method);

            var expected = 42;
            var actual = eval.DynamicInvoke("42");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_ConstructorInfo_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentNullException>(() => mt.Add(default(ConstructorInfo)));

            var ctor = typeof(StaticCtor).GetConstructor(BindingFlags.Static | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

            Assert.ThrowsException<ArgumentException>(() => mt.Add(ctor));
        }

        [TestMethod]
        public void MemberTable_Evaluator_ConstructorInfo_Instance()
        {
            var mt = new MemberTable();

            var ctor = typeof(string).GetConstructor(new[] { typeof(char), typeof(int) });

            mt.Add((MemberInfo)ctor);

            var eval = GetEvaluator(mt, ctor);

            var expected = "***";
            var actual = eval.DynamicInvoke('*', 3);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MemberTable_Add_EventInfo_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentException>(() => mt.Add(typeof(Console).GetEvent(nameof(Console.CancelKeyPress))));
        }

        [TestMethod]
        public void MemberTable_Contains_ArgumentChecking()
        {
            var mt = new MemberTable();

            Assert.ThrowsException<ArgumentNullException>(() => mt.Contains(default));
        }

        [TestMethod]
        public void MemberTable_ReadOnly()
        {
            var mt = new MemberTable();
            var rmt = mt.ToReadOnly();

            var field = typeof(Stuff).GetField(nameof(Stuff.StaticField));
            var property = typeof(Stuff).GetProperty(nameof(Stuff.InstancePropertySetOnly));
            var ctor = typeof(string).GetConstructor(new[] { typeof(char), typeof(int) });

            Assert.ThrowsException<InvalidOperationException>(() => rmt.Add((MemberInfo)field));
            Assert.ThrowsException<InvalidOperationException>(() => rmt.Add(field));
            Assert.ThrowsException<InvalidOperationException>(() => rmt.Add(property));
            Assert.ThrowsException<InvalidOperationException>(() => rmt.Add(ctor));
        }

        private static void AssertHasMember(MemberTable mt, MemberInfo member)
        {
            Assert.IsTrue(mt.Contains(member));
        }

        private static Delegate GetEvaluator(MemberTable mt, MemberInfo member)
        {
            Assert.IsTrue(mt.Contains(member));

            var res = GetEvaluatorFactory().GetEvaluator(member);
            return res;
        }

        private static IEvaluatorFactory GetEvaluatorFactory() => new DefaultEvaluatorFactory();

#pragma warning disable CA1822 // Mark static
        private partial class Stuff
        {
            public static readonly object StaticWriteLock = new();

            public static int StaticField = 42;
            public int InstanceField;

            public static int StaticProperty { get; } = 42;
            public int InstanceProperty { get; set; }
            public int InstancePropertySetOnly { set { } }

            public int this[int x] => 42 * x;
        }
#pragma warning restore CA1822

        private class StaticCtor
        {
            static StaticCtor()
            {
            }
        }
    }
}
