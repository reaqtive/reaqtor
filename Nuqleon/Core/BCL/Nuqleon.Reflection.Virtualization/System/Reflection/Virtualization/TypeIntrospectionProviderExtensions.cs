// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="ITypeIntrospectionProvider"/>.
    /// </summary>
    public static class TypeIntrospectionProviderExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is abstract and must be overridden.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is abstract; otherwise, false.</returns>
        public static bool IsAbstract(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.Abstract) > TypeAttributes.NotPublic;

        /// <summary>
        /// Gets a value indicating whether the string format attribute AnsiClass is selected for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the string format attribute AnsiClass is selected for the <paramref name="type"/>; otherwise, false.</returns>
        public static bool IsAnsiClass(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.StringFormatMask) == TypeAttributes.AnsiClass;

        /// <summary>
        /// Gets a value indicating whether the string format attribute AutoClass is selected for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the string format attribute AutoClass is selected for the <paramref name="type"/>; otherwise, false.</returns>
        public static bool IsAutoClass(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.StringFormatMask) == TypeAttributes.AutoClass;

        /// <summary>
        /// Gets a value indicating whether the class layout attribute AutoLayout is selected for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the class layout attribute AutoLayout is selected for the <paramref name="type"/>; otherwise, false.</returns>
        public static bool IsAutoLayout(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a class; that is, not a value type or interface.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a class; otherwise, false.</returns>
        public static bool IsClass(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Class && !NotNull(provider).IsValueType(type);

        /// <summary>
        /// Gets a value indicating whether the class layout attribute ExplicitLayout is selected for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the class layout attribute ExplicitLayout is selected for the <paramref name="type"/>; otherwise, false.</returns>
        public static bool IsExplicitLayout(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> has a <see cref="System.Runtime.InteropServices.ComImportAttribute" /> attribute applied, indicating that it was imported from a COM type library.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> has a <see cref="System.Runtime.InteropServices.ComImportAttribute" />; otherwise, false.</returns>
        public static bool IsImport(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.Import) > TypeAttributes.NotPublic;

        /// <summary>
        /// Gets a value indicating whether the class layout attribute SequentialLayout is selected for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the class layout attribute SequentialLayout is selected for the <paramref name="type"/>; otherwise, false.</returns>
        public static bool IsLayoutSequential(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> object represents a type whose definition is nested inside the definition of another type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested inside another type; otherwise, false.</returns>
        public static bool IsNested(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetDeclaringType(type) != null;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is nested and visible only within its own assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested and visible only within its own assembly; otherwise, false.</returns>
        public static bool IsNestedAssembly(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is nested and visible only to classes that belong to both its own family and its own assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested and visible only to classes that belong to both its own family and its own assembly; otherwise, false.</returns>
        public static bool IsNestedFamANDAssem(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is nested and visible only within its own family.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested and visible only within its own family; otherwise, false.</returns>
        public static bool IsNestedFamily(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is nested and visible only to classes that belong to either its own family or to its own assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested and visible only to classes that belong to either its own family or to its own assembly; otherwise, false.</returns>
        public static bool IsNestedFamORAssem(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamORAssem;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is nested and declared private.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested and declared private; otherwise, false.</returns>
        public static bool IsNestedPrivate(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is nested and declared public.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is nested and declared public; otherwise, false.</returns>
        public static bool IsNestedPublic(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is not declared public.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is not declared public; otherwise, false.</returns>
        public static bool IsNotPublic(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is declared public.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is declared public; otherwise, false.</returns>
        public static bool IsPublic(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.VisibilityMask) == TypeAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is declared sealed.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is declared sealed; otherwise, false.</returns>
        public static bool IsSealed(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.Sealed) > TypeAttributes.NotPublic;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> has a name that requires special handling.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> has a name that requires special handling; otherwise, false.</returns>
        public static bool IsSpecialName(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.SpecialName) > TypeAttributes.NotPublic;

        /// <summary>
        /// Gets a value indicating whether the string format attribute UnicodeClass is selected for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the string format attribute UnicodeClass is selected for the <paramref name="type"/>; otherwise, false.</returns>
        public static bool IsUnicodeClass(this ITypeIntrospectionProvider provider, Type type) => (NotNull(provider).GetAttributes(type) & TypeAttributes.StringFormatMask) == TypeAttributes.UnicodeClass;

        /// <summary>
        /// Gets the initializer for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the initializer for.</param>
        /// <returns>A <see cref="ConstructorInfo" /> representing the class constructor for the specified <paramref name="type"/>.</returns>
        public static ConstructorInfo TypeInitializer(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetConstructor(type, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, Type.EmptyTypes, modifiers: null);

        /// <summary>
        /// Searches for the interface with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the interface for.</param>
        /// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
        /// <returns>An object representing the interface with the specified name, implemented or inherited by the specified type, if found; otherwise, null.</returns>
        public static Type GetInterface(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetInterface(type, name, ignoreCase: false);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the constructor for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.-or- An empty array of the type <see cref="Type"/> to get a constructor that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
        public static ConstructorInfo GetConstructor(this ITypeIntrospectionProvider provider, Type type, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) => NotNull(NotNull(provider)).GetConstructor(type, bindingAttr, binder, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for a public instance constructor whose parameters match the specified argument types.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the constructor for.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.-or- An empty array of the type <see cref="Type"/> to get a constructor that takes no parameters.</param>
        /// <returns>An object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
        public static ConstructorInfo GetConstructor(this ITypeIntrospectionProvider provider, Type type, Type[] types) => NotNull(NotNull(provider)).GetConstructor(type, BindingFlags.Instance | BindingFlags.Public, binder: null, types, modifiers: null);

        /// <summary>
        /// Searches for a public event with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the event for.</param>
        /// <param name="name">The name of the event to search for.</param>
        /// <returns>An object representing the event that matches the specified requirements, if found; otherwise, null.</returns>
        public static EventInfo GetEvent(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetEvent(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Searches for a public field with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the field for.</param>
        /// <param name="name">The name of the field to search for.</param>
        /// <returns>An object representing the field that matches the specified requirements, if found; otherwise, null.</returns>
        public static FieldInfo GetField(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetField(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Searches for the public members with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="name">The string containing the name of the members to get.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing the members with the specified name, if found; otherwise, an empty array.</returns>
        public static IReadOnlyList<MemberInfo> GetMember(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetMember(type, name, MemberTypes.All, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Searches for the specified members, using the specified binding constraints.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="name">The string containing the name of the members to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing the members with the specified name, if found; otherwise, an empty array.</returns>
        public static IReadOnlyList<MemberInfo> GetMember(this ITypeIntrospectionProvider provider, Type type, string name, BindingFlags bindingAttr) => NotNull(NotNull(provider)).GetMember(type, name, MemberTypes.All, bindingAttr);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.-or- An empty array of the type <see cref="Type"/> to get a method that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public static MethodInfo GetMethod(this ITypeIntrospectionProvider provider, Type type, string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) => NotNull(NotNull(provider)).GetMethod(type, name, bindingAttr, binder, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types and modifiers.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.-or- An empty array of the type <see cref="Type"/> to get a method that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public static MethodInfo GetMethod(this ITypeIntrospectionProvider provider, Type type, string name, Type[] types, ParameterModifier[] modifiers) => NotNull(NotNull(provider)).GetMethod(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.-or- An empty array of the type <see cref="Type"/> to get a method that takes no parameters.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public static MethodInfo GetMethod(this ITypeIntrospectionProvider provider, Type type, string name, Type[] types) => NotNull(NotNull(provider)).GetMethod(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, CallingConventions.Any, types, modifiers: null);

        /// <summary>
        /// Searches for the specified method, using the specified binding constraints.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public static MethodInfo GetMethod(this ITypeIntrospectionProvider provider, Type type, string name, BindingFlags bindingAttr) => NotNull(NotNull(provider)).GetMethod(type, name, bindingAttr, binder: null, CallingConventions.Any, types: null, modifiers: null);

        /// <summary>
        /// Searches for the public method with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public static MethodInfo GetMethod(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetMethod(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, CallingConventions.Any, types: null, modifiers: null);

        /// <summary>
        /// Searches for a public nested type with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the nested type for.</param>
        /// <param name="name">The name of the nested type to search for.</param>
        /// <returns>An object representing the nested type that matches the specified requirements, if found; otherwise, null.</returns>
        public static Type GetNestedType(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetNestedType(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types and modifiers.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.-or- An empty array of the type <see cref="Type"/> to get a property that is not indexed.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this ITypeIntrospectionProvider provider, Type type, string name, Type returnType, Type[] types, ParameterModifier[] modifiers) => NotNull(NotNull(provider)).GetProperty(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, returnType, types, modifiers);

        /// <summary>
        /// Searches for the specified property, using the specified binding constraints.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this ITypeIntrospectionProvider provider, Type type, string name, BindingFlags bindingAttr) => NotNull(NotNull(provider)).GetProperty(type, name, bindingAttr, binder: null, returnType: null, types: null, modifiers: null);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.-or- An empty array of the type <see cref="Type"/> to get a property that is not indexed.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this ITypeIntrospectionProvider provider, Type type, string name, Type returnType, Type[] types) => NotNull(NotNull(provider)).GetProperty(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, returnType, types, modifiers: null);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.-or- An empty array of the type <see cref="Type"/> to get a property that is not indexed.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this ITypeIntrospectionProvider provider, Type type, string name, Type[] types) => NotNull(NotNull(provider)).GetProperty(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, returnType: null, types, modifiers: null);

        /// <summary>
        /// Searches for the specified public property with the specified name and return type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this ITypeIntrospectionProvider provider, Type type, string name, Type returnType) => NotNull(NotNull(provider)).GetProperty(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, returnType, types: null, modifiers: null);

        /// <summary>
        /// Searches for the specified public property with the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public static PropertyInfo GetProperty(this ITypeIntrospectionProvider provider, Type type, string name) => NotNull(NotNull(provider)).GetProperty(type, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, returnType: null, types: null, modifiers: null);

        /// <summary>
        /// Returns all the public the constructors defined for the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the constructors for.</param>
        /// <returns>An array of <see cref="ConstructorInfo" /> objects representing all the public constructors defined for the specified type.-or- An empty array of type <see cref="ConstructorInfo" />, if no public constructors are defined for the specified type, or if none of the defined constructors match the binding constraints.</returns>
        public static IReadOnlyList<ConstructorInfo> GetConstructors(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetConstructors(type, BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// Returns all the public events that are declared or inherited by the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the events for.</param>
        /// <returns>An array of <see cref="EventInfo" /> objects representing all the public events defined for the specified type.-or- An empty array of type <see cref="EventInfo" />, if no public events are defined for the specified type, or if none of the defined events match the binding constraints.</returns>
        public static IReadOnlyList<EventInfo> GetEvents(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetEvents(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns all the public fields defined for the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the fields for.</param>
        /// <returns>An array of <see cref="FieldInfo" /> objects representing all the public fields defined for the specified type.-or- An empty array of type <see cref="FieldInfo" />, if no public fields are defined for the specified type.</returns>
        public static IReadOnlyList<FieldInfo> GetFields(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetFields(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns all the public members defined for the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the members for.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all the public members defined for the specified type.-or- An empty array of type <see cref="MemberInfo" />, if no public members are defined for the specified type.</returns>
        public static IReadOnlyList<MemberInfo> GetMembers(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetMembers(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns all the public methods defined for the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the methods for.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing all the public methods defined for the specified type.-or- An empty array of type <see cref="MethodInfo" />, if no public methods are defined for the specified type.</returns>
        public static IReadOnlyList<MethodInfo> GetMethods(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetMethods(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns all the public types nested in the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the nested types for.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all the public nested types defined for the specified type.-or- An empty array of type <see cref="Type"/>, if no public nested types are defined for the specified type.</returns>
        public static IReadOnlyList<Type> GetNestedTypes(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetNestedTypes(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns all the public properties of for the specified type.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="type">The type to get the properties for.</param>
        /// <returns>An array of <see cref="PropertyInfo" /> objects representing all the public properties defined for the specified type.-or- An empty array of type <see cref="PropertyInfo" />, if no public properties are defined for the specified type.</returns>
        public static IReadOnlyList<PropertyInfo> GetProperties(this ITypeIntrospectionProvider provider, Type type) => NotNull(NotNull(provider)).GetProperties(type, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
    }
}
