// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Scheduler
{
    /// <summary>
    /// Token to observe requests to yield execution.
    /// </summary>
    public readonly struct YieldToken : IEquatable<YieldToken>
    {
        /// <summary>
        /// The yield token source used to check for yield requests. For the default instance
        /// which gets returned by <see cref="None"/>, the field will be set to <c>null</c>.
        /// </summary>
        private readonly IYieldTokenSource _source;

        /// <summary>
        /// Returns an empty <see cref="YieldToken"/> value.
        /// </summary>
        public static YieldToken None => new();

        /// <summary>
        /// Creates a new <see cref="YieldToken"/> value using the specified yield token
        /// <paramref name="source"/> to check for yield requests.
        /// </summary>
        /// <param name="source">The yield token source used to check for yield requests.</param>
        public YieldToken(IYieldTokenSource source) => _source = source;

        /// <summary>
        /// Gets whether yielding has been requested for this token.
        /// </summary>
        public bool IsYieldRequested => _source != null && _source.IsYieldRequested;

        /// <summary>
        /// Determines whether the current <see cref="YieldToken" /> instance is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="other">The other <see cref="YieldToken" /> to which to compare this instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="other" /> is a <see cref="YieldToken" /> and if the two instances are equal; objwise, <c>false</c>.
        /// Two tokens are equal if they are associated with the same token source or if they were both constructed from public <see cref="YieldToken" /> constructors and their <see cref="IsYieldRequested" /> values are equal.
        /// </returns>
        public bool Equals(YieldToken other) => _source == other._source;

        /// <summary>
        /// Determines whether the current <see cref="YieldToken" /> instance is equal to the specified <see cref="Object" />.
        /// </summary>
        /// <param name="obj">The other object to which to compare this instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj" /> is a <see cref="YieldToken" /> and if the two instances are equal; objwise, <c>false</c>.
        /// Two tokens are equal if they are associated with the same token source or if they were both constructed from public <see cref="YieldToken" /> constructors and their <see cref="IsYieldRequested" /> values are equal.
        /// </returns>
        public override bool Equals(object obj) => obj is YieldToken token && Equals(token);

        /// <summary>
        /// Serves as a hash function for a <see cref="YieldToken" />.
        /// </summary>
        /// <returns>A hash code for the current <see cref="YieldToken" /> instance.</returns>
        public override int GetHashCode() => _source == null ? 0 : _source.GetHashCode();

        /// <summary>
        /// Determines whether two <see cref="YieldToken" /> instances are equal.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(YieldToken left, YieldToken right) => left.Equals(right);

        /// <summary>
        /// Determines whether two <see cref="YieldToken" /> instances are not equal.
        /// </summary>
        /// <param name="left">The first instance.</param>
        /// <param name="right">The second instance.</param>
        /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(YieldToken left, YieldToken right) => !left.Equals(right);
    }
}
