// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.Operators
{
    internal sealed class Scan<TSource, TAccumulate>(ISubscribable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulate) : SubscribableBase<TAccumulate>
    {
        private readonly ISubscribable<TSource> _source = source;
        private readonly TAccumulate _seed = seed;
        private readonly Func<TAccumulate, TSource, TAccumulate> _accumulate = accumulate;

        protected override ISubscription SubscribeCore(IObserver<TAccumulate> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Scan<TSource, TAccumulate>, TAccumulate>, IObserver<TSource>
        {
            private TAccumulate _result;

            public _(Scan<TSource, TAccumulate> parent, IObserver<TAccumulate> observer)
                : base(parent, observer)
            {
                _result = Params._seed;
            }

            public override string Name => "rc:Scan+Accumulator";

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

                try
                {
                    _result = Params._accumulate(_result, value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                Output.OnNext(_result);
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

    internal sealed class Scan<TSource>(ISubscribable<TSource> source, Func<TSource, TSource, TSource> Scan) : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source = source;
        private readonly Func<TSource, TSource, TSource> _Scan = Scan;

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _(Scan<TSource> parent, IObserver<TSource> observer) : StatefulUnaryOperator<Scan<TSource>, TSource>(parent, observer), IObserver<TSource>
        {
            private bool _hasValue;
            private TSource _result;

            public override string Name => "rc:Scan+Simple";

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

                if (!_hasValue)
                {
                    _result = value;
                    _hasValue = true;
                    return;
                }

                try
                {
                    _result = Params._Scan(_result, value);
                }
                catch (Exception ex)
                {
                    Output.OnError(ex);
                    Dispose();
                    return;
                }

                Output.OnNext(_result);
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
