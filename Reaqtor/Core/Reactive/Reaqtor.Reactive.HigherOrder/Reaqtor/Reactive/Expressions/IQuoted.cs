// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtor.Reactive.Expressions
{
    // CONSIDER: Move to Nuqleon.Linq.CompilerServices if there's a meaningful generalization

    /// <summary>
    /// Represents a quoted value, i.e. a value that has an expression representation attached to it.
    /// </summary>
    /// <typeparam name="T">Type of the quoted value.</typeparam>
    public interface IQuoted<T> : IExpressible
    {
        /// <summary>
        /// Gets the value represented by the quote.
        /// </summary>
        T Value { get; }
    }
}
