// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// A default implementation of the expression policy.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DefaultExpressionPolicy : IExpressionPolicy
    {
        /// <summary>
        /// Default expression policy without any side-effects.
        /// </summary>
        public static readonly DefaultExpressionPolicy Instance = new();

        private DefaultExpressionPolicy() { }

        /// <summary>
        /// Compiled delegate cache for reuse across expressions.
        /// </summary>
        public ICache<Expression> InMemoryCache => DefaultExpressionCache.Instance;

        /// <summary>
        /// Cache for in-memory storage of expressions.
        /// </summary>
        public ICompiledDelegateCache DelegateCache => DefaultCompiledDelegateCache.Instance;

        /// <summary>
        /// Constant hoister for optimized sharing in expressions and evaluation.
        /// </summary>
        public IConstantHoister ConstantHoister { get; } = System.Linq.CompilerServices.ConstantHoister.Create(useDefaultForNull: false);

        /// <summary>
        /// Specifies whether nested lambda expressions should be outlined into
        /// delegate constants by recursive compilation using the cache.
        /// </summary>
        public bool OutlineCompilation => false;

        /// <summary>
        /// Gets the reflection provider to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        public IReflectionProvider ReflectionProvider => DefaultReflectionProvider.Instance;

        /// <summary>
        /// Gets the expression factory to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        public IExpressionFactory ExpressionFactory => System.Linq.Expressions.ExpressionFactory.Instance;

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed lift functions used
        /// by the expression serializer.
        /// </summary>
        public IMemoizer LiftMemoizer { get; } = Memoizer.Create(ConcurrentMemoizationCacheFactory.Unbounded);

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed reduce functions used
        /// by the expression deserializer.
        /// </summary>
        public IMemoizer ReduceMemoizer => LiftMemoizer;

        private sealed class DefaultCompiledDelegateCache : ICompiledDelegateCache
        {
            public static readonly DefaultCompiledDelegateCache Instance = new();

            private DefaultCompiledDelegateCache() { }

            public int Count => 0;

            public Delegate GetOrAdd(LambdaExpression expression)
            {
                if (expression == null)
                {
                    throw new ArgumentNullException(nameof(expression));
                }

                return expression.Compile();
            }

            public void Clear()
            {
            }
        }

        private sealed class DefaultExpressionCache : ICache<Expression>
        {
            public static readonly DefaultExpressionCache Instance = new();

            private DefaultExpressionCache() { }

            public IDiscardable<Expression> Create(Expression value) => new CacheReference(value);

            private sealed class CacheReference : IDiscardable<Expression>
            {
                public CacheReference(Expression expression)
                {
                    Value = expression;
                }

                public Expression Value { get; }

                public void Dispose()
                {
                }
            }
        }
    }
}
