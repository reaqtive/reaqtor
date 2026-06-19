// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Tasks;
using Reaqtive.TestingFramework;

namespace Test.Reaqtive.Operators
{
    public static class DeclarativeExtensions
    {
#pragma warning disable IDE0060 // Remove unused parameter (used to make declarative tests compile in glitching as well)
        public static ISubscribable<T> Empty<T>(this TestScheduler scheduler)
        {
            return Subscribable.Empty<T>();
        }

        public static ISubscribable<T> Never<T>(this TestScheduler scheduler)
        {
            return Subscribable.Never<T>();
        }

        public static ISubscribable<T> Return<T>(this TestScheduler scheduler, T value)
        {
            return Subscribable.Return<T>(value);
        }

        public static ISubscribable<long> Timer(this TestScheduler scheduler, TimeSpan dueTime)
        {
            return Subscribable.Timer(dueTime);
        }

        public static ISubscribable<long> Timer(this TestScheduler scheduler, DateTimeOffset dueTime)
        {
            return Subscribable.Timer(dueTime);
        }

        public static ISubscribable<long> Timer(this TestScheduler scheduler, TimeSpan dueTime, TimeSpan period)
        {
            return Subscribable.Timer(dueTime, period);
        }

        public static ISubscribable<long> Timer(this TestScheduler scheduler, DateTimeOffset dueTime, TimeSpan period)
        {
            return Subscribable.Timer(dueTime, period);
        }

        public static ISubscribable<T> Throw<T>(this TestScheduler scheduler, Exception error)
        {
            return Subscribable.Throw<T>(error);
        }

        public static ISubscribable<T> Exceptional<T>(this TestScheduler scheduler, Action error, bool onSubscribe)
        {
            return new ExceptionSubscribable<T>(error, onSubscribe);
        }
#pragma warning restore IDE0060 // Remove unused parameter

        private sealed class ExceptionSubscribable<T> : ISubscribable<T>
        {
            private readonly Action _exception;
            private readonly bool _onSubscribe;

            public ExceptionSubscribable(Action ex, bool onSubscribe)
            {
                _exception = ex;
                _onSubscribe = onSubscribe;
            }

            public ISubscription Subscribe(IObserver<T> observer)
            {
                if (_onSubscribe)
                {
                    _exception();
                }

                return new _(this, observer);
            }

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
            {
                return Subscribe(observer);
            }

            private sealed class _ : Operator<ExceptionSubscribable<T>, T>
            {
                public _(ExceptionSubscribable<T> parent, IObserver<T> obv)
                    : base(parent, obv)
                {
                }

                public override void SetContext(IOperatorContext context)
                {
                    if (!Params._onSubscribe)
                    {
                        Params._exception();
                    }

                    context.Scheduler.Schedule(new ActionTask(() => Output.OnError(new NotSupportedException())));
                }
            }
        }
    }
}
