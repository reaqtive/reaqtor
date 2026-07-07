// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Reaqtor.Reliable;

namespace Reaqtor.Remoting.QueryEvaluator.Operators;

public class ToReliableObservable<T> : IReliableObservable<T>
{
    //
    // NB (plan §2.5, adaptation #5): the archived ToReliableObservable<T> shared its Values across (formerly
    //     cross-AppDomain) instances via AppDomain.CurrentDomain.GetData/SetData(_name). There is no AppDomain on
    //     net10.0, so the ledger is replaced with an in-process static store keyed by the same name, exactly as the
    //     ported ThroughputObserver<T> (Reaqtor.Remoting.Deployable.Core) replaces its AppDomain slot. The indexer
    //     get/set on the ConcurrentDictionary gives the same "last writer wins" overwrite that SetData provided and
    //     the same read-back that GetData provided, so every ToReliableObservable<T> constructed with the same name
    //     observes the same values.
    //
    private static readonly ConcurrentDictionary<string, object> s_values = new();

    private readonly string _name;

    public ToReliableObservable(string name, params T[] values)
    {
        _name = name;
        Values = values;
    }

    public IReadOnlyList<T> Values
    {
        get => (IReadOnlyList<T>)s_values[_name];
        private set => s_values[_name] = value;
    }

    public IReliableSubscription Subscribe(IReliableObserver<T> observer)
    {
        return new _(this, observer);
    }

    private sealed class _ : ReliableSubscriptionBase
    {
        private readonly ToReliableObservable<T> _parent;
        private readonly IReliableObserver<T> _observer;


        public _(ToReliableObservable<T> parent, IReliableObserver<T> observer)
        {
            _parent = parent;
            _observer = observer;
        }

        public override Uri ResubscribeUri => throw new NotImplementedException();

        public override void Start(long sequenceId)
        {
            var id = sequenceId;
            foreach (var value in _parent.Values.Skip((int)sequenceId))
            {
                _observer.OnNext(value, id++);
            }
        }

        public override void AcknowledgeRange(long sequenceId)
        {
        }

        public override void DisposeCore()
        {
        }
    }
}
