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
    internal enum StackOperationKind
    {
        Pop,
        Push,
        Peek,
    }

    internal interface IStackOperationFactory<T> : IReadOnlyCollectionOperationFactory<T>
    {
        PopStackOperation<T> Pop();
        PushStackOperation<T> Push(T value);
        PeekStackOperation<T> Peek();
    }

    internal abstract class StackOperation : OperationBase
    {
        public abstract StackOperationKind Kind { get; }

        public static IStackOperationFactory<T> WithElementType<T>() => new StackOperationFactory<T>();

        public static CountReadOnlyCollectionOperation<T> Count<T>() => new();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        public static PushStackOperation<T> Push<T>(T value) => new(value);

        public static PeekStackOperation<T> Peek<T>() => new();

        public static PopStackOperation<T> Pop<T>() => new();

        private sealed class StackOperationFactory<T> : IStackOperationFactory<T>
        {
            public CountReadOnlyCollectionOperation<T> Count() => Count<T>();

            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();

            public PushStackOperation<T> Push(T value) => Push<T>(value);

            public PeekStackOperation<T> Peek() => Peek<T>();

            public PopStackOperation<T> Pop() => Pop<T>();
        }
    }

    internal abstract class StackOperation<T> : StackOperation, IOperation<IStack<T>>
    {
        public abstract void Accept(IStack<T> stack);
    }

    internal abstract class ResultStackOperation<T, R> : StackOperation<T>, IResultOperation<IStack<T>, R>
    {
        public sealed override void Accept(IStack<T> stack) => _ = GetResult(stack);

        public abstract R GetResult(IStack<T> stack);
    }

    internal sealed class PushStackOperation<T> : StackOperation<T>
    {
        internal PushStackOperation(T value)
        {
            Value = value;
        }

        public override StackOperationKind Kind => StackOperationKind.Push;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Push({Value})");

        public override void Accept(IStack<T> stack) => stack.Push(Value);
    }

    internal sealed class PeekStackOperation<T> : ResultStackOperation<T, T>
    {
        internal PeekStackOperation() { }

        public override StackOperationKind Kind => StackOperationKind.Peek;

        protected override string DebugViewCore => FormattableString.Invariant($"Peek()");

        public override T GetResult(IStack<T> stack) => stack.Peek();
    }

    internal sealed class PopStackOperation<T> : ResultStackOperation<T, T>
    {
        internal PopStackOperation() { }

        public override StackOperationKind Kind => StackOperationKind.Pop;

        protected override string DebugViewCore => FormattableString.Invariant($"Pop()");

        public override T GetResult(IStack<T> stack) => stack.Pop();
    }
}
