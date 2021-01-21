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
    /// Provides a set of extension methods for <see cref="IModuleLoadingProvider"/>.
    /// </summary>
    public static class ModuleLoadingProviderExtensions
    {
        /// <summary>
        /// Loads the module, internal to this assembly, with a common object file format (COFF)-based image containing an emitted module, or a resource file.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to load the module into.</param>
        /// <param name="moduleName">The name of the module. This string must correspond to a file name in this assembly's manifest.</param>
        /// <param name="rawModule">A byte array that is a COFF-based image containing an emitted module, or a resource.</param>
        /// <returns>The loaded module.</returns>
        public static Module LoadModule(this IModuleLoadingProvider provider, Assembly assembly, string moduleName, byte[] rawModule) => NotNull(provider).LoadModule(assembly, moduleName, rawModule, rawSymbolStore: null);
    }
}
