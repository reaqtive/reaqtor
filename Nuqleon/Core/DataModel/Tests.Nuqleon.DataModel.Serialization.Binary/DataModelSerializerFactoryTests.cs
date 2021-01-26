// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

using Nuqleon.DataModel.Serialization.Binary;
using Nuqleon.DataModel;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    [TestClass]
    public partial class DataModelSerializerFactoryTests
    {
        private readonly DataModelSerializerFactoryTestCase testCase = new();

        #region DataModelSerializerFactory.TestCases

        [TestMethod]
        public void DataModelSerializerFactory_Primitives()
        {
            Run(testCase.DataModelSerializerFactory_Primitives_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Arrays()
        {
            Run(testCase.DataModelSerializerFactory_Arrays_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Lists()
        {
            Run(testCase.DataModelSerializerFactory_Lists_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Enums()
        {
            Run(testCase.DataModelSerializerFactory_Enums_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Record()
        {
            Run(testCase.DataModelSerializerFactory_Record_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Anonymous()
        {
            Run(testCase.DataModelSerializerFactory_Anonymous_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Entity()
        {
            Run(testCase.DataModelSerializerFactory_Entity_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Tuples()
        {
            Run(testCase.DataModelSerializerFactory_Tuples_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_ArrayConversions()
        {
            Run(testCase.DataModelSerializerFactory_ArrayConversions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_EnumConversions()
        {
            Run(testCase.DataModelSerializerFactory_EnumConversions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralConversions()
        {
            Run(testCase.DataModelSerializerFactory_StructuralConversions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_Expressions()
        {
            Run(testCase.DataModelSerializerFactory_Expressions_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralTypeCycle()
        {
            Run(testCase.DataModelSerializerFactory_StructuralTypeCycle_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralTypeCycle_Anonymous()
        {
            Run(testCase.DataModelSerializerFactory_StructuralTypeCycle_Anonymous_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_ManOrBoy()
        {
            Run(testCase.DataModelSerializerFactory_ManOrBoy_Tests());
        }

        [TestMethod]
        public void DataModelSerializerFactory_IntArrayArray()
        {
            Run(testCase.DataModelSerializerFactory_IntArrayArray_Tests());
        }

        #endregion

        [TestMethod]
        public void DataModelSerializerFactory_ArgumentNull()
        {
            var serializer = new DataTypeBinarySerializer();
            var stream = new MemoryStream();
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Serialize(type: null, stream, value: null));
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Serialize(typeof(int), stream: null, value: null));
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Deserialize(type: null, stream));
            Assert.ThrowsException<ArgumentNullException>(() => serializer.Deserialize(typeof(int), stream: null));
        }

        [TestMethod]
        public void DataModelSerializerFactory_NotSupported()
        {
            var serializer = new DataTypeBinarySerializer();
            var stream = new MemoryStream();
            Assert.ThrowsException<NotSupportedException>(() => serializer.Serialize(typeof(NonDataModelType), stream, value: null));
            Assert.ThrowsException<NotSupportedException>(() => serializer.Serialize(typeof(Func<int>), stream, value: null));
            Assert.ThrowsException<NotSupportedException>(() => serializer.Deserialize(typeof(NonDataModelType), stream));
            Assert.ThrowsException<NotSupportedException>(() => serializer.Deserialize(typeof(Func<int>), stream));
        }

        private class NonDataModelType { }

        [TestMethod]
        public void DataModelSerializerFactory_StructuralWithCycles_NotSupported()
        {
            var inner = new DataModelSerializerFactoryTestCase.InnerCycleType();
            var outer = new DataModelSerializerFactoryTestCase.OuterCycleType { Inner = inner };
            inner.OuterArray = new[] { outer };

            var factory = new DataTypeBinarySerializer();
            using (var stream = new MemoryStream())
            {
                Assert.ThrowsException<InvalidOperationException>(() => factory.Serialize(typeof(DataModelSerializerFactoryTestCase.OuterCycleType), stream, outer));
            }

            inner.OuterArray = null;
            inner.Outer = outer;

            using (var stream = new MemoryStream())
            {
                Assert.ThrowsException<InvalidOperationException>(() => factory.Serialize(typeof(DataModelSerializerFactoryTestCase.OuterCycleType), stream, outer));
            }
        }

        [TestMethod]
        public void DataModelSerializerFactory_AdditiveSchema_Tuples()
        {
            var factory = new DataTypeBinarySerializer();
            var super = Tuple.Create(42, "qux", 0);
            var sub = Tuple.Create(42, "qux");
            var superType = typeof(Tuple<int, string, int>);
            var subType = typeof(Tuple<int, string>);

            var superDataType = (StructuralDataType)DataType.FromType(superType);
            var subDataType = (StructuralDataType)DataType.FromType(subType);
            var comparer = DataTypeObjectEqualityComparer.Default;

            // super-to-sub type
            var subRt = default(object);
            using (var stream = new MemoryStream())
            {
                factory.Serialize(superType, stream, super);
                stream.Position = 0;
                subRt = factory.Deserialize(subType, stream);
            }

            foreach (var subProperty in subDataType.Properties)
            {
                var superProperty = superDataType.Properties.Single(p => p.Name == subProperty.Name);
                Assert.IsTrue(comparer.Equals(subProperty.GetValue(subRt), superProperty.GetValue(super)));
            }

            // sub-to-super type
            var superRt = default(object);
            using (var stream = new MemoryStream())
            {
                factory.Serialize(subType, stream, sub);
                stream.Position = 0;
                superRt = factory.Deserialize(superType, stream);
            }

            foreach (var superProperty in superDataType.Properties)
            {
                var subProperty = subDataType.Properties.SingleOrDefault(p => p.Name == superProperty.Name);
                if (subProperty != null)
                {
                    Assert.IsTrue(comparer.Equals(subProperty.GetValue(sub), superProperty.GetValue(superRt)));
                }
                else
                {
                    Assert.AreEqual(Expression.Default(superProperty.Type.UnderlyingType).Evaluate(), superProperty.GetValue(superRt));
                }
            }
        }

        [TestMethod]
        public void DataModelSerializerFactory_AdditiveSchema_Entity()
        {
            var factory = new DataTypeBinarySerializer();
            var super = new SimpleDataType { Foo = 42, Bar = "baz", Qux = new[] { 1, 2, 3 } };

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

            var sub = new { Bar = "baz", Qux = new List<int> { 1, 2, 3 } };
            var superType = typeof(SimpleDataType);
            var subType = sub.GetType();

#pragma warning restore IDE0050

            var superDataType = (StructuralDataType)DataType.FromType(superType);
            var subDataType = (StructuralDataType)DataType.FromType(subType);
            var comparer = DataTypeObjectEqualityComparer.Default;

            // super-to-sub type
            var subRt = default(object);
            using (var stream = new MemoryStream())
            {
                factory.Serialize(superType, stream, super);
                stream.Position = 0;
                subRt = factory.Deserialize(subType, stream);
            }

            foreach (var subProperty in subDataType.Properties)
            {
                var superProperty = superDataType.Properties.Single(p => p.Name == subProperty.Name);
                Assert.IsTrue(comparer.Equals(subProperty.GetValue(subRt), superProperty.GetValue(super)));
            }

            // sub-to-super type
            var superRt = default(object);
            using (var stream = new MemoryStream())
            {
                factory.Serialize(subType, stream, sub);
                stream.Position = 0;
                superRt = factory.Deserialize(superType, stream);
            }

            foreach (var superProperty in superDataType.Properties)
            {
                var subProperty = subDataType.Properties.SingleOrDefault(p => p.Name == superProperty.Name);
                if (subProperty != null)
                {
                    Assert.IsTrue(comparer.Equals(subProperty.GetValue(sub), superProperty.GetValue(superRt)));
                }
                else
                {
                    Assert.AreEqual(Expression.Default(superProperty.Type.UnderlyingType).Evaluate(), superProperty.GetValue(superRt));
                }
            }
        }

        [TestMethod]
        public void BinarySerialization_Geocoordinate()
        {
            var value = new Geocoordinate
            {
                RequestId = "ab9ce968-b450-4156-a20b-f5610dc358f0",
                UserId = "BillG",
                UserIdHashcode = -1859715092,
                DeviceId = "0100E7F90900ED6B",
                DataCenter = "RED",
                UserSignal = new UserSignal
                {
                    Type = "com.contoso.device.geolocation.geocoordinate",
                    Timestamp = DateTime.Parse("02/24/2016 13:44:24 +00:00", CultureInfo.CreateSpecificCulture("en-US")),
                    ClientRequestid = "00000000-0000-0000-0000-000000000000",
                    AgentInstanceId = "FavoritePlace.Home.EveryExit",
                    Value = new GeocoordinateValue
                    {
                        Status = null,
                        Accuracy = "139",
                        Altitude = null,
                        AltitudeAccuracy = null,
                        Heading = null,
                        Latitude = 47.6393782,
                        Longtitude = -122.130448,
                        PositionSource = null,
                        Speed = null,
                    },
                },
            };

            var tests = new DataModelSerializerFactoryTestCase.Tests
            {
                {"", typeof(Geocoordinate), value, new DataModelSerializerFactoryTestCase.DeepComparer()}
            };

            Run(tests);
        }

#if !DEBUG && !NET5_0 // REVIEW: Some behavioral changes with finalization have been found in .NET 5.0 in Release builds.
        [TestMethod]
        public void DataModelSerializerFactory_GarbageCollectibleSerializerWithCycles()
        {
            var stream = new MemoryStream();

            {
                Do1(stream);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            Assert.AreEqual(1, Volatile.Read(ref MySerializer.Finalized));

            stream.Position = 0;

            {
                Do2(stream);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            Assert.AreEqual(2, Volatile.Read(ref MySerializer.Finalized));

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            static void Do1(MemoryStream stream)
            {
                var factory = new MySerializer();
                factory.Serialize(typeof(DataModelSerializerFactoryTestCase.OuterCycleType), stream, null);
            }

            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            static void Do2(MemoryStream stream)
            {
                var factory = new MySerializer();
                factory.Deserialize(typeof(DataModelSerializerFactoryTestCase.OuterCycleType), stream);
            }
        }
#endif

        public class Geocoordinate
        {
            [Mapping("contoso://entities/geocoordinatesignal/requestid")]
            public string RequestId { get; set; }

            [Mapping("contoso://entities/geocoordinatesignal/userid")]
            public string UserId { get; set; }

            [Mapping("contoso://entities/geocoordinatesignal/useridhashcode")]
            public int UserIdHashcode { get; set; }

            [Mapping("contoso://entities/geocoordinatesignal/deviceid")]
            public string DeviceId { get; set; }

            [Mapping("contoso://entities/geocoordinatesignal/datacenter")]
            public string DataCenter { get; set; }

            [Mapping("contoso://entities/geocoordinatesignal/usersignal")]
            public UserSignal UserSignal { get; set; }
        }

        public class UserSignal
        {
            [Mapping("contoso://entities/geocoordinateusersignal/type")]
            public string Type { get; set; }

            [Mapping("contoso://entities/geocoordinateusersignal/timestamp")]
            public DateTime Timestamp { get; set; }

            [Mapping("contoso://entities/geocoordinateusersignal/clientrequestid")]
            public string ClientRequestid { get; set; }

            [Mapping("contoso://entities/geocoordinateusersignal/agentinstanceid")]
            public string AgentInstanceId { get; set; }

            [Mapping("contoso://entities/geocoordinateusersignal/value")]
            public GeocoordinateValue Value { get; set; }
        }

        public class GeocoordinateValue
        {
            [Mapping("contoso://entities/geocoordinatevalue/status")]
            public string Status { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/accuracy")]
            public string Accuracy { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/altitude")]
            public string Altitude { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/altitudeaccuracy")]
            public string AltitudeAccuracy { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/heading")]
            public string Heading { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/latitude")]
            public double Latitude { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/longitude")]
            public double Longtitude { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/positionsource")]
            public string PositionSource { get; set; }

            [Mapping("contoso://entities/geocoordinatevalue/speed")]
            public string Speed { get; set; }
        }

        private sealed class MySerializer : DataTypeBinarySerializer
        {
            public static int Finalized/* = 0 */;

            ~MySerializer()
            {
                Interlocked.Increment(ref Finalized);
            }
        }

        private class SimpleDataType
        {
            [Mapping("Foo")]
            public int Foo { get; set; }


            [Mapping("Bar")]
            public string Bar { get; set; }


            [Mapping("Qux")]
            public int[] Qux { get; set; }
        }

        private static readonly MemoryStreamPool s_streamPool = MemoryStreamPool.Create(1024);

        private static void Run(DataModelSerializerFactoryTestCase.Tests tests)
        {
            var constantExpressionSerializer = new DataModelSerializerFactoryTestCase.ConstantExpressionSerializer();
            var factory = new DataTypeBinarySerializer(constantExpressionSerializer);
            constantExpressionSerializer.DataSerializer = factory;

            foreach (var test in tests)
            {
                using var holder = s_streamPool.New();

                factory.Serialize(test.InputType, holder.MemoryStream, test.Value);

                holder.MemoryStream.Position = 0;

                var rt = factory.Deserialize(test.OutputType, holder.MemoryStream);
                Assert.IsTrue(test.Comparer.Equals(test.Value, rt), "Expected: {0} Actual: {1}", test.Value, rt);
            }
        }

        private static void RunIsotopeTest(DataModelSerializerFactoryTestCase.IsotopeTestCase test, IEqualityComparer<object> comparer)
        {
            var constantExpressionSerializer = new DataModelSerializerFactoryTestCase.ConstantExpressionSerializer();
            var factory = new DataTypeBinarySerializer(constantExpressionSerializer);
            constantExpressionSerializer.DataSerializer = factory;

            foreach (var isotope in test)
            {
                using var holder = s_streamPool.New();

                factory.Serialize(isotope.Type, holder.MemoryStream, isotope.Value);

                foreach (var otherIsotope in test)
                {
                    holder.MemoryStream.Position = 0;
                    var rt = factory.Deserialize(otherIsotope.Type, holder.MemoryStream);
                    Assert.IsTrue(comparer.Equals(otherIsotope.Value, rt), "Expected: {0} Actual: {1}", otherIsotope.Value, otherIsotope);
                }
            }
        }
    }
}
