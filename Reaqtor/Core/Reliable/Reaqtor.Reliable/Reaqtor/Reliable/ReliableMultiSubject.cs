// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Reaqtive;

namespace Reaqtor.Reliable
{
    public class ReliableMultiSubject<T> : ReliableMultiSubjectBase<T>, IStatefulOperator
    {
        private Uri _id;

        public override Uri Id => _id;

        public string Name => "rcr:MultiSubject";

        public Version Version => Versioning.v1;

        protected override void DisposeCore()
        {
        }

        public IEnumerable<ISubscription> Inputs => Enumerable.Empty<ISubscription>();

        public void Subscribe() { }

        public void SetContext(IOperatorContext context)
        {
            Debug.Assert(context != null);

            _id = context.InstanceId;
        }

        protected override void OnNext(T item, long sequenceId)
        {
            base.OnNext(item, sequenceId);
            NotifySubscriptions();
        }

        protected override void OnError(Exception error)
        {
            base.OnError(error);
            NotifySubscriptions();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            NotifySubscriptions();
        }

        protected override bool ShouldBufferedEventsBeDropped() => SubscriptionCount == 0;
    }
}
