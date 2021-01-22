// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive
{
    /// <summary>
    /// Represents a subscribable source with a grouping key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TSource">Type of the elements produced by the subscribable source.</typeparam>
    public interface IGroupedSubscribable<out TKey, out TSource> : ISubscribable<TSource>
    {
        /// <summary>
        /// Gets the grouping key.
        /// </summary>
        TKey Key { get; }
    }
}
