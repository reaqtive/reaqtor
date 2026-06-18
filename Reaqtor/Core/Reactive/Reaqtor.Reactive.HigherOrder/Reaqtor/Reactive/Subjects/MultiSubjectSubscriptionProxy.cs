// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Reactive
{
    internal sealed class MultiSubjectSubscriptionProxy<TInput, TOutput>(Uri uri, IObserver<TOutput> observer) : UnaryOperator<Uri, TOutput>(uri, observer)
    {
#pragma warning disable CA2213 // "never disposed." This ends up in Input, which is disposed by the base class
        private readonly SingleAssignmentSubscription _subscription = new();

#pragma warning restore CA2213

        protected override ISubscription OnSubscribe()
        {
            return _subscription;
        }

        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            var sub = context.ExecutionEnvironment.GetSubject<TInput, TOutput>(Params).Subscribe(Output);

            SubscriptionInitializeVisitor.Subscribe(sub);

            _subscription.Subscription = sub;
        }
    }
}
