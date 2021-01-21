// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2017 - Created this file.
//

namespace System.Reflection
{
    using static Contracts;

    /// <summary>
    /// Provides a set of extension methods for <see cref="IReflectionTypeSystemProvider"/>.
    /// </summary>
    public static class ReflectionTypeSystemProviderExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the specified <paramref name="type"/> represents an enumeration.
        /// </summary>
        /// <param name="provider">The reflection type system provider.</param>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> represents an enumeration; otherwise, false.</returns>
        public static bool IsEnum(this IReflectionTypeSystemProvider provider, Type type) => NotNull(provider).IsSubclassOf(type, typeof(Enum));
    }
}
