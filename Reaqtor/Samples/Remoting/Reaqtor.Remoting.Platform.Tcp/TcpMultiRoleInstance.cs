// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class TcpMultiRoleInstance : IRunnable<Process>
    {
        private readonly object _instance;
        private readonly TcpMultiRoleRunnable _parent;

        public TcpMultiRoleInstance(object instance, TcpMultiRoleRunnable parent)
        {
            _instance = instance;
            _parent = parent;
        }

        public bool IsRunning => _parent.IsRunning;

        public object Instance => IsRunning ? _instance : null;

        public Process Target => _parent.Target;

        public Task<int> RunAsync(CancellationToken token) => _parent.RunAsync(token);

        public void Dispose() => _parent.Dispose();
    }
}
