// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Conflict with reserved language keywords. (By design; mirror image of reflection APIs.)

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

#if NET472
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
#endif

namespace System.Reflection
{
    /// <summary>
    /// Base class for reflection provider implementations.
    /// </summary>
    public abstract class ReflectionProvider : IReflectionProvider
    {
        /// <summary>
        /// Loads an assembly given the long form of its name.
        /// </summary>
        /// <param name="assemblyString">The long form of the assembly name.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly Load(string assemblyString);

        /// <summary>
        /// Loads an assembly given its <see cref="AssemblyName" />.
        /// </summary>
        /// <param name="assemblyRef">The object that describes the assembly to be loaded.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly Load(AssemblyName assemblyRef);

#if NET472
        /// <summary>
        /// Loads the assembly with a common object file format (COFF)-based image containing an emitted assembly, optionally including symbols and specifying the source for the security context. The assembly is loaded into the application domain of the caller.
        /// </summary>
        /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
        /// <param name="rawSymbolStore">A byte array that contains the raw bytes representing the symbols for the assembly.</param>
        /// <param name="securityContextSource">The source of the security context.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, SecurityContextSource securityContextSource);
#endif

        /// <summary>
        /// Loads the contents of an assembly file on the specified path.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly LoadFile(string path);

        /// <summary>
        /// Loads an assembly given its file name or path, hash value, and hash algorithm.
        /// </summary>
        /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
        /// <param name="hashValue">The value of the computed hash code.</param>
        /// <param name="hashAlgorithm">The hash algorithm used for hashing files and for generating the strong name.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly LoadFrom(string assemblyFile, byte[] hashValue, System.Configuration.Assemblies.AssemblyHashAlgorithm hashAlgorithm);

        /// <summary>
        /// Loads an assembly into the reflection-only context, given its display name.
        /// </summary>
        /// <param name="assemblyString">The display name of the assembly, as returned by the <see cref="AssemblyName.FullName" /> property.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly ReflectionOnlyLoad(string assemblyString);

        /// <summary>
        /// Loads the assembly from a common object file format (COFF)-based image containing an emitted assembly. The assembly is loaded into the reflection-only context of the caller's application domain.
        /// </summary>
        /// <param name="rawAssembly">A byte array that is a COFF-based image containing an emitted assembly.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly ReflectionOnlyLoad(byte[] rawAssembly);

        /// <summary>
        /// Loads an assembly into the reflection-only context, given its path.
        /// </summary>
        /// <param name="assemblyFile">The path of the file that contains the manifest of the assembly.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly ReflectionOnlyLoadFrom(string assemblyFile);

        /// <summary>
        /// Loads an assembly into the load-from context, bypassing some security checks.
        /// </summary>
        /// <param name="assemblyFile">The name or path of the file that contains the manifest of the assembly.</param>
        /// <returns>The loaded assembly.</returns>
        public abstract Assembly UnsafeLoadFrom(string assemblyFile);

        /// <summary>
        /// Loads the module, internal to the specified <paramref name="assembly"/>, with a common object file format (COFF)-based image containing an emitted module, or a resource file. The raw bytes representing the symbols for the module are also loaded.
        /// </summary>
        /// <param name="assembly">The assembly to load the module into.</param>
        /// <param name="moduleName">The name of the module. This string must correspond to a file name in this assembly's manifest.</param>
        /// <param name="rawModule">A byte array that is a COFF-based image containing an emitted module, or a resource.</param>
        /// <param name="rawSymbolStore">A byte array containing the raw bytes representing the symbols for the module. Must be null if this is a resource file.</param>
        /// <returns>The loaded module.</returns>
        public abstract Module LoadModule(Assembly assembly, string moduleName, byte[] rawModule, byte[] rawSymbolStore);

        /// <summary>
        /// Gets the <see cref="Type" /> with the specified name, specifying whether to perform a case-sensitive search and whether to throw an exception if the type is not found.
        /// </summary>
        /// <param name="typeName">The assembly-qualified name of the type to get. See <see cref="Type.AssemblyQualifiedName" />. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
        /// <param name="throwOnError">true to throw an exception if the type cannot be found; false to return null.Specifying false also suppresses some other exception conditions, but not all of them. See the Exceptions section.</param>
        /// <param name="ignoreCase">true to perform a case-insensitive search for <paramref name="typeName" />, false to perform a case-sensitive search for <paramref name="typeName" />. </param>
        /// <returns>The type with the specified name. If the type is not found, the <paramref name="throwOnError" /> parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of <paramref name="throwOnError" />. See the Exceptions section. </returns>
        public abstract Type GetType(string typeName, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Gets the type with the specified name, specifying whether to perform a case-sensitive search and whether to throw an exception if the type is not found, and optionally providing custom methods to resolve the assembly and the type.
        /// </summary>
        /// <param name="typeName">The name of the type to get. If the <paramref name="typeResolver" /> parameter is provided, the type name can be any string that <paramref name="typeResolver" /> is capable of resolving. If the <paramref name="assemblyResolver" /> parameter is provided or if standard type resolution is used, <paramref name="typeName" /> must be an assembly-qualified name (see <see cref="Type.AssemblyQualifiedName" />), unless the type is in the currently executing assembly or in Mscorlib.dll, in which case it is sufficient to supply the type name qualified by its namespace.</param>
        /// <param name="assemblyResolver">A method that locates and returns the assembly that is specified in <paramref name="typeName" />. The assembly name is passed to <paramref name="assemblyResolver" /> as an <see cref="AssemblyName" /> object. If <paramref name="typeName" /> does not contain the name of an assembly, <paramref name="assemblyResolver" /> is not called. If <paramref name="assemblyResolver" /> is not supplied, standard assembly resolution is performed. Caution   Do not pass methods from unknown or untrusted callers. Doing so could result in elevation of privilege for malicious code. Use only methods that you provide or that you are familiar with.</param>
        /// <param name="typeResolver">A method that locates and returns the type that is specified by <paramref name="typeName" /> from the assembly that is returned by <paramref name="assemblyResolver" /> or by standard assembly resolution. If no assembly is provided, the method can provide one. The method also takes a parameter that specifies whether to perform a case-insensitive search; the value of <paramref name="ignoreCase" /> is passed to that parameter. Caution   Do not pass methods from unknown or untrusted callers. </param>
        /// <param name="throwOnError">true to throw an exception if the type cannot be found; false to return null. Specifying false also suppresses some other exception conditions, but not all of them. See the Exceptions section.</param>
        /// <param name="ignoreCase">true to perform a case-insensitive search for <paramref name="typeName" />, false to perform a case-sensitive search for <paramref name="typeName" />. </param>
        /// <returns>The type with the specified name. If the type is not found, the <paramref name="throwOnError" /> parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of <paramref name="throwOnError" />. See the Exceptions section. </returns>
        public abstract Type GetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Gets the <see cref="Type" /> with the specified name, specifying whether to perform a case-sensitive search and whether to throw an exception if the type is not found. The type is loaded for reflection only, not for execution.
        /// </summary>
        /// <param name="typeName">The assembly-qualified name of the <see cref="Type" /> to get. </param>
        /// <param name="throwIfNotFound">true to throw a <see cref="TypeLoadException" /> if the type cannot be found; false to return null if the type cannot be found. Specifying false also suppresses some other exception conditions, but not all of them. See the Exceptions section.</param>
        /// <param name="ignoreCase">true to perform a case-insensitive search for <paramref name="typeName" />; false to perform a case-sensitive search for <paramref name="typeName" />. </param>
        /// <returns>The type with the specified name, if found; otherwise, null. If the type is not found, the <paramref name="throwIfNotFound" /> parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of <paramref name="throwIfNotFound" />. See the Exceptions section.</returns>
        public abstract Type ReflectionOnlyGetType(string typeName, bool throwIfNotFound, bool ignoreCase);

        /// <summary>
        /// Gets the type associated with the specified class identifier (CLSID) from the specified server, specifying whether to throw an exception if an error occurs while loading the type.
        /// </summary>
        /// <param name="clsid">The CLSID of the type to get. </param>
        /// <param name="server">The server from which to load the type. If the server name is null, this method automatically reverts to the local machine. </param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs. </param>
        /// <returns>System.__ComObject regardless of whether the CLSID is valid.</returns>
        public abstract Type GetTypeFromCLSID(Guid clsid, string server, bool throwOnError);

        /// <summary>
        /// Gets the type associated with the specified program identifier (progID) from the specified server, specifying whether to throw an exception if an error occurs while loading the type.
        /// </summary>
        /// <param name="progID">The progID of the <see cref="Type" /> to get. </param>
        /// <param name="server">The server from which to load the type. If the server name is null, this method automatically reverts to the local machine. </param>
        /// <param name="throwOnError">true to throw any exception that occurs.-or- false to ignore any exception that occurs. </param>
        /// <returns>The type associated with the specified program identifier (progID), if <paramref name="progID" /> is a valid entry in the registry and a type is associated with it; otherwise, null.</returns>
        public abstract Type GetTypeFromProgID(string progID, string server, bool throwOnError);

        /// <summary>
        /// Gets the type referenced by the specified type handle.
        /// </summary>
        /// <param name="handle">The object that refers to the type. </param>
        /// <returns>The type referenced by the specified <see cref="RuntimeTypeHandle" />, or null if the <see cref="RuntimeTypeHandle.Value" /> property of <paramref name="handle" /> is null.</returns>
        public abstract Type GetTypeFromHandle(RuntimeTypeHandle handle);

        /// <summary>
        /// Gets a <see cref="FieldInfo" /> for the field represented by the specified handle.
        /// </summary>
        /// <param name="handle">A <see cref="RuntimeFieldHandle" /> structure that contains the handle to the internal metadata representation of a field. </param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field specified by <paramref name="handle" />.</returns>
        public abstract FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle);

