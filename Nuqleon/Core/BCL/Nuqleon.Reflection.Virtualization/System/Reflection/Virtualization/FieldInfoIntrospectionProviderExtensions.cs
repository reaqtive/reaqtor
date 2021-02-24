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
    /// Provides a set of extension methods for <see cref="IFieldInfoIntrospectionProvider"/>.
    /// </summary>
    public static class FieldInfoIntrospectionProviderExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="field"/> is described by <see cref="FieldAttributes.Assembly" />; that is, the field is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="field"/> is exactly described by <see cref="FieldAttributes.Assembly" />; otherwise, false.</returns>
        public static bool IsAssembly(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="field"/> is described by <see cref="FieldAttributes.Family" />; that is, the field is visible only within its class and derived classes.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="field"/> is exactly described by <see cref="FieldAttributes.Family" />; otherwise, false.</returns>
        public static bool IsFamily(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="field"/> is described by <see cref="FieldAttributes.FamANDAssem" />; that is, the field can be accessed from derived classes, but only if they are in the same assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="field"/> is exactly described by <see cref="FieldAttributes.FamANDAssem" />; otherwise, false.</returns>
        public static bool IsFamilyAndAssembly(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the potential visibility of the specified <paramref name="field"/> is described by <see cref="FieldAttributes.FamORAssem" />; that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to retrieve visibility for.</param>
        /// <returns>true if the visibility of the specified <paramref name="field"/> is exactly described by <see cref="FieldAttributes.FamORAssem" />; otherwise, false.</returns>
        public static bool IsFamilyOrAssembly(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;

        /// <summary>
        /// Gets a value indicating whether the field can only be set in the body of the constructor.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to inspect.</param>
        /// <returns>true if the field has the InitOnly attribute set; otherwise, false.</returns>
        public static bool IsInitOnly(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.InitOnly) > FieldAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the value is written at compile time and cannot be changed.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to inspect.</param>
        /// <returns>true if the field has the Literal attribute set; otherwise, false.</returns>
        public static bool IsLiteral(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.Literal) > FieldAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the field has the NotSerialized attribute.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to inspect.</param>
        /// <returns>true if the field has the NotSerialized attribute set; otherwise, false.</returns>
        public static bool IsNotSerialized(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.NotSerialized) > FieldAttributes.PrivateScope;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace Impl suffix. (Mirror image of reflection APIs.)

        /// <summary>
        /// Gets a value indicating whether the field has the PinvokeImpl attribute.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to inspect.</param>
        /// <returns>true if the field has the PinvokeImpl attribute set; otherwise, false.</returns>
        public static bool IsPinvokeImpl(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.PinvokeImpl) > FieldAttributes.PrivateScope;

#pragma warning restore CA1711
#pragma warning restore IDE0079

        /// <summary>
        /// Gets a value indicating whether the field is private.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to retrieve visibility for.</param>
        /// <returns>true if the field is private; otherwise; false.</returns>
        public static bool IsPrivate(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;

        /// <summary>
        /// Gets a value indicating whether the field is public.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to retrieve visibility for.</param>
        /// <returns>true if the field is public; otherwise; false.</returns>
        public static bool IsPublic(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the field has the SpecialName attribute.
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to inspect.</param>
        /// <returns>true if the field has the SpecialName attribute set; otherwise, false.</returns>
        public static bool IsSpecialName(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.SpecialName) > FieldAttributes.PrivateScope;

        /// <summary>
        /// Gets a value indicating whether the field is static..
        /// </summary>
        /// <param name="provider">The reflection introspection provider.</param>
        /// <param name="field">The field to inspect.</param>
        /// <returns>true if the field is static; otherwise, false.</returns>
        public static bool IsStatic(this IFieldInfoIntrospectionProvider provider, FieldInfo field) => (NotNull(provider).GetAttributes(field) & FieldAttributes.Static) > FieldAttributes.PrivateScope;
    }
}
