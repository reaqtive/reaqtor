// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Serialization;

namespace Tests
{
    [TestClass]
    public partial class DeserializerTests
    {
        [TestMethod]
        public void FastDeserializer_Unsupported()
        {
            var pbs = new FastJsonSerializerFactory.ParserStringBuilder(DefaultNameResolver.Instance);
            var bds = typeof(FastJsonSerializerFactory.ParserStringBuilder).GetMethod("Build", BindingFlags.Public | BindingFlags.Instance);
#if !NO_IO
            var pbr = new FastJsonSerializerFactory.ParserReaderBuilder(DefaultNameResolver.Instance);
            var bdr = typeof(FastJsonSerializerFactory.ParserReaderBuilder).GetMethod("Build", BindingFlags.Public | BindingFlags.Instance);
#endif
            var create = typeof(FastJsonSerializerFactory).GetMethod("CreateDeserializer", new[] { typeof(INameResolver), typeof(FastJsonConcurrencyMode) });

            foreach (var type in new[] { typeof(E), typeof(E?), typeof(C), typeof(int[,]), typeof(D), typeof(I), typeof(G<int>), typeof(IDictionary<int, int>) })
            {
                AssertNotSupportedException(create, type, null, null, FastJsonConcurrencyMode.SingleThreaded);

                AssertNotSupportedException(bds, type, pbs);
#if !NO_IO
                AssertNotSupportedException(bdr, type, pbr);
#endif
            }
        }

        private static void AssertNotSupportedException(MethodInfo method, Type type, object instance, params object[] args)
        {
            try
            {
                method.MakeGenericMethod(type).Invoke(instance, args);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is NotSupportedException)
                {
                    return;
                }
            }

            Assert.Fail("Expected exception for type " + type);
        }

        [TestMethod]
        public void FastDeserializer_ArgumentChecking()
        {
            var des = FastJsonSerializerFactory.CreateDeserializer<int>(resolver: null, FastJsonConcurrencyMode.SingleThreaded);

            Assert.ThrowsException<ArgumentNullException>(() => des.Deserialize(default(string)));
#if !NO_IO
            Assert.ThrowsException<ArgumentNullException>(() => des.Deserialize(default(System.IO.TextReader)));
#endif
            Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => des.Deserialize("42a"));

