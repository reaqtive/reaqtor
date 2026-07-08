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

namespace System.Linq.Expressions;

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
        static System()
        {
            // This file used to use collection initializers for each property, but this turned
            // out to bring Roslyn analyzers to a grinding halt. It turns out that the combination
            // of lambda expressions and collection initializers is handled very badly by one
            // particular analyzer (IDE0001), and it used to take about 15 minutes to process
            // this file!
            // The problem is that Roslyn's semantic analyzer caching works at a per-statement
            // level, meaning that if a single expression ends up needing the same information
            // multiple times, it will be recomputed each time. This can make initializer
            // expressions much slower than equivalent code that expresses the Add calls
            // explicitly as a series of statements.
            // It seems that lambda expressions are particularly expensive, so it's the specific
            // combination of lambda expressions inside initializers that is known to be
            // particularly bad (and also a sufficiently unusual thing to do that the Roslyn team
            // does not want to completely overhaul their caching architecture to fix it).
#pragma warning disable IDE0028 // Simplify collection initialization
            ParameterTable stringParameters = new();
            stringParameters.Add(static (char[] value) => new string(value));
            stringParameters.Add(static (char[] value, int startIndex, int length) => new string(value, startIndex, length));

            stringParameters.Add(static (object[] args) => string.Concat(args));
            stringParameters.Add(static (string[] values) => string.Concat(values));

            stringParameters.Add(static (object[] args, string format) => string.Format(format, args));
            stringParameters.Add(static (object[] args, global::System.IFormatProvider provider, string format) => string.Format(provider, format, args));

            stringParameters.Add(static (object[] values, string separator) => string.Join(separator, values));
            stringParameters.Add(static (string[] values, string separator) => string.Join(separator, values));

            stringParameters.Add(static (char[] anyOf, string s) => s.IndexOfAny(anyOf));
            stringParameters.Add(static (char[] anyOf, string s, int startIndex) => s.IndexOfAny(anyOf, startIndex));
            stringParameters.Add(static (char[] anyOf, string s, int startIndex, int count) => s.IndexOfAny(anyOf, startIndex, count));

            stringParameters.Add(static (char[] anyOf, string s) => s.LastIndexOfAny(anyOf));
            stringParameters.Add(static (char[] anyOf, string s, int startIndex) => s.LastIndexOfAny(anyOf, startIndex));
            stringParameters.Add(static (char[] anyOf, string s, int startIndex, int count) => s.LastIndexOfAny(anyOf, startIndex, count));

            stringParameters.Add(static (char[] separator, string s) => s.Split(separator));
            stringParameters.Add(static (char[] separator, string s, global::System.StringSplitOptions options) => s.Split(separator, options));
            stringParameters.Add(static (char[] separator, string s, int count) => s.Split(separator, count));
            stringParameters.Add(static (char[] separator, string s, int count, global::System.StringSplitOptions options) => s.Split(separator, count, options));
            stringParameters.Add(static (string[] separator, string s, global::System.StringSplitOptions options) => s.Split(separator, options));
            stringParameters.Add(static (string[] separator, string s, int count, global::System.StringSplitOptions options) => s.Split(separator, count, options));

            stringParameters.Add(static (char[] trimChars, string s) => s.Trim(trimChars));
            stringParameters.Add(static (char[] trimChars, string s) => s.TrimStart(trimChars));
            stringParameters.Add(static (char[] trimChars, string s) => s.TrimEnd(trimChars));
            String = stringParameters.ToReadOnly();

            ParameterTable bitConverterParameters = new();
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToBoolean(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToChar(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToDouble(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToInt16(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToInt32(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToInt64(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToSingle(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToUInt16(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToUInt32(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToUInt64(value, startIndex));
            bitConverterParameters.Add(static (byte[] value) => global::System.BitConverter.ToString(value));
            bitConverterParameters.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToString(value, startIndex));
            bitConverterParameters.Add(static (byte[] value, int startIndex, int length) => global::System.BitConverter.ToString(value, startIndex, length));
            BitConverter = bitConverterParameters.ToReadOnly();

            ParameterTable convertParameters = new();
            convertParameters.Add(static (byte[] inArray) => global::System.Convert.ToBase64String(inArray));
            convertParameters.Add(static (byte[] inArray, int offset, int length) => global::System.Convert.ToBase64String(inArray, offset, length));
            convertParameters.Add(static (byte[] inArray, int offset, int length, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, offset, length, options));
            convertParameters.Add(static (byte[] inArray, global::System.Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, options));

            convertParameters.Add(static (byte[] inArray) => global::System.Convert.ToHexString(inArray));
            convertParameters.Add(static (byte[] inArray, int offset, int length) => global::System.Convert.ToHexString(inArray, offset, length));
            Convert = convertParameters.ToReadOnly();


            ParameterTable arrayParameters = new();
            arrayParameters.Add(static (global::System.Array array, object value) => global::System.Array.BinarySearch(array, value));
            arrayParameters.Add(static (global::System.Array array, int index, int length, object value) => global::System.Array.BinarySearch(array, index, length, value));

            arrayParameters.Add(static (global::System.Array array, object value) => global::System.Array.IndexOf(array, value));
            arrayParameters.Add(static (global::System.Array array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex));
            arrayParameters.Add(static (global::System.Array array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count));

            arrayParameters.Add(static (global::System.Array array, object value) => global::System.Array.LastIndexOf(array, value));
            arrayParameters.Add(static (global::System.Array array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex));
            arrayParameters.Add(static (global::System.Array array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count));

#if NOTYET // NB: No support for generic types yet.
            arrayParameters.Add((T[] array, T value) => global::System.Array.BinarySearch(array, value));
            arrayParameters.Add((T[] array, int index, int length, T value) => global::System.Array.BinarySearch(array, index, length, value));

            arrayParameters.Add((T[] array, object value) => global::System.Array.IndexOf(array, value));
            arrayParameters.Add((T[] array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex));
            arrayParameters.Add((T[] array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count));

            arrayParameters.Add((T[] array, object value) => global::System.Array.LastIndexOf(array, value));
            arrayParameters.Add((T[] array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex));
            arrayParameters.Add((T[] array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count));
#endif
            Array = arrayParameters.ToReadOnly();

#pragma warning restore IDE0028 // Simplify collection initialization

            AllThisNamespaceOnly = new ParameterTable
            {
                String,
                BitConverter,
                Guid,
                Convert,
                Array,
            }.ToReadOnly();

            AllThisAndChildNamespaces = new ParameterTable
            {
                AllThisNamespaceOnly,
            }.ToReadOnly();
        }

        /// <summary>
        /// Gets a table of const parameters on <see cref="string" />.
        /// </summary>
        public static ParameterTable String { get; }

        /// <summary>
        /// Gets a table of const parameters on <see cref="global::System.BitConverter" />.
        /// </summary>
        public static ParameterTable BitConverter { get; }

        /// <summary>
        /// Gets a table of const parameters on <see cref="global::System.Guid" />.
        /// </summary>
        public static ParameterTable Guid { get; } = new ParameterTable
        {
            static (byte[] b) => new Guid(b),
        }.ToReadOnly();

        /// <summary>
        /// Gets a table of const parameters on <see cref="global::System.Convert" />.
        /// </summary>
        public static ParameterTable Convert { get; }

        /// <summary>
        /// Gets a table of const parameters on <see cref="global::System.Array" />.
        /// </summary>
        public static ParameterTable Array { get; }

        /// <summary>
        /// Gets a table of const parameters in the System namespace.
        /// </summary>
        public static ParameterTable AllThisNamespaceOnly { get; }

        /// <summary>
        /// Gets a table of const parameters in the System namespace and any child namespaces.
        /// </summary>
        public static ParameterTable AllThisAndChildNamespaces { get; }
    }

    /// <summary>
    /// Gets a table of const parameters in the Base Class Library.
    /// </summary>
    public static ParameterTable All { get; } = new ParameterTable
    {
        System.AllThisAndChildNamespaces,
    }.ToReadOnly();
}
