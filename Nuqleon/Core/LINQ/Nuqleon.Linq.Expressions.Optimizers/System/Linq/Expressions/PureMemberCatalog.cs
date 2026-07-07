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
#pragma warning disable IDE0049 // Remove name can be simplified
#pragma warning disable CA1034 // Do not nest types.
#pragma warning disable CA1720 // Identifier X contains type name.
#pragma warning disable CA1724 // Using System.* namespaces by design here.

// Misc. warnings related to usage of methods. These should not apply in expression trees.
#pragma warning disable CA1305
#pragma warning disable CA1307
#pragma warning disable CA1308
#pragma warning disable CA1309

namespace System.Linq.Expressions;

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
            MemberTable booleanMembers = new();
            booleanMembers.Add(() => bool.FalseString);
            booleanMembers.Add(() => bool.TrueString);
            booleanMembers.Add((bool i, bool j) => i.Equals(j));
            booleanMembers.Add((bool i, bool j) => i.CompareTo(j));
            Boolean = booleanMembers.ToReadOnly();

            MemberTable charMembers = new();
            charMembers.Add((string s) => char.Parse(s));
            charMembers.Add((char c) => char.ToString(c));
            charMembers.Add((char c) => char.IsDigit(c));
            charMembers.Add((char c) => char.IsLetter(c));
            charMembers.Add((char c) => char.IsWhiteSpace(c));
            charMembers.Add((char c) => char.IsUpper(c));
            charMembers.Add((char c) => char.IsLower(c));
            charMembers.Add((char c) => char.IsPunctuation(c));
            charMembers.Add((char c) => char.IsLetterOrDigit(c));
            charMembers.Add((char c) => char.ToUpperInvariant(c));
            charMembers.Add((char c) => char.ToLowerInvariant(c));
            charMembers.Add((char c) => char.IsControl(c));
            charMembers.Add((char c) => char.IsNumber(c));
            charMembers.Add((char c) => char.IsSeparator(c));
            charMembers.Add((char c) => char.IsSurrogate(c));
            charMembers.Add((char c) => char.IsSymbol(c));
            charMembers.Add((char c) => char.GetUnicodeCategory(c));
            charMembers.Add((char c) => char.GetNumericValue(c));
            charMembers.Add((char c) => char.IsHighSurrogate(c));
            charMembers.Add((char c) => char.IsLowSurrogate(c));
            charMembers.Add((int utf32) => char.ConvertFromUtf32(utf32));
            charMembers.Add((string s, int index) => char.IsControl(s, index));
            charMembers.Add((string s, int index) => char.IsDigit(s, index));
            charMembers.Add((string s, int index) => char.IsLetter(s, index));
            charMembers.Add((string s, int index) => char.IsLetterOrDigit(s, index));
            charMembers.Add((string s, int index) => char.IsLower(s, index));
            charMembers.Add((string s, int index) => char.IsNumber(s, index));
            charMembers.Add((string s, int index) => char.IsPunctuation(s, index));
            charMembers.Add((string s, int index) => char.IsSeparator(s, index));
            charMembers.Add((string s, int index) => char.IsSurrogate(s, index));
            charMembers.Add((string s, int index) => char.IsSymbol(s, index));
            charMembers.Add((string s, int index) => char.IsUpper(s, index));
            charMembers.Add((string s, int index) => char.IsWhiteSpace(s, index));
            charMembers.Add((string s, int index) => char.GetUnicodeCategory(s, index));
            charMembers.Add((string s, int index) => char.GetNumericValue(s, index));
            charMembers.Add((string s, int index) => char.IsHighSurrogate(s, index));
            charMembers.Add((string s, int index) => char.IsLowSurrogate(s, index));
            charMembers.Add((string s, int index) => char.IsSurrogatePair(s, index));
            charMembers.Add((string s, int index) => char.ConvertToUtf32(s, index));
            charMembers.Add((char highSurrogate, char lowSurrogate) => char.IsSurrogatePair(highSurrogate, lowSurrogate));
            charMembers.Add((char highSurrogate, char lowSurrogate) => char.ConvertToUtf32(highSurrogate, lowSurrogate));
            charMembers.Add((char i, char j) => i.CompareTo(j));
            charMembers.Add((char i, char j) => i.Equals(j));
            charMembers.Add((char c) => c.ToString());
            Char = charMembers.ToReadOnly();

            MemberTable sByteMembers = new();
            sByteMembers.Add((sbyte i, sbyte j) => i.CompareTo(j));
            sByteMembers.Add((sbyte i, sbyte j) => i.Equals(j));
            SByte = sByteMembers.ToReadOnly();

            MemberTable byteMembers = new();
            byteMembers.Add((byte i, byte j) => i.CompareTo(j));
            byteMembers.Add((byte i, byte j) => i.Equals(j));
            Byte = byteMembers.ToReadOnly();

            MemberTable int16Members = new();
            int16Members.Add((short i, short j) => i.CompareTo(j));
            int16Members.Add((short i, short j) => i.Equals(j));
            Int16 = int16Members.ToReadOnly();

            MemberTable uInt16Members = new();
            uInt16Members.Add((ushort i, ushort j) => i.CompareTo(j));
            uInt16Members.Add((ushort i, ushort j) => i.Equals(j));
            UInt16 = uInt16Members.ToReadOnly();

            MemberTable int32Members = new();
            int32Members.Add((int i, int j) => i.CompareTo(j));
            int32Members.Add((int i, int j) => i.Equals(j));
            Int32 = int32Members.ToReadOnly();

            MemberTable uInt32Members = new();
            uInt32Members.Add((uint i, uint j) => i.CompareTo(j));
            uInt32Members.Add((uint i, uint j) => i.Equals(j));
            UInt32 = uInt32Members.ToReadOnly();

            MemberTable int64Members = new();
            int64Members.Add((long i, long j) => i.CompareTo(j));
            int64Members.Add((long i, long j) => i.Equals(j));
            Int64 = int64Members.ToReadOnly();

            MemberTable uInt64Members = new();
            uInt64Members.Add((ulong i, ulong j) => i.CompareTo(j));
            uInt64Members.Add((ulong i, ulong j) => i.Equals(j));
            UInt64 = uInt64Members.ToReadOnly();

            MemberTable halfMembers = new();
            halfMembers.Add(() => global::System.Half.Epsilon);
            halfMembers.Add(() => global::System.Half.MaxValue);
            halfMembers.Add(() => global::System.Half.MinValue);
            halfMembers.Add(() => global::System.Half.NaN);
            halfMembers.Add(() => global::System.Half.NegativeInfinity);
            halfMembers.Add(() => global::System.Half.PositiveInfinity);

            halfMembers.Add((global::System.Half i) => global::System.Half.IsFinite(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsInfinity(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsNaN(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsNegative(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsNegativeInfinity(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsNormal(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsPositiveInfinity(i));
            halfMembers.Add((global::System.Half i) => global::System.Half.IsSubnormal(i));

            halfMembers.Add((global::System.Half i, global::System.Half j) => i.CompareTo(j));
            halfMembers.Add((global::System.Half i, global::System.Half j) => i.Equals(j));

            halfMembers.Add((global::System.Half i) => (float)i);
            halfMembers.Add((global::System.Half i) => (double)i);
            halfMembers.Add((float i) => (global::System.Half)i);
            halfMembers.Add((double i) => (global::System.Half)i);

            halfMembers.Add((global::System.Half i, global::System.Half j) => i == j);
            halfMembers.Add((global::System.Half i, global::System.Half j) => i != j);
            halfMembers.Add((global::System.Half i, global::System.Half j) => i < j);
            halfMembers.Add((global::System.Half i, global::System.Half j) => i <= j);
            halfMembers.Add((global::System.Half i, global::System.Half j) => i > j);
            halfMembers.Add((global::System.Half i, global::System.Half j) => i >= j);
            Half = halfMembers.ToReadOnly();

            MemberTable singleMembers = new();
            singleMembers.Add((float i) => float.IsInfinity(i));
            singleMembers.Add((float i) => float.IsNaN(i));
            singleMembers.Add((float i) => float.IsNegativeInfinity(i));
            singleMembers.Add((float i) => float.IsPositiveInfinity(i));

            singleMembers.Add((float i, float j) => i.CompareTo(j));
            singleMembers.Add((float i, float j) => i.Equals(j));

            singleMembers.Add((float i) => float.IsFinite(i));
            singleMembers.Add((float i) => float.IsNegative(i));
            singleMembers.Add((float i) => float.IsNormal(i));
            singleMembers.Add((float i) => float.IsSubnormal(i));
            Single = singleMembers.ToReadOnly();

            MemberTable doubleMembers = new();
            doubleMembers.Add((double i) => double.IsInfinity(i));
            doubleMembers.Add((double i) => double.IsNaN(i));
            doubleMembers.Add((double i) => double.IsNegativeInfinity(i));
            doubleMembers.Add((double i) => double.IsPositiveInfinity(i));

            doubleMembers.Add((double i, double j) => i.CompareTo(j));
            doubleMembers.Add((double i, double j) => i.Equals(j));

            doubleMembers.Add((double i) => double.IsFinite(i));
            doubleMembers.Add((double i) => double.IsNegative(i));
            doubleMembers.Add((double i) => double.IsNormal(i));
            doubleMembers.Add((double i) => double.IsSubnormal(i));
            Double = doubleMembers.ToReadOnly();

            MemberTable decimalMembers = new();
            decimalMembers.Add((decimal i) => decimal.Ceiling(i));
            decimalMembers.Add((decimal i) => decimal.Floor(i));
            decimalMembers.Add((decimal i) => decimal.Negate(i));
            decimalMembers.Add((decimal i) => decimal.Round(i));
            decimalMembers.Add((decimal i) => decimal.ToByte(i));
            decimalMembers.Add((decimal i) => decimal.ToDouble(i));
            decimalMembers.Add((decimal i) => decimal.ToInt16(i));
            decimalMembers.Add((decimal i) => decimal.ToInt32(i));
            decimalMembers.Add((decimal i) => decimal.ToInt64(i));
            decimalMembers.Add((decimal i) => decimal.ToSByte(i));
            decimalMembers.Add((decimal i) => decimal.ToSingle(i));
            decimalMembers.Add((decimal i) => decimal.ToUInt16(i));
            decimalMembers.Add((decimal i) => decimal.ToUInt32(i));
            decimalMembers.Add((decimal i) => decimal.ToUInt64(i));
            decimalMembers.Add((decimal i) => decimal.ToOACurrency(i));
            decimalMembers.Add((decimal i, decimal j) => decimal.Add(i, j));
            decimalMembers.Add((decimal i, decimal j) => decimal.Compare(i, j));
            decimalMembers.Add((decimal i, decimal j) => decimal.Divide(i, j));
            decimalMembers.Add((decimal i, decimal j) => decimal.Equals(i, j));
            decimalMembers.Add((decimal i, decimal j) => decimal.Multiply(i, j));
            decimalMembers.Add((decimal i, decimal j) => decimal.Remainder(i, j));
            decimalMembers.Add((decimal i, decimal j) => decimal.Subtract(i, j));

            decimalMembers.Add((sbyte i) => (decimal)i);
            decimalMembers.Add((byte i) => (decimal)i);
            decimalMembers.Add((short i) => (decimal)i);
            decimalMembers.Add((ushort i) => (decimal)i);
            decimalMembers.Add((int i) => (decimal)i);
            decimalMembers.Add((uint i) => (decimal)i);
            decimalMembers.Add((long i) => (decimal)i);
            decimalMembers.Add((ulong i) => (decimal)i);
            decimalMembers.Add((char i) => (decimal)i);
            decimalMembers.Add((float i) => (decimal)i);
            decimalMembers.Add((double i) => (decimal)i);

            decimalMembers.Add((decimal i) => -i);
            decimalMembers.Add((decimal i) => (sbyte)i);
            decimalMembers.Add((decimal i) => (byte)i);
            decimalMembers.Add((decimal i) => (short)i);
            decimalMembers.Add((decimal i) => (ushort)i);
            decimalMembers.Add((decimal i) => (int)i);
            decimalMembers.Add((decimal i) => (uint)i);
            decimalMembers.Add((decimal i) => (long)i);
            decimalMembers.Add((decimal i) => (ulong)i);
            decimalMembers.Add((decimal i) => (char)i);
            decimalMembers.Add((decimal i) => (float)i);
            decimalMembers.Add((decimal i) => (double)i);

            decimalMembers.Add((decimal i, decimal j) => i + j);
            decimalMembers.Add((decimal i, decimal j) => i - j);
            decimalMembers.Add((decimal i, decimal j) => i * j);
            decimalMembers.Add((decimal i, decimal j) => i / j);
            decimalMembers.Add((decimal i, decimal j) => i % j);
            decimalMembers.Add((decimal i, decimal j) => i == j);
            decimalMembers.Add((decimal i, decimal j) => i != j);
            decimalMembers.Add((decimal i, decimal j) => i < j);
            decimalMembers.Add((decimal i, decimal j) => i <= j);
            decimalMembers.Add((decimal i, decimal j) => i > j);
            decimalMembers.Add((decimal i, decimal j) => i >= j);

            decimalMembers.Add((decimal i, decimal j) => i.CompareTo(j));
            decimalMembers.Add((decimal i, decimal j) => i.Equals(j));
            Decimal = decimalMembers.ToReadOnly();

            MemberTable stringMembers = new();
            stringMembers.Add((char[] value) => new string(value));
            stringMembers.Add((char[] value, int startIndex, int length) => new string(value, startIndex, length));
            stringMembers.Add((char c, int count) => new string(c, count));

            stringMembers.Add(() => string.Empty);
            stringMembers.Add((string s) => s.Length);
            stringMembers.Add((string s, int index) => s[index]);

            stringMembers.Add((string value) => string.IsNullOrEmpty(value));
            stringMembers.Add((string value) => string.IsNullOrWhiteSpace(value));

            stringMembers.Add((string strA, string strB) => string.CompareOrdinal(strA, strB));
            stringMembers.Add((string strA, int indexA, string strB, int indexB, int length) => string.CompareOrdinal(strA, indexA, strB, indexB, length));

            stringMembers.Add((string str0, string str1) => string.Concat(str0, str1));
            stringMembers.Add((string str0, string str1, string str2) => string.Concat(str0, str1, str2));
            stringMembers.Add((string str0, string str1, string str2, string str3) => string.Concat(str0, str1, str2, str3));
            stringMembers.Add((string[] values) => string.Concat(values));

            stringMembers.Add((string s, string value) => s.Contains(value)); // NB: This uses ordinal comparison

            stringMembers.Add((string s, string value) => s.Equals(value));
            stringMembers.Add((string a, string b) => string.Equals(a, b));

            stringMembers.Add((string s, char value) => s.IndexOf(value));
            stringMembers.Add((string s, char value, int startIndex) => s.IndexOf(value, startIndex));
            stringMembers.Add((string s, char value, int startIndex, int count) => s.IndexOf(value, startIndex, count));

            stringMembers.Add((string s, char[] anyOf) => s.IndexOfAny(anyOf));
            stringMembers.Add((string s, char[] anyOf, int startIndex) => s.IndexOfAny(anyOf, startIndex));
            stringMembers.Add((string s, char[] anyOf, int startIndex, int count) => s.IndexOfAny(anyOf, startIndex, count));

            stringMembers.Add((string s, char value) => s.LastIndexOf(value));
            stringMembers.Add((string s, char value, int startIndex) => s.LastIndexOf(value, startIndex));
            stringMembers.Add((string s, char value, int startIndex, int count) => s.LastIndexOf(value, startIndex, count));

            stringMembers.Add((string s, char[] anyOf) => s.LastIndexOfAny(anyOf));
            stringMembers.Add((string s, char[] anyOf, int startIndex) => s.LastIndexOfAny(anyOf, startIndex));
            stringMembers.Add((string s, char[] anyOf, int startIndex, int count) => s.LastIndexOfAny(anyOf, startIndex, count));

            stringMembers.Add((string s, int startIndex, string value) => s.Insert(startIndex, value));

            stringMembers.Add((string s) => s.IsNormalized());
            stringMembers.Add((string s) => s.Normalize());
            stringMembers.Add((string s, global::System.Text.NormalizationForm normalizationForm) => s.Normalize(normalizationForm));
            stringMembers.Add((string s, global::System.Text.NormalizationForm normalizationForm) => s.IsNormalized(normalizationForm));

            stringMembers.Add((string separator, string[] value) => string.Join(separator, value));
            stringMembers.Add((string separator, string[] value, int startIndex, int count) => string.Join(separator, value, startIndex, count));

            stringMembers.Add((string s, int totalWidth) => s.PadLeft(totalWidth));
            stringMembers.Add((string s, int totalWidth) => s.PadRight(totalWidth));
            stringMembers.Add((string s, int totalWidth, char paddingChar) => s.PadLeft(totalWidth, paddingChar));
            stringMembers.Add((string s, int totalWidth, char paddingChar) => s.PadRight(totalWidth, paddingChar));

            stringMembers.Add((string s, int startIndex) => s.Remove(startIndex));
            stringMembers.Add((string s, int startIndex, int count) => s.Remove(startIndex, count));

            stringMembers.Add((string s, char oldChar, char newChar) => s.Replace(oldChar, newChar));
            stringMembers.Add((string s, string oldValue, string newValue) => s.Replace(oldValue, newValue));

            // NB: Omitting Split which returns a mutable array.

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable IDE0057 // Substring can be simplified (not in expression trees)
            stringMembers.Add((string s, int startIndex) => s.Substring(startIndex));
            stringMembers.Add((string s, int startIndex, int length) => s.Substring(startIndex, length));
#pragma warning restore IDE0057
#pragma warning restore IDE0079

            // NB: Omitting ToCharArray which returns a mutable array.

            stringMembers.Add((string s) => s.ToLowerInvariant());
            stringMembers.Add((string s) => s.ToUpperInvariant());

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with System.Object here.
            //(string s) => s.ToString(),

            // REVIEW: Should we add format providers sample values to tests just for this?
            //(string s, IFormatProvider provider) => s.ToString(provider /* NB: this parameter is always ignored */),

            stringMembers.Add((string s) => s.Trim());
            stringMembers.Add((string s, char[] trimChars) => s.Trim(trimChars));
            stringMembers.Add((string s, char[] trimChars) => s.TrimEnd(trimChars));
            stringMembers.Add((string s, char[] trimChars) => s.TrimStart(trimChars));

            stringMembers.Add((string s1, string s2) => s1 == s2);
            stringMembers.Add((string s1, string s2) => s1 != s2);

            stringMembers.Add((string s, char c) => s.Contains(c));
            stringMembers.Add((string s, char c) => s.StartsWith(c));
            stringMembers.Add((string s, char c) => s.EndsWith(c));
            String = stringMembers.ToReadOnly();

            MemberTable dateTimeMembers = new();
            dateTimeMembers.Add((long ticks) => new global::System.DateTime(ticks));
            dateTimeMembers.Add((long ticks, DateTimeKind kind) => new global::System.DateTime(ticks, kind));
            dateTimeMembers.Add((int year, int month, int day) => new global::System.DateTime(year, month, day));
            dateTimeMembers.Add((int year, int month, int day, int hour, int minute, int second) => new global::System.DateTime(year, month, day, hour, minute, second));
            dateTimeMembers.Add((int year, int month, int day, int hour, int minute, int second, DateTimeKind kind) => new global::System.DateTime(year, month, day, hour, minute, second, kind));
            dateTimeMembers.Add((int year, int month, int day, int hour, int minute, int second, int millisecond) => new global::System.DateTime(year, month, day, hour, minute, second, millisecond));
            dateTimeMembers.Add((int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind) => new global::System.DateTime(year, month, day, hour, minute, second, millisecond, kind));

            dateTimeMembers.Add(() => global::System.DateTime.MaxValue);
            dateTimeMembers.Add(() => global::System.DateTime.MinValue);

            dateTimeMembers.Add((global::System.DateTime dt) => dt.Date);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Day);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.DayOfWeek);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.DayOfYear);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Hour);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Kind);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Millisecond);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Minute);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Month);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Second);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Ticks);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.TimeOfDay);
            dateTimeMembers.Add((global::System.DateTime dt) => dt.Year);

            dateTimeMembers.Add((global::System.DateTime t1, global::System.DateTime t2) => global::System.DateTime.Compare(t1, t2));
            dateTimeMembers.Add((global::System.DateTime t1, global::System.DateTime t2) => global::System.DateTime.Equals(t1, t2));

            dateTimeMembers.Add((double d) => global::System.DateTime.FromOADate(d));
            dateTimeMembers.Add((long fileTime) => global::System.DateTime.FromFileTimeUtc(fileTime));
            dateTimeMembers.Add((int year) => global::System.DateTime.IsLeapYear(year));
            dateTimeMembers.Add((int year, int month) => global::System.DateTime.DaysInMonth(year, month));
            dateTimeMembers.Add((global::System.DateTime value, global::System.DateTimeKind kind) => global::System.DateTime.SpecifyKind(value, kind));

            dateTimeMembers.Add((global::System.DateTime dt) => dt.ToOADate());

            dateTimeMembers.Add((global::System.DateTime dt, global::System.TimeSpan value) => dt.Add(value));
            dateTimeMembers.Add((global::System.DateTime dt, double value) => dt.AddDays(value));
            dateTimeMembers.Add((global::System.DateTime dt, double value) => dt.AddHours(value));
            dateTimeMembers.Add((global::System.DateTime dt, double value) => dt.AddMilliseconds(value));
            dateTimeMembers.Add((global::System.DateTime dt, double value) => dt.AddMinutes(value));
            dateTimeMembers.Add((global::System.DateTime dt, int months) => dt.AddMonths(months));
            dateTimeMembers.Add((global::System.DateTime dt, double value) => dt.AddSeconds(value));
            dateTimeMembers.Add((global::System.DateTime dt, long value) => dt.AddTicks(value));
            dateTimeMembers.Add((global::System.DateTime dt, int value) => dt.AddYears(value));
            dateTimeMembers.Add((global::System.DateTime dt, global::System.DateTime value) => dt.Subtract(value));
            dateTimeMembers.Add((global::System.DateTime dt, global::System.TimeSpan value) => dt.Subtract(value));

            dateTimeMembers.Add((global::System.DateTime dt, global::System.DateTime value) => dt.CompareTo(value));
            dateTimeMembers.Add((global::System.DateTime dt, global::System.DateTime value) => dt.Equals(value));

            dateTimeMembers.Add((global::System.DateTime dt, global::System.TimeSpan ts) => dt + ts);
            dateTimeMembers.Add((global::System.DateTime dt, global::System.TimeSpan ts) => dt - ts);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 - dt2);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 == dt2);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 != dt2);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 < dt2);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 <= dt2);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 > dt2);
            dateTimeMembers.Add((global::System.DateTime dt1, global::System.DateTime dt2) => dt1 >= dt2);
            DateTime = dateTimeMembers.ToReadOnly();

            MemberTable dateTimeOffsetMembers = new();
            dateTimeOffsetMembers.Add((long ticks, global::System.TimeSpan offset) => new global::System.DateTimeOffset(ticks, offset));
            dateTimeOffsetMembers.Add((int year, int month, int day, int hour, int minute, int second, global::System.TimeSpan offset) => new global::System.DateTimeOffset(year, month, day, hour, minute, second, offset));
            dateTimeOffsetMembers.Add((int year, int month, int day, int hour, int minute, int second, int millisecond, global::System.TimeSpan offset) => new global::System.DateTimeOffset(year, month, day, hour, minute, second, millisecond, offset));

            dateTimeOffsetMembers.Add(() => global::System.DateTimeOffset.MaxValue);
            dateTimeOffsetMembers.Add(() => global::System.DateTimeOffset.MinValue);

            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Date);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.DateTime);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Day);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.DayOfWeek);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.DayOfYear);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Hour);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.LocalDateTime);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Millisecond);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Minute);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Month);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Offset);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Second);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Ticks);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.TimeOfDay);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.UtcDateTime);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.UtcTicks);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.Year);

            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto.Add(ts));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, double days) => dto.AddDays(days));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, double hours) => dto.AddHours(hours));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, double milliseconds) => dto.AddMilliseconds(milliseconds));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, double minutes) => dto.AddMinutes(minutes));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, int months) => dto.AddMonths(months));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, double seconds) => dto.AddSeconds(seconds));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, long ticks) => dto.AddTicks(ticks));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, int years) => dto.AddYears(years));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.DateTimeOffset value) => dto.Subtract(value));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.TimeSpan value) => dto.Subtract(value));

            dateTimeOffsetMembers.Add((long fileTime) => global::System.DateTimeOffset.FromFileTime(fileTime));

            dateTimeOffsetMembers.Add((global::System.DateTimeOffset first, global::System.DateTimeOffset second) => global::System.DateTimeOffset.Compare(first, second));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset first, global::System.DateTimeOffset second) => global::System.DateTimeOffset.Equals(first, second));

            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.ToFileTime());
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.ToUniversalTime());
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.TimeSpan offset) => dto.ToOffset(offset));

            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.CompareTo(other));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.Equals(other));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.EqualsExact(other));

            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto + ts);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto - ts);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 - dto2);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 == dto2);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 != dto2);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 < dto2);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 <= dto2);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 > dto2);
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 >= dto2);

            dateTimeOffsetMembers.Add((long seconds) => global::System.DateTimeOffset.FromUnixTimeSeconds(seconds));
            dateTimeOffsetMembers.Add((long milliseconds) => global::System.DateTimeOffset.FromUnixTimeMilliseconds(milliseconds));
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.ToUnixTimeSeconds());
            dateTimeOffsetMembers.Add((global::System.DateTimeOffset dto) => dto.ToUnixTimeMilliseconds());
            DateTimeOffset = dateTimeOffsetMembers.ToReadOnly();

            MemberTable timeSpanMembers = new();
            timeSpanMembers.Add((long ticks) => new global::System.TimeSpan(ticks));
            timeSpanMembers.Add((int hours, int minutes, int seconds) => new global::System.TimeSpan(hours, minutes, seconds));
            timeSpanMembers.Add((int days, int hours, int minutes, int seconds) => new global::System.TimeSpan(days, hours, minutes, seconds));
            timeSpanMembers.Add((int days, int hours, int minutes, int seconds, int milliseconds) => new global::System.TimeSpan(days, hours, minutes, seconds, milliseconds));

            timeSpanMembers.Add(() => global::System.TimeSpan.MaxValue);
            timeSpanMembers.Add(() => global::System.TimeSpan.MinValue);
            timeSpanMembers.Add(() => global::System.TimeSpan.TicksPerDay);
            timeSpanMembers.Add(() => global::System.TimeSpan.TicksPerHour);
            timeSpanMembers.Add(() => global::System.TimeSpan.TicksPerMillisecond);
            timeSpanMembers.Add(() => global::System.TimeSpan.TicksPerMinute);
            timeSpanMembers.Add(() => global::System.TimeSpan.TicksPerSecond);
            timeSpanMembers.Add(() => global::System.TimeSpan.Zero);

            timeSpanMembers.Add((global::System.TimeSpan t) => t.Days);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.Hours);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.Milliseconds);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.Minutes);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.Seconds);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.Ticks);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.TotalDays);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.TotalHours);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.TotalMilliseconds);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.TotalMinutes);
            timeSpanMembers.Add((global::System.TimeSpan t) => t.TotalSeconds);

            timeSpanMembers.Add((long value) => global::System.TimeSpan.FromTicks(value));
            timeSpanMembers.Add((double value) => global::System.TimeSpan.FromDays(value));
            timeSpanMembers.Add((double value) => global::System.TimeSpan.FromHours(value));
            timeSpanMembers.Add((double value) => global::System.TimeSpan.FromMilliseconds(value));
            timeSpanMembers.Add((double value) => global::System.TimeSpan.FromMinutes(value));
            timeSpanMembers.Add((double value) => global::System.TimeSpan.FromSeconds(value));

            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => global::System.TimeSpan.Compare(t1, t2));
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => global::System.TimeSpan.Equals(t1, t2));

            timeSpanMembers.Add((global::System.TimeSpan t) => t.Duration());
            timeSpanMembers.Add((global::System.TimeSpan t) => t.Negate());
            timeSpanMembers.Add((global::System.TimeSpan t, global::System.TimeSpan ts) => t.Add(ts));
            timeSpanMembers.Add((global::System.TimeSpan t, global::System.TimeSpan ts) => t.Subtract(ts));

            timeSpanMembers.Add((global::System.TimeSpan t, global::System.TimeSpan value) => t.CompareTo(value));
            timeSpanMembers.Add((global::System.TimeSpan t, global::System.TimeSpan obj) => t.Equals(obj));

            timeSpanMembers.Add((global::System.TimeSpan t) => -t);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 + t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 - t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 == t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 != t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 < t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 <= t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 > t2);
            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 >= t2);

            timeSpanMembers.Add((global::System.TimeSpan t, double divisor) => t.Divide(divisor));
            timeSpanMembers.Add((global::System.TimeSpan t, global::System.TimeSpan ts) => t.Divide(ts));
            timeSpanMembers.Add((global::System.TimeSpan t, double factor) => t.Multiply(factor));

            timeSpanMembers.Add((global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 / t2);
            timeSpanMembers.Add((global::System.TimeSpan timeSpan, double divisor) => timeSpan / divisor);
            timeSpanMembers.Add((double factor, global::System.TimeSpan timeSpan) => factor * timeSpan);
            timeSpanMembers.Add((global::System.TimeSpan timeSpan, double factor) => timeSpan * factor);
            TimeSpan = timeSpanMembers.ToReadOnly();

            MemberTable guidMembers = new();
            guidMembers.Add((string input) => global::System.Guid.Parse(input));
            guidMembers.Add((string input, string format) => global::System.Guid.ParseExact(input, format));

            guidMembers.Add((byte[] b) => new global::System.Guid(b));
            guidMembers.Add((string g) => new global::System.Guid(g));
            guidMembers.Add((uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) => new global::System.Guid(a, b, c, d, e, f, g, h, i, j, k));
            guidMembers.Add((int a, short b, short c, byte[] d) => new global::System.Guid(a, b, c, d));
            guidMembers.Add((int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) => new global::System.Guid(a, b, c, d, e, f, g, h, i, j, k));

            guidMembers.Add(() => global::System.Guid.Empty);

            guidMembers.Add((global::System.Guid g, global::System.Guid value) => g.CompareTo(value));
            guidMembers.Add((global::System.Guid g, global::System.Guid value) => g.Equals(value));

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
            //(global::System.Guid g) => g.ToString(),
            //(global::System.Guid g, string format) => g.ToString(format),

            // REVIEW: Should we add format providers sample values to tests just for this?
            //(global::System.Guid g, string format, IFormatProvider provider) => s.ToString(format, provider /* NB: this parameter is always ignored */),
            Guid = guidMembers.ToReadOnly();

            MemberTable uriMembers = new();
            uriMembers.Add((string uriString) => new Uri(uriString));
            uriMembers.Add((string uriString, UriKind uriKind) => new Uri(uriString, uriKind));
            uriMembers.Add((Uri baseUri, string relativeUri) => new Uri(baseUri, relativeUri));
            uriMembers.Add((Uri baseUri, Uri relativeUri) => new Uri(baseUri, relativeUri));

            uriMembers.Add((string uriString) => new global::System.Uri(uriString));
            uriMembers.Add((string uriString, global::System.UriKind uriKind) => new global::System.Uri(uriString, uriKind));
            uriMembers.Add((global::System.Uri baseUri, string relativeUri) => new global::System.Uri(baseUri, relativeUri));
            uriMembers.Add((global::System.Uri baseUri, global::System.Uri relativeUri) => new global::System.Uri(baseUri, relativeUri));

            uriMembers.Add(() => global::System.Uri.SchemeDelimiter);
            uriMembers.Add(() => global::System.Uri.UriSchemeFile);
            uriMembers.Add(() => global::System.Uri.UriSchemeFtp);
            uriMembers.Add(() => global::System.Uri.UriSchemeGopher);
            uriMembers.Add(() => global::System.Uri.UriSchemeHttp);
            uriMembers.Add(() => global::System.Uri.UriSchemeHttps);
            uriMembers.Add(() => global::System.Uri.UriSchemeMailto);
            uriMembers.Add(() => global::System.Uri.UriSchemeNetPipe);
            uriMembers.Add(() => global::System.Uri.UriSchemeNetTcp);
            uriMembers.Add(() => global::System.Uri.UriSchemeNews);
            uriMembers.Add(() => global::System.Uri.UriSchemeNntp);

            uriMembers.Add((global::System.Uri u) => u.AbsolutePath);
            uriMembers.Add((global::System.Uri u) => u.AbsoluteUri);
            uriMembers.Add((global::System.Uri u) => u.Authority);
            uriMembers.Add((global::System.Uri u) => u.DnsSafeHost);
            uriMembers.Add((global::System.Uri u) => u.Fragment);
            uriMembers.Add((global::System.Uri u) => u.Host);
            uriMembers.Add((global::System.Uri u) => u.HostNameType);
            //uriMembers.Add((global::System.Uri u) => u.IdnHost); // REVIEW: Not accessible
            uriMembers.Add((global::System.Uri u) => u.IsAbsoluteUri);
            uriMembers.Add((global::System.Uri u) => u.IsDefaultPort);
            uriMembers.Add((global::System.Uri u) => u.IsFile);
            uriMembers.Add((global::System.Uri u) => u.IsLoopback);
            uriMembers.Add((global::System.Uri u) => u.IsUnc);
            uriMembers.Add((global::System.Uri u) => u.LocalPath);
            uriMembers.Add((global::System.Uri u) => u.OriginalString);
            uriMembers.Add((global::System.Uri u) => u.PathAndQuery);
            uriMembers.Add((global::System.Uri u) => u.Port);
            uriMembers.Add((global::System.Uri u) => u.Query);
            uriMembers.Add((global::System.Uri u) => u.Scheme);
            uriMembers.Add((global::System.Uri u) => u.UserEscaped);
            uriMembers.Add((global::System.Uri u) => u.UserInfo);

            uriMembers.Add((string name) => global::System.Uri.CheckHostName(name));
            uriMembers.Add((string schemeName) => global::System.Uri.CheckSchemeName(schemeName));
            uriMembers.Add((char digit) => global::System.Uri.FromHex(digit));
            uriMembers.Add((char character) => global::System.Uri.HexEscape(character));
            uriMembers.Add((char character) => global::System.Uri.IsHexDigit(character));
            uriMembers.Add((string pattern, int index) => global::System.Uri.IsHexEncoding(pattern, index));
            uriMembers.Add((string stringToEscape) => global::System.Uri.EscapeDataString(stringToEscape));
            uriMembers.Add((string stringToUnescape) => global::System.Uri.UnescapeDataString(stringToUnescape));
            uriMembers.Add((string uriString, global::System.UriKind uriKind) => global::System.Uri.IsWellFormedUriString(uriString, uriKind));

            uriMembers.Add((global::System.Uri u1, global::System.Uri u2) => u1 == u2);
            uriMembers.Add((global::System.Uri u1, global::System.Uri u2) => u1 != u2);

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
            //(global::System.Uri u) => u.ToString(),
            uriMembers.Add((global::System.Uri u, global::System.UriPartial part) => u.GetLeftPart(part));
            uriMembers.Add((global::System.Uri u, global::System.Uri uri) => u.IsBaseOf(uri));
            uriMembers.Add((global::System.Uri u) => u.IsWellFormedOriginalString());
            uriMembers.Add((global::System.Uri u, global::System.Uri uri) => u.MakeRelativeUri(uri));
            uriMembers.Add((global::System.Uri u, global::System.UriComponents components, global::System.UriFormat format) => u.GetComponents(components, format));
            Uri = uriMembers.ToReadOnly();

            MemberTable versionMembers = new();
            versionMembers.Add(() => new global::System.Version());
            versionMembers.Add((int major, int minor) => new global::System.Version(major, minor));
            versionMembers.Add((int major, int minor, int build) => new global::System.Version(major, minor, build));
            versionMembers.Add((int major, int minor, int build, int revision) => new global::System.Version(major, minor, build, revision));
            versionMembers.Add((string version) => new global::System.Version(version));

            versionMembers.Add((global::System.Version v) => v.Build);
            versionMembers.Add((global::System.Version v) => v.Major);
            versionMembers.Add((global::System.Version v) => v.MajorRevision);
            versionMembers.Add((global::System.Version v) => v.Minor);
            versionMembers.Add((global::System.Version v) => v.MinorRevision);
            versionMembers.Add((global::System.Version v) => v.Revision);

            versionMembers.Add((string input) => global::System.Version.Parse(input));

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
            //(global::System.Version v) => v.ToString(),
            versionMembers.Add((global::System.Version v, int fieldCount) => v.ToString(fieldCount));
            versionMembers.Add((global::System.Version v, global::System.Version value) => v.CompareTo(value));
            versionMembers.Add((global::System.Version v, global::System.Version obj) => v.Equals(obj));

            versionMembers.Add((global::System.Version v1, global::System.Version v2) => v1 == v2);
            versionMembers.Add((global::System.Version v1, global::System.Version v2) => v1 != v2);
            versionMembers.Add((global::System.Version v1, global::System.Version v2) => v1 < v2);
            versionMembers.Add((global::System.Version v1, global::System.Version v2) => v1 <= v2);
            versionMembers.Add((global::System.Version v1, global::System.Version v2) => v1 > v2);
            versionMembers.Add((global::System.Version v1, global::System.Version v2) => v1 >= v2);
            Version = versionMembers.ToReadOnly();

            MemberTable nullableMembers = new();
            nullableMembers.Add((Nullable<TValue> n) => n.HasValue);
            nullableMembers.Add((Nullable<TValue> n) => n.Value);

            nullableMembers.Add((Nullable<TValue> n) => n.GetValueOrDefault());
            nullableMembers.Add((Nullable<TValue> n, TValue defaultValue) => n.GetValueOrDefault(defaultValue));
            Nullable = nullableMembers.ToReadOnly();

            MemberTable tupleMembers = new();
            //
            // global::System.Tuple<>
            //
            tupleMembers.Add((T t1) => global::System.Tuple.Create(t1));
            tupleMembers.Add((T t1) => new Tuple<T>(t1));
            tupleMembers.Add((Tuple<T> t) => t.Item1);

            //
            // global::System.Tuple<,>
            //
            tupleMembers.Add((T t1, T t2) => global::System.Tuple.Create(t1, t2));
            tupleMembers.Add((T t1, T t2) => new Tuple<T, T>(t1, t2));
            tupleMembers.Add((Tuple<T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T> t) => t.Item2);

            //
            // global::System.Tuple<,,>
            //
            tupleMembers.Add((T t1, T t2, T t3) => global::System.Tuple.Create(t1, t2, t3));
            tupleMembers.Add((T t1, T t2, T t3) => new Tuple<T, T, T>(t1, t2, t3));
            tupleMembers.Add((Tuple<T, T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T, T> t) => t.Item2);
            tupleMembers.Add((Tuple<T, T, T> t) => t.Item3);

            //
            // global::System.Tuple<,,,>
            //
            tupleMembers.Add((T t1, T t2, T t3, T t4) => global::System.Tuple.Create(t1, t2, t3, t4));
            tupleMembers.Add((T t1, T t2, T t3, T t4) => new Tuple<T, T, T, T>(t1, t2, t3, t4));
            tupleMembers.Add((Tuple<T, T, T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T, T, T> t) => t.Item2);
            tupleMembers.Add((Tuple<T, T, T, T> t) => t.Item3);
            tupleMembers.Add((Tuple<T, T, T, T> t) => t.Item4);

            //
            // global::System.Tuple<,,,,>
            //
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5) => global::System.Tuple.Create(t1, t2, t3, t4, t5));
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5) => new Tuple<T, T, T, T, T>(t1, t2, t3, t4, t5));
            tupleMembers.Add((Tuple<T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add((Tuple<T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add((Tuple<T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add((Tuple<T, T, T, T, T> t) => t.Item5);

            //
            // global::System.Tuple<,,,,,>
            //
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5, T t6) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6));
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5, T t6) => new Tuple<T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6));
            tupleMembers.Add((Tuple<T, T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add((Tuple<T, T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add((Tuple<T, T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add((Tuple<T, T, T, T, T, T> t) => t.Item5);
            tupleMembers.Add((Tuple<T, T, T, T, T, T> t) => t.Item6);

            //
            // global::System.Tuple<,,,,,,>
            //
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5, T t6, T t7) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6, t7));
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5, T t6, T t7) => new Tuple<T, T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6, t7));
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item5);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item6);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T> t) => t.Item7);

            //
            // global::System.Tuple<,,,,,,,>
            //
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5, T t6, T t7, T r) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6, t7, r));
            tupleMembers.Add((T t1, T t2, T t3, T t4, T t5, T t6, T t7, T r) => new Tuple<T, T, T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6, t7, r));
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item5);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item6);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Item7);
            tupleMembers.Add((Tuple<T, T, T, T, T, T, T, T> t) => t.Rest);
            Tuple = tupleMembers.ToReadOnly();

            MemberTable valueTupleMembers = new();
            // NB: Value tuples are mutable, so constructors and Create factory calls are not listed here.

            //
            // global::System.ValueTuple<>
            //
            valueTupleMembers.Add((ValueTuple<T> t) => t.Item1);

            //
            // global::System.ValueTuple<,>
            //
            valueTupleMembers.Add((ValueTuple<T, T> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T> t) => t.Item2);

            //
            // global::System.ValueTuple<,,>
            //
            valueTupleMembers.Add((ValueTuple<T, T, T> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T, T> t) => t.Item2);
            valueTupleMembers.Add((ValueTuple<T, T, T> t) => t.Item3);

            //
            // global::System.ValueTuple<,,,>
            //
            valueTupleMembers.Add((ValueTuple<T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add((ValueTuple<T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add((ValueTuple<T, T, T, T> t) => t.Item4);

            //
            // global::System.ValueTuple<,,,,>
            //
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T> t) => t.Item4);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T> t) => t.Item5);

            //
            // global::System.ValueTuple<,,,,,>
            //
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T> t) => t.Item4);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T> t) => t.Item5);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T> t) => t.Item6);

            //
            // global::System.ValueTuple<,,,,,,>
            //
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item4);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item5);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item6);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T> t) => t.Item7);

            //
            // global::System.ValueTuple<,,,,,,,>
            //
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item1);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item2);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item3);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item4);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item5);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item6);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item7);
            valueTupleMembers.Add((ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Rest);
            ValueTuple = valueTupleMembers.ToReadOnly();

            MemberTable mathMembers = new();
            mathMembers.Add(() => global::System.Math.E);
            mathMembers.Add(() => global::System.Math.PI);
            mathMembers.Add(() => global::System.Math.Tau);

            mathMembers.Add((double d) => global::System.Math.Acos(d));
            mathMembers.Add((double d) => global::System.Math.Asin(d));
            mathMembers.Add((double d) => global::System.Math.Atan(d));
            mathMembers.Add((double y, double x) => global::System.Math.Atan2(y, x));
            mathMembers.Add((decimal d) => global::System.Math.Ceiling(d));
            mathMembers.Add((double a) => global::System.Math.Ceiling(a));
            mathMembers.Add((double d) => global::System.Math.Cos(d));
            mathMembers.Add((double value) => global::System.Math.Cosh(value));
            mathMembers.Add((decimal d) => global::System.Math.Floor(d));
            mathMembers.Add((double d) => global::System.Math.Floor(d));
            mathMembers.Add((double a) => global::System.Math.Sin(a));
            mathMembers.Add((double a) => global::System.Math.Tan(a));
            mathMembers.Add((double value) => global::System.Math.Sinh(value));
            mathMembers.Add((double value) => global::System.Math.Tanh(value));
            mathMembers.Add((double a) => global::System.Math.Round(a));
            mathMembers.Add((double value, int digits) => global::System.Math.Round(value, digits));
            mathMembers.Add((double value, MidpointRounding mode) => global::System.Math.Round(value, mode));
            mathMembers.Add((double value, int digits, MidpointRounding mode) => global::System.Math.Round(value, digits, mode));
            mathMembers.Add((decimal d) => global::System.Math.Round(d));
            mathMembers.Add((decimal d, int decimals) => global::System.Math.Round(d, decimals));
            mathMembers.Add((decimal d, MidpointRounding mode) => global::System.Math.Round(d, mode));
            mathMembers.Add((decimal d, int decimals, MidpointRounding mode) => global::System.Math.Round(d, decimals, mode));
            mathMembers.Add((decimal d) => global::System.Math.Truncate(d));
            mathMembers.Add((double d) => global::System.Math.Truncate(d));
            mathMembers.Add((double d) => global::System.Math.Sqrt(d));
            mathMembers.Add((double d) => global::System.Math.Log(d));
            mathMembers.Add((double d) => global::System.Math.Log10(d));
            mathMembers.Add((double d) => global::System.Math.Exp(d));
            mathMembers.Add((double x, double y) => global::System.Math.Pow(x, y));
            mathMembers.Add((double x, double y) => global::System.Math.IEEERemainder(x, y));
            mathMembers.Add((sbyte value) => global::System.Math.Abs(value));
            mathMembers.Add((short value) => global::System.Math.Abs(value));
            mathMembers.Add((int value) => global::System.Math.Abs(value));
            mathMembers.Add((long value) => global::System.Math.Abs(value));
            mathMembers.Add((float value) => global::System.Math.Abs(value));
            mathMembers.Add((double value) => global::System.Math.Abs(value));
            mathMembers.Add((decimal value) => global::System.Math.Abs(value));
            mathMembers.Add((sbyte val1, sbyte val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((byte val1, byte val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((short val1, short val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((ushort val1, ushort val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((int val1, int val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((uint val1, uint val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((long val1, long val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((ulong val1, ulong val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((float val1, float val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((double val1, double val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((decimal val1, decimal val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add((sbyte val1, sbyte val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((byte val1, byte val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((short val1, short val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((ushort val1, ushort val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((int val1, int val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((uint val1, uint val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((long val1, long val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((ulong val1, ulong val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((float val1, float val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((double val1, double val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((decimal val1, decimal val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add((double a, double newBase) => global::System.Math.Log(a, newBase));
            mathMembers.Add((sbyte value) => global::System.Math.Sign(value));
            mathMembers.Add((short value) => global::System.Math.Sign(value));
            mathMembers.Add((int value) => global::System.Math.Sign(value));
            mathMembers.Add((long value) => global::System.Math.Sign(value));
            mathMembers.Add((float value) => global::System.Math.Sign(value));
            mathMembers.Add((double value) => global::System.Math.Sign(value));
            mathMembers.Add((decimal value) => global::System.Math.Sign(value));
            mathMembers.Add((int a, int b) => global::System.Math.BigMul(a, b));

            mathMembers.Add((double d) => global::System.Math.Acosh(d));
            mathMembers.Add((double d) => global::System.Math.Asinh(d));
            mathMembers.Add((double d) => global::System.Math.Atanh(d));
            mathMembers.Add((double d) => global::System.Math.Cbrt(d));
            mathMembers.Add((ulong value, ulong min, ulong max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((uint value, uint min, uint max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((ushort value, ushort min, ushort max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((byte value, byte min, byte max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((long value, long min, long max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((int value, int min, int max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((short value, short min, short max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((sbyte value, sbyte min, sbyte max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((double value, double min, double max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((float value, float min, float max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((decimal value, decimal min, decimal max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add((double x) => global::System.Math.BitDecrement(x));
            mathMembers.Add((double x) => global::System.Math.BitIncrement(x));
            mathMembers.Add((double x) => global::System.Math.ILogB(x));
            mathMembers.Add((double x) => global::System.Math.Log2(x));
            mathMembers.Add((double x, double y) => global::System.Math.CopySign(x, y));
            mathMembers.Add((double x, double y) => global::System.Math.MaxMagnitude(x, y));
            mathMembers.Add((double x, double y) => global::System.Math.MinMagnitude(x, y));
            mathMembers.Add((double x, int n) => global::System.Math.ScaleB(x, n));
            mathMembers.Add((double x, double y, double z) => global::System.Math.FusedMultiplyAdd(x, y, z));
            Math = mathMembers.ToReadOnly();

            MemberTable mathFMembers = new();
            mathFMembers.Add(() => global::System.MathF.E);
            mathFMembers.Add(() => global::System.MathF.PI);

            mathFMembers.Add((float x) => global::System.MathF.Abs(x));
            mathFMembers.Add((float x) => global::System.MathF.Acos(x));
            mathFMembers.Add((float x) => global::System.MathF.Acosh(x));
            mathFMembers.Add((float x) => global::System.MathF.Asin(x));
            mathFMembers.Add((float x) => global::System.MathF.Asinh(x));
            mathFMembers.Add((float x) => global::System.MathF.Atan(x));
            mathFMembers.Add((float y, float x) => global::System.MathF.Atan2(y, x));
            mathFMembers.Add((float x) => global::System.MathF.Atanh(x));
            mathFMembers.Add((float x) => global::System.MathF.Cbrt(x));
            mathFMembers.Add((float x) => global::System.MathF.Ceiling(x));
            mathFMembers.Add((float x) => global::System.MathF.Cos(x));
            mathFMembers.Add((float x) => global::System.MathF.Cosh(x));
            mathFMembers.Add((float x) => global::System.MathF.Exp(x));
            mathFMembers.Add((float x) => global::System.MathF.Floor(x));
            mathFMembers.Add((float x, float y) => global::System.MathF.IEEERemainder(x, y));
            mathFMembers.Add((float x) => global::System.MathF.Log(x));
            mathFMembers.Add((float x, float y) => global::System.MathF.Log(x, y));
            mathFMembers.Add((float x) => global::System.MathF.Log10(x));
            mathFMembers.Add((float x, float y) => global::System.MathF.Max(x, y));
            mathFMembers.Add((float x, float y) => global::System.MathF.Min(x, y));
            mathFMembers.Add((float x, float y) => global::System.MathF.Pow(x, y));
            mathFMembers.Add((float x) => global::System.MathF.Round(x));
            mathFMembers.Add((float x, int digits) => global::System.MathF.Round(x, digits));
            mathFMembers.Add((float x, MidpointRounding mode) => global::System.MathF.Round(x, mode));
            mathFMembers.Add((float x, int digits, MidpointRounding mode) => global::System.MathF.Round(x, digits, mode));
            mathFMembers.Add((float x) => global::System.MathF.Sign(x));
            mathFMembers.Add((float x) => global::System.MathF.Sin(x));
            mathFMembers.Add((float x) => global::System.MathF.Sinh(x));
            mathFMembers.Add((float x) => global::System.MathF.Sqrt(x));
            mathFMembers.Add((float x) => global::System.MathF.Tan(x));
            mathFMembers.Add((float x) => global::System.MathF.Tanh(x));
            mathFMembers.Add((float x) => global::System.MathF.Truncate(x));

            mathFMembers.Add(() => global::System.MathF.Tau);

            mathFMembers.Add((float x) => global::System.MathF.BitDecrement(x));
            mathFMembers.Add((float x) => global::System.MathF.BitIncrement(x));
            mathFMembers.Add((float x, float y) => global::System.MathF.CopySign(x, y));
            mathFMembers.Add((float x, float y, float z) => global::System.MathF.FusedMultiplyAdd(x, y, z));
            mathFMembers.Add((float x) => global::System.MathF.ILogB(x));
            mathFMembers.Add((float x) => global::System.MathF.Log2(x));
            mathFMembers.Add((float x, float y) => global::System.MathF.MaxMagnitude(x, y));
            mathFMembers.Add((float x, float y) => global::System.MathF.MinMagnitude(x, y));
            mathFMembers.Add((float x, int n) => global::System.MathF.ScaleB(x, n));
            MathF = mathFMembers.ToReadOnly();

            MemberTable bitConverterMembers = new();
            // NB: Omitting GetBytes overloads which return a mutable byte[].

            bitConverterMembers.Add((double value) => global::System.BitConverter.DoubleToInt64Bits(value));
            bitConverterMembers.Add((long value) => global::System.BitConverter.Int64BitsToDouble(value));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToBoolean(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToChar(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToDouble(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToInt16(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToInt32(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToInt64(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToSingle(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToUInt16(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToUInt32(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToUInt64(value, startIndex));
            bitConverterMembers.Add((byte[] value) => global::System.BitConverter.ToString(value));
            bitConverterMembers.Add((byte[] value, int startIndex) => global::System.BitConverter.ToString(value, startIndex));
            bitConverterMembers.Add((byte[] value, int startIndex, int length) => global::System.BitConverter.ToString(value, startIndex, length));

            bitConverterMembers.Add((float value) => global::System.BitConverter.SingleToInt32Bits(value));
            bitConverterMembers.Add((int value) => global::System.BitConverter.Int32BitsToSingle(value));
            BitConverter = bitConverterMembers.ToReadOnly();

            MemberTable convertMembers = new();
            convertMembers.Add((Object value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Char value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Single value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Double value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add((Object value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Char value) => global::System.Convert.ToChar(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToChar(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToChar(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToChar(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Single value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Double value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToChar(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToChar(value));
            convertMembers.Add((Object value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Char value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Single value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Double value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToSByte(value));
            convertMembers.Add((Object value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Char value) => global::System.Convert.ToByte(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToByte(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToByte(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToByte(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Single value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Double value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToByte(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToByte(value));
            convertMembers.Add((Object value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Char value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Single value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Double value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToInt16(value));
            convertMembers.Add((Object value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Char value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Single value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Double value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add((Object value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Char value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Single value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Double value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToInt32(value));
            convertMembers.Add((Object value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Char value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Single value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Double value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add((Object value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Char value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Single value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Double value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToInt64(value));
            convertMembers.Add((Object value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Char value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Single value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Double value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add((Object value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Char value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Single value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Double value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToSingle(value));
            convertMembers.Add((Object value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Char value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Single value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Double value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToDouble(value));
            convertMembers.Add((Object value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Char value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Single value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Double value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add((DateTime value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Object value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((SByte value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Byte value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Int16 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((UInt16 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Int32 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((UInt32 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Int64 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((UInt64 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Boolean value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Char value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Single value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Double value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((Decimal value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add((String value) => global::System.Convert.ToString(value));
            convertMembers.Add((byte[] inArray) => global::System.Convert.ToBase64String(inArray));
            convertMembers.Add((byte[] inArray, int offset, int length) => global::System.Convert.ToBase64String(inArray, offset, length));
            convertMembers.Add((byte[] inArray, int offset, int length, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, offset, length, options));
            convertMembers.Add((byte[] inArray, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, options));

            convertMembers.Add((byte[] inArray) => global::System.Convert.ToHexString(inArray));
            convertMembers.Add((byte[] inArray, int offset, int length) => global::System.Convert.ToHexString(inArray, offset, length));
            Convert = convertMembers.ToReadOnly();

            MemberTable arrayMembers = new();
            // NB: Array.Empty<T>() is omitted because the array return type is considered mutable.

            arrayMembers.Add((global::System.Array array) => array.IsFixedSize);
            arrayMembers.Add((global::System.Array array) => array.IsReadOnly);
            arrayMembers.Add((global::System.Array array) => array.IsSynchronized);
            arrayMembers.Add((global::System.Array array) => array.Length);
            arrayMembers.Add((global::System.Array array) => array.LongLength);
            arrayMembers.Add((global::System.Array array) => array.Rank);

            arrayMembers.Add((global::System.Array array, object value) => global::System.Array.BinarySearch(array, value));
            arrayMembers.Add((global::System.Array array, int index, int length, object value) => global::System.Array.BinarySearch(array, index, length, value));
            arrayMembers.Add((T[] array, T value) => global::System.Array.BinarySearch(array, value));
            arrayMembers.Add((T[] array, int index, int length, T value) => global::System.Array.BinarySearch(array, index, length, value));

            arrayMembers.Add((global::System.Array array, int dimension) => array.GetLength(dimension));
            arrayMembers.Add((global::System.Array array, int dimension) => array.GetLongLength(dimension));

            arrayMembers.Add((global::System.Array array, int dimension) => array.GetLowerBound(dimension));
            arrayMembers.Add((global::System.Array array, int dimension) => array.GetUpperBound(dimension));

            // NB: GetValue methods are omitted because these return global::System.Object which is not immutable.

            arrayMembers.Add((global::System.Array array, object value) => global::System.Array.IndexOf(array, value));
            arrayMembers.Add((global::System.Array array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex));
            arrayMembers.Add((global::System.Array array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count));
            arrayMembers.Add((T[] array, object value) => global::System.Array.IndexOf(array, value));
            arrayMembers.Add((T[] array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex));
            arrayMembers.Add((T[] array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count));

            arrayMembers.Add((global::System.Array array, object value) => global::System.Array.LastIndexOf(array, value));
            arrayMembers.Add((global::System.Array array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex));
            arrayMembers.Add((global::System.Array array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count));
            arrayMembers.Add((T[] array, object value) => global::System.Array.LastIndexOf(array, value));
            arrayMembers.Add((T[] array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex));
            arrayMembers.Add((T[] array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count));

            // NB: Methods with Predicate<T> parameters are ; the delegate may not be a pure function.
            // REVIEW: In practice, it is assumed that predicates are pure. Should we provide a table including these members, so users can opt-in?
            Array = arrayMembers.ToReadOnly();

            MemberTable indexMembers = new();
            indexMembers.Add(() => global::System.Index.End);
            indexMembers.Add(() => global::System.Index.Start);

            indexMembers.Add((global::System.Index i) => i.IsFromEnd);
            indexMembers.Add((global::System.Index i) => i.Value);

            indexMembers.Add((int value, bool fromEnd) => new global::System.Index(value, fromEnd));

            indexMembers.Add((int value) => (global::System.Index)value);

            indexMembers.Add((int value) => global::System.Index.FromEnd(value));
            indexMembers.Add((int value) => global::System.Index.FromStart(value));

            indexMembers.Add((global::System.Index i, int length) => i.GetOffset(length));

            indexMembers.Add((global::System.Index i, global::System.Index other) => i.Equals(other));
            Index = indexMembers.ToReadOnly();

            MemberTable rangeMembers = new();
            rangeMembers.Add(() => global::System.Range.All);

            rangeMembers.Add((global::System.Index start, global::System.Index end) => new global::System.Range(start, end));

            rangeMembers.Add((global::System.Index start) => global::System.Range.StartAt(start));
            rangeMembers.Add((global::System.Index end) => global::System.Range.EndAt(end));

            rangeMembers.Add((global::System.Range r) => r.End);
            rangeMembers.Add((global::System.Range r) => r.Start);

            // NB: GetOffsetAndLength is omitted because the ValueTuple<,> return type is not immutable.

            rangeMembers.Add((global::System.Range r, global::System.Range other) => r.Equals(other));
            Range = rangeMembers.ToReadOnly();
#pragma warning restore IDE0028 // Simplify collection initialization
        }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Boolean" />.
        /// </summary>
        public static MemberTable Boolean { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Char" />.
        /// </summary>
        public static MemberTable Char { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.SByte" />.
        /// </summary>
        public static MemberTable SByte { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Byte" />.
        /// </summary>
        public static MemberTable Byte { get; }


        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Int16" />.
        /// </summary>
        public static MemberTable Int16 { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.UInt16" />.
        /// </summary>
        public static MemberTable UInt16 { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Int32" />.
        /// </summary>
        public static MemberTable Int32 { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.UInt32" />.
        /// </summary>
        public static MemberTable UInt32 { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Int64" />.
        /// </summary>
        public static MemberTable Int64 { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.UInt64" />.
        /// </summary>
        public static MemberTable UInt64 { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Half" />.
        /// </summary>
        public static MemberTable Half { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Single" />.
        /// </summary>
        public static MemberTable Single { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Double" />.
        /// </summary>
        public static MemberTable Double { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Decimal" />.
        /// </summary>
        public static MemberTable Decimal { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.String" />.
        /// </summary>
        public static MemberTable String { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.DateTime" />.
        /// </summary>
        public static MemberTable DateTime { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="System.DateTimeOffset" />.
        /// </summary>
        public static MemberTable DateTimeOffset { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.TimeSpan" />.
        /// </summary>
        public static MemberTable TimeSpan { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Guid" />.
        /// </summary>
        public static MemberTable Guid { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Uri" />.
        /// </summary>
        public static MemberTable Uri { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Version" />.
        /// </summary>
        public static MemberTable Version { get; }

        /// <summary>
        /// Gets a table of pure members on nullable types.
        /// </summary>
        public static MemberTable Nullable { get; }

        /// <summary>
        /// Gets a table of pure members on tuple types.
        /// </summary>
        public static MemberTable Tuple { get; }

        /// <summary>
        /// Gets a table of pure members on value tuple types.
        /// </summary>
        public static MemberTable ValueTuple { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Math" />.
        /// </summary>
        public static MemberTable Math { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.MathF" />.
        /// </summary>
        public static MemberTable MathF { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.BitConverter" />.
        /// </summary>
        public static MemberTable BitConverter { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Convert" />.
        /// </summary>
        public static MemberTable Convert { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Array" />.
        /// </summary>
        public static MemberTable Array { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Index" />.
        /// </summary>
        public static MemberTable Index { get; }

        /// <summary>
        /// Gets a table of pure members on <see cref="global::System.Range" />.
        /// </summary>
        public static MemberTable Range { get; }

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
                static RegularExpressions()
                {
                    // See comment in outer 'System' class static constructor for why we're not
                    // using normal initializer expressions.
#pragma warning disable IDE0028 // Simplify collection initialization
                    MemberTable regexMembers = new();
                    regexMembers.Add((string pattern) => new global::System.Text.RegularExpressions.Regex(pattern));
                    regexMembers.Add((string pattern, global::System.Text.RegularExpressions.RegexOptions options) => new global::System.Text.RegularExpressions.Regex(pattern, options));
                    regexMembers.Add((string pattern, global::System.Text.RegularExpressions.RegexOptions options, TimeSpan matchTimeout) => new global::System.Text.RegularExpressions.Regex(pattern, options, matchTimeout));

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r) => r.MatchTimeout);
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r) => r.Options);
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r) => r.RightToLeft);

                    regexMembers.Add((string str) => global::System.Text.RegularExpressions.Regex.Escape(str));

                    // NB: GetGroupNames and GetGroupNumbers are omitted; these return mutable arrays.

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, int i) => r.GroupNameFromNumber(i));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string name) => r.GroupNumberFromName(name));

                    regexMembers.Add((string input, string pattern) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern));
                    regexMembers.Add((string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern, options));
                    regexMembers.Add((string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern, options, matchTimeout));

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input) => r.IsMatch(input));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.IsMatch(input, startat));

                    regexMembers.Add((string input, string pattern) => global::System.Text.RegularExpressions.Regex.Match(input, pattern));
                    regexMembers.Add((string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Match(input, pattern, options));
                    regexMembers.Add((string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Match(input, pattern, options, matchTimeout));

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input) => r.Match(input));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.Match(input, startat));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, int beginning, int length) => r.Match(input, beginning, length));

                    regexMembers.Add((string input, string pattern) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern));
                    regexMembers.Add((string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern, options));
                    regexMembers.Add((string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern, options, matchTimeout));

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input) => r.Matches(input));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.Matches(input, startat));

                    regexMembers.Add((string input, string pattern, string replacement) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement));
                    regexMembers.Add((string input, string pattern, string replacement, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options));
                    regexMembers.Add((string input, string pattern, string replacement, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options, matchTimeout));

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, string replacement) => r.Replace(input, replacement));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, string replacement, int count) => r.Replace(input, replacement, count));
                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r, string input, string replacement, int count, int startat) => r.Replace(input, replacement, count, startat));

                    // NB: Replace overloads with MatchEvaluator are omitted; the delegate may not be a pure function.
                    // REVIEW: In practice, it is assumed that the match evaluator is pure. Should we provide a table including these members, so users can opt-in?

                    // NB: Split overloads are omitted; these return mutable arrays.

                    regexMembers.Add((global::System.Text.RegularExpressions.Regex r) => r.ToString());

                    regexMembers.Add((string str) => global::System.Text.RegularExpressions.Regex.Unescape(str));
                    Regex = regexMembers.ToReadOnly();

                    MemberTable matchCollectionMembers = new();
                    matchCollectionMembers.Add((global::System.Text.RegularExpressions.MatchCollection c) => c.Count);
                    matchCollectionMembers.Add((global::System.Text.RegularExpressions.MatchCollection c) => c.IsReadOnly);
                    matchCollectionMembers.Add((global::System.Text.RegularExpressions.MatchCollection c) => c.IsSynchronized);

                    matchCollectionMembers.Add((global::System.Text.RegularExpressions.MatchCollection c, int i) => c[i]);
                    MatchCollection = matchCollectionMembers.ToReadOnly();

                    MemberTable matchMembers = new();
                    matchMembers.Add(() => global::System.Text.RegularExpressions.Match.Empty);

                    matchMembers.Add((global::System.Text.RegularExpressions.Match m) => m.Groups);

                    matchMembers.Add((global::System.Text.RegularExpressions.Match m) => m.NextMatch());
                    matchMembers.Add((global::System.Text.RegularExpressions.Match m, string replacement) => m.Result(replacement)); // NB: Virtual but only internal derivees are allowed.
                    Match = matchMembers.ToReadOnly();

                    MemberTable groupCollectionMembers = new();
                    groupCollectionMembers.Add((global::System.Text.RegularExpressions.GroupCollection g) => g.Count);
                    groupCollectionMembers.Add((global::System.Text.RegularExpressions.GroupCollection g) => g.IsReadOnly);
                    groupCollectionMembers.Add((global::System.Text.RegularExpressions.GroupCollection g) => g.IsSynchronized);

                    groupCollectionMembers.Add((global::System.Text.RegularExpressions.GroupCollection g, int groupnum) => g[groupnum]);
                    groupCollectionMembers.Add((global::System.Text.RegularExpressions.GroupCollection g, string groupname) => g[groupname]);
                    GroupCollection = groupCollectionMembers.ToReadOnly();

                    MemberTable groupMembers = new();
                    groupMembers.Add((global::System.Text.RegularExpressions.Group g) => g.Name);
                    groupMembers.Add((global::System.Text.RegularExpressions.Group g) => g.Success);
                    groupMembers.Add((global::System.Text.RegularExpressions.Group g) => g.Captures);
                    Group = groupMembers.ToReadOnly();

                    MemberTable captureCollectionMembers = new();
                    captureCollectionMembers.Add((global::System.Text.RegularExpressions.CaptureCollection c) => c.Count);
                    captureCollectionMembers.Add((global::System.Text.RegularExpressions.CaptureCollection c) => c.IsReadOnly);
                    captureCollectionMembers.Add((global::System.Text.RegularExpressions.CaptureCollection c) => c.IsSynchronized);

                    captureCollectionMembers.Add((global::System.Text.RegularExpressions.CaptureCollection c, int i) => c[i]);
                    CaptureCollection = captureCollectionMembers.ToReadOnly();

                    MemberTable captureMembers = new();
                    captureMembers.Add((global::System.Text.RegularExpressions.Capture c) => c.Index);
                    captureMembers.Add((global::System.Text.RegularExpressions.Capture c) => c.Length);
                    captureMembers.Add((global::System.Text.RegularExpressions.Capture c) => c.Value);

                    captureMembers.Add((global::System.Text.RegularExpressions.Capture c) => c.ToString());
                    Capture = captureMembers.ToReadOnly();

#pragma warning restore IDE0028 // Simplify collection initialization

                }
                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Regex" />.
                /// </summary>
                public static MemberTable Regex { get; }

                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.MatchCollection" />.
                /// </summary>
                public static MemberTable MatchCollection { get; }

                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Match" />.
                /// </summary>
                public static MemberTable Match { get; }

                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.GroupCollection" />.
                /// </summary>
                public static MemberTable GroupCollection { get; }

                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Group" />.
                /// </summary>
                public static MemberTable Group { get; }

                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.CaptureCollection" />.
                /// </summary>
                public static MemberTable CaptureCollection { get; }

                /// <summary>
                /// Gets a table of pure members on <see cref="global::System.Text.RegularExpressions.Capture" />.
                /// </summary>
                public static MemberTable Capture { get; }

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
            Half,
            Index,
            Range,
            MathF,
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
