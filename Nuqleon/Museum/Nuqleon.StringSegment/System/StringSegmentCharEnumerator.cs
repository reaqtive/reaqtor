// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 03/30/2016 - Created StringSegment functionality.
//

using System.Collections;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Enumerator for characters in string segments.
    /// </summary>
    public struct StringSegmentCharEnumerator : IEnumerator<char>
    {
        // NB: This use of a (mutable!) struct is a bit thorny here but enables an efficient foreach loop. Given that using a foreach loop on a
        //     string is common use case, it seems worth optimizing for that. Note that System.String has a class-based enumerator but it gets
        //     away with it because compilers optimize foreach enumeration over a string into an index-based for loop instead. Unfortunately,
        //     this choice also leads to the same hairy corner cases as the ones List<T> suffers from.

        private readonly int _length;
        private readonly int _startIndex;
        private string _value;
        private int _index;
        private char _current;

        /// <summary>
        /// Initializes a new character enumerator for a string segment.
        /// </summary>
        /// <param name="value">The string to enumerate over.</param>
        /// <param name="startIndex">The index of the first character to return during enumeration.</param>
        /// <param name="length">The number of characters to return during enumeration.</param>
        internal StringSegmentCharEnumerator(string value, int startIndex, int length)
        {
            _value = value;
            _startIndex = startIndex;
            _length = length;
            _index = -1;
            _current = '\0';
        }

        /// <summary>
        /// Gets the current character in the enumeration.
        /// </summary>
        public readonly char Current
        {
            get
            {
                if (_index == -1)
                {
                    throw new InvalidOperationException("Enumeration hasn't started yet.");
                }

                if (_index >= _length)
                {
                    throw new InvalidOperationException("Enumeration has ended.");
                }

                return _current;
            }
        }

        /// <summary>
        /// Gets the current character in the enumeration.
        /// </summary>
        readonly object IEnumerator.Current => Current;

        /// <summary>
        /// Disposes the enumerator.
        /// </summary>
        public void Dispose()
        {
            if (_value != null)
            {
                _index = _length;
            }

            _value = null;
        }

        /// <summary>
        /// Moves to the next character in the enumeration.
        /// </summary>
        /// <returns>true if a next character is available; otherwise, false.</returns>
        public bool MoveNext()
        {
            if (_index < _length - 1)
            {
                _index++;
                _current = _value[_startIndex + _index];
                return true;
            }

            _index = _length;
            return false;
        }

        /// <summary>
        /// Resets the enumerator.
        /// </summary>
        public void Reset()
        {
#if FIX_4055
            // NB: See https://github.com/dotnet/coreclr/issues/4055 for more info.
            if (_value == null)
                return;
#endif
            _current = '\0';
            _index = -1;
        }
    }
}
