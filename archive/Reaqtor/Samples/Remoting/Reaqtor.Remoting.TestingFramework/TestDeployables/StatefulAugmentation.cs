// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - February 2014 - Created this file.
//

using System;

using Reaqtive;

namespace Reaqtor.Remoting.TestingFramework
{
    internal class StatefulAugmentation<TSource> : SubscribableBase<TSource>
    {
        private readonly ISubscribable<TSource> _source;

        public StatefulAugmentation(ISubscribable<TSource> source)
        {
            _source = source;
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<StatefulAugmentation<TSource>, TSource>
        {
            public _(StatefulAugmentation<TSource> parent, IObserver<TSource> observer)
                : base(parent, observer)
            {
            }

            public override Version Version => Versioning.v1;

            public override string Name => "rx:AugmentTest";

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(Output);
            }
        }
    }
}
