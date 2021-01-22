// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Reactive;

namespace Reaqtor.QueryEngine
{
    internal sealed class HostedOperatorContext : OperatorContext, IHostedOperatorContext
    {
        public HostedOperatorContext(Uri instanceId, IScheduler scheduler, TraceSource traceSource, IExecutionEnvironment executionEnvironment, IReactive service)
            : base(instanceId, scheduler, traceSource, executionEnvironment)
        {
            ReactiveService = service ?? throw new ArgumentNullException(nameof(service));
        }

        public IReactive ReactiveService { get; }
    }
}
