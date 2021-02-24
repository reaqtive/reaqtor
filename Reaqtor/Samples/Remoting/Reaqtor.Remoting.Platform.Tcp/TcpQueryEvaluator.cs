﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class TcpQueryEvaluator : ReactiveQueryEvaluatorBase, IReactiveQueryEvaluator
    {
        public TcpQueryEvaluator(IReactivePlatform platform, string executablePath, int port, string uri)
            : this(platform, new TcpRunnable<IReactiveQueryEvaluatorConnection>(executablePath, port, uri))
        {
        }

        public TcpQueryEvaluator(IReactivePlatform platform, IRunnable runnable)
            : base(platform, runnable, ReactiveServiceType.QueryEvaluator)
        {
        }

        public override Task StartAsync(CancellationToken token)
        {
            Runnable.RunAsync(token);
            var instance = GetInstance<IRemotingReactiveServiceConnection>();
            instance.Configure(Platform.Configuration);
            instance.Start();
            return Task.FromResult(true);
        }
    }
}
