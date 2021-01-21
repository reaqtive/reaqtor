// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;
using System.Globalization;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to invoke reflection objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IReflectionInvocationProvider
    {
        /// <summary>
        /// Locates the specified type from specified <paramref name="assembly"/> and creates an instance of it using the system activator, with optional case-sensitive search and having the specified culture, arguments, and binding and activation attributes.
        /// </summary>
        /// <param name="assembly">The assembly containing the type to instantiate.</param>
        /// <param name="typeName">The <see cref="Type.FullName" /> of the type to locate.</param>
        /// <param name="ignoreCase">true to ignore the case of the type name; otherwise, false.</param>
        /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of bit flags from <see cref="BindingFlags" />. </param>
        /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
        /// <param name="args">An array that contains the arguments to be passed to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to be invoked. If the default constructor is desired, <paramref name="args" /> must be an empty array or null.</param>
        /// <param name="culture">An instance of CultureInfo used to govern the coercion of types. If this is null, the CultureInfo for the current thread is used. (This is necessary to convert a String that represents 1000 to a Double value, for example, since 1000 is represented differently by different cultures.)</param>
        /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <c>System.Runtime.Remoting.Activation.UrlAttribute</c> object. The <c>System.Runtime.Remoting.Activation.UrlAttribute</c> specifies the URL that is required to activate a remote object.</param>
        /// <returns>An instance of the specified type, or null if <paramref name="typeName" /> is not found. The supplied arguments are used to resolve the type, and to bind the constructor that is used to create the instance.</returns>
        object CreateInstance(Assembly assembly, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes);

        /// <summary>
        /// Creates a delegate that can be used to invoke the specified static method.
        /// </summary>
        /// <param name="method">The method to create a delegate for.</param>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <returns>An instance of the specified delegate type that can be used to invoke the specified static method.</returns>
        Delegate CreateDelegate(MethodInfo method, Type delegateType);

        /// <summary>
        /// Creates a delegate that can be used to invoke the specified instance method on the specified target object.
        /// </summary>
        /// <param name="method">The method to create a delegate for.</param>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <param name="target">The target object to invoke the method on.</param>
        /// <returns>An instance of the specified delegate type that can be used to invoke the specified instance method on the specified target object.</returns>
        Delegate CreateDelegate(MethodInfo method, Type delegateType, object target);

        /// <summary>
        /// Invokes the constructor with the specified arguments, under the constraints of the specified <see cref="Binder"/>.
        /// </summary>
        /// <param name="constructor">The constructor to invoke.</param>
        /// <param name="invokeAttr">One of the <see cref="BindingFlags"/> values that specifies the type of binding.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="parameters">An array of type Object used to match the number, order and type of the parameters for this constructor, under the constraints of binder. If this constructor does not require parameters, pass an array with zero elements.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>An instance of the class associated with the constructor.</returns>
        object Invoke(ConstructorInfo constructor, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

        /// <summary>
        /// Invokes the specified member, using the specified binding constraints and matching the specified argument list, modifiers and culture.
        /// </summary>
        /// <param name="type">The type to invoke a member on.</param>
        /// <param name="name">The string containing the name of the constructor, method, property, or field member to invoke.</param>
        /// <param name="invokeAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on. The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="target">The object on which to invoke the specified member.</param>
        /// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the args array. A parameter's associated attributes are stored in the member's signature. The default binder processes this parameter only when calling a COM component.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> object representing the globalization locale to use, which may be necessary for locale-specific conversions, such as converting a numeric String to a Double. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <param name="namedParameters">An array containing the names of the parameters to which the values in the args array are passed.</param>
        /// <returns>An object representing the return value of the invoked member.</returns>
        object InvokeMember(Type type, string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

        /// <summary>
        /// Adds an event handler to an event source.
        /// </summary>
        /// <param name="event">The event to add a handler to.</param>
        /// <param name="target">The event source.</param>
        /// <param name="handler">Encapsulates a method or methods to be invoked when the event is raised by the target.</param>
        void AddEventHandler(EventInfo @event, object target, Delegate handler);

        /// <summary>
        /// Removes an event handler from an event source.
        /// </summary>
        /// <param name="event">The event to remove a handler from.</param>
        /// <param name="target">The event source.</param>
        /// <param name="handler">The delegate to be disassociated from the events raised by target.</param>
        void RemoveEventHandler(EventInfo @event, object target, Delegate handler);

        /// <summary>
        /// Returns a literal value associated with the field by a compiler.
        /// </summary>
        /// <param name="field">The field for which to get the literal value.</param>
        /// <returns>An object that contains the literal value associated with the field. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        object GetRawConstantValue(FieldInfo field);

        // NB: System.Reflection.FieldInfo has an asymmetry in GetValue/SetValue signatures, which is reflected here.

        /// <summary>
        /// Gets the value of a field.
        /// </summary>
        /// <param name="field">The field to get the value from.</param>
        /// <param name="obj">The object whose field value will be retrieved, or null for a static field.</param>
        /// <returns>The value of the field.</returns>
        object GetValue(FieldInfo field, object obj);

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <param name="field">The field to set the value on.</param>
        /// <param name="obj">The object whose field value will be set, or null for a static field.</param>
        /// <param name="value">The value to assign to the field.</param>
        /// <param name="invokeAttr">One of the <see cref="BindingFlags"/> values that specifies the type of binding.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        void SetValue(FieldInfo field, object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

        /// <summary>
        /// Invokes the reflected method or constructor with the given parameters.
        /// </summary>
        /// <param name="method">The method or constructor to invoke.</param>
        /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.</param>
        /// <param name="invokeAttr">One of the <see cref="BindingFlags"/> values that specifies the type of binding.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, this should be null.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>An object containing the return value of the invoked method, or null in the case of a constructor, or null if the method's return type is void.</returns>
        object Invoke(MethodBase method, object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <param name="property">The property for which to get the literal value.</param>
        /// <returns>An object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        object GetConstantValue(PropertyInfo property);

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <param name="property">The property for which to get the literal value.</param>
        /// <returns>An object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        object GetRawConstantValue(PropertyInfo property);

        /// <summary>
        /// Returns the property value of a specified object that has the specified binding, index, and culture-specific information.
        /// </summary>
        /// <param name="property">The property for which to get the value.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, and SetProperty. You must specify a suitable invocation attribute. For example, to invoke a static member, set the Static flag.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>The property value of the specified object.</returns>
        object GetValue(PropertyInfo property, object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

        /// <summary>
        /// Sets the property value for a specified object that has the specified binding, index, and culture-specific information.
        /// </summary>
        /// <param name="property">The property for which to set the value.</param>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. You must specify a suitable invocation attribute. For example, to invoke a static member, set the Static flag.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        void SetValue(PropertyInfo property, object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        /// <param name="parameter">The parameter to get the default value for.</param>
        /// <returns>The default value of the parameter, or <see cref="DBNull.Value" /> if the parameter has no default value.</returns>
        object GetDefaultValue(ParameterInfo parameter);

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        /// <param name="parameter">The parameter to get the default value for.</param>
        /// <returns>The default value of the parameter, or <see cref="DBNull.Value" /> if the parameter has no default value.</returns>
        object GetRawDefaultValue(ParameterInfo parameter);
    }
}
