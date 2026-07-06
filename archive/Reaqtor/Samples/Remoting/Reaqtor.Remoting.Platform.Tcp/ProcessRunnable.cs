// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ProcessRunnable : IRunnable<Process>
    {
        protected ProcessRunnable(string executablePath, params string[] arguments)
        {
            Target = new Process();
            Target.StartInfo.FileName = executablePath;
            Target.StartInfo.Arguments = string.Join(" ", arguments);
        }

        public Process Target
        {
            get;
            private set;
        }

        public bool IsRunning { get; private set; }

        public abstract object Instance
        {
            get;
        }

        public virtual Task<int> RunAsync(CancellationToken token)
        {
            if (!IsRunning)
            {
                var tcs = new TaskCompletionSource<int>();
                Target.EnableRaisingEvents = true;
                Target.Exited += (o, e) =>
                {
                    tcs.SetResult(Target.ExitCode);
                    Dispose();
                };
                token.Register(() => Dispose());
                Target.Start();
                IsRunning = true;
                return tcs.Task;
            }
            else
            {
                throw new InvalidOperationException("Process is already running.");
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsRunning && !Target.HasExited)
                {
                    Target.Kill();
                    Target.WaitForExit();
                }
                IsRunning = false;
            }
        }
    }
}
