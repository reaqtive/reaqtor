// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Reactive
{
    /// <summary>
    /// A proxy for group subjects.
    /// </summary>
    /// <typeparam name="TKey">Type of the key associated with the subject.</typeparam>
    /// <typeparam name="TSource">Type of the elements processed by the subject.</typeparam>
    public sealed class GroupedMultiSubjectProxy<TKey, TSource> : MultiSubjectProxy<TSource, TSource>, IGroupedMultiSubject<TKey, TSource>
    {
        /// <summary>
        /// Create the subject proxy.
        /// </summary>
        /// <param name="uri">The subject URI.</param>
        /// <param name="key">The grouping key.</param>
        public GroupedMultiSubjectProxy(Uri uri, TKey key)
            : base(uri)
        {
            Key = key;
        }

        /// <summary>
        /// Gets the grouping key.
        /// </summary>
        public TKey Key { get; }
    }
}
