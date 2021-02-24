// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Tasks;
using System;

namespace Reaqtive.Operators
{
    internal sealed class Throw<TResult> : SubscribableBase<TResult>
    {
        private readonly Exception _error;

        public Throw(Exception error)
        {
            _error = error;
        }

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : StatefulUnaryOperator<Throw<TResult>, TResult>
        {
            private IOperatorContext _context;

            public _(Throw<TResult> parent, IObserver<TResult> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:Throw";

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
                    _context.TraceSource.Throw_Processing(_context.InstanceId);

                    Output.OnError(Params._error);
                    Dispose();
                }));

                _context.TraceSource.Throw_Scheduling(_context.InstanceId);
            }
        }
    }
}
