// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Hoists constants out of an expression tree into an environment.
    /// </summary>
    public class ConstantHoister : IConstantHoister
    {
        /// <summary>
        /// Default constant hoister without exclusions.
        /// </summary>
        private static readonly ConstantHoister s_default = new(useDefaultForNull: false);

        /// <summary>
        /// Default constant hoister without exclusions, using default expressions for null references.
        /// </summary>
        private static readonly ConstantHoister s_defaultWithDefaultNull = new(useDefaultForNull: true);

        /// <summary>
        /// Indicates whether to substitute null references for default expressions rather than hoisting them.
        /// </summary>
        private readonly bool _useDefaultForNull;

        /// <summary>
        /// List of holes for exclusion of constant hoisting in expression arguments.
        /// The use of hole index 0 indicates the target instance.
        /// </summary>
        /// <remarks>
        /// Most call sites have a limited number of arguments (99th percentile to exceed 5 arguments),
        /// so a list is fine to keep hole indexes.
        /// </remarks>
        private readonly Dictionary<MemberInfo, List<int>> _holes;

        /// <summary>
        /// Creates a new constant hoister without an exclusion list.
        /// </summary>
        public ConstantHoister()
            : this(useDefaultForNull: false, Array.Empty<LambdaExpression>())
        {
        }

        /// <summary>
        /// Creates a new constant hoister with the specified exclusion list.
        /// </summary>
        /// <param name="useDefaultForNull">Indicates whether to substitute null references for default expressions rather than hoisting them.</param>
        /// <param name="exclusions">Exclusion list for constant sites that should be excluded from hoisting.</param>
        public ConstantHoister(bool useDefaultForNull, params LambdaExpression[] exclusions)
        {
            if (exclusions == null)
            {
                exclusions = Array.Empty<LambdaExpression>();
            }

            _useDefaultForNull = useDefaultForNull;
            _holes = new Dictionary<MemberInfo, List<int>>();

            foreach (var exclusion in exclusions)
            {
                var p = exclusion.Parameters;

                if (p.Count == 0)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Exclusion '{0}' has no holes for constants that should be excluded from hoisting.", exclusion));
                }

                var member = default(MemberInfo);
                var holes = default(List<int>);

                switch (exclusion.Body.NodeType)
                {
                    case ExpressionType.Call:
                        {
                            var c = (MethodCallExpression)exclusion.Body;

                            var arguments = new[] { c.Object }.Concat(c.Arguments);

                            member = c.Method;
                            holes = GetHolePositions(exclusion, arguments);
                        }
                        break;
                    case ExpressionType.New:
                        {
                            var c = (NewExpression)exclusion.Body;

                            if (c.Constructor != null)
                            {
                                member = c.Constructor;
                                holes = GetHolePositions(exclusion, c.Arguments);
                            }
                        }
                        break;
                    case ExpressionType.MemberAccess:
                        {
                            var c = (MemberExpression)exclusion.Body;

                            member = c.Member;
                            holes = GetHolePositions(exclusion, new[] { c.Expression });
                        }
                        break;
                    default:
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Exclusion '{0}' is not a method call, new, or member access expression.", exclusion));
                }

                if (member != null)
                {
                    if (_holes.TryGetValue(member, out List<int> existing))
                    {
                        holes = existing.Union(holes).ToList();
                    }

                    _holes[member] = holes;
                }
            }
        }

        /// <summary>
        /// Creates a new constant hoister using the specified exclusion list for constant sites to exclude from hoisting.
        /// </summary>
        /// <param name="useDefaultForNull">Indicates whether to substitute null references for default expressions rather than hoisting them.</param>
        /// <param name="exclusions">Exclusion list for constant sites that should be excluded from hoisting. These sites are specified as lambda parameters.</param>
        /// <returns>Constant hoister instance using the specified exclusion list.</returns>
        /// <example>
        /// Consider the following usage of a constant hoister:
        /// <code>
        ///   var h = ConstantHoister.Create(
        ///               (Expression&lt;Func&lt;string, string&gt;&gt;)(s => string.Format(s, default(object[])))
        ///           );
        ///   
        ///   var e = (Expression&lt;Func&lt;string&gt;&gt;)(() => string.Format("{0} {1}", "bar", 123));
        ///   var r = h.Hoist(e.Body);
        /// </code>
        /// results in the following environment and tree:
        /// <code>
        ///   r.Expression  = string.Format("{0} {1}", p0, p1)
        ///   r.Environment = { p0 -> "bar", p1 -> 123 }
        /// </code>
        /// where the first argument to the string.Format call did not get hoisted.
        /// </example>
        public static ConstantHoister Create(bool useDefaultForNull, params LambdaExpression[] exclusions)
        {
            if (exclusions == null)
                throw new ArgumentNullException(nameof(exclusions));

            return new ConstantHoister(useDefaultForNull, exclusions);
        }

        /// <summary>
        /// Hoists constants in the specified expression and returns an environment.
        /// </summary>
        /// <param name="expression">Expression to hoist constants in.</param>
        /// <param name="useDefaultForNull">Indicates whether to substitute null references for default expressions rather than hoisting them.</param>
        /// <returns>Expression bound by an environment consisting of the hoisted constants.</returns>
        public static ExpressionWithEnvironment Hoist(Expression expression, bool useDefaultForNull)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return (useDefaultForNull ? s_defaultWithDefaultNull : s_default).Hoist(expression);
        }

        /// <summary>
        /// Hoists constants in the specified expression and returns an environment.
        /// </summary>
        /// <param name="expression">Expression to hoist constants in.</param>
        /// <returns>Expression bound by an environment consisting of the hoisted constants.</returns>
        public ExpressionWithEnvironment Hoist(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var impl = new Impl(this);
            var res = impl.Visit(expression);
            return new ExpressionWithEnvironment(res, impl.Environment);
        }

        /// <summary>
        /// Provides a hook for derived classes to rewrite the hoisted constants, e.g. to intern the constant's value.
        /// </summary>
        /// <param name="expression">The constant expression that has been hoisted.</param>
        /// <returns>An expression equivalent to the original hoisted expression.</returns>
        protected virtual Expression Hoist(ConstantExpression expression) => expression;

        /// <summary>
        /// Calls the virtual Hoist method but ensures the returned expression is compatible with the original hoisted expression.
        /// </summary>
        /// <param name="expression">The constant expression that has been hoisted.</param>
        /// <returns>An expression equivalent to the original hoisted expression.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the expression returned is null or has a type incompatible with the original hoisted expression.</exception>
        private Expression HoistSafe(ConstantExpression expression)
        {
            var hoisted = Hoist(expression);

            if (hoisted == null)
            {
                throw new InvalidOperationException("The result of hoisting a constant expression should not be null.");
            }

            //
            // NB: We don't allow covariance here; expressions have the funny property that even expressions can occur in a "ref" or "out" position (having "discard writeback" semantics).
            //
            if (expression.Type != hoisted.Type)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The result of hoisting a constant expression should have the same type as the original constant expression. {0} cannot be assigned to the original type {1}.", hoisted.Type, expression.Type));
            }

            return hoisted;
        }

        /// <summary>
        /// Hoists constants in the specified expression and returns an environment.
        /// </summary>
        /// <param name="expression">Expression to hoist constants in.</param>
        /// <returns>Expression bound by an environment consisting of the hoisted constants.</returns>
        IExpressionWithEnvironment IConstantHoister.Hoist(Expression expression) => Hoist(expression);

        /// <summary>
        /// Gets the positions of holes in the specified argument list.
        /// </summary>
        /// <param name="exclusion">Exclusion containing the holes.</param>
        /// <param name="arguments">Arguments to analyze.</param>
        /// <returns>List with position indexes of arguments that refer to any of the holes declared in the exclusion.</returns>
        private static List<int> GetHolePositions(LambdaExpression exclusion, IEnumerable<Expression> arguments)
        {
            var usedHoles = new HashSet<ParameterExpression>();

            var res = new List<int>();

            var i = 0;
            foreach (var argument in arguments)
            {
                if (argument is ParameterExpression p && exclusion.Parameters.Contains(p))
                {
                    if (!usedHoles.Add(p))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Hole '{0}' is used multiple times in exclusion '{1}'.", p, exclusion));
                    }

                    res.Add(i);
                }
                else
                {
                    AssertExpression(argument, exclusion);
                }

                i++;
            }

            if (usedHoles.Count != exclusion.Parameters.Count)
            {
                var unused = string.Join(", ", exclusion.Parameters.Except(usedHoles));
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "One or more holes declared in exclusion '{0}' are not used: {1}.", exclusion, unused));
            }

            return res;
        }

        /// <summary>
        /// Asserts that the specified argument found in the specified exclusion is not a complex expression.
        /// </summary>
        /// <param name="argument">Argument to check.</param>
        /// <param name="exclusion">Exclusion containing the argument.</param>
        private static void AssertExpression(Expression argument, LambdaExpression exclusion)
        {
            if (argument != null && argument.NodeType != ExpressionType.Constant && argument.NodeType != ExpressionType.Default)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Argument '{0}' in exclusion '{1}' is not a constant or a default expression.", argument, exclusion));
            }
        }

        private sealed class Impl : ExpressionVisitor
        {
            private readonly ConstantHoister _parent;
            private readonly Dictionary<object, ParameterExpression> _constants;
            private readonly Dictionary<Type, ParameterExpression> _nulls;

            public Impl(ConstantHoister parent)
            {
                _parent = parent;
                _constants = new Dictionary<object, ParameterExpression>(new TypeAwareComparer());
                Environment = new List<Binding>();

                if (!_parent._useDefaultForNull)
                {
                    _nulls = new Dictionary<Type, ParameterExpression>();
                }
            }

            private sealed class TypeAwareComparer : IEqualityComparer<object>
            {
                public new bool Equals(object x, object y)
                {
                    if (x.GetType() != y.GetType())
                    {
                        return false;
                    }

                    return object.Equals(x, y);
                }

                public int GetHashCode(object obj) => obj.GetType().GetHashCode() * 17 + obj.GetHashCode();
            }

            public List<Binding> Environment { get; }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != null)
                {
                    if (_parent._holes.TryGetValue(node.Member, out List<int> holes))
                    {
                        var f = VisitArgument(node.Expression, 0, holes);

                        return node.Update(f);
                    }
                }

                return base.VisitMember(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (_parent._holes.TryGetValue(node.Method, out List<int> holes))
                {
                    var o = node.Object;

                    var p = o;
                    if (o != null)
                    {
                        p = VisitArgument(o, 0, holes);
                    }

                    var b = VisitArguments(node.Arguments, 1, holes);

                    return node.Update(p, b);
                }

                return base.VisitMethodCall(node);
            }

            protected override Expression VisitNew(NewExpression node)
            {
                if (node.Constructor != null)
                {
                    if (_parent._holes.TryGetValue(node.Constructor, out List<int> holes))
                    {
                        var b = VisitArguments(node.Arguments, 0, holes);
                        return node.Update(b);
                    }
                }

                return base.VisitNew(node);
            }

            private IEnumerable<Expression> VisitArguments(ReadOnlyCollection<Expression> arguments, int startIndex, List<int> holes)
            {
                var b = new List<Expression>(arguments.Count);

                var i = startIndex;
                foreach (var argument in arguments)
                {
                    b.Add(VisitArgument(argument, i, holes));
                    i++;
                }

                return b;
            }

            private Expression VisitArgument(Expression argument, int index, List<int> holes)
            {
                if (holes.Contains(index) && argument.NodeType == ExpressionType.Constant)
                {
                    return argument;
                }
                else
                {
                    return Visit(argument);
                }
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                ParameterExpression res;

                if (node.Value == null)
                {
                    if (_nulls == null)
                    {
                        return Expression.Default(node.Type);
                    }
                    else
                    {
                        if (!_nulls.TryGetValue(node.Type, out res))
                        {
                            res = GetParameter(node);
                            _nulls[node.Type] = res;

                            var hoisted = _parent.HoistSafe(node);
                            Environment.Add(new Binding(res, hoisted));
                        }
                    }
                }
                else
                {
                    if (!_constants.TryGetValue(node.Value, out res))
                    {
                        res = GetParameter(node);
                        _constants[node.Value] = res;

                        var hoisted = _parent.HoistSafe(node);
                        Environment.Add(new Binding(res, hoisted));
                    }
                }

                return res;
            }

            private ParameterExpression GetParameter(ConstantExpression node) => Expression.Parameter(node.Type, "@p" + Environment.Count);
        }
    }
}
