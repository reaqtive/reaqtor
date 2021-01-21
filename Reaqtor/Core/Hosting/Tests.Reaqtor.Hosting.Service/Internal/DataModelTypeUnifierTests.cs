// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;

using Reaqtor;
using Reaqtor.Hosting.Shared.Serialization;

namespace Tests.Microsoft.Hosting.Shared.Serialization
{
    [TestClass]
    public class DataModelTypeUnifierTests
    {
        [TestMethod]
        public void DataModelTypeUnifier_UnifyFails_ThrowsInvalidOperation()
        {
            var record = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "zoo", typeof(int) } }, valueEquality: true);
            Assert.ThrowsException<InvalidOperationException>(() => Unify(typeof(IAsyncReactiveQbservable<>).MakeGenericType(record), typeof(IReactiveQbservable<Foo>)).ToArray());
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifySimple_NoMappings()
        {
            Assert.AreEqual(0, Unify(typeof(IAsyncReactiveQbservable<int>), typeof(IReactiveQbservable<int>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyEnum_NoMappings()
        {
            Assert.AreEqual(0, Unify(typeof(IAsyncReactiveQbservable<int>), typeof(IReactiveQbservable<FooEnum>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyOther_NoMappings()
        {
            Assert.AreEqual(0, Unify(typeof(IAsyncReactiveQbservable<int>), typeof(List<int>)).ToArray().Length);
            Assert.AreEqual(0, Unify(typeof(IAsyncReactiveQbservable<int>), typeof(List<int>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyDataModelFromStructural()
        {
            var record = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "bar", typeof(int) } }, valueEquality: true);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQbservable<>).MakeGenericType(record), typeof(IReactiveQbservable<Foo>)).ToArray().Length);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQbserver<>).MakeGenericType(record), typeof(IReactiveQbserver<Foo>)).ToArray().Length);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQubjectFactory<,>).MakeGenericType(record, record), typeof(IReactiveQubjectFactory<Foo, Foo>)).ToArray().Length);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQubscriptionFactory<>).MakeGenericType(record), typeof(IReactiveQubscriptionFactory<Foo>)).ToArray().Length);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQubject<>).MakeGenericType(record), typeof(IReactiveQubject<Foo>)).ToArray().Length);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQubject<,>).MakeGenericType(record, record), typeof(IReactiveQubject<Foo, Foo>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyParameterized()
        {
            var record1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "bar", typeof(int) } }, valueEquality: true);
            var record2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "qux", typeof(int) } }, valueEquality: true);
            Assert.AreEqual(2, Unify(typeof(Func<,>).MakeGenericType(record2, typeof(IAsyncReactiveQbservable<>).MakeGenericType(record1)), typeof(Func<Bar, IReactiveQbservable<Foo>>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyRecursive()
        {
            var record = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "bar", typeof(int) } }, valueEquality: true);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQbservable<>).MakeGenericType(typeof(IAsyncReactiveQbservable<>).MakeGenericType(record)), typeof(IReactiveQbservable<IReactiveQbservable<Foo>>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyStructuralRecursive()
        {
            var record1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "bar", typeof(int) } }, valueEquality: true);
            var record2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "obs", typeof(IAsyncReactiveQbservable<>).MakeGenericType(record1) } }, valueEquality: true);

            // This test is used for demonstration of how we will need to recurse into structural types to find unifications
            // Unfortunately, the type unifier is only informed of the expressible variants of the Reactive entity types. It
            // is not aware of implementation specific types, such as ISubscribable, which would more likely be found in
            // structural types.
            Assert.AreEqual(2, Unify(typeof(IAsyncReactiveQbservable<>).MakeGenericType(record2), typeof(IReactiveQbservable<Rec>)).ToArray().Length);
        }

        [TestMethod]
        public void DataModelTypeUnifier_UnifyByPropertyName()
        {
            var record = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "Bar", typeof(int) } }, valueEquality: true);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQbservable<>).MakeGenericType(record), typeof(IReactiveQbservable<FooNoDM>)).ToArray().Length);
            Assert.AreEqual(1, Unify(typeof(IAsyncReactiveQbservable<>).MakeGenericType(record), typeof(IReactiveQbservable<Foo>)).ToArray().Length);
        }

        public enum FooEnum
        {
            A = 0,
        }

        public class Foo
        {
            [Mapping("bar")]
            public int Bar { get; set; }
        }

        public class FooNoDM
        {
            public int Bar { get; set; }
        }

        public class Bar
        {
            [Mapping("qux")]
            public int Qux { get; set; }
        }

        public class Rec
        {
            [Mapping("obs")]
            public IReactiveQbservable<Foo> Obs { get; set; }
        }

        private static TypeSlim Slim(Type type)
        {
            return new TypeToTypeSlimConverter().Visit(type);
        }

        private static IEnumerable<KeyValuePair<TypeSlim, Type>> Unify(Type slimType, Type richType)
        {
            var unifier = new DataModelTypeUnifier();
            unifier.Unify(richType, Slim(slimType));
            return unifier.Entries;
        }
    }
}
