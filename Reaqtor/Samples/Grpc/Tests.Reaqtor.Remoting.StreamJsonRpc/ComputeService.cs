// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Rpc.Demo;

/// <summary>
/// The server-side implementation of <see cref="IComputeService"/>. It is deliberately tiny — the point of the
/// prototype is the RPC <i>shapes</i> StreamJsonRpc enables, each mapped to the archived .NET Remoting primitive
/// it replaces, not the compute itself.
/// </summary>
public sealed class ComputeService : IComputeService
{
    /// <summary>The server's proxy back to the client (set by the harness after attach). Used by <see cref="AggregateViaClientAsync"/>.</summary>
    public IClientCallback Client { get; set; }

    // Reply<T>: the server pushes each result into the client's marshaled observer, then completes it.
    public async Task RunIntoObserverAsync(int[] inputs, IResultObserver observer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(observer);

        try
        {
            foreach (var x in inputs)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await observer.OnNextAsync(x * 10).ConfigureAwait(false);
            }

            await observer.OnCompletedAsync().ConfigureAwait(false);
        }
#pragma warning disable CA1031 // The observer's OnError leg is part of the contract being demonstrated.
        catch (Exception ex)
        {
            await observer.OnErrorAsync(ex.Message).ConfigureAwait(false);
        }
#pragma warning restore CA1031
    }

    // IProgress<T>: report items-processed to the client as the work proceeds.
    public async Task<int> SumWithProgressAsync(int[] inputs, IProgress<int> progress, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(inputs);

        var sum = 0;
        for (var i = 0; i < inputs.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            sum += inputs[i];
            progress?.Report(i + 1);
            await Task.Yield();
        }

        return sum;
    }

    // IAsyncEnumerable<T>: lazily stream 1..count back to the client.
    public async IAsyncEnumerable<int> CountAsync(int count, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (var i = 1; i <= count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return i;
            await Task.Yield();
        }
    }

    // ClientAction / §3.4: the server calls back into the client to resolve each key, then aggregates.
    public async Task<int> AggregateViaClientAsync(int[] keys, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(keys);

        var total = 0;
        foreach (var key in keys)
        {
            cancellationToken.ThrowIfCancellationRequested();
            total += await Client.ResolveAsync(key).ConfigureAwait(false);
        }

        return total;
    }

    // Cancellation: a long op that aborts when the client cancels its token.
    public async Task<int> SlowEchoAsync(int value, TimeSpan delay, CancellationToken cancellationToken)
    {
        await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
        return value;
    }
}
