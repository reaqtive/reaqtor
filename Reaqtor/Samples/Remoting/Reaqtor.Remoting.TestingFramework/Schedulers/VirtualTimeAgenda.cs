// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.TestingFramework
{
    public class VirtualTimeAgenda<TContext> : List<VirtualTimeEvent<TContext>>
        where TContext : ReactiveClientContext
    {
        public void Add(long ticks, Action<TContext> scheduledEvent)
        {
            Add(new VirtualTimeEvent<TContext> { Time = ticks, Event = scheduledEvent });
        }

        public void Add(long ticks, Func<TContext, Task> scheduledEvent)
        {
            Add(new VirtualTimeEvent<TContext> { Time = ticks, AsyncEvent = scheduledEvent, IsAsync = true });
        }
    }
}
