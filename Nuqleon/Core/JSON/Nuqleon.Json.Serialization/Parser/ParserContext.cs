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
    // TODO: Pool per-thread instances of ParserContext in order to allow for synchronization-free StringPool usage.

    /// <summary>
    /// Provides a context for the parser which can be used to access resources and settings.
    /// </summary>
    internal class ParserContext
    {
#if USE_STRINGPOOL
        /// <summary>
        /// Gets the string pool to use for small string allocations due to lexing of numbers.
        /// </summary>
        public readonly StringPool StringPool = new StringPool(64);
#endif

#if !NO_IO
        /// <summary>
        /// Gets a reusable buffer for four characters.
        /// </summary>
        public readonly char[] char4 = new char[4];
#endif
    }
}
