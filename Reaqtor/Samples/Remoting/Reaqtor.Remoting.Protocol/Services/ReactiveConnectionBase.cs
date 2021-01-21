// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    public abstract class ReactiveConnectionBase : MarshalByRefObject, IReactiveConnection
    {
        public void Ping() { }

        public override object InitializeLifetimeService() => null;
    }
}
