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
    /// Interface representing a reflection provider used to load <see cref="Type"/> objects using COM.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IComTypeLoadingProvider
    {
        /// <summary>
        /// Gets the type associated with the specified class identifier (CLSID) from the specified server, specifying whether to throw an exception if an error occurs while loading the type.
        /// </summary>
        /// <param name="clsid">The CLSID of the type to get. </param>
        /// <param name="server">The server from which to load the type. If the server name is null, this method automatically reverts to the local machine. </param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs. </param>
        /// <returns>System.__ComObject regardless of whether the CLSID is valid.</returns>
        Type GetTypeFromCLSID(Guid clsid, string server, bool throwOnError);

        /// <summary>
        /// Gets the type associated with the specified program identifier (progID) from the specified server, specifying whether to throw an exception if an error occurs while loading the type.
        /// </summary>
        /// <param name="progID">The progID of the <see cref="Type" /> to get. </param>
        /// <param name="server">The server from which to load the type. If the server name is null, this method automatically reverts to the local machine. </param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs. </param>
        /// <returns>The type associated with the specified program identifier (progID), if <paramref name="progID" /> is a valid entry in the registry and a type is associated with it; otherwise, null.</returns>
        Type GetTypeFromProgID(string progID, string server, bool throwOnError);
    }
}
