// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using Nuqleon.Json.Expressions;
using Nuqleon.Json.Interop.Newtonsoft;

namespace Tests.Nuqleon.Json.Interop.Newtonsoft
{
    [TestClass]
    public class JsonExpressionWriterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JsonExpressionWriter_ArgumentChecking()
        {
            new JsonExpressionWriter(pool: null);
        }

        [TestMethod]
        public void JsonExpressionWriter_Basics()
        {
            var writer = new JsonExpressionWriter();

            writer.WriteValue(42);

            writer.Flush();

            var expr = writer.Expression;
            var constant = expr as ConstantExpression;
            Assert.IsNotNull(constant);
            Assert.AreEqual(ExpressionType.Number, constant.NodeType);
            Assert.AreEqual("42", (string)constant.Value);

            ((IDisposable)writer).Dispose();

            Assert.ThrowsException<ObjectDisposedException>(() => writer.WriteValue(43));
            Assert.ThrowsException<ObjectDisposedException>(() => writer.Expression);
        }

        [TestMethod]
        public void JsonExpressionWriter_NoInput()
        {
            var writer = new JsonExpressionWriter();

            Assert.ThrowsException<InvalidOperationException>(() => writer.Expression);
        }

        [TestMethod]
        public void JsonExpressionWriter_InvalidInput()
        {
            var writer = new JsonExpressionWriter();

            writer.WriteValue(1);
            writer.WriteValue(2);

            Assert.ThrowsException<InvalidOperationException>(() => writer.Expression);
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Null()
        {
            Roundtrip(new object[] {
                null
            });
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Boolean()
        {
            Roundtrip(
                false,
                true
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Byte()
        {
            Roundtrip(
(object)(byte)0,
                (byte)1,
                (byte)9,
                (byte)90,
                (byte)(byte.MaxValue - 2),
                (byte)(byte.MaxValue - 1),
                byte.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_SByte()
        {
            Roundtrip(
                sbyte.MinValue,
                (sbyte)(sbyte.MinValue + 1),
                (sbyte)(sbyte.MinValue + 2),
                (sbyte)-90,
                (sbyte)-9,
                (sbyte)0,
                (sbyte)1,
                (sbyte)9,
                (sbyte)90,
                (sbyte)(sbyte.MaxValue - 2),
                (sbyte)(sbyte.MaxValue - 1),
                sbyte.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Int16()
        {
            Roundtrip(
                short.MinValue,
                (short)(short.MinValue + 1),
                (short)(short.MinValue + 2),
                (short)-7890,
                (short)-890,
                (short)-90,
                (short)-9,
                (short)-1,
                (short)0,
                (short)1,
                (short)9,
                (short)90,
                (short)890,
                (short)7890,
                (short)(short.MaxValue - 2),
                (short)(short.MaxValue - 1),
                short.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_UInt16()
        {
            Roundtrip(
                (ushort)1,
                (ushort)9,
                (ushort)90,
                (ushort)890,
                (ushort)7890,
                (ushort)(ushort.MaxValue - 2),
                (ushort)(ushort.MaxValue - 1),
                ushort.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Int32()
        {
            Roundtrip(
                int.MinValue,
                int.MinValue + 1,
                int.MinValue + 2,
                -1234567890,
                -234567890,
                -34567890,
                -4567890,
                -567890,
                -67890,
                -7890,
                -890,
                -90,
                -9,
                -1,
                0,
                1,
                9,
                90,
                890,
                7890,
                67890,
                567890,
                4567890,
                34567890,
                234567890,
                1234567890,
                int.MaxValue - 2,
                int.MaxValue - 1,
                int.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_UInt32()
        {
            Roundtrip(
(object)(uint)0,
                (uint)1,
                (uint)9,
                (uint)90,
                (uint)890,
                (uint)7890,
                (uint)67890,
                (uint)567890,
                (uint)4567890,
                (uint)34567890,
                (uint)234567890,
                (uint)1234567890,
                uint.MaxValue - 2,
                uint.MaxValue - 1,
                uint.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Int64()
        {
            Roundtrip(
                long.MinValue,
                long.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_UInt64()
        {
            Roundtrip(
(object)ulong.MinValue,
                ulong.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_DateTime()
        {
            Roundtrip(
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Unspecified),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Local),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc),
                new DateTime(1983, 2, 11, 3, 14, 15, 9, DateTimeKind.Utc),
                new DateTime(1983, 2, 11, 3, 14, 15, 98, DateTimeKind.Utc),
                new DateTime(1983, 2, 11, 3, 14, 15, 987, DateTimeKind.Utc),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(1),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(12),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(123),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(1234),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(12345),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(123456),
                new DateTime(1983, 2, 11, 3, 14, 15, DateTimeKind.Utc).AddTicks(1234567)
            );

            var w = new JsonExpressionWriter() { DateFormatString = "dd/MM/yyyy" };
            w.WriteValue(new DateTime(1983, 2, 11, 3, 14, 15));
            w.Flush();
            Assert.AreEqual("\"11/02/1983\"", w.Expression.ToString());
        }

        [TestMethod]
        public void JsonExpressionWriter_DateTime_TimeZoneHandling()
        {
            foreach (var d in new[]
            {
                new DateTime(2017, 3, 14, 9, 32, 56, DateTimeKind.Unspecified),
                new DateTime(2017, 3, 14, 9, 32, 56, DateTimeKind.Local),
                new DateTime(2017, 3, 14, 9, 32, 56, DateTimeKind.Utc),
            })
            {
                foreach (var h in new[]
                {
                    DateTimeZoneHandling.Local,
                    DateTimeZoneHandling.RoundtripKind,
                    DateTimeZoneHandling.Unspecified,
                    DateTimeZoneHandling.Utc
                })
                {
                    Roundtrip(h, d);
                }
            }
        }

        [TestMethod]
        public void JsonExpressionWriter_DateTimeOffset()
        {
            Roundtrip(
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(0)),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(1)),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(-1)),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, 9, new TimeSpan(2, 30, 0)),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, 98, new TimeSpan(-8, 30, 0)),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, 987, TimeSpan.FromHours(5)),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(1)).AddTicks(1),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(-1)).AddTicks(12),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(2)).AddTicks(123),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(-2)).AddTicks(1234),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(9)).AddTicks(12345),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(-10)).AddTicks(123456),
                new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.FromHours(11)).AddTicks(1234567)
            );

            var w = new JsonExpressionWriter() { DateFormatString = "dd/MM/yyyy" };
            w.WriteValue(new DateTimeOffset(1983, 2, 11, 3, 14, 15, TimeSpan.Zero));
            w.Flush();
            Assert.AreEqual("\"11/02/1983\"", w.Expression.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(JsonWriterException))]
        public void JsonExpressionWriter_Primitives_BigInteger()
        {
            Roundtrip(
                (BigInteger)long.MaxValue + 1
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Float()
        {
            Roundtrip(
                -2.25f,
                -1.0f,
                -0.5f,
                0.0f,
                0.5f,
                1.0f,
                2.25f,
                (float)Math.PI,
                (float)Math.E
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Double()
        {
            Roundtrip(
                -2.25,
                -1.0,
                -0.5,
                0.0,
                0.5,
                1.0,
                2.25,
                Math.PI,
                Math.E
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Decimal()
        {
            Roundtrip(
                FloatParseHandling.Decimal,
                49.95m
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_NullableFloat()
        {
            Roundtrip(
                new { a = (float?)null },
                new { a = (float?)0.0 },
                new { a = (float?)1.0 }
            );

            var w = new JsonExpressionWriter();
            w.WriteValue((float?)null);
            w.Flush();
            Assert.AreEqual("null", w.Expression.ToString());
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_NullableDouble()
        {
            Roundtrip(
                new { a = (double?)null },
                new { a = (double?)0.0 },
                new { a = (double?)1.0 }
            );

            var w = new JsonExpressionWriter();
            w.WriteValue((double?)null);
            w.Flush();
            Assert.AreEqual("null", w.Expression.ToString());
        }

        [TestMethod]
        public void JsonExpressionWriter_Primitives_Char()
        {
            Roundtrip(
                ' ',
                'a',
                'z',
                ' ',
                '\t',
                '\r'
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_String()
        {
            Roundtrip(new object[] {
                null,
                " ",
                "bar",
                "I am Bart",
                "Hello \"Bart\"!",
                "Some\tweird\r\ncharacters..."
            });

            var w = new JsonExpressionWriter();
            w.WriteValue((string)null);
            w.Flush();
            Assert.AreEqual("null", w.Expression.ToString());
        }

        [TestMethod]
        public void JsonExpressionWriter_Guid()
        {
            Roundtrip(
                Guid.Empty,
                Guid.NewGuid()
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_TimeSpan()
        {
            Roundtrip(
                TimeSpan.Zero,
                new TimeSpan(1L),
                new TimeSpan(12L),
                new TimeSpan(123L),
                new TimeSpan(1234L),
                new TimeSpan(12345L),
                new TimeSpan(3, 14, 15),
                -new TimeSpan(3, 14, 15)
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Uri()
        {
            Roundtrip(
                new Uri("http://www.bing.com"),
                new Uri("http://www.bing.com/"),
                new Uri("http://www.bing.com/bar"),
                new Uri("http://www.bing.com/bar?qux=foo"),
                new Uri("http://www.bing.com/bar?qux=foo&baz=42")
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_Arrays()
        {
            var rand = new Random(1983);
            var strs = new[] { null, "", " ", "\t", "bar", "foo", "qux", "baz", "foobar", "quux", "corge", "grault", "garply", "waldo", "fred", "plugh", "xyzzy", "thud" };

            var int32s = Enumerable.Range(0, 100).Select(i => (object)Enumerable.Range(0, i).ToArray()).ToArray();
            var strings = Enumerable.Range(0, 100).Select(i => (object)Enumerable.Range(0, i).Select(j => strs[rand.Next(0, strs.Length)]).ToArray()).ToArray();

            Roundtrip(int32s);
            Roundtrip(strings);
        }

        [TestMethod]
        public void JsonExpressionWriter_Objects()
        {
            Roundtrip(
                new Person(),
                new Person() { Name = "Bart" },
                new Person() { Name = "Bart", Age = 21 },
                new Person() { Name = "Bart", Age = 21, City = new City { Zip = "98004" } }
            );
        }

        [TestMethod]
        public void JsonExpressionWriter_BigObjects()
        {
            Roundtrip(Big8.Values);
        }

        [TestMethod]
        public void JsonExpressionWriter_NaN_Float()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = float.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":0.0}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = float.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"NaN"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = float.NaN }));
        }

        [TestMethod]
        public void JsonExpressionWriter_NaN_Float_Nullable()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = (float?)float.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":null}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = (float?)float.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"NaN"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = (float?)float.NaN }));
        }

        [TestMethod]
        public void JsonExpressionWriter_NaN_Double()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = double.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":0.0}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = double.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"NaN"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = double.NaN }));
        }

        [TestMethod]
        public void JsonExpressionWriter_NaN_Double_Nullable()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = (double?)double.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":null}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = (double?)double.NaN }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"NaN"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = (double?)double.NaN }));
        }

        [TestMethod]
        public void JsonExpressionWriter_Infinity_Float()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = float.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":0.0}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = float.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"Infinity"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = float.PositiveInfinity }));
        }

        [TestMethod]
        public void JsonExpressionWriter_Infinity_Float_Nullable()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = (float?)float.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":null}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = (float?)float.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"Infinity"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = (float?)float.PositiveInfinity }));
        }

        [TestMethod]
        public void JsonExpressionWriter_Infinity_Double()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = double.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":0.0}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = double.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"Infinity"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = double.PositiveInfinity }));
        }

        [TestMethod]
        public void JsonExpressionWriter_Infinity_Double_Nullable()
        {
            var jsonDefault = Serialize(FloatFormatHandling.DefaultValue, new { a = (double?)double.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":null}""", jsonDefault);

            var jsonString = Serialize(FloatFormatHandling.String, new { a = (double?)double.PositiveInfinity }).ToString();
            Assert.AreEqual(/*lang=json,strict*/ """{"a":"Infinity"}""", jsonString);

            Assert.ThrowsException<NotSupportedException>(() => Serialize(FloatFormatHandling.Symbol, new { a = (double?)double.PositiveInfinity }));
        }

        [TestMethod]
        public void JsonExpressionWriter_Concurrent()
        {
            var pool = new JsonInteropResourcePool();

            var tasks = Enumerable.Range(0, 32).Select(i => Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < 1000)
                {
                    Roundtrip(() => new JsonExpressionWriter(pool), Big8.Values);
                }
            })).ToArray();

            Task.WaitAll(tasks);
        }

        [TestMethod]
        public void JsonExpressionWriter_NotSupported()
        {
            AssertNotSupported(w => w.WriteValue(Array.Empty<byte>()));
            AssertNotSupported(w => w.WriteComment("bar"));
            AssertNotSupported(w => w.WriteStartConstructor("bar"));
            AssertNotSupported(w => w.WriteEndConstructor());
            AssertNotSupported(w => w.WriteUndefined());

            Assert.ThrowsException<NotSupportedException>(() =>
            {
                var w = new JsonExpressionWriter() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
                w.WriteValue(DateTime.Now);
            });

            Assert.ThrowsException<NotSupportedException>(() =>
            {
                var w = new JsonExpressionWriter() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
                w.WriteValue(DateTimeOffset.Now);
            });
        }

        [TestMethod]
        public void JsonExpressionWriter_InvalidOperation_EndArray()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var w = new JsonExpressionWriter();
                w.WriteEndArray();
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var w = new JsonExpressionWriter();
                w.WriteValue(1);
                w.WriteEndArray();
            });
        }

        [TestMethod]
        public void JsonExpressionWriter_InvalidOperation_EndArray_NoValidElement()
        {
            foreach (var write in new Action<JsonExpressionWriter>[]
            {
                w => w.WritePropertyName("foo"),
                w => w.WriteStartObject(),
            })
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    var w = new JsonExpressionWriter();
                    w.WriteStartArray();
                    write(w);
                    w.WriteEndArray();
                });
            }
        }

        [TestMethod]
        public void JsonExpressionWriter_InvalidOperation_EndObject()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var w = new JsonExpressionWriter();
                w.WriteEndObject();
            });

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var w = new JsonExpressionWriter();
                w.WriteValue(1);
                w.WriteEndObject();
            });
        }

        [TestMethod]
        public void JsonExpressionWriter_InvalidOperation_EndObject_NoValidProperty()
        {
            foreach (var write in new Action<JsonExpressionWriter>[]
            {
                w => w.WriteStartArray(),
                w => w.WriteValue(1),
            })
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    var w = new JsonExpressionWriter();
                    w.WriteStartObject();
                    write(w);
                    w.WriteValue(1);
                    w.WriteEndObject();
                });
            }
        }

        [TestMethod]
        public void JsonExpressionWriter_InvalidOperation_EndObject_NoValidExpression()
        {
            foreach (var write in new Action<JsonExpressionWriter>[]
            {
                w => w.WritePropertyName("bar"),
                w => w.WriteStartArray(),
            })
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    var w = new JsonExpressionWriter();
                    w.WriteStartObject();
                    w.WritePropertyName("foo");
                    write(w);
                    w.WriteEndObject();
                });
            }
        }

        private static void Roundtrip(params object[] objs) => Roundtrip(() => new JsonExpressionWriter(), objs);

        private static void Roundtrip(Func<JsonExpressionWriter> createWriter, params object[] objs) => Roundtrip(createWriter, () => new JsonSerializer(), objs);

        private static void Roundtrip(DateTimeZoneHandling dtzh, params object[] objs) => Roundtrip(() => new JsonExpressionWriter() { DateTimeZoneHandling = dtzh }, () => new JsonSerializer() { DateTimeZoneHandling = dtzh }, objs);

        private static void Roundtrip(Func<JsonExpressionWriter> createWriter, Func<JsonSerializer> createSerializer, params object[] objs)
        {
            var ser = createSerializer();

            foreach (var obj in objs)
            {
                var sw = new StringWriter();
                ser.Serialize(sw, obj);
                var json = sw.ToString();

                using var writer = createWriter();

                ser.Serialize(writer, obj);
                writer.Flush();

                var newJson = writer.Expression.ToString();

                Assert.AreEqual(json, newJson);
            }
        }

        private static Expression Serialize(FloatFormatHandling ffh, object obj)
        {
            var ser = new JsonSerializer();

            using var writer = new JsonExpressionWriter() { FloatFormatHandling = ffh };

            ser.Serialize(writer, obj);
            writer.Flush();

            return writer.Expression;
        }

        private static void AssertNotSupported(Action<JsonExpressionWriter> a)
        {
            Assert.ThrowsException<NotSupportedException>(() => a(new JsonExpressionWriter()));
        }

        private sealed class Person
        {
            public string Name { get; set; }
            public int? Age { get; set; }
            public City City { get; set; }
        }

        private sealed class City
        {
            public string Zip { get; set; }
        }
    }
}
