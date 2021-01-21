// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class FirstAsync<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;
        private readonly bool _throwOnEmpty;

        public FirstAsync(ISubscribable<TSource> source, Func<TSource, bool> predicate, bool throwOnEmpty)
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

        private sealed class _ : StatefulUnaryOperator<FirstAsync<TSource>, TSource>, IObserver<TSource>
        {
            public _(FirstAsync<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:First";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
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
                    Output.OnNext(value);
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
