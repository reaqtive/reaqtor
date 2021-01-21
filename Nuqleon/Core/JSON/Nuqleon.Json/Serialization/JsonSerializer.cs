// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2009 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Nuqleon.Json.Serialization
{
    using Expressions;
    using Internal;

    /// <summary>
    /// JSON serializer and deserializer.
    /// </summary>
    public sealed class JsonSerializer
    {
        #region Private fields

        /// <summary>
        /// Type used for deserialization.
        /// </summary>
        private readonly Type _type;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new JSON (de)serializer.
        /// </summary>
        /// <param name="type">Type used in deserialization.</param>
        public JsonSerializer(Type type) => _type = type ?? throw new ArgumentNullException(nameof(type));

        #endregion

        #region Public methods

        /// <summary>
        /// Deserializes the given JSON expression into a .NET object.
        /// </summary>
        /// <param name="expression">JSON expression to deserialize.</param>
        /// <returns>Object corresponding to the deserialized JSON expression.</returns>
        public object Deserialize(Expression expression) => Deserialize(expression ?? throw new ArgumentNullException(nameof(expression)), _type);

        /// <summary>
        /// Deserializes the given JSON text into a .NET object.
        /// </summary>
        /// <param name="input">JSON text to deserialize.</param>
        /// <returns>Object corresponding to the deserialized JSON text.</returns>
        /// <remarks>See RFC 4627 for more information. This parses the production specified in section 2: <code>JSON-text = object / array</code>.</remarks>
        public object Deserialize(string input) => Deserialize(Expression.Parse(input));

        /// <summary>
        /// Serializes the given .NET object into JSON text.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>JSON text corresponding to the given .NET object.</returns>
        public string Serialize(object value)
        {
            Stack<object> refs = new Stack<object>();
            return Serialize(value, refs).ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Recursive serialization method.
        /// </summary>
        /// <param name="o">Object to serialize.</param>
        /// <param name="refs">Stack to keep track of objects being serialized, to spot cycles.</param>
        /// <returns>JSON expression tree for the serialized object.</returns>
        private Expression Serialize(object o, Stack<object> refs)
        {
            if (o == null)
            {
                return Expression.Null();
            }

            var type = o.GetType();

            // Nullables are weird. If they're not assigned null, we'll see their underlying value type here.
            // The null check above takes care of our untyped null case.

            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }

            if (type == typeof(string))
            {
                return Expression.String((string)o);
            }
            else if (type == typeof(bool))
            {
                return Expression.Boolean((bool)o);
            }
            else if (type == typeof(int))
            {
                return Expression.Number(((int)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(uint))
            {
                return Expression.Number(((uint)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(byte))
            {
                return Expression.Number(((byte)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(sbyte))
            {
                return Expression.Number(((sbyte)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(long))
            {
                return Expression.Number(((long)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(ulong))
            {
                return Expression.Number(((ulong)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(short))
            {
                return Expression.Number(((short)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(ushort))
            {
                return Expression.Number(((ushort)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(float))
            {
                return Expression.Number(((float)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(double))
            {
                return Expression.Number(((double)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type == typeof(decimal))
            {
                return Expression.Number(((decimal)o).ToString(CultureInfo.InvariantCulture));
            }
            else if (type.IsArray || type.GetInterface("System.Collections.Generic.IList`1", ignoreCase: false) != null || typeof(IList).IsAssignableFrom(type))
            {
                return SerializeArray(o, refs);
            }
            else
            {
                return SerializeObject(o, refs);
            }
        }

        /// <summary>
        /// Serializes an array.
        /// </summary>
        /// <param name="o">Array to be serialized.</param>
        /// <param name="refs">Stack to keep track of objects being serialized, to spot cycles.</param>
        /// <returns>JSON expression tree for the serialized array.</returns>
        private Expression SerializeArray(object o, Stack<object> refs)
        {
            if (refs.Contains(o))
            {
                throw new SerializationException("Detected object reference cycle during serialization.");
            }

            refs.Push(o);

            var elements = new List<Expression>();
            foreach (object element in (IEnumerable)o)
            {
                elements.Add(Serialize(element, refs));
            }

            refs.Pop();

            return Expression.Array(elements);
        }

        /// <summary>
        /// Serializes a .NET object in terms of its properties and fields.
        /// </summary>
        /// <param name="o">Object to be serialized</param>
        /// <param name="refs">Stack to keep track of objects being serialized, to spot cycles.</param>
        /// <returns>JSON expression tree for the serialized object.</returns>
        private Expression SerializeObject(object o, Stack<object> refs)
        {
            if (refs.Contains(o))
            {
                throw new SerializationException("Detected object reference cycle during serialization.");
            }

            refs.Push(o);

            var members = new Dictionary<string, Expression>();

            var type = o.GetType();

            var iDict = type.GetInterface("System.Collections.Generic.IDictionary`2", ignoreCase: false);
            if (iDict != null)
            {
                if (iDict.GetGenericArguments()[0] != typeof(string))
                    throw new SerializationException("Dictionary objects can only be serialized if a string key is used.");

                var keys = (IEnumerable)iDict.GetProperty("Keys").GetValue(o, index: null);
                var index = iDict.GetProperty("Item");

                foreach (string key in keys)
                {
                    object value = index.GetValue(o, new[] { key });
                    members.Add(key, Serialize(value, refs));
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                var dict = (IDictionary)o;
                foreach (var key in dict.Keys)
                {
                    if (key is not string keyString)
                        throw new SerializationException("Dictionary objects can only be serialized if a string key is used.");

                    members.Add(keyString, Serialize(dict[key], refs));
                }
            }
            else
            {
                var res = (from field in type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                           select (Key: field.Name, Value: Serialize(field.GetValue(o), refs)))
                          .Concat(
                           from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           where property.CanRead
                           select (Key: property.Name, Value: Serialize(property.GetValue(o, index: null), refs)));

                foreach (var (Key, Value) in res)
                {
                    members.Add(Key, Value);
                }
            }

            refs.Pop();

            return Expression.Object(members);
        }

        /// <summary>
        /// Deserializes the given JSON expression to an object of the given type.
        /// </summary>
        /// <param name="jsonExpression">JSON expression to deserialize.</param>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object Deserialize(Expression jsonExpression, Type type)
        {
            switch (jsonExpression.NodeType)
            {
                case ExpressionType.Object:
                    return DeserializeObject((ObjectExpression)jsonExpression, type);
                case ExpressionType.Array:
                    return DeserializeArray((ArrayExpression)jsonExpression, type);
                case ExpressionType.Boolean:
                    return DeserializeBoolean((ConstantExpression)jsonExpression, type);
                case ExpressionType.Number:
                    return DeserializeNumber((ConstantExpression)jsonExpression, type);
                case ExpressionType.String:
                    return DeserializeString((ConstantExpression)jsonExpression, type);
                case ExpressionType.Null:
                    return DeserializeNull(type);
                default:
                    Debug.Assert(false, "Incomplete switch on ExpressionType.");
                    throw new NotSupportedException("Unknown expression tree node type.");
            }
        }

        /// <summary>
        /// Deserializes the given JSON object expression to an object of the given type.
        /// </summary>
        /// <param name="objectExpression">JSON object expression to deserialize.</param>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object DeserializeObject(ObjectExpression objectExpression, Type type)
        {
            if (type == typeof(object))
            {
                type = typeof(Dictionary<string, object>);
            }

            var res = Activator.CreateInstance(type);

            var iDict = type.GetInterface("System.Collections.Generic.IDictionary`2", ignoreCase: false);
            if (iDict != null)
            {
                var args = iDict.GetGenericArguments();

                if (args[0] != typeof(string) && args[0] != typeof(object))
                    throw new SerializationException("Objects can only be deserialized to generic dictionary types if a string key is used.");

                Type valuesType = args[1];

                var add = iDict.GetMethod("Add");

                foreach (var member in objectExpression.Members)
                {
                    var memberName = member.Key;
                    add.Invoke(res, new object[] { memberName, Deserialize(member.Value, valuesType) });
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                var iDictObj = (IDictionary)res;

                foreach (var member in objectExpression.Members)
                {
                    var memberName = member.Key;
                    iDictObj.Add(memberName, Deserialize(member.Value, typeof(object)));
                }
            }
            else
            {
                foreach (var member in objectExpression.Members)
                {
                    var memberName = member.Key;

                    var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance);
                    var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);

                    if (property != null)
                    {
                        if (property.GetSetMethod(nonPublic: false) == null)
                            throw new SerializationException("Property '" + memberName + "' on " + type + " is not settable.");

                        try
                        {
                            property.SetValue(res, Deserialize(member.Value, property.PropertyType), index: null);
                        }
                        catch (TargetInvocationException ex)
                        {
                            throw new SerializationException("Failed to set property '" + property.Name + "' on " + type + ".", ex.InnerException);
                        }
                    }
                    else if (field != null)
                    {
                        field.SetValue(res, Deserialize(member.Value, field.FieldType));
                    }
                    else
                    {
                        throw new SerializationException("Can't find property or field '" + memberName + "' on " + type + ".");
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Deserializes the given JSON array expression to an object of the given type.
        /// </summary>
        /// <param name="arrayExpression">JSON array expression to deserialize.</param>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object DeserializeArray(ArrayExpression arrayExpression, Type type)
        {
            if (type == typeof(object))
            {
                type = typeof(List<object>);
            }

            if (type.IsArray) // Do this first. Array implements IList<T> but instantiating an Array as such causes issues.
            {
                if (type.GetArrayRank() != 1)
                    throw new SerializationException("Array deserialization is only supported for single-dimensional arrays.");

                Type elementType = type.GetElementType();
                int count = arrayExpression.ElementCount;
                var array = Array.CreateInstance(elementType, count);

                for (int i = 0; i < count; i++)
                {
                    array.SetValue(Deserialize(arrayExpression.GetElement(i), elementType), i);
                }

                return array;
            }

            var iList = type.GetInterface("System.Collections.Generic.IList`1", ignoreCase: false);
            if (iList != null)
            {
                Type elementType = iList.GetGenericArguments()[0];
                var list = Activator.CreateInstance(type);

                var iColl = type.GetInterface("System.Collections.Generic.ICollection`1", ignoreCase: false); // static guarantee IList<T> : ICollection<T>
                var add = iColl.GetMethod("Add");

                for (int i = 0, n = arrayExpression.ElementCount; i < n; i++)
                {
                    add.Invoke(list, new object[] { Deserialize(arrayExpression.GetElement(i), elementType) });
                }

                return list;
            }
            else if (typeof(IList).IsAssignableFrom(type))
            {
                var list = (IList)Activator.CreateInstance(type);

                for (int i = 0, n = arrayExpression.ElementCount; i < n; i++)
                {
                    list.Add(Deserialize(arrayExpression.GetElement(i), typeof(object)));
                }

                return list;
            }
            else
            {
                throw new SerializationException("Can't deserialize an array into a non-array or list type.");
            }
        }

        /// <summary>
        /// Deserializes the given JSON constant Boolean expression to an object of the given type.
        /// </summary>
        /// <param name="constantExpression">JSON constant Boolean expression to deserialize.</param>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object DeserializeBoolean(ConstantExpression constantExpression, Type type)
        {
            if (type != typeof(object) && type != typeof(bool) && type != typeof(bool?))
                throw new SerializationException("Can't coerce a Boolean into a " + type + ".");

            return constantExpression.Value;
        }

        /// <summary>
        /// Deserializes the given JSON constant string expression to an object of the given type.
        /// </summary>
        /// <param name="constantExpression">JSON constant string expression to deserialize.</param>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object DeserializeString(ConstantExpression constantExpression, Type type)
        {
            var value = (string)constantExpression.Value;

            if (type.IsEnum)
            {
                return Enum.Parse(type, value, ignoreCase: false);
            }

            if (type != typeof(object) && type != typeof(string))
                throw new SerializationException("Can't coerce a String into a " + type + ".");

            return value;
        }

        /// <summary>
        /// Deserializes the given JSON constant numeric expression to an object of the given type.
        /// </summary>
        /// <param name="constantExpression">JSON constant numeric expression to deserialize.</param>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object DeserializeNumber(ConstantExpression constantExpression, Type type)
        {
            var value = (string)constantExpression.Value;

            if (type == typeof(object) || type == typeof(string))
            {
                return value; // Arbitrary numbers keep their string representation. Don't know what to coerce to :-(.
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, value, ignoreCase: false);
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                return int.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(uint) || type == typeof(uint?))
            {
                return uint.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(byte) || type == typeof(byte?))
            {
                return byte.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(sbyte) || type == typeof(sbyte?))
            {
                return sbyte.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                return long.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(ulong) || type == typeof(ulong?))
            {
                return ulong.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(short) || type == typeof(short?))
            {
                return short.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(ushort) || type == typeof(ushort?))
            {
                return ushort.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                return float.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                return double.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return decimal.Parse(value, NumberFormatInfo.InvariantInfo);
            }
            else
                throw new SerializationException("Can't coerce a Number into a " + type + ".)");
        }

        /// <summary>
        /// Deserializes the JSON null literal to an object of the given type.
        /// </summary>
        /// <param name="type">Type to deserialize to.</param>
        /// <returns>Deserialized object for the JSON expression tree.</returns>
        private static object DeserializeNull(Type type)
        {
            if (type.IsValueType && !type.IsNullable())
                throw new SerializationException("Can't deserialize null value to non-nullable type.");

            return null;
        }

        #endregion
    }
}
