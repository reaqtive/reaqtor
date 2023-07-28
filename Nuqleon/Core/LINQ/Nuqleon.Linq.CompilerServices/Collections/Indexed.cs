// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents an object that has an associated index value.
    /// The interpretation of the index is contextual and extrinsic to the object.
    /// </summary>
    /// <typeparam name="T">Type of the object to associate with an index.</typeparam>
    public readonly struct Indexed<T> : IEquatable<Indexed<T>>
    {
        /// <summary>
        /// Creates a new indexed object association.
        /// </summary>
        /// <param name="value">Object to associate with the index.</param>
        /// <param name="index">Index to associate the object with.</param>
        public Indexed(T value, int index)
        {
            Value = value;
            Index = index;
        }

        /// <summary>
        /// Gets the object associated with the index.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the index the object is associated with.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Determines whether this value is equal to the specified object.
        /// </summary>
        /// <param name="obj">Object to compare this value to.</param>
        /// <returns>true if this value equals the specified object; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Indexed<T> indexed && Equals(indexed);

        /// <summary>
        /// Gets a hash code representation of this value.
        /// </summary>
        /// <returns>Hash code representation of this value.</returns>
        public override int GetHashCode() => Index.GetHashCode() + 23 * EqualityComparer<T>.Default.GetHashCode(Value);

        /// <summary>
        /// Determines whether this value equals to the specified indexed value.
        /// Two indexed values matches if both their index and object are equal.
        /// </summary>
        /// <param name="other">Other indexed value to compare this value to.</param>
        /// <returns>true if both indexed values are equal; otherwise, false.</returns>
        public bool Equals(Indexed<T> other) => Index == other.Index && EqualityComparer<T>.Default.Equals(Value, other.Value);

        /// <summary>
        /// Determines whether the specified indexed values are equal.
        /// Two indexed values matches if both their index and object are equal.
        /// </summary>
        /// <param name="objA">First indexed value to compare.</param>
        /// <param name="objB">Second indexed value to compare.</param>
        /// <returns>true if both indexed values are equal; otherwise, false.</returns>
        public static bool operator ==(Indexed<T> objA, Indexed<T> objB) => objA.Equals(objB);

        /// <summary>
        /// Determines whether the specified indexed values are not equal.
        /// Two indexed values matches if both their index and object are equal.
        /// </summary>
        /// <param name="objA">First indexed value to compare.</param>
        /// <param name="objB">Second indexed value to compare.</param>
        /// <returns>true if both indexed values are not equal; otherwise, false.</returns>
        public static bool operator !=(Indexed<T> objA, Indexed<T> objB) => !objA.Equals(objB);

        /// <summary>
        /// Gets the string representation of the indexed value.
        /// </summary>
        /// <returns>String representation of the indexed value.</returns>
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "[{0}] {1}", Index, Value);
    }
}
