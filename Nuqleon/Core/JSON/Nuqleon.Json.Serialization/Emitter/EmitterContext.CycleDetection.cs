// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//   BD - 05/08/2016 - Added support for serialization to text writers.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Nuqleon.Json.Serialization
{
    internal partial class EmitterContext
    {
        /// <summary>
        /// Gets a set to keep object references when serializing objects, used to detect object reference cycles.
        /// </summary>
        public readonly HashSet<object> Cycles = new(ReferenceEqualityComparer.Instance);

        /// <summary>
        /// Clears the cycle tracking set.
        /// </summary>
        private void ClearCycles()
        {
            //
            // NB: We clear the cycles table in case an exception occurs during serialization, leaving
            //     an object in the table.
            //

            Cycles.Clear();
        }

        /// <summary>
        /// Reference equality comparer for objects. This comparer only takes the object reference into account and ignores any specialized hashing or equality behavior.
        /// </summary>
        private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public static readonly IEqualityComparer<object> Instance = new ReferenceEqualityComparer();

            public new bool Equals(object x, object y) => ReferenceEquals(x, y);

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }
    }
}
