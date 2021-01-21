// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Operators
{
    internal sealed class Finally<T> : SubscribableBase<T>
    {
        private readonly ISubscribable<T> _source;
        private readonly Action _finally;

        public Finally(ISubscribable<T> source, Action @finally)
        {
            _source = source;
            _finally = @finally;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Finally<T>, T>, IObserver<T>
        {
            public _(Finally<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Finally";

            public override Version Version => Versioning.v1;

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

            public void OnNext(T value)
            {
                Output.OnNext(value);
            }

            protected override void OnDispose()
            {
                try
                {
                    base.OnDispose();
                }
                finally
                {
                    Params._finally();
                }
            }
        }
    }
}
