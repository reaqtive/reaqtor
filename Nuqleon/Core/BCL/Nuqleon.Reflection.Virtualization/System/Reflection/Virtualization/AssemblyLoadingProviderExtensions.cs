// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

#if NET472
using System.Security;
#endif

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="IAssemblyLoadingProvider"/>.
    /// </summary>
    public static class AssemblyLoadingProviderExtensions
    {
#if NET472
        /// <summary>
        /// Loads the assembly with a common object file format (COFF)-based image containing an emitted assembly. The assembly is loaded into the application domain of the caller.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly. </param>
        /// <returns>The loaded assembly.</returns>
        public static Assembly Load(this IAssemblyLoadingProvider provider, byte[] rawAssembly) => NotNull(provider).Load(rawAssembly, null, SecurityContextSource.CurrentAssembly);

        /// <summary>
        /// Loads the assembly with a common object file format (COFF)-based image containing an emitted assembly, optionally including symbols for the assembly. The assembly is loaded into the application domain of the caller.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly. </param>
        /// <param name="rawSymbolStore">A byte array that contains the raw bytes representing the symbols for the assembly. </param>
        /// <returns>The loaded assembly.</returns>
        public static Assembly Load(this IAssemblyLoadingProvider provider, byte[] rawAssembly, byte[] rawSymbolStore) => NotNull(provider).Load(rawAssembly, rawSymbolStore, SecurityContextSource.CurrentAssembly);
#endif

        /// <summary>
        /// Loads an assembly given its file name or path.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly. </param>
        /// <returns>The loaded assembly.</returns>
        public static Assembly LoadFrom(this IAssemblyLoadingProvider provider, string assemblyFile) => NotNull(provider).LoadFrom(assemblyFile, hashValue: null, System.Configuration.Assemblies.AssemblyHashAlgorithm.None);
    }
}
