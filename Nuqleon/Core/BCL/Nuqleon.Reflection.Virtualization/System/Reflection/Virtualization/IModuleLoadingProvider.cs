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
    public interface IModuleLoadingProvider
    {
        /// <summary>
        /// Loads the module, internal to the specified <paramref name="assembly"/>, with a common object file format (COFF)-based image containing an emitted module, or a resource file. The raw bytes representing the symbols for the module are also loaded.
        /// </summary>
        /// <param name="assembly">The assembly to load the module into.</param>
        /// <param name="moduleName">The name of the module. This string must correspond to a file name in this assembly's manifest.</param>
        /// <param name="rawModule">A byte array that is a COFF-based image containing an emitted module, or a resource.</param>
        /// <param name="rawSymbolStore">A byte array containing the raw bytes representing the symbols for the module. Must be null if this is a resource file.</param>
        /// <returns>The loaded module.</returns>
        Module LoadModule(Assembly assembly, string moduleName, byte[] rawModule, byte[] rawSymbolStore);
    }
}
