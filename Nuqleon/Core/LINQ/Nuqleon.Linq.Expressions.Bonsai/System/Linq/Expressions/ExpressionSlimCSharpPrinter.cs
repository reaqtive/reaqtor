// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.CompilerServices.Bonsai;
using System.Reflection;
using System.Text;

namespace System.Linq.Expressions.Bonsai
{
    internal class ExpressionSlimCSharpPrinter : ExpressionSlimVisitor<string>
    {
        private readonly Stack<IEnumerable<ParameterExpressionSlim>> _environment;
        private readonly List<ParameterExpressionSlim> _globals;

        private readonly Stack<Dictionary<ParameterExpressionSlim, string>> _aliases;
        private readonly Stack<int> _aliasesMax;
        private readonly Dictionary<ParameterExpressionSlim, string> _globalAliases;
        private int _globalAliasesMax = 0;

        private readonly Dictionary<MemberInfoSlim, string> _memberAliases = new();

        private static IDictionary<Type, Func<object, string>> s_numericConstantToString;

        public ExpressionSlimCSharpPrinter()
        {
            _environment = new Stack<IEnumerable<ParameterExpressionSlim>>();
            _globals = new List<ParameterExpressionSlim>();

            _aliases = new Stack<Dictionary<ParameterExpressionSlim, string>>();
            _aliasesMax = new Stack<int>();

            _globalAliases = new Dictionary<ParameterExpressionSlim, string>();
            _aliases.Push(_globalAliases);
            _aliasesMax.Push(-1);
        }

        protected internal override string VisitBinary(BinaryExpressionSlim expression)
        {
            var format = default(string);

            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    format = "{0} + {1}";
                    break;
                case ExpressionType.AddChecked:
                    format = "checked({0} + {1})";
                    break;
                case ExpressionType.And:
                    format = "{0} & {1}";
                    break;
                case ExpressionType.AndAlso:
                    format = "{0} && {1}";
                    break;
                case ExpressionType.ArrayIndex:
                    format = "{0}[{1}]";
                    break;
                case ExpressionType.Coalesce:
                    format = "{0} ?? {1}";
                    break;
                case ExpressionType.Divide:
                    format = "{0} / {1}";
                    break;
                case ExpressionType.Equal:
                    format = "{0} == {1}";
                    break;
                case ExpressionType.ExclusiveOr:
                    format = "{0} ^ {1}";
                    break;
                case ExpressionType.GreaterThan:
                    format = "{0} > {1}";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    format = "{0} >= {1}";
                    break;
                case ExpressionType.LeftShift:
                    format = "{0} << {1}";
                    break;
                case ExpressionType.LessThan:
                    format = "{0} < {1}";
                    break;
                case ExpressionType.LessThanOrEqual:
                    format = "{0} <= {1}";
                    break;
                case ExpressionType.RightShift:
                    format = "{0} >> {1}";
                    break;
                case ExpressionType.Modulo:
                    format = "{0} % {1}";
                    break;
                case ExpressionType.Multiply:
                    format = "{0} * {1}";
                    break;
                case ExpressionType.MultiplyChecked:
                    format = "checked({0} * {1})";
                    break;
                case ExpressionType.NotEqual:
                    format = "{0} != {1}";
                    break;
                case ExpressionType.Or:
                    format = "{0} | {1}";
                    break;
                case ExpressionType.OrElse:
                    format = "{0} || {1}";
                    break;
                case ExpressionType.Power:
                    return PrintMethodCall(expression.Method, expression.Left, expression.Right);
                case ExpressionType.Subtract:
                    format = "{0} - {1}";
                    break;
                case ExpressionType.SubtractChecked:
                    format = "checked({0} - {1})";
                    break;
                case ExpressionType.AddAssign:
                    format = "{0} += {1}";
                    break;
                case ExpressionType.AddAssignChecked:
                    format = "checked({0} += {1})";
                    break;
                case ExpressionType.AndAssign:
                    format = "{0} &= {1}";
                    break;
                case ExpressionType.Assign:
                    format = "{0} = {1}";
                    break;
                case ExpressionType.DivideAssign:
                    format = "{0} /= {1}";
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    format = "{0} ^= {1}";
                    break;
                case ExpressionType.LeftShiftAssign:
                    format = "{0} <<= {1}";
                    break;
                case ExpressionType.ModuloAssign:
                    format = "{0} %= {1}";
                    break;
                case ExpressionType.MultiplyAssign:
                    format = "{0} *= {1}";
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    format = "checked({0} *= {1})";
                    break;
                case ExpressionType.OrAssign:
                    format = "{0} |= {1}";
                    break;
                case ExpressionType.PowerAssign:
                    format = "{0} = " + EscapeFormat(PrintMethodCall(expression.Method, expression.Left, expression.Right));
                    break;
                case ExpressionType.RightShiftAssign:
                    format = "{0} >>= {1}";
                    break;
                case ExpressionType.SubtractAssign:
                    format = "{0} -= {1}";
                    break;
                case ExpressionType.SubtractAssignChecked:
                    format = "checked({0} -= {1})";
                    break;
            }

