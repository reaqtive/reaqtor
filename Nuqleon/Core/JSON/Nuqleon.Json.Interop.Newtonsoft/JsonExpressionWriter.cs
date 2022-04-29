// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using Newtonsoft.Json;

using Nuqleon.Json.Expressions;

namespace Nuqleon.Json.Interop.Newtonsoft
{
    /// <summary>
    /// Represents a JSON writer to produce an <see cref="Expressions.Expression"/> object.
    /// </summary>
    public sealed class JsonExpressionWriter : JsonWriter
    {
        //
        // NB: The algorithm used here is akin to a fairly standard shift/reduce parser. All Write
        //     operations except for WriteEndObject and WriteEndArray push tokens onto the token
        //     stack. The End methods reduce the stack by popping the array elements or the object
        //     property/value pairs, and push the built JSON object in lieu of the Start token.
        //
        //     Example:
        //
        //              Input                    Token stack
        //       ===================   ==================================
        //       WriteStartArray()     [StartArray]
        //       WriteValue(1)         [StartArray, Number(1)]
        //       WriteValue(2)         [StartArray, Number(1), Number(2)]
        //       WriteEndArray()       [Array(Number(1), Number(2))]
        //

        private readonly JsonInteropResourcePool _pool;
        private TokenStack _tokens;

        /// <summary>
        /// Creates a new JSON expression writer.
        /// </summary>
        public JsonExpressionWriter() => _tokens = new TokenStack();

        /// <summary>
        /// Creates a new JSON expression writer.
        /// </summary>
        /// <param name="pool">Resource pool to use for reuse of commonly allocated data structures.</param>
        public JsonExpressionWriter(JsonInteropResourcePool pool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _tokens = _pool.Pool.Allocate();
        }

        /// <summary>
        /// Gets the expression that was built.
        /// </summary>
        public Expression Expression
        {
            get
            {
                CheckDisposed();

                var count = _tokens.Count;

                if (count == 0)
                    throw new InvalidOperationException("No JSON expression object has been built yet.");

                if (count > 1)
                    throw new InvalidOperationException("No final JSON expression object has been produced. This could indicate the written JSON token sequence is incomplete or invalid.");

                return _tokens[0].Expression;
            }
        }

        /// <summary>
        /// Flushes the writer.
        /// </summary>
        public override void Flush()
        {
            // NB: No work to do here.
        }

        /// <summary>
        /// Disposes the JSON expression writer.
        /// </summary>
        /// <param name="disposing">Indicates if the method was called from the Dispose method or a finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            // NB: This class is sealed and the base class doesn't have a finalizer, so disposing should
            //     never be false. However, even if there was a finalizer, it's okay to return the stack
            //     to the pool unconditionally, provided it's done only once, hence the use of Exchange.

