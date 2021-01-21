// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;

using Reaqtive;

namespace Playground
{
    internal static partial class EngineIntegrationTests
    {
        /// <summary>
        /// Source of events received from a subject registered in a <see cref="StreamManager"/> accessed through the <see cref="IOperatorContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the events.</typeparam>
        private sealed class Source<T> : SubscribableBase<T>
        {
            private readonly string _id;

            public Source(string id)
            {
                _id = id;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer) => new _(this, observer);

            private sealed class _ : ContextSwitchOperator<Source<T>, T>, IUnloadableOperator
            {
                private StreamManager _streamManager;
                private IDisposable _dispose;

                public _(Source<T> parent, IObserver<T> observer)
                    : base(parent, observer)
                {
                }

                public override string Name => "Source";

                public override Version Version => new(1, 0, 0, 0);

                public override void SetContext(IOperatorContext context)
                {
                    base.SetContext(context);

                    if (!context.TryGetElement("StreamManager", out _streamManager))
                    {
                        throw new InvalidOperationException("Need stream manager.");
                    }
                }

                protected override void OnStart()
                {
                    base.OnStart();

                    _dispose = _streamManager.GetSubject<T>(Params._id).Subscribe(this);
                }

                protected override void OnDispose()
                {
                    base.OnDispose();

                    _dispose?.Dispose();
                }

                public void Unload()
                {
                    _dispose?.Dispose();
                }
            }
        }
    }
}