        /// <summary>
        /// Gets a <see cref="FieldInfo" /> for the field represented by the specified handle, for the specified generic type.
        /// </summary>
        /// <param name="handle">A <see cref="RuntimeFieldHandle" /> structure that contains the handle to the internal metadata representation of a field.</param>
        /// <param name="declaringType">A <see cref="RuntimeTypeHandle" /> structure that contains the handle to the generic type that defines the field.</param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
        public abstract FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType);

        /// <summary>
        /// Gets method information by using the method's internal metadata representation (handle).
        /// </summary>
        /// <param name="handle">The method's handle. </param>
        /// <returns>A <see cref="MethodBase"/> containing information about the method.</returns>
        public abstract MethodBase GetMethodFromHandle(RuntimeMethodHandle handle);

        /// <summary>
        /// Gets a <see cref="MethodBase" /> object for the constructor or method represented by the specified handle, for the specified generic type.
        /// </summary>
        /// <param name="handle">A handle to the internal metadata representation of a constructor or method.</param>
        /// <param name="declaringType">A handle to the generic type that defines the constructor or method.</param>
        /// <returns>A <see cref="MethodBase" /> object representing the method or constructor specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
        public abstract MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType);

#if !NET5_0
        /// <summary>
        /// Gets the location of the assembly as specified originally, for example, in an <see cref="AssemblyName" /> object.
        /// </summary>
        /// <param name="assembly">The assembly to get the location for.</param>
        /// <returns>The location of the assembly as specified originally.</returns>
        public abstract string GetCodeBase(Assembly assembly);
#endif

        /// <summary>
        /// Gets the types defined in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>An array of types defined in the specified <paramref name="assembly"/>.</returns>
        public abstract IEnumerable<TypeInfo> GetDefinedTypes(Assembly assembly);

        /// <summary>
        /// Gets the entry point of this assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the entry point for.</param>
        /// <returns>An object that represents the entry point of this assembly. If no entry point is found (for example, the assembly is a DLL), null is returned.</returns>
        public abstract MethodInfo GetEntryPoint(Assembly assembly);

#if !NET5_0
        /// <summary>
        /// Gets the URI, including escape characters, that represents the codebase.
        /// </summary>
        /// <param name="assembly">The assembly to get the location for.</param>
        /// <returns>A URI with escape characters.</returns>
        public abstract string GetEscapedCodeBase(Assembly assembly);
#endif

#if NET472
        /// <summary>
        /// Gets the evidence for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get evidence for.</param>
        /// <returns>The evidence for the specified <paramref name="assembly"/>.</returns>
        public abstract Evidence GetEvidence(Assembly assembly);
#endif

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the display name for.</param>
        /// <returns>The display name of the assembly.</returns>
        public abstract string GetFullName(Assembly assembly);

#if !NET5_0
        /// <summary>
        /// Gets a value indicating whether the assembly was loaded from the global assembly cache.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the assembly was loaded from the global assembly cache; otherwise, false.</returns>
        public abstract bool GetGlobalAssemblyCache(Assembly assembly);
#endif

        /// <summary>
        /// Gets the host context with which the assembly was loaded.
        /// </summary>
        /// <param name="assembly">The assembly to get the host context for.</param>
        /// <returns>An <see cref="long" /> value that indicates the host context with which the assembly was loaded, if any.</returns>
        public abstract long GetHostContext(Assembly assembly);

        /// <summary>
        /// Gets a string representing the version of the common language runtime (CLR) saved in the file containing the manifest.
        /// </summary>
        /// <param name="assembly">The assembly to get the runtime version for.</param>
        /// <returns>The CLR version folder name. This is not a full path.</returns>
        public abstract string GetImageRuntimeVersion(Assembly assembly);

        /// <summary>
        /// Gets a value that indicates whether the current assembly was generated dynamically in the current process by using reflection emit.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the current assembly was generated dynamically in the current process; otherwise, false.</returns>
        public abstract bool IsDynamic(Assembly assembly);

        /// <summary>
        /// Gets a value that indicates whether the current assembly is loaded with full trust.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the current assembly is loaded with full trust; otherwise, false.</returns>
        public abstract bool IsFullyTrusted(Assembly assembly);

        /// <summary>
        /// Gets the path or UNC location of the loaded file that contains the manifest.
        /// </summary>
        /// <param name="assembly">The assembly to get the location for.</param>
        /// <returns>The location of the loaded file that contains the manifest. If the loaded file was shadow-copied, the location is that of the file after being shadow-copied. If the assembly is loaded from a byte array, such as when using the <see cref="System.Reflection.Assembly.Load(System.Byte[])" /> method overload, the value returned is an empty string ("").</returns>
        public abstract string GetLocation(Assembly assembly);

        /// <summary>
        /// Gets the module that contains the manifest for the current assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest module for.</param>
        /// <returns>The module that contains the manifest for the assembly.</returns>
        public abstract Module GetManifestModule(Assembly assembly);

#if NET472
        /// <summary>
        /// Gets the grant set of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the grant set for.</param>
        /// <returns>The grant set of the specified <paramref name="assembly"/>.</returns>
        public abstract PermissionSet GetPermissionSet(Assembly assembly);
