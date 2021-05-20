// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class SingleAsync<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;
        private readonly bool _throwOnEmpty;

        public SingleAsync(ISubscribable<TSource> source, Func<TSource, bool> predicate, bool throwOnEmpty)
        {
            Debug.Assert(source != null);

            _source = source;
            _predicate = predicate ?? (_ => true);
            _throwOnEmpty = throwOnEmpty;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SingleAsync<TSource>, TSource>, IObserver<TSource>
        {
            private bool _hasValue;
            private TSource _value;

            public _(SingleAsync<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Single";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (_hasValue)
                {
                    Output.OnNext(_value);
                    Output.OnCompleted();
                }
                else
                {
                    if (Params._throwOnEmpty)
                    {
                        Output.OnError(new InvalidOperationException("Observable has no elements."));
                    }
                    else
                    {
                        Output.OnNext(default);
                        Output.OnCompleted();
                    }
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
                    b = Params._predicate(value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                if (b)
                {
                    if (_hasValue)
                    {
                        Output.OnError(new InvalidOperationException("Observable contains more than one element."));
                        Dispose();
                    }
                    else
                    {
                        _hasValue = true;
                        _value = value;
                        StateChanged = true;
                    }
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
                _value = reader.Read<TSource>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_hasValue);
                writer.Write(_value);
            }
        }
    }
}
