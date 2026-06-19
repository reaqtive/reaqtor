// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;

namespace Reaqtor.QueryEngine
{
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2213 // Field 'stream' is not disposed. (This class doesn't own the stream.)

    /// <summary>
    /// Base class for state writers.
    /// </summary>
    internal class WriterBase : IDisposable
    {
        private readonly ISerializationPolicy _policy;
        protected readonly Stream _stream;
        protected ISerializer _serializer;

        /// <summary>
        /// Creates a writer to the specified underlying <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="policy">The serialization policy to use when serializing objects.</param>
        public WriterBase(Stream stream, ISerializationPolicy policy)
        {
            _stream = stream;
            _policy = policy;
        }

        /// <summary>
        /// Writes a magic header.
        /// </summary>
        public void WriteHeader()
        {
            using var sw = new BinaryWriter(_stream, Encoding.UTF8, leaveOpen: true);

            // Magic
            {
                sw.Write('B');
                sw.Write('D');
            }

            // QE version
            {
                var version = Versioning.v1;
                sw.Write(version.Major);
                sw.Write(version.Minor);
                sw.Write(version.Build);
                sw.Write(version.Revision);
            }

            // Flags
            {
                sw.Write(0);
            }

            // Serializer
            {
                var serializer = _policy.GetSerializer();

                sw.Write(serializer.Name);

                sw.Write(serializer.Version.Major);
                sw.Write(serializer.Version.Minor);
                sw.Write(serializer.Version.Build);
                sw.Write(serializer.Version.Revision);

                _serializer = serializer;
            }
        }

        /// <summary>
        /// Disposes the writer, which emits a magic footer.
        /// </summary>
        public void Dispose()
        {
            _stream.WriteByte(0xDE);
            _stream.WriteByte(0xAD);
            _stream.WriteByte(0xDE);
            _stream.WriteByte(0xAD);
        }
    }
}
