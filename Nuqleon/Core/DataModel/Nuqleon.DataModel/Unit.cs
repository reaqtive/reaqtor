// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Nuqleon.DataModel
{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1801 // Remove unused parameter

    /// <summary>
    /// Type to represent the singleton empty value (like void).
    /// </summary>
    public readonly struct Unit : IEquatable<Unit>
    {
        /// <summary>
        /// Compares two unit values for equality. Always returns true.
        /// </summary>
        /// <param name="unit1">First unit value.</param>
        /// <param name="unit2">Second unit value.</param>
        /// <returns>true</returns>
        public static bool operator ==(Unit unit1, Unit unit2) => true;

        /// <summary>
        /// Compares two unit values for inequality. Always returns true.
        /// </summary>
        /// <param name="unit1">First unit value.</param>
        /// <param name="unit2">Second unit value.</param>
        /// <returns>false</returns>
        public static bool operator !=(Unit unit1, Unit unit2) => false;

        /// <summary>
        /// Compares the unit value against the specified one. Always returns true.
        /// </summary>
        /// <param name="other">Unit value to compare to.</param>
        /// <returns>true</returns>
        public bool Equals(Unit other) => true;

        /// <summary>
        /// Compares the unit value against the specified object. Only returns trye of the object is a unit value.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>true if the specified object is a unit value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Unit;

        /// <summary>
        /// Gets the hash code for the unit value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => 0;

        /// <summary>
        /// Returns a friendly string representation of the unit value.
        /// </summary>
        /// <returns>Friendly string representation of the unit value.</returns>
        public override string ToString() => "()";
    }

#pragma warning restore CA1801
#pragma warning restore IDE0060
#pragma warning restore IDE0079
}
