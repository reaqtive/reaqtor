// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 03/30/2016 - Created StringSegment functionality.
//

namespace System
{

#if ALLOW_UNSAFE && HAS_MEMCPY

    using System.Diagnostics;

    internal unsafe struct UnsafeCharBuffer
    {
        private char* _buffer;
        private int _totalSize;
        private int _length;

        public UnsafeCharBuffer(char* buffer, int totalSize)
        {
            _buffer = buffer;
            _totalSize = totalSize;
            _length = 0;
        }

        public void Append(StringSegment segment)
        {
            if (StringSegment.IsNullOrEmpty(segment))
                return;

            AppendCore(segment.String, segment.StartIndex, segment.Length);
        }

        public void Append(char c, int count)
        {
            var rem = _totalSize - _length;

            if (rem < count)
                throw new IndexOutOfRangeException();

            // NB: This algorithm is similar to new string(c, count) and only differs in the computation of the offset.

            var dst = _buffer + _length;

            while (((uint)dst & 3) != 0 && count > 0)
            {
                *dst++ = c;
                count--;
            }

            var cc = (uint)((c << 16) | c);

            if (count >= 4)
            {
                count -= 4;

                do
                {
                    ((uint*)dst)[0] = cc;
                    ((uint*)dst)[1] = cc;
                    dst += 4;
                    count -= 4;
                } while (count >= 0);
            }

            if ((count & 2) != 0)
            {
                ((uint*)dst)[0] = cc;
                dst += 2;
            }

            if ((count & 1) != 0)
            {
                dst[0] = c;
            }

            _length += count;
        }

        public void Append(string value, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (startIndex < 0 || startIndex >= value.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex + length > value.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            AppendCore(value, startIndex, length);
        }

        private void AppendCore(string value, int startIndex, int length)
        {
            Debug.Assert(startIndex >= 0 && startIndex < value.Length);
            Debug.Assert(startIndex + length <= value.Length);

            var rem = _totalSize - _length;
            var num = length;

            if (rem < num)
                throw new IndexOutOfRangeException();

            fixed (char* segmentStr = value)
            {
                var src = segmentStr + startIndex;
                var dst = _buffer + _length;
                Buffer.MemoryCopy((void*)src, (void*)dst, rem * sizeof(char), num * sizeof(char));
            }

            _length += num;

            Debug.Assert(_length <= _totalSize);
        }
    }

#endif

}
