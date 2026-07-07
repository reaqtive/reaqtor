// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;

using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Hubs;

namespace Reaqtor.Remoting.Rpc.MagicOnionDemo;

/// <summary>Server-side unary service implementation.</summary>
public sealed class GreeterService : ServiceBase<IGreeterService>, IGreeterService
{
    public UnaryResult<int> SumAsync(int a, int b) => UnaryResult.FromResult(a + b);
}

/// <summary>
/// Server-side StreamingHub. <c>this.Client</c> is the receiver proxy for the connected client, so
/// <see cref="StartFeedAsync"/> pushes results back into the caller — a server→client call over the same hub
/// connection (the MagicOnion-native form of the StreamJsonRpc Reply&lt;T&gt;/ClientAction demos).
/// </summary>
public sealed class ComputeHub : StreamingHubBase<IComputeHub, IComputeHubReceiver>, IComputeHub
{
    public Task<int> SumAsync(int[] values) => Task.FromResult(values.Sum());

    public Task StartFeedAsync(int count, int factor)
    {
        for (var i = 1; i <= count; i++)
        {
            Client.OnValue(i * factor); // server -> THIS client
        }

        Client.OnCompleted();
        return Task.CompletedTask;
    }
}
