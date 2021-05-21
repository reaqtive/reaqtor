// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Contains<TSource> : SubscribableBase<bool>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly TSource _element;
        private readonly IEqualityComparer<TSource> _comparer;

        public Contains(ISubscribable<TSource> source, TSource element, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(comparer != null);

            _source = source;
            _element = element;
            _comparer = comparer;
        }

        protected override ISubscription SubscribeCore(IObserver<bool> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Contains<TSource>, bool>, IObserver<TSource>
        {
            public _(Contains<TSource> parent, IObserver<bool> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Contains";

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
                bool equals;

                try
                {
                    equals = Params._comparer.Equals(value, Params._element);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                if (equals)
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
