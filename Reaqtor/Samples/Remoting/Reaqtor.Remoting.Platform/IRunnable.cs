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
    public interface IRunnable : IDisposable
    {
        bool IsRunning { get; }
        object Instance { get; }
        Task<int> RunAsync(CancellationToken token);
    }

    public interface IRunnable<T> : IRunnable, IDisposable
    {
        T Target { get; }
    }
}
