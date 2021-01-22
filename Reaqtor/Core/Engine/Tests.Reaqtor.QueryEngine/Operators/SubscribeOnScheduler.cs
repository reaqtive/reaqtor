// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive;

using Reaqtor;

namespace Tests.Reaqtor.QueryEngine
{
    /// <summary>
    /// Simple test operator that switches an incoming thread to a scheduler thread.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class SubscribableOnScheduler<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;

        public SubscribableOnScheduler(ISubscribable<TSource> source)
        {
            Debug.Assert(source != null);
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : ContextSwitchOperator<SubscribableOnScheduler<TSource>, TSource>
        {
            public _(SubscribableOnScheduler<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rct:SubscribableOnScheduler";

            public override Version Version => Versioning.v1;

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                return new[] { Params._source.Subscribe(this) };
            }
        }
    }
}
