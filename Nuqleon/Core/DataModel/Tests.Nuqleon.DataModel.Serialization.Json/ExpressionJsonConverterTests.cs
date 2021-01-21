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
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Reflection;

using Nuqleon.DataModel;
using Nuqleon.DataModel.Serialization.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JsonExpression = Nuqleon.Json.Expressions.Expression;

namespace Tests.Nuqleon.DataModel.Serialization.Json
{
    [TestClass]
    public class ExpressionJsonConverterTests
    {
        [TestMethod]
        public void ExpressionJsonConverter_CanConvert()
        {
            var ejc = GetConverter();

            var tru = new[]
            {
                typeof(Expression),
                typeof(BinaryExpression),
                typeof(ExpressionSlim),
                typeof(BinaryExpressionSlim),
            };

            var fls = new[]
            {
                typeof(int),
                typeof(DateTime),
                typeof(string),
            };

            foreach (var t in tru)
            {
                Assert.IsTrue(ejc.CanConvert(t));
            }

            foreach (var f in fls)
            {
                Assert.IsFalse(ejc.CanConvert(f));
            }
        }

        [TestMethod]
        public void ExpressionJsonConverter_Roundtrip_Expression()
        {
            var c = Expression.Constant(42);

            var ser = new SerializationHelper(new Version(0, 9, 0, 0)).DataSerializer;

            var ms = new MemoryStream();
            ser.Serialize(c, ms);
            ms.Position = 0;
            var res = ser.Deserialize<Expression>(ms) as ConstantExpression;

            Assert.IsNotNull(res);
            Assert.AreEqual(42, res.Value);
        }

        [TestMethod]
        public void ExpressionJsonConverter_Roundtrip_ExpressionSlim()
        {
            var c = Expression.Constant(42).ToExpressionSlim();

            var ser = new SerializationHelper(new Version(0, 9, 0, 0)).DataSerializer;

            var ms = new MemoryStream();
            ser.Serialize(c, ms);
            ms.Position = 0;
            var res = ser.Deserialize<ExpressionSlim>(ms) as ConstantExpressionSlim;

            Assert.IsNotNull(res);
            Assert.AreEqual(42, res.Value.Reduce(typeof(int)));
        }

        private static ExpressionJsonConverter GetConverter()
        {
            var ser = new SerializationHelper(new Version(0, 9, 0, 0));

            var ejc = new ExpressionJsonConverter(ser.ExpressionSerializer);
            return ejc;
        }

        #region Expression serialization helpers

        private class SerializationHelper
        {
            private readonly MethodInfo _genericSerialize;
            private readonly MethodInfo _genericDeserialize;

            private readonly BonsaiExpressionSerializer _bonsaiSerializer;
            private readonly DataModelInvertedTypeSpace _invertedTypeSpace;

            public SerializationHelper(Version bonsaiVersion)
            {
                _invertedTypeSpace = new DataModelInvertedTypeSpace();
                _bonsaiSerializer = new DataModelBonsaiExpressionSerializer(_invertedTypeSpace, SerializeConstantFactory, DeserializeConstantFactory, bonsaiVersion);
                DataSerializer = DataSerializer.Create(_bonsaiSerializer);
                _genericSerialize = DataSerializer.GetType().GetMethod(nameof(DataSerializer.Serialize));
                _genericDeserialize = DataSerializer.GetType().GetMethod(nameof(DataSerializer.Deserialize));
            }

            public DataSerializer DataSerializer { get; }

            public IExpressionSerializer ExpressionSerializer => _bonsaiSerializer;

            public Func<object, JsonExpression> SerializeConstantFactory(Type type)
            {
                return obj => SerializeConstant(obj, type);
            }

            private JsonExpression SerializeConstant(object obj, Type type)
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);

                var serialize = _genericSerialize.MakeGenericMethod(new[] { type });
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

                var deserialize = _genericDeserialize.MakeGenericMethod(new[] { type });

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
                private readonly ExpressionSlimToExpressionConverter _reducer;

                public DataModelBonsaiExpressionSerializer(InvertedTypeSpace invertedTypeSpace, Func<Type, Func<object, JsonExpression>> liftFactory, Func<Type, Func<JsonExpression, object>> reduceFactory, Version version)
                    : base(liftFactory, reduceFactory, version)
                {
                    _reducer = new ExpressionSlimToExpressionConverter(invertedTypeSpace);
                }

                public override Expression Reduce(ExpressionSlim expression)
                {
                    return _reducer.Visit(expression);
                }
            }
        }

        #endregion
    }
}
