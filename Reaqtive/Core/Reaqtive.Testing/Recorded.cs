// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Reaqtive.Testing
{
    /// <summary>
    /// Record of a value including the virtual time it was produced on.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    [DebuggerDisplay("{Value}@{Time}")]
#if !NO_SERIALIZABLE
    [Serializable]
#endif
    public struct Recorded<T> : IEquatable<Recorded<T>>
    {
        /// <summary>
        /// Gets the virtual time the value was produced on.
        /// </summary>
        public long Time { get; }

        /// <summary>
        /// Gets the recorded value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Creates a new object recording the production of the specified value at the given virtual time.
        /// </summary>
        /// <param name="time">Virtual time the value was produced on.</param>
        /// <param name="value">Value that was produced.</param>
        public Recorded(long time, T value)
        {
            Time = time;
            Value = value;
        }

        /// <summary>
        /// Checks whether the given recorded object is equal to the current instance.
        /// </summary>
        /// <param name="other">Recorded object to check for equality.</param>
        /// <returns>true if both objects are equal; false otherwise.</returns>
        public bool Equals(Recorded<T> other) => Time == other.Time && EqualityComparer<T>.Default.Equals(Value, other.Value);

        /// <summary>
        /// Determines whether the two specified <see cref="Recorded{T}"/> values have the same Time and Value.
        /// </summary>
        /// <param name="left">The first <see cref="Recorded{T}"/> value to compare.</param>
        /// <param name="right">The second <see cref="Recorded{T}"/> value to compare.</param>
        /// <returns>true if the first <see cref="Recorded{T}"/> value has the same Time and Value as the second <see cref="Recorded{T}"/> value; otherwise, false.</returns>
        public static bool operator ==(Recorded<T> left, Recorded<T> right) => left.Equals(right);

        /// <summary>
        /// Determines whether the two specified <see cref="Recorded{T}"/> values don't have the same Time and Value.
        /// </summary>
        /// <param name="left">The first <see cref="Recorded{T}"/> value to compare.</param>
        /// <param name="right">The second <see cref="Recorded{T}"/> value to compare.</param>
        /// <returns>true if the first <see cref="Recorded{T}"/> value has a different Time or Value as the second <see cref="Recorded{T}"/> value; otherwise, false.</returns>
        public static bool operator !=(Recorded<T> left, Recorded<T> right) => !left.Equals(right);

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current <see cref="Recorded{T}"/> value.
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current <see cref="Recorded{T}"/> value.</param>
        /// <returns>true if the specified System.Object is equal to the current <see cref="Recorded{T}"/> value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Recorded<T> recorded && Equals(recorded);

        /// <summary>
        /// Returns the hash code for the current <see cref="Recorded{T}"/> value.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Recorded{T}"/> value.</returns>
        public override int GetHashCode() => Time.GetHashCode() + EqualityComparer<T>.Default.GetHashCode(Value);

        /// <summary>
        /// Returns a string representation of the current <see cref="Recorded{T}"/> value.
        /// </summary>
        /// <returns>String representation of the current <see cref="Recorded{T}"/> value.</returns>
        public override string ToString() => Value.ToString() + "@" + Time.ToString(CultureInfo.CurrentCulture);
    }
}
