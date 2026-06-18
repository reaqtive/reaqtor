// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/23/2016 - Added concurrency modes.
//

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Provides settings for the creation of fast JSON serializers or deserializers.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of fast JSON serializer settings given the specified concurrency mode.
    /// </remarks>
    /// <param name="concurrencyMode">Intended concurrency usage pattern for a fast JSON serializer or deserializer.</param>
    public class FastJsonSerializerSettings(FastJsonConcurrencyMode concurrencyMode)
    {

        /// <summary>
        /// Gets the intended concurrency usage pattern for a fast JSON serializer or deserializer.
        /// </summary>
        public FastJsonConcurrencyMode ConcurrencyMode { get; } = concurrencyMode;
    }
}
