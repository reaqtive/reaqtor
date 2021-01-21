// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
// 

using System;
using System.IO;
using System.Linq.Expressions;

using Newtonsoft.Json;

using Nuqleon.DataModel.Serialization.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nuqleon.DataModel.Serialization.JsonTest
{
    public partial class DataSerializerTests
    {
        [TestMethod]
        public void DataSerializer_Create_ArgumentChecks()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var tries = new Action[]
            {
                () => DataSerializer.Create(default(IExpressionSerializer)),
                () => DataSerializer.Create(default(Func<Expression, string>), _ => null),
                () => DataSerializer.Create(e => null, default(Func<string, Expression>)),

#pragma warning disable 618 // Deprecated
                () => DataSerializer.Create(default(IExpressionSerializer), includePrivate: false),
                () => DataSerializer.Create(default(Func<Expression, string>), _ => null, includePrivate: false),
                () => DataSerializer.Create(e => null, default(Func<string, Expression>), includePrivate: false),
#pragma warning restore 618 // Deprecated
            };
#pragma warning restore IDE0034 // Simplify 'default' expression

            foreach (var t in tries)
            {
                Assert.ThrowsException<ArgumentNullException>(t);
            }
        }

        [TestMethod]
        public void DataSerializer_Create_Simple()
        {
            var sh = new SerializationHelper();

            var es = sh.ExpressionSerializer;

            var sers = new[]
            {
                DataSerializer.Create(),
                DataSerializer.Create(es),
                DataSerializer.Create(e => null, _ => null),

#pragma warning disable 618 // Deprecated
                DataSerializer.Create(es, includePrivate: false),
                DataSerializer.Create(e => null, _ => null, includePrivate: false),
#pragma warning restore 618 // Deprecated
            };

            foreach (var ser in sers)
            {
                AssertRoundtrips(ser);
            }
        }

        [TestMethod]
        public void JsonDataSerializer_ArgumentChecks()
        {
            var json = (JsonDataSerializer)DataSerializer.Create();

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            var tries = new Action[]
            {
                () => json.Deserialize<int>(default(Stream)),
                () => json.DeserializeFrom<int>(default(TextReader)),
                () => json.DeserializeFrom<int>(default(JsonReader)),
                () => json.Serialize(42, default(Stream)),
                () => json.SerializeTo(42, default(TextWriter)),
                () => json.SerializeTo(42, default(JsonWriter)),
            };
#pragma warning restore IDE0034 // Simplify 'default' expression

            foreach (var t in tries)
            {
                Assert.ThrowsException<ArgumentNullException>(t);
            }
        }

        [TestMethod]
        public void DataSerializer_Json_TextReaderAndWriter()
        {
            var p = new Person { Name = "Bart", Age = 21 };

            var ser = (JsonDataSerializer)DataSerializer.Create();

            var sw = new StringWriter();
            ser.SerializeTo(p, sw);

            var json = sw.ToString();

            var sr = new StringReader(json);
            var res = ser.DeserializeFrom<Person>(sr);

            Assert.AreEqual(p.Name, res.Name);
            Assert.AreEqual(p.Age, res.Age);
        }

        [TestMethod]
        public void DataSerializer_Json_JsonReaderAndWriter()
        {
            var p = new Person { Name = "Bart", Age = 21 };

            var ser = (JsonDataSerializer)DataSerializer.Create();

            var sw = new StringWriter();
            var jw = new JsonTextWriter(sw);
            ser.SerializeTo(p, jw);

            var json = sw.ToString();

            var sr = new StringReader(json);
            var jr = new JsonTextReader(sr);
            var res = ser.DeserializeFrom<Person>(jr);

            Assert.AreEqual(p.Name, res.Name);
            Assert.AreEqual(p.Age, res.Age);
        }

        private static void AssertRoundtrips(DataSerializer ser)
        {
            var o = new MyObj { X = 42 };

            var ms = new MemoryStream();
            ser.Serialize(o, ms);

            ms.Position = 0;
            var res = ser.Deserialize<MyObj>(ms);

            Assert.AreEqual(42, res.X);
        }

        private class MyObj
        {
            [Mapping("Foo")]
            public int X { get; set; }
        }
    }
}
