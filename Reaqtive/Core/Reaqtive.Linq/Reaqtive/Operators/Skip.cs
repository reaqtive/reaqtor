// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Skip<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly int _skipCount;

        public Skip(ISubscribable<TSource> source, int skipCount)
        {
            Debug.Assert(source != null);

            _source = source;
            _skipCount = skipCount;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Skip<TSource>, TSource>, IObserver<TSource>
        {
            private int _valuesToSkip;

            public _(Skip<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
                _valuesToSkip = parent._skipCount;
            }

            public override string Name => "rc:Skip";

            public override Version Version => Versioning.v1;

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _valuesToSkip = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_valuesToSkip);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

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

            public void OnNext(TSource value)
            {
                if (_valuesToSkip <= 0)
                {
                    Output.OnNext(value);
                }
                else
                {
                    _valuesToSkip--;

                    StateChanged = true;
                }
            }
        }
    }
}
