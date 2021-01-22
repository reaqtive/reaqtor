// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    public sealed class InMemoryRunnable : IRunnable<Func<CancellationToken, Task<object>>>
    {
        public InMemoryRunnable(Func<CancellationToken, Task<object>> task)
        {
            Target = task;
        }

        public Func<CancellationToken, Task<object>> Target { get; }

        public bool IsRunning { get; private set; }

        public object Instance { get; private set; }

        public Task<int> RunAsync(CancellationToken token)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                return Target(token).ContinueWith(t =>
                    {
                        Instance = t.Result;
                        IsRunning = false;
                        return 0;
                    });
            }
            else
            {
                throw new InvalidOperationException("Task is already running.");
            }
        }

        public void Dispose()
        {
            IsRunning = false;
            Instance = null;
        }
    }
}
