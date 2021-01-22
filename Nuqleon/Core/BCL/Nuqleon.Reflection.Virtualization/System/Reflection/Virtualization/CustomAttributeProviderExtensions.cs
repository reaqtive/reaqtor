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
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IAssemblyIntrospectionProvider provider, Assembly assembly) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(assembly, typeof(T), inherit: true);

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
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IMemberInfoIntrospectionProvider provider, MemberInfo member) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(member, typeof(T), inherit: true);

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
        /// Gets the custom attributes of the specified type <typeparamref name="T"/> applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attributes to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get custom attributes from.</param>
        /// <returns>A read-only list of custom attribute instances.</returns>
        public static IReadOnlyList<T> GetCustomAttributes<T>(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) where T : Attribute
            => (IReadOnlyList<T>)NotNull(provider).GetCustomAttributes(parameter, typeof(T), inherit: true);

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
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="assembly">The assembly to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IAssemblyIntrospectionProvider provider, Assembly assembly) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(assembly));

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
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="member">The member to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IMemberInfoIntrospectionProvider provider, MemberInfo member) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(member));

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
        /// Gets the single custom attribute of the specified type <typeparamref name="T"/> applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute to retrieve.</typeparam>
        /// <param name="provider">The reflection provider.</param>
        /// <param name="parameter">The parameter to get the custom attribute from.</param>
        /// <returns>The single applied custom attribute instance, or null if no such attribute was found.</returns>
        public static T GetCustomAttribute<T>(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) where T : Attribute
            => Single(NotNull(provider).GetCustomAttributes<T>(parameter));

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
