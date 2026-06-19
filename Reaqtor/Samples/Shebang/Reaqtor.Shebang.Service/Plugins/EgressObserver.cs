// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

using Reaqtor.Reliable;
using Reaqtor.Shebang.Service;

namespace Reaqtor.Shebang.Extensions
{
    //
    // Stateful observer connecting to the outside world through the ingress/egress manager to send
    // events. The observer is stateful because it remembers sequence IDs it has assigned to events
    // it has generated. Upon recovery, it continues from the last sequence ID.
    //
    // The lifecycle of artifacts is more or less like this:
    //
    //   new() -> SetContext [-> Load] -> Start -> Save* -> IUnloadable.Unload
    //
    // 1. An instance is constructed (either due to recovery or because of a new query).
    // 2. SetContext provides context, e.g. to fish out a service provided by the environment.
    // 3. If the artifact is recovered from state, a call to LoadState is made.
    // 4. Start is called to kick off the work.
    // 5. During each checkpoint:
    //    a. The StateChanged property is called to check if the artifact is dirty.
    //    b. SaveState is called if the artifact was dirty.
    //    c. StateSaved is called if the checkpoint was successfully persisted.
    // 6. If the engine is unloaded and the artifact implement IUnloadable, Unload is called.
    // 7. When the artifact is disposed, Dispose is called.
    //

    internal sealed class EgressObserver<T> : StatefulObserver<T>
    {
        private readonly string _name;
        private IReliableObserver<T> _observer;
        private long _sequenceId;

        public EgressObserver(string name) => _name = name;

        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            if (!context.TryGetElement<IIngressEgressManager>("IngressEgressManager", out var iemgr))
            {
                throw new InvalidOperationException("Ingress/egress manager not found");
            }

            _observer = iemgr.GetObserver<T>(_name);
        }

        public override string Name => "sb:Egress";

        public override Version Version => new(1, 0, 0, 0);

        protected override void OnNextCore(T value)
        {
            _observer.OnNext(value, _sequenceId++);

            StateChanged = true; // Mark the artifact dirty for differential checkpointing.
        }

        protected override void OnErrorCore(Exception error) => _observer.OnError(error);

        protected override void OnCompletedCore() => _observer.OnCompleted();

        protected override void OnStart()
        {
            base.OnStart();

            _observer.OnStarted();
        }

        protected override void SaveStateCore(IOperatorStateWriter writer)
        {
            base.SaveStateCore(writer);

            writer.Write(_sequenceId);
        }

        protected override void LoadStateCore(IOperatorStateReader reader)
        {
            base.LoadStateCore(reader);

            _sequenceId = reader.Read<long>();
        }
    }
}
