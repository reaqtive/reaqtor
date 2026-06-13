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
    internal enum ReadOnlyListOperationKind
    {
        Get,
    }

    internal interface IReadOnlyListOperationFactory<T> : IReadOnlyCollectionOperationFactory<T>
    {
        GetReadOnlyListOperation<T> Get(int index);
    }

    internal abstract class ReadOnlyListOperation : OperationBase
    {
        public abstract ReadOnlyListOperationKind Kind { get; }

        public static IReadOnlyListOperationFactory<T> WithElementType<T>() => new ReadOnlyListOperationFactory<T>();

        public static GetReadOnlyListOperation<T> Get<T>(int index) => new(index);

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        private sealed class ReadOnlyListOperationFactory<T> : IReadOnlyListOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public GetReadOnlyListOperation<T> Get(int index) => Get<T>(index);
        }
    }

    internal abstract class ReadOnlyListOperation<T> : ReadOnlyListOperation, IOperation<IReadOnlyList<T>>
    {
        public abstract void Accept(IReadOnlyList<T> list);
    }

    internal abstract class ResultReadOnlyListOperation<T, R> : ReadOnlyListOperation<T>, IResultOperation<IReadOnlyList<T>, R>
    {
        public sealed override void Accept(IReadOnlyList<T> list) => _ = GetResult(list);

        public abstract R GetResult(IReadOnlyList<T> list);
    }

    internal sealed class GetReadOnlyListOperation<T> : ResultReadOnlyListOperation<T, T>
    {
        internal GetReadOnlyListOperation(int index)
        {
            Index = index;
        }

        public override ReadOnlyListOperationKind Kind => ReadOnlyListOperationKind.Get;

        public int Index { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Get({Index})");

        public override T GetResult(IReadOnlyList<T> list) => list[Index];
    }
}
