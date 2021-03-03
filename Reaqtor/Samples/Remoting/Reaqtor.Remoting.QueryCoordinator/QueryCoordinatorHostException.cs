// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.QueryCoordinator
{
    using System;
    using System.Runtime.Serialization;

    using Protocol.FaultHandling;

#pragma warning disable CA1032 // Implement standard exception constructors. (Overloads have optional parameter.)

    /// <summary>An exception for errors from the query coordinator host</summary>
    [Serializable]
    public class QueryCoordinatorHostException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCoordinatorHostException"/> class.
        /// </summary>
        /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
        public QueryCoordinatorHostException(bool transient = false)
            : base(transient)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCoordinatorHostException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
        public QueryCoordinatorHostException(string message, bool transient = false)
            : base(message, transient)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCoordinatorHostException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The inner exception</param>
        /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
        public QueryCoordinatorHostException(string message, Exception exception, bool transient = false)
            : base(message, exception, transient)
        {
        }

        protected QueryCoordinatorHostException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
