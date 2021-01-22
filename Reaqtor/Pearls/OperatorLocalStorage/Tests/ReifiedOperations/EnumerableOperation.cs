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
    internal enum EnumerableOperationKind
    {
        Enumerate,
    }

    internal interface IEnumerableOperationFactory<T>
    {
        EnumerateEnumerableOperation<T> Enumerate();
    }

    internal abstract class EnumerableOperation : OperationBase
    {
        public abstract EnumerableOperationKind Kind { get; }

        public static IEnumerableOperationFactory<T> WithElementType<T>() => new EnumerableOperationFactory<T>();

        public static EnumerateEnumerableOperation<T> Enumerate<T>() => new();

        private sealed class EnumerableOperationFactory<T> : IEnumerableOperationFactory<T>
        {
            public EnumerateEnumerableOperation<T> Enumerate() => Enumerate<T>();
        }
    }

    internal abstract class EnumerableOperation<T> : EnumerableOperation, IOperation<IEnumerable<T>>
    {
        public abstract void Accept(IEnumerable<T> array);
    }

    internal abstract class ResultEnumerableOperation<T, R> : EnumerableOperation<T>, IResultOperation<IEnumerable<T>, R>
    {
        public sealed override void Accept(IEnumerable<T> array) => _ = GetResult(array);

        public abstract R GetResult(IEnumerable<T> array);
    }

    internal sealed class EnumerateEnumerableOperation<T> : ResultEnumerableOperation<T, IEnumerable<T>>
    {
        internal EnumerateEnumerableOperation() { }

        public override EnumerableOperationKind Kind => EnumerableOperationKind.Enumerate;

        protected override string DebugViewCore => "Enumerate()";

        public override IEnumerable<T> GetResult(IEnumerable<T> array) => array;
    }
}
