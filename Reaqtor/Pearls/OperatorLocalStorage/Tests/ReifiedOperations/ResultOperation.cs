// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Linq.Expressions;

namespace Tests.ReifiedOperations
{
    internal enum ResultOperationKind
    {
        This,
        Select,
        Bind,
        Apply,
    }

    internal static class ResultOperationExtensions
    {
        public static SelectResultOperation<TValue, TResult, TNewResult> Select<TValue, TResult, TNewResult>(this IResultOperation<TValue, TResult> operation, Expression<Func<TResult, TNewResult>> selector) => ResultOperation.Select(operation, selector);

        public static BindResultOperation<TValue, TResult, TReified> Bind<TValue, TResult, TReified>(this IReifiedResultOperation<TValue, TResult, TReified> operation, Expression<Func<TReified, IOperation<TResult>>> selector) => ResultOperation.Bind(operation, selector);

        public static ApplyOperation<TValue, TResult> Apply<TValue, TResult>(this IResultOperation<TValue, TResult> operation, Expression<Func<TResult, IOperation<TValue>>> selector) => ResultOperation.Apply(operation, selector);
    }

    internal abstract class ResultOperation : OperationBase
    {
        public abstract ResultOperationKind Kind { get; }

        public static ThisResultOperation<TValue> This<TValue>() => new();

        public static SelectResultOperation<TValue, TResult, TNewResult> Select<TValue, TResult, TNewResult>(IResultOperation<TValue, TResult> operation, Expression<Func<TResult, TNewResult>> selector)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectResultOperation<TValue, TResult, TNewResult>(operation, selector);
        }

        public static BindResultOperation<TValue, TResult, TReified> Bind<TValue, TResult, TReified>(IReifiedResultOperation<TValue, TResult, TReified> operation, Expression<Func<TReified, IOperation<TResult>>> selector)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new BindResultOperation<TValue, TResult, TReified>(operation, selector);
        }

        public static ApplyOperation<TValue, TResult> Apply<TValue, TResult>(IResultOperation<TValue, TResult> operation, Expression<Func<TResult, IOperation<TValue>>> selector)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new ApplyOperation<TValue, TResult>(operation, selector);
        }
    }

    internal abstract class ResultOperation<TValue> : ResultOperation, IOperation<TValue>
    {
        internal ResultOperation() { }

        public abstract void Accept(TValue value);
    }

    internal abstract class ResultResultOperation<TValue, TResult> : ResultOperation<TValue>, IResultOperation<TValue, TResult>
    {
        internal ResultResultOperation() { }

        public sealed override void Accept(TValue value) => _ = GetResult(value);
        public abstract TResult GetResult(TValue value);
    }

    internal sealed class ThisResultOperation<TValue> : ResultResultOperation<TValue, TValue>
    {
        internal ThisResultOperation() { }

        public override ResultOperationKind Kind => ResultOperationKind.This;

        protected override string DebugViewCore => FormattableString.Invariant($"This()");

        public override TValue GetResult(TValue value) => value;
    }

    internal sealed class SelectResultOperation<TValue, TResult, TNewResult> : ResultResultOperation<TValue, TNewResult>
    {
        internal SelectResultOperation(IResultOperation<TValue, TResult> operation, Expression<Func<TResult, TNewResult>> selector)
        {
            Operation = operation;
            Selector = selector;
            SelectorDelegate = selector.Compile();
        }

        public override ResultOperationKind Kind => ResultOperationKind.Select;

        public IResultOperation<TValue, TResult> Operation { get; }
        public Expression<Func<TResult, TNewResult>> Selector { get; }
        public Func<TResult, TNewResult> SelectorDelegate { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Select({Operation.DebugView}, {Selector})");

        public override TNewResult GetResult(TValue value) => SelectorDelegate(Operation.GetResult(value));
    }

    internal sealed class BindResultOperation<TValue, TResult, TReified> : ResultOperation<TValue>
    {
        internal BindResultOperation(IReifiedResultOperation<TValue, TResult, TReified> operation, Expression<Func<TReified, IOperation<TResult>>> selector)
        {
            Operation = operation;
            Selector = selector;
            SelectorDelegate = selector.Compile();
        }

        public override ResultOperationKind Kind => ResultOperationKind.Bind;

        public IReifiedResultOperation<TValue, TResult, TReified> Operation { get; }
        public Expression<Func<TReified, IOperation<TResult>>> Selector { get; }
        public Func<TReified, IOperation<TResult>> SelectorDelegate { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Bind({Operation.DebugView}, {Selector})");

        public override void Accept(TValue value)
        {
            var innerOperation = SelectorDelegate(Operation.Reified);
            var outerValue = Operation.GetResult(value);
            innerOperation.Accept(outerValue);
        }
    }

    internal sealed class ApplyOperation<TValue, TResult> : ResultOperation<TValue>
    {
        internal ApplyOperation(IResultOperation<TValue, TResult> operation, Expression<Func<TResult, IOperation<TValue>>> selector)
        {
            Operation = operation;
            Selector = selector;
            SelectorDelegate = selector.Compile();
        }

        public override ResultOperationKind Kind => ResultOperationKind.Apply;

        public IResultOperation<TValue, TResult> Operation { get; }
        public Expression<Func<TResult, IOperation<TValue>>> Selector { get; }
        public Func<TResult, IOperation<TValue>> SelectorDelegate { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Apply({Operation.DebugView}, {Selector})");

        public override void Accept(TValue value) => SelectorDelegate(Operation.GetResult(value)).Accept(value);
    }
}
