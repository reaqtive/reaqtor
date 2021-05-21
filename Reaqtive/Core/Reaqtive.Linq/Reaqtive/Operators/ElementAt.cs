// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class ElementAt<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly int _index;
        private readonly bool _throwIfNotFound;

        public ElementAt(ISubscribable<TSource> source, int index, bool throwIfNotFound)
        {
            Debug.Assert(source != null);
            Debug.Assert(index >= 0);

            _source = source;
            _index = index;
            _throwIfNotFound = throwIfNotFound;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<ElementAt<TSource>, TSource>, IObserver<TSource>
        {
            private int _currentIndex;

            public _(ElementAt<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:ElementAt";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (Params._throwIfNotFound)
                {
                    Output.OnError(new ArgumentOutOfRangeException("The element at the specified index was not found."));
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
                if (_currentIndex == Params._index)
                {
                    Output.OnNext(value);
                    Output.OnCompleted();
                    Dispose();
                }
                else
                {
                    checked
                    {
                        _currentIndex++;
                    }
                    StateChanged = true;
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _currentIndex = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_currentIndex);
            }
        }
    }
}
