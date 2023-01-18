// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Conversion from a reliable observable to a subscribable for consumption of events in a query engine.
    /// Supports checkpointing of sequence number watermarks to support replay of events after failover.
    /// </summary>
    /// <typeparam name="T">The type of the events.</typeparam>
    internal sealed class ReliableSubscriptionInput<T> : SubscribableBase<T>
    {
        private readonly IReliableObservable<T> _source;

        private readonly bool _switchContext;

        /// <summary>
        /// Creates a new reliable observable converter.
        /// </summary>
        /// <param name="source">The reliable observable to subscribe to.</param>
        /// <param name="switchContext">true if events need to be context switched into the engine via the scheduler; otherwise, false.</param>
        public ReliableSubscriptionInput(IReliableObservable<T> source, bool switchContext)
        {
            _source = source;
            _switchContext = switchContext;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            if (_switchContext)
            {
                return new _ContextSwitched(this, observer);
            }
            else
            {
                return new _(this, observer);
            }
        }

        private sealed class _ : StatefulOperator<ReliableSubscriptionInput<T>, T>, IReliableObserver<T>
        {
            private IReliableSubscription _subscription;

            public _(ReliableSubscriptionInput<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
                CheckpointWatermark = -1;
                HighWatermark = CheckpointWatermark;
            }

            public override string Name => "rcr:SubscribableInput";

            public override Version Version => Versioning.v1;

            public long CheckpointWatermark
            {
                get;
                private set;
            }

            public long HighWatermark
            {
                get;
                private set;
            }

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId)
            {
                Output.OnNext(item);
                HighWatermark = sequenceId;
                StateChanged = true;
            }

            public void OnStarted()
            {
                // TODO
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                Output.OnError(error);
                Dispose();
                StateChanged = true;
            }

            public void OnCompleted()
            {
                Output.OnCompleted();
                Dispose();
                StateChanged = true;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                CheckpointWatermark = reader.Read<long>();
                HighWatermark = CheckpointWatermark;
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                CheckpointWatermark = HighWatermark;
                writer.Write(CheckpointWatermark);
            }

            public override void OnStateSaved()
            {
                base.OnStateSaved();

                _subscription.AcknowledgeRange(CheckpointWatermark);
            }

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _subscription = Params._source.Subscribe(this);
                return new ISubscription[] { _subscription };
            }

            protected override void OnStart()
            {
                _subscription.Start(CheckpointWatermark + 1);
            }
        }

        // TODO: Reduce duplicated code.
        private sealed class _ContextSwitched : ContextSwitchOperator<ReliableSubscriptionInput<T>, T>, IReliableObserver<T>
        {
            private IReliableSubscription _subscription;
            private IOperatorContext _context;

            public _ContextSwitched(ReliableSubscriptionInput<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
                CheckpointWatermark = -1;
                HighWatermark = CheckpointWatermark;
            }

            public override string Name => "rcr:SubscribableInput+Switch";

            public override Version Version => Versioning.v1;

            public long CheckpointWatermark
            {
                get;
                private set;
            }

            public long HighWatermark
            {
                get;
                private set;
            }

            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(T item, long sequenceId)
            {
                base.OnNext(item);
                HighWatermark = sequenceId;
                StateChanged = true;
            }

            public void OnStarted()
            {
                // TODO
                throw new NotImplementedException();
            }

            public override void OnError(Exception error)
            {
                base.OnError(error);
                _subscription.Dispose(); // only dispose the subscription; otherwise, we'd kill the scheduler's work too
                StateChanged = true;
            }

            public override void OnCompleted()
            {
                base.OnCompleted();
                _subscription.Dispose(); // only dispose the subscription; otherwise, we'd kill the scheduler's work too
                StateChanged = true;
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                CheckpointWatermark = reader.Read<long>();
                HighWatermark = CheckpointWatermark;
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                CheckpointWatermark = HighWatermark;
                writer.Write(CheckpointWatermark);
            }

            public override void OnStateSaved()
            {
                base.OnStateSaved();

                _subscription.AcknowledgeRange(CheckpointWatermark);

                _context?.TraceSource.ReliableSubscriptionInput_OnStateSaved(_context.InstanceId, CheckpointWatermark);
            }

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                var baseSubscription = base.OnSubscribe();
                _subscription = Params._source.Subscribe(this);

                return baseSubscription.Concat(new ISubscription[] { _subscription });
            }

            protected override void OnStart()
            {
                _subscription.Start(CheckpointWatermark + 1);

                _context?.TraceSource.ReliableSubscriptionInput_OnStart(_context.InstanceId, CheckpointWatermark + 1);
            }
        }
    }
}
