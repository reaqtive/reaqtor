// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

//
// NB: This moved from Reaqtor to the query engine, but for backward compat reasons we keep the
//     exception type in the parent namespace.
//

namespace Reaqtor.Reactive
{
    /// <summary>
    /// Exception representing failures in higher order subscriptions.
    /// </summary>
    [Serializable]
    public class HigherOrderSubscriptionFailedException : Exception
    {
        /// <summary>
        /// Creates a new higher order subscription failure exception.
        /// </summary>
        public HigherOrderSubscriptionFailedException() { }

        /// <summary>
        /// Creates a new higher order subscription failure exception with the specified message.
        /// </summary>
        /// <param name="message">Error message.</param>
        public HigherOrderSubscriptionFailedException(string message) : base(message) { }

        /// <summary>
        /// Creates a new higher order subscription failure exception with the specified message and inner exception.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">Inner exception.</param>
        public HigherOrderSubscriptionFailedException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Creates a new higher order subscription failure exception from the specified serialization information.
        /// </summary>
        /// <param name="info">Serialization information to deserialize the exception from.</param>
        /// <param name="context">Streaming context to deserialize the exception from.</param>
        protected HigherOrderSubscriptionFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
