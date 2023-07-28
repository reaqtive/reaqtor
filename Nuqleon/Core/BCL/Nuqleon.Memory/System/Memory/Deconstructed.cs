// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 12/11/2014 - Generated the code in this file.
//

using System.Collections.Generic;

namespace System.Memory
{
    /// <summary>
    /// A struct container for a deconstructed cached value.
    /// </summary>
    /// <typeparam name="TCached">Type of the cached component.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
    public readonly struct Deconstructed<TCached, TNonCached> : IEquatable<Deconstructed<TCached, TNonCached>>
    {
        /// <summary>
        /// Creates the deconstructed cached value;
        /// </summary>
        /// <param name="cached">The cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        public Deconstructed(TCached cached, TNonCached nonCached)
        {
            Cached = cached;
            NonCached = nonCached;
        }

        /// <summary>
        /// The cached component.
        /// </summary>
        public TCached Cached { get; }

        /// <summary>
        /// The non-cached component.
        /// </summary>
        public TNonCached NonCached { get; }

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public override bool Equals(object obj) =>
               obj is Deconstructed<TCached, TNonCached> deconstructed
            && Equals(deconstructed);

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public bool Equals(Deconstructed<TCached, TNonCached> other) =>
               FastEqualityComparer<TNonCached>.Default.Equals(NonCached, other.NonCached)
            && FastEqualityComparer<TCached>.Default.Equals(Cached, other.Cached);

        /// <summary>
        /// Gets a hash code for the instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<TNonCached>.Default.GetHashCode(NonCached),
                FastEqualityComparer<TCached>.Default.GetHashCode(Cached)
            );

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public static bool operator ==(Deconstructed<TCached, TNonCached> left, Deconstructed<TCached, TNonCached> right) => left.Equals(right);

        /// <summary>
        /// Checks if two instances are not equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if not equal, <b>false</b> otherwise.</returns>
        public static bool operator !=(Deconstructed<TCached, TNonCached> left, Deconstructed<TCached, TNonCached> right) => !left.Equals(right);
    }
}
