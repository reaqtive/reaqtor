// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Reliable.Client
{
    public abstract class ReliableReactiveSubscriptionBase : IReliableReactiveSubscription
    {
        public abstract Uri ResubscribeUri { get; }

        public void Start(long sequenceId) => StartCore(sequenceId);

        protected abstract void StartCore(long sequenceId);

        public void AcknowledgeRange(long sequenceId) => AcknowledgeRangeCore(sequenceId);

        protected abstract void AcknowledgeRangeCore(long sequenceId);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCore();
            }
        }

        protected abstract void DisposeCore();

        public abstract void Accept(ISubscriptionVisitor visitor);
    }
}
