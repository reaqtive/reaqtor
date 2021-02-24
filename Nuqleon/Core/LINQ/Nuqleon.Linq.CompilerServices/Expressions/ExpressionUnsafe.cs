// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - November 2014 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Not checking arguments for null by design; unsafe factories bypass argument validation in general.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Reflection;
using System.Runtime.CompilerServices;

#if !NETSTANDARD2_0
using System.Reflection.Emit;
#endif

namespace System.Linq.Expressions
{
    //
    // NB: Implementing a portable and robust version of this functionality is a bit tricky due to differences in the internals of the expression API
    //     that were introduced starting with .NET Core. We proceed as follows:
    //
    //      - For some expression types, have different known shortcuts and select the one we find (if any).
    //      - Graceful fallback to "safe" factories if a shortcut is not found.
    //
    //     In essence, we try our best to find a (known) shortcut, and fall back to the public factory method if none works.
    //
    // REVIEW: We should review some of this strategy going forward. In particular:
    //
    //          - How do we find out when fallback happens silently so we can optimize by adding other cases? Some special build flavor that asserts?
    //          - Should we move this functionality to a platform-specific assembly? Still has issues of breaking changes that may affect us.
    //

    /// <summary>
    /// Provides factory methods for expressions that bypass argument validation. These factory methods should be used
    /// with extreme caution, only if type safety guarantees are provided elsewhere. A typical use of these factory
    /// methods is for expression deserialization.
    /// </summary>
    public sealed class ExpressionUnsafe : Expression
    {
        #region Constructors

        /// <summary>
        /// Private default constructor to prevent instantiation. This type is meant to be used for its factory methods only.
        /// </summary>
        private ExpressionUnsafe()
        {
        }

        #endregion

        #region Fields

        /// <summary>
        /// Singleton instance of DefaultExpression for typeof(void).
        /// </summary>
        private static readonly Expression s_void = Expression.Empty();

        /// <summary>
        /// Constructor arguments for <see cref="Expression{TDelegate}"/>.
        /// </summary>
        private static readonly Type[] s_expressionCtorArgs = new[] { typeof(Expression), typeof(string), typeof(bool), typeof(ReadOnlyCollection<ParameterExpression>) };

        /// <summary>
        /// Cache of compiled delegates for <see cref="Expression{TDelegate}"/> constructors.
        /// </summary>
        private static readonly ConditionalWeakTable<Type, Func<Expression, string, bool, ReadOnlyCollection<ParameterExpression>, LambdaExpression>> s_lambdaCtors = new();

        #region Delegates

        #region Binary

        /// <summary>
        /// Constructor for SimpleBinaryExpression.
        /// </summary>
        private static readonly Lazy<Func<ExpressionType, Expression, Expression, Type, BinaryExpression>> s_SimpleBinaryExpressionCtor = new(() => CreateCtorDelegate<Func<ExpressionType, Expression, Expression, Type, BinaryExpression>>("System.Linq.Expressions.SimpleBinaryExpression", new Type[] { typeof(ExpressionType), typeof(Expression), typeof(Expression), typeof(Type) }, null));

        #endregion

