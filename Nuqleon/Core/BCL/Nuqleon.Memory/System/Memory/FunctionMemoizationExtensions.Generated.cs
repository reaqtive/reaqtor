// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// #define USE_VALUETUPLE // REVIEW: Is the use of ValueTuple desirable? Cf. the implementation of GetHashCode for arity >= 8.

using System.Collections.Generic;

namespace System.Memory
{
    partial class FunctionMemoizationExtensions
    {
        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, TResult>> Memoize<T1, T2, TResult>(this IMemoizer memoizer, Func<T1, T2, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2), TResult>(a => function(a.Item1, a.Item2), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, TResult>>((t1, t2) => memoized((t1, t2)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2>, TResult>(a => function(a.Item1, a.Item2), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, TResult>>((t1, t2) => memoized(new Tuplet<T1, T2>(t1, t2)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, TResult>> Memoize<T1, T2, T3, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3), TResult>(a => function(a.Item1, a.Item2, a.Item3), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, TResult>>((t1, t2, t3) => memoized((t1, t2, t3)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3>, TResult>(a => function(a.Item1, a.Item2, a.Item3), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, TResult>>((t1, t2, t3) => memoized(new Tuplet<T1, T2, T3>(t1, t2, t3)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, TResult>> Memoize<T1, T2, T3, T4, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, TResult>>((t1, t2, t3, t4) => memoized((t1, t2, t3, t4)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, TResult>>((t1, t2, t3, t4) => memoized(new Tuplet<T1, T2, T3, T4>(t1, t2, t3, t4)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>> Memoize<T1, T2, T3, T4, T5, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>>((t1, t2, t3, t4, t5) => memoized((t1, t2, t3, t4, t5)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>>((t1, t2, t3, t4, t5) => memoized(new Tuplet<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>> Memoize<T1, T2, T3, T4, T5, T6, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>>((t1, t2, t3, t4, t5, t6) => memoized((t1, t2, t3, t4, t5, t6)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>>((t1, t2, t3, t4, t5, t6) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>((t1, t2, t3, t4, t5, t6, t7) => memoized((t1, t2, t3, t4, t5, t6, t7)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>((t1, t2, t3, t4, t5, t6, t7) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8) => memoized((t1, t2, t3, t4, t5, t6, t7, t8)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>(t1, t2, t3, t4, t5, t6, t7, t8)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1, t2, t3, t4, t5, t6, t7, t8, t9)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument.</typeparam>
        /// <typeparam name="T15">Type of the fifteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument.</typeparam>
        /// <typeparam name="T15">Type of the fifteenth function argument.</typeparam>
        /// <typeparam name="T16">Type of the sixteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

#if USE_VALUETUPLE
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15, a.Item16), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16)), res.Cache);
#else
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15, a.Item16), options, comparer: null);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, TResult>> Memoize<T1, T2, TResult>(this IMemoizer memoizer, Func<T1, T2, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2);
            var res = memoizer.Memoize<(T1, T2), TResult>(a => function(a.Item1, a.Item2), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, TResult>>((t1, t2) => memoized((t1, t2)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2);
            var res = memoizer.Memoize<Tuplet<T1, T2>, TResult>(a => function(a.Item1, a.Item2), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, TResult>>((t1, t2) => memoized(new Tuplet<T1, T2>(t1, t2)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, TResult>> Memoize<T1, T2, T3, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3);
            var res = memoizer.Memoize<(T1, T2, T3), TResult>(a => function(a.Item1, a.Item2, a.Item3), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, TResult>>((t1, t2, t3) => memoized((t1, t2, t3)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3>, TResult>(a => function(a.Item1, a.Item2, a.Item3), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, TResult>>((t1, t2, t3) => memoized(new Tuplet<T1, T2, T3>(t1, t2, t3)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, TResult>> Memoize<T1, T2, T3, T4, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4);
            var res = memoizer.Memoize<(T1, T2, T3, T4), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, TResult>>((t1, t2, t3, t4) => memoized((t1, t2, t3, t4)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, TResult>>((t1, t2, t3, t4) => memoized(new Tuplet<T1, T2, T3, T4>(t1, t2, t3, t4)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>> Memoize<T1, T2, T3, T4, T5, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>>((t1, t2, t3, t4, t5) => memoized((t1, t2, t3, t4, t5)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>>((t1, t2, t3, t4, t5) => memoized(new Tuplet<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>> Memoize<T1, T2, T3, T4, T5, T6, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>>((t1, t2, t3, t4, t5, t6) => memoized((t1, t2, t3, t4, t5, t6)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>>((t1, t2, t3, t4, t5, t6) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>((t1, t2, t3, t4, t5, t6, t7) => memoized((t1, t2, t3, t4, t5, t6, t7)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>((t1, t2, t3, t4, t5, t6, t7) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8) => memoized((t1, t2, t3, t4, t5, t6, t7, t8)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8>(t1, t2, t3, t4, t5, t6, t7, t8)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9>(t1, t2, t3, t4, t5, t6, t7, t8, t9)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer11">Comparer to compare the eleventh function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));
            if (comparer11 == null)
                throw new ArgumentNullException(nameof(comparer11));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer11">Comparer to compare the eleventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer12">Comparer to compare the twelfth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));
            if (comparer11 == null)
                throw new ArgumentNullException(nameof(comparer11));
            if (comparer12 == null)
                throw new ArgumentNullException(nameof(comparer12));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer11">Comparer to compare the eleventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer12">Comparer to compare the twelfth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer13">Comparer to compare the thirteenth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));
            if (comparer11 == null)
                throw new ArgumentNullException(nameof(comparer11));
            if (comparer12 == null)
                throw new ArgumentNullException(nameof(comparer12));
            if (comparer13 == null)
                throw new ArgumentNullException(nameof(comparer13));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer11">Comparer to compare the eleventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer12">Comparer to compare the twelfth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer13">Comparer to compare the thirteenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer14">Comparer to compare the fourteenth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));
            if (comparer11 == null)
                throw new ArgumentNullException(nameof(comparer11));
            if (comparer12 == null)
                throw new ArgumentNullException(nameof(comparer12));
            if (comparer13 == null)
                throw new ArgumentNullException(nameof(comparer13));
            if (comparer14 == null)
                throw new ArgumentNullException(nameof(comparer14));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument.</typeparam>
        /// <typeparam name="T15">Type of the fifteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer11">Comparer to compare the eleventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer12">Comparer to compare the twelfth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer13">Comparer to compare the thirteenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer14">Comparer to compare the fourteenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer15">Comparer to compare the fifteenth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));
            if (comparer11 == null)
                throw new ArgumentNullException(nameof(comparer11));
            if (comparer12 == null)
                throw new ArgumentNullException(nameof(comparer12));
            if (comparer13 == null)
                throw new ArgumentNullException(nameof(comparer13));
            if (comparer14 == null)
                throw new ArgumentNullException(nameof(comparer14));
            if (comparer15 == null)
                throw new ArgumentNullException(nameof(comparer15));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> and the specified comparers to compare function arguments against cache entries.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument.</typeparam>
        /// <typeparam name="T2">Type of the second function argument.</typeparam>
        /// <typeparam name="T3">Type of the third function argument.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument.</typeparam>
        /// <typeparam name="T15">Type of the fifteenth function argument.</typeparam>
        /// <typeparam name="T16">Type of the sixteenth function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="comparer1">Comparer to compare the first function argument during lookup in the memoization cache.</param>
        /// <param name="comparer2">Comparer to compare the second function argument during lookup in the memoization cache.</param>
        /// <param name="comparer3">Comparer to compare the third function argument during lookup in the memoization cache.</param>
        /// <param name="comparer4">Comparer to compare the fourth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer5">Comparer to compare the fifth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer6">Comparer to compare the sixth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer7">Comparer to compare the seventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer8">Comparer to compare the eighth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer9">Comparer to compare the ninth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer10">Comparer to compare the tenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer11">Comparer to compare the eleventh function argument during lookup in the memoization cache.</param>
        /// <param name="comparer12">Comparer to compare the twelfth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer13">Comparer to compare the thirteenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer14">Comparer to compare the fourteenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer15">Comparer to compare the fifteenth function argument during lookup in the memoization cache.</param>
        /// <param name="comparer16">Comparer to compare the sixteenth function argument during lookup in the memoization cache.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, MemoizationOptions options, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, IEqualityComparer<T3> comparer3, IEqualityComparer<T4> comparer4, IEqualityComparer<T5> comparer5, IEqualityComparer<T6> comparer6, IEqualityComparer<T7> comparer7, IEqualityComparer<T8> comparer8, IEqualityComparer<T9> comparer9, IEqualityComparer<T10> comparer10, IEqualityComparer<T11> comparer11, IEqualityComparer<T12> comparer12, IEqualityComparer<T13> comparer13, IEqualityComparer<T14> comparer14, IEqualityComparer<T15> comparer15, IEqualityComparer<T16> comparer16)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));
            if (comparer1 == null)
                throw new ArgumentNullException(nameof(comparer1));
            if (comparer2 == null)
                throw new ArgumentNullException(nameof(comparer2));
            if (comparer3 == null)
                throw new ArgumentNullException(nameof(comparer3));
            if (comparer4 == null)
                throw new ArgumentNullException(nameof(comparer4));
            if (comparer5 == null)
                throw new ArgumentNullException(nameof(comparer5));
            if (comparer6 == null)
                throw new ArgumentNullException(nameof(comparer6));
            if (comparer7 == null)
                throw new ArgumentNullException(nameof(comparer7));
            if (comparer8 == null)
                throw new ArgumentNullException(nameof(comparer8));
            if (comparer9 == null)
                throw new ArgumentNullException(nameof(comparer9));
            if (comparer10 == null)
                throw new ArgumentNullException(nameof(comparer10));
            if (comparer11 == null)
                throw new ArgumentNullException(nameof(comparer11));
            if (comparer12 == null)
                throw new ArgumentNullException(nameof(comparer12));
            if (comparer13 == null)
                throw new ArgumentNullException(nameof(comparer13));
            if (comparer14 == null)
                throw new ArgumentNullException(nameof(comparer14));
            if (comparer15 == null)
                throw new ArgumentNullException(nameof(comparer15));
            if (comparer16 == null)
                throw new ArgumentNullException(nameof(comparer16));

#if USE_VALUETUPLE
            var comparer = EqualityComparerExtensions.CombineWithValueTuple(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15, comparer16);
            var res = memoizer.Memoize<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16), TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15, a.Item16), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16)), res.Cache);
#else
            var comparer = EqualityComparerExtensions.CombineWithTuplet(comparer1, comparer2, comparer3, comparer4, comparer5, comparer6, comparer7, comparer8, comparer9, comparer10, comparer11, comparer12, comparer13, comparer14, comparer15, comparer16);
            var res = memoizer.Memoize<Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TResult>(a => function(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, a.Item7, a.Item8, a.Item9, a.Item10, a.Item11, a.Item12, a.Item13, a.Item14, a.Item15, a.Item16), options, comparer);
            var memoized = res.Delegate;
            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized(new Tuplet<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16)), res.Cache);
#endif
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, TResult>> MemoizeWeak<T1, T2, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, TResult>>(t1 =>
                    {
                        return memoizer.MemoizeWeak<T2, TResult>(t2 =>
                            function(t1, t2),
                            options
                        ).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, TResult>>((t1, t2) => memoized(t1)(t2), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, TResult>> MemoizeWeak<T1, T2, T3, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, TResult>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, TResult>>(t2 =>
                        {
                            return memoizer.MemoizeWeak<T3, TResult>(t3 =>
                                function(t1, t2, t3),
                                options
                            ).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, TResult>>((t1, t2, t3) => memoized(t1)(t2)(t3), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, TResult>> MemoizeWeak<T1, T2, T3, T4, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, TResult>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, TResult>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, TResult>>(t3 =>
                            {
                                return memoizer.MemoizeWeak<T4, TResult>(t4 =>
                                    function(t1, t2, t3, t4),
                                    options
                                ).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, TResult>>((t1, t2, t3, t4) => memoized(t1)(t2)(t3)(t4), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, TResult>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, TResult>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, TResult>>(t4 =>
                                {
                                    return memoizer.MemoizeWeak<T5, TResult>(t5 =>
                                        function(t1, t2, t3, t4, t5),
                                        options
                                    ).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, TResult>>((t1, t2, t3, t4, t5) => memoized(t1)(t2)(t3)(t4)(t5), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TResult>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, TResult>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, TResult>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, TResult>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, TResult>>(t5 =>
                                    {
                                        return memoizer.MemoizeWeak<T6, TResult>(t6 =>
                                            function(t1, t2, t3, t4, t5, t6),
                                            options
                                        ).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, TResult>>((t1, t2, t3, t4, t5, t6) => memoized(t1)(t2)(t3)(t4)(t5)(t6), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, TResult>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, TResult>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, TResult>>(t6 =>
                                        {
                                            return memoizer.MemoizeWeak<T7, TResult>(t7 =>
                                                function(t1, t2, t3, t4, t5, t6, t7),
                                                options
                                            ).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>((t1, t2, t3, t4, t5, t6, t7) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, TResult>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, TResult>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, TResult>>(t7 =>
                                            {
                                                return memoizer.MemoizeWeak<T8, TResult>(t8 =>
                                                    function(t1, t2, t3, t4, t5, t6, t7, t8),
                                                    options
                                                ).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, TResult>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, TResult>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, TResult>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, TResult>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, TResult>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, TResult>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, TResult>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, TResult>>(t8 =>
                                                {
                                                    return memoizer.MemoizeWeak<T9, TResult>(t9 =>
                                                        function(t1, t2, t3, t4, t5, t6, t7, t8, t9),
                                                        options
                                                    ).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, TResult>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, TResult>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, TResult>>(t9 =>
                                                    {
                                                        return memoizer.MemoizeWeak<T10, TResult>(t10 =>
                                                            function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10),
                                                            options
                                                        ).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, Func<T11, TResult>>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, Func<T11, TResult>>>(t9 =>
                                                    {
                                                        // Note the closure over t10 does not keep the cache entry alive due to CWT's guarantees.
                                                        return memoizer.MemoizeWeak<T10, Func<T11, TResult>>(t10 =>
                                                        {
                                                            return memoizer.MemoizeWeak<T11, TResult>(t11 =>
                                                                function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11),
                                                                options
                                                            ).Delegate;
                                                        }).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10)(t11), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, Func<T11, Func<T12, TResult>>>>(t9 =>
                                                    {
                                                        // Note the closure over t10 does not keep the cache entry alive due to CWT's guarantees.
                                                        return memoizer.MemoizeWeak<T10, Func<T11, Func<T12, TResult>>>(t10 =>
                                                        {
                                                            // Note the closure over t11 does not keep the cache entry alive due to CWT's guarantees.
                                                            return memoizer.MemoizeWeak<T11, Func<T12, TResult>>(t11 =>
                                                            {
                                                                return memoizer.MemoizeWeak<T12, TResult>(t12 =>
                                                                    function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12),
                                                                    options
                                                                ).Delegate;
                                                            }).Delegate;
                                                        }).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10)(t11)(t12), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>(t9 =>
                                                    {
                                                        // Note the closure over t10 does not keep the cache entry alive due to CWT's guarantees.
                                                        return memoizer.MemoizeWeak<T10, Func<T11, Func<T12, Func<T13, TResult>>>>(t10 =>
                                                        {
                                                            // Note the closure over t11 does not keep the cache entry alive due to CWT's guarantees.
                                                            return memoizer.MemoizeWeak<T11, Func<T12, Func<T13, TResult>>>(t11 =>
                                                            {
                                                                // Note the closure over t12 does not keep the cache entry alive due to CWT's guarantees.
                                                                return memoizer.MemoizeWeak<T12, Func<T13, TResult>>(t12 =>
                                                                {
                                                                    return memoizer.MemoizeWeak<T13, TResult>(t13 =>
                                                                        function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13),
                                                                        options
                                                                    ).Delegate;
                                                                }).Delegate;
                                                            }).Delegate;
                                                        }).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10)(t11)(t12)(t13), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
            where T14 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>(t9 =>
                                                    {
                                                        // Note the closure over t10 does not keep the cache entry alive due to CWT's guarantees.
                                                        return memoizer.MemoizeWeak<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>(t10 =>
                                                        {
                                                            // Note the closure over t11 does not keep the cache entry alive due to CWT's guarantees.
                                                            return memoizer.MemoizeWeak<T11, Func<T12, Func<T13, Func<T14, TResult>>>>(t11 =>
                                                            {
                                                                // Note the closure over t12 does not keep the cache entry alive due to CWT's guarantees.
                                                                return memoizer.MemoizeWeak<T12, Func<T13, Func<T14, TResult>>>(t12 =>
                                                                {
                                                                    // Note the closure over t13 does not keep the cache entry alive due to CWT's guarantees.
                                                                    return memoizer.MemoizeWeak<T13, Func<T14, TResult>>(t13 =>
                                                                    {
                                                                        return memoizer.MemoizeWeak<T14, TResult>(t14 =>
                                                                            function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14),
                                                                            options
                                                                        ).Delegate;
                                                                    }).Delegate;
                                                                }).Delegate;
                                                            }).Delegate;
                                                        }).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10)(t11)(t12)(t13)(t14), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T15">Type of the fifteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
            where T14 : class
            where T15 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>(t9 =>
                                                    {
                                                        // Note the closure over t10 does not keep the cache entry alive due to CWT's guarantees.
                                                        return memoizer.MemoizeWeak<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>(t10 =>
                                                        {
                                                            // Note the closure over t11 does not keep the cache entry alive due to CWT's guarantees.
                                                            return memoizer.MemoizeWeak<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>(t11 =>
                                                            {
                                                                // Note the closure over t12 does not keep the cache entry alive due to CWT's guarantees.
                                                                return memoizer.MemoizeWeak<T12, Func<T13, Func<T14, Func<T15, TResult>>>>(t12 =>
                                                                {
                                                                    // Note the closure over t13 does not keep the cache entry alive due to CWT's guarantees.
                                                                    return memoizer.MemoizeWeak<T13, Func<T14, Func<T15, TResult>>>(t13 =>
                                                                    {
                                                                        // Note the closure over t14 does not keep the cache entry alive due to CWT's guarantees.
                                                                        return memoizer.MemoizeWeak<T14, Func<T15, TResult>>(t14 =>
                                                                        {
                                                                            return memoizer.MemoizeWeak<T15, TResult>(t15 =>
                                                                                function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15),
                                                                                options
                                                                            ).Delegate;
                                                                        }).Delegate;
                                                                    }).Delegate;
                                                                }).Delegate;
                                                            }).Delegate;
                                                        }).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10)(t11)(t12)(t13)(t14)(t15), mem.Cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/> without keeping function arguments alive by the memoization cache.
        /// </summary>
        /// <typeparam name="T1">Type of the first function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T2">Type of the second function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T3">Type of the third function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T4">Type of the fourth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T5">Type of the fifth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T6">Type of the sixth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T7">Type of the seventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T8">Type of the eighth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T9">Type of the ninth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T10">Type of the tenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T11">Type of the eleventh function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T12">Type of the twelfth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T13">Type of the thirteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T14">Type of the fourteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T15">Type of the fifteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="T16">Type of the sixteenth function argument. This type has to be a reference type.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        /// <remarks>
        /// This method curries the specified function in order to build a hierarchy of weak reference memoization cache and can be expensive in terms of memory usage.
        /// If only a subset of function arguments need to be treated as weak references, consider manually curring the function, as illustrated below.
        /// <code><![CDATA[
        ///     //
        ///     // The memoization cache for this function should not keep T1 alive, but T2 poses no risk for a space leak:
        ///     //
        ///     Func<T1, T2, R> f = (x, y) => { return /* implementation */ };
        ///     
        ///     //
        ///     // Assume we have weak memoizers and regular memoizers available:
        ///     //
        ///     IMemoizer memstrong = ...;
        ///     IWeakMemoizer memweak = ...;
        ///     
        ///     //
        ///     // With this, we can manually memoize the function by curring it, as shown in a step-by-step fashion below:
        ///     //
        ///     // 1. Just curry the function
        ///     //
        ///     Func<T1, Func<T2, R>> memf1 = x => y => f(x, y);
        ///     
        ///     //
        ///     // 2. Memoize on the first argument using a weak memoizer
        ///     //
        ///     Func<T1, Func<T2, R>> memf2 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return y => f(x, y);
        ///                                   }).Delegate;
        ///     
        ///     //
        ///     // 3. Memoize the inner function
        ///     //
        ///     Func<T1, Func<T2, R>> memf3 = memweak.MemoizeWeak<T1, Func<T2, R>>(x =>
        ///                                   {
        ///                                       return memstrong.Memoize<T2, R>(y => f(x, y)).Delegate;
        ///                                   }).Delegate;
        ///
        ///     //
        ///     // Finally, we can uncurry the memoized function:
        ///     //
        ///     Func<T1, T2, R> memf = (x, y) => memf3(x)(y);
        /// ]]></code>
        /// Note that each entry in the weak memoization cache contains a closure over the first function argument due to the curried function application as well as a memoization cache for the second function argument.
        /// E.g. for a unary function there will be a total of 1 + N caches, where N is the number of (live references to) first function arguments that were fed to the memoized function.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Design of MemoizedDelegate")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Reduction of overloads; defaults are trivial zeros and nulls and can be specified manually if needed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2004:RemoveCallsToGCKeepAlive", Justification = "Not dealing with SafeHandle conversions here")]
        public static IMemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> MemoizeWeak<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this IWeakMemoizer memoizer, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where T9 : class
            where T10 : class
            where T11 : class
            where T12 : class
            where T13 : class
            where T14 : class
            where T15 : class
            where T16 : class
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var mem =
                    // Note the closure over t1 does not keep the cache entry alive due to CWT's guarantees.
                    memoizer.MemoizeWeak<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>>>>>(t1 =>
                    {
                        // Note the closure over t2 does not keep the cache entry alive due to CWT's guarantees.
                        return memoizer.MemoizeWeak<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>>>>(t2 =>
                        {
                            // Note the closure over t3 does not keep the cache entry alive due to CWT's guarantees.
                            return memoizer.MemoizeWeak<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>>>(t3 =>
                            {
                                // Note the closure over t4 does not keep the cache entry alive due to CWT's guarantees.
                                return memoizer.MemoizeWeak<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>>(t4 =>
                                {
                                    // Note the closure over t5 does not keep the cache entry alive due to CWT's guarantees.
                                    return memoizer.MemoizeWeak<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>(t5 =>
                                    {
                                        // Note the closure over t6 does not keep the cache entry alive due to CWT's guarantees.
                                        return memoizer.MemoizeWeak<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>(t6 =>
                                        {
                                            // Note the closure over t7 does not keep the cache entry alive due to CWT's guarantees.
                                            return memoizer.MemoizeWeak<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>(t7 =>
                                            {
                                                // Note the closure over t8 does not keep the cache entry alive due to CWT's guarantees.
                                                return memoizer.MemoizeWeak<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>(t8 =>
                                                {
                                                    // Note the closure over t9 does not keep the cache entry alive due to CWT's guarantees.
                                                    return memoizer.MemoizeWeak<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>(t9 =>
                                                    {
                                                        // Note the closure over t10 does not keep the cache entry alive due to CWT's guarantees.
                                                        return memoizer.MemoizeWeak<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>(t10 =>
                                                        {
                                                            // Note the closure over t11 does not keep the cache entry alive due to CWT's guarantees.
                                                            return memoizer.MemoizeWeak<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>(t11 =>
                                                            {
                                                                // Note the closure over t12 does not keep the cache entry alive due to CWT's guarantees.
                                                                return memoizer.MemoizeWeak<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>(t12 =>
                                                                {
                                                                    // Note the closure over t13 does not keep the cache entry alive due to CWT's guarantees.
                                                                    return memoizer.MemoizeWeak<T13, Func<T14, Func<T15, Func<T16, TResult>>>>(t13 =>
                                                                    {
                                                                        // Note the closure over t14 does not keep the cache entry alive due to CWT's guarantees.
                                                                        return memoizer.MemoizeWeak<T14, Func<T15, Func<T16, TResult>>>(t14 =>
                                                                        {
                                                                            // Note the closure over t15 does not keep the cache entry alive due to CWT's guarantees.
                                                                            return memoizer.MemoizeWeak<T15, Func<T16, TResult>>(t15 =>
                                                                            {
                                                                                return memoizer.MemoizeWeak<T16, TResult>(t16 =>
                                                                                    function(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16),
                                                                                    options
                                                                                ).Delegate;
                                                                            }).Delegate;
                                                                        }).Delegate;
                                                                    }).Delegate;
                                                                }).Delegate;
                                                            }).Delegate;
                                                        }).Delegate;
                                                    }).Delegate;
                                                }).Delegate;
                                            }).Delegate;
                                        }).Delegate;
                                    }).Delegate;
                                }).Delegate;
                            }).Delegate;
                        }).Delegate;
                    });

            var memoized = mem.Delegate;

            return new MemoizedDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => memoized(t1)(t2)(t3)(t4)(t5)(t6)(t7)(t8)(t9)(t10)(t11)(t12)(t13)(t14)(t15)(t16), mem.Cache);
        }

    }
}
