// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

namespace System
{
    /// <summary>
    /// Provides a set of extension methods for System.Object.
    /// </summary>
    public static class ObjectExtensions
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1720 // Identifier 'object' contains type name. (Mirroring expression tree APIs.)

        /// <summary>
        /// Performs a let binding, applying the specified function to the specified object and returning its result.
        /// This method allows for fluent expression syntax when writing expressions over objects.
        /// </summary>
        /// <typeparam name="TArg">Type of the object to apply the function to.</typeparam>
        /// <typeparam name="TResult">Return type of the function.</typeparam>
        /// <param name="object">Object to apply the function to.</param>
        /// <param name="function">Function to apply to the object.</param>
        /// <returns>Result of applying the specified function to the specified object.</returns>
        public static TResult Let<TArg, TResult>(this TArg @object, Func<TArg, TResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            return function(@object);
        }

#pragma warning restore CA1720
#pragma warning restore IDE0079
    }
}
