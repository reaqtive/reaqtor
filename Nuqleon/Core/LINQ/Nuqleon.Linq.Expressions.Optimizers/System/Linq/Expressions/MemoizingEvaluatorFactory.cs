// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Memory;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Factory for member invocation delegates with support to cache compiled delegates.
    /// </summary>
    public class MemoizingEvaluatorFactory : DefaultEvaluatorFactory
    {
        /// <summary>
        /// Memoized delegate for the <see cref="GetEvaluator(MemberInfo)"/> method. The use of this delegate
        /// is subject to <see cref="ShouldCache(MemberInfo)"/> checks.
        /// </summary>
        private readonly IMemoizedDelegate<Func<MemberInfo, Delegate>> _getEvaluator;

        /// <summary>
        /// Creates a new evaluator factory using the specified <paramref name="memoizer"/> to cache the
        /// compiled delegates created by <see cref="ShouldCache(MemberInfo)"/>. Note that the member objects
        /// and the evaluator delegates can be held alive by the cache created through the memoizer. See
        /// other constructor overloads for alternative options, if needed.
        /// </summary>
        /// <param name="memoizer">The memoizer to use for caching <see cref="GetEvaluator(MemberInfo)"/> calls.</param>
        public MemoizingEvaluatorFactory(IMemoizer memoizer)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));

            _getEvaluator = memoizer.Memoize<MemberInfo, Delegate>(base.GetEvaluator);
        }

        /// <summary>
        /// Creates a new evaluator factory using the specified <paramref name="memoizer"/> to cache the
        /// compiled delegates created by <see cref="ShouldCache(MemberInfo)"/>. A weak memoizer is used which
        /// can help to prevent the cache from keeping member objects and evaluator delegates alive.
        /// </summary>
        /// <param name="memoizer">The memoizer to use for caching <see cref="GetEvaluator(MemberInfo)"/> calls.</param>
        public MemoizingEvaluatorFactory(IWeakMemoizer memoizer)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));

            _getEvaluator = memoizer.MemoizeWeak<MemberInfo, Delegate>(base.GetEvaluator);
        }

        /// <summary>
        /// Gets the underlying cache used for the memoized <see cref="GetEvaluator(MemberInfo)"/> method.
        /// </summary>
        public IMemoizationCache GetEvaluatorMemberCache => _getEvaluator.Cache;

        /// <summary>
        /// Checks whether an evaluator delegate for the specified member should be cached.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><c>true</c> if the evaluator delegate should be cached; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method enables derived classes to have a more fine-grained control over caching, for example
        /// to suppress caching of generic members which may cause to unbounded cache growth.
        /// </remarks>
        public virtual bool ShouldCache(MemberInfo member) => true;

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="member"/>.</returns>
        public override Delegate GetEvaluator(MemberInfo member)
        {
            if (ShouldCache(member))
            {
                return _getEvaluator.Delegate(member);
            }
            else
            {
                return base.GetEvaluator(member);
            }
        }

        // NB: No caching for GetEvaluator(UnaryExpression) or GetEvaluator(BinaryExpression); these
        //     provide their own caching in the expression optimizer because the number of distinct
        //     delegates these can generate is bounded (i.e. only for primitive types).
    }
}
