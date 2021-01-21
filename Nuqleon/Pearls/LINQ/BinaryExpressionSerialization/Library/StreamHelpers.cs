// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.IO;
using System.Text;

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
    /// <summary>
    /// Provides miscellaneous helpers to deal with streams.
    /// </summary>
    internal static class StreamHelpers
    {
        /// <summary>
        /// Array pool for arrays containing 4 bytes, e.g. for the representation of 32 bit integer values.
        /// </summary>
        private static readonly ArrayPool<byte> s_int32ArrayPool = new(4);

        /// <summary>
        /// Writes the specified integer value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Integer value to write to the stream.</param>
        public static unsafe void WriteInt32(this Stream stream, int value)
        {
            var array = s_int32ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(int*)ptr = value;
                }

                stream.Write(array, 0, 4);
            }
            finally
            {
                s_int32ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer value from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Integer value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe int ReadInt32(this Stream stream)
        {
            var array = s_int32ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 4);

                if (i != 4)
                {
                    throw new EndOfStreamException("Expected 4-byte integer value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((int*)pbyte);
                }
            }
            finally
            {
                s_int32ArrayPool.Release(array);
            }
        }

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
                var name = Encoding.UTF8.GetBytes(value);
                stream.Write(name, 0, name.Length);
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
            var len = stream.ReadUInt32Compact();

            if (len > 0U)
            {
                var buffer = new byte[len - 1];
                var n = stream.Read(buffer, 0, buffer.Length);
                if (n < buffer.Length)
                {
                    throw new EndOfStreamException("Expected at least " + n + " bytes to read the string characters.");
                }

                return Encoding.UTF8.GetString(buffer);
            }

            return null;
        }

        /// <summary>
        /// Reads a single byte from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Byte read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        private static byte ReadByte(Stream stream)
        {
            var i = stream.ReadByte();

            if (i < 0)
            {
                throw new EndOfStreamException("Expected byte.");
            }

            return (byte)i;
        }
    }
}
