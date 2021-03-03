// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting
{
    /// <summary>
    /// Contract mainly used to check parameters in methods.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Checks that the condition is not false if it is throws exception
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">The condition to check.</param>
        private static void Requires<TException>(bool condition) where TException : Exception, new()
        {
            if (condition == false)
            {
                throw new TException();
            }
        }

        /// <summary>
        /// Requires the object is not null.
        /// </summary>
        /// <param name="obj">The object to check for not null</param>
        public static void RequiresNotNull(object obj)
        {
            Requires<ArgumentNullException>(obj != null);
        }

        /// <summary>
        /// Requires that the string is not null or empty.
        /// </summary>
        /// <param name="s">The string to check for not null or empty.</param>
        public static void RequiresNotNullOrEmpty(string s)
        {
            Requires<ArgumentNullException>(string.IsNullOrEmpty(s) == false);
        }
    }
}
