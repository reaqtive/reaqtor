// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Interface for fast JSON serializers.
    /// </summary>
    /// <typeparam name="T">The type of the objects to serialize.</typeparam>
    public interface IFastJsonSerializer<in T>
    {
        /// <summary>
        /// Serializes the specified object to JSON.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string containing the result of serializing the specified object.</returns>
        string Serialize(T value);

#if !NO_IO
        /// <summary>
        /// Serializes the specified object to JSON into the specified text writer.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="writer">The text writer to append the JSON output to.</param>
        void Serialize(T value, System.IO.TextWriter writer);
#endif
    }
}
