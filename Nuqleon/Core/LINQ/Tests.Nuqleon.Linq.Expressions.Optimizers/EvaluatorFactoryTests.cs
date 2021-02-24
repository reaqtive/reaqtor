// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public class EvaluatorFactoryTests
    {
        [TestMethod]
        public void EvaluatorFactory_StaticField()
        {
            var field = typeof(Bar).GetField(nameof(Bar.StaticField));
            var d = GetEvaluatorFactory().GetEvaluator(field);
            Assert.AreEqual(42, d.DynamicInvoke(Array.Empty<object>()));
        }

        [TestMethod]
        public void EvaluatorFactory_InstanceField()
        {
            var field = typeof(Bar).GetField(nameof(Bar.InstanceField));
            var d = GetEvaluatorFactory().GetEvaluator(field);
            Assert.AreEqual(42, d.DynamicInvoke(new object[] { new Bar() }));
        }

        [TestMethod]
        public void EvaluatorFactory_StaticProperty()
        {
            var property = typeof(Bar).GetProperty(nameof(Bar.StaticProperty));
            var d = GetEvaluatorFactory().GetEvaluator(property);
            Assert.AreEqual(42, d.DynamicInvoke(Array.Empty<object>()));
        }

        [TestMethod]
        public void EvaluatorFactory_InstanceProperty()
        {
            var property = typeof(Bar).GetProperty(nameof(Bar.InstanceProperty));
            var d = GetEvaluatorFactory().GetEvaluator(property);
            Assert.AreEqual(42, d.DynamicInvoke(new object[] { new Bar() }));
        }

        [TestMethod]
        public void EvaluatorFactory_InstancePropertyIndexed()
        {
            var property = typeof(Bar).GetProperty("Item");
            var d = GetEvaluatorFactory().GetEvaluator(property);
            Assert.AreEqual(42, d.DynamicInvoke(new object[] { new Bar(), 39, 3 }));
        }

        [TestMethod]
        public void EvaluatorFactory_Constructor()
        {
            var ctor = typeof(Bar).GetConstructor(new[] { typeof(int), typeof(int) });
            var d = GetEvaluatorFactory().GetEvaluator(ctor);
            Assert.AreEqual(42, ((Bar)d.DynamicInvoke(new object[] { 39, 3 })).InstanceField);
        }

        [TestMethod]
        public void EvaluatorFactory_StaticMethod()
        {
            var method = typeof(Bar).GetMethod(nameof(Bar.StaticMethod));
            var d = GetEvaluatorFactory().GetEvaluator(method);
            Assert.AreEqual(42, d.DynamicInvoke(new object[] { 39, 3 }));
        }

        [TestMethod]
        public void EvaluatorFactory_InstanceMethod()
        {
            var method = typeof(Bar).GetMethod(nameof(Bar.InstanceMethod));
            var d = GetEvaluatorFactory().GetEvaluator(method);
            Assert.AreEqual(42, d.DynamicInvoke(new object[] { new Bar(), 39, 3 }));
        }

        [TestMethod]
        public void EvaluatorFactory_Type()
        {
            var d = GetEvaluatorFactory().GetEvaluator(typeof(Foo));
            Assert.IsTrue(d.DynamicInvoke(Array.Empty<object>()) is Foo);
        }

        [TestMethod]
        public void EvaluatorFactory_Event()
        {
            var evt = typeof(Bar).GetEvent(nameof(Bar.InstanceEvent));
            Assert.ThrowsException<InvalidOperationException>(() => GetEvaluatorFactory().GetEvaluator(evt));
        }

        private static IEvaluatorFactory GetEvaluatorFactory() => new DefaultEvaluatorFactory();

#pragma warning disable CA1822 // Mark static
        private sealed class Bar
        {
            public static int StaticField = 42;
            public int InstanceField = 42;

            public static int StaticProperty => 42;
            public int InstanceProperty => 42;

            public event Action InstanceEvent;

            public int this[int x, int y] => x + y;

            public Bar()
            {
            }

            public Bar(int x, int y)
            {
                InstanceField = x + y;
            }

            public static int StaticMethod(int x, int y) => x + y;
            public int InstanceMethod(int x, int y) => x + y;
        }
#pragma warning restore CA1822

        private struct Foo { }
    }
}
