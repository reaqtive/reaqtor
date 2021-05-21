// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class LastAsync<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, bool>? _predicateOrElseUseNoFilter;
        private readonly bool _throwOnEmpty;

        public LastAsync(ISubscribable<TSource> source, Func<TSource, bool>? predicate, bool throwOnEmpty)
        {
            Debug.Assert(source != null);

            _source = source!;
            _predicateOrElseUseNoFilter = predicate;
            _throwOnEmpty = throwOnEmpty;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<LastAsync<TSource>, TSource>, IObserver<TSource>
        {
            private bool _hasValue;
            private TSource _lastValue = default!;

            public _(LastAsync<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Last";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_hasValue)
                {
                    Output.OnNext(_lastValue);
                    Output.OnCompleted();
                }
                else if (Params._throwOnEmpty)
                {
                    Output.OnError(new InvalidOperationException("Observable has no elements."));
                }
                else
                {
                    Output.OnNext(default!);
                    Output.OnCompleted();
                }

                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(TSource value)
            {
                var b = false;

                try
                {
                    // If predicate is null then don't filter values

                    b = Params._predicateOrElseUseNoFilter == null || Params._predicateOrElseUseNoFilter(value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                if (b)
                {
                    _hasValue = true;
                    _lastValue = value;
                    StateChanged = true;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _lastValue = reader.Read<TSource>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_hasValue);
                writer.Write(_lastValue);
            }
        }
    }
}
