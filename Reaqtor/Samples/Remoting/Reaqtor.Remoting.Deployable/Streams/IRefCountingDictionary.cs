// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.Remoting.Deployable.Streams
{
    internal interface IRefCountingDictionary<TKey, TValue>
    {
        bool Release(TKey key, out TValue value);

        TValue AddRef(TKey key, Func<TKey, TValue> factory);

        bool TryGetValue(TKey key, out TValue value);

        IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
    }
}
