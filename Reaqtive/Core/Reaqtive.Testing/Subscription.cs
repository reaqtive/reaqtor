﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;
using System.Diagnostics;
using System.Globalization;

namespace Reaqtive.Testing
{
    /// <summary>
    /// Records information about subscriptions to and unsubscriptions from observable sequences.
    /// </summary>
    [DebuggerDisplay("({Subscribe}, {Unsubscribe})")]
#if !NO_SERIALIZABLE
    [Serializable]
#endif
    public struct Subscription : IEquatable<Subscription>
    {
        /// <summary>
        /// Infinite virtual time value, used to indicate an unsubscription never took place.
        /// </summary>
        public const long Infinite = long.MaxValue;

        /// <summary>
        /// Gets the subscription virtual time.
        /// </summary>
        public long Subscribe { get; }

        /// <summary>
        /// Gets the unsubscription virtual time.
        /// </summary>
        public long Unsubscribe { get; }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription time.
        /// </summary>
        /// <param name="subscribe">Virtual time at which the subscription occurred.</param>-
        public Subscription(long subscribe)
        {
            Subscribe = subscribe;
            Unsubscribe = Infinite;
        }

        /// <summary>
        /// Creates a new subscription object with the given virtual subscription and unsubscription time.
        /// </summary>
        /// <param name="subscribe">Virtual time at which the subscription occurred.</param>
        /// <param name="unsubscribe">Virtual time at which the unsubscription occurred.</param>
        public Subscription(long subscribe, long unsubscribe)
        {
            Subscribe = subscribe;
            Unsubscribe = unsubscribe;
        }

        /// <summary>
        /// Checks whether the given subscription is equal to the current instance.
        /// </summary>
        /// <param name="other">Subscription object to check for equality.</param>
        /// <returns>true if both objects are equal; false otherwise.</returns>
        public bool Equals(Subscription other) => Subscribe == other.Subscribe && Unsubscribe == other.Unsubscribe;

        /// <summary>
        /// Determines whether the two specified Subscription values have the same Subscribe and Unsubscribe.
        /// </summary>
        /// <param name="left">The first Subscription value to compare.</param>
        /// <param name="right">The second Subscription value to compare.</param>
        /// <returns>true if the first Subscription value has the same Subscribe and Unsubscribe as the second Subscription value; otherwise, false.</returns>
        public static bool operator ==(Subscription left, Subscription right) => left.Equals(right);

        /// <summary>
        /// Determines whether the two specified Subscription values don't have the same Subscribe and Unsubscribe.
        /// </summary>
        /// <param name="left">The first Subscription value to compare.</param>
        /// <param name="right">The second Subscription value to compare.</param>
        /// <returns>true if the first Subscription value has a different Subscribe or Unsubscribe as the second Subscription value; otherwise, false.</returns>
        public static bool operator !=(Subscription left, Subscription right) => !left.Equals(right);

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current Subscription value.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current Subscription value.</param>
        /// <returns>true if the specified System.Object is equal to the current Subscription value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Subscription subscription && Equals(subscription);

        /// <summary>
        /// Returns the hash code for the current Subscription value.
        /// </summary>
        /// <returns>A hash code for the current Subscription value.</returns>
        public override int GetHashCode() => Subscribe.GetHashCode() ^ Unsubscribe.GetHashCode();

        /// <summary>
        /// Returns a string representation of the current Subscription value.
        /// </summary>
        /// <returns>String representation of the current Subscription value.</returns>
        public override string ToString()
        {
            if (Unsubscribe == Infinite)
                return string.Format(CultureInfo.CurrentCulture, "({0}, Infinite)", Subscribe);
            else
                return string.Format(CultureInfo.CurrentCulture, "({0}, {1})", Subscribe, Unsubscribe);
        }
    }
}
