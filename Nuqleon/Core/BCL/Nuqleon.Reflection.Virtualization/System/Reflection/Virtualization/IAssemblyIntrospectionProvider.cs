// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;

#if NET472
using System.Security.Policy;
#endif

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to introspect <see cref="Assembly"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IAssemblyIntrospectionProvider
    {
#if !NET5_0
        /// <summary>
        /// Gets the location of the assembly as specified originally, for example, in an <see cref="AssemblyName" /> object.
        /// </summary>
        /// <param name="assembly">The assembly to get the location for.</param>
        /// <returns>The location of the assembly as specified originally.</returns>
        string GetCodeBase(Assembly assembly);
#endif

        /// <summary>
        /// Gets the types defined in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>An array of types defined in the specified <paramref name="assembly"/>.</returns>
        IEnumerable<Type> GetDefinedTypes(Assembly assembly);

        /// <summary>
        /// Gets the entry point of this assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the entry point for.</param>
        /// <returns>An object that represents the entry point of this assembly. If no entry point is found (for example, the assembly is a DLL), null is returned.</returns>
        MethodInfo GetEntryPoint(Assembly assembly);

#if !NET5_0
        /// <summary>
        /// Gets the URI, including escape characters, that represents the codebase.
        /// </summary>
        /// <param name="assembly">The assembly to get the location for.</param>
        /// <returns>A URI with escape characters.</returns>
        string GetEscapedCodeBase(Assembly assembly);
#endif

#if NET472
        /// <summary>
        /// Gets the evidence for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get evidence for.</param>
        /// <returns>The evidence for the specified <paramref name="assembly"/>.</returns>
        Evidence GetEvidence(Assembly assembly);
#endif

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the display name for.</param>
        /// <returns>The display name of the assembly.</returns>
        string GetFullName(Assembly assembly);

#if !NET5_0
        /// <summary>
        /// Gets a value indicating whether the assembly was loaded from the global assembly cache.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the assembly was loaded from the global assembly cache; otherwise, false.</returns>
        bool GetGlobalAssemblyCache(Assembly assembly);
#endif

        /// <summary>
        /// Gets the host context with which the assembly was loaded.
        /// </summary>
        /// <param name="assembly">The assembly to get the host context for.</param>
        /// <returns>An <see cref="long" /> value that indicates the host context with which the assembly was loaded, if any.</returns>
        long GetHostContext(Assembly assembly);

        /// <summary>
        /// Gets a string representing the version of the common language runtime (CLR) saved in the file containing the manifest.
        /// </summary>
        /// <param name="assembly">The assembly to get the runtime version for.</param>
        /// <returns>The CLR version folder name. This is not a full path.</returns>
        string GetImageRuntimeVersion(Assembly assembly);

        /// <summary>
        /// Gets a value that indicates whether the current assembly was generated dynamically in the current process by using reflection emit.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the current assembly was generated dynamically in the current process; otherwise, false.</returns>
        bool IsDynamic(Assembly assembly);

        /// <summary>
        /// Gets a value that indicates whether the current assembly is loaded with full trust.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the current assembly is loaded with full trust; otherwise, false.</returns>
        bool IsFullyTrusted(Assembly assembly);

        /// <summary>
        /// Gets the path or UNC location of the loaded file that contains the manifest.
        /// </summary>
        /// <param name="assembly">The assembly to get the location for.</param>
        /// <returns>The location of the loaded file that contains the manifest. If the loaded file was shadow-copied, the location is that of the file after being shadow-copied. If the assembly is loaded from a byte array, such as when using the <see cref="M:System.Reflection.Assembly.Load(System.Byte[])" /> method overload, the value returned is an empty string ("").</returns>
        string GetLocation(Assembly assembly);

        /// <summary>
        /// Gets the module that contains the manifest for the current assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest module for.</param>
        /// <returns>The module that contains the manifest for the assembly.</returns>
        Module GetManifestModule(Assembly assembly);

#if NET472
        /// <summary>
        /// Gets the grant set of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the grant set for.</param>
        /// <returns>The grant set of the specified <paramref name="assembly"/>.</returns>
        PermissionSet GetPermissionSet(Assembly assembly);
#endif

        /// <summary>
        /// Gets a <see cref="bool" /> value indicating whether specified <paramref name="assembly"/> was loaded into the reflection-only context.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the assembly was loaded into the reflection-only context, rather than the execution context; otherwise, false.</returns>
        bool GetReflectionOnly(Assembly assembly);

        /// <summary>
        /// Gets a value that indicates which set of security rules the common language runtime (CLR) enforces for this assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the security rules for.</param>
        /// <returns>The security rule set that the CLR enforces for this assembly.</returns>
        SecurityRuleSet GetSecurityRuleSet(Assembly assembly);

        /// <summary>
        /// Adds a module resolve handler.
        /// </summary>
        /// <param name="assembly">The assembly to add a module resolve handler for.</param>
        /// <param name="handler">The module resolve handler to add.</param>
        /// <remarks>Occurs when the common language runtime class loader cannot resolve a reference to an internal module of an assembly through normal means.</remarks>
        void AddModuleResolve(Assembly assembly, ModuleResolveEventHandler handler);

        /// <summary>
        /// Removes a module resolve handler.
        /// </summary>
        /// <param name="assembly">The assembly to remove a module resolve handler for.</param>
        /// <param name="handler">The module resolve handler to remove.</param>
        /// <remarks>Occurs when the common language runtime class loader cannot resolve a reference to an internal module of an assembly through normal means.</remarks>
        void RemoveModuleResolve(Assembly assembly, ModuleResolveEventHandler handler);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Assembly" />. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/>.</returns>
        IReadOnlyList<object> GetCustomAttributes(Assembly assembly, bool inherit);

        /// <summary>
        /// Gets the custom attributes for the specified <paramref name="assembly"/> as specified by type.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Assembly" />.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/> as specified by <paramref name="attributeType" />.</returns>
        IReadOnlyList<object> GetCustomAttributes(Assembly assembly, Type attributeType, bool inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the specified <paramref name="assembly"/>, expressed as <see cref="CustomAttributeData" /> objects.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes data for.</param>
        /// <returns>A generic list of <see cref="CustomAttributeData" /> objects representing data about the attributes that have been applied to the specified <paramref name="assembly"/>.</returns>
        IEnumerable<CustomAttributeData> GetCustomAttributesData(Assembly assembly);

        /// <summary>
        /// Gets the public types defined in the specified <paramref name="assembly"/> that are visible outside the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get exported types from.</param>
        /// <returns>An array that represents the types defined in this assembly that are visible outside the assembly.</returns>
        IReadOnlyList<Type> GetExportedTypes(Assembly assembly);

        /// <summary>
        /// Gets a <see cref="FileStream" /> for the specified file in the file table of the manifest of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the file from.</param>
        /// <param name="name">The name of the specified file. Do not include the path to the file.</param>
        /// <returns>A stream that contains the specified file, or null if the file is not found.</returns>
        FileStream GetFile(Assembly assembly, string name);

        /// <summary>
        /// Gets the files in the file table of an assembly manifest, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the files from.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of streams that contain the files.</returns>
        IReadOnlyList<FileStream> GetFiles(Assembly assembly, bool getResourceModules);

        /// <summary>
        /// Gets all the loaded modules that are part of the specified <paramref name="assembly"/>, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the loaded modules for.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of modules.</returns>
        IReadOnlyList<Module> GetLoadedModules(Assembly assembly, bool getResourceModules);

        /// <summary>
        /// Returns information about how the given resource has been persisted.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource info from.</param>
        /// <param name="resourceName">The case-sensitive name of the resource. </param>
        /// <returns>An object that is populated with information about the resource's topology, or null if the resource is not found.</returns>
        ManifestResourceInfo GetManifestResourceInfo(Assembly assembly, string resourceName);

        /// <summary>
        /// Returns the names of all the resources in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource names from.</param>
        /// <returns>An array that contains the names of all the resources.</returns>
        IReadOnlyList<string> GetManifestResourceNames(Assembly assembly);

        /// <summary>
        /// Loads the specified manifest resource, scoped by the namespace of the specified type, from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource stream from.</param>
        /// <param name="type">The type whose namespace is used to scope the manifest resource name. </param>
        /// <param name="name">The case-sensitive name of the manifest resource being requested. </param>
        /// <returns>The manifest resource; or null if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
        Stream GetManifestResourceStream(Assembly assembly, Type type, string name);

        /// <summary>
        /// Loads the specified manifest resource from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource stream from.</param>
        /// <param name="name">The case-sensitive name of the manifest resource being requested. </param>
        /// <returns>The manifest resource; or null if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
        Stream GetManifestResourceStream(Assembly assembly, string name);

        /// <summary>
        /// Gets the specified module in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the module from.</param>
        /// <param name="name">The name of the module being requested.</param>
        /// <returns>The module being requested, or null if the module is not found.</returns>
        Module GetModule(Assembly assembly, string name);

        /// <summary>
        /// Gets all the modules that are part of the specified <paramref name="assembly"/>, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the modules for.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of modules.</returns>
        IReadOnlyList<Module> GetModules(Assembly assembly, bool getResourceModules);

        /// <summary>
        /// Gets an <see cref="AssemblyName" /> for the specified <paramref name="assembly"/>, setting the codebase as specified by <paramref name="copiedName" />.
        /// </summary>
        /// <param name="assembly">The assembly to get the name for.</param>
        /// <param name="copiedName">true to set the <see cref="Assembly.CodeBase" /> to the location of the assembly after it was shadow copied; false to set <see cref="Assembly.CodeBase" /> to the original location. </param>
        /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
        AssemblyName GetName(Assembly assembly, bool copiedName);

        /// <summary>
        /// Gets the <see cref="AssemblyName" /> objects for all the assemblies referenced by the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the referenced assemblies for.</param>
        /// <returns>An array that contains the fully parsed display names of all the assemblies referenced by the specified <paramref name="assembly"/>.</returns>
        IReadOnlyList<AssemblyName> GetReferencedAssemblies(Assembly assembly);

        /// <summary>
        /// Gets the satellite assembly for the specified culture.
        /// </summary>
        /// <param name="assembly">The assembly to get the satellite assembly from.</param>
        /// <param name="culture">The specified culture. </param>
        /// <returns>The specified satellite assembly.</returns>
        Assembly GetSatelliteAssembly(Assembly assembly, CultureInfo culture);

        /// <summary>
        /// Gets the specified version of the satellite assembly for the specified culture.
        /// </summary>
        /// <param name="assembly">The assembly to get the satellite assembly from.</param>
        /// <param name="culture">The specified culture. </param>
        /// <param name="version">The version of the satellite assembly. </param>
        /// <returns>The specified satellite assembly.</returns>
        Assembly GetSatelliteAssembly(Assembly assembly, CultureInfo culture, Version version);

        /// <summary>
        /// Gets the <see cref="Type" /> object with the specified name in the specified <paramref name="assembly"/>, with the options of ignoring the case, and of throwing an exception if the type is not found.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve the type from.</param>
        /// <param name="name">The full name of the type.</param>
        /// <param name="throwOnError">true to throw an exception if the type is not found; false to return null.</param>
        /// <param name="ignoreCase">true to ignore the case of the type name; otherwise, false.</param>
        /// <returns>An object that represents the specified class.</returns>
        Type GetType(Assembly assembly, string name, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Gets the types defined in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve the types from.</param>
        /// <returns>An array that contains all the types that are defined in this assembly.</returns>
        IReadOnlyList<Type> GetTypes(Assembly assembly);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="assembly"/>; otherwise, false.</returns>
        bool IsDefined(Assembly assembly, Type attributeType, bool inherit);
    }
}
