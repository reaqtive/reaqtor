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
    /// Provides a set of extension methods for <see cref="IAssemblyLoadingProvider"/>.
    /// </summary>
    public static class AssemblyLoadingProviderExtensions
    {

        /// <summary>
        /// Loads an assembly given its file name or path.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly. </param>
        /// <returns>The loaded assembly.</returns>
        public static Assembly LoadFrom(this IAssemblyLoadingProvider provider, string assemblyFile) => NotNull(provider).LoadFrom(assemblyFile, hashValue: null, System.Configuration.Assemblies.AssemblyHashAlgorithm.None);
    }
}
