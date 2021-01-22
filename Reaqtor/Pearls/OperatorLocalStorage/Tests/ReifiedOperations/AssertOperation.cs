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
    internal enum AssertOperationKind
    {
        AreEqual,
        AreNotEqual,
        AreSame,
        AreNotSame,
        AreSequenceEqual,
        AreNotSequenceEqual,
        IsFalse,
        IsTrue,
        ThrowsException,
    }

    internal interface IAssertOperationFactory
    {
        AreEqualAssertOperation<TValue, TResult> AreEqual<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected);
        AreNotEqualAssertOperation<TValue, TResult> AreNotEqual<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected);
        AreSameAssertOperation<TValue, TResult> AreSame<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected) where TResult : class;
        AreNotSameAssertOperation<TValue, TResult> AreNotSame<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected) where TResult : class;
        AreSequenceEqualAssertOperation<TValue, TResult> AreSequenceEqual<TValue, TResult>(IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected);
        AreNotSequenceEqualAssertOperation<TValue, TResult> AreNotSequenceEqual<TValue, TResult>(IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected);
        IsFalseAssertOperation<TValue> IsFalse<TValue>(IResultOperation<TValue, bool> operation);
        IsTrueAssertOperation<TValue> IsTrue<TValue>(IResultOperation<TValue, bool> operation);
        IAssertThrowsExceptionOperationFactory<TException> ThrowsException<TException>() where TException : Exception;
    }

    internal interface IAssertThrowsExceptionOperationFactory<TException> where TException : Exception
    {
        ThrowsExceptionAssertOperation<TValue, TException> When<TValue>(IOperation<TValue> operation);
    }

    internal abstract class AssertOperation : OperationBase
    {
        public abstract AssertOperationKind Kind { get; }

        public static IAssertOperationFactory WithAssert(IAssert assert)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));

            return new AssertOperationFactory(assert);
        }

        public static AreEqualAssertOperation<TValue, TResult> AreEqual<TValue, TResult>(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new AreEqualAssertOperation<TValue, TResult>(assert, operation, expected);
        }

        public static AreNotEqualAssertOperation<TValue, TResult> AreNotEqual<TValue, TResult>(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new AreNotEqualAssertOperation<TValue, TResult>(assert, operation, expected);
        }

        public static AreSameAssertOperation<TValue, TResult> AreSame<TValue, TResult>(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected) where TResult : class
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new AreSameAssertOperation<TValue, TResult>(assert, operation, expected);
        }

        public static AreNotSameAssertOperation<TValue, TResult> AreNotSame<TValue, TResult>(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected) where TResult : class
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new AreNotSameAssertOperation<TValue, TResult>(assert, operation, expected);
        }

        public static AreSequenceEqualAssertOperation<TValue, TResult> AreSequenceEqual<TValue, TResult>(IAssert assert, IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (expected == null)
                throw new ArgumentNullException(nameof(expected));

            return new AreSequenceEqualAssertOperation<TValue, TResult>(assert, operation, expected);
        }

        public static AreNotSequenceEqualAssertOperation<TValue, TResult> AreNotSequenceEqual<TValue, TResult>(IAssert assert, IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (expected == null)
                throw new ArgumentNullException(nameof(expected));

            return new AreNotSequenceEqualAssertOperation<TValue, TResult>(assert, operation, expected);
        }

        public static IsFalseAssertOperation<TValue> IsFalse<TValue>(IAssert assert, IResultOperation<TValue, bool> operation)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new IsFalseAssertOperation<TValue>(assert, operation);
        }

        public static IsTrueAssertOperation<TValue> IsTrue<TValue>(IAssert assert, IResultOperation<TValue, bool> operation)
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new IsTrueAssertOperation<TValue>(assert, operation);
        }

        public static ThrowsExceptionAssertOperation<TValue, TException> ThrowsException<TValue, TException>(IAssert assert, IOperation<TValue> operation) where TException : Exception
        {
            if (assert == null)
                throw new ArgumentNullException(nameof(assert));
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new ThrowsExceptionAssertOperation<TValue, TException>(assert, operation);
        }

        private sealed class AssertOperationFactory : IAssertOperationFactory
        {
            private readonly IAssert _assert;

            public AssertOperationFactory(IAssert assert)
            {
                _assert = assert;
            }

            public AreEqualAssertOperation<TValue, TResult> AreEqual<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected) => AssertOperation.AreEqual(_assert, operation, expected);

            public AreNotEqualAssertOperation<TValue, TResult> AreNotEqual<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected) => AssertOperation.AreNotEqual(_assert, operation, expected);

            public AreSameAssertOperation<TValue, TResult> AreSame<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected) where TResult : class => AssertOperation.AreSame(_assert, operation, expected);

            public AreNotSameAssertOperation<TValue, TResult> AreNotSame<TValue, TResult>(IResultOperation<TValue, TResult> operation, TResult expected) where TResult : class => AssertOperation.AreNotSame(_assert, operation, expected);

            public AreSequenceEqualAssertOperation<TValue, TResult> AreSequenceEqual<TValue, TResult>(IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected) => AssertOperation.AreSequenceEqual(_assert, operation, expected);

            public AreNotSequenceEqualAssertOperation<TValue, TResult> AreNotSequenceEqual<TValue, TResult>(IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected) => AssertOperation.AreNotSequenceEqual(_assert, operation, expected);

            public IAssertThrowsExceptionOperationFactory<TException> ThrowsException<TException>() where TException : Exception => new AssertThrowsExceptionOperationFactory<TException>(_assert);

            public IsFalseAssertOperation<TValue> IsFalse<TValue>(IResultOperation<TValue, bool> operation) => AssertOperation.IsFalse(_assert, operation);

            public IsTrueAssertOperation<TValue> IsTrue<TValue>(IResultOperation<TValue, bool> operation) => AssertOperation.IsTrue(_assert, operation);

            private sealed class AssertThrowsExceptionOperationFactory<TException> : IAssertThrowsExceptionOperationFactory<TException> where TException : Exception
            {
                private readonly IAssert _assert;

                public AssertThrowsExceptionOperationFactory(IAssert assert)
                {
                    _assert = assert;
                }

                public ThrowsExceptionAssertOperation<TValue, TException> When<TValue>(IOperation<TValue> operation) => AssertOperation.ThrowsException<TValue, TException>(_assert, operation);
            }
        }
    }

    internal abstract class AssertOperation<TValue> : AssertOperation, IOperation<TValue>
    {
        protected AssertOperation(IAssert assert)
        {
            Assert = assert;
        }

        public IAssert Assert { get; }

        public abstract void Accept(TValue value);
    }

    internal abstract class AssertOperation<TValue, TResult> : AssertOperation<TValue>
    {
        protected AssertOperation(IAssert assert, IResultOperation<TValue, TResult> operation)
            : base(assert)
        {
            Operation = operation;
        }

        public IResultOperation<TValue, TResult> Operation { get; }

        protected void Apply(TValue value, Action<TResult> assert)
        {
            var actual = Operation.GetResult(value);
            assert(actual);
        }
    }

    internal abstract class AssertExpectedOperation<TValue, TResult> : AssertOperation<TValue, TResult>
    {
        protected AssertExpectedOperation(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
            : base(assert, operation)
        {
            Expected = expected;
        }

        public TResult Expected { get; }
    }

    internal sealed class AreEqualAssertOperation<TValue, TResult> : AssertExpectedOperation<TValue, TResult>
    {
        internal AreEqualAssertOperation(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
            : base(assert, operation, expected)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.AreEqual;

        protected override string DebugViewCore => FormattableString.Invariant($"AreEqual({Operation.DebugView}, {Expected})");

        public override void Accept(TValue value) => Apply(value, actual => Assert.AreEqual(Expected, actual, FormattableString.Invariant($"{DebugView} Actual = {actual}")));
    }

    internal sealed class AreNotEqualAssertOperation<TValue, TResult> : AssertExpectedOperation<TValue, TResult>
    {
        internal AreNotEqualAssertOperation(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
            : base(assert, operation, expected)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.AreNotEqual;

        protected override string DebugViewCore => FormattableString.Invariant($"AreNotEqual({Operation.DebugView}, {Expected})");

        public override void Accept(TValue value) => Apply(value, actual => Assert.AreNotEqual(Expected, actual, FormattableString.Invariant($"{DebugView} Actual = {actual}")));
    }

    internal sealed class AreSameAssertOperation<TValue, TResult> : AssertExpectedOperation<TValue, TResult> where TResult : class
    {
        internal AreSameAssertOperation(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
            : base(assert, operation, expected)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.AreSame;

        protected override string DebugViewCore => FormattableString.Invariant($"AreSame({Operation.DebugView}, {Expected})");

        public override void Accept(TValue value) => Apply(value, actual => Assert.AreSame(Expected, actual, FormattableString.Invariant($"{DebugView} Actual = {actual}")));
    }

    internal sealed class AreNotSameAssertOperation<TValue, TResult> : AssertExpectedOperation<TValue, TResult> where TResult : class
    {
        internal AreNotSameAssertOperation(IAssert assert, IResultOperation<TValue, TResult> operation, TResult expected)
            : base(assert, operation, expected)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.AreNotSame;

        protected override string DebugViewCore => FormattableString.Invariant($"AreNotSame({Operation.DebugView}, {Expected})");

        public override void Accept(TValue value) => Apply(value, actual => Assert.AreNotSame(Expected, actual, FormattableString.Invariant($"{DebugView} Actual = {actual}")));
    }

    internal sealed class AreSequenceEqualAssertOperation<TValue, TResult> : AssertExpectedOperation<TValue, IEnumerable<TResult>>
    {
        internal AreSequenceEqualAssertOperation(IAssert assert, IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected)
            : base(assert, operation, expected)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.AreSequenceEqual;

        protected override string DebugViewCore => FormattableString.Invariant($"AreSequenceEqual({Operation.DebugView}, [{string.Join(", ", Expected)}])");

        public override void Accept(TValue value) => Apply(value, actual => Assert.AreSequenceEqual(Expected, actual, FormattableString.Invariant($"{DebugView} Actual = [{string.Join(", ", actual)}]")));
    }

    internal sealed class AreNotSequenceEqualAssertOperation<TValue, TResult> : AssertExpectedOperation<TValue, IEnumerable<TResult>>
    {
        internal AreNotSequenceEqualAssertOperation(IAssert assert, IResultOperation<TValue, IEnumerable<TResult>> operation, IEnumerable<TResult> expected)
            : base(assert, operation, expected)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.AreNotSequenceEqual;

        protected override string DebugViewCore => FormattableString.Invariant($"AreNotSequenceEqual({Operation.DebugView}, [{string.Join(", ", Expected)}])");

        public override void Accept(TValue value) => Apply(value, actual => Assert.AreNotSequenceEqual(Expected, actual, FormattableString.Invariant($"{DebugView} Actual = [{string.Join(", ", actual)}]")));
    }

    internal sealed class ThrowsExceptionAssertOperation<TValue, TException> : AssertOperation<TValue> where TException : Exception
    {
        internal ThrowsExceptionAssertOperation(IAssert assert, IOperation<TValue> operation)
            : base(assert)
        {
            Operation = operation;
        }

        public override AssertOperationKind Kind => AssertOperationKind.ThrowsException;

        public IOperation<TValue> Operation { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Throws<{typeof(TException).Name}>({Operation.DebugView})");

        public override void Accept(TValue value) => Assert.ThrowsException<TException>(() => Operation.Accept(value), DebugView);
    }

    internal sealed class IsFalseAssertOperation<TValue> : AssertExpectedOperation<TValue, bool>
    {
        internal IsFalseAssertOperation(IAssert assert, IResultOperation<TValue, bool> operation)
            : base(assert, operation, expected: false)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.IsFalse;

        protected override string DebugViewCore => FormattableString.Invariant($"IsFalse({Operation.DebugView})");

        public override void Accept(TValue value) => Apply(value, actual => Assert.IsFalse(actual, DebugView));
    }

    internal sealed class IsTrueAssertOperation<TValue> : AssertExpectedOperation<TValue, bool>
    {
        internal IsTrueAssertOperation(IAssert assert, IResultOperation<TValue, bool> operation)
            : base(assert, operation, expected: true)
        {
        }

        public override AssertOperationKind Kind => AssertOperationKind.IsTrue;

        protected override string DebugViewCore => FormattableString.Invariant($"IsTrue({Operation.DebugView})");

        public override void Accept(TValue value) => Apply(value, actual => Assert.IsTrue(actual, DebugView));
    }
}
