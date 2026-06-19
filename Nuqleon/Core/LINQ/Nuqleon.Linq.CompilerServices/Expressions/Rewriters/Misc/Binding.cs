// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Binding of a parameter expression to a value.
    /// </summary>
    public readonly struct Binding : IEquatable<Binding>
    {
        /// <summary>
        /// Creates a new binding with the specified parameter and value.
        /// </summary>
        /// <param name="parameter">Parameter that's bound by the specified value.</param>
        /// <param name="value">Value the specified parameter is bound to.</param>
        public Binding(ParameterExpression parameter, Expression value)
        {
            Parameter = parameter;
            Value = value;
        }

        /// <summary>
        /// Gets the bound parameter.
        /// </summary>
        public ParameterExpression Parameter { get; }

        /// <summary>
        /// Gets the value of the binding.
        /// </summary>
        public Expression Value { get; }

        /// <summary>
        /// Checks if an instance is equal to the current instance.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns>
        /// <b>true</b> if the other instance equals the current instance, <b>false</b> otherwise.
        /// </returns>
        public override bool Equals(object obj) => obj is Binding binding && Equals(binding);

        /// <summary>
        /// Checks if an instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// <b>true</b> if the other instance equals the current instance, <b>false</b> otherwise.
        /// </returns>
        public bool Equals(Binding other) =>
               EqualityComparer<ParameterExpression>.Default.Equals(Parameter, other.Parameter)
            && EqualityComparer<Expression>.Default.Equals(Value, other.Value);

        /// <summary>
        /// Generates a hash code for the instance.
        /// </summary>
        /// <returns>The hash code for the instance.</returns>
        public override int GetHashCode() =>
            HashHelpers.Combine(
                EqualityComparer<ParameterExpression>.Default.GetHashCode(Parameter),
                EqualityComparer<Expression>.Default.GetHashCode(Value)
            );

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>
        /// <b>true</b> if the instances are equal, <b>false</b> otherwise.
        /// </returns>
        public static bool operator ==(Binding left, Binding right) => left.Equals(right);

        /// <summary>
        /// Checks if two instances are not equal.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>
        /// <b>true</b> if the instances are not equal, <b>false</b> otherwise.
        /// </returns>
        public static bool operator !=(Binding left, Binding right) => !left.Equals(right);
    }
}
