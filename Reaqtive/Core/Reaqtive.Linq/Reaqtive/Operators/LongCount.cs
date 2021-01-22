// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Operators
{
    internal sealed class LongCount<TSource> : SubscribableBase<long>
    {
        private readonly ISubscribable<TSource> _source;

        public LongCount(ISubscribable<TSource> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<long> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<LongCount<TSource>, long>, IObserver<TSource>
        {
            private long _count;

            public _(LongCount<TSource> parent, IObserver<long> observer)
                : base(parent, observer)
            {
                _count = 0;
            }

            public override string Name => "rc:LongCount";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_count);
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
                try
                {
                    checked
                    {
                        StateChanged = true;
                        _count++;
                    }
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _count = reader.Read<long>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<long>(_count);
            }
        }
    }
}
