// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;

namespace Tests.ReifiedOperations
{
    internal enum QueueOperationKind
    {
        Dequeue,
        Enqueue,
        Peek,
    }

    internal interface IQueueOperationFactory<T> : IReadOnlyCollectionOperationFactory<T>
    {
        DequeueQueueOperation<T> Dequeue();
        EnqueueQueueOperation<T> Enqueue(T value);
        PeekQueueOperation<T> Peek();
    }

    internal abstract class QueueOperation : OperationBase
    {
        public abstract QueueOperationKind Kind { get; }

        public static IQueueOperationFactory<T> WithElementType<T>() => new QueueOperationFactory<T>();

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static EnqueueQueueOperation<T> Enqueue<T>(T value) => new(value);

        public static PeekQueueOperation<T> Peek<T>() => new();

        public static DequeueQueueOperation<T> Dequeue<T>() => new();

        private sealed class QueueOperationFactory<T> : IQueueOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public EnqueueQueueOperation<T> Enqueue(T value) => Enqueue<T>(value);

            public PeekQueueOperation<T> Peek() => Peek<T>();

            public DequeueQueueOperation<T> Dequeue() => Dequeue<T>();
        }
    }

    internal abstract class QueueOperation<T> : QueueOperation, IOperation<IQueue<T>>
    {
        public abstract void Accept(IQueue<T> queue);
    }

    internal abstract class ResultQueueOperation<T, R> : QueueOperation<T>, IResultOperation<IQueue<T>, R>
    {
        public sealed override void Accept(IQueue<T> queue) => _ = GetResult(queue);

        public abstract R GetResult(IQueue<T> queue);
    }

    internal sealed class EnqueueQueueOperation<T> : QueueOperation<T>
    {
        internal EnqueueQueueOperation(T value)
        {
            Value = value;
        }

        public override QueueOperationKind Kind => QueueOperationKind.Enqueue;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Enqueue({Value})");

        public override void Accept(IQueue<T> queue) => queue.Enqueue(Value);
    }

    internal sealed class PeekQueueOperation<T> : ResultQueueOperation<T, T>
    {
        internal PeekQueueOperation() { }

        public override QueueOperationKind Kind => QueueOperationKind.Peek;

        protected override string DebugViewCore => FormattableString.Invariant($"Peek()");

        public override T GetResult(IQueue<T> queue) => queue.Peek();
    }

    internal sealed class DequeueQueueOperation<T> : ResultQueueOperation<T, T>
    {
        internal DequeueQueueOperation() { }

        public override QueueOperationKind Kind => QueueOperationKind.Dequeue;

        protected override string DebugViewCore => FormattableString.Invariant($"Dequeue()");

        public override T GetResult(IQueue<T> queue) => queue.Dequeue();
    }
}
