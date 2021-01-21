// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Applies beta reduction on invocation expressions (applications) applied to lambda expressions (abstractions).
    /// Various expression tree rewrites can use this step to inline expressions in a template represented by a lambda expression.
    /// </summary>
    /// <example>
    /// Consider the following expression:
    /// <code>
    /// Expression.Invoke(Expression.Lambda(x, x), Expression.Constant(42))
    /// </code>
    /// After beta reduction, the resulting expression is:
    /// <code>
    /// Expression.Constant(42)
    /// </code>
    /// </example>
    public static class BetaReducer
    {
        /// <summary>
        /// Applies beta reduction on invocation expressions in the given expression. Only arguments of an atomic expression node type will be inlined.
        /// </summary>
        /// <param name="expression">Expression to apply beta reductions on.</param>
        /// <returns>Expression after applying beta reductions.</returns>
        public static Expression Reduce(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ReduceCore(expression, BetaReductionNodeTypes.Atoms, BetaReductionRestrictions.None);
        }

        /// <summary>
        /// Applies beta reduction on invocation expressions in the given expression, using the specified configuration flags.
        /// This is an advanced method which should be used with care when side-effects in expression trees are critical to maintain.
        /// Some configurations may lead to changes in timing and the arity of side-effects.
        /// </summary>
        /// <param name="expression">Expression to apply beta reductions on.</param>
        /// <param name="nodeTypes">Flags to restrict the argument expression node types that will be inlined during beta reduction. (Default: Atoms)</param>
        /// <param name="restrictions">Flags to restrict the number of uses of each argument expression during inlining. (Default: None)</param>
        /// <returns>Expression after applying beta reductions.</returns>
        public static Expression Reduce(Expression expression, BetaReductionNodeTypes nodeTypes, BetaReductionRestrictions restrictions)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return ReduceCore(expression, nodeTypes, restrictions);
        }

        /// <summary>
        /// Applies beta reduction on invocation expressions in the given expression, using the specified configuration flags, until no further reductions are possible.
        /// This is an advanced method which should be used with care when side-effects in expression trees are critical to maintain.
        /// Some configurations may lead to changes in timing and the arity of side-effects.
        /// </summary>
        /// <param name="expression">Expression to apply beta reductions on.</param>
        /// <param name="nodeTypes">Flags to restrict the argument expression node types that will be inlined during beta reduction.</param>
        /// <param name="restrictions">Flags to restrict the number of uses of each argument expression during inlining.</param>
        /// <param name="throwOnCycle">Indicates whether to throw an exception if the reduction gets stuck in a cyclic reduction (e.g. for a recursive lambda expression). If set to false, the reduction stops and the current expression is returned.</param>
        /// <returns>Expression after applying beta reductions.</returns>
        public static Expression ReduceEager(Expression expression, BetaReductionNodeTypes nodeTypes, BetaReductionRestrictions restrictions, bool throwOnCycle)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var history = new HashSet<Expression>(new ExpressionEqualityComparer());

            var current = expression;
            var reduced = default(Expression);

            var i = 0;
            while (current != reduced)
            {
                if (!history.Add(current))
                {
                    if (throwOnCycle)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Irreducible recursive lambda expression detected: '{0}'.", current));
                    }
                    else
                    {
                        break;
                    }
                }

                reduced = current;
                current = ReduceCore(current, nodeTypes, restrictions);

                i++;
            }

            return current;
        }

        private static Expression ReduceCore(Expression expression, BetaReductionNodeTypes nodeTypes, BetaReductionRestrictions restrictions) => new BetaReductionExpressionVisitor(nodeTypes, restrictions).Visit(expression);

        private abstract class Entry
        {
            public Entry(Expression expression) => Expression = expression;

            public Expression Expression { get; }

            public virtual int Count => 0;

            public virtual void AddRef()
            {
            }

            public abstract bool ShouldSubstitute
            {
                get;
            }
        }

        private sealed class Preservation : Entry
        {
            public Preservation(Expression expression)
                : base(expression)
            {
            }

            public override bool ShouldSubstitute => false;
        }

        private sealed class Substitution : Entry
        {
            private int _count;

            public Substitution(Expression expression)
                : base(expression)
            {
            }

            public override void AddRef() => _count++;

            public override int Count => _count;

            public override bool ShouldSubstitute => true;
        }

        private sealed class BetaReductionExpressionVisitor : ScopedExpressionVisitor<Entry>
        {
            private readonly bool _includeConstant;
            private readonly bool _includeDefault;
            private readonly bool _includeParameter;
            private readonly bool _includeQuote;
            private readonly bool _includeMolecules;
            private readonly bool _inDangerOfCaptures;

            private readonly bool _disallowDiscard;
            private readonly bool _disallowMultiple;

            public BetaReductionExpressionVisitor(BetaReductionNodeTypes nodeTypes, BetaReductionRestrictions restrictions)
            {
                _includeConstant = (nodeTypes & BetaReductionNodeTypes.Constant) != 0;
                _includeDefault = (nodeTypes & BetaReductionNodeTypes.Default) != 0;
                _includeParameter = (nodeTypes & BetaReductionNodeTypes.Parameter) != 0;
                _includeQuote = (nodeTypes & BetaReductionNodeTypes.Quote) != 0;
                _includeMolecules = (nodeTypes & BetaReductionNodeTypes.Molecules) != 0;
                _inDangerOfCaptures = _includeParameter | _includeQuote | _includeMolecules;

                _disallowDiscard = (restrictions & BetaReductionRestrictions.DisallowDiscard) != 0;
                _disallowMultiple = (restrictions & BetaReductionRestrictions.DisallowMultiple) != 0;
            }

            protected override Expression VisitInvocation(InvocationExpression node)
            {
                if (node.Expression.NodeType == ExpressionType.Lambda)
                {
                    var lambda = (LambdaExpression)node.Expression;
                    var arguments = Visit(node.Arguments);

                    var parameters = lambda.Parameters;
                    var n = parameters.Count;

                    var frame = new KeyValuePair<ParameterExpression, Entry>[n];
                    var excludeCount = 0;

                    for (var i = 0; i < n; i++)
                    {
                        var parameter = parameters[i];
                        var argument = arguments[i];
                        var include = argument.NodeType switch
                        {
                            ExpressionType.Constant => _includeConstant,
                            ExpressionType.Default => _includeDefault,
                            ExpressionType.Parameter => _includeParameter,
                            ExpressionType.Quote => _includeQuote,
                            _ => _includeMolecules,
                        };
                        if (include && _inDangerOfCaptures)
                        {
                            include = CanInline(lambda.Body, parameter, argument);
                        }

                        Entry entry;

                        if (include)
                        {
                            entry = new Substitution(argument);
                        }
                        else
                        {
                            entry = new Preservation(argument);
                            excludeCount++;
                        }

                        frame[i] = new KeyValuePair<ParameterExpression, Entry>(parameter, entry);
                    }

                    base.Push(frame);

                    var body = Visit(lambda.Body);

                    base.Pop();

                    var remainderParameters = default(List<ParameterExpression>);
                    var remainderArguments = default(List<Expression>);

                    foreach (var kv in frame)
                    {
                        var entry = kv.Value;

                        if (entry.ShouldSubstitute)
                        {
                            var msg = default(string);

                            if (entry.Count == 0)
                            {
                                if (_disallowDiscard)
                                {
                                    msg = "Expression '{0}' bound to parameter '{1}' could not be processed by beta reduction. The bound parameter does not occur in the body '{2}' and is not allowed to be discarded.";
                                }
                            }
                            else if (entry.Count > 1)
                            {
                                if (_disallowMultiple)
                                {
                                    msg = "Expression '{0}' bound to parameter '{1}' could not be processed by beta reduction. The bound parameter is not allowed to be used multiple times in '{2}'.";
                                }
                            }

                            if (msg != null)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, msg, entry.Expression, kv.Key, lambda));
                            }
                        }
                        else
                        {
                            if (remainderParameters == null)
                            {
                                remainderParameters = new List<ParameterExpression>(excludeCount);
                                remainderArguments = new List<Expression>(excludeCount);
                            }

                            remainderParameters.Add(kv.Key);
                            remainderArguments.Add(entry.Expression);
                        }
                    }

                    if (remainderParameters != null)
                    {
                        return Expression.Invoke(Expression.Lambda(body, remainderParameters), remainderArguments);
                    }
                    else
                    {
                        return body;
                    }
                }

                return base.VisitInvocation(node);
            }

            private static bool CanInline(Expression context, ParameterExpression parameter, Expression expression)
            {
                var fvs = FreeVariableScanner.Scan(expression).AsCollection();

                if (fvs.Count > 0)
                {
                    var ca = new CaptureAnalyzer(parameter, fvs);
                    ca.Visit(context);

                    return !ca.HasCapture;
                }

                return true;
            }

            private sealed class CaptureAnalyzer : ScopedExpressionVisitor<ParameterExpression>
            {
                private readonly ParameterExpression _parameter;
                private readonly IEnumerable<ParameterExpression> _freeVariables;

                public CaptureAnalyzer(ParameterExpression parameter, IEnumerable<ParameterExpression> freeVariables)
                {
                    _parameter = parameter;
                    _freeVariables = freeVariables;
                    HasCapture = false;
                }

                public bool HasCapture { get; private set; }

                protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (!HasCapture)
                    {
                        if (node == _parameter)
                        {
                            foreach (var fv in _freeVariables)
                            {
                                if (base.TryLookup(fv, out _))
                                {
                                    HasCapture = true;
                                }
                            }
                        }
                    }

                    return base.VisitParameter(node);
                }
            }

            protected override Entry GetState(ParameterExpression parameter) => new Preservation(parameter);

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (base.TryLookup(node, out Entry state) && state.ShouldSubstitute)
                {
                    state.AddRef();
                    return state.Expression;
                }

                return base.VisitParameter(node);
            }
        }
    }

    /// <summary>
    /// Flags to control which expression node types for invocation arguments are allowed during a beta reduction.
    /// Because beta reduction moves expressions to usage sites of parameters, timing of side-effects can change.
    /// </summary>
    [Flags]
    public enum BetaReductionNodeTypes
    {
        /// <summary>
        /// No expression node types are considered.
        /// </summary>
        None = 0,

        /// <summary>
        /// Nodes of type ExpressionType.Constant. Usage of constant expressions is free of side-effects and is safe for inlining during beta reduction.
        /// </summary>
        Constant = 1,

        /// <summary>
        /// Nodes of type ExpressionType.Default. Usage of default expressions is free of side-effects and is safe for inlining during beta reduction.
        /// </summary>
        Default = 2,

        /// <summary>
        /// Nodes of type ExpressionType.Parameter. Usage of parameter expressions is free of side-effects and is safe for inlining during beta reduction.
        /// </summary>
        Parameter = 4,

        /// <summary>
        /// Nodes of type ExpressionType.Quote. Usage of quotation expressions is free of side-effects and is safe for inlining during beta reduction.
        /// </summary>
        Quote = 8,

        /// <summary>
        /// Expression tree nodes with atomic structure, which are free of side-effects and are safe for inlining during beta reduction.
        /// </summary>
        Atoms = Constant | Default | Parameter | Quote,

        /// <summary>
        /// Expression tree nodes with non-atomic structure, which may exhibit side-effects whose timing could be affected by beta reduction.
        /// </summary>
        Molecules = 128,

        /// <summary>
        /// All expression tree nodes. This includes expressions which may exhibit side-effects whose timing could be affected by beta reduction.
        /// </summary>
        Unrestricted = Atoms | Molecules,
    }

    /// <summary>
    /// Flags to control restrictions for usage of argument expressions in the body of a lambda during beta reduction.
    /// Because beta reduction moves expressions to usage sites of parameters, the arity of side-effects can change.
    /// </summary>
    [Flags]
    public enum BetaReductionRestrictions
    {
        /// <summary>
        /// No restriction. An argument expression can be used any number of times ('*' Kleene closure).
        /// </summary>
        None = 0,

        /// <summary>
        /// An argument expression cannot be discarded and needs to be used at least once ('+' Kleene closure).
        /// This is a static restriction and provides no guarantees about dynamic runtime behavior, which is dependent on the use site(s) of the expression.
        /// </summary>
        DisallowDiscard = 1,

        /// <summary>
        /// An argument expression can be used at most once ('?' Kleene closure).
        /// This is a static restriction and provides no guarantees about dynamic runtime behavior, which is dependent on the use site(s) of the expression.
        /// </summary>
        DisallowMultiple = 2,

        /// <summary>
        /// An argument expression should be used exactly once.
        /// This is a static restriction and provides no guarantees about dynamic runtime behavior, which is dependent on the use site(s) of the expression.
        /// </summary>
        ExactlyOnce = DisallowDiscard | DisallowMultiple,
    }
}
