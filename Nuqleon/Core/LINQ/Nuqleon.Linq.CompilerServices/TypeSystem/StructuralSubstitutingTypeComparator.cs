// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Comparator for structural types, obtaining a dictionary of type substitutions between the left and right side of type comparisons.
    /// </summary>
    public class StructuralSubstitutingTypeComparator : StructuralTypeEqualityComparator
    {
        /// <summary>
        /// Creates a new comparator for structural types.
        /// </summary>
        public StructuralSubstitutingTypeComparator() => Substitutions = new Dictionary<Type, Type>();

        /// <summary>
        /// Gets a dictionary of discovered type substitutions.
        /// </summary>
        public IDictionary<Type, Type> Substitutions { get; }

        /// <summary>
        /// Compares two structural types.
        /// </summary>
        /// <param name="definingType">Type defining the structure (left-hand side).</param>
        /// <param name="bindingType">Type being bound (right-hand side).</param>
        /// <returns><c>true</c> if both types are considered equal; otherwise, <c>false</c>.</returns>
        protected override bool EqualsStructural(Type definingType, Type bindingType)
        {
            if (ReferenceEquals(definingType, bindingType))
            {
                return true;
            }

            var structuralEquals = base.EqualsStructural(definingType, bindingType);

            if (structuralEquals)
            {
                if (!Substitutions.TryGetValue(bindingType, out var mappedType))
                {
                    Substitutions.Add(bindingType, definingType);
                }
                else if (!Object.ReferenceEquals(mappedType, definingType))
                {
                    if (definingType == null)
                        throw new ArgumentNullException(nameof(definingType));

                    if (bindingType == null)
                        throw new ArgumentNullException(nameof(bindingType));

                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Attempting to map '{0}' to '{1}' which is already mapped to '{2}'; cannot map the binding type to more than one defining type.",
                            bindingType.AssemblyQualifiedName,
                            definingType.AssemblyQualifiedName,
                            mappedType.AssemblyQualifiedName));
                }
            }

            return structuralEquals;
        }
    }
}
