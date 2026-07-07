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
            booleanMembers.Add(static () => bool.FalseString);
            booleanMembers.Add(static () => bool.TrueString);
            booleanMembers.Add(static (bool i, bool j) => i.Equals(j));
            booleanMembers.Add(static (bool i, bool j) => i.CompareTo(j));
            Boolean = booleanMembers.ToReadOnly();

            MemberTable charMembers = new();
            charMembers.Add(static (string s) => char.Parse(s));
            charMembers.Add(static (char c) => char.ToString(c));
            charMembers.Add(static (char c) => char.IsDigit(c));
            charMembers.Add(static (char c) => char.IsLetter(c));
            charMembers.Add(static (char c) => char.IsWhiteSpace(c));
            charMembers.Add(static (char c) => char.IsUpper(c));
            charMembers.Add(static (char c) => char.IsLower(c));
            charMembers.Add(static (char c) => char.IsPunctuation(c));
            charMembers.Add(static (char c) => char.IsLetterOrDigit(c));
            charMembers.Add(static (char c) => char.ToUpperInvariant(c));
            charMembers.Add(static (char c) => char.ToLowerInvariant(c));
            charMembers.Add(static (char c) => char.IsControl(c));
            charMembers.Add(static (char c) => char.IsNumber(c));
            charMembers.Add(static (char c) => char.IsSeparator(c));
            charMembers.Add(static (char c) => char.IsSurrogate(c));
            charMembers.Add(static (char c) => char.IsSymbol(c));
            charMembers.Add(static (char c) => char.GetUnicodeCategory(c));
            charMembers.Add(static (char c) => char.GetNumericValue(c));
            charMembers.Add(static (char c) => char.IsHighSurrogate(c));
            charMembers.Add(static (char c) => char.IsLowSurrogate(c));
            charMembers.Add(static (int utf32) => char.ConvertFromUtf32(utf32));
            charMembers.Add(static (string s, int index) => char.IsControl(s, index));
            charMembers.Add(static (string s, int index) => char.IsDigit(s, index));
            charMembers.Add(static (string s, int index) => char.IsLetter(s, index));
            charMembers.Add(static (string s, int index) => char.IsLetterOrDigit(s, index));
            charMembers.Add(static (string s, int index) => char.IsLower(s, index));
            charMembers.Add(static (string s, int index) => char.IsNumber(s, index));
            charMembers.Add(static (string s, int index) => char.IsPunctuation(s, index));
            charMembers.Add(static (string s, int index) => char.IsSeparator(s, index));
            charMembers.Add(static (string s, int index) => char.IsSurrogate(s, index));
            charMembers.Add(static (string s, int index) => char.IsSymbol(s, index));
            charMembers.Add(static (string s, int index) => char.IsUpper(s, index));
            charMembers.Add(static (string s, int index) => char.IsWhiteSpace(s, index));
            charMembers.Add(static (string s, int index) => char.GetUnicodeCategory(s, index));
            charMembers.Add(static (string s, int index) => char.GetNumericValue(s, index));
            charMembers.Add(static (string s, int index) => char.IsHighSurrogate(s, index));
            charMembers.Add(static (string s, int index) => char.IsLowSurrogate(s, index));
            charMembers.Add(static (string s, int index) => char.IsSurrogatePair(s, index));
            charMembers.Add(static (string s, int index) => char.ConvertToUtf32(s, index));
            charMembers.Add(static (char highSurrogate, char lowSurrogate) => char.IsSurrogatePair(highSurrogate, lowSurrogate));
            charMembers.Add(static (char highSurrogate, char lowSurrogate) => char.ConvertToUtf32(highSurrogate, lowSurrogate));
            charMembers.Add(static (char i, char j) => i.CompareTo(j));
            charMembers.Add(static (char i, char j) => i.Equals(j));
            charMembers.Add(static (char c) => c.ToString());
            Char = charMembers.ToReadOnly();

            MemberTable sByteMembers = new();
            sByteMembers.Add(static (sbyte i, sbyte j) => i.CompareTo(j));
            sByteMembers.Add(static (sbyte i, sbyte j) => i.Equals(j));
            SByte = sByteMembers.ToReadOnly();

            MemberTable byteMembers = new();
            byteMembers.Add(static (byte i, byte j) => i.CompareTo(j));
            byteMembers.Add(static (byte i, byte j) => i.Equals(j));
            Byte = byteMembers.ToReadOnly();

            MemberTable int16Members = new();
            int16Members.Add(static (short i, short j) => i.CompareTo(j));
            int16Members.Add(static (short i, short j) => i.Equals(j));
            Int16 = int16Members.ToReadOnly();

            MemberTable uInt16Members = new();
            uInt16Members.Add(static (ushort i, ushort j) => i.CompareTo(j));
            uInt16Members.Add(static (ushort i, ushort j) => i.Equals(j));
            UInt16 = uInt16Members.ToReadOnly();

            MemberTable int32Members = new();
            int32Members.Add(static (int i, int j) => i.CompareTo(j));
            int32Members.Add(static (int i, int j) => i.Equals(j));
            Int32 = int32Members.ToReadOnly();

            MemberTable uInt32Members = new();
            uInt32Members.Add(static (uint i, uint j) => i.CompareTo(j));
            uInt32Members.Add(static (uint i, uint j) => i.Equals(j));
            UInt32 = uInt32Members.ToReadOnly();

            MemberTable int64Members = new();
            int64Members.Add(static (long i, long j) => i.CompareTo(j));
            int64Members.Add(static (long i, long j) => i.Equals(j));
            Int64 = int64Members.ToReadOnly();

            MemberTable uInt64Members = new();
            uInt64Members.Add(static (ulong i, ulong j) => i.CompareTo(j));
            uInt64Members.Add(static (ulong i, ulong j) => i.Equals(j));
            UInt64 = uInt64Members.ToReadOnly();

            MemberTable halfMembers = new();
            halfMembers.Add(static () => global::System.Half.Epsilon);
            halfMembers.Add(static () => global::System.Half.MaxValue);
            halfMembers.Add(static () => global::System.Half.MinValue);
            halfMembers.Add(static () => global::System.Half.NaN);
            halfMembers.Add(static () => global::System.Half.NegativeInfinity);
            halfMembers.Add(static () => global::System.Half.PositiveInfinity);

            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsFinite(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsInfinity(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsNaN(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsNegative(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsNegativeInfinity(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsNormal(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsPositiveInfinity(i));
            halfMembers.Add(static (global::System.Half i) => global::System.Half.IsSubnormal(i));

            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i.CompareTo(j));
            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i.Equals(j));

            halfMembers.Add(static (global::System.Half i) => (float)i);
            halfMembers.Add(static (global::System.Half i) => (double)i);
            halfMembers.Add(static (float i) => (global::System.Half)i);
            halfMembers.Add(static (double i) => (global::System.Half)i);

            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i == j);
            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i != j);
            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i < j);
            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i <= j);
            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i > j);
            halfMembers.Add(static (global::System.Half i, global::System.Half j) => i >= j);
            Half = halfMembers.ToReadOnly();

            MemberTable singleMembers = new();
            singleMembers.Add(static (float i) => float.IsInfinity(i));
            singleMembers.Add(static (float i) => float.IsNaN(i));
            singleMembers.Add(static (float i) => float.IsNegativeInfinity(i));
            singleMembers.Add(static (float i) => float.IsPositiveInfinity(i));

            singleMembers.Add(static (float i, float j) => i.CompareTo(j));
            singleMembers.Add(static (float i, float j) => i.Equals(j));

            singleMembers.Add(static (float i) => float.IsFinite(i));
            singleMembers.Add(static (float i) => float.IsNegative(i));
            singleMembers.Add(static (float i) => float.IsNormal(i));
            singleMembers.Add(static (float i) => float.IsSubnormal(i));
            Single = singleMembers.ToReadOnly();

            MemberTable doubleMembers = new();
            doubleMembers.Add(static (double i) => double.IsInfinity(i));
            doubleMembers.Add(static (double i) => double.IsNaN(i));
            doubleMembers.Add(static (double i) => double.IsNegativeInfinity(i));
            doubleMembers.Add(static (double i) => double.IsPositiveInfinity(i));

            doubleMembers.Add(static (double i, double j) => i.CompareTo(j));
            doubleMembers.Add(static (double i, double j) => i.Equals(j));

            doubleMembers.Add(static (double i) => double.IsFinite(i));
            doubleMembers.Add(static (double i) => double.IsNegative(i));
            doubleMembers.Add(static (double i) => double.IsNormal(i));
            doubleMembers.Add(static (double i) => double.IsSubnormal(i));
            Double = doubleMembers.ToReadOnly();

            MemberTable decimalMembers = new();
            decimalMembers.Add(static (decimal i) => decimal.Ceiling(i));
            decimalMembers.Add(static (decimal i) => decimal.Floor(i));
            decimalMembers.Add(static (decimal i) => decimal.Negate(i));
            decimalMembers.Add(static (decimal i) => decimal.Round(i));
            decimalMembers.Add(static (decimal i) => decimal.ToByte(i));
            decimalMembers.Add(static (decimal i) => decimal.ToDouble(i));
            decimalMembers.Add(static (decimal i) => decimal.ToInt16(i));
            decimalMembers.Add(static (decimal i) => decimal.ToInt32(i));
            decimalMembers.Add(static (decimal i) => decimal.ToInt64(i));
            decimalMembers.Add(static (decimal i) => decimal.ToSByte(i));
            decimalMembers.Add(static (decimal i) => decimal.ToSingle(i));
            decimalMembers.Add(static (decimal i) => decimal.ToUInt16(i));
            decimalMembers.Add(static (decimal i) => decimal.ToUInt32(i));
            decimalMembers.Add(static (decimal i) => decimal.ToUInt64(i));
            decimalMembers.Add(static (decimal i) => decimal.ToOACurrency(i));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Add(i, j));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Compare(i, j));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Divide(i, j));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Equals(i, j));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Multiply(i, j));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Remainder(i, j));
            decimalMembers.Add(static (decimal i, decimal j) => decimal.Subtract(i, j));

            decimalMembers.Add(static (sbyte i) => (decimal)i);
            decimalMembers.Add(static (byte i) => (decimal)i);
            decimalMembers.Add(static (short i) => (decimal)i);
            decimalMembers.Add(static (ushort i) => (decimal)i);
            decimalMembers.Add(static (int i) => (decimal)i);
            decimalMembers.Add(static (uint i) => (decimal)i);
            decimalMembers.Add(static (long i) => (decimal)i);
            decimalMembers.Add(static (ulong i) => (decimal)i);
            decimalMembers.Add(static (char i) => (decimal)i);
            decimalMembers.Add(static (float i) => (decimal)i);
            decimalMembers.Add(static (double i) => (decimal)i);

            decimalMembers.Add(static (decimal i) => -i);
            decimalMembers.Add(static (decimal i) => (sbyte)i);
            decimalMembers.Add(static (decimal i) => (byte)i);
            decimalMembers.Add(static (decimal i) => (short)i);
            decimalMembers.Add(static (decimal i) => (ushort)i);
            decimalMembers.Add(static (decimal i) => (int)i);
            decimalMembers.Add(static (decimal i) => (uint)i);
            decimalMembers.Add(static (decimal i) => (long)i);
            decimalMembers.Add(static (decimal i) => (ulong)i);
            decimalMembers.Add(static (decimal i) => (char)i);
            decimalMembers.Add(static (decimal i) => (float)i);
            decimalMembers.Add(static (decimal i) => (double)i);

            decimalMembers.Add(static (decimal i, decimal j) => i + j);
            decimalMembers.Add(static (decimal i, decimal j) => i - j);
            decimalMembers.Add(static (decimal i, decimal j) => i * j);
            decimalMembers.Add(static (decimal i, decimal j) => i / j);
            decimalMembers.Add(static (decimal i, decimal j) => i % j);
            decimalMembers.Add(static (decimal i, decimal j) => i == j);
            decimalMembers.Add(static (decimal i, decimal j) => i != j);
            decimalMembers.Add(static (decimal i, decimal j) => i < j);
            decimalMembers.Add(static (decimal i, decimal j) => i <= j);
            decimalMembers.Add(static (decimal i, decimal j) => i > j);
            decimalMembers.Add(static (decimal i, decimal j) => i >= j);

            decimalMembers.Add(static (decimal i, decimal j) => i.CompareTo(j));
            decimalMembers.Add(static (decimal i, decimal j) => i.Equals(j));
            Decimal = decimalMembers.ToReadOnly();

            MemberTable stringMembers = new();
            stringMembers.Add(static (char[] value) => new string(value));
            stringMembers.Add(static (char[] value, int startIndex, int length) => new string(value, startIndex, length));
            stringMembers.Add(static (char c, int count) => new string(c, count));

            stringMembers.Add(static () => string.Empty);
            stringMembers.Add(static (string s) => s.Length);
            stringMembers.Add(static (string s, int index) => s[index]);

            stringMembers.Add(static (string value) => string.IsNullOrEmpty(value));
            stringMembers.Add(static (string value) => string.IsNullOrWhiteSpace(value));

            stringMembers.Add(static (string strA, string strB) => string.CompareOrdinal(strA, strB));
            stringMembers.Add(static (string strA, int indexA, string strB, int indexB, int length) => string.CompareOrdinal(strA, indexA, strB, indexB, length));

            stringMembers.Add(static (string str0, string str1) => string.Concat(str0, str1));
            stringMembers.Add(static (string str0, string str1, string str2) => string.Concat(str0, str1, str2));
            stringMembers.Add(static (string str0, string str1, string str2, string str3) => string.Concat(str0, str1, str2, str3));
            stringMembers.Add(static (string[] values) => string.Concat(values));

            stringMembers.Add(static (string s, string value) => s.Contains(value)); // NB: This uses ordinal comparison

            stringMembers.Add(static (string s, string value) => s.Equals(value));
            stringMembers.Add(static (string a, string b) => string.Equals(a, b));

            stringMembers.Add(static (string s, char value) => s.IndexOf(value));
            stringMembers.Add(static (string s, char value, int startIndex) => s.IndexOf(value, startIndex));
            stringMembers.Add(static (string s, char value, int startIndex, int count) => s.IndexOf(value, startIndex, count));

            stringMembers.Add(static (string s, char[] anyOf) => s.IndexOfAny(anyOf));
            stringMembers.Add(static (string s, char[] anyOf, int startIndex) => s.IndexOfAny(anyOf, startIndex));
            stringMembers.Add(static (string s, char[] anyOf, int startIndex, int count) => s.IndexOfAny(anyOf, startIndex, count));

            stringMembers.Add(static (string s, char value) => s.LastIndexOf(value));
            stringMembers.Add(static (string s, char value, int startIndex) => s.LastIndexOf(value, startIndex));
            stringMembers.Add(static (string s, char value, int startIndex, int count) => s.LastIndexOf(value, startIndex, count));

            stringMembers.Add(static (string s, char[] anyOf) => s.LastIndexOfAny(anyOf));
            stringMembers.Add(static (string s, char[] anyOf, int startIndex) => s.LastIndexOfAny(anyOf, startIndex));
            stringMembers.Add(static (string s, char[] anyOf, int startIndex, int count) => s.LastIndexOfAny(anyOf, startIndex, count));

            stringMembers.Add(static (string s, int startIndex, string value) => s.Insert(startIndex, value));

            stringMembers.Add(static (string s) => s.IsNormalized());
            stringMembers.Add(static (string s) => s.Normalize());
            stringMembers.Add(static (string s, global::System.Text.NormalizationForm normalizationForm) => s.Normalize(normalizationForm));
            stringMembers.Add(static (string s, global::System.Text.NormalizationForm normalizationForm) => s.IsNormalized(normalizationForm));

            stringMembers.Add(static (string separator, string[] value) => string.Join(separator, value));
            stringMembers.Add(static (string separator, string[] value, int startIndex, int count) => string.Join(separator, value, startIndex, count));

            stringMembers.Add(static (string s, int totalWidth) => s.PadLeft(totalWidth));
            stringMembers.Add(static (string s, int totalWidth) => s.PadRight(totalWidth));
            stringMembers.Add(static (string s, int totalWidth, char paddingChar) => s.PadLeft(totalWidth, paddingChar));
            stringMembers.Add(static (string s, int totalWidth, char paddingChar) => s.PadRight(totalWidth, paddingChar));

            stringMembers.Add(static (string s, int startIndex) => s.Remove(startIndex));
            stringMembers.Add(static (string s, int startIndex, int count) => s.Remove(startIndex, count));

            stringMembers.Add(static (string s, char oldChar, char newChar) => s.Replace(oldChar, newChar));
            stringMembers.Add(static (string s, string oldValue, string newValue) => s.Replace(oldValue, newValue));

            // NB: Omitting Split which returns a mutable array.

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable IDE0057 // Substring can be simplified (not in expression trees)
            stringMembers.Add(static (string s, int startIndex) => s.Substring(startIndex));
            stringMembers.Add(static (string s, int startIndex, int length) => s.Substring(startIndex, length));
