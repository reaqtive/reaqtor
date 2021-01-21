// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Linq;

namespace Tests.ReifiedOperations
{
    internal enum OperationKind
    {
        Sequence,
    }

    internal interface IOperationFactory
    {
        ThisResultOperation<TValue> This<TValue>();
    }

    internal abstract class Operation : OperationBase
    {
        public abstract OperationKind Kind { get; }

        public static SequenceOperation<TValue> Sequence<TValue>(params IOperation<TValue>[] operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            return new SequenceOperation<TValue>(operations);
        }

        public static ThisResultOperation<TValue> This<TValue>() => new();
    }

    internal sealed class SequenceOperation<TValue> : Operation, IOperation<TValue>
    {
        internal SequenceOperation(IOperation<TValue>[] operations)
        {
            Operations = operations;
        }

        public override OperationKind Kind => OperationKind.Sequence;

        public IOperation<TValue>[] Operations { get; }

        protected override string DebugViewCore => "{ " + string.Join("; ", Operations.Select(o => o.DebugView)) + " }";

        public void Accept(TValue value)
        {
            foreach (var operation in Operations)
            {
                operation.Accept(value);
            }
        }
    }
}
