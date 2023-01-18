// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;

using Reaqtor.Reactive.Expressions;

namespace Reaqtor.Expressions
{
    //
    // CONSIDER: Move to System.Linq.CompilerServices if there's a meaningful generalization.
    //

    /// <summary>
    /// Factory for quoted expressions. This class and its method are included in quoted expression trees
    /// and can therefore get persisted as part of a serialized expression tree. DO NOT CHANGE this class's
    /// or any of its members' names.
    /// </summary>
    internal static class QuoteFactory
    {
        //
        // CONSIDER: It may be desirable to wire a cache instance to the quote factory through the arguments slot
        //           of the Create calls, allowing the cache to be maintained per engine (similar to the other
        //           aspects controlled by the expression policy).
        //

        /// <summary>
        /// A cache for code creators for each quoted type. The key of a cache entry is the quote type, which is not
        /// strongly rooted in order to support collection of dynamically generated types. The value of a cache entry
        /// is a dictionary that maps the runtime types of the arguments passed to the quote instantiation operation
        /// onto <see cref="Creator"/> instances, which encapsulate the quote object instantiation logic.
        /// </summary>
        /// <remarks>
        /// This cache is not bounded but in practice the number of unique quote types is limited to the number of
        /// different sequence types used in subscription expressions. Many data structures exist that are linear
        /// in the same way. The only drawback is that this is a global cache.
        /// </remarks>
        private static readonly ConditionalWeakTable<Type, ConcurrentDictionary<ArgTypes, Creator>> s_creators = new();

        /// <summary>
        /// Creates a new quoted expression representation.
        /// </summary>
        /// <typeparam name="T">Type of the value represented by the quote.</typeparam>
        /// <typeparam name="R">Type of the quote which implements <see cref="IQuoted{T}"/>.</typeparam>
        /// <param name="value">Value to be quoted.</param>
        /// <param name="expression">Expression representation of the quoted value.</param>
        /// <param name="args">Additional constructor arguments given to the quoted type <typeparamref name="R"/>.</param>
        /// <returns>Quote object containing the given value and its expression representation.</returns>
        public static T Create<T, R>(T value, Expression expression, params object[] args)
            where R : IQuoted<T>
        {
            if (value is R)
            {
                return value;
            }

            return CreateInstance<T, R>(value, expression, args ?? Array.Empty<object>());
        }

        /// <summary>
        /// Creates a new quoted expression representation including an evaluation environment.
        /// </summary>
        /// <typeparam name="T">Type of the value represented by the quote.</typeparam>
        /// <typeparam name="R">Type of the quote which implements <see cref="IQuoted{T}"/>.</typeparam>
        /// <param name="value">Value to be quoted.</param>
        /// <param name="expression">Expression representation of the quoted value.</param>
        /// <param name="environment">Environment of parameters in scope inside the quote.</param>
        /// <param name="args">Additional constructor arguments given to the quoted type <typeparamref name="R"/>.</param>
        /// <returns>Quote object containing the given value and its expression representation.</returns>
        public static T Create<T, R>(T value, Expression expression, ValueBinding[] environment, params object[] args)
            where R : IQuoted<T>
        {
            if (value is R)
            {
                return value;
            }

            var n = environment.Length;

            var parameters = new ParameterExpression[n];
            var arguments = new Expression[n];

            for (var i = 0; i < n; i++)
            {
                var binding = environment[i];
                parameters[i] = binding.Variable;
                arguments[i] = Expression.Constant(binding.Value, binding.Variable.Type);
            }

            //
            // NB: We don't reduce the invocation expression in order to avoid many copies of expression trees;
            //     the `expression` is likely shared, e.g. in a query like xs.SelectMany(x => f(x)) where f(x)
            //     gets applied with a different environment many times.
            //

            var expr = Expression.Invoke(Expression.Lambda(expression, parameters), arguments);

            return CreateInstance<T, R>(value, expr, args ?? Array.Empty<object>());
        }

