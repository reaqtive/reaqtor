// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Nuqleon.DataModel.Serialization.Binary
{
    /// <summary>
    /// Provides miscellaneous helpers to deal with streams.
    /// </summary>
    internal static partial class StreamHelpers
    {
        /// <summary>
        /// Writes an unsigned integer value to the stream using a compact representation when possible.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Unsigned integer value to write to the stream.</param>
        public static void WriteUInt32Compact(this Stream stream, uint value)
        {
            var val = value;
            if (value <= 0x7F)
            {
                stream.WriteByte((byte)val);
            }
            else
            {
                var b1 = (byte)(val & 0xFF);
                val >>= 8;

                if (value <= 0x3FFF)
                {
                    var b2 = (byte)(val | 0x80);

                    stream.WriteByte(b2);
                    stream.WriteByte(b1);
                }
                else
                {
                    var b2 = (byte)(val & 0xFF);
                    val >>= 8;

                    if (value <= 0x1FFFFF)
                    {
                        var b3 = (byte)(val | 0xC0);

                        stream.WriteByte(b3);
                        stream.WriteByte(b2);
                        stream.WriteByte(b1);
                    }
                    else
                    {
                        var b3 = (byte)(val & 0xFF);
                        val >>= 8;

                        if (value <= 0xFFFFFFF)
                        {
                            var b4 = (byte)(val | 0xE0);

                            stream.WriteByte(b4);
                            stream.WriteByte(b3);
                            stream.WriteByte(b2);
                            stream.WriteByte(b1);
                        }
                        else
                        {
                            var b4 = (byte)(val & 0xFF);
                            val >>= 8;

                            var b5 = (byte)(val | 0xF0);

                            stream.WriteByte(b5);
                            stream.WriteByte(b4);
                            stream.WriteByte(b3);
                            stream.WriteByte(b2);
                            stream.WriteByte(b1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads an unsigned integer value from the stream assuming it was written using a compact representation.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Unsigned integer value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static uint ReadUInt32Compact(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            uint res;

            var b1 = ReadByte(stream);
            if ((b1 & 0x80) != 0)
            {
                var b2 = ReadByte(stream);

                if ((b1 & 0x40) != 0)
                {
                    var b3 = ReadByte(stream);

                    if ((b1 & 0x20) != 0)
                    {
                        var b4 = ReadByte(stream);

                        if ((b1 & 0x10) != 0)
                        {
                            var b5 = ReadByte(stream);

                            res = (uint)(b1 & 0x1F);
                            res <<= 8;
                            res |= b2;
                            res <<= 8;
                            res |= b3;
                            res <<= 8;
                            res |= b4;
                            res <<= 8;
                            res |= b5;
                        }
                        else
                        {
                            res = (uint)(b1 & 0xF);
                            res <<= 8;
                            res |= b2;
                            res <<= 8;
                            res |= b3;
                            res <<= 8;
                            res |= b4;
                        }
                    }
                    else
                    {
                        res = (uint)(b1 & 0x3F);
                        res <<= 8;
                        res |= b2;
                        res <<= 8;
                        res |= b3;
                    }
                }
                else
                {
                    res = (uint)(b1 & 0x7F);
                    res <<= 8;
                    res |= b2;
                }
            }
            else
            {
                res = b1;
            }

            return res;
        }

        /// <summary>
        /// Maximum size of a string to use a pooled byte array.
        /// </summary>
        internal const int MAX_POOLED_STRING_BYTES = 1024;

        /// <summary>
        /// Array pool for strings represented in less than MAX_POOLED_STRING_BYTES bytes.
        /// </summary>
        private static readonly ArrayPool<byte> s_stringPool = new(MAX_POOLED_STRING_BYTES);

        /// <summary>
        /// Writes the specified string (or a null reference) to the stream using a length prefix.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">String to write to the stream.</param>
        public static void WriteString(this Stream stream, string value)
        {
            var len = 0U;
            if (value != null)
            {
                len = (uint)Encoding.UTF8.GetByteCount(value) + 1U;
            }

            stream.WriteUInt32Compact(len);

            if (len > 0U)
            {
                if (len - 1 <= MAX_POOLED_STRING_BYTES)
                {
                    var buffer = s_stringPool.Get();
                    try
                    {
                        var written = Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
                        Debug.Assert(written == len - 1);
                        stream.Write(buffer, 0, written);
                    }
                    finally
                    {
                        s_stringPool.Release(buffer);
                    }
                }
                else
                {
                    var buffer = Encoding.UTF8.GetBytes(value);
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// Reads a string (or a null reference) from the stream assuming it was written using a length prefix.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>String read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static string ReadString(this Stream stream)
        {
            var len = (int)stream.ReadUInt32Compact();

            if (len > 0U)
            {
                byte[] buffer;
                if (len - 1 <= MAX_POOLED_STRING_BYTES)
                {
                    buffer = s_stringPool.Get();
                }
                else
                {
                    buffer = new byte[len - 1];
                }

                try
                {
                    var n = stream.Read(buffer, 0, len - 1);
                    if (n < len - 1)
                    {
                        throw new EndOfStreamException("Expected at least " + n + " bytes to read the string characters.");
                    }

                    return Encoding.UTF8.GetString(buffer, 0, len - 1);
                }
                finally
                {
                    if (len - 1 <= MAX_POOLED_STRING_BYTES)
                    {
                        s_stringPool.Release(buffer);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Writes the specified Guid value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Guid value to write to the stream.</param>
        public static unsafe void WriteGuid(this Stream stream, Guid value)
        {
            var array = s_size16ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Guid*)ptr = value;
                }

                stream.Write(array, 0, 16);
            }
            finally
            {
                s_size16ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Guid from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Guid value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Guid ReadGuid(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size16ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 16);

                if (i != 16)
                {
                    throw new EndOfStreamException("Expected 16-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Guid*)pbyte);
                }
            }
            finally
            {
                s_size16ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Byte array for true Boolean value.
        /// </summary>
        private static readonly byte[] s_trueBool = GetBooleanBytes(true);

        /// <summary>
        /// Byte array for false Boolean value.
        /// </summary>
        private static readonly byte[] s_falseBool = GetBooleanBytes(false);

        /// <summary>
        /// Writes the specified Boolean value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Boolean value to write to the stream.</param>
        public static unsafe void WriteBoolean(this Stream stream, bool value)
        {
            var array = value ? s_trueBool : s_falseBool;
            stream.Write(array, 0, 1);
        }

        /// <summary>
        /// Reads an integer Boolean from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Boolean value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe bool ReadBoolean(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size1ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 1);

                if (i != 1)
                {
                    throw new EndOfStreamException("Expected 1-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((bool*)pbyte);
                }
            }
            finally
            {
                s_size1ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads a single byte from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Byte read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static byte ReadByte(Stream stream)
        {
            var i = stream.ReadByte();

            if (i < 0)
            {
                throw new EndOfStreamException("Expected byte.");
            }

            return (byte)i;
        }

        /// <summary>
        /// Creates a byte array from a Boolean value.
        /// </summary>
        /// <param name="value">The Boolean value.</param>
        /// <returns>The byte array.</returns>
        private static unsafe byte[] GetBooleanBytes(bool value)
        {
            var array = new byte[1];
            fixed (byte* ptr = array)
            {
                *(bool*)ptr = value;
            }
            return array;
        }
    }
}
