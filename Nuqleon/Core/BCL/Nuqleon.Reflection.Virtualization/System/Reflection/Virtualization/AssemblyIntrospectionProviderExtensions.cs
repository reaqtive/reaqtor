// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;
using System.IO;

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="IAssemblyIntrospectionProvider"/>.
    /// </summary>
    public static class AssemblyIntrospectionProviderExtensions
    {
        /// <summary>
        /// Gets the files in the file table of an assembly manifest.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="assembly">The assembly to get the files for.</param>
        /// <returns>An array of streams that contain the files.</returns>
        public static IReadOnlyList<FileStream> GetFiles(this IAssemblyIntrospectionProvider provider, Assembly assembly) => NotNull(provider).GetFiles(assembly, getResourceModules: false);

        /// <summary>
        /// Gets all the loaded modules that are part of this assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="assembly">The assembly to get the loaded modules for.</param>
        /// <returns>An array of modules.</returns>
        public static IReadOnlyList<Module> GetLoadedModules(this IAssemblyIntrospectionProvider provider, Assembly assembly) => NotNull(provider).GetLoadedModules(assembly, getResourceModules: false);

        /// <summary>
        /// Gets all the modules that are part of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="assembly">The assembly to get the modules for.</param>
        /// <returns>An array of modules.</returns>
        public static IReadOnlyList<Module> GetModules(this IAssemblyIntrospectionProvider provider, Assembly assembly) => NotNull(provider).GetModules(assembly, getResourceModules: false);

        /// <summary>
        /// Gets an <see cref="AssemblyName" /> for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="assembly">The assembly to get the name for.</param>
        /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
        public static AssemblyName GetName(this IAssemblyIntrospectionProvider provider, Assembly assembly) => NotNull(provider).GetName(assembly, copiedName: false);

        /// <summary>
        /// Gets the <see cref="Type" /> object with the specified name in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="assembly">The assembly to get the type from.</param>
        /// <param name="name">The full name of the type. </param>
        /// <returns>An object that represents the specified class, or null if the class is not found.</returns>
        public static Type GetType(this IAssemblyIntrospectionProvider provider, Assembly assembly, string name) => NotNull(provider).GetType(assembly, name, throwOnError: false, ignoreCase: false);

        /// <summary>
        /// Gets the <see cref="Type" /> object with the specified name in the specified <paramref name="assembly"/> and optionally throws an exception if the type is not found.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="assembly">The assembly to get the type from.</param>
        /// <param name="name">The full name of the type. </param>
        /// <param name="throwOnError">true to throw an exception if the type is not found; false to return null. </param>
        /// <returns>An object that represents the specified class.</returns>
        public static Type GetType(this IAssemblyIntrospectionProvider provider, Assembly assembly, string name, bool throwOnError) => NotNull(provider).GetType(assembly, name, throwOnError, ignoreCase: false);
    }
}