            Assert.ThrowsException<ArgumentNullException>(() => FastJsonSerializerFactory.CreateDeserializer<int>(resolver: null, settings: null));
        }

        [TestMethod]
        public void FastDeserializer_Boolean()
        {
            AssertDeserialize(new Asserts<bool>
            {
                { "false", false },
                { "true", true },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableBoolean()
        {
            AssertDeserialize(new Asserts<bool?>
            {
                { "null", null },
                { "false", false },
                { "true", true },
            });
        }

        [TestMethod]
        public void FastDeserializer_Char()
        {
            AssertDeserialize(new Asserts<char>
            {
                { "\"a\"", 'a' },
                { "\"\\t\"", '\t' },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableChar()
        {
            AssertDeserialize(new Asserts<char?>
            {
                { "null", null },
                { "\"a\"", 'a' },
                { "\"\\t\"", '\t' },
            });
        }

        [TestMethod]
        public void FastDeserializer_Single()
        {
            AssertDeserialize(new Asserts<float>
            {
                { "12.34", 12.34f },
                { "-98.7e1", -987f },
                { "1234567890.123456789012345678901234567890123456789012345678901234567890", float.Parse("1234567890.123456789012345678901234567890123456789012345678901234567890") },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableSingle()
        {
            AssertDeserialize(new Asserts<float?>
            {
                { "null", null },
                { "12.34", 12.34f },
                { "-98.7e1", -987f },
            });
        }

        [TestMethod]
        public void FastDeserializer_Double()
        {
            AssertDeserialize(new Asserts<double>
            {
                { "12.34", 12.34d },
                { "-98.7e1", -987d },
                { "1234567890.123456789012345678901234567890123456789012345678901234567890", double.Parse("1234567890.123456789012345678901234567890123456789012345678901234567890") },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableDouble()
        {
            AssertDeserialize(new Asserts<double?>
            {
                { "null", null },
                { "12.34", 12.34d },
                { "-98.7e1", -987d },
            });
        }

        [TestMethod]
        public void FastDeserializer_Decimal()
        {
            AssertDeserialize(new Asserts<decimal>
            {
                { "12.34", 12.34m },
                { "-98.7e1", -987m },
                { "1234567890.123456789012345678901234567890123456789012345678901234567890", decimal.Parse("1234567890.123456789012345678901234567890123456789012345678901234567890") },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableDecimal()
        {
            AssertDeserialize(new Asserts<decimal?>
            {
                { "null", null },
                { "12.34", 12.34m },
                { "-98.7e1", -987m },
            });
        }

        [TestMethod]
        public void FastDeserializer_DateTime()
        {
            AssertDeserialize(new Asserts<DateTime>
            {
                { "\"2016-02-11T12:34:56.789Z\"", new DateTime(2016, 2, 11, 12, 34, 56, 789) },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableDateTime()
        {
            AssertDeserialize(new Asserts<DateTime?>
            {
                { "null", null },
                { "\"2016-02-11T12:34:56.789Z\"", new DateTime(2016, 2, 11, 12, 34, 56, 789) },
            });
        }

        [TestMethod]
        public void FastDeserializer_DateTimeOffset()
        {
            AssertDeserialize(new Asserts<DateTimeOffset>
            {
                { "\"2016-02-11T12:34:56.789Z\"", new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, TimeSpan.Zero) },
                { "\"2016-02-11T12:34:56.789+08:00\"", new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)) },
            });
        }

        [TestMethod]
        public void FastDeserializer_NullableDateTimeOffset()
        {
            AssertDeserialize(new Asserts<DateTimeOffset?>
            {
                { "null", null },
                { "\"2016-02-11T12:34:56.789Z\"", new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, TimeSpan.Zero) },
                { "\"2016-02-11T12:34:56.789+08:00\"", new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)) },
            });
        }

        [TestMethod]
        public void FastDeserializer_String()
        {
            AssertDeserialize(new Asserts<string>
            {
                { "null", null },
                { "\"\"", "" },
                { "\"bar\"", "bar" },
                { "\"bar\\tfoo\"", "bar\tfoo" },
            });
        }

        [TestMethod]
        public void FastDeserializer_Int32Array()
        {
            AssertDeserialize(new Asserts<int[]>
            {
                { "null", null },
                { "[]", Array.Empty<int>() },
                { "[42]", new[] { 42 } },
                { "[ 42]", new[] { 42 } },
                { "[  42]", new[] { 42 } },
                { "[42 ]", new[] { 42 } },
                { "[42  ]", new[] { 42 } },
                { "[ 42 ]", new[] { 42 } },
                { "[  42  ]", new[] { 42 } },
                { "[2,3,5,7]", new[] { 2, 3, 5, 7 } },
                { "[ 2,3  ,5, 7  ]", new[] { 2, 3, 5, 7 } },
                { "[ 2,3  ,5, 7, 11,13,17,19, 23  ]", new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 } },
            },
            (expected, actual) =>
            {
                if (expected == null)
                {
                    return actual == null;
                }

                if (actual == null)
                {
                    return false;
                }

                return expected.SequenceEqual(actual);
            });

            AssertDeserializeFails<int[]>(
                "",
                " ",
                "  ",
                "n",
                "nu",
                "nul",
                "true",
                "[",
                "[ ",
                "[42",
                "[42 ",
                "[42b",
                "[42[",
                "[42{",
                "[42:",
                "[42n",
                "[42t",
                "[42f",
                "[42 b",
                "[42,",
                "[42, ",
                "[42,43",
                "[42,43 ",
                "[42,43,",
                "[42,43, ",
                "[42,43,]",
                "[42,43, ]"
            );
        }

        [TestMethod]
        public void FastDeserializer_Int32ArrayArray()
        {
            AssertDeserialize(new Asserts<int[][]>
            {
                { "null", null },
                { "[null]", new int[][] { null } },
                { "[[42]]", new int[][] { new int[] { 42 } } },
                { "[  [ 42 ] \t , null\r\n, [ 43, 44 ]\r\n]", new int[][] { new int[] { 42 }, null, new int[] { 43, 44 } } },
            },
            (expected, actual) =>
            {
                if (expected == null)
                {
                    return actual == null;
                }

                if (actual == null || expected.Length != actual.Length)
                {
                    return false;
                }

                return expected.Zip(actual, (e, a) =>
                {
                    if (e == null)
                    {
                        return a == null;
                    }

                    if (a == null)
                    {
                        return false;
                    }

                    return e.SequenceEqual(a);
                }).All(b => b);
            });
        }

        [TestMethod]
        public void FastDeserializer_Int32List()
        {
            AssertDeserialize(new Asserts<List<int>>
            {
                { "[ 2,3  ,5, 7  ]", new List<int> { 2, 3, 5, 7 } },
            },
            (expected, actual) =>
            {
                if (expected == null)
                {
                    return actual == null;
                }

                if (actual == null)
                {
                    return false;
                }

                return expected.SequenceEqual(actual);
            });

            AssertDeserialize(new Asserts<IList<int>>
            {
                { "[ ]", new List<int>() },
                { "[42]", new List<int> { 42 } },
                { "[ 2,3  ,5, 7  ]", new List<int> { 2, 3, 5, 7 } },
            },
            (expected, actual) =>
            {
                if (expected == null)
                {
                    return actual == null;
                }

                if (actual == null)
                {
                    return false;
                }

                return expected.SequenceEqual(actual);
            });

            AssertDeserialize(new Asserts<IEnumerable<int>>
            {
                { "[ ]", new List<int>() },
                { "[42]", new List<int> { 42 } },
                { "[ 2,3  ,5, 7  ]", new List<int> { 2, 3, 5, 7 } },
            },
            (expected, actual) =>
            {
                if (expected == null)
                {
                    return actual == null;
                }

                if (actual == null)
                {
                    return false;
                }

                return expected.SequenceEqual(actual);
            });

            AssertDeserialize(new Asserts<IReadOnlyList<int>>
            {
                { "[ ]", new List<int>() },
                { "[42]", new List<int> { 42 } },
                { "[ 2,3  ,5, 7  ]", new List<int> { 2, 3, 5, 7 } },
            },
            (expected, actual) =>
            {
                if (expected == null)
                {
                    return actual == null;
                }

                if (actual == null)
                {
                    return false;
                }

                return expected.SequenceEqual(actual);
            });
        }

        [TestMethod]
        public void FastDeserializer_PersonClass()
        {
            AssertDeserialize(new Asserts<PersonClass>
            {
                // Null
                { "null", null },

                // Variations in spacing
                { "{\"Name\":\"Bart\",\"Age\":21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\":\"Bart\",\"Age\":21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\" :\"Bart\",\"Age\":21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\": \"Bart\",\"Age\":21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\" ,\"Age\":21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\", \"Age\":21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\",\"Age\" :21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\",\"Age\": 21}", new PersonClass { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\",\"Age\":21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\" : \"Bart\" , \"Age\" : 21 }", new PersonClass { Name = "Bart", Age = 21 } },

                // Independence of order
                { "{ \"Age\": 21, \"Name\": \"Bart\" }", new PersonClass { Name = "Bart", Age = 21 } },

                // Optional properties
                { "{ \"Name\": \"Bart\" }", new PersonClass { Name = "Bart", Age = 0 } },
                { "{ \"Age\": 21 }", new PersonClass { Name = null, Age = 21 } },
                { "{ }", new PersonClass { Name = null, Age = 0 } },

                // Extra properties
                { "{ \"Name\": \"Bart\", \"Bar\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"N\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"Na\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"Nam\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"NamX\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"NameX\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },

                // UTF-16 decoding in keys
                { "{ \"N\\u0061me\": \"Bart\", \"A\\u0067e\": 21 }", new PersonClass { Name = "Bart", Age = 21 } },

                // Attempt to set unsettable members
                { "{ \"Bar\": 42, \"Foo\": 42, \"Qux\": 42, \"Item\": 42 }", new PersonClass() },
            });

            AssertDeserializeFails<PersonClass>(
                "",
                " ",
                "  ",
                "n",
                "nu",
                "nul",
                "true",
                "{",
                "{ ",
                "{ X",
                "{ \"",
                "{ \"B",
                "{ \"Ba",
                "{ \"Bar",
                "{ \"Bar\"",
                "{ \"Bar\"X",
                "{ \"Bar\" X",
                "{ \"Bar\":X",
                "{ \"Bar\" :X",
                "{ \"Bar\" : X",
                "{ \"Bar\" : 42",
                "{ \"Bar\" : 42,",
                "{ \"Bar\" : 42X",
                "{ \"Bar\" : 42,X",
                "{ \"Bar\" : 42,}",
                "{ \"Bar\" : 42, }",
                "{ \"Bar\" : 42, \"Foo\": null, }"
            );
        }

        [TestMethod]
        public void FastDeserializer_PersonStruct()
        {
            AssertDeserialize(new Asserts<PersonStruct>
            {
                // Variations in spacing
                { "{\"Name\":\"Bart\",\"Age\":21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\":\"Bart\",\"Age\":21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\" :\"Bart\",\"Age\":21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\": \"Bart\",\"Age\":21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\" ,\"Age\":21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\", \"Age\":21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\",\"Age\" :21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\",\"Age\": 21}", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{\"Name\":\"Bart\",\"Age\":21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\" : \"Bart\" , \"Age\" : 21 }", new PersonStruct { Name = "Bart", Age = 21 } },

                // Independence of order
                { "{ \"Age\": 21, \"Name\": \"Bart\" }", new PersonStruct { Name = "Bart", Age = 21 } },

                // Optional properties
                { "{ \"Name\": \"Bart\" }", new PersonStruct { Name = "Bart", Age = 0 } },
                { "{ \"Age\": 21 }", new PersonStruct { Name = null, Age = 21 } },
                { "{ }", new PersonStruct { Name = null, Age = 0 } },

                // Extra properties
                { "{ \"Name\": \"Bart\", \"Bar\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"N\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"Na\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"Nam\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"NamX\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ \"Name\": \"Bart\", \"NameX\": [ { \"qux\": 16 }, true, null, \"foo\" ], \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },

                // UTF-16 decoding in keys
                { "{ \"N\\u0061me\": \"Bart\", \"A\\u0067e\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
            });

            AssertDeserializeFails<PersonStruct>(
                "",
                " ",
                "  ",
                "n",
                "nu",
                "nul",
                "null",
                "true",
                "{",
                "{ ",
                "{ X",
                "{ \"",
                "{ \"B",
                "{ \"Ba",
                "{ \"Bar",
                "{ \"Bar\"",
                "{ \"Bar\"X",
                "{ \"Bar\" X",
                "{ \"Bar\":X",
                "{ \"Bar\" :X",
                "{ \"Bar\" : X",
                "{ \"Bar\" : 42",
                "{ \"Bar\" : 42,",
                "{ \"Bar\" : 42X",
                "{ \"Bar\" : 42,X",
                "{ \"Bar\" : 42,}",
                "{ \"Bar\" : 42, }",
                "{ \"Bar\" : 42, \"Foo\": null, }"
            );
        }

        [TestMethod]
        public void FastDeserializer_NullablePersonStruct()
        {
            AssertDeserialize(new Asserts<PersonStruct?>
            {
                { "null", null },
                { "{ \"Name\": \"Bart\", \"Age\": 21 }", new PersonStruct { Name = "Bart", Age = 21 } },
                { "{ }", new PersonStruct { Name = null, Age = 0 } },
            });

            AssertDeserializeFails<PersonStruct?>(
                "",
                " ",
                "  ",
                "n",
                "nu",
                "nul",
                "true",
                "{",
                "{ ",
                "{ X",
                "{ \"",
                "{ \"B",
                "{ \"Ba",
                "{ \"Bar",
                "{ \"Bar\"",
                "{ \"Bar\"X",
                "{ \"Bar\" X",
                "{ \"Bar\":",
                "{ \"Bar\":X",
                "{ \"Bar\" :X",
                "{ \"Bar\" : X",
                "{ \"Bar\" : 42",
                "{ \"Bar\" : 42,",
                "{ \"Bar\" : 42X",
                "{ \"Bar\" : 42,X",
                "{ \"Bar\" : 42,}",
                "{ \"Bar\" : 42, }",
                "{ \"Bar\" : 42, \"Foo\": null, }"
            );
        }

        [TestMethod]
        public void FastDeserializer_PersonWithParent()
        {
            AssertDeserialize(new Asserts<PersonWithParent>
            {
                { "{ \"Name\": \"Bart\", \"Age\": 21 }", new PersonWithParent { Name = "Bart", Age = 21, Parent = null } },
                { "{ \"Name\": \"Bart\", \"Age\": 21, \"Parent\": { \"Name\": \"Homer\", \"Age\": 42 } }", new PersonWithParent { Name = "Bart", Age = 21, Parent = new PersonWithParent { Name = "Homer", Age = 42 } } },
            });
        }

        [TestMethod]
        public void FastDeserializer_Strange()
        {
            AssertDeserialize(new Asserts<Strange>
            {
                // JSON = `{ "bar": 42 }`       property = `bar`
                { "{ \"bar\": 42 }", new Strange { } },

                // JSON = `{ "bar\"": 42 }`     property = `bar"`
                { "{ \"bar\\\"\": 42 }", new Strange { Bar1 = 42 } },

                // JSON = `{ "bar\"foo": 42 }`  property = `bar"foo`
                { "{ \"bar\\\"foo\": 42 }", new Strange { } },

                // JSON = `{ "bar\":": 42 }`    property = `bar":`
                { "{ \"bar\\\":\": 42 }", new Strange { Bar2 = 42 } },

                // JSON = `{ "bar\"foo": 42 }`  property = `bar":foo`
                { "{ \"bar\\\":foo\": 42 }", new Strange { } },

                // JSON = `{ "foo\\t": 42 }`    property = `foo\t`
                { "{ \"foo\\\\t\": 42 }", new Strange { Foo1 = 42 } },

                // JSON = `{ "foo\\n": 42 }`    property = `foo\n`
                { "{ \"foo\\\\n\": 42 }", new Strange { Foo2 = 42 } },
            }, new MyResolver());
        }

        [TestMethod]
        public void FastDeserializer_Any()
        {
            AssertDeserialize(new Asserts<object>
            {
                { "null", null },

                { "true", true },
                { "false", false },

                { "0", double.Parse("0") },
                { "1", double.Parse("1") },
                { "2", double.Parse("2") },
                { "3", double.Parse("3") },
                { "4", double.Parse("4") },
                { "5", double.Parse("5") },
                { "6", double.Parse("6") },
                { "7", double.Parse("7") },
                { "8", double.Parse("8") },
                { "9", double.Parse("9") },
                { "-42", double.Parse("-42") },
                { "987.65", double.Parse("987.65") },

                { "\"bar\"", "bar" },
            });

            AssertDeserialize(new Asserts<object>
            {
                { "[]", Array.Empty<object>() },
                { "[42,true,\"bar\",null]", new object[] { double.Parse("42"), true, "bar", null }  },
            },
            (expected, actual) =>
            {
                var e = (object[])expected;
                var a = (object[])actual;

                return e.SequenceEqual(a);
            });

            AssertDeserialize(new Asserts<object>
            {
                { "{}", new Dictionary<string, object>() },
                { "{\"bar\": 42, \"foo\": \"qux\", \"baz\": true}", new Dictionary<string, object> { { "bar", double.Parse("42") }, { "foo", "qux" }, { "baz", true } } },
            },
            (expected, actual) =>
            {
                var e = (IDictionary<string, object>)expected;
                var a = (IDictionary<string, object>)actual;

                return e.OrderBy(kv => kv.Key).SequenceEqual(a.OrderBy(kv => kv.Key));
            });

            AssertDeserializeFails<object>(
                "",
                " ",
                "  ",

                "n",
                "nu",
                "nul",

                "t",
                "tr",
                "tru",
                "f",
                "fa",
                "fal",
                "fals",

                "-",

                "{",
                "{ ",
                "{ X",
                "{ \"",
                "{ \"Bar",
                "{ \"Bar\"",
                "{ \"Bar\"X",
                "{ \"Bar\":",
                "{ \"Bar\":X",
                "{ \"Bar\" :X",
                "{ \"Bar\" : X",
                "{ \"Bar\" : 42",
                "{ \"Bar\" : 42,",
                "{ \"Bar\" : 42X",
                "{ \"Bar\" : 42,X",
                "{ \"Bar\" : 42,}",

                "[",
                "[ ",
                "[ X",
                "[ 42",
                "[ 42X",
                "[ 42,",
                "[ 42,X",
                "[ 42,]"
            );
        }

        [TestMethod]
        public void FastDeserializer_AnyArray()
        {
            AssertDeserialize(new Asserts<object[]>
            {
                { "[]", Array.Empty<object>() },
                { "[42,true,\"bar\",null]", new object[] { double.Parse("42"), true, "bar", null }  },
            },
            (expected, actual) =>
            {
                var e = expected;
                var a = actual;

                return e.SequenceEqual(a);
            });
        }

        [TestMethod]
        public void FastDeserializer_AnyObject_Dictionary()
        {
            AssertDeserialize(new Asserts<Dictionary<string, object>>
            {
                { "{}", new Dictionary<string, object>() },
                { "{\"bar\": 42, \"foo\": \"qux\", \"baz\": true}", new Dictionary<string, object> { { "bar", double.Parse("42") }, { "foo", "qux" }, { "baz", true } } },
            },
            (expected, actual) =>
            {
                var e = expected;
                var a = actual;

                return e.OrderBy(kv => kv.Key).SequenceEqual(a.OrderBy(kv => kv.Key));
            });
        }

        [TestMethod]
        public void FastDeserializer_AnyObject_IDictionary()
        {
            AssertDeserialize(new Asserts<IDictionary<string, object>>
            {
                { "{}", new Dictionary<string, object>() },
                { "{\"bar\": 42, \"foo\": \"qux\", \"baz\": true}", new Dictionary<string, object> { { "bar", double.Parse("42") }, { "foo", "qux" }, { "baz", true } } },
            },
            (expected, actual) =>
            {
                var e = expected;
                var a = actual;

                return e.OrderBy(kv => kv.Key).SequenceEqual(a.OrderBy(kv => kv.Key));
            });
        }

        [TestMethod]
        public void FastDeserializer_AnyObject_IReadOnlyDictionary()
        {
            AssertDeserialize(new Asserts<IReadOnlyDictionary<string, object>>
            {
                { "{}", new Dictionary<string, object>() },
                { "{\"bar\": 42, \"foo\": \"qux\", \"baz\": true}", new Dictionary<string, object> { { "bar", double.Parse("42") }, { "foo", "qux" }, { "baz", true } } },
            },
            (expected, actual) =>
            {
                var e = expected;
                var a = actual;

                return e.OrderBy(kv => kv.Key).SequenceEqual(a.OrderBy(kv => kv.Key));
            });
        }

        private static void AssertDeserialize<T>(Asserts<T> asserts)
        {
            foreach (var des in new[]
            {
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver: null, FastJsonConcurrencyMode.SingleThreaded),
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver: null, FastJsonConcurrencyMode.ThreadSafe),
            })
            {
                foreach (var kv in asserts)
                {
                    Assert.AreEqual(kv.Value, des.Deserialize(kv.Key));

#if !NO_IO
                    Assert.AreEqual(kv.Value, des.Deserialize(new System.IO.StringReader(kv.Key)));
#endif
                }
            }
        }

        private static void AssertDeserialize<T>(Asserts<T> asserts, Func<T, T, bool> assert)
        {
            foreach (var des in new[]
            {
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver: null, FastJsonConcurrencyMode.SingleThreaded),
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver: null, FastJsonConcurrencyMode.ThreadSafe),
            })
            {
                foreach (var kv in asserts)
                {
                    Assert.IsTrue(assert(kv.Value, des.Deserialize(kv.Key)));

#if !NO_IO
                    Assert.IsTrue(assert(kv.Value, des.Deserialize(new System.IO.StringReader(kv.Key))));
#endif
                }
            }
        }

        private static void AssertDeserialize<T>(Asserts<T> asserts, INameResolver resolver)
        {
            foreach (var des in new[]
            {
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver, FastJsonConcurrencyMode.SingleThreaded),
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver, FastJsonConcurrencyMode.ThreadSafe),
            })
            {
                foreach (var kv in asserts)
                {
                    Assert.AreEqual(kv.Value, des.Deserialize(kv.Key));

#if !NO_IO
                    Assert.AreEqual(kv.Value, des.Deserialize(new System.IO.StringReader(kv.Key)));
#endif
                }
            }
        }

#if UNUSED
        private static void AssertDeserialize<T>(Asserts<T> asserts, INameResolver resolver, Func<T, T, bool> assert)
        {
            foreach (var des in new[]
            {
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver, FastJsonConcurrencyMode.SingleThreaded),
                FastJsonSerializerFactory.CreateDeserializer<T>(resolver, FastJsonConcurrencyMode.ThreadSafe),
            })
            {
                foreach (var kv in asserts)
                {
                    Assert.IsTrue(assert(kv.Value, des.Deserialize(kv.Key)));

#if !NO_IO
                    Assert.IsTrue(assert(kv.Value, des.Deserialize(new System.IO.StringReader(kv.Key))));
#endif
                }
            }
        }
#endif

        private static void AssertDeserializeFails<T>(params string[] inputs)
        {
            var des = FastJsonSerializerFactory.CreateDeserializer<T>(resolver: null, FastJsonConcurrencyMode.SingleThreaded);

            foreach (var json in inputs)
            {
                Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => des.Deserialize(json));

#if !NO_IO
                Assert.ThrowsException<Nuqleon.Json.Parser.ParseException>(() => des.Deserialize(new System.IO.StringReader(json)));
#endif
            }
        }

        private sealed class Asserts<T> : Dictionary<string, T>
        {
            public new void Add(string s, T value)
            {
                this[s] = value;
            }
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable CA1822 // Mark members as static
        private sealed class PersonClass : IEquatable<PersonClass>
        {
            // NB: These properties and fields cannot be set.
            public readonly int Bar = 42;
            public const int Foo = 42;
            public int Qux => 0;
            public int this[int x] { get => 0; set { } }

            public string Name { get; set; }
            public int Age;

            public override bool Equals(object obj)
            {
                return Equals(obj as PersonClass);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(PersonClass other)
            {
                return other != null && other.Name == Name && other.Age == Age;
            }
        }
#pragma warning restore CA1822
#pragma warning restore IDE0079

        private struct PersonStruct : IEquatable<PersonStruct>
        {
            public string Name;
            public int Age { get; set; }

            public override bool Equals(object obj)
            {
                return obj is PersonStruct person && Equals(person);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(PersonStruct other)
            {
                return other.Name == Name && other.Age == Age;
            }
        }

        private sealed class PersonWithParent : IEquatable<PersonWithParent>
        {
            public string Name { get; set; }
            public int Age;
            public PersonWithParent Parent { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as PersonWithParent);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(PersonWithParent other)
            {
                return other != null && other.Name == Name && other.Age == Age && EqualityComparer<PersonWithParent>.Default.Equals(other.Parent, Parent);
            }
        }

        private sealed class Strange : IEquatable<Strange>
        {
            [Identifier("bar\"")]   // NB: overrun  would match `bar"`  with JSON containing property `"bar": 42` and fail on having consumed up to "
            public int Bar1;

            [Identifier("bar\":")]  // NB: overrun  would match `bar":` with JSON containing property `"bar": 42` and fail on having consumed up to :
            public int Bar2;

            [Identifier("foo\\t")]  // NB: underrun would match `foo\`  with JSON containing property `"foo\t": 42` and consider t to be proper input
            public int Foo1;

            [Identifier("foo\\n")]  // NB: underrun would match `foo\`  with JSON containing property `"foo\n": 42` and consider n to be proper input
            public int Foo2;

            public override bool Equals(object obj)
            {
                return Equals(obj as Strange);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(Strange other)
            {
                return other != null && other.Bar1 == Bar1 && other.Bar2 == Bar2 && other.Foo1 == Foo1 && other.Foo2 == Foo2;
            }
        }

        private enum E { }
        private abstract class C { }
        private interface I { }
        private delegate void D();
        private class G<T> { }

        private sealed class MyResolver : INameResolver
        {
            public IEnumerable<string> GetNames(FieldInfo field)
            {
                var attr = field.GetCustomAttribute<IdentifierAttribute>();
                if (attr != null)
                {
                    yield return attr.Id;
                }
            }

            public IEnumerable<string> GetNames(PropertyInfo property)
            {
                var attr = property.GetCustomAttribute<IdentifierAttribute>();
                if (attr != null)
                {
                    yield return attr.Id;
                }
            }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        private sealed class IdentifierAttribute : Attribute
        {
            public IdentifierAttribute(string id)
            {
                Id = id;
            }

            public string Id;
        }
    }
}
