// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

using Reaqtor.Reliable;
using Reaqtor.Shebang.Service;

namespace Reaqtor.Shebang.Extensions
{
    //
    // Similar to EgressObserver<T> but receives events from the outside world.
    //

    internal sealed class IngressObservable<T> : ISubscribable<T>
    {
        private readonly string _name;

        public IngressObservable(string name) => _name = name;

        public ISubscription Subscribe(IObserver<T> observer) => new Subscription(this, observer);

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer) => throw new NotSupportedException();

        private sealed class Subscription : ContextSwitchOperator<IngressObservable<T>, T>, IReliableObserver<T>, IUnloadableOperator
        {
            private IReliableSubscription _subscription;
            private long _sequenceId;
            private long _watermark;

            public Subscription(IngressObservable<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
                _sequenceId = -1; // NB: To start from the event that is published rather than replaying all of the history (when using 0).
            }

            public override string Name => "sb:Ingress";

            public override Version Version => new(1, 0, 0, 0);

            Uri IReliableObserver<T>.ResubscribeUri => throw new NotImplementedException();

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                if (!context.TryGetElement<IIngressEgressManager>("IngressEgressManager", out var iemgr))
                {
                    throw new InvalidOperationException("Ingress/egress manager not found");
                }

                _subscription = iemgr.GetObservable<T>(Params._name).Subscribe(this);
            }

            protected override void OnStart()
            {
                base.OnStart();

                if (_sequenceId > long.MinValue)
                {
                    _subscription.Start(_sequenceId);
                }
            }

            protected override void OnDispose()
            {
                base.OnDispose();

                _subscription?.Dispose();
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_sequenceId);
                _watermark = _sequenceId;
            }

            public override void OnStateSaved()
            {
                base.OnStateSaved();

                _subscription.AcknowledgeRange(_watermark);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _sequenceId = reader.Read<long>();
            }

            void IReliableObserver<T>.OnNext(T item, long sequenceId)
            {
                OnNext(item);

                _sequenceId = sequenceId;
                StateChanged = true;
            }

            void IReliableObserver<T>.OnStarted() { }

            void IReliableObserver<T>.OnError(Exception error)
            {
                OnError(error);

                _sequenceId = long.MinValue;
                StateChanged = true;
            }

            void IReliableObserver<T>.OnCompleted()
            {
                OnCompleted();

                _sequenceId = long.MinValue;
                StateChanged = true;
            }

            public void Unload() => _subscription?.Dispose();
        }
    }
}
