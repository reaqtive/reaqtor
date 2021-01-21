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
    internal enum ReadOnlyCollectionOperationKind
    {
        Count,
    }

    internal interface IReadOnlyCollectionOperationFactory<T> : IEnumerableOperationFactory<T>
    {
        CountReadOnlyCollectionOperation<T> Count();
    }

    internal abstract class ReadOnlyCollectionOperation : OperationBase
    {
        public abstract ReadOnlyCollectionOperationKind Kind { get; }

        public static IReadOnlyCollectionOperationFactory<T> WithElementType<T>() => new ReadOnlyCollectionOperationFactory<T>();

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        private sealed class ReadOnlyCollectionOperationFactory<T> : IReadOnlyCollectionOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();
        }
    }

    internal abstract class ReadOnlyCollectionOperation<T> : ReadOnlyCollectionOperation, IOperation<IReadOnlyCollection<T>>
    {
        public abstract void Accept(IReadOnlyCollection<T> collection);
    }

    internal abstract class ResultReadOnlyCollectionOperation<T, R> : ReadOnlyCollectionOperation<T>, IResultOperation<IReadOnlyCollection<T>, R>
    {
        public sealed override void Accept(IReadOnlyCollection<T> collection) => _ = GetResult(collection);

        public abstract R GetResult(IReadOnlyCollection<T> collection);
    }

    internal sealed class CountReadOnlyCollectionOperation<T> : ResultReadOnlyCollectionOperation<T, int>
    {
        internal CountReadOnlyCollectionOperation() { }

        public override ReadOnlyCollectionOperationKind Kind => ReadOnlyCollectionOperationKind.Count;

        protected override string DebugViewCore => "Count()";

        public override int GetResult(IReadOnlyCollection<T> collection) => collection.Count;
    }
}
