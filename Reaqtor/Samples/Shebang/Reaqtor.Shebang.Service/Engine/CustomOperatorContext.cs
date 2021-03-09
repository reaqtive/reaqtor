// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Reactive;

namespace Reaqtor.Shebang.Service
{
    //
    // Instances of OperatorContext are given to artifacts hosted within the engine through the SetContext
    // method they can override. This mechanism is used to inject dependencies, and we use it here to provide
    // access to the IngressEgressManager which is used by IngressObservable<T> and EgressObserver<T>.
    //

    public sealed class CustomOperatorContext : IHostedOperatorContext
    {
        private readonly IHostedOperatorContext _parent;
        private readonly IReadOnlyDictionary<string, object> _context;

        public CustomOperatorContext(IHostedOperatorContext parent, IReadOnlyDictionary<string, object> context) => (_parent, _context) = (parent, context);

        public Uri InstanceId => _parent.InstanceId;

        public IReactive ReactiveService => _parent.ReactiveService;

        public IScheduler Scheduler => _parent.Scheduler;

        public TraceSource TraceSource => _parent.TraceSource;

        public IExecutionEnvironment ExecutionEnvironment => _parent.ExecutionEnvironment;

        public bool TryGetElement<T>(string id, out T value)
        {
            if (_context.TryGetValue(id, out var obj) && obj is T res)
            {
                value = res;
                return true;
            }

            return _parent.TryGetElement<T>(id, out value);
        }
    }
}
