// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 05/09/2017 - Generated fast comparer functionality.
//

namespace System.Collections.Generic
{
    /// <summary>
    /// Provides fast access to the default <see cref="IComparer{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to compare.</typeparam>
    /// <remarks>
    /// This type works around performance limitations in the Desktop CLR implementation of
    /// <see cref="Comparer{T}.Default"/> which have been lifted in .NET Core.
    /// </remarks>
    internal static class FastComparer<T>
    {
        /// <summary>
        /// Gets a cached copy of the default comparer instance.
        /// </summary>
        public static Comparer<T> Default { get; } = Comparer<T>.Default;
    }
}
