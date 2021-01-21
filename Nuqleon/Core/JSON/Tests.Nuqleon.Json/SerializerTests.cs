// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Nuqleon.Json.Expressions;
using Nuqleon.Json.Parser;
using Nuqleon.Json.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Tests.Nuqleon.Json
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void Serializer_Null()
        {
            foreach (var t in new[] {
                typeof(object),
                typeof(string),
                typeof(int?),
                typeof(bool?),
            })
            {
                var ser = new JsonSerializer(t);
                var res = ser.Serialize(value: null);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Null, json.NodeType);

                var des = ser.Deserialize(json);
                Assert.IsNull(des);
            }
        }

        [TestMethod]
        public void Serializer_Boolean()
        {
            foreach (var o in new[] {
                true,
                false
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Boolean, json.NodeType);
                Assert.AreEqual(o, ((ConstantExpression)json).Value);

                var des = ser.Deserialize(json);
                Assert.AreEqual(o, des);
            }
        }

        [TestMethod]
        public void Serializer_Numbers()
        {
            foreach (var o in new object[] {
                (byte)123,
                (sbyte)123,
                (short)123,
                (ushort)123,
                123,
                123u,
                123L,
                123UL,
                123.45f,
                123.45d,
                123.45m,
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Number, json.NodeType);
                Assert.AreEqual(res.ToString(CultureInfo.InvariantCulture), json.ToString());
                Assert.AreEqual(res.ToString(CultureInfo.InvariantCulture), (string)((ConstantExpression)json).Value);

                var des = ser.Deserialize(json);
                Assert.AreEqual(o, des);
            }
        }

        [TestMethod]
        public void Serializer_Arrays1()
        {
            foreach (var o in new object[] {
                new[] { 1, 2, 3 },
                //new ArrayList { 1, 2, 3 },
                new List<int> { 1, 2, 3 },
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Array, json.NodeType);
                var arr = (ArrayExpression)json;
                var elements = arr.Elements.Select(e => int.Parse((string)((ConstantExpression)e).Value));
                var original = ((IEnumerable)o).Cast<int>();
                Assert.IsTrue(elements.SequenceEqual(original));

                var des = ser.Deserialize(json);
                Assert.AreEqual(o.GetType(), des.GetType());
                var results = ((IEnumerable)des).Cast<int>(); // By design - JSON numbers aren't roundtrippable to an exact type
                Assert.IsTrue(results.SequenceEqual(original));
            }
        }

        [TestMethod]
        public void Serializer_Arrays2()
        {
            foreach (var o in new object[] {
                new ArrayList { 1, 2, 3 },
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Array, json.NodeType);
                var arr = (ArrayExpression)json;
                var elements = arr.Elements.Select(e => int.Parse((string)((ConstantExpression)e).Value));
                var original = ((IEnumerable)o).Cast<int>();
                Assert.IsTrue(elements.SequenceEqual(original));

                var des = ser.Deserialize(json);
                Assert.AreEqual(o.GetType(), des.GetType());
                var results = ((IEnumerable)des).Cast<string>().Select(s => int.Parse(s)); // By design - JSON numbers aren't roundtrippable to an exact type
                Assert.IsTrue(results.SequenceEqual(original));
            }
        }

        [TestMethod]
        public void Serializer_Arrays3()
        {
            var xs = new[] { 1, 2, 3 };

            var ser = new JsonSerializer(typeof(int[]));
            var res = ser.Serialize(xs);

            var des = new JsonSerializer(typeof(object));
            var ys = des.Deserialize(res);

            var d = ys as IList;
            Assert.IsNotNull(d);

            Assert.IsTrue(int.Parse((string)d[0]) == 1); // TODO: by design?
            Assert.IsTrue(int.Parse((string)d[1]) == 2);
            Assert.IsTrue(int.Parse((string)d[2]) == 3);
        }

        [TestMethod]
        public void Serializer_Objects1()
        {
            foreach (var o in new[] {
                new Dictionary<string, int>
                {
                    { "bar", 1 },
                    { "foo", 2 },
                },
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Object, json.NodeType);
                var obj = (ObjectExpression)json;

                var newKeys = obj.Members.Keys.OrderBy(k => k);
                var oldKeys = o.Keys.OrderBy(k => k);

                Assert.IsTrue(newKeys.SequenceEqual(oldKeys));

                var des = ser.Deserialize(json);
                Assert.AreEqual(o.GetType(), des.GetType());
                var dict = (Dictionary<string, int>)des;

                Assert.AreEqual(1, dict["bar"]);
                Assert.AreEqual(2, dict["foo"]);
            }
        }

        [TestMethod]
        public void Serializer_Objects2()
        {
            foreach (var o in new[] {
                new Hashtable
                {
                    { "bar", 1 },
                    { "foo", 2 },
                }
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Object, json.NodeType);
                var obj = (ObjectExpression)json;

                var newKeys = obj.Members.Keys.OrderBy(k => k);
                var oldKeys = o.Keys.Cast<string>().OrderBy(k => k);

                Assert.IsTrue(newKeys.SequenceEqual(oldKeys));

                var des = ser.Deserialize(json);
                Assert.AreEqual(o.GetType(), des.GetType());
                var dict = (Hashtable)des;

                Assert.AreEqual("1", dict["bar"]); // By design - JSON numbers aren't roundtrippable to an exact type
                Assert.AreEqual("2", dict["foo"]);
            }
        }

        [TestMethod]
        public void Serializer_Objects3()
        {
            foreach (var o in new[] {
                new Person { Age = 21, Name = "Bart" }
            })
            {
                var ser = new JsonSerializer(o.GetType());
                var res = ser.Serialize(o);
                var json = JsonParser.Parse(res, ensureTopLevelObjectOrArray: false);
                Assert.AreEqual(ExpressionType.Object, json.NodeType);
                var obj = (ObjectExpression)json;

                var newKeys = obj.Members.Keys.OrderBy(k => k);
                var oldKeys = new[] { "Age", "Name" };

                Assert.IsTrue(newKeys.SequenceEqual(oldKeys));

                var des = ser.Deserialize(json);
                Assert.AreEqual(o.GetType(), des.GetType());
                var person = (Person)des;

                Assert.AreEqual("Bart", person.Name);
                Assert.AreEqual(21, person.Age);
            }
        }

        [TestMethod]
        public void Serializer_Objects4()
        {
            var p = new Person { Age = 21, Name = "Bart" };

            var ser = new JsonSerializer(typeof(Person));
            var res = Expression.Parse(ser.Serialize(p), ensureTopLevelObjectOrArray: false);

            var des = new JsonSerializer(typeof(object));
            var q = des.Deserialize(res);

            var d = q as Dictionary<string, object>;
            Assert.IsNotNull(d);

            Assert.IsTrue(int.Parse((string)d["Age"]) == 21); // TODO: by design?
            Assert.IsTrue((string)d["Name"] == "Bart");
        }

        [TestMethod]
        public void Serializer_Boolean_Coercions()
        {
            var ser = new JsonSerializer(typeof(bool));
            var res = Expression.Parse(ser.Serialize(true), ensureTopLevelObjectOrArray: false);

            foreach (var t in new[] { typeof(bool), typeof(bool?), typeof(object) })
            {
                var des = new JsonSerializer(t);
                var b = (bool)des.Deserialize(res);
                Assert.IsTrue(b);
            }

            Assert.ThrowsException<SerializationException>(() => new JsonSerializer(typeof(int)).Deserialize(res));
        }

        [TestMethod]
        public void Serializer_String()
        {
            var ser = new JsonSerializer(typeof(string));
            var res = Expression.Parse(ser.Serialize("bar"), ensureTopLevelObjectOrArray: false);

            foreach (var t in new[] { typeof(string), typeof(object) })
            {
                var des = new JsonSerializer(t);
                var b = (string)des.Deserialize(res);
                Assert.AreEqual("bar", b);
            }

            Assert.ThrowsException<SerializationException>(() => new JsonSerializer(typeof(int)).Deserialize(res));
        }

        [TestMethod]
        public void Serializer_String_Enum()
        {
            var des = new JsonSerializer(typeof(ConsoleColor));
            var j = Expression.Parse("\"Red\"", ensureTopLevelObjectOrArray: false);
            var exp = des.Deserialize(j);
            var c = (ConsoleColor)exp;
            Assert.AreEqual(ConsoleColor.Red, c);
        }

        [TestMethod]
        public void Serializer_Number_Enum()
        {
            var des = new JsonSerializer(typeof(ConsoleColor));
            var j = Expression.Parse("12", ensureTopLevelObjectOrArray: false);
            var exp = des.Deserialize(j);
            var c = (ConsoleColor)exp;
            Assert.AreEqual(ConsoleColor.Red, c);
        }

        [TestMethod]
        public void Serializer_Null_Coercions()
        {
            var res = Expression.Parse("null", ensureTopLevelObjectOrArray: false);

            foreach (var t in new[] { typeof(string), typeof(object), typeof(int?) })
            {
                var des = new JsonSerializer(t);
                Assert.IsNull(des.Deserialize(res));
            }

            Assert.ThrowsException<SerializationException>(() => new JsonSerializer(typeof(int)).Deserialize(res));
        }

        private sealed class Person
        {
            public string Name;
            public int Age { get; set; }
        }
    }
}
