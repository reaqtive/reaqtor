// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    /// <summary>
    /// Provides simple object serializer functions for primitive types.
    /// </summary>
    public class ObjectSerializer
    {
        /// <summary>
        /// Gets a serializer for the given type.
        /// </summary>
        /// <param name="type">Type to get a serializer for.</param>
        /// <returns>Object serializer for instances of the specified type.</returns>
        public Func<object, Json.Expression> GetSerializer(Type type)
        {
            return obj => SerializeObject(obj, type);
        }

        /// <summary>
        /// Gets a serializer for the given type that returns instances of a JSON object representation.
        /// </summary>
        /// <param name="type">Type to get a serializer for.</param>
        /// <returns>Object serializer for instances of the specified type.</returns>
        public Func<object, object> GetJsonSerializer(Type type)
        {
            return obj => SerializeObject(obj, type);
        }

        /// <summary>
        /// Gets a deserializer for the given type.
        /// </summary>
        /// <param name="type">Type to get a deserializer for.</param>
        /// <returns>Object deserializer for instances of the specified type.</returns>
        public Func<Json.Expression, object> GetDeserializer(Type type)
        {
            return obj => DeserializeObject(obj, type);
        }

        /// <summary>
        /// Gets a deserializer for the given type which expected JSON instances.
        /// </summary>
        /// <param name="type">Type to get a deserializer for.</param>
        /// <returns>Object deserializer for instances of the specified type.</returns>
        public Func<object, object> GetJsonDeserializer(Type type)
        {
            return obj => DeserializeObject((Json.Expression)obj, type);
        }

        private static readonly Dictionary<Type, Func<Json.Expression, object>> s_deserializeObjectSlow = new()
        {
#if NO_INLINEPRIMITIVES
            { typeof(Double), json => double.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(Int32), json => int.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(String), json => (string)((Json.ConstantExpression)json).Value },
            { typeof(Boolean), json => (bool)((Json.ConstantExpression)json).Value },
#endif
            { typeof(Single), json => float.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(Int64), json => long.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(SByte), json => sbyte.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(Byte), json => byte.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(Int16), json => short.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(UInt16), json => ushort.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(UInt32), json => uint.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(UInt64), json => ulong.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture) },
            { typeof(DateTimeOffset), json => DateTimeOffset.ParseExact((string)((Json.ConstantExpression)json).Value, @"o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) },
            { typeof(DateTime), json => DateTime.ParseExact((string)((Json.ConstantExpression)json).Value, @"o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) },
            { typeof(TimeSpan), json => TimeSpan.ParseExact((string)((Json.ConstantExpression)json).Value, @"c", CultureInfo.InvariantCulture) },
            { typeof(Guid), json => Guid.ParseExact((string)((Json.ConstantExpression)json).Value, @"B") },
        };

        private static readonly Dictionary<Type, Func<object, Json.Expression>> s_serializeObjectSlow = new()
        {
#if NO_INLINEPRIMITIVES
            { typeof(Double), obj => Json.Expression.Number(((double)obj).ToString("R")) },
            { typeof(Int32), obj => Json.Expression.Number(((int)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(String), obj => Json.Expression.String((string)obj) },
            { typeof(Boolean), obj => (bool)obj ? Json.Expression.Boolean(true) : Json.Expression.Boolean(false) },
#endif
            { typeof(Single), obj => Json.Expression.Number(((float)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(Int64), obj => Json.Expression.Number(((long)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(SByte), obj => Json.Expression.Number(((sbyte)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(Byte), obj => Json.Expression.Number(((byte)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(Int16), obj => Json.Expression.Number(((short)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(UInt16), obj => Json.Expression.Number(((ushort)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(UInt32), obj => Json.Expression.Number(((uint)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(UInt64), obj => Json.Expression.Number(((ulong)obj).ToString(CultureInfo.InvariantCulture)) },
            { typeof(DateTimeOffset), obj => Json.Expression.String(((DateTimeOffset)obj).ToString(@"o", CultureInfo.InvariantCulture)) },
            { typeof(DateTime), obj => Json.Expression.String(((DateTime)obj).ToString(@"o", CultureInfo.InvariantCulture)) },
            { typeof(TimeSpan), obj => Json.Expression.String(((TimeSpan)obj).ToString(@"c", CultureInfo.InvariantCulture)) },
            { typeof(Guid), obj => Json.Expression.String(((Guid)obj).ToString(@"B")) },
        };

        private object DeserializeObject(Json.Expression json, Type type)
        {
            switch (json.NodeType)
            {
                case Json.ExpressionType.Null:
                    return null;
#if !NO_INLINEPRIMITIVES
                case Json.ExpressionType.Boolean:
                    return (bool)((Json.ConstantExpression)json).Value;
                case Json.ExpressionType.String:
                    if (type == typeof(string))
                    {
                        return (string)((Json.ConstantExpression)json).Value;
                    }
                    else
                    {
                        if (s_deserializeObjectSlow.TryGetValue(type, out Func<Json.Expression, object> deserialize))
                        {
                            return deserialize(json);
                        }
                    }
                    break;
                case Json.ExpressionType.Number:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GetGenericArguments()[0];
                    }

                    if (type == typeof(double))
                    {
                        return double.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture);
                    }
                    else if (type == typeof(int))
                    {
                        return int.Parse((string)((Json.ConstantExpression)json).Value, CultureInfo.InvariantCulture);
                    }
                    else if (type.IsEnum)
                    {
                        var value = DeserializeObject(json, Enum.GetUnderlyingType(type));
                        return Enum.ToObject(type, value);
                    }
                    else
                    {
#else
                default:
                    {
#endif
                        if (s_deserializeObjectSlow.TryGetValue(type, out Func<Json.Expression, object> deserialize))
                        {
                            return deserialize(json);
                        }
                    }
                    break;
            }

            throw new NotImplementedException();
        }

        private Json.Expression SerializeObject(object obj, Type type)
        {
            if (obj is null)
            {
                return Json.Expression.Null();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return SerializeObject(type.GetProperty("Value").GetValue(obj), type.GetGenericArguments()[0]);
            }
            else
            {
#if !NO_INLINEPRIMITIVES
                if (type == typeof(double))
                {
                    return Json.Expression.Number(((double)obj).ToString("R", CultureInfo.InvariantCulture));
                }
                else if (type == typeof(int))
                {
                    return ((int)obj).ToJsonNumber();
                }
                else if (type == typeof(string))
                {
                    return Json.Expression.String((string)obj);
                }
                else if (type == typeof(bool))
                {
                    return (bool)obj ? Json.Expression.Boolean(true) : Json.Expression.Boolean(false);
                }
                else if (type.IsEnum)
                {
                    var underlyingType = Enum.GetUnderlyingType(type);
                    var value = Convert.ChangeType(obj, underlyingType, CultureInfo.InvariantCulture);
                    return SerializeObject(value, underlyingType);
                }
                else
#endif
                {
                    if (s_serializeObjectSlow.TryGetValue(type, out Func<object, Json.Expression> serialize))
                    {
                        return serialize(obj);
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
