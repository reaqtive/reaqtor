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
    /// Provides a set of extension methods for <see cref="IComTypeLoadingProvider"/>.
    /// </summary>
    public static class ComTypeLoadingProviderExtensions
    {
        /// <summary>
        /// Gets the type associated with the specified class identifier (CLSID).
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="clsid">The CLSID of the type to get. </param>
        /// <returns>System.__ComObject regardless of whether the CLSID is valid.</returns>
        public static Type GetTypeFromCLSID(this IComTypeLoadingProvider provider, Guid clsid) => NotNull(provider).GetTypeFromCLSID(clsid, server: null, throwOnError: false);

        /// <summary>
        /// Gets the type associated with the specified class identifier (CLSID), specifying whether to throw an exception if an error occurs while loading the type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="clsid">The CLSID of the type to get. </param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs. </param>
        /// <returns>System.__ComObject regardless of whether the CLSID is valid.</returns>
        public static Type GetTypeFromCLSID(this IComTypeLoadingProvider provider, Guid clsid, bool throwOnError) => NotNull(provider).GetTypeFromCLSID(clsid, server: null, throwOnError);

        /// <summary>
        /// Gets the type associated with the specified class identifier (CLSID) from the specified server.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="clsid">The CLSID of the type to get. </param>
        /// <param name="server">The server from which to load the type. If the server name is null, this method automatically reverts to the local machine. </param>
        /// <returns>System.__ComObject regardless of whether the CLSID is valid.</returns>
        public static Type GetTypeFromCLSID(this IComTypeLoadingProvider provider, Guid clsid, string server) => NotNull(provider).GetTypeFromCLSID(clsid, server, throwOnError: false);

        /// <summary>
        /// Gets the type associated with the specified program identifier (ProgID), returning null if an error is encountered while loading the <see cref="Type" />.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="progID">The ProgID of the type to get. </param>
        /// <returns>The type associated with the specified ProgID, if <paramref name="progID" /> is a valid entry in the registry and a type is associated with it; otherwise, null.</returns>
        public static Type GetTypeFromProgID(this IComTypeLoadingProvider provider, string progID) => NotNull(provider).GetTypeFromProgID(progID, server: null, throwOnError: false);

        /// <summary>
        /// Gets the type associated with the specified program identifier (ProgID), specifying whether to throw an exception if an error occurs while loading the type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="progID">The ProgID of the type to get. </param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs. </param>
        /// <returns>The type associated with the specified program identifier (ProgID), if <paramref name="progID" /> is a valid entry in the registry and a type is associated with it; otherwise, null.</returns>
        public static Type GetTypeFromProgID(this IComTypeLoadingProvider provider, string progID, bool throwOnError) => NotNull(provider).GetTypeFromProgID(progID, server: null, throwOnError);

        /// <summary>
        /// Gets the type associated with the specified program identifier (progID) from the specified server, returning null if an error is encountered while loading the type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="progID">The progID of the type to get. </param>
        /// <param name="server">The server from which to load the type. If the server name is null, this method automatically reverts to the local machine. </param>
        /// <returns>The type associated with the specified program identifier (progID), if <paramref name="progID" /> is a valid entry in the registry and a type is associated with it; otherwise, null.</returns>
        public static Type GetTypeFromProgID(this IComTypeLoadingProvider provider, string progID, string server) => NotNull(provider).GetTypeFromProgID(progID, server, throwOnError: false);
    }
}
