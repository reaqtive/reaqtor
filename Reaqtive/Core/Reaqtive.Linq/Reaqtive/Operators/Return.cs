// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive.Tasks;

namespace Reaqtive.Operators
{
    internal sealed class Return<TResult> : SubscribableBase<TResult>
    {
        private readonly TResult _value;

        public Return(TResult value)
        {
            _value = value;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Return<TResult>, TResult>
        {
            private IOperatorContext _context;

            public _(Return<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Return";

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
                    _context.TraceSource.Return_Processing(_context.InstanceId);

                    Output.OnNext(Params._value);
                    Output.OnCompleted();
                    Dispose();
                }));

                _context.TraceSource.Return_Scheduling(_context.InstanceId);
            }
        }
    }
}
