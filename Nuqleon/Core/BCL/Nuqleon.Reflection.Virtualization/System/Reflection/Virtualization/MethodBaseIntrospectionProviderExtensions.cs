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
    /// Provides a set of extension methods for <see cref="IMethodBaseIntrospectionProvider"/>.
    /// </summary>
    public static class MethodBaseIntrospectionProviderExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is abstract; otherwise, false.</returns>
        public static bool IsAbstract(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.Abstract) > MethodAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="method"/> is described by <see cref="MethodAttributes.Assembly" />; that is, the method is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="method"/> is exactly described by <see cref="MethodAttributes.Assembly" />; otherwise, false.</returns>
        public static bool IsAssembly(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;

        /// <summary>
        /// Gets a value indicating whether the method is a constructor.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to check for being a constructor.</param>
        /// <returns>true if this method is a constructor represented by a <see cref="ConstructorInfo" /> object; otherwise, false.</returns>
        public static bool IsConstructor(this IMethodBaseIntrospectionProvider provider, MethodBase method) => method is ConstructorInfo && (NotNull(provider).GetAttributes(method) & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="method"/> is described by <see cref="MethodAttributes.Family" />; that is, the method is visible only within its class and derived classes.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="method"/> is exactly described by <see cref="MethodAttributes.Family" />; otherwise, false.</returns>
        public static bool IsFamily(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="method"/> is described by <see cref="MethodAttributes.FamANDAssem" />; that is, the method can be accessed from derived classes, but only if they are in the same assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="method"/> is exactly described by <see cref="MethodAttributes.FamANDAssem" />; otherwise, false.</returns>
        public static bool IsFamilyAndAssembly(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="method"/> is described by <see cref="MethodAttributes.FamORAssem" />; that is, the method can be accessed by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="method"/> is exactly described by <see cref="MethodAttributes.FamORAssem" />; otherwise, false.</returns>
        public static bool IsFamilyOrAssembly(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;

        /// <summary>
        /// Gets a value indicating whether the method is final.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is final; otherwise, false.</returns>
        public static bool IsFinal(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.Final) > MethodAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is hidden by signature; otherwise, false.</returns>
        public static bool IsHideBySig(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.HideBySig) > MethodAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the method is private.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is private; otherwise, false.</returns>
        public static bool IsPrivate(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;

        /// <summary>
        /// Gets a value indicating whether the method is public.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is public; otherwise, false.</returns>
        public static bool IsPublic(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the method has a special name.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method has a special name; otherwise, false.</returns>
        public static bool IsSpecialName(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.SpecialName) > MethodAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the method is static.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is static; otherwise, false.</returns>
        public static bool IsStatic(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.Static) > MethodAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the method is virtual.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="method">The method to inspect.</param>
        /// <returns>true if the method is virtual; otherwise, false.</returns>
        public static bool IsVirtual(this IMethodBaseIntrospectionProvider provider, MethodBase method) => (NotNull(provider).GetAttributes(method) & MethodAttributes.Virtual) > MethodAttributes.PrivateScope;
    }
}
