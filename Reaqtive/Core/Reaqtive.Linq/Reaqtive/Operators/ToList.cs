// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Reaqtive.Operators
{
    internal sealed class ToList<TSource>(ISubscribable<TSource> source) : SubscribableBase<IList<TSource>>
    {
        private readonly ISubscribable<TSource> _source = source;

        protected override ISubscription SubscribeCore(IObserver<IList<TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _(ToList<TSource> parent, IObserver<IList<TSource>> observer) : StatefulUnaryOperator<ToList<TSource>, IList<TSource>>(parent, observer), IObserver<TSource>
        {
            private const string MAXLISTSIZESETTING = "rx://operators/toList/settings/maxListSize";
            private int _maxListSize;

            private List<TSource> _values;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXLISTSIZESETTING, out _maxListSize);
            }

            protected override void OnStart()
            {
                _values ??= [];
            }

            public override string Name => "rc:ToList";

            public override Version Version => Versioning.v1;

            public void OnCompleted()
            {
                Output.OnNext(_values);
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
                if (_values.Count >= _maxListSize)
                {
                    Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of elements accumulated by the ToList operator exceeded {0} items. Please apply the ToList operator to a sequence with less elements, or use a streaming approach to achieve the same effect.", _maxListSize)));
                    Dispose();

                    return;
                }

                _values.Add(value);
                StateChanged = true;
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(this);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<List<TSource>>(_values);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _values = reader.Read<List<TSource>>();
            }
        }
    }
}
