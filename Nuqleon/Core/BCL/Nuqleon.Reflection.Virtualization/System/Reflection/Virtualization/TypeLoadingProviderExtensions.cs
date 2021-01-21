// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="ITypeLoadingProvider"/>.
    /// </summary>
    public static class TypeLoadingProviderExtensions
    {
        /// <summary>
        /// Gets the <see cref="Type" /> with the specified name, performing a case-sensitive search.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="typeName">The assembly-qualified name of the type to get. See <see cref="Type.AssemblyQualifiedName" />. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
        /// <returns>The type with the specified name, if found; otherwise, null.</returns>
        public static Type GetType(this ITypeLoadingProvider provider, string typeName) => NotNull(provider).GetType(typeName, throwOnError: false, ignoreCase: false);

        /// <summary>
        /// Gets the <see cref="Type" /> with the specified name, performing a case-sensitive search and specifying whether to throw an exception if the type is not found.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="typeName">The assembly-qualified name of the type to get. See <see cref="Type.AssemblyQualifiedName" />. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
        /// <param name="throwOnError">true to throw an exception if the type cannot be found; false to return null. Specifying false also suppresses some other exception conditions, but not all of them. See the Exceptions section.</param>
        /// <returns>The type with the specified name. If the type is not found, the <paramref name="throwOnError" /> parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of <paramref name="throwOnError" />. See the Exceptions section. </returns>
        public static Type GetType(this ITypeLoadingProvider provider, string typeName, bool throwOnError) => NotNull(provider).GetType(typeName, throwOnError, ignoreCase: false);

        /// <summary>
        /// Gets the type with the specified name, optionally providing custom methods to resolve the assembly and the type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="typeName">The name of the type to get. If the <paramref name="typeResolver" /> parameter is provided, the type name can be any string that <paramref name="typeResolver" /> is capable of resolving. If the <paramref name="assemblyResolver" /> parameter is provided or if standard type resolution is used, <paramref name="typeName" /> must be an assembly-qualified name (see <see cref="System.Type.AssemblyQualifiedName" />), unless the type is in the currently executing assembly or in Mscorlib.dll, in which case it is sufficient to supply the type name qualified by its namespace.</param>
        /// <param name="assemblyResolver">A method that locates and returns the assembly that is specified in <paramref name="typeName" />. The assembly name is passed to <paramref name="assemblyResolver" /> as an <see cref="AssemblyName" /> object. If <paramref name="typeName" /> does not contain the name of an assembly, <paramref name="assemblyResolver" /> is not called. If <paramref name="assemblyResolver" /> is not supplied, standard assembly resolution is performed. Caution   Do not pass methods from unknown or untrusted callers. Doing so could result in elevation of privilege for malicious code. Use only methods that you provide or that you are familiar with. </param>
        /// <param name="typeResolver">A method that locates and returns the type that is specified by <paramref name="typeName" /> from the assembly that is returned by <paramref name="assemblyResolver" /> or by standard assembly resolution. If no assembly is provided, the <paramref name="typeResolver" /> method can provide one. The method also takes a parameter that specifies whether to perform a case-insensitive search; false is passed to that parameter. Caution   Do not pass methods from unknown or untrusted callers. </param>
        /// <returns>The type with the specified name, or null if the type is not found. </returns>
        public static Type GetType(this ITypeLoadingProvider provider, string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver) => NotNull(provider).GetType(typeName, assemblyResolver, typeResolver, throwOnError: false, ignoreCase: false);

        /// <summary>
        /// Gets the type with the specified name, specifying whether to throw an exception if the type is not found, and optionally providing custom methods to resolve the assembly and the type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="typeName">The name of the type to get. If the <paramref name="typeResolver" /> parameter is provided, the type name can be any string that <paramref name="typeResolver" /> is capable of resolving. If the <paramref name="assemblyResolver" /> parameter is provided or if standard type resolution is used, <paramref name="typeName" /> must be an assembly-qualified name (see <see cref="System.Type.AssemblyQualifiedName" />), unless the type is in the currently executing assembly or in Mscorlib.dll, in which case it is sufficient to supply the type name qualified by its namespace.</param>
        /// <param name="assemblyResolver">A method that locates and returns the assembly that is specified in <paramref name="typeName" />. The assembly name is passed to <paramref name="assemblyResolver" /> as an <see cref="AssemblyName" /> object. If <paramref name="typeName" /> does not contain the name of an assembly, <paramref name="assemblyResolver" /> is not called. If <paramref name="assemblyResolver" /> is not supplied, standard assembly resolution is performed. Caution   Do not pass methods from unknown or untrusted callers. Doing so could result in elevation of privilege for malicious code. Use only methods that you provide or that you are familiar with. </param>
        /// <param name="typeResolver">A method that locates and returns the type that is specified by <paramref name="typeName" /> from the assembly that is returned by <paramref name="assemblyResolver" /> or by standard assembly resolution. If no assembly is provided, the method can provide one. The method also takes a parameter that specifies whether to perform a case-insensitive search; false is passed to that parameter. Caution   Do not pass methods from unknown or untrusted callers. </param>
        /// <param name="throwOnError">true to throw an exception if the type cannot be found; false to return null. Specifying false also suppresses some other exception conditions, but not all of them. See the Exceptions section.</param>
        /// <returns>The type with the specified name. If the type is not found, the <paramref name="throwOnError" /> parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of <paramref name="throwOnError" />. See the Exceptions section. </returns>
        public static Type GetType(this ITypeLoadingProvider provider, string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError) => NotNull(provider).GetType(typeName, assemblyResolver, typeResolver, throwOnError, ignoreCase: false);
    }
}
