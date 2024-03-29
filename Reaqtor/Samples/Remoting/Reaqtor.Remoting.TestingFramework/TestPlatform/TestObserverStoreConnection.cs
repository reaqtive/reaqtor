﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TestObserverStoreConnection : KeyValueStoreConnection<string, ITestObserver<string>>
    {
        public const string ContextHandle = "TestObserverStoreConnection";
    }
}
