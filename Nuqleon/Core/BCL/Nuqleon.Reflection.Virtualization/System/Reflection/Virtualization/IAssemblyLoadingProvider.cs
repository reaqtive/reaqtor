// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.CodeDom.Compiler;

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to load <see cref="Assembly"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IAssemblyLoadingProvider
    {
        /// <summary>
        /// Loads an assembly given the long form of its name.
        /// </summary>
        /// <param name="assemblyString">The long form of the assembly name.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly Load(string assemblyString);

        /// <summary>
        /// Loads an assembly given its <see cref="AssemblyName" />.
        /// </summary>
        /// <param name="assemblyRef">The object that describes the assembly to be loaded.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly Load(AssemblyName assemblyRef);

#if NET472
        /// <summary>
        /// Loads the assembly with a common object file format (COFF)-based image containing an emitted assembly, optionally including symbols and specifying the source for the security context. The assembly is loaded into the application domain of the caller.
        /// </summary>
        /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
        /// <param name="rawSymbolStore">A byte array that contains the raw bytes representing the symbols for the assembly.</param>
        /// <param name="securityContextSource">The source of the security context.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, System.Security.SecurityContextSource securityContextSource);
#endif

        /// <summary>
        /// Loads the contents of an assembly file on the specified path.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly LoadFile(string path);

        /// <summary>
        /// Loads an assembly given its file name or path, hash value, and hash algorithm.
        /// </summary>
        /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
        /// <param name="hashValue">The value of the computed hash code.</param>
        /// <param name="hashAlgorithm">The hash algorithm used for hashing files and for generating the strong name.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly LoadFrom(string assemblyFile, byte[] hashValue, System.Configuration.Assemblies.AssemblyHashAlgorithm hashAlgorithm);

        /// <summary>
        /// Loads an assembly into the reflection-only context, given its display name.
        /// </summary>
        /// <param name="assemblyString">The display name of the assembly, as returned by the <see cref="AssemblyName.FullName" /> property.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly ReflectionOnlyLoad(string assemblyString);

        /// <summary>
        /// Loads the assembly from a common object file format (COFF)-based image containing an emitted assembly. The assembly is loaded into the reflection-only context of the caller's application domain.
        /// </summary>
        /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly ReflectionOnlyLoad(byte[] rawAssembly);

        /// <summary>
        /// Loads an assembly into the reflection-only context, given its path.
        /// </summary>
        /// <param name="assemblyFile">The path of the file that contains the manifest of the assembly.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly ReflectionOnlyLoadFrom(string assemblyFile);

        /// <summary>
        /// Loads an assembly into the load-from context, bypassing some security checks.
        /// </summary>
        /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
        /// <returns>The loaded assembly.</returns>
        Assembly UnsafeLoadFrom(string assemblyFile);
    }
}
