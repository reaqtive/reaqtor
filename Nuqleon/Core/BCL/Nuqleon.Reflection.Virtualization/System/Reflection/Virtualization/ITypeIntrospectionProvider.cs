// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to introspect <see cref="Type"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface ITypeIntrospectionProvider : IMemberInfoIntrospectionProvider, IReflectionTypeSystemProvider
    {
        // NB: Enum-related stuff on System.Type can be virtualized separately as "Enum virtualization",
        //     where the bulk of the functionality is exposed anyway.

        /// <summary>
        /// Returns an array of <see cref="Type" /> objects representing a filtered list of interfaces implemented or inherited by the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to find interfaces for.</param>
        /// <param name="filter">The delegate that compares the interfaces against <paramref name="filterCriteria" />.</param>
        /// <param name="filterCriteria">The search criteria that determines whether an interface should be included in the returned array.</param>
        /// <returns>An array of <see cref="Type" /> objects representing a filtered list of the interfaces implemented or inherited by the specified <paramref name="type"/>, or an empty array of type <see cref="Type" /> if no interfaces matching the filter are implemented or inherited by the specified <paramref name="type"/>.</returns>
        IReadOnlyList<Type> FindInterfaces(Type type, TypeFilter filter, object filterCriteria);

        /// <summary>
        /// Returns a filtered array of <see cref="MemberInfo" /> objects of the specified member type.
        /// </summary>
        /// <param name="type">The type to find members in.</param>
        /// <param name="memberType">An object that indicates the type of member to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.-or- Zero, to return null.</param>
        /// <param name="filter">The delegate that does the comparisons, returning true if the member currently being inspected matches the <paramref name="filterCriteria" /> and false otherwise. You can use the FilterAttribute, FilterName, and FilterNameIgnoreCase delegates supplied by this class. The first uses the fields of FieldAttributes, MethodAttributes, and MethodImplAttributes as search criteria, and the other two delegates use String objects as the search criteria.</param>
        /// <param name="filterCriteria">The search criteria that determines whether a member is returned in the array of MemberInfo objects.The fields of FieldAttributes, MethodAttributes, and MethodImplAttributes can be used in conjunction with the FilterAttribute delegate supplied by this class.</param>
        /// <returns>A filtered array of <see cref="MemberInfo" /> objects of the specified member type.-or- An empty array of type <see cref="MemberInfo" />, if the specified <paramref name="type"/> does not have members of type <paramref name="memberType" /> that match the filter criteria.</returns>
        IReadOnlyList<MemberInfo> FindMembers(Type type, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria);

        /// <summary>
        /// Gets the number of dimensions in an <see cref="Array" />.
        /// </summary>
        /// <param name="type">The type to get the rank for.</param>
        /// <returns>An <see cref="int" /> containing the number of dimensions in the specified <paramref name="type"/>.</returns>
        int GetArrayRank(Type type);

        /// <summary>
        /// Searches for the members defined for the specified <paramref name="type"/> whose <see cref="DefaultMemberAttribute" /> is set.
        /// </summary>
        /// <param name="type">The type to get the default members for.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all default members of the specified <paramref name="type"/>.-or- An empty array of type <see cref="MemberInfo" />, if the <paramref name="type"/> does not have default members.</returns>
        IReadOnlyList<MemberInfo> GetDefaultMembers(Type type);

        /// <summary>
        /// Gets the <see cref="Assembly" /> in which the specified <paramref name="type"/> is declared. For generic types, gets the <see cref="Assembly" /> in which the generic type is defined.
        /// </summary>
        /// <param name="type">The type to get the declaring assembly for.</param>
        /// <returns>An <see cref="Assembly" /> instance that describes the assembly containing the specified <paramref name="type"/>. For generic types, the instance describes the assembly that contains the generic type definition, not the assembly that creates and uses a particular constructed type.</returns>
        Assembly GetAssembly(Type type);

        /// <summary>
        /// Gets the assembly-qualified name of the specified <paramref name="type"/>, which includes the name of the assembly from which the specified <paramref name="type"/> was loaded.
        /// </summary>
        /// <param name="type">The type to get the assembly-qualified name for.</param>
        /// <returns>The assembly-qualified name of the specified <paramref name="type"/>, which includes the name of the assembly from which the specified <paramref name="type"/> was loaded, or null if the current instance represents a generic type parameter.</returns>
        string GetAssemblyQualifiedName(Type type);

        /// <summary>
        /// Gets the attributes associated with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the attributes for.</param>
        /// <returns>A <see cref="TypeAttributes" /> object representing the attribute set of the specified <paramref name="type"/>, unless the specified <paramref name="type"/> represents a generic type parameter, in which case the value is unspecified.</returns>
        TypeAttributes GetAttributes(Type type);

        /// <summary>
        /// Gets the type from which the specified <paramref name="type"/> directly inherits.
        /// </summary>
        /// <param name="type">The type to get the base type for.</param>
        /// <returns>The <see cref="Type" /> from which the specified <paramref name="type"/> directly inherits, or null if the type represents the <see cref="object" /> class or an interface.</returns>
        Type GetBaseType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> object has type parameters that have not been replaced by specific types.
        /// </summary>
        /// <param name="type">The type to check for generic parameters.</param>
        /// <returns>true if the specified <paramref name="type"/> object is itself a generic type parameter or has type parameters for which specific types have not been supplied; otherwise, false.</returns>
        bool ContainsGenericParameters(Type type);

        /// <summary>
        /// Gets a <see cref="MethodBase" /> that represents the declaring method, if the specified <paramref name="type"/> represents a type parameter of a generic method.
        /// </summary>
        /// <param name="type">The type to get the declaring method for.</param>
        /// <returns>If the specified <paramref name="type"/> represents a type parameter of a generic method, a <see cref="MethodBase" /> that represents declaring method; otherwise, null.</returns>
        MethodBase GetDeclaringMethod(Type type);

        /// <summary>
        /// Gets the type that declares the current nested type or generic type parameter.
        /// </summary>
        /// <param name="type">The type to get the declaring type for.</param>
        /// <returns>A <see cref="Type" /> object representing the enclosing type, if the specified <paramref name="type"/> is a nested type; or the generic type definition, if the specified <paramref name="type"/> is a type parameter of a generic type; or the type that declares the generic method, if the specified <paramref name="type"/> is a type parameter of a generic method; otherwise, null.</returns>
        Type GetDeclaringType(Type type);

        /// <summary>
        /// Gets the fully qualified name of the specified <paramref name="type"/>, including the namespace of the <see cref="Type" /> but not the assembly.
        /// </summary>
        /// <param name="type">The type to get the full name for.</param>
        /// <returns>The fully qualified name of the specified <paramref name="type"/>, including the namespace of the <see cref="Type" /> but not the assembly; or null if the specified <paramref name="type"/> represents a generic type parameter, an array type, pointer type, or byref type based on a type parameter, or a generic type that is not a generic type definition but contains unresolved type parameters.</returns>
        string GetFullName(Type type);

        /// <summary>
        /// Gets a combination of <see cref="GenericParameterAttributes" /> flags that describe the covariance and special constraints of the specified generic type parameter.
        /// </summary>
        /// <param name="type">The type to get the generic parameter attributes for.</param>
        /// <returns>A bitwise combination of <see cref="GenericParameterAttributes" /> values that describes the covariance and special constraints of the specified generic type parameter.</returns>
        GenericParameterAttributes GetGenericParameterAttributes(Type type);

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that declared the parameter, when the specified <paramref name="type"/> represents a type parameter of a generic type or a generic method.
        /// </summary>
        /// <param name="type">The type to get the generic parameter position for.</param>
        /// <returns>The position of a type parameter in the type parameter list of the generic type or method that defines the parameter. Position numbers begin at 0.</returns>
        int GetGenericParameterPosition(Type type);

        /// <summary>
        /// Gets the GUID associated with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the associated GUID for.</param>
        /// <returns>The GUID associated with the specified <paramref name="type"/>.</returns>
        Guid GetGuid(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> encompasses or refers to another type; that is, whether the specified <paramref name="type"/> is an array, a pointer, or is passed by reference.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an array, a pointer, or is passed by reference; otherwise, false.</returns>
        bool HasElementType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a COM object.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a COM object; otherwise, false.</returns>
        bool IsCOMObject(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a constructed generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a constructed generic type; otherwise, false.</returns>
        bool IsConstructedGenericType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> can be hosted in a context.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> can be hosted in a context; otherwise, false.</returns>
        bool IsContextful(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> represents a type parameter in the definition of a generic type or method.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> object represents a type parameter of a generic type definition or generic method definition; otherwise, false.</returns>
        bool IsGenericParameter(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a generic type; otherwise, false.</returns>
        bool IsGenericType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> represents a generic type definition, from which other generic types can be constructed.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> object represents a generic type definition; otherwise, false.</returns>
        bool IsGenericTypeDefinition(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is marshaled by reference.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is marshaled by reference; otherwise, false.</returns>
        bool IsMarshalByRef(Type type);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="type"/> is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is security-critical or security-safe-critical at the current trust level; false if it is transparent.</returns>
        bool IsSecurityCritical(Type type);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="type"/> is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is security-safe-critical at the current trust level; false if it is security-critical or transparent.</returns>
        bool IsSecuritySafeCritical(Type type);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="type"/> is transparent at the current trust level, and therefore cannot perform critical operations.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is security-transparent at the current trust level; otherwise, false.</returns>
        bool IsSecurityTransparent(Type type);

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Type" /> is serializable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is serializable; otherwise, false.</returns>
        bool IsSerializable(Type type);

        /// <summary>
        /// Gets the module in which the specified <paramref name="type"/> is defined.
        /// </summary>
        /// <param name="type">The type to get the module for.</param>
        /// <returns>The module in which the specified <paramref name="type"/> is defined.</returns>
        Module GetModule(Type type);

        /// <summary>
        /// Gets the namespace of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the namespace for.</param>
        /// <returns>The namespace of the specified <paramref name="type"/>; null if the type has no namespace or represents a generic parameter.</returns>
        string GetNamespace(Type type);

        /// <summary>
        /// Gets a <see cref="StructLayoutAttribute" /> that describes the layout of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the struct layout attribute for.</param>
        /// <returns>A <see cref="StructLayoutAttribute" /> that describes the gross layout features of the specified <paramref name="type"/>.</returns>
        StructLayoutAttribute GetStructLayoutAttribute(Type type);

        /// <summary>
        /// Gets the handle for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the runtime handle for.</param>
        /// <returns>The handle for the specified <paramref name="type"/>.</returns>
        RuntimeTypeHandle GetTypeHandle(Type type);

        /// <summary>
        /// Indicates the type provided by the common language runtime that represents the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the underlying type for.</param>
        /// <returns>The underlying system type for the specified <paramref name="type"/>.</returns>
        Type GetUnderlyingSystemType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is an array.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an array; otherwise, false.</returns>
        bool IsArray(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is passed by reference.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a passed by reference; otherwise, false.</returns>
        bool IsByRef(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is an interface type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an interface type; otherwise, false.</returns>
        bool IsInterface(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a pointer.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a pointer; otherwise, false.</returns>
        bool IsPointer(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is one of the primitive types.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is one of the primitive types; otherwise, false.</returns>
        bool IsPrimitive(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a value type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a value type; otherwise, false.</returns>
        bool IsValueType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> can be accessed by code outside the assembly.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a public type or a public nested type such that all the enclosing types are public; otherwise, false.</returns>
        bool IsVisible(Type type);

        /// <summary>
        /// Gets the element type of an array, by-ref, or pointer type.
        /// </summary>
        /// <param name="type">The type to get the element type for.</param>
        /// <returns>The element type of the specified type.</returns>
        Type GetElementType(Type type);

        /// <summary>
        /// Gets the generic type definition of the specified generic type.
        /// </summary>
        /// <param name="type">The type to get the generic type definition for.</param>
        /// <returns>The generic type definition of the specified type.</returns>
        Type GetGenericTypeDefinition(Type type);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the constraints on the generic type parameter.
        /// </summary>
        /// <param name="type">The generic parameter type to get the constraints for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the constraints on the specified generic type parameter.</returns>
        IReadOnlyList<Type> GetGenericParameterConstraints(Type type);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic type or the type parameters of a generic type definition.
        /// </summary>
        /// <param name="type">The type to get the generic arguments for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic type. Returns an empty array if the specified type is not a generic type.</returns>
        IReadOnlyList<Type> GetGenericArguments(Type type);

        /// <summary>
        /// Searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
        /// </summary>
        /// <param name="type">The type to get the interface for.</param>
        /// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
        /// <param name="ignoreCase">true to ignore the case of that part of <paramref name="name" /> that specifies the simple interface name (the part that specifies the namespace must be correctly cased).-or- false to perform a case-sensitive search for all parts of <paramref name="name" />.</param>
        /// <returns>An object representing the interface with the specified name, implemented or inherited by the specified type, if found; otherwise, null.</returns>
        Type GetInterface(Type type, string name, bool ignoreCase);

        /// <summary>
        /// Gets all the interfaces implemented or inherited by the specified type.
        /// </summary>
        /// <param name="type">The type to get the interfaces for.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all the interfaces implemented or inherited by the specified type.-or- An empty array of type <see cref="Type"/>, if no interfaces are implemented or inherited by the specified type.</returns>
        IReadOnlyList<Type> GetInterfaces(Type type);

        /// <summary>
        /// Returns an interface mapping for the specified interface type.
        /// </summary>
        /// <param name="type">The type to get the interface mapping for.</param>
        /// <param name="interfaceType">The interface type to retrieve a mapping for.</param>
        /// <returns>An object that represents the interface mapping for <paramref name="interfaceType" /></returns>
        InterfaceMapping GetInterfaceMap(Type type, Type interfaceType);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="type">The type to get the constructor for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.-or- An empty array of the type <see cref="Type"/> to get a constructor that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
        ConstructorInfo GetConstructor(Type type, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for the constructors defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the constructors for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="ConstructorInfo" /> objects representing all constructors defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="ConstructorInfo" />, if no constructors are defined for the specified type, or if none of the defined constructors match the binding constraints.</returns>
        IReadOnlyList<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for an event with the specified name, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the event for.</param>
        /// <param name="name">The name of the event to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the event that matches the specified requirements, if found; otherwise, null.</returns>
        EventInfo GetEvent(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for events that are declared or inherited by the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the events for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="EventInfo" /> objects representing all events defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="EventInfo" />, if no events are defined for the specified type, or if none of the defined events match the binding constraints.</returns>
        IReadOnlyList<EventInfo> GetEvents(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for a field with the specified name, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the field for.</param>
        /// <param name="name">The name of the field to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the field that matches the specified requirements, if found; otherwise, null.</returns>
        FieldInfo GetField(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for fields defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the fields for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="FieldInfo" /> objects representing all fields defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="FieldInfo" />, if no fields are defined for the specified type, or if none of the defined fields match the binding constraints.</returns>
        IReadOnlyList<FieldInfo> GetFields(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for the specified members of the specified member type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="name">The string containing the name of the members to get.</param>
        /// <param name="memberTypes">The types of members to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing the members with the specified name, if found; otherwise, an empty array.</returns>
        IReadOnlyList<MemberInfo> GetMember(Type type, string name, MemberTypes memberTypes, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for members defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all members defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="MemberInfo" />, if no members are defined for the specified type, or if none of the defined members match the binding constraints.</returns>
        IReadOnlyList<MemberInfo> GetMembers(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.-or- An empty array of the type <see cref="Type"/> to get a method that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for methods defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the methods for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing all methods defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="MethodInfo" />, if no methods are defined for the specified type, or if none of the defined methods match the binding constraints.</returns>
        IReadOnlyList<MethodInfo> GetMethods(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for a nested type with the specified name, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the nested type for.</param>
        /// <param name="name">The name of the nested type to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the nested type that matches the specified requirements, if found; otherwise, null.</returns>
        Type GetNestedType(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for types nested in the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the nested types for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all nested types defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="Type"/>, if no nested types are defined for the specified type, or if none of the nested types methods match the binding constraints.</returns>
        IReadOnlyList<Type> GetNestedTypes(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for the specified property whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.-or- An empty array of the type <see cref="Type"/> to get a property that is not indexed.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        PropertyInfo GetProperty(Type type, string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for properties of for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the properties for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="PropertyInfo" /> objects representing all properties defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="PropertyInfo" />, if no properties are defined for the specified type, or if none of the defined properties match the binding constraints.</returns>
        IReadOnlyList<PropertyInfo> GetProperties(Type type, BindingFlags bindingAttr);
    }
}
