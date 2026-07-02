// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.Collections.Generic;

namespace Tests.ReifiedOperations
{
    internal enum SortedSetOperationKind
    {
        Min,
        Max,
        Reverse,
        GetViewBetween,
    }

    internal interface ISortedSetOperationFactory<T> : ISetOperationFactory<T>
    {
        MinSortedSetOperation<T> Min();
        MaxSortedSetOperation<T> Max();
        ReverseSortedSetOperation<T> Reverse();
        GetViewBetweenSortedSetOperation<T> GetViewBetween(T lowerValue, T upperValue);
    }

    internal abstract class SortedSetOperation : OperationBase
    {
        public abstract SortedSetOperationKind Kind { get; }

        public static ISortedSetOperationFactory<T> WithElementType<T>() => new SortedSetOperationFactory<T>();

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

        public static MinSortedSetOperation<T> Min<T>() => new();

        public static MaxSortedSetOperation<T> Max<T>() => new();

        public static GetViewBetweenSortedSetOperation<T> GetViewBetween<T>(T lowerValue, T upperValue) => new(lowerValue, upperValue);

        public static ReverseSortedSetOperation<T> Reverse<T>() => new();

        private sealed class SortedSetOperationFactory<T> : ISortedSetOperationFactory<T>
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

            public MinSortedSetOperation<T> Min() => Min<T>();

            public MaxSortedSetOperation<T> Max() => Max<T>();

            public ReverseSortedSetOperation<T> Reverse() => Reverse<T>();

            public GetViewBetweenSortedSetOperation<T> GetViewBetween(T lowerValue, T upperValue) => GetViewBetween<T>(lowerValue, upperValue);
        }
    }

    internal abstract class SortedSetOperation<T> : SortedSetOperation, IOperation<ISortedSet<T>>
    {
        public abstract void Accept(ISortedSet<T> set);
    }

    internal abstract class ResultSortedSetOperation<T, R> : SortedSetOperation<T>, IResultOperation<ISortedSet<T>, R>
    {
        public sealed override void Accept(ISortedSet<T> set) => _ = GetResult(set);

        public abstract R GetResult(ISortedSet<T> set);
    }

    internal sealed class MinSortedSetOperation<T> : ResultSortedSetOperation<T, T>
    {
        public override SortedSetOperationKind Kind => SortedSetOperationKind.Min;

        protected override string DebugViewCore => "Min()";

        public override T GetResult(ISortedSet<T> set) => set.Min;
    }

    internal sealed class MaxSortedSetOperation<T> : ResultSortedSetOperation<T, T>
    {
        public override SortedSetOperationKind Kind => SortedSetOperationKind.Max;

        protected override string DebugViewCore => "Max()";

        public override T GetResult(ISortedSet<T> set) => set.Max;
    }

    internal sealed class ReverseSortedSetOperation<T> : ResultSortedSetOperation<T, IEnumerable<T>>
    {
        public override SortedSetOperationKind Kind => SortedSetOperationKind.Reverse;

        protected override string DebugViewCore => "Reverse()";

        public override IEnumerable<T> GetResult(ISortedSet<T> set) => set.Reverse();
    }

    internal sealed class GetViewBetweenSortedSetOperation<T> : ResultSortedSetOperation<T, ISortedSet<T>>, IReifiedResultOperation<ISortedSet<T>, ISortedSet<T>, ISortedSetOperationFactory<T>>
    {
        public GetViewBetweenSortedSetOperation(T lowerValue, T upperValue)
        {
            LowerValue = lowerValue;
            UpperValue = upperValue;
        }

        public override SortedSetOperationKind Kind => SortedSetOperationKind.GetViewBetween;

        public T LowerValue { get; }
        public T UpperValue { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"GetViewBetween({LowerValue}, {UpperValue})");

        public ISortedSetOperationFactory<T> Reified => PersistedSortedSetOperation.WithType<T>();

        public override ISortedSet<T> GetResult(ISortedSet<T> set) => set.GetViewBetween(LowerValue, UpperValue);

    }
}
