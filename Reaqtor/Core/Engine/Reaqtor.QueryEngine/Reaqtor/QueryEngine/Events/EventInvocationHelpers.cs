// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Provides helpers to invoke query engine events.
    /// </summary>
    internal static class EventInvocationHelpers
    {
        /// <summary>
        /// Invokes the specified event but ignores (but logs) any exceptions thrown by the exception handler.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="handler">The event to invoke.</param>
        /// <param name="sender">The sender raising the event.</param>
        /// <param name="args">The arguments passed to the event handler(s).</param>
        /// <param name="method">The name of the caller, used for logging purposes in case of failure.</param>
        public static void InvokeSafe<TEventArgs>(this EventHandler<TEventArgs> handler, CheckpointingQueryEngine sender, TEventArgs args, [CallerMemberName] string method = null)
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Goal of Safe variant.)

            try
            {
                handler(sender, args);
            }
            catch (Exception ex)
            {
                sender.TraceSource.FailSafe_Exception(method, ex);
            }

#pragma warning restore CA1031
#pragma warning restore IDE0079
        }
    }
}
