﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;
using Reaqtive.Storage;

namespace Tests.ReifiedOperations
{
    internal interface IPersistedSetOperationFactory<T> : ISetOperationFactory<T>, IPersistedOperationFactory<IPersistedSet<T>>
    {
    }

    internal static class PersistedSetOperation
    {
        public static IPersistedSetOperationFactory<T> WithType<T>() => new PersistedSetOperationFactory<T>();

        private sealed class PersistedSetOperationFactory<T> : IPersistedSetOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => SetOperation.Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => SetOperation.Enumerate<T>();

            public GetIdPersistedOperation<IPersistedSet<T>> GetId() => GetId<IPersistedSet<T>>();

            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => PersistedOperation.GetId<TPersisted>();

            public AddSetOperation<T> Add(T value) => SetOperation.Add(value);

            public UnionWithSetOperation<T> UnionWith(IEnumerable<T> other) => SetOperation.UnionWith(other);

            public IntersectWithSetOperation<T> IntersectWith(IEnumerable<T> other) => SetOperation.IntersectWith(other);

            public ExceptWithSetOperation<T> ExceptWith(IEnumerable<T> other) => SetOperation.ExceptWith(other);

            public SymmetricExceptWithSetOperation<T> SymmetricExceptWith(IEnumerable<T> other) => SetOperation.SymmetricExceptWith(other);

            public IsSubsetOfSetOperation<T> IsSubsetOf(IEnumerable<T> other) => SetOperation.IsSubsetOf(other);

            public IsSupersetOfSetOperation<T> IsSupersetOf(IEnumerable<T> other) => SetOperation.IsSupersetOf(other);

            public IsProperSupersetOfSetOperation<T> IsProperSupersetOf(IEnumerable<T> other) => SetOperation.IsProperSupersetOf(other);

            public IsProperSubsetOfSetOperation<T> IsProperSubsetOf(IEnumerable<T> other) => SetOperation.IsProperSubsetOf(other);

            public OverlapsSetOperation<T> Overlaps(IEnumerable<T> other) => SetOperation.Overlaps(other);

            public SetEqualsSetOperation<T> SetEquals(IEnumerable<T> other) => SetOperation.SetEquals(other);

            AddCollectionOperation<T> ICollectionOperationFactory<T>.Add(T value) => CollectionOperation.Add(value);

            public ClearCollectionOperation<T> Clear() => SetOperation.Clear<T>();

            public ContainsCollectionOperation<T> Contains(T value) => SetOperation.Contains(value);

            public CopyToCollectionOperation<T> CopyTo(T[] array, int index) => SetOperation.CopyTo(array, index);

            public RemoveCollectionOperation<T> Remove(T value) => SetOperation.Remove(value);

            public IsReadOnlyCollectionOperation<T> IsReadOnly() => SetOperation.IsReadOnly<T>();

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();

            public ThisResultOperation<IPersistedSet<T>> This() => This<IPersistedSet<T>>();
        }
    }
}
