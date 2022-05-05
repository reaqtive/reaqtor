// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 03/30/2016 - Created StringSegment functionality.
//

namespace System.Text
{
    /// <summary>
    /// Provides a set of extension methods for StringBuilder to work with string segments.
    /// </summary>
    public static class StringBuilderStringSegmentExtensions
    {
        /// <summary>
        /// Appends a copy of the specified substring to this instance.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="value">The string that contains the substring to append.</param>
        /// <param name="startIndex">The starting position of the substring within value.</param>
        /// <param name="count">The number of characters in value to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder Append(this StringBuilder builder, StringSegment value, int startIndex, int count)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (value.String == null)
            {
                if (startIndex == 0 && count == 0)
                {
                    return builder;
                }

                throw new ArgumentNullException(nameof(value));
            }

            if (count == 0)
            {
                return builder;
            }

            if (startIndex > value.Length - count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            return builder.Append(value.String, value.StartIndex + startIndex, count);
        }

        /// <summary>
        /// Appends a copy of the specified string to this instance.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="value">The string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder Append(this StringBuilder builder, StringSegment value)
        {
            // NB: This overload is deceptive; the instance method taking in System.Object takes precedence when using "instance invocation" syntax.
            //     It doesn't hurt to be declared as an extension method, but one will have to call it through "static invocation" syntax instead.

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (StringSegment.IsNullOrEmpty(value))
                return builder;

            return builder.Append(value.String, value.StartIndex, value.Length);
        }

        /// <summary>
        /// Appends a copy of the specified string followed by the default line terminator to the end of the specified StringBuilder object.
        /// </summary>
        /// <param name="builder">The string builder to append to.</param>
        /// <param name="value"></param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder AppendLine(this StringBuilder builder, StringSegment value)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            Append(builder, value);
            return builder.Append(Environment.NewLine);
        }


        // NB: It'd be nice to support Insert as well, but there doesn't seem to be an efficient way to do it without allocating the substring
        //     of the segment. The only candidate overloads to Insert take in a char[] or string. Similar restrictions apply for Replace.

        // public static StringBuilder Insert(this StringBuilder builder, int index, StringSegment value);
        // public static StringBuilder Replace(this StringBuilder builder, StringSegment oldValue, StringSegment newValue);
    }
}
