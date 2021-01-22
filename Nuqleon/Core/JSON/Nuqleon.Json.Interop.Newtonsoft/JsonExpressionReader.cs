// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Nuqleon.Json.Expressions;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Numerics;
using System.Threading;

namespace Nuqleon.Json.Interop.Newtonsoft
{
    /// <summary>
    /// Represents a JSON reader over an <see cref="Expression"/> object.
    /// </summary>
    public sealed class JsonExpressionReader : JsonReader
    {
        private const NumberStyles Styles = NumberStyles.Float; // REVIEW

        private readonly JsonInteropResourcePool _pool;
        private TokenStack _tokens;

        /// <summary>
        /// Creates a new JSON reader over the specified <paramref name="json"/> expression.
        /// </summary>
        /// <param name="json">The JSON expression to read.</param>
        public JsonExpressionReader(Expression json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            _tokens = new TokenStack();
            _tokens.Push(new Token { Expression = json });
        }

        /// <summary>
        /// Creates a new JSON reader over the specified <paramref name="json"/> expression.
        /// </summary>
        /// <param name="json">The JSON expression to read.</param>
        /// <param name="pool">Resource pool to use for reuse of commonly allocated data structures.</param>
        public JsonExpressionReader(Expression json, JsonInteropResourcePool pool)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _tokens = _pool.Pool.Allocate();
            _tokens.Push(new Token { Expression = json });
        }

        /// <summary>
        /// Disposes the JSON expression reader.
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
        /// Reads the next JSON token from the expression.
        /// </summary>
        /// <returns>true if the next token was read successfully; false if there are no more tokens to read.</returns>
        public override bool Read()
        {
            if (_tokens == null)
                throw new ObjectDisposedException("this");

            if (!_tokens.TryPop(out Token token))
            {
                SetToken(JsonToken.None);
                return false;
            }

            Expression expr;
            if (token.Property != null)
            {
                SetToken(JsonToken.PropertyName, token.Property);
            }
            else if ((expr = token.Expression) != null)
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.Null:
                        SetToken(JsonToken.Null);
                        break;
                    case ExpressionType.Boolean:
                        SetToken(JsonToken.Boolean, ((ConstantExpression)expr).Value);
                        break;
                    case ExpressionType.Array:
                        {
                            var arr = (ArrayExpression)expr;
                            var idx = _tokens.Count;
                            _tokens.Push(arr.ElementCount + 1);
                            _tokens[idx++] = new Token { JsonToken = JsonToken.EndArray };
                            for (int i = arr.ElementCount - 1; i >= 0; i--)
                            {
                                _tokens[idx++] = new Token { Expression = arr.GetElement(i) };
                            }
                            SetToken(JsonToken.StartArray);
                        }
                        break;
                    case ExpressionType.String:
                        SetToken(JsonToken.String, ((ConstantExpression)expr).Value);
                        break;
                    case ExpressionType.Number:
                        var str = (string)((ConstantExpression)expr).Value;
#if NET5_0
                        if (str.Contains('.', StringComparison.Ordinal))
#else
                        if (str.IndexOf('.') >= 0)
#endif
                        {
                            if (FloatParseHandling == FloatParseHandling.Double)
                            {
                                if (double.TryParse(str, Styles, CultureInfo.InvariantCulture, out double dbl))
                                {
                                    SetToken(JsonToken.Float, dbl);
                                }
                                else
                                {
                                    throw new JsonReaderException(string.Format(CultureInfo.InvariantCulture, "Input string '{0}' is not a valid double.", str));
                                }
                            }
                            else
                            {
                                if (decimal.TryParse(str, Styles, CultureInfo.InvariantCulture, out decimal dec))
                                {
                                    SetToken(JsonToken.Float, dec);
                                }
                                else
                                {
                                    throw new JsonReaderException(string.Format(CultureInfo.InvariantCulture, "Input string '{0}' is not a valid decimal.", str));
                                }
                            }
                        }
                        else
                        {
                            // PERF: Consider using an optimized parsing algorithm here.

                            if (long.TryParse(str, out long val64))
                            {
                                SetToken(JsonToken.Integer, val64);
                            }
                            else
                            {
                                if (BigInteger.TryParse(str, out BigInteger big))
                                {
                                    SetToken(JsonToken.Integer, big);
                                }
                                else
                                {
                                    throw new JsonReaderException(string.Format(CultureInfo.InvariantCulture, "Input string '{0}' is not a valid integer.", str));
                                }
                            }
                        }
                        break;
                    case ExpressionType.Object:
                        {
                            var obj = (ObjectExpression)expr;
                            var members = obj.Members;
                            var propAndValueCount = members.Count * 2;
                            var idx = _tokens.Count + propAndValueCount;
                            _tokens.Push(propAndValueCount + 1);
                            foreach (var member in obj.Members) // NB: Unfortunately, we got an enumerator allocation here
                            {
                                _tokens[idx--] = new Token { Property = member.Key };
                                _tokens[idx--] = new Token { Expression = member.Value };
                            }
                            _tokens[idx] = new Token { JsonToken = JsonToken.EndObject };
                            SetToken(JsonToken.StartObject);
                        }
                        break;
                }
            }
            else
            {
                SetToken(token.JsonToken);
            }

            return true;
        }
    }
}
