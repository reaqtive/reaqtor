// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Uitlity to track the execution of operations and ensure completion of all pending operations
    /// when a request to dispose is made. Upon receiving a request to dispose, a barrier is erected
    /// that prevents future operations from starting. When all pending operations have completed,
    /// the component is disposed and the call to <see cref="DisposeAsync"/> returns.
    /// </summary>
    internal sealed class OperationTracker : IAsyncDisposable
    {
        /// <summary>
        /// Event used to wait for the completion of pending operations.
        /// </summary>
        private readonly AsyncManualResetEvent _evt = new();

        /// <summary>
        /// The number of running operations; see state transition diagram below.
        /// </summary>
        private int _count;

        //
        // The _count field keeps track of the number of running operations but while the component is
        // running and when it is being disposed. Its values encode the current state as follows:
        //
        // int.MinValue - The component has been disposed; no further requests will be allowed. All
        //                calls to Enter will throw ObjectDisposedException.
        //
        // < 0 - A request to dispose the component has been made and remaining work is being drained.
        //       The absolute value of _count denotes the number of still running operations. As these
        //       operations complete, the _count value will increase towards 0, but will transition to
        //       int.MinValue rather than 0 when all pending work has completed.
        //
        //   0 - The component is idle.
        //
        // > 0 - The component is active. New operations are allowed to start by calling Enter. The
        //       _count field keeps track of the number of operations that are running but have not yet
        //       completed by calling Exit.
        //

        /// <summary>
        /// Starts to track a new operation.
        /// </summary>
        /// <returns>Disposable tracker object; should be disposed when the operation ends.</returns>
        public Tracker Enter()
        {
            while (true)
            {
                var count = Volatile.Read(ref _count);

                if (count < 0)
                {
                    //
                    // Either int.MinValue (Disposed) or another value < 0 (Disposing). Don't allow
                    // new work to come in.
                    //
                    throw new ObjectDisposedException("this");
                }

                if (Interlocked.CompareExchange(ref _count, count + 1, count) == count)
                {
                    //
                    // If we manage to increment the number of running operations, return a tracker to
                    // have the caller decrement it when the operation is complete.
                    //
                    return new Tracker(this);
                }
            }
        }

        /// <summary>
        /// Disposes the component by draining the remaining work. This call blocks until all work has
        /// completed.
        /// </summary>
#if NET6_0 || NETSTANDARD2_1
        public async ValueTask DisposeAsync()
#else
        public async Task DisposeAsync(CancellationToken token)
#endif
        {
            while (true)
            {
                int count = Volatile.Read(ref _count);

                if (count == int.MinValue)
                {
                    //
                    // Already disposed. No need to wait for the event either.
                    //
                    return;
                }
                else if (count < 0)
                {
                    //
                    // We're currently disposing; wait for the event that signals all work is done, as set
                    // by the Exit method.
                    //
                    await _evt.WaitAsync().ConfigureAwait(false);
                    break;
                }
                else if (count == 0)
                {
                    //
                    // Attempt to transition from Idle to Disposed.
                    //
                    if (Interlocked.CompareExchange(ref _count, int.MinValue, count) == count)
                    {
                        break;
                    }
                }
                else
                {
                    //
                    // We're the first one to get the request to dispose and attempt to transition to the
                    // next Disposing phase by flipping the sign on _count. If we succeed to do so, we can
                    // start to wait for the completion of all operations as signaled through the event by
                    // the Exit method.
                    //
                    if (Interlocked.CompareExchange(ref _count, -count, count) == count)
                    {
                        await _evt.WaitAsync().ConfigureAwait(false);
                        break;
                    }
                }
            }
        }

        private void Exit()
        {
            while (true)
            {
                var count = Volatile.Read(ref _count);

                if (count < -1)
                {
                    //
                    // If we have a negative count and incrementing it won't reach 0 (which requires us to
                    // transition to the Disposed state directly), then attempt to move towards 0 to account
                    // for the completion of the operation.
                    //
                    if (Interlocked.CompareExchange(ref _count, count + 1, count) == count)
                    {
                        break;
                    }
                }
                else if (count == -1)
                {
                    //
                    // If the count is currently -1, we don't want to transition to 0 which is used to represent
                    // the Idle state. Instead, we attempt to transition to Disposed dirctly.
                    //
                    if (Interlocked.CompareExchange(ref _count, int.MinValue, count) == count)
                    {
                        _evt.Set();
                        break;
                    }
                }
                else if (count == 0)
                {
                    //
                    // This should never happen; we're exiting for unaccounted work. Note that the Tracker guards
                    // against double-dispose.
                    //
                    throw new InvariantException();
                }
                else
                {
                    //
                    // If count is positive, just attempt to decrement by one.
                    //
                    if (Interlocked.CompareExchange(ref _count, count - 1, count) == count)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Tracker used to observe the completion of an operation.
        /// </summary>
        /// <remarks>
        /// We could make this into a struct but we'd lose the safeguarding against duplicate calls to Exit if a
        /// copy of the struct is made prior to calling Dispose. While unlikely, let's err on the side of caution.
        /// The additional small allocation cost is negligible for the code paths this is used on (i.e. high-level
        /// management operations and DDL operations which are allocation heavy).
        /// </remarks>
        public sealed class Tracker : IDisposable
        {
            private OperationTracker _parent;

            internal Tracker(OperationTracker parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Dispose method used to indicate the completion of an operation.
            /// </summary>
            public void Dispose()
            {
                //
                // NB: Protect against calling Exit multiple times in case of double-Dispose.
                //
                Interlocked.Exchange(ref _parent, null)?.Exit();
            }
        }

        private sealed class AsyncManualResetEvent
        {
            private readonly TaskCompletionSource<bool> _tcs = new();

            public Task WaitAsync() => _tcs.Task;

            public void Set() => _tcs.TrySetResult(true);
        }
    }
}