#endif

        /// <summary>
        /// Gets a <see cref="bool" /> value indicating whether specified <paramref name="assembly"/> was loaded into the reflection-only context.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>true if the assembly was loaded into the reflection-only context, rather than the execution context; otherwise, false.</returns>
        public abstract bool GetReflectionOnly(Assembly assembly);

        /// <summary>
        /// Gets a value that indicates which set of security rules the common language runtime (CLR) enforces for this assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the security rules for.</param>
        /// <returns>The security rule set that the CLR enforces for this assembly.</returns>
        public abstract SecurityRuleSet GetSecurityRuleSet(Assembly assembly);

        /// <summary>
        /// Adds a module resolve handler.
        /// </summary>
        /// <param name="assembly">The assembly to add a module resolve handler for.</param>
        /// <param name="handler">The module resolve handler to add.</param>
        /// <remarks>Occurs when the common language runtime class loader cannot resolve a reference to an internal module of an assembly through normal means.</remarks>
        public abstract void AddModuleResolve(Assembly assembly, ModuleResolveEventHandler handler);

        /// <summary>
        /// Removes a module resolve handler.
        /// </summary>
        /// <param name="assembly">The assembly to remove a module resolve handler for.</param>
        /// <param name="handler">The module resolve handler to remove.</param>
        /// <remarks>Occurs when the common language runtime class loader cannot resolve a reference to an internal module of an assembly through normal means.</remarks>
        public abstract void RemoveModuleResolve(Assembly assembly, ModuleResolveEventHandler handler);

        /// <summary>
        /// Locates the specified type from specified <paramref name="assembly"/> and creates an instance of it using the system activator, with optional case-sensitive search and having the specified culture, arguments, and binding and activation attributes.
        /// </summary>
        /// <param name="assembly">The assembly containing the type to instantiate.</param>
        /// <param name="typeName">The <see cref="Type.FullName" /> of the type to locate.</param>
        /// <param name="ignoreCase">true to ignore the case of the type name; otherwise, false.</param>
        /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of bit flags from <see cref="BindingFlags" />. </param>
        /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
        /// <param name="args">An array that contains the arguments to be passed to the constructor. This array of arguments must match in number, order, and type the parameters of the constructor to be invoked. If the default constructor is desired, <paramref name="args" /> must be an empty array or null.</param>
        /// <param name="culture">An instance of CultureInfo used to govern the coercion of types. If this is null, the CultureInfo for the current thread is used. (This is necessary to convert a String that represents 1000 to a Double value, for example, since 1000 is represented differently by different cultures.)</param>
        /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. Typically, an array that contains a single <c>System.Runtime.Remoting.Activation.UrlAttribute</c> object. The <c>System.Runtime.Remoting.Activation.UrlAttribute</c> specifies the URL that is required to activate a remote object.</param>
        /// <returns>An instance of the specified type, or null if <paramref name="typeName" /> is not found. The supplied arguments are used to resolve the type, and to bind the constructor that is used to create the instance.</returns>
        public abstract object CreateInstance(Assembly assembly, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Assembly" />. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/>.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(Assembly assembly, bool inherit);

        /// <summary>
        /// Gets the custom attributes for the specified <paramref name="assembly"/> as specified by type.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Assembly" />.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/> as specified by <paramref name="attributeType" />.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(Assembly assembly, Type attributeType, bool inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the specified <paramref name="assembly"/>, expressed as <see cref="CustomAttributeData" /> objects.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes data for.</param>
        /// <returns>A generic list of <see cref="CustomAttributeData" /> objects representing data about the attributes that have been applied to the specified <paramref name="assembly"/>.</returns>
        public abstract IEnumerable<CustomAttributeData> GetCustomAttributesData(Assembly assembly);

        /// <summary>
        /// Gets the public types defined in the specified <paramref name="assembly"/> that are visible outside the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get exported types from.</param>
        /// <returns>An array that represents the types defined in this assembly that are visible outside the assembly.</returns>
        public abstract IReadOnlyList<Type> GetExportedTypes(Assembly assembly);

        /// <summary>
        /// Gets a <see cref="FileStream" /> for the specified file in the file table of the manifest of the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the file from.</param>
        /// <param name="name">The name of the specified file. Do not include the path to the file.</param>
        /// <returns>A stream that contains the specified file, or null if the file is not found.</returns>
        public abstract FileStream GetFile(Assembly assembly, string name);

        /// <summary>
        /// Gets the files in the file table of an assembly manifest, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the files from.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of streams that contain the files.</returns>
        public abstract IReadOnlyList<FileStream> GetFiles(Assembly assembly, bool getResourceModules);

        /// <summary>
        /// Gets all the loaded modules that are part of the specified <paramref name="assembly"/>, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the loaded modules for.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of modules.</returns>
        public abstract IReadOnlyList<Module> GetLoadedModules(Assembly assembly, bool getResourceModules);

        /// <summary>
        /// Returns information about how the given resource has been persisted.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource info from.</param>
        /// <param name="resourceName">The case-sensitive name of the resource. </param>
        /// <returns>An object that is populated with information about the resource's topology, or null if the resource is not found.</returns>
        public abstract ManifestResourceInfo GetManifestResourceInfo(Assembly assembly, string resourceName);

        /// <summary>
        /// Returns the names of all the resources in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource names from.</param>
        /// <returns>An array that contains the names of all the resources.</returns>
        public abstract IReadOnlyList<string> GetManifestResourceNames(Assembly assembly);

        /// <summary>
        /// Loads the specified manifest resource, scoped by the namespace of the specified type, from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource stream from.</param>
        /// <param name="type">The type whose namespace is used to scope the manifest resource name. </param>
        /// <param name="name">The case-sensitive name of the manifest resource being requested. </param>
        /// <returns>The manifest resource; or null if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
        public abstract Stream GetManifestResourceStream(Assembly assembly, Type type, string name);

        /// <summary>
        /// Loads the specified manifest resource from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the manifest resource stream from.</param>
        /// <param name="name">The case-sensitive name of the manifest resource being requested. </param>
        /// <returns>The manifest resource; or null if no resources were specified during compilation or if the resource is not visible to the caller.</returns>
        public abstract Stream GetManifestResourceStream(Assembly assembly, string name);

        /// <summary>
        /// Gets the specified module in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the module from.</param>
        /// <param name="name">The name of the module being requested.</param>
        /// <returns>The module being requested, or null if the module is not found.</returns>
        public abstract Module GetModule(Assembly assembly, string name);

        /// <summary>
        /// Gets all the modules that are part of the specified <paramref name="assembly"/>, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the modules for.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of modules.</returns>
        public abstract IReadOnlyList<Module> GetModules(Assembly assembly, bool getResourceModules);

        /// <summary>
        /// Gets an <see cref="AssemblyName" /> for the specified <paramref name="assembly"/>, setting the codebase as specified by <paramref name="copiedName" />.
        /// </summary>
        /// <param name="assembly">The assembly to get the name for.</param>
        /// <param name="copiedName">true to set the <see cref="Assembly.CodeBase" /> to the location of the assembly after it was shadow copied; false to set <see cref="Assembly.CodeBase" /> to the original location. </param>
        /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
        public abstract AssemblyName GetName(Assembly assembly, bool copiedName);

        /// <summary>
        /// Gets the <see cref="AssemblyName" /> objects for all the assemblies referenced by the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the referenced assemblies for.</param>
        /// <returns>An array that contains the fully parsed display names of all the assemblies referenced by the specified <paramref name="assembly"/>.</returns>
        public abstract IReadOnlyList<AssemblyName> GetReferencedAssemblies(Assembly assembly);

        /// <summary>
        /// Gets the satellite assembly for the specified culture.
        /// </summary>
        /// <param name="assembly">The assembly to get the satellite assembly from.</param>
        /// <param name="culture">The specified culture. </param>
        /// <returns>The specified satellite assembly.</returns>
        public abstract Assembly GetSatelliteAssembly(Assembly assembly, CultureInfo culture);

        /// <summary>
        /// Gets the specified version of the satellite assembly for the specified culture.
        /// </summary>
        /// <param name="assembly">The assembly to get the satellite assembly from.</param>
        /// <param name="culture">The specified culture. </param>
        /// <param name="version">The version of the satellite assembly. </param>
        /// <returns>The specified satellite assembly.</returns>
        public abstract Assembly GetSatelliteAssembly(Assembly assembly, CultureInfo culture, Version version);

        /// <summary>
        /// Gets the <see cref="Type" /> object with the specified name in the specified <paramref name="assembly"/>, with the options of ignoring the case, and of throwing an exception if the type is not found.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve the type from.</param>
        /// <param name="name">The full name of the type.</param>
        /// <param name="throwOnError">true to throw an exception if the type is not found; false to return null.</param>
        /// <param name="ignoreCase">true to ignore the case of the type name; otherwise, false.</param>
        /// <returns>An object that represents the specified class.</returns>
        public abstract Type GetType(Assembly assembly, string name, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Gets the types defined in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve the types from.</param>
        /// <returns>An array that contains all the types that are defined in this assembly.</returns>
        public abstract IReadOnlyList<Type> GetTypes(Assembly assembly);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="assembly"/>; otherwise, false.</returns>
        public abstract bool IsDefined(Assembly assembly, Type attributeType, bool inherit);

        /// <summary>
        /// Gets the appropriate <see cref="Assembly" /> for the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get the assembly for.</param>
        /// <returns>An <see cref="Assembly" /> object.</returns>
        public abstract Assembly GetAssembly(Module module);

        /// <summary>
        /// Gets a string representing the fully qualified name and path to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get the fully qualified name for.</param>
        /// <returns>The fully qualified module name.</returns>
        public abstract string GetFullyQualifiedName(Module module);

        /// <summary>
        /// Gets the metadata stream version.
        /// </summary>
        /// <param name="module">The module to get the metadata stream version for.</param>
        /// <returns>A 32-bit integer representing the metadata stream version. The high-order two bytes represent the major version number, and the low-order two bytes represent the minor version number.</returns>
        public abstract int GetMDStreamVersion(Module module);

        /// <summary>
        /// Gets a token that identifies the module in metadata.
        /// </summary>
        /// <param name="module">The module to get the metadata token for.</param>
        /// <returns>An integer token that identifies the current module in metadata.</returns>
        public abstract int GetMetadataToken(Module module);

        /// <summary>
        /// Gets a handle for the module.
        /// </summary>
        /// <param name="module">The module to get the module handle for.</param>
        /// <returns>A <see cref="ModuleHandle" /> structure for the current module.</returns>
        public abstract ModuleHandle GetModuleHandle(Module module);

        /// <summary>
        /// Gets a universally unique identifier (UUID) that can be used to distinguish between two versions of a module.
        /// </summary>
        /// <param name="module">The module to get the module version for.</param>
        /// <returns>A <see cref="Guid" /> that can be used to distinguish between two versions of a module.</returns>
        public abstract Guid GetModuleVersionId(Module module);

        /// <summary>
        /// Gets a String representing the name of the module with the path removed.
        /// </summary>
        /// <param name="module">The module to get the name for.</param>
        /// <returns>The module name with no path.</returns>
        public abstract string GetName(Module module);

        /// <summary>
        /// Gets a string representing the name of the module.
        /// </summary>
        /// <param name="module">The module to get the name for.</param>
        /// <returns>The module name.</returns>
        public abstract string GetScopeName(Module module);

        /// <summary>
        /// Returns an array of classes accepted by the given filter and filter criteria.
        /// </summary>
        /// <param name="module">The module to find types in.</param>
        /// <param name="filter">The delegate used to filter the classes. </param>
        /// <param name="filterCriteria">An Object used to filter the classes. </param>
        /// <returns>An array of type <see cref="Type"/> containing classes that were accepted by the filter.</returns>
        public abstract IReadOnlyList<Type> FindTypes(Module module, TypeFilter filter, object filterCriteria);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Module" />. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/>.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(Module module, bool inherit);

        /// <summary>
        /// Gets the custom attributes for the specified <paramref name="module"/> as specified by type.
        /// </summary>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Module" />.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/> as specified by <paramref name="attributeType" />.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(Module module, Type attributeType, bool inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the specified <paramref name="module"/>, expressed as <see cref="CustomAttributeData" /> objects.
        /// </summary>
        /// <param name="module">The module to get custom attributes data for.</param>
        /// <returns>A generic list of <see cref="CustomAttributeData" /> objects representing data about the attributes that have been applied to the specified <paramref name="module"/>.</returns>
        public abstract IEnumerable<CustomAttributeData> GetCustomAttributesData(Module module);

        /// <summary>
        /// Returns a field having the specified name and binding attributes.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="name">The field name.</param>
        /// <param name="bindingAttr">One of the <see cref="BindingFlags" /> bit flags used to control the search. </param>
        /// <returns>A <see cref="FieldInfo"/> object having the specified name and binding attributes, or null if the field does not exist.</returns>
        public abstract FieldInfo GetField(Module module, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Returns the global fields defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="bindingFlags">A bitwise combination of <see cref="BindingFlags" /> values that limit the search.</param>
        /// <returns>An array of type <see cref="FieldInfo" /> representing the global fields defined on the module that match the specified binding flags; if no global fields match the binding flags, an empty array is returned.</returns>
        public abstract IReadOnlyList<FieldInfo> GetFields(Module module, BindingFlags bindingFlags);

#if NET5_0 || NETSTANDARD2_1
        /// <summary>
        /// Searches for the specified method whose parameters match the specified generic parameter count, argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="genericParameterCount">The number of generic type parameters of the method.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.-or- An empty array of the type <see cref="Type"/> to get a method that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract MethodInfo GetMethod(Type type, string name, int genericParameterCount, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);
#endif

        /// <summary>
        /// Returns the method implementation in accordance with the specified criteria.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="name">The method name.</param>
        /// <param name="bindingAttr">One of the BindingFlags bit flags used to control the search.</param>
        /// <param name="binder">An object that implements Binder, containing properties related to this method.</param>
        /// <param name="callConvention">The calling convention for the method.</param>
        /// <param name="types">The parameter types to search for.</param>
        /// <param name="modifiers">An array of parameter modifiers used to make binding work with parameter signatures in which the types have been modified.</param>
        /// <returns>A <see cref="MethodInfo"/> object containing implementation information as specified, or null if the method does not exist.</returns>
        public abstract MethodInfo GetMethod(Module module, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for the specified method, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Returns the global methods defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="bindingFlags">A bitwise combination of <see cref="BindingFlags" /> values that limit the search.</param>
        /// <returns>An array of type <see cref="MethodInfo" /> representing the global methods defined on the module that match the specified binding flags; if no global methods match the binding flags, an empty array is returned.</returns>
        public abstract IReadOnlyList<MethodInfo> GetMethods(Module module, BindingFlags bindingFlags);

        /// <summary>
        /// Gets a pair of values indicating the nature of the code in a module and the platform targeted by the module.
        /// </summary>
        /// <param name="module">The module to get the PE kind for.</param>
        /// <param name="peKind">When this method returns, a combination of the <see cref="PortableExecutableKinds" /> values indicating the nature of the code in the module.</param>
        /// <param name="machine">When this method returns, one of the <see cref="ImageFileMachine" /> values indicating the platform targeted by the module.</param>
        public abstract void GetPEKind(Module module, out PortableExecutableKinds peKind, out ImageFileMachine machine);

#if NET472
        /// <summary>
        /// Returns an X509Certificate object corresponding to the certificate included in the Authenticode signature of the assembly which this module belongs to. If the assembly has not been Authenticode signed, null is returned.
        /// </summary>
        /// <param name="module">The module to get the signer certificate for.</param>
        /// <returns>An X509Certificate object, or null if the assembly to which this module belongs has not been Authenticode signed.</returns>
        public abstract X509Certificate GetSignerCertificate(Module module);
#endif

        /// <summary>
        /// Returns the specified type, specifying whether to make a case-sensitive search of the module and whether to throw an exception if the type cannot be found.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <param name="throwOnError">true to throw an exception if the type cannot be found; false to return null.</param>
        /// <param name="ignoreCase">true for case-insensitive search; otherwise, false.</param>
        /// <returns>A <see cref="Type" /> object representing the specified type, if the type is declared in this module; otherwise, null.</returns>
        public abstract Type GetType(Module module, string className, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Returns all the types defined within the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <returns>An array of type <see cref="Type"/> containing types defined within the module that is reflected by this instance.</returns>
        public abstract IReadOnlyList<Type> GetTypes(Module module);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="module"/>; otherwise, false.</returns>
        public abstract bool IsDefined(Module module, Type attributeType, bool inherit);

        /// <summary>
        /// Gets a value indicating whether the object is a resource.
        /// </summary>
        /// <param name="module">The module to check.</param>
        /// <returns>true if the object is a resource; otherwise, false.</returns>
        public abstract bool IsResource(Module module);

        /// <summary>
        /// Returns the field identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field that is identified by the specified metadata token.</returns>
        public abstract FieldInfo ResolveField(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the type or member identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="MemberInfo" /> object representing the type or member that is identified by the specified metadata token.</returns>
        public abstract MemberInfo ResolveMember(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="MethodBase" /> object representing the method that is identified by the specified metadata token.</returns>
        public abstract MethodBase ResolveMethod(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the signature blob identified by a metadata token.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a signature in the module.</param>
        /// <returns>An array of bytes representing the signature blob.</returns>
        public abstract byte[] ResolveSignature(Module module, int metadataToken);

        /// <summary>
        /// Returns the string identified by the specified metadata token.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a string in the string heap of the module. </param>
        /// <returns>A <see cref="string" /> containing a string value from the metadata string heap.</returns>
        public abstract string ResolveString(Module module, int metadataToken);

        /// <summary>
        /// Returns the type identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="System.Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="System.Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="Type" /> object representing the type that is identified by the specified metadata token.</returns>
        public abstract Type ResolveType(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Gets the class object that was used to obtain the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member for which to get the reflected type.</param>
        /// <returns>The <see cref="Type"/> object through which the specified <see cref="MemberInfo"/> object was obtained.</returns>
        public abstract Type GetReflectedType(MemberInfo member);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="member"/>; otherwise, false.</returns>
        public abstract bool IsDefined(MemberInfo member, Type attributeType, bool inherit);

        /// <summary>
        /// Returns an array of <see cref="Type" /> objects representing a filtered list of interfaces implemented or inherited by the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to find interfaces for.</param>
        /// <param name="filter">The delegate that compares the interfaces against <paramref name="filterCriteria" />.</param>
        /// <param name="filterCriteria">The search criteria that determines whether an interface should be included in the returned array.</param>
        /// <returns>An array of <see cref="Type" /> objects representing a filtered list of the interfaces implemented or inherited by the specified <paramref name="type"/>, or an empty array of type <see cref="Type" /> if no interfaces matching the filter are implemented or inherited by the specified <paramref name="type"/>.</returns>
        public abstract IReadOnlyList<Type> FindInterfaces(Type type, TypeFilter filter, object filterCriteria);

        /// <summary>
        /// Returns a filtered array of <see cref="MemberInfo" /> objects of the specified member type.
        /// </summary>
        /// <param name="type">The type to find members in.</param>
        /// <param name="memberType">An object that indicates the type of member to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.-or- Zero, to return null.</param>
        /// <param name="filter">The delegate that does the comparisons, returning true if the member currently being inspected matches the <paramref name="filterCriteria" /> and false otherwise. You can use the FilterAttribute, FilterName, and FilterNameIgnoreCase delegates supplied by this class. The first uses the fields of FieldAttributes, MethodAttributes, and MethodImplAttributes as search criteria, and the other two delegates use String objects as the search criteria.</param>
        /// <param name="filterCriteria">The search criteria that determines whether a member is returned in the array of MemberInfo objects.The fields of FieldAttributes, MethodAttributes, and MethodImplAttributes can be used in conjunction with the FilterAttribute delegate supplied by this class.</param>
        /// <returns>A filtered array of <see cref="MemberInfo" /> objects of the specified member type.-or- An empty array of type <see cref="MemberInfo" />, if the specified <paramref name="type"/> does not have members of type <paramref name="memberType" /> that match the filter criteria.</returns>
        public abstract IReadOnlyList<MemberInfo> FindMembers(Type type, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria);

        /// <summary>
        /// Gets the number of dimensions in an <see cref="Array" />.
        /// </summary>
        /// <param name="type">The type to get the rank for.</param>
        /// <returns>An <see cref="int" /> containing the number of dimensions in the specified <paramref name="type"/>.</returns>
        public abstract int GetArrayRank(Type type);

        /// <summary>
        /// Searches for the members defined for the specified <paramref name="type"/> whose <see cref="DefaultMemberAttribute" /> is set.
        /// </summary>
        /// <param name="type">The type to get the default members for.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all default members of the specified <paramref name="type"/>.-or- An empty array of type <see cref="MemberInfo" />, if the <paramref name="type"/> does not have default members.</returns>
        public abstract IReadOnlyList<MemberInfo> GetDefaultMembers(Type type);

        /// <summary>
        /// Gets the <see cref="Assembly" /> in which the specified <paramref name="type"/> is declared. For generic types, gets the <see cref="Assembly" /> in which the generic type is defined.
        /// </summary>
        /// <param name="type">The type to get the declaring assembly for.</param>
        /// <returns>An <see cref="Assembly" /> instance that describes the assembly containing the specified <paramref name="type"/>. For generic types, the instance describes the assembly that contains the generic type definition, not the assembly that creates and uses a particular constructed type.</returns>
        public abstract Assembly GetAssembly(Type type);

        /// <summary>
        /// Gets the assembly-qualified name of the specified <paramref name="type"/>, which includes the name of the assembly from which the specified <paramref name="type"/> was loaded.
        /// </summary>
        /// <param name="type">The type to get the assembly-qualified name for.</param>
        /// <returns>The assembly-qualified name of the specified <paramref name="type"/>, which includes the name of the assembly from which the specified <paramref name="type"/> was loaded, or null if the current instance represents a generic type parameter.</returns>
        public abstract string GetAssemblyQualifiedName(Type type);

        /// <summary>
        /// Gets the attributes associated with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the attributes for.</param>
        /// <returns>A <see cref="TypeAttributes" /> object representing the attribute set of the specified <paramref name="type"/>, unless the specified <paramref name="type"/> represents a generic type parameter, in which case the value is unspecified.</returns>
        public abstract TypeAttributes GetAttributes(Type type);

        /// <summary>
        /// Gets the type from which the specified <paramref name="type"/> directly inherits.
        /// </summary>
        /// <param name="type">The type to get the base type for.</param>
        /// <returns>The <see cref="Type" /> from which the specified <paramref name="type"/> directly inherits, or null if the type represents the <see cref="object" /> class or an interface.</returns>
        public abstract Type GetBaseType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> object has type parameters that have not been replaced by specific types.
        /// </summary>
        /// <param name="type">The type to check for generic parameters.</param>
        /// <returns>true if the specified <paramref name="type"/> object is itself a generic type parameter or has type parameters for which specific types have not been supplied; otherwise, false.</returns>
        public abstract bool ContainsGenericParameters(Type type);

        /// <summary>
        /// Gets a <see cref="MethodBase" /> that represents the declaring method, if the specified <paramref name="type"/> represents a type parameter of a generic method.
        /// </summary>
        /// <param name="type">The type to get the declaring method for.</param>
        /// <returns>If the specified <paramref name="type"/> represents a type parameter of a generic method, a <see cref="MethodBase" /> that represents declaring method; otherwise, null.</returns>
        public abstract MethodBase GetDeclaringMethod(Type type);

        /// <summary>
        /// Gets the type that declares the current nested type or generic type parameter.
        /// </summary>
        /// <param name="type">The type to get the declaring type for.</param>
        /// <returns>A <see cref="Type" /> object representing the enclosing type, if the specified <paramref name="type"/> is a nested type; or the generic type definition, if the specified <paramref name="type"/> is a type parameter of a generic type; or the type that declares the generic method, if the specified <paramref name="type"/> is a type parameter of a generic method; otherwise, null.</returns>
        public abstract Type GetDeclaringType(Type type);

        /// <summary>
        /// Gets the fully qualified name of the specified <paramref name="type"/>, including the namespace of the <see cref="Type" /> but not the assembly.
        /// </summary>
        /// <param name="type">The type to get the full name for.</param>
        /// <returns>The fully qualified name of the specified <paramref name="type"/>, including the namespace of the <see cref="Type" /> but not the assembly; or null if the specified <paramref name="type"/> represents a generic type parameter, an array type, pointer type, or byref type based on a type parameter, or a generic type that is not a generic type definition but contains unresolved type parameters.</returns>
        public abstract string GetFullName(Type type);

        /// <summary>
        /// Gets a combination of <see cref="GenericParameterAttributes" /> flags that describe the covariance and special constraints of the specified generic type parameter.
        /// </summary>
        /// <param name="type">The type to get the generic parameter attributes for.</param>
        /// <returns>A bitwise combination of <see cref="GenericParameterAttributes" /> values that describes the covariance and special constraints of the specified generic type parameter.</returns>
        public abstract GenericParameterAttributes GetGenericParameterAttributes(Type type);

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that declared the parameter, when the specified <paramref name="type"/> represents a type parameter of a generic type or a generic method.
        /// </summary>
        /// <param name="type">The type to get the generic parameter position for.</param>
        /// <returns>The position of a type parameter in the type parameter list of the generic type or method that defines the parameter. Position numbers begin at 0.</returns>
        public abstract int GetGenericParameterPosition(Type type);

        /// <summary>
        /// Gets the GUID associated with the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the associated GUID for.</param>
        /// <returns>The GUID associated with the specified <paramref name="type"/>.</returns>
        public abstract Guid GetGuid(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> encompasses or refers to another type; that is, whether the specified <paramref name="type"/> is an array, a pointer, or is passed by reference.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an array, a pointer, or is passed by reference; otherwise, false.</returns>
        public abstract bool HasElementType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a COM object.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a COM object; otherwise, false.</returns>
        public abstract bool IsCOMObject(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a constructed generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a constructed generic type; otherwise, false.</returns>
        public abstract bool IsConstructedGenericType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> can be hosted in a context.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> can be hosted in a context; otherwise, false.</returns>
        public abstract bool IsContextful(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> represents a type parameter in the definition of a generic type or method.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> object represents a type parameter of a generic type definition or generic method definition; otherwise, false.</returns>
        public abstract bool IsGenericParameter(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a generic type; otherwise, false.</returns>
        public abstract bool IsGenericType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> represents a generic type definition, from which other generic types can be constructed.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> object represents a generic type definition; otherwise, false.</returns>
        public abstract bool IsGenericTypeDefinition(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is marshaled by reference.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is marshaled by reference; otherwise, false.</returns>
        public abstract bool IsMarshalByRef(Type type);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="type"/> is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is security-critical or security-safe-critical at the current trust level; false if it is transparent.</returns>
        public abstract bool IsSecurityCritical(Type type);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="type"/> is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is security-safe-critical at the current trust level; false if it is security-critical or transparent.</returns>
        public abstract bool IsSecuritySafeCritical(Type type);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="type"/> is transparent at the current trust level, and therefore cannot perform critical operations.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is security-transparent at the current trust level; otherwise, false.</returns>
        public abstract bool IsSecurityTransparent(Type type);

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Type" /> is serializable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is serializable; otherwise, false.</returns>
        public abstract bool IsSerializable(Type type);

        /// <summary>
        /// Gets the module in which the specified <paramref name="type"/> is defined.
        /// </summary>
        /// <param name="type">The type to get the module for.</param>
        /// <returns>The module in which the specified <paramref name="type"/> is defined.</returns>
        public abstract Module GetModule(Type type);

        /// <summary>
        /// Gets the namespace of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the namespace for.</param>
        /// <returns>The namespace of the specified <paramref name="type"/>; null if the type has no namespace or represents a generic parameter.</returns>
        public abstract string GetNamespace(Type type);

        /// <summary>
        /// Gets a <see cref="StructLayoutAttribute" /> that describes the layout of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the struct layout attribute for.</param>
        /// <returns>A <see cref="StructLayoutAttribute" /> that describes the gross layout features of the specified <paramref name="type"/>.</returns>
        public abstract StructLayoutAttribute GetStructLayoutAttribute(Type type);

        /// <summary>
        /// Gets the handle for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the runtime handle for.</param>
        /// <returns>The handle for the specified <paramref name="type"/>.</returns>
        public abstract RuntimeTypeHandle GetTypeHandle(Type type);

        /// <summary>
        /// Indicates the type provided by the common language runtime that represents the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the underlying type for.</param>
        /// <returns>The underlying system type for the specified <paramref name="type"/>.</returns>
        public abstract Type GetUnderlyingSystemType(Type type);

        /// <summary>
        /// Creates a delegate that can be used to invoke the specified static method.
        /// </summary>
        /// <param name="method">The method to create a delegate for.</param>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <returns>An instance of the specified delegate type that can be used to invoke the specified static method.</returns>
        public abstract Delegate CreateDelegate(MethodInfo method, Type delegateType);

        /// <summary>
        /// Creates a delegate that can be used to invoke the specified instance method on the specified target object.
        /// </summary>
        /// <param name="method">The method to create a delegate for.</param>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <param name="target">The target object to invoke the method on.</param>
        /// <returns>An instance of the specified delegate type that can be used to invoke the specified instance method on the specified target object.</returns>
        public abstract Delegate CreateDelegate(MethodInfo method, Type delegateType, object target);

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        /// <param name="parameter">The parameter to get the default value for.</param>
        /// <returns>The default value of the parameter, or <see cref="DBNull.Value" /> if the parameter has no default value.</returns>
        public abstract object GetDefaultValue(ParameterInfo parameter);

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        /// <param name="parameter">The parameter to get the default value for.</param>
        /// <returns>The default value of the parameter, or <see cref="DBNull.Value" /> if the parameter has no default value.</returns>
        public abstract object GetRawDefaultValue(ParameterInfo parameter);

        /// <summary>
        /// Gets the attributes associated with the specified field.
        /// </summary>
        /// <param name="field">The field to get the attributes for.</param>
        /// <returns>The attributes associated with the specified field.</returns>
        public abstract FieldAttributes GetAttributes(FieldInfo field);

        /// <summary>
        /// Gets a <see cref="RuntimeFieldHandle"/>, which is a handle to the internal metadata representation of a field.
        /// </summary>
        /// <param name="field">The field to get the runtime handle for.</param>
        /// <returns>A handle to the internal metadata representation of a field.</returns>
        public abstract RuntimeFieldHandle GetFieldHandle(FieldInfo field);

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <param name="field">The field to get the type for.</param>
        /// <returns>The type of the field.</returns>
        public abstract Type GetFieldType(FieldInfo field);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="field"/> is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>true if the specified <paramref name="field"/> is security-critical or security-safe-critical at the current trust level; false if it is transparent.</returns>
        public abstract bool IsSecurityCritical(FieldInfo field);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="field"/> is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>true if the specified <paramref name="field"/> is security-safe-critical at the current trust level; false if it is security-critical or transparent.</returns>
        public abstract bool IsSecuritySafeCritical(FieldInfo field);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="field"/> is transparent at the current trust level, and therefore cannot perform critical operations.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>true if the specified <paramref name="field"/> is security-transparent at the current trust level; otherwise, false.</returns>
        public abstract bool IsSecurityTransparent(FieldInfo field);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the field.
        /// </summary>
        /// <param name="field">The field to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="field" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public abstract IReadOnlyList<Type> GetOptionalCustomModifiers(FieldInfo field);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the field.
        /// </summary>
        /// <param name="field">The field to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="field" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public abstract IReadOnlyList<Type> GetRequiredCustomModifiers(FieldInfo field);

        /// <summary>
        /// Gets the attributes associated with the specified event.
        /// </summary>
        /// <param name="event">The event to get the attributes for.</param>
        /// <returns>The attributes associated with the specified event.</returns>
        public abstract EventAttributes GetAttributes(EventInfo @event);

        /// <summary>
        /// Gets the type of the event handler.
        /// </summary>
        /// <param name="event">The event to get the event handler type for.</param>
        /// <returns>The type of the event handler.</returns>
        public abstract Type GetEventHandlerType(EventInfo @event);

        /// <summary>
        /// Gets a value indicating whether the event is multicast.
        /// </summary>
        /// <param name="event">The event to check for multicast.</param>
        /// <returns>true if the delegate is an instance of a multicast delegate; otherwise, false.</returns>
        public abstract bool IsMulticast(EventInfo @event);

        /// <summary>
        /// Gets the attributes associated with the specified property.
        /// </summary>
        /// <param name="property">The property to get the attributes for.</param>
        /// <returns>The attributes associated with the specified property.</returns>
        public abstract PropertyAttributes GetAttributes(PropertyInfo property);

        /// <summary>
        /// Gets the type of the specified property.
        /// </summary>
        /// <param name="property">The property to get the type for.</param>
        /// <returns>The property type type of the property.</returns>
        public abstract Type GetPropertyType(PropertyInfo property);

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        /// <param name="property">The property to check for readability.</param>
        /// <returns>true if this property can be read; otherwise, false.</returns>
        public abstract bool CanRead(PropertyInfo property);

        /// <summary>
        /// Gets a value indicating whether the property can be written.
        /// </summary>
        /// <param name="property">The property to check for writeable.</param>
        /// <returns>true if this property can be written; otherwise, false.</returns>
        public abstract bool CanWrite(PropertyInfo property);

        /// <summary>
        /// Gets the custom attributes data defined on the specified member.
        /// </summary>
        /// <param name="member">The member to get the custom attributes data for.</param>
        /// <returns>The custom attributes data for the specified member.</returns>
        public abstract IEnumerable<CustomAttributeData> GetCustomAttributesData(MemberInfo member);

        /// <summary>
        /// Returns an array of all custom attributes applied to the specified member.
        /// </summary>
        /// <param name="member">The member to get the custom attributes for.</param>
        /// <param name="inherit">true to search the specified member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
        /// <returns>An array that contains all the custom attributes applied to the specified member, or an array with zero elements if no attributes are defined.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(MemberInfo member, bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to the specified member and identified by the specified <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="member">The member to get the custom attributes for.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">true to search the specified member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
        /// <returns>An array of custom attributes applied to the specified member, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit);

        /// <summary>
        /// Gets the declaring type of the specified member.
        /// </summary>
        /// <param name="member">The member to get the declaring type for.</param>
        /// <returns>The declaring type of the specified member.</returns>
        public abstract Type GetDeclaringType(MemberInfo member);

        /// <summary>
        /// Gets a value that identifies a metadata element.
        /// </summary>
        /// <param name="member">The member to get the metadata token for.</param>
        /// <returns>A value which, in combination with <see cref="MemberInfo.Module" />, uniquely identifies a metadata element.</returns>
        public abstract int GetMetadataToken(MemberInfo member);

        /// <summary>
        /// Gets the module the member is defined in.
        /// </summary>
        /// <param name="member">The member to get the module for.</param>
        /// <returns>The module defining the member.</returns>
        public abstract Module GetModule(MemberInfo member);

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="member">The member to get the name for.</param>
        /// <returns>The name of the member.</returns>
        public abstract string GetName(MemberInfo member);

        /// <summary>
        /// Gets the attributes associated with the specified method.
        /// </summary>
        /// <param name="method">The property to get the method for.</param>
        /// <returns>The attributes associated with the specified method.</returns>
        public abstract MethodAttributes GetAttributes(MethodBase method);

        /// <summary>
        /// Gets the calling convention of the specified method.
        /// </summary>
        /// <param name="method">The method to get the calling convention for.</param>
        /// <returns>The calling convention of the specified method.</returns>
        public abstract CallingConventions GetCallingConvention(MethodBase method);

        /// <summary>
        /// Gets the method body of the specified method.
        /// </summary>
        /// <param name="method">The method to get the method body for.</param>
        /// <returns>The method body of the specified method.</returns>
        public abstract MethodBody GetMethodBody(MethodBase method);

        /// <summary>
        /// Gets a handle to the internal metadata representation of the specified method.
        /// </summary>
        /// <param name="method">The method to get a method handle for.</param>
        /// <returns>A <see cref="RuntimeMethodHandle" /> object.</returns>
        public abstract RuntimeMethodHandle GetMethodHandle(MethodBase method);

        /// <summary>
        /// Returns the <see cref="MethodImplAttributes" /> flags of the specified method.
        /// </summary>
        /// <param name="method">The method to get the implementation flags for.</param>
        /// <returns>The <see cref="MethodImplAttributes"/> flags.</returns>
        public abstract MethodImplAttributes GetMethodImplementationFlags(MethodBase method);

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        /// <param name="method">The method to check for generic parameters.</param>
        /// <returns>true if the specified <paramref name="method" /> object represents a generic method that contains unassigned generic type parameters; otherwise, false.</returns>
        public abstract bool ContainsGenericParameters(MethodBase method);

        /// <summary>
        /// Gets a value indicating whether the specified method is generic.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the specified <paramref name="method" /> represents a generic method; otherwise, false.</returns>
        public abstract bool IsGenericMethod(MethodBase method);

        /// <summary>
        /// Gets a value indicating whether the specified method is a generic method definition.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the specified <paramref name="method" /> represents a generic method definition; otherwise, false.</returns>
        public abstract bool IsGenericMethodDefinition(MethodBase method);

        /// <summary>
        /// Gets a value that indicates whether the specified method or constructor is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the current method or constructor is security-critical or security-safe-critical at the current trust level; false if it is transparent.</returns>
        public abstract bool IsSecurityCritical(MethodBase method);

        /// <summary>
        /// Gets a value that indicates whether the specified method or constructor is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the method or constructor is security-safe-critical at the current trust level; false if it is security-critical or transparent.</returns>
        public abstract bool IsSecuritySafeCritical(MethodBase method);

        /// <summary>
        /// Gets a value that indicates whether the specified method or constructor is transparent at the current trust level, and therefore cannot perform critical operations.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the method or constructor is security-transparent at the current trust level; otherwise, false.</returns>
        public abstract bool IsSecurityTransparent(MethodBase method);

        /// <summary>
        /// Returns the <see cref="MethodInfo"/> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
        /// </summary>
        /// <param name="method">The method to get the base definition for.</param>
        /// <returns>A <see cref="MethodInfo"/> object for the first implementation of this method.</returns>
        public abstract MethodInfo GetBaseDefinition(MethodInfo method);

        /// <summary>
        /// Gets the attributes for the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the attributes for.</param>
        /// <returns>A <see cref="ParameterAttributes"/> object representing the attributes for this parameter.</returns>
        public abstract ParameterAttributes GetAttributes(ParameterInfo parameter);

        /// <summary>
        /// Gets the custom attributes data defined on the specified parameter.
        /// </summary>
        /// <param name="parameter">The member to get the custom attributes data for.</param>
        /// <returns>The custom attributes data for the specified parameter.</returns>
        public abstract IEnumerable<CustomAttributeData> GetCustomAttributesData(ParameterInfo parameter);

        /// <summary>
        /// Returns an array of all custom attributes applied to the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the custom attributes for.</param>
        /// <param name="inherit">true to search the specified parameter's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>An array that contains all the custom attributes applied to the specified parameter, or an array with zero elements if no attributes are defined.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(ParameterInfo parameter, bool inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to the specified parameter and identified by the specified <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="parameter">The parameter to get the custom attributes for.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">true to search the specified parameter's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>An array of custom attributes applied to the specified parameter, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        public abstract IReadOnlyList<object> GetCustomAttributes(ParameterInfo parameter, Type attributeType, bool inherit);

        /// <summary>
        /// Gets a value indicating whether the specified parameter has a default value.
        /// </summary>
        /// <param name="parameter">The parameter to check for a default value.</param>
        /// <returns>true if the specified parameter has a default value; otherwise, false.</returns>
        public abstract bool HasDefaultValue(ParameterInfo parameter);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="parameter"/>; otherwise, false.</returns>
        public abstract bool IsDefined(ParameterInfo parameter, Type attributeType, bool inherit);

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        /// <param name="parameter">The parameter to get the member for.</param>
        /// <returns>The member which implanted the parameter represented by <paramref name="parameter"/>.</returns>
        public abstract MemberInfo GetMember(ParameterInfo parameter);

        /// <summary>
        /// Gets a value that identifies this parameter in metadata.
        /// </summary>
        /// <param name="parameter">The parameter to get the metadata token for.</param>
        /// <returns>A value which, in combination with the module, uniquely identifies this parameter in metadata.</returns>
        public abstract int GetMetadataToken(ParameterInfo parameter);

        /// <summary>
        /// Gets the name of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the name for.</param>
        /// <returns>The name of the specified parameter.</returns>
        public abstract string GetName(ParameterInfo parameter);

        /// <summary>
        /// Gets the type of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the type for.</param>
        /// <returns>The type of the specified parameter.</returns>
        public abstract Type GetParameterType(ParameterInfo parameter);

        /// <summary>
        /// Gets the zero-based position of the parameter in the formal parameter list.
        /// </summary>
        /// <param name="parameter">The parameter to get the position for.</param>
        /// <returns>An integer representing the position the specified parameter occupies in the parameter list.</returns>
        public abstract int GetPosition(ParameterInfo parameter);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="parameter" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public abstract IReadOnlyList<Type> GetOptionalCustomModifiers(ParameterInfo parameter);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="parameter" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public abstract IReadOnlyList<Type> GetRequiredCustomModifiers(ParameterInfo parameter);

        /// <summary>
        /// Adds an event handler to an event source.
        /// </summary>
        /// <param name="event">The event to add a handler to.</param>
        /// <param name="target">The event source.</param>
        /// <param name="handler">Encapsulates a method or methods to be invoked when the event is raised by the target.</param>
        public abstract void AddEventHandler(EventInfo @event, object target, Delegate handler);

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public get, set, and other accessors of the specified property.
        /// </summary>
        /// <param name="property">The property to get the accessors for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects whose elements reflect the get, set, and other accessors of the specified property. If <paramref name="nonPublic" /> is true, this array contains public and non-public get, set, and other accessors. If <paramref name="nonPublic" /> is false, this array contains only public get, set, and other accessors. If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.</returns>
        public abstract IReadOnlyList<MethodInfo> GetAccessors(PropertyInfo property, bool nonPublic);

        /// <summary>
        /// Retrieves the <see cref="MethodInfo"/> object for the <see cref="EventInfo.AddEventHandler(object, Delegate)" /> method of the event, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="event">The event to get the add method for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo" /> object representing the method used to add an event handler delegate from the event source.</returns>
        public abstract MethodInfo GetAddMethod(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <param name="property">The property for which to get the literal value.</param>
        /// <returns>An object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        public abstract object GetConstantValue(PropertyInfo property);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="type">The type to get the constructor for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.-or- An empty array of the type <see cref="Type"/> to get a constructor that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the constructor that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract ConstructorInfo GetConstructor(Type type, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for the constructors defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the constructors for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="ConstructorInfo" /> objects representing all constructors defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="ConstructorInfo" />, if no constructors are defined for the specified type, or if none of the defined constructors match the binding constraints.</returns>
        public abstract IReadOnlyList<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Gets the element type of an array, by-ref, or pointer type.
        /// </summary>
        /// <param name="type">The type to get the element type for.</param>
        /// <returns>The element type of the specified type.</returns>
        public abstract Type GetElementType(Type type);

        /// <summary>
        /// Searches for an event with the specified name, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the event for.</param>
        /// <param name="name">The name of the event to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the event that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract EventInfo GetEvent(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for events that are declared or inherited by the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the events for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="EventInfo" /> objects representing all events defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="EventInfo" />, if no events are defined for the specified type, or if none of the defined events match the binding constraints.</returns>
        public abstract IReadOnlyList<EventInfo> GetEvents(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for a field with the specified name, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the field for.</param>
        /// <param name="name">The name of the field to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the field that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract FieldInfo GetField(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for fields defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the fields for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="FieldInfo" /> objects representing all fields defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="FieldInfo" />, if no fields are defined for the specified type, or if none of the defined fields match the binding constraints.</returns>
        public abstract IReadOnlyList<FieldInfo> GetFields(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <param name="method">The method to get the generic arguments for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic method. Returns an empty array if the specified method is not a generic method.</returns>
        public abstract IReadOnlyList<Type> GetGenericArguments(MethodInfo method);

        /// <summary>
        /// Gets a <see cref="ParameterInfo" /> object that contains information about the return type of the method, such as whether the return type has custom modifiers.
        /// </summary>
        /// <param name="method">The method to get the return parameter for.</param>
        /// <returns>A <see cref="ParameterInfo" /> object that contains information about the return type.</returns>
        public abstract ParameterInfo GetReturnParameter(MethodInfo method);

        /// <summary>
        /// Gets the return type of the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method to get the return type for.</param>
        /// <returns>A <see cref="Type" /> object that contains information about the return type.</returns>
        public abstract Type GetReturnType(MethodInfo method);

        /// <summary>
        /// Gets the custom attributes for the return type of the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method to get the custom attributes for the return type for.</param>
        /// <returns>An <see cref="ICustomAttributeProvider"/> object representing the custom attributes for the return type.</returns>
        public abstract ICustomAttributeProvider GetReturnTypeCustomAttributes(MethodInfo method);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic type or the type parameters of a generic type definition.
        /// </summary>
        /// <param name="type">The type to get the generic arguments for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic type. Returns an empty array if the specified type is not a generic type.</returns>
        public abstract IReadOnlyList<Type> GetGenericArguments(Type type);

        /// <summary>
        /// Gets the generic method definition of the specified generic method.
        /// </summary>
        /// <param name="method">The method to get the generic method definition for.</param>
        /// <returns>The generic method definition of the specified method.</returns>
        public abstract MethodInfo GetGenericMethodDefinition(MethodInfo method);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the constraints on the generic type parameter.
        /// </summary>
        /// <param name="type">The generic parameter type to get the constraints for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the constraints on the specified generic type parameter.</returns>
        public abstract IReadOnlyList<Type> GetGenericParameterConstraints(Type type);

        /// <summary>
        /// Gets the generic type definition of the specified generic type.
        /// </summary>
        /// <param name="type">The type to get the generic type definition for.</param>
        /// <returns>The generic type definition of the specified type.</returns>
        public abstract Type GetGenericTypeDefinition(Type type);

        /// <summary>
        /// Returns the public or non-public get accessor for this property.
        /// </summary>
        /// <param name="property">The property to get the get accessor for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the get accessor for this property, if <paramref name="nonPublic" /> is true. Returns null if <paramref name="nonPublic" /> is false and the get accessor is non-public, or if <paramref name="nonPublic" /> is true but no get accessors exist.</returns>
        public abstract MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic);

        /// <summary>
        /// Returns an array of all the index parameters for the property.
        /// </summary>
        /// <param name="property">The property to get the index parameters for.</param>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
        public abstract IReadOnlyList<ParameterInfo> GetIndexParameters(PropertyInfo property);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the property.
        /// </summary>
        /// <param name="property">The property to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the <paramref name="property" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public abstract IReadOnlyList<Type> GetOptionalCustomModifiers(PropertyInfo property);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the property.
        /// </summary>
        /// <param name="property">The property to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the <paramref name="property" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public abstract IReadOnlyList<Type> GetRequiredCustomModifiers(PropertyInfo property);

        /// <summary>
        /// Searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
        /// </summary>
        /// <param name="type">The type to get the interface for.</param>
        /// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
        /// <param name="ignoreCase">true to ignore the case of that part of <paramref name="name" /> that specifies the simple interface name (the part that specifies the namespace must be correctly cased).-or- false to perform a case-sensitive search for all parts of <paramref name="name" />.</param>
        /// <returns>An object representing the interface with the specified name, implemented or inherited by the specified type, if found; otherwise, null.</returns>
        public abstract Type GetInterface(Type type, string name, bool ignoreCase);

        /// <summary>
        /// Gets all the interfaces implemented or inherited by the specified type.
        /// </summary>
        /// <param name="type">The type to get the interfaces for.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all the interfaces implemented or inherited by the specified type.-or- An empty array of type <see cref="Type"/>, if no interfaces are implemented or inherited by the specified type.</returns>
        public abstract IReadOnlyList<Type> GetInterfaces(Type type);

        /// <summary>
        /// Returns an interface mapping for the specified interface type.
        /// </summary>
        /// <param name="type">The type to get the interface mapping for.</param>
        /// <param name="interfaceType">The interface type to retrieve a mapping for.</param>
        /// <returns>An object that represents the interface mapping for <paramref name="interfaceType" /></returns>
        public abstract InterfaceMapping GetInterfaceMap(Type type, Type interfaceType);

        /// <summary>
        /// Searches for the specified members of the specified member type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="name">The string containing the name of the members to get.</param>
        /// <param name="memberTypes">The types of members to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing the members with the specified name, if found; otherwise, an empty array.</returns>
        public abstract IReadOnlyList<MemberInfo> GetMember(Type type, string name, MemberTypes memberTypes, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for members defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all members defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="MemberInfo" />, if no members are defined for the specified type, or if none of the defined members match the binding constraints.</returns>
        public abstract IReadOnlyList<MemberInfo> GetMembers(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="type">The type to get the method for.</param>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="callConvention">The object that specifies the set of rules to use regarding the order and layout of arguments, how the return value is passed, what registers are used for arguments, and the stack is cleaned up.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.-or- An empty array of the type <see cref="Type"/> to get a method that takes no parameters.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for methods defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the methods for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing all methods defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="MethodInfo" />, if no methods are defined for the specified type, or if none of the defined methods match the binding constraints.</returns>
        public abstract IReadOnlyList<MethodInfo> GetMethods(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for a nested type with the specified name, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the nested type for.</param>
        /// <param name="name">The name of the nested type to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the nested type that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract Type GetNestedType(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for types nested in the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the nested types for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all nested types defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="Type"/>, if no nested types are defined for the specified type, or if none of the nested types methods match the binding constraints.</returns>
        public abstract IReadOnlyList<Type> GetNestedTypes(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the .other directive.
        /// </summary>
        /// <param name="event">The event to get the associated methods for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing the public methods that have been associated with the event in metadata by using the .other directive. If there are no such public methods, an empty array is returned.</returns>
        public abstract IReadOnlyList<MethodInfo> GetOtherMethods(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Gets the parameters of the specified method or constructor.
        /// </summary>
        /// <param name="method">The method or constructor to get the parameters for.</param>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) of the specified <see cref="MethodBase"/> instance.</returns>
        public abstract IReadOnlyList<ParameterInfo> GetParameters(MethodBase method);

        /// <summary>
        /// Searches for properties of for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the properties for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="PropertyInfo" /> objects representing all properties defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="PropertyInfo" />, if no properties are defined for the specified type, or if none of the defined properties match the binding constraints.</returns>
        public abstract IReadOnlyList<PropertyInfo> GetProperties(Type type, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for the specified property whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection.-or- A null reference, to use the <see cref="Type.DefaultBinder" />.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.-or- An empty array of the type <see cref="Type"/> to get a property that is not indexed.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier" /> objects representing the attributes associated with the corresponding element in the <paramref name="types" /> array. The default binder does not process this parameter.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract PropertyInfo GetProperty(Type type, string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Searches for the specified property, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract PropertyInfo GetProperty(Type type, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Searches for the specified public property with the specified name and return type.
        /// </summary>
        /// <param name="type">The type to get the property for.</param>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        public abstract PropertyInfo GetProperty(Type type, string name, Type returnType);

        /// <summary>
        /// Returns the method that is called when the event is raised, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="event">The event to get the raise method for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo"/> object that was called when the event was raised.</returns>
        public abstract MethodInfo GetRaiseMethod(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <param name="property">The property for which to get the literal value.</param>
        /// <returns>An object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        public abstract object GetRawConstantValue(PropertyInfo property);

        /// <summary>
        /// Returns a literal value associated with the field by a compiler.
        /// </summary>
        /// <param name="field">The field for which to get the literal value.</param>
        /// <returns>An object that contains the literal value associated with the field. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        public abstract object GetRawConstantValue(FieldInfo field);

        /// <summary>
        /// Retrieves the <see cref="MethodInfo"/> object for the <see cref="EventInfo.RemoveEventHandler(object, Delegate)" /> method of the event, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="event">The event to get the remove method for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo" /> object representing the method used to remove an event handler delegate from the event source.</returns>
        public abstract MethodInfo GetRemoveMethod(EventInfo @event, bool nonPublic);

        /// <summary>
        /// Returns the public or non-public set accessor for this property.
        /// </summary>
        /// <param name="property">The property to get the set accessor for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>A <see cref="MethodInfo"/> object representing the set accessor for this property, if <paramref name="nonPublic" /> is true. Returns null if <paramref name="nonPublic" /> is false and the set accessor is non-public, or if <paramref name="nonPublic" /> is true but no set accessors exist.</returns>
        public abstract MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic);

        /// <summary>
        /// Gets the value of a field.
        /// </summary>
        /// <param name="field">The field to get the value from.</param>
        /// <param name="obj">The object whose field value will be retrieved, or null for a static field.</param>
        /// <returns>The value of the field.</returns>
        public abstract object GetValue(FieldInfo field, object obj);

        /// <summary>
        /// Returns the property value of a specified object that has the specified binding, index, and culture-specific information.
        /// </summary>
        /// <param name="property">The property for which to get the value.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, and SetProperty. You must specify a suitable invocation attribute. For example, to invoke a static member, set the Static flag.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>The property value of the specified object.</returns>
        public abstract object GetValue(PropertyInfo property, object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

        /// <summary>
        /// Invokes the constructor with the specified arguments, under the constraints of the specified <see cref="Binder"/>.
        /// </summary>
        /// <param name="constructor">The constructor to invoke.</param>
        /// <param name="invokeAttr">One of the <see cref="BindingFlags"/> values that specifies the type of binding.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="parameters">An array of type Object used to match the number, order and type of the parameters for this constructor, under the constraints of binder. If this constructor does not require parameters, pass an array with zero elements.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>An instance of the class associated with the constructor.</returns>
        public abstract object Invoke(ConstructorInfo constructor, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

        /// <summary>
        /// Invokes the reflected method or constructor with the given parameters.
        /// </summary>
        /// <param name="method">The method or constructor to invoke.</param>
        /// <param name="obj">The object on which to invoke the method or constructor. If a method is static, this argument is ignored. If a constructor is static, this argument must be null or an instance of the class that defines the constructor.</param>
        /// <param name="invokeAttr">One of the <see cref="BindingFlags"/> values that specifies the type of binding.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="parameters">An argument list for the invoked method or constructor. This is an array of objects with the same number, order, and type as the parameters of the method or constructor to be invoked. If there are no parameters, this should be null.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <returns>An object containing the return value of the invoked method, or null in the case of a constructor, or null if the method's return type is void.</returns>
        public abstract object Invoke(MethodBase method, object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

        /// <summary>
        /// Invokes the specified member, using the specified binding constraints and matching the specified argument list, modifiers and culture.
        /// </summary>
        /// <param name="type">The type to invoke a member on.</param>
        /// <param name="name">The string containing the name of the constructor, method, property, or field member to invoke.</param>
        /// <param name="invokeAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. The access can be one of the BindingFlags such as Public, NonPublic, Private, InvokeMethod, GetField, and so on. The type of lookup need not be specified. If the type of lookup is omitted, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static are used.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, which can involve selection of an overloaded method, coercion of argument types, and invocation of a member through reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="target">The object on which to invoke the specified member.</param>
        /// <param name="args">An array containing the arguments to pass to the member to invoke.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the args array. A parameter's associated attributes are stored in the member's signature. The default binder processes this parameter only when calling a COM component.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> object representing the globalization locale to use, which may be necessary for locale-specific conversions, such as converting a numeric String to a Double. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        /// <param name="namedParameters">An array containing the names of the parameters to which the values in the args array are passed.</param>
        /// <returns>An object representing the return value of the invoked member.</returns>
        public abstract object InvokeMember(Type type, string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is an array.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an array; otherwise, false.</returns>
        public abstract bool IsArray(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is passed by reference.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a passed by reference; otherwise, false.</returns>
        public abstract bool IsByRef(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is an interface type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an interface type; otherwise, false.</returns>
        public abstract bool IsInterface(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a pointer.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a pointer; otherwise, false.</returns>
        public abstract bool IsPointer(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is one of the primitive types.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is one of the primitive types; otherwise, false.</returns>
        public abstract bool IsPrimitive(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> is a value type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a value type; otherwise, false.</returns>
        public abstract bool IsValueType(Type type);

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> can be accessed by code outside the assembly.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is a public type or a public nested type such that all the enclosing types are public; otherwise, false.</returns>
        public abstract bool IsVisible(Type type);

        /// <summary>
        /// Determines whether an instance of the specified <paramref name="type"/> can be assigned from an instance of the specified type <paramref name="c"/>.
        /// </summary>
        /// <param name="type">The type to check for assignment compatibility.</param>
        /// <param name="c">The type to compare with the specified <paramref name="type"/>.</param>
        /// <returns>true if <paramref name="c" /> and the specified <paramref name="type"/> represent the same type, or if the specified <paramref name="type"/> is in the inheritance hierarchy of <paramref name="c" />, or if the specified <paramref name="type"/> is an interface that <paramref name="c" /> implements, or if <paramref name="c" /> is a generic type parameter and the specified <paramref name="type"/> represents one of the constraints of <paramref name="c" />. false if none of these conditions are true, or if <paramref name="c" /> is null.</returns>
        public abstract bool IsAssignableFrom(Type type, Type c);

        /// <summary>
        /// Determines whether two COM types have the same identity and are eligible for type equivalence.
        /// </summary>
        /// <param name="type">The COM type that is tested for equivalence with the other type.</param>
        /// <param name="other">The COM type that is tested for equivalence with the specified type.</param>
        /// <returns>true if the COM types are equivalent; otherwise, false. This method also returns false if one type is in an assembly that is loaded for execution, and the other is in an assembly that is loaded into the reflection-only context.</returns>
        public abstract bool IsEquivalentTo(Type type, Type other);

        /// <summary>
        /// Determines whether the specified object is an instance of the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type to compare the object against.</param>
        /// <param name="o">The object to compare with the specified type.</param>
        /// <returns>true if the specified <paramref name="type"/> is in the inheritance hierarchy of the object represented by <paramref name="o" />, or if the specified <paramref name="type"/> is an interface that <paramref name="o" /> supports. false if neither of these conditions is the case, or if <paramref name="o" /> is null, or if the specified <paramref name="type"/> is an open generic type (that is, <see cref="Type.ContainsGenericParameters" /> returns true).</returns>
        public abstract bool IsInstanceOfType(Type type, object o);

        /// <summary>
        /// Determines whether the class represented by the specified <paramref name="type" /> derives from the class represented by the specified type <paramref name="c"/>.
        /// </summary>
        /// <param name="type">The type to check for being a subclass.</param>
        /// <param name="c">The type to compare with the specified type.</param>
        /// <returns>true if the type represented by the <paramref name="c" /> parameter and the specified <paramref name="type"/> represent classes, and the class represented by the specified <paramref name="type"/> derives from the class represented by <paramref name="c" />; otherwise, false. This method also returns false if <paramref name="c" /> and the specified <paramref name="type"/> represent the same class.</returns>
        public abstract bool IsSubclassOf(Type type, Type c);

        /// <summary>
        /// Makes a single-dimensional (vector) array type with the specified element type.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <returns>A single-dimensional (vector) array type with the specified element type.</returns>
        public abstract Type MakeArrayType(Type elementType);

        /// <summary>
        /// Makes a multi-dimensional array type with the specified element type and rank.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <param name="rank">The rank of the multi-dimensional array.</param>
        /// <returns>A multi-dimensional array type with the specified element type and rank.</returns>
        public abstract Type MakeArrayType(Type elementType, int rank);

        /// <summary>
        /// Makes a by-ref type with the specified underlying element type.
        /// </summary>
        /// <param name="elementType">The underlying element type.</param>
        /// <returns>A by-ref type with the specified underlying element type.</returns>
        public abstract Type MakeByRefType(Type elementType);

        /// <summary>
        /// Makes a generic method with the specified generic method definition and type arguments.
        /// </summary>
        /// <param name="genericMethodDefinition">The generic method definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>A generic method with the specified generic method definition and type arguments.</returns>
        public abstract MethodInfo MakeGenericMethod(MethodInfo genericMethodDefinition, params Type[] typeArguments);

        /// <summary>
        /// Makes a generic type with the specified generic type definition and type arguments.
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>A generic type with the specified generic type definition and type arguments.</returns>
        public abstract Type MakeGenericType(Type genericTypeDefinition, params Type[] typeArguments);

        /// <summary>
        /// Makes a pointer type with the specified underlying element type.
        /// </summary>
        /// <param name="elementType">The underlying element type.</param>
        /// <returns>A pointer type with the specified underlying element type.</returns>
        public abstract Type MakePointerType(Type elementType);

        /// <summary>
        /// Removes an event handler from an event source.
        /// </summary>
        /// <param name="event">The event to remove a handler from.</param>
        /// <param name="target">The event source.</param>
        /// <param name="handler">The delegate to be disassociated from the events raised by target.</param>
        public abstract void RemoveEventHandler(EventInfo @event, object target, Delegate handler);

        /// <summary>
        /// Sets the value of a field.
        /// </summary>
        /// <param name="field">The field to set the value on.</param>
        /// <param name="obj">The object whose field value will be set, or null for a static field.</param>
        /// <param name="value">The value to assign to the field.</param>
        /// <param name="invokeAttr">One of the <see cref="BindingFlags"/> values that specifies the type of binding.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        public abstract void SetValue(FieldInfo field, object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

        /// <summary>
        /// Sets the property value for a specified object that has the specified binding, index, and culture-specific information.
        /// </summary>
        /// <param name="property">The property for which to set the value.</param>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. You must specify a suitable invocation attribute. For example, to invoke a static member, set the Static flag.</param>
        /// <param name="binder">A <see cref="Binder"/> that defines a set of properties and enables the binding, coercion of argument types, and invocation of members using reflection. If binder is null, then <see cref="Type.DefaultBinder"/> is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <param name="culture">A <see cref="CultureInfo"/> used to govern the coercion of types. If this is null, the <see cref="CultureInfo"/> for the current thread is used.</param>
        public abstract void SetValue(PropertyInfo property, object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);
    }
}
