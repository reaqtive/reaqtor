// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Select<TSource, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, TResult> _selector;

        public Select(ISubscribable<TSource> source, Func<TSource, TResult> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            _source = source;
            _selector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Select<TSource, TResult>, TResult>, IObserver<TSource>
        {
            public _(Select<TSource, TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Select";

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
                TResult result;
                try
                {
                    result = Params._selector(value);
                }
                catch (Exception exception)
                {
                    Output.OnError(exception);
                    Dispose();
                    return;
                }

                Output.OnNext(result);
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }

    internal sealed class SelectIndexed<TSource, TResult> : SubscribableBase<TResult>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, int, TResult> _indexSelector;

        public SelectIndexed(ISubscribable<TSource> source, Func<TSource, int, TResult> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            _source = source;
            _indexSelector = selector;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<SelectIndexed<TSource, TResult>, TResult>, IObserver<TSource>
        {
            private int _currentIndex;

            public _(SelectIndexed<TSource, TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Select+Index";

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

                TResult result;
                try
                {
                    result = Params._indexSelector(value, checked(_currentIndex++));
                }
                catch (Exception exception)
                {
                    Output.OnError(exception);
                    Dispose();
                    return;
                }

                Output.OnNext(result);
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

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }
        }
    }
}
