// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// A bundle for data related to <see cref="CommandVerb.New"/> commands.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    public struct NewCommandData<TExpression> : IEquatable<NewCommandData<TExpression>>
    {
        private const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

        /// <summary>
        /// The URI of the resource to create.
        /// </summary>
        public readonly Uri Uri;

        /// <summary>
        /// The expression representing the resource to create.
        /// </summary>
        public readonly TExpression Expression;

        /// <summary>
        /// The state object of the resource to create.
        /// </summary>
        public readonly object State;

        /// <summary>
        /// Instantiates the bundle.
        /// </summary>
        /// <param name="uri">The URI of the resource to create.</param>
        /// <param name="expression">The expression representing the resource to create.</param>
        /// <param name="state">The state object of the resource to create.</param>
        public NewCommandData(Uri uri, TExpression expression, object state)
        {
            Uri = uri;
            Expression = expression;
            State = state;
        }

        /// <summary>
        /// Checks if the current instance is equal to some other instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>True if the current instance equals the other instance, false otherwise.</returns>
        public bool Equals(NewCommandData<TExpression> other) =>
               Uri == other.Uri
            && EqualityComparer<TExpression>.Default.Equals(Expression, other.Expression)
            && State == other.State;

        /// <summary>
        /// Checks if the current instance is equal to some other object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the current instance equals the other object, false otherwise.</returns>
        public override bool Equals(object obj) => obj is NewCommandData<TExpression> data && Equals(data);

        /// <summary>
        /// Gets a hash code for the bundle.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hash = Uri.GetHashCode();
            unchecked
            {
                hash = (int)(hash * Prime) + Expression.GetHashCode();
                return (int)(hash * Prime) + State.GetHashCode();
            }
        }

        /// <summary>
        /// Checks if two instances are equal.
        /// </summary>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance.</param>
        /// <returns>True if the two instances are equal, false otherwise.</returns>
        public static bool operator ==(NewCommandData<TExpression> x, NewCommandData<TExpression> y) => x.Equals(y);

        /// <summary>
        /// Checks if two instances are not equal.
        /// </summary>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The second instance.</param>
        /// <returns>True if the two instances are not equal, false otherwise.</returns>
        public static bool operator !=(NewCommandData<TExpression> x, NewCommandData<TExpression> y) => !x.Equals(y);
    }
}
