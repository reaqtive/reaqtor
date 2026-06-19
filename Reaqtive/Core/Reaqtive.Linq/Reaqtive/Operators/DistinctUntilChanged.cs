// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class DistinctUntilChanged<TResult, TKey> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly Func<TResult, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _equalityComparer;

        public DistinctUntilChanged(ISubscribable<TResult> source, Func<TResult, TKey> keySelector, IEqualityComparer<TKey> comparer)
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

        private sealed class _ : StatefulUnaryOperator<DistinctUntilChanged<TResult, TKey>, TResult>, IObserver<TResult>
        {
            private bool _hasCurrentKey;
            private TKey _currentKey;

            public _(DistinctUntilChanged<TResult, TKey> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:DistinctUntilChanged";

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
                var comparerEquals = false;
                try
                {
                    key = Params._keySelector(value);
                    if (_hasCurrentKey)
                    {
                        comparerEquals = Params._equalityComparer.Equals(_currentKey, key);
                    }
                }
                catch (Exception exception)
                {
                    OnError(exception);
                    return;
                }

                if (!_hasCurrentKey || !comparerEquals)
                {
                    _hasCurrentKey = true;
                    _currentKey = key;

                    StateChanged = true;

                    Output.OnNext(value);
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _currentKey = reader.Read<TKey>();
                _hasCurrentKey = reader.Read<bool>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_currentKey);
                writer.Write(_hasCurrentKey);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }
}
