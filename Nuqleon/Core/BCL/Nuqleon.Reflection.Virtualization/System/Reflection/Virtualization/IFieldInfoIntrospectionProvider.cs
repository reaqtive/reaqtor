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
    /// Interface representing a reflection provider used to introspect <see cref="FieldInfo"/> objects.
    /// </summary>
    [GeneratedCode("", "1.0.0.0")] // NB: A mirror image of System.Reflection APIs, so considering this to be "generated".
    public interface IFieldInfoIntrospectionProvider : IMemberInfoIntrospectionProvider
    {
        /// <summary>
        /// Gets the attributes associated with the specified field.
        /// </summary>
        /// <param name="field">The field to get the attributes for.</param>
        /// <returns>The attributes associated with the specified field.</returns>
        FieldAttributes GetAttributes(FieldInfo field);

        /// <summary>
        /// Gets a <see cref="RuntimeFieldHandle"/>, which is a handle to the internal metadata representation of a field.
        /// </summary>
        /// <param name="field">The field to get the runtime handle for.</param>
        /// <returns>A handle to the internal metadata representation of a field.</returns>
        RuntimeFieldHandle GetFieldHandle(FieldInfo field);

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <param name="field">The field to get the type for.</param>
        /// <returns>The type of the field.</returns>
        Type GetFieldType(FieldInfo field);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="field"/> is security-critical or security-safe-critical at the current trust level, and therefore can perform critical operations.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>true if the specified <paramref name="field"/> is security-critical or security-safe-critical at the current trust level; false if it is transparent.</returns>
        bool IsSecurityCritical(FieldInfo field);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="field"/> is security-safe-critical at the current trust level; that is, whether it can perform critical operations and can be accessed by transparent code.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>true if the specified <paramref name="field"/> is security-safe-critical at the current trust level; false if it is security-critical or transparent.</returns>
        bool IsSecuritySafeCritical(FieldInfo field);

        /// <summary>
        /// Gets a value that indicates whether the specified <paramref name="field"/> is transparent at the current trust level, and therefore cannot perform critical operations.
        /// </summary>
        /// <param name="field">The field to check.</param>
        /// <returns>true if the specified <paramref name="field"/> is security-transparent at the current trust level; otherwise, false.</returns>
        bool IsSecurityTransparent(FieldInfo field);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the field.
        /// </summary>
        /// <param name="field">The field to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="field" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        IReadOnlyList<Type> GetOptionalCustomModifiers(FieldInfo field);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the field.
        /// </summary>
        /// <param name="field">The field to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="field" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        IReadOnlyList<Type> GetRequiredCustomModifiers(FieldInfo field);
    }
}
