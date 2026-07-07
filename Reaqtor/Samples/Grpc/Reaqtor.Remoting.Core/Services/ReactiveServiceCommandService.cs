// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 03/23/2015 - Created the archived ReactiveServiceCommandService.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    //
    // NB: net10.0, in-process replacement for the deleted .NET Remoting service adapter (archived
    //     Reaqtor.Remoting.Protocol/Protocol/Command/Generated/ReactiveServiceCommandService.cs):
    //
    //       ReactiveServiceCommandService : RemoteServiceBase, IReactiveServiceCommandRemoting
    //           adapted the asynchronous IReactiveServiceCommand.ExecuteAsync(token) back to the
    //           observer-push IReactiveServiceCommandRemoting.Execute(IObserver<string>) via
    //           RemoteServiceBase.Invoke<T> (in the now-deleted Nuqleon.Runtime.Remoting.Tasks layer).
    //
    //     RemoteServiceBase.Invoke ran the task under a CancellationTokenSource and, on completion,
    //     pushed the single result to the reply observer (OnError on fault/cancel, OnNext on success —
    //     it never calls OnCompleted, matching the client-side Reply<T> contract). It tracked the CTS in
    //     a GUID-keyed dictionary so a *remote* RemoteCancellationDisposable.Dispose() could find and
    //     cancel it across the marshaling boundary. In-process there is no boundary, so the returned
    //     disposable holds the CTS directly and the GUID registry is dropped (plan §2.5). This is the
    //     exact server-side mirror of InProcessReactiveServiceCommand (the client-side adapter).
    //

    /// <summary>
    /// In-process adapter from the asynchronous <see cref="IReactiveServiceCommand"/> to the
    /// observer-push <see cref="IReactiveServiceCommandRemoting"/>. Used by the engine service
    /// connections to expose their command channel. Replaces the archived
    /// <c>ReactiveServiceCommandService : RemoteServiceBase</c> .NET Remoting adapter.
    /// </summary>
    public sealed class ReactiveServiceCommandService : IReactiveServiceCommandRemoting
    {
        private readonly IReactiveServiceCommand _obj;

        /// <summary>
        /// Creates the service adapter over the given asynchronous command.
        /// </summary>
        /// <param name="obj">The underlying asynchronous command.</param>
        public ReactiveServiceCommandService(IReactiveServiceCommand obj)
        {
            _obj = obj ?? throw new ArgumentNullException(nameof(obj));
        }

        public IReactiveServiceConnection Connection => _obj.Connection;

        public CommandVerb Verb => _obj.Verb;

        public CommandNoun Noun => _obj.Noun;

        public string CommandText => _obj.CommandText;

        public IDisposable Execute(IObserver<string> result)
        {
            ArgumentNullException.ThrowIfNull(result);

            var cts = new CancellationTokenSource();

            _obj.ExecuteAsync(cts.Token).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    result.OnError(t.Exception);
                }
                else if (t.IsCanceled)
                {
                    result.OnError(new OperationCanceledException());
                }
                else if (t.IsCompleted)
                {
                    result.OnNext(t.Result);
                }
            }, TaskScheduler.Default);

            return new CancelDisposable(cts);
        }

        //
        // NB: Replaces the marshaled RemoteCancellationDisposable + ICancellationProvider GUID registry
        //     with a direct, idempotent cancel-and-dispose of the operation's CancellationTokenSource.
        //
        private sealed class CancelDisposable : IDisposable
        {
            private CancellationTokenSource _cts;

            public CancelDisposable(CancellationTokenSource cts) => _cts = cts;

            public void Dispose()
            {
                var cts = Interlocked.Exchange(ref _cts, null);
                if (cts != null)
                {
                    cts.Cancel();
                    cts.Dispose();
                }
            }
        }
    }
}