            var tokens = Interlocked.Exchange(ref _tokens, null);
            if (tokens != null)
            {
                _pool?.Pool.Free(tokens);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Writes a null value.
        /// </summary>
        public override void WriteNull()
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Null() });
        }

        /// <summary>
        /// Writes a <see cref="bool" /> value.
        /// </summary>
        /// <param name="value">The <see cref="bool" /> value to write.</param>
        public override void WriteValue(bool value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Boolean(value) });
        }

        /// <summary>
        /// Writes a <see cref="decimal" /> value.
        /// </summary>
        /// <param name="value">The <see cref="decimal" /> value to write.</param>
        public override void WriteValue(decimal value)
        {
            CheckDisposed();

            // NB: Use of `null` formatter matches Newtonsoft behavior.
            var str = EnsureDecimalPlace(value.ToString(format: null, CultureInfo.InvariantCulture));

            _tokens.Push(new Token { Expression = Expression.Number(str) });
        }

        /// <summary>
        /// Writes a <see cref="double" /> value.
        /// </summary>
        /// <param name="value">The <see cref="double" /> value to write.</param>
        public override void WriteValue(double value)
        {
            CheckDisposed();

            // NB: Use of "R" formatter matches Newtonsoft behavior.
            var str = value.ToString("R", CultureInfo.InvariantCulture);

            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                switch (FloatFormatHandling)
                {
                    case FloatFormatHandling.DefaultValue:
                        _tokens.Push(new Token { Expression = Expression.Number("0.0") });
                        break;
                    case FloatFormatHandling.String:
                        _tokens.Push(new Token { Expression = Expression.String(str) });
                        break;
                    case FloatFormatHandling.Symbol:
                        throw new NotSupportedException("FloatFormatHandling.Symbol is not supported.");
                }
            }
            else
            {
                str = EnsureDecimalPlace(str);
                _tokens.Push(new Token { Expression = Expression.Number(str) });
            }
        }

        /// <summary>
        /// Writes a <see cref="Nullable{Double}" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Double}" /> value to write.</param>
        public override void WriteValue(double? value)
        {
            CheckDisposed();

            if (value == null)
            {
                _tokens.Push(new Token { Expression = Expression.Null() });
            }
            else
            {
                var val = value.Value;

                if (double.IsNaN(val) || double.IsInfinity(val))
                {
                    switch (FloatFormatHandling)
                    {
                        case FloatFormatHandling.DefaultValue:
                            _tokens.Push(new Token { Expression = Expression.Null() });
                            break;
                        case FloatFormatHandling.String:
                            // NB: Use of "R" formatter matches Newtonsoft behavior.
                            var str = val.ToString("R", CultureInfo.InvariantCulture);
                            _tokens.Push(new Token { Expression = Expression.String(str) });
                            break;
                        case FloatFormatHandling.Symbol:
                            throw new NotSupportedException("FloatFormatHandling.Symbol is not supported.");
                    }
                }
                else
                {
                    WriteValue(value.Value);
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="float" /> value.
        /// </summary>
        /// <param name="value">The <see cref="float" /> value to write.</param>
        public override void WriteValue(float value)
        {
            CheckDisposed();

            // NB: Use of "R" formatter matches Newtonsoft behavior.
            var str = value.ToString("R", CultureInfo.InvariantCulture);

            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                switch (FloatFormatHandling)
                {
                    case FloatFormatHandling.DefaultValue:
                        _tokens.Push(new Token { Expression = Expression.Number("0.0") });
                        break;
                    case FloatFormatHandling.String:
                        _tokens.Push(new Token { Expression = Expression.String(str) });
                        break;
                    case FloatFormatHandling.Symbol:
                        throw new NotSupportedException("FloatFormatHandling.Symbol is not supported.");
                }
            }
            else
            {
                str = EnsureDecimalPlace(str);
                _tokens.Push(new Token { Expression = Expression.Number(str) });
            }
        }

        /// <summary>
        /// Writes a <see cref="Nullable{Single}" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Nullable{Single}" /> value to write.</param>
        public override void WriteValue(float? value)
        {
            CheckDisposed();

            if (value == null)
            {
                _tokens.Push(new Token { Expression = Expression.Null() });
            }
            else
            {
                var val = value.Value;

                if (float.IsNaN(val) || float.IsInfinity(val))
                {
                    switch (FloatFormatHandling)
                    {
                        case FloatFormatHandling.DefaultValue:
                            _tokens.Push(new Token { Expression = Expression.Null() });
                            break;
                        case FloatFormatHandling.String:
                            // NB: Use of "R" formatter matches Newtonsoft behavior.
                            var str = val.ToString("R", CultureInfo.InvariantCulture);
                            _tokens.Push(new Token { Expression = Expression.String(str) });
                            break;
                        case FloatFormatHandling.Symbol:
                            throw new NotSupportedException("FloatFormatHandling.Symbol is not supported.");
                    }
                }
                else
                {
                    WriteValue(value.Value);
                }
            }
        }

        /// <summary>
        /// Writes a <see cref="byte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="byte" /> value to write.</param>
        public override void WriteValue(byte value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="int" /> value.
        /// </summary>
        /// <param name="value">The <see cref="int" /> value to write.</param>
        public override void WriteValue(int value)
        {
            CheckDisposed();

            // PERF: Avoid allocation of strings for small values.

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The <see cref="long" /> value to write.</param>
        public override void WriteValue(long value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="sbyte" /> value.
        /// </summary>
        /// <param name="value">The <see cref="sbyte" /> value to write.</param>
        [CLSCompliant(false)]
        public override void WriteValue(sbyte value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="short" /> value.
        /// </summary>
        /// <param name="value">The <see cref="short" /> value to write.</param>
        public override void WriteValue(short value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="string" /> value.
        /// </summary>
        /// <param name="value">The <see cref="string" /> value to write.</param>
        public override void WriteValue(string value)
        {
            CheckDisposed();

            if (value == null)
            {
                _tokens.Push(new Token { Expression = Expression.Null() });
            }
            else
            {
                _tokens.Push(new Token { Expression = Expression.String(value) });
            }
        }

        /// <summary>
        /// Writes a <see cref="uint" /> value.
        /// </summary>
        /// <param name="value">The <see cref="uint" /> value to write.</param>
        [CLSCompliant(false)]
        public override void WriteValue(uint value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="ulong" /> value.
        /// </summary>
        /// <param name="value">The <see cref="ulong" /> value to write.</param>
        [CLSCompliant(false)]
        public override void WriteValue(ulong value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="ushort" /> value.
        /// </summary>
        /// <param name="value">The <see cref="ushort" /> value to write.</param>
        [CLSCompliant(false)]
        public override void WriteValue(ushort value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.Number(value.ToString(CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="char" /> value.
        /// </summary>
        /// <param name="value">The <see cref="char" /> value to write.</param>
        public override void WriteValue(char value)
        {
            CheckDisposed();

            _tokens.Push(new Token { Expression = Expression.String(char.ToString(value)) });
        }

        /// <summary>
        /// Writes a <see cref="TimeSpan" /> value.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan" /> value to write.</param>
        public override void WriteValue(TimeSpan value)
        {
            CheckDisposed();

            // NB: Use of `null` formatter matches Newtonsoft behavior.
            _tokens.Push(new Token { Expression = Expression.String(value.ToString(format: null, CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="Guid" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Guid" /> value to write.</param>
        public override void WriteValue(Guid value)
        {
            CheckDisposed();

            // NB: Use of "D" formatter matches Newtonsoft behavior.
            _tokens.Push(new Token { Expression = Expression.String(value.ToString("D", CultureInfo.InvariantCulture)) });
        }

        /// <summary>
        /// Writes a <see cref="Uri" /> value.
        /// </summary>
        /// <param name="value">The <see cref="Uri" /> value to write.</param>
        public override void WriteValue(Uri value)
        {
            CheckDisposed();

            // NB: Use of OriginalString matches Newtonsoft behavior.
            _tokens.Push(new Token { Expression = Expression.String(value?.OriginalString) });
        }

        /// <summary>
        /// Writes a <see cref="DateTime" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime" /> value to write.</param>
        public override void WriteValue(DateTime value)
        {
            CheckDisposed();

            value = EnsureDateTime(value);

            var str = !string.IsNullOrEmpty(DateFormatString)
                ? value.ToString(DateFormatString, Culture)
                : ToDateTimeString(value, null, value.Kind);

            _tokens.Push(new Token { Expression = Expression.String(str) });
        }

        /// <summary>
        /// Writes a <see cref="DateTimeOffset" /> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset" /> value to write.</param>
        public override void WriteValue(DateTimeOffset value)
        {
            CheckDisposed();

            var str = !string.IsNullOrEmpty(DateFormatString)
                ? value.ToString(DateFormatString, Culture)
                : ToDateTimeString(value.DateTime, value.Offset, DateTimeKind.Local);

            _tokens.Push(new Token { Expression = Expression.String(str) });
        }

        /// <summary>
        /// Writes the beginning of a JSON array.
        /// </summary>
        public override void WriteStartArray()
        {
            CheckDisposed();

            _tokens.Push(new Token { JsonToken = JsonToken.StartArray });
        }

        /// <summary>
        /// Writes the end of an array.
        /// </summary>
        public override void WriteEndArray()
        {
            CheckDisposed();

            var startIndex = _tokens.Count - 1;

            while (startIndex >= 0 && _tokens[startIndex].JsonToken != JsonToken.StartArray)
            {
                startIndex--;
            }

            if (startIndex < 0)
                throw new InvalidOperationException("No matching 'StartArray' token found.");

            var len = _tokens.Count - startIndex - 1;
            Debug.Assert(len >= 0);

            // PERF: Consider optimized behavior for len == 0 and/or small values of len.

            var exprs = new Expression[len];

            for (int i = startIndex + 1, j = 0; i < _tokens.Count; i++, j++)
            {
                var expression = _tokens[i].Expression;
                exprs[j] = expression ?? throw new InvalidOperationException("Expected an expression on the token stack.");
            }

            _tokens.Pop(len);

            Debug.Assert(_tokens[startIndex].JsonToken == JsonToken.StartArray);
            _tokens[startIndex] = new Token { Expression = Expression.Array(exprs) };
        }

        /// <summary>
        /// Writes the beginning of a JSON object.
        /// </summary>
        public override void WriteStartObject()
        {
            CheckDisposed();

            _tokens.Push(new Token { JsonToken = JsonToken.StartObject });
        }

        /// <summary>
        /// Writes the property name of a name/value pair on a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public override void WritePropertyName(string name)
        {
            CheckDisposed();

            _tokens.Push(new Token { Property = name });
        }

        /// <summary>
        /// Writes the property name of a name/value pair on a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="escape">A flag to indicate whether the text should be escaped when it is written as a JSON property name.</param>
        public override void WritePropertyName(string name, bool escape)
        {
            // NB: Nuqleon.Json will handle escaping, so don't do it here.
            WritePropertyName(name);
        }

        /// <summary>
        /// Writes the end of a JSON object.
        /// </summary>
        public override void WriteEndObject()
        {
            CheckDisposed();

            var startIndex = _tokens.Count - 1;

            while (startIndex >= 0 && _tokens[startIndex].JsonToken != JsonToken.StartObject)
            {
                startIndex--;
            }

            if (startIndex < 0)
                throw new InvalidOperationException("No matching 'StartObject' token found.");

            var len = _tokens.Count - startIndex - 1;
            Debug.Assert(len >= 0);

            // PERF: Consider optimized behavior for len == 0 and/or small values of len.

            var exprs = new Dictionary<string, Expression>(len / 2);

            for (var i = startIndex + 1; i < _tokens.Count; i += 2)
            {
                var property = _tokens[i].Property;

                if (property == null)
                    throw new InvalidOperationException("Expected 'PropertyName' token.");

                var expression = _tokens[i + 1].Expression;
                exprs[property] = expression ?? throw new InvalidOperationException("Expected an expression on the token stack.");
            }

            _tokens.Pop(len);

            Debug.Assert(_tokens[startIndex].JsonToken == JsonToken.StartObject);
            _tokens[startIndex] = new Token { Expression = Expression.Object(exprs) };
        }

        /// <summary>
        /// Writes a <see cref="byte" />[] value.
        /// </summary>
        /// <param name="value">The <see cref="byte" />[] value to write.</param>
        /// <remarks>This is not supported by <see cref="JsonExpressionWriter"/>.</remarks>
        public override void WriteValue(byte[] value) => throw new NotSupportedException("Writing an object of type 'byte[]' is not supported.");

        /// <summary>
        /// Writes out a comment <code>/*...*/</code> containing the specified text.
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        /// <remarks>This is not supported by <see cref="JsonExpressionWriter"/>.</remarks>
        public override void WriteComment(string text) => throw new NotSupportedException("Writing a comment is not supported.");

        /// <summary>
        /// Writes the end constructor.
        /// </summary>
        /// <remarks>This is not supported by <see cref="JsonExpressionWriter"/>.</remarks>
        public override void WriteEndConstructor() => throw new NotSupportedException("Writing a constructor is not supported.");

        /// <summary>
        /// Writes the start of a constructor with the given name.
        /// </summary>
        /// <param name="name">The name of the constructor.</param>
        /// <remarks>This is not supported by <see cref="JsonExpressionWriter"/>.</remarks>
        public override void WriteStartConstructor(string name) => throw new NotSupportedException("Writing a constructor is not supported.");

        /// <summary>
        /// Writes an undefined value.
        /// </summary>
        /// <remarks>This is not supported by <see cref="JsonExpressionWriter"/>.</remarks>
        public override void WriteUndefined() => throw new NotSupportedException("Writing an undefined value is not supported.");

        private void CheckDisposed()
        {
            if (_tokens == null)
                throw new ObjectDisposedException("this");
        }

        private static string EnsureDecimalPlace(string text)
        {
            // NB: This helper is modified from Newtonsoft JSON.

#if NET6_0 || NETSTANDARD2_1
            if (text.Contains('.', StringComparison.Ordinal) || text.Contains('E', StringComparison.Ordinal) || text.Contains('e', StringComparison.Ordinal))
#else
            if (text.IndexOf('.') >= 0 || text.IndexOf('E') >= 0 || text.IndexOf('e') >= 0)
#endif
            {
                return text;
            }

            return text + ".0";
        }

        private DateTime EnsureDateTime(DateTime value)
        {
            // NB: This helper is modified from Newtonsoft JSON.

            switch (DateTimeZoneHandling)
            {
                case DateTimeZoneHandling.Local:
                    value = SwitchToLocalTime(value);
                    break;
                case DateTimeZoneHandling.Utc:
                    value = SwitchToUtcTime(value);
                    break;
                case DateTimeZoneHandling.Unspecified:
                    value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
                    break;
            }

            return value;
        }

        private static DateTime SwitchToLocalTime(DateTime value)
        {
            // NB: This helper is modified from Newtonsoft JSON.

            return value.Kind switch
            {
                DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Local),
                DateTimeKind.Utc => value.ToLocalTime(),
                _ => value,
            };
        }

        private static DateTime SwitchToUtcTime(DateTime value)
        {
            // NB: This helper is modified from Newtonsoft JSON.

            return value.Kind switch
            {
                DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Utc),
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => value,
            };
        }

        private string ToDateTimeString(DateTime value, TimeSpan? offset, DateTimeKind kind)
        {
            if (DateFormatHandling == DateFormatHandling.MicrosoftDateFormat)
            {
                throw new NotSupportedException("DateFormatHandling.MicrosoftDateFormat is not supported.");
            }

            const int YearDayMonthHourMinuteSecond =
                /* year */ 4 + /* - */ 1 +
                /* month */ 2 + /* - */ 1 +
                /* day */ 2 + /* T */ 1 +
                /* hour */2 + /* : */ 1 +
                /* minute */ 2 + /* : */ 1 +
                /* second */ 2;

            var len = YearDayMonthHourMinuteSecond;

            int ticks = (int)(value.Ticks % 10000000L);
            int ticksLength = 0;

            if (ticks != 0)
            {
                ticksLength = 7;

                while (ticks % 10 == 0)
                {
                    ticksLength--;
                    ticks /= 10;
                }

                len += /* . */ 1 + ticksLength;
            }

            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    break;
                case DateTimeKind.Local:
                    len += /* +|- */ 1 + /* hour */ 2 + /* : */ 1 + /* minute */ 2;
                    break;
                case DateTimeKind.Utc:
                    len += /* Z */ 1;
                    break;
            }

            var chars = new char[len];

            var i = 0;
            EmitInt32PadFour(chars, ref i, value.Year);
            chars[i++] = '-';
            EmitInt32PadTwo(chars, ref i, value.Month);
            chars[i++] = '-';
            EmitInt32PadTwo(chars, ref i, value.Day);
            chars[i++] = 'T';
            EmitInt32PadTwo(chars, ref i, value.Hour);
            chars[i++] = ':';
            EmitInt32PadTwo(chars, ref i, value.Minute);
            chars[i++] = ':';
            EmitInt32PadTwo(chars, ref i, value.Second);

            if (ticksLength > 0)
            {
                chars[i++] = '.';

                var digit = ticksLength;
                while (digit-- != 0)
                {
                    chars[i + digit] = (char)('0' + ticks % 10);
                    ticks /= 10;
                }

                i += ticksLength;
            }

            switch (kind)
            {
                case DateTimeKind.Local:
                    var o = offset ?? GetUtcOffset(value);
                    chars[i++] = o.Ticks >= 0L ? '+' : '-';
                    EmitInt32PadTwo(chars, ref i, Math.Abs(o.Hours));
                    chars[i++] = ':';
                    EmitInt32PadTwo(chars, ref i, Math.Abs(o.Minutes));
                    break;
                case DateTimeKind.Utc:
                    chars[i++] = 'Z';
                    break;
            }

            Debug.Assert(i == len);

            return new string(chars);
        }

        private static void EmitInt32PadFour(char[] chars, ref int index, int value)
        {
            Debug.Assert(value is >= 0 and <= 9999);

            var div = Math.DivRem(value, 1000, out value);
            chars[index++] = (char)('0' + div);

            div = Math.DivRem(value, 100, out value);
            chars[index++] = (char)('0' + div);

            div = Math.DivRem(value, 10, out value);
            chars[index++] = (char)('0' + div);
            chars[index++] = (char)('0' + value);
        }

        private static void EmitInt32PadTwo(char[] chars, ref int index, int value)
        {
            Debug.Assert(value is >= 0 and <= 99);

            var div = Math.DivRem(value, 10, out value);
            chars[index++] = (char)('0' + div);
            chars[index++] = (char)('0' + value);
        }

        private static TimeSpan GetUtcOffset(DateTime d) => TimeZoneInfo.Local.GetUtcOffset(d);
    }
}
