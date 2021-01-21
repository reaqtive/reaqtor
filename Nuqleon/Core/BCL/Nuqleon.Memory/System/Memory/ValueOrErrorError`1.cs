// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Created ValueOrError functionality.
//

using System.Runtime.ExceptionServices;

namespace System.Memory
{
    internal class ValueOrErrorError<T> : IValueOrError<T>
    {
        private readonly ExceptionDispatchInfo _error;

        public ValueOrErrorError(ExceptionDispatchInfo error) => _error = error;

        public ValueOrErrorKind Kind => ValueOrErrorKind.Error;

        public T Value
        {
            get
            {
                _error.Throw();
                return default;
            }
        }

        public Exception Exception => _error.SourceException;
    }
}
