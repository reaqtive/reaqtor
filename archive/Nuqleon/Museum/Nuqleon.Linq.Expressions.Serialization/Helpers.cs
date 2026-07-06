// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;

namespace Nuqleon.Linq.Expressions.Serialization
{
    /// <summary>
    /// Internal helper functions.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Let extension method to allow for fluent expression-centric code.
        /// </summary>
        /// <typeparam name="T">Type of the let binding input.</typeparam>
        /// <typeparam name="R">Type of the let binding result.</typeparam>
        /// <param name="value">Let binding input.</param>
        /// <param name="f">Function to execute under the let binding.</param>
        /// <returns>Let binding result.</returns>
        public static R Let<T, R>(this T value, Func<T, R> f)
        {
            return f(value);
        }
    }
}
