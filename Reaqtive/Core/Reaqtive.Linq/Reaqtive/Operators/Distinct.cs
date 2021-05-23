// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

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
            private const string MAXDISTINCTELEMENTS = "rx://operators/distinct/settings/maxDistinctElements";
            private int _maxDistinctElements;

            private HashSet<TKey> _keySet;

            public _(Distinct<TResult, TKey> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXDISTINCTELEMENTS, out _maxDistinctElements);
            }

            protected override void OnStart()
            {
                _keySet ??= new HashSet<TKey>(Params._equalityComparer);
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
                bool added;

                try
                {
                    TKey key = Params._keySelector(value);
                    added = _keySet.Add(key);
                }
                catch (Exception exception)
                {
                    OnError(exception);
                    Dispose();
                    return;
                }

                if (added)
                {
                    if (_keySet.Count > _maxDistinctElements)
                    {
                        OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of distinct elements produced by the Distinct operator exceeded {0} items. Please adjust the Distinct operator parameters to avoid exceeding this limit.", _maxDistinctElements)));
                        Dispose();
                    }
                    else
                    {
                        StateChanged = true;
                        Output.OnNext(value);
                    }
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
