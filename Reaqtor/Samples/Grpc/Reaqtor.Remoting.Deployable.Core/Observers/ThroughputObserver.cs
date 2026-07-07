// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Threading;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Deployable
{
    [KnownType]
    public class ThroughputObserver<T> : IObserver<T>
    {
        //
        // NB: The archived ThroughputObserver shared its Countable across (formerly cross-AppDomain) observer
        //     instances via AppDomain.CurrentDomain.GetData/SetData(id). There is no AppDomain on net10.0, so the
        //     ledger is replaced with an in-process static store keyed by the same id. GetOrAdd gives the same
        //     one-time, atomic "create if absent" semantics the original lock around the AppDomain slot provided,
        //     so every ThroughputObserver<T> constructed with the same id observes the same Countable.
        //
        private static readonly ConcurrentDictionary<string, Countable> s_countables = new();
        private readonly Countable _countable;

        public ThroughputObserver(string id)
        {
            _countable = s_countables.GetOrAdd(id, _ => new Countable());
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
            _countable.Increment();
        }

        private sealed class Countable : ICountable
        {
            private long _count;

            public long Count => _count;

            public void Increment() => Interlocked.Increment(ref _count);
        }
    }
}
