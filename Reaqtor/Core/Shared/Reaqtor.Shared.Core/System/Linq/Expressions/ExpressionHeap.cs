// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2014 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Memory;

namespace System.Linq.Expressions
{
    /// <summary>
    /// A heap for compacting expressions by hoisting constants and sharing the remaining template.
    /// </summary>
    public class ExpressionHeap : Cache<Expression, Expression, IReadOnlyIndexed>
    {
        /// <summary>
        /// A single boxed instance of `Unit` for use as a separator in the bundle.
        /// </summary>
        private static readonly object s_canary = new Unit();

        private readonly IConstantHoister _hoister;

        /// <summary>
        /// Initializes the expression heap with a default constant hoister and template cache.
        /// </summary>
        public ExpressionHeap()
            : this(ConstantHoister.Create(useDefaultForNull: false))
        {
        }

        /// <summary>
        /// Initializes the expression heap with a default template cache.
        /// </summary>
        /// <param name="hoister">The constant hoister.</param>
        public ExpressionHeap(IConstantHoister hoister)
            : this(hoister, comparer => new Cache<Expression>(new CacheStorage<Expression>(comparer)))
        {
        }

        /// <summary>
        /// Initializes the expression heap with the given template cache.
        /// </summary>
        /// <param name="hoister">The constant hoister.</param>
        /// <param name="cacheFactory">
        /// Creates the template cache using the given equality comparer.
        /// </param>
        public ExpressionHeap(IConstantHoister hoister, Func<IEqualityComparer<Expression>, ICache<Expression>> cacheFactory)
            : base((cacheFactory ?? throw new ArgumentNullException(nameof(cacheFactory)))(Comparer.Instance))
        {
            _hoister = hoister ?? throw new ArgumentNullException(nameof(hoister));
        }

        /// <summary>
        /// Deconstructs the expression into a template and a set of filler values used to reconstruct from the template.
        /// </summary>
        /// <param name="value">The original expression.</param>
        /// <returns>The deconstructed expression.</returns>
        protected sealed override Deconstructed<Expression, IReadOnlyIndexed> Deconstruct(Expression value)
        {
            var globals = FreeVariableScanner.Scan(value).Where(p => !ShouldShareGlobal(p)).ToArray();
            var hoisted = _hoister.Hoist(value);
            var bindingCount = hoisted.Bindings.Count;
            var count = bindingCount + globals.Length;

            var shared = default(Expression);
            var unshared = default(IReadOnlyIndexed);
            if (count > 0)
            {
                var parameters = new ParameterExpression[count];
                var values = new object[count + 1 /* canary */];

                for (var i = 0; i < bindingCount; ++i)
                {
                    var binding = hoisted.Bindings[i];
                    parameters[i] = binding.Parameter;

                    // For now, only hoisted constants are supported... throw?
                    Debug.Assert(binding.Value is ConstantExpression);
                    var val = binding.Value.Evaluate();

                    // Recursively cache constants containing expressions
                    if (val is Expression expressionValue)
                    {
                        val = Create(expressionValue);
                    }

                    values[i] = val;
                }

                // The canary signals that the rest of the values are for globals.
                // The reason is to support cases where ConstantExpressions may
                // contain ParameterExpressions, and recursive caching may not
                // be desired. Strictly speaking, this is probably overkill,
                // since we recursively cache constants containing expressions.
                // Thus the presence of a global parameter in a bundle could
                // also signal that it should be slotted in as a parameter.
                values[bindingCount] = s_canary;

                for (var i = 0; i < globals.Length; ++i)
                {
                    parameters[i + bindingCount] = globals[i];
                    values[i + bindingCount + 1] = globals[i];
                }

                shared = HeapLambda.Create(hoisted.Expression, parameters);
                unshared = Bundle.Create(values);
            }
            else
            {
                shared = value;
                unshared = Bundle.Create(Array.Empty<object>());
            }

            // Must be Expression<Delegate> to avoid the automatic creation of
            // a delegate type in a non-collectible assembly
            return Deconstructed.Create(shared, unshared);
        }

        /// <summary>
        /// Reconstructs the expression from a template and a set of filler values.
        /// </summary>
        /// <param name="deconstructed">The deconstructed expression.</param>
        /// <returns>An equivalent expression to the one that was originally cached.</returns>
        protected sealed override Expression Reconstruct(Deconstructed<Expression, IReadOnlyIndexed> deconstructed)
        {
            if (deconstructed.Cached is not HeapLambda lambda)
            {
                return deconstructed.Cached;
            }

            var count = lambda.Parameters.Length;
            var values = new Dictionary<ParameterExpression, SlotValue>(count);
            var kind = SlotKind.Constant;

            for (var i = 0; i < count + 1; ++i)
            {
                var value = deconstructed.NonCached[i];
                if (value is Unit)
                {
                    kind = SlotKind.Parameter;
                }
                else
                {
                    var pindex = kind == SlotKind.Constant ? i : i - 1;
                    values.Add(lambda.Parameters[pindex], new SlotValue(value, kind));
                }
            }

            var visitor = new Revisitor(values);
            return visitor.Visit(lambda.Body);
        }

