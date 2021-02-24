// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Memory
{
    /// <summary>
    /// Provides a set of extension methods for memoization of functions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Generated code")]
    public static partial class FunctionMemoizationExtensions
    {
        /// <summary>
        /// Types of tuplets used for argument bundles. The tuplet type at position n represents an (n + 1)-tuplet.
        /// </summary>
        private static readonly Type[] s_argsTypes = new[]
        {
            typeof(Tuplet<>),
            typeof(Tuplet<,>),
            typeof(Tuplet<,,>),
            typeof(Tuplet<,,,>),
            typeof(Tuplet<,,,,>),
            typeof(Tuplet<,,,,,>),
            typeof(Tuplet<,,,,,,>),
            typeof(Tuplet<,,,,,,,>),
            typeof(Tuplet<,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,,,,,,>),
            typeof(Tuplet<,,,,,,,,,,,,,,,>),
        };

        /// <summary>
        /// Type of the tuplet with a TRest parameter, used for spilling of parameters if the number exceeds 16.
        /// </summary>
        private static readonly Type s_argsRestType = typeof(Tuplet<,,,,,,,,,,,,,,,,>);

        /// <summary>
        /// Types of functions we have pre-compiled memoization methods for. The function type at position n represents a function with n + 1 parameters.
        /// </summary>
        private static readonly List<Type> s_funcTypes = new(new[]
        {
            typeof(Func<>),
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,,>),
        });

        /// <summary>
        /// Lazy computation of Memoize functions. The method at position n can memoize a function with n parameters.
        /// </summary>
        private static readonly Lazy<MethodInfo[]> s_memoizeFuncMethods = new(() => FindMemoizeMethods(nameof(Memoize), typeof(IMemoizer)));

        /// <summary>
        /// Lazy computation of MemoizeWeak functions. The method at position n can memoize a function with n parameters.
        /// </summary>
        private static readonly Lazy<MethodInfo[]> s_memoizeWeakFuncMethods = new(() => FindMemoizeMethods(nameof(MemoizeWeak), typeof(IWeakMemoizer)));

        /// <summary>
        /// Lazily initialized <see cref="IMemoizer.Memoize{T, TResult}"/> method info.
        /// </summary>
        private static MethodInfo s_IMemoizer_Memoize;

        /// <summary>
        /// Gets the <see cref="IMemoizer.Memoize{T, TResult}"/> method info.
        /// </summary>
        private static MethodInfo IMemoizer_Memoize => s_IMemoizer_Memoize ??= typeof(IMemoizer).GetMethod(nameof(Memoize));

        /// <summary>
        /// Lazily initialized <see cref="IWeakMemoizer.MemoizeWeak{T, TResult}"/> method info.
        /// </summary>
        private static MethodInfo s_IWeakMemoizer_MemoizeWeak;

        /// <summary>
        /// Gets the <see cref="IWeakMemoizer.MemoizeWeak{T, TResult}"/> method info.
        /// </summary>
        private static MethodInfo IWeakMemoizer_MemoizeWeak => s_IWeakMemoizer_MemoizeWeak ??= typeof(IWeakMemoizer).GetMethod(nameof(MemoizeWeak));

        /// <summary>
        /// Singleton instance of a <see cref="ConstantExpression"/> of type <see cref="object"/> whose <see cref="ConstantExpression.Value"/> is <c>null</c>.
        /// </summary>
        private static readonly Expression s_constantNull = Expression.Constant(value: null);

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// The memoization cache uses the default equality comparer for all the function's argument values.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the function.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<TDelegate> Memoize<TDelegate>(this IMemoizer memoizer, TDelegate function, MemoizationOptions options = MemoizationOptions.None)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            //
            // NB: In principle, memoizing a constant could return the original object.
            //
            var delegateType = typeof(TDelegate);
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException("The type of the function should be a delegate type.", nameof(function));

            //
            // If the delegate type is a generic instantiation of one of the Func<> types,
            // we can use the precompiled memoization method.
            //
            if (delegateType.IsGenericType)
            {
                var genDef = delegateType.GetGenericTypeDefinition();

                if (s_funcTypes.Contains(genDef))
                {
                    var genArgs = delegateType.GetGenericArguments();
                    var memoizeFuncMethod = s_memoizeFuncMethods.Value[genArgs.Length - 1].MakeGenericMethod(genArgs);
                    return (MemoizedDelegate<TDelegate>)memoizeFuncMethod.Invoke(obj: null, new object[] { memoizer, function, options });
                }
            }

            //
            // Analyze the delegate type by inspecting the signature of its Invoke method
            // to check for pointers and ref types which we won't memoize.
            //
            // NB: In principle, we could support ref parameters by treating them as both
            //     input and output. In that case, we'd need to hoist the function with a
            //     tupletized return type as well. For now, we'll refrain from this.
            //
            var invokeMethod = delegateType.GetMethod("Invoke");
            var invokeParameters = invokeMethod.GetParameters();

            var argCount = invokeParameters.Length;

            var argumentTypes = new Type[argCount];
            for (var i = 0; i < argCount; i++)
            {
                var invokeParameter = invokeParameters[i];
                var invokeParameterType = invokeParameter.ParameterType;
                AssertNoRefOrPointer(invokeParameterType, invokeParameter.Name);

                argumentTypes[i] = invokeParameterType;
            }

            var returnType = invokeMethod.ReturnType;
            AssertNoRefOrPointer(returnType, "return");

            //
            // Check for empty arguments and void-returning functions. We'll support both
            // by using System.Object as the parameter type and/or return type. Function
            // parameters get tupletized, see GetArgsType and UnpackArgs.
            //
            var hasNoArgs = argCount == 0;
            var hasNoReturn = returnType == typeof(void);

            var argsType = hasNoArgs ? typeof(object) : GetArgsType(argumentTypes);
            var resultType = hasNoReturn ? typeof(object) : returnType;

            var argsParam = Expression.Parameter(argsType);
            var args = hasNoArgs ? Array.Empty<Expression>() : UnpackArgs(argsParam, argCount);

            //
            // Create the invocation expression from the function and return null in case
            // the original function was void-returning.
            //
            var invokeFunction = Expression.Invoke(Expression.Constant(function), args);

            var memoizeeBody = hasNoReturn ? Expression.Block(invokeFunction, s_constantNull) : (Expression)invokeFunction;

            //
            // The memoized lambda will be of type Func<TArgs, TResult> where TArgs can
            // be a tuplet and TResult can be Systme.Object in lieu of void. We can now
            // memoize this function.
            //
            var memoizeeLambda = Expression.Lambda(memoizeeBody, argsParam);
            var memoizeeFunction = memoizeeLambda.Compile();

            var memoizeMethod = IMemoizer_Memoize.MakeGenericMethod(argsType, resultType);
            var memoizedDelegate = memoizeMethod.Invoke(memoizer, new object[] { memoizeeFunction, options, null });

            //
            // Get the Cache and Delegate properties of the result. We'll deconstruct the
            // memoized delegate in order to create the final resulting memoized function
            // which packs arguments to the function using CreateArgs and creates a lambda
            // of type TDelegate. The Cache gets re-wrapped in the resulting instance of
            // MemoizedDelegate<TDelegate>.
            //
            var cacheProperty = memoizeMethod.ReturnType.GetProperty(nameof(MemoizedDelegate<object>.Cache));
            var delegateProperty = memoizeMethod.ReturnType.GetProperty(nameof(MemoizedDelegate<object>.Delegate));

            var cache = (IMemoizationCache)cacheProperty.GetValue(memoizedDelegate, index: null);
            var @delegate = delegateProperty.GetValue(memoizedDelegate, index: null);
            Expression argsValue;
            ParameterExpression[] parameters;
            if (hasNoArgs)
            {
                parameters = Array.Empty<ParameterExpression>();
                argsValue = s_constantNull;
            }
            else
            {
                parameters = new ParameterExpression[argumentTypes.Length];

                for (var i = 0; i < argumentTypes.Length; i++)
                {
                    parameters[i] = Expression.Parameter(argumentTypes[i], "p" + i);
                }

                argsValue = CreateArgs(parameters);
            }

            var memoizedLambda = Expression.Lambda<TDelegate>(Expression.Invoke(Expression.Constant(@delegate), argsValue), parameters);
            var memoizedFunction = memoizedLambda.Compile();

            return new MemoizedDelegate<TDelegate>(memoizedFunction, cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="weakMemoizer"/> without keeping arguments alive.
        /// The memoization cache uses the default equality comparer for all the function's argument values.
        /// If the delegate parameters contain value types, the <paramref name="memoizer"/> is used to memoize these.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the function.</typeparam>
        /// <param name="weakMemoizer">The weak memoizer used to memoize the function's arguments that are reference types.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <param name="memoizer">The memoizer used to memoize the function's arguments that are value types.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<TDelegate> MemoizeWeak<TDelegate>(this IWeakMemoizer weakMemoizer, TDelegate function, MemoizationOptions options = MemoizationOptions.None, IMemoizer memoizer = null)
        {
            if (weakMemoizer == null)
                throw new ArgumentNullException(nameof(weakMemoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            //
            // NB: In principle, memoizing a constant could return the original object.
            //
            var delegateType = typeof(TDelegate);
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException("The type of the function should be a delegate type.", nameof(function));

            //
            // If the delegate type is a generic instantiation of one of the Func<> types,
            // we can use the precompiled memoization method. This can only be done if all
            // of the parameters are reference types, so check for that.
            //
            if (delegateType.IsGenericType)
            {
                var genDef = delegateType.GetGenericTypeDefinition();

                if (s_funcTypes.Contains(genDef))
                {
                    var genArgs = delegateType.GetGenericArguments();
                    if (genArgs.All(genArg => !genArg.IsValueType))
                    {
                        var memoizeFuncMethod = s_memoizeWeakFuncMethods.Value[genArgs.Length - 1].MakeGenericMethod(genArgs);
                        return (MemoizedDelegate<TDelegate>)memoizeFuncMethod.Invoke(obj: null, new object[] { weakMemoizer, function, options });
                    }
                }
            }

            //
            // Analyze the delegate type by inspecting the signature of its Invoke method
            // to check for pointers and ref types which we won't memoize.
            //
            // NB: The parameter type checks are happening further down.
            //
            // NB: In principle, we could support ref parameters by treating them as both
            //     input and output. In that case, we'd need to hoist the function with a
            //     tupletized return type as well. For now, we'll refrain from this.
            //
            var invokeMethod = delegateType.GetMethod("Invoke");
            var invokeParameters = invokeMethod.GetParameters();

            var returnType = invokeMethod.ReturnType;
            AssertNoRefOrPointer(returnType, "return");

            var argCount = invokeParameters.Length;

            //
            // Check for empty arguments and void-returning functions. We'll support both
            // by using System.Object as the parameter type and/or return type. Function
            // parameters get tupletized, see GetArgsType and UnpackArgs.
            //
            var hasNoArgs = argCount == 0;
            var hasNoReturn = returnType == typeof(void);

            //
            // NB: We could do a much better job here if we knew which parameters have a large domain,
            //     so we can reduce the number of small inner caches due to currying. For example:
            //
            //     (o1, v1, o2, v2) => f(o1, v1, o2, v2)
            //
            //     In here, o1 and o2 are reference types, and v1 and v2 are value types. We can emit
            //     code to curry the following ways:
            //
            //     o1 => o2 => (v1, v2) => f(o1, v1, o2, v2)
            //     o1 => (v1, v2) => o2 => f(o1, v1, o2, v2)
            //     o2 => o1 => (v1, v2) => f(o1, v1, o2, v2)
            //     o2 => (v1, v2) => o1 => f(o1, v1, o2, v2)
            //     (v1, v2) => o1 => o2 => f(o1, v1, o2, v2)
            //     (v1, v2) => o2 => o1 => f(o1, v1, o2, v2)
            //
            //     We like to keep the number of inner caches low, so want a low number of entries at
            //     the first partial applications of the memoized function. As such, avoid Cartesian
            //     products of value types, without any domain analysis (e.g. a Boolean has only two
            //     possible values), because in general such a product has a higher cardinality than
            //     an individual object or value. This leaves us with:
            //
            //     o1 => o2 => (v1, v2) => f(o1, v1, o2, v2)
            //     o1 => (v1, v2) => o2 => f(o1, v1, o2, v2)
            //     o2 => o1 => (v1, v2) => f(o1, v1, o2, v2)
            //     o2 => (v1, v2) => o1 => f(o1, v1, o2, v2)
            //
            //     By putting a weak cache on a curried form with a reference type as the input, the
            //     lifetime of inner caches can become shorter (at the expense of possible churn over
            //     time). To keep things simple, we'll use the first form which matches lexical order
            //     of the reference type parameters:
            //
            //     o1 => o2 => (v1, v2) => f(o1, v1, o2, v2)
            //

            var parameters = default(ParameterExpression[]);
            var arguments = default(List<Expression>);
            var curriedInvocation = default(Expression);

            if (hasNoArgs)
            {
                //
                // If there are no arguments, simply invoke the function and insert a fake parameter
                // of type System.Object in order to call MemoizeWeak. Also insert a dummy return
                // statement of type System.Object in case the function is void-returning.
                //
                parameters = Array.Empty<ParameterExpression>();
                arguments = new List<Expression>(1) { s_constantNull };

                curriedInvocation = Expression.Invoke(Expression.Constant(function));

                if (hasNoReturn)
                {
                    curriedInvocation = Expression.Block(curriedInvocation, s_constantNull);
                }

                var ignored = Expression.Parameter(typeof(object), "_");

                curriedInvocation = Expression.Call(
                    Expression.Constant(weakMemoizer, typeof(IWeakMemoizer)),
                    IWeakMemoizer_MemoizeWeak.MakeGenericMethod(typeof(object), curriedInvocation.Type),
                    Expression.Lambda(
                        curriedInvocation,
                        ignored
                    ),
                    Expression.Constant(options)
                );
            }
            else
            {
                //
                // If there are arguments, we'll split them into two groups: one for value types and
                // another for reference types. Based on this, we generate code to curry the function
                // as follows:
                //
                //     o1 => o2 => (v1, v2) => f(o1, v1, o2, v2)
                //
                // Next, we'll memoize the reference type parameters using the weak memoizer and the
                // value type parameters as a bundle using the non-weak memoizer.
                //
                // In the example above, we'll end up with (modulo parameter names):
                //
                //     parameters          = { o1, v1, o2, v2 }
                //     valueParameters     = { v1, v2 }
                //     curriedParameters   = { o1, o2 }
                //     invocationArguments = { o1, v.Item1, o2, v.Item2 }
                //
                // where v.* is obtained using UnpackArgs and v will be a tuplet type to represent the
                // bundle (v1, v2).
                //
                parameters = new ParameterExpression[argCount];
                arguments = new List<Expression>();

                var invocationArguments = new Expression[argCount];
                var valueParameters = new List<Expression>();
                var curriedParameters = new List<ParameterExpression>();

                for (var i = 0; i < argCount; i++)
                {
                    //
                    // Analyze each parameter's type to check for by-ref or pointer types, and to check
                    // for a value type or a reference type. Value types are collected to build a bundle,
                    // reference types are added to the list of curried parameters.
                    //
                    var invokeParameter = invokeParameters[i];
                    var invokeParameterType = invokeParameter.ParameterType;

                    AssertNoRefOrPointer(invokeParameterType, invokeParameter.Name);

                    var parameter = Expression.Parameter(invokeParameterType, "p" + i);
                    parameters[i] = parameter;

                    if (!invokeParameterType.IsValueType)
                    {
                        invocationArguments[i] = parameter;
                        curriedParameters.Add(parameter);
                    }
                    else
                    {
                        if (memoizer == null)
                        {
                            throw new ArgumentNullException(nameof(memoizer), "The delegate type contains parameters that are value types. A memoizer needs to be specified.");
                        }

                        valueParameters.Add(parameter);
                    }
                }

                //
                // If we have any value type parameters, create the bundle. The following steps are taken:
                //
                // - Create a parameter to represent the bundle, using a tuplet type obtained via GetArgsType.
                // - Create the unpack expressions for the function invocation, using UnpackArgs.
                // - Create the pack expression to bundle arguments, using CreateArgs.
                //
                var valueArgsType = default(Type);

                if (valueParameters.Count > 0)
                {
                    var allValueTypes = new Type[valueParameters.Count];
                    for (var i = 0; i < valueParameters.Count; i++)
                    {
                        allValueTypes[i] = valueParameters[i].Type;
                    }

                    valueArgsType = GetArgsType(allValueTypes);
                    var valueArgsParameter = Expression.Parameter(valueArgsType, "args");
                    curriedParameters.Add(valueArgsParameter);

                    var argsValues = UnpackArgs(valueArgsParameter, allValueTypes.Length);

                    var argIndex = 0;
                    for (var i = 0; i < argCount; i++)
                    {
                        if (invocationArguments[i] == null)
                        {
                            invocationArguments[i] = argsValues[argIndex];
                            argIndex++;
                        }
                    }

                    arguments.Add(CreateArgs(valueParameters));
                }

                //
                // Build the curried invocation expression, optionally adding a return statement using a
                // null reference in case the memoized function returns void.
                //
                curriedInvocation = Expression.Invoke(Expression.Constant(function), invocationArguments);

                if (hasNoReturn)
                {
                    curriedInvocation = Expression.Block(curriedInvocation, s_constantNull);
                }

                //
                // Emit a call to Memoize in case we have any value type parameters. Also track the index
                // of the last reference type parameter so we can start emitting .Delegate lookups for the
                // nested caches, as shown below:
                //
                //     o1 => o2 => (v1, v2) => f(o1, v1, o2, v2)
                //
                // becomes (omitting options and comparer parameters)
                //
                //                             o1 => o2 =>         values => f(o1, values.Item1, o2, values.Item2)
                //                             o1 => o2 => Memoize(values => f(o1, values.Item1, o2, values.Item2)).Delegate
                //
                // and (see below)
                //
                //                 o1 => MemoizeWeak(o2 => Memoize(values => f(o1, values.Item1, o2, values.Item2)).Delegate).Delegate
                //     MemoizeWeak(o1 => MemoizeWeak(o2 => Memoize(values => f(o1, values.Item1, o2, values.Item2)).Delegate).Delegate)
                //
                var hasCurried = false;
                var lastReferenceIndex = curriedParameters.Count - 1;

                if (valueArgsType != null)
                {
                    var curriedParameter = curriedParameters[lastReferenceIndex];

                    curriedInvocation = Expression.Call(
                        Expression.Constant(memoizer, typeof(IMemoizer)),
                        IMemoizer_Memoize.MakeGenericMethod(valueArgsType, curriedInvocation.Type),
                        Expression.Lambda(
                            curriedInvocation,
                            curriedParameter
                        ),
                        Expression.Constant(options),
                        Expression.Property(expression: null, typeof(FastEqualityComparer<>).MakeGenericType(valueArgsType).GetProperty(nameof(FastEqualityComparer<object>.Default)))
                    );

                    hasCurried = true;
                    lastReferenceIndex--;
                }

                //
                // Continue up the curried reference type parameter chain to create the nested caches:
                //
                //                 o1 => MemoizeWeak(o2 => Memoize(values => f(o1, values.Item1, o2, values.Item2)).Delegate).Delegate
                //     MemoizeWeak(o1 => MemoizeWeak(o2 => Memoize(values => f(o1, values.Item1, o2, values.Item2)).Delegate).Delegate)
                //
                for (var i = lastReferenceIndex; i >= 0; i--)
                {
                    if (hasCurried)
                    {
                        curriedInvocation = Expression.Property(curriedInvocation, "Delegate");
                    }

                    var curriedParameter = curriedParameters[i];

                    curriedInvocation = Expression.Call(
                        Expression.Constant(weakMemoizer, typeof(IWeakMemoizer)),
                        IWeakMemoizer_MemoizeWeak.MakeGenericMethod(curriedParameter.Type, curriedInvocation.Type),
                        Expression.Lambda(
                            curriedInvocation,
                            curriedParameter
                        ),
                        Expression.Constant(hasCurried ? MemoizationOptions.None : options)
                    );

                    arguments.Add(curriedParameter);

                    hasCurried = true;
                }

                //
                // At this point, we have the curried invocation, without any dereferencing of the
                // memoized delegate's Cache or Delegate properties.
                //
                //     MemoizeWeak(o1 => MemoizeWeak(o2 => Memoize(values => f(o1, values.Item1, o2, values.Item2)).Delegate).Delegate)
                //
            }

            var memoizedDelegate = Expression.Lambda(curriedInvocation).Compile().DynamicInvoke();

            //
            // Get the Cache and Delegate properties of the result. We'll deconstruct the
            // memoized delegate in order to create the final resulting memoized function
            // which packs arguments to the function using CreateArgs and creates a lambda
            // of type TDelegate. The Cache gets re-wrapped in the resulting instance of
            // MemoizedDelegate<TDelegate>.
            //
            var delegateProperty = curriedInvocation.Type.GetProperty(nameof(MemoizedDelegate<object>.Delegate));
            var @delegate = delegateProperty.GetValue(memoizedDelegate, index: null);

            var cacheProperty = curriedInvocation.Type.GetProperty(nameof(MemoizedDelegate<object>.Cache));
            var cache = (IMemoizationCache)cacheProperty.GetValue(memoizedDelegate, index: null);

            //
            // Emit the code to invoke the curried memoized delegate, for example:
            //
            //     (o1, v1, o2, v2) => memoized.Delegate(o1)(o2)(new Tuplet<V1, V2>(v1, v2))
            //
            var application = (Expression)Expression.Constant(@delegate);

            for (var i = arguments.Count - 1; i >= 0; i--)
            {
                application = Expression.Invoke(application, arguments[i]);
            }

            var lambda = Expression.Lambda<TDelegate>(application, parameters);

            return new MemoizedDelegate<TDelegate>(lambda.Compile(), cache);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="function"/> using the specified <paramref name="memoizer"/>.
        /// </summary>
        /// <typeparam name="T">Type of the function argument.</typeparam>
        /// <typeparam name="TResult">Type of the function result.</typeparam>
        /// <param name="memoizer">The memoizer used to memoize the function.</param>
        /// <param name="function">The function to memoize.</param>
        /// <param name="options">Flags to influence the memoization behavior.</param>
        /// <returns>A memoized delegate containing the memoized function and providing access to the memoization cache.</returns>
        public static IMemoizedDelegate<Func<T, TResult>> MemoizeWeak<T, TResult>(this IWeakMemoizer memoizer, Func<T, TResult> function, MemoizationOptions options = MemoizationOptions.None)
            where T : class
        {
            //
            // NB: This method is used by s_memoizeWeakFuncMethods as a convenience to bind MemoizeWeak<TDelegate> calls.
            //

            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return memoizer.MemoizeWeak<T, TResult>(function, options);
        }

        /// <summary>
        /// Asserts that the specified type is not a by-ref or pointer type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="name">The name of the parameter to use for error reporting if the type doesn't match the assert.</param>
        /// <exception cref="System.NotSupportedException">Thrown if the specified type does not match the assert.</exception>
        private static void AssertNoRefOrPointer(Type type, string name)
        {
            if (type.IsByRef || type.IsPointer)
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The '{0}' parameter has type '{1}' which cannot be memoized.", name, type));
        }

        /// <summary>
        /// Finds the pre-computed memoize methods.
        /// </summary>
        /// <param name="name">Name of the memoize method family to find.</param>
        /// <param name="firstParameterType">Type of the first parameter to the memoize method.</param>
        /// <returns>List of memoize methods. The method at position n can memoize a function with n parameters.</returns>
        private static MethodInfo[] FindMemoizeMethods(string name, Type firstParameterType)
        {
            //
            // NB: This runs only once, so we won't bother optimizing the LINQ expression below.
            //
            var res = from m in typeof(FunctionMemoizationExtensions).GetMethods()
                      where m.IsGenericMethodDefinition && m.Name == name
                      let p = m.GetParameters()
                      where p.Length == 3
                      let g = m.GetGenericArguments()
                      where p[0].ParameterType == firstParameterType
                      where p[1].ParameterType == s_funcTypes[g.Length - 1].MakeGenericType(g)
                      where p[2].ParameterType == typeof(MemoizationOptions)
                      orderby g.Length
                      select m;

            return res.ToArray();
        }

        /// <summary>
        /// Gets a tuplet type to bundle the specified argument types.
        /// </summary>
        /// <param name="types">The argument types to bundle in a tuplet.</param>
        /// <returns>Tuplet type for a bundle holding arguments of the specified types; this type can contain nested tuplets types.</returns>
        private static Type GetArgsType(Type[] types)
        {
            //
            // With N = 3
            //
            //    n  decomposed           iterations (f,l,c)
            //   --  -------------------  -------------------------
            //    1  (a)                  (0,0,1)
            //    2  (a,b)                (0,1,2)
            //    3  (a,b,c)              (0,2,3)
            //    4  (a,b,c,(d))          (3,3,1)->(0,2,3)
            //    5  (a,b,c,(d,e))        (3,4,2)->(0,2,3)
            //    6  (a,b,c,(d,e,f))      (3,5,3)->(0,2,3)
            //    7  (a,b,c,(d,e,f,(g)))  (6,6,1)->(3,5,3)->(0,2,3)

            var N = s_argsTypes.Length;
            var n = types.Length;

            var firstIndex = (n - 1) / N * N;
            var lastIndex = n - 1;

            var type = default(Type);

            while (firstIndex >= 0)
            {
                var count = lastIndex - firstIndex + 1;

                var argTypes = new Type[count + (type != null ? 1 : 0)];

                for (var i = 0; i < count; i++)
                {
                    argTypes[i] = types[firstIndex + i];
                }

                if (type != null)
                {
                    argTypes[count] = type;
                    type = s_argsRestType.MakeGenericType(argTypes);
                }
                else
                {
                    type = s_argsTypes[count - 1].MakeGenericType(argTypes);
                }

                lastIndex = firstIndex - 1;
                firstIndex -= N;
            }

            return type;
        }

        /// <summary>
        /// Creates an expression to wrap the specified arguments in a bundle using a tuplet.
        /// </summary>
        /// <param name="arguments">The argument expressions to wrap in a bundle using a tuplet.</param>
        /// <returns>An expression to create an argument bundle using a tuplet, containing the specified argument expressions.</returns>
        private static Expression CreateArgs(IList<Expression> arguments)
        {
            var N = s_argsTypes.Length;
            var n = arguments.Count;

            var firstIndex = (n - 1) / N * N;
            var lastIndex = n - 1;

            var expression = default(Expression);
            var type = default(Type);

            while (firstIndex >= 0)
            {
                var count = lastIndex - firstIndex + 1;

                var argExpressions = new Expression[count + (expression != null ? 1 : 0)];
                var argTypes = new Type[count + (type != null ? 1 : 0)];

                for (var i = 0; i < count; i++)
                {
                    argExpressions[i] = arguments[firstIndex + i];
                    argTypes[i] = argExpressions[i].Type;
                }

                if (expression != null)
                {
                    argExpressions[count] = expression;
                    argTypes[count] = type;

                    type = s_argsRestType.MakeGenericType(argTypes);
                    expression = Expression.New(type.GetConstructors().Single(), argExpressions);
                }
                else
                {
                    type = s_argsTypes[count - 1].MakeGenericType(argTypes);
                    expression = Expression.New(type.GetConstructors().Single(), argExpressions);
                }

                lastIndex = firstIndex - 1;
                firstIndex -= N;
            }

            return expression;
        }

        /// <summary>
        /// Gets an array of expressions that can be used to unpack the arguments of an argument bundle contained in a tuplet.
        /// </summary>
        /// <param name="args">The expression containing the tuplet with the argument bundle.</param>
        /// <param name="count">The number of arguments held in the tuplet.</param>
        /// <returns>An array of expressions to access arguments [0, count-1] from the specified expression.</returns>
        private static Expression[] UnpackArgs(Expression args, int count)
        {
            var res = new Expression[count];

            var N = s_argsTypes.Length;
            var ordinal = 1;

            for (var i = 0; i < count; i++)
            {
                if (ordinal == N + 1)
                {
                    ordinal = 1;
                    args = Expression.Property(args, "Rest");
                }

                res[i] = Expression.Property(args, "Item" + ordinal);

                ordinal++;
            }

            return res;
        }
    }
}
