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
    // TODO: Add [ReadOnly]LinkedListNodeOperation.

    internal enum LinkedListOperationKind
    {
        First,
        Last,

        AddAfter,
        AddBefore,
        AddFirst,
        AddLast,

        Find,
        FindLast,

        Remove,
        RemoveFirst,
        RemoveLast,
    }

    internal interface ILinkedListOperationFactory<T> : ICollectionOperationFactory<T>, IReadOnlyLinkedListOperationFactory<T>
    {
        new FirstLinkedListOperation<T> First();
        new LastLinkedListOperation<T> Last();

        AddAfterLinkedListOperation<T> AddAfter(ILinkedListNode<T> node, T value);
        AddBeforeLinkedListOperation<T> AddBefore(ILinkedListNode<T> node, T value);
        AddFirstLinkedListOperation<T> AddFirst(T value);
        AddLastLinkedListOperation<T> AddLast(T value);

        FindLinkedListOperation<T> Find(T value);
        FindLastLinkedListOperation<T> FindLast(T value);

        RemoveLinkedListOperation<T> Remove(ILinkedListNode<T> node);
        RemoveFirstLinkedListOperation<T> RemoveFirst();
        RemoveLastLinkedListOperation<T> RemoveLast();
    }

    internal abstract class LinkedListOperation : OperationBase
    {
        public abstract LinkedListOperationKind Kind { get; }

        public static ILinkedListOperationFactory<T> WithElementType<T>() => new LinkedListOperationFactory<T>();

        public static AddCollectionOperation<T> Add<T>(T value) => new(value);

        public static AddAfterLinkedListOperation<T> AddAfter<T>(ILinkedListNode<T> node, T value) => new(node, value);

        public static AddBeforeLinkedListOperation<T> AddBefore<T>(ILinkedListNode<T> node, T value) => new(node, value);

        public static AddFirstLinkedListOperation<T> AddFirst<T>(T value) => new(value);

        public static AddLastLinkedListOperation<T> AddLast<T>(T value) => new(value);

        public static ClearCollectionOperation<T> Clear<T>() => new();

        public static ContainsCollectionOperation<T> Contains<T>(T value) => new(value);

        public static CopyToCollectionOperation<T> CopyTo<T>(T[] array, int index) => new(array, index);

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static FindLinkedListOperation<T> Find<T>(T value) => new(value);

        public static FindLastLinkedListOperation<T> FindLast<T>(T value) => new(value);

        public static FirstLinkedListOperation<T> First<T>() => new();

        public static IsReadOnlyCollectionOperation<T> IsReadOnly<T>() => new();

        public static LastLinkedListOperation<T> Last<T>() => new();

        public static RemoveCollectionOperation<T> Remove<T>(T value) => new(value);

        public static RemoveLinkedListOperation<T> Remove<T>(ILinkedListNode<T> node) => new(node);

        public static RemoveFirstLinkedListOperation<T> RemoveFirst<T>() => new();

        public static RemoveLastLinkedListOperation<T> RemoveLast<T>() => new();

        private sealed class LinkedListOperationFactory<T> : ILinkedListOperationFactory<T>
        {
            public AddCollectionOperation<T> Add(T value) => Add<T>(value);

            public AddAfterLinkedListOperation<T> AddAfter(ILinkedListNode<T> node, T value) => AddAfter<T>(node, value);

            public AddBeforeLinkedListOperation<T> AddBefore(ILinkedListNode<T> node, T value) => AddBefore<T>(node, value);

            public AddFirstLinkedListOperation<T> AddFirst(T value) => AddFirst<T>(value);

            public AddLastLinkedListOperation<T> AddLast(T value) => AddLast<T>(value);

            public ClearCollectionOperation<T> Clear() => Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => Contains<T>(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => CopyTo<T>(array, index);

            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public FindLinkedListOperation<T> Find(T value) => Find<T>(value);

            public FindLastLinkedListOperation<T> FindLast(T value) => FindLast<T>(value);

            public FirstLinkedListOperation<T> First() => First<T>();

            FirstReadOnlyLinkedListOperation<T> IReadOnlyLinkedListOperationFactory<T>.First() => ReadOnlyLinkedListOperation.First<T>();

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => IsReadOnly<T>();

            public LastLinkedListOperation<T> Last() => Last<T>();

            LastReadOnlyLinkedListOperation<T> IReadOnlyLinkedListOperationFactory<T>.Last() => ReadOnlyLinkedListOperation.Last<T>();

            public RemoveCollectionOperation<T> Remove(T value) => Remove<T>(value);

            public RemoveLinkedListOperation<T> Remove(ILinkedListNode<T> node) => Remove<T>(node);

            public RemoveFirstLinkedListOperation<T> RemoveFirst() => RemoveFirst<T>();

            public RemoveLastLinkedListOperation<T> RemoveLast() => RemoveLast<T>();
        }
    }

    internal abstract class LinkedListOperation<T> : LinkedListOperation, IOperation<ILinkedList<T>>
    {
        public abstract void Accept(ILinkedList<T> list);
    }

    internal abstract class ResultLinkedListOperation<T, R> : LinkedListOperation<T>, IResultOperation<ILinkedList<T>, R>
    {
        public sealed override void Accept(ILinkedList<T> list) => _ = GetResult(list);

        public abstract R GetResult(ILinkedList<T> list);
    }

    internal sealed class AddAfterLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal AddAfterLinkedListOperation(ILinkedListNode<T> node, T value)
        {
            Node = node;
            Value = value;
        }

        public ILinkedListNode<T> Node { get; }

        public T Value { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.AddAfter;

        protected override string DebugViewCore => FormattableString.Invariant($"AddAfter({Node}, {Value})");

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.AddAfter(Node, Value);
    }

    internal sealed class AddBeforeLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal AddBeforeLinkedListOperation(ILinkedListNode<T> node, T value)
        {
            Node = node;
            Value = value;
        }

        public ILinkedListNode<T> Node { get; }

        public T Value { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.AddBefore;

        protected override string DebugViewCore => FormattableString.Invariant($"AddBefore({Node}, {Value})");

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.AddBefore(Node, Value);
    }

    internal sealed class AddFirstLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal AddFirstLinkedListOperation(T value) => Value = value;

        public T Value { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.AddFirst;

        protected override string DebugViewCore => FormattableString.Invariant($"AddFirst({Value})");

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.AddFirst(Value);
    }

    internal sealed class AddLastLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal AddLastLinkedListOperation(T value) => Value = value;

        public T Value { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.AddLast;

        protected override string DebugViewCore => FormattableString.Invariant($"AddLast({Value})");

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.AddLast(Value);
    }

    internal sealed class FirstLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal FirstLinkedListOperation() { }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.First;

        protected override string DebugViewCore => "First()";

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.First;
    }

    internal sealed class LastLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal LastLinkedListOperation() { }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.Last;

        protected override string DebugViewCore => "Last()";

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.Last;
    }

    internal sealed class FindLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal FindLinkedListOperation(T value) => Value = value;

        public T Value { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.Find;

        protected override string DebugViewCore => FormattableString.Invariant($"Find({Value})");

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.Find(Value);
    }

    internal sealed class FindLastLinkedListOperation<T> : ResultLinkedListOperation<T, ILinkedListNode<T>>
    {
        internal FindLastLinkedListOperation(T value) => Value = value;

        public T Value { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.FindLast;

        protected override string DebugViewCore => FormattableString.Invariant($"FindLast({Value})");

        public override ILinkedListNode<T> GetResult(ILinkedList<T> list) => list.FindLast(Value);
    }

    internal sealed class RemoveLinkedListOperation<T> : LinkedListOperation<T>
    {
        internal RemoveLinkedListOperation(ILinkedListNode<T> node) => Node = node;

        public ILinkedListNode<T> Node { get; }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.Remove;

        protected override string DebugViewCore => FormattableString.Invariant($"Remove({Node})");

        public override void Accept(ILinkedList<T> list) => list.Remove(Node);
    }

    internal sealed class RemoveFirstLinkedListOperation<T> : LinkedListOperation<T>
    {
        internal RemoveFirstLinkedListOperation() { }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.RemoveFirst;

        protected override string DebugViewCore => "RemoveFirst()";

        public override void Accept(ILinkedList<T> list) => list.RemoveFirst();
    }

    internal sealed class RemoveLastLinkedListOperation<T> : LinkedListOperation<T>
    {
        internal RemoveLastLinkedListOperation() { }

        public override LinkedListOperationKind Kind => LinkedListOperationKind.RemoveLast;

        protected override string DebugViewCore => "RemoveLast()";

        public override void Accept(ILinkedList<T> list) => list.RemoveLast();
    }
}
