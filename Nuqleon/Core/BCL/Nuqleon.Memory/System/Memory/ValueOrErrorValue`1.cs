// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/06/2015 - Extended ValueOrError functionality.
//

namespace System.Memory
{
    internal class ValueOrErrorValue<T> : IValueOrError<T>
    {
        public ValueOrErrorValue(T value) => Value = value;

        public ValueOrErrorKind Kind => ValueOrErrorKind.Value;

        public T Value { get; }

        public Exception Exception => throw new InvalidOperationException("The object represents a value and does not have an error.");
    }
}
