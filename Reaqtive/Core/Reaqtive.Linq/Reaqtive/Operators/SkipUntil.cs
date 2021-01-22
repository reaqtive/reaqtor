// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Reaqtive.Operators
{
    internal sealed class SkipUntil<TSource, TOther> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;
        private readonly ISubscribable<TOther> _other;
        private readonly bool _terminateEarly;

        public SkipUntil(ISubscribable<TSource> source, ISubscribable<TOther> other, bool terminateEarly = false)
        {
            Debug.Assert(source != null);
            Debug.Assert(other != null);

            _source = source;
            _other = other;
            _terminateEarly = terminateEarly;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulOperator<SkipUntil<TSource, TOther>, TSource>, IObserver<TSource>
        {
            private OtherObserver _otherObserver;
            private SingleAssignmentSubscription _firstSubscription;

            public _(SkipUntil<TSource, TOther> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SkipUntil";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                _firstSubscription = new SingleAssignmentSubscription();
                var otherSubscription = new SingleAssignmentSubscription();

                var firstObserver = new FirstObserver(this, _firstSubscription);
                _otherObserver = new OtherObserver(this, firstObserver, otherSubscription);
                _firstSubscription.Subscription = Params._source.Subscribe(firstObserver);
                otherSubscription.Subscription = Params._other.Take(1).Subscribe(_otherObserver);

                return new[] { _firstSubscription, otherSubscription };
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
                Output.OnNext(value);
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write(_otherObserver.HasSignaled);
                writer.Write(_firstSubscription.IsDisposed);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                var signaled = reader.Read<bool>();
                var firstDisposed = reader.Read<bool>();

                if (firstDisposed)
                {
                    _firstSubscription.Dispose();
                }

                if (signaled)
                {
                    _otherObserver.MarkSignaled();
                }
            }

            /// <summary>
            /// Observer implementation to be attached to the first observable. 
            /// It uses underneath another instance of <see cref="IObserver{TFirst}"/> to push each of the notifications that it receives. 
            /// Initially it will use an observer that will discard all messages until the second observable signals (at which point it will start push notifications to the SkipUntil observer)
            /// </summary>
            private sealed class FirstObserver : IObserver<TSource>
            {
                public volatile IObserver<TSource> _observer;

                private readonly _ _parent;
                private readonly ISubscription _subscription;

                public FirstObserver(_ parent, ISubscription subscription)
                {
                    _observer = NopObserver<TSource>.Instance;
                    _parent = parent;
                    _subscription = subscription;
                }

                public IObserver<TSource> Observer
                {
                    get => _observer;
                    set => _observer = value;
                }

                public void OnCompleted()
                {
                    var observer = _parent.Params._terminateEarly ? _parent.Output : _observer;
                    observer.OnCompleted();
                    _subscription.Dispose();
                }

                public void OnError(Exception error)
                {
                    // directly notify the SkipUntil subscriber about the error
                    _parent.OnError(error);
                }

                public void OnNext(TSource value)
                {
                    // use the proxy observer here to forward the notifications. 
                    // It will forward to SkipUntil observer if the second observable has triggered, otherwise the notification will be discarded
                    _observer.OnNext(value);
                }
            }

            /// <summary>
            /// Observer implementation to be attached to the second observable. 
            /// Once the second observable signals (OnNext or OnCompleted) it notifies the First observer to start pushing its notifications to the SkipUntil subscriber
            /// </summary>
            private sealed class OtherObserver : IObserver<TOther>
            {
                private readonly FirstObserver _firstObserver;
                private readonly _ _skipUntilObserver;
                private readonly ISubscription _subscription;

                public OtherObserver(_ skipUntilOperator, FirstObserver firstObserver, ISubscription subscription)
                {
                    _skipUntilObserver = skipUntilOperator;
                    _firstObserver = firstObserver;
                    _subscription = subscription;
                }

                public void OnCompleted()
                {
                    _subscription.Dispose();
                }

                public void OnError(Exception error)
                {
                    // this will also dispose the SkipUntil subscription
                    _skipUntilObserver.OnError(error);
                }

                public void OnNext(TOther value)
                {
                    MarkSignaled();
                }

                internal void MarkSignaled()
                {
                    _firstObserver.Observer = _skipUntilObserver;
                    _subscription.Dispose();
                }

                public bool HasSignaled => _firstObserver.Observer != NopObserver<TSource>.Instance;
            }
        }
    }
}
