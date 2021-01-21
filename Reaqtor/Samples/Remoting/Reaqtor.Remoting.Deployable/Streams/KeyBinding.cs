// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public class KeyBinding<TElement, TKey> : TypeErasedKeyBinding<TElement>
    {
        public KeyBinding(IKeySelector<TElement, TKey> keySelector, TKey key, IEqualityComparer<TKey> keyComparer)
        {
            KeySelector = keySelector;
            Key = key;
            KeyComparer = keyComparer;
        }

        public override Type KeyType => typeof(TKey);

        public IKeySelector<TElement, TKey> KeySelector { get; }

        public TKey Key { get; }

        public IEqualityComparer<TKey> KeyComparer { get; }
    }
}
