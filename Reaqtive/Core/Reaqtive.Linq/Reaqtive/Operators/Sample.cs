// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class Sample<TSource, TSample> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly ISubscribable<TSample> _sampler;

        public Sample(ISubscribable<TSource> source, ISubscribable<TSample> sampler)
        {
            Debug.Assert(source != null);
            Debug.Assert(sampler != null);

            _source = source;
            _sampler = sampler;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulOperator<Sample<TSource, TSample>, TSource>
        {
            private readonly object _syncLock = new();

            /// <summary>
            /// We will push source notifications only if the sampled value has changed since the last sampling
            /// </summary>
            private bool _hasValue;
            private TSource _lastValue;

            /// <summary>
            /// When the source has completed, we will wait for the sampler to give us the trigger to push the OnCompleted notification
            /// </summary>
            private bool _atSourceEnd;

#pragma warning disable CA2213 // "never disposed." This ends up in Inputs, all of which are disposed by the base class
            private ISubscription _sourceSubscription;
#pragma warning restore CA2213

            public _(Sample<TSource, TSample> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Sample";

            public override Version Version => Versioning.v1;

            public void OnError(Exception error)
            {
                lock (_syncLock)
                {
                    Output.OnError(error);
                    Dispose();
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _hasValue = reader.Read<bool>();
                _lastValue = reader.Read<TSource>();
                _atSourceEnd = reader.Read<bool>();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_hasValue);
                writer.Write(_lastValue);
                writer.Write(_atSourceEnd);
            }

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _sourceSubscription = Params._source.Subscribe(new SourceObserver(this));
                var samplerSubscription = Params._sampler.Subscribe(new SamplerObserver(this));

                return new[] { _sourceSubscription, samplerSubscription };
            }

            private void OnSample()
            {
                lock (_syncLock)
                {
                    if (_hasValue)
                    {
                        Output.OnNext(_lastValue);

                        _lastValue = default;
                        _hasValue = false;

                        StateChanged = true;
                    }

                    if (_atSourceEnd)
                    {
                        Output.OnCompleted();
                        Dispose();
                    }
                }
            }

            private void SourceDone()
            {
                lock (_syncLock)
                {
                    _atSourceEnd = true;
                    _sourceSubscription.Dispose();

                    StateChanged = true;
                }
            }

            private void UpdateLastValue(TSource value)
            {
                lock (_syncLock)
                {
                    _lastValue = value;
                    _hasValue = true;

                    StateChanged = true;
                }
            }

            private sealed class SourceObserver : IObserver<TSource>
            {
                private readonly _ _parent;

                public SourceObserver(_ parent)
                {
                    _parent = parent;
                }

                public void OnCompleted()
                {
                    _parent.SourceDone();
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnNext(TSource value)
                {
                    _parent.UpdateLastValue(value);
                }
            }

            private sealed class SamplerObserver : IObserver<TSample>
            {
                private readonly _ _parent;

                public SamplerObserver(_ parent)
                {
                    _parent = parent;
                }

                public void OnCompleted()
                {
                    _parent.OnSample();
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnNext(TSample value)
                {
                    _parent.OnSample();
                }
            }
        }
    }
}
