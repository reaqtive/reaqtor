// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using Reaqtive.Tasks;

namespace Reaqtive.Operators
{
    internal sealed class Empty<TResult> : SubscribableBase<TResult>
    {
        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Empty<TResult>, TResult>
        {
            private IOperatorContext _context;

            public _(Empty<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Empty";

            public override Version Version => Versioning.v1;

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _context = context;
            }

            protected override void OnStart()
            {
                _context.Scheduler.Schedule(new ActionTask(() =>
                {
                    _context.TraceSource.Empty_Processing(_context.InstanceId);

                    Output.OnCompleted();
                    Dispose();
                }));

                _context.TraceSource.Empty_Scheduling(_context.InstanceId);
            }
        }
    }
}
