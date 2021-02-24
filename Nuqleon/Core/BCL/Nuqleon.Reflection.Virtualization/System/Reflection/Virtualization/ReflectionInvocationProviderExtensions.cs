// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Globalization;

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="IReflectionInvocationProvider"/>.
    /// </summary>
    public static class ReflectionInvocationProviderExtensions
    {
        /// <summary>
        /// Locates the specified type from specified <paramref name="assembly"/> and creates an instance of it using the system activator, using case-sensitive search.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="assembly">The assembly containing the type to instantiate.</param>
        /// <param name="typeName">The <see cref="Type.FullName" /> of the type to locate.</param>
        /// <returns>An instance of the specified type, or null if <paramref name="typeName" /> is not found. The supplied arguments are used to resolve the type, and to bind the constructor that is used to create the instance.</returns>
        public static object CreateInstance(this IReflectionInvocationProvider provider, Assembly assembly, string typeName) => NotNull(provider).CreateInstance(assembly, typeName, ignoreCase: false, BindingFlags.Instance | BindingFlags.Public, binder: null, args: null, culture: null, activationAttributes: null);

        /// <summary>
        /// Locates the specified type from specified <paramref name="assembly"/> and creates an instance of it using the system activator, with optional case-sensitive search.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="assembly">The assembly containing the type to instantiate.</param>
        /// <param name="typeName">The <see cref="Type.FullName" /> of the type to locate.</param>
        /// <param name="ignoreCase">true to ignore the case of the type name; otherwise, false.</param>
        /// <returns>An instance of the specified type, or null if <paramref name="typeName" /> is not found. The supplied arguments are used to resolve the type, and to bind the constructor that is used to create the instance.</returns>
        public static object CreateInstance(this IReflectionInvocationProvider provider, Assembly assembly, string typeName, bool ignoreCase) => NotNull(provider).CreateInstance(assembly, typeName, ignoreCase, BindingFlags.Instance | BindingFlags.Public, binder: null, args: null, culture: null, activationAttributes: null);

        /// <summary>
        /// Invokes the constructor reflected by the instance that has the specified parameters, providing default values for the parameters not commonly used.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="constructor">The constructor to invoke.</param>
        /// <param name="parameters">An array of values that matches the number, order and type (under the constraints of the default binder) of the parameters for this constructor. If this constructor takes no parameters, then use either an array with zero elements or null.</param>
        /// <returns>An instance of the class associated with the constructor.</returns>
        public static object Invoke(this IReflectionInvocationProvider provider, ConstructorInfo constructor, object[] parameters) => NotNull(provider).Invoke(constructor, BindingFlags.Default, binder: null, parameters, culture: null);

        /// <summary>
        /// Invokes the specified member, using the specified binding constraints and matching the specified argument list.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="type">The type to invoke a member on.</param>
        /// <param name="name">The string containing the name of the constructor, method, property, or field member to invoke.</param>
        /// <param name="invokeAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on. The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="target">The object on which to invoke the specified member.</param>
        /// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
        /// <returns>An object representing the return value of the invoked member.</returns>
        public static object InvokeMember(this IReflectionInvocationProvider provider, Type type, string name, BindingFlags invokeAttr, Binder binder, object target, object[] args) => NotNull(provider).InvokeMember(type, name, invokeAttr, binder, target, args, modifiers: null, culture: null, namedParameters: null);

        /// <summary>
        /// Invokes the specified member, using the specified binding constraints and matching the specified argument list and culture.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="type">The type to invoke a member on.</param>
        /// <param name="name">The string containing the name of the constructor, method, property, or field member to invoke.</param>
        /// <param name="invokeAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on. The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="target">The object on which to invoke the specified member.</param>
        /// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> object representing the globalization locale to use, which may be necessary for locale-specific conversions, such as converting a numeric String to a Double. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>An object representing the return value of the invoked member.</returns>
        public static object InvokeMember(this IReflectionInvocationProvider provider, Type type, string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, CultureInfo culture) => NotNull(provider).InvokeMember(type, name, invokeAttr, binder, target, args, modifiers: null, culture, namedParameters: null);

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="field">The field to set the value on.</param>
        /// <param name="obj">The object whose field value will be set, or null for a static field.</param>
        /// <param name="value">The value to assign to the field.</param>
        public static void SetValue(this IReflectionInvocationProvider provider, FieldInfo field, object obj, object value) => NotNull(provider).SetValue(field, obj, value, BindingFlags.Default, Type.DefaultBinder, culture: null);

        /// <summary>
        /// Invokes the reflected method or constructor with the given parameters.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="method">The method or constructor to invoke.</param>
        /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.</param>
        /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, this should be null.</param>
        /// <returns>An object containing the return value of the invoked method, or null in the case of a constructor, or null if the method's return type is void.</returns>
        public static object Invoke(this IReflectionInvocationProvider provider, MethodBase method, object obj, object[] parameters) => NotNull(provider).Invoke(method, obj, BindingFlags.Default, binder: null, parameters, culture: null);

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="property">The property for which to get the value.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        public static object GetValue(this IReflectionInvocationProvider provider, PropertyInfo property, object obj) => NotNull(provider).GetValue(property, obj, index: null);

        /// <summary>
        /// Returns the property value of a specified object that has the specified index.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="property">The property for which to get the value.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>The property value of the specified object.</returns>
        public static object GetValue(this IReflectionInvocationProvider provider, PropertyInfo property, object obj, object[] index) => NotNull(provider).GetValue(property, obj, BindingFlags.Default, binder: null, index, culture: null);

        /// <summary>
        /// Sets the property value for a specified object.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="property">The property for which to set the value.</param>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        public static void SetValue(this IReflectionInvocationProvider provider, PropertyInfo property, object obj, object value) => NotNull(provider).SetValue(property, obj, value, index: null);

        /// <summary>
        /// Sets the property value for a specified object that has the specified index.
        /// </summary>
        /// <param name="provider">The reflection invocation provider.</param>
        /// <param name="property">The property for which to set the value.</param>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        public static void SetValue(this IReflectionInvocationProvider provider, PropertyInfo property, object obj, object value, object[] index) => NotNull(provider).SetValue(property, obj, value, BindingFlags.Default, binder: null, index, culture: null);
    }
}