        #region Call

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with 1 argument.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, MethodCallExpression>> s_MethodCallExpression1Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpression1", new Type[] { typeof(MethodInfo), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with 2 arguments.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, Expression, MethodCallExpression>> s_MethodCallExpression2Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, Expression, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpression2", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with 3 arguments.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, Expression, Expression, MethodCallExpression>> s_MethodCallExpression3Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, Expression, Expression, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpression3", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(Expression), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with 4 arguments.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, Expression, Expression, Expression, MethodCallExpression>> s_MethodCallExpression4Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, Expression, Expression, Expression, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpression4", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(Expression), typeof(Expression), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with 5 arguments.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, Expression, Expression, Expression, Expression, MethodCallExpression>> s_MethodCallExpression5Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, Expression, Expression, Expression, Expression, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpression5", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(Expression), typeof(Expression), typeof(Expression), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with N arguments (option 1, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, IList<Expression>, MethodCallExpression>> s_MethodCallExpressionNCtor1 = new(() => CreateCtorDelegate<Func<MethodInfo, IList<Expression>, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpressionN", new Type[] { typeof(MethodInfo), typeof(IList<Expression>) }, null));

        /// <summary>
        /// Constructor for MethodCallExpression targeting a static method with N arguments (option 2, with fallback to safe factory).
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, IReadOnlyList<Expression>, MethodCallExpression>> s_MethodCallExpressionNCtor2 = new(() => CreateCtorDelegate<Func<MethodInfo, IReadOnlyList<Expression>, MethodCallExpression>>("System.Linq.Expressions.MethodCallExpressionN", new Type[] { typeof(MethodInfo), typeof(IReadOnlyList<Expression>) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting an instance method with 2 arguments.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, Expression, Expression, MethodCallExpression>> s_InstanceMethodCallExpression2Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, Expression, Expression, MethodCallExpression>>("System.Linq.Expressions.InstanceMethodCallExpression2", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(Expression), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting an instance method with 3 arguments.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, Expression, Expression, Expression, MethodCallExpression>> s_InstanceMethodCallExpression3Ctor = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, Expression, Expression, Expression, MethodCallExpression>>("System.Linq.Expressions.InstanceMethodCallExpression3", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(Expression), typeof(Expression), typeof(Expression) }, Expression.Call));

        /// <summary>
        /// Constructor for MethodCallExpression targeting an instance method with N arguments (option 1, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, IList<Expression>, MethodCallExpression>> s_InstanceMethodCallExpressionNCtor1 = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, IList<Expression>, MethodCallExpression>>("System.Linq.Expressions.InstanceMethodCallExpressionN", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(IList<Expression>) }, null));

        /// <summary>
        /// Constructor for MethodCallExpression targeting an instance method with N argument (option 2, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, Expression, IReadOnlyList<Expression>, MethodCallExpression>> s_InstanceMethodCallExpressionNCtor2 = new(() => CreateCtorDelegate<Func<MethodInfo, Expression, IReadOnlyList<Expression>, MethodCallExpression>>("System.Linq.Expressions.InstanceMethodCallExpressionN", new Type[] { typeof(MethodInfo), typeof(Expression), typeof(IReadOnlyList<Expression>) }, null));

        #endregion

        #region Conditional

        /// <summary>
        /// Make factory for ConditionalExpression.
        /// </summary>
        private static readonly Lazy<Func<Expression, Expression, Expression, Type, ConditionalExpression>> s_ConditionalExpressionMake = new(() => CreateMakeDelegate<Func<Expression, Expression, Expression, Type, ConditionalExpression>>(typeof(ConditionalExpression), new Type[] { typeof(Expression), typeof(Expression), typeof(Expression), typeof(Type) }, Expression.Condition));

        #endregion

        #region ElementInit

        /// <summary>
        /// Constructor for ElementInit.
        /// </summary>
        private static readonly Lazy<Func<MethodInfo, ReadOnlyCollection<Expression>, ElementInit>> s_ElementInitCtor = new(() => CreateCtorDelegate<Func<MethodInfo, ReadOnlyCollection<Expression>, ElementInit>>(typeof(ElementInit), new Type[] { typeof(MethodInfo), typeof(ReadOnlyCollection<Expression>) }, Expression.ElementInit));

        #endregion

        #region Index

        /// <summary>
        /// Constructor for IndexExpression (option 1, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<Expression, PropertyInfo, IList<Expression>, IndexExpression>> s_IndexExpressionCtor1 = new(() => CreateCtorDelegate<Func<Expression, PropertyInfo, IList<Expression>, IndexExpression>>(typeof(IndexExpression), new Type[] { typeof(Expression), typeof(PropertyInfo), typeof(IList<Expression>) }, null));

        /// <summary>
        /// Constructor for IndexExpression (option 2, with fallback to <c>Expression.MakeIndex</c>).
        /// </summary>
        private static readonly Lazy<Func<Expression, PropertyInfo, IReadOnlyList<Expression>, IndexExpression>> s_IndexExpressionCtor2 = new(() => CreateCtorDelegate<Func<Expression, PropertyInfo, IReadOnlyList<Expression>, IndexExpression>>(typeof(IndexExpression), new Type[] { typeof(Expression), typeof(PropertyInfo), typeof(IReadOnlyList<Expression>) }, Expression.MakeIndex));

        #endregion

        #region Invoke

        /// <summary>
        /// Constructor for InvokeExpression (option 1, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<Expression, IList<Expression>, Type, InvocationExpression>> s_InvocationExpressionCtor1 = new(() => CreateCtorDelegate<Func<Expression, IList<Expression>, Type, InvocationExpression>>(typeof(InvocationExpression), new Type[] { typeof(Expression), typeof(IList<Expression>), typeof(Type) }, null));

        /// <summary>
        /// Constructor for InvokeExpression (option 2, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<Expression, IReadOnlyList<Expression>, Type, InvocationExpression>> s_InvocationExpressionCtor2 = new(() => CreateCtorDelegate<Func<Expression, IReadOnlyList<Expression>, Type, InvocationExpression>>("System.Linq.Expressions.InvocationExpressionN", new Type[] { typeof(Expression), typeof(IReadOnlyList<Expression>), typeof(Type) }, null));

        #endregion

        #region Lambda

        /// <summary>
        /// CreateLambda method to create non-generic lambda expression nodes.
        /// </summary>
        private static readonly Lazy<Func<Type, Expression, string, bool, ReadOnlyCollection<ParameterExpression>, LambdaExpression>> s_CreateLambda = new(() => CreateDelegate<Func<Type, Expression, string, bool, ReadOnlyCollection<ParameterExpression>, LambdaExpression>>(typeof(Expression), "CreateLambda", new[] { typeof(Type), typeof(Expression), typeof(string), typeof(bool), typeof(ReadOnlyCollection<ParameterExpression>) }, Expression.Lambda));

        #endregion

        #region ListInit

        /// <summary>
        /// Constructor for ListInitExpression.
        /// </summary>
        private static readonly Lazy<Func<NewExpression, ReadOnlyCollection<ElementInit>, ListInitExpression>> s_ListInitExpressionCtor = new(() => CreateCtorDelegate<Func<NewExpression, ReadOnlyCollection<ElementInit>, ListInitExpression>>(typeof(ListInitExpression), new Type[] { typeof(NewExpression), typeof(ReadOnlyCollection<ElementInit>) }, Expression.ListInit));

        #endregion

        #region MemberAssignment

        /// <summary>
        /// Constructor for MemberAssignment.
        /// </summary>
        private static readonly Lazy<Func<MemberInfo, Expression, MemberAssignment>> s_MemberAssignmentCtor = new(() => CreateCtorDelegate<Func<MemberInfo, Expression, MemberAssignment>>(typeof(MemberAssignment), new Type[] { typeof(MemberInfo), typeof(Expression) }, Expression.Bind));

        #endregion

        #region MemberListBinding

        /// <summary>
        /// Constructor for MemberListBinding.
        /// </summary>
        private static readonly Lazy<Func<MemberInfo, ReadOnlyCollection<ElementInit>, MemberListBinding>> s_MemberListBindingCtor = new(() => CreateCtorDelegate<Func<MemberInfo, ReadOnlyCollection<ElementInit>, MemberListBinding>>(typeof(MemberListBinding), new Type[] { typeof(MemberInfo), typeof(ReadOnlyCollection<ElementInit>) }, Expression.ListBind));

        #endregion

        #region MemberMemberBinding

        /// <summary>
        /// Constructor for MemberMemberBinding.
        /// </summary>
        private static readonly Lazy<Func<MemberInfo, ReadOnlyCollection<MemberBinding>, MemberMemberBinding>> s_MemberMemberBindingCtor = new(() => CreateCtorDelegate<Func<MemberInfo, ReadOnlyCollection<MemberBinding>, MemberMemberBinding>>(typeof(MemberMemberBinding), new Type[] { typeof(MemberInfo), typeof(ReadOnlyCollection<MemberBinding>) }, Expression.MemberBind));

        #endregion

        #region MemberInit

        /// <summary>
        /// Constructor for MemberInitExpression.
        /// </summary>
        private static readonly Lazy<Func<NewExpression, ReadOnlyCollection<MemberBinding>, MemberInitExpression>> s_MemberInitExpressionCtor = new(() => CreateCtorDelegate<Func<NewExpression, ReadOnlyCollection<MemberBinding>, MemberInitExpression>>(typeof(MemberInitExpression), new Type[] { typeof(NewExpression), typeof(ReadOnlyCollection<MemberBinding>) }, Expression.MemberInit));

        #endregion

        #region New

        /// <summary>
        /// Constructor for NewExpression (option 1, with fallback to <c>null</c>).
        /// </summary>
        private static readonly Lazy<Func<ConstructorInfo, IList<Expression>, ReadOnlyCollection<MemberInfo>, NewExpression>> s_NewExpressionCtor1 = new(() => CreateCtorDelegate<Func<ConstructorInfo, IList<Expression>, ReadOnlyCollection<MemberInfo>, NewExpression>>(typeof(NewExpression), new Type[] { typeof(ConstructorInfo), typeof(IList<Expression>), typeof(ReadOnlyCollection<MemberInfo>) }, null));

        /// <summary>
        /// Constructor for NewExpression (option 1, with fallback to <c>Expression.New</c>).
        /// </summary>
        private static readonly Lazy<Func<ConstructorInfo, IReadOnlyList<Expression>, ReadOnlyCollection<MemberInfo>, NewExpression>> s_NewExpressionCtor2 = new(() => CreateCtorDelegate<Func<ConstructorInfo, IReadOnlyList<Expression>, ReadOnlyCollection<MemberInfo>, NewExpression>>(typeof(NewExpression), new Type[] { typeof(ConstructorInfo), typeof(IReadOnlyList<Expression>), typeof(ReadOnlyCollection<MemberInfo>) }, Expression.New));

        #endregion

        #region NewArray

        /// <summary>
        /// Make factory for NewArrayExpression.
        /// </summary>
        private static readonly Lazy<Func<ExpressionType, Type, ReadOnlyCollection<Expression>, NewArrayExpression>> s_NewArrayExpressionMake = new(() => CreateMakeDelegate<Func<ExpressionType, Type, ReadOnlyCollection<Expression>, NewArrayExpression>>(typeof(NewArrayExpression), new Type[] { typeof(ExpressionType), typeof(Type), typeof(ReadOnlyCollection<Expression>) }, MakeNewArrayExpression));

        private static NewArrayExpression MakeNewArrayExpression(ExpressionType kind, Type type, ReadOnlyCollection<Expression> expressions)
        {
            if (kind == ExpressionType.NewArrayBounds)
            {
                return Expression.NewArrayBounds(type, expressions);
            }
            else
            {
                return Expression.NewArrayInit(type, expressions);
            }
        }

        #endregion

        #region Unary

        /// <summary>
        /// Constructor for UnaryExpression.
        /// </summary>
        private static readonly Lazy<Func<ExpressionType, Expression, Type, MethodInfo, UnaryExpression>> s_UnaryExpressionCtor = new(() => CreateCtorDelegate<Func<ExpressionType, Expression, Type, MethodInfo, UnaryExpression>>(typeof(UnaryExpression), new Type[] { typeof(ExpressionType), typeof(Expression), typeof(Type), typeof(MethodInfo) }, Expression.MakeUnary));

        #endregion

        #region Helpers

        /// <summary>
        /// Finds the Invoke method for the delegate invocation of the specified expression.
        /// </summary>
        /// <param name="expression">The expression to find a delegate invocation method for.</param>
        /// <returns>The Invoke method that can be used to invoke the expression through its delegate type.</returns>
        /// <exception cref="ArgumentException">The type of the expression is not a delegate type.</exception>
        private static MethodInfo GetInvokeMethod(Expression expression)
        {
            var delegateType = expression.Type;

            if (!delegateType.IsSubclassOf(typeof(MulticastDelegate)))
            {
                var type = delegateType.FindGenericType(typeof(Expression<>));

                if (type == null)
                {
                    throw new ArgumentException("The expression's type is not invocable.");
                }

                delegateType = type.GetGenericArguments()[0];
            }

            return delegateType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Finds the PropertyInfo that uses the specified MethodInfo as an accessor.
        /// </summary>
        /// <param name="method">The method to find a corresponding property for.</param>
        /// <returns>The property for which the specified method is an accessor.</returns>
        /// <exception cref="ArgumentException">The specified method is not a property accessor.</exception>
        private static PropertyInfo GetProperty(MethodInfo method)
        {
            var declaringType = method.DeclaringType;

            if (declaringType != null)
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
                bindingFlags |= method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;

                var properties = declaringType.GetProperties(bindingFlags);

                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];

                    if (property.CanRead && CheckMethod(property.GetGetMethod(nonPublic: true)))
                    {
                        return property;
                    }

                    if (property.CanWrite && CheckMethod(property.GetSetMethod(nonPublic: true)))
                    {
                        return property;
                    }
                }
            }

            throw new ArgumentException(FormattableString.Invariant($"The specified method {method.Name} on type {declaringType} is not a property accessor."));

            bool CheckMethod(MethodInfo propertyMethod)
            {
                if (method.Equals(propertyMethod))
                {
                    return true;
                }

                return declaringType.IsInterface && method.Name == propertyMethod.Name && declaringType.GetMethod(method.Name) == propertyMethod;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Helpers

        /// <summary>
        /// Creates a delegate to create an instance of the type specified in <paramref name="type"/> using the constructor with the specified <paramref name="parameterTypes"/>.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the delegate to create.</typeparam>
        /// <param name="type">Type for which to create a constructor delegate.</param>
        /// <param name="parameterTypes">Types of the constructor parameters.</param>
        /// <param name="fallback">Fallback to use when no constructor is found.</param>
        /// <returns>Delegate that can be used to create an instance of the specified type, using the specified parameter types.</returns>
        private static TDelegate CreateCtorDelegate<TDelegate>(Type type, Type[] parameterTypes, TDelegate fallback)
            where TDelegate : class
        {
            var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);

            if (ctor == null)
                return fallback;

#if NETSTANDARD2_0
            var parameters = parameterTypes.Select(pt => Expression.Parameter(pt)).ToArray();
            return Expression.Lambda<TDelegate>(Expression.New(ctor, parameters), parameters).Compile();
#else
            var mtd = new DynamicMethod(".ctor" + type.Name + "@" + parameterTypes.Length, type, parameterTypes, true);

            var il = mtd.GetILGenerator();

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i);
            }

            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            return (TDelegate)(object)mtd.CreateDelegate(typeof(TDelegate));
