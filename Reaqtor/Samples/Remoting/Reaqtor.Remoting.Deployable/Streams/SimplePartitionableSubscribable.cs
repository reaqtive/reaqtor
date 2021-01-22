// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Reflection;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable.Streams
{
    // IPartitionableSubscribable proof of concept. Does not partition in the subject, instead applies Where filters for partitions.

    public sealed class SimplePartitionableSubscribable<TSource> : PartitionableSubscribableBase<TSource>
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (documents the signature)
        private static readonly MethodInfo s_addFilter = ((MethodInfo)ReflectionHelpers.InfoOf(() => SimplePartitionableSubscribable<TSource>.AddFilter<int>(default(ISubscribable<TSource>), default(KeyBinding<TSource, int>)))).GetGenericMethodDefinition();
#pragma warning restore IDE0034 // Simplify 'default' expression

        private readonly ISubscribable<TSource> _source;

        internal SimplePartitionableSubscribable(ISubscribable<TSource> source)
            : base(null)
        {
            _source = source;
        }

        private SimplePartitionableSubscribable(ISubscribable<TSource> source, IList<TypeErasedKeyBinding<TSource>> bindings)
            : base(bindings)
        {
            _source = source;
        }

        protected override IPartitionableSubscribable<TSource> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<TSource>> bindings)
        {
            return new SimplePartitionableSubscribable<TSource>(_source, bindings);
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            var res = _source;
            foreach (var filter in _bindings)
            {
                var g = s_addFilter.MakeGenericMethod(filter.KeyType);
                res = (ISubscribable<TSource>)g.Invoke(null, new object[] { res, filter });
            }

            return res.Subscribe(observer);
        }

        private static ISubscribable<TSource> AddFilter<TKey>(ISubscribable<TSource> subscribable, KeyBinding<TSource, TKey> binding)
        {
            return subscribable.Where(x => binding.KeyComparer.Equals(binding.KeySelector.Invoke(x), binding.Key));
        }
    }

    public sealed class SimplePartitionableSubscribable<TSource, TKey, TInner> : PartitionableSubscribableBase<TSource, TKey, TInner>
        where TInner : IImmutableBindingHolder<TInner, TSource>
    {
        internal SimplePartitionableSubscribable(TInner source, IKeySelector<TSource, TKey> keySelector, IList<TypeErasedKeyBinding<TSource>> bindings)
            : base(source, keySelector, bindings)
        {
        }

        protected override IPartitionableSubscribable<TSource, TKey, TInner> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<TSource>> bindings)
        {
            return new SimplePartitionableSubscribable<TSource, TKey, TInner>(_source, _keySelector, bindings);
        }
    }
}
