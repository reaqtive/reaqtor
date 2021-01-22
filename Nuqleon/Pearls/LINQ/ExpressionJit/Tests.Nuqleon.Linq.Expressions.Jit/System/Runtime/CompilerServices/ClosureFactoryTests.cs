// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class ClosureFactoryTests
    {
        [TestMethod]
        public void GetClosureType()
        {
            for (var i = 0; i < Data.Types.Length; i++)
            {
                AssertClosureType(Data.Types.Take(i).ToArray(), Data.Values);
            }
        }

        [TestMethod]
        public void GetClosureType_Runtime()
        {
            //
            // Check creation of closure types of various arities.
            //
            foreach (var i in new[] { 17, 18, 31, 65 })
            {
                var types1 = Enumerable.Repeat(typeof(char), i).ToArray();
                var types2 = Enumerable.Repeat(typeof(bool), i).ToArray();

                //
                // Should reuse the same generic closure types.
                //
                var c1 = ClosureFactory.GetClosureType(types1);
                var c2 = ClosureFactory.GetClosureType(types1);
                Assert.AreSame(c1, c2);

                //
                // Generic type definitions should be reused across type instantiations.
                //
                var c3 = ClosureFactory.GetClosureType(types2);
                Assert.AreSame(c1.GetGenericTypeDefinition(), c3.GetGenericTypeDefinition());
            }
        }

        private static void AssertClosureType(Type[] types, Dictionary<Type, object[]> values)
        {
            //
            // Create closure type through factory.
            //
            var c = ClosureFactory.GetClosureType(types);

            //
            // Assert some type system properties.
            //
            Assert.IsTrue(c.IsPublic);
            Assert.IsTrue(c.IsSealed);
            Assert.IsTrue(c.IsClass);
            Assert.AreEqual(typeof(object), c.BaseType);
            Assert.IsTrue(typeof(IRuntimeVariables).IsAssignableFrom(c));

            //
            // Ensure existence of a default constructor.
            //
            var ctor = c.GetConstructor(Type.EmptyTypes);
            Assert.IsNotNull(ctor);

            //
            // Create an instance to test behavior.
            //
            var o = Activator.CreateInstance(c);
            var r = (IRuntimeVariables)o;

            //
            // Assert that Count returns the right value.
            //
            Assert.AreEqual(types.Length, r.Count);

            //
            // Assert indexer exception behavior.
            //
            var n = types.Length;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => r[-1]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => r[-1] = null);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => r[n]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => r[n] = null);

            //
            // Find fields according to the Item* naming pattern.
            //
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var value = values[type][0];

                var field = c.GetField("Item" + (i + 1));
                Assert.IsNotNull(field);
                Assert.AreEqual(type, field.FieldType);
                Assert.IsTrue(field.IsPublic);
                Assert.IsFalse(field.IsStatic);
                Assert.IsFalse(field.IsLiteral);
                Assert.IsFalse(field.IsInitOnly);

                //
                // Set value of the field.
                //
                field.SetValue(o, value);

                //
                // Check behavior of indexer getter.
                //
                Assert.AreEqual(value, r[i]);

                //
                // Assert regular behavior of indexer setter.
                //
                var newValue = values[type][1];
                r[i] = newValue;
                Assert.AreEqual(newValue, field.GetValue(o));
                Assert.AreEqual(newValue, r[i]);
                r[i] = value;
                Assert.AreEqual(value, field.GetValue(o));
                Assert.AreEqual(value, r[i]);

                //
                // Assert cast behavior of indexer setter.
                //
                Assert.ThrowsException<InvalidCastException>(() => { r[i] = typeof(int); });
            }
        }
    }
}
