// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtive.Expressions;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class EntityReaderWriterTests
    {
        [TestMethod]
        public void EntityReaderWriter_Roundtrip_Definitions()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());

            var def = new OtherDefinitionEntity(new Uri("io:/1"), Expression.Default(typeof(object)), null);
            var stream = new MemoryStream();
            using (var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1)))
            {
                writer.WriteHeader();
                writer.Save(def);
                writer.Dispose();
            }

            foreach (var kind in new[]
                {
                    ReactiveEntityKind.Observable,
                    ReactiveEntityKind.Observer,
                    ReactiveEntityKind.StreamFactory,
                    ReactiveEntityKind.Other,
                })
            {
                stream.Position = 0;
                var rt = default(ReactiveEntity);
                using (var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1)))
                {
                    reader.ReadHeader();
                    rt = reader.Load(kind);
                    reader.ReadFooter();
                }

                Assert.AreEqual(kind, rt.Kind);
                Assert.AreEqual(def.Uri, rt.Uri);
                AssertEqual(def.Expression, rt.Expression);
                Assert.AreEqual(def.State, rt.State);
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_Stream()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());

            var sub = new SubjectEntity(new Uri("io:/1"), Expression.Default(typeof(object)), null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            var rt = Read();

            Assert.AreEqual(ReactiveEntityKind.Stream, rt.Kind);
            Assert.AreEqual(sub.Uri, rt.Uri);
            AssertEqual(sub.Expression, rt.Expression);
            Assert.AreEqual(sub.State, rt.State);

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                writer.WriteHeader();
                writer.Save(sub);
            }

            ReactiveEntity Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                reader.ReadHeader();

                var rt = reader.Load(ReactiveEntityKind.Stream);

                reader.ReadFooter();

                return rt;
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_Subscription()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());

            var sub = new SubscriptionEntity(new Uri("io:/1"), Expression.Default(typeof(object)), null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            var rt = Read();

            Assert.AreEqual(ReactiveEntityKind.Subscription, rt.Kind);
            Assert.AreEqual(sub.Uri, rt.Uri);
            AssertEqual(sub.Expression, rt.Expression);
            Assert.AreEqual(sub.State, rt.State);

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                writer.WriteHeader();
                writer.Save(sub);
            }

            ReactiveEntity Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                reader.ReadHeader();

                var rt = reader.Load(ReactiveEntityKind.Subscription);

                reader.ReadFooter();

                return rt;
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_ReliableSubscription()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());

            var sub = new ReliableSubscriptionEntity(new Uri("io:/1"), Expression.Default(typeof(object)), null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            var rt = Read();

            Assert.AreEqual(ReactiveEntityKind.ReliableSubscription, rt.Kind);
            Assert.AreEqual(sub.Uri, rt.Uri);
            AssertEqual(sub.Expression, rt.Expression);
            Assert.AreEqual(sub.State, rt.State);

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                writer.WriteHeader();
                writer.Save(sub);
            }

            ReactiveEntity Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                reader.ReadHeader();

                var rt = reader.Load(ReactiveEntityKind.ReliableSubscription);

                reader.ReadFooter();

                return rt;
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_None_ThrowsArgumentException()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());

            var def = new OtherDefinitionEntity(new Uri("io:/1"), Expression.Default(typeof(object)), null);
            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            Read();

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                writer.WriteHeader();
                writer.Save(def);
            }

            void Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v1));

                reader.ReadHeader();

                AssertEx.ThrowsException<ArgumentException>(() => reader.Load(ReactiveEntityKind.None), ex => Assert.AreEqual("kind", ex.ParamName));
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_V3_NotTemplatized()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());

            var io = new ObservableDefinitionEntity(new Uri("io:/1"), Expression.Default(typeof(object)), null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            Read();

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                writer.WriteHeader();
                writer.Save(io);
            }

            void Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                reader.ReadHeader();

                _ = reader.Load(ReactiveEntityKind.Observable);

                reader.ReadFooter();
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_V3_Templatized_NoConstants()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());
            var templatizer = new QueryEngineRegistryTemplatizer(registry);
            var templatized = templatizer.Templatize(Expression.Default(typeof(object)));

            var io = new ObservableDefinitionEntity(new Uri("io:/1"), templatized, null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            Read();

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                writer.WriteHeader();
                writer.Save(io);
            }

            void Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                reader.ReadHeader();

                _ = reader.Load(ReactiveEntityKind.Observable);

                reader.ReadFooter();
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_V3_Templatized_WithConstants()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());
            var templatizer = new QueryEngineRegistryTemplatizer(registry);
            var templatized = templatizer.Templatize(Expression.Constant(42));

            var io = new ObservableDefinitionEntity(new Uri("io:/1"), templatized, null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            Read();

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                writer.WriteHeader();
                writer.Save(io);
            }

            void Read()
            {
                using var reader = new EntityReader(stream, registry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                reader.ReadHeader();

                _ = reader.Load(ReactiveEntityKind.Observable);

                reader.ReadFooter();
            }
        }

        [TestMethod]
        public void EntityReaderWriter_Roundtrip_V3_Templatized_MissingTemplate()
        {
            var registry = new QueryEngineRegistry(new MockQueryEngineRegistry());
            var templatizer = new QueryEngineRegistryTemplatizer(registry);
            var templatized = templatizer.Templatize(Expression.Default(typeof(object)));

            var io = new ObservableDefinitionEntity(new Uri("io:/1"), templatized, null);

            var stream = new MemoryStream();

            Write();

            stream.Position = 0;

            Read();

            void Write()
            {
                using var writer = new EntityWriter(stream, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                writer.WriteHeader();
                writer.Save(io);
            }

            void Read()
            {
                var emptyRegistry = new QueryEngineRegistry(new MockQueryEngineRegistry());

                using var reader = new EntityReader(stream, emptyRegistry, new SerializationPolicy(DefaultExpressionPolicy.Instance, Versioning.v3));

                reader.ReadHeader();

                Assert.ThrowsException<InvalidOperationException>(() => reader.Load(ReactiveEntityKind.Observable));
            }
        }

        private static void AssertEqual(Expression x, Expression y)
        {
            Assert.IsTrue(new ExpressionEqualityComparer().Equals(x, y));
        }
    }
}
