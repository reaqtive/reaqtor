// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Reaqtor.QueryEngine
{
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2213 // Field 'stream' is not disposed. (This class doesn't own the stream.)

    /// <summary>
    /// Base class for state readers.
    /// </summary>
    internal class ReaderBase : IDisposable
    {
        private readonly ISerializationPolicy _policy;
        protected readonly Stream _stream;
        protected ISerializer _serializer;

        /// <summary>
        /// Creates a reader from the specified underlying <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="policy">The serialization policy to use when deserializing objects.</param>
        public ReaderBase(Stream stream, ISerializationPolicy policy)
        {
            _stream = stream;
            _policy = policy;
        }

        /// <summary>
        /// Reads and asserts the magic header.
        /// </summary>
        public void ReadHeader()
        {
            using var sr = new BinaryReader(_stream, Encoding.UTF8, leaveOpen: true);

            // Magic
            {
                var position = _stream.Position;

                try
                {
                    var b = sr.ReadChar();
                    var d = sr.ReadChar();

                    if (b != 'B' || d != 'D')
                    {
                        throw MissingHeader(position);
                    }
                }
                catch (EndOfStreamException)
                {
                    throw MissingHeader(position);
                }
            }

            // QE version
            {
                var position = _stream.Position;

                try
                {
                    var major = sr.ReadInt32();
                    var minor = sr.ReadInt32();
                    var build = sr.ReadInt32();
                    var revision = sr.ReadInt32();

                    var version = new Version(major, minor, build, revision);
                    if (version < Versioning.v1)
                    {
                        throw UnsupportedVersion(position, version);
                    }
                }
                catch (EndOfStreamException)
                {
                    throw MissingVersion(position);
                }
            }

            // Flags
            {
                var position = _stream.Position;

                try
                {
                    var flags = sr.ReadInt32();
                }
                catch (EndOfStreamException)
                {
                    throw MissingFlags(position);
                }
            }

            // Serializer
            {
                var position = _stream.Position;

                try
                {
                    var serName = sr.ReadString();

                    var serMajor = sr.ReadInt32();
                    var serMinor = sr.ReadInt32();
                    var serBuild = sr.ReadInt32();
                    var serRevision = sr.ReadInt32();
                    var serVersion = new Version(serMajor, serMinor, serBuild, serRevision);

                    _serializer = _policy.GetSerializer(serName, serVersion);
                }
                catch (EndOfStreamException)
                {
                    throw MissingSerializer(position);
                }
            }
        }

        /// <summary>
        /// Reads and checks the magic footer.
        /// </summary>
        public void ReadFooter()
        {
            var position = _stream.Position;

            var de1 = (byte)_stream.ReadByte();
            var ad1 = (byte)_stream.ReadByte();
            var de2 = (byte)_stream.ReadByte();
            var ad2 = (byte)_stream.ReadByte();

            if (de1 != 0xDE || de2 != 0xDE || ad1 != 0xAD || ad2 != 0xAD)
            {
                throw MissingTerminator(position, new[] { de1, ad1, de2, ad2 });
            }
        }

        /// <summary>
        /// Disposes the reader.
        /// </summary>
        public void Dispose()
        {
        }

        private InvalidDataException MissingHeader(long position)
        {
            var blob = _stream.GetBase64Blob();
            return new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Missing magic header in checkpoint state. Position = {0}, State = {1}", position, blob));
        }

        private InvalidDataException MissingVersion(long position)
        {
            var blob = _stream.GetBase64Blob();
            return new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Missing state version in checkpoint state. Position = {0}, State = {1}", position, blob));
        }

        private InvalidDataException UnsupportedVersion(long position, Version version)
        {
            var blob = _stream.GetBase64Blob();
            return new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Checkpoint state version {0} is not supported. Position = {1}, State = {2}", version, position, blob));
        }

        private InvalidDataException MissingFlags(long position)
        {
            var blob = _stream.GetBase64Blob();
            return new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Missing flags in checkpoint state. Position = {0}, State = {1}", position, blob));
        }

        private InvalidDataException MissingSerializer(long position)
        {
            var blob = _stream.GetBase64Blob();
            return new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Missing serializer info in checkpoint state. Position = {0}, State = {1}", position, blob));
        }

        private InvalidDataException MissingTerminator(long position, byte[] bytes)
        {
            var blob = _stream.GetBase64Blob();
            return new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Missing checkpoint state terminator. Read = {0}, Position = {1}, State = {2}", string.Join(", ", bytes), position, blob));
        }
    }
}
