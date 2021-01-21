// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public static class PartitionableSubscribableExtensions
    {
        public static IPartitionableSubscribable<T> ToPartitionableSubscribable<T>(this ISubscribable<T> source)
        {
            return new SimplePartitionableSubscribable<T>(source);
        }

        public static IPartitionableSubscribable<TSource, TKey, IPartitionableSubscribable<TSource>> AddPartition<TSource, TKey>(this IPartitionableSubscribable<TSource> source, IKeySelector<TSource, TKey> keySelector)
        {
            return new SimplePartitionableSubscribable<TSource, TKey, IPartitionableSubscribable<TSource>>(source, keySelector, null);
        }

        public static IPartitionableSubscribable<TSource, TKey, IPartitionableSubscribable<TSource>> AddPartition<TSource, TKey>(this IPartitionableSubscribable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return new SimplePartitionableSubscribable<TSource, TKey, IPartitionableSubscribable<TSource>>(source, new LambdaKeySelector<TSource, TKey>(keySelector), null);
        }

        public static IPartitionableSubscribable<TSource, TNextKey, IPartitionableSubscribable<TSource, TKey, TInner>> AddPartition<TSource, TKey, TInner, TNextKey>(this IPartitionableSubscribable<TSource, TKey, TInner> source, IKeySelector<TSource, TNextKey> keySelector)
            where TInner : IImmutableBindingHolder<TInner, TSource>
        {
            return new SimplePartitionableSubscribable<TSource, TNextKey, IPartitionableSubscribable<TSource, TKey, TInner>>(source, keySelector, null);
        }

        public static TInner Bind<TSource, TKey, TInner>(this IPartitionableSubscribable<TSource, TKey, TInner> source, TKey key, IEqualityComparer<TKey> keyComparer)
            where TInner : IImmutableBindingHolder<TInner, TSource>
        {
            return source.Parent.AppendBindings(new[] { new KeyBinding<TSource, TKey>(source.Selector, key, keyComparer) }.Concat(source.Bindings).ToList());
        }
    }
}
