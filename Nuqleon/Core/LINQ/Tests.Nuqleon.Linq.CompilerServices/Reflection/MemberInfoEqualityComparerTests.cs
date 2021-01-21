// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - Semptember 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Linq;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class MemberInfoEqualityComparerTests
    {
        #region Mismatched Inputs

        [TestMethod]
        public void MemberInfoComparer_Equals_Nulls()
        {
            AssertEqual(null, null);
            AssertNotEqual(null, typeof(Foo).GetConstructor(new Type[] { typeof(int), typeof(int) }));
            AssertNotEqual(typeof(Foo).GetConstructor(new Type[] { typeof(int), typeof(int) }), null);
        }

        [TestMethod]
        public void MemberInfoComparer_Equals_DifferentMemberTypes()
        {
            AssertNotEqual(typeof(Foo).GetConstructor(new Type[] { typeof(int), typeof(int) }), typeof(Foo).GetField("Field1"));
        }

        #endregion

        #region Normal Behavior

        [TestMethod]
        public void MemberInfoComparer_EqualsConstructor_Simple()
        {
            var ctor1 = typeof(Foo).GetConstructor(new Type[] { typeof(int), typeof(int) });
            var ctor1copy = typeof(Foo).GetConstructor(new Type[] { typeof(int), typeof(int) });
            var ctor2 = typeof(Foo).GetConstructor(new Type[] { typeof(int) });

            AssertEqual(ctor1, ctor1copy);
            AssertNotEqual(ctor1, ctor2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsEvent_Simple()
        {
            var event1 = typeof(Foo).GetEvent("Changed");
            var event1copy = typeof(Foo).GetEvent("Changed");
            var event2 = typeof(Foo).GetEvent("Clicked");

            AssertEqual(event1, event1copy);
            AssertNotEqual(event1, event2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsField_Simple()
        {
            var field1 = typeof(Foo).GetField("Field1");
            var field1copy = typeof(Foo).GetField("Field1");
            var field2 = typeof(Foo).GetField("Field2");

            AssertEqual(field1, field1copy);
            AssertNotEqual(field1, field2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsMethod_Simple()
        {
            var method1 = typeof(Foo).GetProperty("Property1").GetGetMethod();
            var method1copy = typeof(Foo).GetProperty("Property1").GetGetMethod();
            var method2 = typeof(Foo).GetProperty("Property2").GetGetMethod();

            AssertEqual(method1, method1copy);
            AssertNotEqual(method1, method2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsNestedType_Simple()
        {
            var nested1 = typeof(Foo).GetNestedType("Bar");
            var nested1copy = typeof(Foo).GetNestedType("Bar");
            var nested2 = typeof(Foo).GetNestedType("Qux");

            AssertEqual(nested1, nested1copy);
            AssertNotEqual(nested1, nested2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsProperty_Simple()
        {
            var property1 = typeof(Foo).GetProperty("Property1");
            var property1copy = typeof(Foo).GetProperty("Property1");
            var property2 = typeof(Foo).GetProperty("Property2");

            AssertEqual(property1, property1copy);
            AssertNotEqual(property1, property2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsType_Simple()
        {
            var type1 = typeof(int).GetTypeInfo();
            var type1copy = typeof(int).GetTypeInfo();
            var type2 = typeof(bool).GetTypeInfo();

            AssertEqual(type1, type1copy);
            AssertNotEqual(type1, type2);
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsCustom_Subclass()
        {
            var eq = new MyCustomMemberComparer();

            var m = new MyCustomMemberInfo();

            Assert.IsTrue(eq.Equals(m, m));
            Assert.AreEqual(42, eq.GetHashCode(m));
        }

        [TestMethod]
        public void MemberInfoComparer_EqualsExtension_Subclass()
        {
            var eq = new MyMemberComparer();

            var m = new MyMemberInfo();

            Assert.IsTrue(eq.Equals(m, m));
            Assert.AreEqual(42, eq.GetHashCode(m));
        }

        [TestMethod]
        public void MemberInfoComparer_ResolveMembers_Succeed()
        {
            var eq = new MyMemberComparer();

            var members = new[]
            {
                typeof(Foo).GetConstructor(new[] { typeof(int) }),
                typeof(Foo).GetEvent("Changed"),
                ReflectionHelpers.InfoOf((Foo foo) => foo.Field1),
                ReflectionHelpers.InfoOf((Foo foo) => foo.Property1),
                ReflectionHelpers.InfoOf((Foo foo) => foo.Method1()),
                typeof(Foo).GetNestedType("Bar"),
            };

            foreach (var member in members)
            {
                Assert.IsNotNull(eq.ResolveMember(typeof(Foo), member));
            }
        }

        #endregion

        #region Exception Behavior

        [TestMethod]
        public void MemberInfoComparer_ArgumentChecking()
        {
            var eq = new MemberInfoEqualityComparer();

            AssertEx.ThrowsException<ArgumentNullException>(() => eq.ResolveMember(targetType: null, typeof(object).GetMethods().First()), ex => Assert.AreEqual("targetType", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => eq.ResolveMember(typeof(object), member: null), ex => Assert.AreEqual("member", ex.ParamName));
        }

        [TestMethod]
        public void MemberInfoComparer_Custom_ThrowsNotImplemented()
        {
            var eq = new MemberInfoEqualityComparer();
            var custom1 = new MyCustomMemberInfo();
            var custom2 = new MyCustomMemberInfo();

            Assert.ThrowsException<NotImplementedException>(() => eq.Equals(custom1, custom2));
            Assert.ThrowsException<NotImplementedException>(() => eq.GetHashCode(custom1));
            Assert.ThrowsException<NotImplementedException>(() => eq.ResolveMember(typeof(object), custom1));
        }

        [TestMethod]
        public void MemberInfoComparer_Default_ThrowsNotSupported()
        {
            var eq = new MemberInfoEqualityComparer();

            var member1 = new MyMemberInfo();
            var member2 = new MyMemberInfo();

            Assert.ThrowsException<NotSupportedException>(() => eq.Equals(member1, member2));
            Assert.ThrowsException<NotSupportedException>(() => eq.GetHashCode(member1));
            Assert.ThrowsException<NotSupportedException>(() => eq.ResolveMember(typeof(object), member1));
        }

        #endregion

        #region Helper methods

        private static void AssertEqual(MemberInfo m1, MemberInfo m2)
        {
            var eq = new MemberInfoEqualityComparer();
            Assert.IsTrue(eq.Equals(m1, m2));
            Assert.AreEqual(eq.GetHashCode(m1), eq.GetHashCode(m2));
        }

        private static void AssertNotEqual(MemberInfo m1, MemberInfo m2)
        {
            var eq = new MemberInfoEqualityComparer();
            Assert.IsFalse(eq.Equals(m1, m2));
        }

        #endregion

        #region Helper Classes

        private class MyMemberInfo : MemberInfo
        {
            public override Type DeclaringType => throw new NotImplementedException();

            public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotImplementedException();

            public override object[] GetCustomAttributes(bool inherit) => throw new NotImplementedException();

            public override bool IsDefined(Type attributeType, bool inherit) => throw new NotImplementedException();

            public override MemberTypes MemberType => MemberTypes.All;

            public override string Name => throw new NotImplementedException();

            public override Type ReflectedType => throw new NotImplementedException();
        }

        private class MyCustomMemberInfo : MyMemberInfo
        {
            public override MemberTypes MemberType => MemberTypes.Custom;
        }

        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public delegate void ClickedEventHandler(object sender, EventArgs e);

#pragma warning disable CA1822 // Mark static
        private class Foo
        {
            public int Field1;
            public int Field2;

            public event ChangedEventHandler Changed;
            public event ClickedEventHandler Clicked;

            public class Bar
            {
                public string BarProperty { get; set; }
            }

            public class Qux
            {
                public string QuxProperty { get; set; }
            }

            public Foo(int field1)
            {
                Field1 = field1;
                Changed = (obj, e) => { };
                Clicked = (obj, e) => { };
                Clicked.Invoke(sender: null, e: null);
                Changed.Invoke(sender: null, e: null);
            }

            public Foo(int field1, int field2)
            {
                Field1 = field1;
                Field2 = field2;
            }

            public int Property1 { get; set; }
            public int Property2 { get; set; }

            public int Method1() => 0;
        }
#pragma warning restore CA1822

        private sealed class MyMemberComparer : MemberInfoEqualityComparer
        {
            protected override bool EqualsExtension(MemberInfo x, MemberInfo y)
            {
                Assert.IsTrue(x is MyMemberInfo);
                Assert.IsTrue(y is MyMemberInfo);

                return object.ReferenceEquals(x, y);
            }

            protected override int GetHashCodeExtension(MemberInfo obj)
            {
                Assert.IsTrue(obj is MyMemberInfo);

                return 42;
            }
        }

        private sealed class MyCustomMemberComparer : MemberInfoEqualityComparer
        {
            protected override bool EqualsCustom(MemberInfo x, MemberInfo y)
            {
                Assert.IsTrue(x is MyCustomMemberInfo);
                Assert.IsTrue(y is MyCustomMemberInfo);

                return object.ReferenceEquals(x, y);
            }

            protected override int GetHashCodeCustom(MemberInfo obj)
            {
                Assert.IsTrue(obj is MyCustomMemberInfo);

                return 42;
            }
        }

        #endregion
    }
}
