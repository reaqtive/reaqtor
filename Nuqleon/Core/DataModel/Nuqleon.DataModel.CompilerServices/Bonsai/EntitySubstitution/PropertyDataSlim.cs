// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    /// <summary>
    /// Container for structural type property data.
    /// </summary>
    public readonly struct PropertyDataSlim : IEquatable<PropertyDataSlim>
    {
        /// <summary>
        /// Instantiates a property data container for structural types.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        public PropertyDataSlim(string name, TypeSlim type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of the property.
        /// </summary>
        public TypeSlim Type { get; }

        /// <summary>
        /// Checks if the current object is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>true if both objects are equal; otherwise, false.</returns>
        public bool Equals(PropertyDataSlim other) => Name == other.Name && Type == other.Type;

        /// <summary>
        /// Checks if the current object is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>true if both objects are equal; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is PropertyDataSlim p && Equals(p);

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
#if NET6_0 || NETSTANDARD2_1
                Name?.GetHashCode(StringComparison.Ordinal) ?? 0,
#else
                Name?.GetHashCode() ?? 0,
#endif
                Type?.GetHashCode() ?? 0
            );

        /// <summary>
        /// Checks if the specified objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if both objects are equal; otherwise, false.</returns>
        public static bool operator ==(PropertyDataSlim left, PropertyDataSlim right) => left.Equals(right);

        /// <summary>
        /// Checks if the specified objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if both objects are not equal; otherwise, false.</returns>
        public static bool operator !=(PropertyDataSlim left, PropertyDataSlim right) => !(left == right);
    }
}
