// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal static partial class StreamHelpers
    {
        /// <summary>
        /// Array pool for arrays containing 1 bytes, e.g. for the representation of 8 bit integer values.
        /// </summary>
        private static readonly ArrayPool<byte> s_size1ArrayPool = new ArrayPool<byte>(1);

        /// <summary>
        /// Writes the specified SByte value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">SByte value to write to the stream.</param>
        public static unsafe void WriteSByte(this Stream stream, SByte value)
        {
            var array = s_size1ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(SByte*)ptr = value;
                }

                stream.Write(array, 0, 1);
            }
            finally
            {
                s_size1ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer SByte from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>SByte value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe SByte ReadSByte(this Stream stream)
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
                    return *((SByte*)pbyte);
                }
            }
            finally
            {
                s_size1ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Array pool for arrays containing 2 bytes, e.g. for the representation of 16 bit integer values.
        /// </summary>
        private static readonly ArrayPool<byte> s_size2ArrayPool = new ArrayPool<byte>(2);

        /// <summary>
        /// Writes the specified Int16 value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Int16 value to write to the stream.</param>
        public static unsafe void WriteInt16(this Stream stream, Int16 value)
        {
            var array = s_size2ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Int16*)ptr = value;
                }

                stream.Write(array, 0, 2);
            }
            finally
            {
                s_size2ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Int16 from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Int16 value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Int16 ReadInt16(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size2ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 2);

                if (i != 2)
                {
                    throw new EndOfStreamException("Expected 2-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Int16*)pbyte);
                }
            }
            finally
            {
                s_size2ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Writes the specified UInt16 value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">UInt16 value to write to the stream.</param>
        public static unsafe void WriteUInt16(this Stream stream, UInt16 value)
        {
            var array = s_size2ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(UInt16*)ptr = value;
                }

                stream.Write(array, 0, 2);
            }
            finally
            {
                s_size2ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer UInt16 from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>UInt16 value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe UInt16 ReadUInt16(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size2ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 2);

                if (i != 2)
                {
                    throw new EndOfStreamException("Expected 2-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((UInt16*)pbyte);
                }
            }
            finally
            {
                s_size2ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Array pool for arrays containing 4 bytes, e.g. for the representation of 32 bit integer values.
        /// </summary>
        private static readonly ArrayPool<byte> s_size4ArrayPool = new ArrayPool<byte>(4);

        /// <summary>
        /// Writes the specified Int32 value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Int32 value to write to the stream.</param>
        public static unsafe void WriteInt32(this Stream stream, Int32 value)
        {
            var array = s_size4ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Int32*)ptr = value;
                }

                stream.Write(array, 0, 4);
            }
            finally
            {
                s_size4ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Int32 from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Int32 value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Int32 ReadInt32(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size4ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 4);

                if (i != 4)
                {
                    throw new EndOfStreamException("Expected 4-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Int32*)pbyte);
                }
            }
            finally
            {
                s_size4ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Writes the specified UInt32 value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">UInt32 value to write to the stream.</param>
        public static unsafe void WriteUInt32(this Stream stream, UInt32 value)
        {
            var array = s_size4ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(UInt32*)ptr = value;
                }

                stream.Write(array, 0, 4);
            }
            finally
            {
                s_size4ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer UInt32 from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>UInt32 value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe UInt32 ReadUInt32(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size4ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 4);

                if (i != 4)
                {
                    throw new EndOfStreamException("Expected 4-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((UInt32*)pbyte);
                }
            }
            finally
            {
                s_size4ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Array pool for arrays containing 8 bytes, e.g. for the representation of 64 bit integer values.
        /// </summary>
        private static readonly ArrayPool<byte> s_size8ArrayPool = new ArrayPool<byte>(8);

        /// <summary>
        /// Writes the specified Int64 value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Int64 value to write to the stream.</param>
        public static unsafe void WriteInt64(this Stream stream, Int64 value)
        {
            var array = s_size8ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Int64*)ptr = value;
                }

                stream.Write(array, 0, 8);
            }
            finally
            {
                s_size8ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Int64 from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Int64 value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Int64 ReadInt64(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size8ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 8);

                if (i != 8)
                {
                    throw new EndOfStreamException("Expected 8-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Int64*)pbyte);
                }
            }
            finally
            {
                s_size8ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Writes the specified UInt64 value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">UInt64 value to write to the stream.</param>
        public static unsafe void WriteUInt64(this Stream stream, UInt64 value)
        {
            var array = s_size8ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(UInt64*)ptr = value;
                }

                stream.Write(array, 0, 8);
            }
            finally
            {
                s_size8ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer UInt64 from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>UInt64 value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe UInt64 ReadUInt64(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size8ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 8);

                if (i != 8)
                {
                    throw new EndOfStreamException("Expected 8-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((UInt64*)pbyte);
                }
            }
            finally
            {
                s_size8ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Writes the specified Single value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Single value to write to the stream.</param>
        public static unsafe void WriteSingle(this Stream stream, Single value)
        {
            var array = s_size4ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Single*)ptr = value;
                }

                stream.Write(array, 0, 4);
            }
            finally
            {
                s_size4ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Single from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Single value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Single ReadSingle(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size4ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 4);

                if (i != 4)
                {
                    throw new EndOfStreamException("Expected 4-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Single*)pbyte);
                }
            }
            finally
            {
                s_size4ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Writes the specified Double value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Double value to write to the stream.</param>
        public static unsafe void WriteDouble(this Stream stream, Double value)
        {
            var array = s_size8ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Double*)ptr = value;
                }

                stream.Write(array, 0, 8);
            }
            finally
            {
                s_size8ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Double from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Double value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Double ReadDouble(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size8ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 8);

                if (i != 8)
                {
                    throw new EndOfStreamException("Expected 8-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Double*)pbyte);
                }
            }
            finally
            {
                s_size8ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Array pool for arrays containing 16 bytes, e.g. for the representation of 128 bit integer values.
        /// </summary>
        private static readonly ArrayPool<byte> s_size16ArrayPool = new ArrayPool<byte>(16);

        /// <summary>
        /// Writes the specified Decimal value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Decimal value to write to the stream.</param>
        public static unsafe void WriteDecimal(this Stream stream, Decimal value)
        {
            var array = s_size16ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Decimal*)ptr = value;
                }

                stream.Write(array, 0, 16);
            }
            finally
            {
                s_size16ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Decimal from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Decimal value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Decimal ReadDecimal(this Stream stream)
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
                    return *((Decimal*)pbyte);
                }
            }
            finally
            {
                s_size16ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Writes the specified Char value to the stream.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        /// <param name="value">Char value to write to the stream.</param>
        public static unsafe void WriteChar(this Stream stream, Char value)
        {
            var array = s_size2ArrayPool.Get();
            try
            {
                fixed (byte* ptr = array)
                {
                    *(Char*)ptr = value;
                }

                stream.Write(array, 0, 2);
            }
            finally
            {
                s_size2ArrayPool.Release(array);
            }
        }

        /// <summary>
        /// Reads an integer Char from the stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Char value read from the stream.</returns>
        /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached prematurely.</exception>
        public static unsafe Char ReadChar(this Stream stream)
        {
            Debug.Assert(BitConverter.IsLittleEndian, "This method is implemented assuming little endian.");

            var array = s_size2ArrayPool.Get();

            try
            {
                var i = stream.Read(array, 0, 2);

                if (i != 2)
                {
                    throw new EndOfStreamException("Expected 2-byte value.");
                }

                fixed (byte* pbyte = &array[0])
                {
                    return *((Char*)pbyte);
                }
            }
            finally
            {
                s_size2ArrayPool.Release(array);
            }
        }

    }
}
