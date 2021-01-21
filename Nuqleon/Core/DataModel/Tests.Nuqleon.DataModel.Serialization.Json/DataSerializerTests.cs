// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// EK - June 2013
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using Nuqleon.DataModel.Serialization.Json;

using JsonExpression = Nuqleon.Json.Expressions.Expression;

namespace Nuqleon.DataModel.Serialization.JsonTest
{
    /// <summary>
    /// Tests for JSON data serializer.
    /// </summary>
    [TestClass]
    public partial class DataSerializerTests
    {
        /// <summary>
        /// The JSON serializer.
        /// </summary>
        private DataSerializer _jsonSerializer;

        /// <summary>
        /// Generic serialize method.
        /// </summary>
        private MethodInfo _genericSerialize;

        /// <summary>
        /// Generic deserialize method.
        /// </summary>
        private MethodInfo _genericDeserialize;

        /// <summary>
        /// Stream used for serialization.
        /// </summary>
        private MemoryStream _stream;

        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var helper = new SerializationHelper();
            _jsonSerializer = helper.DataSerializer;

            _stream = new MemoryStream();
            _genericSerialize = _jsonSerializer.GetType().GetMethod(nameof(DataSerializer.Serialize));
            _genericDeserialize = _jsonSerializer.GetType().GetMethod(nameof(DataSerializer.Deserialize));
        }

