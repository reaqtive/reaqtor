// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Provides a set of extension methods for arrays.
    /// </summary>
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Copies the specified array without the first element.
        /// </summary>
        /// <typeparam name="T">the type of the elements in the array.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// <returns>A new array containing the elements of the original array, except for the first one.</returns>
        public static T[] RemoveFirst<T>(this T[] array)
        {
            var length = array.Length;

            Debug.Assert(length > 0, "Expected an array with at least one element.");

            //
            // Return a singleton whenever we can. This method does not provide any
            // guarantee about the referential uniqueness of the result.
            //
            if (length == 1)
            {
                return Array.Empty<T>();
            }

            var res = new T[length - 1];
            Array.Copy(array, 1, res, 0, length - 1);
            return res;
        }

        /// <summary>
        /// Projects the elements of the specified array using the specified selector function.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source array.</typeparam>
        /// <typeparam name="R">The type of the elements in the result array.</typeparam>
        /// <param name="array">The array whose elements to project.</param>
        /// <param name="selector">The selector function to apply to the elements in the source array.</param>
        /// <returns>An array whose elements are the result of applying the selector function to the corresponding elements in the source array.</returns>
        public static R[] Map<T, R>(this T[] array, Func<T, R> selector)
        {
            var length = array.Length;

            //
            // Return a singleton whenever we can. This method does not provide any
            // guarantee about the referential uniqueness of the result.
            //
            if (length == 0)
            {
                return Array.Empty<R>();
            }

            var res = new R[length];

            //
            // NB: Using `array.Length` for the loop condition to allow for the JIT to
            //     eliminate some bounds checks by recognizing the array iteration pattern.
            //
            for (var i = 0; i < array.Length; i++)
            {
                res[i] = selector(array[i]);
            }

            return res;
        }
    }
}
