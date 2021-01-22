// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

using Reaqtive;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// Annotation that keeps rewrites from touching this type. To be used sparingly and only for observables, observers, stream factories that have been deployed to the service (e.g. Feeds, HTTP post observer).
    /// </summary>
    [KnownType]
    public class RangeObservable : ISubscribable<int>
    {
        /// <summary>
        /// The count
        /// </summary>
        private readonly int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeObservable"/> class.
        /// </summary>
        /// <param name="cnt">The count.</param>
        public RangeObservable(int count)
        {
            _count = count;
        }

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>the range observable</returns>
        IDisposable IObservable<int>.Subscribe(IObserver<int> observer) => Subscribe(observer);

        /// <summary>
        /// Subscribes the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <returns>a subscription</returns>
        public ISubscription Subscribe(IObserver<int> observer) => new _(this, observer);

        /// <summary>
        /// TODO: Needs some explanation here. Ask IRP Core team
        /// Internal Operator
        /// </summary>
        private sealed class _ : ContextSwitchOperator<RangeObservable, int>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="_"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="observer">The observer.</param>
            public _(RangeObservable parent, IObserver<int> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rct:Range";

            public override Version Version => new(1, 0, 0, 0);

            /// <summary>
            /// This differs from Rx in that we added more phasing. Observables are now "artic cold", where Subscribe keeps them "cold"
            /// until OnStart triggers their execution (or state is recovered from a checkpoint after failure).
            /// </summary>
            protected override void OnStart()
            {
                for (var i = 0; i < Params._count; ++i)
                {
                    OnNext(i);
                }

                OnCompleted();

                base.OnStart();
            }
        }
    }
}
