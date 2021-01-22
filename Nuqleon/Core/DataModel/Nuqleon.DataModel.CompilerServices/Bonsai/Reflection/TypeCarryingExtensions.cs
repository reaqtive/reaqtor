// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    /// <summary>
    /// Extensions to associate a slim type with a type.
    /// </summary>
    internal static class TypeSlimCarrierExtensions
    {
        private static readonly ConditionalWeakTable<TypeSlim, Type> _originalTypes = new();

        /// <summary>
        /// Get a type carried by a slim type, if one exists.
        /// </summary>
        /// <param name="slimType">The slim type.</param>
        /// <param name="carriedType">The carried type.</param>
        /// <returns>
        /// <b>true</b> if the slim type is carrying a type, <b>false</b> otherwise.
        /// </returns>
        public static bool TryGetCarriedType(this TypeSlim slimType, out Type carriedType) => _originalTypes.TryGetValue(slimType, out carriedType);

        /// <summary>
        /// Associate a slim type with a type.
        /// </summary>
        /// <param name="slimType">The slim type.</param>
        /// <param name="carriedType">The type to carry.</param>
        public static void CarryType(this TypeSlim slimType, Type carriedType)
        {
            if (!_originalTypes.TryGetValue(slimType, out var existingType))
            {
                existingType = GetOrAdd(slimType, carriedType);
            }

            if (existingType != carriedType)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Original type '{0}' cannot be set for slim type '{1}'; type '{2}' was already set.", carriedType.ToCSharpStringPretty(), slimType, existingType.ToCSharpStringPretty()));
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)] // NB: avoids creating a closure
        private static Type GetOrAdd(TypeSlim slimType, Type carriedType) => _originalTypes.GetValue(slimType, _ => carriedType);
    }
}