        /// <summary>
        /// Cleanups the test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _stream.Dispose();
        }

        /// <summary>
        /// Tests null stream to serializer serialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataSerialize_Serialize_NullArguments()
        {
            _jsonSerializer.Serialize<string>(value: null, serialized: null);
        }

        /// <summary>
        /// Tests null stream to serializer deserialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataSerialize_Deserialize_NullArguments()
        {
            _jsonSerializer.Deserialize<string>(serialized: null);
        }

        /// <summary>
        /// Tests garbage stream to serializer deserialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DataSerializerException))]
        public void DataSerialize_Deserialize_Bad_Stream()
        {
            var writer = new StreamWriter(_stream);
            writer.Write("!@#%"); writer.Flush();
            _jsonSerializer.Deserialize<int>(_stream);
        }

        /// <summary>
        /// Tests garbage stream to serializer deserialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DataSerializerException))]
        public void DataSerialize_Deserialize_Bad_TextReader()
        {
            (_jsonSerializer as JsonDataSerializer)?.DeserializeFrom<int>(new StringReader("!@#%"));
        }

        /// <summary>
        /// Tests garbage stream to serializer deserialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DataSerializerException))]
        public void DataSerialize_Deserialize_Bad_JsonReader()
        {
            (_jsonSerializer as JsonDataSerializer)?.DeserializeFrom<int>(new JsonTextReader(new StringReader("!@#%")));
        }

        /// <summary>
        /// Tests cyclic object passed to to serializer serialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DataSerializerException))]
        public void DataSerialize_Serialize_Cylic_Stream()
        {
            _jsonSerializer.Serialize(new Cycle(), _stream);
        }

        /// <summary>
        /// Tests cyclic object passed to to serializer serialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DataSerializerException))]
        public void DataSerialize_Serialize_Cylic_TextWriter()
        {
            (_jsonSerializer as JsonDataSerializer)?.SerializeTo(new Cycle(), new StringWriter());
        }

        /// <summary>
        /// Tests cyclic object passed to to serializer serialize method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DataSerializerException))]
        public void DataSerialize_Serialize_Cylic_JsonWriter()
        {
            (_jsonSerializer as JsonDataSerializer)?.SerializeTo(new Cycle(), new JsonTextWriter(new StringWriter()));
        }

        /// <summary>
        /// Tests serialization of primitive types.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestPrimitiveTypeSerializationDeserialization()
        {
            var input = new List<object>
                            {
                                true,
                                false,
                                'a',
                                byte.MaxValue,
                                byte.MinValue,
                                sbyte.MaxValue,
                                sbyte.MinValue,
                                short.MaxValue,
                                short.MinValue,
                                ushort.MaxValue,
                                ushort.MinValue,
                                int.MaxValue,
                                int.MinValue,
                                uint.MaxValue,
                                uint.MinValue,
                                long.MaxValue,
                                ulong.MinValue,
                                float.MaxValue,
                                float.MinValue,
                                float.NegativeInfinity,
                                float.PositiveInfinity,
                                double.MaxValue,
                                double.MinValue,
                                double.PositiveInfinity,
                                double.NegativeInfinity,
                                Guid.NewGuid(),
                                EnumByte.EnumValue1,
                                EnumInt.EnumValue2,
                                EnumLong.EnumValue1,
                                EnumSbyte.EnumValue2,
                                EnumShort.EnumValue1,
                                EnumUint.EnumValue2,
                                EnumUlong.EnumValue1,
                                EnumUshort.EnumValue2,
                                (EnumInt)13
                            };

            foreach (var expected in input)
            {
                _stream.SetLength(0);
                var serialization = _genericSerialize.MakeGenericMethod(new[] { expected.GetType() });
                serialization.Invoke(_jsonSerializer, new[] { expected, _stream });

                _stream.Position = 0;
                var deserialization = _genericDeserialize.MakeGenericMethod(new[] { expected.GetType() });
                var actual = deserialization.Invoke(_jsonSerializer, new object[] { _stream });

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Tests GUID serialization.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestGuid()
        {
            Guid expected = Guid.NewGuid();
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<Guid>(_stream));
        }

        /// <summary>
        /// Tests a random string.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestString()
        {
            const string Expected = "Test string";
            _jsonSerializer.Serialize(Expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(Expected, _jsonSerializer.Deserialize<string>(_stream));
        }

        /// <summary>
        /// Tests an empty string.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestEmptyString()
        {
            _jsonSerializer.Serialize(string.Empty, _stream);
            _stream.Position = 0;
            Assert.AreEqual(string.Empty, _jsonSerializer.Deserialize<string>(_stream));
        }

        /// <summary>
        /// Tests null string.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestNullString()
        {
            _jsonSerializer.Serialize((string)null, _stream);
            _stream.Position = 0;
            Assert.AreEqual(null, _jsonSerializer.Deserialize<string>(_stream));
        }

        /// <summary>
        /// Tests a uri serialization.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestUri()
        {
            var expected = new Uri("http://www.microsoft.com");
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<Uri>(_stream));
        }

        /// <summary>
        /// Tests URI serialization and invariance under normalization.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestUri_NormalizationBehavior()
        {
            var uris = new[]
            {
                new Uri("reactor://platform.bing.com"),                                 // (*) Trailing slash
                new Uri("reactor://platform.bing.com/"),
                new Uri("reactor://platform.bing.com/observables"),
                new Uri("reactor://platform.bing.com/observables/"),
                new Uri("reactor://platform.bing.com/observables/xs"),
                new Uri("reactor://platform.bing.com/observables/xs ys"),
                new Uri("reactor://platform.bing.com/observables/xs%20ys"),             // (*) Unescaping behavior
                new Uri("reactor://platform.bing.com/observables/xs?bar=foo&qux=baz"),
            };

            var neq = 0;

            foreach (var uri in uris)
            {
                var ms = new MemoryStream();
                _jsonSerializer.Serialize(uri, ms);
                ms.Position = 0;
                var res = _jsonSerializer.Deserialize<Uri>(ms);

                Assert.AreEqual(uri, res);

                // Note: this is the canonical representation used in Reactor / IRP Core
                Assert.AreEqual(uri.AbsoluteUri, res.AbsoluteUri);
                Assert.AreEqual(uri.ToString(), res.ToString());

                if (uri.OriginalString != res.OriginalString)
                {
                    neq++;
                }
            }

            Assert.AreEqual(0, neq); // See (*) annotations
        }

        /// <summary>
        /// Tests local date time.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestLocalDateTime()
        {
            var expected = DateTime.Now;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<DateTime>(_stream));
        }

        /// <summary>
        /// Tests UTC date time.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestUtcDateTime()
        {
            var expected = DateTime.UtcNow;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<DateTime>(_stream));
        }

        /// <summary>
        /// Tests local date time offset.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestDateTimeOffset()
        {
            DateTimeOffset expected = DateTime.Now;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<DateTimeOffset>(_stream));
        }

        /// <summary>
        /// Tests UTC date time offset.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestUtcDateTimeOffset()
        {
            DateTimeOffset expected = DateTime.UtcNow;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<DateTimeOffset>(_stream));
        }

        /// <summary>
        /// Tests timespan serialization.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestTimespan()
        {
            var expected = new TimeSpan(1, 3, 2, 3, 3);
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.AreEqual(expected, _jsonSerializer.Deserialize<TimeSpan>(_stream));
        }

        /// <summary>
        /// Tests the byte array.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestByteArray()
        {
            var expected = new byte[] { 1, 2, 3, 4, 5 };
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            Assert.IsTrue(expected.SequenceEqual(_jsonSerializer.Deserialize<byte[]>(_stream)));
        }

        /// <summary>
        /// Serialization of different but matching structural types.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestMatchingStructuralTypes()
        {
            var expected = new ClassWithByteEnumMember();
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ClassWithShortEnumMember>(_stream);

            Assert.AreEqual((int)expected.EnumField, (int)actual.EnumField);
        }

        /// <summary>
        /// Test of primitive null(able).
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestNullablePrimitive()
        {
            int? expected = 1;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<int?>(_stream);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests null.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestNullNullable()
        {
            int? expected = null;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<int?>(_stream);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests struct null(able).
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestNullableStruct()
        {
            DateTime? expected = DateTime.Now;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<DateTime?>(_stream);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testing a flat class.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestFlatClass()
        {
            var expected = FlatClass.Create();
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<FlatClass>(_stream);
            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Testing a flat class.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestFlatClass2()
        {
            var memoryStream = new MemoryStream(Convert.FromBase64String("77u/eyJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlYm9vbCI6ZmFsc2UsImNvbnRvc286Ly9lbnRpdGllcy9mbGF0L3R5cGVzaG9ydCI6LTE2MTM3LCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlaW50IjoxODM1ODExOTcwLCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlbnVsbGFibGVpbnQiOm51bGwsImNvbnRvc286Ly9lbnRpdGllcy9mbGF0L3R5cGVsb25nIjo3MTQ5Mzg3MjksImNvbnRvc286Ly9lbnRpdGllcy9mbGF0L3R5cGV1c2hvcnQiOjY1MjU4LCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBldWludCI6MTk3NDczMjA3NywiY29udG9zbzovL2VudGl0aWVzL2ZsYXQvdHlwZXVsb25nIjoxNTY5MzA0MDI5LCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlc3RyaW5nIjoiVGVzdDIwNzAzMzU1MzMiLCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlZmxvYXQiOjAuNTg1ODY2ODY4LCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlZG91YmxlIjowLjA2NzQ2ODcyNzAzODkyNTg1NSwiY29udG9zbzovL2VudGl0aWVzL2ZsYXQvdHlwZWRhdGV0aW1lIjoiMjAxNC0wNy0xOFQxMjozNDowNC43Mzg5ODcxLTA3OjAwIiwiY29udG9zbzovL2VudGl0aWVzL2ZsYXQvdHlwZXVyaSI6ImNvbnRvc286Ly9lbnRpdGllcy9mbGF0IiwiY29udG9zbzovL2VudGl0aWVzL2ZsYXQvdHlwZWRhdGV0aW1lb2Zmc2V0IjoiMjAxNC0wNy0xOFQxMjozNDowNC43Mzk5ODgyLTA3OjAwIiwiY29udG9zbzovL2VudGl0aWVzL2ZsYXQvdHlwZXRpbWVzcGFuIjoiMS4wMjowMzowNC4wMDUwMDAwIiwiY29udG9zbzovL2VudGl0aWVzL3BhcmVudC90eXBlZ3VpZCI6IjAxOTVjZjY4LTE0ZmMtNDkxYS1iNGE4LTlkZWQ1NDE5YTM4NiIsImNvbnRvc286Ly9lbnRpdGllcy9mbGF0L3R5cGVpZW51bWVyYWJsZWJvb2wiOlt0cnVlLGZhbHNlLHRydWVdLCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlYXJyYXlib29sIjpbZmFsc2UsdHJ1ZSxmYWxzZV0sImNvbnRvc286Ly9lbnRpdGllcy9mbGF0L3R5cGVlbnVtaW50IjowLCJjb250b3NvOi8vZW50aXRpZXMvZmxhdC90eXBlbnVsbGFibGVlbnVtaW50IjoxfQ=="));
            var actual = _jsonSerializer.Deserialize<FlatClass>(memoryStream);
            Assert.AreEqual(1835811970, actual.TypeInt);
            Assert.IsTrue(actual.TypeArrayBool[1]);
        }

        /// <summary>
        /// Testing a nested class.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestNestedClass()
        {
            var expected = NestedClass.Create();
            expected.NotSerializable = 13;

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<NestedClass>(_stream);

            Assert.IsTrue(expected.Equals(actual));
            Assert.AreNotEqual(expected.NotSerializable, actual.NotSerializable);
        }

        /// <summary>
        /// Testing an empty class.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestEmptyClass()
        {
            var expected = new EmptyClass { NotSerializable = 13 };

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<EmptyClass>(_stream);

            Assert.AreNotEqual(expected.NotSerializable, actual.NotSerializable);
        }

        /// <summary>
        /// Testing a recursive class.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestRecursiveClass()
        {
            var expected = RecursiveClass.Create();

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<RecursiveClass>(_stream);

            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Testing inheritance.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestInheritance()
        {
            var expected = InheritedClass.Create();

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<InheritedClass>(_stream);

            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Testing duplicate mapping.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DataSerializer_TestDuplicateMapping()
        {
            var expected = new DuplicateMappingClass();
            _jsonSerializer.Serialize(expected, _stream);
        }

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        /// <summary>
        /// Testing anonymous type.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestAnonymousClass()
        {
            var expected = new { CurrentTime = DateTime.UtcNow, Value = FlatClass.Create() };

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = Deserialize(expected, _jsonSerializer, _stream);

            Assert.AreEqual(expected.CurrentTime, actual.CurrentTime);
            Assert.IsTrue(expected.Value.Equals(actual.Value));
        }

#pragma warning restore IDE0050

        /// <summary>
        /// Testing record type.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestRecordClass()
        {
            var expected = new Person { Name = "Bart", Age = 21 };

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;

            var rec = RuntimeCompiler.CreateRecordType(new[] {
                new KeyValuePair<string, Type>("contoso://entities/person/name", typeof(string)),
                new KeyValuePair<string, Type>("contoso://entities/person/age", typeof(int)),
            }, valueEquality: true);

            var name = rec.GetProperty("contoso://entities/person/name");
            var age = rec.GetProperty("contoso://entities/person/age");

            var actual = _genericDeserialize.MakeGenericMethod(rec).Invoke(_jsonSerializer, new[] { _stream });

            Assert.AreEqual(expected.Name, name.GetValue(actual));
            Assert.AreEqual(expected.Age, age.GetValue(actual));
        }

        /// <summary>
        /// Testing tuple type.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestSimpleTuple()
        {
            var expected = new Tuple<int, DateTime, bool>(13, DateTime.Now, true);

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = Deserialize(expected, _jsonSerializer, _stream);

            Assert.AreEqual(expected, actual);
        }

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        /// <summary>
        /// Testing tuple to anonymous serialization.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestTupleToAnonymous()
        {
            var expected = new Tuple<int, DateTime, bool>(13, DateTime.Now, true);

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = Deserialize(
                new { Item1 = 0, Item2 = DateTime.Now, Item3 = false },
                _jsonSerializer,
                _stream);

            Assert.AreEqual(expected.Item1, actual.Item1);
            Assert.AreEqual(expected.Item2, actual.Item2);
            Assert.AreEqual(expected.Item3, actual.Item3);
        }

#pragma warning restore IDE0050

        /// <summary>
        /// Tests the custom tuple serialization.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestCustomTuple()
        {
            var expected = new TupleClass(3) { Property = 2 };

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = Deserialize(expected, _jsonSerializer, _stream);

            Assert.AreEqual(expected.Item1, actual.Item1);
            Assert.AreEqual(expected.Property, actual.Property);
        }

        /// <summary>
        /// Tests serialization of collections.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestCollections()
        {
            var expected = CollectionsClass.Create();

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<CollectionsClass>(_stream);

            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests serialization of an object to a dictionary.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestObjectToDictionary()
        {
            var expected = new ClassWithByteEnumMember { EnumField = EnumByte.EnumValue1 };

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<Dictionary<string, EnumByte>>(_stream);

            Assert.AreEqual(expected.EnumField, actual["contoso://entities/enumclass/typeEnum"]);
        }

        /// <summary>
        /// Tests serialization of a dictionary to an object.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestDictionaryToObject()
        {
            var expected = new Dictionary<string, EnumByte> { { "contoso://entities/enumclass/typeEnum", EnumByte.EnumValue1 } };

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ClassWithByteEnumMember>(_stream);

            Assert.AreEqual(expected["contoso://entities/enumclass/typeEnum"], actual.EnumField);
        }

        /// <summary>
        /// Tests the dictionary with a not supported key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DataSerializer_TestDictionaryWithNotSupportedKey()
        {
            var expected = new Dictionary<int, int> { { 1, 2 } };
            _jsonSerializer.Serialize(expected, _stream);
        }

        /// <summary>
        /// Tests the dictionary with mapping.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DataSerializer_TestDictionaryWithMapping()
        {
            var expected = new DictionaryWithMapping { { "whatever", 1 } };
            _jsonSerializer.Serialize(expected, _stream);
        }

        /// <summary>
        /// Tests the list with mapping.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DataSerializer_TestListWithMapping()
        {
            var expected = new ListWithMapping { "whatever" };
            _jsonSerializer.Serialize(expected, _stream);
        }

        /// <summary>
        /// Tests expression constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestExpressionConstant()
        {
            var expected = Expression.Constant(42);
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ConstantExpression>(_stream);
            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(expected.Type, actual.Type);
        }

        /// <summary>
        /// Tests expression constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestExpressionConstant_DateTimeOffset()
        {
            var dto = new DateTimeOffset(30, TimeSpan.Zero);
            var expected = Expression.Constant(dto);
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ConstantExpression>(_stream);
            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(dto.Offset, ((DateTimeOffset)actual.Value).Offset);
            Assert.AreEqual(expected.Type, actual.Type);
        }

        /// <summary>
        /// Tests expression within constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestConstantContainingExpression()
        {
            var expected = new ExpressionContainer { Expression = Expression.Constant(42) };
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ExpressionContainer>(_stream);
            Assert.AreEqual(expected.Expression.Value, actual.Expression.Value);
            Assert.AreEqual(expected.Expression.Type, actual.Expression.Type);
        }

        /// <summary>
        /// Tests null expression constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestNullExpressionConstant()
        {
            var expected = (Expression)null;
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<Expression>(_stream);
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Tests null expression within constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestConstantContainingNullExpression()
        {
            var expected = new ExpressionContainer();
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ExpressionContainer>(_stream);
            Assert.IsNull(actual.Expression);
        }

        /// <summary>
        /// Tests serialization of private fields.
        /// </summary>
        [TestMethod]
        public void DataSerializer_SerializationOfPrivateFields()
        {
            var expected = CheckpointState.Create();

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<CheckpointState>(_stream);

            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Tests serialization of private fields.
        /// </summary>
        [TestMethod]
        [Ignore, Description("Run this test in isolation, otherwise reflection cache will contain mapping from previous run without private serialization.")]
        public void DataSerializer_SerializationOfPrivateFieldsWithCustomSettings()
        {
            var expected = CheckpointState.Create();
            var helper = new SerializationHelper(includePrivate: true);
            var serializer = helper.DataSerializer;

            serializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = serializer.Deserialize<CheckpointState>(_stream);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testing serialization of inherited private fields/properties.
        /// </summary>
        [TestMethod]
        public void DataSerializer_SerializationOfInheritedPrivateFields()
        {
            var expected = InheritedCheckpointState.Create();
            var helper = new SerializationHelper(includePrivate: true);
            var serializer = helper.DataSerializer;

            serializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = serializer.Deserialize<InheritedCheckpointState>(_stream);

            Assert.IsTrue(expected.Equals(actual));
        }

        /// <summary>
        /// Tests complex expression within constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestExpressionManOrBoy1()
        {
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping in expression tree.)
            var expr = (Expression<Func<IEnumerable<string>, IEnumerable<int>>>)(xs => from x in xs where x.Length > 17 select Math.Abs((int)x[0]));
#pragma warning restore IDE0004

            var expected = new ExpressionContainer2 { Expression = expr };
            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ExpressionContainer2>(_stream);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(expected.Expression, actual.Expression));
        }

        /// <summary>
        /// Tests complex expression with constant.
        /// </summary>
        [TestMethod]
        public void DataSerializer_TestExpressionManOrBoy2()
        {
            var expr = (Expression<Func<ExpressionContainer>>)(() => new ExpressionContainer { Expression = Expression.Constant("foo") });
            var c2 = new ExpressionContainer2 { Expression = expr, Expression2 = Expression.Constant("foo2") };
            var expected = Expression.Constant(c2);

            _jsonSerializer.Serialize(expected, _stream);
            _stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<ConstantExpression>(_stream);

            Assert.IsNotNull(actual);
            Assert.AreNotSame(expected, actual);
            Assert.AreNotSame(expected.Value, actual.Value);

            var actualC2 = (actual.Value as ExpressionContainer2);

            Assert.IsNotNull(actualC2);
            Assert.AreNotSame(c2, actualC2);

            Assert.AreEqual("foo2", actualC2.Expression2.Value);

            var actualExpr = (actualC2.Expression as Expression<Func<ExpressionContainer>>);

            Assert.IsNotNull(actualExpr);
            Assert.AreNotSame(expr, actualExpr);

            var expectedContainer = expr.Compile()();
            var actualContainer = actualExpr.Compile()();

            Assert.AreNotSame(expectedContainer, actualContainer);

            Assert.AreEqual("foo", actualContainer.Expression.Value);
        }

        [TestMethod]
        [Ignore, Description("We have backed out this feature for the meantime.")]
        public void DataSerializer_ExpressionMergedContextTest()
        {
            var expr = (Expression<Func<ExpressionContainer>>)(() => new ExpressionContainer { Expression = Expression.Constant("foo") });
            var c2 = new ExpressionContainer2 { Expression = expr, Expression2 = Expression.Constant("foo2") };
            var expected = Expression.Constant(c2);

            var helper = new SerializationHelper();

            var memoryStream = new MemoryStream();
            helper.DataSerializer.Serialize(expected, memoryStream);
            var reader = new StreamReader(memoryStream);
            memoryStream.Position = 0;
            var json = reader.ReadToEnd();
            Assert.IsTrue(json.Split(new[] { "\"Context\"" }, 10, StringSplitOptions.None).Length == 2);

            memoryStream.Position = 0;

            var actual = helper.DataSerializer.Deserialize<ConstantExpression>(memoryStream);

            Assert.IsNotNull(actual);
            Assert.AreNotSame(expected, actual);
            Assert.AreNotSame(expected.Value, actual.Value);

            var actualC2 = (actual.Value as ExpressionContainer2);

            Assert.IsNotNull(actualC2);
            Assert.AreNotSame(c2, actualC2);

            Assert.AreEqual("foo2", actualC2.Expression2.Value);

            var actualExpr = (actualC2.Expression as Expression<Func<ExpressionContainer>>);

            Assert.IsNotNull(actualExpr);
            Assert.AreNotSame(expr, actualExpr);

            var expectedContainer = expr.Compile()();
            var actualContainer = actualExpr.Compile()();

            Assert.AreNotSame(expectedContainer, actualContainer);

            Assert.AreEqual("foo", actualContainer.Expression.Value);
        }

        [TestMethod]
        public void DataSerializer_BasicCustomConverter()
        {
            var foo = new Foo { Bar = new Bar { X = 42 } };

            var ser = DataSerializer.Create(new BonsaiExpressionSerializer(_ => x => null, _ => x => null, new Version(0, 8)), new BarConv());

            var ms = new MemoryStream();
            ser.Serialize(foo, ms);
            ms.Position = 0;

            var foo2 = ser.Deserialize<Foo>(ms);

            Assert.AreEqual((42 + 1) * 2, foo2.Bar.X);
        }

        [TestMethod]
        public void DataSerializer_QuotedCustomConverter_Simple1()
        {
            var ser = new SerializationHelper(new QuoteConv());

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<Qum<int>>(new Qum<int>(Expression.Constant(42)), ms);
            ms.Position = 0;

            var q = ser.DataSerializer.Deserialize<IQum<int>>(ms);
            Assert.AreEqual(42, q.Eval());
        }

        [TestMethod]
        public void DataSerializer_QuotedCustomConverter_Simple2()
        {
            var ser = new SerializationHelper(new QuoteConv());

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<IQum<int>>(new Qum<int>(Expression.Constant(42)), ms);
            ms.Position = 0;

            var q = ser.DataSerializer.Deserialize<INum<int>>(ms);
            Assert.AreEqual(42, q.Eval());
        }

        [TestMethod]
        public void DataSerializer_QuotedCustomConverter_Null()
        {
            var ser = new SerializationHelper(new QuoteConv());

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<Qum<int>>(value: null, ms);
            ms.Position = 0;

            var q = ser.DataSerializer.Deserialize<INum<int>>(ms);
            Assert.IsNull(q);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DataSerializer_QuotedCustomConverter_Fail()
        {
            var ser = new SerializationHelper(new QuoteConv());

            var ms = new MemoryStream();

            ser.DataSerializer.Serialize<Num<int>>(new Num<int>(42), ms);
        }

        [TestMethod]
        public void DataSerializer_ExpressionToExpression()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr1
            {
                Expression = Expression.Constant(42)
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr1>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr1>(ms);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(e.Expression, f.Expression));
        }

        [TestMethod]
        public void DataSerializer_ExpressionToExpression_Null()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr1
            {
                Expression = null
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr1>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr1>(ms);

            Assert.IsNull(f.Expression);
        }

        [TestMethod]
        public void DataSerializer_ExpressionToExpressionSlim()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr1
            {
                Expression = Expression.Constant(42)
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr1>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr2>(ms);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(e.Expression, f.Expression.ToExpression()));
        }

        [TestMethod]
        public void DataSerializer_ExpressionToExpressionSlim_Null()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr1
            {
                Expression = null
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr1>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr2>(ms);

            Assert.IsNull(f.Expression);
        }

        [TestMethod]
        public void DataSerializer_ExpressionSlimToExpression()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr2
            {
                Expression = Expression.Constant(42).ToExpressionSlim()
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr2>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr1>(ms);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(e.Expression.ToExpression(), f.Expression));
        }

        [TestMethod]
        public void DataSerializer_ExpressionSlimToExpression_Null()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr2
            {
                Expression = null
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr2>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr1>(ms);

            Assert.IsNull(f.Expression);
        }

        [TestMethod]
        public void DataSerializer_ExpressionSlimToExpressionSlim()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr2
            {
                Expression = Expression.Constant(42).ToExpressionSlim()
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr2>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr2>(ms);

            Assert.IsTrue(new ExpressionEqualityComparer().Equals(e.Expression.ToExpression(), f.Expression.ToExpression()));
        }

        [TestMethod]
        public void DataSerializer_ExpressionSlimToExpressionSlim_Null()
        {
            var ser = new SerializationHelper();

            var e = new MyExpr2
            {
                Expression = null
            };

            var ms = new MemoryStream();
            ser.DataSerializer.Serialize<MyExpr2>(e, ms);
            ms.Position = 0;

            var f = ser.DataSerializer.Deserialize<MyExpr2>(ms);

            Assert.IsNull(f.Expression);
        }

        private class MyExpr1
        {
            [Mapping("expr")]
            public Expression Expression { get; set; }
        }

        private class MyExpr2
        {
            [Mapping("expr")]
            public ExpressionSlim Expression { get; set; }
        }

        private class QuoteConv : DataConverter
        {
            public override bool TryCanConvert(Type fromType, out Type toType)
            {
                if (fromType.FindGenericType(typeof(INum<>)) != null)
                {
                    toType = typeof(Expression);
                    return true;
                }

                toType = null;
                return false;
            }

            public override object ConvertTo(object value, Type sourceType, Type targetType)
            {
                Assert.AreEqual(typeof(Expression), targetType);

                if (value == null)
                {
                    return null;
                }

                if (value.GetType().FindGenericType(typeof(IQum<>)) == null)
                {
                    throw new NotSupportedException("Unquoted object.");
                }

                return ((IQum)value).Expression;
            }

            public override object ConvertFrom(object value, Type sourceType, Type targetType)
            {
                var expr = (Expression)value;

                if (expr == null)
                {
                    return null;
                }

                var obs = targetType.FindGenericType(typeof(INum<>));
                var elem = obs.GetGenericArguments()[0];

                var type = typeof(Qum<>).MakeGenericType(elem);
                return Activator.CreateInstance(type, new object[] { expr });
            }
        }

        private interface INum<T>
        {
            T Eval();
        }

        private class Num<T> : INum<T>
        {
            private readonly T _value;

            public Num(T value) => _value = value;

            public T Eval() => _value;
        }


        private interface IQum
        {
            Expression Expression { get; }
        }

        private interface IQum<T> : INum<T>, IQum
        {
        }

        private class Qum<T> : IQum<T>
        {
            public Qum(Expression expression) => Expression = expression;

            public T Eval() => Expression.Evaluate<T>();

            public Expression Expression { get; }
        }

        private class BarConv : DataConverter
        {
            public override bool TryCanConvert(Type fromType, out Type toType)
            {
                if (typeof(IBar).IsAssignableFrom(fromType))
                {
                    toType = typeof(BarHolder);
                    return true;
                }

                toType = null;
                return false;
            }

            public override object ConvertTo(object value, Type sourceType, Type targetType)
            {
                Assert.AreEqual(typeof(Bar), sourceType);
                Assert.AreEqual(typeof(BarHolder), targetType);
                return new BarHolder { BarX = ((IBar)value).X + 1 };
            }

            public override object ConvertFrom(object value, Type sourceType, Type targetType)
            {
                Assert.AreEqual(typeof(BarHolder), sourceType);
                Assert.AreEqual(typeof(IBar), targetType);
                return new Bar { X = ((BarHolder)value).BarX * 2 };
            }
        }

        private class BarHolder
        {
            [Mapping("X")]
            public int BarX { get; set; }
        }

        private class Foo
        {
            [Mapping("bar")]
            public IBar Bar { get; set; }
        }

        private interface IBar
        {
            int X { get; set; }
        }

        private class Bar : IBar
        {
            public int X { get; set; }
        }

        private class Cycle
        {
            [Mapping("it")]
            public Cycle Loop => this;
        }

        /// <summary>
        /// Helper method for de-serializing anonymous types without reflection.
        /// </summary>
        /// <typeparam name="T">Type of de-serialized object.</typeparam>
        /// <param name="_">An instance of an anonymous type.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Deserialized object.</returns>
        private static T Deserialize<T>(T _, DataSerializer serializer, Stream stream)
        {
            return serializer.Deserialize<T>(stream);
        }

        /// <summary>
        /// Helper method to ease debugging. Reads the string from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Stream as string.</returns>
        public static string ReadStringFromStream(Stream stream)
        {
            stream.Position = 0;

            using var sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }

        #region Expression serialization helpers

        private class SerializationHelper
        {
            private readonly MethodInfo genericSerialize;
            private readonly MethodInfo genericDeserialize;

            private readonly BonsaiExpressionSerializer bonsaiSerializer;

            public SerializationHelper(bool includePrivate)
            {
                bonsaiSerializer = new DataModelBonsaiExpressionSerializer(SerializeConstantFactory, DeserializeConstantFactory);
#pragma warning disable 618
                DataSerializer = DataSerializer.Create(bonsaiSerializer, includePrivate);
#pragma warning restore 618
                genericSerialize = DataSerializer.GetType().GetMethod("Serialize");
                genericDeserialize = DataSerializer.GetType().GetMethod("Deserialize");
            }

            public SerializationHelper(params DataConverter[] converters)
            {
                bonsaiSerializer = new DataModelBonsaiExpressionSerializer(SerializeConstantFactory, DeserializeConstantFactory);
                DataSerializer = DataSerializer.Create(bonsaiSerializer, converters);
                genericSerialize = DataSerializer.GetType().GetMethod("Serialize");
                genericDeserialize = DataSerializer.GetType().GetMethod("Deserialize");
            }

            public DataSerializer DataSerializer { get; }

            public IExpressionSerializer ExpressionSerializer => bonsaiSerializer;

            public Func<object, JsonExpression> SerializeConstantFactory(Type type)
            {
                return obj => SerializeConstant(obj, type);
            }

            private JsonExpression SerializeConstant(object obj, Type type)
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);

                var serialize = genericSerialize.MakeGenericMethod(new[] { type });
                serialize.Invoke(DataSerializer, new[] { obj, stream });

                stream.Position = 0;

                return JsonExpression.Parse(reader.ReadToEnd(), ensureTopLevelObjectOrArray: false);
            }

            public Func<JsonExpression, object> DeserializeConstantFactory(Type type)
            {
                return json => DeserializeConstant(json, type);
            }

            private object DeserializeConstant(JsonExpression obj, Type type)
            {
                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);

                var deserialize = genericDeserialize.MakeGenericMethod(new[] { type });

                writer.Write(obj.ToString());
                writer.Flush();

                stream.Position = 0;

                return deserialize.Invoke(DataSerializer, new[] { stream });
            }

            private class DataModelInvertedTypeSpace : InvertedTypeSpace
            {
                protected override PropertyInfo GetPropertyCore(PropertyInfoSlim propertySlim, Type declaringType, Type propertyType, Type[] indexParameterTypes)
                {
                    if (propertySlim.DeclaringType.Kind == TypeSlimKind.Structural)
                    {
                        return declaringType.GetProperties().Single(prop => prop.GetCustomAttribute<MappingAttribute>(inherit: false).Uri == propertySlim.Name || prop.Name == propertySlim.Name);
                    }

                    return base.GetPropertyCore(propertySlim, declaringType, propertyType, indexParameterTypes);
                }
            }

            private class DataModelBonsaiExpressionSerializer : BonsaiExpressionSerializer
            {
                public DataModelBonsaiExpressionSerializer(Func<Type, Func<object, JsonExpression>> liftFactory, Func<Type, Func<JsonExpression, object>> reduceFactory)
                    : base(liftFactory, reduceFactory)
                {
                }

                public override Expression Reduce(ExpressionSlim expression)
                {
                    return new ExpressionSlimToExpressionConverter(new DataModelInvertedTypeSpace()).Visit(expression);
                }
            }
        }

        #endregion
    }
}