        /// <summary>
        /// Creates an instance of the quote type <typeparamref name="T"/> with the specified arguments.
        /// </summary>
        /// <param name="value">The value to pass to the quote constructor.</param>
        /// <param name="expression">The expression to pass to the quote constructor.</param>
        /// <param name="args">The additional arguments to pass to the quote constructor.</param>
        /// <returns>An instance of the quote type <typeparamref name="T"/> with the specified arguments.</returns>
        private static T CreateInstance<T, R>(T value, Expression expression, object[] args)
        {
            var quoteType = typeof(R);

            //
            // First, try to get the dictionary of creators for the quote type, organized by argument types. We
            // retrieve the element using TryGetValue rather than GetOrCreateValue in order to get control over
            // the creation code path, because we want to avoid using the default ConcurrentDictionary<K, V>
            // constructors which assumes a high degree of concurrency (4 * CPU count) and a high capacity (31).
            //

            if (!s_creators.TryGetValue(quoteType, out var ctors))
            {
                //
                // NB: We anticipate very few write accesses to each entry in the concurrent dictionary. In fact,
                //     with the current quote types, we only expect one entry that gets written just once. As
                //     such, we don't need high degrees of concurrency for writes. Note that locks are only used
                //     by the concurrent dictionary for write operations.
                //

                ctors = s_creators.GetValue(quoteType, t => new ConcurrentDictionary<ArgTypes, Creator>(concurrencyLevel: 1, capacity: 1));
            }

            //
            // Next, build an ArgTypes struct value. Unfortunately, this requires allocating a Type array, but
            // this is no worse than finding a ConstructorInfo object from parameter types, or instantiating an
            // object[] to perform an Activator.CreateInstance (which internally also allocates a Type[]).
            //

            var ctorParameterTypes = new Type[args.Length + 2];

            ctorParameterTypes[0] = typeof(T);
            ctorParameterTypes[1] = typeof(Expression);

            for (var i = 0; i < args.Length; i++)
            {
                ctorParameterTypes[i + 2] = args[i]?.GetType() ?? typeof(object);
            }

            var argTypes = new ArgTypes(ctorParameterTypes);

            //
            // Finally, get or create a Creator<T, R> instance from the dictionary. We do this using a double-
            // check pattern as well, so we can avoid delegate allocation or instance allocation for the common
            // path (which would be required to call GetOrAdd).
            //

            if (!ctors.TryGetValue(argTypes, out var creator))
            {
                creator = ctors.GetOrAdd(argTypes, new Creator<T, R>(argTypes));
            }

            return ((Creator<T, R>)creator).Invoke(value, expression, args);
        }

