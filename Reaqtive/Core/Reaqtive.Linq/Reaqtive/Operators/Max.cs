// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtive.Operators
{
    internal sealed class Max<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly IComparer<TSource> _comparer;

        public Max(ISubscribable<TSource> source, IComparer<TSource> comparer)
        {
            _source = source;
            _comparer = comparer;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            if (default(TSource) == null)
            {
                return new N(this, observer);
            }
            else
            {
                return new _(this, observer);
            }
        }

        private sealed class _ : StatefulUnaryOperator<Max<TSource>, TSource>, IObserver<TSource>
        {
            private bool _hasValue;
            private TSource _res;

            public _(Max<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Max";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                }
                else
                {
                    Output.OnNext(_res);
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
                if (_hasValue)
                {
                    bool res;
                    try
                    {
                        res = Params._comparer.Compare(value, _res) > 0;
                    }
                    catch (Exception ex)
                    {
                        Output.OnError(ex);
                        Dispose();
                        return;
                    }

                    if (res)
                    {
                        StateChanged = true;
                        _res = value;
                    }
                }
                else
                {
                    StateChanged = true;
                    _res = value;
                    _hasValue = true;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<TSource>(_res);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _res = reader.Read<TSource>();
            }
        }

        private sealed class N : StatefulUnaryOperator<Max<TSource>, TSource>, IObserver<TSource>
        {
            private TSource _res;

            public N(Max<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:MaxNullable";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_res);
                Output.OnCompleted();
                Dispose();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
            }

            public void OnNext(TSource value)
            {
                bool res;
                try
                {
                    res = value != null && (_res == null || Params._comparer.Compare(value, _res) > 0);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                if (res)
                {
                    _res = value;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<TSource>(_res);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _res = reader.Read<TSource>();
            }
        }
    }
}