#pragma warning restore IDE0057
#pragma warning restore IDE0079

            // NB: Omitting ToCharArray which returns a mutable array.

            stringMembers.Add(static (string s) => s.ToLowerInvariant());
            stringMembers.Add(static (string s) => s.ToUpperInvariant());

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with System.Object here.
            //(string s) => s.ToString(),

            // REVIEW: Should we add format providers sample values to tests just for this?
            //(string s, IFormatProvider provider) => s.ToString(provider /* NB: this parameter is always ignored */),

            stringMembers.Add(static (string s) => s.Trim());
            stringMembers.Add(static (string s, char[] trimChars) => s.Trim(trimChars));
            stringMembers.Add(static (string s, char[] trimChars) => s.TrimEnd(trimChars));
            stringMembers.Add(static (string s, char[] trimChars) => s.TrimStart(trimChars));

            stringMembers.Add(static (string s1, string s2) => s1 == s2);
            stringMembers.Add(static (string s1, string s2) => s1 != s2);

            stringMembers.Add(static (string s, char c) => s.Contains(c));
            stringMembers.Add(static (string s, char c) => s.StartsWith(c));
            stringMembers.Add(static (string s, char c) => s.EndsWith(c));
            String = stringMembers.ToReadOnly();

            MemberTable dateTimeMembers = new();
            dateTimeMembers.Add(static (long ticks) => new global::System.DateTime(ticks));
            dateTimeMembers.Add(static (long ticks, DateTimeKind kind) => new global::System.DateTime(ticks, kind));
            dateTimeMembers.Add(static (int year, int month, int day) => new global::System.DateTime(year, month, day));
            dateTimeMembers.Add(static (int year, int month, int day, int hour, int minute, int second) => new global::System.DateTime(year, month, day, hour, minute, second));
            dateTimeMembers.Add(static (int year, int month, int day, int hour, int minute, int second, DateTimeKind kind) => new global::System.DateTime(year, month, day, hour, minute, second, kind));
            dateTimeMembers.Add(static (int year, int month, int day, int hour, int minute, int second, int millisecond) => new global::System.DateTime(year, month, day, hour, minute, second, millisecond));
            dateTimeMembers.Add(static (int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind) => new global::System.DateTime(year, month, day, hour, minute, second, millisecond, kind));

            dateTimeMembers.Add(static () => global::System.DateTime.MaxValue);
            dateTimeMembers.Add(static () => global::System.DateTime.MinValue);

            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Date);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Day);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.DayOfWeek);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.DayOfYear);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Hour);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Kind);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Millisecond);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Minute);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Month);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Second);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Ticks);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.TimeOfDay);
            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.Year);

            dateTimeMembers.Add(static (global::System.DateTime t1, global::System.DateTime t2) => global::System.DateTime.Compare(t1, t2));
            dateTimeMembers.Add(static (global::System.DateTime t1, global::System.DateTime t2) => global::System.DateTime.Equals(t1, t2));

            dateTimeMembers.Add(static (double d) => global::System.DateTime.FromOADate(d));
            dateTimeMembers.Add(static (long fileTime) => global::System.DateTime.FromFileTimeUtc(fileTime));
            dateTimeMembers.Add(static (int year) => global::System.DateTime.IsLeapYear(year));
            dateTimeMembers.Add(static (int year, int month) => global::System.DateTime.DaysInMonth(year, month));
            dateTimeMembers.Add(static (global::System.DateTime value, global::System.DateTimeKind kind) => global::System.DateTime.SpecifyKind(value, kind));

            dateTimeMembers.Add(static (global::System.DateTime dt) => dt.ToOADate());

            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.TimeSpan value) => dt.Add(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, double value) => dt.AddDays(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, double value) => dt.AddHours(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, double value) => dt.AddMilliseconds(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, double value) => dt.AddMinutes(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, int months) => dt.AddMonths(months));
            dateTimeMembers.Add(static (global::System.DateTime dt, double value) => dt.AddSeconds(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, long value) => dt.AddTicks(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, int value) => dt.AddYears(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.DateTime value) => dt.Subtract(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.TimeSpan value) => dt.Subtract(value));

            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.DateTime value) => dt.CompareTo(value));
            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.DateTime value) => dt.Equals(value));

            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.TimeSpan ts) => dt + ts);
            dateTimeMembers.Add(static (global::System.DateTime dt, global::System.TimeSpan ts) => dt - ts);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 - dt2);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 == dt2);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 != dt2);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 < dt2);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 <= dt2);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 > dt2);
            dateTimeMembers.Add(static (global::System.DateTime dt1, global::System.DateTime dt2) => dt1 >= dt2);
            DateTime = dateTimeMembers.ToReadOnly();

            MemberTable dateTimeOffsetMembers = new();
            dateTimeOffsetMembers.Add(static (long ticks, global::System.TimeSpan offset) => new global::System.DateTimeOffset(ticks, offset));
            dateTimeOffsetMembers.Add(static (int year, int month, int day, int hour, int minute, int second, global::System.TimeSpan offset) => new global::System.DateTimeOffset(year, month, day, hour, minute, second, offset));
            dateTimeOffsetMembers.Add(static (int year, int month, int day, int hour, int minute, int second, int millisecond, global::System.TimeSpan offset) => new global::System.DateTimeOffset(year, month, day, hour, minute, second, millisecond, offset));

            dateTimeOffsetMembers.Add(static () => global::System.DateTimeOffset.MaxValue);
            dateTimeOffsetMembers.Add(static () => global::System.DateTimeOffset.MinValue);

            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Date);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.DateTime);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Day);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.DayOfWeek);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.DayOfYear);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Hour);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.LocalDateTime);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Millisecond);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Minute);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Month);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Offset);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Second);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Ticks);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.TimeOfDay);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.UtcDateTime);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.UtcTicks);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.Year);

            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto.Add(ts));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, double days) => dto.AddDays(days));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, double hours) => dto.AddHours(hours));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, double milliseconds) => dto.AddMilliseconds(milliseconds));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, double minutes) => dto.AddMinutes(minutes));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, int months) => dto.AddMonths(months));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, double seconds) => dto.AddSeconds(seconds));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, long ticks) => dto.AddTicks(ticks));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, int years) => dto.AddYears(years));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.DateTimeOffset value) => dto.Subtract(value));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.TimeSpan value) => dto.Subtract(value));

            dateTimeOffsetMembers.Add(static (long fileTime) => global::System.DateTimeOffset.FromFileTime(fileTime));

            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset first, global::System.DateTimeOffset second) => global::System.DateTimeOffset.Compare(first, second));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset first, global::System.DateTimeOffset second) => global::System.DateTimeOffset.Equals(first, second));

            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.ToFileTime());
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.ToUniversalTime());
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.TimeSpan offset) => dto.ToOffset(offset));

            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.CompareTo(other));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.Equals(other));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.DateTimeOffset other) => dto.EqualsExact(other));

            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto + ts);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto, global::System.TimeSpan ts) => dto - ts);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 - dto2);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 == dto2);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 != dto2);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 < dto2);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 <= dto2);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 > dto2);
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto1, global::System.DateTimeOffset dto2) => dto1 >= dto2);

            dateTimeOffsetMembers.Add(static (long seconds) => global::System.DateTimeOffset.FromUnixTimeSeconds(seconds));
            dateTimeOffsetMembers.Add(static (long milliseconds) => global::System.DateTimeOffset.FromUnixTimeMilliseconds(milliseconds));
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.ToUnixTimeSeconds());
            dateTimeOffsetMembers.Add(static (global::System.DateTimeOffset dto) => dto.ToUnixTimeMilliseconds());
            DateTimeOffset = dateTimeOffsetMembers.ToReadOnly();

            MemberTable timeSpanMembers = new();
            timeSpanMembers.Add(static (long ticks) => new global::System.TimeSpan(ticks));
            timeSpanMembers.Add(static (int hours, int minutes, int seconds) => new global::System.TimeSpan(hours, minutes, seconds));
            timeSpanMembers.Add(static (int days, int hours, int minutes, int seconds) => new global::System.TimeSpan(days, hours, minutes, seconds));
            timeSpanMembers.Add(static (int days, int hours, int minutes, int seconds, int milliseconds) => new global::System.TimeSpan(days, hours, minutes, seconds, milliseconds));

            timeSpanMembers.Add(static () => global::System.TimeSpan.MaxValue);
            timeSpanMembers.Add(static () => global::System.TimeSpan.MinValue);
            timeSpanMembers.Add(static () => global::System.TimeSpan.TicksPerDay);
            timeSpanMembers.Add(static () => global::System.TimeSpan.TicksPerHour);
            timeSpanMembers.Add(static () => global::System.TimeSpan.TicksPerMillisecond);
            timeSpanMembers.Add(static () => global::System.TimeSpan.TicksPerMinute);
            timeSpanMembers.Add(static () => global::System.TimeSpan.TicksPerSecond);
            timeSpanMembers.Add(static () => global::System.TimeSpan.Zero);

            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Days);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Hours);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Milliseconds);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Minutes);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Seconds);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Ticks);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.TotalDays);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.TotalHours);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.TotalMilliseconds);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.TotalMinutes);
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.TotalSeconds);

            timeSpanMembers.Add(static (long value) => global::System.TimeSpan.FromTicks(value));
            timeSpanMembers.Add(static (double value) => global::System.TimeSpan.FromDays(value));
            timeSpanMembers.Add(static (double value) => global::System.TimeSpan.FromHours(value));
            timeSpanMembers.Add(static (double value) => global::System.TimeSpan.FromMilliseconds(value));
            timeSpanMembers.Add(static (double value) => global::System.TimeSpan.FromMinutes(value));
            timeSpanMembers.Add(static (double value) => global::System.TimeSpan.FromSeconds(value));

            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => global::System.TimeSpan.Compare(t1, t2));
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => global::System.TimeSpan.Equals(t1, t2));

            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Duration());
            timeSpanMembers.Add(static (global::System.TimeSpan t) => t.Negate());
            timeSpanMembers.Add(static (global::System.TimeSpan t, global::System.TimeSpan ts) => t.Add(ts));
            timeSpanMembers.Add(static (global::System.TimeSpan t, global::System.TimeSpan ts) => t.Subtract(ts));

            timeSpanMembers.Add(static (global::System.TimeSpan t, global::System.TimeSpan value) => t.CompareTo(value));
            timeSpanMembers.Add(static (global::System.TimeSpan t, global::System.TimeSpan obj) => t.Equals(obj));

            timeSpanMembers.Add(static (global::System.TimeSpan t) => -t);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 + t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 - t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 == t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 != t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 < t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 <= t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 > t2);
            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 >= t2);

            timeSpanMembers.Add(static (global::System.TimeSpan t, double divisor) => t.Divide(divisor));
            timeSpanMembers.Add(static (global::System.TimeSpan t, global::System.TimeSpan ts) => t.Divide(ts));
            timeSpanMembers.Add(static (global::System.TimeSpan t, double factor) => t.Multiply(factor));

            timeSpanMembers.Add(static (global::System.TimeSpan t1, global::System.TimeSpan t2) => t1 / t2);
            timeSpanMembers.Add(static (global::System.TimeSpan timeSpan, double divisor) => timeSpan / divisor);
            timeSpanMembers.Add(static (double factor, global::System.TimeSpan timeSpan) => factor * timeSpan);
            timeSpanMembers.Add(static (global::System.TimeSpan timeSpan, double factor) => timeSpan * factor);
            TimeSpan = timeSpanMembers.ToReadOnly();

            MemberTable guidMembers = new();
            guidMembers.Add(static (string input) => global::System.Guid.Parse(input));
            guidMembers.Add(static (string input, string format) => global::System.Guid.ParseExact(input, format));

            guidMembers.Add(static (byte[] b) => new global::System.Guid(b));
            guidMembers.Add(static (string g) => new global::System.Guid(g));
            guidMembers.Add(static (uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) => new global::System.Guid(a, b, c, d, e, f, g, h, i, j, k));
            guidMembers.Add(static (int a, short b, short c, byte[] d) => new global::System.Guid(a, b, c, d));
            guidMembers.Add(static (int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) => new global::System.Guid(a, b, c, d, e, f, g, h, i, j, k));

            guidMembers.Add(static () => global::System.Guid.Empty);

            guidMembers.Add(static (global::System.Guid g, global::System.Guid value) => g.CompareTo(value));
            guidMembers.Add(static (global::System.Guid g, global::System.Guid value) => g.Equals(value));

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
            //(global::System.Guid g) => g.ToString(),
            //(global::System.Guid g, string format) => g.ToString(format),

            // REVIEW: Should we add format providers sample values to tests just for this?
            //(global::System.Guid g, string format, IFormatProvider provider) => s.ToString(format, provider /* NB: this parameter is always ignored */),
            Guid = guidMembers.ToReadOnly();

            MemberTable uriMembers = new();
            uriMembers.Add(static (string uriString) => new Uri(uriString));
            uriMembers.Add(static (string uriString, UriKind uriKind) => new Uri(uriString, uriKind));
            uriMembers.Add(static (Uri baseUri, string relativeUri) => new Uri(baseUri, relativeUri));
            uriMembers.Add(static (Uri baseUri, Uri relativeUri) => new Uri(baseUri, relativeUri));

            uriMembers.Add(static (string uriString) => new global::System.Uri(uriString));
            uriMembers.Add(static (string uriString, global::System.UriKind uriKind) => new global::System.Uri(uriString, uriKind));
            uriMembers.Add(static (global::System.Uri baseUri, string relativeUri) => new global::System.Uri(baseUri, relativeUri));
            uriMembers.Add(static (global::System.Uri baseUri, global::System.Uri relativeUri) => new global::System.Uri(baseUri, relativeUri));

            uriMembers.Add(static () => global::System.Uri.SchemeDelimiter);
            uriMembers.Add(static () => global::System.Uri.UriSchemeFile);
            uriMembers.Add(static () => global::System.Uri.UriSchemeFtp);
            uriMembers.Add(static () => global::System.Uri.UriSchemeGopher);
            uriMembers.Add(static () => global::System.Uri.UriSchemeHttp);
            uriMembers.Add(static () => global::System.Uri.UriSchemeHttps);
            uriMembers.Add(static () => global::System.Uri.UriSchemeMailto);
            uriMembers.Add(static () => global::System.Uri.UriSchemeNetPipe);
            uriMembers.Add(static () => global::System.Uri.UriSchemeNetTcp);
            uriMembers.Add(static () => global::System.Uri.UriSchemeNews);
            uriMembers.Add(static () => global::System.Uri.UriSchemeNntp);

            uriMembers.Add(static (global::System.Uri u) => u.AbsolutePath);
            uriMembers.Add(static (global::System.Uri u) => u.AbsoluteUri);
            uriMembers.Add(static (global::System.Uri u) => u.Authority);
            uriMembers.Add(static (global::System.Uri u) => u.DnsSafeHost);
            uriMembers.Add(static (global::System.Uri u) => u.Fragment);
            uriMembers.Add(static (global::System.Uri u) => u.Host);
            uriMembers.Add(static (global::System.Uri u) => u.HostNameType);
            //uriMembers.Add((global::System.Uri u) => u.IdnHost); // REVIEW: Not accessible
            uriMembers.Add(static (global::System.Uri u) => u.IsAbsoluteUri);
            uriMembers.Add(static (global::System.Uri u) => u.IsDefaultPort);
            uriMembers.Add(static (global::System.Uri u) => u.IsFile);
            uriMembers.Add(static (global::System.Uri u) => u.IsLoopback);
            uriMembers.Add(static (global::System.Uri u) => u.IsUnc);
            uriMembers.Add(static (global::System.Uri u) => u.LocalPath);
            uriMembers.Add(static (global::System.Uri u) => u.OriginalString);
            uriMembers.Add(static (global::System.Uri u) => u.PathAndQuery);
            uriMembers.Add(static (global::System.Uri u) => u.Port);
            uriMembers.Add(static (global::System.Uri u) => u.Query);
            uriMembers.Add(static (global::System.Uri u) => u.Scheme);
            uriMembers.Add(static (global::System.Uri u) => u.UserEscaped);
            uriMembers.Add(static (global::System.Uri u) => u.UserInfo);

            uriMembers.Add(static (string name) => global::System.Uri.CheckHostName(name));
            uriMembers.Add(static (string schemeName) => global::System.Uri.CheckSchemeName(schemeName));
            uriMembers.Add(static (char digit) => global::System.Uri.FromHex(digit));
            uriMembers.Add(static (char character) => global::System.Uri.HexEscape(character));
            uriMembers.Add(static (char character) => global::System.Uri.IsHexDigit(character));
            uriMembers.Add(static (string pattern, int index) => global::System.Uri.IsHexEncoding(pattern, index));
            uriMembers.Add(static (string stringToEscape) => global::System.Uri.EscapeDataString(stringToEscape));
            uriMembers.Add(static (string stringToUnescape) => global::System.Uri.UnescapeDataString(stringToUnescape));
            uriMembers.Add(static (string uriString, global::System.UriKind uriKind) => global::System.Uri.IsWellFormedUriString(uriString, uriKind));

            uriMembers.Add(static (global::System.Uri u1, global::System.Uri u2) => u1 == u2);
            uriMembers.Add(static (global::System.Uri u1, global::System.Uri u2) => u1 != u2);

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
            //(global::System.Uri u) => u.ToString(),
            uriMembers.Add(static (global::System.Uri u, global::System.UriPartial part) => u.GetLeftPart(part));
            uriMembers.Add(static (global::System.Uri u, global::System.Uri uri) => u.IsBaseOf(uri));
            uriMembers.Add(static (global::System.Uri u) => u.IsWellFormedOriginalString());
            uriMembers.Add(static (global::System.Uri u, global::System.Uri uri) => u.MakeRelativeUri(uri));
            uriMembers.Add(static (global::System.Uri u, global::System.UriComponents components, global::System.UriFormat format) => u.GetComponents(components, format));
            Uri = uriMembers.ToReadOnly();

            MemberTable versionMembers = new();
            versionMembers.Add(static () => new global::System.Version());
            versionMembers.Add(static (int major, int minor) => new global::System.Version(major, minor));
            versionMembers.Add(static (int major, int minor, int build) => new global::System.Version(major, minor, build));
            versionMembers.Add(static (int major, int minor, int build, int revision) => new global::System.Version(major, minor, build, revision));
            versionMembers.Add(static (string version) => new global::System.Version(version));

            versionMembers.Add(static (global::System.Version v) => v.Build);
            versionMembers.Add(static (global::System.Version v) => v.Major);
            versionMembers.Add(static (global::System.Version v) => v.MajorRevision);
            versionMembers.Add(static (global::System.Version v) => v.Minor);
            versionMembers.Add(static (global::System.Version v) => v.MinorRevision);
            versionMembers.Add(static (global::System.Version v) => v.Revision);

            versionMembers.Add(static (string input) => global::System.Version.Parse(input));

            // REVIEW: Test issue; we use MethodInfo.DeclaringType to find an instance value, so end up with global::System.Object here.
            //(global::System.Version v) => v.ToString(),
            versionMembers.Add(static (global::System.Version v, int fieldCount) => v.ToString(fieldCount));
            versionMembers.Add(static (global::System.Version v, global::System.Version value) => v.CompareTo(value));
            versionMembers.Add(static (global::System.Version v, global::System.Version obj) => v.Equals(obj));

            versionMembers.Add(static (global::System.Version v1, global::System.Version v2) => v1 == v2);
            versionMembers.Add(static (global::System.Version v1, global::System.Version v2) => v1 != v2);
            versionMembers.Add(static (global::System.Version v1, global::System.Version v2) => v1 < v2);
            versionMembers.Add(static (global::System.Version v1, global::System.Version v2) => v1 <= v2);
            versionMembers.Add(static (global::System.Version v1, global::System.Version v2) => v1 > v2);
            versionMembers.Add(static (global::System.Version v1, global::System.Version v2) => v1 >= v2);
            Version = versionMembers.ToReadOnly();

            MemberTable nullableMembers = new();
            nullableMembers.Add(static (Nullable<TValue> n) => n.HasValue);
            nullableMembers.Add(static (Nullable<TValue> n) => n.Value);

            nullableMembers.Add(static (Nullable<TValue> n) => n.GetValueOrDefault());
            nullableMembers.Add(static (Nullable<TValue> n, TValue defaultValue) => n.GetValueOrDefault(defaultValue));
            Nullable = nullableMembers.ToReadOnly();

            MemberTable tupleMembers = new();
            //
            // global::System.Tuple<>
            //
            tupleMembers.Add(static (T t1) => global::System.Tuple.Create(t1));
            tupleMembers.Add(static (T t1) => new Tuple<T>(t1));
            tupleMembers.Add(static (Tuple<T> t) => t.Item1);

            //
            // global::System.Tuple<,>
            //
            tupleMembers.Add(static (T t1, T t2) => global::System.Tuple.Create(t1, t2));
            tupleMembers.Add(static (T t1, T t2) => new Tuple<T, T>(t1, t2));
            tupleMembers.Add(static (Tuple<T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T> t) => t.Item2);

            //
            // global::System.Tuple<,,>
            //
            tupleMembers.Add(static (T t1, T t2, T t3) => global::System.Tuple.Create(t1, t2, t3));
            tupleMembers.Add(static (T t1, T t2, T t3) => new Tuple<T, T, T>(t1, t2, t3));
            tupleMembers.Add(static (Tuple<T, T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T, T> t) => t.Item2);
            tupleMembers.Add(static (Tuple<T, T, T> t) => t.Item3);

            //
            // global::System.Tuple<,,,>
            //
            tupleMembers.Add(static (T t1, T t2, T t3, T t4) => global::System.Tuple.Create(t1, t2, t3, t4));
            tupleMembers.Add(static (T t1, T t2, T t3, T t4) => new Tuple<T, T, T, T>(t1, t2, t3, t4));
            tupleMembers.Add(static (Tuple<T, T, T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T, T, T> t) => t.Item2);
            tupleMembers.Add(static (Tuple<T, T, T, T> t) => t.Item3);
            tupleMembers.Add(static (Tuple<T, T, T, T> t) => t.Item4);

            //
            // global::System.Tuple<,,,,>
            //
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5) => global::System.Tuple.Create(t1, t2, t3, t4, t5));
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5) => new Tuple<T, T, T, T, T>(t1, t2, t3, t4, t5));
            tupleMembers.Add(static (Tuple<T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add(static (Tuple<T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add(static (Tuple<T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add(static (Tuple<T, T, T, T, T> t) => t.Item5);

            //
            // global::System.Tuple<,,,,,>
            //
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5, T t6) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6));
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5, T t6) => new Tuple<T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6));
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T> t) => t.Item5);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T> t) => t.Item6);

            //
            // global::System.Tuple<,,,,,,>
            //
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5, T t6, T t7) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6, t7));
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5, T t6, T t7) => new Tuple<T, T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6, t7));
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item5);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item6);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T> t) => t.Item7);

            //
            // global::System.Tuple<,,,,,,,>
            //
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5, T t6, T t7, T r) => global::System.Tuple.Create(t1, t2, t3, t4, t5, t6, t7, r));
            tupleMembers.Add(static (T t1, T t2, T t3, T t4, T t5, T t6, T t7, T r) => new Tuple<T, T, T, T, T, T, T, T>(t1, t2, t3, t4, t5, t6, t7, r));
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item1);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item2);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item3);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item4);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item5);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item6);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Item7);
            tupleMembers.Add(static (Tuple<T, T, T, T, T, T, T, T> t) => t.Rest);
            Tuple = tupleMembers.ToReadOnly();

            MemberTable valueTupleMembers = new();
            // NB: Value tuples are mutable, so constructors and Create factory calls are not listed here.

            //
            // global::System.ValueTuple<>
            //
            valueTupleMembers.Add(static (ValueTuple<T> t) => t.Item1);

            //
            // global::System.ValueTuple<,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T> t) => t.Item2);

            //
            // global::System.ValueTuple<,,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T, T> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T, T> t) => t.Item2);
            valueTupleMembers.Add(static (ValueTuple<T, T, T> t) => t.Item3);

            //
            // global::System.ValueTuple<,,,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T> t) => t.Item4);

            //
            // global::System.ValueTuple<,,,,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T> t) => t.Item4);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T> t) => t.Item5);

            //
            // global::System.ValueTuple<,,,,,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T> t) => t.Item4);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T> t) => t.Item5);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T> t) => t.Item6);

            //
            // global::System.ValueTuple<,,,,,,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item2);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item3);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item4);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item5);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item6);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T> t) => t.Item7);

            //
            // global::System.ValueTuple<,,,,,,,>
            //
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item1);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item2);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item3);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item4);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item5);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item6);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Item7);
            valueTupleMembers.Add(static (ValueTuple<T, T, T, T, T, T, T, TValue> t) => t.Rest);
            ValueTuple = valueTupleMembers.ToReadOnly();

            MemberTable mathMembers = new();
            mathMembers.Add(static () => global::System.Math.E);
            mathMembers.Add(static () => global::System.Math.PI);
            mathMembers.Add(static () => global::System.Math.Tau);

            mathMembers.Add(static (double d) => global::System.Math.Acos(d));
            mathMembers.Add(static (double d) => global::System.Math.Asin(d));
            mathMembers.Add(static (double d) => global::System.Math.Atan(d));
            mathMembers.Add(static (double y, double x) => global::System.Math.Atan2(y, x));
            mathMembers.Add(static (decimal d) => global::System.Math.Ceiling(d));
            mathMembers.Add(static (double a) => global::System.Math.Ceiling(a));
            mathMembers.Add(static (double d) => global::System.Math.Cos(d));
            mathMembers.Add(static (double value) => global::System.Math.Cosh(value));
            mathMembers.Add(static (decimal d) => global::System.Math.Floor(d));
            mathMembers.Add(static (double d) => global::System.Math.Floor(d));
            mathMembers.Add(static (double a) => global::System.Math.Sin(a));
            mathMembers.Add(static (double a) => global::System.Math.Tan(a));
            mathMembers.Add(static (double value) => global::System.Math.Sinh(value));
            mathMembers.Add(static (double value) => global::System.Math.Tanh(value));
            mathMembers.Add(static (double a) => global::System.Math.Round(a));
            mathMembers.Add(static (double value, int digits) => global::System.Math.Round(value, digits));
            mathMembers.Add(static (double value, MidpointRounding mode) => global::System.Math.Round(value, mode));
            mathMembers.Add(static (double value, int digits, MidpointRounding mode) => global::System.Math.Round(value, digits, mode));
            mathMembers.Add(static (decimal d) => global::System.Math.Round(d));
            mathMembers.Add(static (decimal d, int decimals) => global::System.Math.Round(d, decimals));
            mathMembers.Add(static (decimal d, MidpointRounding mode) => global::System.Math.Round(d, mode));
            mathMembers.Add(static (decimal d, int decimals, MidpointRounding mode) => global::System.Math.Round(d, decimals, mode));
            mathMembers.Add(static (decimal d) => global::System.Math.Truncate(d));
            mathMembers.Add(static (double d) => global::System.Math.Truncate(d));
            mathMembers.Add(static (double d) => global::System.Math.Sqrt(d));
            mathMembers.Add(static (double d) => global::System.Math.Log(d));
            mathMembers.Add(static (double d) => global::System.Math.Log10(d));
            mathMembers.Add(static (double d) => global::System.Math.Exp(d));
            mathMembers.Add(static (double x, double y) => global::System.Math.Pow(x, y));
            mathMembers.Add(static (double x, double y) => global::System.Math.IEEERemainder(x, y));
            mathMembers.Add(static (sbyte value) => global::System.Math.Abs(value));
            mathMembers.Add(static (short value) => global::System.Math.Abs(value));
            mathMembers.Add(static (int value) => global::System.Math.Abs(value));
            mathMembers.Add(static (long value) => global::System.Math.Abs(value));
            mathMembers.Add(static (float value) => global::System.Math.Abs(value));
            mathMembers.Add(static (double value) => global::System.Math.Abs(value));
            mathMembers.Add(static (decimal value) => global::System.Math.Abs(value));
            mathMembers.Add(static (sbyte val1, sbyte val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (byte val1, byte val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (short val1, short val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (ushort val1, ushort val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (int val1, int val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (uint val1, uint val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (long val1, long val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (ulong val1, ulong val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (float val1, float val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (double val1, double val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (decimal val1, decimal val2) => global::System.Math.Max(val1, val2));
            mathMembers.Add(static (sbyte val1, sbyte val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (byte val1, byte val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (short val1, short val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (ushort val1, ushort val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (int val1, int val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (uint val1, uint val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (long val1, long val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (ulong val1, ulong val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (float val1, float val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (double val1, double val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (decimal val1, decimal val2) => global::System.Math.Min(val1, val2));
            mathMembers.Add(static (double a, double newBase) => global::System.Math.Log(a, newBase));
            mathMembers.Add(static (sbyte value) => global::System.Math.Sign(value));
            mathMembers.Add(static (short value) => global::System.Math.Sign(value));
            mathMembers.Add(static (int value) => global::System.Math.Sign(value));
            mathMembers.Add(static (long value) => global::System.Math.Sign(value));
            mathMembers.Add(static (float value) => global::System.Math.Sign(value));
            mathMembers.Add(static (double value) => global::System.Math.Sign(value));
            mathMembers.Add(static (decimal value) => global::System.Math.Sign(value));
            mathMembers.Add(static (int a, int b) => global::System.Math.BigMul(a, b));

            mathMembers.Add(static (double d) => global::System.Math.Acosh(d));
            mathMembers.Add(static (double d) => global::System.Math.Asinh(d));
            mathMembers.Add(static (double d) => global::System.Math.Atanh(d));
            mathMembers.Add(static (double d) => global::System.Math.Cbrt(d));
            mathMembers.Add(static (ulong value, ulong min, ulong max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (uint value, uint min, uint max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (ushort value, ushort min, ushort max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (byte value, byte min, byte max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (long value, long min, long max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (int value, int min, int max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (short value, short min, short max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (sbyte value, sbyte min, sbyte max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (double value, double min, double max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (float value, float min, float max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (decimal value, decimal min, decimal max) => global::System.Math.Clamp(value, min, max));
            mathMembers.Add(static (double x) => global::System.Math.BitDecrement(x));
            mathMembers.Add(static (double x) => global::System.Math.BitIncrement(x));
            mathMembers.Add(static (double x) => global::System.Math.ILogB(x));
            mathMembers.Add(static (double x) => global::System.Math.Log2(x));
            mathMembers.Add(static (double x, double y) => global::System.Math.CopySign(x, y));
            mathMembers.Add(static (double x, double y) => global::System.Math.MaxMagnitude(x, y));
            mathMembers.Add(static (double x, double y) => global::System.Math.MinMagnitude(x, y));
            mathMembers.Add(static (double x, int n) => global::System.Math.ScaleB(x, n));
            mathMembers.Add(static (double x, double y, double z) => global::System.Math.FusedMultiplyAdd(x, y, z));
            Math = mathMembers.ToReadOnly();

            MemberTable mathFMembers = new();
            mathFMembers.Add(static () => global::System.MathF.E);
            mathFMembers.Add(static () => global::System.MathF.PI);

            mathFMembers.Add(static (float x) => global::System.MathF.Abs(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Acos(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Acosh(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Asin(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Asinh(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Atan(x));
            mathFMembers.Add(static (float y, float x) => global::System.MathF.Atan2(y, x));
            mathFMembers.Add(static (float x) => global::System.MathF.Atanh(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Cbrt(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Ceiling(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Cos(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Cosh(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Exp(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Floor(x));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.IEEERemainder(x, y));
            mathFMembers.Add(static (float x) => global::System.MathF.Log(x));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.Log(x, y));
            mathFMembers.Add(static (float x) => global::System.MathF.Log10(x));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.Max(x, y));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.Min(x, y));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.Pow(x, y));
            mathFMembers.Add(static (float x) => global::System.MathF.Round(x));
            mathFMembers.Add(static (float x, int digits) => global::System.MathF.Round(x, digits));
            mathFMembers.Add(static (float x, MidpointRounding mode) => global::System.MathF.Round(x, mode));
            mathFMembers.Add(static (float x, int digits, MidpointRounding mode) => global::System.MathF.Round(x, digits, mode));
            mathFMembers.Add(static (float x) => global::System.MathF.Sign(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Sin(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Sinh(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Sqrt(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Tan(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Tanh(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Truncate(x));

            mathFMembers.Add(static () => global::System.MathF.Tau);

            mathFMembers.Add(static (float x) => global::System.MathF.BitDecrement(x));
            mathFMembers.Add(static (float x) => global::System.MathF.BitIncrement(x));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.CopySign(x, y));
            mathFMembers.Add(static (float x, float y, float z) => global::System.MathF.FusedMultiplyAdd(x, y, z));
            mathFMembers.Add(static (float x) => global::System.MathF.ILogB(x));
            mathFMembers.Add(static (float x) => global::System.MathF.Log2(x));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.MaxMagnitude(x, y));
            mathFMembers.Add(static (float x, float y) => global::System.MathF.MinMagnitude(x, y));
            mathFMembers.Add(static (float x, int n) => global::System.MathF.ScaleB(x, n));
            MathF = mathFMembers.ToReadOnly();

            MemberTable bitConverterMembers = new();
            // NB: Omitting GetBytes overloads which return a mutable byte[].

            bitConverterMembers.Add(static (double value) => global::System.BitConverter.DoubleToInt64Bits(value));
            bitConverterMembers.Add(static (long value) => global::System.BitConverter.Int64BitsToDouble(value));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToBoolean(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToChar(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToDouble(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToInt16(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToInt32(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToInt64(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToSingle(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToUInt16(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToUInt32(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToUInt64(value, startIndex));
            bitConverterMembers.Add(static (byte[] value) => global::System.BitConverter.ToString(value));
            bitConverterMembers.Add(static (byte[] value, int startIndex) => global::System.BitConverter.ToString(value, startIndex));
            bitConverterMembers.Add(static (byte[] value, int startIndex, int length) => global::System.BitConverter.ToString(value, startIndex, length));

            bitConverterMembers.Add(static (float value) => global::System.BitConverter.SingleToInt32Bits(value));
            bitConverterMembers.Add(static (int value) => global::System.BitConverter.Int32BitsToSingle(value));
            BitConverter = bitConverterMembers.ToReadOnly();

            MemberTable convertMembers = new();
            convertMembers.Add(static (Object value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToBoolean(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToChar(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToSByte(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToByte(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToInt16(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToUInt16(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToInt32(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToUInt32(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToInt64(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToUInt64(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToSingle(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToDouble(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToDecimal(value));
            convertMembers.Add(static (DateTime value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Object value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (SByte value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Byte value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Int16 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (UInt16 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Int32 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (UInt32 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Int64 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (UInt64 value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Boolean value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Char value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Single value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Double value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (Decimal value) => global::System.Convert.ToDateTime(value));
            convertMembers.Add(static (String value) => global::System.Convert.ToString(value));
            convertMembers.Add(static (byte[] inArray) => global::System.Convert.ToBase64String(inArray));
            convertMembers.Add(static (byte[] inArray, int offset, int length) => global::System.Convert.ToBase64String(inArray, offset, length));
            convertMembers.Add(static (byte[] inArray, int offset, int length, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, offset, length, options));
            convertMembers.Add(static (byte[] inArray, Base64FormattingOptions options) => global::System.Convert.ToBase64String(inArray, options));

            convertMembers.Add(static (byte[] inArray) => global::System.Convert.ToHexString(inArray));
            convertMembers.Add(static (byte[] inArray, int offset, int length) => global::System.Convert.ToHexString(inArray, offset, length));
            Convert = convertMembers.ToReadOnly();

            MemberTable arrayMembers = new();
            // NB: Array.Empty<T>() is omitted because the array return type is considered mutable.

            arrayMembers.Add(static (global::System.Array array) => array.IsFixedSize);
            arrayMembers.Add(static (global::System.Array array) => array.IsReadOnly);
            arrayMembers.Add(static (global::System.Array array) => array.IsSynchronized);
            arrayMembers.Add(static (global::System.Array array) => array.Length);
            arrayMembers.Add(static (global::System.Array array) => array.LongLength);
            arrayMembers.Add(static (global::System.Array array) => array.Rank);

            arrayMembers.Add(static (global::System.Array array, object value) => global::System.Array.BinarySearch(array, value));
            arrayMembers.Add(static (global::System.Array array, int index, int length, object value) => global::System.Array.BinarySearch(array, index, length, value));
            arrayMembers.Add(static (T[] array, T value) => global::System.Array.BinarySearch(array, value));
            arrayMembers.Add(static (T[] array, int index, int length, T value) => global::System.Array.BinarySearch(array, index, length, value));

            arrayMembers.Add(static (global::System.Array array, int dimension) => array.GetLength(dimension));
            arrayMembers.Add(static (global::System.Array array, int dimension) => array.GetLongLength(dimension));

            arrayMembers.Add(static (global::System.Array array, int dimension) => array.GetLowerBound(dimension));
            arrayMembers.Add(static (global::System.Array array, int dimension) => array.GetUpperBound(dimension));

            // NB: GetValue methods are omitted because these return global::System.Object which is not immutable.

            arrayMembers.Add(static (global::System.Array array, object value) => global::System.Array.IndexOf(array, value));
            arrayMembers.Add(static (global::System.Array array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex));
            arrayMembers.Add(static (global::System.Array array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count));
            arrayMembers.Add(static (T[] array, object value) => global::System.Array.IndexOf(array, value));
            arrayMembers.Add(static (T[] array, object value, int startIndex) => global::System.Array.IndexOf(array, value, startIndex));
            arrayMembers.Add(static (T[] array, object value, int startIndex, int count) => global::System.Array.IndexOf(array, value, startIndex, count));

            arrayMembers.Add(static (global::System.Array array, object value) => global::System.Array.LastIndexOf(array, value));
            arrayMembers.Add(static (global::System.Array array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex));
            arrayMembers.Add(static (global::System.Array array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count));
            arrayMembers.Add(static (T[] array, object value) => global::System.Array.LastIndexOf(array, value));
            arrayMembers.Add(static (T[] array, object value, int startIndex) => global::System.Array.LastIndexOf(array, value, startIndex));
            arrayMembers.Add(static (T[] array, object value, int startIndex, int count) => global::System.Array.LastIndexOf(array, value, startIndex, count));

            // NB: Methods with Predicate<T> parameters are ; the delegate may not be a pure function.
            // REVIEW: In practice, it is assumed that predicates are pure. Should we provide a table including these members, so users can opt-in?
            Array = arrayMembers.ToReadOnly();

            MemberTable indexMembers = new();
            indexMembers.Add(static () => global::System.Index.End);
            indexMembers.Add(static () => global::System.Index.Start);

            indexMembers.Add(static (global::System.Index i) => i.IsFromEnd);
            indexMembers.Add(static (global::System.Index i) => i.Value);

            indexMembers.Add(static (int value, bool fromEnd) => new global::System.Index(value, fromEnd));

            indexMembers.Add(static (int value) => (global::System.Index)value);

            indexMembers.Add(static (int value) => global::System.Index.FromEnd(value));
            indexMembers.Add(static (int value) => global::System.Index.FromStart(value));

            indexMembers.Add(static (global::System.Index i, int length) => i.GetOffset(length));

            indexMembers.Add(static (global::System.Index i, global::System.Index other) => i.Equals(other));
            Index = indexMembers.ToReadOnly();

            MemberTable rangeMembers = new();
            rangeMembers.Add(static () => global::System.Range.All);

            rangeMembers.Add(static (global::System.Index start, global::System.Index end) => new global::System.Range(start, end));

            rangeMembers.Add(static (global::System.Index start) => global::System.Range.StartAt(start));
            rangeMembers.Add(static (global::System.Index end) => global::System.Range.EndAt(end));

            rangeMembers.Add(static (global::System.Range r) => r.End);
            rangeMembers.Add(static (global::System.Range r) => r.Start);

            // NB: GetOffsetAndLength is omitted because the ValueTuple<,> return type is not immutable.

            rangeMembers.Add(static (global::System.Range r, global::System.Range other) => r.Equals(other));
            Range = rangeMembers.ToReadOnly();
#pragma warning restore IDE0028 // Simplify collection initialization

            AllThisNamespaceOnly = new MemberTable
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

            AllThisAndChildNamespaces = new MemberTable
            {
                AllThisNamespaceOnly,

                Collections.AllThisAndChildNamespaces,
                Text.AllThisAndChildNamespaces,
            }.ToReadOnly();
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
                    static (T t1, T t2) => new global::System.Collections.Generic.KeyValuePair<T, T>(t1, t2),

                    static (global::System.Collections.Generic.KeyValuePair<T, T> t) => t.Key,
                    static (global::System.Collections.Generic.KeyValuePair<T, T> t) => t.Value,
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
                    regexMembers.Add(static (string pattern) => new global::System.Text.RegularExpressions.Regex(pattern));
                    regexMembers.Add(static (string pattern, global::System.Text.RegularExpressions.RegexOptions options) => new global::System.Text.RegularExpressions.Regex(pattern, options));
                    regexMembers.Add(static (string pattern, global::System.Text.RegularExpressions.RegexOptions options, TimeSpan matchTimeout) => new global::System.Text.RegularExpressions.Regex(pattern, options, matchTimeout));

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r) => r.MatchTimeout);
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r) => r.Options);
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r) => r.RightToLeft);

                    regexMembers.Add(static (string str) => global::System.Text.RegularExpressions.Regex.Escape(str));

                    // NB: GetGroupNames and GetGroupNumbers are omitted; these return mutable arrays.

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, int i) => r.GroupNameFromNumber(i));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string name) => r.GroupNumberFromName(name));

                    regexMembers.Add(static (string input, string pattern) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern));
                    regexMembers.Add(static (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern, options));
                    regexMembers.Add(static (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.IsMatch(input, pattern, options, matchTimeout));

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input) => r.IsMatch(input));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.IsMatch(input, startat));

                    regexMembers.Add(static (string input, string pattern) => global::System.Text.RegularExpressions.Regex.Match(input, pattern));
                    regexMembers.Add(static (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Match(input, pattern, options));
                    regexMembers.Add(static (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Match(input, pattern, options, matchTimeout));

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input) => r.Match(input));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.Match(input, startat));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, int beginning, int length) => r.Match(input, beginning, length));

                    regexMembers.Add(static (string input, string pattern) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern));
                    regexMembers.Add(static (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern, options));
                    regexMembers.Add(static (string input, string pattern, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Matches(input, pattern, options, matchTimeout));

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input) => r.Matches(input));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, int startat) => r.Matches(input, startat));

                    regexMembers.Add(static (string input, string pattern, string replacement) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement));
                    regexMembers.Add(static (string input, string pattern, string replacement, global::System.Text.RegularExpressions.RegexOptions options) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options));
                    regexMembers.Add(static (string input, string pattern, string replacement, global::System.Text.RegularExpressions.RegexOptions options, global::System.TimeSpan matchTimeout) => global::System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement, options, matchTimeout));

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, string replacement) => r.Replace(input, replacement));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, string replacement, int count) => r.Replace(input, replacement, count));
                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r, string input, string replacement, int count, int startat) => r.Replace(input, replacement, count, startat));

                    // NB: Replace overloads with MatchEvaluator are omitted; the delegate may not be a pure function.
                    // REVIEW: In practice, it is assumed that the match evaluator is pure. Should we provide a table including these members, so users can opt-in?

                    // NB: Split overloads are omitted; these return mutable arrays.

                    regexMembers.Add(static (global::System.Text.RegularExpressions.Regex r) => r.ToString());

                    regexMembers.Add(static (string str) => global::System.Text.RegularExpressions.Regex.Unescape(str));
                    Regex = regexMembers.ToReadOnly();

                    MemberTable matchCollectionMembers = new();
                    matchCollectionMembers.Add(static (global::System.Text.RegularExpressions.MatchCollection c) => c.Count);
                    matchCollectionMembers.Add(static (global::System.Text.RegularExpressions.MatchCollection c) => c.IsReadOnly);
                    matchCollectionMembers.Add(static (global::System.Text.RegularExpressions.MatchCollection c) => c.IsSynchronized);

                    matchCollectionMembers.Add(static (global::System.Text.RegularExpressions.MatchCollection c, int i) => c[i]);
                    MatchCollection = matchCollectionMembers.ToReadOnly();

                    MemberTable matchMembers = new();
                    matchMembers.Add(static () => global::System.Text.RegularExpressions.Match.Empty);

                    matchMembers.Add(static (global::System.Text.RegularExpressions.Match m) => m.Groups);

                    matchMembers.Add(static (global::System.Text.RegularExpressions.Match m) => m.NextMatch());
                    matchMembers.Add(static (global::System.Text.RegularExpressions.Match m, string replacement) => m.Result(replacement)); // NB: Virtual but only internal derivees are allowed.
                    Match = matchMembers.ToReadOnly();

                    MemberTable groupCollectionMembers = new();
                    groupCollectionMembers.Add(static (global::System.Text.RegularExpressions.GroupCollection g) => g.Count);
                    groupCollectionMembers.Add(static (global::System.Text.RegularExpressions.GroupCollection g) => g.IsReadOnly);
                    groupCollectionMembers.Add(static (global::System.Text.RegularExpressions.GroupCollection g) => g.IsSynchronized);

                    groupCollectionMembers.Add(static (global::System.Text.RegularExpressions.GroupCollection g, int groupnum) => g[groupnum]);
                    groupCollectionMembers.Add(static (global::System.Text.RegularExpressions.GroupCollection g, string groupname) => g[groupname]);
                    GroupCollection = groupCollectionMembers.ToReadOnly();

                    MemberTable groupMembers = new();
                    groupMembers.Add(static (global::System.Text.RegularExpressions.Group g) => g.Name);
                    groupMembers.Add(static (global::System.Text.RegularExpressions.Group g) => g.Success);
                    groupMembers.Add(static (global::System.Text.RegularExpressions.Group g) => g.Captures);
                    Group = groupMembers.ToReadOnly();

                    MemberTable captureCollectionMembers = new();
                    captureCollectionMembers.Add(static (global::System.Text.RegularExpressions.CaptureCollection c) => c.Count);
                    captureCollectionMembers.Add(static (global::System.Text.RegularExpressions.CaptureCollection c) => c.IsReadOnly);
                    captureCollectionMembers.Add(static (global::System.Text.RegularExpressions.CaptureCollection c) => c.IsSynchronized);

                    captureCollectionMembers.Add(static (global::System.Text.RegularExpressions.CaptureCollection c, int i) => c[i]);
                    CaptureCollection = captureCollectionMembers.ToReadOnly();

                    MemberTable captureMembers = new();
                    captureMembers.Add(static (global::System.Text.RegularExpressions.Capture c) => c.Index);
                    captureMembers.Add(static (global::System.Text.RegularExpressions.Capture c) => c.Length);
                    captureMembers.Add(static (global::System.Text.RegularExpressions.Capture c) => c.Value);

                    captureMembers.Add(static (global::System.Text.RegularExpressions.Capture c) => c.ToString());
                    Capture = captureMembers.ToReadOnly();

#pragma warning restore IDE0028 // Simplify collection initialization

                    AllThisNamespaceOnly = new MemberTable
                    {
                        Regex,
                        MatchCollection,
                        Match,
                        GroupCollection,
                        Group,
                        CaptureCollection,
                        Capture,
                    }.ToReadOnly();

                    AllThisAndChildNamespaces = new MemberTable
                    {
                        AllThisNamespaceOnly,
                    }.ToReadOnly();
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
                public static MemberTable AllThisNamespaceOnly { get; }

                /// <summary>
                /// Gets a table of pure members in the System.Text.RegularExpressions namespace and any child namespace.
                /// </summary>
                public static MemberTable AllThisAndChildNamespaces { get; }
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
        public static MemberTable AllThisNamespaceOnly { get; }

        /// <summary>
        /// Gets a table of pure members in the System namespace.
        /// </summary>
        public static MemberTable AllThisAndChildNamespaces { get; }
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