        /// <summary>
        /// A struct holding an array of <see cref="Type"/> instances representing the types of the arguments
        /// passed to a quote instantiation call. Equality semantics are based on pairwise equality of these
        /// argument types.
        /// </summary>
        private readonly struct ArgTypes : IEquatable<ArgTypes>
        {
            /// <summary>
            /// A shared instance of a <see cref="ParameterExpression"/> of with <see cref="Expression.Type"/>
            /// set to <see cref="Expression"/>.
            /// </summary>
            private static readonly ParameterExpression s_expression = Expression.Parameter(typeof(Expression), "expression");

            /// <summary>
            /// A shared instance of a <see cref="ParameterExpression"/> of with <see cref="Expression.Type"/>
            /// set to <c>object[]</c>.
            /// </summary>
            private static readonly ParameterExpression s_args = Expression.Parameter(typeof(object[]), "args");

            /// <summary>
            /// An array containing shared instances for <see cref="ExpressionType.ArrayIndex"/> expressions
            /// that index into <see cref="s_args"/> at the index corresponding to the array slot.
            /// </summary>
            private static readonly Expression[] s_argsIndices = new Expression[8];

            /// <summary>
            /// The array of argument types.
            /// </summary>
            private readonly Type[] _types;

            /// <summary>
            /// Creates a new <see cref="ArgTypes"/> instance using the specified argument <paramref name="types"/>.
            /// </summary>
            /// <param name="types">The array of argument types.</param>
            public ArgTypes(Type[] types)
            {
                _types = types;
            }

            /// <summary>
            /// Gets the public instance constructor on the specified <paramref name="type"/> using the parameter
            /// types specified by the current instance.
            /// </summary>
            /// <param name="type">The type for which to get a constructor.</param>
            /// <returns>The public constructor instance with matching parameter types, or <c>null</c>.</returns>
            public ConstructorInfo GetConstructor(Type type)
            {
                return type.GetConstructor(_types);
            }

            /// <summary>
            /// Gets a compiled constructor function on the specified <paramref name="type"/> using the parameter
            /// types specified by the current instance.
            /// </summary>
            /// <typeparam name="T">The type of the value parameter.</typeparam>
            /// <param name="type">The type for which to get a constructor.</param>
            /// <returns>A compiled constructor function for fast instantiation of quotes.</returns>
            public Func<T, Expression, object[], T> Compile<T>(Type type)
            {
                //
                // Build an expression of the shape:
                //
                //   (T value, Expression expression, object[] args) => new R(value, expression, (T0)args[0], ..., (Tn)args[n])
                //

                var valueParameter = Expression.Parameter(typeof(T), "value");

                var ctor = GetConstructor(type);
                var parameters = ctor.GetParameters();
                var count = parameters.Length;

                var args = new Expression[count];

                args[0] = valueParameter;
                args[1] = s_expression;

                for (var i = 0; i < count - 2; i++)
                {
                    var parameterType = parameters[i + 2].ParameterType;
                    args[i + 2] = Expression.Convert(GetArgumentAt(i), parameterType);
                }

                var expr =
                    Expression.Lambda<Func<T, Expression, object[], T>>(
                        Expression.New(ctor, args),
                        valueParameter,
                        s_expression,
                        s_args
                    );

                return expr.Compile();
            }

            /// <summary>
            /// Gets an <see cref="ExpressionType.ArrayIndex"/> expression that indexes into <see cref="s_args"/>
            /// at the specified <paramref name="index"/>.
            /// </summary>
            /// <param name="index">The index of the argument to retrieve.</param>
            /// <returns>A new <see cref="ExpressionType.ArrayIndex"/> expression or a cache instance.</returns>
            private static Expression GetArgumentAt(int index)
            {
                if (index < s_argsIndices.Length)
                {
                    return s_argsIndices[index] ?? (s_argsIndices[index] = Create());
                }

                return Create();

                Expression Create()
                {
                    return Expression.ArrayIndex(s_args, Expression.Constant(index));
                }
            }

            /// <summary>
            /// Checks whether the specified object is equal to the current instance.
            /// </summary>
            /// <param name="other">The object to check equality for.</param>
            /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
            public bool Equals(ArgTypes other)
            {
                var count = _types.Length;

                if (other._types.Length != count)
                {
                    return false;
                }

                for (var i = 0; i < count; i++)
                {
                    if (_types[i] != other._types[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Checks whether the specified object is equal to the current instance.
            /// </summary>
            /// <param name="obj">The object to check equality for.</param>
            /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj) => obj is ArgTypes other && Equals(other);

            /// <summary>
            /// Gets a hash code for the current instance.
            /// </summary>
            /// <returns>A hash code for the current instance.</returns>
            /// <remarks>
            /// The hash code returned by this function is simply the length of the types array represented
            /// by the current instance. In the context of the quoter, there's typically just one entry per
            /// type because quote types are typically instantiated using one custom argument in addition to
            /// the fixed value and expression arguments.
            /// </remarks>
            public override int GetHashCode() => _types.Length;
        }

        /// <summary>
        /// Weakly typed base class for <see cref="Creator{T, R}"/> types.
        /// </summary>
        private class Creator
        {
        }

        /// <summary>
        /// Represents a quote creator for a quote of type <typeparamref name="R"/> which is exposed as an object
        /// of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The interface type of the quote.</typeparam>
        /// <typeparam name="R">The concrete type of the quote implementation.</typeparam>
        private sealed class Creator<T, R> : Creator
        {
            /// <summary>
            /// The number of invocations to the <see cref="Invoke"/> method required to trigger creation
            /// of a compiled constructor delegate.
            /// </summary>
            /// <remarks>
            /// This number if currently set to a low threshold because it's very common for an expression
            /// to contain many nodes of the same type that needs quotation. Furthermore, many of these
            /// expressions tend to be similar and benefit from quick quote creation. In particular recovery
            /// scenarios benefit from having compiled quote constructors. Also note that the performance
            /// gain from compilation compared to reflection-based invocation is about a factor 4x, so this
            /// number also makes sense to trigger optimization around the elbow point.
            /// </remarks>
            private const int CompilationInvocationThreshold = 4;

            /// <summary>
            /// The underlying constructor function invoked by <see cref="Invoke"/>. This function can be
            /// substituted at runtime by <see cref="SpeedupAndInvoke"/> for a faster compiled one.
            /// </summary>
            private Func<T, Expression, object[], T> _createInstance;

            /// <summary>
            /// Creates a new <see cref="Creator"/> that supports creating quotes of type <c>R</c> by
            /// invoking the constructor with the parameter types specified in <paramref name="argTypes"/>.
            /// </summary>
            /// <param name="argTypes">The types of the constructor parameters.</param>
            public Creator(ArgTypes argTypes)
            {
                //
                // NB: We intentionally create a closure over the ctor, count, and argTypes variables
                //     for the lambda expression below. This enables the Creator<T, R> instance to stay
                //     lean after it replaces the slow initial reflection-based constructor delegate
                //     (referencing this closure object) for a fast compilation-based constructor delegate,
                //     at which point this auxiliary state is no longer needed.
                //

                var ctor = argTypes.GetConstructor(typeof(R));
                var count = 0;

                _createInstance = (value, expression, args) =>
                {
                    if (Interlocked.Increment(ref count) == CompilationInvocationThreshold)
                    {
                        return SpeedupAndInvoke(argTypes, value, expression, args);
                    }

                    var ctorArgs = new object[args.Length + 2];
                    ctorArgs[0] = value;
                    ctorArgs[1] = expression;
                    Array.Copy(args, 0, ctorArgs, 2, args.Length);

                    try
                    {
                        return (T)ctor.Invoke(ctorArgs);
                    }
                    catch (TargetInvocationException tie)
                    {
                        ExceptionDispatchInfo.Capture(tie.InnerException).Throw();
                        return default; // NB: Unreachable code.
                    }
                };
            }

            /// <summary>
            /// Invokes the constructor of the quote type <c>T</c> with the specified arguments.
            /// </summary>
            /// <param name="value">The value to pass to the quote constructor.</param>
            /// <param name="expression">The expression to pass to the quote constructor.</param>
            /// <param name="args">The additional arguments to pass to the quote constructor.</param>
            /// <returns>An instance of the quote type <c>T</c> with the specified arguments.</returns>
            public T Invoke(T value, Expression expression, object[] args)
            {
                //
                // NB: We don't bother synchronizing access to this field for reads in Invoke and writes
                //     in SpeedupAndInvoke. Worst case, we end up reading the slow reflection-based creator
                //     delegate while the fast compilation-based creator is being prepared and assigned.
                //

                return _createInstance(value, expression, args);
            }

            /// <summary>
            /// Replaces the underlying quote constructor delegate by a compiled one and invokes it with
            /// the specified arguments. All subsequent calls to <see cref="Invoke"/> will use the compiled
            /// constructor delegate after this function returns.
            /// </summary>
            /// <param name="argTypes">The types of the constructor parameters.</param>
            /// <param name="value">The value to pass to the quote constructor.</param>
            /// <param name="expression">The expression to pass to the quote constructor.</param>
            /// <param name="args">The additional arguments to pass to the quote constructor.</param>
            /// <returns>An instance of the quote type <c>T</c> with the specified arguments.</returns>
            private T SpeedupAndInvoke(ArgTypes argTypes, T value, Expression expression, object[] args)
            {
                Interlocked.Exchange(ref _createInstance, argTypes.Compile<T>(typeof(R)));

                return _createInstance(value, expression, args);
            }
        }
    }

    /// <summary>
    /// Representing of a binding of a free variable to value.
    /// </summary>
    internal struct ValueBinding
    {
#pragma warning disable IDE0034 // Simplify 'default' expression

        /// <summary>
        /// The <see cref="ConstructorInfo"/> object for the constructor on <see cref="ValueBinding"/>.
        /// </summary>
        public static readonly ConstructorInfo Constructor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new ValueBinding(default(ParameterExpression), default(object)));

#pragma warning restore IDE0034 // Simplify 'default' expression

        /// <summary>
        /// Creates a new <see cref="ValueBinding"/> with the specified arguments.
        /// </summary>
        /// <param name="variable">The free variable that's bound to <paramref name="value"/>.</param>
        /// <param name="value">The value that's bound to <paramref name="variable"/>.</param>
        public ValueBinding(ParameterExpression variable, object value)
        {
            Variable = variable;
            Value = value;
        }

        /// <summary>
        /// Gets the free variable that's bound to <see cref="Value"/>.
        /// </summary>
        public ParameterExpression Variable;

        /// <summary>
        /// Gets the value that's bound to <see cref="Variable"/>.
        /// </summary>
        public object Value;
    }
}
