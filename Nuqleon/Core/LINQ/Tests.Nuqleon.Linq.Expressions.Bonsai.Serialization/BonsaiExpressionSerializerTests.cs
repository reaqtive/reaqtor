// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - February 2015 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Serialization;

using Json = Nuqleon.Json.Expressions;

namespace Tests
{
    [TestClass]
    public class BonsaiExpressionSerializerTests
    {
        [TestMethod]
        public void BonsaiExpressionSerializer_NoFactories_ThrowsNotImplemented()
        {
            var serializer = new BonsaiExpressionSerializer();
            var testSerializer = new TestSerializer();
            var bonsai = testSerializer.Serialize(testSerializer.Lift(Expression.Constant(42)));

            Assert.ThrowsException<NotImplementedException>(() => serializer.Serialize(serializer.Lift(Expression.Constant(42))));
            Assert.ThrowsException<NotImplementedException>(() => serializer.Reduce(serializer.Deserialize(bonsai)));
        }

        [TestMethod]
        public void BonsaiExpressionSerializer_OverrideFactories()
        {
            var serializer = new TestSerializer();

            var value = 42;
            var constant = Expression.Constant(value);

            var bonsai = serializer.Serialize(serializer.Lift(constant));
            var rt = serializer.Reduce(serializer.Deserialize(bonsai));

            Assert.AreEqual(value, ((ConstantExpression)rt).Value);
        }

        [TestMethod]
        public void BonsaiExpressionSerializer_CanReLift()
        {
            var serializer = new TestSerializer();

            var value = 42;
            var constant = Expression.Constant(value);

            var lifted = serializer.Lift(constant);

            var bonsai1 = serializer.Serialize(lifted);
            var slim1 = serializer.Deserialize(bonsai1);

            Assert.AreEqual(@"{""Context"":{""Types"":[[""::"",""System.Int32"",0]],""Assemblies"":[""STD""],""Version"":""0.9.0.0""},""Expression"":["":"",42,0]}", StripMscorlib(bonsai1));

            var bonsai2 = serializer.Serialize(slim1);
            var slim2 = serializer.Deserialize(bonsai2);

            Assert.AreEqual(StripMscorlib(bonsai1), StripMscorlib(bonsai2));

            var reduced = serializer.Reduce(slim2);

            Assert.AreEqual(value, ((ConstantExpression)reduced).Value);

            static string StripMscorlib(string bonsai) =>
                bonsai
                .Replace("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "STD")
                .Replace("System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", "STD")
                .Replace("System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", "STD");
        }

        [TestMethod]
        public void BonsaiExpressionSerializer_Legacy()
        {
            var serializer = new BonsaiExpressionSerializer(t => o => Json.Expression.Parse(new JsonSerializer(t).Serialize(o), ensureTopLevelObjectOrArray: false), t => json => new JsonSerializer(t).Deserialize(json));

            var value = 42;
            var constant = Expression.Constant(value);
            var bonsai = serializer.Serialize(serializer.Lift(constant));
            var rt = serializer.Reduce(serializer.Deserialize(bonsai));
            Assert.AreEqual(value, ((ConstantExpression)rt).Value);
        }

        private sealed class TestSerializer : BonsaiExpressionSerializer
        {
            protected override Func<object, Json.Expression> GetConstantSerializer(Type type)
            {
                // REVIEW: Nuqleon.Json has an odd asymmetry in Serialize and Deserialize signatures,
                //         due to the inability to overload by return type. However, it seems odd we
                //         have to go serialize string and subsequently parse into Expression.

                return o => Json.Expression.Parse(new JsonSerializer(type).Serialize(o), ensureTopLevelObjectOrArray: false);
            }

            protected override Func<Json.Expression, object> GetConstantDeserializer(Type type)
            {
                return json => new JsonSerializer(type).Deserialize(json);
            }
        }
    }
}
