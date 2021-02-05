// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;

namespace Tests.ReifiedOperations
{
    internal enum ReadOnlyLinkedListOperationKind
    {
        First,
        Last,
    }

    internal interface IReadOnlyLinkedListOperationFactory<T> : IReadOnlyCollectionOperationFactory<T>
    {
        FirstReadOnlyLinkedListOperation<T> First();
        LastReadOnlyLinkedListOperation<T> Last();
    }

    internal abstract class ReadOnlyLinkedListOperation : OperationBase
    {
        public abstract ReadOnlyLinkedListOperationKind Kind { get; }

        public static IReadOnlyLinkedListOperationFactory<T> WithElementType<T>() => new ReadOnlyLinkedListOperationFactory<T>();

        public static FirstReadOnlyLinkedListOperation<T> First<T>() => new();

        public static LastReadOnlyLinkedListOperation<T> Last<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        private sealed class ReadOnlyLinkedListOperationFactory<T> : IReadOnlyLinkedListOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public FirstReadOnlyLinkedListOperation<T> First() => First<T>();

            public LastReadOnlyLinkedListOperation<T> Last() => Last<T>();
        }
    }

    internal abstract class ReadOnlyLinkedListOperation<T> : ReadOnlyLinkedListOperation, IOperation<IReadOnlyLinkedList<T>>
    {
        public abstract void Accept(IReadOnlyLinkedList<T> list);
    }

    internal abstract class ResultReadOnlyLinkedListOperation<T, R> : ReadOnlyLinkedListOperation<T>, IResultOperation<IReadOnlyLinkedList<T>, R>
    {
        public sealed override void Accept(IReadOnlyLinkedList<T> list) => _ = GetResult(list);

        public abstract R GetResult(IReadOnlyLinkedList<T> list);
    }

    internal sealed class FirstReadOnlyLinkedListOperation<T> : ResultReadOnlyLinkedListOperation<T, IReadOnlyLinkedListNode<T>>
    {
        public override ReadOnlyLinkedListOperationKind Kind => ReadOnlyLinkedListOperationKind.First;

        protected override string DebugViewCore => "First";

        public override IReadOnlyLinkedListNode<T> GetResult(IReadOnlyLinkedList<T> list) => list.First;
    }

    internal sealed class LastReadOnlyLinkedListOperation<T> : ResultReadOnlyLinkedListOperation<T, IReadOnlyLinkedListNode<T>>
    {
        public override ReadOnlyLinkedListOperationKind Kind => ReadOnlyLinkedListOperationKind.First;

        protected override string DebugViewCore => "Last";

        public override IReadOnlyLinkedListNode<T> GetResult(IReadOnlyLinkedList<T> list) => list.Last;
    }
}