        /// <summary>
        /// Checks if a global parameter should be shared in the cache.
        /// </summary>
        /// <param name="parameter">The global parameter.</param>
        /// <returns>
        /// <b>true</b> if the parameter expression should be part of the
        /// shared template expression, <b>false</b> otherwise.
        /// </returns>
        protected virtual bool ShouldShareGlobal(ParameterExpression parameter) => false;

        //
        // PERF: Consider using pooled comparator instances.
        //

        private sealed class Comparer : ExpressionEqualityComparer
        {
            public static readonly Comparer Instance = new();

            private Comparer() : base(() => new Comparator()) { }

            private sealed class Comparator : ExpressionEqualityComparator
            {
                private const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

                protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y) => x.Name == y.Name && Equals(x.Type, y.Type);

                protected override bool EqualsExtension(Expression x, Expression y)
                {
                    if (x is HeapLambda xLambda && y is HeapLambda yLambda)
                    {
                        if (xLambda.Parameters.Length != yLambda.Parameters.Length)
                        {
                            return false;
                        }

                        EqualsPush(xLambda.Parameters, yLambda.Parameters);
                        try
                        {
                            return
                                EqualsHeapLambdaParameters(xLambda.Parameters, yLambda.Parameters) &&
                                Equals(xLambda.Body, yLambda.Body);
                        }
                        finally
                        {
                            EqualsPop();
                        }
                    }

                    return base.EqualsExtension(x, y);
                }

                private bool EqualsHeapLambdaParameters(ParameterExpression[] x, ParameterExpression[] y)
                {
                    Debug.Assert(x.Length == y.Length);

                    for (var i = 0; i < x.Length; ++i)
                    {
                        if (!Equals(x[i].Type, y[i].Type))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                protected override int GetHashCodeGlobalParameter(ParameterExpression obj)
                {
                    var hash = EqualityComparer<string>.Default.GetHashCode(obj.Name);

                    unchecked
                    {
                        return (int)(hash * Prime) + GetHashCode(obj.Type);
                    }
                }

                protected override int GetHashCodeExtension(Expression obj)
                {
                    if (obj is HeapLambda lambda)
                    {
                        GetHashCodePush(lambda.Parameters);
                        try
                        {
                            unchecked
                            {
                                var hash = GetHashCodeHeapLambdaParameters(lambda.Parameters);
                                return (int)(hash * Prime) + GetHashCode(lambda.Body);
                            }
                        }
                        finally
                        {
                            GetHashCodePop();
                        }
                    }

                    return base.GetHashCodeExtension(obj);
                }

                private int GetHashCodeHeapLambdaParameters(ParameterExpression[] obj)
                {
                    unchecked
                    {
                        var hash = (int)Prime;
                        for (var i = 0; i < obj.Length; ++i)
                        {
                            hash = (int)(hash * Prime) + GetHashCode(obj[i].Type);
                        }

                        return hash;
                    }
                }
            }
        }

        private sealed class Revisitor : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly Dictionary<ParameterExpression, SlotValue> _values;

            public Revisitor(Dictionary<ParameterExpression, SlotValue> values) => _values = values;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!TryLookup(node, out _) && _values.TryGetValue(node, out SlotValue slotValue))
                {
                    if (slotValue.Kind == SlotKind.Constant)
                    {
                        return slotValue.Value is IDiscardable<Expression> cachedReference && typeof(Expression).IsAssignableFrom(node.Type)
                            ? Expression.Constant(cachedReference.Value, node.Type)
                            : Expression.Constant(slotValue.Value, node.Type);
                    }
                    else
                    {
                        Debug.Assert(slotValue.Kind == SlotKind.Parameter);
                        return (ParameterExpression)slotValue.Value;
                    }
                }

                return base.VisitParameter(node);
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }

        private readonly struct SlotValue
        {
            public SlotValue(object value, SlotKind kind)
            {
                Value = value;
                Kind = kind;
            }

            public object Value { get; }

            public SlotKind Kind { get; }
        }

        private enum SlotKind : byte
        {
            Constant = 1,
            Parameter = 2,
        }

        private struct Unit { }

        private sealed class HeapLambda : Expression
        {
            private HeapLambda(Expression body, ParameterExpression[] parameters)
            {
                Body = body;
                Parameters = parameters;
            }

            public override ExpressionType NodeType => ExpressionType.Extension;

            public Expression Body { get; }

            public ParameterExpression[] Parameters { get; }

            public static HeapLambda Create(Expression body, ParameterExpression[] parameters) => new(body, parameters);
        }
    }
}
