// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Where<TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly Func<TResult, bool> _predicate;

        public Where(ISubscribable<TResult> source, Func<TResult, bool> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            _source = source;
            _predicate = predicate;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Where<TResult>, TResult>, IObserver<TResult>
        {
            public _(Where<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Where";

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
                bool propagate;
                try
                {
                    propagate = Params._predicate(value);
                }
                catch (Exception exception)
                {
                    Output.OnError(exception);
                    Dispose();
                    return;
                }

                if (propagate)
                {
                    Output.OnNext(value);
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }

    internal sealed class WhereIndexed<TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TResult> _source;
        private readonly Func<TResult, int, bool> _indexPredicate;

        public WhereIndexed(ISubscribable<TResult> source, Func<TResult, int, bool> indexPredicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(indexPredicate != null);

            _source = source;
            _indexPredicate = indexPredicate;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<WhereIndexed<TResult>, TResult>, IObserver<TResult>
        {
            private int _currentIndex;

            public _(WhereIndexed<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Where+Index";

            public override Version Version => Versioning.v1;

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
                StateChanged = true;

                bool propagate;
                try
                {
                    propagate = Params._indexPredicate(value, checked(_currentIndex++));
                }
                catch (Exception exception)
                {
                    Output.OnError(exception);
                    Dispose();
                    return;
                }

                if (propagate)
                {
                    Output.OnNext(value);
                }
            }
        }
    }
}
