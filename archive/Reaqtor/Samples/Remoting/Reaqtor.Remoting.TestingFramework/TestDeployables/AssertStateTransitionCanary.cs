// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - February 2014 - Created this file.
//

using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

namespace Reaqtor.Remoting.TestingFramework
{
    internal class AssertStateTransitionCanary<T> : SubscribableBase<T>
    {
        private readonly ISubscribable<T> _source;

        public AssertStateTransitionCanary(ISubscribable<T> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : UnaryOperator<AssertStateTransitionCanary<T>, T>, IStatefulOperator
        {
            private int subscribed = 0;
            private int contextSet = 0;
            private int started = 0;
            private int stateSavedOnce = 0;

            public _(AssertStateTransitionCanary<T> parent, IObserver<T> observer)
                : base(parent, observer)
            {
            }

            #region IVersioned

            public string Name => "rx:AssertStateTransitionCanary";

            public Version Version => Versioning.v1;

            #endregion

            #region IOperator

            protected override ISubscription OnSubscribe()
            {
                Assert.IsFalse(IsDisposed);
                Assert.AreEqual(0, Interlocked.Exchange(ref subscribed, 1));
                Assert.AreEqual(0, contextSet);
                Assert.AreEqual(0, started);
                Assert.AreEqual(0, stateSavedOnce);

                return Params._source.Subscribe(Output);
            }

            public override void SetContext(IOperatorContext context)
            {
                Assert.IsFalse(IsDisposed);
                Assert.AreEqual(1, subscribed);
                Assert.AreEqual(0, Interlocked.Exchange(ref contextSet, 1));
                Assert.AreEqual(0, started);
                Assert.AreEqual(0, stateSavedOnce);

                base.SetContext(context);
            }

            protected override void OnStart()
            {
                Assert.IsFalse(IsDisposed);
                Assert.AreEqual(1, subscribed);
                Assert.AreEqual(1, contextSet);
                Assert.AreEqual(0, Interlocked.Exchange(ref started, 1));
                Assert.AreEqual(0, stateSavedOnce);

                base.OnStart();
            }

            protected override void OnDispose()
            {
                Assert.AreEqual(1, subscribed);
                Assert.AreEqual(1, contextSet);

                base.OnDispose();
            }

            #endregion

            #region IStatefulOperator

            public void LoadState(IOperatorStateReader reader, Version version)
            {
                // TODO: Investigate if we should load state on a disposed operator
                //Assert.IsFalse(IsDisposed);
                Assert.AreEqual(1, subscribed);
                // TODO: Investigate if context should be set before LoadState
                //Assert.AreEqual(0, contextSet);
                Assert.AreEqual(0, started);
                Assert.AreEqual(0, stateSavedOnce);
            }

            public void SaveState(IOperatorStateWriter writer, Version version)
            {
                Assert.AreEqual(1, subscribed);
                Assert.AreEqual(1, contextSet);
                //Assert.AreEqual(1, started);

                stateSavedOnce = 1;
            }

            public void OnStateSaved()
            {
                Assert.AreEqual(1, subscribed);
                Assert.AreEqual(1, contextSet);
                // TODO: Should we call these if the state wasn't saved?
                //Assert.AreEqual(1, started);
                //Assert.AreEqual(1, stateSavedOnce);
            }

            public bool StateChanged => false;

            #endregion
        }
    }
}
