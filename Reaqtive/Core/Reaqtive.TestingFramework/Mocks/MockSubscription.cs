// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.TestingFramework.Mocks
{
    public sealed class MockSubscription : ISubscription
    {
        private readonly Action _dispose;
        private bool _disposed;

        public MockSubscription(Action dispose)
        {
            _dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
        }

        public MockSubscription(IDisposable disposable)
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            _dispose = disposable.Dispose;
        }

        public void Accept(ISubscriptionVisitor visitor)
        {
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _dispose();
            }
        }
    }
}
