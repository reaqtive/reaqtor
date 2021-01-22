// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System.Memory;

namespace Nuqleon.Json.Serialization
{
    /// <summary>
    /// Provides a context for the emitter which can be used to access resources and settings.
    /// </summary>
    internal partial class EmitterContext : IClearable
    {
#if !NO_IO
        private readonly FastJsonSerializerFactory.EmitterStringBuilder _builderString;
        private readonly FastJsonSerializerFactory.EmitterWriterBuilder _builderWriter;

        public EmitterContext(FastJsonSerializerFactory.EmitterStringBuilder builderString, FastJsonSerializerFactory.EmitterWriterBuilder builderWriter)
        {
            _builderString = builderString;
            _builderWriter = builderWriter;
        }
#else
        private readonly FastJsonSerializerFactory.EmitterStringBuilder _builderString;

        public EmitterContext(FastJsonSerializerFactory.EmitterStringBuilder builderString)
        {
            _builderString = builderString;
        }
#endif

#if !(ALLOW_UNSAFE && HAS_APPEND_CHARSTAR) || !NO_IO

        //
        // NB: Just enough to hold the digits of ulong.MaxValue
        //

        public readonly char[] IntegerDigitBuffer = new char[20];

#endif

        /// <summary>
        /// Clears the state in the context in order to allow for reuse.
        /// </summary>
        public void Clear() => ClearCycles();
    }
}
