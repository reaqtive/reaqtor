// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Deployable.Streams
{
    public interface IPartitionableSubscribable<TSource, TKey, TInner> : IImmutableBindingHolder<IPartitionableSubscribable<TSource, TKey, TInner>, TSource>
        where TInner : IImmutableBindingHolder<TInner, TSource>
    {
        IKeySelector<TSource, TKey> Selector { get; }

        TInner Parent { get; }
    }
}
