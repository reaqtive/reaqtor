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
    internal enum ListOperationKind
    {
        IndexOf,
        Insert,
        RemoveAt,
        Set,
    }

    internal interface IListOperationFactory<T> : ICollectionOperationFactory<T>, IReadOnlyListOperationFactory<T>
    {
        IndexOfListOperation<T> IndexOf(T value);
        InsertListOperation<T> Insert(int index, T value);
        RemoveAtListOperation<T> RemoveAt(int index);
        SetListOperation<T> Set(int index, T value);
    }

    internal abstract class ListOperation : OperationBase
    {
        public abstract ListOperationKind Kind { get; }

        public static IListOperationFactory<T> WithElementType<T>() => new ListOperationFactory<T>();

        public static AddCollectionOperation<T> Add<T>(T value) => new(value);

        public static ClearCollectionOperation<T> Clear<T>() => new();

        public static ContainsCollectionOperation<T> Contains<T>(T value) => new(value);

        public static CopyToCollectionOperation<T> CopyTo<T>(T[] array, int index) => new(array, index);

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static GetReadOnlyListOperation<T> Get<T>(int index) => new(index);

        public static IndexOfListOperation<T> IndexOf<T>(T value) => new(value);

        public static InsertListOperation<T> Insert<T>(int index, T value) => new(index, value);

        public static IsReadOnlyCollectionOperation<T> IsReadOnly<T>() => new();

        public static RemoveCollectionOperation<T> Remove<T>(T value) => new(value);

        public static RemoveAtListOperation<T> RemoveAt<T>(int index) => new(index);

        public static SetListOperation<T> Set<T>(int index, T value) => new(index, value);

        private sealed class ListOperationFactory<T> : IListOperationFactory<T>
        {
            public AddCollectionOperation<T> Add(T value) => Add<T>(value);

            public ClearCollectionOperation<T> Clear() => Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => Contains<T>(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => CopyTo<T>(array, index);

            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public GetReadOnlyListOperation<T> Get(int index) => Get<T>(index);

            public IndexOfListOperation<T> IndexOf(T value) => IndexOf<T>(value);

            public InsertListOperation<T> Insert(int index, T value) => Insert<T>(index, value);

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => IsReadOnly<T>();

            public RemoveCollectionOperation<T> Remove(T value) => Remove<T>(value);

            public RemoveAtListOperation<T> RemoveAt(int index) => RemoveAt<T>(index);

            public SetListOperation<T> Set(int index, T value) => Set<T>(index, value);
        }
    }

    internal abstract class ListOperation<T> : ListOperation, IOperation<IList<T>>
    {
        public abstract void Accept(IList<T> list);
    }

    internal abstract class ResultListOperation<T, R> : ListOperation<T>, IResultOperation<IList<T>, R>
    {
        public sealed override void Accept(IList<T> list) => _ = GetResult(list);

        public abstract R GetResult(IList<T> list);
    }

    internal sealed class IndexOfListOperation<T> : ResultListOperation<T, int>
    {
        internal IndexOfListOperation(T value)
        {
            Value = value;
        }

        public override ListOperationKind Kind => ListOperationKind.IndexOf;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"IndexOf({Value})");

        public override int GetResult(IList<T> list) => list.IndexOf(Value);
    }

    internal sealed class InsertListOperation<T> : ListOperation<T>
    {
        internal InsertListOperation(int index, T value)
        {
            Index = index;
            Value = value;
        }

        public override ListOperationKind Kind => ListOperationKind.Insert;

        public int Index { get; }
        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Insert({Index}, {Value})");

        public override void Accept(IList<T> list) => list.Insert(Index, Value);
    }

    internal sealed class RemoveAtListOperation<T> : ListOperation<T>
    {
        internal RemoveAtListOperation(int index)
        {
            Index = index;
        }

        public override ListOperationKind Kind => ListOperationKind.RemoveAt;

        public int Index { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"RemoveAt({Index})");

        public override void Accept(IList<T> list) => list.RemoveAt(Index);
    }

    internal sealed class SetListOperation<T> : ListOperation<T>
    {
        internal SetListOperation(int index, T value)
        {
            Index = index;
            Value = value;
        }

        public override ListOperationKind Kind => ListOperationKind.Set;

        public int Index { get; }
        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Set({Index}, {Value})");

        public override void Accept(IList<T> list) => list[Index] = Value;
    }
}
