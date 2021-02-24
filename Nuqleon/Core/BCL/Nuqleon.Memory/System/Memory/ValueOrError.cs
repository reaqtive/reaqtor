// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Created ValueOrError functionality.
//

using System.Runtime.ExceptionServices;

namespace System.Memory
{
    /// <summary>
    /// Provides a set of factory methods to create objects that represent a value or an error.
    /// </summary>
    public static class ValueOrError
    {
        /// <summary>
        /// Creates an object that represents a value.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value represented by the object.</param>
        /// <returns>An object representing the specified <paramref name="value"/>.</returns>
        public static IValueOrError<T> CreateValue<T>(T value) => new ValueOrErrorValue<T>(value);

        /// <summary>
        /// Creates an object that represents an error.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="exception">The error represented by the object.</param>
        /// <returns>An object representing the specified <paramref name="exception"/>.</returns>
        public static IValueOrError<T> CreateError<T>(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new ValueOrErrorError<T>(ExceptionDispatchInfo.Capture(exception));
        }
    }
}
