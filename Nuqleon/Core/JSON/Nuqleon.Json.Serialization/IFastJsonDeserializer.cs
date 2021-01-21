// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/05/2016 - Created fast JSON deserializer functionality.
//   BD - 05/08/2016 - Added support for deserialization from text readers.
//

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Interface for fast JSON deserializers.
    /// </summary>
    /// <typeparam name="T">The type of the objects to deserialize.</typeparam>
    public interface IFastJsonDeserializer<out T>
    {
        /// <summary>
        /// Deserializes the specified JSON string into an object.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An instance of <typeparamref name="T"/> containing the result of deserializing the specified JSON input.</returns>
        T Deserialize(string json);

#if !NO_IO
        /// <summary>
        /// Deserializes the JSON payload from a text reader into an object.
        /// </summary>
        /// <param name="reader">The reader containing the JSON payload to deserialize.</param>
        /// <returns>An instance of <typeparamref name="T"/> containing the result of deserializing the specified JSON input.</returns>
        T Deserialize(System.IO.TextReader reader);
#endif
    }
}
