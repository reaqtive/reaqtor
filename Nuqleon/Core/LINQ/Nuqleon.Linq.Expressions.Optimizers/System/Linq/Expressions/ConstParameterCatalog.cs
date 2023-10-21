// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2017 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1034 // Do not nest types.
#pragma warning disable CA1305 // Use IFormatProvider does not apply in expression trees.
#pragma warning disable CA1720 // Identifier 'X' contains type name.
#pragma warning disable CA1724 // Using System.* namespaces by design here.

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a catalog of const parameters in the Base Class Library.
    /// </summary>
    public static class ConstParameterCatalog
    {
        /// <summary>
        /// Const parameters in the System namespace.
        /// </summary>
        public static class System
        {
            /// <summary>
            /// Gets a table of const parameters on <see cref="global::System.String" />.
            /// </summary>
            public static ParameterTable String { get; } = new ParameterTable
            {
                (char[] value) => new string(value),
                (char[] value, int startIndex, int length) => new string(value, startIndex, length),

                (object[] args) => string.Concat(args),
                (string[] values) => string.Concat(values),

                (object[] args, string format) => string.Format(format, args),
                (object[] args, global::System.IFormatProvider provider, string format) => string.Format(provider, format, args),

                (object[] values, string separator) => string.Join(separator, values),
                (string[] values, string separator) => string.Join(separator, values),

                (char[] anyOf, string s) => s.IndexOfAny(anyOf),
                (char[] anyOf, string s, int startIndex) => s.IndexOfAny(anyOf, startIndex),
                (char[] anyOf, string s, int startIndex, int count) => s.IndexOfAny(anyOf, startIndex, count),

                (char[] anyOf, string s) => s.LastIndexOfAny(anyOf),
                (char[] anyOf, string s, int startIndex) => s.LastIndexOfAny(anyOf, startIndex),
                (char[] anyOf, string s, int startIndex, int count) => s.LastIndexOfAny(anyOf, startIndex, count),

                (char[] separator, string s) => s.Split(separator),
                (char[] separator, string s, global::System.StringSplitOptions options) => s.Split(separator, options),
                (char[] separator, string s, int count) => s.Split(separator, count),
                (char[] separator, string s, int count, global::System.StringSplitOptions options) => s.Split(separator, count, options),
                (string[] separator, string s, global::System.StringSplitOptions options) => s.Split(separator, options),
                (string[] separator, string s, int count, global::System.StringSplitOptions options) => s.Split(separator, count, options),

                (char[] trimChars, string s) => s.Trim(trimChars),
                (char[] trimChars, string s) => s.TrimStart(trimChars),
                (char[] trimChars, string s) => s.TrimEnd(trimChars),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of const parameters on <see cref="global::System.BitConverter" />.
            /// </summary>
            public static ParameterTable BitConverter { get; } = new ParameterTable
            {
                (byte[] value, int startIndex) => global::System.BitConverter.ToBoolean(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToChar(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToDouble(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToInt16(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToInt32(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToInt64(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToSingle(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToUInt16(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToUInt32(value, startIndex),
                (byte[] value, int startIndex) => global::System.BitConverter.ToUInt64(value, startIndex),
                (byte[] value) => global::System.BitConverter.ToString(value),
                (byte[] value, int startIndex) => global::System.BitConverter.ToString(value, startIndex),
                (byte[] value, int startIndex, int length) => global::System.BitConverter.ToString(value, startIndex, length),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of const parameters on <see cref="global::System.Guid" />.
            /// </summary>
            public static ParameterTable Guid { get; } = new ParameterTable
            {
                (byte[] b) => new Guid(b),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of const parameters on <see cref="global::System.Convert" />.
            /// </summary>
            public static ParameterTable Convert { get; } = new ParameterTable
            {
                (byte[] inArray) => global::System.Convert.ToBase64String(inArray),
                (byte[] inArray, int offset, int length) => global::System.Convert.ToBase64String(inArray, offset, length),
                (byte[] inArray, int offset, int length, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, offset, length, options),
                (byte[] inArray, global::System.Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, options),

#if NET8_0
                (byte[] inArray) => global::System.Convert.ToHexString(inArray),
                (byte[] inArray, int offset, int length) => global::System.Convert.ToHexString(inArray, offset, length),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of const parameters on <see cref="global::System.Array" />.
            /// </summary>
            public static ParameterTable Array { get; } = new ParameterTable
            {
                (global::System.Array array, object value) => global::System.Array.BinarySearch(array, value),
                (global::System.Array array, int index, int length, object value) => global::System.Array.BinarySearch(array, index, length, value),

                (global::System.Array array, object value) => global::System.Array.IndexOf(array, value),
                (global::System.Array array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex),
                (global::System.Array array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count),

                (global::System.Array array, object value) => global::System.Array.LastIndexOf(array, value),
                (global::System.Array array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex),
                (global::System.Array array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count),

#if NOTYET // NB: No support for generic types yet.
                (T[] array, T value) => global::System.Array.BinarySearch(array, value),
                (T[] array, int index, int length, T value) => global::System.Array.BinarySearch(array, index, length, value),

                (T[] array, object value) => global::System.Array.IndexOf(array, value),
                (T[] array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex),
                (T[] array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count),

                (T[] array, object value) => global::System.Array.LastIndexOf(array, value),
                (T[] array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex),
                (T[] array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of const parameters in the System namespace.
            /// </summary>
            public static ParameterTable AllThisNamespaceOnly { get; } = new ParameterTable
            {
                String,
                BitConverter,
                Guid,
                Convert,
                Array,
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of const parameters in the System namespace and any child namespaces.
            /// </summary>
            public static ParameterTable AllThisAndChildNamespaces { get; } = new ParameterTable
            {
                AllThisNamespaceOnly,
            }.ToReadOnly();
        }

        /// <summary>
        /// Gets a table of const parameters in the Base Class Library.
        /// </summary>
        public static ParameterTable All { get; } = new ParameterTable
        {
            System.AllThisAndChildNamespaces,
        }.ToReadOnly();
    }
}
