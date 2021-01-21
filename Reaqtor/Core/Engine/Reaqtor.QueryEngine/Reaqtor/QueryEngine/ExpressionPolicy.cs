// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;

using Reaqtive.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Expression policy with settable properties.
    /// </summary>
    public class ExpressionPolicy : IExpressionPolicy
    {
        private ICompiledDelegateCache _delegateCache;
        private ICache<Expression> _inMemoryCache;
        private IConstantHoister _constantHoister;
        private bool? _outlineCompilation;
        private IReflectionProvider _reflectionProvider;
        private IExpressionFactory _expressionFactory;
        private IMemoizer _liftMemoizer;
        private IMemoizer _reduceMemoizer;

        /// <summary>
        /// Internal constructor, this is only used for <see cref="ConfigurationOptions" />.
        /// </summary>
        internal ExpressionPolicy() { }

        /// <summary>
        /// Compiled delegate cache for reuse across expressions.
        /// </summary>
        public ICompiledDelegateCache DelegateCache
        {
            get => _delegateCache ?? DefaultExpressionPolicy.Instance.DelegateCache;
            set => _delegateCache = value;
        }

        /// <summary>
        /// Cache for in-memory storage of expressions.
        /// </summary>
        public ICache<Expression> InMemoryCache
        {
            get => _inMemoryCache ?? DefaultExpressionPolicy.Instance.InMemoryCache;
            set => _inMemoryCache = value;
        }

        /// <summary>
        /// Constant hoister for optimized sharing in expressions and evaluation.
        /// </summary>
        public IConstantHoister ConstantHoister
        {
            get => _constantHoister ?? DefaultExpressionPolicy.Instance.ConstantHoister;
            set => _constantHoister = value;
        }

        /// <summary>
        /// Specifies whether nested lambda expressions should be outlined into
        /// delegate constants by recursive compilation using the cache.
        /// </summary>
        public bool OutlineCompilation
        {
            get => _outlineCompilation ?? DefaultExpressionPolicy.Instance.OutlineCompilation;
            set => _outlineCompilation = value;
        }

        /// <summary>
        /// Gets the reflection provider to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        public IReflectionProvider ReflectionProvider
        {
            get => _reflectionProvider ?? DefaultExpressionPolicy.Instance.ReflectionProvider;
            set => _reflectionProvider = value;
        }

        /// <summary>
        /// Gets the expression factory to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        public IExpressionFactory ExpressionFactory
        {
            get => _expressionFactory ?? DefaultExpressionPolicy.Instance.ExpressionFactory;
            set => _expressionFactory = value;
        }

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed lift functions used
        /// by the expression serializer.
        /// </summary>
        public IMemoizer LiftMemoizer
        {
            get => _liftMemoizer ?? DefaultExpressionPolicy.Instance.LiftMemoizer;
            set => _liftMemoizer = value;
        }

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed reduce functions used
        /// by the expression deserializer.
        /// </summary>
        public IMemoizer ReduceMemoizer
        {
            get => _reduceMemoizer ?? DefaultExpressionPolicy.Instance.ReduceMemoizer;
            set => _reduceMemoizer = value;
        }
    }
}
