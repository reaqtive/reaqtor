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
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    public sealed class AppDomainRunnable : IRunnable<AppDomain>
    {
        private readonly Type _type;

        public AppDomainRunnable(string friendlyName, Type type)
        {
            _type = type;
            Target = AppDomain.CreateDomain(friendlyName, null, new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory });
        }

        public AppDomain Target
        {
            get;
            private set;
        }

        public bool IsRunning { get; private set; }

        public object Instance { get; private set; }

        public Task<int> RunAsync(CancellationToken token)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Runnable is already running.");
            }
            else if (Target == null)
            {
                throw new InvalidOperationException("AppDomain has already been unloaded.");
            }
            else
            {
                IsRunning = true;
                var tcs = new TaskCompletionSource<int>();
                token.Register(() => Dispose());
                Instance = Target.CreateInstanceAndUnwrap(_type.Assembly.FullName, _type.FullName);
                return Task.FromResult(default(int));
            }
        }

        public void Dispose()
        {
            if (Target != null)
            {
                AppDomain.Unload(Target);
                Target = null;
                IsRunning = false;
                Instance = null;
            }
        }

        public void Marshal(object obj)
        {
            Target.SetData("marshallee", obj);
            Target.DoCallBack(
                () => RemotingServices.Marshal((MarshalByRefObject)AppDomain.CurrentDomain.GetData("marshallee"))
            );
        }

        public void SetListener(TraceListener listener)
        {
            Target.SetData("listener", listener);
            Target.DoCallBack(
                () => Trace.Listeners.Add((TraceListener)AppDomain.CurrentDomain.GetData("listener"))
            );
        }
    }
}
