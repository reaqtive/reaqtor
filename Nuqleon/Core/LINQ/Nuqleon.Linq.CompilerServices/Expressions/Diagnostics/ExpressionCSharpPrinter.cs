// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Linq.Expressions
{
    internal sealed class ExpressionCSharpPrinter : ExpressionVisitorNarrow<string>
    {
        private readonly bool _allowCompilerGeneratedNames;
        private readonly Stack<IEnumerable<ParameterExpression>> _environment;

        private readonly Stack<Dictionary<ParameterExpression, string>> _aliases;
        private readonly Stack<int> _aliasesMax;
        private readonly Dictionary<ParameterExpression, string> _globalAliases;
        private int _globalAliasesMax = 0;

        private readonly Dictionary<object, string> _constantNames = new();

        public IDictionary<string, ConstantExpression> Constants { get; } = new Dictionary<string, ConstantExpression>();

        public IList<ParameterExpression> Globals { get; }

        public ExpressionCSharpPrinter(bool allowCompilerGeneratedNames)
        {
            _allowCompilerGeneratedNames = allowCompilerGeneratedNames;

            _environment = new Stack<IEnumerable<ParameterExpression>>();
            Globals = new List<ParameterExpression>();

            _aliases = new Stack<Dictionary<ParameterExpression, string>>();
            _aliasesMax = new Stack<int>();

            _globalAliases = new Dictionary<ParameterExpression, string>();
            _aliases.Push(_globalAliases);
            _aliasesMax.Push(-1);
        }

        protected override string VisitBinary(BinaryExpression expression)
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
                    return PrintMethodCall(expression.Method, @object: null, new List<Expression> { expression.Left, expression.Right }.AsReadOnly());
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
                    format = "{0} = " + PrintMethodCall(expression.Method, @object: null, new List<Expression> { expression.Left, expression.Right }.AsReadOnly());
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

        protected override string VisitConditional(ConditionalExpression expression)
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

        protected override string VisitConstant(ConstantExpression expression)
        {
            var type = expression.Type;
            var value = expression.Value;

            if (value == null)
            {
                return Paren(PrintType(type)) + "null";
            }

            if (type == typeof(int))
            {
                return ((int)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(double))
            {
                var val = (double)value;
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
            else if (type == typeof(bool))
            {
                return ((bool)value) ? "true" : "false";
            }
            else if (type == typeof(string))
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
            else if (type == typeof(char))
            {
                var c = (char)value;
                var d = Escape(c);
                return "'" + d + "'";
            }
            else if (type == typeof(long))
            {
                return ((long)value).ToString(CultureInfo.InvariantCulture) + "L";
            }
            else if (type == typeof(float))
            {
                var val = (float)value;
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
                    return val.ToString("R", CultureInfo.InvariantCulture) + "F";
                }
            }
            else if (type == typeof(decimal))
            {
                return ((decimal)value).ToString(CultureInfo.InvariantCulture) + "M";
            }
            else if (type == typeof(byte))
            {
                return "(byte)" + ((byte)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(sbyte))
            {
                return "(sbyte)" + ((sbyte)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(short))
            {
                return "(short)" + ((short)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(ushort))
            {
                return "(ushort)" + ((ushort)value).ToString(CultureInfo.InvariantCulture);
            }
            else if (type == typeof(uint))
            {
                return ((uint)value).ToString(CultureInfo.InvariantCulture) + "U";
            }
            else if (type == typeof(ulong))
            {
                return ((ulong)value).ToString(CultureInfo.InvariantCulture) + "UL";
            }
            else if (type == typeof(Type).GetType(/* RuntimeType */))
            {
                return "typeof(" + PrintType((Type)value) + ")";
            }
            else if (type.IsEnum)
            {
                var et = PrintType(type);

                if (!type.IsDefined(typeof(FlagsAttribute), inherit: false))
                {
                    var n = Enum.GetName(type, value);
                    if (string.IsNullOrEmpty(n))
                    {
                        return Paren(et) + int.Parse(value.ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return et + "." + n;
                    }
                }
                else
                {
                    var val = value.ToString();
                    if (int.TryParse(val, out int num))
                    {
                        return Paren(et) + val;
                    }
                    else
                    {
                        return string.Join(" | ", val.Split('|', ',').Select(p => et + "." + p.Trim()));
                    }
                }
            }

            var name = default(string);

            if (!expression.Type.IsValueType)
            {
                if (_constantNames.TryGetValue(value, out name))
                {
                    return name;
                }
            }

            name = "__c" + Constants.Count;
            _constantNames[value] = name;
            Constants[name] = expression;

            return name;
        }

        protected override string VisitInvocation(InvocationExpression expression)
        {
            var l = expression.Expression;
            if (l.NodeType == ExpressionType.Lambda)
            {
                l = Expression.Convert(l, l.Type);
            }

            var e = Visit(l);

            var ip = GetPrecedence(expression);
            var ep = GetPrecedence(l);

            if (ip < ep)
            {
                e = Paren(e);
            }

            return e + JoinParen(Visit(expression.Arguments));
        }

        protected override string VisitMethodCall(MethodCallExpression expression) => PrintMethodCall(expression.Method, expression.Object, expression.Arguments);

        private static PropertyInfo GetPropertyByGetter(MethodInfo method)
        {
            var candidates = method.DeclaringType.GetProperties().Where(p => p.GetGetMethod() == method).ToList();
            if (candidates.Count == 1)
            {
                var property = candidates[0];
                return property;
            }

            return null;
        }

        private string PrintMethodCall(MethodInfo method, Expression @object, ReadOnlyCollection<Expression> arguments)
        {
            var args = Visit(arguments).AsEnumerable();

            var lhs = default(string);
            if (method.IsStatic)
            {
                lhs = PrintType(method.DeclaringType);

                if (method.IsDefined(typeof(ExtensionAttribute), inherit: false))
                {
                    lhs = args.First();
                    args = args.Skip(1);
                }
            }
            else
            {
                lhs = Visit(@object);

                if (method.DeclaringType.IsArray && method.DeclaringType.GetArrayRank() > 1 && method.Name == "Get")
                {
                    return lhs + JoinBracket(args);
                }
                else
                {
                    var property = GetPropertyByGetter(method);
                    if (property != null)
                    {
                        if (property.GetIndexParameters().Length != 0)
                        {
                            return lhs + JoinBracket(args);
                        }
                    }
                }
            }

            var methodName = method.Name;

            if (method.IsGenericMethod)
            {
                if (!method.GetGenericArguments().Any(t => t.IsAnonymousType()))
                {
                    methodName += JoinAngle(method.GetGenericArguments().Select(PrintType));
                }
            }

            return lhs + "." + methodName + JoinParen(args);
        }

        private static void Register(ParameterExpression p, string prefix, ref int max, Dictionary<ParameterExpression, string> aliases)
        {
            if (string.IsNullOrWhiteSpace(p.Name) || p.Name.StartsWith("<>h__", StringComparison.Ordinal))
            {
                ++max;
                aliases[p] = prefix + (max > 0 ? max.ToString(CultureInfo.InvariantCulture) : "");
            }
        }

        protected override string VisitLambda<T>(Expression<T> expression)
        {
            var aliases = new Dictionary<ParameterExpression, string>();

            var max = _aliasesMax.Peek();

            foreach (var p in expression.Parameters)
            {
                Register(p, "t", ref max, aliases);
            }

            _environment.Push(expression.Parameters);
            _aliases.Push(aliases);
            _aliasesMax.Push(max);

            var requiresImplicit = expression.Parameters.Any(p => p.Type.IsAnonymousType());

            var ps = default(string);

            if (requiresImplicit)
            {
                if (expression.Parameters.Count != 1)
                {
                    ps = JoinParen(expression.Parameters.Select(GetParameterName));
                }
                else
                {
                    ps = GetParameterName(expression.Parameters[0]);
                }
            }
            else
            {
                ps = JoinParen(expression.Parameters.Select(p => PrintType(p.Type) + " " + GetParameterName(p)));
            }

            var body = Visit(expression.Body);

            _ = _aliasesMax.Pop();
            _ = _aliases.Pop();
            _ = _environment.Pop();

            return ps + " => " + body;
        }

        protected override string VisitListInit(ListInitExpression expression)
        {
            var n = Visit(expression.NewExpression);

            if (expression.NewExpression.Arguments.Count == 0)
            {
                if (n.EndsWith("()", StringComparison.Ordinal))
                {
#if NET6_0 || NETSTANDARD2_1
                    n = n[0..^2];
#else
                    n = n.Substring(0, n.Length - 2);
#endif
                }
            }

            return n + " " + JoinCurly(Visit(expression.Initializers));
        }

        private IEnumerable<string> Visit(IEnumerable<ElementInit> elements) => elements.Select(Visit).ToArray();

        private string Visit(ElementInit element)
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

        private string GetMemberName(MemberInfo member)
        {
            if (!_memberAliases.TryGetValue(member, out string res))
            {
                res = member.Name;
            }

            return res;
        }

        protected override string VisitMember(MemberExpression expression)
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

        protected override string VisitMemberInit(MemberInitExpression expression)
        {
            var n = Visit(expression.NewExpression);

            if (expression.NewExpression.Arguments.Count == 0)
            {
                if (n.EndsWith("()", StringComparison.Ordinal))
                {
#if NET6_0 || NETSTANDARD2_1
                    n = n[0..^2];
#else
                    n = n.Substring(0, n.Length - 2);
#endif
                }
            }

            return n + " " + JoinCurly(Visit(expression.Bindings));
        }

        private IEnumerable<string> Visit(IEnumerable<MemberBinding> bindings) => bindings.Select(Visit).ToArray();

        private string Visit(MemberBinding binding)
        {
            var found = false;
            var res = default(string);

            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    found = true;
                    res = Visit((MemberAssignment)binding);
                    break;
                case MemberBindingType.ListBinding:
                    found = true;
                    res = Visit((MemberListBinding)binding);
                    break;
                case MemberBindingType.MemberBinding:
                    found = true;
                    res = Visit((MemberMemberBinding)binding);
                    break;
            }

            Invariant.Assert(found, "Unknown member binding.");

            return res;
        }

        private string Visit(MemberAssignment binding) => GetMemberName(binding.Member) + " = " + Visit(binding.Expression);

        private string Visit(MemberListBinding binding) => GetMemberName(binding.Member) + " = " + JoinCurly(Visit(binding.Initializers));

        private string Visit(MemberMemberBinding binding) => GetMemberName(binding.Member) + " = " + JoinCurly(Visit(binding.Bindings));

        private readonly Dictionary<MemberInfo, string> _memberAliases = new();

        protected override string VisitNew(NewExpression expression)
        {
            if (!expression.Type.IsRecordType() && expression.Members != null)
            {
                return "new " + JoinCurly(expression.Members.Zip(expression.Arguments, (m, a) =>
                {
                    var shortForm = false;

                    if (a is ParameterExpression p)
                    {
                        if (p.Name == m.Name)
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
                        if (a is MemberExpression i)
                        {
                            if (i.Member.Name == m.Name)
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
            else if (expression.Type.IsAnonymousType() && expression.Members == null)
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
                var args = Visit(expression.Arguments);
                return "new " + PrintType(expression.Type) + JoinParen(args);
            }
        }

        protected override string VisitNewArray(NewArrayExpression expression)
        {
            var elemType = expression.Type.GetElementType();
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

        protected override string VisitParameter(ParameterExpression expression)
        {
            if (!_environment.SelectMany(env => env).Concat(Globals).Contains(expression))
            {
                Globals.Add(expression);
                Register(expression, "g", ref _globalAliasesMax, _globalAliases);
            }

            return GetParameterName(expression);
        }

        private string GetParameterName(ParameterExpression expression)
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

        protected override string VisitTypeBinary(TypeBinaryExpression expression)
        {
            var format = default(string);

            switch (expression.NodeType)
            {
                case ExpressionType.TypeIs:
                    format = "{0} is " + PrintType(expression.TypeOperand);
                    break;
                case ExpressionType.TypeEqual:
                    format = "{0} typeEqual " + PrintType(expression.TypeOperand);
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

        protected override string VisitUnary(UnaryExpression expression)
        {
            var format = default(string);

            switch (expression.NodeType)
            {
                case ExpressionType.ArrayLength:
                    format = "{0}.Length";
                    break;
                case ExpressionType.Convert:
                    format = "(" + PrintType(expression.Type) + "){0}";
                    break;
                case ExpressionType.ConvertChecked:
                    format = "checked((" + PrintType(expression.Type) + "){0})";
                    break;
                case ExpressionType.Negate:
                    format = "-{0}";
                    break;
                case ExpressionType.NegateChecked:
                    format = "checked(-{0})";
                    break;
                case ExpressionType.Not:
                    if (expression.Operand.Type.IsInteger()
                        || (expression.Method != null && expression.Method.Name == "op_OnesComplement")) // TODO
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
                    format = "{0} as " + PrintType(expression.Type);
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
                    throw Errors.NotSupportedNode(expression.NodeType);
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

        protected override string VisitDefault(DefaultExpression expression) => "default(" + PrintType(expression.Type) + ")";

        private static int GetPrecedence(Expression expression)
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

        private static Associativity GetAssociativity(Expression expression)
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

        private static string JoinParen<T>(IEnumerable<T> elements) => Join(elements, '(', ')', enablePadding: false);

        private static string JoinAngle<T>(IEnumerable<T> elements) => Join(elements, '<', '>', enablePadding: false);

        private static string JoinBracket<T>(IEnumerable<T> elements) => Join(elements, '[', ']', enablePadding: false);

        private static string JoinCurly<T>(IEnumerable<T> elements) => Join(elements, '{', '}', enablePadding: true);

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

        private static string Paren(string s) => "(" + s + ")";

        private string PrintType(Type type) => type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, !_allowCompilerGeneratedNames);

        private enum Associativity
        {
            Left,
            Right,
        }
    }
}
