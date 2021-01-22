// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor.Hosting.Shared.Tools;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public interface ITestReactivePlatformClient : IReactivePlatformClient, IDisposable
    {
        ITestScheduler Scheduler { get; }
        ITestableQbservable<T> CreateHotObservable<T>(params Recorded<INotification<T>>[] messages);
        ITestableQbservable<T> CreateColdObservable<T>(params Recorded<INotification<T>>[] messages);
        ITestObserver<T> Start<T>(Func<IAsyncReactiveQbservable<T>> create, long created, long subscribed, long disposed);
        void CleanupEntity(Uri subscriptionUri, ReactiveEntityType entityType);
    }
}
