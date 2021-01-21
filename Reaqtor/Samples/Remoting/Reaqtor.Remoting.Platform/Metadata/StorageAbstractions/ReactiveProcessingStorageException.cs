// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;
using System.Text;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Reactive Processing Storage Exception 
    /// </summary>
    [Serializable]
    public class ReactiveProcessingStorageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveProcessingStorageException" /> class.
        /// </summary>
        protected ReactiveProcessingStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCode = (ErrorCodes)info.GetInt32("ErrorCode");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveProcessingStorageException" /> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception</param>
        public ReactiveProcessingStorageException(
            ErrorCodes errorCode,
            string message,
            Exception inner)
            : base(message, inner)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Possible storage error codes
        /// </summary>
        public enum ErrorCodes
        {
            /// <summary>
            /// Unexpected error
            /// </summary>
            Unexpected,

            /// <summary>
            /// Invalid connection string
            /// </summary>
            InvalidConnectionString,

            /// <summary>
            /// The entity was not found
            /// </summary>
            EntityNotFound,

            /// <summary>
            /// The entity already exists
            /// </summary>
            EntityAlreadyExists,

            /// <summary>
            /// The add entity failed
            /// </summary>
            AddEntityFailed,

            /// <summary>
            /// The operation failed
            /// </summary>
            OperationFailed
        }

        /// <summary>
        /// Gets the error code
        /// </summary>
        public ErrorCodes ErrorCode { get; private set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString());
            sb.Append(", ErrorCode: ").Append(ErrorCode);
            return sb.ToString();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ErrorCode", (int)ErrorCode, typeof(int));
        }
    }
}
