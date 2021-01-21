// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using Reaqtive;

namespace Reaqtor.Remoting.Deployable.Streams
{
    internal class WrappedSubscription : ISubscription
    {
        private readonly ISubscription _inner;
        private Action _dispose;

        private WrappedSubscription(ISubscription inner, Action dispose)
        {
            _inner = inner;
            _dispose = dispose;
        }

        public static ISubscription Create(ISubscription inner)
        {
            return inner;
        }

        public static ISubscription Create(ISubscription inner, Action dispose)
        {
            return new WrappedSubscription(inner, dispose);
        }

        public void Accept(ISubscriptionVisitor visitor)
        {
            _inner.Accept(visitor);
        }

        public void Dispose()
        {
            Action action = Interlocked.Exchange(ref _dispose, null);

            if (action != null)
            {
                _inner.Dispose();
                action();
            }
        }
    }
}
