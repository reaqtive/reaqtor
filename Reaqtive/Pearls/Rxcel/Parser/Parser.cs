// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Reactive Excel implementation using classic Rx to demonstrate the concepts around
// building reactive computational graphs.
//

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rxcel
{
    internal sealed class Parser
    {
        private Token[] _tokens;
        private int _i;

        public ExcelExpression Parse(string expression)
        {
            _tokens = Lex(expression).ToArray();
            _i = 0;

            return Expr();
        }

        private Token Peek()
        {
            if (_i >= _tokens.Length)
            {
                return null;
            }

            return _tokens[_i];
        }

        private Token Read()
        {
            if (_i >= _tokens.Length)
            {
                return null;
            }

            return _tokens[_i++];
        }

        private ExcelExpression Expr()
        {
            var res = Term();

            var next = Peek();
            if (next != null)
            {
                switch (next.Kind)
                {
                    case TokenKind.Plus:
                        Read();
                        res = ExcelExpression.Add(res, Expr());
                        break;
                    case TokenKind.Dash:
                        Read();
                        res = ExcelExpression.Subtract(res, Expr());
                        break;
                }
            }

            return res;
        }

        private ExcelExpression Term()
        {
            var res = Factor();

            var next = Peek();
            if (next != null)
            {
                switch (next.Kind)
                {
                    case TokenKind.Star:
                        Read();
                        res = ExcelExpression.Multiply(res, Expr());
                        break;
                    case TokenKind.Slash:
                        Read();
                        res = ExcelExpression.Divide(res, Expr());
                        break;
                    case TokenKind.Percent:
                        Read();
                        res = ExcelExpression.Modulo(res, Expr());
                        break;
                }
            }

            return res;
        }

        private ExcelExpression Factor()
        {
            var token = Read();

            if (token == null)
            {
                throw new InvalidOperationException();
            }

            switch (token.Kind)
            {
                case TokenKind.Number:
                    return ExcelExpression.Number(double.Parse(token.Value, CultureInfo.CurrentCulture /* UI */));
                case TokenKind.Identifier:
                    var next = Peek();
                    if (next != null && next.Kind == TokenKind.OpenParen)
                    {
                        return ExcelExpression.Formula(token.Value, ArgList());
                    }
                    else if (next != null && next.Kind == TokenKind.Colon)
                    {
                        Read();
                        var endRange = Peek();
                        if (endRange == null || endRange.Kind != TokenKind.Identifier)
                        {
                            throw new InvalidOperationException("Incomplete range expression.");
                        }
                        endRange = Read();
                        return ExcelExpression.Range(token.Value, endRange.Value);
                    }
                    else
                    {
                        return ExcelExpression.Cell(token.Value);
                    }
                case TokenKind.OpenParen:
                    var res = Expr();
                    var closeParen = Read();
                    if (closeParen == null || closeParen.Kind != TokenKind.CloseParen)
                    {
                        throw new InvalidOperationException();
                    }
                    return res;
                default:
                    throw new InvalidOperationException("Unexpected token: " + token.Kind);
            }
        }

        private ExcelExpression[] ArgList()
        {
            var args = new List<ExcelExpression>();

            var openParen = Read();

            if (openParen == null || openParen.Kind != TokenKind.OpenParen)
            {
                throw new InvalidOperationException();
            }

            while (true)
            {
                args.Add(Expr());

                var next = Peek();

                if (next == null)
                {
                    throw new InvalidOperationException();
                }
                else if (next.Kind == TokenKind.CloseParen)
                {
                    Read();
                    break;
                }
                else if (next.Kind == TokenKind.Comma)
                {
                    Read();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return args.ToArray();
        }

        private static IEnumerable<Token> Lex(string s)
        {
            var n = s.Length;

            var i = 0;

            while (i < n)
            {
                var c = s[i];

                switch (c)
                {
                    case '(':
                        yield return new Token { Kind = TokenKind.OpenParen, Value = "(", Position = i };
                        break;
                    case ')':
                        yield return new Token { Kind = TokenKind.CloseParen, Value = ")", Position = i };
                        break;
                    case '+':
                        yield return new Token { Kind = TokenKind.Plus, Value = "+", Position = i };
                        break;
                    case '-':
                        {
                            if (i + 1 < n && char.IsDigit(s[i + 1]))
                            {
                                goto default;
                            }
                        }
                        yield return new Token { Kind = TokenKind.Dash, Value = "-", Position = i };
                        break;
                    case '*':
                        yield return new Token { Kind = TokenKind.Star, Value = "*", Position = i };
                        break;
                    case '/':
                        yield return new Token { Kind = TokenKind.Slash, Value = "/", Position = i };
                        break;
                    case '%':
                        yield return new Token { Kind = TokenKind.Percent, Value = "%", Position = i };
                        break;
                    case ',':
                        yield return new Token { Kind = TokenKind.Comma, Value = ",", Position = i };
                        break;
                    case ':':
                        yield return new Token { Kind = TokenKind.Colon, Value = ":", Position = i };
                        break;
                    default:
                        {
                            if (char.IsWhiteSpace(c))
                            {
                                i++;
                                continue;
                            }
                            else if (c == '-' || char.IsDigit(c))
                            {
                                var b = i;
                                var e = b;

                                var j = i + 1;
                                var p = false;

                                while (j < n)
                                {
                                    var d = s[j];

                                    if (d == '.')
                                    {
                                        if (p)
                                        {
                                            throw new InvalidOperationException(); // TODO
                                        }
                                        else
                                        {
                                            p = true;
                                        }
                                    }
                                    else if (!char.IsDigit(d))
                                    {
                                        e = j - 1;
                                        break;
                                    }

                                    j++;
                                }

                                if (j == n)
                                {
                                    e = j - 1;
                                }

                                var value = s.Substring(b, e - b + 1);
                                i = e;

                                yield return new Token { Kind = TokenKind.Number, Value = value, Position = b };
                            }
                            else if (char.IsLetter(c))
                            {
                                var b = i;
                                var e = b;

                                var j = i + 1;

                                while (j < n)
                                {
                                    var d = s[j];

                                    if (!char.IsLetterOrDigit(d))
                                    {
                                        e = j - 1;
                                        break;
                                    }

                                    j++;
                                }

                                if (j == n)
                                {
                                    e = j - 1;
                                }

                                var value = s.Substring(b, e - b + 1);
                                i = e;

                                yield return new Token { Kind = TokenKind.Identifier, Value = value, Position = b };
                            }
                            else
                            {
                                throw new InvalidOperationException("Unexpected token: " + c);
                            }
                        }
                        break;
                }

                i++;
            }
        }

        public static bool TryParseCell(string cell, out int row, out int col)
        {
            row = 0;
            col = 0;

            var regEx = new Regex("([A-Z]+)([0-9]+)");

            var m = regEx.Match(cell.ToUpper(CultureInfo.InvariantCulture));

            if (!m.Success)
            {
                return false;
            }
            else
            {
                var colS = m.Groups[1].Value;
                var rowS = m.Groups[2].Value;

                col = FromBase26(colS);
                row = int.Parse(rowS, CultureInfo.InvariantCulture);

                return true;
            }
        }

        private static int FromBase26(string s)
        {
            var res = 0;
            foreach (var c in s)
            {
                int rem = c - 'A' + 1;
                res = res * 26 + rem;
            }
            return res;
        }
    }
}
