// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class DefaultIfEmpty<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;

        public DefaultIfEmpty(ISubscribable<TSource> source)
        {
            Debug.Assert(source != null);

            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<DefaultIfEmpty<TSource>, TSource>, IObserver<TSource>
        {
            private bool _isNotEmpty;

            public _(DefaultIfEmpty<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:DefaultIfEmpty";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_isNotEmpty)
                {
                    Output.OnNext(default);
                }
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
                _isNotEmpty |= (StateChanged = true);
                Output.OnNext(value);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);
                _isNotEmpty = reader.Read<bool>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);
                writer.Write(_isNotEmpty);
            }
        }
    }
}
