// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Deployable
{
    [KnownType]
    public class ThroughputObserver<T> : IObserver<T>
    {
        private static readonly object s_appDomainLock = new();
        private readonly Countable _countable;

        public ThroughputObserver(string id)
        {
            _countable = (Countable)AppDomain.CurrentDomain.GetData(id);
            if (_countable == null)
            {
                lock (s_appDomainLock)
                {
                    _countable = (Countable)AppDomain.CurrentDomain.GetData(id);
                    if (_countable == null)
                    {
                        _countable = new Countable();
                        AppDomain.CurrentDomain.SetData(id, _countable);
                    }
                }
            }
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
