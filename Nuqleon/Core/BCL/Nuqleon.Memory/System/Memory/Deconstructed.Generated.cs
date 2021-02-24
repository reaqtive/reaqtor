// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Memory
{
    /// <summary>
    /// Static helpers for creating deconstructed containers.
    /// </summary>
    public static class Deconstructed
    {
        /// <summary>
        /// Creates a deconstructed instance from cached and non-cached components.
        /// </summary>
        /// <typeparam name="TCached">Type of the cached component.</typeparam>
        /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
        /// <param name="cached">The cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        /// <returns>The deconstructed instance.</returns>
        public static Deconstructed<TCached, TNonCached> Create<TCached, TNonCached>(TCached cached, TNonCached nonCached) => new Deconstructed<TCached, TNonCached>(cached, nonCached);

        /// <summary>
        /// Creates a deconstructed instance from cached and non-cached components.
        /// </summary>
        /// <typeparam name="TCached1">Type of the first cached component.</typeparam>
        /// <typeparam name="TCached2">Type of the second cached component.</typeparam>
        /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
        /// <param name="cached1">The first cached component.</param>
        /// <param name="cached2">The second cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        /// <returns>The deconstructed instance.</returns>
        public static Deconstructed<TCached1, TCached2, TNonCached> Create<TCached1, TCached2, TNonCached>(TCached1 cached1, TCached2 cached2, TNonCached nonCached) => new Deconstructed<TCached1, TCached2, TNonCached>(cached1, cached2, nonCached);

        /// <summary>
        /// Creates a deconstructed instance from cached and non-cached components.
        /// </summary>
        /// <typeparam name="TCached1">Type of the first cached component.</typeparam>
        /// <typeparam name="TCached2">Type of the second cached component.</typeparam>
        /// <typeparam name="TCached3">Type of the third cached component.</typeparam>
        /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
        /// <param name="cached1">The first cached component.</param>
        /// <param name="cached2">The second cached component.</param>
        /// <param name="cached3">The third cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        /// <returns>The deconstructed instance.</returns>
        public static Deconstructed<TCached1, TCached2, TCached3, TNonCached> Create<TCached1, TCached2, TCached3, TNonCached>(TCached1 cached1, TCached2 cached2, TCached3 cached3, TNonCached nonCached) => new Deconstructed<TCached1, TCached2, TCached3, TNonCached>(cached1, cached2, cached3, nonCached);

        /// <summary>
        /// Creates a deconstructed instance from cached and non-cached components.
        /// </summary>
        /// <typeparam name="TCached1">Type of the first cached component.</typeparam>
        /// <typeparam name="TCached2">Type of the second cached component.</typeparam>
        /// <typeparam name="TCached3">Type of the third cached component.</typeparam>
        /// <typeparam name="TCached4">Type of the fourth cached component.</typeparam>
        /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
        /// <param name="cached1">The first cached component.</param>
        /// <param name="cached2">The second cached component.</param>
        /// <param name="cached3">The third cached component.</param>
        /// <param name="cached4">The fourth cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        /// <returns>The deconstructed instance.</returns>
        public static Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> Create<TCached1, TCached2, TCached3, TCached4, TNonCached>(TCached1 cached1, TCached2 cached2, TCached3 cached3, TCached4 cached4, TNonCached nonCached) => new Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached>(cached1, cached2, cached3, cached4, nonCached);

    }

    /// <summary>
    /// A struct container for a deconstructed cached value.
    /// </summary>
    /// <typeparam name="TCached1">Type of the first cached component.</typeparam>
    /// <typeparam name="TCached2">Type of the second cached component.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "The point is to bundle a set of arbitrary types.")]
    public struct Deconstructed<TCached1, TCached2, TNonCached> : IEquatable<Deconstructed<TCached1, TCached2, TNonCached>>
    {
        /// <summary>
        /// Creates the deconstructed cached value;
        /// </summary>
        /// <param name="cached1">The first cached component.</param>
        /// <param name="cached2">The second cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        public Deconstructed(TCached1 cached1, TCached2 cached2, TNonCached nonCached)
        {
            Cached1 = cached1;
            Cached2 = cached2;
            NonCached = nonCached;
        }

        /// <summary>
        /// The first cached component.
        /// </summary>
        public TCached1 Cached1 { get; }

        /// <summary>
        /// The second cached component.
        /// </summary>
        public TCached2 Cached2 { get; }

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
               obj is Deconstructed<TCached1, TCached2, TNonCached>
            && Equals((Deconstructed<TCached1, TCached2, TNonCached>)obj);

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public bool Equals(Deconstructed<TCached1, TCached2, TNonCached> other) =>
               FastEqualityComparer<TNonCached>.Default.Equals(NonCached, other.NonCached)
            && FastEqualityComparer<TCached1>.Default.Equals(Cached1, other.Cached1)
            && FastEqualityComparer<TCached2>.Default.Equals(Cached2, other.Cached2);

        /// <summary>
        /// Gets a hash code for the instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<TCached1>.Default.GetHashCode(Cached1),
                FastEqualityComparer<TCached2>.Default.GetHashCode(Cached2)
            );

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public static bool operator ==(Deconstructed<TCached1, TCached2, TNonCached> left, Deconstructed<TCached1, TCached2, TNonCached> right) => left.Equals(right);

        /// <summary>
        /// Checks if two instances are not equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if not equal, <b>false</b> otherwise.</returns>
        public static bool operator !=(Deconstructed<TCached1, TCached2, TNonCached> left, Deconstructed<TCached1, TCached2, TNonCached> right) => !left.Equals(right);
    }

    /// <summary>
    /// A struct container for a deconstructed cached value.
    /// </summary>
    /// <typeparam name="TCached1">Type of the first cached component.</typeparam>
    /// <typeparam name="TCached2">Type of the second cached component.</typeparam>
    /// <typeparam name="TCached3">Type of the third cached component.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "The point is to bundle a set of arbitrary types.")]
    public struct Deconstructed<TCached1, TCached2, TCached3, TNonCached> : IEquatable<Deconstructed<TCached1, TCached2, TCached3, TNonCached>>
    {
        /// <summary>
        /// Creates the deconstructed cached value;
        /// </summary>
        /// <param name="cached1">The first cached component.</param>
        /// <param name="cached2">The second cached component.</param>
        /// <param name="cached3">The third cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        public Deconstructed(TCached1 cached1, TCached2 cached2, TCached3 cached3, TNonCached nonCached)
        {
            Cached1 = cached1;
            Cached2 = cached2;
            Cached3 = cached3;
            NonCached = nonCached;
        }

        /// <summary>
        /// The first cached component.
        /// </summary>
        public TCached1 Cached1 { get; }

        /// <summary>
        /// The second cached component.
        /// </summary>
        public TCached2 Cached2 { get; }

        /// <summary>
        /// The third cached component.
        /// </summary>
        public TCached3 Cached3 { get; }

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
               obj is Deconstructed<TCached1, TCached2, TCached3, TNonCached>
            && Equals((Deconstructed<TCached1, TCached2, TCached3, TNonCached>)obj);

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public bool Equals(Deconstructed<TCached1, TCached2, TCached3, TNonCached> other) =>
               FastEqualityComparer<TNonCached>.Default.Equals(NonCached, other.NonCached)
            && FastEqualityComparer<TCached1>.Default.Equals(Cached1, other.Cached1)
            && FastEqualityComparer<TCached2>.Default.Equals(Cached2, other.Cached2)
            && FastEqualityComparer<TCached3>.Default.Equals(Cached3, other.Cached3);

        /// <summary>
        /// Gets a hash code for the instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<TCached1>.Default.GetHashCode(Cached1),
                FastEqualityComparer<TCached2>.Default.GetHashCode(Cached2),
                FastEqualityComparer<TCached3>.Default.GetHashCode(Cached3)
            );

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public static bool operator ==(Deconstructed<TCached1, TCached2, TCached3, TNonCached> left, Deconstructed<TCached1, TCached2, TCached3, TNonCached> right) => left.Equals(right);

        /// <summary>
        /// Checks if two instances are not equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if not equal, <b>false</b> otherwise.</returns>
        public static bool operator !=(Deconstructed<TCached1, TCached2, TCached3, TNonCached> left, Deconstructed<TCached1, TCached2, TCached3, TNonCached> right) => !left.Equals(right);
    }

    /// <summary>
    /// A struct container for a deconstructed cached value.
    /// </summary>
    /// <typeparam name="TCached1">Type of the first cached component.</typeparam>
    /// <typeparam name="TCached2">Type of the second cached component.</typeparam>
    /// <typeparam name="TCached3">Type of the third cached component.</typeparam>
    /// <typeparam name="TCached4">Type of the fourth cached component.</typeparam>
    /// <typeparam name="TNonCached">Type of the non-cached component.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "The point is to bundle a set of arbitrary types.")]
    public struct Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> : IEquatable<Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached>>
    {
        /// <summary>
        /// Creates the deconstructed cached value;
        /// </summary>
        /// <param name="cached1">The first cached component.</param>
        /// <param name="cached2">The second cached component.</param>
        /// <param name="cached3">The third cached component.</param>
        /// <param name="cached4">The fourth cached component.</param>
        /// <param name="nonCached">The non-cached component.</param>
        public Deconstructed(TCached1 cached1, TCached2 cached2, TCached3 cached3, TCached4 cached4, TNonCached nonCached)
        {
            Cached1 = cached1;
            Cached2 = cached2;
            Cached3 = cached3;
            Cached4 = cached4;
            NonCached = nonCached;
        }

        /// <summary>
        /// The first cached component.
        /// </summary>
        public TCached1 Cached1 { get; }

        /// <summary>
        /// The second cached component.
        /// </summary>
        public TCached2 Cached2 { get; }

        /// <summary>
        /// The third cached component.
        /// </summary>
        public TCached3 Cached3 { get; }

        /// <summary>
        /// The fourth cached component.
        /// </summary>
        public TCached4 Cached4 { get; }

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
               obj is Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached>
            && Equals((Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached>)obj);

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public bool Equals(Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> other) =>
               FastEqualityComparer<TNonCached>.Default.Equals(NonCached, other.NonCached)
            && FastEqualityComparer<TCached1>.Default.Equals(Cached1, other.Cached1)
            && FastEqualityComparer<TCached2>.Default.Equals(Cached2, other.Cached2)
            && FastEqualityComparer<TCached3>.Default.Equals(Cached3, other.Cached3)
            && FastEqualityComparer<TCached4>.Default.Equals(Cached4, other.Cached4);

        /// <summary>
        /// Gets a hash code for the instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                FastEqualityComparer<TCached1>.Default.GetHashCode(Cached1),
                FastEqualityComparer<TCached2>.Default.GetHashCode(Cached2),
                FastEqualityComparer<TCached3>.Default.GetHashCode(Cached3),
                FastEqualityComparer<TCached4>.Default.GetHashCode(Cached4)
            );

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if equal, <b>false</b> otherwise.</returns>
        public static bool operator ==(Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> left, Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> right) => left.Equals(right);

        /// <summary>
        /// Checks if two instances are not equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns><b>true</b> if not equal, <b>false</b> otherwise.</returns>
        public static bool operator !=(Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> left, Deconstructed<TCached1, TCached2, TCached3, TCached4, TNonCached> right) => !left.Equals(right);
    }

}
