// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Nuqleon.Json.Expressions;
using Nuqleon.Json.Interop.Newtonsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Tests.Nuqleon.Json.Interop.Newtonsoft
{
    [TestClass]
    public class JsonExpressionReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JsonExpressionReader_ArgumentChecking()
        {
            new JsonExpressionReader(json: null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JsonExpressionReader_ArgumentChecking_Pool1()
        {
            new JsonExpressionReader(json: null, new JsonInteropResourcePool());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void JsonExpressionReader_ArgumentChecking_Pool2()
        {
            new JsonExpressionReader(Expression.Null(), pool: null);
        }

        [TestMethod]
        public void JsonExpressionReader_Basics()
        {
            var reader = new JsonExpressionReader(Expression.Boolean(true));

            Assert.IsTrue(reader.Read());
            Assert.AreEqual(JsonToken.Boolean, reader.TokenType);

            Assert.IsFalse(reader.Read());
            Assert.AreEqual(JsonToken.None, reader.TokenType);

            Assert.IsFalse(reader.Read());
            Assert.AreEqual(JsonToken.None, reader.TokenType);

            ((IDisposable)reader).Dispose();

            var hasThrown = false;
            try
            {
                reader.Read();
            }
            catch (ObjectDisposedException)
            {
                hasThrown = true;
            }

            Assert.IsTrue(hasThrown);
        }

        [TestMethod]
        public void JsonExpressionReader_Primitives_Null()
        {
            Roundtrip(new object[] {
                null
            });
        }

        [TestMethod]
        public void JsonExpressionReader_Primitives_Boolean()
        {
            Roundtrip(
                false,
                true
            );
        }

        [TestMethod]
        public void JsonExpressionReader_Primitives_Int32()
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
        public void JsonExpressionReader_Primitives_Int64()
        {
            Roundtrip(
                long.MinValue,
                long.MaxValue
            );
        }

        [TestMethod]
        public void JsonExpressionReader_Primitives_BigInteger()
        {
            Roundtrip(
                (BigInteger)long.MaxValue + 1
            );
        }

        [TestMethod]
        public void JsonExpressionReader_Primitives_Double()
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
        [ExpectedException(typeof(JsonReaderException))]
        public void JsonExpressionReader_Primitives_Double_InvalidInput()
        {
            Deserialize(FloatParseHandling.Double, Expression.Number("12.34???"));
        }

        [TestMethod]
        public void JsonExpressionReader_Primitives_Decimal()
        {
            Roundtrip(
                FloatParseHandling.Decimal,
                49.95m
            );
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void JsonExpressionReader_Primitives_Decimal_InvalidInput()
        {
            Deserialize(FloatParseHandling.Decimal, Expression.Number("12.34???"));
        }

        [TestMethod]
        public void JsonExpressionReader_Arrays()
        {
            var rand = new Random(1983);
            var strs = new[] { null, "", " ", "\t", "bar", "foo", "qux", "baz", "foobar", "quux", "corge", "grault", "garply", "waldo", "fred", "plugh", "xyzzy", "thud" };

            var int32s = Enumerable.Range(0, 100).Select(i => (object)Enumerable.Range(0, i).ToArray()).ToArray();
            var strings = Enumerable.Range(0, 100).Select(i => (object)Enumerable.Range(0, i).Select(j => strs[rand.Next(0, strs.Length)]).ToArray()).ToArray();

            Roundtrip(int32s);
            Roundtrip(strings);
        }

        [TestMethod]
        public void JsonExpressionReader_Objects()
        {
            Roundtrip(
                new Person(),
                new Person() { Name = "Bart" },
                new Person() { Name = "Bart", Age = 21 },
                new Person() { Name = "Bart", Age = 21, City = new City { Zip = "98004" } }
            );
        }

        [TestMethod]
        public void JsonExpressionReader_BigObjects()
        {
            Roundtrip(Big8.Values);
        }

        [TestMethod]
        public void JsonExpressionReader_Concurrent()
        {
            var pool = new JsonInteropResourcePool();

            var tasks = Enumerable.Range(0, 32).Select(i => Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < 1000)
                {
                    Roundtrip(expr => new JsonExpressionReader(expr, pool), Big8.Values);
                }
            })).ToArray();

            Task.WaitAll(tasks);
        }

        private static void Roundtrip(params object[] objs) => Roundtrip(FloatParseHandling.Double, objs);

        private static void Roundtrip(FloatParseHandling fph, params object[] objs) => Roundtrip(expr => new JsonExpressionReader(expr) { FloatParseHandling = fph }, objs);

        private static void Roundtrip(Func<Expression, JsonExpressionReader> createReader, params object[] objs)
        {
            var ser = new JsonSerializer();

            foreach (var obj in objs)
            {
                var sw = new StringWriter();
                ser.Serialize(sw, obj);
                var json = sw.ToString();

                var expr = Expression.Parse(json, ensureTopLevelObjectOrArray: false);

                using var reader = createReader(expr);

                var res = ser.Deserialize(reader);

                var newSw = new StringWriter();
                ser.Serialize(newSw, res);
                var newJson = newSw.ToString();

                Assert.AreEqual(json, newJson);
            }
        }

        private static void Deserialize(FloatParseHandling fph, Expression expr)
        {
            var ser = new JsonSerializer();

            using var reader = new JsonExpressionReader(expr) { FloatParseHandling = fph };

            ser.Deserialize(reader);
        }

        private class Person
        {
            public string Name { get; set; }
            public int? Age { get; set; }
            public City City { get; set; }
        }

        private class City
        {
            public string Zip { get; set; }
        }
    }
}
