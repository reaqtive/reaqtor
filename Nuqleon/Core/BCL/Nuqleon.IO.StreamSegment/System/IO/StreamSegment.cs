// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 01/04/2017 - Ported StreamSegment functionality.
//

#pragma warning disable CA1710 // Rename System.IO.StreamSegment to end in Stream. (Analogous naming to ArraySegment.)

namespace System.IO
{
    /// <summary>
    /// Delimits a section of a stream.
    /// </summary>
    public sealed class StreamSegment : Stream
    {
#pragma warning disable CA2213 // "Change the Dispose method to call Close or Dispose on this field." We don't own the underlying stream, so this is an inappropriate suggestion.
        private readonly Stream _stream;
#pragma warning restore CA2213
        private readonly long _offset;
        private long _count;

        private long _position;

        /// <summary>
        /// Creates a new instance of the <see cref="StreamSegment"/> class representing a segment in the specified stream at the given offset and with the given length.
        /// </summary>
        /// <param name="stream">Stream for which to obtain a stream segment.</param>
        /// <param name="offset">Offset in the underlying stream where the segment starts.</param>
        /// <param name="count">Number of bytes in the segment, starting from the specified offset.</param>
        public StreamSegment(Stream stream, long offset, long count)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (offset < 0L)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0L)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (stream.Length - offset < count)
                throw new ArgumentException("Specified offset and count fall outside the range of the stream.");

            _stream = stream;
            _offset = offset;
            _count = count;

            _position = offset;
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead => _stream.CanRead;

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek => _stream.CanSeek;

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite => _stream.CanWrite;

        /// <summary>
        /// Gets a value indicating whether the current stream can time out.
        /// </summary>
        public override bool CanTimeout => _stream.CanTimeout;

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush() => _stream.Flush();

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length => _count;

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get => _position - _offset;

            set
            {
                if (value < 0L)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _position = checked(_offset + value);
            }
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Specified offset and count fall outside the range of the buffer array.");

            var remainder = GetAvailableLength(count);

            _stream.Position = _position;

            var length = _stream.Read(buffer, offset, remainder);

            _position += length;

            return length;
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    {
                        if (offset < 0L)
                            throw new ArgumentOutOfRangeException(nameof(offset), "Cannot seek before the begin of the stream segment.");

                        _position = checked(_offset + offset);
                    }
                    break;
                case SeekOrigin.Current:
                    {
                        var newPosition = checked(_position + offset);
                        if (newPosition < _offset)
                            throw new ArgumentOutOfRangeException(nameof(offset), "Cannot seek before the begin of the stream segment.");

                        _position = newPosition;
                    }
                    break;
                case SeekOrigin.End:
                    {
                        var newPosition = checked(_offset + _count + offset);
                        if (newPosition < _offset)
                            throw new ArgumentOutOfRangeException(nameof(offset), "Cannot seek before the begin of the stream segment.");

                        _position = newPosition;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin specified.", nameof(origin));
            }

            return _position;
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            if (value < 0L)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (value > _count)
                throw new ArgumentOutOfRangeException(nameof(value), "Cannot extend the stream segment beyond its current length.");

            _count = value;
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("Specified offset and count fall outside the range of the buffer array.");

            var length = GetAvailableLength(count);

            if (length < count)
                throw new ArgumentException("Specified offset and count fall outside the range of the stream.");

            _stream.Position = _position;

            _stream.Write(buffer, offset, length);

            _position += length;
        }

        private int GetAvailableLength(int count)
        {
            var rem = checked((int)(_count - Position));

            if (rem < 0)
                return 0;

            return Math.Min(count, rem);
        }
    }
}
