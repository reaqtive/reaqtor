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
    /// Specifies the intended concurrency usage pattern for a fast JSON serializer or deserializer.
    /// </summary>
    public enum FastJsonConcurrencyMode
    {
        /// <summary>
        /// The fast JSON serializer or deserializer will be used by only one thread at a time.
        /// This mode is the fastest and is useful for single-threaded applications or message dispatchers.
        /// </summary>
        SingleThreaded,

        /// <summary>
        /// The fast JSON serializer or deserializer will be used from multiple thread concurrently.
        /// An alternative to using the thread-safe setting is to allocate a serializer or deserializer per thread.
        /// </summary>
        ThreadSafe,
    }
}
