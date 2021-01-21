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
        /// Sink for events sent into a subject registered in a <see cref="StreamManager"/> accessed through the <see cref="IOperatorContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the events.</typeparam>
        private sealed class Sink<T> : StatefulObserver<T>
        {
            private readonly string _id;
            private StreamManager _streamManager;
            private IObserver<T> _observer;

            public Sink(string id)
            {
                _id = id;
            }

            public override string Name => "Sink";

            public override Version Version => new(1, 0, 0, 0);

            protected override void OnCompletedCore() => _observer.OnCompleted();

            protected override void OnErrorCore(Exception error) => _observer.OnError(error);

            protected override void OnNextCore(T value) => _observer.OnNext(value);

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
                _observer = _streamManager.GetSubject<T>(_id);
            }

            protected override void OnDispose()
            {
            }
        }
    }
}
