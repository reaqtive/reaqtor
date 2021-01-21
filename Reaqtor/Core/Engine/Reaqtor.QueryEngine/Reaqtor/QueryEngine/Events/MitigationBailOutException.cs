// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1064 // Exceptions should be public. (Scoped to the query engine; should not escape.)

using System;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Exception type used to denote that an entity recovery error occurred and the recovery process needs to bail out
    /// in order for the mitigation mechanism to kick in and try to apply an alternative recovery strategy.
    /// </summary>
    /// <remarks>
    /// Recovery errors of entities should be rare, hence exceptions are used to interrupt the recovery process.
    /// </remarks>
    internal sealed class MitigationBailOutException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="MitigationBailOutException"/> instance.
        /// </summary>
        public MitigationBailOutException() { }

        /// <summary>
        /// Creates a new <see cref="MitigationBailOutException"/> instance with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        public MitigationBailOutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="MitigationBailOutException"/> instance with the specified <paramref name="message"/> and the specified <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public MitigationBailOutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
