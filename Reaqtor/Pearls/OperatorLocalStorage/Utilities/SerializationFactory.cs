// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Nuqleon.DataModel;

using Nuqleon.Json.Serialization;

using Reaqtive.Serialization;

namespace Utilities
{
    /// <summary>
    /// Implementation of <see cref="ISerializationFactory"/> using the fast JSON serializer.
    /// </summary>
    public sealed class SerializationFactory : ISerializationFactory
    {
        /// <summary>
        /// Cache of serializers by type.
        /// </summary>
        private readonly ConditionalWeakTable<Type, object> _serializers = new();

        /// <summary>
        /// Cache of deserializers by type.
        /// </summary>
        private readonly ConditionalWeakTable<Type, object> _deserializers = new();

        /// <summary>
        /// The reflection object for <see cref="GetSerializer{T}"/>.
        /// </summary>
        private static readonly MethodInfo s_GetSerializer = typeof(SerializationFactory).GetMethod(nameof(GetSerializer));

        /// <summary>
        /// The reflection object for <see cref="GetDeserializer{T}"/>.
        /// </summary>
        private static readonly MethodInfo s_GetDeserializer = typeof(SerializationFactory).GetMethod(nameof(GetDeserializer));

        /// <summary>
        /// Gets a serializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <returns>A serializer for objects of type <typeparamref name="T"/>.</returns>
        public ISerializer<T> GetSerializer<T>()
        {
            if (_serializers.TryGetValue(typeof(T), out var res))
            {
                return (ISerializer<T>)res;
            }

            return GetSerializerSlow<T>();
        }

        /// <summary>
        /// Gets a serializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <returns>A serializer for objects of type <typeparamref name="T"/>.</returns>
        /// <remarks>This method is called from <see cref="GetSerializer{T}"/> to avoid allocating a delegate.</remarks>
        private ISerializer<T> GetSerializerSlow<T>()
        {
            return (ISerializer<T>)_serializers.GetValue(typeof(T), type => GetSerializerCore<T>());
        }

        /// <summary>
        /// Creates a serializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <returns>A serializer for objects of type <typeparamref name="T"/>.</returns>
        private ISerializer<T> GetSerializerCore<T>()
        {
            if (TryGetKeyValuePairEntity(typeof(T), out var keyValueEntityType))
            {
                var projectingSerializerType = typeof(ProjectingSerializer<,>).MakeGenericType(typeof(T), keyValueEntityType);

                object entitySerializer;

                try
                {
                    entitySerializer = s_GetSerializer.MakeGenericMethod(keyValueEntityType).Invoke(this, Array.Empty<object>());
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }

                var selectorType = typeof(Func<,>).MakeGenericType(typeof(T), keyValueEntityType);

                var parameter = Expression.Parameter(typeof(T));

                var selectorExpression =
                    Expression.Lambda(
                        selectorType,
                        Expression.MemberInit(
                            Expression.New(keyValueEntityType),
                            Expression.Bind(
                                keyValueEntityType.GetProperty("key"),
                                Expression.Property(parameter, "Key")
                            ),
                            Expression.Bind(
                                keyValueEntityType.GetProperty("value"),
                                Expression.Property(parameter, "Value")
                            )
                        ),
                        parameter
                    );

                var selector = selectorExpression.Compile();

                return (ISerializer<T>)Activator.CreateInstance(projectingSerializerType, new object[] { selector, entitySerializer });
            }

            return new Serializer<T>();
        }

        /// <summary>
        /// Gets a deserializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        /// <returns>A deserializer for objects of type <typeparamref name="T"/>.</returns>
        public IDeserializer<T> GetDeserializer<T>()
        {
            if (_deserializers.TryGetValue(typeof(T), out var res))
            {
                return (IDeserializer<T>)res;
            }

            return GetDeserializerSlow<T>();
        }

        /// <summary>
        /// Gets a deserializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        /// <returns>A deserializer for objects of type <typeparamref name="T"/>.</returns>
        /// <remarks>This method is called from <see cref="GetDeserializer{T}"/> to avoid allocating a delegate.</remarks>
        private IDeserializer<T> GetDeserializerSlow<T>()
        {
            return (IDeserializer<T>)_deserializers.GetValue(typeof(T), type => GetDeserializerCore<T>());
        }

