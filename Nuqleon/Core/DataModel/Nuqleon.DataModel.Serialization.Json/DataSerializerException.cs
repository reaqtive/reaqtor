// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Exception thrown when there is an error during serialization.
    /// </summary>
    [Serializable]
    public sealed class DataSerializerException : SerializationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSerializerException"/> class.
        /// </summary>
        public DataSerializerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSerializerException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataSerializerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSerializerException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DataSerializerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSerializerException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        private DataSerializerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