            Invariant.Assert(format != null, "Unknown binary expression: " + expression.NodeType);

            var l = Visit(expression.Left);
            var r = Visit(expression.Right);

            var bp = GetPrecedence(expression);
            var lp = GetPrecedence(expression.Left);
            var rp = GetPrecedence(expression.Right);

            if (bp < lp)
            {
                l = Paren(l);
            }
            else if (bp == lp)
            {
                var ba = GetAssociativity(expression);
                var bl = GetAssociativity(expression.Left);

                if (!(ba == Associativity.Left && bl == Associativity.Left))
                {
                    l = Paren(l);
                }
            }

            if (bp < rp)
            {
                if (expression.NodeType != ExpressionType.ArrayIndex)
                {
                    r = Paren(r);
                }
            }
            else if (bp == rp)
            {
                var ba = GetAssociativity(expression);
                var br = GetAssociativity(expression.Right);

                if (!(ba == Associativity.Right && br == Associativity.Right))
                {
                    r = Paren(r);
                }
            }

            return string.Format(CultureInfo.InvariantCulture, format, l, r);
        }

        protected internal override string VisitConditional(ConditionalExpressionSlim expression)
        {
            var t = Visit(expression.Test);

            var cp = GetPrecedence(expression);
            var tp = GetPrecedence(expression.Test);

            if (cp <= tp)
            {
                t = Paren(t);
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} ? {1} : {2}",
                t,
                Visit(expression.IfTrue),
                Visit(expression.IfFalse)
            );
        }

        private static IDictionary<Type, Func<object, string>> NumericConstantToString
        {
            get
            {
                s_numericConstantToString ??= new Dictionary<Type, Func<object, string>>
                    {
                        {
                            typeof(char), value =>
                            {
                                var c = ConvertTo(value, char.Parse);
                                var d = Escape(c);
                                return "'" + d + "'";
                            }
                        },
                        {
                            typeof(byte), value =>
                            {
                                return "(byte)" + ConvertTo(value, byte.Parse).ToString(CultureInfo.InvariantCulture);
                            }
                        },
                        {
                            typeof(sbyte), value =>
                            {
                                return "(sbyte)" + ConvertTo(value, sbyte.Parse).ToString(CultureInfo.InvariantCulture);
                            }
                        },
                        {
                            typeof(short), value =>
                            {
                                return "(short)" + ConvertTo(value, short.Parse).ToString(CultureInfo.InvariantCulture);
                            }
                        },
                        {
                            typeof(ushort), value =>
                            {
                                return "(ushort)" + ConvertTo(value, ushort.Parse).ToString(CultureInfo.InvariantCulture);
                            }
                        },
                        {
                            typeof(int), value =>
                            {
                                return ConvertTo(value, int.Parse).ToString(CultureInfo.InvariantCulture);
                            }
                        },
                        {
                            typeof(uint), value =>
                            {
                                return ConvertTo(value, uint.Parse).ToString(CultureInfo.InvariantCulture) + "U";
                            }
                        },
                        {
                            typeof(long), value =>
                            {
                                return ConvertTo(value, long.Parse).ToString(CultureInfo.InvariantCulture) + "L";
                            }
                        },
                        {
                            typeof(ulong), value =>
                            {
                                return ConvertTo(value, ulong.Parse).ToString(CultureInfo.InvariantCulture) + "UL";
                            }
                        },
                        {
                            typeof(double), value =>
                            {
                                var val = ConvertTo(value, double.Parse);
                                if (double.IsNegativeInfinity(val))
                                {
                                    return "double.NegativeInfinity";
                                }
                                else if (double.IsPositiveInfinity(val))
                                {
                                    return "double.PositiveInfinity";
                                }
                                else if (double.IsNaN(val))
                                {
                                    return "double.NaN";
                                }
                                else
                                {
                                    return val.ToString(CultureInfo.InvariantCulture) + "D";
                                }
                            }
                        },
                        {
                            typeof(float), value =>
                            {
                                var val = ConvertTo(value, float.Parse);
                                if (float.IsNegativeInfinity(val))
                                {
                                    return "float.NegativeInfinity";
                                }
                                else if (float.IsPositiveInfinity(val))
                                {
                                    return "float.PositiveInfinity";
                                }
                                else if (float.IsNaN(val))
                                {
                                    return "float.NaN";
                                }
                                else
                                {
                                    return val.ToString(CultureInfo.InvariantCulture) + "F";
                                }
                            }
                        },
                        {
                            typeof(decimal), value =>
                            {
                                return ConvertTo(value, decimal.Parse).ToString(CultureInfo.InvariantCulture) + "M";
                            }
                        },
                    };

                return s_numericConstantToString;
            }
        }

        private static T ConvertTo<T>(object value, Func<string, T> parse)
        {
            if (value is string str)
            {
                return parse(str);
            }

            return (T)value;
        }

        protected internal override string VisitConstant(ConstantExpressionSlim expression)
        {
            var type = expression.Type;
            var value = default(object);

            if (expression.Value != null)
            {
                if (TryGetCSharpType(type, out Type fatType))
                {
                    var obj = expression.Value.Reduce(fatType);
                    return PrintValue(obj, type);
                }

                value = expression.Value.Value;
            }

            if (value == null)
            {
                return Paren(PrintType(type)) + "null";
            }

            if (type.IsRecordType() || type.IsAnonymousType())
            {
                return string.Format(CultureInfo.InvariantCulture, "/* {0} */", value.ToString());
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "default({0}) /* {1} */", PrintType(type), value.ToString());
            }
        }

        private static bool TryGetCSharpType(TypeSlim slimType, out Type fatType)
        {
            if (slimType.IsNullableType())
            {
                if (TryGetCSharpType(slimType.GetNonNullableType(), out Type nonNullFatType))
                {
                    fatType = typeof(Nullable<>).MakeGenericType(nonNullFatType);
                    return true;
                }
            }
            else if (slimType.IsBooleanType() || slimType.IsStringType() || slimType.IsNumeric(includeDecimal: true))
            {
                fatType = slimType.ToType();
                return true;
            }
            else if (slimType.Kind == TypeSlimKind.Simple)
            {
                if (slimType == typeof(Type).ToTypeSlim() ||
                    slimType == typeof(int).GetType().ToTypeSlim() /* NB: support RuntimeType */)
                {
                    fatType = typeof(Type);
                    return true;
                }
            }

            fatType = null;
            return false;
        }

        private static string PrintValue(object value, TypeSlim type)
        {
            if (value == null)
            {
                return Paren(PrintType(type)) + "null";
            }
            else
            {
                if (type.IsNullableType())
                {
                    return Paren(PrintType(type)) + PrintValue(value, type.GetNonNullableType());
                }
                else if (type.IsBooleanType())
                {
                    return ConvertTo(value, bool.Parse) ? "true" : "false";
                }
                else if (type.IsStringType())
                {
                    var sb = new StringBuilder();
                    sb.Append('\"');
                    foreach (var c in (string)value)
                    {
                        sb.Append(c == '\"' ? "\\\"" : Escape(c));
                    }
                    sb.Append('\"');
                    return sb.ToString();
                }
                else if (type.IsNumeric(includeDecimal: true))
                {
                    var typeFat = type.ToType();
                    return NumericConstantToString[typeFat](value);
                }
                else if (type.Kind == TypeSlimKind.Simple)
                {
                    var fatType = type.ToType();

                    if (typeof(Type).IsAssignableFrom(fatType))
                    {
                        return "typeof(" + ((Type)value).ToCSharpStringPretty() + ")";
                    }
                }

                return string.Format(CultureInfo.InvariantCulture, "default({0}) /* {1} */", PrintType(type), value.ToString());
            }
        }

        private static string Escape(char c)
        {
            return c switch
            {
                '\0' => "\\0",
                '\a' => "\\a",
                '\b' => "\\b",
                '\f' => "\\f",
                '\n' => "\\n",
                '\r' => "\\r",
                '\t' => "\\t",
                '\v' => "\\v",
                '\'' => "\\'",
                '\\' => "\\\\",
                _ => c.ToString(),
            };
        }

        protected internal override string VisitDefault(DefaultExpressionSlim expression)
        {
            return "default(" + PrintType(expression.Type) + ")";
        }

        protected internal override string VisitIndex(IndexExpressionSlim expression)
        {
            throw new NotImplementedException(); // TODO
        }

        protected internal override string VisitInvocation(InvocationExpressionSlim expression)
        {
            var l = expression.Expression;
            var e = Visit(l);

            var ip = GetPrecedence(expression);
            var ep = GetPrecedence(l);

            if (ip < ep)
            {
                e = Paren(e);
            }

            return e + JoinParen(VisitArguments(expression));
        }

        protected internal override string VisitLambda(LambdaExpressionSlim expression)
        {
            var aliases = new Dictionary<ParameterExpressionSlim, string>();

            var max = _aliasesMax.Peek();

            foreach (var p in expression.Parameters)
            {
                Register(p, "t", ref max, aliases);
            }

            _environment.Push(expression.Parameters);
            _aliases.Push(aliases);
            _aliasesMax.Push(max);

            var ps = default(string);

            if (expression.Parameters.Count != 1)
            {
                ps = JoinParen(expression.Parameters.Select(GetParameterName));
            }
            else
            {
                ps = GetParameterName(expression.Parameters[0]);
            }

            var body = Visit(expression.Body);

            _aliasesMax.Pop();
            _aliases.Pop();
            _environment.Pop();

            return ps + " => " + body;
        }

        protected internal override string VisitListInit(ListInitExpressionSlim expression)
        {
            var n = Visit(expression.NewExpression);

            if (expression.NewExpression.ArgumentCount == 0)
            {
                if (n.EndsWith("()", StringComparison.Ordinal))
                {
#if NET8_0 || NETSTANDARD2_1
                    n = n[0..^2];
#else
                    n = n.Substring(0, n.Length - 2);
#endif
                }
            }

            return n + " " + JoinCurly(Visit(expression.Initializers));
        }

        private IEnumerable<string> Visit(IEnumerable<ElementInitSlim> elements)
        {
            return elements.Select(Visit).ToArray();
        }

        private string Visit(ElementInitSlim element)
        {
            if (element.Arguments.Count == 1)
            {
                return Visit(element.Arguments[0]);
            }
            else
            {
                return JoinCurly(Visit(element.Arguments));
            }
        }

        private string GetMemberName(MemberInfoSlim member)
        {
            if (!_memberAliases.TryGetValue(member, out string res))
            {
                res = member.GetName();
            }

            return res;
        }

        protected internal override string VisitMember(MemberExpressionSlim expression)
        {
            if (expression.Expression == null)
            {
                return PrintType(expression.Member.DeclaringType) + "." + GetMemberName(expression.Member);
            }
            else
            {
                var e = Visit(expression.Expression);

                var mp = GetPrecedence(expression);
                var ep = GetPrecedence(expression.Expression);

                if (mp < ep)
                {
                    e = Paren(e);
                }

                return e + "." + GetMemberName(expression.Member);
            }
        }

        protected internal override string VisitMemberInit(MemberInitExpressionSlim expression)
        {
            string n;

            var newExpr = expression.NewExpression;

            if (newExpr.Type.IsRecordType() && expression.NewExpression.ArgumentCount == 0)
            {
                n = "new";
            }
            else
            {
                n = Visit(newExpr);

                if (newExpr.Arguments.Count == 0)
                {
                    if (n.EndsWith("()", StringComparison.Ordinal))
                    {
#if NET8_0 || NETSTANDARD2_1
                        n = n[0..^2];
#else
                        n = n.Substring(0, n.Length - 2);
#endif
                    }
                }
            }

            return n + " " + JoinCurly(Visit(expression.Bindings));
        }

        private IEnumerable<string> Visit(IEnumerable<MemberBindingSlim> bindings)
        {
            return bindings.Select(Visit).ToArray();
        }

        private string Visit(MemberBindingSlim binding)
        {
            var found = false;
            var res = default(string);

            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    found = true;
                    res = Visit((MemberAssignmentSlim)binding);
                    break;
                case MemberBindingType.ListBinding:
                    found = true;
                    res = Visit((MemberListBindingSlim)binding);
                    break;
                case MemberBindingType.MemberBinding:
                    found = true;
                    res = Visit((MemberMemberBindingSlim)binding);
                    break;
            }

            Invariant.Assert(found, "Unknown member binding.");

            return res;
        }

        private string Visit(MemberAssignmentSlim binding)
        {
            return GetMemberName(binding.Member) + " = " + Visit(binding.Expression);
        }

        private string Visit(MemberListBindingSlim binding)
        {
            return GetMemberName(binding.Member) + " = " + JoinCurly(Visit(binding.Initializers));
        }

        private string Visit(MemberMemberBindingSlim binding)
        {
            return GetMemberName(binding.Member) + " = " + JoinCurly(Visit(binding.Bindings));
        }

        protected internal override string VisitMethodCall(MethodCallExpressionSlim expression)
        {
            return PrintMethodCall(expression.Method, expression.Object, (IArgumentProviderSlim)expression);
        }

        protected internal override string VisitNew(NewExpressionSlim expression)
        {
            if (expression.Members != null)
            {
                return "new " + JoinCurly(expression.Members.Zip(expression.Arguments /* PERF: this may allocate */, (m, a) =>
                {
                    var shortForm = false;

                    if (a is ParameterExpressionSlim p)
                    {
                        if (p.Name == m.GetName())
                        {
                            shortForm = true;

                            var alias = GetParameterName(p);
                            if (p.Name != alias)
                            {
                                _memberAliases[m] = alias;
                            }
                        }
                    }
                    else
                    {
                        if (a is MemberExpressionSlim i)
                        {
                            if (i.Member.GetName() == m.GetName())
                            {
                                shortForm = true;
                            }
                        }
                    }

                    var b = Visit(a);
                    if (shortForm)
                    {
                        return b;
                    }
                    else
                    {
                        return GetMemberName(m) + " = " + b;
                    }
                }));
            }
            else if (expression.Type.IsAnonymousType())
            {
                //
                // NB: This case works around a regression in the Roslyn compiler where "new { }" is turned into a NewExpression
                //     which no longer has an empty Members collection (unlike the prior native compilers).
                //
                //     See https://github.com/dotnet/roslyn/issues/12297 for more information and a status on a fix for this regression.
                //
                return "new { }";
            }
            else
            {
                var args = VisitArguments(expression);
                return "new " + PrintType(expression.GetTypeSlim()) + JoinParen(args);
            }
        }

        protected internal override string VisitNewArray(NewArrayExpressionSlim expression)
        {
            var elemType = expression.GetTypeSlim().GetElementType();
            var expressions = Visit(expression.Expressions);

            if (expression.NodeType == ExpressionType.NewArrayBounds)
            {
                return "new " + PrintType(elemType) + JoinBracket(expressions);
            }
            else
            {
                //
                // TODO: implicitly typed arrays
                //
                if (elemType.IsCompilerGenerated())
                {
                    return "new[] " + JoinCurly(expressions);
                }
                else
                {
                    return "new " + PrintType(elemType) + "[] " + JoinCurly(expressions);
                }
            }
        }

        protected internal override string VisitParameter(ParameterExpressionSlim expression)
        {
            if (!_environment.SelectMany(env => env).Concat(_globals).Contains(expression))
            {
                _globals.Add(expression);
                Register(expression, "g", ref _globalAliasesMax, _globalAliases);
            }

            return GetParameterName(expression);
        }

        protected internal override string VisitTypeBinary(TypeBinaryExpressionSlim expression)
        {
            var format = default(string);

            switch (expression.NodeType)
            {
                case ExpressionType.TypeIs:
                    format = "{0} is " + EscapeFormat(PrintType(expression.TypeOperand));
                    break;
                case ExpressionType.TypeEqual:
                    format = "{0} typeEqual " + EscapeFormat(PrintType(expression.TypeOperand));
                    break;
            }

            Invariant.Assert(format != null, "Unknown type binary expression: " + expression.NodeType);

            var e = Visit(expression.Expression);

            var tp = GetPrecedence(expression);
            var ep = GetPrecedence(expression.Expression);

            if (tp < ep)
            {
                e = Paren(e);
            }

            return string.Format(CultureInfo.InvariantCulture, format, e);
        }

        protected internal override string VisitUnary(UnaryExpressionSlim expression)
        {
            var format = default(string);

            switch (expression.NodeType)
            {
                case ExpressionType.ArrayLength:
                    format = "{0}.Length";
                    break;
                case ExpressionType.Convert:
                    format = "(" + EscapeFormat(PrintType(expression.Type)) + "){0}";
                    break;
                case ExpressionType.ConvertChecked:
                    format = "checked((" + EscapeFormat(PrintType(expression.Type)) + "){0})";
                    break;
                case ExpressionType.Negate:
                    format = "-{0}";
                    break;
                case ExpressionType.NegateChecked:
                    format = "checked(-{0})";
                    break;
                case ExpressionType.Not:
                    if (expression.Operand.GetTypeSlim().IsInteger()
                        || (expression.Method != null && expression.Method.GetName() == "op_OnesComplement")) // TODO
                    {
                        format = "~{0}";
                    }
                    else
                    {
                        format = "!{0}";
                    }
                    break;
                case ExpressionType.Quote:
                    format = "{0}";
                    break;
                case ExpressionType.TypeAs:
                    format = "{0} as " + EscapeFormat(PrintType(expression.Type));
                    break;
                case ExpressionType.UnaryPlus:
                    format = "+{0}";
                    break;
                case ExpressionType.OnesComplement:
                    format = "~{0}";
                    break;
                case ExpressionType.PostDecrementAssign:
                    format = "{0}--";
                    break;
                case ExpressionType.PostIncrementAssign:
                    format = "{0}++";
                    break;
                case ExpressionType.PreDecrementAssign:
                    format = "--{0}";
                    break;
                case ExpressionType.PreIncrementAssign:
                    format = "++{0}";
                    break;
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.Throw:
                case ExpressionType.Unbox:
                    throw new NotSupportedException("Node type not supported: " + expression.NodeType);
            }

            Invariant.Assert(format != null, "Unknown unary expression: " + expression.NodeType);

            var o = Visit(expression.Operand);

            var up = GetPrecedence(expression);
            var op = GetPrecedence(expression.Operand);

            if (up < op)
            {
                o = Paren(o);
            }
            else if (up == op)
            {
                if ((expression.NodeType == ExpressionType.UnaryPlus && expression.Operand.NodeType == ExpressionType.UnaryPlus)
                    || (expression.NodeType == ExpressionType.Negate && expression.Operand.NodeType == ExpressionType.Negate))
                {
                    o = Paren(o);
                }
            }

            return string.Format(CultureInfo.InvariantCulture, format, o);
        }

        private static string EscapeFormat(string s)
        {
#if NET8_0 || NETSTANDARD2_1
            return s.Replace("{", "{{", StringComparison.Ordinal).Replace("}", "}}", StringComparison.Ordinal);
#else
            return s.Replace("{", "{{").Replace("}", "}}");
#endif
        }

        private static int GetPrecedence(ExpressionSlim expression)
        {
            var res = default(int?);

            //
            // Table from http://msdn.microsoft.com/en-us/library/6a71f45d(v=vs.110).aspx
            //
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                case ExpressionType.Parameter:
                    res = 0;
                    break;

                //
                // Primary
                //
                case ExpressionType.MemberAccess:
                case ExpressionType.Invoke:
                case ExpressionType.Call:
                case ExpressionType.ArrayIndex:
                case ExpressionType.ArrayLength:
                case ExpressionType.New:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                case ExpressionType.MemberInit:
                case ExpressionType.ListInit:
                case ExpressionType.AddChecked:
                case ExpressionType.ConvertChecked:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NegateChecked:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Default:
                case ExpressionType.Index:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.SubtractAssignChecked:
                    res = 1;
                    break;

                //
                // Unary
                //
                case ExpressionType.UnaryPlus:
                case ExpressionType.Negate:
                case ExpressionType.Not:
                case ExpressionType.OnesComplement:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.Convert:
                    res = 2;
                    break;

                //
                // Multiplicative
                //
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                    res = 3;
                    break;

                //
                // Additive
                //
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                    res = 4;
                    break;

                //
                // Shift
                //
                case ExpressionType.LeftShift:
                case ExpressionType.RightShift:
                    res = 5;
                    break;

                //
                // Relational and type testing
                //
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.TypeIs:
                case ExpressionType.TypeAs:
                    res = 6;
                    break;

                //
                // Equality
                //
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    res = 7;
                    break;

                //
                // Logical AND
                //
                case ExpressionType.And:
                    res = 8;
                    break;

                //
                // Logical XOR
                //
                case ExpressionType.ExclusiveOr:
                    res = 9;
                    break;

                //
                // Logical OR
                //
                case ExpressionType.Or:
                    res = 10;
                    break;

                //
                // Conditional AND
                //
                case ExpressionType.AndAlso:
                    res = 11;
                    break;

                //
                // Conditional OR
                //
                case ExpressionType.OrElse:
                    res = 12;
                    break;

                //
                // Null-coalescing
                //
                case ExpressionType.Coalesce:
                    res = 13;
                    break;

                //
                // Conditional
                //
                case ExpressionType.Conditional:
                    res = 14;
                    break;

                //
                // Assignment and lambda expression
                //
                case ExpressionType.Lambda:
                case ExpressionType.Quote:
                case ExpressionType.Assign:
                case ExpressionType.AddAssign:
                case ExpressionType.SubtractAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.DivideAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.AndAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.PowerAssign:
                    res = 15;
                    break;
            }

            Invariant.Assert(res != null, "Unknown node type: " + expression.NodeType);

            return res.Value;
        }

        private static Associativity GetAssociativity(ExpressionSlim expression)
        {
            var res = default(Associativity?);

            switch (expression.NodeType)
            {
                case ExpressionType.Coalesce:
                case ExpressionType.Assign:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.DivideAssign:
                case ExpressionType.ModuloAssign:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.AndAssign:
                case ExpressionType.OrAssign:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.PowerAssign:
                    res = Associativity.Right;
                    break;
            }

            return res ?? Associativity.Left;
        }

        private static string JoinParen<T>(IEnumerable<T> elements)
        {
            return Join(elements, '(', ')', enablePadding: false);
        }

        private static string JoinAngle<T>(IEnumerable<T> elements)
        {
            return Join(elements, '<', '>', enablePadding: false);
        }

        private static string JoinBracket<T>(IEnumerable<T> elements)
        {
            return Join(elements, '[', ']', enablePadding: false);
        }

        private static string JoinCurly<T>(IEnumerable<T> elements)
        {
            return Join(elements, '{', '}', enablePadding: true);
        }

        private static string Join<T>(IEnumerable<T> elements, char start, char end, bool enablePadding)
        {
            var sb = new StringBuilder();

            sb.Append(start);

            if (enablePadding)
            {
                sb.Append(' ');
            }

            var n = 0;

            foreach (var element in elements)
            {
                if (n != 0)
                {
                    sb.Append(", ");
                }

                sb.Append(element);
                n++;
            }

            if (enablePadding && n > 0)
            {
                sb.Append(' ');
            }

            sb.Append(end);

            return sb.ToString();
        }

        private static string Paren(string s)
        {
            return "(" + s + ")";
        }

        private static string PrintType(TypeSlim type)
        {
            return type.ToCSharpString();
        }

        private enum Associativity
        {
            Left,
            Right
        }

        private static void Register(ParameterExpressionSlim p, string prefix, ref int max, Dictionary<ParameterExpressionSlim, string> aliases)
        {
            if (string.IsNullOrWhiteSpace(p.Name) || p.Name.StartsWith("<>h__", StringComparison.Ordinal))
            {
                ++max;
                aliases[p] = prefix + (max > 0 ? max.ToString(CultureInfo.InvariantCulture) : "");
            }
        }

        private string GetParameterName(ParameterExpressionSlim expression)
        {
            foreach (var aliases in _aliases)
            {
                if (aliases.TryGetValue(expression, out string alias))
                {
                    return alias;
                }
            }

            Debug.Assert(!string.IsNullOrWhiteSpace(expression.Name));

            return expression.Name; // TODO
        }

        private string PrintMethodCall(MethodInfoSlim method, ExpressionSlim left, ExpressionSlim right)
        {
            var lhs = PrintType(method.DeclaringType);

            var methodName = GetMethodName(method);

            return lhs + "." + methodName + JoinParen(new[] { Visit(left), Visit(right) });
        }

        private string PrintMethodCall(MethodInfoSlim method, ExpressionSlim @object, IArgumentProviderSlim arguments)
        {
            var args = VisitArguments(arguments);

            string lhs;

            if (@object == null)
            {
                lhs = PrintType(method.DeclaringType);
            }
            else
            {
                lhs = Visit(@object);

                if (method.DeclaringType.IsArray() && method.DeclaringType.GetArrayRank() > 1 && method.GetName() == "Get")
                {
                    return lhs + JoinBracket(args);
                }
            }

            var methodName = GetMethodName(method);

            return lhs + "." + methodName + JoinParen(args);
        }

        private static string GetMethodName(MethodInfoSlim method)
        {
            var methodName = method.GetName();

            if (method.IsGenericMethod)
            {
                if (!method.GetGenericArguments().Any(t => t.IsAnonymousType()))
                {
                    methodName += JoinAngle(method.GetGenericArguments().Select(PrintType));
                }
            }

            return methodName;
        }

        #region Not Implemented

        protected internal override string VisitBlock(BlockExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        protected internal override string VisitExtension(ExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        protected internal override string VisitGoto(GotoExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        protected internal override string VisitLabel(LabelExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        protected internal override string VisitLoop(LoopExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        protected internal override string VisitSwitch(SwitchExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        protected internal override string VisitTry(TryExpressionSlim node)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal static class PrintingExtensions
    {
        public static TypeSlim GetTypeSlim(this ExpressionSlim e)
        {
            var t = new TypeSlimDerivationVisitor().Visit(e);
            return t;
        }

        public static TypeSlim GetElementType(this TypeSlim t)
        {
            var a = (ArrayTypeSlim)t;
            return a.ElementType;
        }

        public static int? GetArrayRank(this TypeSlim t)
        {
            var a = (ArrayTypeSlim)t;
            return a.Rank;
        }

        public static bool IsArray(this TypeSlim t)
        {
            return t.Kind == TypeSlimKind.Array;
        }

        public static ReadOnlyCollection<TypeSlim> GetGenericArguments(this MethodInfoSlim m)
        {
            var g = (GenericMethodInfoSlim)m;
            return g.GenericArguments;
        }

        public static bool IsRecordType(this TypeSlim t)
        {
            return t.Kind == TypeSlimKind.Structural;
        }

        public static bool IsAnonymousType(this TypeSlim t)
        {
            return t.Kind == TypeSlimKind.Structural;
        }

        public static bool IsCompilerGenerated(this TypeSlim t)
        {
            return t.Kind == TypeSlimKind.Structural;
        }

        public static string GetName(this MemberInfoSlim m)
        {
            return m switch
            {
                SimpleMethodInfoSlimBase smtd => smtd.Name,
                GenericMethodInfoSlim gmtd => gmtd.GenericMethodDefinition.GetName(),
                PropertyInfoSlim prp => prp.Name,
                FieldInfoSlim fld => fld.Name,
                _ => "<unknown>",
            };
        }
    }
}
