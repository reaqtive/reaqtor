// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reaqtive.Scheduler;

namespace Reaqtive.TestingFramework.Mocks
{
    public class MockOperatorContext : IOperatorContext
    {
        public Uri InstanceId
        {
            get;
            set;
        }

        public IScheduler Scheduler
        {
            get;
            set;
        }

        public TraceSource TraceSource
        {
            get;
            set;
        }

        public IExecutionEnvironment ExecutionEnvironment
        {
            get;
            set;
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2227 // Collection properties should be read only. (Used for object initializer syntax. Can't yet use `init` in cross-platform manner. Acceptable for mock.)

        public IDictionary<string, object> Settings
        {
            get;
            set;
        }

#pragma warning restore CA2227
#pragma warning restore IDE0079

        public bool TryGetElement<T>(string id, out T value)
        {
            if (Settings != null)
            {
                if (Settings.TryGetValue(id, out var res))
                {
                    value = (T)res;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
