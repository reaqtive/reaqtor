// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class SkipWhile<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public SkipWhile(ISubscribable<TSource> source, Func<TSource, bool> predicate)
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

        private sealed class _ : StatefulUnaryOperator<SkipWhile<TSource>, TSource>, IObserver<TSource>
        {
            private bool _running;

            public _(SkipWhile<TSource> source, IObserver<TSource> observer)
                : base(source, observer)
            {
            }

            public override string Name => "rc:SkipWhile";

            public override Version Version => Versioning.v1;

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _running = reader.Read<bool>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_running);
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

            public void OnNext(TSource value)
            {
                if (!_running)
                {
                    try
                    {
                        if (!Params._predicate(value))
                        {
                            StateChanged = true;
                            _running = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Output.OnError(ex);
                        Dispose();
                        return;
                    }
                }

                if (_running)
                {
                    Output.OnNext(value);
                }
            }
        }
    }

    internal sealed class SkipWhileIndexed<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, int, bool> _indexPredicate;

        public SkipWhileIndexed(ISubscribable<TSource> source, Func<TSource, int, bool> indexPredicate)
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

        private sealed class _ : StatefulUnaryOperator<SkipWhileIndexed<TSource>, TSource>, IObserver<TSource>
        {
            private bool _running;
            private int _currentIndex;

            public _(SkipWhileIndexed<TSource> source, IObserver<TSource> observer)
                : base(source, observer)
            {
            }

            public override string Name => "rc:SkipWhile+Index";

            public override Version Version => Versioning.v1;

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _running = reader.Read<bool>();
                _currentIndex = reader.Read<int>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_running);
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

            public void OnNext(TSource value)
            {
                if (!_running)
                {
                    StateChanged = true;

                    try
                    {
                        _running = !Params._indexPredicate(value, checked(_currentIndex++));
                    }
                    catch (Exception ex)
                    {
                        Output.OnError(ex);
                        Dispose();
                        return;
                    }
                }

                if (_running)
                {
                    Output.OnNext(value);
                }
            }
        }
    }
}
