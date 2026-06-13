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
    public struct VirtualTimeEvent<TContext> : IEquatable<VirtualTimeEvent<TContext>>
    {
        public long Time { get; set; }
        public bool IsAsync { get; set; }
        public Action<TContext> Event { get; set; }
        public Func<TContext, Task> AsyncEvent { get; set; }

        public override readonly bool Equals(object obj) => obj is VirtualTimeEvent<TContext> other && Equals(other);

        public readonly bool Equals(VirtualTimeEvent<TContext> other) => Time == other.Time && IsAsync == other.IsAsync && Event == other.Event && AsyncEvent == other.AsyncEvent;

        public override readonly int GetHashCode() => HashCombine(Time.GetHashCode(), IsAsync.GetHashCode(), Event?.GetHashCode() ?? 0, AsyncEvent?.GetHashCode() ?? 0);

        public static bool operator ==(VirtualTimeEvent<TContext> left, VirtualTimeEvent<TContext> right) => left.Equals(right);

        public static bool operator !=(VirtualTimeEvent<TContext> left, VirtualTimeEvent<TContext> right) => !(left == right);

        private static int HashCombine(int a, int b, int c, int d)
        {
            var h = a;
            h = h * 31 + b;
            h = h * 31 + c;
            h = h * 31 + d;
            return h;
        }
    }
}
