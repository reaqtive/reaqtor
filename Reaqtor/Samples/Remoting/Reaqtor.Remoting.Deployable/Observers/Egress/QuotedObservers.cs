// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive;
using System.Threading.Tasks;

using Nuqleon.DataModel;

using Reaqtive;

#pragma warning disable CA1303 // Do not pass literals as localized parameters. (No localization for tracing.)

namespace Reaqtor.Remoting.Deployable
{
    [KnownType]
    public sealed class EOP<TArgs, TInput> : IObserver<TInput>, IExpressible, ISubscription, IOperator
    {
        private readonly string Uri;
        private readonly TArgs Args;

#pragma warning disable CA2213 // REVIEW: Disposable fields should be disposed.
        private readonly BlockingCollection<Notification<TInput>> _notifications = new(/* defaults to queue underneath */);
#pragma warning restore CA2213

        // NB: Don't change constructor signature; it is matched in expression trees.

#pragma warning disable CA1054 // URI-like parameters should not be strings. (Gets bound as a string by the environment.)

        public EOP(string uri, TArgs args)
        {
            Uri = uri;
            Args = args;
        }

#pragma warning restore CA1054

        public Expression Expression { get; set; }

        public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

        public void Accept(ISubscriptionVisitor visitor) => visitor?.Visit(this);

        public void Dispose() => _notifications.CompleteAdding();

        public void OnCompleted() => _notifications.Add(Notification.CreateOnCompleted<TInput>());

        public void OnError(Exception error) => _notifications.Add(Notification.CreateOnError<TInput>(error));

        public void OnNext(TInput value) => _notifications.Add(Notification.CreateOnNext<TInput>(value));

        public void Subscribe() { }

        public void SetContext(IOperatorContext context) => Console.WriteLine(nameof(SetContext));

        public void Start()
        {
            Console.WriteLine($"Start - Expression = {Expression}");

            //
            // This part runs locally to re-normalize the expression to send it to some remote system.
            //

            var expr = new Requoter().Visit(Expression);

            Console.WriteLine($"Start - Normalized = {expr}");

            //
            // This mimics a remote system receiving the normalized expression and doing the binding and compilation.
            //

            var bound = Binder.Bind(expr);

            Console.WriteLine($"Start - Bound = {bound}");

            var observer = bound.Evaluate<IObserver<TInput>>();

            //
            // This mimics a remote system processing the event that are sent over the system boundary through some
            // channel which is simulated here using an in-memory concurrent queue.
            //

            Task.Run(() =>
            {
                Console.WriteLine($"Egress observer {Uri}({Args})");

                try
                {
                    foreach (var n in _notifications.GetConsumingEnumerable())
                    {
                        n.Accept(observer);
                    }
                }
                catch (OperationCanceledException)
                {
                }
            });
        }
    }
}
