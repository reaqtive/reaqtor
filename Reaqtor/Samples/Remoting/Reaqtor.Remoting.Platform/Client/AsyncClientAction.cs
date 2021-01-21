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

namespace Reaqtor.Remoting.Client
{
    public class AsyncClientAction : MarshalByRefObject, IRemoteAction
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

        public override object InitializeLifetimeService() => null;
    }
}
