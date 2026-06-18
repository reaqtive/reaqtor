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
    internal sealed class HostedOperatorContext(Uri instanceId, IScheduler scheduler, TraceSource traceSource, IExecutionEnvironment executionEnvironment, IReactive service) : OperatorContext(instanceId, scheduler, traceSource, executionEnvironment), IHostedOperatorContext
    {
        public IReactive ReactiveService { get; } = service ?? throw new ArgumentNullException(nameof(service));
    }
}
