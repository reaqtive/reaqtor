// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2013 - Created this file.
//

namespace Reaqtor
{
    /// <summary>
    /// Interface for grouped observables.
    /// </summary>
    /// <typeparam name="TKey">Type of the key of the group.</typeparam>
    /// <typeparam name="TElement">Type of the data produced by the observable.</typeparam>
    public interface IReactiveGroupedObservable<out TKey, out TElement> : IReactiveObservable<TElement>
    {
        /// <summary>
        /// Gets the key of the group.
        /// </summary>
        TKey Key { get; }
    }
}
