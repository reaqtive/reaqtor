// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System;
using System.IO;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Reflection;
using System.Text;

using Nuqleon.DataModel.Serialization.Json;

using Json = Nuqleon.Json.Expressions;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class SerializationHelper
    {
        private readonly IExpressionSerializer _bonsaiSerializer;
        private readonly JsonDataSerializer _dataSerializer;
        private readonly MethodInfo _genericSerialize;
        private readonly MethodInfo _genericDeserialize;

        public SerializationHelper()
            : this((liftFactory, reduceFactory) => new BonsaiExpressionSerializer(liftFactory, reduceFactory))
        {
        }

        public SerializationHelper(GetSerializer getSerializer)
        {
            _bonsaiSerializer = getSerializer(SerializeConstantFactory, DeserializeConstantFactory);
            _dataSerializer = (JsonDataSerializer)DataSerializer.Create(_bonsaiSerializer);
            _genericSerialize = _dataSerializer.GetType().GetMethod(nameof(DataSerializer.Serialize));
            _genericDeserialize = _dataSerializer.GetType().GetMethod(nameof(DataSerializer.Deserialize));
        }

        public void Serialize<T>(T value, Stream stream)
        {
            _dataSerializer.Serialize<T>(value, stream);
        }

        public string Serialize<T>(T value)
        {
            var stream = new MemoryStream();

            try
            {
                using var reader = new StreamReader(stream, Encoding.Default, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: true);

                Serialize<T>(value, stream);

                stream.Position = 0;

                return reader.ReadToEnd();
            }
            finally
            {
                stream?.Dispose();
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            return _dataSerializer.Deserialize<T>(stream);
        }

        public T Deserialize<T>(string value)
        {
            var stream = new MemoryStream();

            try
            {
                using var writer = new StreamWriter(stream, Encoding.Default, bufferSize: 1024, leaveOpen: true);

                writer.Write(value);
                writer.Flush();

                stream.Position = 0;

                return Deserialize<T>(stream);
            }
            finally
            {
                stream?.Dispose();
            }
        }

        private Func<object, Json.Expression> SerializeConstantFactory(Type type)
        {
            return obj => SerializeConstant(obj, type);
        }

        private Json.Expression SerializeConstant(object obj, Type type)
        {
            string json;

            var stream = new MemoryStream();

            try
            {
                using var reader = new StreamReader(stream);

                stream = null;

                var serialize = _genericSerialize.MakeGenericMethod(new[] { type });
                serialize.Invoke(_dataSerializer, new[] { obj, reader.BaseStream as MemoryStream });

                ((MemoryStream)reader.BaseStream).Position = 0;

                json = reader.ReadToEnd();
            }
            finally
            {
                stream?.Dispose();
            }

            return Json.Expression.Parse(json, ensureTopLevelObjectOrArray: false);
        }

        private Func<Json.Expression, object> DeserializeConstantFactory(Type type)
        {
            return json => DeserializeConstant(json, type);
        }

        private object DeserializeConstant(Json.Expression json, Type type)
        {
            object result;

            var stream = new MemoryStream();

            try
            {
                using var writer = new StreamWriter(stream);

                stream = null;

                var deserialize = _genericDeserialize.MakeGenericMethod(new[] { type });

                writer.Write(json.ToString());
                writer.Flush();

                ((MemoryStream)writer.BaseStream).Position = 0;

                result = deserialize.Invoke(_dataSerializer, new[] { writer.BaseStream as MemoryStream });
            }
            finally
            {
                stream?.Dispose();
            }

            return result;
        }
    }

    internal delegate IExpressionSerializer GetSerializer(Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory);
}
