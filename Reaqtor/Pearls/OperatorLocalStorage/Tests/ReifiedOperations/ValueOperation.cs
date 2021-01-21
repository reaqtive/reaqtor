// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;

namespace Tests.ReifiedOperations
{
    internal enum ValueOperationKind
    {
        Get,
        Set,
    }

    internal interface IValueOperationFactory<T>
    {
        GetValueOperation<T> Get();

        SetValueOperation<T> Set(T value);
    }

    internal abstract class ValueOperation : OperationBase
    {
        public abstract ValueOperationKind Kind { get; }

        public static IValueOperationFactory<T> WithType<T>() => new ValueOperationFactory<T>();

        public static GetValueOperation<T> Get<T>() => new();

        public static SetValueOperation<T> Set<T>(T value) => new(value);

        private sealed class ValueOperationFactory<T> : IValueOperationFactory<T>
        {
            public GetValueOperation<T> Get() => Get<T>();

            public SetValueOperation<T> Set(T value) => Set<T>(value);
        }
    }

    internal abstract class ValueOperation<T> : ValueOperation, IOperation<IValue<T>>
    {
        public abstract void Accept(IValue<T> value);
    }

    internal abstract class ResultValueOperation<T, R> : ValueOperation<T>, IResultOperation<IValue<T>, R>
    {
        public sealed override void Accept(IValue<T> value) => _ = GetResult(value);

        public abstract R GetResult(IValue<T> value);
    }

    internal sealed class GetValueOperation<T> : ResultValueOperation<T, T>
    {
        internal GetValueOperation() { }

        public override ValueOperationKind Kind => ValueOperationKind.Get;

        protected override string DebugViewCore => "Get()";

        public override T GetResult(IValue<T> value) => value.Value;
    }

    internal sealed class SetValueOperation<T> : ValueOperation<T>
    {
        internal SetValueOperation(T value)
        {
            Value = value;
        }

        public override ValueOperationKind Kind => ValueOperationKind.Set;

        public T Value { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Set({Value})");

        public override void Accept(IValue<T> value) => value.Value = Value;
    }
}
