// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class TakeWhile<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public TakeWhile(ISubscribable<TSource> source, Func<TSource, bool> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            _source = source;
            _predicate = predicate;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<TakeWhile<TSource>, TSource>, IObserver<TSource>
        {
            public _(TakeWhile<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:TakeWhile";

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

            public void OnNext(TSource value)
            {
                var running = true;

                try
                {
                    running = Params._predicate(value);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                    return;
                }

                if (running)
                {
                    Output.OnNext(value);
                }
                else
                {
                    OnCompleted();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }

    internal sealed class TakeWhileIndexed<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, int, bool> _indexPredicate;

        public TakeWhileIndexed(ISubscribable<TSource> source, Func<TSource, int, bool> indexPredicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(indexPredicate != null);

            _source = source;
            _indexPredicate = indexPredicate;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<TakeWhileIndexed<TSource>, TSource>, IObserver<TSource>
        {
            private int _currentIndex;

            public _(TakeWhileIndexed<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:TakeWhile+Index";

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

            public void OnNext(TSource value)
            {
                StateChanged = true;

                var running = true;

                try
                {
                    running = Params._indexPredicate(value, checked(_currentIndex++));
                }
                catch (Exception ex)
                {
                    OnError(ex);
                    return;
                }

                if (running)
                {
                    Output.OnNext(value);
                }
                else
                {
                    Output.OnCompleted();
                    Dispose();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_currentIndex);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _currentIndex = reader.Read<int>();
            }
        }
    }
}