#endif
        }

        /// <summary>
        /// Creates a delegate to create an instance of the type specified in <paramref name="typeName"/> using the constructor with the specified <paramref name="parameterTypes"/>.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the delegate to create.</typeparam>
        /// <param name="typeName">Name of the type for which to create a constructor delegate.</param>
        /// <param name="parameterTypes">Types of the constructor parameters.</param>
        /// <param name="fallback">Fallback to use when no constructor is found.</param>
        /// <returns>Delegate that can be used to create an instance of the specified type, using the specified parameter types.</returns>
        private static TDelegate CreateCtorDelegate<TDelegate>(string typeName, Type[] parameterTypes, TDelegate fallback)
            where TDelegate : class
        {
            var type = typeof(Expression).Assembly.GetType(typeName);

            return CreateCtorDelegate(type, parameterTypes, fallback);
        }

        /// <summary>
        /// Creates a delegate to create an instance of the type specified in <paramref name="type"/> using a static Make factory method with the specified <paramref name="parameterTypes"/>.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the delegate to create.</typeparam>
        /// <param name="type">Type for which to create a factory method delegate.</param>
        /// <param name="parameterTypes">Types of the factory method parameters.</param>
        /// <param name="fallback">Fallback to use when no factory method is found.</param>
        /// <returns>Delegate that can be used to create an instance of the specified type, using the specified parameter types.</returns>
        private static TDelegate CreateMakeDelegate<TDelegate>(Type type, Type[] parameterTypes, TDelegate fallback)
            where TDelegate : class
        {
            return CreateDelegate(type, "Make", parameterTypes, fallback);
        }

        /// <summary>
        /// Creates a delegate for the method with the specified <paramref name="name"/> on the specified <paramref name="type"/> and with the specified <paramref name="parameterTypes"/>.
        /// </summary>
        /// <typeparam name="TDelegate">Type of the delegate to create.</typeparam>
        /// <param name="type">Type containing the method for which to create a delegate.</param>
        /// <param name="name">Name of the method for which to create a delegate.</param>
        /// <param name="parameterTypes">Parameter types of the method for which to create a delegate.</param>
        /// <param name="fallback">Fallback to use when no factory method is found.</param>
        /// <returns>Delegate that can be used to invoke the method specified by the parameters.</returns>
        private static TDelegate CreateDelegate<TDelegate>(Type type, string name, Type[] parameterTypes, TDelegate fallback)
           where TDelegate : class
        {
            var method = type.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, parameterTypes, null);

            if (method == null)
                return fallback;

            return (TDelegate)(object)method.CreateDelegate(typeof(TDelegate));
        }

        #endregion

        #region Binary factories

        #region Array index

        /// <summary>
        /// Creates a BinaryExpression that represents applying an array index operator to an array of rank one.
        /// </summary>
        /// <param name="array">An Expression to set the Left property equal to.</param>
        /// <param name="index">An Expression to set the Right property equal to.</param>
        /// <returns>A BinaryExpression that has the NodeType property equal to ArrayIndex and the Left and Right properties set to the specified values.</returns>
        public static new BinaryExpression ArrayIndex(Expression array, Expression index)
        {
            return s_SimpleBinaryExpressionCtor.Value?.Invoke(ExpressionType.ArrayIndex, array, index, array.Type.GetElementType()) ?? Expression.ArrayIndex(array, index);
        }

        #endregion

        #region Make factories

        /// <summary>
        /// Creates a <see cref="BinaryExpression"/>, given the left and right operands, by calling an appropriate factory method.
        /// </summary>
        /// <param name="binaryType">The <see cref="ExpressionType"/> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="Expression"/> that represents the left operand.</param>
        /// <param name="right">An <see cref="Expression"/> that represents the right operand.</param>
        /// <returns>The <see cref="BinaryExpression"/> that results from calling the appropriate factory method.</returns>
        public static new BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right)
        {
            return MakeBinary(binaryType, left, right, liftToNull: false, method: null, conversion: null);
        }

        /// <summary>
        /// Creates a <see cref="BinaryExpression"/>, given the left and right operands, by calling an appropriate factory method.
        /// </summary>
        /// <param name="binaryType">The <see cref="ExpressionType"/> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="Expression"/> that represents the left operand.</param>
        /// <param name="right">An <see cref="Expression"/> that represents the right operand.</param>
        /// <param name="liftToNull">true to set IsLiftedToNull to true; false to set IsLiftedToNull to false.</param>
        /// <param name="method">A <see cref="MethodInfo"/> that specifies the implementing method.</param>
        /// <returns>The <see cref="BinaryExpression"/> that results from calling the appropriate factory method.</returns>
        public static new BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method)
        {
            return MakeBinary(binaryType, left, right, liftToNull, method, conversion: null);
        }

        /// <summary>
        /// Creates a <see cref="BinaryExpression"/>, given the left and right operands, by calling an appropriate factory method.
        /// </summary>
        /// <param name="binaryType">The <see cref="ExpressionType"/> that specifies the type of binary operation.</param>
        /// <param name="left">An <see cref="Expression"/> that represents the left operand.</param>
        /// <param name="right">An <see cref="Expression"/> that represents the right operand.</param>
        /// <param name="liftToNull">true to set IsLiftedToNull to true; false to set IsLiftedToNull to false.</param>
        /// <param name="method">A <see cref="MethodInfo"/> that specifies the implementing method.</param>
        /// <param name="conversion">A <see cref="LambdaExpression"/> that represents a type conversion function. This parameter is used if binaryType is Coalesce or compound assignment.</param>
        /// <returns>The <see cref="BinaryExpression"/> that results from calling the appropriate factory method.</returns>
        public static new BinaryExpression MakeBinary(ExpressionType binaryType, Expression left, Expression right, bool liftToNull, MethodInfo method, LambdaExpression conversion)
        {
            return binaryType switch
            {
                ExpressionType.Add => Add(left, right, method),
                ExpressionType.AddChecked => AddChecked(left, right, method),
                ExpressionType.And => And(left, right, method),
                ExpressionType.AndAlso => AndAlso(left, right, method),
                ExpressionType.ArrayIndex => ArrayIndex(left, right),
                ExpressionType.Coalesce => Coalesce(left, right, conversion),
                ExpressionType.Divide => Divide(left, right, method),
                ExpressionType.Equal => Equal(left, right, liftToNull, method),
                ExpressionType.ExclusiveOr => ExclusiveOr(left, right, method),
                ExpressionType.GreaterThan => GreaterThan(left, right, liftToNull, method),
                ExpressionType.GreaterThanOrEqual => GreaterThanOrEqual(left, right, liftToNull, method),
                ExpressionType.LeftShift => LeftShift(left, right, method),
                ExpressionType.LessThan => LessThan(left, right, liftToNull, method),
                ExpressionType.LessThanOrEqual => LessThanOrEqual(left, right, liftToNull, method),
                ExpressionType.Modulo => Modulo(left, right, method),
                ExpressionType.Multiply => Multiply(left, right, method),
                ExpressionType.MultiplyChecked => MultiplyChecked(left, right, method),
                ExpressionType.NotEqual => NotEqual(left, right, liftToNull, method),
                ExpressionType.Or => Or(left, right, method),
                ExpressionType.OrElse => OrElse(left, right, method),
                ExpressionType.Power => Power(left, right, method),
                ExpressionType.RightShift => RightShift(left, right, method),
                ExpressionType.Subtract => Subtract(left, right, method),
                ExpressionType.SubtractChecked => SubtractChecked(left, right, method),
                ExpressionType.Assign => Assign(left, right),
                ExpressionType.AddAssign => AddAssign(left, right, method, conversion),
                ExpressionType.AndAssign => AndAssign(left, right, method, conversion),
                ExpressionType.DivideAssign => DivideAssign(left, right, method, conversion),
                ExpressionType.ExclusiveOrAssign => ExclusiveOrAssign(left, right, method, conversion),
                ExpressionType.LeftShiftAssign => LeftShiftAssign(left, right, method, conversion),
                ExpressionType.ModuloAssign => ModuloAssign(left, right, method, conversion),
                ExpressionType.MultiplyAssign => MultiplyAssign(left, right, method, conversion),
                ExpressionType.OrAssign => OrAssign(left, right, method, conversion),
                ExpressionType.PowerAssign => PowerAssign(left, right, method, conversion),
                ExpressionType.RightShiftAssign => RightShiftAssign(left, right, method, conversion),
                ExpressionType.SubtractAssign => SubtractAssign(left, right, method, conversion),
                ExpressionType.AddAssignChecked => AddAssignChecked(left, right, method, conversion),
                ExpressionType.MultiplyAssignChecked => MultiplyAssignChecked(left, right, method, conversion),
                ExpressionType.SubtractAssignChecked => SubtractAssignChecked(left, right, method, conversion),
                _ => throw new ArgumentException("Unhandled binary: " + binaryType, nameof(binaryType)),
            };
        }

        #endregion

        #endregion

        #region Bind factories

        /// <summary>Creates a <see cref="MemberAssignment" /> that represents the initialization of a field or property.</summary>
        /// <returns>A <see cref="MemberAssignment" /> that has <see cref="MemberBinding.BindingType" /> equal to <see cref="MemberBindingType.Assignment" /> and the <see cref="MemberBinding.Member" /> and <see cref="MemberAssignment.Expression" /> properties set to the specified values.</returns>
        /// <param name="member">A <see cref="MemberInfo" /> to set the <see cref="MemberBinding.Member" /> property equal to.</param>
        /// <param name="expression">An <see cref="Expression" /> to set the <see cref="MemberAssignment.Expression" /> property equal to.</param>
        public static new MemberAssignment Bind(MemberInfo member, Expression expression)
        {
            return s_MemberAssignmentCtor.Value(member, expression);
        }

        /// <summary>Creates a <see cref="MemberAssignment" /> that represents the initialization of a member by using a property accessor method.</summary>
        /// <returns>A <see cref="MemberAssignment" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.Assignment" />, the <see cref="MemberBinding.Member" /> property set to the <see cref="PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and the <see cref="MemberAssignment.Expression" /> property set to <paramref name="expression" />.</returns>
        /// <param name="propertyAccessor">A <see cref="MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="expression">An <see cref="Expression" /> to set the <see cref="MemberAssignment.Expression" /> property equal to.</param>
        public static new MemberAssignment Bind(MethodInfo propertyAccessor, Expression expression)
        {
            return s_MemberAssignmentCtor.Value(GetProperty(propertyAccessor), expression);
        }

        /// <summary>Creates a <see cref="MemberListBinding" /> where the member is a field or property.</summary>
        /// <returns>A <see cref="MemberListBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.ListBinding" /> and the <see cref="MemberBinding.Member" /> and <see cref="MemberListBinding.Initializers" /> properties set to the specified values.</returns>
        /// <param name="member">A <see cref="MemberInfo" /> that represents a field or property to set the <see cref="MemberBinding.Member" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="MemberListBinding.Initializers" /> collection.</param>
        public static new MemberListBinding ListBind(MemberInfo member, ElementInit[] initializers)
        {
            return s_MemberListBindingCtor.Value(member, initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberListBinding" /> where the member is a field or property.</summary>
        /// <returns>A <see cref="MemberListBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.ListBinding" /> and the <see cref="MemberBinding.Member" /> and <see cref="MemberListBinding.Initializers" /> properties set to the specified values.</returns>
        /// <param name="member">A <see cref="MemberInfo" /> that represents a field or property to set the <see cref="MemberBinding.Member" /> property equal to.</param>
        /// <param name="initializers">An <see cref="IEnumerable{T}" /> that contains <see cref="System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="MemberListBinding.Initializers" /> collection.</param>
        public static new MemberListBinding ListBind(MemberInfo member, IEnumerable<ElementInit> initializers)
        {
            return s_MemberListBindingCtor.Value(member, initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberListBinding" /> object based on a specified property accessor method.</summary>
        /// <returns>A <see cref="MemberListBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.ListBinding" />, the <see cref="MemberBinding.Member" /> property set to the <see cref="MemberInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="MemberListBinding.Initializers" /> populated with the elements of <paramref name="initializers" />.</returns>
        /// <param name="propertyAccessor">A <see cref="MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="initializers">An array of <see cref="System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="MemberListBinding.Initializers" /> collection.</param>
        public static new MemberListBinding ListBind(MethodInfo propertyAccessor, ElementInit[] initializers)
        {
            return s_MemberListBindingCtor.Value(GetProperty(propertyAccessor), initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberListBinding" /> based on a specified property accessor method.</summary>
        /// <returns>A <see cref="MemberListBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.ListBinding" />, the <see cref="MemberBinding.Member" /> property set to the <see cref="MemberInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="MemberListBinding.Initializers" /> populated with the elements of <paramref name="initializers" />.</returns>
        /// <param name="propertyAccessor">A <see cref="MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="initializers">An <see cref="IEnumerable{T}" /> that contains <see cref="System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="MemberListBinding.Initializers" /> collection.</param>
        public static new MemberListBinding ListBind(MethodInfo propertyAccessor, IEnumerable<ElementInit> initializers)
        {
            return s_MemberListBindingCtor.Value(GetProperty(propertyAccessor), initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberMemberBinding" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <returns>A <see cref="MemberMemberBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.MemberBinding" /> and the <see cref="MemberBinding.Member" /> and <see cref="MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <param name="member">The <see cref="MemberInfo" /> to set the <see cref="MemberBinding.Member" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="MemberBinding" /> objects to use to populate the <see cref="MemberMemberBinding.Bindings" /> collection.</param>
        public static new MemberMemberBinding MemberBind(MemberInfo member, MemberBinding[] bindings)
        {
            return s_MemberMemberBindingCtor.Value(member, bindings.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberMemberBinding" /> that represents the recursive initialization of members of a field or property.</summary>
        /// <returns>A <see cref="MemberMemberBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.MemberBinding" /> and the <see cref="MemberBinding.Member" /> and <see cref="MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <param name="member">The <see cref="MemberInfo" /> to set the <see cref="MemberBinding.Member" /> property equal to.</param>
        /// <param name="bindings">An <see cref="IEnumerable{T}" /> that contains <see cref="MemberBinding" /> objects to use to populate the <see cref="MemberMemberBinding.Bindings" /> collection.</param>
        public static new MemberMemberBinding MemberBind(MemberInfo member, IEnumerable<MemberBinding> bindings)
        {
            return s_MemberMemberBindingCtor.Value(member, bindings.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberMemberBinding" /> that represents the recursive initialization of members of a member that is accessed by using a property accessor method.</summary>
        /// <returns>A <see cref="MemberMemberBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.MemberBinding" />, the <see cref="MemberBinding.Member" /> property set to the <see cref="PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <param name="propertyAccessor">The <see cref="MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="bindings">An array of <see cref="MemberBinding" /> objects to use to populate the <see cref="MemberMemberBinding.Bindings" /> collection.</param>
        public static new MemberMemberBinding MemberBind(MethodInfo propertyAccessor, MemberBinding[] bindings)
        {
            return s_MemberMemberBindingCtor.Value(GetProperty(propertyAccessor), bindings.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="MemberMemberBinding" /> that represents the recursive initialization of members of a member that is accessed by using a property accessor method.</summary>
        /// <returns>A <see cref="MemberMemberBinding" /> that has the <see cref="MemberBinding.BindingType" /> property equal to <see cref="MemberBindingType.MemberBinding" />, the <see cref="MemberBinding.Member" /> property set to the <see cref="PropertyInfo" /> that represents the property accessed in <paramref name="propertyAccessor" />, and <see cref="MemberMemberBinding.Bindings" /> properties set to the specified values.</returns>
        /// <param name="propertyAccessor">The <see cref="MethodInfo" /> that represents a property accessor method.</param>
        /// <param name="bindings">An <see cref="IEnumerable{T}" /> that contains <see cref="MemberBinding" /> objects to use to populate the <see cref="MemberMemberBinding.Bindings" /> collection.</param>
        public static new MemberMemberBinding MemberBind(MethodInfo propertyAccessor, IEnumerable<MemberBinding> bindings)
        {
            return s_MemberMemberBindingCtor.Value(GetProperty(propertyAccessor), bindings.ToReadOnlyUnsafe(/* OWNS */));
        }

        #endregion

        #region Call factories

        #region ArrayIndex

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents applying an array index operator to a multidimensional array.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="array">An array of <see cref="Expression" /> instances - indexes for the array index operation.</param>
        /// <param name="indexes">An array of <see cref="Expression" /> objects to use to populate the <see cref="MethodCallExpression.Arguments" /> collection.</param>
        public static new MethodCallExpression ArrayIndex(Expression array, Expression[] indexes)
        {
            var method = array.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public);

            return s_InstanceMethodCallExpressionNCtor1.Value?.Invoke(method, array, /* OWNS */ indexes) ?? s_InstanceMethodCallExpressionNCtor2.Value?.Invoke(method, array, /* OWNS */ indexes) ?? Expression.ArrayIndex(array, indexes);
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents applying an array index operator to an array of rank more than one.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="array">An <see cref="Expression" /> to set the <see cref="MethodCallExpression.Object" /> property equal to.</param>
        /// <param name="indexes">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="MethodCallExpression.Arguments" /> collection.</param>
        public static new MethodCallExpression ArrayIndex(Expression array, IEnumerable<Expression> indexes)
        {
            var method = array.Type.GetMethod("Get", BindingFlags.Instance | BindingFlags.Public);

            return s_InstanceMethodCallExpressionNCtor1.Value?.Invoke(method, array, indexes.ToIListUnsafe()) ?? s_InstanceMethodCallExpressionNCtor2.Value?.Invoke(method, array, indexes.ToIReadOnlyListUnsafe()) ?? Expression.ArrayIndex(array, indexes);
        }

        #endregion

        #region Call

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static (Shared in Visual Basic) method that takes one argument.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        public static new MethodCallExpression Call(MethodInfo method, Expression arg0)
        {
            return s_MethodCallExpression1Ctor.Value(method, arg0);
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static (Shared in Visual Basic) method that has arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Method" /> and <see cref="MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="method">A <see cref="MethodInfo" /> that represents a static (Shared in Visual Basic) method to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="Expression" /> objects to use to populate the <see cref="MethodCallExpression.Arguments" /> collection.</param>
        public static new MethodCallExpression Call(MethodInfo method, Expression[] arguments)
        {
            return s_MethodCallExpressionNCtor1.Value?.Invoke(method, /* OWNS */ arguments) ?? s_MethodCallExpressionNCtor2.Value(method, /* OWNS */ arguments);
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static (Shared in Visual Basic) method.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arguments">A collection of <see cref="Expression" /> that represents the call arguments.</param>
        public static new MethodCallExpression Call(MethodInfo method, IEnumerable<Expression> arguments)
        {
            return s_MethodCallExpressionNCtor1.Value?.Invoke(method, arguments.ToIListUnsafe()) ?? s_MethodCallExpressionNCtor2.Value(method, arguments.ToIReadOnlyListUnsafe());
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a method that takes no arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="instance">An <see cref="Expression" /> that specifies the instance for an instance method call (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        public static new MethodCallExpression Call(Expression instance, MethodInfo method)
        {
            return Call(instance, method, Array.Empty<Expression>());
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static method that takes two arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="Expression" /> that represents the second argument.</param>
        public static new MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1)
        {
            return s_MethodCallExpression2Ctor.Value(method, arg0, arg1);
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a method that takes arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" />, <see cref="MethodCallExpression.Method" />, and <see cref="MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="instance">An <see cref="Expression" /> that specifies the instance fo an instance method call (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="Expression" /> objects to use to populate the <see cref="MethodCallExpression.Arguments" /> collection.</param>
        public static new MethodCallExpression Call(Expression instance, MethodInfo method, Expression[] arguments)
        {
            if (instance == null)
            {
                return arguments.Length switch
                {
                    1 => s_MethodCallExpression1Ctor.Value(method, arguments[0]),
                    2 => s_MethodCallExpression2Ctor.Value(method, arguments[0], arguments[1]),
                    3 => s_MethodCallExpression3Ctor.Value(method, arguments[0], arguments[1], arguments[2]),
                    4 => s_MethodCallExpression4Ctor.Value(method, arguments[0], arguments[1], arguments[2], arguments[3]),
                    5 => s_MethodCallExpression5Ctor.Value(method, arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]),
                    _ => s_MethodCallExpressionNCtor1.Value?.Invoke(method, /* OWNS */ arguments) ?? s_MethodCallExpressionNCtor2.Value(method, /* OWNS */ arguments),
                };
            }
            else
            {
                return arguments.Length switch
                {
                    2 => s_InstanceMethodCallExpression2Ctor.Value(method, instance, arguments[0], arguments[1]),
                    3 => s_InstanceMethodCallExpression3Ctor.Value(method, instance, arguments[0], arguments[1], arguments[2]),
                    _ => s_InstanceMethodCallExpressionNCtor1.Value?.Invoke(method, instance, /* OWNS */ arguments) ?? s_InstanceMethodCallExpressionNCtor2.Value?.Invoke(method, instance, /* OWNS */ arguments) ?? Expression.Call(instance, method, arguments),
                };
            }
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a method that takes arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" />, <see cref="MethodCallExpression.Method" />, and <see cref="MethodCallExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="instance">An <see cref="Expression" /> to set the <see cref="MethodCallExpression.Object" /> property equal to (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="MethodCallExpression.Arguments" /> collection.</param>
        public static new MethodCallExpression Call(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
        {
            var args = arguments.ToIListUnsafe();

            if (instance == null)
            {
                return args.Count switch
                {
                    1 => s_MethodCallExpression1Ctor.Value(method, args[0]),
                    2 => s_MethodCallExpression2Ctor.Value(method, args[0], args[1]),
                    3 => s_MethodCallExpression3Ctor.Value(method, args[0], args[1], args[2]),
                    4 => s_MethodCallExpression4Ctor.Value(method, args[0], args[1], args[2], args[3]),
                    5 => s_MethodCallExpression5Ctor.Value(method, args[0], args[1], args[2], args[3], args[4]),
                    _ => s_MethodCallExpressionNCtor1.Value?.Invoke(method, /* OWNS */ args) ?? s_MethodCallExpressionNCtor2.Value(method, /* OWNS */ args.ToIReadOnlyListUnsafe()),
                };
            }
            else
            {
                return args.Count switch
                {
                    2 => s_InstanceMethodCallExpression2Ctor.Value(method, instance, args[0], args[1]),
                    3 => s_InstanceMethodCallExpression3Ctor.Value(method, instance, args[0], args[1], args[2]),
                    _ => s_InstanceMethodCallExpressionNCtor1.Value?.Invoke(method, instance, /* OWNS */ args) ?? s_InstanceMethodCallExpressionNCtor2.Value?.Invoke(method, instance, /* OWNS */ args.ToIReadOnlyListUnsafe()) ?? Expression.Call(instance, method, args),
                };
            }
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static method that takes three arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="Expression" /> that represents the third argument.</param>
        public static new MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2)
        {
            return s_MethodCallExpression3Ctor.Value(method, arg0, arg1, arg2);
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a method that takes two arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="instance">An <see cref="Expression" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="Expression" /> that represents the second argument.</param>
        public static new MethodCallExpression Call(Expression instance, MethodInfo method, Expression arg0, Expression arg1)
        {
            if (instance == null)
            {
                return s_MethodCallExpression2Ctor.Value(method, arg0, arg1);
            }
            else
            {
                return s_InstanceMethodCallExpression2Ctor.Value(method, instance, arg0, arg1);
            }
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static method that takes four arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="Expression" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="Expression" /> that represents the fourth argument.</param>
        public static new MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3)
        {
            return s_MethodCallExpression4Ctor.Value(method, arg0, arg1, arg2, arg3);
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a method that takes three arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="instance">An <see cref="Expression" /> that specifies the instance for an instance call. (pass null for a static (Shared in Visual Basic) method).</param>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the target method.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="Expression" /> that represents the third argument.</param>
        public static new MethodCallExpression Call(Expression instance, MethodInfo method, Expression arg0, Expression arg1, Expression arg2)
        {
            if (instance == null)
            {
                return s_MethodCallExpression3Ctor.Value(method, arg0, arg1, arg2);
            }
            else
            {
                return s_InstanceMethodCallExpression3Ctor.Value(method, instance, arg0, arg1, arg2);
            }
        }

        /// <summary>Creates a <see cref="MethodCallExpression" /> that represents a call to a static method that takes five arguments.</summary>
        /// <returns>A <see cref="MethodCallExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Call" /> and the <see cref="MethodCallExpression.Object" /> and <see cref="MethodCallExpression.Method" /> properties set to the specified values.</returns>
        /// <param name="method">A <see cref="MethodInfo" /> to set the <see cref="MethodCallExpression.Method" /> property equal to.</param>
        /// <param name="arg0">The <see cref="Expression" /> that represents the first argument.</param>
        /// <param name="arg1">The <see cref="Expression" /> that represents the second argument.</param>
        /// <param name="arg2">The <see cref="Expression" /> that represents the third argument.</param>
        /// <param name="arg3">The <see cref="Expression" /> that represents the fourth argument.</param>
        /// <param name="arg4">The <see cref="Expression" /> that represents the fifth argument.</param>
        public static new MethodCallExpression Call(MethodInfo method, Expression arg0, Expression arg1, Expression arg2, Expression arg3, Expression arg4)
        {
            return s_MethodCallExpression5Ctor.Value(method, arg0, arg1, arg2, arg3, arg4);
        }

        #endregion

        #endregion

        #region Conditional factories

        /// <summary>Creates a <see cref="ConditionalExpression" /> that represents a conditional statement.</summary>
        /// <returns>A <see cref="ConditionalExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Conditional" /> and the <see cref="ConditionalExpression.Test" />, <see cref="ConditionalExpression.IfTrue" />, and <see cref="ConditionalExpression.IfFalse" /> properties set to the specified values.</returns>
        /// <param name="test">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfFalse" /> property equal to.</param>
        public static new ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse)
        {
            return s_ConditionalExpressionMake.Value(test, ifTrue, ifFalse, ifTrue.Type);
        }

        /// <summary>Creates a <see cref="ConditionalExpression" /> that represents a conditional statement.</summary>
        /// <returns>A <see cref="ConditionalExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Conditional" /> and the <see cref="ConditionalExpression.Test" />, <see cref="ConditionalExpression.IfTrue" />, and <see cref="ConditionalExpression.IfFalse" /> properties set to the specified values.</returns>
        /// <param name="test">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfFalse" /> property equal to.</param>
        /// <param name="type">A <see cref="Expression.Type" /> to set the <see cref="Expression.Type" /> property equal to.</param>
        public static new ConditionalExpression Condition(Expression test, Expression ifTrue, Expression ifFalse, Type type)
        {
            return s_ConditionalExpressionMake.Value(test, ifTrue, ifFalse, type);
        }

        /// <summary>Creates a <see cref="ConditionalExpression" /> that represents a conditional block with an if statement.</summary>
        /// <returns>A <see cref="ConditionalExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Conditional" /> and the <see cref="ConditionalExpression.Test" />, <see cref="ConditionalExpression.IfTrue" />, properties set to the specified values. The <see cref="ConditionalExpression.IfFalse" /> property is set to default expression and the type of the resulting <see cref="ConditionalExpression" /> returned by this method is <see cref="Void" />.</returns>
        /// <param name="test">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfTrue" /> property equal to.</param>
        public static new ConditionalExpression IfThen(Expression test, Expression ifTrue)
        {
            return s_ConditionalExpressionMake.Value(test, ifTrue, s_void, typeof(void));
        }

        /// <summary>Creates a <see cref="ConditionalExpression" /> that represents a conditional block with if and else statements.</summary>
        /// <returns>A <see cref="ConditionalExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Conditional" /> and the <see cref="ConditionalExpression.Test" />, <see cref="ConditionalExpression.IfTrue" />, and <see cref="ConditionalExpression.IfFalse" /> properties set to the specified values. The type of the resulting <see cref="ConditionalExpression" /> returned by this method is <see cref="Void" />.</returns>
        /// <param name="test">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.Test" /> property equal to.</param>
        /// <param name="ifTrue">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfTrue" /> property equal to.</param>
        /// <param name="ifFalse">An <see cref="Expression" /> to set the <see cref="ConditionalExpression.IfFalse" /> property equal to.</param>
        public static new ConditionalExpression IfThenElse(Expression test, Expression ifTrue, Expression ifFalse)
        {
            return s_ConditionalExpressionMake.Value(test, ifTrue, ifFalse, typeof(void));
        }

        #endregion

        #region ElementInit factories

        /// <summary>Creates an <see cref="System.Linq.Expressions.ElementInit" />, given an array of values as the second argument.</summary>
        /// <returns>An <see cref="System.Linq.Expressions.ElementInit" /> that has the <see cref="ElementInit.AddMethod" /> and <see cref="ElementInit.Arguments" /> properties set to the specified values.</returns>
        /// <param name="addMethod">A <see cref="MethodInfo" /> to set the <see cref="ElementInit.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="Expression" /> objects to set the <see cref="ElementInit.Arguments" /> property equal to.</param>
        public static new ElementInit ElementInit(MethodInfo addMethod, Expression[] arguments)
        {
            return s_ElementInitCtor.Value(addMethod, arguments.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates an <see cref="System.Linq.Expressions.ElementInit" />, given an <see cref="IEnumerable{T}" /> as the second argument.</summary>
        /// <returns>An <see cref="System.Linq.Expressions.ElementInit" /> that has the <see cref="ElementInit.AddMethod" /> and <see cref="ElementInit.Arguments" /> properties set to the specified values.</returns>
        /// <param name="addMethod">A <see cref="MethodInfo" /> to set the <see cref="ElementInit.AddMethod" /> property equal to.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to set the <see cref="ElementInit.Arguments" /> property equal to.</param>
        public static new ElementInit ElementInit(MethodInfo addMethod, IEnumerable<Expression> arguments)
        {
            return s_ElementInitCtor.Value(addMethod, arguments.ToReadOnlyUnsafe(/* OWNS */));
        }

        #endregion

        #region Index factories

        /// <summary>Creates an <see cref="IndexExpression" /> to access an array.</summary>
        /// <returns>The created <see cref="IndexExpression" />.</returns>
        /// <param name="array">An expression representing the array to index.</param>
        /// <param name="indexes">An array that contains expressions used to index the array.</param>
        public static new IndexExpression ArrayAccess(Expression array, Expression[] indexes)
        {
            return s_IndexExpressionCtor1.Value?.Invoke(array, null, /* OWNS */ indexes) ?? s_IndexExpressionCtor2.Value(array, null, /* OWNS */ indexes);
        }

        /// <summary>Creates an <see cref="IndexExpression" /> to access a multidimensional array.</summary>
        /// <returns>The created <see cref="IndexExpression" />.</returns>
        /// <param name="array">An expression that represents the multidimensional array.</param>
        /// <param name="indexes">An <see cref="IEnumerable{T}" /> containing expressions used to index the array.</param>
        public static new IndexExpression ArrayAccess(Expression array, IEnumerable<Expression> indexes)
        {
            return s_IndexExpressionCtor1.Value?.Invoke(array, null, indexes.ToIListUnsafe()) ?? s_IndexExpressionCtor2.Value(array, null, indexes.ToIReadOnlyListUnsafe());
        }

        /// <summary>Creates an <see cref="IndexExpression" /> that represents accessing an indexed property in an object.</summary>
        /// <returns>The created <see cref="IndexExpression" />.</returns>
        /// <param name="instance">The object to which the property belongs. It should be null if the property is static (shared in Visual Basic).</param>
        /// <param name="indexer">An <see cref="Expression" /> representing the property to index.</param>
        /// <param name="arguments">An IEnumerable&lt;Expression&gt; (IEnumerable (Of Expression) in Visual Basic) that contains the arguments that will be used to index the property.</param>
        public static new IndexExpression MakeIndex(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments)
        {
            if (indexer != null)
            {
                return Property(instance, indexer, arguments);
            }

            return ArrayAccess(instance, arguments);
        }

        /// <summary>Creates an <see cref="IndexExpression" /> representing the access to an indexed property.</summary>
        /// <returns>The created <see cref="IndexExpression" />.</returns>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="PropertyInfo" /> that represents the property to index.</param>
        /// <param name="arguments">An array of <see cref="Expression" /> objects that are used to index the property.</param>
        public static new IndexExpression Property(Expression instance, PropertyInfo indexer, Expression[] arguments)
        {
            return s_IndexExpressionCtor1.Value?.Invoke(instance, indexer, /* OWNS */ arguments) ?? s_IndexExpressionCtor2.Value(instance, indexer, /* OWNS */ arguments);
        }

        /// <summary>Creates an <see cref="IndexExpression" /> representing the access to an indexed property.</summary>
        /// <returns>The created <see cref="IndexExpression" />.</returns>
        /// <param name="instance">The object to which the property belongs. If the property is static/shared, it must be null.</param>
        /// <param name="indexer">The <see cref="PropertyInfo" /> that represents the property to index.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> of <see cref="Expression" /> objects that are used to index the property.</param>
        public static new IndexExpression Property(Expression instance, PropertyInfo indexer, IEnumerable<Expression> arguments)
        {
            return s_IndexExpressionCtor1.Value?.Invoke(instance, indexer, arguments.ToIListUnsafe()) ?? s_IndexExpressionCtor2.Value(instance, indexer, arguments.ToIReadOnlyListUnsafe());
        }

        #endregion

        #region Invoke factories

        /// <summary>Creates an <see cref="InvocationExpression" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <returns>An <see cref="InvocationExpression" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        /// <param name="expression">An <see cref="Expression" /> that represents the delegate or lambda expression to be applied.</param>
        /// <param name="arguments">An array of <see cref="Expression" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        public static new InvocationExpression Invoke(Expression expression, Expression[] arguments)
        {
            var invokeMethod = GetInvokeMethod(expression);

            return s_InvocationExpressionCtor1.Value?.Invoke(expression, /* OWNS */ arguments, invokeMethod.ReturnType) ?? s_InvocationExpressionCtor2.Value?.Invoke(expression, /* OWNS */ arguments, invokeMethod.ReturnType) ?? Expression.Invoke(expression, arguments);
        }

        /// <summary>Creates an <see cref="InvocationExpression" /> that applies a delegate or lambda expression to a list of argument expressions.</summary>
        /// <returns>An <see cref="InvocationExpression" /> that applies the specified delegate or lambda expression to the provided arguments.</returns>
        /// <param name="expression">An <see cref="Expression" /> that represents the delegate or lambda expression to be applied to.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects that represent the arguments that the delegate or lambda expression is applied to.</param>
        public static new InvocationExpression Invoke(Expression expression, IEnumerable<Expression> arguments)
        {
            var invokeMethod = GetInvokeMethod(expression);

            return s_InvocationExpressionCtor1.Value?.Invoke(expression, arguments.ToIListUnsafe(), invokeMethod.ReturnType) ?? s_InvocationExpressionCtor2.Value?.Invoke(expression, arguments.ToIReadOnlyListUnsafe(), invokeMethod.ReturnType) ?? Expression.Invoke(expression, arguments);
        }

        #endregion

        #region Lambda factories

        /// <summary>Creates a <see cref="LambdaExpression" /> by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Expression body, ParameterExpression[] parameters)
        {
            var ps = parameters.ToReadOnlyUnsafe(/* OWNS */);
            var delegateType = GetDelegateType(body, ps);
            return s_CreateLambda.Value(delegateType, body, null, false, ps);
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Expression body, IEnumerable<ParameterExpression> parameters)
        {
            var ps = parameters.ToReadOnlyUnsafe(/* OWNS */);
            var delegateType = GetDelegateType(body, ps);
            return s_CreateLambda.Value(delegateType, body, null, false, ps);
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An array that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Expression body, bool tailCall, ParameterExpression[] parameters)
        {
            var ps = parameters.ToReadOnlyUnsafe(/* OWNS */);
            var delegateType = GetDelegateType(body, ps);
            return s_CreateLambda.Value(delegateType, body, null, tailCall, ps);
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Expression body, bool tailCall, IEnumerable<ParameterExpression> parameters)
        {
            var ps = parameters.ToReadOnlyUnsafe(/* OWNS */);
            var delegateType = GetDelegateType(body, ps);
            return s_CreateLambda.Value(delegateType, body, null, tailCall, ps);
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Expression body, string name, IEnumerable<ParameterExpression> parameters)
        {
            var ps = parameters.ToReadOnlyUnsafe(/* OWNS */);
            var delegateType = GetDelegateType(body, ps);
            return s_CreateLambda.Value(delegateType, body, name, false, ps);
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters)
        {
            var ps = parameters.ToReadOnlyUnsafe(/* OWNS */);
            var delegateType = GetDelegateType(body, ps);
            return s_CreateLambda.Value(delegateType, body, name, tailCall, ps);
        }

        /// <summary>
        /// Gets the Func or Action delegate type for a lambda expression with the specified body expression and parameter expressions.
        /// </summary>
        /// <param name="body">Body of the lambda expression.</param>
        /// <param name="parameters">Parameters of the lambda expression.</param>
        /// <returns>Func or Action delegate type based on the types of the specified body and parameter expressions.</returns>
        private static Type GetDelegateType(Expression body, ReadOnlyCollection<ParameterExpression> parameters)
        {
            var n = parameters.Count;

            var types = new Type[n + 1];

            for (var i = 0; i < n; i++)
            {
                types[i] = parameters[i].Type;
            }

            types[n] = body.Type;

            return GetDelegateType(types);
        }

        /// <summary>Creates a <see cref="LambdaExpression" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <returns>An object that represents a lambda expression which has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="delegateType">A <see cref="Type" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Type delegateType, Expression body, ParameterExpression[] parameters)
        {
            return s_CreateLambda.Value(delegateType, body, null, false, parameters.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="LambdaExpression" /> by first constructing a delegate type. It can be used when the delegate type is not known at compile time.</summary>
        /// <returns>An object that represents a lambda expression which has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="delegateType">A <see cref="Type" /> that represents a delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Type delegateType, Expression body, IEnumerable<ParameterExpression> parameters)
        {
            return s_CreateLambda.Value(delegateType, body, null, false, parameters.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="delegateType">A <see cref="Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An array that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, ParameterExpression[] parameters)
        {
            return s_CreateLambda.Value(delegateType, body, null, tailCall, parameters.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="delegateType">A <see cref="Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Type delegateType, Expression body, bool tailCall, IEnumerable<ParameterExpression> parameters)
        {
            return s_CreateLambda.Value(delegateType, body, null, tailCall, parameters.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="delegateType">A <see cref="Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        public static new LambdaExpression Lambda(Type delegateType, Expression body, string name, IEnumerable<ParameterExpression> parameters)
        {
            return s_CreateLambda.Value(delegateType, body, name, false, parameters.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a LambdaExpression by first constructing a delegate type.</summary>
        /// <returns>A <see cref="LambdaExpression" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to Lambda and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="delegateType">A <see cref="Expression.Type" /> representing the delegate signature for the lambda.</param>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to. </param>
        /// <param name="name">The name for the lambda. Used for emitting debug information.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression. </param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection. </param>
        public static new LambdaExpression Lambda(Type delegateType, Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters)
        {
            return s_CreateLambda.Value(delegateType, body, name, tailCall, parameters.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates an <see cref="Expression{TDelegate}" /> where the delegate type is known at compile time.</summary>
        /// <returns>An <see cref="Expression{TDelegate}" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An array of <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">A delegate type.</typeparam>
        public static new Expression<TDelegate> Lambda<TDelegate>(Expression body, ParameterExpression[] parameters)
        {
            return (Expression<TDelegate>)GetLambdaCtor<TDelegate>()(body, null, false, parameters.ToReadOnly(/* OWNS */));
        }

        /// <summary>Creates an <see cref="Expression{TDelegate}" /> where the delegate type is known at compile time.</summary>
        /// <returns>An <see cref="Expression{TDelegate}" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">A delegate type.</typeparam>
        public static new Expression<TDelegate> Lambda<TDelegate>(Expression body, IEnumerable<ParameterExpression> parameters)
        {
            return (Expression<TDelegate>)GetLambdaCtor<TDelegate>()(body, null, false, parameters.ToReadOnly(/* OWNS */));
        }

        /// <summary>Creates an <see cref="Expression{TDelegate}" /> where the delegate type is known at compile time.</summary>
        /// <returns>An <see cref="Expression{TDelegate}" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An array that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        public static new Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, ParameterExpression[] parameters)
        {
            return (Expression<TDelegate>)GetLambdaCtor<TDelegate>()(body, null, tailCall, parameters.ToReadOnly(/* OWNS */));
        }

        /// <summary>Creates an <see cref="Expression{TDelegate}" /> where the delegate type is known at compile time.</summary>
        /// <returns>An <see cref="Expression{TDelegate}" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" />and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        public static new Expression<TDelegate> Lambda<TDelegate>(Expression body, bool tailCall, IEnumerable<ParameterExpression> parameters)
        {
            return (Expression<TDelegate>)GetLambdaCtor<TDelegate>()(body, null, tailCall, parameters.ToReadOnly(/* OWNS */));
        }

        /// <summary>Creates an <see cref="Expression{TDelegate}" /> where the delegate type is known at compile time.</summary>
        /// <returns>An <see cref="Expression{TDelegate}" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name of the lambda. Used for generating debugging information.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        public static new Expression<TDelegate> Lambda<TDelegate>(Expression body, string name, IEnumerable<ParameterExpression> parameters)
        {
            return (Expression<TDelegate>)GetLambdaCtor<TDelegate>()(body, name, false, parameters.ToReadOnly(/* OWNS */));
        }

        /// <summary>Creates an <see cref="Expression{TDelegate}" /> where the delegate type is known at compile time.</summary>
        /// <returns>An <see cref="Expression{TDelegate}" /> that has the <see cref="LambdaExpression.NodeType" /> property equal to <see cref="ExpressionType.Lambda" /> and the <see cref="LambdaExpression.Body" /> and <see cref="LambdaExpression.Parameters" /> properties set to the specified values.</returns>
        /// <param name="body">An <see cref="Expression" /> to set the <see cref="LambdaExpression.Body" /> property equal to.</param>
        /// <param name="name">The name of the lambda. Used for generating debugging info.</param>
        /// <param name="tailCall">A <see cref="bool" /> that indicates if tail call optimization will be applied when compiling the created expression.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}" /> that contains <see cref="ParameterExpression" /> objects to use to populate the <see cref="LambdaExpression.Parameters" /> collection.</param>
        /// <typeparam name="TDelegate">The delegate type. </typeparam>
        public static new Expression<TDelegate> Lambda<TDelegate>(Expression body, string name, bool tailCall, IEnumerable<ParameterExpression> parameters)
        {
            return (Expression<TDelegate>)GetLambdaCtor<TDelegate>()(body, name, tailCall, parameters.ToReadOnly(/* OWNS */));
        }

        /// <summary>
        /// Gets the constructor for <see cref="Expression{TDelegate}"/>.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type.</typeparam>
        /// <returns>Delegate to invoke the constructor on <see cref="Expression{TDelegate}"/>.</returns>
        private static Func<Expression, string, bool, ReadOnlyCollection<ParameterExpression>, LambdaExpression> GetLambdaCtor<TDelegate>()
        {
            if (!s_lambdaCtors.TryGetValue(typeof(TDelegate), out Func<Expression, string, bool, ReadOnlyCollection<ParameterExpression>, LambdaExpression> ctor))
            {
                ctor = s_lambdaCtors.GetValue(typeof(TDelegate), key => CreateCtorDelegate<Func<Expression, string, bool, ReadOnlyCollection<ParameterExpression>, LambdaExpression>>(typeof(Expression<>).MakeGenericType(key), s_expressionCtorArgs, Expression.Lambda));
            }

            return ctor;
        }

        #endregion

        #region ListInit factories

        /// <summary>Creates a <see cref="ListInitExpression" /> that uses specified <see cref="System.Linq.Expressions.ElementInit" /> objects to initialize a collection.</summary>
        /// <returns>A <see cref="ListInitExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.ListInit" /> and the <see cref="ListInitExpression.NewExpression" /> and <see cref="ListInitExpression.Initializers" /> properties set to the specified values.</returns>
        /// <param name="newExpression">A <see cref="NewExpression" /> to set the <see cref="ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An array of <see cref="System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="ListInitExpression.Initializers" /> collection.</param>
        public static new ListInitExpression ListInit(NewExpression newExpression, ElementInit[] initializers)
        {
            return s_ListInitExpressionCtor.Value(newExpression, initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="ListInitExpression" /> that uses specified <see cref="System.Linq.Expressions.ElementInit" /> objects to initialize a collection.</summary>
        /// <returns>A <see cref="ListInitExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.ListInit" /> and the <see cref="ListInitExpression.NewExpression" /> and <see cref="ListInitExpression.Initializers" /> properties set to the specified values.</returns>
        /// <param name="newExpression">A <see cref="NewExpression" /> to set the <see cref="ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="initializers">An <see cref="IEnumerable{T}" /> that contains <see cref="System.Linq.Expressions.ElementInit" /> objects to use to populate the <see cref="ListInitExpression.Initializers" /> collection.</param>
        public static new ListInitExpression ListInit(NewExpression newExpression, IEnumerable<ElementInit> initializers)
        {
            return s_ListInitExpressionCtor.Value(newExpression, initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="ListInitExpression" /> that uses a specified method to add elements to a collection.</summary>
        /// <returns>A <see cref="ListInitExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.ListInit" /> and the <see cref="ListInitExpression.NewExpression" /> property set to the specified value.</returns>
        /// <param name="newExpression">A <see cref="NewExpression" /> to set the <see cref="ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="MethodInfo" /> that represents an instance method that takes one argument, that adds an element to a collection.</param>
        /// <param name="initializers">An array of <see cref="Expression" /> objects to use to populate the <see cref="ListInitExpression.Initializers" /> collection.</param>
        public static new ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, Expression[] initializers)
        {
            var n = initializers.Length;

            var inits = new ElementInit[n];
            for (var i = 0; i < n; i++)
            {
                inits[i] = ElementInit(addMethod, initializers[i]);
            }

            return s_ListInitExpressionCtor.Value(newExpression, inits.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="ListInitExpression" /> that uses a specified method to add elements to a collection.</summary>
        /// <returns>A <see cref="ListInitExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.ListInit" /> and the <see cref="ListInitExpression.NewExpression" /> property set to the specified value.</returns>
        /// <param name="newExpression">A <see cref="NewExpression" /> to set the <see cref="ListInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="addMethod">A <see cref="MethodInfo" /> that represents an instance method named "Add" (case insensitive), that adds an element to a collection.</param>
        /// <param name="initializers">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="ListInitExpression.Initializers" /> collection.</param>
        public static new ListInitExpression ListInit(NewExpression newExpression, MethodInfo addMethod, IEnumerable<Expression> initializers)
        {
            var inits = new List<ElementInit>();
            foreach (var initializer in initializers)
            {
                inits.Add(ElementInit(addMethod, initializer));
            }

            return s_ListInitExpressionCtor.Value(newExpression, inits.ToReadOnlyUnsafe(/* OWNS */));
        }

        #endregion

        #region MemberInit factories

        /// <summary>Creates a <see cref="MemberInitExpression" />.</summary>
        /// <returns>A <see cref="MemberInitExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.MemberInit" /> and the <see cref="MemberInitExpression.NewExpression" /> and <see cref="MemberInitExpression.Bindings" /> properties set to the specified values.</returns>
        /// <param name="newExpression">A <see cref="NewExpression" /> to set the <see cref="MemberInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An array of <see cref="MemberBinding" /> objects to use to populate the <see cref="MemberInitExpression.Bindings" /> collection.</param>
        public static new MemberInitExpression MemberInit(NewExpression newExpression, MemberBinding[] bindings)
        {
            return s_MemberInitExpressionCtor.Value(newExpression, bindings.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Represents an expression that creates a new object and initializes a property of the object.</summary>
        /// <returns>A <see cref="MemberInitExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.MemberInit" /> and the <see cref="MemberInitExpression.NewExpression" /> and <see cref="MemberInitExpression.Bindings" /> properties set to the specified values.</returns>
        /// <param name="newExpression">A <see cref="NewExpression" /> to set the <see cref="MemberInitExpression.NewExpression" /> property equal to.</param>
        /// <param name="bindings">An <see cref="IEnumerable{T}" /> that contains <see cref="MemberBinding" /> objects to use to populate the <see cref="MemberInitExpression.Bindings" /> collection.</param>
        public static new MemberInitExpression MemberInit(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
        {
            return s_MemberInitExpressionCtor.Value(newExpression, bindings.ToReadOnlyUnsafe(/* OWNS */));
        }

        #endregion

        #region New factories

        /// <summary>Creates a <see cref="NewExpression" /> that represents calling the specified constructor that takes no arguments.</summary>
        /// <returns>A <see cref="NewExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.New" /> and the <see cref="NewExpression.Constructor" /> property set to the specified value.</returns>
        /// <param name="constructor">The <see cref="ConstructorInfo" /> to set the <see cref="NewExpression.Constructor" /> property equal to.</param>
        public static new NewExpression New(ConstructorInfo constructor)
        {
            return s_NewExpressionCtor1.Value?.Invoke(constructor, EmptyReadOnlyCollection<Expression>.Instance, null) ?? s_NewExpressionCtor2.Value(constructor, EmptyReadOnlyCollection<Expression>.Instance, null);
        }

        /// <summary>Creates a <see cref="NewExpression" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <returns>A <see cref="NewExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.New" /> and the <see cref="NewExpression.Constructor" /> and <see cref="NewExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="constructor">The <see cref="ConstructorInfo" /> to set the <see cref="NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An array of <see cref="Expression" /> objects to use to populate the <see cref="NewExpression.Arguments" /> collection.</param>
        public static new NewExpression New(ConstructorInfo constructor, Expression[] arguments)
        {
            return s_NewExpressionCtor1.Value?.Invoke(constructor, /* OWNS */ arguments, null) ?? s_NewExpressionCtor2.Value(constructor, /* OWNS */ arguments, null);
        }

        /// <summary>Creates a <see cref="NewExpression" /> that represents calling the specified constructor with the specified arguments.</summary>
        /// <returns>A <see cref="NewExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.New" /> and the <see cref="NewExpression.Constructor" /> and <see cref="NewExpression.Arguments" /> properties set to the specified values.</returns>
        /// <param name="constructor">The <see cref="ConstructorInfo" /> to set the <see cref="NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="NewExpression.Arguments" /> collection.</param>
        public static new NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments)
        {
            return s_NewExpressionCtor1.Value?.Invoke(constructor, arguments.ToIListUnsafe(), null) ?? s_NewExpressionCtor2.Value(constructor, arguments.ToIReadOnlyListUnsafe(), null);
        }

        /// <summary>Creates a <see cref="NewExpression" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified.</summary>
        /// <returns>A <see cref="NewExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.New" /> and the <see cref="NewExpression.Constructor" />, <see cref="NewExpression.Arguments" /> and <see cref="NewExpression.Members" /> properties set to the specified values.</returns>
        /// <param name="constructor">The <see cref="ConstructorInfo" /> to set the <see cref="NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="NewExpression.Arguments" /> collection.</param>
        /// <param name="members">An <see cref="IEnumerable{T}" /> that contains <see cref="MemberInfo" /> objects to use to populate the <see cref="NewExpression.Members" /> collection.</param>
        public static new NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, IEnumerable<MemberInfo> members)
        {
            return s_NewExpressionCtor1.Value?.Invoke(constructor, arguments.ToIListUnsafe(), members.ToReadOnlyUnsafe(/* OWNS */)) ?? s_NewExpressionCtor2.Value(constructor, arguments.ToIReadOnlyListUnsafe(), members.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="NewExpression" /> that represents calling the specified constructor with the specified arguments. The members that access the constructor initialized fields are specified as an array.</summary>
        /// <returns>A <see cref="NewExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.New" /> and the <see cref="NewExpression.Constructor" />, <see cref="NewExpression.Arguments" /> and <see cref="NewExpression.Members" /> properties set to the specified values.</returns>
        /// <param name="constructor">The <see cref="ConstructorInfo" /> to set the <see cref="NewExpression.Constructor" /> property equal to.</param>
        /// <param name="arguments">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="NewExpression.Arguments" /> collection.</param>
        /// <param name="members">An array of <see cref="MemberInfo" /> objects to use to populate the <see cref="NewExpression.Members" /> collection.</param>
        public static new NewExpression New(ConstructorInfo constructor, IEnumerable<Expression> arguments, MemberInfo[] members)
        {
            return s_NewExpressionCtor1.Value?.Invoke(constructor, arguments.ToIListUnsafe(), members.ToReadOnlyUnsafe(/* OWNS */)) ?? s_NewExpressionCtor2.Value(constructor, arguments.ToIReadOnlyListUnsafe(), members.ToReadOnlyUnsafe(/* OWNS */));
        }

        #endregion

        #region NewArray factories

        /// <summary>Creates a <see cref="NewArrayExpression" /> that represents creating an array that has a specified rank.</summary>
        /// <returns>A <see cref="NewArrayExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.NewArrayBounds" /> and the <see cref="NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <param name="type">A <see cref="Type" /> that represents the element type of the array.</param>
        /// <param name="bounds">An array of <see cref="Expression" /> objects to use to populate the <see cref="NewArrayExpression.Expressions" /> collection.</param>
        public static new NewArrayExpression NewArrayBounds(Type type, Expression[] bounds)
        {
            var arrayType = bounds.Length == 1 ? type.MakeArrayType() : type.MakeArrayType(bounds.Length);
            return s_NewArrayExpressionMake.Value(ExpressionType.NewArrayBounds, arrayType, bounds.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="NewArrayExpression" /> that represents creating an array that has a specified rank.</summary>
        /// <returns>A <see cref="NewArrayExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.NewArrayBounds" /> and the <see cref="NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <param name="type">A <see cref="Type" /> that represents the element type of the array.</param>
        /// <param name="bounds">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="NewArrayExpression.Expressions" /> collection.</param>
        public static new NewArrayExpression NewArrayBounds(Type type, IEnumerable<Expression> bounds)
        {
            var b = bounds.ToReadOnlyUnsafe(/* OWNS */);
            var arrayType = b.Count == 1 ? type.MakeArrayType() : type.MakeArrayType(b.Count);
            return s_NewArrayExpressionMake.Value(ExpressionType.NewArrayBounds, arrayType, b);
        }

        /// <summary>Creates a <see cref="NewArrayExpression" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <returns>A <see cref="NewArrayExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.NewArrayInit" /> and the <see cref="NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <param name="type">A <see cref="Type" /> that represents the element type of the array.</param>
        /// <param name="initializers">An array of <see cref="Expression" /> objects to use to populate the <see cref="NewArrayExpression.Expressions" /> collection.</param>
        public static new NewArrayExpression NewArrayInit(Type type, Expression[] initializers)
        {
            var arrayType = type.MakeArrayType();
            return s_NewArrayExpressionMake.Value(ExpressionType.NewArrayInit, arrayType, initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        /// <summary>Creates a <see cref="NewArrayExpression" /> that represents creating a one-dimensional array and initializing it from a list of elements.</summary>
        /// <returns>A <see cref="NewArrayExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.NewArrayInit" /> and the <see cref="NewArrayExpression.Expressions" /> property set to the specified value.</returns>
        /// <param name="type">A <see cref="Type" /> that represents the element type of the array.</param>
        /// <param name="initializers">An <see cref="IEnumerable{T}" /> that contains <see cref="Expression" /> objects to use to populate the <see cref="NewArrayExpression.Expressions" /> collection.</param>
        public static new NewArrayExpression NewArrayInit(Type type, IEnumerable<Expression> initializers)
        {
            var arrayType = type.MakeArrayType();
            return s_NewArrayExpressionMake.Value(ExpressionType.NewArrayInit, arrayType, initializers.ToReadOnlyUnsafe(/* OWNS */));
        }

        #endregion

        #region Unary factories

        #region ArrayLength

        /// <summary>Creates a <see cref="UnaryExpression" /> that represents an expression for obtaining the length of a one-dimensional array.</summary>
        /// <returns>A <see cref="UnaryExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.ArrayLength" /> and the <see cref="UnaryExpression.Operand" /> property equal to <paramref name="array" />.</returns>
        /// <param name="array">An <see cref="Expression" /> to set the <see cref="UnaryExpression.Operand" /> property equal to.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="array" /> is null.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="array" />.Type does not represent an array type.</exception>
        public static new UnaryExpression ArrayLength(Expression array)
        {
            return s_UnaryExpressionCtor.Value(ExpressionType.ArrayLength, array, typeof(int), null);
        }

        #endregion

        #region Conversions

        /// <summary>Creates a <see cref="UnaryExpression" /> that represents an explicit reference or boxing conversion where null is supplied if the conversion fails.</summary>
        /// <returns>A <see cref="UnaryExpression" /> that has the <see cref="Expression.NodeType" /> property equal to <see cref="ExpressionType.TypeAs" /> and the <see cref="UnaryExpression.Operand" /> and <see cref="Expression.Type" /> properties set to the specified values.</returns>
        /// <param name="expression">An <see cref="Expression" /> to set the <see cref="UnaryExpression.Operand" /> property equal to.</param>
        /// <param name="type">A <see cref="Type" /> to set the <see cref="Expression.Type" /> property equal to.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="expression" /> or <paramref name="type" /> is null.</exception>
        public static new UnaryExpression TypeAs(Expression expression, Type type)
        {
            return s_UnaryExpressionCtor.Value(ExpressionType.TypeAs, expression, type, null);
        }

        #endregion

        #region Make factories

        /// <summary>Creates a <see cref="UnaryExpression" />, given an operand, by calling the appropriate factory method.</summary>
        /// <returns>The <see cref="UnaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <param name="unaryType">The <see cref="ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="Expression" /> that represents the operand.</param>
        /// <param name="type">The <see cref="Type" /> that specifies the type to be converted to (pass null if not applicable).</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="operand" /> is null.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
        public static new UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type)
        {
            return MakeUnary(unaryType, operand, type, method: null);
        }

        /// <summary>Creates a <see cref="UnaryExpression" />, given an operand and implementing method, by calling the appropriate factory method.</summary>
        /// <returns>The <see cref="UnaryExpression" /> that results from calling the appropriate factory method.</returns>
        /// <param name="unaryType">The <see cref="ExpressionType" /> that specifies the type of unary operation.</param>
        /// <param name="operand">An <see cref="Expression" /> that represents the operand.</param>
        /// <param name="type">The <see cref="Type" /> that specifies the type to be converted to (pass null if not applicable).</param>
        /// <param name="method">The <see cref="MethodInfo" /> that represents the implementing method.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="operand" /> is null.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="unaryType" /> does not correspond to a unary expression node.</exception>
        public static new UnaryExpression MakeUnary(ExpressionType unaryType, Expression operand, Type type, MethodInfo method)
        {
            return unaryType switch
            {
                ExpressionType.Negate => Negate(operand, method),
                ExpressionType.NegateChecked => NegateChecked(operand, method),
                ExpressionType.Not => Not(operand, method),
                ExpressionType.IsFalse => IsFalse(operand, method),
                ExpressionType.IsTrue => IsTrue(operand, method),
                ExpressionType.OnesComplement => OnesComplement(operand, method),
                ExpressionType.ArrayLength => ArrayLength(operand),
                ExpressionType.Convert => Convert(operand, type, method),
                ExpressionType.ConvertChecked => ConvertChecked(operand, type, method),
                ExpressionType.Throw => Throw(operand, type),
                ExpressionType.TypeAs => TypeAs(operand, type),
                ExpressionType.Quote => Quote(operand),
                ExpressionType.UnaryPlus => UnaryPlus(operand, method),
                ExpressionType.Unbox => Unbox(operand, type),
                ExpressionType.Increment => Increment(operand, method),
                ExpressionType.Decrement => Decrement(operand, method),
                ExpressionType.PreIncrementAssign => PreIncrementAssign(operand, method),
                ExpressionType.PostIncrementAssign => PostIncrementAssign(operand, method),
                ExpressionType.PreDecrementAssign => PreDecrementAssign(operand, method),
                ExpressionType.PostDecrementAssign => PostDecrementAssign(operand, method),
                _ => throw new ArgumentException("Unhandled unary: " + unaryType, nameof(unaryType)),
            };
        }

        #endregion

        #region Unbox

        /// <summary>Creates a <see cref="UnaryExpression" /> that represents an explicit unboxing.</summary>
        /// <returns>An instance of <see cref="UnaryExpression" />.</returns>
        /// <param name="expression">An <see cref="Expression" /> to unbox.</param>
        /// <param name="type">The new <see cref="Type" /> of the expression.</param>
        public static new UnaryExpression Unbox(Expression expression, Type type)
        {
            return s_UnaryExpressionCtor.Value(ExpressionType.Unbox, expression, type, null);
        }

        #endregion

        #endregion
    }
}
