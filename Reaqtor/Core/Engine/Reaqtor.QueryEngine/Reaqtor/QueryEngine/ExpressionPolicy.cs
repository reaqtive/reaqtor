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
        private bool? _outlineCompilation;

        /// <summary>
        /// Internal constructor, this is only used for <see cref="ConfigurationOptions" />.
        /// </summary>
        internal ExpressionPolicy() { }

        /// <summary>
        /// Compiled delegate cache for reuse across expressions.
        /// </summary>
        public ICompiledDelegateCache DelegateCache
        {
            get => field ?? DefaultExpressionPolicy.Instance.DelegateCache;
            set;
        }

        /// <summary>
        /// Cache for in-memory storage of expressions.
        /// </summary>
        public ICache<Expression> InMemoryCache
        {
            get => field ?? DefaultExpressionPolicy.Instance.InMemoryCache;
            set;
        }

        /// <summary>
        /// Constant hoister for optimized sharing in expressions and evaluation.
        /// </summary>
        public IConstantHoister ConstantHoister
        {
            get => field ?? DefaultExpressionPolicy.Instance.ConstantHoister;
            set;
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
            get => field ?? DefaultExpressionPolicy.Instance.ReflectionProvider;
            set;
        }

        /// <summary>
        /// Gets the expression factory to use for reducing ExpressionSlim instances
        /// to Expression instances during deserialization.
        /// </summary>
        public IExpressionFactory ExpressionFactory
        {
            get => field ?? DefaultExpressionPolicy.Instance.ExpressionFactory;
            set;
        }

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed lift functions used
        /// by the expression serializer.
        /// </summary>
        public IMemoizer LiftMemoizer
        {
            get => field ?? DefaultExpressionPolicy.Instance.LiftMemoizer;
            set;
        }

        /// <summary>
        /// Gets the memoizer used to memoize strongly typed reduce functions used
        /// by the expression deserializer.
        /// </summary>
        public IMemoizer ReduceMemoizer
        {
            get => field ?? DefaultExpressionPolicy.Instance.ReduceMemoizer;
            set;
        }
    }
}
