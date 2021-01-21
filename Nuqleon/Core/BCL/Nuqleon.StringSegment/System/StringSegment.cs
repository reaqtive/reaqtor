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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    /// <summary>
    /// Value representing a segment in a string used to reduce allocations of string copies.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("BD", "1")] // NB: Suppresses code analysis warnings for this file; lots of behavior is a verbatim copy of System.String behavior.
    public struct StringSegment : IEquatable<StringSegment>, IEnumerable<char>, IComparable, IComparable<StringSegment>
    {
        private const int TrimHead = 0;
        private const int TrimTail = 1;
        private const int TrimBoth = 2;

        private static readonly StringSegment[] s_emptyArray = Array.Empty<StringSegment>();

        private readonly string _string;
        private readonly int _startIndex;
        private readonly int _length;

        /// <summary>
        /// Gets an empty string segment.
        /// </summary>
        public static readonly StringSegment Empty = new StringSegment(string.Empty);

        /// <summary>
        /// Creates a string segment representing the specified string.
        /// </summary>
        /// <param name="value">The string to wrap in a string segment.</param>
        public StringSegment(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _string = value;
            _startIndex = 0;
            _length = value.Length;
        }

        /// <summary>
        /// Initializes a new string segment to the value indicated by an array of Unicode characters.
        /// </summary>
        /// <param name="value">An array of Unicode characters.</param>
        public StringSegment(char[] value) : this(new string(value)) { }

        /// <summary>
        /// Initializes a new string segment to the value indicated by an array of Unicode characters, a starting character position within that array, and a length.
        /// </summary>
        /// <param name="value">An array of Unicode characters.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="length">The number of characters within value to use.</param>
        public StringSegment(char[] value, int startIndex, int length) : this(new string(value, startIndex, length)) { }

        /// <summary>
        /// Initializes a new string segment to the value indicated by a specified Unicode character repeated a specified number of times.
        /// </summary>
        /// <param name="c">A Unicode character.</param>
        /// <param name="count">The number of times c occurs.</param>
        public StringSegment(char c, int count) : this(new string(c, count)) { }

#if ALLOW_UNSAFE
        /// <summary>
        /// Initializes a new string segment to the value indicated by a pointer to an array of 8-bit signed integers.
        /// </summary>
        /// <param name="value">A pointer to a null-terminated array of 8-bit signed integers.</param>
        public unsafe StringSegment(sbyte* value) : this(new string(value)) { }

        /// <summary>
        /// Initializes a new string segment to the value indicated by a specified pointer to an array of Unicode characters.
        /// </summary>
        /// <param name="value">A pointer to a null-terminated array of Unicode characters.</param>
        public unsafe StringSegment(char* value) : this(new string(value)) { }

        /// <summary>
        /// Initializes a new string segment to the value indicated by a specified pointer to an array of 8-bit signed integers, a starting position within that array, and a length.
        /// </summary>
        /// <param name="value">A pointer to an array of 8-bit signed integers.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="length">The number of characters within value to use.</param>
        public unsafe StringSegment(sbyte* value, int startIndex, int length) : this(new string(value, startIndex, length)) { }

        /// <summary>
        /// Initializes a new string segment to the value indicated by a specified pointer to an array of Unicode characters, a starting character position within that array, and a length.
        /// </summary>
        /// <param name="value">A pointer to an array of Unicode characters.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="length">The number of characters within value to use.</param>
        public unsafe StringSegment(char* value, int startIndex, int length) : this(new string(value, startIndex, length)) { }

        /// <summary>
        /// Initializes a new string segment to the value indicated by a specified pointer to an array of 8-bit signed integers, a starting position within that array, a length, and an System.Text.Encoding object.
        /// </summary>
        /// <param name="value">A pointer to an array of 8-bit signed integers.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="length">The number of characters within value to use.</param>
        /// <param name="enc">An object that specifies how the array referenced by value is encoded. If enc is null, ANSI encoding is assumed.</param>
        public unsafe StringSegment(sbyte* value, int startIndex, int length, Encoding enc) : this(new string(value, startIndex, length, enc)) { }
#endif

        /// <summary>
        /// Creates a string segment representing the specified number of characters in the specified string starting from the specified start index.
        /// </summary>
        /// <param name="value">The string to wrap in a string segment.</param>
        /// <param name="startIndex">The index of the first character exposed by the segment.</param>
        /// <param name="length">The number of characters exposed by the segment, starting from the specified start index.</param>
        public StringSegment(string value, int startIndex, int length)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex + length > value.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            _string = value;
            _startIndex = startIndex;
            _length = length;
        }

        /// <summary>
        /// Gets the string over which the string segment was created.
        /// </summary>
        public string String => _string;

        /// <summary>
        /// Gets the index of the first character in the underlying string exposed by the string segment.
        /// </summary>
        public int StartIndex => _startIndex;

        /// <summary>
        /// Gets the number of characters in the string segment.
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// Gets the character at the specified index.
        /// </summary>
        /// <param name="index">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index.</returns>
        [IndexerName("Chars")]
        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();

                return String[StartIndex + index]; // NB: OK to cause null reference
            }
        }

        /// <summary>
        /// Implicit conversion from String to StringSegment.
        /// </summary>
        /// <param name="value">The string to convert to a string segment.</param>
        public static implicit operator StringSegment(string value) => value == null ? new StringSegment() : new StringSegment(value);

        /// <summary>
        /// Explicit conversion from StringSegment to String. This method may cause object allocation.
        /// </summary>
        /// <param name="segment">The string segment to convert to a string.</param>
        public static explicit operator string(StringSegment segment) => segment.ToString();

        /// <summary>
        /// Checks whether the specified string segments are equal.
        /// </summary>
        /// <param name="a">The first string segment to compare for equality.</param>
        /// <param name="b">The second string segment to compare for equality.</param>
        /// <returns>true if the specified string segments are equal; otherwise, false.</returns>
        public static bool operator ==(StringSegment a, StringSegment b) => a.Equals(b);

        /// <summary>
        /// Checks whether the specified string segments are not equal.
        /// </summary>
        /// <param name="a">The first string segment to compare for inequality.</param>
        /// <param name="b">The second string segment to compare for inequality.</param>
        /// <returns>true if the specified string segments are not equal; otherwise, false.</returns>
        public static bool operator !=(StringSegment a, StringSegment b) => !(a == b);

        /// <summary>
        /// Gets a hash code for the string segment.
        /// </summary>
        /// <returns>Hash code for the string segment.</returns>
        public override int GetHashCode() => String == null ? 0 : String.GetHashCode() ^ StartIndex ^ Length;  // NB: same as ArraySegment<T>

        /// <summary>
        /// Checks whether the string segment is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare the string segment to.</param>
        /// <returns>true if the string segment is equal to the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is StringSegment && Equals((StringSegment)obj);  // NB: doesn't support string, wouldn't be commutative

        /// <summary>
        /// Checks whether the contents of the string segment are equal to the contents of the specified string segment.
        /// </summary>
        /// <param name="value">The string segment to compare to.</param>
        /// <returns>true if the string segment is equal to the specified string segment; otherwise, false.</returns>
        public bool Equals(StringSegment value)
        {
            if (String == null && value.String == null)
            {
                // NB: if the underlying string is null, all operations on the string segment will have the same behavior, so we treat the values equal
                return true;
            }

            if (String == null || value.String == null)
            {
                return false;
            }

            if (StrictEquals(value))
            {
                return true;
            }

            if (Length != value.Length)
            {
                return false;
            }

            // CONSIDER: Use unsafe code to optimize, but it seems String.EqualsHelpers uses platform-specific optimizations which we can't do here.
            //           Unfortunately, we don't have access to the nativeCompareOrdinalEx method with start indexes and counts, which could offer
            //           another option to do equality checking based on the comparison resulting in 0.

            for (var i = 0; i < Length; i++)
            {
                if (this[i] != value[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a string representing the contents of the string segment. This method may cause object allocation.
        /// </summary>
        /// <returns>String representing the contents of the string segment.</returns>
        public override string ToString() => String == null ? string.Empty : String.Substring(StartIndex, Length);  // NB: does not allocate if it includes the whole string

        /// <summary>
        /// Gets a string representing the contents of the string segment. This method may cause object allocation.
        /// </summary>
        /// <param name="provider">(Reserved) An object that supplies culture-specific formatting information.</param>
        /// <returns>String representing the contents of the string segment.</returns>
        public string ToString(IFormatProvider provider) => ToString();

        /// <summary>
        /// Compares the string represented by the string segment to the string represented by the specified string segment and indicates whether this instance precedes,
        /// follows, or appears in the same position in the sort order as the specified string segment.
        /// </summary>
        /// <param name="strB">The string segment to compare to.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(StringSegment strB)
        {
            if (strB.String == null)
            {
                return String == null ? 0 : 1;
            }

            if (StrictEquals(strB))
            {
                return 0;
            }

            return CultureInfo.CurrentCulture.CompareInfo.Compare(String, StartIndex, Length, strB.String, strB.StartIndex, strB.Length, CompareOptions.None);
        }

        /// <summary>
        /// Compares the string represented by the string segment to the specified object and indicates whether this instance precedes,
        /// follows, or appears in the same position in the sort order as the specified object.
        /// </summary>
        /// <param name="value">The object to compare to.</param>
        /// <returns>A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.</returns>
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return String == null ? 0 : 1;
            }

            // NB: not supporting string because it wouldn't be commutative

            if (!(value is StringSegment))
            {
                throw new ArgumentException("Argument must be a string or a string segment.", nameof(value));
            }

            var strB = (StringSegment)value;

            return CompareTo(strB);
        }

        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <returns>true if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
        public bool Contains(StringSegment value) => IndexOf(value, StringComparison.Ordinal) >= 0;

        /// <summary>
        /// Copies a specified number of characters from a specified position in this instance to a specified position in an array of Unicode characters.
        /// </summary>
        /// <param name="sourceIndex">The index of the first character in this instance to copy.</param>
        /// <param name="destination">An array of Unicode characters to which characters in this instance are copied.</param>
        /// <param name="destinationIndex">The index in destination at which the copy operation begins.</param>
        /// <param name="count">The number of characters in this instance to copy to destination.</param>
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            if (String == null)
                throw new NullReferenceException();
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (sourceIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(sourceIndex));
            if (count > Length - sourceIndex)
                throw new ArgumentOutOfRangeException(nameof(sourceIndex));
            if (destinationIndex < 0 || destinationIndex > destination.Length - count)
                throw new ArgumentOutOfRangeException(nameof(destinationIndex));

            if (count > 0)
            {
                String.CopyTo(StartIndex + sourceIndex, destination, destinationIndex, count);
            }
        }

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <returns>true if value matches the end of this string segment; otherwise, false.</returns>
        public bool EndsWith(StringSegment value) => EndsWith(value, StringComparison.CurrentCulture);

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string when compared using the specified comparison option.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how this string and value are compared.</param>
        /// <returns>true if this string segment ends with value; otherwise, false.</returns>
        public bool EndsWith(StringSegment value, StringComparison comparisonType)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));

            if (StrictEquals(value))
                return true;

            if (value.Length == 0)
                return true;

            if (value.Length > Length)
                return false;

            return string.Compare(String, StartIndex + Length - value.Length, value.String, value.StartIndex, value.Length, comparisonType) == 0;
        }

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string when compared using the specified culture.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <param name="culture">Cultural information that determines how this string and value are compared. If culture is null, the current culture is used.</param>
        /// <returns>true if the value parameter matches the end of this string segment; otherwise, false.</returns>
        public bool EndsWith(StringSegment value, bool ignoreCase, CultureInfo culture)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));

            if (StrictEquals(value))
                return true;

            if (value.Length == 0)
                return true;

            if (value.Length > Length)
                return false;

            return string.Compare(String, StartIndex + Length - value.Length, value.String, value.StartIndex, value.Length, ignoreCase, culture ?? CultureInfo.CurrentCulture) == 0;
        }

        /// <summary>
        /// Determines whether this string and a specified string segment have the same value. A parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="value">The string to compare to this instance.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared.</param>
        /// <returns>true if the value of the value parameter is the same as this string; otherwise, false.</returns>
        public bool Equals(StringSegment value, StringComparison comparisonType) => Compare(this, value, comparisonType) == 0;

        /// <summary>
        /// Retrieves an object that can iterate through the individual characters in this string.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Retrieves an object that can iterate through the individual characters in this string.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Retrieves an object that can iterate through the individual characters in this string.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public StringSegmentCharEnumerator GetEnumerator() => new StringSegmentCharEnumerator(String, StartIndex, Length);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified Unicode character in this string.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <returns>The zero-based index position of value if that character is found, or -1 if it is not.</returns>
        public int IndexOf(char value) => IndexOf(value, 0, Length);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified Unicode character in this string. The search starts at a specified character position.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>The zero-based index position of value if that character is found, or -1 if it is not.</returns>
        public int IndexOf(char value, int startIndex) => IndexOf(value, startIndex, Length - startIndex);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified character in this instance. The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>The zero-based index position of value if that character is found, or -1 if it is not.</returns>
        public int IndexOf(char value, int startIndex, int count)
        {
            if (String == null)
                throw new NullReferenceException();
            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(count));

            var endIndex = startIndex + count;

            for (var i = startIndex; i < endIndex; i++)
            {
                if (String[StartIndex + i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in this instance.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is 0.</returns>
        public int IndexOf(StringSegment value) => IndexOf(value, StringComparison.CurrentCulture);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in this instance. A parameter specifies the type of search to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is 0.</returns>
        public int IndexOf(StringSegment value, StringComparison comparisonType) => IndexOf(value, 0, Length, comparisonType);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in this instance. The search starts at a specified character position.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is startIndex.</returns>
        public int IndexOf(StringSegment value, int startIndex) => IndexOf(value, startIndex, StringComparison.CurrentCulture);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified character in this instance. The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is startIndex.</returns>
        public int IndexOf(StringSegment value, int startIndex, int count) => IndexOf(value, startIndex, count, StringComparison.CurrentCulture);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in this instance. Parameters specify the starting search position in the current string and the type of search to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is startIndex.</returns>
        public int IndexOf(StringSegment value, int startIndex, StringComparison comparisonType) => IndexOf(value, startIndex, Length - startIndex, comparisonType);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified string in this instance. Parameters specify the starting search position in the current string, the number of characters in the current string to search, and the type of search to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The zero-based index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is startIndex.</returns>
        public int IndexOf(StringSegment value, int startIndex, int count, StringComparison comparisonType)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));

            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count > Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (value.Length == 0)
                return startIndex;

            var num = count - value.Length;

            if (num >= 0)
            {
                var endIndex = startIndex + num;

                for (var i = startIndex; i <= endIndex; i++)
                {
                    if (string.Compare(String, StartIndex + i, value.String, value.StartIndex, value.Length, comparisonType) == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <returns>The zero-based index position of the first occurrence in this instance where any character in anyOf was found; -1 if no character in anyOf was found.</returns>
        public int IndexOfAny(char[] anyOf) => IndexOfAny(anyOf, 0, Length);

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters. The search starts at a specified character position.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>The zero-based index position of the first occurrence in this instance where any character in anyOf was found; -1 if no character in anyOf was found.</returns>
        public int IndexOfAny(char[] anyOf, int startIndex) => IndexOfAny(anyOf, startIndex, Length - startIndex);

        /// <summary>
        /// Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters. The search starts at a specified character position and examines a specified number of character positions.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>The zero-based index position of the first occurrence in this instance where any character in anyOf was found; -1 if no character in anyOf was found.</returns>
        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            if (String == null)
                throw new NullReferenceException();
            if (anyOf == null)
                throw new ArgumentNullException(nameof(anyOf));

            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > Length - count)
                throw new ArgumentOutOfRangeException(nameof(count));

            // CONSIDER: Stackalloc a probabilistic map when unsafe code is allowed in order to avoid the inner loop when possible.

            var endIndex = startIndex + count;

            for (var i = startIndex; i < endIndex; i++)
            {
                var c = String[StartIndex + i];

                foreach (var d in anyOf)
                {
                    if (c == d)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns a new string in which a specified string is inserted at a specified index position in this instance.
        /// </summary>
        /// <param name="startIndex">The zero-based index position of the insertion.</param>
        /// <param name="value">The string to insert.</param>
        /// <returns>A new string that is equivalent to this instance, but with value inserted at position startIndex.</returns>
        public StringSegment Insert(int startIndex, StringSegment value)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (Length + value.Length == 0)
                return Empty;

            // NB: string does cause copy if value is empty; do we need to retain that behavior here? (docs state "A new string" is returned)
            if (value.Length == 0)
                return this;

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var num = Length + value.Length;

                var res = new string('\0', num); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, num);

                    buffer.Append(String, StartIndex, startIndex);
                    buffer.Append(value.String, value.StartIndex, value.Length);
                    buffer.Append(String, StartIndex + startIndex, Length - startIndex);
                }

                return res;
            }
#else
            var sb = new StringBuilder(Length + value.Length);

            sb.Append(String, StartIndex, startIndex);
            sb.Append(value.String, value.StartIndex, value.Length);
            sb.Append(String, StartIndex + startIndex, Length - startIndex);

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified Unicode character in this string.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <returns>The zero-based index position of value if that character is found, or -1 if it is not.</returns>
        public int LastIndexOf(char value) => LastIndexOf(value, Length - 1, Length);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified Unicode character in this string. The search starts at a specified character position and proceeds backward toward the beginning of the string.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <returns>The zero-based index position of value if that character is found, or -1 if it is not found or if the current instance equals Empty.</returns>
        public int LastIndexOf(char value, int startIndex) => LastIndexOf(value, startIndex, startIndex + 1);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of the specified character in this instance. he search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.
        /// </summary>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>The zero-based index position of value if that character is found, or -1 if it is not found or if the current instance equals Empty.</returns>
        public int LastIndexOf(char value, int startIndex, int count)
        {
            if (String == null)
                throw new NullReferenceException();

            if (Length == 0)
                return -1;

            if (startIndex < 0 || startIndex >= Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count - 1 > startIndex)
                throw new ArgumentOutOfRangeException(nameof(count));

            var endIndex = startIndex - count + 1;

            for (int i = startIndex; i >= endIndex; i--)
            {
                if (String[StartIndex + i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string within this instance.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <returns>The zero-based starting index position of value if that string is found, or -1 if it is not. If value is Empty, the return value is the last index position in this instance.</returns>
        public int LastIndexOf(StringSegment value) => LastIndexOf(value, Length - 1, Length, StringComparison.CurrentCulture);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <returns>The zero-based starting index position of value if that string is found, or -1 if it is not found or if the current instance equals Empty. If value is Empty, the return value is the smaller of startIndex and the last index position in this instance.</returns>
        public int LastIndexOf(StringSegment value, int startIndex) => LastIndexOf(value, startIndex, startIndex + 1, StringComparison.CurrentCulture);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of a specified string within this instance. A parameter specifies the type of search to use for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The zero-based starting index position of the value parameter if that string is found, or -1 if it is not. If value is Empty, the return value is the last index position in this instance.</returns>
        public int LastIndexOf(StringSegment value, StringComparison comparisonType) => LastIndexOf(value, Length - 1, Length, comparisonType);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>The zero-based starting index position of value if that string is found, or -1 if it is not found or if the current instance equals Empty. If value is Empty, the return value is the smaller of startIndex and the last index position in this instance.</returns>
        public int LastIndexOf(StringSegment value, int startIndex, int count) => LastIndexOf(value, startIndex, count, StringComparison.CurrentCulture);

        /// <summary>
        /// Reports the zero-based index of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string. A parameter specifies the type of comparison to perform when searching for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The zero-based starting index position of the value parameter if that string is found, or -1 if it is not found or if the current instance equals Empty. If value is Empty, the return value is the smaller of startIndex and the last index position in this instance.</returns>
        public int LastIndexOf(StringSegment value, int startIndex, StringComparison comparisonType) => LastIndexOf(value, startIndex, startIndex + 1, comparisonType);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence of a specified string within this instance. The search starts at a specified character position and proceeds backward toward the beginning of the string for the specified number of character positions. A parameter specifies the type of comparison to perform when searching for the specified string.
        /// </summary>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>The zero-based starting index position of the value parameter if that string is found, or -1 if it is not found or if the current instance equals Empty. If value is Empty, the return value is the smaller of startIndex and the last index position in this instance.</returns>
        public int LastIndexOf(StringSegment value, int startIndex, int count, StringComparison comparisonType)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));

            if (Length == 0 && (startIndex == -1 || startIndex == 0))
            {
                return value.Length == 0 ? 0 : -1;
            }

            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            var minIndex = startIndex - count + 1;

            if (startIndex == Length)
            {
                startIndex--;
                if (count > 0)
                    count--;

                if (value.Length == 0 && count >= 0 && minIndex >= 0)
#if NET5_0 // https://github.com/dotnet/runtime/issues/13383
                    return startIndex + 1;
#else
                    return startIndex;
#endif
            }

            if (count < 0 || minIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (value.Length == 0)
#if NET5_0 // https://github.com/dotnet/runtime/issues/13383
                return startIndex + 1;
#else
                return startIndex;
#endif

            var num = count - value.Length;

            if (num >= 0)
            {
                var endIndex = startIndex - value.Length + 1;

                for (var i = endIndex; i >= minIndex; i--)
                {
                    if (string.Compare(String, StartIndex + i, value.String, value.StartIndex, value.Length, comparisonType) == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Reports the zero-based index position of the last occurrence in this instance of one or more characters specified in a Unicode array.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <returns>The index position of the last occurrence in this instance where any character in anyOf was found; -1 if no character in anyOf was found.</returns>
        public int LastIndexOfAny(char[] anyOf) => LastIndexOfAny(anyOf, Length - 1, Length);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence in this instance of one or more characters specified in a Unicode array. The search starts at a specified character position and proceeds backward toward the beginning of the string.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <returns></returns>
        public int LastIndexOfAny(char[] anyOf, int startIndex) => LastIndexOfAny(anyOf, startIndex, startIndex + 1);

        /// <summary>
        /// Reports the zero-based index position of the last occurrence in this instance of one or more characters specified in a Unicode array. The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.
        /// </summary>
        /// <param name="anyOf">A Unicode character array containing one or more characters to seek.</param>
        /// <param name="startIndex">The search starting position. The search proceeds from startIndex toward the beginning of this instance.</param>
        /// <param name="count">The number of character positions to examine.</param>
        /// <returns>The index position of the last occurrence in this instance where any character in anyOf was found; -1 if no character in anyOf was found or if the current instance equals Empty.</returns>
        public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
        {
            if (String == null)
                throw new NullReferenceException();
            if (anyOf == null)
                throw new ArgumentNullException(nameof(anyOf));

            if (Length == 0)
                return -1;

            if (startIndex < 0 || startIndex >= Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || count - 1 > startIndex)
                throw new ArgumentOutOfRangeException(nameof(count));

            // CONSIDER: Stackalloc a probabilistic map when unsafe code is allowed in order to avoid the inner loop when possible.

            var endIndex = startIndex - count + 1;

            for (int i = startIndex; i >= endIndex; i--)
            {
                var c = String[StartIndex + i];

                foreach (var d in anyOf)
                {
                    if (c == d)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns a new string that right-aligns the characters in this instance by padding them with spaces on the left, for a specified total length.
        /// </summary>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <returns>A new string that is equivalent to this instance, but right-aligned and padded on the left with as many spaces as needed to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.</returns>
        public StringSegment PadLeft(int totalWidth) => PadHelper(totalWidth, ' ', isRightPadded: false);

        /// <summary>
        /// Returns a new string that right-aligns the characters in this instance by padding them on the left with a specified Unicode character, for a specified total length.
        /// </summary>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <returns>A new string that is equivalent to this instance, but right-aligned and padded on the left with as many paddingChar characters as needed to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.</returns>
        public StringSegment PadLeft(int totalWidth, char paddingChar) => PadHelper(totalWidth, paddingChar, isRightPadded: false);

        /// <summary>
        /// Returns a new string that left-aligns the characters in this instance by padding them with spaces on the right, for a specified total length.
        /// </summary>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <returns>A new string that is equivalent to this instance, but left-aligned and padded on the right with as many spaces as needed to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.</returns>
        public StringSegment PadRight(int totalWidth) => PadHelper(totalWidth, ' ', isRightPadded: true);

        /// <summary>
        /// Returns a new string that left-aligns the characters in this instance by padding them on the right with a specified Unicode character, for a specified total length.
        /// </summary>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <returns>A new string that is equivalent to this instance, but left-aligned and padded on the right with as many paddingChar characters as needed to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.</returns>
        public StringSegment PadRight(int totalWidth, char paddingChar) => PadHelper(totalWidth, paddingChar, isRightPadded: true);

        /// <summary>
        /// Helper method for string padding.
        /// </summary>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <param name="paddingChar">A Unicode padding character.</param>
        /// <param name="isRightPadded">Indicates whether padding happens on the right side (i.e. creates a left-aligned string).</param>
        /// <returns>A new string that is equivalent to this instance, but padded on the specified side with as many paddingChar characters as needed to create a length of totalWidth. However, if totalWidth is less than the length of this instance, the method returns a reference to the existing instance.</returns>
        private StringSegment PadHelper(int totalWidth, char paddingChar, bool isRightPadded)
        {
            if (String == null)
                throw new NullReferenceException();
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException(nameof(totalWidth));

            var padLength = totalWidth - Length;

            // NB: This behavior is different from padding in System.String where a padding length of 0 causes a copy of the string to be returned.

            if (padLength <= 0)
            {
                return this;
            }

            // NB: We can opportunistically look whether the original string has the desired padding characters in place, so we can just return a
            //     bigger StringSegment including these characters. If these characters are not in place, this will bail out quickly and resort
            //     to doing allocations. However, if the original string from which the segment is obtained is rather "sparse" then we may have a
            //     good chance to avoid allocations.

            if (isRightPadded)
            {
                var endIndex = StartIndex + totalWidth - 1;

                if (endIndex < String.Length)
                {
                    var i = StartIndex + Length;

                    for (; i <= endIndex; i++)
                    {
                        if (String[i] != paddingChar)
                            break;
                    }

                    if (i > endIndex)
                    {
                        return new StringSegment(String, StartIndex, totalWidth);
                    }
                }
            }
            else
            {
                var startIndex = StartIndex + Length - totalWidth;

                if (startIndex >= 0)
                {
                    var i = StartIndex - 1;

                    for (; i >= startIndex; i--)
                    {
                        if (String[i] != paddingChar)
                            break;
                    }

                    if (i < startIndex)
                    {
                        return new StringSegment(String, startIndex, totalWidth);
                    }
                }
            }

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', totalWidth); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, totalWidth);

                    if (!isRightPadded)
                    {
                        buffer.Append(paddingChar, padLength);
                    }

                    buffer.Append(this);

                    if (isRightPadded)
                    {
                        buffer.Append(paddingChar, padLength);
                    }
                }

                return res;
            }
#else
            var sb = new StringBuilder(totalWidth);

            if (!isRightPadded)
            {
                sb.Append(paddingChar, padLength);
            }

            sb.Append(String, StartIndex, Length);

            if (isRightPadded)
            {
                sb.Append(paddingChar, padLength);
            }

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Returns a new string in which all the characters in the current instance, beginning at a specified position and continuing through the last position, have been deleted.
        /// </summary>
        /// <param name="startIndex">The zero-based position to begin deleting characters.</param>
        /// <returns>A new string that is equivalent to this string except for the removed characters.</returns>
        public StringSegment Remove(int startIndex)
        {
            if (String == null)
                throw new NullReferenceException();
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex >= Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            return Substring(0, startIndex);
        }

        /// <summary>
        /// Returns a new string in which a specified number of characters in the current instance beginning at a specified position have been deleted.
        /// </summary>
        /// <param name="startIndex">The zero-based position to begin deleting characters.</param>
        /// <param name="count">The number of characters to delete.</param>
        /// <returns>A new string that is equivalent to this instance except for the removed characters.</returns>
        public StringSegment Remove(int startIndex, int count)
        {
            if (String == null)
                throw new NullReferenceException();
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count > Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(count));

            var num = Length - count;

            if (num == 0)
                return Empty;

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', num); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, num);

                    buffer.Append(String, StartIndex, startIndex);
                    buffer.Append(String, StartIndex + startIndex + count, num - startIndex);
                }

                return res;
            }
#else
            var sb = new StringBuilder(num);

            sb.Append(String, StartIndex, startIndex);
            sb.Append(String, StartIndex + startIndex + count, num - startIndex);

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
        /// </summary>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue. If oldValue is not found in the current instance, the method returns the current instance unchanged.</returns>
        public StringSegment Replace(StringSegment oldValue, StringSegment newValue)
        {
            if (String == null)
                throw new NullReferenceException();
            if (oldValue.String == null)
                throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0)
                throw new ArgumentException(string.Empty, nameof(oldValue));

            if (oldValue.Length == 1 && newValue.Length == 1)
            {
                return Replace(oldValue[0], newValue[0]);
            }

            var sb = default(StringBuilder);

            var i = 0;
            var n = Length - oldValue.Length;

            while (i <= n)
            {
                if (string.Compare(String, StartIndex + i, oldValue.String, oldValue.StartIndex, oldValue.Length) == 0)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(Length);
                        sb.Append(String, StartIndex, i);
                    }

                    if (newValue != null && newValue.Length > 0)
                    {
                        sb.Append(newValue.String, newValue.StartIndex, newValue.Length);
                    }

                    i += oldValue.Length;
                }
                else
                {
                    if (sb != null)
                    {
                        // CONSIDER: Don't append character by character but keep track of a range to copy.

                        sb.Append(this[i]);
                    }

                    i++;
                }
            }

            if (i != Length)
            {
                if (sb != null)
                {
                    sb.Append(String, StartIndex + i, Length - i);
                }
            }

            return sb == null ? this : sb.ToString();
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified Unicode character in this instance are replaced with another specified Unicode character.
        /// </summary>
        /// <param name="oldChar">The Unicode character to be replaced.</param>
        /// <param name="newChar">The Unicode character to replace all occurrences of oldChar.</param>
        /// <returns>A string that is equivalent to this instance except that all instances of oldChar are replaced with newChar. If oldChar is not found in the current instance, the method returns the current instance unchanged.</returns>
        public StringSegment Replace(char oldChar, char newChar)
        {
            if (String == null)
                throw new NullReferenceException();

            // CONSIDER: Maybe this can benefit from unsafe code with memcpy.

            var sb = default(StringBuilder);

            for (var i = 0; i < Length; i++)
            {
                var c = this[i];

                if (c == oldChar)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(Length);
                        sb.Append(String, StartIndex, Length);
                    }

                    sb[i] = newChar;
                }
            }

            return sb == null ? this : sb.ToString();
        }

        /// <summary>
        /// Splits a string into substrings that are based on the characters in an array.
        /// </summary>
        /// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <returns>An array whose elements contain the substrings from this instance that are delimited by one or more characters in separator.</returns>
        public StringSegment[] Split(params char[] separator) => SplitInternal(separator, int.MaxValue, StringSplitOptions.None);

        /// <summary>
        /// Splits a string into substrings based on the characters in an array. You can specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="options">StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public StringSegment[] Split(char[] separator, StringSplitOptions options) => SplitInternal(separator, int.MaxValue, options);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the characters in an array. You also specify the maximum number of substrings to return.
        /// </summary>
        /// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <returns>An array whose elements contain the substrings in this instance that are delimited by one or more characters in separator.</returns>
        public StringSegment[] Split(char[] separator, int count) => SplitInternal(separator, count, StringSplitOptions.None);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the characters in an array. You can specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        public StringSegment[] Split(char[] separator, int count, StringSplitOptions options) => SplitInternal(separator, count, options);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the characters in an array. You can specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more characters in separator.</returns>
        private StringSegment[] SplitInternal(char[] separator, int count, StringSplitOptions options)
        {
            if (String == null)
                throw new NullReferenceException();
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
                throw new ArgumentException(string.Empty, nameof(options));

            var omitEmptyEntries = (options == StringSplitOptions.RemoveEmptyEntries);

            if (count == 0 || (omitEmptyEntries && Length == 0))
                return s_emptyArray;

            var sepList = new int[Length]; // CONSIDER: same as System.String; should evaluate whether to use a (pooled) list instead
            var numReplaces = MakeSeparatorList(separator, ref sepList);

            if (numReplaces == 0 || count == 1)
                return new StringSegment[] { this };

            return omitEmptyEntries ? InternalSplitOmitEmptyEntries(sepList, lengthList: null, numReplaces, count) : InternalSplitKeepEmptyEntries(sepList, lengthList: null, numReplaces, count);
        }

        /// <summary>
        /// Makes a list of separator positions where splits have to occur.
        /// </summary>
        /// <param name="separator">A character array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="sepList">An array of separator positions where splits have to occur.</param>
        /// <returns>The number of separator positions found.</returns>
        private int MakeSeparatorList(char[] separator, ref int[] sepList)
        {
            var foundCount = 0;

            var sepListCount = sepList.Length;

            if (separator == null || separator.Length == 0)
            {
                for (var i = 0; i < Length && foundCount < sepListCount; i++)
                {
                    if (char.IsWhiteSpace(this[i]))
                    {
                        sepList[foundCount++] = i;
                    }
                }
            }
            else
            {
                for (var i = 0; i < Length && foundCount < sepListCount; i++)
                {
                    foreach (var c in separator)
                    {
                        if (this[i] == c)
                        {
                            sepList[foundCount++] = i;
                            break;
                        }
                    }
                }
            }

            return foundCount;
        }

        /// <summary>
        /// Splits a string into substrings based on the strings in an array. You can specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">A string array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="options">StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in separator.</returns>
        public StringSegment[] Split(StringSegment[] separator, StringSplitOptions options) => Split(separator, int.MaxValue, options);

        /// <summary>
        /// Splits a string into a maximum number of substrings based on the strings in an array. You can specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="separator">A string array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">StringSplitOptions.RemoveEmptyEntries to omit empty array elements from the array returned; or StringSplitOptions.None to include empty array elements in the array returned.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by one or more strings in separator.</returns>
        public StringSegment[] Split(StringSegment[] separator, int count, StringSplitOptions options)
        {
            if (String == null)
                throw new NullReferenceException();
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
                throw new ArgumentException(string.Empty, nameof(options));

            var omitEmptyEntries = (options == StringSplitOptions.RemoveEmptyEntries);

            if (separator == null || separator.Length == 0)
                return SplitInternal(default(char[]), count, options);

            if (count == 0 || (omitEmptyEntries && Length == 0))
                return s_emptyArray;

            var sepList = new int[Length]; // CONSIDER: same as System.String; should evaluate whether to use a (pooled) list instead
            var lengthList = new int[Length]; // CONSIDER: same as System.String; should evaluate whether to use a (pooled) list instead
            var numReplaces = MakeSeparatorList(separator, ref sepList, ref lengthList);

            if (numReplaces == 0 || count == 1)
                return new StringSegment[] { this };

            return omitEmptyEntries ? InternalSplitOmitEmptyEntries(sepList, lengthList, numReplaces, count) : InternalSplitKeepEmptyEntries(sepList, lengthList, numReplaces, count);
        }

        /// <summary>
        /// Makes a list of separator positions and lengths where splits have to occur.
        /// </summary>
        /// <param name="separators">A string array that delimits the substrings in this string, an empty array that contains no delimiters, or null.</param>
        /// <param name="sepList">An array of separator positions where splits have to occur.</param>
        /// <param name="lengthList">An array of separator lengths at the corresponding separator positions returned in <paramref name="sepList"/>.</param>
        /// <returns>The number of separator positions found.</returns>
        private int MakeSeparatorList(StringSegment[] separators, ref int[] sepList, ref int[] lengthList)
        {
            var foundCount = 0;

            var sepListCount = sepList.Length;
            var lengthListCount = lengthList.Length;

            for (var i = 0; i < Length && foundCount < sepListCount; i++)
            {
                for (var j = 0; j < separators.Length; j++)
                {
                    var separator = separators[j];

                    if (IsNullOrEmpty(separator))
                        continue;

                    var currentSepLength = separator.Length;

                    if (currentSepLength <= Length - i)
                    {
                        if (string.CompareOrdinal(String, StartIndex + i, separator.String, separator.StartIndex, currentSepLength) == 0)
                        {
                            sepList[foundCount] = i;
                            lengthList[foundCount] = currentSepLength;
                            foundCount++;
                            i += currentSepLength - 1;
                            break;
                        }
                    }
                }
            }

            return foundCount;
        }

        /// <summary>
        /// Performs a string split based on the specified separator positions and lengths, including empty entries.
        /// </summary>
        /// <param name="sepList">An array of separator positions where splits have to occur.</param>
        /// <param name="lengthList">An array of separator lengths at the corresponding separator positions or null if the length is always 1.</param>
        /// <param name="numReplaces">The number of separator positions found.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by the separator positions.</returns>
        private StringSegment[] InternalSplitKeepEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
        {
            var currIndex = 0;
            var arrIndex = 0;

            count--;

            var numActualReplaces = numReplaces < count ? numReplaces : count;
            var splitStrings = new StringSegment[numActualReplaces + 1];

            for (var i = 0; i < numActualReplaces && currIndex < Length; i++)
            {
                splitStrings[arrIndex++] = Substring(currIndex, sepList[i] - currIndex);
                currIndex = sepList[i] + (lengthList == null ? 1 : lengthList[i]);
            }

            if (currIndex < Length && numActualReplaces >= 0)
            {
                splitStrings[arrIndex] = Substring(currIndex);
            }
            else if (arrIndex == numActualReplaces)
            {
                splitStrings[arrIndex] = Empty;
            }

            return splitStrings;
        }

        /// <summary>
        /// Performs a string split based on the specified separator positions and lengths, omitting empty entries.
        /// </summary>
        /// <param name="sepList">An array of separator positions where splits have to occur.</param>
        /// <param name="lengthList">An array of separator lengths at the corresponding separator positions or null if the length is always 1.</param>
        /// <param name="numReplaces">The number of separator positions found.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <returns>An array whose elements contain the substrings in this string that are delimited by the separator positions.</returns>
        private StringSegment[] InternalSplitOmitEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
        {
            var currIndex = 0;
            var arrIndex = 0;

            var maxItems = numReplaces < count ? numReplaces + 1 : count;
            var splitStrings = new StringSegment[maxItems];

            for (var i = 0; i < numReplaces && currIndex < Length; i++)
            {
                if (sepList[i] - currIndex > 0)
                {
                    splitStrings[arrIndex++] = Substring(currIndex, sepList[i] - currIndex);
                }

                currIndex = sepList[i] + (lengthList == null ? 1 : lengthList[i]);

                if (arrIndex == count - 1)
                {
                    while (i < numReplaces - 1 && currIndex == sepList[++i])
                    {
                        currIndex += lengthList == null ? 1 : lengthList[i];
                    }

                    break;
                }
            }

            if (currIndex < Length)
            {
                splitStrings[arrIndex++] = Substring(currIndex);
            }

            var res = splitStrings;

            if (arrIndex != maxItems)
            {
                res = new StringSegment[arrIndex];

                for (var j = 0; j < arrIndex; j++)
                {
                    res[j] = splitStrings[j];
                }
            }

            return res;
        }

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <returns>true if value matches the beginning of this string segment; otherwise, false.</returns>
        public bool StartsWith(StringSegment value) => StartsWith(value, StringComparison.CurrentCulture);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string when compared using the specified comparison option.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how this string and value are compared.</param>
        /// <returns>true if this string segment begins with value; otherwise, false.</returns>
        public bool StartsWith(StringSegment value, StringComparison comparisonType)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));

            if (StrictEquals(value))
                return true;

            if (value.Length == 0)
                return true;

            if (value.Length > Length)
                return false;

            return string.Compare(String, StartIndex, value.String, value.StartIndex, value.Length, comparisonType) == 0;
        }

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string when compared using the specified culture.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <param name="culture">Cultural information that determines how this string and value are compared. If culture is null, the current culture is used.</param>
        /// <returns>true if the value parameter matches the beginning of this string segment; otherwise, false.</returns>
        public bool StartsWith(StringSegment value, bool ignoreCase, CultureInfo culture)
        {
            if (String == null)
                throw new NullReferenceException();
            if (value.String == null)
                throw new ArgumentNullException(nameof(value));

            if (StrictEquals(value))
                return true;

            if (value.Length == 0)
                return true;

            if (value.Length > Length)
                return false;

            return string.Compare(String, StartIndex, value.String, value.StartIndex, value.Length, ignoreCase, culture ?? CultureInfo.CurrentCulture) == 0;
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and continues to the end of the string.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <returns>A string that is equivalent to the substring that begins at startIndex in this instance, or Empty if startIndex is equal to the length of this instance.</returns>
        public StringSegment Substring(int startIndex) => Substring(startIndex, Length - startIndex);

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A string that is equivalent to the substring of length length that begins at startIndex in this instance, or Empty if startIndex is equal to the length of this instance and length is zero.</returns>
        public StringSegment Substring(int startIndex, int length)
        {
            if (String == null)
                throw new NullReferenceException();
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (startIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex > Length - length)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (length == 0)
                return string.Empty;

            if (startIndex == 0 && length == Length)
                return this;

            return new StringSegment(String, StartIndex + startIndex, length);
        }

        /// <summary>
        /// Copies the characters in this instance to a Unicode character array.
        /// </summary>
        /// <returns>A Unicode character array whose elements are the individual characters of this instance. If this instance is an empty string, the returned array is empty and has a zero length.</returns>
        public char[] ToCharArray()
        {
            if (String == null)
                throw new NullReferenceException();

            return String.ToCharArray(StartIndex, Length);
        }

        /// <summary>
        /// Copies the characters in a specified substring in this instance to a Unicode character array.
        /// </summary>
        /// <param name="startIndex">The starting position of a substring in this instance.</param>
        /// <param name="length">The length of the substring in this instance.</param>
        /// <returns>A Unicode character array whose elements are the length number of characters in this instance starting from character position startIndex.</returns>
        public char[] ToCharArray(int startIndex, int length)
        {
            if (String == null)
                throw new NullReferenceException();
            if (startIndex < 0 || startIndex > Length || startIndex > Length - length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            return String.ToCharArray(StartIndex + startIndex, length);
        }

        /// <summary>
        /// Returns a copy of this string converted to lowercase.
        /// </summary>
        /// <returns>A string in lowercase.</returns>
        public StringSegment ToLower() => ToLower(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a copy of this string converted to lowercase, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The lowercase equivalent of the current string.</returns>
        public StringSegment ToLower(CultureInfo culture)
        {
            if (String == null)
                throw new NullReferenceException();
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            // CONSIDER: No allocation if no characters change; check BCL behavior.

#if ALLOW_UNSAFE
            unsafe
            {
                var res = new string('\0', Length); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* str = res)
                {
                    for (var i = 0; i < Length; i++)
                    {
                        var c = this[i];
                        str[i] = culture.TextInfo.ToLower(c);
                    }
                }

                return res;
            }
#else
            var sb = new StringBuilder(Length);

            for (var i = 0; i < Length; i++)
            {
                var c = this[i];
                sb.Append(culture.TextInfo.ToLower(c));
            }

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Returns a copy of this System.String object converted to lowercase using the casing rules of the invariant culture.
        /// </summary>
        /// <returns>The lowercase equivalent of the current string.</returns>
        public StringSegment ToLowerInvariant() => ToLower(CultureInfo.InvariantCulture);

        /// <summary>
        /// Returns a copy of this string converted to uppercase.
        /// </summary>
        /// <returns>The uppercase equivalent of the current string.</returns>
        public StringSegment ToUpper() => ToUpper(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns a copy of this string converted to uppercase, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The uppercase equivalent of the current string.</returns>
        public StringSegment ToUpper(CultureInfo culture)
        {
            if (String == null)
                throw new NullReferenceException();
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            // CONSIDER: No allocation if no characters change; check BCL behavior.

#if ALLOW_UNSAFE
            unsafe
            {
                var res = new string('\0', Length); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* str = res)
                {
                    for (var i = 0; i < Length; i++)
                    {
                        var c = this[i];
                        str[i] = culture.TextInfo.ToUpper(c);
                    }
                }

                return res;
            }
#else
            var sb = new StringBuilder(Length);

            for (var i = 0; i < Length; i++)
            {
                var c = this[i];
                sb.Append(culture.TextInfo.ToUpper(c));
            }

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Returns a copy of this System.String object converted to uppercase using the casing rules of the invariant culture.
        /// </summary>
        /// <returns>The uppercase equivalent of the current string.</returns>
        public StringSegment ToUpperInvariant() => ToUpper(CultureInfo.InvariantCulture);

        /// <summary>
        /// Removes all leading and trailing white-space characters from the current System.String object.
        /// </summary>
        /// <returns>The string that remains after all white-space characters are removed from the start and end of the current string. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
        public StringSegment Trim() => TrimHelper(TrimBoth);

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified in an array from the current System.String object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>The string that remains after all occurrences of the characters in the trimChars parameter are removed from the start and end of the current string. If trimChars is null or an empty array, white-space characters are removed instead. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
        public StringSegment Trim(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
            {
                return TrimHelper(TrimBoth);
            }

            return TrimHelper(trimChars, TrimBoth);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified in an array from the current System.String object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>The string that remains after all occurrences of the characters in the trimChars parameter are removed from the end of the current string. If trimChars is null or an empty array, Unicode white-space characters are removed instead. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
        public StringSegment TrimEnd(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
            {
                return TrimHelper(TrimTail);
            }

            return TrimHelper(trimChars, TrimTail);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of characters specified in an array from the current System.String object.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <returns>The string that remains after all occurrences of characters in the trimChars parameter are removed from the start of the current string. If trimChars is null or an empty array, white-space characters are removed instead. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
        public StringSegment TrimStart(params char[] trimChars)
        {
            if (trimChars == null || trimChars.Length == 0)
            {
                return TrimHelper(TrimHead);
            }

            return TrimHelper(trimChars, TrimHead);
        }

        /// <summary>
        /// Helper function to perform string trimming of a specified kind.
        /// </summary>
        /// <param name="trimType">Any of the following trim kinds: TrimHead, TrimTail, or TrimBoth.</param>
        /// <returns>The string that remains after all white-space are removed from the start and/or end of the current string. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
        private StringSegment TrimHelper(int trimType)
        {
            if (String == null)
                throw new NullReferenceException();

            var end = Length - 1;
            var start = 0;

            if (trimType != TrimTail)
            {
                for (start = 0; start < Length; start++)
                {
                    if (!char.IsWhiteSpace(this[start]))
                        break;
                }
            }

            if (trimType != TrimHead)
            {
                for (end = Length - 1; end >= start; end--)
                {
                    if (!char.IsWhiteSpace(this[end]))
                        break;
                }
            }

            return CreateTrimmedString(start, end);
        }

        /// <summary>
        /// Helper function to perform string trimming of a specified kind with the specified trim characters.
        /// </summary>
        /// <param name="trimChars">An array of Unicode characters to remove, or null.</param>
        /// <param name="trimType">Any of the following trim kinds: TrimHead, TrimTail, or TrimBoth.</param>
        /// <returns>The string that remains after all occurrences of characters in the trimChars parameter are removed from the start and/or end of the current string. If trimChars is null or an empty array, white-space characters are removed instead. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
        private StringSegment TrimHelper(char[] trimChars, int trimType)
        {
            if (String == null)
                throw new NullReferenceException();

            var end = Length - 1;
            var start = 0;

            if (trimType != TrimTail)
            {
                for (start = 0; start < Length; start++)
                {
                    var i = 0;
                    var c = this[start];

                    for (i = 0; i < trimChars.Length; i++)
                    {
                        if (trimChars[i] == c)
                            break;
                    }

                    if (i == trimChars.Length)
                        break;
                }
            }

            if (trimType != TrimHead)
            {
                for (end = Length - 1; end >= start; end--)
                {
                    var i = 0;
                    var c = this[end];

                    for (i = 0; i < trimChars.Length; i++)
                    {
                        if (trimChars[i] == c)
                            break;
                    }

                    if (i == trimChars.Length)
                        break;
                }
            }

            return CreateTrimmedString(start, end);
        }

        /// <summary>
        /// Creates a trimmed string from the specified start and end index in the current instance.
        /// </summary>
        /// <param name="start">The index of the first character to keep.</param>
        /// <param name="end">The index of the last character to keep.</param>
        /// <returns>A substring of the current string between the specified start and end indexes.</returns>
        private StringSegment CreateTrimmedString(int start, int end)
        {
            var len = end - start + 1;

            if (len == Length)
            {
                return this;
            }

            if (len == 0)
            {
                return Empty;
            }

            return Substring(start, len);
        }

        /// <summary>
        /// Compares two specified string segments and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, StringSegment strB) => CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);


        /// <summary>
        /// Compares two specified string segments, ignoring or honoring their case, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, StringSegment strB, bool ignoreCase) => CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);

        /// <summary>
        /// Compares two specified string segments, ignoring or honoring their case, and using culture-specific information to influence the comparison, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <param name="culture">An object that supplies culture-specific comparison information.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, StringSegment strB, bool ignoreCase, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            return culture.CompareInfo.Compare(strA, strB, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        /// <summary>
        /// Compares two specified string segments using the specified rules, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, StringSegment strB, StringComparison comparisonType)
        {
            if (strA.String == null && strB.String == null)
            {
                return 0;
            }

            if (strA.String == null)
            {
                return -1;
            }

            if (strB.String == null)
            {
                return 1;
            }

            switch (comparisonType)
            {
                case StringComparison.CurrentCulture:
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
                case StringComparison.CurrentCultureIgnoreCase:
                    return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
                case StringComparison.InvariantCulture:
                    return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
                case StringComparison.InvariantCultureIgnoreCase:
                    return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
                case StringComparison.Ordinal:
                    return CompareOrdinal(strA, strB);
                case StringComparison.OrdinalIgnoreCase:
                    return CompareOrdinalIgnoreCase(strA, strB);
                default:
                    throw new ArgumentException("Unsupported string comparison type.", nameof(comparisonType));
            }
        }

        /// <summary>
        /// Compares two specified string segments using the specified comparison options and culture-specific information to influence the comparison, and returns an integer that indicates the relationship of the two strings to each other in the sort order.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <param name="culture">An object that supplies culture-specific comparison information.</param>
        /// <param name="options">Options to use when performing the comparison (such as ignoring case or symbols).</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, StringSegment strB, CultureInfo culture, CompareOptions options)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            return culture.CompareInfo.Compare(strA, strB, options);
        }

        /// <summary>
        /// Compares substrings of two specified string segments and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The position of the substring within strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The position of the substring within strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, int indexA, StringSegment strB, int indexB, int length)
        {
            CheckCompareArguments(strA, indexA, strB, indexB, length, out var lengthA, out var lengthB);

            return CultureInfo.CurrentCulture.CompareInfo.Compare(strA.String, strA.StartIndex + indexA, lengthA, strB.String, strB.StartIndex + indexB, lengthB, CompareOptions.None);
        }

        /// <summary>
        /// Compares substrings of two specified string segments, ignoring or honoring their case, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The position of the substring within strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The position of the substring within strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, int indexA, StringSegment strB, int indexB, int length, bool ignoreCase)
        {
            CheckCompareArguments(strA, indexA, strB, indexB, length, out var lengthA, out var lengthB);

            return CultureInfo.CurrentCulture.CompareInfo.Compare(strA.String, strA.StartIndex + indexA, lengthA, strB.String, strB.StartIndex + indexB, lengthB, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        /// <summary>
        /// Compares substrings of two specified string segments using the specified rules, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The position of the substring within strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The position of the substring within strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, int indexA, StringSegment strB, int indexB, int length, StringComparison comparisonType)
        {
            var num = CheckCompareArguments(strA, indexA, strB, indexB, length, out var lengthA, out var lengthB);

            var res = string.Compare(strA.String, strA.StartIndex + indexA, strB.String, strB.StartIndex + indexB, num, comparisonType);

            if (res == 0 && (lengthA != length || lengthB != length))
            {
                res = lengthA - lengthB;
            }

            return res;
        }

        /// <summary>
        /// Compares substrings of two specified string segments, ignoring or honoring their case and using culture-specific information to influence the comparison, and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The position of the substring within strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The position of the substring within strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
        /// <param name="culture">An object that supplies culture-specific comparison information.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, int indexA, StringSegment strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            CheckCompareArguments(strA, indexA, strB, indexB, length, out var lengthA, out var lengthB);

            return culture.CompareInfo.Compare(strA.String, strA.StartIndex + indexA, lengthA, strB.String, strB.StartIndex + indexB, lengthB, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        /// <summary>
        /// Compares substrings of two specified string segments using the specified comparison options and culture-specific information to influence the comparison, and returns an integer that indicates the relationship of the two substrings to each other in the sort order.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The position of the substring within strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The position of the substring within strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <param name="culture">An object that supplies culture-specific comparison information.</param>
        /// <param name="options">Options to use when performing the comparison (such as ignoring case or symbols).</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int Compare(StringSegment strA, int indexA, StringSegment strB, int indexB, int length, CultureInfo culture, CompareOptions options)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            CheckCompareArguments(strA, indexA, strB, indexB, length, out var lengthA, out var lengthB);

            return culture.CompareInfo.Compare(strA.String, strA.StartIndex + indexA, lengthA, strB.String, strB.StartIndex + indexB, lengthB, options);
        }

        /// <summary>
        /// Compares two specified string segments by evaluating the numeric values of the corresponding System.Char objects in each string.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int CompareOrdinal(StringSegment strA, StringSegment strB)
        {
            var res = string.CompareOrdinal(strA.String, strA.StartIndex, strB.String, strB.StartIndex, Math.Min(strA.Length, strB.Length));

            if (res == 0)
            {
                res = strA.Length - strB.Length;
            }

            return res;
        }

        /// <summary>
        /// Compares two specified string segments by evaluating the numeric values of the corresponding System.Char objects in each string but ignoring their casing.
        /// </summary>
        /// <param name="strA">The first string to compare.</param>
        /// <param name="strB">The second string to compare.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        private static int CompareOrdinalIgnoreCase(StringSegment strA, StringSegment strB)
        {
            var res = string.Compare(strA.String, strA.StartIndex, strB.String, strB.StartIndex, Math.Min(strA.Length, strB.Length), StringComparison.OrdinalIgnoreCase);

            if (res == 0)
            {
                res = strA.Length - strB.Length;
            }

            return res;
        }

        /// <summary>
        /// Compares substrings of two specified string segments by evaluating the numeric values of the corresponding System.Char objects in each substring.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The starting index of the substring in strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The starting index of the substring in strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <returns>A 32-bit signed integer that indicates the lexical relationship between the two comparands.</returns>
        public static int CompareOrdinal(StringSegment strA, int indexA, StringSegment strB, int indexB, int length)
        {
            var num = CheckCompareArguments(strA, indexA, strB, indexB, length, out var lengthA, out var lengthB);

            var res = string.CompareOrdinal(strA.String, strA.StartIndex + indexA, strB.String, strB.StartIndex + indexB, num);

            if (res == 0 && (lengthA != length || lengthB != length))
            {
                res = lengthA - lengthB;
            }

            return res;
        }

        /// <summary>
        /// Checks the arguments used in a comparison operation, returning the number of characters to compare based on string lengths and indexes.
        /// </summary>
        /// <param name="strA">The first string to use in the comparison.</param>
        /// <param name="indexA">The starting index of the substring in strA.</param>
        /// <param name="strB">The second string to use in the comparison.</param>
        /// <param name="indexB">The starting index of the substring in strB.</param>
        /// <param name="length">The maximum number of characters in the substrings to compare.</param>
        /// <param name="lengthA">The number of characters to compare in strA.</param>
        /// <param name="lengthB">The number of characters to compare in strB.</param>
        /// <returns>The number of characters to compare, i.e. the lesser of the lengths.</returns>
        private static int CheckCompareArguments(StringSegment strA, int indexA, StringSegment strB, int indexB, int length, out int lengthA, out int lengthB)
        {
            if (indexA < 0 || indexA > strA.Length)
                throw new ArgumentOutOfRangeException(nameof(indexA));
            if (indexB < 0 || indexB > strB.Length)
                throw new ArgumentOutOfRangeException(nameof(indexB));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if ((strA.String == null || strB.String == null) && length > 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            lengthA = length;
            lengthB = length;

            if (strA.String != null)
            {
                if (strA.Length - indexA < lengthA)
                {
                    lengthA = strA.Length - indexA;
                }
            }

            if (strB.String != null)
            {
                if (strB.Length - indexB < lengthB)
                {
                    lengthB = strB.Length - indexB;
                }
            }

            return Math.Min(length, Math.Min(lengthA, lengthB));
        }

        /// <summary>
        /// Concatenates two specified string segments.
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <returns>The concatenation of str0 and str1.</returns>
        public static StringSegment Concat(StringSegment str0, StringSegment str1)
        {
            if (IsNullOrEmpty(str0))
            {
                if (IsNullOrEmpty(str1))
                {
                    return Empty;
                }

                return str1;
            }

            if (IsNullOrEmpty(str1))
            {
                return str0;
            }

            var num = str0.Length + str1.Length;

            if ((object)str0.String == (object)str1.String)
            {
                if (str0.StartIndex + str0.Length == str1.StartIndex)
                {
                    return new StringSegment(str0.String, str0.StartIndex, num);
                }
            }

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', num); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, num);

                    buffer.Append(str0.String, str0.StartIndex, str0.Length);
                    buffer.Append(str1.String, str1.StartIndex, str1.Length);
                }

                return res;
            }
#else
            var sb = new StringBuilder(num);

            sb.Append(str0.String, str0.StartIndex, str0.Length);
            sb.Append(str1.String, str1.StartIndex, str1.Length);

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Concatenates three specified string segments.
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <param name="str2">The third string to concatenate.</param>
        /// <returns>The concatenation of str0, str1, and str2.</returns>
        public static StringSegment Concat(StringSegment str0, StringSegment str1, StringSegment str2)
        {
            if (IsNullOrEmpty(str0) && IsNullOrEmpty(str1) && IsNullOrEmpty(str2))
            {
                return Empty;
            }

            if (str0.String == null)
            {
                str0 = Empty;
            }

            if (str1.String == null)
            {
                str1 = Empty;
            }

            if (str2.String == null)
            {
                str2 = Empty;
            }

            var num = str0.Length + str1.Length + str2.Length;

            if ((object)str0.String == (object)str1.String && (object)str1.String == (object)str2.String)
            {
                if (str0.StartIndex + str0.Length == str1.StartIndex && str1.StartIndex + str1.Length == str2.StartIndex)
                {
                    return new StringSegment(str0.String, str0.StartIndex, num);
                }
            }

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', num); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, num);

                    buffer.Append(str0.String, str0.StartIndex, str0.Length);
                    buffer.Append(str1.String, str1.StartIndex, str1.Length);
                    buffer.Append(str2.String, str2.StartIndex, str2.Length);
                }

                return res;
            }
#else
            var sb = new StringBuilder(num);

            sb.Append(str0.String, str0.StartIndex, str0.Length);
            sb.Append(str1.String, str1.StartIndex, str1.Length);
            sb.Append(str2.String, str2.StartIndex, str2.Length);

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Concatenates four specified string segments.
        /// </summary>
        /// <param name="str0">The first string to concatenate.</param>
        /// <param name="str1">The second string to concatenate.</param>
        /// <param name="str2">The third string to concatenate.</param>
        /// <param name="str3">The fourth string to concatenate.</param>
        /// <returns>The concatenation of str0, str1, str2, and str3.</returns>
        public static StringSegment Concat(StringSegment str0, StringSegment str1, StringSegment str2, StringSegment str3)
        {
            if (IsNullOrEmpty(str0) && IsNullOrEmpty(str1) && IsNullOrEmpty(str2) && IsNullOrEmpty(str3))
            {
                return Empty;
            }

            if (str0.String == null)
            {
                str0 = Empty;
            }

            if (str1.String == null)
            {
                str1 = Empty;
            }

            if (str2.String == null)
            {
                str2 = Empty;
            }

            if (str3.String == null)
            {
                str3 = Empty;
            }

            var num = str0.Length + str1.Length + str2.Length + str3.Length;

            if ((object)str0.String == (object)str1.String && (object)str1.String == (object)str2.String && (object)str2.String == (object)str3.String)
            {
                if (str0.StartIndex + str0.Length == str1.StartIndex && str1.StartIndex + str1.Length == str2.StartIndex && str2.StartIndex + str2.Length == str3.StartIndex)
                {
                    return new StringSegment(str0.String, str0.StartIndex, num);
                }
            }

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', num); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, num);

                    buffer.Append(str0.String, str0.StartIndex, str0.Length);
                    buffer.Append(str1.String, str1.StartIndex, str1.Length);
                    buffer.Append(str2.String, str2.StartIndex, str2.Length);
                    buffer.Append(str3.String, str3.StartIndex, str3.Length);
                }

                return res;
            }
#else
            var sb = new StringBuilder(num);

            sb.Append(str0.String, str0.StartIndex, str0.Length);
            sb.Append(str1.String, str1.StartIndex, str1.Length);
            sb.Append(str2.String, str2.StartIndex, str2.Length);
            sb.Append(str3.String, str3.StartIndex, str3.Length);

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Concatenates the elements of a specified string segment array.
        /// </summary>
        /// <param name="values">An array of string segments.</param>
        /// <returns>The concatenated elements of values.</returns>
        public static StringSegment Concat(params StringSegment[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (values.Length == 0)
            {
                return Empty;
            }

            var text = values[0];

            if (values.Length == 1)
            {
                return text.String == null ? Empty : text;
            }

            var str = text.String ?? string.Empty;
            var idx = text.StartIndex;
            var num = text.Length;

            var allAdjacent = true;

            for (var i = 1; i < values.Length; i++)
            {
                text = values[i];

                if (allAdjacent)
                {
                    allAdjacent = (object)str == (object)(text.String ?? string.Empty) && text.StartIndex == idx + num;
                }

                num += text.Length;

                CheckOverflow(num);
            }

            if (allAdjacent)
            {
                return new StringSegment(str, idx, num);
            }

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', num); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                var rem = num;

                fixed (char* destinationStr = res)
                {
                    var buffer = new UnsafeCharBuffer(destinationStr, num);

                    for (var i = 0; i < values.Length; i++)
                    {
                        text = values[i];

                        buffer.Append(text.String, text.StartIndex, text.Length);
                    }
                }

                return res;
            }
#else
            var sb = new StringBuilder(num);

            for (var i = 0; i < values.Length; i++)
            {
                text = values[i];

                sb.Append(text.String, text.StartIndex, text.Length);
            }

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Concatenates the members of a constructed IEnumerable{T} collection of type string segments.
        /// </summary>
        /// <param name="values">A collection object that implements IEnumerable{T} and whose generic type argument is StringSegment.</param>
        /// <returns>The concatenated strings in values.</returns>
        public static StringSegment Concat(IEnumerable<StringSegment> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            // CONSIDER: Optimizations for IList<StringSegment> that can rely on Count.

            if (values is StringSegment[] array)
            {
                return Concat(array);
            }

            using (var psb = PooledStringBuilder.New())
            {
                var sb = psb.StringBuilder;

                using (var enumerator = values.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var value = enumerator.Current;

                        if (value.String != null && value.Length > 0)
                        {
                            sb.Append(value.String, value.StartIndex, value.Length);
                        }
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Creates the string representation of a specified object.
        /// </summary>
        /// <param name="arg0">The object to represent, or null.</param>
        /// <returns>The string representation of the value of arg0, or Empty if arg0 is null.</returns>
        public static StringSegment Concat(object arg0)
        {
            if (arg0 is StringSegment str0)
            {
                return str0;
            }

            return string.Concat(arg0);
        }

        /// <summary>
        /// Concatenates the string representations of two specified objects.
        /// </summary>
        /// <param name="arg0">The first object to concatenate.</param>
        /// <param name="arg1">The second object to concatenate.</param>
        /// <returns>The concatenated string representations of the values of arg0 and arg1.</returns>
        public static StringSegment Concat(object arg0, object arg1)
        {
            if (arg0 is StringSegment str0 && arg1 is StringSegment str1)
            {
                return Concat(str0, str1);
            }

            return string.Concat(arg0, arg1);
        }

        /// <summary>
        /// Concatenates the string representations of three specified objects.
        /// </summary>
        /// <param name="arg0">The first object to concatenate.</param>
        /// <param name="arg1">The second object to concatenate.</param>
        /// <param name="arg2">The third object to concatenate.</param>
        /// <returns>The concatenated string representations of the values of arg0, arg1, and arg2.</returns>
        public static StringSegment Concat(object arg0, object arg1, object arg2)
        {
            if (arg0 is StringSegment str0 && arg1 is StringSegment str1 && arg2 is StringSegment str2)
            {
                return Concat(str0, str1, str2);
            }

            return string.Concat(arg0, arg1, arg2);
        }

        /// <summary>
        /// Concatenates the string representations of four specified objects.
        /// </summary>
        /// <param name="arg0">The first object to concatenate.</param>
        /// <param name="arg1">The second object to concatenate.</param>
        /// <param name="arg2">The third object to concatenate.</param>
        /// <param name="arg3">The fourth object to concatenate.</param>
        /// <returns>The concatenated string representations of the values of arg0, arg1, arg2, and arg3.</returns>
        public static StringSegment Concat(object arg0, object arg1, object arg2, object arg3)
        {
            if (arg0 is StringSegment str0 && arg1 is StringSegment str1 && arg2 is StringSegment str2 && arg3 is StringSegment str3)
            {
                return Concat(str0, str1, str2, str3);
            }

            return string.Concat(arg0, arg1, arg2, arg3);
        }

#if !NETSTANDARD
        /// <summary>
        /// Concatenates the string representations of four specified objects and the objects in the arg iterator.
        /// </summary>
        /// <param name="arg0">The first object to concatenate.</param>
        /// <param name="arg1">The second object to concatenate.</param>
        /// <param name="arg2">The third object to concatenate.</param>
        /// <param name="arg3">The fourth object to concatenate.</param>
        /// <returns>The concatenated string representations of the values of arg0, arg1, arg2, arg3, and the values in the arg iterator.</returns>
        [CLSCompliant(false)] // NB: __arglist is not CLS compliant
        public static StringSegment Concat(object arg0, object arg1, object arg2, object arg3, __arglist)
        {
            // NB: The case where all arguments are boxed StringSegment instances is very unlikely, so we won't bother to attempt an optimization here.

            var argIterator = new ArgIterator(__arglist);

            var num = argIterator.GetRemainingCount() + 4;

            var array = new object[num];

            array[0] = arg0;
            array[1] = arg1;
            array[2] = arg2;
            array[3] = arg3;

            for (var i = 4; i < num; i++)
            {
                array[i] = TypedReference.ToObject(argIterator.GetNextArg());
            }

            return string.Concat(array);
        }
#endif

        /// <summary>
        /// Concatenates the string representations of the elements in a specified System.Object array.
        /// </summary>
        /// <param name="args">An object array that contains the elements to concatenate.</param>
        /// <returns>The concatenated string representations of the values of the elements in args.</returns>
        public static StringSegment Concat(params object[] args) => string.Concat(args);

        /// <summary>
        /// Concatenates the members of an IEnumerable{T} implementation.
        /// </summary>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <param name="values">A collection object that implements the IEnumerable{T} interface.</param>
        /// <returns>The concatenated members in values.</returns>
        public static StringSegment Concat<T>(IEnumerable<T> values) => typeof(T) == typeof(StringSegment) ? Concat((IEnumerable<StringSegment>)values) : string.Concat(values);

#if !NET5_0 // https://github.com/dotnet/runtime/issues/27515
        /// <summary>
        /// Creates a string segment containing new instance of a System.String with the same value as a specified string segment.
        /// </summary>
        /// <param name="str">The string to copy.</param>
        /// <returns>A new string with the same value as str.</returns>
        public static StringSegment Copy(StringSegment str)
        {
            if (str.String == null)
                throw new ArgumentNullException(nameof(str));

            var res = str.ToString();

            if (res == str.String)
            {
                res = string.Copy(res);
            }

            return res;
        }
#endif

        /// <summary>
        /// Determines whether two specified string segments have the same value.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <returns>true if the value of a is the same as the value of b; otherwise, false. If both a and b are null, the method returns true.</returns>
        public static bool Equals(StringSegment a, StringSegment b) => a.Equals(b);

        /// <summary>
        /// Determines whether two specified string segments have the same value. A parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="a">The first string to compare, or null.</param>
        /// <param name="b">The second string to compare, or null.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
        /// <returns>true if the value of the a parameter is equal to the value of the b parameter; otherwise, false.</returns>
        public static bool Equals(StringSegment a, StringSegment b, StringComparison comparisonType) => a.Equals(b, comparisonType);

        // NB: The implementations of the Format methods relies on string allocation for the format string right now. It's rather unlikely that formatters
        //     come from a bigger string as segments, so we can live with this allocation and stay away from building a format string parser a la the one
        //     that exists in StringBuilder.AppendFormatHelper. This is really a place where StringSegment-based functionality would have to be exposed in
        //     the StringBuilder methods.

        /// <summary>
        /// Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static StringSegment Format(StringSegment format, params object[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException(nameof(format));

            return string.Format((string)format /* ALLOC */, args);
        }

        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format.</param>
        /// <returns>A copy of format in which any format items are replaced by the string representation of arg0.</returns>
        public static StringSegment Format(StringSegment format, object arg0)
        {
            if (format.String == null)
                throw new ArgumentNullException(nameof(format));

            return string.Format((string)format /* ALLOC */, arg0);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representation of two specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>A copy of format in which format items are replaced by the string representations of arg0 and arg1.</returns>
        public static StringSegment Format(StringSegment format, object arg0, object arg1)
        {
            if (format.String == null)
                throw new ArgumentNullException(nameof(format));

            return string.Format((string)format /* ALLOC */, arg0, arg1);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representation of three specified objects.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <param name="arg2">The third object to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representations of arg0, arg1, and arg2.</returns>
        public static StringSegment Format(StringSegment format, object arg0, object arg1, object arg2)
        {
            if (format.String == null)
                throw new ArgumentNullException(nameof(format));

            return string.Format((string)format /* ALLOC */, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representations of corresponding objects in a specified array. A parameter supplies culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static StringSegment Format(IFormatProvider provider, StringSegment format, params object[] args)
        {
            if (format.String == null)
                throw new ArgumentNullException(nameof(format));

            return string.Format(provider, (string)format /* ALLOC */, args);
        }


        // NB: Semantics for interning methods on StringSegment are debatable. Do they imply an implicit ToString operation prior to calling the underlying
        //     interning methods for String? Would an IsInterned avoid such an allocation by checking whether the whole underlying String is interned? It
        //     seems better to steer away from these questions and omit this functionality.

        // public static StringSegment Intern(StringSegment str);
        // public static StringSegment IsInterned(StringSegment str);


        // NB: Use of these methods is rare enough to warrant their omission given the fact that we don't have a good way to perform these checks in an
        //     allocation-free manner. The most efficient option would be to check whether the whole underlying string is normalized.

        // public bool IsNormalized() { return IsNormalized(NormalizationForm.FormC); }
        // public bool IsNormalized(NormalizationForm normalizationForm) { return ToString().IsNormalized(normalizationForm); } // NB: no helper handy to do this without allocations


        /// <summary>
        /// Indicates whether the specified string is null or an empty string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(StringSegment value) => value.String == null || value.Length == 0;

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""), or if value consists exclusively of white-space characters; otherwise, false.</returns>
        public static bool IsNullOrWhiteSpace(StringSegment value)
        {
            if (value.String == null)
                return true;

            for (var i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Concatenates the members of a constructed IEnumerable{T} of collection of string segments, using the specified separator between each member.
        /// </summary>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns Empty.</returns>
        public static StringSegment Join(StringSegment separator, IEnumerable<StringSegment> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (separator.String == null)
                separator = Empty;

            using (var enumerator = values.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return string.Empty;

                using (var psb = PooledStringBuilder.New())
                {
                    var sb = psb.StringBuilder;

                    var current = enumerator.Current;

                    if (!IsNullOrEmpty(current))
                    {
                        sb.Append(enumerator.Current);
                    }

                    while (enumerator.MoveNext())
                    {
                        sb.Append(separator.String, separator.StartIndex, separator.Length);

                        current = enumerator.Current;

                        if (!IsNullOrEmpty(current))
                        {
                            sb.Append(current.String, current.StartIndex, current.Length);
                        }
                    }

                    return sb.ToString();
                }
            }
        }

        /// <summary>
        /// Concatenates all the elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if value has more than one element.</param>
        /// <param name="value">An array that contains the elements to concatenate.</param>
        /// <returns>A string that consists of the elements in value delimited by the separator string. If value is an empty array, the method returns Empty.</returns>
        public static StringSegment Join(StringSegment separator, params StringSegment[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Join(separator, value, 0, value.Length);
        }

        /// <summary>
        /// Concatenates the specified elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if value has more than one element.</param>
        /// <param name="value">An array that contains the elements to concatenate.</param>
        /// <param name="startIndex">The first element in value to use.</param>
        /// <param name="count">The number of elements of value to use.</param>
        /// <returns>A string that consists of the strings in value delimited by the separator string -or- Empty if count is zero, value has no elements, or separator and all the elements of value are System.String.Empty..</returns>
        public static StringSegment Join(StringSegment separator, StringSegment[] value, int startIndex, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex > value.Length - count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (separator.String == null)
                separator = Empty;

            if (count == 0)
                return Empty;

            var item = default(StringSegment);
            var jointLength = 0;
            var endIndex = startIndex + count - 1;

            for (var stringToJoinIndex = startIndex; stringToJoinIndex <= endIndex; stringToJoinIndex++)
            {
                item = value[stringToJoinIndex];

                if (item.String != null)
                {
                    jointLength += item.Length;
                }
            }

            jointLength += (count - 1) * separator.Length;

            CheckOverflow(jointLength + 1);

            if (jointLength == 0)
                return Empty;

#if ALLOW_UNSAFE && HAS_MEMCPY
            unsafe
            {
                var res = new string('\0', jointLength); // NB: This ctor simply performs a FastAllocateString if the character is '\0'.

                fixed (char* separatorStr = separator.String)
                {
                    fixed (char* destinationStr = res)
                    {
                        var buffer = new UnsafeCharBuffer(destinationStr, jointLength);

                        item = value[startIndex];
                        buffer.Append(item);

                        for (var stringToJoinIndex = startIndex + 1; stringToJoinIndex <= endIndex; stringToJoinIndex++)
                        {
                            buffer.Append(separator);

                            item = value[stringToJoinIndex];
                            buffer.Append(item);
                        }
                    }
                }

                return res;
            }
#else
            var sb = new StringBuilder(jointLength);

            item = value[startIndex];
            sb.Append(item.String, item.StartIndex, item.Length);

            for (var stringToJoinIndex = startIndex + 1; stringToJoinIndex <= endIndex; stringToJoinIndex++)
            {
                sb.Append(separator.String, separator.StartIndex, separator.Length);

                item = value[stringToJoinIndex];
                sb.Append(item.String, item.StartIndex, item.Length);
            }

            return sb.ToString();
#endif
        }

        /// <summary>
        /// Concatenates the elements of an object array, using the specified separator between each element.
        /// </summary>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        /// <param name="values">An array that contains the elements to concatenate.</param>
        /// <returns>A string that consists of the elements of values delimited by the separator string. If values is an empty array, the method returns Empty.</returns>
        public static StringSegment Join(StringSegment separator, params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

#if NETSTANDARD2_0
            if (values.Length == 0)
#else
            if (values.Length == 0 || values[0] == null) // COMPAT: Interesting quirk in behavior here due to the use of null in the first position.
#endif
                return Empty;

            if (separator.String == null)
                separator = Empty;

            using (var psb = PooledStringBuilder.New())
            {
                var sb = psb.StringBuilder;

#if NETSTANDARD2_0
                var s = values[0]?.ToString() ?? "";
#else
                // COMPAT: See remark on quirk above; no need to null check here.

                var s = values[0].ToString(); // CONSIDER: Check for a StringSegment and perform an Append for a range.
#endif

                if (s != null)
                {
                    sb.Append(s);
                }

                for (var i = 1; i < values.Length; i++)
                {
                    sb.Append(separator.String, separator.StartIndex, separator.Length);

                    s = values[i] == null ? default(string) : values[i].ToString(); // CONSIDER: Check for a StringSegment and perform an Append for a range.

                    if (s != null)
                    {
                        sb.Append(s);
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Concatenates the members of a collection, using the specified separator between each member.
        /// </summary>
        /// <typeparam name="T">The type of the members of values.</typeparam>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
        /// <param name="values">A collection that contains the objects to concatenate.</param>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns Empty.</returns>
        public static StringSegment Join<T>(StringSegment separator, IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (separator.String == null)
                separator = Empty;

            using (var enumerator = values.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return Empty;

                using (var psb = PooledStringBuilder.New())
                {
                    var sb = psb.StringBuilder;

                    var current = enumerator.Current;
                    var s = current == null ? default(string) : current.ToString();

                    if (s != null)
                    {
                        sb.Append(s);
                    }

                    while (enumerator.MoveNext())
                    {
                        sb.Append(separator.String, separator.StartIndex, separator.Length);

                        current = enumerator.Current;

                        s = current == null ? default(string) : current.ToString();

                        if (s != null)
                        {
                            sb.Append(s);
                        }
                    }

                    return sb.ToString();
                }
            }
        }

#if NET5_0
        /// <summary>
        /// Creates a new read-only span over the string segment.
        /// </summary>
        /// <returns>A read-only span over the string segment.</returns>
        public ReadOnlySpan<char> AsSpan() => String.AsSpan(StartIndex, Length);
#endif

        /// <summary>
        /// Helper method to check for strict equality of two string segments, i.e. they have the same fields.
        /// </summary>
        /// <param name="value">The string segment to compare the current string segment against.</param>
        /// <returns>true if the specified string segment is strictly equal to the current value; otherwise, false.</returns>
        private bool StrictEquals(StringSegment value) => ReferenceEquals(String, value.String) && StartIndex == value.StartIndex && Length == value.Length;  // CONSIDER: Use this helper in more cases to shortcircuit evaluation.

        /// <summary>
        /// Checks whether the specified value has overflown and throws an OutOfMemoryException of that's the case.
        /// </summary>
        /// <param name="value">The value to check.</param>
        [ExcludeFromCodeCoverage]
        private static void CheckOverflow(int value)
        {
            if (value < 0)
                throw new OutOfMemoryException();
        }
    }
}
