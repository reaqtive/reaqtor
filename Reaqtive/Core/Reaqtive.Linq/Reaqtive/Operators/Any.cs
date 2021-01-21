// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Operators
{
    internal sealed class Any<TSource> : SubscribableBase<bool>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public Any(ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        protected override ISubscription SubscribeCore(IObserver<bool> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Any<TSource>, bool>, IObserver<TSource>
        {
            public _(Any<TSource> parent, IObserver<bool> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Any";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(false);
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
                    res = Params._predicate(value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                if (res)
                {
                    Output.OnNext(true);
                    Output.OnCompleted();
                    Dispose();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }
}
