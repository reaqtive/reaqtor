// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Client;

// NB (plan §3.4): de-MBR'd. The archived AsyncClientAction derived from MarshalByRefObject (and overrode
//     InitializeLifetimeService to disable lease expiry) so the QE could ship a client-authored async lambda
//     across the .NET Remoting boundary to run server-side. The scheduler now runs fully in-process (no
//     transport), so the action is invoked directly; the MarshalByRefObject base and the lease override are
//     removed, keeping only IRemoteAction.
public class AsyncClientAction : IRemoteAction
{
    private readonly Func<Task> _action;

    public AsyncClientAction(Func<Task> action)
    {
        _action = action;
    }

    public void Invoke()
    {
        _action().Wait();
    }
}
