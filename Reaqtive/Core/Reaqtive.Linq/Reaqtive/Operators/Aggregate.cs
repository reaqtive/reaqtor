// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Operators
{
    internal sealed class Aggregate<TSource, TAccumulate> : SubscribableBase<TAccumulate>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly TAccumulate _seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulate;

        public Aggregate(ISubscribable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulate)
        {
            _source = source;
            _seed = seed;
            _accumulate = accumulate;
        }

        protected override ISubscription SubscribeCore(IObserver<TAccumulate> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Aggregate<TSource, TAccumulate>, TAccumulate>, IObserver<TSource>
        {
            private TAccumulate _result;

            public _(Aggregate<TSource, TAccumulate> parent, IObserver<TAccumulate> observer)
                : base(parent, observer)
            {
                _result = Params._seed;
            }

            public override string Name => "rc:Aggregate+Accumulator";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_result);
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

                try
                {
                    _result = Params._accumulate(_result, value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _result = reader.Read<TAccumulate>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<TAccumulate>(_result);
            }
        }
    }

    internal sealed class Aggregate<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly Func<TSource, TSource, TSource> _aggregate;

        public Aggregate(ISubscribable<TSource> source, Func<TSource, TSource, TSource> aggregate)
        {
            _source = source;
            _aggregate = aggregate;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Aggregate<TSource>, TSource>, IObserver<TSource>
        {
            private bool _hasValue;
            private TSource _result;

            public _(Aggregate<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Aggregate+Simple";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                if (!_hasValue)
                {
                    Output.OnError(new InvalidOperationException("Sequence contains no elements."));
                }
                else
                {
                    Output.OnNext(_result);
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
                StateChanged = true;

                if (!_hasValue)
                {
                    _result = value;
                    _hasValue = true;
                    return;
                }

                try
                {
                    _result = Params._aggregate(_result, value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                }
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _result = reader.Read<TSource>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<bool>(_hasValue);
                writer.Write<TSource>(_result);
            }
        }
    }
}
