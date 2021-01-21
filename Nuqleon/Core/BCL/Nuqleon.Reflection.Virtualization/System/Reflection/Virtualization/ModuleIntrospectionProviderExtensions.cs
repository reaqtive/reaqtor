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
    /// Provides a set of extension methods for <see cref="IModuleIntrospectionProvider"/>.
    /// </summary>
    public static class ModuleIntrospectionProviderExtensions
    {
        /// <summary>
        /// Returns a field having the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <param name="name">The field name.</param>
        /// <returns>A <see cref="FieldInfo"/> object having the specified name, or null if the field does not exist.</returns>
        public static FieldInfo GetField(this IModuleIntrospectionProvider provider, Module module, string name) => NotNull(provider).GetField(module, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns the global fields defined on the module.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <returns>An array of <see cref="FieldInfo" /> objects representing the global fields defined on the module; if there are no global fields, an empty array is returned.</returns>
        public static IReadOnlyList<FieldInfo> GetFields(this IModuleIntrospectionProvider provider, Module module) => NotNull(provider).GetFields(module, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns a method having the specified name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <param name="name">The method name.</param>
        /// <returns>A <see cref="MethodInfo"/> object having the specified name, or null if the method does not exist.</returns>
        public static MethodInfo GetMethod(this IModuleIntrospectionProvider provider, Module module, string name) => NotNull(provider).GetMethod(module, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, CallingConventions.Any, types: null, modifiers: null);

        /// <summary>
        /// Returns a method having the specified name and parameter types.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <param name="name">The method name.</param>
        /// <param name="types">The parameter types to search for.</param>
        /// <returns>A <see cref="MethodInfo"/> object in accordance with the specified criteria, or null if the method does not exist.</returns>
        public static MethodInfo GetMethod(this IModuleIntrospectionProvider provider, Module module, string name, Type[] types) => NotNull(provider).GetMethod(module, name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, binder: null, CallingConventions.Any, types, modifiers: null);

        /// <summary>
        /// Returns the global methods defined on the module.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing all the global methods defined on the module; if there are no global methods, an empty array is returned.</returns>
        public static IReadOnlyList<MethodInfo> GetMethods(this IModuleIntrospectionProvider provider, Module module) => NotNull(provider).GetMethods(module, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Returns the specified type, searching the module with the specified case sensitivity.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <param name="ignoreCase">true for case-insensitive search; otherwise, false.</param>
        /// <returns>A <see cref="Type"/> object representing the given type, if the type is in this module; otherwise, null.</returns>
        public static Type GetType(this IModuleIntrospectionProvider provider, Module module, string className, bool ignoreCase) => NotNull(provider).GetType(module, className, throwOnError: false, ignoreCase);

        /// <summary>
        /// Returns the specified type, performing a case-sensitive search.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to search in.</param>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <returns>A <see cref="Type"/> object representing the given type, if the type is in this module; otherwise, null.</returns>
        public static Type GetType(this IModuleIntrospectionProvider provider, Module module, string className) => NotNull(provider).GetType(module, className, throwOnError: false, ignoreCase: false);

        /// <summary>
        /// Returns the field identified by the specified metadata token.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field that is identified by the specified metadata token.</returns>
        public static FieldInfo ResolveField(this IModuleIntrospectionProvider provider, Module module, int metadataToken) => NotNull(provider).ResolveField(module, metadataToken, genericTypeArguments: null, genericMethodArguments: null);

        /// <summary>
        /// Returns the type or member identified by the specified metadata token.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <returns>A <see cref="MemberInfo" /> object representing the type or member that is identified by the specified metadata token.</returns>
        public static MemberInfo ResolveMember(this IModuleIntrospectionProvider provider, Module module, int metadataToken) => NotNull(provider).ResolveMember(module, metadataToken, genericTypeArguments: null, genericMethodArguments: null);

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <returns>A <see cref="MethodBase" /> object representing the method or constructor that is identified by the specified metadata token.</returns>
        public static MethodBase ResolveMethod(this IModuleIntrospectionProvider provider, Module module, int metadataToken) => NotNull(provider).ResolveMethod(module, metadataToken, genericTypeArguments: null, genericMethodArguments: null);

        /// <summary>
        /// Returns the type identified by the specified metadata token.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
        /// <returns>A <see cref="Type" /> object representing the type that is identified by the specified metadata token.</returns>
        public static Type ResolveType(this IModuleIntrospectionProvider provider, Module module, int metadataToken) => NotNull(provider).ResolveType(module, metadataToken, genericTypeArguments: null, genericMethodArguments: null);
    }
}
