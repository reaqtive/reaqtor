// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Protocol.FaultHandling;

#pragma warning disable CA1032 // Implement standard exception constructors. (Overloads with default parameters for transient suffice.)

//
// NB: The archived copy implemented the legacy ISerializable pattern (a
//     protected BaseException(SerializationInfo, StreamingContext) ctor and a GetObjectData
//     override). Those are obsolete on net10.0 (SYSLIB0051, an error here) and are dead weight
//     once exceptions cross the wire as ErrorInfo, so they are stripped during the port. The
//     [Serializable] attribute, the _transient field / IsTransient(), and the three normal
//     constructors are kept.
//

/// <summary>Base exception for all exceptions thrown by RIPP</summary>
[Serializable]
public class BaseException : Exception
{
    /// <summary>
    /// A flag indicating if the exceptions is due to a transient error. Transient errors indicate errors that are potentially resolved with retries.
    /// </summary>
    private readonly bool _transient;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseException"/> class.
    /// </summary>
    /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
    public BaseException(bool transient = false)
        : base()
    {
        _transient = transient;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
    public BaseException(string message, bool transient = false)
        : base(message)
    {
        _transient = transient;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="exception">The inner exception</param>
    /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
    public BaseException(string message, Exception exception, bool transient = false)
        : base(message, exception)
    {
        _transient = transient;
    }

    /// <summary>
    /// Gets a flag indicating if the error was due to a transient error
    /// </summary>
    /// <returns><code>true</code> if the exception is due to a transient error</returns>
    public bool IsTransient() => _transient;
}