        /// <summary>
        /// Creates a deserializer for objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        /// <returns>A deserializer for objects of type <typeparamref name="T"/>.</returns>
        private IDeserializer<T> GetDeserializerCore<T>()
        {
            if (TryGetKeyValuePairEntity(typeof(T), out var keyValueEntityType))
            {
                var projectingDeserializerType = typeof(ProjectingDeserializer<,>).MakeGenericType(typeof(T), keyValueEntityType);

                object entityDeserializer;

                try
                {
                    entityDeserializer = s_GetDeserializer.MakeGenericMethod(keyValueEntityType).Invoke(this, Array.Empty<object>());
                }
                catch (TargetInvocationException tie)
                {
                    throw tie.InnerException;
                }

                var selectorType = typeof(Func<,>).MakeGenericType(keyValueEntityType, typeof(T));

                var parameter = Expression.Parameter(keyValueEntityType);

                var selectorExpression =
                    Expression.Lambda(
                        selectorType,
                        Expression.New(
                            typeof(T).GetConstructors()[0],
                            Expression.Property(parameter, "key"),
                            Expression.Property(parameter, "value")
                        ),
                        parameter
                    );

                var selector = selectorExpression.Compile();

                return (IDeserializer<T>)Activator.CreateInstance(projectingDeserializerType, new object[] { selector, entityDeserializer });
            }

            return new Deserializer<T>();
        }

        private static bool TryGetKeyValuePairEntity(Type type, out Type keyValueEntityType)
        {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                var args = type.GetGenericArguments();

                keyValueEntityType = RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("key", args[0]), new KeyValuePair<string, Type>("value", args[1]) }, valueEquality: true);

                return true;
            }

            keyValueEntityType = null;
            return false;
        }

        private sealed class Serializer<T> : ISerializer<T>
        {
            private readonly IFastJsonSerializer<T> _serializer;

            public Serializer()
            {
                _serializer = FastJsonSerializerFactory.CreateSerializer<T>(NameProvider.Instance, FastJsonConcurrencyMode.ThreadSafe);
            }

            public void Serialize(T value, Stream stream)
            {
                var json = _serializer.Serialize(value);

                using var sw = new StreamWriter(stream);

                sw.Write(json);
            }
        }

        private sealed class Deserializer<T> : IDeserializer<T>
        {
            private readonly IFastJsonDeserializer<T> _deserializer;

            public Deserializer()
            {
                _deserializer = FastJsonSerializerFactory.CreateDeserializer<T>(NameResolver.Instance, FastJsonConcurrencyMode.ThreadSafe);
            }

            public T Deserialize(Stream stream)
            {
                var json = default(string);

                using (var sr = new StreamReader(stream))
                {
                    json = sr.ReadToEnd();
                }

                return _deserializer.Deserialize(json);
            }
        }

        private sealed class NameProvider : INameProvider
        {
            public static readonly NameProvider Instance = new();

            public string GetName(PropertyInfo property) => GetName((MemberInfo)property);

            public string GetName(FieldInfo field) => GetName((MemberInfo)field);

            private static string GetName(MemberInfo member) => member.GetCustomAttribute<MappingAttribute>()?.Uri ?? member.Name;
        }

        private sealed class NameResolver : INameResolver
        {
            public static readonly NameResolver Instance = new();

            public IEnumerable<string> GetNames(PropertyInfo property) => GetNames((MemberInfo)property);

            public IEnumerable<string> GetNames(FieldInfo field) => GetNames((MemberInfo)field);

            private static IEnumerable<string> GetNames(MemberInfo member) => new[] { member.GetCustomAttribute<MappingAttribute>()?.Uri ?? member.Name };
        }

        private sealed class ProjectingSerializer<TInput, TOutput> : ISerializer<TInput>
        {
            private readonly Func<TInput, TOutput> _selector;
            private readonly ISerializer<TOutput> _serializer;

            public ProjectingSerializer(Func<TInput, TOutput> selector, ISerializer<TOutput> serializer)
            {
                _selector = selector;
                _serializer = serializer;
            }

            public void Serialize(TInput value, Stream stream) => _serializer.Serialize(_selector(value), stream);
        }

        private sealed class ProjectingDeserializer<TInput, TOutput> : IDeserializer<TInput>
        {
            private readonly Func<TOutput, TInput> _selector;
            private readonly IDeserializer<TOutput> _deserializer;

            public ProjectingDeserializer(Func<TOutput, TInput> selector, IDeserializer<TOutput> deserializer)
            {
                _selector = selector;
                _deserializer = deserializer;
            }

            public TInput Deserialize(Stream stream) => _selector(_deserializer.Deserialize(stream));
        }
    }
}
