// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable.Streams
{
    // IPartitionableSubscribable proof of concept. Does not partition in the subject, instead applies Where filters for partitions.

    public abstract class PartitionableSubscribableBase<TSource> : IPartitionableSubscribable<TSource>
    {
        protected readonly TypeErasedKeyBinding<TSource>[] _bindings;

        protected PartitionableSubscribableBase(IList<TypeErasedKeyBinding<TSource>> bindings)
        {
            _bindings =
                (bindings ?? Array.Empty<TypeErasedKeyBinding<TSource>>()) as TypeErasedKeyBinding<TSource>[]
                ?? (bindings.Count == 0 ? Array.Empty<TypeErasedKeyBinding<TSource>>() : bindings.ToArray());
            Bindings = _bindings.ToReadOnly();
        }

        public IPartitionableSubscribable<TSource> AppendBindings(IList<TypeErasedKeyBinding<TSource>> bindings)
        {
            var appended = new TypeErasedKeyBinding<TSource>[bindings.Count + _bindings.Length];

            var origLen = _bindings.Length;
            Array.Copy(_bindings, appended, origLen);

            for (var i = 0; i < bindings.Count; i++)
                appended[i + origLen] = bindings[i];

            return UpdatePartitionableSubscribable(appended);
        }

        protected abstract IPartitionableSubscribable<TSource> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<TSource>> bindings);

        public ISubscription Subscribe(IObserver<TSource> observer) => SubscribeCore(observer);

        protected abstract ISubscription SubscribeCore(IObserver<TSource> observer);

        IDisposable IObservable<TSource>.Subscribe(IObserver<TSource> observer) => Subscribe(observer);

        public IReadOnlyList<TypeErasedKeyBinding<TSource>> Bindings { get; }
    }

    public abstract class PartitionableSubscribableBase<TSource, TKey, TInner> : IPartitionableSubscribable<TSource, TKey, TInner>
        where TInner : IImmutableBindingHolder<TInner, TSource>
    {
        protected readonly TypeErasedKeyBinding<TSource>[] _bindings;
        protected readonly TInner _source;
        protected readonly IKeySelector<TSource, TKey> _keySelector;

        protected PartitionableSubscribableBase(TInner source, IKeySelector<TSource, TKey> keySelector, IList<TypeErasedKeyBinding<TSource>> bindings)
        {
            _source = source;
            _keySelector = keySelector;
            _bindings = (bindings ?? Array.Empty<TypeErasedKeyBinding<TSource>>()) as TypeErasedKeyBinding<TSource>[]
                ?? (bindings.Count == 0 ? Array.Empty<TypeErasedKeyBinding<TSource>>() : bindings.ToArray());
            Bindings = _bindings.ToReadOnly();
        }

        public TInner Bind(TKey key, IEqualityComparer<TKey> keyComparer)
        {
            var appended = new TypeErasedKeyBinding<TSource>[1 + _bindings.Length];
            appended[0] = new KeyBinding<TSource, TKey>(_keySelector, key, keyComparer);
            Array.Copy(_bindings, 0, appended, 1, _bindings.Length);
            return _source.AppendBindings(appended);
        }

        public IPartitionableSubscribable<TSource, TKey, TInner> AppendBindings(IList<TypeErasedKeyBinding<TSource>> bindings)
        {
            var appended = new TypeErasedKeyBinding<TSource>[bindings.Count + _bindings.Length];

            var origLen = _bindings.Length;
            Array.Copy(_bindings, appended, origLen);

            for (var i = 0; i < bindings.Count; i++)
                appended[i + origLen] = bindings[i];

            return UpdatePartitionableSubscribable(appended);
        }

        protected abstract IPartitionableSubscribable<TSource, TKey, TInner> UpdatePartitionableSubscribable(IList<TypeErasedKeyBinding<TSource>> bindings);

        public IKeySelector<TSource, TKey> Selector => _keySelector;

        public TInner Parent => _source;

        public IReadOnlyList<TypeErasedKeyBinding<TSource>> Bindings { get; }
    }
}
