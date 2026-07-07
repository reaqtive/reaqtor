// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public static class PartitionableSubscribable
    {
        public static IPartitionableSubscribable<TSource, TKey, IPartitionableSubscribable<TSource>> AddPartition<TSource, TKey>(this IPartitionableSubscribable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.AddPartition(new FuncKeySelector<TSource, TKey>(keySelector));
        }

        public static IPartitionableSubscribable<TSource, TNextKey, IPartitionableSubscribable<TSource, TKey, TInner>> AddPartition<TSource, TNextKey, TInner, TKey>(this IPartitionableSubscribable<TSource, TKey, TInner> source, Func<TSource, TNextKey> keySelector)
            where TInner : IImmutableBindingHolder<TInner, TSource>
        {
            return source.AddPartition(new FuncKeySelector<TSource, TNextKey>(keySelector));
        }

        private sealed class FuncKeySelector<T, TResult> : IKeySelector<T, TResult>
        {
            private readonly Func<T, TResult> _f;

            public FuncKeySelector(Func<T, TResult> f) => _f = f;

            public TResult Invoke(T arg) => _f(arg);

            public override int GetHashCode() => _f.GetHashCode();

            public override bool Equals(object obj) => obj is FuncKeySelector<T, TResult> fks && _f.Equals(fks._f);
        }
    }
}
