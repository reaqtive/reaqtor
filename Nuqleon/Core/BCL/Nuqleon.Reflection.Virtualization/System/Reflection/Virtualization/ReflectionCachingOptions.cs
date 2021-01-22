// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

namespace System.Reflection
{
    /// <summary>
    /// Flags enum with options to control the degree of caching of reflection carried out by <see cref="CachingDefaultReflectionProvider"/>.
    /// </summary>
    [Flags]
    public enum ReflectionCachingOptions
    {
        /// <summary>
        /// Disables all caching.
        /// </summary>
        None = 0,

        /// <summary>
        /// Enables caching of GetCustomAttributes and related methods such as IsDefined.
        /// </summary>
        IntrospectionCustomAttributes = 1,

        /// <summary>
        /// Enables caching of GetFields, GetMethods, etc. methods.
        /// </summary>
        IntrospectionGetMethods = 2,

        /// <summary>
        /// Enables caching of FindInterfaces, FindTypes, and FindMembers.
        /// </summary>
        IntrospectionFindMethods = 4,

        /// <summary>
        /// Enables caching of MakeGeneric methods.
        /// </summary>
        CreationGenericObjects = 8,

        /// <summary>
        /// Enables all caching options.
        /// </summary>
        All = IntrospectionCustomAttributes | IntrospectionGetMethods | CreationGenericObjects,
    }
}
