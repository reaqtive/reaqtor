// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Take<TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly int _count;

        public Take(ISubscribable<TResult> source, int count)
        {
            Debug.Assert(source != null);
            Debug.Assert(count > 0);

            _source = source;
            _count = count;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Take<TResult>, TResult>, IObserver<TResult>
        {
            private int _remaining;

            public _(Take<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
                _remaining = Params._count;
            }

            public override string Name => "rc:Take";

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
                if (_remaining > 0)
                {
                    --_remaining;
                    StateChanged = true;

                    Output.OnNext(value);
                    if (_remaining == 0)
                    {
                        Output.OnCompleted();
                        Dispose();
                    }
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _remaining = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_remaining);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }
}
