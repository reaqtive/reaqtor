// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class RuntimeCompilerTests
    {
        #region Anonymous types

        [TestMethod]
        public void DefineAnonymousType_ArgumentChecks()
        {
            var rtc = new RuntimeCompiler();

            var atb = rtc.GetNewAnonymousTypeBuilder();

            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(anonymousTypeBuilder: null, Array.Empty<KeyValuePair<string, Type>>()), ex => Assert.AreEqual("anonymousTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(atb, (KeyValuePair<string, Type>[])null), ex => Assert.AreEqual("properties", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(anonymousTypeBuilder: null, Array.Empty<KeyValuePair<string, Type>>(), Array.Empty<string>()), ex => Assert.AreEqual("anonymousTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(atb, (KeyValuePair<string, Type>[])null, Array.Empty<string>()), ex => Assert.AreEqual("properties", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(anonymousTypeBuilder: null, Array.Empty<StructuralFieldDeclaration>()), ex => Assert.AreEqual("anonymousTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(atb, (StructuralFieldDeclaration[])null), ex => Assert.AreEqual("properties", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(anonymousTypeBuilder: null, Array.Empty<StructuralFieldDeclaration>(), Array.Empty<string>()), ex => Assert.AreEqual("anonymousTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineAnonymousType(atb, (StructuralFieldDeclaration[])null, Array.Empty<string>()), ex => Assert.AreEqual("properties", ex.ParamName));
        }

        [TestMethod]
        public void CreateAnonymousType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateAnonymousType((KeyValuePair<string, Type>[])null), ex => Assert.AreEqual("properties", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateAnonymousType((KeyValuePair<string, Type>[])null, Array.Empty<string>()), ex => Assert.AreEqual("properties", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateAnonymousType((StructuralFieldDeclaration[])null), ex => Assert.AreEqual("properties", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateAnonymousType((StructuralFieldDeclaration[])null, Array.Empty<string>()), ex => Assert.AreEqual("properties", ex.ParamName));
        }

        [TestMethod]
        public void AnonymousType_UnknownKey()
        {
            var rtc = new RuntimeCompiler();

            var atb = rtc.GetNewAnonymousTypeBuilder();

            AssertEx.ThrowsException<ArgumentException>(() => rtc.DefineAnonymousType(atb, new[] { new KeyValuePair<string, Type>("Bar", typeof(int)) }, "Baz"), ex => Assert.AreEqual("keys", ex.ParamName));
        }

        [TestMethod]
        public void AnonymousType_CSharp()
        {
            AnonymousType_CSharp_Impl(() =>
            {
                var rtc = new RuntimeCompiler();

                var atb = rtc.GetNewAnonymousTypeBuilder();

                rtc.DefineAnonymousType(atb, new[]
                {
                    new KeyValuePair<string, Type>("Name", typeof(string)),
                    new KeyValuePair<string, Type>("Age", typeof(int)),
                });

                return atb.CreateType();
            });
        }

        [TestMethod]
        public void AnonymousType_CSharp_StructuralFields()
        {
            AnonymousType_CSharp_Impl(() =>
            {
                var rtc = new RuntimeCompiler();

                var atb = rtc.GetNewAnonymousTypeBuilder();

                rtc.DefineAnonymousType(atb, new[]
                {
                    new StructuralFieldDeclaration("Name", typeof(string)),
                    new StructuralFieldDeclaration("Age", typeof(int)),
                });

                return atb.CreateType();
            });
        }

        [TestMethod]
        public void AnonymousType_CSharp_Static()
        {
            AnonymousType_CSharp_Impl(() =>
            {
                return RuntimeCompiler.CreateAnonymousType(new[]
                {
                    new StructuralFieldDeclaration("Name", typeof(string)),
                    new StructuralFieldDeclaration("Age", typeof(int)),
                });
            });
        }

        [TestMethod]
        public void AnonymousType_CSharp_StructuralFields_Static()
        {
            AnonymousType_CSharp_Impl(() =>
            {
                return RuntimeCompiler.CreateAnonymousType(new[]
                {
                    new StructuralFieldDeclaration("Name", typeof(string)),
                    new StructuralFieldDeclaration("Age", typeof(int)),
                });
            });
        }

        [TestMethod]
        public void AnonymousType_CSharp_StructuralFields_Static_WithKeys()
        {
            AnonymousType_CSharp_Impl(() =>
            {
                return RuntimeCompiler.CreateAnonymousType(new[]
                {
                    new StructuralFieldDeclaration("Name", typeof(string)),
                    new StructuralFieldDeclaration("Age", typeof(int)),
                }, new[] { "Name", "Age" });
            });
        }

        private static void AnonymousType_CSharp_Impl(Func<Type> createType)
        {
            var ant = createType();

            Assert.IsTrue(ant.IsAnonymousType());

            Assert.IsTrue(ant.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false));

            var ctors = ant.GetConstructors();
            Assert.AreEqual(1, ctors.Length);

            var ctorParams = ctors[0].GetParameters();
            Assert.AreEqual(2, ctorParams.Length);

            var ctorParam0 = ctorParams[0];
            Assert.AreEqual("Name", ctorParam0.Name);
            Assert.AreEqual(typeof(string), ctorParam0.ParameterType);

            var ctorParam1 = ctorParams[1];
            Assert.AreEqual("Age", ctorParam1.Name);
            Assert.AreEqual(typeof(int), ctorParam1.ParameterType);

            var props = ant.GetProperties().OrderByDescending(p => p.Name).ToArray();
            Assert.AreEqual(2, props.Length);

            var prop0 = props[0];
            Assert.AreEqual("Name", prop0.Name);
            Assert.AreEqual(typeof(string), prop0.PropertyType);
            Assert.IsTrue(prop0.CanRead);
            Assert.IsFalse(prop0.CanWrite);

            var prop1 = props[1];
            Assert.AreEqual("Age", prop1.Name);
            Assert.AreEqual(typeof(int), prop1.PropertyType);
            Assert.IsTrue(prop1.CanRead);
            Assert.IsFalse(prop1.CanWrite);

            var bart = Activator.CreateInstance(ant, new object[] { "Bart", 10 });

            Assert.AreEqual("Bart", prop0.GetValue(bart));
            Assert.AreEqual(10, prop1.GetValue(bart));
            Assert.AreEqual("{ Name = Bart, Age = 10 }", bart.ToString());

            var jojo = Activator.CreateInstance(ant, new object[] { "Bart", 10 });

            Assert.IsTrue(bart.Equals(jojo));
            Assert.IsTrue(jojo.Equals(bart));
            Assert.AreEqual(bart.GetHashCode(), jojo.GetHashCode());

            var lisa = Activator.CreateInstance(ant, new object[] { "Lisa", 8 });

            Assert.IsFalse(bart.Equals(lisa));
            Assert.IsFalse(lisa.Equals(bart));
            Assert.AreNotEqual(bart.GetHashCode(), lisa.GetHashCode());
        }

        [TestMethod]
        public void AnonymousType_VisualBasic()
        {
            AnonymousType_VisualBasic_Impl(() =>
            {
                var rtc = new RuntimeCompiler();

                var atb = rtc.GetNewAnonymousTypeBuilder();

                rtc.DefineAnonymousType(atb, new[]
                {
                    new KeyValuePair<string, Type>("Name", typeof(string)),
                    new KeyValuePair<string, Type>("Age", typeof(int)),
                }, new[] { "Name" });

                return atb.CreateType();
            });
        }

        [TestMethod]
        public void AnonymousType_VisualBasic_Static()
        {
            AnonymousType_VisualBasic_Impl(() =>
            {
                return RuntimeCompiler.CreateAnonymousType(new[]
                {
                    new KeyValuePair<string, Type>("Name", typeof(string)),
                    new KeyValuePair<string, Type>("Age", typeof(int)),
                }, new[] { "Name" });
            });
        }

        private static void AnonymousType_VisualBasic_Impl(Func<Type> createType)
        {
            var ant = createType();

            Assert.IsTrue(ant.IsAnonymousType());

            Assert.IsTrue(ant.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false));

            var ctors = ant.GetConstructors();
            Assert.AreEqual(1, ctors.Length);

            var ctorParams = ctors[0].GetParameters();
            Assert.AreEqual(2, ctorParams.Length);

            var ctorParam0 = ctorParams[0];
            Assert.AreEqual("Name", ctorParam0.Name);
            Assert.AreEqual(typeof(string), ctorParam0.ParameterType);

            var ctorParam1 = ctorParams[1];
            Assert.AreEqual("Age", ctorParam1.Name);
            Assert.AreEqual(typeof(int), ctorParam1.ParameterType);

            var props = ant.GetProperties().OrderByDescending(p => p.Name).ToArray();
            Assert.AreEqual(2, props.Length);

            var prop0 = props[0];
            Assert.AreEqual("Name", prop0.Name);
            Assert.AreEqual(typeof(string), prop0.PropertyType);
            Assert.IsTrue(prop0.CanRead);
            Assert.IsFalse(prop0.CanWrite);

            var prop1 = props[1];
            Assert.AreEqual("Age", prop1.Name);
            Assert.AreEqual(typeof(int), prop1.PropertyType);
            Assert.IsTrue(prop1.CanRead);
            Assert.IsTrue(prop1.CanWrite);

            var bart = Activator.CreateInstance(ant, new object[] { "Bart", 10 });

            Assert.AreEqual("Bart", prop0.GetValue(bart));
            Assert.AreEqual(10, prop1.GetValue(bart));
            Assert.AreEqual("{ Name = Bart, Age = 10 }", bart.ToString());

            var jojo = Activator.CreateInstance(ant, new object[] { "Bart", 12 });

            Assert.IsTrue(bart.Equals(jojo));
            Assert.IsTrue(jojo.Equals(bart));
            Assert.AreEqual(bart.GetHashCode(), jojo.GetHashCode());

            prop1.SetValue(bart, 11);
            Assert.AreEqual(11, prop1.GetValue(bart));
            Assert.AreEqual("{ Name = Bart, Age = 11 }", bart.ToString());

            Assert.IsTrue(bart.Equals(jojo));
            Assert.IsTrue(jojo.Equals(bart));
            Assert.AreEqual(bart.GetHashCode(), jojo.GetHashCode());

            var lisa = Activator.CreateInstance(ant, new object[] { "Lisa", 8 });

            Assert.IsFalse(bart.Equals(lisa));
            Assert.IsFalse(lisa.Equals(bart));
            Assert.AreNotEqual(bart.GetHashCode(), lisa.GetHashCode());
        }

        [TestMethod]
        public void AnonymousType_NoKeys()
        {
            var rtc = new RuntimeCompiler();

            var atb = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineAnonymousType(atb, Array.Empty<KeyValuePair<string, Type>>());

            var ant = atb.CreateType();

            Assert.IsTrue(ant.IsAnonymousType());

            Assert.IsTrue(ant.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false));

            var ctors = ant.GetConstructors();
            Assert.AreEqual(1, ctors.Length);

            var ctorParams = ctors[0].GetParameters();
            Assert.AreEqual(0, ctorParams.Length);

            var props = ant.GetProperties();
            Assert.AreEqual(0, props.Length);

            var foo = Activator.CreateInstance(ant);

            Assert.AreEqual("{ }", foo.ToString());

            var bar = Activator.CreateInstance(ant);

            Assert.IsTrue(foo.Equals(bar));
            Assert.IsTrue(bar.Equals(foo));
            Assert.AreEqual(bar.GetHashCode(), foo.GetHashCode());
        }

        [TestMethod]
        public void AnonymousType_Nested()
        {
            var rtc = new RuntimeCompiler();

            var atb1 = rtc.GetNewAnonymousTypeBuilder();
            var atb2 = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineAnonymousType(atb1, new[]
            {
                new KeyValuePair<string, Type>("Qux", typeof(int)),
            });

            rtc.DefineAnonymousType(atb2, new[]
            {
                new KeyValuePair<string, Type>("Foo", atb1),
                new KeyValuePair<string, Type>("Baz", typeof(int)),
            }, new[] { "Foo" });

            var foo = atb1.CreateType();
            var bar = atb2.CreateType();

            var objFoo1 = Activator.CreateInstance(foo, new object[] { 42 });
            var objFoo2 = Activator.CreateInstance(foo, new object[] { 42 });

            var objBar1 = Activator.CreateInstance(bar, new object[] { objFoo1, 43 });
            var objBar2 = Activator.CreateInstance(bar, new object[] { objFoo2, 44 });

            Assert.AreEqual(objBar1, objBar2);
            Assert.AreEqual("{ Foo = { Qux = 42 }, Baz = 43 }", objBar1.ToString());
            Assert.AreEqual("{ Foo = { Qux = 42 }, Baz = 44 }", objBar2.ToString());
        }

        [TestMethod]
        public void AnonymousType_Recursive()
        {
            var rtc = new RuntimeCompiler();

            var atb1 = rtc.GetNewAnonymousTypeBuilder();
            var atb2 = rtc.GetNewAnonymousTypeBuilder();

            rtc.DefineAnonymousType(atb1, new[]
            {
                new KeyValuePair<string, Type>("Qux", typeof(int)),
                new KeyValuePair<string, Type>("Bar", atb2),
            }, Array.Empty<string>());

            rtc.DefineAnonymousType(atb2, new[]
            {
                new KeyValuePair<string, Type>("Baz", typeof(int)),
                new KeyValuePair<string, Type>("Foo", atb1),
            }, Array.Empty<string>());

            var foo = atb1.CreateType();
            var bar = atb2.CreateType();

            var objFoo = Activator.CreateInstance(foo, new object[] { 42, null });
            var objBar = Activator.CreateInstance(bar, new object[] { 43, null });

            var fooBar = foo.GetProperty("Bar");
            fooBar.SetValue(objFoo, objBar);

            var barFoo = bar.GetProperty("Foo");
            barFoo.SetValue(objBar, objFoo);

            Assert.AreSame(fooBar.GetValue(objFoo), objBar);
            Assert.AreSame(barFoo.GetValue(objBar), objFoo);
        }

        [TestMethod]
        public void AnonymousType_Visibility()
        {
            var props = new[] { new KeyValuePair<string, Type>("bar", typeof(Bar)) };
            var props2 = new[] { new StructuralFieldDeclaration("bar", typeof(string), new List<CustomAttributeDeclaration> { new CustomAttributeDeclaration(typeof(BarAttribute), new List<object>().AsReadOnly()) }.AsReadOnly()) };
            var props3 = new[] { new StructuralFieldDeclaration("bar", typeof(Bar)) };

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateAnonymousType(props));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewAnonymousTypeBuilder(); rtc.DefineAnonymousType(atb, props); });

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateAnonymousType(props2));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewAnonymousTypeBuilder(); rtc.DefineAnonymousType(atb, props2); });

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateAnonymousType(props3));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewAnonymousTypeBuilder(); rtc.DefineAnonymousType(atb, props3); });

        }

        [TestMethod]
        public void AnonymousType_PropertyAttribute_NoMatchingConstructor()
        {
            var props = new[] { new StructuralFieldDeclaration("bar", typeof(string), new List<CustomAttributeDeclaration> { new CustomAttributeDeclaration(typeof(FooAttribute), new List<object> { 0 }.AsReadOnly()) }.AsReadOnly()) };

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateAnonymousType(props));
        }

        [TestMethod]
        public void AnonymousType_PropertiesAttributes()
        {
            var expectedAttributeValue = "bing://foo";
            var customAttribute = new CustomAttributeDeclaration(typeof(FooAttribute), new List<object> { expectedAttributeValue }.AsReadOnly());
            var props = new[] { new StructuralFieldDeclaration("Foo", typeof(int), new List<CustomAttributeDeclaration> { customAttribute }.AsReadOnly()) };
            var type = RuntimeCompiler.CreateAnonymousType(props);
            var property = type.GetProperty("Foo");
            var actualAttribute = property.GetCustomAttribute<FooAttribute>(inherit: false);

            Assert.IsNotNull(actualAttribute);
            Assert.AreEqual(expectedAttributeValue, actualAttribute.Uri);
        }

        #endregion

        #region Closure types

        [TestMethod]
        public void DefineClosureType_ArgumentChecks()
        {
            var rtc = new RuntimeCompiler();

            var atb = rtc.GetNewClosureTypeBuilder();

            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineClosureType(closureTypeBuilder: null, Array.Empty<KeyValuePair<string, Type>>()), ex => Assert.AreEqual("closureTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineClosureType(atb, fields: null), ex => Assert.AreEqual("fields", ex.ParamName));
        }

        [TestMethod]
        public void CreateClosureType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateClosureType(fields: null), ex => Assert.AreEqual("fields", ex.ParamName));
        }

        [TestMethod]
        public void ClosureType_Empty()
        {
            var rtc = new RuntimeCompiler();

            var ctb = rtc.GetNewClosureTypeBuilder();

            rtc.DefineClosureType(ctb, Array.Empty<KeyValuePair<string, Type>>());

            var clt = ctb.CreateType();

            Assert.IsTrue(clt.IsClosureClass());

            Assert.IsTrue(clt.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false));
        }

        [TestMethod]
        public void ClosureType_NonTrivial()
        {
            ClosureType_NonTrivial_Impl(() =>
            {
                var rtc = new RuntimeCompiler();

                var ctb = rtc.GetNewClosureTypeBuilder();

                rtc.DefineClosureType(ctb, new[]
                {
                    new KeyValuePair<string, Type>("bar", typeof(int)),
                    new KeyValuePair<string, Type>("foo", typeof(string)),
                });

                return ctb.CreateType();
            });
        }

        [TestMethod]
        public void ClosureType_NonTrivial_Static()
        {
            ClosureType_NonTrivial_Impl(() =>
            {
                return RuntimeCompiler.CreateClosureType(new[]
                {
                    new KeyValuePair<string, Type>("bar", typeof(int)),
                    new KeyValuePair<string, Type>("foo", typeof(string)),
                });
            });
        }

        private static void ClosureType_NonTrivial_Impl(Func<Type> createType)
        {
            var clt = createType();

            Assert.IsTrue(clt.IsClosureClass());

            Assert.IsTrue(clt.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false));

            var flds = clt.GetFields();
            Assert.AreEqual(2, flds.Length);

            var fld0 = flds[0];
            Assert.AreEqual("bar", fld0.Name);
            Assert.AreEqual(typeof(int), fld0.FieldType);

            var fld1 = flds[1];
            Assert.AreEqual("foo", fld1.Name);
            Assert.AreEqual(typeof(string), fld1.FieldType);

            var obj = Activator.CreateInstance(clt);
            fld0.SetValue(obj, 42);
            fld1.SetValue(obj, "qux");

            Assert.AreEqual(42, fld0.GetValue(obj));
            Assert.AreEqual("qux", fld1.GetValue(obj));
        }

        [TestMethod]
        public void ClosureType_Visibility()
        {
            var props = new[] { new KeyValuePair<string, Type>("bar", typeof(Bar)) };

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateClosureType(props));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewClosureTypeBuilder(); rtc.DefineClosureType(atb, props); });
        }

        #endregion

        #region Record types

        [TestMethod]
        public void DefineRecordType_ArgumentChecks()
        {
            var rtc = new RuntimeCompiler();

            var rtb = rtc.GetNewRecordTypeBuilder();

            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineRecordType(recordTypeBuilder: null, Array.Empty<KeyValuePair<string, Type>>(), valueEquality: true), ex => Assert.AreEqual("recordTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineRecordType(rtb, properties: (KeyValuePair<string, Type>[])null, valueEquality: true), ex => Assert.AreEqual("properties", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineRecordType(recordTypeBuilder: null, Array.Empty<StructuralFieldDeclaration>(), valueEquality: true), ex => Assert.AreEqual("recordTypeBuilder", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => rtc.DefineRecordType(rtb, properties: (StructuralFieldDeclaration[])null, valueEquality: true), ex => Assert.AreEqual("properties", ex.ParamName));
        }

        [TestMethod]
        public void CreateRecordType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateRecordType(properties: (KeyValuePair<string, Type>[])null, valueEquality: true), ex => Assert.AreEqual("properties", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => RuntimeCompiler.CreateRecordType(properties: (StructuralFieldDeclaration[])null, valueEquality: true), ex => Assert.AreEqual("properties", ex.ParamName));
        }

        [TestMethod]
        public void RecordType_TypeBuilder()
        {
            foreach (var eq in new[] { true, false })
            {
                RecordType_Impl(() =>
                {
                    var rtc = new RuntimeCompiler();

                    var rtb = rtc.GetNewRecordTypeBuilder();

                    rtc.DefineRecordType(rtb, new[]
                    {
                        new KeyValuePair<string, Type>("Name", typeof(string)),
                        new KeyValuePair<string, Type>("Age", typeof(int)),
                    }, eq);

                    return rtb.CreateType();
                }, eq);
            }
        }

        [TestMethod]
        public void RecordType_TypeBuilder_StructuralFields()
        {
            foreach (var eq in new[] { true, false })
            {
                RecordType_Impl(() =>
                {
                    var rtc = new RuntimeCompiler();

                    var rtb = rtc.GetNewRecordTypeBuilder();

                    rtc.DefineRecordType(rtb, new[]
                    {
                        new StructuralFieldDeclaration("Name", typeof(string)),
                        new StructuralFieldDeclaration("Age", typeof(int)),
                    }, eq);

                    return rtb.CreateType();
                }, eq);
            }
        }

        [TestMethod]
        public void RecordType_Create()
        {
            foreach (var eq in new[] { true, false })
            {
                RecordType_Impl(() =>
                {
                    return RuntimeCompiler.CreateRecordType(new[]
                    {
                        new KeyValuePair<string, Type>("Name", typeof(string)),
                        new KeyValuePair<string, Type>("Age", typeof(int)),
                    }, eq);
                }, eq);
            }
        }

        [TestMethod]
        public void RecordType_Create_StructuralFields()
        {
            foreach (var eq in new[] { true, false })
            {
                RecordType_Impl(() =>
                {
                    return RuntimeCompiler.CreateRecordType(new[]
                    {
                        new StructuralFieldDeclaration("Name", typeof(string)),
                        new StructuralFieldDeclaration("Age", typeof(int)),
                    }, eq);
                }, eq);
            }
        }

        private static void RecordType_Impl(Func<Type> createType, bool valueEquality)
        {
            var rdt = createType();

            Assert.IsTrue(rdt.IsRecordType());

            Assert.IsTrue(rdt.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false));

            var ctors = rdt.GetConstructors();
            Assert.AreEqual(1, ctors.Length);

            var ctorParams = ctors[0].GetParameters();
            Assert.AreEqual(0, ctorParams.Length);

            var props = rdt.GetProperties().OrderByDescending(p => p.Name).ToArray();
            Assert.AreEqual(2, props.Length);

            var prop0 = props[0];
            Assert.AreEqual("Name", prop0.Name);
            Assert.AreEqual(typeof(string), prop0.PropertyType);
            Assert.IsTrue(prop0.CanRead);
            Assert.IsTrue(prop0.CanWrite);

            var prop1 = props[1];
            Assert.AreEqual("Age", prop1.Name);
            Assert.AreEqual(typeof(int), prop1.PropertyType);
            Assert.IsTrue(prop1.CanRead);
            Assert.IsTrue(prop1.CanWrite);

            var bart = Activator.CreateInstance(rdt);
            prop0.SetValue(bart, "Bart");
            prop1.SetValue(bart, 10);

            Assert.IsTrue(bart.Equals(bart));
            Assert.AreEqual(bart.GetHashCode(), bart.GetHashCode());

            Assert.AreEqual("Bart", prop0.GetValue(bart));
            Assert.AreEqual(10, prop1.GetValue(bart));
            Assert.AreEqual("{ Name = Bart, Age = 10 }", bart.ToString());

            var jojo = Activator.CreateInstance(rdt);
            prop0.SetValue(jojo, "Bart");
            prop1.SetValue(jojo, 10);

            if (valueEquality)
            {
                Assert.IsTrue(bart.Equals(jojo));
                Assert.IsTrue(jojo.Equals(bart));
                Assert.AreEqual(bart.GetHashCode(), jojo.GetHashCode());
            }
            else
            {
                Assert.IsFalse(bart.Equals(jojo));
            }

            var lisa = Activator.CreateInstance(rdt);
            prop0.SetValue(lisa, "Lisa");
            prop1.SetValue(lisa, 8);

            Assert.IsFalse(bart.Equals(lisa));
            Assert.IsFalse(lisa.Equals(bart));

            if (valueEquality)
            {
                Assert.AreNotEqual(bart.GetHashCode(), lisa.GetHashCode());
            }
        }

        [TestMethod]
        public void RecordType_Visibility()
        {
            var props = new[] { new KeyValuePair<string, Type>("bar", typeof(Bar)) };
            var props2 = new[] { new StructuralFieldDeclaration("bar", typeof(string), new List<CustomAttributeDeclaration> { new CustomAttributeDeclaration(typeof(BarAttribute), new List<object>().AsReadOnly()) }.AsReadOnly()) };
            var props3 = new[] { new StructuralFieldDeclaration("bar", typeof(Bar)) };

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateRecordType(props, valueEquality: true));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewRecordTypeBuilder(); rtc.DefineRecordType(atb, props, valueEquality: true); });

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateRecordType(props2, valueEquality: true));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewRecordTypeBuilder(); rtc.DefineRecordType(atb, props2, valueEquality: true); });

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateRecordType(props3, valueEquality: true));
            Assert.ThrowsException<InvalidOperationException>(() => { var rtc = new RuntimeCompiler(); var atb = rtc.GetNewRecordTypeBuilder(); rtc.DefineRecordType(atb, props3, valueEquality: true); });
        }

        [TestMethod]
        public void RecordType_PropertyAttribute_NoMatchingConstructor()
        {
            var props = new[] { new StructuralFieldDeclaration("bar", typeof(string), new List<CustomAttributeDeclaration> { new CustomAttributeDeclaration(typeof(FooAttribute), new List<object> { 0 }.AsReadOnly()) }.AsReadOnly()) };

            Assert.ThrowsException<InvalidOperationException>(() => RuntimeCompiler.CreateRecordType(props, valueEquality: true));
        }

        [TestMethod]
        public void RecordType_PropertiesAttributes()
        {
            var expectedAttributeValue = "bing://foo";
            var customAttribute = new CustomAttributeDeclaration(typeof(FooAttribute), new List<object> { expectedAttributeValue }.AsReadOnly());
            var props = new[] { new StructuralFieldDeclaration("Foo", typeof(int), new List<CustomAttributeDeclaration> { customAttribute }.AsReadOnly()) };
            var type = RuntimeCompiler.CreateRecordType(props, valueEquality: true);
            var property = type.GetProperty("Foo");
            var actualAttribute = property.GetCustomAttribute<FooAttribute>(inherit: false);

            Assert.IsNotNull(actualAttribute);
            Assert.AreEqual(expectedAttributeValue, actualAttribute.Uri);
        }


        #endregion

        #region CustomAttributeDeclaration

        [TestMethod]
        public void CustomAttributeDeclaration_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new CustomAttributeDeclaration(type: null, new List<object>().AsReadOnly()), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new CustomAttributeDeclaration(typeof(FooAttribute), default(ReadOnlyCollection<object>)), ex => Assert.AreEqual("arguments", ex.ParamName));
            Assert.ThrowsException<InvalidOperationException>(() => new CustomAttributeDeclaration(typeof(Bar), new List<object>().AsReadOnly()));
        }

        #endregion

        #region Helper types

        private sealed class Bar
        {
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
        private sealed class BarAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
        public sealed class FooAttribute : Attribute
        {
            public FooAttribute(string uri) => Uri = uri;

            public string Uri { get; }
        }

        #endregion
    }
}
