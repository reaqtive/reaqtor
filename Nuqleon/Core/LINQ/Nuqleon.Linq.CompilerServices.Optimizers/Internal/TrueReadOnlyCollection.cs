// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Read-only collection whose inner collection is unreachable, so it cannot be mutated.
    /// Upon creating an instance of this type, the ownership of the specified collection is transferred.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the collection.</typeparam>
    internal sealed class TrueReadOnlyCollection<T> : ReadOnlyCollection<T>
    {
        /// <summary>
        /// Creates a new read-only wrapper around the specified inner collection.
        /// </summary>
        /// <param name="list">Inner collection to wrap in a read-only container. Ownership of this collection should be transferred upon calling this constructor.</param>
        public TrueReadOnlyCollection(IList<T> list)
            : base(list)
        {
        }
    }
}
