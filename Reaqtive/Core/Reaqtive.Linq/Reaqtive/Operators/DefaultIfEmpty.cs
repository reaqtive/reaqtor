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
        private readonly TSource _defaultValue;

        public DefaultIfEmpty(ISubscribable<TSource> source, TSource defaultValue = default)
        {
            Debug.Assert(source != null);

            _source = source;
            _defaultValue = defaultValue;
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
                    Output.OnNext(Params._defaultValue);
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
                if (!_isNotEmpty)
                {
                    _isNotEmpty = true;
                    StateChanged = true;
                }

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
