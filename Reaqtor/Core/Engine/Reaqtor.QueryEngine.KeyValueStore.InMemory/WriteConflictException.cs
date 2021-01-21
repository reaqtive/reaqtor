// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Exception to indicate a write conflict in the key/value store.
    /// </summary>
    [Serializable]
    public partial class WriteConflictException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="WriteConflictException"/>.
        /// </summary>
        public WriteConflictException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="WriteConflictException"/> with the specified message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public WriteConflictException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="WriteConflictException"/> with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public WriteConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteConflictException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected WriteConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
