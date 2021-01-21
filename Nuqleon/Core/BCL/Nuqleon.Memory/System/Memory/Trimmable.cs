// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

namespace System.Memory
{
    /// <summary>
    /// Provides a set of methods to work with trimmable objects.
    /// </summary>
    public static class Trimmable
    {
        /// <summary>
        /// Creates a new trimmable object using the specified <paramref name="trim"/> function.
        /// </summary>
        /// <typeparam name="T">The type of the elements held by the trimmable data structure.</typeparam>
        /// <param name="trim">The trim function to use for the trimmable implementation.</param>
        /// <returns>An object implementing the trimmable interface using the specified <paramref name="trim"/> function.</returns>
        public static ITrimmable<T> Create<T>(Func<Func<T, bool>, int> trim)
        {
            if (trim == null)
                throw new ArgumentNullException(nameof(trim));

            return new Impl<T>(trim);
        }

        private sealed class Impl<T> : ITrimmable<T>
        {
            private readonly Func<Func<T, bool>, int> _trim;

            public Impl(Func<Func<T, bool>, int> trim) => _trim = trim;

            public int Trim(Func<T, bool> shouldTrim)
            {
                if (shouldTrim == null)
                    throw new ArgumentNullException(nameof(shouldTrim));

                return _trim(shouldTrim);
            }
        }
    }
}
