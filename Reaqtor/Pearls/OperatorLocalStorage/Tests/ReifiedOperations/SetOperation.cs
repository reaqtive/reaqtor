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
    internal enum SetOperationKind
    {
        Add,
        UnionWith,
        IntersectWith,
        ExceptWith,
        SymmetricExceptWith,
        IsSubsetOf,
        IsSupersetOf,
        IsProperSupersetOf,
        IsProperSubsetOf,
        Overlaps,
        SetEquals,
    }

    internal interface ISetOperationFactory<T> : ICollectionOperationFactory<T>
    {
        new AddSetOperation<T> Add(T value);
        UnionWithSetOperation<T> UnionWith(IEnumerable<T> other);
        IntersectWithSetOperation<T> IntersectWith(IEnumerable<T> other);
        ExceptWithSetOperation<T> ExceptWith(IEnumerable<T> other);
        SymmetricExceptWithSetOperation<T> SymmetricExceptWith(IEnumerable<T> other);
        IsSubsetOfSetOperation<T> IsSubsetOf(IEnumerable<T> other);
        IsSupersetOfSetOperation<T> IsSupersetOf(IEnumerable<T> other);
        IsProperSupersetOfSetOperation<T> IsProperSupersetOf(IEnumerable<T> other);
        IsProperSubsetOfSetOperation<T> IsProperSubsetOf(IEnumerable<T> other);
        OverlapsSetOperation<T> Overlaps(IEnumerable<T> other);
        SetEqualsSetOperation<T> SetEquals(IEnumerable<T> other);
    }

    internal abstract class SetOperation : OperationBase
    {
        public abstract SetOperationKind Kind { get; }

        public static ISetOperationFactory<T> WithElementType<T>() => new SetOperationFactory<T>();

        public static AddSetOperation<T> Add<T>(T value) => new(value);

        public static ClearCollectionOperation<T> Clear<T>() => new();

        public static ContainsCollectionOperation<T> Contains<T>(T value) => new(value);

        public static CopyToCollectionOperation<T> CopyTo<T>(T[] array, int index) => new(array, index);

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static IsReadOnlyCollectionOperation<T> IsReadOnly<T>() => new();

        public static RemoveCollectionOperation<T> Remove<T>(T value) => new(value);

        public static UnionWithSetOperation<T> UnionWith<T>(IEnumerable<T> other) => new(other);

        public static IntersectWithSetOperation<T> IntersectWith<T>(IEnumerable<T> other) => new(other);

        public static ExceptWithSetOperation<T> ExceptWith<T>(IEnumerable<T> other) => new(other);

        public static SymmetricExceptWithSetOperation<T> SymmetricExceptWith<T>(IEnumerable<T> other) => new(other);

        public static IsSubsetOfSetOperation<T> IsSubsetOf<T>(IEnumerable<T> other) => new(other);

        public static IsSupersetOfSetOperation<T> IsSupersetOf<T>(IEnumerable<T> other) => new(other);

        public static IsProperSupersetOfSetOperation<T> IsProperSupersetOf<T>(IEnumerable<T> other) => new(other);

        public static IsProperSubsetOfSetOperation<T> IsProperSubsetOf<T>(IEnumerable<T> other) => new(other);

        public static OverlapsSetOperation<T> Overlaps<T>(IEnumerable<T> other) => new(other);

        public static SetEqualsSetOperation<T> SetEquals<T>(IEnumerable<T> other) => new(other);

        private sealed class SetOperationFactory<T> : ISetOperationFactory<T>
        {
            public AddSetOperation<T> Add(T value) => Add<T>(value);

            public ClearCollectionOperation<T> Clear() => Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => Contains<T>(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => CopyTo<T>(array, index);

            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => IsReadOnly<T>();

            public RemoveCollectionOperation<T> Remove(T value) => Remove<T>(value);

            AddCollectionOperation<T> ICollectionOperationFactory<T>.Add(T value) => new(value);

            public UnionWithSetOperation<T> UnionWith(IEnumerable<T> other) => UnionWith<T>(other);

            public IntersectWithSetOperation<T> IntersectWith(IEnumerable<T> other) => IntersectWith<T>(other);

            public ExceptWithSetOperation<T> ExceptWith(IEnumerable<T> other) => ExceptWith<T>(other);

            public SymmetricExceptWithSetOperation<T> SymmetricExceptWith(IEnumerable<T> other) => SymmetricExceptWith<T>(other);

            public IsSubsetOfSetOperation<T> IsSubsetOf(IEnumerable<T> other) => IsSubsetOf<T>(other);

            public IsSupersetOfSetOperation<T> IsSupersetOf(IEnumerable<T> other) => IsSupersetOf<T>(other);

            public IsProperSupersetOfSetOperation<T> IsProperSupersetOf(IEnumerable<T> other) => IsProperSupersetOf<T>(other);

            public IsProperSubsetOfSetOperation<T> IsProperSubsetOf(IEnumerable<T> other) => IsProperSubsetOf<T>(other);

            public OverlapsSetOperation<T> Overlaps(IEnumerable<T> other) => Overlaps<T>(other);

            public SetEqualsSetOperation<T> SetEquals(IEnumerable<T> other) => SetEquals<T>(other);
        }
    }

    internal abstract class SetOperation<T> : SetOperation, IOperation<ISet<T>>
    {
        public abstract void Accept(ISet<T> set);
    }

    internal abstract class ResultSetOperation<T, R> : SetOperation<T>, IResultOperation<ISet<T>, R>
    {
        public sealed override void Accept(ISet<T> set) => _ = GetResult(set);

        public abstract R GetResult(ISet<T> set);
    }

    internal abstract class OperatorSetOperation<T> : SetOperation<T>
    {
        public OperatorSetOperation(IEnumerable<T> other)
        {
            Other = other;
        }

        public IEnumerable<T> Other { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}({Other})");
    }

    internal abstract class PredicateSetOperation<T> : ResultSetOperation<T, bool>
    {
        public PredicateSetOperation(IEnumerable<T> other)
        {
            Other = other;
        }

        public IEnumerable<T> Other { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}({Other})");
    }

    internal sealed class AddSetOperation<T> : ResultSetOperation<T, bool>
    {
        internal AddSetOperation(T value)
        {
            Value = value;
        }

        public override SetOperationKind Kind => SetOperationKind.Add;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Add({Value})");

        public override bool GetResult(ISet<T> set) => set.Add(Value);
    }

    internal sealed class UnionWithSetOperation<T> : OperatorSetOperation<T>
    {
        public UnionWithSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.UnionWith;

        public override void Accept(ISet<T> set) => set.UnionWith(Other);
    }

    internal sealed class IntersectWithSetOperation<T> : OperatorSetOperation<T>
    {
        public IntersectWithSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.IntersectWith;

        public override void Accept(ISet<T> set) => set.IntersectWith(Other);
    }

    internal sealed class ExceptWithSetOperation<T> : OperatorSetOperation<T>
    {
        public ExceptWithSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.ExceptWith;

        public override void Accept(ISet<T> set) => set.ExceptWith(Other);
    }

    internal sealed class SymmetricExceptWithSetOperation<T> : OperatorSetOperation<T>
    {
        public SymmetricExceptWithSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.SymmetricExceptWith;

        public override void Accept(ISet<T> set) => set.SymmetricExceptWith(Other);
    }

    internal sealed class IsSubsetOfSetOperation<T> : PredicateSetOperation<T>
    {
        public IsSubsetOfSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.IsSubsetOf;

        public override bool GetResult(ISet<T> set) => set.IsSubsetOf(Other);
    }

    internal sealed class IsSupersetOfSetOperation<T> : PredicateSetOperation<T>
    {
        public IsSupersetOfSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.IsSupersetOf;

        public override bool GetResult(ISet<T> set) => set.IsSupersetOf(Other);
    }

    internal sealed class IsProperSupersetOfSetOperation<T> : PredicateSetOperation<T>
    {
        public IsProperSupersetOfSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.IsProperSupersetOf;

        public override bool GetResult(ISet<T> set) => set.IsProperSupersetOf(Other);
    }

    internal sealed class IsProperSubsetOfSetOperation<T> : PredicateSetOperation<T>
    {
        public IsProperSubsetOfSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.IsProperSubsetOf;

        public override bool GetResult(ISet<T> set) => set.IsProperSubsetOf(Other);
    }

    internal sealed class OverlapsSetOperation<T> : PredicateSetOperation<T>
    {
        public OverlapsSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.Overlaps;

        public override bool GetResult(ISet<T> set) => set.Overlaps(Other);
    }

    internal sealed class SetEqualsSetOperation<T> : PredicateSetOperation<T>
    {
        public SetEqualsSetOperation(IEnumerable<T> other)
            : base(other)
        {
        }

        public override SetOperationKind Kind => SetOperationKind.SetEquals;

        public override bool GetResult(ISet<T> set) => set.SetEquals(Other);
    }
}
