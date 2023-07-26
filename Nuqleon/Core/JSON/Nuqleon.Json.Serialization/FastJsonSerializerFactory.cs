// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 04/23/2016 - Added concurrency modes.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Factory for fast JSON serializers.
    /// </summary>
    public static partial class FastJsonSerializerFactory
    {
        /// <summary>
        /// Creates a JSON serializer specialized for objects of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <param name="provider">Name provider used to determine JSON object keys for properties and fields.</param>
        /// <param name="concurrencyMode">The intended concurrency usage pattern for a fast JSON serializer or deserializer.</param>
        /// <returns>A fast JSON serializer specified for objects of the specified type <typeparamref name="T"/>.</returns>
        public static IFastJsonSerializer<T> CreateSerializer<T>(INameProvider provider, FastJsonConcurrencyMode concurrencyMode) => CreateSerializer<T>(provider, new FastJsonSerializerSettings(concurrencyMode));

        /// <summary>
        /// Creates a JSON serializer specialized for objects of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to serialize.</typeparam>
        /// <param name="provider">Name provider used to determine JSON object keys for properties and fields.</param>
        /// <param name="settings">The settings to configure the resulting serializer.</param>
        /// <returns>A fast JSON serializer specified for objects of the specified type <typeparamref name="T"/>.</returns>
        public static IFastJsonSerializer<T> CreateSerializer<T>(INameProvider provider, FastJsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            provider ??= DefaultNameProvider.Instance;

            //
            // CONSIDER: Caching of serializers per type in a ConditionalWeakTable. However, we have to ensure different configurations
            //           don't get mixed up (e.g. based on providers).
            //
            // CONSIDER: Adding support for registration of custom serializers for given types. These would follow the EmitAction delegate
            //           and append an object of the given type to the StringBuilder.
            //

            var builderString = new EmitterStringBuilder(provider);
            var emitterString = builderString.Build<T>();

#if !NO_IO
            var builderWriter = new EmitterWriterBuilder(provider);
            var emitterWriter = builderWriter.Build<T>();

            if (settings.ConcurrencyMode == FastJsonConcurrencyMode.SingleThreaded)
            {
                return new Serializer<T>(emitterString, builderString, emitterWriter, builderWriter);
            }
            else
            {
                return new SafeSerializer<T>(emitterString, builderString, emitterWriter, builderWriter);
            }
#else
            if (settings.ConcurrencyMode == FastJsonConcurrencyMode.SingleThreaded)
            {
                return new Serializer<T>(emitterString, builderString);
            }
            else
            {
                return new SafeSerializer<T>(emitterString, builderString);
            }
#endif
        }

        /// <summary>
        /// Creates a JSON deserializer specialized for objects of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        /// <param name="resolver">Name resolver used to find JSON object keys for properties and fields.</param>
        /// <param name="concurrencyMode">The intended concurrency usage pattern for a fast JSON serializer or deserializer.</param>
        /// <returns>A fast JSON deserializer specified for objects of the specified type <typeparamref name="T"/>.</returns>
        public static IFastJsonDeserializer<T> CreateDeserializer<T>(INameResolver resolver, FastJsonConcurrencyMode concurrencyMode) => CreateDeserializer<T>(resolver, new FastJsonSerializerSettings(concurrencyMode));

        /// <summary>
        /// Creates a JSON deserializer specialized for objects of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
        /// <param name="resolver">Name resolver used to find JSON object keys for properties and fields.</param>
        /// <param name="settings">The settings to configure the resulting deserializer.</param>
        /// <returns>A fast JSON deserializer specified for objects of the specified type <typeparamref name="T"/>.</returns>
        public static IFastJsonDeserializer<T> CreateDeserializer<T>(INameResolver resolver, FastJsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            resolver ??= DefaultNameResolver.Instance;

            //
            // CONSIDER: Caching of deserializers per type in a ConditionalWeakTable. However, we have to ensure different configurations
            //           don't get mixed up (e.g. based on resolvers).
            //
            // CONSIDER: Adding support for registration of custom deserializers for given types. These would accept a "natural" representation
            //           of a JSON primitive and perform conversions on them, e.g. take a String and return an instance of type T. There's a
            //           couple of options to support this:
            //
            //           1. Accept a Func<string, T>, allocate a string containing a JSON decoded string, and hand it to the extension. This
            //              approach requires allocations but is useful if the string has to be copied anyway, e.g. for a Uri.
            //
            //           2. Accept a Func<string, T>, scan for the length of a JSON decoded string, grab an instance from the pool, and hand
            //              it to the extension. This avoids allocations but assumes the callback doesn't extend the lifetime of the string
            //              beyond the synchronous invocation. It is useful for things such as TNumeric.Parse.
            //
            //           3. Accept a Func<TryLexChar, T>, where TryLexChar is a delegate with signature TryFunc<char>, i.e. returning a Char
            //              in an output parameter and returning a Boolean to indicate whether a value is available. The lex function stops
            //              yielding characters upon encountering the closing " of the string literal, and decodes the JSON characters as
            //              it's lexing. If we want to avoid allocations of delegates and closures, we need to assume that the callback does
            //              not extend the lifetime of the lex function beyond the synchronous invocation. This would be useful for things
            //              such as parsing a version number.
            //

            var builderString = new ParserStringBuilder(resolver);
            var parseString = builderString.Build<T>();

#if !NO_IO
            var builderReader = new ParserReaderBuilder(resolver);
            var parseReader = builderReader.Build<T>();

            if (settings.ConcurrencyMode == FastJsonConcurrencyMode.SingleThreaded)
            {
                return new Deserializer<T>(parseString, parseReader);
            }
            else
            {
                return new SafeDeserializer<T>(parseString, parseReader);
            }
#else
            if (settings.ConcurrencyMode == FastJsonConcurrencyMode.SingleThreaded)
            {
                return new Deserializer<T>(parseString);
            }
            else
            {
                return new SafeDeserializer<T>(parseString);
            }
#endif
        }
    }
}
