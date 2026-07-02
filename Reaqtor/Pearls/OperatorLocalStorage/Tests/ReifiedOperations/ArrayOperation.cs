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
    internal enum ArrayOperationKind
    {
        Length,
        Set,
    }

    internal interface IArrayOperationFactory<T> : IReadOnlyListOperationFactory<T>
    {
        LengthArrayOperation<T> Length();

        SetArrayOperation<T> Set(int index, T value);
    }

    internal abstract class ArrayOperation : OperationBase
    {
        public abstract ArrayOperationKind Kind { get; }

        public static IArrayOperationFactory<T> WithElementType<T>() => new ArrayOperationFactory<T>();

        public static LengthArrayOperation<T> Length<T>() => new();

        public static GetReadOnlyListOperation<T> Get<T>(int index) => new(index);

        public static SetArrayOperation<T> Set<T>(int index, T value) => new(index, value);

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        private sealed class ArrayOperationFactory<T> : IArrayOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public GetReadOnlyListOperation<T> Get(int index) => Get<T>(index);

            public LengthArrayOperation<T> Length() => Length<T>();

            public SetArrayOperation<T> Set(int index, T value) => Set<T>(index, value);
        }
    }

    internal abstract class ArrayOperation<T> : ArrayOperation, IOperation<IArray<T>>
    {
        public abstract void Accept(IArray<T> array);
    }

    internal abstract class ResultArrayOperation<T, R> : ArrayOperation<T>, IResultOperation<IArray<T>, R>
    {
        public sealed override void Accept(IArray<T> array) => _ = GetResult(array);

        public abstract R GetResult(IArray<T> array);
    }

    internal sealed class LengthArrayOperation<T> : ResultArrayOperation<T, int>
    {
        internal LengthArrayOperation() { }

        public override ArrayOperationKind Kind => ArrayOperationKind.Length;

        protected override string DebugViewCore => "Length";

        public override int GetResult(IArray<T> array) => array.Length;
    }

    internal sealed class SetArrayOperation<T> : ArrayOperation<T>
    {
        internal SetArrayOperation(int index, T value)
        {
            Index = index;
            Value = value;
        }

        public override ArrayOperationKind Kind => ArrayOperationKind.Set;

        public int Index { get; }
        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Set({Index}, {Value})");

        public override void Accept(IArray<T> array) => array[Index] = Value;
    }
}
