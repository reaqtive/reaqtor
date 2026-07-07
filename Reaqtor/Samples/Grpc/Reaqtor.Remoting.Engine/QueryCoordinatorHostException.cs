// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.QueryCoordinator
{
    using System;

    using Protocol.FaultHandling;

#pragma warning disable CA1032 // Implement standard exception constructors. (Overloads have optional parameter.)

    //
    // NB (plan §2.5 / §6.1): the archived QueryCoordinatorHostException implemented the legacy ISerializable pattern
    //     (a protected QueryCoordinatorHostException(SerializationInfo, StreamingContext) ctor that chained to the
    //     matching BaseException ctor). That ctor is obsolete on net10.0 (SYSLIB0051, an error here) and was stripped
    //     from the ported BaseException (Reaqtor.Remoting.Core), so the base ctor no longer exists. We strip the
    //     ISerializable ctor (and its System.Runtime.Serialization using) here too, mirroring BaseException exactly;
    //     the [Serializable] attribute and the three normal constructors are kept. Exceptions cross the wire as
    //     ErrorInfo (plan §6.1), so the serialization ctor is dead weight.
    //

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
    }
}
