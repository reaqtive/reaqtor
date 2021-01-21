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
    internal enum CollectionOperationKind
    {
        Add,
        Clear,
        Contains,
        CopyTo,
        IsReadOnly,
        Remove,
    }

    internal interface ICollectionOperationFactory<T> : IReadOnlyCollectionOperationFactory<T>
    {
        AddCollectionOperation<T> Add(T value);
        ClearCollectionOperation<T> Clear();
        ContainsCollectionOperation<T> Contains(T value);
        CopyToCollectionOperation<T> CopyTo(T[] array, int index);
        RemoveCollectionOperation<T> Remove(T value);
        IsReadOnlyCollectionOperation<T> IsReadOnly();
    }

    internal abstract class CollectionOperation : OperationBase
    {
        public abstract CollectionOperationKind Kind { get; }

        public static ICollectionOperationFactory<T> WithElementType<T>() => new CollectionOperationFactory<T>();

        public static AddCollectionOperation<T> Add<T>(T value) => new(value);

        public static ClearCollectionOperation<T> Clear<T>() => new();

        public static ContainsCollectionOperation<T> Contains<T>(T value) => new(value);

        public static CopyToCollectionOperation<T> CopyTo<T>(T[] array, int index) => new(array, index);

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static IsReadOnlyCollectionOperation<T> IsReadOnly<T>() => new();

        public static RemoveCollectionOperation<T> Remove<T>(T value) => new(value);

        private sealed class CollectionOperationFactory<T> : ICollectionOperationFactory<T>
        {
            public AddCollectionOperation<T> Add(T value) => Add<T>(value);

            public ClearCollectionOperation<T> Clear() => Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => Contains<T>(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => CopyTo<T>(array, index);

            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => IsReadOnly<T>();

            public RemoveCollectionOperation<T> Remove(T value) => Remove<T>(value);
        }
    }

    internal abstract class CollectionOperation<T> : CollectionOperation, IOperation<ICollection<T>>
    {
        public abstract void Accept(ICollection<T> collection);
    }

    internal abstract class ResultCollectionOperation<T, R> : CollectionOperation<T>, IResultOperation<ICollection<T>, R>
    {
        public sealed override void Accept(ICollection<T> collection) => _ = GetResult(collection);

        public abstract R GetResult(ICollection<T> collection);
    }

    internal sealed class AddCollectionOperation<T> : CollectionOperation<T>
    {
        internal AddCollectionOperation(T value)
        {
            Value = value;
        }

        public override CollectionOperationKind Kind => CollectionOperationKind.Add;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Add({Value})");

        public override void Accept(ICollection<T> collection) => collection.Add(Value);
    }

    internal sealed class CopyToCollectionOperation<T> : CollectionOperation<T>
    {
        internal CopyToCollectionOperation(T[] array, int index)
        {
            Array = array;
            Index = index;
        }

        public override CollectionOperationKind Kind => CollectionOperationKind.CopyTo;

        public T[] Array { get; }
        public int Index { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"CopyTo({Array}, {Index})");

        public override void Accept(ICollection<T> collection) => collection.CopyTo(Array, Index);
    }

    internal sealed class ClearCollectionOperation<T> : CollectionOperation<T>
    {
        internal ClearCollectionOperation() { }

        public override CollectionOperationKind Kind => CollectionOperationKind.Clear;

        protected override string DebugViewCore => "Clear()";

        public override void Accept(ICollection<T> collection) => collection.Clear();
    }

    internal sealed class ContainsCollectionOperation<T> : ResultCollectionOperation<T, bool>
    {
        internal ContainsCollectionOperation(T value)
        {
            Value = value;
        }

        public override CollectionOperationKind Kind => CollectionOperationKind.Contains;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Contains({Value})");

        public override bool GetResult(ICollection<T> collection) => collection.Contains(Value);
    }

    internal sealed class RemoveCollectionOperation<T> : ResultCollectionOperation<T, bool>
    {
        internal RemoveCollectionOperation(T value)
        {
            Value = value;
        }

        public override CollectionOperationKind Kind => CollectionOperationKind.Remove;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Remove({Value})");

        public override bool GetResult(ICollection<T> collection) => collection.Remove(Value);
    }

    internal sealed class IsReadOnlyCollectionOperation<T> : ResultCollectionOperation<T, bool>
    {
        internal IsReadOnlyCollectionOperation() { }

        public override CollectionOperationKind Kind => CollectionOperationKind.IsReadOnly;

        protected override string DebugViewCore => "IsReadOnly()";

        public override bool GetResult(ICollection<T> collection) => collection.IsReadOnly;
    }
}
