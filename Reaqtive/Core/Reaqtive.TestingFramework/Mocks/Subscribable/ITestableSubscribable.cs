// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    public interface ITestableSubscribable<T> : ISubscribable<T>
    {
        IList<Subscription> Subscriptions { get; }
        IList<Recorded<Notification<T>>> ObserverMessages { get; }
    }
}
