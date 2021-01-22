// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

using Reaqtor.Reliable;

namespace Reaqtor.Remoting.QueryEvaluator.Operators
{
    public class ToReliableObservable<T> : IReliableObservable<T>
    {
        private readonly string _name;

        public ToReliableObservable(string name, params T[] values)
        {
            _name = name;
            Values = values;
        }

        public T[] Values
        {
            get => (T[])AppDomain.CurrentDomain.GetData(_name);
            private set => AppDomain.CurrentDomain.SetData(_name, value);
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
}
