// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - March 2015 - Created the archived LocalReactiveServiceConnection / ReactiveServiceCommandProxy.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol;

//
// NB: This is the net10.0, in-process replacement for the deleted .NET Remoting binding pair
//     (archived Reaqtor.Remoting.Protocol):
//
//       - LocalReactiveServiceConnection : IReactiveServiceConnection
//           wrapped an IRemotingReactiveServiceConnection and, on CreateCommand, wrapped the
//           returned IReactiveServiceCommandRemoting in a ReactiveServiceCommandProxy.
//       - ReactiveServiceCommandProxy : RemoteProxyBase, IReactiveServiceCommand
//           adapted IReactiveServiceCommandRemoting.Execute(IObserver<string>) to
//           IReactiveServiceCommand.ExecuteAsync(token) via RemoteProxyBase.Invoke<T> +
//           Reply<T> (a MarshalByRefObject observer-to-TaskCompletionSource bridge in the
//           now-deleted Nuqleon.Runtime.Remoting.Tasks layer).
//
//     RemoteProxyBase / Reply<T> contained no actual marshaling logic — just a
//     TaskCompletionSource bridge — so on net10.0 we inline that bridge here and drop the
//     Nuqleon.Runtime.Remoting.Tasks dependency entirely (see plan §2.5). For the in-proc QC->QE
//     leg and the in-proc client->QC leg of the InMemory oracle, this connects directly to the
//     engine's IRemotingReactiveServiceConnection with no transport in between. The gRPC
//     transport provides its own IReactiveServiceConnection (Reaqtor.Remoting.Grpc.*).
//

/// <summary>
/// In-process <see cref="IReactiveServiceConnection"/> that calls a target
/// <see cref="IRemotingReactiveServiceConnection"/> (an engine service connection) directly,
/// with no transport in between. Replaces the archived
/// <c>LocalReactiveServiceConnection</c>/<c>ReactiveServiceCommandProxy</c> .NET Remoting pair.
/// </summary>
public sealed class InProcessReactiveServiceConnection : IReactiveServiceConnection
{
    private readonly IRemotingReactiveServiceConnection _connection;

    /// <summary>
    /// Creates an in-process connection over the given engine service connection.
    /// </summary>
    /// <param name="connection">The target engine service connection (e.g. a query evaluator or coordinator).</param>
    public InProcessReactiveServiceConnection(IRemotingReactiveServiceConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <summary>
    /// Creates a command bound to this connection, adapting the target's remoting command
    /// (observer-push) to the asynchronous <see cref="IReactiveServiceCommand"/> shape.
    /// </summary>
    public IReactiveServiceCommand CreateCommand(CommandVerb verb, CommandNoun noun, string commandText)
    {
        return new InProcessReactiveServiceCommand(_connection.CreateCommand(verb, noun, commandText));
    }
}

/// <summary>
/// In-process adapter from <see cref="IReactiveServiceCommandRemoting"/> (observer-push) to the
/// asynchronous <see cref="IReactiveServiceCommand"/>. Inlines the observer-to-task bridge that
/// the archived <c>RemoteProxyBase.Invoke&lt;T&gt;</c> + <c>Reply&lt;T&gt;</c> provided.
/// </summary>
internal sealed class InProcessReactiveServiceCommand : IReactiveServiceCommand
{
    private readonly IReactiveServiceCommandRemoting _service;

    public InProcessReactiveServiceCommand(IReactiveServiceCommandRemoting service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public IReactiveServiceConnection Connection => _service.Connection;

    public CommandVerb Verb => _service.Verb;

    public CommandNoun Noun => _service.Noun;

    public string CommandText => _service.CommandText;

    public async Task<string> ExecuteAsync(CancellationToken token)
    {
        var tcs = new TaskCompletionSource<string>();

        var cancel = _service.Execute(new ReplyObserver(tcs));

        using var ctr = token.Register(() =>
        {
            tcs.TrySetCanceled();
            cancel.Dispose();
        });

        return await tcs.Task.ConfigureAwait(false);
    }

    //
    // NB: Mirrors the archived Reply<T> semantics exactly: the command channel signals a single
    //     result via OnNext (or a fault via OnError) and never calls OnCompleted.
    //
    private sealed class ReplyObserver : IObserver<string>
    {
        private readonly TaskCompletionSource<string> _tcs;

        public ReplyObserver(TaskCompletionSource<string> tcs) => _tcs = tcs;

        public void OnNext(string value) => _tcs.TrySetResult(value);

        public void OnError(Exception error)
        {
            if (error is OperationCanceledException)
            {
                _tcs.TrySetCanceled();
            }
            else
            {
                _tcs.TrySetException(error);
            }
        }

        public void OnCompleted() => throw new InvalidOperationException("This observer channel should not be used.");
    }
}
