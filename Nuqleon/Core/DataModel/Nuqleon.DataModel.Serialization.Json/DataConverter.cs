// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Provides an extensibility point to convert objects of custom types during serialization and deserialization.
    /// </summary>
    public abstract class DataConverter
    {
        /// <summary>
        /// Creates a new DataConverter instance.
        /// </summary>
        protected DataConverter() => Converter = new ConverterImpl(this);

        internal JsonConverter Converter { get; }

        /// <summary>
        /// Checks whether objects of specified type can be converted into objects of some target type.
        /// This method is called during serialization using the runtime type of the object to serialize,
        /// and during deserialization using the static type of the assignment target.
        /// </summary>
        /// <param name="fromType">Type to check for a suitable conversion.</param>
        /// <param name="targetType">Type suitable for conversion of objects of the original type.</param>
        /// <returns>true if the data converter can convert instances of the specified type; otherwise, false.</returns>
        public abstract bool TryCanConvert(Type fromType, out Type targetType);

        /// <summary>
        /// Converts an object of the specified source type to the corresponding target type during serialization.
        /// </summary>
        /// <param name="value">Object to convert to the specified target type.</param>
        /// <param name="sourceType">Type of the source object.</param>
        /// <param name="targetType">Type to convert the object into.</param>
        /// <returns>Conversion of the given object to the specified target type, used for further serialization.</returns>
        public abstract object ConvertTo(object value, Type sourceType, Type targetType);

        /// <summary>
        /// Converts an object of the specified source type to the corresponding target type during deserialization.
        /// </summary>
        /// <param name="value">Object to convert to the specified target type.</param>
        /// <param name="sourceType">Type of the source object.</param>
        /// <param name="targetType">Type to convert the object into.</param>
        /// <returns>Conversion of the given object to the specified target type, used for further deserialization.</returns>
        public abstract object ConvertFrom(object value, Type sourceType, Type targetType);

        private sealed class ConverterImpl : JsonConverter
        {
            private readonly DataConverter _parent;
            private readonly ConcurrentDictionary<Type, Type> _typeMap;

            public ConverterImpl(DataConverter parent)
            {
                _parent = parent;
                _typeMap = new ConcurrentDictionary<Type, Type>();
            }

            public override bool CanConvert(Type objectType)
            {
                if (_typeMap.ContainsKey(objectType))
                {
                    return true;
                }

                var res = _parent.TryCanConvert(objectType, out var to);
                if (res)
                {
                    var mappedType = _typeMap.GetOrAdd(objectType, to);
                    if (mappedType != to)
                    {
                        throw new InvalidOperationException(
                            string.Format(CultureInfo.InvariantCulture, "Type '{0}' has already been mapped to type '{1}'; cannot convert to type '{2}'.", objectType, mappedType, to));
                    }
                }

                return res;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Debug.Assert(reader != null, nameof(reader));
                Debug.Assert(objectType != null, nameof(objectType));
                Debug.Assert(serializer != null, nameof(serializer));

                if (_typeMap.TryGetValue(objectType, out var to))
                {
                    var res = serializer.Deserialize(reader, to);
                    return _parent.ConvertFrom(res, to, objectType);
                }

                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Debug.Assert(writer != null, nameof(writer));
                Debug.Assert(serializer != null, nameof(serializer));

                if (value == null)
                {
                    writer.WriteNull();
                    return;
                }

                var type = value.GetType();
                if (_typeMap.TryGetValue(type, out var to))
                {
                    var res = _parent.ConvertTo(value, type, to);
                    serializer.Serialize(writer, res);
                    return;
                }

                throw new NotImplementedException();
            }
        }
    }
}
