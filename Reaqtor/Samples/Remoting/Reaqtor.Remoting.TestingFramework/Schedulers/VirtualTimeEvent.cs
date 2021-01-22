// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.TestingFramework
{
    public struct VirtualTimeEvent<TContext>
    {
        public long Time;
        public bool IsAsync;
        public Action<TContext> Event;
        public Func<TContext, Task> AsyncEvent;
    }
}
