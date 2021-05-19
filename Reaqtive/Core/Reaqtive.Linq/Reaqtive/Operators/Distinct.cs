// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Distinct<TResult, TKey> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly Func<TResult, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _equalityComparer;

        public Distinct(ISubscribable<TResult> source, Func<TResult, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);
            Debug.Assert(comparer != null);

            _source = source;
            _keySelector = keySelector;
            _equalityComparer = comparer;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Distinct<TResult, TKey>, TResult>, IObserver<TResult>
        {
            private HashSet<TKey> _keySet;

            public _(Distinct<TResult, TKey> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            protected override void OnStart()
            {
                if (_keySet == null)
                {
                    _keySet = new HashSet<TKey>(Params._equalityComparer);
                }
            }

            public override string Name => "rc:Distinct";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(TResult value)
            {
                TKey key;
                var added = false;
                try
                {
                    key = Params._keySelector(value);
                    added = _keySet.Add(key);
                }
                catch (Exception exception)
                {
                    OnError(exception);
                    return;
                }

                if (added)
                {
                    StateChanged = true;

                    Output.OnNext(value);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _keySet = reader.Read<HashSet<TKey>>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_keySet);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }
}
