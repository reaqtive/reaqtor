// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive.Tasks;

namespace Reaqtive.Operators
{
    internal sealed class Throw<TResult>(Exception error) : SubscribableBase<TResult>
    {
        private readonly Exception _error = error;

        protected override ISubscription SubscribeCore(IObserver<TResult> observer)
        {
            return new _(this, observer);
        }

        private sealed class _(Throw<TResult> parent, IObserver<TResult> observer) : StatefulUnaryOperator<Throw<TResult>, TResult>(parent, observer)
        {
            private IOperatorContext _context;

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
