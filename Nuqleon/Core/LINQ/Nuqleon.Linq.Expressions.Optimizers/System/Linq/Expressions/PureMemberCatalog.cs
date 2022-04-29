// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Linq.CompilerServices.TypeSystem;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1034 // Do not nest types.
#pragma warning disable CA1720 // Identifier X contains type name.
#pragma warning disable CA1724 // Using System.* namespaces by design here.

// Misc. warnings related to usage of methods. These should not apply in expression trees.
#pragma warning disable CA1305
#pragma warning disable CA1307
#pragma warning disable CA1308
#pragma warning disable CA1309

namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides a catalog of pure members in the Base Class Library.
    /// </summary>
    public static class PureMemberCatalog
    {
        //
        // REVIEW:
        //
        // - ToString on primitives is culture-dependent. How do we enable users to specify that
        //   these can be reduced (e.g. if it's known that a system always runs in the same culture
        //   or the user doesn't care)? Should we use specialized tables for these that can be
        //   added in an opt-in fashion?
        //
        // - Parse, similar remarks as above.
        //
        // - Entries in these tables can only return immutable objects. A complimentary strategy
        //   could be added where a mutable return type supports cloning, so each use site causes
        //   the creation of a clone of an evaluated constant. E.g. string.Split returns an array
        //   but rather than reevaluating this each time, we could evaluate it once and use cloning
        //   of an array at each use site. Note this requires hoisting the evaluated constant value
        //   to some scope where it can be shared across many use sites. Furthermore, if we can
        //   figure out if mutations are applied for a given use site, we can avoid making a clone.
        //   An example is s.Split(c)[0].Length. Right now, the impurity of Split prevents further
        //   evaluation of the pure indexing and Length operations.
        //
        // - The following are omitted, for reasons that can be revisited:
        //   - ArraySegment<T> can be used to alias an array, which is mutable
        //   - Enum has various members, but many are rarely used
        //   - FormattableString is abstract; string.Format code is more common
        //   - IntPtr and UIntPtr are very rarely used
        //   - System.Numerics types introduce an assembly dependency
        //   - TimeZone[Info] needs thorough review (native API calls, localization)
        //   - Type brings in a slew of related reflection types

        /// <summary>
        /// Pure members in the System namespace.
        /// </summary>
        public static class System
        {
            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Boolean" />.
            /// </summary>
            public static MemberTable Boolean { get; } = new MemberTable
            {
                () => bool.FalseString,
                () => bool.TrueString,

                (bool i, bool j) => i.Equals(j),
                (bool i, bool j) => i.CompareTo(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Char" />.
            /// </summary>
            public static MemberTable Char { get; } = new MemberTable
            {
                (string s) => char.Parse(s),
                (char c) => char.ToString(c),
                (char c) => char.IsDigit(c),
                (char c) => char.IsLetter(c),
                (char c) => char.IsWhiteSpace(c),
                (char c) => char.IsUpper(c),
                (char c) => char.IsLower(c),
                (char c) => char.IsPunctuation(c),
                (char c) => char.IsLetterOrDigit(c),
                (char c) => char.ToUpperInvariant(c),
                (char c) => char.ToLowerInvariant(c),
                (char c) => char.IsControl(c),
                (char c) => char.IsNumber(c),
                (char c) => char.IsSeparator(c),
                (char c) => char.IsSurrogate(c),
                (char c) => char.IsSymbol(c),
                (char c) => char.GetUnicodeCategory(c),
                (char c) => char.GetNumericValue(c),
                (char c) => char.IsHighSurrogate(c),
                (char c) => char.IsLowSurrogate(c),
                (int utf32) => char.ConvertFromUtf32(utf32),
                (string s, int index) => char.IsControl(s, index),
                (string s, int index) => char.IsDigit(s, index),
                (string s, int index) => char.IsLetter(s, index),
                (string s, int index) => char.IsLetterOrDigit(s, index),
                (string s, int index) => char.IsLower(s, index),
                (string s, int index) => char.IsNumber(s, index),
                (string s, int index) => char.IsPunctuation(s, index),
                (string s, int index) => char.IsSeparator(s, index),
                (string s, int index) => char.IsSurrogate(s, index),
                (string s, int index) => char.IsSymbol(s, index),
                (string s, int index) => char.IsUpper(s, index),
                (string s, int index) => char.IsWhiteSpace(s, index),
                (string s, int index) => char.GetUnicodeCategory(s, index),
                (string s, int index) => char.GetNumericValue(s, index),
                (string s, int index) => char.IsHighSurrogate(s, index),
                (string s, int index) => char.IsLowSurrogate(s, index),
                (string s, int index) => char.IsSurrogatePair(s, index),
                (string s, int index) => char.ConvertToUtf32(s, index),
                (char highSurrogate, char lowSurrogate) => char.IsSurrogatePair(highSurrogate, lowSurrogate),
                (char highSurrogate, char lowSurrogate) => char.ConvertToUtf32(highSurrogate, lowSurrogate),

                (char i, char j) => i.CompareTo(j),
                (char i, char j) => i.Equals(j),
                (char c) => c.ToString(),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.SByte" />.
            /// </summary>
            public static MemberTable SByte { get; } = new MemberTable
            {
                (sbyte i, sbyte j) => i.CompareTo(j),
                (sbyte i, sbyte j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Byte" />.
            /// </summary>
            public static MemberTable Byte { get; } = new MemberTable
            {
                (byte i, byte j) => i.CompareTo(j),
                (byte i, byte j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Int16" />.
            /// </summary>
            public static MemberTable Int16 { get; } = new MemberTable
            {
                (short i, short j) => i.CompareTo(j),
                (short i, short j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.UInt16" />.
            /// </summary>
            public static MemberTable UInt16 { get; } = new MemberTable
            {
                (ushort i, ushort j) => i.CompareTo(j),
                (ushort i, ushort j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Int32" />.
            /// </summary>
            public static MemberTable Int32 { get; } = new MemberTable
            {
                (int i, int j) => i.CompareTo(j),
                (int i, int j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.UInt32" />.
            /// </summary>
            public static MemberTable UInt32 { get; } = new MemberTable
            {
                (uint i, uint j) => i.CompareTo(j),
                (uint i, uint j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Int64" />.
            /// </summary>
            public static MemberTable Int64 { get; } = new MemberTable
            {
                (long i, long j) => i.CompareTo(j),
                (long i, long j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.UInt64" />.
            /// </summary>
            public static MemberTable UInt64 { get; } = new MemberTable
            {
                (ulong i, ulong j) => i.CompareTo(j),
                (ulong i, ulong j) => i.Equals(j),
            }.ToReadOnly();

#if NET6_0
            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Half" />.
            /// </summary>
            public static MemberTable Half { get; } = new MemberTable
            {
                () => global::System.Half.Epsilon,
                () => global::System.Half.MaxValue,
                () => global::System.Half.MinValue,
                () => global::System.Half.NaN,
                () => global::System.Half.NegativeInfinity,
                () => global::System.Half.PositiveInfinity,

                (global::System.Half i) => global::System.Half.IsFinite(i),
                (global::System.Half i) => global::System.Half.IsInfinity(i),
                (global::System.Half i) => global::System.Half.IsNaN(i),
                (global::System.Half i) => global::System.Half.IsNegative(i),
                (global::System.Half i) => global::System.Half.IsNegativeInfinity(i),
                (global::System.Half i) => global::System.Half.IsNormal(i),
                (global::System.Half i) => global::System.Half.IsPositiveInfinity(i),
                (global::System.Half i) => global::System.Half.IsSubnormal(i),

                (global::System.Half i, global::System.Half j) => i.CompareTo(j),
                (global::System.Half i, global::System.Half j) => i.Equals(j),

                (global::System.Half i) => (float)i,
                (global::System.Half i) => (double)i,
                (float i) => (global::System.Half)i,
                (double i) => (global::System.Half)i,

                (global::System.Half i, global::System.Half j) => i == j,
                (global::System.Half i, global::System.Half j) => i != j,
                (global::System.Half i, global::System.Half j) => i < j,
                (global::System.Half i, global::System.Half j) => i <= j,
                (global::System.Half i, global::System.Half j) => i > j,
                (global::System.Half i, global::System.Half j) => i >= j,
            }.ToReadOnly();
#endif

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Single" />.
            /// </summary>
            public static MemberTable Single { get; } = new MemberTable
            {
                (float i) => float.IsInfinity(i),
                (float i) => float.IsNaN(i),
                (float i) => float.IsNegativeInfinity(i),
                (float i) => float.IsPositiveInfinity(i),

                (float i, float j) => i.CompareTo(j),
                (float i, float j) => i.Equals(j),

#if NET6_0 || NETSTANDARD2_1
                (float i) => float.IsFinite(i),
                (float i) => float.IsNegative(i),
                (float i) => float.IsNormal(i),
                (float i) => float.IsSubnormal(i),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Double" />.
            /// </summary>
            public static MemberTable Double { get; } = new MemberTable
            {
                (double i) => double.IsInfinity(i),
                (double i) => double.IsNaN(i),
                (double i) => double.IsNegativeInfinity(i),
                (double i) => double.IsPositiveInfinity(i),

                (double i, double j) => i.CompareTo(j),
                (double i, double j) => i.Equals(j),

#if NET6_0 || NETSTANDARD2_1
                (double i) => double.IsFinite(i),
                (double i) => double.IsNegative(i),
                (double i) => double.IsNormal(i),
                (double i) => double.IsSubnormal(i),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Decimal" />.
            /// </summary>
            public static MemberTable Decimal { get; } = new MemberTable
            {
                (decimal i) => decimal.Ceiling(i),
                (decimal i) => decimal.Floor(i),
                (decimal i) => decimal.Negate(i),
                (decimal i) => decimal.Round(i),
                (decimal i) => decimal.ToByte(i),
                (decimal i) => decimal.ToDouble(i),
                (decimal i) => decimal.ToInt16(i),
                (decimal i) => decimal.ToInt32(i),
                (decimal i) => decimal.ToInt64(i),
                (decimal i) => decimal.ToSByte(i),
                (decimal i) => decimal.ToSingle(i),
                (decimal i) => decimal.ToUInt16(i),
                (decimal i) => decimal.ToUInt32(i),
                (decimal i) => decimal.ToUInt64(i),
                (decimal i) => decimal.ToOACurrency(i),
                (decimal i, decimal j) => decimal.Add(i, j),
                (decimal i, decimal j) => decimal.Compare(i, j),
                (decimal i, decimal j) => decimal.Divide(i, j),
                (decimal i, decimal j) => decimal.Equals(i, j),
                (decimal i, decimal j) => decimal.Multiply(i, j),
                (decimal i, decimal j) => decimal.Remainder(i, j),
                (decimal i, decimal j) => decimal.Subtract(i, j),

                (sbyte i) => (decimal)i,
                (byte i) => (decimal)i,
                (short i) => (decimal)i,
                (ushort i) => (decimal)i,
                (int i) => (decimal)i,
                (uint i) => (decimal)i,
                (long i) => (decimal)i,
                (ulong i) => (decimal)i,
                (char i) => (decimal)i,
                (float i) => (decimal)i,
                (double i) => (decimal)i,

                (decimal i) => -i,
                (decimal i) => (sbyte)i,
                (decimal i) => (byte)i,
                (decimal i) => (short)i,
                (decimal i) => (ushort)i,
                (decimal i) => (int)i,
                (decimal i) => (uint)i,
                (decimal i) => (long)i,
                (decimal i) => (ulong)i,
                (decimal i) => (char)i,
                (decimal i) => (float)i,
                (decimal i) => (double)i,

                (decimal i, decimal j) => i + j,
                (decimal i, decimal j) => i - j,
                (decimal i, decimal j) => i * j,
                (decimal i, decimal j) => i / j,
                (decimal i, decimal j) => i % j,
                (decimal i, decimal j) => i == j,
                (decimal i, decimal j) => i != j,
                (decimal i, decimal j) => i < j,
                (decimal i, decimal j) => i <= j,
                (decimal i, decimal j) => i > j,
                (decimal i, decimal j) => i >= j,

                (decimal i, decimal j) => i.CompareTo(j),
                (decimal i, decimal j) => i.Equals(j),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.String" />.
            /// </summary>
            public static MemberTable String { get; } = new MemberTable
            {
                (char[] value) => new string(value),
                (char[] value, int startIndex, int length) => new string(value, startIndex, length),
                (char c, int count) => new string(c, count),

                () => string.Empty,
                (string s) => s.Length,
                (string s, int index) => s[index],

                (string value) => string.IsNullOrEmpty(value),
                (string value) => string.IsNullOrWhiteSpace(value),

                (string strA, string strB) => string.CompareOrdinal(strA, strB),
                (string strA, int indexA, string strB, int indexB, int length) => string.CompareOrdinal(strA, indexA, strB, indexB, length),

                (string str0, string str1) => string.Concat(str0, str1),
                (string str0, string str1, string str2) => string.Concat(str0, str1, str2),
                (string str0, string str1, string str2, string str3) => string.Concat(str0, str1, str2, str3),
                (string[] values) => string.Concat(values),

                (string s, string value) => s.Contains(value), // NB: This uses ordinal comparison

                (string s, string value) => s.Equals(value),
                (string a, string b) => string.Equals(a, b),

                (string s, char value) => s.IndexOf(value),
                (string s, char value, int startIndex) => s.IndexOf(value, startIndex),
                (string s, char value, int startIndex, int count) => s.IndexOf(value, startIndex, count),

                (string s, char[] anyOf) => s.IndexOfAny(anyOf),
                (string s, char[] anyOf, int startIndex) => s.IndexOfAny(anyOf, startIndex),
                (string s, char[] anyOf, int startIndex, int count) => s.IndexOfAny(anyOf, startIndex, count),

                (string s, char value) => s.LastIndexOf(value),
                (string s, char value, int startIndex) => s.LastIndexOf(value, startIndex),
                (string s, char value, int startIndex, int count) => s.LastIndexOf(value, startIndex, count),

                (string s, char[] anyOf) => s.LastIndexOfAny(anyOf),
                (string s, char[] anyOf, int startIndex) => s.LastIndexOfAny(anyOf, startIndex),
                (string s, char[] anyOf, int startIndex, int count) => s.LastIndexOfAny(anyOf, startIndex, count),

                (string s, int startIndex, string value) => s.Insert(startIndex, value),

                (string s) => s.IsNormalized(),
                (string s) => s.Normalize(),
                (string s, global::System.Text.NormalizationForm normalizationForm) => s.Normalize(normalizationForm),
                (string s, global::System.Text.NormalizationForm normalizationForm) => s.IsNormalized(normalizationForm),

                (string separator, string[] value) => string.Join(separator, value),
                (string separator, string[] value, int startIndex, int count) => string.Join(separator, value, startIndex, count),

                (string s, int totalWidth) => s.PadLeft(totalWidth),
                (string s, int totalWidth) => s.PadRight(totalWidth),
                (string s, int totalWidth, char paddingChar) => s.PadLeft(totalWidth, paddingChar),
                (string s, int totalWidth, char paddingChar) => s.PadRight(totalWidth, paddingChar),

                (string s, int startIndex) => s.Remove(startIndex),
                (string s, int startIndex, int count) => s.Remove(startIndex, count),

                (string s, char oldChar, char newChar) => s.Replace(oldChar, newChar),
                (string s, string oldValue, string newValue) => s.Replace(oldValue, newValue),

                // NB: Omitting Split which returns a mutable array.

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable IDE0057 // Substring can be simplified (not in expression trees)
                (string s, int startIndex) => s.Substring(startIndex),
                (string s, int startIndex, int length) => s.Substring(startIndex, length),
#pragma warning restore IDE0057
#pragma warning restore IDE0079

                // NB: Omitting ToCharArray which returns a mutable array.

                (string s) => s.ToLowerInvariant(),
                (string s) => s.ToUpperInvariant(),

                // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with System.Object here.
                //(string s) => s.ToString(),

                // REVIEW: Should we add format providers sample values to tests just for this?
                //(string s, IFormatProvider provider) => s.ToString(provider /* NB: this parameter is always ignored */),

                (string s) => s.Trim(),
                (string s, char[] trimChars) => s.Trim(trimChars),
                (string s, char[] trimChars) => s.TrimEnd(trimChars),
                (string s, char[] trimChars) => s.TrimStart(trimChars),

                (string s1, string s2) => s1 == s2,
                (string s1, string s2) => s1 != s2,

#if NET6_0 || NETSTANDARD2_1
                (string s, char c) => s.Contains(c),
                (string s, char c) => s.StartsWith(c),
                (string s, char c) => s.EndsWith(c),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.DateTime" />.
            /// </summary>
            public static MemberTable DateTime { get; } = new MemberTable
            {
                (long ticks) => new global::System.DateTime(ticks),
                (long ticks, DateTimeKind kind) => new global::System.DateTime(ticks, kind),
                (int year, int month, int day) => new global::System.DateTime(year, month, day),
                (int year, int month, int day, int hour, int minute, int second) => new global::System.DateTime(year, month, day, hour, minute, second),
                (int year, int month, int day, int hour, int minute, int second, DateTimeKind kind) => new global::System.DateTime(year, month, day, hour, minute, second, kind),
                (int year, int month, int day, int hour, int minute, int second, int millisecond) => new global::System.DateTime(year, month, day, hour, minute, second, millisecond),
                (int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind) => new global::System.DateTime(year, month, day, hour, minute, second, millisecond, kind),

                () => global::System.DateTime.MaxValue,
                () => global::System.DateTime.MinValue,

                (global::System.DateTime dt) => dt.Date,
                (global::System.DateTime dt) => dt.Day,
                (global::System.DateTime dt) => dt.DayOfWeek,
                (global::System.DateTime dt) => dt.DayOfYear,
                (global::System.DateTime dt) => dt.Hour,
                (global::System.DateTime dt) => dt.Kind,
                (global::System.DateTime dt) => dt.Millisecond,
                (global::System.DateTime dt) => dt.Minute,
                (global::System.DateTime dt) => dt.Month,
                (global::System.DateTime dt) => dt.Second,
                (global::System.DateTime dt) => dt.Ticks,
                (global::System.DateTime dt) => dt.TimeOfDay,
                (global::System.DateTime dt) => dt.Year,

                (global::System.DateTime t1, global::System.DateTime t2) => global::System.DateTime.Compare(t1, t2),
                (global::System.DateTime t1, global::System.DateTime t2) => global::System.DateTime.Equals(t1, t2),

                (double d) => global::System.DateTime.FromOADate(d),
                (long fileTime) => global::System.DateTime.FromFileTimeUtc(fileTime),
                (int year) => global::System.DateTime.IsLeapYear(year),
                (int year, int month) => global::System.DateTime.DaysInMonth(year, month),
                (global::System.DateTime value, global::System.DateTimeKind kind) => global::System.DateTime.SpecifyKind(value, kind),

                (global::System.DateTime dt) => dt.ToOADate(),

                (global::System.DateTime dt, global::System.TimeSpan value) => dt.Add(value),
                (global::System.DateTime dt, double value) => dt.AddDays(value),
                (global::System.DateTime dt, double value) => dt.AddHours(value),
                (global::System.DateTime dt, double value) => dt.AddMilliseconds(value),
                (global::System.DateTime dt, double value) => dt.AddMinutes(value),
                (global::System.DateTime dt, int months) => dt.AddMonths(months),
                (global::System.DateTime dt, double value) => dt.AddSeconds(value),
                (global::System.DateTime dt, long value) => dt.AddTicks(value),
                (global::System.DateTime dt, int value) => dt.AddYears(value),
                (global::System.DateTime dt, global::System.DateTime value) => dt.Subtract(value),
                (global::System.DateTime dt, global::System.TimeSpan value) => dt.Subtract(value),

                (global::System.DateTime dt, global::System.DateTime value) => dt.CompareTo(value),
                (global::System.DateTime dt, global::System.DateTime value) => dt.Equals(value),

                (global::System.DateTime dt, global::System.TimeSpan ts) => dt + ts,
                (global::System.DateTime dt, global::System.TimeSpan ts) => dt - ts,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 - dt2,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 == dt2,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 != dt2,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 < dt2,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 <= dt2,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 > dt2,
                (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 >= dt2,
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="System.DateTimeOffset" />.
            /// </summary>
            public static MemberTable DateTimeOffset { get; } = new MemberTable
            {
                (long ticks, global::System.TimeSpan offset) => new global::System.DateTimeOffset(ticks, offset),
                (int year, int month, int day, int hour, int minute, int second, global::System.TimeSpan offset) => new global::System.DateTimeOffset(year, month, day, hour, minute, second, offset),
                (int year, int month, int day, int hour, int minute, int second, int millisecond, global::System.TimeSpan offset) => new global::System.DateTimeOffset(year, month, day, hour, minute, second, millisecond, offset),

                () => global::System.DateTimeOffset.MaxValue,
                () => global::System.DateTimeOffset.MinValue,

                (global::System.DateTimeOffset dto) => dto.Date,
                (global::System.DateTimeOffset dto) => dto.DateTime,
                (global::System.DateTimeOffset dto) => dto.Day,
                (global::System.DateTimeOffset dto) => dto.DayOfWeek,
                (global::System.DateTimeOffset dto) => dto.DayOfYear,
                (global::System.DateTimeOffset dto) => dto.Hour,
                (global::System.DateTimeOffset dto) => dto.LocalDateTime,
                (global::System.DateTimeOffset dto) => dto.Millisecond,
                (global::System.DateTimeOffset dto) => dto.Minute,
                (global::System.DateTimeOffset dto) => dto.Month,
                (global::System.DateTimeOffset dto) => dto.Offset,
                (global::System.DateTimeOffset dto) => dto.Second,
                (global::System.DateTimeOffset dto) => dto.Ticks,
                (global::System.DateTimeOffset dto) => dto.TimeOfDay,
                (global::System.DateTimeOffset dto) => dto.UtcDateTime,
                (global::System.DateTimeOffset dto) => dto.UtcTicks,
                (global::System.DateTimeOffset dto) => dto.Year,

                (global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto.Add(ts),
                (global::System.DateTimeOffset dto, double days) => dto.AddDays(days),
                (global::System.DateTimeOffset dto, double hours) => dto.AddHours(hours),
                (global::System.DateTimeOffset dto, double milliseconds) => dto.AddMilliseconds(milliseconds),
                (global::System.DateTimeOffset dto, double minutes) => dto.AddMinutes(minutes),
                (global::System.DateTimeOffset dto, int months) => dto.AddMonths(months),
                (global::System.DateTimeOffset dto, double seconds) => dto.AddSeconds(seconds),
                (global::System.DateTimeOffset dto, long ticks) => dto.AddTicks(ticks),
                (global::System.DateTimeOffset dto, int years) => dto.AddYears(years),
                (global::System.DateTimeOffset dto, global::System.DateTimeOffset value) => dto.Subtract(value),
                (global::System.DateTimeOffset dto, global::System.TimeSpan value) => dto.Subtract(value),

                (long fileTime) => global::System.DateTimeOffset.FromFileTime(fileTime),

                (global::System.DateTimeOffset first, global::System.DateTimeOffset second) => global::System.DateTimeOffset.Compare(first, second),
                (global::System.DateTimeOffset first, global::System.DateTimeOffset second) => global::System.DateTimeOffset.Equals(first, second),

                (global::System.DateTimeOffset dto) => dto.ToFileTime(),
                (global::System.DateTimeOffset dto) => dto.ToUniversalTime(),
                (global::System.DateTimeOffset dto, global::System.TimeSpan offset) => dto.ToOffset(offset),

                (global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.CompareTo(other),
                (global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.Equals(other),
                (global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.EqualsExact(other),

                (global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto + ts,
                (global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto - ts,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 - dto2,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 == dto2,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 != dto2,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 < dto2,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 <= dto2,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 > dto2,
                (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 >= dto2,

                // REVIEW: these are not accessible
                //(long seconds) => global::System.DateTimeOffset.FromUnixTimeSeconds(seconds),
                //(long milliseconds) => global::System.DateTimeOffset.FromUnixTimeMilliseconds(milliseconds),
                //(global::System.DateTimeOffset dto) => dto.ToUnixTimeSeconds(),
                //(global::System.DateTimeOffset dto) => dto.ToUnixTimeMilliseconds(),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.TimeSpan" />.
            /// </summary>
            public static MemberTable TimeSpan { get; } = new MemberTable
            {
                (long ticks) => new global::System.TimeSpan(ticks),
                (int hours, int minutes, int seconds) => new global::System.TimeSpan(hours, minutes, seconds),
                (int days, int hours, int minutes, int seconds) => new global::System.TimeSpan(days, hours, minutes, seconds),
                (int days, int hours, int minutes, int seconds, int milliseconds) => new global::System.TimeSpan(days, hours, minutes, seconds, milliseconds),

                () => global::System.TimeSpan.MaxValue,
                () => global::System.TimeSpan.MinValue,
                () => global::System.TimeSpan.TicksPerDay,
                () => global::System.TimeSpan.TicksPerHour,
                () => global::System.TimeSpan.TicksPerMillisecond,
                () => global::System.TimeSpan.TicksPerMinute,
                () => global::System.TimeSpan.TicksPerSecond,
                () => global::System.TimeSpan.Zero,

                (global::System.TimeSpan t) => t.Days,
                (global::System.TimeSpan t) => t.Hours,
                (global::System.TimeSpan t) => t.Milliseconds,
                (global::System.TimeSpan t) => t.Minutes,
                (global::System.TimeSpan t) => t.Seconds,
                (global::System.TimeSpan t) => t.Ticks,
                (global::System.TimeSpan t) => t.TotalDays,
                (global::System.TimeSpan t) => t.TotalHours,
                (global::System.TimeSpan t) => t.TotalMilliseconds,
                (global::System.TimeSpan t) => t.TotalMinutes,
                (global::System.TimeSpan t) => t.TotalSeconds,

                (long value) => global::System.TimeSpan.FromTicks(value),
                (double value) => global::System.TimeSpan.FromDays(value),
                (double value) => global::System.TimeSpan.FromHours(value),
                (double value) => global::System.TimeSpan.FromMilliseconds(value),
                (double value) => global::System.TimeSpan.FromMinutes(value),
                (double value) => global::System.TimeSpan.FromSeconds(value),

                (global::System.TimeSpan t1, global::System.TimeSpan t2) => global::System.TimeSpan.Compare(t1, t2),
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => global::System.TimeSpan.Equals(t1, t2),

                (global::System.TimeSpan t) => t.Duration(),
                (global::System.TimeSpan t) => t.Negate(),
                (global::System.TimeSpan t, global::System.TimeSpan ts) => t.Add(ts),
                (global::System.TimeSpan t, global::System.TimeSpan ts) => t.Subtract(ts),

                (global::System.TimeSpan t, global::System.TimeSpan value) => t.CompareTo(value),
                (global::System.TimeSpan t, global::System.TimeSpan obj) => t.Equals(obj),

                (global::System.TimeSpan t) => -t,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 + t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 - t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 == t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 != t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 < t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 <= t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 > t2,
                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 >= t2,

#if NET6_0 || NETSTANDARD2_1
                (global::System.TimeSpan t, double divisor) => t.Divide(divisor),
                (global::System.TimeSpan t, global::System.TimeSpan ts) => t.Divide(ts),
                (global::System.TimeSpan t, double factor) => t.Multiply(factor),

                (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 / t2,
                (global::System.TimeSpan timeSpan, double divisor) => timeSpan / divisor,
                (double factor, global::System.TimeSpan timeSpan) => factor * timeSpan,
                (global::System.TimeSpan timeSpan, double factor) => timeSpan * factor,
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Guid" />.
            /// </summary>
            public static MemberTable Guid { get; } = new MemberTable
            {
                (string input) => global::System.Guid.Parse(input),
                (string input, string format) => global::System.Guid.ParseExact(input, format),

                (byte[] b) => new global::System.Guid(b),
                (string g) => new global::System.Guid(g),
                (uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) => new global::System.Guid(a, b, c, d, e, f, g, h, i, j, k),
                (int a, short b, short c, byte[] d) => new global::System.Guid(a, b, c, d),
                (int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) => new global::System.Guid(a, b, c, d, e, f, g, h, i, j, k),

                () => global::System.Guid.Empty,

                (global::System.Guid g, global::System.Guid value) => g.CompareTo(value),
                (global::System.Guid g, global::System.Guid value) => g.Equals(value),

                // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
                //(global::System.Guid g) => g.ToString(),
                //(global::System.Guid g, string format) => g.ToString(format),

                // REVIEW: Should we add format providers sample values to tests just for this?
                //(global::System.Guid g, string format, IFormatProvider provider) => s.ToString(format, provider /* NB: this parameter is always ignored */),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Uri" />.
            /// </summary>
            public static MemberTable Uri { get; } = new MemberTable
            {
                (string uriString) => new Uri(uriString),
                (string uriString, UriKind uriKind) => new Uri(uriString, uriKind),
                (Uri baseUri, string relativeUri) => new Uri(baseUri, relativeUri),
                (Uri baseUri, Uri relativeUri) => new Uri(baseUri, relativeUri),

                (string uriString) => new global::System.Uri(uriString),
                (string uriString, global::System.UriKind uriKind) => new global::System.Uri(uriString, uriKind),
                (global::System.Uri baseUri, string relativeUri) => new global::System.Uri(baseUri, relativeUri),
                (global::System.Uri baseUri, global::System.Uri relativeUri) => new global::System.Uri(baseUri, relativeUri),

                () => global::System.Uri.SchemeDelimiter,
                () => global::System.Uri.UriSchemeFile,
                () => global::System.Uri.UriSchemeFtp,
                () => global::System.Uri.UriSchemeGopher,
                () => global::System.Uri.UriSchemeHttp,
                () => global::System.Uri.UriSchemeHttps,
                () => global::System.Uri.UriSchemeMailto,
                () => global::System.Uri.UriSchemeNetPipe,
                () => global::System.Uri.UriSchemeNetTcp,
                () => global::System.Uri.UriSchemeNews,
                () => global::System.Uri.UriSchemeNntp,

                (global::System.Uri u) => u.AbsolutePath,
                (global::System.Uri u) => u.AbsoluteUri,
                (global::System.Uri u) => u.Authority,
                (global::System.Uri u) => u.DnsSafeHost,
                (global::System.Uri u) => u.Fragment,
                (global::System.Uri u) => u.Host,
                (global::System.Uri u) => u.HostNameType,
                //(global::System.Uri u) => u.IdnHost, // REVIEW: Not accessible
                (global::System.Uri u) => u.IsAbsoluteUri,
                (global::System.Uri u) => u.IsDefaultPort,
                (global::System.Uri u) => u.IsFile,
                (global::System.Uri u) => u.IsLoopback,
                (global::System.Uri u) => u.IsUnc,
                (global::System.Uri u) => u.LocalPath,
                (global::System.Uri u) => u.OriginalString,
                (global::System.Uri u) => u.PathAndQuery,
                (global::System.Uri u) => u.Port,
                (global::System.Uri u) => u.Query,
                (global::System.Uri u) => u.Scheme,
                (global::System.Uri u) => u.UserEscaped,
                (global::System.Uri u) => u.UserInfo,

                (string name) => global::System.Uri.CheckHostName(name),
                (string schemeName) => global::System.Uri.CheckSchemeName(schemeName),
                (char digit) => global::System.Uri.FromHex(digit),
                (char character) => global::System.Uri.HexEscape(character),
                (char character) => global::System.Uri.IsHexDigit(character),
                (string pattern, int index) => global::System.Uri.IsHexEncoding(pattern, index),
                (string stringToEscape) => global::System.Uri.EscapeDataString(stringToEscape),
                //(string stringToEscape) => global::System.Uri.EscapeUriString(stringToEscape),
                (string stringToUnescape) => global::System.Uri.UnescapeDataString(stringToUnescape),
                (string uriString, global::System.UriKind uriKind) => global::System.Uri.IsWellFormedUriString(uriString, uriKind),

                (global::System.Uri u1, global::System.Uri u2) => u1 == u2,
                (global::System.Uri u1, global::System.Uri u2) => u1 != u2,

                // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
                //(global::System.Uri u) => u.ToString(),
                (global::System.Uri u, global::System.UriPartial part) => u.GetLeftPart(part),
                (global::System.Uri u, global::System.Uri uri) => u.IsBaseOf(uri),
                (global::System.Uri u) => u.IsWellFormedOriginalString(),
                (global::System.Uri u, global::System.Uri uri) => u.MakeRelativeUri(uri),
                (global::System.Uri u, global::System.UriComponents components, global::System.UriFormat format) => u.GetComponents(components, format),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Version" />.
            /// </summary>
            public static MemberTable Version { get; } = new MemberTable
            {
                () => new global::System.Version(),
                (int major, int minor) => new global::System.Version(major, minor),
                (int major, int minor, int build) => new global::System.Version(major, minor, build),
                (int major, int minor, int build, int revision) => new global::System.Version(major, minor, build, revision),
                (string version) => new global::System.Version(version),

                (global::System.Version v) => v.Build,
                (global::System.Version v) => v.Major,
                (global::System.Version v) => v.MajorRevision,
                (global::System.Version v) => v.Minor,
                (global::System.Version v) => v.MinorRevision,
                (global::System.Version v) => v.Revision,

                (string input) => global::System.Version.Parse(input),

                // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
                //(global::System.Version v) => v.ToString(),
                (global::System.Version v, int fieldCount) => v.ToString(fieldCount),
                (global::System.Version v, global::System.Version value) => v.CompareTo(value),
                (global::System.Version v, global::System.Version obj) => v.Equals(obj),

                (global::System.Version v1, global::System.Version v2) => v1 == v2,
                (global::System.Version v1, global::System.Version v2) => v1 != v2,
                (global::System.Version v1, global::System.Version v2) => v1 < v2,
                (global::System.Version v1, global::System.Version v2) => v1 <= v2,
                (global::System.Version v1, global::System.Version v2) => v1 > v2,
                (global::System.Version v1, global::System.Version v2) => v1 >= v2,
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on nullable types.
            /// </summary>
            public static MemberTable Nullable { get; } = new MemberTable
            {
                (Nullable<TValue> n) => n.HasValue,
                (Nullable<TValue> n) => n.Value,

                (Nullable<TValue> n) => n.GetValueOrDefault(),
                (Nullable<TValue> n, TValue defaultValue) => n.GetValueOrDefault(defaultValue),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on tuple types.
            /// </summary>
            public static MemberTable Tuple { get; } = new MemberTable
            {
                //
                // global::System.Tuple<>
                //
                (T t1) => global::System.Tuple.Create(t1),
                (T t1) => new Tuple<T>(t1),
                (Tuple<T> t) => t.Item1,

                //
                // global::System.Tuple<,>
                //
                (T t1, T t2) => global::System.Tuple.Create(t1, t2),
                (T t1, T t2) => new Tuple<T, T>(t1, t2),
                (Tuple<T, T> t) => t.Item1,
                (Tuple<T, T> t) => t.Item2,

                //
                // global::System.Tuple<,,>
                //
                (T t1, T t2, T t3) => global::System.Tuple.Create(t1, t2, t3),
                (T t1, T t2, T t3) => new Tuple<T, T, T>(t1, t2, t3),
                (Tuple<T, T, T> t) => t.Item1,
                (Tuple<T, T, T> t) => t.Item2,
                (Tuple<T, T, T> t) => t.Item3,

                //
                // global::System.Tuple<,,,>
                //
                (T t1, T t2, T t3, T t4) => global::System.Tuple.Create(t1, t2, t3, t4),
                (T t1, T t2, T t3, T t4) => new Tuple<T, T, T, T>(t1, t2, t3, t4),
                (Tuple<T, T, T, T> t) => t.Item1,
                (Tuple<T, T, T, T> t) => t.Item2,
                (Tuple<T, T, T, T> t) => t.Item3,
                (Tuple<T, T, T, T> t) => t.Item4,

                //
                // global::System.Tuple<,,,,>
                //
                (T t1, T t2, T t3, T t4, T t5) => global::System.Tuple.Create(t1, t2, t3, t4, t5),
                (T t1, T t2, T t3, T t4, T t5) => new Tuple<T, T, T, T, T>(t1, t2, t3, t4, t5),
                (Tuple<T, T, T, T, T> t) => t.Item1,
                (Tuple<T, T, T, T, T> t) => t.Item2,
                (Tuple<T, T, T, T, T> t) => t.Item3,
                (Tuple<T, T, T, T, T> t) => t.Item4,
                (Tuple<T, T, T, T, T> t) => t.Item5,

                //
                // global::System.Tuple<,,,,,>
                //
                (T t1, T t2, T t3, T t4, T t5, T t6) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6),
                (T t1, T t2, T t3, T t4, T t5, T t6) => new Tuple<T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6),
                (Tuple<T, T, T, T, T, T> t) => t.Item1,
                (Tuple<T, T, T, T, T, T> t) => t.Item2,
                (Tuple<T, T, T, T, T, T> t) => t.Item3,
                (Tuple<T, T, T, T, T, T> t) => t.Item4,
                (Tuple<T, T, T, T, T, T> t) => t.Item5,
                (Tuple<T, T, T, T, T, T> t) => t.Item6,

                //
                // global::System.Tuple<,,,,,,>
                //
                (T t1, T t2, T t3, T t4, T t5, T t6, T t7) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6, t7),
                (T t1, T t2, T t3, T t4, T t5, T t6, T t7) => new Tuple<T, T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6, t7),
                (Tuple<T, T, T, T, T, T, T> t) => t.Item1,
                (Tuple<T, T, T, T, T, T, T> t) => t.Item2,
                (Tuple<T, T, T, T, T, T, T> t) => t.Item3,
                (Tuple<T, T, T, T, T, T, T> t) => t.Item4,
                (Tuple<T, T, T, T, T, T, T> t) => t.Item5,
                (Tuple<T, T, T, T, T, T, T> t) => t.Item6,
                (Tuple<T, T, T, T, T, T, T> t) => t.Item7,

                //
                // global::System.Tuple<,,,,,,,>
                //
                (T t1, T t2, T t3, T t4, T t5, T t6, T t7, T r) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6, t7, r),
                (T t1, T t2, T t3, T t4, T t5, T t6, T t7, T r) => new Tuple<T, T, T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6, t7, r),
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item1,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item2,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item3,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item4,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item5,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item6,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Item7,
                (Tuple<T, T, T, T, T, T, T, T> t) => t.Rest,
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on value tuple types.
            /// </summary>
            public static MemberTable ValueTuple { get; } = new MemberTable
            {
                // NB: Value tuples are mutable, so constructors and Create factory calls are not listed here.

                //
                // global::System.ValueTuple<>
                //
                (ValueTuple<T> t) => t.Item1,

                //
                // global::System.ValueTuple<,>
                //
                (ValueTuple<T, T> t) => t.Item1,
                (ValueTuple<T, T> t) => t.Item2,

                //
                // global::System.ValueTuple<,,>
                //
                (ValueTuple<T, T, T> t) => t.Item1,
                (ValueTuple<T, T, T> t) => t.Item2,
                (ValueTuple<T, T, T> t) => t.Item3,

                //
                // global::System.ValueTuple<,,,>
                //
                (ValueTuple<T, T, T, T> t) => t.Item1,
                (ValueTuple<T, T, T, T> t) => t.Item2,
                (ValueTuple<T, T, T, T> t) => t.Item3,
                (ValueTuple<T, T, T, T> t) => t.Item4,

                //
                // global::System.ValueTuple<,,,,>
                //
                (ValueTuple<T, T, T, T, T> t) => t.Item1,
                (ValueTuple<T, T, T, T, T> t) => t.Item2,
                (ValueTuple<T, T, T, T, T> t) => t.Item3,
                (ValueTuple<T, T, T, T, T> t) => t.Item4,
                (ValueTuple<T, T, T, T, T> t) => t.Item5,

                //
                // global::System.ValueTuple<,,,,,>
                //
                (ValueTuple<T, T, T, T, T, T> t) => t.Item1,
                (ValueTuple<T, T, T, T, T, T> t) => t.Item2,
                (ValueTuple<T, T, T, T, T, T> t) => t.Item3,
                (ValueTuple<T, T, T, T, T, T> t) => t.Item4,
                (ValueTuple<T, T, T, T, T, T> t) => t.Item5,
                (ValueTuple<T, T, T, T, T, T> t) => t.Item6,

                //
                // global::System.ValueTuple<,,,,,,>
                //
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item1,
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item2,
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item3,
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item4,
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item5,
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item6,
                (ValueTuple<T, T, T, T, T, T, T> t) => t.Item7,

                //
                // global::System.ValueTuple<,,,,,,,>
                //
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item1,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item2,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item3,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item4,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item5,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item6,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item7,
                (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Rest,
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Math" />.
            /// </summary>
            public static MemberTable Math { get; } = new MemberTable
            {
                () => global::System.Math.E,
                () => global::System.Math.PI,
#if NET6_0
                () => global::System.Math.Tau,
#endif

                (double d) => global::System.Math.Acos(d),
                (double d) => global::System.Math.Asin(d),
                (double d) => global::System.Math.Atan(d),
                (double y, double x) => global::System.Math.Atan2(y, x),
                (decimal d) => global::System.Math.Ceiling(d),
                (double a) => global::System.Math.Ceiling(a),
                (double d) => global::System.Math.Cos(d),
                (double value) => global::System.Math.Cosh(value),
                (decimal d) => global::System.Math.Floor(d),
                (double d) => global::System.Math.Floor(d),
                (double a) => global::System.Math.Sin(a),
                (double a) => global::System.Math.Tan(a),
                (double value) => global::System.Math.Sinh(value),
                (double value) => global::System.Math.Tanh(value),
                (double a) => global::System.Math.Round(a),
                (double value, int digits) => global::System.Math.Round(value, digits),
                (double value, MidpointRounding mode) => global::System.Math.Round(value, mode),
                (double value, int digits, MidpointRounding mode) => global::System.Math.Round(value, digits, mode),
                (decimal d) => global::System.Math.Round(d),
                (decimal d, int decimals) => global::System.Math.Round(d, decimals),
                (decimal d, MidpointRounding mode) => global::System.Math.Round(d, mode),
                (decimal d, int decimals, MidpointRounding mode) => global::System.Math.Round(d, decimals, mode),
                (decimal d) => global::System.Math.Truncate(d),
                (double d) => global::System.Math.Truncate(d),
                (double d) => global::System.Math.Sqrt(d),
                (double d) => global::System.Math.Log(d),
                (double d) => global::System.Math.Log10(d),
                (double d) => global::System.Math.Exp(d),
                (double x, double y) => global::System.Math.Pow(x, y),
                (double x, double y) => global::System.Math.IEEERemainder(x, y),
                (sbyte value) => global::System.Math.Abs(value),
                (short value) => global::System.Math.Abs(value),
                (int value) => global::System.Math.Abs(value),
                (long value) => global::System.Math.Abs(value),
                (float value) => global::System.Math.Abs(value),
                (double value) => global::System.Math.Abs(value),
                (decimal value) => global::System.Math.Abs(value),
                (sbyte val1, sbyte val2) => global::System.Math.Max(val1, val2),
                (byte val1, byte val2) => global::System.Math.Max(val1, val2),
                (short val1, short val2) => global::System.Math.Max(val1, val2),
                (ushort val1, ushort val2) => global::System.Math.Max(val1, val2),
                (int val1, int val2) => global::System.Math.Max(val1, val2),
                (uint val1, uint val2) => global::System.Math.Max(val1, val2),
                (long val1, long val2) => global::System.Math.Max(val1, val2),
                (ulong val1, ulong val2) => global::System.Math.Max(val1, val2),
                (float val1, float val2) => global::System.Math.Max(val1, val2),
                (double val1, double val2) => global::System.Math.Max(val1, val2),
                (decimal val1, decimal val2) => global::System.Math.Max(val1, val2),
                (sbyte val1, sbyte val2) => global::System.Math.Min(val1, val2),
                (byte val1, byte val2) => global::System.Math.Min(val1, val2),
                (short val1, short val2) => global::System.Math.Min(val1, val2),
                (ushort val1, ushort val2) => global::System.Math.Min(val1, val2),
                (int val1, int val2) => global::System.Math.Min(val1, val2),
                (uint val1, uint val2) => global::System.Math.Min(val1, val2),
                (long val1, long val2) => global::System.Math.Min(val1, val2),
                (ulong val1, ulong val2) => global::System.Math.Min(val1, val2),
                (float val1, float val2) => global::System.Math.Min(val1, val2),
                (double val1, double val2) => global::System.Math.Min(val1, val2),
                (decimal val1, decimal val2) => global::System.Math.Min(val1, val2),
                (double a, double newBase) => global::System.Math.Log(a, newBase),
                (sbyte value) => global::System.Math.Sign(value),
                (short value) => global::System.Math.Sign(value),
                (int value) => global::System.Math.Sign(value),
                (long value) => global::System.Math.Sign(value),
                (float value) => global::System.Math.Sign(value),
                (double value) => global::System.Math.Sign(value),
                (decimal value) => global::System.Math.Sign(value),
                (int a, int b) => global::System.Math.BigMul(a, b),

#if NET6_0 || NETSTANDARD2_1
                (double d) => global::System.Math.Acosh(d),
                (double d) => global::System.Math.Asinh(d),
                (double d) => global::System.Math.Atanh(d),
                (double d) => global::System.Math.Cbrt(d),
                (ulong value, ulong min, ulong max) => global::System.Math.Clamp(value, min, max),
                (uint value, uint min, uint max) => global::System.Math.Clamp(value, min, max),
                (ushort value, ushort min, ushort max) => global::System.Math.Clamp(value, min, max),
                (byte value, byte min, byte max) => global::System.Math.Clamp(value, min, max),
                (long value, long min, long max) => global::System.Math.Clamp(value, min, max),
                (int value, int min, int max) => global::System.Math.Clamp(value, min, max),
                (short value, short min, short max) => global::System.Math.Clamp(value, min, max),
                (sbyte value, sbyte min, sbyte max) => global::System.Math.Clamp(value, min, max),
                (double value, double min, double max) => global::System.Math.Clamp(value, min, max),
                (float value, float min, float max) => global::System.Math.Clamp(value, min, max),
                (decimal value, decimal min, decimal max) => global::System.Math.Clamp(value, min, max),
#endif
#if NET6_0
                (double x) => global::System.Math.BitDecrement(x),
                (double x) => global::System.Math.BitIncrement(x),
                (double x) => global::System.Math.ILogB(x),
                (double x) => global::System.Math.Log2(x),
                (double x, double y) => global::System.Math.CopySign(x, y),
                (double x, double y) => global::System.Math.MaxMagnitude(x, y),
                (double x, double y) => global::System.Math.MinMagnitude(x, y),
                (double x, int n) => global::System.Math.ScaleB(x, n),
                (double x, double y, double z) => global::System.Math.FusedMultiplyAdd(x, y, z),
#endif
            }.ToReadOnly();

#if NET6_0 || NETSTANDARD2_1
            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.MathF" />.
            /// </summary>
            public static MemberTable MathF { get; } = new MemberTable
            {
                () => global::System.MathF.E,
                () => global::System.MathF.PI,

                (float x) => global::System.MathF.Abs(x),
                (float x) => global::System.MathF.Acos(x),
                (float x) => global::System.MathF.Acosh(x),
                (float x) => global::System.MathF.Asin(x),
                (float x) => global::System.MathF.Asinh(x),
                (float x) => global::System.MathF.Atan(x),
                (float y, float x) => global::System.MathF.Atan2(y, x),
                (float x) => global::System.MathF.Atanh(x),
                (float x) => global::System.MathF.Cbrt(x),
                (float x) => global::System.MathF.Ceiling(x),
                (float x) => global::System.MathF.Cos(x),
                (float x) => global::System.MathF.Cosh(x),
                (float x) => global::System.MathF.Exp(x),
                (float x) => global::System.MathF.Floor(x),
                (float x, float y) => global::System.MathF.IEEERemainder(x, y),
                (float x) => global::System.MathF.Log(x),
                (float x, float y) => global::System.MathF.Log(x, y),
                (float x) => global::System.MathF.Log10(x),
                (float x, float y) => global::System.MathF.Max(x, y),
                (float x, float y) => global::System.MathF.Min(x, y),
                (float x, float y) => global::System.MathF.Pow(x, y),
                (float x) => global::System.MathF.Round(x),
                (float x, int digits) => global::System.MathF.Round(x, digits),
                (float x, MidpointRounding mode) => global::System.MathF.Round(x, mode),
                (float x, int digits, MidpointRounding mode) => global::System.MathF.Round(x, digits, mode),
                (float x) => global::System.MathF.Sign(x),
                (float x) => global::System.MathF.Sin(x),
                (float x) => global::System.MathF.Sinh(x),
                (float x) => global::System.MathF.Sqrt(x),
                (float x) => global::System.MathF.Tan(x),
                (float x) => global::System.MathF.Tanh(x),
                (float x) => global::System.MathF.Truncate(x),

#if NET6_0
                () => global::System.MathF.Tau,

                (float x) => global::System.MathF.BitDecrement(x),
                (float x) => global::System.MathF.BitIncrement(x),
                (float x, float y) => global::System.MathF.CopySign(x, y),
                (float x, float y, float z) => global::System.MathF.FusedMultiplyAdd(x, y, z),
                (float x) => global::System.MathF.ILogB(x),
                (float x) => global::System.MathF.Log2(x),
                (float x, float y) => global::System.MathF.MaxMagnitude(x, y),
                (float x, float y) => global::System.MathF.MinMagnitude(x, y),
                (float x, int n) => global::System.MathF.ScaleB(x, n),
#endif
            }.ToReadOnly();
#endif

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.BitConverter" />.
            /// </summary>
            public static MemberTable BitConverter { get; } = new MemberTable
            {
                // NB: Omitting GetBytes overloads which return a mutable byte[].

                (double value) => global::System.BitConverter.DoubleToInt64Bits(value),
                (long value) => global::System.BitConverter.Int64BitsToDouble(value),
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

#if NET6_0 || NETSTANDARD2_1
                (float value) => global::System.BitConverter.SingleToInt32Bits(value),
                (int value) => global::System.BitConverter.Int32BitsToSingle(value),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Convert" />.
            /// </summary>
            public static MemberTable Convert { get; } = new MemberTable
            {
                (Object value) => global::System.Convert.ToBoolean(value),
                (Boolean value) => global::System.Convert.ToBoolean(value),
                (SByte value) => global::System.Convert.ToBoolean(value),
                (Char value) => global::System.Convert.ToBoolean(value),
                (Byte value) => global::System.Convert.ToBoolean(value),
                (Int16 value) => global::System.Convert.ToBoolean(value),
                (UInt16 value) => global::System.Convert.ToBoolean(value),
                (Int32 value) => global::System.Convert.ToBoolean(value),
                (UInt32 value) => global::System.Convert.ToBoolean(value),
                (Int64 value) => global::System.Convert.ToBoolean(value),
                (UInt64 value) => global::System.Convert.ToBoolean(value),
                (Single value) => global::System.Convert.ToBoolean(value),
                (Double value) => global::System.Convert.ToBoolean(value),
                (Decimal value) => global::System.Convert.ToBoolean(value),
                (DateTime value) => global::System.Convert.ToBoolean(value),
                (Object value) => global::System.Convert.ToChar(value),
                (Boolean value) => global::System.Convert.ToChar(value),
                (Char value) => global::System.Convert.ToChar(value),
                (SByte value) => global::System.Convert.ToChar(value),
                (Byte value) => global::System.Convert.ToChar(value),
                (Int16 value) => global::System.Convert.ToChar(value),
                (UInt16 value) => global::System.Convert.ToChar(value),
                (Int32 value) => global::System.Convert.ToChar(value),
                (UInt32 value) => global::System.Convert.ToChar(value),
                (Int64 value) => global::System.Convert.ToChar(value),
                (UInt64 value) => global::System.Convert.ToChar(value),
                (Single value) => global::System.Convert.ToChar(value),
                (Double value) => global::System.Convert.ToChar(value),
                (Decimal value) => global::System.Convert.ToChar(value),
                (DateTime value) => global::System.Convert.ToChar(value),
                (Object value) => global::System.Convert.ToSByte(value),
                (Boolean value) => global::System.Convert.ToSByte(value),
                (SByte value) => global::System.Convert.ToSByte(value),
                (Char value) => global::System.Convert.ToSByte(value),
                (Byte value) => global::System.Convert.ToSByte(value),
                (Int16 value) => global::System.Convert.ToSByte(value),
                (UInt16 value) => global::System.Convert.ToSByte(value),
                (Int32 value) => global::System.Convert.ToSByte(value),
                (UInt32 value) => global::System.Convert.ToSByte(value),
                (Int64 value) => global::System.Convert.ToSByte(value),
                (UInt64 value) => global::System.Convert.ToSByte(value),
                (Single value) => global::System.Convert.ToSByte(value),
                (Double value) => global::System.Convert.ToSByte(value),
                (Decimal value) => global::System.Convert.ToSByte(value),
                (DateTime value) => global::System.Convert.ToSByte(value),
                (Object value) => global::System.Convert.ToByte(value),
                (Boolean value) => global::System.Convert.ToByte(value),
                (Byte value) => global::System.Convert.ToByte(value),
                (Char value) => global::System.Convert.ToByte(value),
                (SByte value) => global::System.Convert.ToByte(value),
                (Int16 value) => global::System.Convert.ToByte(value),
                (UInt16 value) => global::System.Convert.ToByte(value),
                (Int32 value) => global::System.Convert.ToByte(value),
                (UInt32 value) => global::System.Convert.ToByte(value),
                (Int64 value) => global::System.Convert.ToByte(value),
                (UInt64 value) => global::System.Convert.ToByte(value),
                (Single value) => global::System.Convert.ToByte(value),
                (Double value) => global::System.Convert.ToByte(value),
                (Decimal value) => global::System.Convert.ToByte(value),
                (DateTime value) => global::System.Convert.ToByte(value),
                (Object value) => global::System.Convert.ToInt16(value),
                (Boolean value) => global::System.Convert.ToInt16(value),
                (Char value) => global::System.Convert.ToInt16(value),
                (SByte value) => global::System.Convert.ToInt16(value),
                (Byte value) => global::System.Convert.ToInt16(value),
                (UInt16 value) => global::System.Convert.ToInt16(value),
                (Int32 value) => global::System.Convert.ToInt16(value),
                (UInt32 value) => global::System.Convert.ToInt16(value),
                (Int16 value) => global::System.Convert.ToInt16(value),
                (Int64 value) => global::System.Convert.ToInt16(value),
                (UInt64 value) => global::System.Convert.ToInt16(value),
                (Single value) => global::System.Convert.ToInt16(value),
                (Double value) => global::System.Convert.ToInt16(value),
                (Decimal value) => global::System.Convert.ToInt16(value),
                (DateTime value) => global::System.Convert.ToInt16(value),
                (Object value) => global::System.Convert.ToUInt16(value),
                (Boolean value) => global::System.Convert.ToUInt16(value),
                (Char value) => global::System.Convert.ToUInt16(value),
                (SByte value) => global::System.Convert.ToUInt16(value),
                (Byte value) => global::System.Convert.ToUInt16(value),
                (Int16 value) => global::System.Convert.ToUInt16(value),
                (Int32 value) => global::System.Convert.ToUInt16(value),
                (UInt16 value) => global::System.Convert.ToUInt16(value),
                (UInt32 value) => global::System.Convert.ToUInt16(value),
                (Int64 value) => global::System.Convert.ToUInt16(value),
                (UInt64 value) => global::System.Convert.ToUInt16(value),
                (Single value) => global::System.Convert.ToUInt16(value),
                (Double value) => global::System.Convert.ToUInt16(value),
                (Decimal value) => global::System.Convert.ToUInt16(value),
                (DateTime value) => global::System.Convert.ToUInt16(value),
                (Object value) => global::System.Convert.ToInt32(value),
                (Boolean value) => global::System.Convert.ToInt32(value),
                (Char value) => global::System.Convert.ToInt32(value),
                (SByte value) => global::System.Convert.ToInt32(value),
                (Byte value) => global::System.Convert.ToInt32(value),
                (Int16 value) => global::System.Convert.ToInt32(value),
                (UInt16 value) => global::System.Convert.ToInt32(value),
                (UInt32 value) => global::System.Convert.ToInt32(value),
                (Int32 value) => global::System.Convert.ToInt32(value),
                (Int64 value) => global::System.Convert.ToInt32(value),
                (UInt64 value) => global::System.Convert.ToInt32(value),
                (Single value) => global::System.Convert.ToInt32(value),
                (Double value) => global::System.Convert.ToInt32(value),
                (Decimal value) => global::System.Convert.ToInt32(value),
                (DateTime value) => global::System.Convert.ToInt32(value),
                (Object value) => global::System.Convert.ToUInt32(value),
                (Boolean value) => global::System.Convert.ToUInt32(value),
                (Char value) => global::System.Convert.ToUInt32(value),
                (SByte value) => global::System.Convert.ToUInt32(value),
                (Byte value) => global::System.Convert.ToUInt32(value),
                (Int16 value) => global::System.Convert.ToUInt32(value),
                (UInt16 value) => global::System.Convert.ToUInt32(value),
                (Int32 value) => global::System.Convert.ToUInt32(value),
                (UInt32 value) => global::System.Convert.ToUInt32(value),
                (Int64 value) => global::System.Convert.ToUInt32(value),
                (UInt64 value) => global::System.Convert.ToUInt32(value),
                (Single value) => global::System.Convert.ToUInt32(value),
                (Double value) => global::System.Convert.ToUInt32(value),
                (Decimal value) => global::System.Convert.ToUInt32(value),
                (DateTime value) => global::System.Convert.ToUInt32(value),
                (Object value) => global::System.Convert.ToInt64(value),
                (Boolean value) => global::System.Convert.ToInt64(value),
                (Char value) => global::System.Convert.ToInt64(value),
                (SByte value) => global::System.Convert.ToInt64(value),
                (Byte value) => global::System.Convert.ToInt64(value),
                (Int16 value) => global::System.Convert.ToInt64(value),
                (UInt16 value) => global::System.Convert.ToInt64(value),
                (Int32 value) => global::System.Convert.ToInt64(value),
                (UInt32 value) => global::System.Convert.ToInt64(value),
                (UInt64 value) => global::System.Convert.ToInt64(value),
                (Int64 value) => global::System.Convert.ToInt64(value),
                (Single value) => global::System.Convert.ToInt64(value),
                (Double value) => global::System.Convert.ToInt64(value),
                (Decimal value) => global::System.Convert.ToInt64(value),
                (DateTime value) => global::System.Convert.ToInt64(value),
                (Object value) => global::System.Convert.ToUInt64(value),
                (Boolean value) => global::System.Convert.ToUInt64(value),
                (Char value) => global::System.Convert.ToUInt64(value),
                (SByte value) => global::System.Convert.ToUInt64(value),
                (Byte value) => global::System.Convert.ToUInt64(value),
                (Int16 value) => global::System.Convert.ToUInt64(value),
                (UInt16 value) => global::System.Convert.ToUInt64(value),
                (Int32 value) => global::System.Convert.ToUInt64(value),
                (UInt32 value) => global::System.Convert.ToUInt64(value),
                (Int64 value) => global::System.Convert.ToUInt64(value),
                (UInt64 value) => global::System.Convert.ToUInt64(value),
                (Single value) => global::System.Convert.ToUInt64(value),
                (Double value) => global::System.Convert.ToUInt64(value),
                (Decimal value) => global::System.Convert.ToUInt64(value),
                (DateTime value) => global::System.Convert.ToUInt64(value),
                (Object value) => global::System.Convert.ToSingle(value),
                (SByte value) => global::System.Convert.ToSingle(value),
                (Byte value) => global::System.Convert.ToSingle(value),
                (Char value) => global::System.Convert.ToSingle(value),
                (Int16 value) => global::System.Convert.ToSingle(value),
                (UInt16 value) => global::System.Convert.ToSingle(value),
                (Int32 value) => global::System.Convert.ToSingle(value),
                (UInt32 value) => global::System.Convert.ToSingle(value),
                (Int64 value) => global::System.Convert.ToSingle(value),
                (UInt64 value) => global::System.Convert.ToSingle(value),
                (Single value) => global::System.Convert.ToSingle(value),
                (Double value) => global::System.Convert.ToSingle(value),
                (Decimal value) => global::System.Convert.ToSingle(value),
                (Boolean value) => global::System.Convert.ToSingle(value),
                (DateTime value) => global::System.Convert.ToSingle(value),
                (Object value) => global::System.Convert.ToDouble(value),
                (SByte value) => global::System.Convert.ToDouble(value),
                (Byte value) => global::System.Convert.ToDouble(value),
                (Int16 value) => global::System.Convert.ToDouble(value),
                (Char value) => global::System.Convert.ToDouble(value),
                (UInt16 value) => global::System.Convert.ToDouble(value),
                (Int32 value) => global::System.Convert.ToDouble(value),
                (UInt32 value) => global::System.Convert.ToDouble(value),
                (Int64 value) => global::System.Convert.ToDouble(value),
                (UInt64 value) => global::System.Convert.ToDouble(value),
                (Single value) => global::System.Convert.ToDouble(value),
                (Double value) => global::System.Convert.ToDouble(value),
                (Decimal value) => global::System.Convert.ToDouble(value),
                (Boolean value) => global::System.Convert.ToDouble(value),
                (DateTime value) => global::System.Convert.ToDouble(value),
                (Object value) => global::System.Convert.ToDecimal(value),
                (SByte value) => global::System.Convert.ToDecimal(value),
                (Byte value) => global::System.Convert.ToDecimal(value),
                (Char value) => global::System.Convert.ToDecimal(value),
                (Int16 value) => global::System.Convert.ToDecimal(value),
                (UInt16 value) => global::System.Convert.ToDecimal(value),
                (Int32 value) => global::System.Convert.ToDecimal(value),
                (UInt32 value) => global::System.Convert.ToDecimal(value),
                (Int64 value) => global::System.Convert.ToDecimal(value),
                (UInt64 value) => global::System.Convert.ToDecimal(value),
                (Single value) => global::System.Convert.ToDecimal(value),
                (Double value) => global::System.Convert.ToDecimal(value),
                (Decimal value) => global::System.Convert.ToDecimal(value),
                (Boolean value) => global::System.Convert.ToDecimal(value),
                (DateTime value) => global::System.Convert.ToDecimal(value),
                (DateTime value) => global::System.Convert.ToDateTime(value),
                (Object value) => global::System.Convert.ToDateTime(value),
                (SByte value) => global::System.Convert.ToDateTime(value),
                (Byte value) => global::System.Convert.ToDateTime(value),
                (Int16 value) => global::System.Convert.ToDateTime(value),
                (UInt16 value) => global::System.Convert.ToDateTime(value),
                (Int32 value) => global::System.Convert.ToDateTime(value),
                (UInt32 value) => global::System.Convert.ToDateTime(value),
                (Int64 value) => global::System.Convert.ToDateTime(value),
                (UInt64 value) => global::System.Convert.ToDateTime(value),
                (Boolean value) => global::System.Convert.ToDateTime(value),
                (Char value) => global::System.Convert.ToDateTime(value),
                (Single value) => global::System.Convert.ToDateTime(value),
                (Double value) => global::System.Convert.ToDateTime(value),
                (Decimal value) => global::System.Convert.ToDateTime(value),
                (String value) => global::System.Convert.ToString(value),
                (byte[] inArray) => global::System.Convert.ToBase64String(inArray),
                (byte[] inArray, int offset, int length) => global::System.Convert.ToBase64String(inArray, offset, length),
                (byte[] inArray, int offset, int length, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, offset, length, options),
                (byte[] inArray, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, options),

#if NET6_0
                (byte[] inArray) => global::System.Convert.ToHexString(inArray),
                (byte[] inArray, int offset, int length) => global::System.Convert.ToHexString(inArray, offset, length),
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Array" />.
            /// </summary>
            public static MemberTable Array { get; } = new MemberTable
            {
                // NB: Array.Empty<T>() is omitted because the array return type is considered mutable.

                (global::System.Array array) => array.IsFixedSize,
                (global::System.Array array) => array.IsReadOnly,
                (global::System.Array array) => array.IsSynchronized,
                (global::System.Array array) => array.Length,
                (global::System.Array array) => array.LongLength,
                (global::System.Array array) => array.Rank,

                (global::System.Array array, object value) => global::System.Array.BinarySearch(array, value),
                (global::System.Array array, int index, int length, object value) => global::System.Array.BinarySearch(array, index, length, value),
                (T[] array, T value) => global::System.Array.BinarySearch(array, value),
                (T[] array, int index, int length, T value) => global::System.Array.BinarySearch(array, index, length, value),

                (global::System.Array array, int dimension) => array.GetLength(dimension),
                (global::System.Array array, int dimension) => array.GetLongLength(dimension),

                (global::System.Array array, int dimension) => array.GetLowerBound(dimension),
                (global::System.Array array, int dimension) => array.GetUpperBound(dimension),

                // NB: GetValue methods are omitted because these return global::System.Object which is not immutable.

                (global::System.Array array, object value) => global::System.Array.IndexOf(array, value),
                (global::System.Array array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex),
                (global::System.Array array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count),
                (T[] array, object value) => global::System.Array.IndexOf(array, value),
                (T[] array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex),
                (T[] array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count),

                (global::System.Array array, object value) => global::System.Array.LastIndexOf(array, value),
                (global::System.Array array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex),
                (global::System.Array array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count),
                (T[] array, object value) => global::System.Array.LastIndexOf(array, value),
                (T[] array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex),
                (T[] array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count),

                // NB: Methods with Predicate<T> parameters are ; the delegate may not be a pure function.
                // REVIEW: In practice, it is assumed that predicates are pure. Should we provide a table including these members, so users can opt-in?
            }.ToReadOnly();

#if NET6_0 || NETSTANDARD2_1
            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Index" />.
            /// </summary>
            public static MemberTable Index { get; } = new MemberTable
            {
                () => global::System.Index.End,
                () => global::System.Index.Start,

                (global::System.Index i) => i.IsFromEnd,
                (global::System.Index i) => i.Value,

                (int value, bool fromEnd) => new global::System.Index(value, fromEnd),

                (int value) => (global::System.Index)value,

                (int value) => global::System.Index.FromEnd(value),
                (int value) => global::System.Index.FromStart(value),

                (global::System.Index i, int length) => i.GetOffset(length),

                (global::System.Index i, global::System.Index other) => i.Equals(other),
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members on <see cref="global::System.Range" />.
            /// </summary>
            public static MemberTable Range { get; } = new MemberTable
            {
                () => global::System.Range.All,

                (global::System.Index start, global::System.Index end) => new global::System.Range(start, end),

                (global::System.Index start) => global::System.Range.StartAt(start),
                (global::System.Index end) => global::System.Range.EndAt(end),

                (global::System.Range r) => r.End,
                (global::System.Range r) => r.Start,

                // NB: GetOffsetAndLength is omitted because the ValueTuple<,> return type is not immutable.

                (global::System.Range r, global::System.Range other) => r.Equals(other),
            }.ToReadOnly();
#endif

            /// <summary>
            /// Pure members in the System.Collections namespace.
            /// </summary>
            public static class Collections
            {
                /// <summary>
                /// Pure members in the System.Collections.Generic namespace.
                /// </summary>
                public static class Generic
                {
                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Collections.Generic.KeyValuePair{TKey, TValue}" />.
                    /// </summary>
                    public static MemberTable KeyValuePair { get; } = new MemberTable
                    {
                        (T t1, T t2) => new global::System.Collections.Generic.KeyValuePair<T, T>(t1, t2),

                        (global::System.Collections.Generic.KeyValuePair<T, T> t) => t.Key,
                        (global::System.Collections.Generic.KeyValuePair<T, T> t) => t.Value,
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members in the System.Collections.Generic namespace.
                    /// </summary>
                    public static MemberTable AllThisNamespaceOnly { get; } = new MemberTable
                    {
                        KeyValuePair,
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members in the System.Collections.Generic namespace and any child namespace.
                    /// </summary>
                    public static MemberTable AllThisAndChildNamespaces { get; } = new MemberTable
                    {
                        AllThisNamespaceOnly,
                    }.ToReadOnly();
                }

                /// <summary>
                /// Gets a table of pure members in the System.Collections namespace.
                /// </summary>
                public static MemberTable AllThisNamespaceOnly { get; } = new MemberTable
                {
                }.ToReadOnly();

                /// <summary>
                /// Gets a table of pure members in the System.Collections namespace and any child namespace.
                /// </summary>
                public static MemberTable AllThisAndChildNamespaces { get; } = new MemberTable
                {
                    AllThisNamespaceOnly,

                    Generic.AllThisAndChildNamespaces,
                }.ToReadOnly();
            }

            /// <summary>
            /// Pure members in the System.Text namespace.
            /// </summary>
            public static class Text
            {
                /// <summary>
                /// Pure members in the System.Text.RegularExpressions namespace.
                /// </summary>
                public static class RegularExpressions
                {
                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Regex" />.
                    /// </summary>
                    public static MemberTable Regex { get; } = new MemberTable
                    {
                        (string pattern) => new global::System.Text.RegularExpressions.Regex(pattern),
                        (string pattern, global::System.Text.RegularExpressions.RegexOptions options) => new global::System.Text.RegularExpressions.Regex(pattern, options),
                        (string pattern, global::System.Text.RegularExpressions.RegexOptions options, TimeSpan matchTimeout) => new global::System.Text.RegularExpressions.Regex(pattern, options, matchTimeout),

                        (global::System.Text.RegularExpressions.Regex r) => r.MatchTimeout,
                        (global::System.Text.RegularExpressions.Regex r) => r.Options,
                        (global::System.Text.RegularExpressions.Regex r) => r.RightToLeft,

                        (string str) => global::System.Text.RegularExpressions.Regex.Escape(str),

                        // NB: GetGroupNames and GetGroupNumbers are omitted; these return mutable arrays.

                        (global::System.Text.RegularExpressions.Regex r, int i) => r.GroupNameFromNumber(i),
                        (global::System.Text.RegularExpressions.Regex r, string name) => r.GroupNumberFromName(name),

                        (string input, string pattern) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern),
                        (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern, options),
                        (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern, options, matchTimeout),

                        (global::System.Text.RegularExpressions.Regex r, string input) => r.IsMatch(input),
                        (global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.IsMatch(input, startat),

                        (string input, string pattern) => global::System.Text.RegularExpressions.Regex.Match(input, pattern),
                        (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Match(input, pattern, options),
                        (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Match(input, pattern, options, matchTimeout),

                        (global::System.Text.RegularExpressions.Regex r, string input) => r.Match(input),
                        (global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.Match(input, startat),
                        (global::System.Text.RegularExpressions.Regex r, string input, int beginning, int length) => r.Match(input, beginning, length),

                        (string input, string pattern) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern),
                        (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern, options),
                        (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern, options, matchTimeout),

                        (global::System.Text.RegularExpressions.Regex r, string input) => r.Matches(input),
                        (global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.Matches(input, startat),

                        (string input, string pattern, string replacement) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement),
                        (string input, string pattern, string replacement, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options),
                        (string input, string pattern, string replacement, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options, matchTimeout),

                        (global::System.Text.RegularExpressions.Regex r, string input, string replacement) => r.Replace(input, replacement),
                        (global::System.Text.RegularExpressions.Regex r, string input, string replacement, int count) => r.Replace(input, replacement, count),
                        (global::System.Text.RegularExpressions.Regex r, string input, string replacement, int count, int startat) => r.Replace(input, replacement, count, startat),

                        // NB: Replace overloads with MatchEvaluator are omitted; the delegate may not be a pure function.
                        // REVIEW: In practice, it is assumed that the match evaluator is pure. Should we provide a table including these members, so users can opt-in?

                        // NB: Split overloads are omitted; these return mutable arrays.

                        (global::System.Text.RegularExpressions.Regex r) => r.ToString(),

                        (string str) => global::System.Text.RegularExpressions.Regex.Unescape(str),
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.MatchCollection" />.
                    /// </summary>
                    public static MemberTable MatchCollection { get; } = new MemberTable
                    {
                        (global::System.Text.RegularExpressions.MatchCollection c) => c.Count,
                        (global::System.Text.RegularExpressions.MatchCollection c) => c.IsReadOnly,
                        (global::System.Text.RegularExpressions.MatchCollection c) => c.IsSynchronized,

                        (global::System.Text.RegularExpressions.MatchCollection c, int i) => c[i],
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Match" />.
                    /// </summary>
                    public static MemberTable Match { get; } = new MemberTable
                    {
                        () => global::System.Text.RegularExpressions.Match.Empty,

                        (global::System.Text.RegularExpressions.Match m) => m.Groups,

                        (global::System.Text.RegularExpressions.Match m) => m.NextMatch(),
                        (global::System.Text.RegularExpressions.Match m, string replacement) => m.Result(replacement), // NB: Virtual but only internal derivees are allowed.
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.GroupCollection" />.
                    /// </summary>
                    public static MemberTable GroupCollection { get; } = new MemberTable
                    {
                        (global::System.Text.RegularExpressions.GroupCollection g) => g.Count,
                        (global::System.Text.RegularExpressions.GroupCollection g) => g.IsReadOnly,
                        (global::System.Text.RegularExpressions.GroupCollection g) => g.IsSynchronized,

                        (global::System.Text.RegularExpressions.GroupCollection g, int groupnum) => g[groupnum],
                        (global::System.Text.RegularExpressions.GroupCollection g, string groupname) => g[groupname],
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Group" />.
                    /// </summary>
                    public static MemberTable Group { get; } = new MemberTable
                    {
                        // (global::System.Text.RegularExpressions.Group g) => g.Name, // NB: Not in .NET Standard 2.0.
                        (global::System.Text.RegularExpressions.Group g) => g.Success,
                        (global::System.Text.RegularExpressions.Group g) => g.Captures,
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.CaptureCollection" />.
                    /// </summary>
                    public static MemberTable CaptureCollection { get; } = new MemberTable
                    {
                        (global::System.Text.RegularExpressions.CaptureCollection c) => c.Count,
                        (global::System.Text.RegularExpressions.CaptureCollection c) => c.IsReadOnly,
                        (global::System.Text.RegularExpressions.CaptureCollection c) => c.IsSynchronized,

                        (global::System.Text.RegularExpressions.CaptureCollection c, int i) => c[i],
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Capture" />.
                    /// </summary>
                    public static MemberTable Capture { get; } = new MemberTable
                    {
                        (global::System.Text.RegularExpressions.Capture c) => c.Index,
                        (global::System.Text.RegularExpressions.Capture c) => c.Length,
                        (global::System.Text.RegularExpressions.Capture c) => c.Value,

                        (global::System.Text.RegularExpressions.Capture c) => c.ToString(),
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members in the System.Text.RegularExpressions namespace.
                    /// </summary>
                    public static MemberTable AllThisNamespaceOnly { get; } = new MemberTable
                    {
                        Regex,
                        MatchCollection,
                        Match,
                        GroupCollection,
                        Group,
                        CaptureCollection,
                        Capture,
                    }.ToReadOnly();

                    /// <summary>
                    /// Gets a table of pure members in the System.Text.RegularExpressions namespace and any child namespace.
                    /// </summary>
                    public static MemberTable AllThisAndChildNamespaces { get; } = new MemberTable
                    {
                        AllThisNamespaceOnly,
                    }.ToReadOnly();
                }

                /// <summary>
                /// Gets a table of pure members in the System.Text namespace.
                /// </summary>
                public static MemberTable AllThisNamespaceOnly { get; } = new MemberTable
                {
                }.ToReadOnly();

                /// <summary>
                /// Gets a table of pure members in the System.Text namespace and any child namespace.
                /// </summary>
                public static MemberTable AllThisAndChildNamespaces { get; } = new MemberTable
                {
                    AllThisNamespaceOnly,

                    RegularExpressions.AllThisAndChildNamespaces,
                }.ToReadOnly();
            }

            /// <summary>
            /// Gets a table of pure members in the System namespace.
            /// </summary>
            public static MemberTable AllThisNamespaceOnly { get; } = new MemberTable
            {
                Boolean,
                Char,
                SByte,
                Byte,
                Int16,
                UInt16,
                Int32,
                UInt32,
                Int64,
                UInt64,
                Single,
                Double,
                Decimal,
                String,
                DateTime,
                DateTimeOffset,
                Guid,
                TimeSpan,
                Uri,
                Version,
                Nullable,
                Tuple,
                ValueTuple,
                Math,
                BitConverter,
                Convert,
                Array,
#if NET6_0
                Half,
#endif
#if NET6_0 || NETSTANDARD2_1
                Index,
                Range,
                MathF,
#endif
            }.ToReadOnly();

            /// <summary>
            /// Gets a table of pure members in the System namespace.
            /// </summary>
            public static MemberTable AllThisAndChildNamespaces { get; } = new MemberTable
            {
                AllThisNamespaceOnly,

                Collections.AllThisAndChildNamespaces,
                Text.AllThisAndChildNamespaces,
            }.ToReadOnly();
        }

        /// <summary>
        /// Gets a table of pure members in the Base Class Library.
        /// </summary>
        public static MemberTable All { get; } = new MemberTable
        {
            System.AllThisAndChildNamespaces,
        }.ToReadOnly();
    }

    [TypeWildcard]
    internal struct TValue { }
}
