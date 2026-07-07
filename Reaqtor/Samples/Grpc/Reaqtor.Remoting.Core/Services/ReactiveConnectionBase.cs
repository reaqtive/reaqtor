// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this file.
//

namespace Reaqtor.Remoting.Protocol;

//
// NB: In the archived (.NET Remoting) stack this derived from MarshalByRefObject and overrode
//     InitializeLifetimeService to opt out of lease-based lifetime. On net10.0 there is no
//     .NET Remoting / MarshalByRefObject, so this is a plain base class with the same members
//     minus the remoting/lease-lifetime override. Transport lifetime is handled by gRPC hosts.
//
public abstract class ReactiveConnectionBase : IReactiveConnection
{
    public void Ping() { }
}
