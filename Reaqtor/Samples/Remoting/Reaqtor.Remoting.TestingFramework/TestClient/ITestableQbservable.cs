// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System.Collections.Generic;

using Reaqtive.Testing;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public interface ITestableQbservable<T> : IAsyncReactiveQbservable<T>
    {
        IList<Subscription> Subscriptions { get; }
        IList<Recorded<INotification<T>>> ObserverMessages { get; }
    }
}
