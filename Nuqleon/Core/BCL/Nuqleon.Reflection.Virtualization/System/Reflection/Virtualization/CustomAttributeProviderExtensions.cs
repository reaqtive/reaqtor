// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;

namespace System.Reflection
{
    using static Contracts;

    // NB: Using a different type name than CustomAttributeExtensions in the BCL.

    /// <summary>
    /// Provides a set of extension methods to make inspecting custom attributes easier.
    /// </summary>
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="assembly"/>; otherwise, false.</returns>
        public static bool IsDefined(this IAssemblyIntrospectionProvider provider, Assembly assembly, Type attributeType)
            => NotNull(provider).IsDefined(assembly, attributeType, inherit: true);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="module">The module to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="module"/>; otherwise, false.</returns>
        public static bool IsDefined(this IModuleIntrospectionProvider provider, Module module, Type attributeType)
            => NotNull(provider).IsDefined(module, attributeType, inherit: true);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="member"/>; otherwise, false.</returns>
        public static bool IsDefined(this IMemberInfoIntrospectionProvider provider, MemberInfo member, Type attributeType)
            => NotNull(provider).IsDefined(member, attributeType, inherit: true);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="parameter"/>; otherwise, false.</returns>
        public static bool IsDefined(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, Type attributeType)
            => NotNull(provider).IsDefined(parameter, attributeType, inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IAssemblyIntrospectionProvider provider, Assembly assembly)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(assembly, typeof(Attribute), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="assembly"/> as specified by type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IAssemblyIntrospectionProvider provider, Assembly assembly, Type attributeType)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(assembly, attributeType, inherit: true);

        /// <summary>
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IAssemblyIntrospectionProvider provider, Assembly assembly) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(assembly, typeof(T), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IModuleIntrospectionProvider provider, Module module)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(module, typeof(Attribute), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="module"/> as specified by type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IModuleIntrospectionProvider provider, Module module, Type attributeType)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(module, attributeType, inherit: true);

        /// <summary>
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="module">The module to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IModuleIntrospectionProvider provider, Module module) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(module, typeof(T), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes for.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="member"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IMemberInfoIntrospectionProvider provider, MemberInfo member)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(member, typeof(Attribute), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="member"/> as specified by type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="member"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IMemberInfoIntrospectionProvider provider, MemberInfo member, Type attributeType)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(member, attributeType, inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="member"/> as specified by type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="member"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IMemberInfoIntrospectionProvider provider, MemberInfo member, Type attributeType, bool inherit)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(member, attributeType, inherit);

        /// <summary>
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IMemberInfoIntrospectionProvider provider, MemberInfo member) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(member, typeof(T), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes for.</param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="member"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IMemberInfoIntrospectionProvider provider, MemberInfo member, bool inherit)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(member, typeof(Attribute), inherit);

        /// <summary>
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes from.</param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IMemberInfoIntrospectionProvider provider, MemberInfo member, bool inherit) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(member, typeof(T), inherit);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes for.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="parameter"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(parameter, typeof(Attribute), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="parameter"/> as specified by type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="parameter"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, Type attributeType)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(parameter, attributeType, inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="parameter"/> as specified by type.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="parameter"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, Type attributeType, bool inherit)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(parameter, attributeType, inherit);

        /// <summary>
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(parameter, typeof(T), inherit: true);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes for.</param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="parameter"/>.</returns>
        public static IReadOnlyList<Attribute> GetCustomAttributes(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, bool inherit)
            => (IReadOnlyList<Attribute>)NotNull(provider).GetCustomAttributes(parameter, typeof(Attribute), inherit);

        /// <summary>
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes from.</param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, bool inherit) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(parameter, typeof(T), inherit);

        /// <summary>
        /// Gets the single custom attribute of the specified type applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get the custom attribute from.</param>
        /// <param name="attributeType">The type for which the custom attribute is to be returned. </param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static Attribute GetCustomAttribute(this IAssemblyIntrospectionProvider provider, Assembly assembly, Type attributeType)
            => Single(GetCustomAttributes(provider, assembly, attributeType));

        /// <summary>
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IAssemblyIntrospectionProvider provider, Assembly assembly) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(assembly));

        /// <summary>
        /// Gets the single custom attribute of the specified type applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="module">The module to get the custom attribute from.</param>
        /// <param name="attributeType">The type for which the custom attribute is to be returned. </param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static Attribute GetCustomAttribute(this IModuleIntrospectionProvider provider, Module module, Type attributeType)
            => Single(GetCustomAttributes(provider, module, attributeType));

        /// <summary>
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="module">The module to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IModuleIntrospectionProvider provider, Module module) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(module));

        /// <summary>
        /// Gets the single custom attribute of the specified type applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get the custom attribute from.</param>
        /// <param name="attributeType">The type for which the custom attribute is to be returned. </param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static Attribute GetCustomAttribute(this IMemberInfoIntrospectionProvider provider, MemberInfo member, Type attributeType)
            => Single(GetCustomAttributes(provider, member, attributeType));

        /// <summary>
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IMemberInfoIntrospectionProvider provider, MemberInfo member) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(member));

        /// <summary>
        /// Gets the single custom attribute of the specified type applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get the custom attribute from.</param>
        /// <param name="attributeType">The type for which the custom attribute is to be returned. </param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static Attribute GetCustomAttribute(this IMemberInfoIntrospectionProvider provider, MemberInfo member, Type attributeType, bool inherit)
            => Single(GetCustomAttributes(provider, member, attributeType, inherit));

        /// <summary>
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get the custom attribute from.</param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IMemberInfoIntrospectionProvider provider, MemberInfo member, bool inherit) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(member, inherit));

        /// <summary>
        /// Gets the single custom attribute of the specified type applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get the custom attribute from.</param>
        /// <param name="attributeType">The type for which the custom attribute is to be returned. </param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static Attribute GetCustomAttribute(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, Type attributeType)
            => Single(GetCustomAttributes(provider, parameter, attributeType));

        /// <summary>
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(parameter));

        /// <summary>
        /// Gets the single custom attribute of the specified type applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get the custom attribute from.</param>
        /// <param name="attributeType">The type for which the custom attribute is to be returned. </param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static Attribute GetCustomAttribute(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, Type attributeType, bool inherit)
            => Single(GetCustomAttributes(provider, parameter, attributeType, inherit));

        /// <summary>
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get the custom attribute from.</param>
        /// <param name="inherit">Indicates whether or not to retrieve attributes from a base type.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter, bool inherit) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(parameter, inherit));

        private static T Single<T>(IReadOnlyList<T> attributes) where T : Attribute
        {
            if (attributes == null || attributes.Count == 0)
            {
                return null;
            }

            if (attributes.Count == 1)
            {
                return attributes[0];
            }

            throw new AmbiguousMatchException(); // CONSIDER: Get an exception from reflection and use its message.
        }
    }
}
