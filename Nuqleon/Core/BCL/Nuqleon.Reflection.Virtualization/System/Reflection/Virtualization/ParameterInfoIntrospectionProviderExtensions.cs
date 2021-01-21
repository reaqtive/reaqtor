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
    /// Provides a set of extension methods for <see cref="IParameterInfoIntrospectionProvider"/>.
    /// </summary>
    public static class ParameterInfoIntrospectionProviderExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="parameter"/> is an input parameter.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns>true if the parameter is an input parameter; otherwise, false.</returns>
        public static bool IsIn(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) => (NotNull(provider).GetAttributes(parameter) & ParameterAttributes.In) > ParameterAttributes.None;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="parameter"/> is an locale identifier (lcid).
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns>true if the parameter is a local identifier (lcid); otherwise, false.</returns>
        public static bool IsLcid(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) => (NotNull(provider).GetAttributes(parameter) & ParameterAttributes.Lcid) > ParameterAttributes.None;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="parameter"/> is a optional.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns>true if the parameter is a optional; otherwise, false.</returns>
        public static bool IsOptional(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) => (NotNull(provider).GetAttributes(parameter) & ParameterAttributes.Optional) > ParameterAttributes.None;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="parameter"/> is an output parameter.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns>true if the parameter is an output parameter; otherwise, false.</returns>
        public static bool IsOut(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) => (NotNull(provider).GetAttributes(parameter) & ParameterAttributes.Out) > ParameterAttributes.None;

        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="parameter"/> is a Retval parameter.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns>true if the parameter is a Retval parameter; otherwise, false.</returns>
        public static bool IsRetval(this IParameterInfoIntrospectionProvider provider, ParameterInfo parameter) => (NotNull(provider).GetAttributes(parameter) & ParameterAttributes.Retval) > ParameterAttributes.None;
    }
}
