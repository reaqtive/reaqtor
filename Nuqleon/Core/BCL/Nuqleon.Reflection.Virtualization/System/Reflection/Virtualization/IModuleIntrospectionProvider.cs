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

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to introspect <see cref="Module"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IModuleIntrospectionProvider
    {
        /// <summary>
        /// Gets the appropriate <see cref="Assembly" /> for the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get the assembly for.</param>
        /// <returns>An <see cref="Assembly" /> object.</returns>
        Assembly GetAssembly(Module module);

        /// <summary>
        /// Gets a string representing the fully qualified name and path to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get the fully qualified name for.</param>
        /// <returns>The fully qualified module name.</returns>
        string GetFullyQualifiedName(Module module);

        /// <summary>
        /// Gets the metadata stream version.
        /// </summary>
        /// <param name="module">The module to get the stream version for.</param>
        /// <returns>A 32-bit integer representing the metadata stream version. The high-order two bytes represent the major version number, and the low-order two bytes represent the minor version number.</returns>
        int GetMDStreamVersion(Module module);

        /// <summary>
        /// Gets a token that identifies the module in metadata.
        /// </summary>
        /// <param name="module">The module to get the metadata token for.</param>
        /// <returns>An integer token that identifies the current module in metadata.</returns>
        int GetMetadataToken(Module module);

        /// <summary>
        /// Gets a handle for the module.
        /// </summary>
        /// <param name="module">The module to get the module handle for.</param>
        /// <returns>A <see cref="ModuleHandle" /> structure for the current module.</returns>
        ModuleHandle GetModuleHandle(Module module);

        /// <summary>
        /// Gets a universally unique identifier (UUID) that can be used to distinguish between two versions of a module.
        /// </summary>
        /// <param name="module">The module to get the module version for.</param>
        /// <returns>A <see cref="Guid" /> that can be used to distinguish between two versions of a module.</returns>
        Guid GetModuleVersionId(Module module);

        /// <summary>
        /// Gets a String representing the name of the module with the path removed.
        /// </summary>
        /// <param name="module">The module to get the name for.</param>
        /// <returns>The module name with no path.</returns>
        string GetName(Module module);

        /// <summary>
        /// Gets a string representing the name of the module.
        /// </summary>
        /// <param name="module">The module to get the name for.</param>
        /// <returns>The module name.</returns>
        string GetScopeName(Module module);

        /// <summary>
        /// Returns an array of classes accepted by the given filter and filter criteria.
        /// </summary>
        /// <param name="module">The module to find types in.</param>
        /// <param name="filter">The delegate used to filter the classes. </param>
        /// <param name="filterCriteria">An Object used to filter the classes. </param>
        /// <returns>An array of type <see cref="Type"/> containing classes that were accepted by the filter.</returns>
        IReadOnlyList<Type> FindTypes(Module module, TypeFilter filter, object filterCriteria);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Module" />. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/>.</returns>
        IReadOnlyList<object> GetCustomAttributes(Module module, bool inherit);

        /// <summary>
        /// Gets the custom attributes for the specified <paramref name="module"/> as specified by type.
        /// </summary>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Module" />.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/> as specified by <paramref name="attributeType" />.</returns>
        IReadOnlyList<object> GetCustomAttributes(Module module, Type attributeType, bool inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the specified <paramref name="module"/>, expressed as <see cref="CustomAttributeData" /> objects.
        /// </summary>
        /// <param name="module">The module to get custom attributes data for.</param>
        /// <returns>A generic list of <see cref="CustomAttributeData" /> objects representing data about the attributes that have been applied to the specified <paramref name="module"/>.</returns>
        IEnumerable<CustomAttributeData> GetCustomAttributesData(Module module);

        /// <summary>
        /// Returns a field having the specified name and binding attributes.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="name">The field name.</param>
        /// <param name="bindingAttr">One of the <see cref="BindingFlags" /> bit flags used to control the search. </param>
        /// <returns>A <see cref="FieldInfo"/> object having the specified name and binding attributes, or null if the field does not exist.</returns>
        FieldInfo GetField(Module module, string name, BindingFlags bindingAttr);

        /// <summary>
        /// Returns the global fields defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="bindingFlags">A bitwise combination of <see cref="BindingFlags" /> values that limit the search.</param>
        /// <returns>An array of type <see cref="FieldInfo" /> representing the global fields defined on the module that match the specified binding flags; if no global fields match the binding flags, an empty array is returned.</returns>
        IReadOnlyList<FieldInfo> GetFields(Module module, BindingFlags bindingFlags);

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
        MethodInfo GetMethod(Module module, string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

        /// <summary>
        /// Returns the global methods defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="bindingFlags">A bitwise combination of <see cref="BindingFlags" /> values that limit the search.</param>
        /// <returns>An array of type <see cref="MethodInfo" /> representing the global methods defined on the module that match the specified binding flags; if no global methods match the binding flags, an empty array is returned.</returns>
        IReadOnlyList<MethodInfo> GetMethods(Module module, BindingFlags bindingFlags);

        /// <summary>
        /// Gets a pair of values indicating the nature of the code in a module and the platform targeted by the module.
        /// </summary>
        /// <param name="module">The module to get the PE kind for.</param>
        /// <param name="peKind">When this method returns, a combination of the <see cref="PortableExecutableKinds" /> values indicating the nature of the code in the module.</param>
        /// <param name="machine">When this method returns, one of the <see cref="ImageFileMachine" /> values indicating the platform targeted by the module.</param>
        void GetPEKind(Module module, out PortableExecutableKinds peKind, out ImageFileMachine machine);

#if NET472
        /// <summary>
        /// Returns an X509Certificate object corresponding to the certificate included in the Authenticode signature of the assembly which this module belongs to. If the assembly has not been Authenticode signed, null is returned.
        /// </summary>
        /// <param name="module">The module to get the signer certificate for.</param>
        /// <returns>An X509Certificate object, or null if the assembly to which this module belongs has not been Authenticode signed.</returns>
        System.Security.Cryptography.X509Certificates.X509Certificate GetSignerCertificate(Module module);
#endif

        /// <summary>
        /// Returns the specified type, specifying whether to make a case-sensitive search of the module and whether to throw an exception if the type cannot be found.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
        /// <param name="throwOnError">true to throw an exception if the type cannot be found; false to return null.</param>
        /// <param name="ignoreCase">true for case-insensitive search; otherwise, false.</param>
        /// <returns>A <see cref="Type" /> object representing the specified type, if the type is declared in this module; otherwise, null.</returns>
        Type GetType(Module module, string className, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Returns all the types defined within the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <returns>An array of type <see cref="Type"/> containing types defined within the module that is reflected by this instance.</returns>
        IReadOnlyList<Type> GetTypes(Module module);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="module"/>; otherwise, false.</returns>
        bool IsDefined(Module module, Type attributeType, bool inherit);

        /// <summary>
        /// Gets a value indicating whether the object is a resource.
        /// </summary>
        /// <param name="module">The module to check.</param>
        /// <returns>true if the object is a resource; otherwise, false.</returns>
        bool IsResource(Module module);

        /// <summary>
        /// Returns the field identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="FieldInfo" /> object representing the field that is identified by the specified metadata token.</returns>
        FieldInfo ResolveField(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the type or member identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="MemberInfo" /> object representing the type or member that is identified by the specified metadata token.</returns>
        MemberInfo ResolveMember(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="MethodBase" /> object representing the method that is identified by the specified metadata token.</returns>
        MethodBase ResolveMethod(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        /// <summary>
        /// Returns the signature blob identified by a metadata token.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a signature in the module.</param>
        /// <returns>An array of bytes representing the signature blob.</returns>
        byte[] ResolveSignature(Module module, int metadataToken);

        /// <summary>
        /// Returns the string identified by the specified metadata token.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a string in the string heap of the module. </param>
        /// <returns>A <see cref="string" /> containing a string value from the metadata string heap.</returns>
        string ResolveString(Module module, int metadataToken);

        /// <summary>
        /// Returns the type identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="module">The module to resolve the metadata token in.</param>
        /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the type where the token is in scope, or null if that type is not generic. </param>
        /// <param name="genericMethodArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the method where the token is in scope, or null if that method is not generic.</param>
        /// <returns>A <see cref="Type" /> object representing the type that is identified by the specified metadata token.</returns>
        Type ResolveType(Module module, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);
    }
}
